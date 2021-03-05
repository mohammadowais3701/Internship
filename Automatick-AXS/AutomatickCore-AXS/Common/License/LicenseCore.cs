using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Xml;
using IOEx;
using System.Runtime.InteropServices;
using System.Text;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Diagnostics;

/// <summary>
/// Summary description for LicenseCore
/// </summary>
[Serializable]
public class LicenseCore
{
    Boolean isPopUpShown = false;
    string licenseFilePath;
    bool ifShowMessages;
    String appPreFix = "";
    String hardDiskSerial = "";
    String licenseCode = "";
    String processorID = "";

    public String AppPreFix
    {
        get { return appPreFix; }
        set { appPreFix = value; }
    }

    public String HardDiskSerial
    {
        get { return hardDiskSerial; }
        set { hardDiskSerial = value; }
    }

    public String LicenseCode
    {
        get { return licenseCode; }
        set { licenseCode = value; }
    }

    public String ProcessorID
    {
        get { return processorID; }
        set { processorID = value; }
    }

    public LicenseCore(String appPreFix, string licFilePath, bool shownMessages)
    {
        licenseFilePath = licFilePath;
        ifShowMessages = shownMessages;
        this.AppPreFix = appPreFix;
    }

    public string ReadLicenseFile()
    {
        string strContent = "";

        try
        {
            if (File.Exists(licenseFilePath))
            {
                StreamReader sr = new StreamReader(licenseFilePath, System.Text.Encoding.UTF8);
                strContent = sr.ReadToEnd();
                sr.Close();
                String strContent2 = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(strContent));
                String[] strSplit = strContent2.Split('^');
                if (strSplit.Length >= 2)
                {
                    if (strSplit[1] == System.Windows.Forms.Application.StartupPath)
                    {
                        strContent = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strSplit[0]));
                    }
                    else
                    {
                        strContent = "";
                    }
                }
                else
                {
                    strContent = "";
                }
            }
        }
        catch (Exception)
        {
            strContent = "";
        }
        return strContent;
    }
    public void WriteLicenseFile(string contents)
    {
        try
        {
            contents += "^" + System.Windows.Forms.Application.StartupPath;
            contents = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(contents));
            StreamWriter sr = new StreamWriter(licenseFilePath, false, System.Text.Encoding.UTF8);
            sr.Write(contents);
            sr.Close();
        }
        catch (Exception)
        {

        }

    }


    [DllImport("kernel32.dll")]
    private static extern long GetVolumeInformation(string PathName, StringBuilder VolumeNameBuffer, UInt32 VolumeNameSize, ref UInt32 VolumeSerialNumber, ref UInt32 MaximumComponentLength, ref UInt32 FileSystemFlags, StringBuilder FileSystemNameBuffer, UInt32 FileSystemNameSize);

    string getAlternator()
    {
        String source = "TB2011^" + Environment.UserName + "^" + Environment.MachineName;

        String Hash = "";
        try
        {
            Hash = getMd5Hash(source);
        }
        catch { Hash = "Error"; }
        return Hash;
    }
    // Hash an input string and return the hash as
    // a 32 character hexadecimal string.
    static string getMd5Hash(string input)
    {
        // Create a new instance of the MD5CryptoServiceProvider object.
        MD5 md5Hasher = MD5.Create();

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    // Verify a hash against a string.
    static bool verifyMd5Hash(string input, string hash)
    {
        // Hash the input.
        string hashOfInput = getMd5Hash(input);

        // Create a StringComparer an comare the hashes.
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        if (0 == comparer.Compare(hashOfInput, hash))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    string getHarddiskSerialNumber1()
    {
        string harddiskSerialNumber = "";
        try
        {

            DriveListEx m_list = new DriveListEx();
            m_list.Load();
            if (m_list.Count > 0)
            {
                harddiskSerialNumber = m_list[0].ModelNumber + "_" + m_list[0].SerialNumber;
                harddiskSerialNumber = harddiskSerialNumber.Replace(" ", "_");
            }

        }
        catch (Exception e)
        {
            harddiskSerialNumber = "Error: " + e.Message;
        }
        if (String.IsNullOrEmpty(harddiskSerialNumber))
        {
            harddiskSerialNumber = "Empty";
        }
        return harddiskSerialNumber;
    }
    string getHarddiskSerialNumber2()
    {
        string harddiskSerialNumber = "";
        try
        {

            string sQuery = "SELECT SerialNumber FROM Win32_DiskDrive";
            ManagementObjectSearcher oManagement = new ManagementObjectSearcher(sQuery);
            ManagementObjectCollection oCollection = oManagement.Get();

            foreach (ManagementObject oManagementObject in oCollection)
            {
                harddiskSerialNumber = (string)oManagementObject["SerialNumber"];
            }
            if (harddiskSerialNumber != null)
            {
                harddiskSerialNumber = harddiskSerialNumber.Trim();
            }


        }
        catch (Exception e)
        {
            harddiskSerialNumber = "Error: " + e.Message;
        }
        if (String.IsNullOrEmpty(harddiskSerialNumber))
        {
            harddiskSerialNumber = "Empty";
        }
        return harddiskSerialNumber;
    }
    string getHarddiskSerialNumber3()
    {
        string harddiskSerialNumber = "";
        String strDriveLetter = "";
        try
        {

            uint serNum = 0;
            uint maxCompLen = 0;
            StringBuilder VolLabel = new StringBuilder(256); // Label
            UInt32 VolFlags = new UInt32();
            StringBuilder FSName = new StringBuilder(256); // File System Name
            string[] Drives = Environment.GetLogicalDrives();
            if (Drives.Length > 0)
            {
                strDriveLetter = Drives[0];
            }
            else
            {
                strDriveLetter += "C:\\"; // fix up the passed-in drive letter for the API call
            }

            long Ret = GetVolumeInformation(strDriveLetter, VolLabel, (UInt32)VolLabel.Capacity, ref serNum, ref maxCompLen, ref VolFlags, FSName, (UInt32)FSName.Capacity);

            harddiskSerialNumber = Convert.ToString(serNum);


        }
        catch (Exception e)
        {
            harddiskSerialNumber = "Error: " + e.Message;
        }
        if (String.IsNullOrEmpty(harddiskSerialNumber))
        {
            harddiskSerialNumber = "Empty";
        }
        return harddiskSerialNumber;
    }

    string getProcessorID()
    {
        string processorId = "";
        try
        {
            string sQuery = "SELECT ProcessorId FROM Win32_Processor";
            ManagementObjectSearcher oManagement = new ManagementObjectSearcher(sQuery);
            ManagementObjectCollection oCollection = oManagement.Get();

            foreach (ManagementObject oManagementObject in oCollection)
            {
                processorId = (string)oManagementObject["ProcessorId"];
            }
            if (processorId != null)
            {
                processorId = processorId.Trim();
            }
        }
        catch (Exception e)
        {
            processorId = "Error: " + e.Message;
        }
        if (String.IsNullOrEmpty(processorId))
        {
            processorId = "EMPTY";
        }
        return processorId;
    }
    private bool customXertificateValidation(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error)
    {
        return true;
    }
    public Boolean CheckVersionUpdate()
    {
        bool result = false;
        try
        {
            String LicenseID = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(this.ReadLicenseFile()));
            String version_id = string.Empty;
            WebClient wc = new WebClient();

            if (File.Exists(Application.StartupPath + @"\.version"))
            {
                using (TextReader _reader = new StreamReader(Application.StartupPath + @"\.version"))
                {
                    version_id = new System.Text.ASCIIEncoding().GetString(Convert.FromBase64String(_reader.ReadToEnd()));
                }
            }
            else
            {
                version_id = "-1";
                using (TextWriter _writer = new StreamWriter(Application.StartupPath + @"\.version"))
                {
                    _writer.Write(Convert.ToBase64String(new System.Text.ASCIIEncoding().GetBytes(version_id)));
                }
                wc.DownloadData("https://license.ticketpeers.com/LicensingSystem/ValidateLicense.asmx/sv?version_id=" + version_id + "&license_id=" + LicenseID);
            }


            wc = new WebClient();
            byte[] _data = wc.DownloadData(@"https://license.ticketpeers.com/LicensingSystem/ValidateLicense.asmx/checkUpdates?license_id=" + LicenseID + "&version_id=" + version_id);
            System.Text.ASCIIEncoding encode = new System.Text.ASCIIEncoding();
            String response = encode.GetString(_data);

            Process[] tixToxServ = System.Diagnostics.Process.GetProcessesByName("AXSTixToxService");

            foreach (var item in tixToxServ)
            {
                try
                {
                    item.Kill();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            result = response.Contains("Yes");
            if (result)
            {
                DialogResult resultDialog;
                resultDialog = MessageBox.Show("A new update is available. Do you wish to update?", "Update Available", MessageBoxButtons.YesNo);

                if (resultDialog == DialogResult.Yes)
                {
                    //Download Updater files from DROPBOX
                    if (File.Exists(Application.StartupPath + @"\Updater.exe"))
                    {
                        try
                        {
                            File.Copy(Application.StartupPath + @"\Updater.exe", Application.StartupPath + @"\Updater-Backup.exe", true);
                            File.Delete(Application.StartupPath + @"\Updater.exe");
                        }
                        catch { }
                    }
                    try
                    {
                        using (WebClient wcUpdater = new WebClient())
                        {
                            wcUpdater.DownloadFile(@"https://license.ticketpeers.com/LicensingSystem/Files/Updater.exe", Application.StartupPath + @"\Updater.exe");
                        }
                    }
                    catch
                    {
                        if (File.Exists(Application.StartupPath + @"\Updater.exe"))
                        {
                            File.Delete(Application.StartupPath + @"\Updater.exe");
                        }
                        if (File.Exists(Application.StartupPath + @"\Updater-Backup.exe"))
                        {
                            File.Copy(Application.StartupPath + @"\Updater-Backup.exe", Application.StartupPath + @"\Updater.exe");
                        }
                    }
                    if (!File.Exists(Application.StartupPath + @"\Ionic.Zip.dll"))
                    {
                        try
                        {
                            using (WebClient wcUpdater = new WebClient())
                            {
                                wcUpdater.DownloadFile(@"https://license.ticketpeers.com/LicensingSystem/Files/Ionic.Zip.dll", Application.StartupPath + @"\Ionic.Zip.dll");
                            }
                        }
                        catch { }
                    }

                    //

                    result = true;
                    Process process = new Process();
                    process.StartInfo.Arguments = response.Split('\n')[1] + " " + LicenseID + " " + System.AppDomain.CurrentDomain.FriendlyName;
                    process.StartInfo.FileName = Application.StartupPath + @"\Updater.exe";
                    process.StartInfo.WorkingDirectory = Application.StartupPath;
                    process.EnableRaisingEvents = true;
                    process.Start();

                    try
                    {
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    catch (Exception currentProcessExitException)
                    {
                        Debug.WriteLine(currentProcessExitException.Message);
                    }

                    tixToxServ = System.Diagnostics.Process.GetProcessesByName("AXSTixToxService");

                    foreach (var item in tixToxServ)
                    {
                        try
                        {
                            item.Kill();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    tixToxServ = System.Diagnostics.Process.GetProcessesByName("AAXTixTox");

                    foreach (var item in tixToxServ)
                    {
                        try
                        {
                            item.Kill();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    result = false;
                }
            }
        }
        catch (Exception)
        {
            result = false;
        }

        return result;
    }
    public String GetLicenseID()
    {
        string LicenseCode = ReadLicenseFile();
        if (!string.IsNullOrEmpty(LicenseCode))
        {
            LicenseCode = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(LicenseCode));
        }

        this.LicenseCode = LicenseCode;

        return LicenseCode;
    }
    public Boolean ValidateLicense()
    {

        string SerailWebServiceURL = "SU.SU";
        //string SerailWebServiceURL = "http://ticketschecker.com/ServiceURL.xml";
        //string SerailWebServiceURL = "http://localhost:50610/SerialInfo/ServiceURL.xml";

        XmlDocument xmlServiceURL = GetServiceURL();

        try
        {
            //xmlServiceURL.Load(SerailWebServiceURL);
        }
        catch (Exception)
        {
            try
            {
                SerailWebServiceURL = "SU.SU";

                //xmlServiceURL.Load(SerailWebServiceURL);
            }
            catch (Exception)
            {
                return false;
            }
        }

        Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);


        string LicenseCode = ReadLicenseFile();

        string processorID = getProcessorID();

        string harddiskSerial = String.Empty;
        if (oConfig.AppSettings.Settings["licoption"] != null)
        {
            switch (oConfig.AppSettings.Settings["licoption"].Value)
            {
                case "H1":
                    harddiskSerial = getHarddiskSerialNumber1();
                    break;
                case "H2":
                    harddiskSerial = getHarddiskSerialNumber2();
                    break;
                case "H3":
                    harddiskSerial = getHarddiskSerialNumber3();
                    break;
                case "A":
                    harddiskSerial = getAlternator();
                    break;
                default:
                    harddiskSerial = getHarddiskSerialNumber1();
                    break;
            }
        }
        else
        {
            harddiskSerial = getHarddiskSerialNumber1();
        }

        this.HardDiskSerial = harddiskSerial;
        this.ProcessorID = processorID;
        this.LicenseCode = LicenseCode;

        if (string.IsNullOrEmpty(LicenseCode) && ifShowMessages)
        {

            LicenseAPI.frmRequest frmReq = new LicenseAPI.frmRequest(AppPreFix, processorID, harddiskSerial, AppPreFix, xmlServiceURL, licenseFilePath);
            frmReq.ShowDialog();
            return false;
        }
        else if (System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(LicenseCode)).StartsWith("REQ"))
        {
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(customXertificateValidation);

            String urlCheck = xmlServiceURL.SelectSingleNode("//serviceURL/VALIDATESERIAL").InnerText.Trim();

            System.Net.WebClient webClientCHK = new System.Net.WebClient();
            String resultCHK = String.Empty;

            try
            {
                resultCHK = webClientCHK.DownloadString(urlCheck + "/CheckLicenseID?ProccessorID=" + processorID + "&HarddiskSerial=" + harddiskSerial + "&RequestID=" + System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(LicenseCode)));
            }
            catch (Exception)
            {
                webClientCHK = new System.Net.WebClient();
                xmlServiceURL = null;
                xmlServiceURL = GetServiceURLAlternate();
                urlCheck = xmlServiceURL.SelectSingleNode("//serviceURL/VALIDATESERIAL").InnerText.Trim();

                resultCHK = webClientCHK.DownloadString(urlCheck + "/CheckLicenseID?ProccessorID=" + processorID + "&HarddiskSerial=" + harddiskSerial + "&RequestID=" + System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(LicenseCode)));
            }


            switch (resultCHK)
            {
                case "notfound":
                    if (ifShowMessages)
                    {
                        LicenseAPI.frmRequest frmReq = new LicenseAPI.frmRequest(AppPreFix, processorID, harddiskSerial, AppPreFix, xmlServiceURL, licenseFilePath);
                        frmReq.ShowDialog();
                    }
                    return false;
                case "pending":
                    if (ifShowMessages)
                    {
                        System.Windows.Forms.MessageBox.Show("Your request is pending for the approval.");
                    }
                    return false;
                case "expired":
                    if (ifShowMessages)
                    {
                        System.Windows.Forms.MessageBox.Show("Your software has been expired. Please contact your vendor.");
                    }
                    return false;
                case "":

                    if (ifShowMessages)
                    {
                        System.Windows.Forms.MessageBox.Show("Your Licence is not valid please contact us to get your licence.");
                        LicenseAPI.frmRequest frmReq = new LicenseAPI.frmRequest(AppPreFix, processorID, harddiskSerial, AppPreFix, xmlServiceURL, licenseFilePath);
                        frmReq.ShowDialog();
                    }
                    return false;
                default:
                    if (resultCHK.StartsWith(AppPreFix))
                    {
                        this.WriteLicenseFile(resultCHK);
                        LicenseCode = ReadLicenseFile();
                    }
                    else if (System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(LicenseCode)).StartsWith("REQ"))
                    {
                        if (ifShowMessages)
                        {
                            System.Windows.Forms.MessageBox.Show("Your request is pending for the approval.");
                        }
                    }
                    break;
            }

        }

        try
        {
            LicenseCode = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(LicenseCode));
            if (LicenseCode.StartsWith(AppPreFix))
            {
                Boolean isValidated = false;
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(customXertificateValidation);

                    SerailWebServiceURL = xmlServiceURL.SelectSingleNode("//serviceURL/VALIDATESERIAL").InnerText.Trim();

                    System.Net.WebClient webClient = new System.Net.WebClient();
                    String result = String.Empty;

                    try
                    {
                        result = webClient.DownloadString(SerailWebServiceURL + "/ValidateLicenseID?LicenseID=" + LicenseCode + "&ProccessorID=" + processorID + "&HarddiskSerial=" + harddiskSerial);
                    }
                    catch (Exception)
                    {
                        webClient = new System.Net.WebClient();
                        xmlServiceURL = null;
                        xmlServiceURL = GetServiceURLAlternate();
                        SerailWebServiceURL = xmlServiceURL.SelectSingleNode("//serviceURL/VALIDATESERIAL").InnerText.Trim();

                        result = webClient.DownloadString(SerailWebServiceURL + "/ValidateLicenseID?LicenseID=" + LicenseCode + "&ProccessorID=" + processorID + "&HarddiskSerial=" + harddiskSerial);
                    }

                    if (!String.IsNullOrEmpty(result))
                    {
                        if (result == "validated")
                        {
                            isValidated = true;
                        }
                        else if (result == "expired")
                        {
                            isValidated = false;
                            if (ifShowMessages)
                            {
                                System.Windows.Forms.MessageBox.Show("Your software has been expired");
                            }
                        }
                        else if (result == "notfound")
                        {
                            isValidated = false;
                            if (ifShowMessages)
                            {
                                LicenseAPI.frmRequest frmReq = new LicenseAPI.frmRequest(AppPreFix, processorID, harddiskSerial, AppPreFix, xmlServiceURL, licenseFilePath);
                                frmReq.ShowDialog();
                            }
                        }
                        else if (result == "abused")
                        {
                            isValidated = false;
                            if (ifShowMessages)
                            {
                                System.Windows.Forms.MessageBox.Show("You or someone else tried to abuse the license. Please contact to vendor.");
                            }
                        }
                        else
                        {
                            isValidated = false;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("There are some issue in validating the software. Please contact to vendor.");
                        isValidated = false;
                    }

                }
                catch (Exception ex2)
                {
                    System.Windows.Forms.MessageBox.Show("There are some issue in validating the software. Please contact to vendor." + ex2.Message);
                    isValidated = false;
                }

                return isValidated;

            }
            else if (!LicenseCode.StartsWith("REQ"))
            {
                if (ifShowMessages)
                {
                    System.Windows.Forms.MessageBox.Show("Invalid License");
                }
                this.WriteLicenseFile("");
            }
        }
        catch (Exception ex1)
        {
            System.Windows.Forms.MessageBox.Show("There are some issue in validating the software. Please contact to vendor." + ex1.Message);
            return false;
        }

        return false;
    }

    public static XmlDocument GetServiceURL()
    {
        XmlDocument xmlServiceURL = new XmlDocument();
        XmlElement serviceURLNode = xmlServiceURL.CreateElement("serviceURL");
        xmlServiceURL.AppendChild(serviceURLNode);

        XmlElement VALIDATESERIALNode = xmlServiceURL.CreateElement("VALIDATESERIAL");
        VALIDATESERIALNode.InnerText = "https://license.ticketpeers.com/LicensingSystem/ValidateLicense.asmx";
        serviceURLNode.AppendChild(VALIDATESERIALNode);

        XmlElement REQUESTSERIAL = xmlServiceURL.CreateElement("REQUESTSERIAL");
        REQUESTSERIAL.InnerText = "https://license.ticketpeers.com/LicensingSystem/LicenseRequest.asmx";
        serviceURLNode.AppendChild(REQUESTSERIAL);
        return xmlServiceURL;
    }
    public static XmlDocument GetServiceURLAlternate()
    {
        XmlDocument xmlServiceURL = new XmlDocument();
        XmlElement serviceURLNode = xmlServiceURL.CreateElement("serviceURL");
        xmlServiceURL.AppendChild(serviceURLNode);

        XmlElement VALIDATESERIALNode = xmlServiceURL.CreateElement("VALIDATESERIAL");
        VALIDATESERIALNode.InnerText = "https://50.31.14.55/SerialinfoNew/ValidateLicense.asmx";
        serviceURLNode.AppendChild(VALIDATESERIALNode);

        XmlElement REQUESTSERIAL = xmlServiceURL.CreateElement("REQUESTSERIAL");
        REQUESTSERIAL.InnerText = "https://50.31.14.55/SerialinfoNew/LicenseRequest.asmx";
        serviceURLNode.AppendChild(REQUESTSERIAL);
        return xmlServiceURL;
    }
}
