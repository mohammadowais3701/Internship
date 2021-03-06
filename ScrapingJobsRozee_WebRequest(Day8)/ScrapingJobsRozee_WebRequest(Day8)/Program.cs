﻿using System;
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
        static int flag = 0;
        static string idstr = "";
        static StreamWriter w;
        static WebRequest rq;
        static WebResponse res;
        static StreamReader reader;
        static String str;
        static MatchCollection mt;
        static MatchCollection m;
        static Stream data;
        static string patt;
        static Regex reg;
        static int num;
        static string link;
        static int n;
    
        static void Main(string[] args)
        {
           
        
            try
            {
                w = new StreamWriter("output.csv");
                var newLine = string.Format("JOB_ID,TITLE,DESCRIPTION,DISPLAY DATE,LINK,CITY,COUNTRY");
                w.WriteLine(newLine);
                w.Flush();
                num = 0;
                link = "https://www.rozee.pk/job/jsearch/q/all/fc/1184/fpn/"; 
                rq = WebRequest.Create(link+num);
                //  rq.ContentType = "application/xml";
                res= rq.GetResponse();
                data = res.GetResponseStream();
               patt = "\"list\":\\[.*?},{.*?},";
            //    reg = new Regex(patt);
               reader = new StreamReader(data, Encoding.UTF8);

                str = reader.ReadToEnd();
            //    mt = reg.Matches(str);
                n = pagesjobs(str,patt);

                patt = "\"jobs.*";


                reg = new Regex(patt);


                
               
                mt = reg.Matches(str);
               
                
                Jobs();
           


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            Console.ReadLine();
        }
        static void Jobs() {
           
            if (mt.Count == 0)
            {
                return;
            }

            foreach (var i in mt)
            {


                string[] str1 = (i.ToString()).Split('}');

                foreach (var s in str1)
                {

                    if (s.StartsWith("],\"sponsoredIds\""))
                    {

                        break;

                    }


                    JsonString(s.Remove(0, 1) + "}");

                }


            }

            num = num + n;
            Console.WriteLine(num);



            rq = WebRequest.Create(link+ num);
            res = rq.GetResponse();

            data = res.GetResponseStream();
           
           // patt = "\"jobs.*";
            reader = new StreamReader(data, Encoding.UTF8);
            str = reader.ReadToEnd();
            mt = reg.Matches(str);
            Jobs();     
        
        }


        //Num of Pages Counting here
        static int pagesjobs(String s,String p) {
            reg = new Regex(p);
            mt = reg.Matches(str);
            
            foreach (var k in mt)
            {

                patt = "fpn\":\\d*";
                reg = new Regex(patt);
                m = reg.Matches(k.ToString());

                foreach (var h in m)
                {

                    n = Convert.ToInt32(h.ToString().Split(':')[1]);

                }
               
            }
            return n;
        }

        static void JsonString(string str)
        {
           // Console.WriteLine(str);
            string patt = "{\"jid\":.*";
            Regex reg = new Regex(patt);
            var mt = reg.Matches(str);
           // Console.WriteLine(str);

            foreach (var i in mt)
            {
                // Console.WriteLine(i);
                flag = 1;
                var spl = (i.ToString()).Split(',');
                string jid = "", des = "", title = "", permalink = "", displaydate = "", country = "", city = "";
                foreach (var s in spl)
                {

                    patt = "\".*\"";
                    reg = new Regex(patt);
                    mt = reg.Matches(s);

                    foreach (var e in mt)
                    {


                        // Console.WriteLine(e);
                        if ((e.ToString()).StartsWith("\"jid\""))
                        {
                            if (idstr == "")
                            {
                                idstr = e.ToString().Split(':')[1].Trim('\"');
                                jid = e.ToString().Split(':')[1].Trim('\"');
                            }
                            /*  else if (idstr == e.ToString().Split(':')[1].Trim('\"')) {
                               
                                  flag = 1;
                                  //Console.WriteLine(flag);
                                 break;
                              }*/
                            else
                            {
                                jid = e.ToString().Split(':')[1].Trim('\"');
                            }

                            Console.WriteLine("jobId={0}", jid);
                            // Console.WriteLine(jid);
                        }
                        else if ((e.ToString()).StartsWith("\"description\""))
                        {
                            des = e.ToString().Split(':')[1].Replace("\\u00a0", " ").Replace("\"", "").Replace("\\", "");
                            Console.WriteLine("Des={0}", des);
                        }
                        else if ((e.ToString()).StartsWith("\"title\""))
                        {
                            title = e.ToString().Split(':')[1].Trim('\"');
                            Console.WriteLine("title={0}", title);
                            // Console.WriteLine(title);
                        }
                        else if ((e.ToString()).StartsWith("\"permaLink\""))
                        {
                            permalink = "https://www.rozee.pk/" + e.ToString().Split(':')[1].Trim('\"');
                            Console.WriteLine("link={0}", permalink);
                        }
                        else if ((e.ToString()).StartsWith("\"displayDate\""))
                        {
                            displaydate = e.ToString().Split(':')[1].Split(' ')[0].Trim('\"');
                            Console.WriteLine("posted Date={0}", displaydate);
                        }
                        else if ((e.ToString()).StartsWith("\"country\""))
                        {
                            country = e.ToString().Split(':')[1].Trim('\"');
                            Console.WriteLine("country={0}", country);
                        }
                        else if ((e.ToString()).StartsWith("\"city\""))
                        {
                            city = e.ToString().Split(':')[1].Trim('\"');
                            Console.WriteLine("city={0}", city);
                        }

                    }




                   /* if (flag == 1)
                    {
                        // Console.WriteLine(flag);
                        break;
                    }*/
                }



                var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6}", jid, title, des, displaydate, permalink, city, country);


                w.WriteLine(newLine);
                w.Flush();



             /*   if (flag == 1)
                {
                    //Console.WriteLine(flag);
                    break;
                }*/



            }

        }
    }

}
