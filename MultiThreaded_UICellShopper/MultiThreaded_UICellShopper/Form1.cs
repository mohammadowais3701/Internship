using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Diagnostics;
namespace MultiThreaded_UICellShopper
{
    public partial class Form1 : Form
    {
      BindingList<ProductDetails> myproducts;
     delegate void Del_WriteData(ProductDetails obj);
     static Del_WriteData mydele;
     static bool check=false;
     List<Thread> myThreadsRunning ;
 

     
        public Form1()
        {
            InitializeComponent();
            myproducts =new BindingList<ProductDetails>();
            productDetailsBindingSource.DataSource = myproducts;
            mydele = new Del_WriteData(writeData);
       
            myThreadsRunning = new List<Thread>();
         
       
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void FillGrid() {
            try
            {
               
                cellShopper("http://cellshopper.com");

            }
            catch(Exception ex){

                MessageBox.Show(ex.Message);
            
            }
        
        }
        private void button1_Click(object sender, EventArgs e)
        {
        
          
            try {
                FillGrid();
            
            }
            catch(Exception ex){
            MessageBox.Show(ex.Message);
            
            }
          
            
        }


        static Stream makeWebRequest(string web)
        {
            Stream data;
            Uri uri = new Uri(web);

           
            HttpWebRequest myreq = (HttpWebRequest)WebRequest.CreateHttp(uri);
            WebProxy proxy = new WebProxy("127.0.0.1", 8888);
            myreq.Proxy = proxy;

            myreq.Method = "GET";
            myreq.AllowAutoRedirect = false;
            myreq.ReadWriteTimeout = 10000;
            myreq.Timeout = 10000;


            HttpWebResponse myres = (HttpWebResponse)myreq.GetResponse();

          




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
      void cellShopper(string s)
        {
            Stream data;
            int threads;
            try
            {
                threads = Convert.ToInt32(textBox1.Text);
            }
            catch(Exception ex) {
                threads = 5;
            }

            try
            {
                data = makeWebRequest(s);
                var page = new HtmlAgilityPack.HtmlDocument();
                page.Load(data);
                if (page.DocumentNode.SelectSingleNode("//div[@id='primarynav']/ul[1]/li/a") != null)
                {
                    ThreadPool.SetMinThreads(1, 1);
                    ThreadPool.SetMaxThreads(threads, threads);
                    HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//div[@id='primarynav']/ul[1]/li/a");
               
                    foreach (var n in nodes)
                    {
                      //  Console.WriteLine(n.Attributes["href"].Value);
                       ThreadPool.QueueUserWorkItem(InternalLink, "http://cellshopper.com/store/" + n.Attributes["href"].Value); 
                    //    new Thread(() => InternalLink("http://cellshopper.com/store/" + n.Attributes["href"].Value)).Start();
                    }
                }
                data.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }






        }
    void InternalLink(Object s)
        {
            String st = s as string;
            Stream data;
            try
            {
                data = makeWebRequest(st);
                var page = new HtmlAgilityPack.HtmlDocument();
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
                    InternalLink3(st);
                }
                else
                {
                    return;
                }
                data.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
     static Object myobj = new object();
     void InternalLink3(string s)
        {
            Stream data;  
            try
            {
                data = makeWebRequest(s);
                var page = new HtmlAgilityPack.HtmlDocument();
                page.Load(data);
                HtmlNodeCollection nodes = page.DocumentNode.SelectNodes("//div[@id='product_page_details']");
                if (nodes == null)
                {
                    return;
                }
                ProductDetails obj = new ProductDetails();
                //foreach (var n in nodes)
               
                    MatchCollection mt = Regex.Matches(page.DocumentNode.InnerHtml, "<h2 style=.*>");
                    foreach (var k in mt)
                    {
                        obj.productName = Convert.ToString(k);
                    }
                    Console.WriteLine();
                    obj.productName = obj.productName.Split('>')[1].Split('<')[0].Replace(",", "");

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
              lock(myobj){
                  foreach (ProductDetails product in myproducts)
                  {
                      if (product.itemId.Equals(obj.itemId))
                      {      
                          return;
                      }
                  }
               mydele(obj);
            }
                nodes = page.DocumentNode.SelectNodes("//a[@class='thickbox']");
                if (nodes == null)
                {
                    return;
                }
                Directory.CreateDirectory(obj.itemCode);
                string localFilename = obj.itemCode;
                int i = 1;
                foreach (var n in nodes)
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(n.Attributes["href"].Value, localFilename + "/" + i + ".jpg");
                    }
                    i++;
                }
                data.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("In Internal Link3 " + ex.Message);
            }
        }

void phoneList_productList(HtmlNodeCollection nodes)
        {
            string comparision = "";
            foreach (var n in nodes)
            {
                string att = (n.Attributes["href"].Value).Replace("amp;", "");               
                comparision = att;
                InternalLink("http://cellshopper.com/store/" + att);

            }
        }
void writeData(ProductDetails obj) {
            try
            {
                if (dataGridView1.InvokeRequired)
                {       
                    dataGridView1.Invoke(new MethodInvoker(() =>
                    {  myproducts.Add(new ProductDetails() { itemCode = obj.itemCode, productName = obj.productName, price = obj.price, itemId = obj.itemId, weight = obj.weight, cat = obj.cat });
                    }));
                }
                else
                {
                    myproducts.Add(new ProductDetails() { itemCode = obj.itemCode, productName = obj.productName, price = obj.price, itemId = obj.itemId, weight = obj.weight, cat = obj.cat });  
                }
            }
            catch (Exception ex)
            {
               Debug.WriteLine(ex.Message);
            }
        
        }
     
bool StartThreads(HtmlNode node) {
        check=false;
              //  Console.WriteLine(n.Attributes["href"].Value);
          if (myThreadsRunning.Count <= Convert.ToInt32(textBox1.Text))
          {
              check=true;
              myThreadsRunning.Add(new Thread(() => InternalLink("http://cellshopper.com/store/" + node.Attributes["href"].Value)));
              myThreadsRunning[myThreadsRunning.Count - 1].Start();
              return check;
          }
          else {
              for (int i = 0; i < myThreadsRunning.Count; i++) {
                  if (myThreadsRunning[i].ThreadState.Equals("Stopped")) {
                      MessageBox.Show(i+"Thread Stopped");
                      myThreadsRunning.RemoveAt(i);
                      myThreadsRunning.Add(new Thread(() => InternalLink("http://cellshopper.com/store/" + node.Attributes["href"].Value)));
                      myThreadsRunning[myThreadsRunning.Count - 1].Start();
                      check=true;
                  }
              }
              return check;
          }
}
           }
    
  }
