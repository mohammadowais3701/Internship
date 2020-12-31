using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Xml;

namespace PostFormData_Day8_
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                WebRequest rq = WebRequest.Create("https://ptsv2.com/t/gl2j1-1609398507/post");
                  rq.Method = "POST";
                  rq.ContentType = "Application/x-www-form-urlencoded";
                  string formdata = "name=Owais";
                  formdata += "&ID=12345";
                  
                  var data = Encoding.UTF8.GetBytes(formdata);
                  rq.ContentLength = formdata.Length;
                  String username = "owais37";
                  String password = "123";
                  String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));

                   rq.Headers.Add("Authorization", "B " + encoded);
                  var stream = rq.GetRequestStream();
                  stream.Write(data, 0, formdata.Length);
                  Console.WriteLine(rq.Headers);
             
                
                var res = (HttpWebResponse)rq.GetResponse();
               
                
              

                var respstr = new StreamReader(res.GetResponseStream()).ReadToEnd();
               Console.WriteLine(respstr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                  

            }

            Console.WriteLine();






        }
    }

}
