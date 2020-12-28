using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;   




namespace httpWebReq_Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://www.contoso.com/");
                //Console.WriteLine(req.GetResponse());
                WebClient cl1 = new WebClient();
                //  byte[] data = cl1.DownloadData("http://www.contoso.com/");
                //cl1.DownloadFile("http://www.contoso.com/", "temp.asp");

                Stream dat = cl1.OpenRead("http://www.contoso.com/");

                StreamReader reader = new StreamReader(dat);
                string str = "";
               
                while ((str=reader.ReadLine())!=null)
                {
                    
                    Console.WriteLine(str);
                  

                }
                dat.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
           
            Console.ReadKey();
            
        }
    }
}
