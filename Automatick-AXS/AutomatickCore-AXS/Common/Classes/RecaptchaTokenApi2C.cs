using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class RecaptchaTokenApi2C
    {
        public String GetRecaptchaToken(string key2C, string captchakey, string host)
        {
            string recaptchaToken = string.Empty;

            try
            {
                BrowserSession session = new BrowserSession();

                String strHtml = session.Get("http://2captcha.com/in.php?key=" + key2C + "&method=userrecaptcha&googlekey=" + captchakey + "&pageurl=" + host);

                if (strHtml.Contains("OK"))
                {
                    string requestId = strHtml.Split('|')[1];

                    recaptchaToken = PollRequest(key2C, requestId, session);

                    return recaptchaToken;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return string.Empty;
        }

        public String GetHCaptchaToken(string key2C, string sitekey, string url, string proxy)
        {
            string recaptchaToken = string.Empty;

            try
            {
                BrowserSession session = new BrowserSession();


                try
                {

                    if (!String.IsNullOrEmpty(proxy))
                    {
                        string[] splitedString = proxy.Split(':');

                        if (splitedString.Count() == 2)
                        {
                            proxy = splitedString[0] + ":" + splitedString[1];
                        }

                        else if (splitedString.Count() == 4)
                        {
                            proxy = splitedString[2] + ":" + splitedString[3] + "@" + splitedString[0] + ":" + splitedString[1];

                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.Source);
                }

                string urlCaptcha = "https://2captcha.com/in.php?key=" + key2C + "&method=hcaptcha&sitekey=" + sitekey + "&pageurl=" + url;

                //if (!string.IsNullOrEmpty(proxy))
                //{
                //    urlCaptcha += "&proxy=" + proxy;
                //}

                String strHtml = session.Get(urlCaptcha);

                if (strHtml.Contains("OK"))
                {
                    string requestId = strHtml.Split('|')[1];

                    recaptchaToken = PollRequest(key2C, requestId, session);

                    return recaptchaToken;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return string.Empty;
        }

        private  String PollRequest(string key2c, string requestKey, BrowserSession session)
        {
            try
            {
                String strHtml = session.Get("http://2captcha.com/res.php?key=" + key2c + "&action=get&id=" + requestKey);

                do
                {
                    Thread.Sleep(1000);

                    strHtml = session.Get("http://2captcha.com/res.php?key=" + key2c + "&action=get&id=" + requestKey);
                  
                    if (strHtml.Contains("OK"))
                    {
                        string recaptchaToken = strHtml.Split('|')[1];
                        return recaptchaToken;

                    }
                }
                while (!strHtml.Contains("OK") && strHtml.Contains("CAPCHA_NOT_READY"));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return string.Empty;
        }
    }
}
