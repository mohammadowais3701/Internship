using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;


namespace UploadFileOnServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                FileStream fstream = new FileStream(@"E:\\mongoDB_C#.pdf", FileMode.Open);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://10.0.0.209:8735/dataUpload/");
                req.ContentLength = fstream.Length;
                req.AllowWriteStreamBuffering = true;
                req.Method = "POST";
              //  req.ContentType = "multipart";
                byte[] indata = new byte [fstream.Length];
                
                int bytes_read = fstream.Read(indata, 0, indata.Length);
                
                Stream stdata = req.GetRequestStream();
                while (bytes_read > 0)

            {
                stdata.Write(indata, 0, indata.Length);
                bytes_read = fstream.Read(indata, 0, indata.Length);
               
            }
                fstream.Close();
                stdata.Close();

              /*  using (Stream stdata = req.GetRequestStream()) {
                    var file = File.OpenRead("");
                   
                    StreamReader sr = new StreamReader(file);
                    string str = sr.ReadToEnd();
                    stdata.Close();
                    Console.WriteLine(str);
                    
                }*/
               

            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
