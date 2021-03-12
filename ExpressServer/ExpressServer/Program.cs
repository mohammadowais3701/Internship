using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace ExpressServer
{
    class Program
    {
        static void Main(string[] args)
        {
            startApp();
           
        }
        static void startApp() {
            string status = "Registered Successfully";
            status = Req("http://localhost:3000/loginRoutes/registration", "POST");
            if (status.Contains("Registered Successfully"))
            {
                doReq();
            }

        }
        static void  doReq(){
            string Token = Req("http://localhost:3000/loginRoutes/login/name=Owais&password=123", "GET");
            string Details = Req("http://localhost:3000/wiki/AuthorsList/", "POST", Token);
            string t = Req("http://localhost:3000/wiki/", "GET", Token);
        }
        static string Req(String url,String Method, String Token="") {
            try
            {
                if (Method == "GET")
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = "GET";
                    req.ContentType = "application/json";
                    req.Headers["auth"] = Token;
                    HttpWebResponse myres = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(myres.GetResponseStream());
                    Token = reader.ReadToEnd();
                    return Token;
                }
                else {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = "POST";
                    req.ContentType = "application/json";
                    req.Headers["auth"] = Token;
                    string data = string.Empty;
                    if (!url.Contains("registration"))
                    {
                       
                         data = "{\"id\":3,\"name\":\"Owais\"}";
                        
                    }
                    else {
                       Console.WriteLine("Enter UserName");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter Password");
                        string pass = Console.ReadLine();
                        Console.WriteLine("Enter Role");
                        string role = Console.ReadLine();
                      /*  string name = "owais";
                        string pass = "123";
                        string role = "editorrr";*/
                        data = "{\"username\":\""+name+"\",\"password\":\""+pass+"\",\"role\":\""+role+"\"}";

                    }
                    using (var streamWriter = new StreamWriter(req.GetRequestStream()))
                    {
                            streamWriter.Write(data);
                    }
                   
                    HttpWebResponse myres = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(myres.GetResponseStream());
                    String Details = reader.ReadToEnd();
                    return Details;     
                }
                
            }
            catch (Exception ex) {

                Console.WriteLine(ex.Message);
                
            }
            return "ERROR";
        }
    }
}
