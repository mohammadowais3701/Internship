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
      
       static StreamWriter w;
       

   static void Main(string[] args)
        {
           
          //  cellShopper("http://cellshopper.com");
            makeWebRequest("http://cellshopper.com");
            
        }




   static Stream makeWebRequest(string web) {
            Stream data;
            HttpWebRequest myreq = (HttpWebRequest)WebRequest.CreateHttp(web);

            myreq.Method = "GET";
        //  myreq.AllowAutoRedirect = false;
            
            HttpWebResponse myres = (HttpWebResponse)myreq.GetResponse();
            Console.WriteLine(myres.Headers["location"]);
            if (myres.Headers["location"]== null)
            {
                data = myres.GetResponseStream();
                return data;
            }
            //   Console.WriteLine(myres.Headers["location"]);

            else  {

                if (myres.Headers["location"].StartsWith(".."))
                {
                    string[] str = web.Split('/');
                    

                }
                else
                {
                    data = makeWebRequest(web + "/" + myres.Headers["location"]);
                    return data;
                }
            }
          
            

        
        }

  static void cellShopper(string s) {
      Stream data;
      Regex reg;
      MatchCollection mt;
     
      try
      {
          w = new StreamWriter("output.csv");
         

              var newLine = string.Format("PRODUCT NAME,ITEM CODE,ITEM ID,WEIGHT,PRICE,CATEGORY");

              w.WriteLine(newLine);
              w.Flush();


          
          data = makeWebRequest(s);
          var page = new HtmlDocument();
         
          page.Load(data);
          //
          if (page.DocumentNode.SelectNodes("//ul[1]/li/a") != null)
          {
              HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//ul[1]/li/a");
              foreach (var n in nodes)
              {
               
                  reg = new Regex("home.php.*\\d");
                  mt = reg.Matches(n.Attributes["href"].Value);
                  foreach (var m in mt)
                  {


       //new Thread(() => InternalLink("http://cellshopper.com/store/" + m)).Start();
          InternalLink("http://cellshopper.com/store/" + m);
                  }


              }

          }


          data.Close();
    
      }
      catch (Exception ex)
      {
          Console.WriteLine("In Cell shopper");
          Console.WriteLine(ex.Message);
        
      }

      

            
            
        
        }
  static void InternalLink(string s) {

      Stream data;
      try
      {
          data = makeWebRequest(s);
      
          var page = new HtmlDocument();
        
          page.Load(data);

      if (page.DocumentNode.SelectNodes("//div[@id='content']/div[@id='phonelist']/a[1]")!=null)
          {

              HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//div[@id='content']/div[@id='phonelist']/a[1]");
              foreach (var n in nodes)
              {
                  string att = (n.Attributes["href"].Value).Replace("amp;", "");
                  Console.WriteLine("http://cellshopper.com/store/" + att);

                  InternalLink("http://cellshopper.com/store/" + att);

              }
          }
          else if (page.DocumentNode.SelectNodes("//div[@id='content']//div[@id='products_list']/a[1]") != null)
          {
              HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//div[@id='content']//div[@id='products_list']/a[1]");
              foreach (var n in nodes)
              {
                  string att = (n.Attributes["href"].Value).Replace("amp;", "");

                  Console.WriteLine("here2");
                  Console.WriteLine("http://cellshopper.com/store/" + att);
                  InternalLink("http://cellshopper.com/store/" + att);

              }
          }
      else if (page.DocumentNode.SelectNodes("//div[@id='product_page_details']") != null)
      {
          InternalLink3(s);

      }
      else {
          return;
      
      }
      data.Close();
   
      }
      catch (Exception ex) {
          Console.WriteLine("IN Internal Link");
          Console.WriteLine(ex.Message);
      
      }

 
 
      }

static void InternalLink3(string s) {
    Stream data;
    String str;
   
    try
    {
        data = makeWebRequest(s);

        var page = new HtmlDocument();
   
        page.Load(data);

       
        HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//div[@id='product_page_details']");
        if (nodes == null)
        {
            return;

        }
        Regex reg;
        MatchCollection mt;
        ProductDetails obj=new ProductDetails();
      lock(obj){  
        foreach (var n in nodes)
        {
            
             reg = new Regex("<h2 style=.*>");
             mt = reg.Matches(n.InnerHtml);
             foreach(var k in mt){
                 obj.productName =Convert.ToString(k);
             }
             Console.WriteLine();
             obj.productName = obj.productName.Split('>')[1].Split('<')[0].Replace(",","|");
             reg = new Regex("Item Code:.*<");
             mt = reg.Matches(n.InnerHtml);
             foreach (var k in mt)
             {
                 obj.itemCode = Convert.ToString(k);
             }
             obj.itemCode = obj.itemCode.Split('<')[0].Split(':')[1].Trim().Replace("&nbsp;", "");

            // itemcode= n.InnerText.Split('\n')[4].Split(':')[1];

             reg = new Regex("Item ID:.*<");
             mt = reg.Matches(n.InnerHtml);
             foreach (var k in mt)
             {
                 obj.itemId = Convert.ToString(k);
             }
             obj.itemId = obj.itemId.Split('<')[0].Split(':')[1].Replace("&nbsp;","");
            // itemId=n.InnerText.Split('\n')[5].Split(':')[1].Replace("&nbsp;","");
             reg = new Regex("Weight.*<");
             mt = reg.Matches(n.InnerHtml);
             foreach (var k in mt)
             {
                obj.weight= Convert.ToString(k);
             }
             obj.weight = obj.weight.Split('<')[0].Split(':')[1].Replace("&nbsp;", "");
            // weight=n.InnerText.Split('\n')[6].Split(':')[1].Replace("&nbsp;", "");

             reg = new Regex("Price:.*<");
             mt = reg.Matches(n.InnerHtml);
             foreach (var k in mt)
             {
                 obj.price = Convert.ToString(k);
             }
             string[] prices = obj.price.Split(':')[1].Replace("&nbsp;", "").Replace("<s>", "").Replace("</s>", ",").Replace("<", "").Split(',');
             if (prices.Length == 2)
                obj.price = prices[1];
             else 
                obj.price=prices[0];
             
             //price=n.InnerText.Split('\n')[7].Split(':')[1].Replace("&nbsp;", "");
          

            
           // Console.WriteLine("http://cellshopper.com/store/" + n.Attributes["href"].Value);

        }

        obj.cat = "";
            nodes = page.DocumentNode.SelectNodes("//div[@id='bread-crumb']/a");
            if (nodes == null)
            {
                return;

            }
            foreach (var n in nodes) {
                obj.cat += n.InnerText+":";
            }
            obj.cat=obj.cat.Remove(obj.cat.Length - 1);

            var newLine = string.Format("{0},{1},{2},{3},{4},{5}", obj.productName, obj.itemCode, obj.itemId, obj.weight, obj.price, obj.cat);

           
            w.WriteLine(newLine);
                //File.AppendAllText("output.csv", newLine);
            w.Flush();

            
            
            nodes = page.DocumentNode.SelectNodes("//a[@class='thickbox']");
            if (nodes == null)
            {
                return;

            }
            Directory.CreateDirectory(obj.itemCode);
             string localFilename = obj.itemCode;
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
      data.Close();
    }
    catch (Exception ex)
    {
         Console.WriteLine("IN Internal Link3");
        Console.WriteLine(ex.Message);

    }



}

       


    }
}
