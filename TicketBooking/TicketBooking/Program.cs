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
            string str = makeWebRequest("https://tix.axs.com/M8rFNQAAAABVAbtHAwAAAABb%2fv%2f%2f%2fwD%2f%2f%2f%2f%2fEEJvd2VyeUJPUy1Sb3lhbGUA%2f%2f%2f%2f%2f%2f%2f%2f%2f%2f8%3d");
            if (!str.Equals(""))
            {
                getHeaders obj = JsonConvert.DeserializeObject<getHeaders>(str);

                Console.WriteLine(obj.PID);
                Console.WriteLine(obj.ajaxHeader);
                string cookies = Distill.getCookiesForFirstPage("https://tix.axs.com" + obj.PID, obj.ajaxHeader);

                cookies = cookies.Replace("Path=/,", "");
                cookies = cookies.Replace("Path=/", "");
                cookies = cookies.Replace("HttpOnly;", "");
                Regex reg = new Regex("Max-Age=.*?;");
                cookies = reg.Replace(cookies, "");
                Console.WriteLine(cookies);

                str = makeWebRequest("https://tix.axs.com/M8rFNQAAAABVAbtHAwAAAABb%2fv%2f%2f%2fwD%2f%2f%2f%2f%2fEEJvd2VyeUJPUy1Sb3lhbGUA%2f%2f%2f%2f%2f%2f%2f%2f%2f%2f8%3d", "", cookies);
                Console.WriteLine(str);
                str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/pre-flow/v2/M8rFNQAAAABVAbtHAwAAAABb%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FEEJvd2VyeUJPUy1Sb3lhbGUA%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F8%3D/phase?reservation=false", "", cookies);
            }
            else {
                 str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/pre-flow/v2/M8rFNQAAAABVAbtHAwAAAABb%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FEEJvd2VyeUJPUy1Sb3lhbGUA%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F8%3D/phase?reservation=false","","","needed");

               // var webencode = System.Net.WebUtility.UrlEncode(str);
                //Console.WriteLine(webencode);
              //  str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/onsale/v2/M8rFNQAAAABVAbtHAwAAAABb%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FEEJvd2VyeUJPUy1Sb3lhbGUA%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F8%3D?sessionID=" + webencode);

            str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/inventory/v2/M8rFNQAAAABVAbtHAwAAAABb%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FEEJvd2VyeUJPUy1Sb3lhbGUA%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F8%3D/price?locale=en-US&getSections=true&grouped=true&includeSoldOuts=false&includeDynamicPrice=true","","",str);
            }
               
           
        
           // cookies = Distill.getCookiesForFirstPage("https://tix.axs.com" + obj.PID, obj.ajaxHeader);
            //Console.WriteLine(cookies);

        }
        static string makeWebRequest(string web,string parOfURL="",string cookies="",string FanSightTab="") {
            Stream data;
            try
            {
                Uri uri = new Uri(web+parOfURL);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                req.Method = "GET";
             //   req.AllowAutoRedirect = false;
                WebProxy proxy = new WebProxy("127.0.0.1", 8888);
                req.Proxy = proxy;
                req.AutomaticDecompression = DecompressionMethods.GZip;
                req.Credentials = CredentialCache.DefaultCredentials;
                req.Accept = "text/html,application/xhtml+xml,application/xml,application/json;q=0.9,image/webp,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.104 Safari/537.36";
                req.Headers["Cookie"]=cookies;
                req.Headers["FanSight-Tab"] = FanSightTab;
                req.Headers["Accept-Language"] = "en-US,en;q=0.5";
                req.KeepAlive = true;
             //   Console.WriteLine(req.Headers);
               // String[] cookiesArray = cookies.Split(';');
                HttpWebResponse myres = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(myres.GetResponseStream());
               

                string str= reader.ReadToEnd();
                Console.WriteLine();
              
                
                HtmlDocument doc=new HtmlDocument();
                doc.LoadHtml(str);
                try
                {
                   // Console.WriteLine(reader.ReadToEnd());
                    string  patt="\"\\/twrlniirtohjvlki.js\\?PID.*?\"";
                    Regex reg=new Regex(patt);
                    Match m = reg.Match(str);
                    if (str.Contains("referenceNumber")) {
                         patt = "referenceNumber.*?,";
                         reg = new Regex(patt);
                         m = reg.Match(str);
                        
                         string refn = Convert.ToString(m).Split(':')[1].Replace(",","");
                         Console.WriteLine(myres.Headers["set-cookie"]);
                         return myres.Headers["set-cookie"];
                    }
                    else{
                    if (Convert.ToString(m).Equals(""))
                    {
                        HtmlNode node = doc.DocumentNode.SelectSingleNode("//script[@src and @type='text/javascript']");
                        Console.WriteLine(node.Attributes["src"].Value);
                        //Console.WriteLine(req.Host);

                       string d= makeWebRequest("https://" + req.Host, node.Attributes["src"].Value);
                       return d;
                    }
                    else 
                    {
                        patt = "ajax_header:.*?,";
                        reg = new Regex(patt);
                        Match ajaxHeader = reg.Match(str);
                        string json = "{'PID':"+Convert.ToString(m)+",'ajaxHeader':"+Convert.ToString(ajaxHeader).Split(':')[1]+"}";
                        return json;
                    
                    }}
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    return "";
                }
               
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
    }
    class getHeaders{

        public string PID { get; set; }
       public string ajaxHeader { get; set; }
    }
}
