using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Xml;
using System.Runtime.InteropServices;
using System.Text;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using System.Diagnostics;

/// <summary>
/// Summary description for LicenseCore
/// </summary>
public class LicenseCore
{
    private bool customXertificateValidation(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error)
    {
        return true;
    }

    public Boolean ValidateLicense(String LicenseCode, String processorID, String harddiskSerial, String AppPreFix)
    {
        string SerailWebServiceURL = "SU.SU";

        XmlDocument xmlServiceURL = GetServiceURL();

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
                        }
                        else if (result == "notfound")
                        {
                            isValidated = false;
                        }
                        else if (result == "abused")
                        {
                            isValidated = false;
                        }
                        else
                        {
                            isValidated = false;
                        }
                    }
                    else
                    {
                        isValidated = false;
                    }

                }
                catch (Exception ex2)
                {
                    isValidated = false;
                }

                return isValidated;

            }
            else if (!LicenseCode.StartsWith("REQ"))
            {
                return false;
            }
        }
        catch (Exception ex1)
        {
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