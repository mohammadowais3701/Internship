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
        private static long counter;
        static List<ProductDetails> myproducts=new List<ProductDetails>();


        static void Main(string[] args)
        {

            cellShopper("http://cellshopper.com");
            //          makeWebRequest("http://cellshopper.com");


        }




        static Stream makeWebRequest(string web)
        {
            Stream data;
            Uri uri = new Uri(web);
            Console.WriteLine(uri);
            HttpWebRequest myreq = (HttpWebRequest)WebRequest.CreateHttp(uri);
            WebProxy proxy = new WebProxy("127.0.0.1", 8888);
            myreq.Proxy = proxy;

            myreq.Method = "GET";
            myreq.AllowAutoRedirect = false;


            HttpWebResponse myres = (HttpWebResponse)myreq.GetResponse();

            Console.WriteLine(myres.Headers["location"]);




            if (myres.Headers["location"] == null)
            {

                data = myres.GetResponseStream();
                return data;
            }


            else
            {

                if (myres.Headers["location"].StartsWith(".."))
                {
                    // Console.WriteLine(myres.Headers["location"]);
                    string loc = "", cur_str = "";

                    for (int i = 0; i < uri.Segments.Length - 2; i++)
                    {

                        cur_str += uri.Segments[i];
                    }
                    cur_str += myres.Headers["location"].Replace("../", "");
                    loc = "http://" + uri.Host;



                    data = makeWebRequest(loc + cur_str);
                    return data;

                }
                else
                {

                    data = makeWebRequest(web + "/" + myres.Headers["location"]);
                    return data;
                }
            }




        }

        static void cellShopper(string s)
        {
            Stream data;
           

            try
            {
                w = new StreamWriter("output.csv");


                var newLine = string.Format("PRODUCT NAME,ITEM CODE,ITEM ID,WEIGHT,PRICE,CATEGORY");

                w.WriteLine(newLine);
                w.Flush();

                w.Close();

                data = makeWebRequest(s);
                var page = new HtmlDocument();

                page.Load(data);
              
              if( page.DocumentNode.SelectSingleNode("//div[@id='primarynav']/ul[1]/li/a")!=null)

                {

                    HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//div[@id='primarynav']/ul[1]/li/a");
                    foreach (var n in nodes)
                    {
                      //  Console.WriteLine(n.Attributes["href"].Value);



                        Thread t1 = new Thread(() => InternalLink("http://cellshopper.com/store/" + n.Attributes["href"].Value));
                            t1.Start();

                        //InternalLink("http://cellshopper.com/store/" + n.Attributes["href"].Value);
                      


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


        static void InternalLink(string s)
        {

            Stream data;
            try
            {
                data = makeWebRequest(s);

                var page = new HtmlDocument();

                page.Load(data);

                if (page.DocumentNode.SelectNodes("//div[@id='content']/div[@id='phonelist']/a") != null)
                {


                    HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//div[@id='content']/div[@id='phonelist']/a[not(.//img)]");
                    phoneList_productList(nodes);

                }
                else if (page.DocumentNode.SelectNodes("//div[@id='content']//div[@id='products_list']/a") != null)
                {

                    HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//div[@id='content']//div[@id='products_list']/a");
                    phoneList_productList(nodes);

                }
                else if (page.DocumentNode.SelectNodes("//div[@id='product_page_details']") != null)
                {


                    InternalLink3(s);

                }
                else
                {
                    return;

                }
                data.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("IN Internal Link");
                Console.WriteLine(ex.Message);

            }



        }
      static  Object myobj = new object();
        static void InternalLink3(string s)
        {

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
                
                


                ProductDetails obj = new ProductDetails();

                //foreach (var n in nodes)
                {
                    MatchCollection mt = Regex.Matches(page.DocumentNode.InnerHtml, "<h2 style=.*>");
                    foreach (var k in mt)
                    {
                        obj.productName = Convert.ToString(k);
                    }
                    Console.WriteLine();
                    obj.productName = obj.productName.Split('>')[1].Split('<')[0].Replace(",","");

                    mt = Regex.Matches(page.DocumentNode.InnerHtml, "Item Code:.*<");
                    foreach (Match k in mt)
                    {
                        obj.itemCode = k.Value;
                    }
                    obj.itemCode = obj.itemCode.Split('<')[0].Split(':')[1].Trim().Replace("&nbsp;", "");

                    mt = Regex.Matches(page.DocumentNode.InnerHtml, "Item ID:.*<");
                    foreach (Match k in mt)
                    {
                        obj.itemId = k.Value;
                    }
                    obj.itemId = obj.itemId.Split('<')[0].Split(':')[1].Replace("&nbsp;", "");

                    mt = Regex.Matches(page.DocumentNode.InnerHtml, "Weight.*<");
                    foreach (Match k in mt)
                    {
                        obj.weight = k.Value;
                    }
                    obj.weight = obj.weight.Split('<')[0].Split(':')[1].Replace("&nbsp;", "");

                    mt = Regex.Matches(page.DocumentNode.InnerHtml, "Price:.*<");

                    foreach (Match k in mt)
                    {
                        obj.price = k.Value;
                    }
                    string[] prices = obj.price.Split(':')[1].Replace("&nbsp;", "").Replace("<s>", "").Replace("</s>", ",").Replace("<", "").Split(',');
                    if (prices.Length == 2)
                        obj.price = prices[1];
                    else
                        obj.price = prices[0];

                    //price=n.InnerText.Split('\n')[7].Split(':')[1].Replace("&nbsp;", "");



                    // Console.WriteLine("http://cellshopper.com/store/" + n.Attributes["href"].Value);

                }

                obj.cat = "";
                nodes = page.DocumentNode.SelectNodes("//div[@id='bread-crumb']/a");
                if (nodes == null)
                {
                    return;

                }
                foreach (var n in nodes)
                {
                    obj.cat += n.InnerText + ":";
                }
                obj.cat = obj.cat.Remove(obj.cat.Length - 1);

                Interlocked.Increment(ref counter);

                var newLine = string.Format("{0},{1},{2},{3},{4},{5}", obj.productName, obj.itemCode, obj.itemId, obj.weight, obj.price, obj.cat);
                lock (myobj)
                {   int check=0;
                foreach (ProductDetails product in myproducts)
                {
                    if (product.itemId.Equals(obj.itemId))
                    {
                        check = 1;
                        break;
                    }
                    if (check == 0) {
                        myproducts.Add(obj);
                    
                    }
                }
                    

                   // File.AppendAllLines("output.csv", new String[] { newLine });

                }

                Console.Title = counter.ToString();

                nodes = page.DocumentNode.SelectNodes("//a[@class='thickbox']");
                if (nodes == null)
                {
                    return;

                }
                Directory.CreateDirectory(obj.itemCode);
                string localFilename = obj.itemCode;
                int i = 1;
                lock (myobj)
                {
                    foreach (var n in nodes)
                    {


                        using (WebClient client = new WebClient())
                        {
                            client.DownloadFile(n.Attributes["href"].Value, localFilename + "/" + i + ".jpg");
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


        static void phoneList_productList(HtmlNodeCollection nodes)
        {
            string comparision = "";
            foreach (var n in nodes)
            {
                string att = (n.Attributes["href"].Value).Replace("amp;", "");
               
                Console.WriteLine("http://cellshopper.com/store/" + att);
                comparision = att;
                InternalLink("http://cellshopper.com/store/" + att);

            }

        }
        static void writeProducts() { 
        
        
        }

    }
}
