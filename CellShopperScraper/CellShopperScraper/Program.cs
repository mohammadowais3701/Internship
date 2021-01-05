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
       static Regex reg;
       static string patt;
       static MatchCollection mt;
       static string st;
       static List<string> Categories;
       static string xml;
       static StreamWriter w;
       

        static void Main(string[] args)
        {
            cellShopper();
        }

  static void cellShopper() {
      try
      {
          w = new StreamWriter("output.csv");
          var newLine = string.Format("PRODUCT NAME,ITEM CODE,ITEM ID,WEIGHT,PRICE,CATEGORY");
          w.WriteLine(newLine);
          w.Flush();
          //HttpWebRequest request = WebRequest.CreateHttp() as HttpWebRequest;
          req = WebRequest.Create("http://cellshopper.com");
          // request header missing
          res = req.GetResponse();

      }
      catch (Exception ex) {
          Console.WriteLine(ex.Message);
      
      }
      try
      {
          data = res.GetResponseStream();

          var page = new HtmlDocument();
          patt = "home.php.*\\d";
          reg = new Regex(patt);
          page.Load(data);
          HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//ul[1]/li/a");
          Categories = new List<string>();
          foreach (var n in nodes)
          {
              mt = reg.Matches(n.Attributes["href"].Value);
              foreach (var m in mt)
              {
                  patt = "home.php.*\\d";
                  xml = "//div[@id='content']/div[@id='phonelist']/a[1]";
                 // Categories.Add("http://cellshopper.com/store/" + m);
              //    InternalLink("http://cellshopper.com/store/" + m);
                  new Thread(() => InternalLink("http://cellshopper.com/store/" + m)).Start();

              }


          }
      }
      catch (Exception ex) {

          Console.WriteLine(ex.Message);
      }

    
      //InternalLink(Categories[0]);
    
            
            
        
        }
  static void InternalLink(string s) {
     
      try
      {
          req = WebRequest.Create(s);
          res = req.GetResponse();

      }
      catch (Exception ex)
      {
          Console.WriteLine(ex.Message);

      }
      try
      {
          data = res.GetResponseStream();
      
          var page = new HtmlDocument();
         // patt = "home.php.*\\d";
          //reg = new Regex(patt);
          page.Load(data);
          HtmlNodeCollection nodes = page.DocumentNode.SelectNodes(xml);
          if (nodes.Count == 0) {
              return;
          
          }
          foreach (var n in nodes) {
              string att = (n.Attributes["href"].Value).Replace("amp;", "");
              Console.WriteLine("http://cellshopper.com/store/"+att);
              xml = "//div[@id='content']/div[@id='phonelist']/a[1]";
              InternalLink1("http://cellshopper.com/store/" + att);
          
          }
      }
      catch (Exception ex) {
          Console.WriteLine(ex.Message);
      
      }

      //sample
      patt = "";
    
   // InternalLink1("http://cellshopper.com/store/home.php?cat=29997&amp;retailcat=");

      }

static void InternalLink1(string s){

    try
    {
        req = WebRequest.Create(s);
        res = req.GetResponse();

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);

    }
    try
    {
        data = res.GetResponseStream();

        var page = new HtmlDocument();
        // patt = "home.php.*\\d";
        //reg = new Regex(patt);
        page.Load(data);
        HtmlNodeCollection nodes = page.DocumentNode.SelectNodes(xml);
        if (nodes.Count == 0)
        {
            return;

        }
        foreach (var n in nodes)
        {
            string att = (n.Attributes["href"].Value).Replace("amp;","");
         //   InternalLink2("http://cellshopper.com/store/" + att);
         //   Console.WriteLine("here");
            xml = "//div[@id='content']//div[@id='products_list']/a[1]";
        //    Console.WriteLine("http://cellshopper.com/store/" + att);
      InternalLink2("http://cellshopper.com/store/" + att);

        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);

    }
    //xml = "//div[@id='content']//div[@id='products_list']/a[1]";
   // xml = "//div[@id='content']//div[@id='productlist']/a[1]";
// InternalLink2("http://cellshopper.com/store/home.php?cat=30001&retailcat=5");


}
static void InternalLink2(string s)
{

    try
    {
        req = WebRequest.Create(s);
        res = req.GetResponse();

    }
    catch (Exception ex)
    {
        
        Console.WriteLine(ex.Message);

    }
    try
    {
        data = res.GetResponseStream();

        var page = new HtmlDocument();
        // patt = "home.php.*\\d";
        //reg = new Regex(patt);
       // Console.WriteLine("here");
        page.Load(data);
      
    
        HtmlNodeCollection nodes = page.DocumentNode.SelectNodes(xml);
        if (nodes.Count == 0)
        {
            return;

        }
        foreach (var n in nodes)
        {
            string att = (n.Attributes["href"].Value).Replace("amp;", "");
            xml = "//div[@id='product_page_details']";
          Console.WriteLine("here2");
           Console.WriteLine("http://cellshopper.com/store/" + att);
   InternalLink3("http://cellshopper.com/store/" + att);

        }
    }
    catch (Exception ex)
    {
        
        Console.WriteLine(ex.Message);

    }
    xml = "//div[@id='product_page_details']";
    // xml = "//div[@id='content']//div[@id='productlist']/a[1]";
 //   InternalLink3("http://cellshopper.com/store/product.php?productid=22357&cat=29999&page=1");


}
static void InternalLink3(string s) {
    try
    {
        req = WebRequest.Create(s);
        res = req.GetResponse();

    }
    catch (Exception ex)
    {

        Console.WriteLine(ex.Message);

    }
    try
    {
        data = res.GetResponseStream();

        var page = new HtmlDocument();
        // patt = "home.php.*\\d";
        //reg = new Regex(patt);
        // Console.WriteLine("here");
        page.Load(data);


        HtmlNodeCollection nodes = page.DocumentNode.SelectNodes(xml);
        if (nodes.Count == 0)
        {
            return;

        }
        string productName="", itemcode="", itemId="", weight="", price="";
        foreach (var n in nodes)
        {
            
             productName=n.InnerText.Split('\n')[2];
             itemcode= n.InnerText.Split('\n')[4].Split(':')[1];
             itemId=n.InnerText.Split('\n')[5].Split(':')[1].Replace("&nbsp;","");
             weight=n.InnerText.Split('\n')[6].Split(':')[1].Replace("&nbsp;", "");
             price=n.InnerText.Split('\n')[7].Split(':')[1].Replace("&nbsp;", "");
            Console.WriteLine(productName);

            
           // Console.WriteLine("http://cellshopper.com/store/" + n.Attributes["href"].Value);

        }
        string cat="";
        xml = "//div[@id='bread-crumb']/a";
            nodes = page.DocumentNode.SelectNodes(xml);
            foreach (var n in nodes) {
                cat += n.InnerText+":";
            }
            cat=cat.Remove(cat.Length - 1);

            var newLine = string.Format("{0},{1},{2},{3},{4},{5}", productName, itemcode, itemId, weight, price, cat);


            w.WriteLine(newLine);
            w.Flush();
            xml = "//a[@class='thickbox']";
            nodes = page.DocumentNode.SelectNodes(xml);
         
            Directory.CreateDirectory(itemcode);
             string localFilename = itemcode;
             int i=1;
            foreach (var n in nodes)
            {
                
               
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(n.Attributes["href"].Value, localFilename+"/"+i+".jpg");
                }
                i++;
                Console.WriteLine(n.Attributes["href"].Value);
            }

    }
    catch (Exception ex)
    {

        Console.WriteLine(ex.Message);

    }



}

       


    }
}
