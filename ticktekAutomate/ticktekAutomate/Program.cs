using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using HtmlAgilityPack;
using System.Net;
using Newtonsoft.Json;

namespace ticktekAutomate
{
    class Program
    {////*[@id="v"]
        static void Main(string[] args)
        {
            selectVenue("https://premier.ticketek.com.au/shows/show.aspx?sh=TAMEIMPA20");
            Console.ReadKey();
        }
        static Stream makeWebRequest(string web) {
            Stream data;
            Uri myurl = new Uri(web);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls| SecurityProtocolType.Tls11| SecurityProtocolType.Tls12;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(myurl);
          //  WebProxy proxy = new WebProxy("127.0.0.1", 8888);
            //req.Proxy = proxy;
            req.Method = "GET";
            HttpWebResponse myres = (HttpWebResponse)req.GetResponse();   
            data = myres.GetResponseStream();
            return data;
        }
        static void selectVenue(string str) {
            Stream data;
            try
            {
                data = makeWebRequest(str);
                var page = new HtmlAgilityPack.HtmlDocument();
                page.Load(data);
              //  Console.WriteLine(page.DocumentNode.InnerHtml);
                HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//body");
                foreach (HtmlNode n in nodes) {
                    Console.WriteLine(n.InnerHtml);
                }
            }
            catch (Exception ex) {
                Console.WriteLine("In Select Venue");
                Console.WriteLine(ex.Message);
            }
        }

    }
}
