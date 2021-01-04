using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace CellShopperScraper
{
    class Program
    {
       static WebRequest req;
       static WebResponse res;
       static Stream data;
       static StreamReader reader;
       static string str;


        static void Main(string[] args)
        {
            cellShopper();
        }

  static void cellShopper() {
      try
      {
          req = WebRequest.Create("http://cellshopper.com");
          res = req.GetResponse();

      }
      catch (Exception ex) {
          Console.WriteLine(ex.Message);
      
      }
            data = res.GetResponseStream();
            //reader = new StreamReader(data, Encoding.UTF8);
            //str = reader.ReadToEnd();
            var page = new HtmlDocument();

            page.Load(data);
            HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//ul[1]/li/a");

            foreach (var n in nodes) {
               
                Console.WriteLine(n.Attributes["href"].Value);
            
            }


            
            
        
        }
    }
}
