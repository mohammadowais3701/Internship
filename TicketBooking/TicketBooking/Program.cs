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
                cookie = cookie + "cf_chl_2="+cf_chl_2 +";cf_chl_prog=a21";
                queueItUrl = "https://tix.axs.com" + action;
                cookie = updateCookies(cookie+";");
                cookie= makeWebRequest("https://tix.axs.com"+action, "", cookie , "", "", "POST", "", "application/x-www-form-urlencoded", postData);
                dic.Remove("cf_chl_2");
                dic.Remove("cf_chl_prog");
                cookie = updateCookies(cookie);
                dic.Remove("cf_chl_2");
                dic.Remove("cf_chl_prog");
                cookie = updateCookies();
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
                cookies = updateCookies(cooki + ";");
                cookies += "AMCV_B7B972315A1341150A495EFE%40AdobeOrg=1075005958%7CMCIDTS%7C18306%7CMCMID%7C83045388051431756754518782686414686273%7CMCOPTOUT-1581622579s%7CNONE%7CvVersion%7C4.4.1;AMCVS_B7B972315A1341150A495EFE%40AdobeOrg=1;s_gnr7="+ UnixTimeNow(DateTime.Now) +"-New; s_cc=true;";
                cookies = updateCookies(cookies);
                str = getContent("https://tix.axs.com/js/bundle_" + str,cookies);
                string  patt = "recaptchaV3SiteKey:.*?,";
                reg = new Regex(patt);
                m = reg.Match(str);
                string key = Convert.ToString(m).Split(':')[1].Replace("\"", "");
                key = key.Replace(",", "");
            /*    str = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/metadata/operations/all", "", cookies, "");
                cookies = str.Split('|')[1];
                cookies = garbageRemoving(cookies + ";");*/
              
                //;gpv_pn=tix.axs.com%3Acheckout%3Acaptcha;gpv_c7=tix.axs.com%3A
            //    cookies = updateCookies(cookies);
          //      cookies = updateCookies(cooki+";");
              //  cookies += "AMCV_B7B972315A1341150A495EFE%40AdobeOrg=870038026%7CMCIDTS%7C18688%7CMCMID%7C02431469850514197525418140895094715957%7CMCOPTOUT-1614588812s%7CNONE%7CvVersion%7C5.0.0; AMCVS_B7B972315A1341150A495EFE%40AdobeOrg=1;gpv_pn=tix.axs.com%3Acheckout%3Acaptcha;gpv_c7=tix.axs.com%3A;s_gnr7="+UnixTimeNow(DateTime.Now)+"-New; s_cc=true;";
                //cookies = updateCookies(cookies);
                dic["s_gnr7"] = Convert.ToString(UnixTimeNow(DateTime.Now));
                cookies = updateCookies();
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
             //   cookies = temp;
                dic.Remove("__cfduid");
                dic.Remove("__cf_bm");
                dic["s_gnr7"] = Convert.ToString(UnixTimeNow(DateTime.Now));
                //;gpv_pn=tix.axs.com%3Acheckout%3Acaptcha;gpv_c7=tix.axs.com%3A
                dic.Add("gpv_pn", "tix.axs.com%3Acheckout%3Acaptcha");
                dic.Add("gpv_c7", "tix.axs.com%3A");
                cookies = updateCookies();
                temp = cookies;
                postData = "{\"locale\":\"en-US\",\"meta\":{\"version\":\"" + version.Replace(".js", "") + "\"},\"recaptchaToken\":\"" + Token + "\",\"queueItUrl\":\"" + queueItUrl + "\"}";

                cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/session/v2/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F", "", temp, FanSightTab, "", "POST", Token, "application/json", postData);
                dic.Remove("gpv_pn");
                dic.Remove("gpv_c7");
                if (cookies.Contains("needToSolveRecaptcha"))
                {
                   
                    cookies = garbageRemoving(cookies.Split('|')[0]);
                    Token = Distill.ChecknSolveCaptcha("https://tix.axs.com");
                    dic["s_gnr7"] = Convert.ToString(UnixTimeNow(DateTime.Now));
                    cookies = updateCookies(cookies);
                   // dic.Remove("gpv_pn");
                    //dic.Remove("gpv_c7");
                
                    temp = cookies;
                    do
                    {
                       // Token = Distill.ChecknSolveCaptcha("https://tix.axs.com");

                        postData = "{\"token\":\"" + Token + "\"}";
                        cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/recaptcha-verification/v1/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F", "", temp, FanSightTab, "", "POST", Token, "application/json", postData);
                      /*  if (jsonString.Contains("false")) 
                          Token = Distill.ChecknSolveCaptcha("https://tix.axs.com");*/
                           
                        
                    } while (jsonString.Contains("false"));
                
                }
                //   cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/session/v2/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F", "", cookies, FanSightTab, "", "POST", Token, "application/json", postData);
          /*      do{
                    postData = "{\"locale\":\"en-US\",\"meta\":{\"version\":\"" + version.Replace(".js", "") + "\"},\"recaptchaToken\":\"" + Token + "\",\"queueItUrl\":\"" + queueItUrl + "\"}";
                    cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/session/v2/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F", "", temp, FanSightTab, "", "POST", Token, "application/json", postData);
                    if (cookies.Contains("needToSolveRecaptcha")) {
                     Token = Distill.ChecknSolveCaptcha("https://tix.axs.com");
                    }
                } while (cookies.Contains("needToSolveRecaptcha") || cookies.Contains("Unauthorized"));*/
                /*   //   do
               // {

                if (cookies.Split('|')[1].Equals("needToSolveRecaptcha"))
                {
                    postData = "{\"Token\":\"" + Token + "\"}";
                    cookies = garbageRemoving(cookies.Split('|')[0]);
                  //  temp = cookies.Replace("|", "");
                    cookies = updateCookies(cookies.Split('|')[0]);
                    temp = cookies;
                    do
                    {
                        cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/recaptcha-verification/v1/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F", "", temp, FanSightTab, "", "POST", Token, "application/json", postData);
                    } while (cookies.Contains("Missing Recaptcha"));
                    } //    Token= Distill.ChecknSolveCaptcha("https://tix.axs.com");
               // } while (cookies.Equals("needToSolveRecaptcha"));*/
             //   dic.Add(" gpv_pn", "tix.axs.com%3Acheckout%3Acaptcha");
               // dic.Add(" gpv_c7", "tix.axs.com%3A");
                cookies = garbageRemoving(cookies.Split('|')[0]);
                cookies= cookies.Replace("|", "");
                dic["s_gnr7"] = Convert.ToString(UnixTimeNow(DateTime.Now));
                cookies = updateCookies(cookies);
                cookies = cookies.Replace("|", "");
               
                cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/onsale/v2/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F?sessionID=", "", cookies, FanSightTab, webencode);

                var gpv_c7 = System.Net.WebUtility.UrlEncode("tix.axs.com:select tickets");
                gpv_c7 = gpv_c7.Replace("+", "%20");
                reg=new Regex("\"eventID\":\\d*");
                m=reg.Match(jsonString);
                string eventID = Convert.ToString(m).Split(':')[1];
                reg = new Regex("\"contextID\":\\d*");
                m = reg.Match(jsonString);

                string contextID = Convert.ToString(m).Split(':')[1];
                var gpv_pn = System.Net.WebUtility.UrlEncode("tix.axs.com:selection:select tickets:"+contextID+":"+eventID);
                gpv_pn = gpv_pn.Replace("+", "%20");
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
                dic.Add("gpv_c7", gpv_c7);
                dic.Add("gpv_pn", gpv_pn);
                dic["s_gnr7"] = Convert.ToString(UnixTimeNow(DateTime.Now));
                cookies = updateCookies();
                postData = "{\"selections\":[{\"offerID\":\"" + Convert.ToString(obj["offerId"]) + "\",\"offerGroupID\":\""+Convert.ToString(obj["offerGroupID"])+"\",\"productID\":\""+Convert.ToString(obj["productID"])+"\",\"searches\":[{\"quantity\":"+1+",\"sectionID\":null,\"priceTypeID\":\""+Convert.ToString(obj["priceTypeID"])+"\",\"reqR\":[-1,-1]}],\"optionalFeeId\":null}],\"nextPage\":\"shop__delivery-method-page\",\"overwrite\":true}";
               cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/cart/v2/add-items?onsaleID=2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F&locale=en-US&checkPriceChange=true", "", cookies, FanSightTab, "", "POST", "", "application/json", postData);


            /*   cookies = garbageRemoving(str.Split('|')[0]);
               dic["s_gnr7"] = Convert.ToString(UnixTimeNow(DateTime.Now));
               cookies = updateCookies(cookies);
               temp = cookies;
               postData = "{\"locale\":\"en-US\",\"meta\":{\"version\":\"" + version.Replace(".js", "") + "\"},\"recaptchaToken\":\"" + Token + "\",\"queueItUrl\":\"" + queueItUrl + "\"}";
               cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/session/v2/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F", "", temp, FanSightTab, "", "POST", Token, "application/json", postData);
               dic.Remove("gpv_pn");
               dic.Remove("gpv_c7");
               if (cookies.Contains("needToSolveRecaptcha"))
               {

                   cookies = garbageRemoving(cookies.Split('|')[0]);

                   cookies = updateCookies(cookies.Split('|')[0]);
                   // dic.Remove("gpv_pn");
                   //dic.Remove("gpv_c7");

                   temp = cookies;
                   do
                   {
                       postData = "{\"token\":\"" + Token + "\"}";
                       cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/recaptcha-verification/v1/2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F", "", temp, FanSightTab, "", "POST", Token, "application/json", postData);
                       if (jsonString.Contains("false"))
                       {
                           Token = Distill.ChecknSolveCaptcha("https://tix.axs.com");
                       }
                   } while (jsonString.Contains("false"));

               }
               dic.Add("gpv_c7", gpv_c7);
               dic.Add("gpv_pn", gpv_pn);
               cookies = updateCookies();
               postData = "{\"selections\":[{\"offerID\":\"" + Convert.ToString(obj["offerId"]) + "\",\"offerGroupID\":\"" + Convert.ToString(obj["offerGroupID"]) + "\",\"productID\":\"" + Convert.ToString(obj["productID"]) + "\",\"searches\":[{\"quantity\":" + 1 + ",\"sectionID\":null,\"priceTypeID\":\"" + Convert.ToString(obj["priceTypeID"]) + "\",\"reqR\":[-1,-1]}],\"optionalFeeId\":null}],\"nextPage\":\"shop__delivery-method-page\",\"overwrite\":true}";
               cookies = makeWebRequest("https://unifiedapicommerce.us-prod0.axs.com/veritix/cart/v2/add-items?onsaleID=2L07CQAAAACSWLmzMgAAAADl%2Fv%2F%2F%2FwD%2F%2F%2F%2F%2FBXRoZW1lAP%2F%2F%2F%2F%2F%2F%2F%2F%2F%2F&locale=en-US&checkPriceChange=true", "", cookies, FanSightTab, "", "POST", "", "application/json", postData);*/
           //    jsonString="{"results":[{"offerID":"850639192","productID":"850639106","offerGroupID":"850639193","success":true}],"cart":{"cartID":1008480,"expiration":1614756113647,"subTotal":6750,"feeTotal":1555,"donationTotal":0,"grandTotal":8305,"selections":[{"category":"ADMISSIONS","isUpsell":false,"offerType":"STANDARD","offerID":"850639192","offerGroupID":"850639193","productID":"850639106","eventID":"2796","types":[{"priceTypeID":"45068","priceLevelID":"3964","label":"Regular 2","unitPrice":6750,"quantity":1,"priceTotal":6750,"items":[{"sectionID":"21","sectionLabel":"GAFLOOR","rowID":"21","rowLabel":"GA","seatID":"432","seatLabel":"GA","info1":"","info2":"","deliveryMethodId":0,"assignedCustomerId":0,"neighborhoodPrintDescription":"Admissions","sectionPrintDescription":"GA FLOOR"}],"preTaxUnitPrice":6750,"taxes":[],"fullPriceCodeInfo":{"unitPrice":null,"label":null}}],"quantity":1,"priceTotal":6750,"deliveryMethodID":"3621"}],"fees":[{"id":"850936929","label":"Convenience Fee","unitPrice":1555,"quantity":1,"total":1555,"taxes":[],"preTaxUnitPrice":0}],"taxes":[],"paymentMethods":[{"id":"101","code":"VI","token":null,"type":"CreditCard"},{"id":"102","code":"MC","token":null,"type":"CreditCard"},{"id":"100","code":"AX","token":null,"type":"CreditCard"},{"id":"103","code":"DI","token":null,"type":"CreditCard"},{"id":"2006","code":"PayPal","token":"eyJ2ZXJzaW9uIjoyLCJhdXRob3JpemF0aW9uRmluZ2VycHJpbnQiOiJleUowZVhBaU9pSktWMVFpTENKaGJHY2lPaUpGVXpJMU5pSXNJbXRwWkNJNklqSXdNVGd3TkRJMk1UWXRjSEp2WkhWamRHbHZiaUlzSW1semN5STZJbWgwZEhCek9pOHZZWEJwTG1KeVlXbHVkSEpsWldkaGRHVjNZWGt1WTI5dEluMC5leUpsZUhBaU9qRTJNVFE0TkRJeU1UUXNJbXAwYVNJNklqRTFPR0V5TWpoaExUUXdOVFF0TkRKbFppMWhOekV3TFdabU5qVXpOamt5TVRBMVpDSXNJbk4xWWlJNkltWmtlalI0TmpRMmRtTm9lblJxTW5RaUxDSnBjM01pT2lKb2RIUndjem92TDJGd2FTNWljbUZwYm5SeVpXVm5ZWFJsZDJGNUxtTnZiU0lzSW0xbGNtTm9ZVzUwSWpwN0luQjFZbXhwWTE5cFpDSTZJbVprZWpSNE5qUTJkbU5vZW5ScU1uUWlMQ0oyWlhKcFpubGZZMkZ5WkY5aWVWOWtaV1poZFd4MElqcG1ZV3h6Wlgwc0luSnBaMmgwY3lJNld5SnRZVzVoWjJWZmRtRjFiSFFpWFN3aWMyTnZjR1VpT2xzaVFuSmhhVzUwY21WbE9sWmhkV3gwSWwwc0ltOXdkR2x2Ym5NaU9udDlmUS44bmtJbnByR2ZKZDlBNkZGbnQyVzJGdTEzanlTYlRyZzhXOGZEdTUxQndSOUtqSjVNSHV0Tk1xdGZDd2E1bXl3YlJ0eVNreHBydW52aUVtblVkaWtnZyIsImNvbmZpZ1VybCI6Imh0dHBzOi8vYXBpLmJyYWludHJlZWdhdGV3YXkuY29tOjQ0My9tZXJjaGFudHMvZmR6NHg2NDZ2Y2h6dGoydC9jbGllbnRfYXBpL3YxL2NvbmZpZ3VyYXRpb24iLCJncmFwaFFMIjp7InVybCI6Imh0dHBzOi8vcGF5bWVudHMuYnJhaW50cmVlLWFwaS5jb20vZ3JhcGhxbCIsImRhdGUiOiIyMDE4LTA1LTA4IiwiZmVhdHVyZXMiOlsidG9rZW5pemVfY3JlZGl0X2NhcmRzIl19LCJjbGllbnRBcGlVcmwiOiJodHRwczovL2FwaS5icmFpbnRyZWVnYXRld2F5LmNvbTo0NDMvbWVyY2hhbnRzL2ZkejR4NjQ2dmNoenRqMnQvY2xpZW50X2FwaSIsImVudmlyb25tZW50IjoicHJvZHVjdGlvbiIsIm1lcmNoYW50SWQiOiJmZHo0eDY0NnZjaHp0ajJ0IiwiYXNzZXRzVXJsIjoiaHR0cHM6Ly9hc3NldHMuYnJhaW50cmVlZ2F0ZXdheS5jb20iLCJhdXRoVXJsIjoiaHR0cHM6Ly9hdXRoLnZlbm1vLmNvbSIsInZlbm1vIjoib2ZmIiwiY2hhbGxlbmdlcyI6W10sInRocmVlRFNlY3VyZUVuYWJsZWQiOmZhbHNlLCJhbmFseXRpY3MiOnsidXJsIjoiaHR0cHM6Ly9jbGllbnQtYW5hbHl0aWNzLmJyYWludHJlZWdhdGV3YXkuY29tL2ZkejR4NjQ2dmNoenRqMnQifSwicGF5cGFsRW5hYmxlZCI6dHJ1ZSwicGF5cGFsIjp7ImJpbGxpbmdBZ3JlZW1lbnRzRW5hYmxlZCI6dHJ1ZSwiZW52aXJvbm1lbnROb05ldHdvcmsiOmZhbHNlLCJ1bnZldHRlZE1lcmNoYW50IjpmYWxzZSwiYWxsb3dIdHRwIjpmYWxzZSwiZGlzcGxheU5hbWUiOiJBWFMgR3JvdXAgTExDIiwiY2xpZW50SWQiOiJBVFVXYkVwOFNWR0hzdGFrZ3g5UERjY25vYjJ5eHNvSkdCM3lGM2RMWWZGVGduc0N4Q1psUFVxV2JESGlSQmV0V195Ql9EQUNQZzhYSFFqNyIsInByaXZhY3lVcmwiOiJodHRwczovL3d3dy5heHMuY29tL2Fib3V0LXByaXZhY3ktcG9saWN5X1VTX3YyLmh0bWwiLCJ1c2VyQWdyZWVtZW50VXJsIjoiaHR0cHM6Ly93d3cuYXhzLmNvbS9hYm91dC10ZXJtcy1vZi11c2VfVVNfdjEuaHRtbCIsImJhc2VVcmwiOiJodHRwczovL2Fzc2V0cy5icmFpbnRyZWVnYXRld2F5LmNvbSIsImFzc2V0c1VybCI6Imh0dHBzOi8vY2hlY2tvdXQucGF5cGFsLmNvbSIsImRpcmVjdEJhc2VVcmwiOm51bGwsImVudmlyb25tZW50IjoibGl2ZSIsImJyYWludHJlZUNsaWVudElkIjoiQVJLcllSRGgzQUdYRHpXN3NPXzNiU2txLVUxQzdIR191V05DLXo1N0xqWVNETlVPU2FPdElhOXE2VnBXIiwibWVyY2hhbnRBY2NvdW50SWQiOiJwcF9heHNfZGVmYXVsdCIsImN1cnJlbmN5SXNvQ29kZSI6IlVTRCJ9fQ==","type":"PayPal"}],"salesTaxTotal":null,"taxCompProfiles":[],"deliveryMethods":{"850639192":[{"id":"3621","description":"AXS Mobile ID - Free (Recommended)","process":"FlashSeats","specialInstructions":"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\"><p>\n\t<strong>AXS Mobile ID</strong><br />\n\tShow your AXS Mobile ID in the AXS app then get it scanned to enter. Your AXS Mobile ID is the secure and unique code for all of your tickets, and all you need is the AXS app to use it. No paper tickets required.<br />\n\t&nbsp;<br />\n\tTo use your tickets:<br />\n\t1 &ndash; Download the AXS Mobile App <strong>(</strong><a href=\"https://itunes.apple.com/us/app/axs/id679805463\" target=\"_blank\"><strong>iOS&nbsp;</strong></a><strong>&nbsp;or&nbsp;</strong><a href=\"https://play.google.com/store/apps/details?id=com.axs.android&amp;hl=en\" target=\"_blank\"><strong>Android</strong></a><strong>).</strong><br />\n\t2 &ndash; Open the app and sign in to view your AXS Mobile ID and ticket info.<br />\n\t3 &ndash; Show your AXS Mobile ID in the app at the door to scan and enter.<br />\n\t<br />\n\tBuying tickets for a group? Make sure everyone enters together or, for events with transfer enabled,&nbsp;use the app to transfer tickets to everyone&nbsp;before the event.</p>\n<p>\n\t<strong>NOTE: Ticket delivery or transfer may be delayed or disabled for some events.</strong></p>\n"},{"id":"7242","description":"Standard Mail - $15.00","process":"StandardShipping","specialInstructions":"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\"><p>\n\t<strong>STANDARD MAIL: </strong>On the next business day after your purchase, your tickets will be sent to you via USPS (United States Postal Service) and will arrive in 5-10 business days. For assistance please visit <a href=\"http://support.axs.com/\">http://support.axs.com</a>.</p>\n"},{"id":"3624","description":"Will Call - $6.00","process":"WillCall","specialInstructions":"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\"><p>\n\t<span><strong><span><strong><span><strong><span><strong><span><strong>WILL CALL: </strong></span></strong></span></strong></span></strong></span></strong><span><span><span><span>Please present a valid photo ID, confirmation number/email, and the credit card used for purchase. For assistance please visit <a href=\"http://support.axs.com\">http://support.axs.com</a>.</span></span></span></span></span></p>\n<p>\n\t&nbsp;</p>\n"}]},"isEMV":false,"donations":[]},"message":"New cart successfully created.","status":{"status":201,"message":"Created"},"sessionExpireAt":1614757613647}";
               JObject obj1 = JObject.Parse(jsonString);
               
               JArray cart = JArray.Parse(obj1["cart"].ToString());
               JObject obj2 = JObject.Parse(obj1["cart"].ToString());
               if (obj2.Property("selections") != null)
               {
                   JArray sel = JArray.Parse(obj2["selections"].ToString());
                   Console.WriteLine(sel[0]);
               }

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
                req.KeepAlive = true;
                if (web.Contains("veritix"))
                {
                    req.Accept = "application/json";
                }
                else
                {
                    req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                }
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:72.0) Gecko/20100101 Firefox/72.0";
                req.Headers["Cookie"] = cookies;
               // req.Headers["DNT"] = "1";

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
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:72.0) Gecko/20100101 Firefox/72.0";
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
 static public long UnixTimeNow(DateTime datetime)
    {
        TimeSpan _TimeSpan = (datetime - new DateTime(1970, 1, 1, 0, 0, 0));
        return (long)_TimeSpan.TotalMilliseconds;
    }
    }
    class getHeaders
    {

        public string PID { get; set; }
        public string ajaxHeader { get; set; }
    }

}
