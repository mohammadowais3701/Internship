using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AXSTixToxService
{
    class Program
    {
        private static HttpListener listener;
        private static Thread listenThread;
        static String usKey = "6LexTBoTAAAAAESv_PtNKgDQM7ZP9KOKedZUbYay";

        // regular-ass C# entry point!
        static void Main(string[] args)
        {
            //while (!Debugger.IsAttached)
            //{
            //    // console.out.writeline("waiting for debugger");
            //    Thread.Sleep(5000);
            //}

            listener = new HttpListener();
            //Can add multiple prefixes for others.
            listener.Prefixes.Add("http://localhost:8086/");
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

            listener.Start();
            listenThread = new Thread(new ParameterizedThreadStart(startlistener));
            listenThread.Start();
        }

        private static void startlistener(object s)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        ////blocks until a client has connected to the server
                        if (listener.IsListening)
                        {
                            ProcessRequest();
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void ProcessRequest()
        {
            try
            {
                if (listener.IsListening)
                {
                    var result = listener.BeginGetContext(ListenerCallback, listener);  //Begins asynchronously retrieving an incoming request.
                    result.AsyncWaitHandle.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void ListenerCallback(IAsyncResult result)
        {
            try
            {
                //Completes an asynchronous operation to retrieve an incoming client request.
                var context = listener.EndGetContext(result);
                Thread.Sleep(1000);
                var data_text = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd();

                var cleaned_data = System.Web.HttpUtility.UrlDecode(data_text);

                context.Response.StatusCode = 200;
                context.Response.StatusDescription = "OK";

                StringBuilder sb = new StringBuilder();

                if (context.Request.HttpMethod.ToLower().Equals("get"))
                {
                    String key = usKey;
                    if (!String.IsNullOrEmpty(context.Request.Url.Query))
                    {
                        key = context.Request.Url.Query.Replace("?", "");
                    }

                    sb.Append("<html><head><title>reCAPTCHA demo: Simple page</title><script src=\"https://www.google.com/recaptcha/api.js\" async defer></script></head><body>   <form action=\"?\" method=\"POST\"><div class=\"g-recaptcha\" data-sitekey=\"" + key + "\"></div><br/><input type=\"submit\" value=\"Submit\"></form></body></html>");
                }
                else if (context.Request.HttpMethod.ToLower().Equals("post"))
                {
                    String response = String.Empty;
                    if (cleaned_data.Contains("g-recaptcha-response"))
                    {
                        response = cleaned_data.Replace("g-recaptcha-response=", String.Empty);
                        //Console.WriteLine(response);
                    }
                    sb.Append(String.Format("<html><head><title>last page</title></head><body><form><input type=\"hidden\" id=\"g-captcha-response\" value=\"{0}\" /></form></body></html>", cleaned_data));
                }

                byte[] b = Encoding.UTF8.GetBytes(sb.ToString());
                context.Response.ContentLength64 = b.Length;
                context.Response.OutputStream.Write(b, 0, b.Length);
                context.Response.OutputStream.Close();

                context.Response.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
