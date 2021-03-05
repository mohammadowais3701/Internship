using HtmlAgilityPack;
using LotGenerator_Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AXSEventsWebInterface
{
    public class AXSUtils
    {
        private static string XmlUrl = String.Empty;

        public static Boolean PostUrlToService(String url, String password = null)
        {
            Boolean result = false;
            String WR = String.Empty, eventID = String.Empty;

            try
            {
                url = url.Replace(";;;slash;;;", "/");
                url = url.Replace("%2F", "/");
                String[] query = url.Split('&');

                foreach (var item in query)
                {
                    if (item.Split('=')[0].ToLower().Equals("eventid"))
                    {
                        eventID = item.Split('=')[1];
                        break;
                    }
                    else if (item.Split('=')[0].ToLower().Equals("event"))
                    {
                        eventID = item.Split('=')[1];
                        break;
                    }
                }

                WR = HttpUtility.ParseQueryString((new Uri(url)).Query).Get("wr");

                if (string.IsNullOrEmpty(WR))
                {
                    if (url.Contains("/shop/#/"))
                    {
                        String w = url.Substring(url.IndexOf("#") + 2);
                        string[] split = w.Split('/');
                        if (split[0].Contains("?"))
                        {
                            split = split[0].Split('?');
                            WR = split[0];
                        }
                        else
                        {
                            WR = split[0];
                        }
                    }
                }

                if (string.IsNullOrEmpty(WR))
                {
                    String[] _query = url.Substring(url.IndexOf('?') + 1).Split('&');

                    foreach (var item in _query)
                    {
                        if (item.Split('=')[0].ToLower().Equals("wr"))
                        {
                            WR = item.Split('=')[1];
                            break;
                        }
                    }
                }

                //if (!processWaitingRoom(url))
                //{
                //    if (!String.IsNullOrEmpty(password))
                //    {
                //        #region Password Event handling
                //        try
                //        {
                //            HtmlDocument doc = null;
                //            string strNew = MD5Hash(WR + password + "90f22b2e");
                //            //doc.LoadHtml(post(this.XmlUrl.Replace("/info/", "/bfox/") + "?methodName=session.get&serverStr=" + this.XmlUrl.Replace("/info/", "/bfox/"), String.Format("<methodCall><methodName>session.get</methodName><params><param><value><string>{0}</string></value></param><param><value><array><data><value><string>accessQty</string></value><value><string>searchRules</string></value><value><string>options</string></value></data></array></value></param></params></methodCall>", key)));

                //            String parentIssCode = String.Empty;
                //            if (!url.ToLower().Contains("/shop/"))
                //            {
                //                int counter = 0;

                //            Retry:
                //                doc = new HtmlDocument();
                //                doc.LoadHtml(post(XmlUrl + "?methodName=showshop.jumpW&wroom=" + WR + "&lang=en&pc=" + password, String.Format("<methodCall><methodName>showshop.jumpW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>{1}</string></value></param><param><value><string>{2}</string></value></param><param><value><string>.promoGO:28:13:1</string></value></param></params></methodCall>", WR, password, strNew)));

                //                parentIssCode = doc.DocumentNode.SelectSingleNode("//methodresponse").InnerText.Trim();
                //                if (!String.IsNullOrEmpty(parentIssCode))
                //                {
                //                    url = url.Replace(WR, parentIssCode);
                //                    WR = parentIssCode;
                //                }
                //                else
                //                {
                //                    counter++;

                //                    if (counter <= 10)
                //                    {
                //                        goto Retry;
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                String str = MD5Hash(WR + password + "90f22b2e");

                //                String resutl = post(XmlUrl + "showshop.jumpW/" + WR + "/" + password + "/" + str + "?resp=json", null, "application/json");
                //                JObject obj = JObject.Parse(resutl);

                //                parentIssCode = obj["result"].ToString();

                //                if (!String.IsNullOrEmpty(parentIssCode))
                //                {
                //                    url = url.Replace(WR, parentIssCode);
                //                    WR = parentIssCode;
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            Debug.WriteLine(ex.Message);
                //        }
                //        #endregion 
                //    }
                //}

                using (TcpClient client = new TcpClient("axs1.ticketpeers.com", 44500))
                {
                    NetworkStream stream = client.GetStream();

                    ClientRequest clientReq = new ClientRequest();
                    clientReq.AppPrefix = "AAX";
                    clientReq.URL = checkO2Link(url);
                    clientReq.WR = WR;
                    clientReq.EventID = eventID;
                    clientReq.Password = password;
                    clientReq.LicenseID = "system";

                    byte[] buffer = Encoding.UTF8.GetBytes(Encryptor.Encrypt(JsonConvert.SerializeObject(clientReq)) + "<EOF>");

                    stream.Write(buffer, 0, buffer.Length);
                    stream.Close();
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        private static Boolean processWaitingRoom(String url)
        {
            bool ifValidated = false;
            string wr = string.Empty;
            HtmlDocument doc = new HtmlDocument();

            try
            {
                //if (string.IsNullOrEmpty(this.wrNew))
                {
                    wr = HttpUtility.ParseQueryString((new Uri(url)).Query).Get("wr");

                    if (string.IsNullOrEmpty(wr))
                    {
                        if (url.Contains("/shop/#/"))
                        {
                            String w = url.Substring(url.IndexOf("#") + 2);
                            string[] split = w.Split('/');
                            if (split[0].Contains("?"))
                            {
                                split = split[0].Split('?');
                                wr = split[0];
                            }
                            else
                            {
                                wr = split[0];
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(wr))
                    {
                        String[] query = url.Substring(url.IndexOf('?') + 1).Split('&');

                        foreach (var item in query)
                        {
                            if (item.Split('=')[0].ToLower().Equals("wr"))
                            {
                                wr = item.Split('=')[1];
                                break;
                            }
                        }
                    }
                }

            Retry:
                if (!url.Contains("/shop/#/"))
                {
                    doc.LoadHtml(post(String.Format("https://tickets.axs.com/xmlrpc?methodName=getPhase&lotID=noLotId_7&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>noLotId_7</string></value></param></params></methodCall>", wr), "text/xml"));
                }
                else
                {
                    doc.LoadHtml(post(String.Format("https://tickets.axs.com/xmlrpc?methodName=getPhase&lotID=noLotId&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>noLotId</string></value></param></params></methodCall>", wr), "text/xml"));//"application/x-www-form-urlencoded"));
                }

                HtmlNode enterPauseNode = doc.DocumentNode.SelectSingleNode("//name[text() = 'enterPause']");

                HtmlNodeCollection nodehash = doc.DocumentNode.SelectNodes("//member/name");
                if (nodehash != null)
                {
                    foreach (HtmlNode item in nodehash)
                    {
                        if (item.InnerHtml.Contains("info_path"))
                        {
                            if (item.NextSibling.FirstChild != null)
                            {
                                XmlUrl = item.NextSibling.FirstChild.InnerText.Trim();
                            }
                            else
                            {
                                XmlUrl = item.NextSibling.NextSibling.FirstChild.InnerText.Trim();
                            }
                        }
                    }
                }

                if ((enterPauseNode != null))
                {
                    int enterPause = 0;

                    if (enterPauseNode.NextSibling.ChildNodes.Count > 0)
                    {
                        enterPause = Convert.ToInt32(enterPauseNode.NextSibling.ChildNodes[0].InnerHtml);
                    }
                    else
                    {
                        enterPause = Convert.ToInt32(enterPauseNode.NextSibling.NextSibling.ChildNodes[0].InnerHtml);
                    }
                    enterPause *= 1000;

                    ifValidated = true;
                }
                else
                {
                    ifValidated = false;
                }
            }
            catch (Exception)
            {
                ifValidated = false;
            }

            return ifValidated;
        }

        private static String checkO2Link(String O2link)
        {
            string link = O2link;
            try
            {
                if (O2link.Contains("o2priority"))
                {
                    string remove = O2link.Substring(0, O2link.IndexOf('?'));
                    O2link = O2link.Replace(remove, "https://axsmobile.eventshopper.com/mobilewroom/");
                    link = O2link;
                }
            }
            catch
            { }
            return link;
        }

        public static String MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public static String post(string url, string postdata)
        {
            string URL = url; string Result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.Expect100Continue = false;
            request.ServicePoint.Expect100Continue = false;
            request.KeepAlive = true;

            if (url.Contains("jumpW"))
            {
                if (!url.Contains("/shop/"))
                {
                    //webRequest.Headers.Add("X-Requested-With", "ShockwaveFlash/21.0.0.182");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
                    request.ContentType = "text/html";
                    request.Accept = "*/*";
                }
                else
                {
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
                    request.ContentType = "application/json";
                    request.Accept = "*/*";
                    request.Referer = "https://tickets.axs.com/shop/";
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    request.Headers.Add("Origin", "https://tickets.axs.com");
                }
            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            HttpWebResponse resp = null;
            Encoding respenc = null;
            var isGZipEncoding = false;
            Stream stream = null;

            if (!String.IsNullOrEmpty(postdata))
            {
                request.Method = "POST";
                string body = postdata;
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
            }
            else
            {
                request.Method = "GET";
            }

            try
            {
                resp = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException we)
            {
                resp = (HttpWebResponse)we.Response;
            }

            if (resp != null)
            {
                if (!string.IsNullOrEmpty(resp.ContentEncoding))
                {
                    isGZipEncoding = resp.ContentEncoding.ToLower().StartsWith("gzip") ? true : false;
                    if (!isGZipEncoding)
                    {
                        respenc = Encoding.GetEncoding(resp.ContentEncoding);
                    }
                }

                if (isGZipEncoding)
                {
                    stream = new GZipStream(resp.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    stream = resp.GetResponseStream();
                }

                StreamReader sr = new StreamReader(stream);
                Result = sr.ReadToEnd();
            }
            stream.Close();
            return Result;
        }

        public static String post(string url, string postdata, String contentType)
        {
            string URL = url; string Result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.Expect100Continue = false;
            request.ServicePoint.Expect100Continue = false;
            request.KeepAlive = true;

            if (url.Contains("jumpW"))
            {
                if (!url.Contains("/shop/"))
                {
                    //webRequest.Headers.Add("X-Requested-With", "ShockwaveFlash/21.0.0.182");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
                    request.ContentType = "text/html";
                    request.Accept = "*/*";
                }
                else
                {
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
                    request.ContentType = "application/json";
                    request.Accept = "*/*";
                    request.Referer = "https://tickets.axs.com/shop/";
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    request.Headers.Add("Origin", "https://tickets.axs.com");
                }
            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            if (!String.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }

            HttpWebResponse resp = null;
            Encoding respenc = null;
            var isGZipEncoding = false;
            Stream stream = null;

            if (!String.IsNullOrEmpty(postdata))
            {
                request.Method = "POST";
                string body = postdata;
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
            }
            else
            {
                request.Method = "GET";
            }

            try
            {
                resp = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException we)
            {
                resp = (HttpWebResponse)we.Response;
            }

            if (resp != null)
            {
                if (!string.IsNullOrEmpty(resp.ContentEncoding))
                {
                    isGZipEncoding = resp.ContentEncoding.ToLower().StartsWith("gzip") ? true : false;
                    if (!isGZipEncoding)
                    {
                        respenc = Encoding.GetEncoding(resp.ContentEncoding);
                    }
                }
                if (isGZipEncoding)
                {
                    stream = new GZipStream(resp.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    stream = resp.GetResponseStream();
                }

                StreamReader sr = new StreamReader(stream);
                Result = sr.ReadToEnd();
            }
            stream.Close();
            return Result;
        }
    }
}
