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

/// <summary>
/// Summary description for LicenseCore
/// </summary>
public class LicenseCorePPM
{
    string licenseFilePath;
    bool ifShowMessages;
    String AppPreFix = "PMT";
    string _PPMLicenseID = "";
    bool _ifValidated = false;
    public String PPMLicenseID
    {
        get { return _PPMLicenseID; }
    }
    public Boolean IsValidated
    {
        get { return _ifValidated; }
    }
    public LicenseCorePPM(string licFilePath, bool shownMessages)
    {
        licenseFilePath = licFilePath;
        ifShowMessages = shownMessages;
        //
        // TODO: Add constructor logic here
        //
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
                    if (strSplit[1] == Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TB\\PM")
                    {
                        strContent = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(strSplit[0]));
                    }
                    else if (strSplit[0].StartsWith("REQ"))
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
            //contents += "^" + System.Windows.Forms.Application.StartupPath;
            //
            contents += "^" + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TB\\PM";
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
    private bool customXertificateValidation(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error){
        return true;
    }
    public void RegisterPPM()
    {
        string SerailWebServiceURL = "SU.SU";

        XmlDocument xmlServiceURL = new XmlDocument();
        XmlElement serviceURLNode = xmlServiceURL.CreateElement("serviceURL");
        xmlServiceURL.AppendChild(serviceURLNode);

        XmlElement VALIDATESERIALNode = xmlServiceURL.CreateElement("VALIDATESERIAL");
        VALIDATESERIALNode.InnerText = "http://95.211.166.125/PPM/ValidateLicense.asmx";
        serviceURLNode.AppendChild(VALIDATESERIALNode);

        XmlElement REQUESTSERIAL = xmlServiceURL.CreateElement("REQUESTSERIAL");
        REQUESTSERIAL.InnerText = "http://95.211.166.125/PPM/LicenseRequest.asmx";
        serviceURLNode.AppendChild(REQUESTSERIAL);

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
                return;
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


        if (string.IsNullOrEmpty(LicenseCode) && ifShowMessages)
        {

            LicenseAPI.frmRequestPPPM frmReq = new LicenseAPI.frmRequestPPPM(processorID, harddiskSerial, AppPreFix, xmlServiceURL, licenseFilePath);
            frmReq.ShowDialog();
        }
        else if (System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(LicenseCode)).StartsWith("REQ"))
        {
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(customXertificateValidation);

            String urlCheck = xmlServiceURL.SelectSingleNode("//serviceURL/VALIDATESERIAL").InnerText.Trim();
            System.Net.WebClient webClientCHK = new System.Net.WebClient();
            String resultCHK = webClientCHK.DownloadString(urlCheck + "/CheckLicenseID?ProccessorID=" + processorID + "&HarddiskSerial=" + harddiskSerial + "&RequestID=" + System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(LicenseCode)));

            switch (resultCHK)
            {
                case "notfound":
                    if (ifShowMessages)
                    {
                        LicenseAPI.frmRequestPPPM frmReq = new LicenseAPI.frmRequestPPPM(processorID, harddiskSerial, AppPreFix, xmlServiceURL, licenseFilePath);
                        frmReq.ShowDialog();
                    }
                    return;
                case "pending":
                    if (ifShowMessages)
                    {
                        System.Windows.Forms.MessageBox.Show("Your request is pending for the approval.");
                    }
                    return;
                case "expired":
                    if (ifShowMessages)
                    {
                        System.Windows.Forms.MessageBox.Show("Your software has been expired. Please contact your vendor.");
                    }
                    return;
                case "":

                    if (ifShowMessages)
                    {
                        System.Windows.Forms.MessageBox.Show("Your Licence is not valid please contact us to get your licence.");
                        LicenseAPI.frmRequestPPPM frmReq = new LicenseAPI.frmRequestPPPM(processorID, harddiskSerial, AppPreFix, xmlServiceURL, licenseFilePath);
                        frmReq.ShowDialog();
                    }
                    return;
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
        else if (!String.IsNullOrEmpty(LicenseCode))
        {
            LicenseCode = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(LicenseCode));
            if (LicenseCode.StartsWith(AppPreFix))
            {
                
                try
                {
                    ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(customXertificateValidation);

                    SerailWebServiceURL = xmlServiceURL.SelectSingleNode("//serviceURL/VALIDATESERIAL").InnerText.Trim();
                    System.Net.WebClient webClient = new System.Net.WebClient();
                    String result = webClient.DownloadString(SerailWebServiceURL + "/ValidateLicenseID?LicenseID=" + LicenseCode + "&ProccessorID=" + processorID + "&HarddiskSerial=" + harddiskSerial);
                    if (!String.IsNullOrEmpty(result))
                    {
                        if (result == "validated")
                        {
                            _PPMLicenseID = LicenseCode;
                            _ifValidated = true;
                        }
                        else if (result == "expired")
                        {
                            if (ifShowMessages)
                            {
                                System.Windows.Forms.MessageBox.Show("Your software has been expired");
                            }
                        }
                        else if (result == "notfound")
                        {
                            if (ifShowMessages)
                            {
                                LicenseAPI.frmRequestPPPM frmReq = new LicenseAPI.frmRequestPPPM(processorID, harddiskSerial, AppPreFix, xmlServiceURL, licenseFilePath);
                                frmReq.ShowDialog();
                            }
                        }
                        else if (result == "abused")
                        {
                            if (ifShowMessages)
                            {
                                System.Windows.Forms.MessageBox.Show("You or someone else tried to abuse the license. Please contact to vendor.");
                            }
                        }

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("There are some issue in validating the software. Please contact to vendor.");
                    }

                }
                catch (Exception ex2)
                {
                    System.Windows.Forms.MessageBox.Show("There are some issue in validating the software. Please contact to vendor." + ex2.Message);
                }
            }
        }
    }
    public Boolean ValidateLicense()
    {
        
        string SerailWebServiceURL = "SU.SU";

        XmlDocument xmlServiceURL = new XmlDocument();
        XmlElement serviceURLNode = xmlServiceURL.CreateElement("serviceURL");
        xmlServiceURL.AppendChild(serviceURLNode);

        XmlElement VALIDATESERIALNode = xmlServiceURL.CreateElement("VALIDATESERIAL");
        VALIDATESERIALNode.InnerText = "http://95.211.166.125/PPM/ValidateLicense.asmx";
        serviceURLNode.AppendChild(VALIDATESERIALNode);

        XmlElement REQUESTSERIAL = xmlServiceURL.CreateElement("REQUESTSERIAL");
        REQUESTSERIAL.InnerText = "http://95.211.166.125/PPM/LicenseRequest.asmx";
        serviceURLNode.AppendChild(REQUESTSERIAL);

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
               
        try
        {
            if (!String.IsNullOrEmpty(LicenseCode))
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
                        String result = webClient.DownloadString(SerailWebServiceURL + "/ValidateLicenseID?LicenseID=" + LicenseCode + "&ProccessorID=" + processorID + "&HarddiskSerial=" + harddiskSerial);
                        if (!String.IsNullOrEmpty(result))
                        {
                            if (result == "validated")
                            {
                                _PPMLicenseID = LicenseCode;
                                _ifValidated = true;
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
                                //if (ifShowMessages)
                                //{
                                //    LicenseAPI.frmRequestPPPM frmReq = new LicenseAPI.frmRequestPPPM(processorID, harddiskSerial, AppPreFix, xmlServiceURL, licenseFilePath);
                                //    frmReq.ShowDialog();
                                //}
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
            
        }
        catch (Exception ex1)
        {
            System.Windows.Forms.MessageBox.Show("There are some issue in validating the software. Please contact to vendor." + ex1.Message);
            return false;
        }

        return false;
    }

}
