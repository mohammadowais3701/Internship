using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Diagnostics;
using Newtonsoft.Json;


namespace TicketBooking
{
    class Program
    {
        static Dictionary<string, string> dic;
        static void Main(string[] args)
        {
            start();


        }
        static void start() {
             
           dic = new Dictionary<string, string>();
           string str = makeWebRequest("https://tix.axs.com/M8rFNQAAAABVAbtHAwAAAABb%2fv%2f%2f%2fwD%2f%2f%2f%2f%2fEEJvd2VyeUJPUy1Sb3lhbGUA%2f%2f%2f%2f%2f%2f%2f%2f%2f%2f8%3d");
           if (str.Contains("hcaptcha")) {
               Regex reg = new Regex("__cf_chl_captcha_tk__=.*?\"");
               Match m = reg.Match(str);
               string captch_tk = Convert.ToString(m).Replace("\"","");
               string cookie = updateCookies();
               string Token = Distill.ChecknSolveCaptcha("https://tix.axs.com","hcaptcha");
               str = makeWebRequest("https://tix.axs.com/M8rFNQAAAABVAbtHAwAAAABb%2fv%2f%2f%2fwD%2f%2f%2f%2f%2fEEJvd2VyeUJPUy1Sb3lhbGUA%2f%2f%2f%2f%2f%2f%2f%2f%2f%2f8%3d//shop/search?__cf_chl_captcha_tk__="+Token);
           }
           string cookies = str.Split('|')[1];
           cookies = garbageRemoving(cookies+";");
           cookies = updateCookies(cookies);
           str = str.Split('|')[0];
            if (!str.Equals("release-8.0.13-1.js"))
            {
                getHeaders obj = JsonConvert.DeserializeObject<getHeaders>(str);
                Console.WriteLine(obj.PID);
                Console.WriteLine(obj.ajaxHeader);
                cookies = Distill.getCookiesForFirstPage("https://tix.axs.com" + obj.PID, obj.ajaxHeader);
                cookies = garbageRemoving(cookies);
                Console.WriteLine(cookies);
                str = makeWebRequest("https://tix.axs.com/M8rFNQAAAABVAbtHAwAAAABb%2fv%2f%2f%2fwD%2f%2f%2f%2f%2fEEJvd2VyeUJPUy1Sb3lhbGUA%2f%2f%2f%2f%2f%2f%2f%2f%2f%2f8%3d", "", cookies);
                Console.WriteLine(str);
                str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/pre-flow/v2/M8rFNQAAAABVAbtHAwAAAABb%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FEEJvd2VyeUJPUy1Sb3lhbGUA%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F8%3D/phase?reservation=false", "", cookies);
            }
            else
            {
                str = getContent("https://tix.axs.com/js/bundle_" + str, cookies);
                string patt = "recaptchaV3SiteKey:.*?,";
                Regex reg = new Regex(patt);
                Match m = reg.Match(str);
                string key = Convert.ToString(m).Split(':')[1].Replace("\"","");
                key = key.Replace(",", "");
                string Token = Distill.ChecknSolveCaptcha("https://tix.axs.com");

                str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/metadata/operations/all", "", cookies, "");
                cookies = str.Split('|')[1];
                cookies = garbageRemoving(cookies + ";");
                cookies = updateCookies(cookies);
                str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/pre-flow/v2/M8rFNQAAAABVAbtHAwAAAABb%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FEEJvd2VyeUJPUy1Sb3lhbGUA%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F8%3D/phase?reservation=false", "", cookies, "needed");
                var webencode = System.Net.WebUtility.UrlEncode(str.Split('|')[0]);
                Console.WriteLine(webencode);
                cookies = garbageRemoving(str.Split('|')[1]);
                cookies = cookies.Replace("|", "");
                string temp = cookies;
                cookies = updateCookies(cookies);
                do
                {
                    cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/session/v2/M8rFNQAAAABVAbtHAwAAAABb%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FEEJvd2VyeUJPUy1Sb3lhbGUA%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F8%3D", "", temp, str.Split('|')[2], "", "POST", Token);
                    if (cookies.Equals("needToSolveRecaptcha"))
                    Token = Distill.ChecknSolveCaptcha("https://tix.axs.com", key);
                } while (cookies.Equals("needToSolveRecaptcha"));
                cookies = garbageRemoving(cookies.Split('|')[0]);
                cookies = updateCookies(cookies);
                cookies = cookies.Replace("|", "");
                cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/onsale/v2/M8rFNQAAAABVAbtHAwAAAABb%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FEEJvd2VyeUJPUy1Sb3lhbGUA%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F8%3D?sessionID=", "", cookies, "", webencode);
                String FanSightTab = cookies.Split('|')[1].Split(':')[1];
                cookies = garbageRemoving(cookies.Split('|')[0]);
                cookies = updateCookies(cookies);
                cookies = cookies.Replace("|", "");
                str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/inventory/v2/M8rFNQAAAABVAbtHAwAAAABb%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FEEJvd2VyeUJPUy1Sb3lhbGUA%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F8%3D/price?locale=en-US&getSections=true&grouped=true&includeSoldOuts=false&includeDynamicPrice=true", "", cookies, FanSightTab);
            }



            // cookies = Distill.getCookiesForFirstPage("https://tix.axs.com" + obj.PID, obj.ajaxHeader);
            //Console.WriteLine(cookies);
        
        }
        static string makeWebRequest(string web,string parOfURL="",string cookies="",string FanSightTab="",string sessionID="",string type="",string Token="") {
            Stream data;

            try
            {
                Uri uri = new Uri(web + sessionID + parOfURL);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                //   req.AllowAutoRedirect = false;
                WebProxy proxy = new WebProxy("127.0.0.1", 8888);
                req.Proxy = proxy;
                req.ServicePoint.Expect100Continue = false;
                // req.AutomaticDecompression = DecompressionMethods.GZip;
                req.Credentials = CredentialCache.DefaultCredentials;
                //  req.Accept = "application/json";
                req.Headers.Add("Accept-Encoding", "gzip,deflate,br");
                req.Accept = "text/html,application/xhtml+xml,application/xml,application/json;q=0.9,image/webp,*/*;q=0.8";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:85.0) Gecko/20100101 Firefox/85.0";
                req.Headers["Cookie"] = cookies;
                req.Headers["DNT"] = "1";
                req.Headers["FanSight-Tab"] = FanSightTab;
                req.Headers["Accept-Language"] = "en-US,en;q=0.5";
                req.KeepAlive = true;
                req.Referer = "https://tix.axs.com/";
                req.Headers["Origin"] = "https://tix.axs.com/";
                if (type.Equals(""))
                    req.Method = "GET";
                else
                {
                    req.ContentType = "application/json";
                    req.Method = type;
                    using (var streamWriter = new StreamWriter(req.GetRequestStream()))
                    {
                        string json = "{\"locale\":\"en-US\",\"meta\":{\"version\":\"release-8.0.13-1\"},\"recaptchaToken\":\"" + Token + "\",\"queueItUrl\":\"https://tix.axs.com/M8rFNQAAAABVAbtHAwAAAABb%2fv%2f%2f%2fwD%2f%2f%2f%2f%2fEEJvd2VyeUJPUy1Sb3lhbGUA%2f%2f%2f%2f%2f%2f%2f%2f%2f%2f8%3d\"}";
                        streamWriter.Write(json);
                    }
                }
                req.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                HttpWebResponse myres = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(myres.GetResponseStream());
                string str = reader.ReadToEnd();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(str);
                try
                {
                    if (str.Contains("needToSolveRecaptcha"))
                    {
                        return "needToSolveRecaptcha";
                    }
                    // Console.WriteLine(reader.ReadToEnd());
                    string patt = "\"\\/twrlniirtohjvlki.js\\?PID.*?\"";
                    Regex reg = new Regex(patt);
                    Match m = reg.Match(str);

                    if (!sessionID.Equals("") || str.Contains("Session created"))
                    {
                        string cookie = myres.Headers["Set-Cookie"];
                        cookie += "|FanSightTab" + myres.Headers["FanSight-Tab"];
                        return cookie;

                    }
                    else if (str.Contains("referenceNumber"))
                    {
                        patt = "referenceNumber.*?,";
                        reg = new Regex(patt);
                        m = reg.Match(str);

                        string refn = Convert.ToString(m).Split(':')[1].Replace(",", "");
                        Console.WriteLine(myres.Headers["set-cookie"]);

                        return refn + "|" + myres.Headers["set-cookie"] + "|" + myres.Headers["FanSight-Tab"];
                    }
                    else
                    {
                        if (Convert.ToString(m).Equals(""))
                        {
                            HtmlNode node = doc.DocumentNode.SelectSingleNode("//script[@src and @type='text/javascript']");
                            if (node == null)
                            {

                                patt = "release-.*?(.js)";
                                reg = new Regex(patt);
                                m = reg.Match(str);
                                return Convert.ToString(m) + "|" + myres.Headers["Set-Cookie"];

                            }
                            Console.WriteLine(node.Attributes["src"].Value);
                            //Console.WriteLine(req.Host);

                            string d = makeWebRequest("https://" + req.Host, node.Attributes["src"].Value);
                            return d;
                        }
                        else
                        {
                            patt = "ajax_header:.*?,";
                            reg = new Regex(patt);
                            Match ajaxHeader = reg.Match(str);
                            string json = "{'PID':" + Convert.ToString(m) + ",'ajaxHeader':" + Convert.ToString(ajaxHeader).Split(':')[1] + "}";
                            return json;

                        }
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);

                    return "";
                }

            }
            catch (WebException ex)
            {

                var res = ex.Response;
                if (res != null)
                {

                    var datastream = res.GetResponseStream();
                    var reader = new StreamReader(datastream);
                    cookies = res.Headers["Set-Cookie"];
                    cookies = garbageRemoving(cookies + ";");
                    cookies = updateCookies(cookies);
                    string str = reader.ReadToEnd();
                    return str;


                }

                return "";
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
    public static string getContent(string web ,string cookies="") {
        try
        {
            Uri uri = new Uri(web);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            req.Method = "GET";
            //   req.AllowAutoRedirect = false;
            WebProxy proxy = new WebProxy("127.0.0.1", 8888);
            req.Proxy = proxy;
            // req.AutomaticDecompression = DecompressionMethods.GZip;
            req.Credentials = CredentialCache.DefaultCredentials;
            //  req.Accept = "application/json";
            req.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            req.Accept = "text/html,application/xhtml+xml,application/xml,application/json;q=0.9,image/webp,*/*;q=0.8";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.4 (KHTML, like Gecko) Chrome/22.0.1229.94 Safari/537.4";
            if(!cookies.Equals(""))
            req.Headers["Cookie"] = cookies;
           // req.Headers["DNT"] = "1";
            //req.Headers["Security"]="Upgrade-Insecure-Requests: 1";
            req.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
            req.Headers["Accept-Charset"] = " ISO-8859-1,utf-8;q=0.7,*;q=0.3";
            HttpWebResponse myres = (HttpWebResponse)req.GetResponse();
            StreamReader reader = new StreamReader(myres.GetResponseStream());
            string str = reader.ReadToEnd();
            return str;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return "";
        }
            
        }
    static string garbageRemoving(string cooki) {
        Regex reg = new Regex(";*[Pp]ath=\\/?;");
       string  cookies = reg.Replace(cooki, "");
       reg = new Regex("(;)*SameSite=.*?[;]");
        cookies = reg.Replace(cookies, "");
      
        cookies = cookies.Replace("Path=/", "");
        cookies = cookies.Replace("HttpOnly;", "");
        reg = new Regex("[mM]ax-Age=.*?;");
        cookies = reg.Replace(cookies, "");
        cookies = cookies.Replace("Secure", "");
      //  cookies = cookies.Replace("; SameSite=None;", "");
        //cookies = cookies.Replace("SameSite=.*?;", "");
        cookies = cookies.Replace(",", "");
        reg = new Regex("[Ee]xpires.*?;");
        cookies = reg.Replace(cookies, "");
        reg = new Regex("[Dd]omain.*?;");
        cookies = reg.Replace(cookies, "");
        reg = new Regex("FanSightTabs:.*");
        Match m = reg.Match(cookies);
        reg = new Regex("FanSightTabs:.*");
        cookies = reg.Replace(cookies, "");
        cookies = cookies.Replace("SameSite=Strict", "");
        cookies = cookies.Replace("|", "").Replace(' ', '\b').Replace("\b", "").Replace(";;", ";");
        return cookies+"|"+Convert.ToString(m);
    }

    static string updateCookies(string cookies="") {
        try
        {
            string[] split = cookies.Split(';');
            for (int i = 0; i < split.Length - 1; i++)
            {
                string temp = split[i];
                if (dic.ContainsKey(temp.Split('=')[0]))
                {
                    dic[temp.Split('=')[0]] = temp.Split('=')[1];
                }
                else
                {
                    dic.Add(temp.Split('=')[0], temp.Split('=')[1]);
                }
            }
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        
        }
        cookies = "";
        foreach (KeyValuePair<String, String> item in dic)
        {
            cookies += item.Key + "=" + item.Value + ";";

        }
        return cookies;
    
    }
    }
    class getHeaders{

        public string PID { get; set; }
       public string ajaxHeader { get; set; }
    }
}
