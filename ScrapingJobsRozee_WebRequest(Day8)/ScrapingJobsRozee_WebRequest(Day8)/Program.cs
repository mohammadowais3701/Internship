using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Xml;
using System.Json;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
namespace ScrapingJobsRozee_WebRequest_Day8_
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                WebRequest rq = WebRequest.Create("https://www.rozee.pk/job/jsearch/q/all/fc/1184/fpn/20");
              //  rq.ContentType = "application/xml";
                WebResponse res = rq.GetResponse();
                Stream data = res.GetResponseStream();
                string patt = "\"jobs.*";

                Regex reg = new Regex(patt);
               
               
               StreamReader reader;
               reader = new StreamReader(data, Encoding.UTF8);
               
                String str = reader.ReadToEnd();
                MatchCollection mt = reg.Matches(str);
                foreach (var i in mt) {
                 
                   string[] str1=(i.ToString()).Split('}');

                    foreach(var s in str1){
                        if (s.StartsWith("],\"sponsoredIds\""))
                        {

                            break;

                        }
                       
                        
                       JsonString(s.Remove(0,1)+"}");
                    }

                
                }
               // XmlDocument xml = new XmlDocument();
                //Console.WriteLine(res.Headers);
               
               // xml.Load(res.GetResponseStream());

             

            //  xml = JsonConvert.;

               
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            
            }
            Console.ReadLine();
        }

    static void JsonString(string str) {
        string patt = "{\"jid\":.*";
        Regex reg = new Regex(patt);
        var mt = reg.Matches(str);
        
        foreach (var i in mt) {
            Console.WriteLine(i);
        
        }
          
        }
    }
    class DATA {
       public string Title
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;

        }
        public DateTime postedDate
        {

            get;
            set;
        }

        public string url
        {
            get;
            set;


        }
    
    }
}
