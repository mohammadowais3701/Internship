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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;



namespace TicketBooking
{
    class Program
    {

        static Dictionary<string, string> dic;
      static String jsonString = String.Empty;
      static String Token = String.Empty;
      static string url = "https://tix.axs.com/2L07CQAAAACSWLmzMgAAAADl%2fv%2f%2f%2fwD%2f%2f%2f%2f%2fBXRoZW1lAP%2f%2f%2f%2f%2f%2f%2f%2f%2f%2f";
        static void Main(string[] args)
        {
            start();


        }
        static void start()
        {
           try{
            dic = new Dictionary<string, string>();
            string str = makeWebRequest(url);
            string cookie="";
            string queueItUrl = "";
            if (str.Contains("hcaptcha"))
            {
                var page = new HtmlDocument();
                page.LoadHtml(str);

                HtmlNode node = page.DocumentNode.SelectSingleNode("//form[@class='challenge-form interactive-form']");
               string action= node.Attributes["action"].Value;
                Regex reg = new Regex("__cf_chl_captcha_tk__=.*?\"");
                Match m = reg.Match(str);
                string captch_tk = Convert.ToString(m).Replace("\"", "").Split('=')[1];
                cookie = updateCookies();
                Token  = Distill.ChecknSolveCaptcha("https://tix.axs.com", "hcaptcha");
                reg = new Regex("name=\"r\".*value=\".*?\"");
                m = reg.Match(str);
                string r = Convert.ToString(m);
                reg = new Regex("value=\".*?\"");
                m = reg.Match(r);
                r = Convert.ToString(m).Split('\"')[1];

                reg = new Regex("name=\"vc\".*value=\".*?\"");
                m = reg.Match(str);
                string vc = Convert.ToString(m);
                reg = new Regex("value=\".*?\"");
                m = reg.Match(vc);
                vc = Convert.ToString(m).Split('\"')[1];

                reg = new Regex("name=\"cf_captcha_kind\".*value=\".*?\"");
                m = reg.Match(str);
                string kind = Convert.ToString(m);
                reg = new Regex("value=\".*?\"");
                m = reg.Match(kind);
                kind = Convert.ToString(m).Split('\"')[1];

                reg = new Regex("cRay:.*,?");
                m = reg.Match(str);
                string id = Convert.ToString(m);
                id = id.Split(':')[1].Replace(",", "").Replace("\"","").Trim();

                reg = new Regex("cHash:.*,?");
                m = reg.Match(str);
                string cf_chl_2 = Convert.ToString(m);
                cf_chl_2 = cf_chl_2.Split(':')[1].Replace(",", "").Replace("\"", "").Trim();
              //  reg = new Regex("[\r\n\t\f\v]");
                r = r.Replace("+", "");
             //   r = r.Trim().Replace(" ","");
               

                string postData = "r=" + r + "&cf_captcha_kind=" + kind + "&vc=" + vc + "&h-captcha-response=" + Token+"&id="+id;// "&captcha_vc=69a3dc615bc2ff628ff9b54737b15a42&captcha_answer=VzgvSyVVTISz-10-62523a05dadf1943&cf_ch_verify=plat&h-captcha-response=captchka";
                cookie = cookie + "cf_chl_2=" + cf_chl_2 + ";cf_chl_prog=a21";
                queueItUrl = "https://tix.axs.com" + action;
                cookie = updateCookies(cookie+";");
                cookie= makeWebRequest("https://tix.axs.com"+action, "", cookie , "", "", "POST", "", "application/x-www-form-urlencoded", postData);
                cookie = updateCookies(cookie);

                str = makeWebRequest(url,"",cookie);
            }
            string cookies =str.Split('|')[1];
            cookies = garbageRemoving(cookies );
            cookies = updateCookies(cookies);
            str = str.Split('|')[0];
            
            if (str.Equals(""))
            {
                getHeaders obj = JsonConvert.DeserializeObject<getHeaders>(str);
                Console.WriteLine(obj.PID);
                Console.WriteLine(obj.ajaxHeader);
                cookies = Distill.getCookiesForFirstPage("https://tix.axs.com" + obj.PID, obj.ajaxHeader);
                cookies = garbageRemoving(cookies);
                Console.WriteLine(cookies);
                str = makeWebRequest(url, "", cookies);
                Console.WriteLine(str);
                str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/pre-flow/v2/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F/phase?reservation=false", "", cookies);
            }
            else
            {
                Regex reg = new Regex("cf_clearance=.*?;");
                Match m = reg.Match(cookies);
                string cooki = Convert.ToString(m).Replace(";","");
                string version = str;
                str = getContent("https://tix.axs.com/js/bundle_" + str,cookies);
                string  patt = "recaptchaV3SiteKey:.*?,";
                reg = new Regex(patt);
                m = reg.Match(str);
                string key = Convert.ToString(m).Split(':')[1].Replace("\"", "");
                key = key.Replace(",", "");
                str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/metadata/operations/all", "", cookies, "");
                cookies = str.Split('|')[1];
                cookies = garbageRemoving(cookies + ";");
                dic.Clear();
            
                cookies = updateCookies(cookies);
                cookies = updateCookies(cooki+";");
                cookies += "AMCV_B7B972315A1341150A495EFE%40AdobeOrg=870038026%7CMCIDTS%7C18682%7CMCMID%7C91546822546792475073344016533577201971%7CMCOPTOUT-1614096554s%7CNONE%7CvVersion%7C5.0.0;AMCVS_B7B972315A1341150A495EFE%40AdobeOrg=1; gpv_pn=tix.axs.com%3Acheckout%3Acaptcha; gpv_c7=tix.axs.com%3A; s_gnr7=1614089354835-New; s_cc=true;";
                cookies = updateCookies(cookies);
                str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/pre-flow/v2/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F/phase?reservation=false", "", cookies, "needed");
                var webencode = System.Net.WebUtility.UrlEncode(str.Split('|')[0]);
                Console.WriteLine(webencode);
                cookies = garbageRemoving(str.Split('|')[1]);
                String FanSightTab = str.Split('|')[2];
                cookies = cookies.Replace("|", "");
                
                //cookies = garbageRemoving(cookies.Split('|')[0]);
                cookies = updateCookies(cookies);
                string temp = cookies;
                //str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/socket.io/?transport=polling", "", cookies, FanSightTab);
                //cookies = garbageRemoving(str.Split('|')[1]);
            //    temp = cookies.Replace("|", "");
                //cookies = updateCookies(cookies);
                string Token = Distill.ChecknSolveCaptcha("https://tix.axs.com");
                string postData = "";
                cookies = temp;
                cookies = updateCookies(cookies);
                postData = "{\"locale\":\"en-US\",\"meta\":{\"version\":\"" + version.Replace(".js", "") + "\"},\"recaptchaToken\":\"" + Token + "\",\"queueItUrl\":\"" + queueItUrl + "\"}";
                temp = cookies;
                cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/session/v2/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F", "", cookies, FanSightTab, "", "POST", Token, "application/json", postData);
             //   do
               // {

                if (cookies.Split('|')[1].Equals("needToSolveRecaptcha"))
                {
                    postData = "{\"Token\":\"" + Token + "\"}";
                    cookies = garbageRemoving(cookies.Split('|')[0]);
                  //  temp = cookies.Replace("|", "");
                    cookies = updateCookies(cookies.Split('|')[0]);
                    cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/recaptcha-verification/v1/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F", "", cookies, FanSightTab, "", "POST", Token, "application/json", postData);
                } //    Token= Distill.ChecknSolveCaptcha("https://tix.axs.com");
               // } while (cookies.Equals("needToSolveRecaptcha"));
                cookies = garbageRemoving(cookies.Split('|')[0]);
                cookies= cookies.Replace("|", "");
                cookies = updateCookies(cookies);
                cookies = cookies.Replace("|", "");
               
                cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/onsale/v2/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F?sessionID=", "", cookies, FanSightTab, webencode);

             
                cookies = garbageRemoving(cookies.Split('|')[0]);
                temp = cookies;
                cookies = updateCookies(cookies.Replace(";",""));
              //  cookies = garbageRemoving(cookies.Split('|')[0]);
                //temp = cookies;
                temp = cookies;
              //  string Token = Distill.ChecknSolveCaptcha("https://tix.axs.com");

             /*   string postData = "";
                do
                {
                    cookies = temp;
                   postData= "{\"locale\":\"en-US\",\"meta\":{\"version\":\"" + version.Replace(".js", "") + "\"},\"recaptchaToken\":\"" + Token + "\",\"queueItUrl\":\"" + queueItUrl + "\"}";
                   cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/session/v2/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F", "", cookies, FanSightTab, "", "POST", Token, "application/json", postData);
                   if (cookies.Equals("needToSolveRecaptcha"))
                        Token = Distill.ChecknSolveCaptcha("https://tix.axs.com");
                } while (cookies.Equals("needToSolveRecaptcha"));*/
                //FanSightTab = cookies.Split('|')[1].Split(':')[1];
            /*    cookies = garbageRemoving(cookies.Split('|')[0]);
                temp = cookies.Replace("|", "");
                cookies = updateCookies(cookies);
                cookies = cookies.Replace("|", "");*/
                str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/inventory/v2/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F/price?locale=en-US&getSections=true&grouped=true&includeSoldOuts=false&includeDynamicPrice=true", "", temp, FanSightTab);
                cookies = garbageRemoving(str.Split('|')[0]);
                cookies = updateCookies(cookies);
                JObject obj = getJson(jsonString);
             //   str = "{\"offerId\":\"" + offerId + "\",\"offerGroupID\":\"" + offerGroupID + "\",\"productID\":\"" + productID + "\",\"qty\":\"" + qty + "\",\"priceTypeID\":\"" + priceTypeID + "\",\"section\":\"" + section + "\"}";
             
                postData = "{\"selections\":[{\"offerID\":\"" + Convert.ToString(obj["offerId"]) + "\",\"offerGroupID\":\""+Convert.ToString(obj["offerGroupID"])+"\",\"productID\":\""+Convert.ToString(obj["productID"])+"\",\"searches\":[{\"quantity\":"+Convert.ToInt32(obj["qty"])+",\"sectionID\":null,\"priceTypeID\":\""+Convert.ToString(obj["priceTypeID"])+"\",\"req\":[-1,-1]}],\"optionalFeeId\":null}],\"nextPage\":\"shop__delivery-method-page\",\"overwrite\":true}";
               cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/cart/v2/add-items?onsaleID=2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F&locale=en-US&checkPriceChange=true", "", cookies, FanSightTab, "", "POST", "", "application/json", postData);

            }

        }
        catch(Exception ex){
    
    Console.WriteLine(ex.Message);
    }

            // cookies = Distill.getCookiesForFirstPage("https://tix.axs.com" + obj.PID, obj.ajaxHeader);
            //Console.WriteLine(cookies);

        }
        static string makeWebRequest(string web, string parOfURL = "", string cookies = "", string FanSightTab = "", string sessionID = "", string type = "", string Token = "", string contenttype = "", string postData = "")
        {


            try
            {
                Uri uri = new Uri(web + sessionID + parOfURL);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                req.AllowAutoRedirect = false;
                WebProxy proxy = new WebProxy("127.0.0.1", 8888);
                req.Proxy = proxy;
                req.ServicePoint.Expect100Continue = false;
                // req.AutomaticDecompression = DecompressionMethods.GZip;
                req.Credentials = CredentialCache.DefaultCredentials;
                //  req.Accept = "application/json";
                req.Headers.Add("Accept-Encoding", "gzip,deflate,br");
                if (web.Contains("veritix"))
                {
                    req.Accept = "application/json";
                }
                else
                {
                    req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                }
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:85.0) Gecko/20100101 Firefox/85.0";
                req.Headers["Cookie"] = cookies;
                req.Headers["DNT"] = "1";

                req.Headers["Accept-Language"] = "en-US,en;q=0.5";
                req.KeepAlive = true;
                req.Referer = "https://tix.axs.com/M8rFNQAAAABVAbtHAwAAAABb%2fv%2f%2f%2fwD%2f%2f%2f%2f%2fEEJvd2VyeUJPUy1Sb3lhbGUA%2f%2f%2f%2f%2f%2f%2f%2f%2f%2f8%3d";
                req.Headers["Origin"] = "https://tix.axs.com";
                if (type.Equals(""))
                {
                    req.Method = "GET";
                    req.Headers["FanSight-Tab"] = FanSightTab;
                    if (!FanSightTab.Equals(""))
                        req.Referer = "https://tix.axs.com/";
                    
                }
                else
                {
                    req.Method = type;
                    if (!FanSightTab.Equals(""))
                    {
                        req.Referer = "https://tix.axs.com/";
                      
                        req.Headers["FanSight-Tab"] = FanSightTab;
                    }
                    if (contenttype.Equals("application/x-www-form-urlencoded"))
                    {
                        req.Headers["Upgrade-Insecure-Requests"] = "1";
                        req.ContentType = "application/x-www-form-urlencoded";
                        var data = Encoding.ASCII.GetBytes(postData);
                        req.ContentLength = data.Length;

                        using (var stream = req.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                            
                        }

                    }
                    else if (contenttype.Equals("application/json"))
                    {
                        req.ContentType = "application/json";
                        using (var streamWriter = new StreamWriter(req.GetRequestStream()))
                        {
                            //string json = "{\"locale\":\"en-US\",\"meta\":{\"version\":\"release-8.0.13-1\"},\"recaptchaToken\":\"" + Token + "\",\"queueItUrl\":\"https://tix.axs.com/M8rFNQAAAABVAbtHAwAAAABb%2fv%2f%2f%2fwD%2f%2f%2f%2f%2fEEJvd2VyeUJPUy1Sb3lhbGUA%2f%2f%2f%2f%2f%2f%2f%2f%2f%2f8%3d\"}";
                            streamWriter.Write(postData);
                        }
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
                    jsonString = str;
                    if (str.Contains("302 Found")) {

                        cookies = myres.Headers["Set-Cookie"];
                        cookies = garbageRemoving(cookies + ";");
                      //  Regex regx = new Regex("cf_clearance=.*?;");
                        //Match ms = regx.Match(cookies);
                        cookies = updateCookies(cookies);
                        return cookies;
                    }

                    if (str.Contains("needToSolveRecaptcha"))
                    {
                        return myres.Headers["Set-Cookie"]+ "|needToSolveRecaptcha";
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
                        Console.WriteLine(myres.Headers["FanSight-Tab"]);
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
                    Console.WriteLine(res.Headers);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
        public static string getContent(string web, string cookies = "")
        {
            try
            {
                Uri uri = new Uri(web);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                req.Method = "GET";
                req.AllowAutoRedirect = false;
                WebProxy proxy = new WebProxy("127.0.0.1", 8888);
                req.Proxy = proxy;
                // req.AutomaticDecompression = DecompressionMethods.GZip;
                req.Credentials = CredentialCache.DefaultCredentials;
                if (web.Contains("tix.axs"))
                {
                    req.Referer = "https://tix.axs.com/";
                    req.Accept = "*/*";
                    req.Headers["DNT"] = "1";


                }
                else {
                    req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    req.Headers["Origin"] = "https://tix.axs.com";
                    req.ContentType = "application/json";
                }
                    
                req.Headers.Add("Accept-Encoding", "gzip,deflate,br");
                req.KeepAlive = true;
               
           //     req.Accept = "application/json";
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:85.0) Gecko/20100101 Firefox/85.0";
                if (!cookies.Equals(""))
                    req.Headers["Cookie"] = cookies;
                // req.Headers["DNT"] = "1";
                //req.Headers["Security"]="Upgrade-Insecure-Requests: 1";
                req.Headers["Accept-Language"] = "en-US,en;q=0.5";
              //  req.Headers["Accept-Charset"] = " ISO-8859-1,utf-8;q=0.7,*;q=0.3";
                HttpWebResponse myres = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(myres.GetResponseStream());
                string str = reader.ReadToEnd();
                return str;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }

        }
        static string garbageRemoving(string cooki)
        {
            Regex reg = new Regex(";*[Pp]ath=\\/?;");
            string cookies = reg.Replace(cooki, "");
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
            return cookies + "|" + Convert.ToString(m);
        }

   static string updateCookies(string cookies = "")
        {
            try
            {
                string[] split = cookies.Split(';');
                for (int i = 0; i < split.Length - 1; i++)
                {
                    string temp = split[i];
                    if (dic.ContainsKey(temp.Split('=')[0]))
                    {
                        if (!temp.Split('=')[1].Equals(""))
                        dic[temp.Split('=')[0]] = temp.Split('=')[1];
                    }
                    else
                    {
                        dic.Add(temp.Split('=')[0], temp.Split('=')[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            cookies = "";
            foreach (KeyValuePair<String, String> item in dic)
            {
                cookies += item.Key + "=" + item.Value + ";";

            }
            return cookies;

        }

    static JObject getJson(string str){
        JObject obj = JObject.Parse(str);


        var offerPricesObj = obj["offerPrices"].FirstOrDefault();
        string offerId = String.Empty;
        string offerGroupID = String.Empty;

        if (offerPricesObj != null)
        {
            offerId = Convert.ToString(offerPricesObj["offerID"]);
            offerGroupID = Convert.ToString(offerPricesObj["offerGroupID"]);
            //   qty=Convert.ToInt32(offerPricesObj["order"]);
            var zonePricesobj = offerPricesObj["zonePrices"].FirstOrDefault();
            string productID = String.Empty;

            if (zonePricesobj != null)
            {
                productID = Convert.ToString(zonePricesobj["productID"]);
                var priceLevelsObj = zonePricesobj["priceLevels"].FirstOrDefault();
                int qty;
                if (priceLevelsObj != null)
                {
                    qty = Convert.ToInt32(priceLevelsObj["order"]);
                    var priceObj = priceLevelsObj["prices"].FirstOrDefault();
                    string priceTypeID = String.Empty;
                    if (priceObj != null)
                    {
                        priceTypeID = Convert.ToString(priceObj["priceTypeID"]);

                    }
                    var availabilityObj = priceLevelsObj["availability"].FirstOrDefault();
                    String section = String.Empty;
                    if (availabilityObj != null)
                    {
                        try
                        {
                            section = Convert.ToString(availabilityObj.Children().ToList()[1]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        str = "{\"offerId\":\"" + offerId + "\",\"offerGroupID\":\"" + offerGroupID + "\",\"productID\":\"" + productID + "\",\"qty\":\"" + qty + "\",\"priceTypeID\":\"" + priceTypeID + "\",\"section\":\"" + section + "\"}";
                        obj = JObject.Parse(str);
                    }
                    obj = JObject.Parse(str);
                    
                }
            }

        }
        return obj;
    
    }
    }
    class getHeaders
    {

        public string PID { get; set; }
        public string ajaxHeader { get; set; }
    }

}
