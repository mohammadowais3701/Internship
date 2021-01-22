using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Xml;

namespace PostDATAonWCFServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://10.0.0.209:8735/dataInsert/4/2");
                req.Method = "POST";
                req.ContentType = "html/text";
                string data = "This is post data";
                byte[] arr = Encoding.UTF8.GetBytes(data);
                Stream stdata = req.GetRequestStream();
                stdata.Write(arr, 0, arr.Length);
                stdata.Close();
                var res = (HttpWebResponse)req.GetResponse();
                var respstr = new StreamReader(res.GetResponseStream()).ReadToEnd();
               
                Console.WriteLine(respstr.Split('>')[1].Split('<')[0]);
                Console.ReadLine();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
         
        }
    }
    class jsonText{
        public string str
        {
            get;
            set;
        }
        public string text
        {
            get;
            set;
        }
    
    }
}
