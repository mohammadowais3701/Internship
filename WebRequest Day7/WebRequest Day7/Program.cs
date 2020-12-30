using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;

namespace WebRequest_Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            a.age = 25;
            a.Name="Ali";
            try
            {
                WebRequest req = WebRequest.Create("https://ptsv2.com/t/d45yr-1609319583/post/");
                req.Method = "POST";
                string str = "{'Name':'Owais','Age':21}";
                byte[] byt = Encoding.UTF8.GetBytes(str);

                req.ContentType = "json";
                Stream dstrea = req.GetRequestStream();
                dstrea.Write(byt, 0, byt.Length);
                dstrea.Close();
                WebResponse res = req.GetResponse();
            //    Console.WriteLine(res.GetResponseStream());
                Console.WriteLine(res);
            }
            catch (Exception ex) {
                Console.WriteLine("First Try Catch");
                Console.WriteLine(ex.Message);
            
            
            }
            try
            {
                WebRequest req = WebRequest.Create("https://ptsv2.com/t/d45yr-1609319583/d/4999192988614656");
                
                WebResponse res = req.GetResponse();
               Stream    dstrea = res.GetResponseStream();


                StreamReader reader = new StreamReader(dstrea);

                string responseFromServer = reader.ReadToEnd();

                Console.WriteLine(responseFromServer);


            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }

        

    }

    class A {
        public string Name
        {

            get;
            set;
        }
        public int age
        {

            get;
            set;

        }
    
    }
}
