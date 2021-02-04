using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Diagnostics;
using Newtonsoft.Json;

namespace TicketBooking
{
    class Program
    {
        static void Main(string[] args)
        {
            makeWebRequest("https://tix.axs.com/vYAtIwAAAADK1ZUBAwAAAAC3%2fv%2f%2f%2fwD%2f%2f%2f%2f%2fBU1pY3JvAP%2f%2f%2f%2f%2f%2f%2f%2f%2f%2f");
        }
        static void makeWebRequest(string web) {
            Stream data;
            try
            {
               
                Uri uri = new Uri(web);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                req.Method = "GET";
            //    req.AllowAutoRedirect = false;
                WebProxy proxy = new WebProxy("127.0.0.1", 8888);
                req.Proxy = proxy;
              //  req.AutomaticDecompression = DecompressionMethods.GZip;
                req.Credentials = CredentialCache.DefaultCredentials;
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:85.0) Gecko/20100101 Firefox/85.0";
                Console.WriteLine(req.Headers);
                HttpWebResponse myres = (HttpWebResponse)req.GetResponse();
                var reader = new System.IO.StreamReader(myres.GetResponseStream());
                Console.WriteLine(reader.ReadToEnd());
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            
            }

            

        
        }
    }
}
