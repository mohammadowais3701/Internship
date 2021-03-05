using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TokenUtilityLibrary
{
    public class LicensingToken
    {

        public static string GetLicenseToken(string _licenseCode,string _processorId,string _hardDiskSerial)
        {
            string licenseCode = string.Empty;

            try
            {
                String url = "http://95.211.166.125:1235/users/token/generate";

                String post = "LicenseID=" + System.Web.HttpUtility.UrlEncode(Base64Decode(_licenseCode)) + "&ProccessorID=" + System.Web.HttpUtility.UrlEncode(_processorId) + "&HarddiskSerial=" + System.Web.HttpUtility.UrlEncode(_hardDiskSerial);

                HttpWebRequest req = HttpWebRequest.CreateHttp(url);
               
                req.Method = "POST";
               
                req.ContentType = "application/x-www-form-urlencoded";
              
                req.ContentLength = Encoding.ASCII.GetBytes(post).Length;
              
                req.GetRequestStream().Write(Encoding.ASCII.GetBytes(post), 0, Encoding.ASCII.GetBytes(post).Length);
               
                licenseCode = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return licenseCode;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
