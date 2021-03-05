using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class RecaptchaTokenRDC
    {
        public static string nUsername;
        public static string nPassowrd;

        public string PostReCaptcha(string username, string password, string googleKey, string pageUrl, string proxy, string proxyType)
        {
            WebPostRequest myPost = new WebPostRequest("http://captchatypers.com/CaptchaApi/UploadRecaptchav1.ashx");  //("http://captchatypers.com/CaptchaApi/UploadRecaptcha.ashx");
            myPost.Add("action", "UPLOADCAPTCHA");
            myPost.Add("username", username);
            myPost.Add("password", password);
            myPost.Add("googlekey", googleKey);
            myPost.Add("pageurl", pageUrl);
            myPost.Add("proxy", proxy);
            myPost.Add("proxytype", proxyType);
            return myPost.GetResponse();
        }

        public String PostUsernameAndPasswordRD(string username, string password)
        {
            String result = String.Empty;
            try
            {
                WebPostRequest myPost = new WebPostRequest("http://95.211.166.145/CheckLogin.ashx");
                myPost.Add("username", username);
                myPost.Add("password", password);
                result = myPost.GetResponse();
            }
            catch (Exception)
            {
                WebPostRequest myPost = new WebPostRequest("http://50.31.14.55/CheckLogin.ashx");
                myPost.Add("username", username);
                myPost.Add("password", password);
                result = myPost.GetResponse();
            }

            return result;
        }

        public string GetText(string username, string password, string captchaId)
        {
            WebPostRequest myPost = new WebPostRequest("http://captchatypers.com/CaptchaApi/GetRecaptchaText.ashx");
            myPost.Add("action", "GETTEXT");
            myPost.Add("username", username);
            myPost.Add("password", password);
            myPost.Add("captchaid", captchaId);
            return myPost.GetResponse();
        }

        public string SetBadImage(string username, string password, string imageid)
        {
            WebPostRequest myPost = new WebPostRequest("http://captchatypers.com/Forms/SetBadImage.ashx");
            myPost.Add("action", "SETBADIMAGE");
            myPost.Add("username", username);
            myPost.Add("password", password);
            myPost.Add("imageid", imageid);
            return myPost.GetResponse();
        }


        public String GetRecaptchaToken(string username, string password, string captchakey, string host, string proxy, string proxyType)
        {
            string recaptchaToken = string.Empty;

            try
            {
                if (String.IsNullOrEmpty(nUsername) && String.IsNullOrEmpty(nPassowrd))
                {
                    string result =  PostUsernameAndPasswordRD(username, password);
                    nUsername = result.Split(',')[0]; nPassowrd = result.Split(',')[1];
                }

                String captchaid =  PostReCaptcha(nUsername, nPassowrd, captchakey, host, proxy, proxyType);
                String response = string.Empty;

                do
                {
                    response =  GetText(nUsername, nPassowrd, captchaid);

                    Thread.Sleep(500);
                }
                while (response.ToUpper().Contains("NOT_DECODED"));

                if (response.ToLower().Contains("error"))
                {
                    SetBadImage(nUsername, nPassowrd, captchaid);
                }

                return response;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return string.Empty;
        }
    }
}
