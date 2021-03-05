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

namespace Automatick.Core
{
    public class CPTCaptchaService : IAutoCaptchaService
    {
        AutoCaptchaServices _autoCaptchaServices;
        Captcha _captcha = null;
        String CPTLastImageID = "";
        AutoResetEvent _wait = null;
        public String CaptchaError
        {
            get;
            set;
        }
        Boolean ifRecap = false;

        public AutoCaptchaServices AutoCaptchaServices
        {
            get
            {
                return this._autoCaptchaServices;
            }
            set
            {
                this._autoCaptchaServices = value;
            }
        }

        public CPTCaptchaService(AutoCaptchaServices autoCaptchaServices, Captcha captcha)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = captcha;
            this.CaptchaError = "";
            this._wait = new AutoResetEvent(false);

            if (!String.IsNullOrEmpty(this._autoCaptchaServices.CPTUserName) && !String.IsNullOrEmpty(this._autoCaptchaServices.CPTPassword))
            {
                if (String.IsNullOrEmpty(this._autoCaptchaServices.NewCPTUserName) || String.IsNullOrEmpty(this._autoCaptchaServices.NewCPTPassword))
                {
                    this._autoCaptchaServices.retrieveNewCPTUserNameAndPasswordThread(this._autoCaptchaServices);
                }
            }
        }

        public CPTCaptchaService(AutoCaptchaServices autoCaptchaServices, Captcha captcha, Boolean ifRecap)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = captcha;
            this.CaptchaError = "";
            this.ifRecap = ifRecap;
            this._wait = new AutoResetEvent(false);

            if (!String.IsNullOrEmpty(this._autoCaptchaServices.CPTUserName) && !String.IsNullOrEmpty(this._autoCaptchaServices.CPTPassword))
            {
                if (String.IsNullOrEmpty(this._autoCaptchaServices.NewCPTUserName) || String.IsNullOrEmpty(this._autoCaptchaServices.NewCPTPassword))
                {                    
                    this._autoCaptchaServices.retrieveNewCPTUserNameAndPasswordThread(this._autoCaptchaServices);
                }
            }  
        }

        public CPTCaptchaService(AutoCaptchaServices autoCaptchaServices)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = null;
            this.CaptchaError = "";
            this._wait = new AutoResetEvent(false);
        }


        void solveThreadHandler()
        {
            try
            {
                this.CaptchaError = "";
                String captchaResult = "";

                if (!this.ifRecap)
                {
                    captchaResult = this.PostCaptcha(this._autoCaptchaServices.NewCPTUserName, this._autoCaptchaServices.NewCPTPassword, this._captcha.CaptchesBytes); 
                }
                else
                {
                    captchaResult = this.PostRecaptchaV2Captcha(this._autoCaptchaServices.NewCPTUserName, this._autoCaptchaServices.NewCPTPassword, this._captcha.CaptchesBytes); 
                }

                if (!String.IsNullOrEmpty(captchaResult))
                {
                    string[] splitted = captchaResult.Split('|');
                    if (splitted.Length >= 2)
                    {
                        this._captcha.CaptchaWords = splitted[1];
                        CPTLastImageID = splitted[0];
                    }
                    else
                    {
                        this.CaptchaError = captchaResult;
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                try
                {
                    this._wait.Set();
                }
                catch { }
            }
        }

        public Boolean solve()
        {
            Boolean result = false;
            try
            {
                Thread th = new Thread(new ThreadStart(this.solveThreadHandler));
                th.Priority = ThreadPriority.Normal;
                th.IsBackground = true;
                th.SetApartmentState(ApartmentState.STA);
                th.Start();

                this._wait.WaitOne();

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                try
                {
                    this._wait.Close();
                    GC.SuppressFinalize(this._wait);
                    this._wait = null;
                }
                catch { }
            }
            return result;
        }

        public Boolean reportBadCaptcha()
        {
            Boolean result = false;
            try
            {
                if (!String.IsNullOrEmpty(CPTLastImageID))
                {
                    string res = this.SetBadImage(this._autoCaptchaServices.NewCPTUserName, this._autoCaptchaServices.NewCPTPassword, CPTLastImageID);
                }

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        #region RDCaptcha members

        #region RDCaptcha methods

        public string GetBalance(string username, string password)
        {
            WebPostRequest myPost = new WebPostRequest("http://captchatypers.com/Forms/RequestBalance.ashx");
            myPost.Add("action", "REQUESTBALANCE");
            myPost.Add("username", username);
            myPost.Add("password", password);
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

        public string PostCaptcha(string username, string password, string file)
        {
            WebPostRequest myPost = new WebPostRequest("http://captchatypers.com/Forms/FileUploadAndGetTextCD.ashx");
            myPost.Add("action", "UPLOADCAPTCHA");
            myPost.Add("username", username);
            myPost.Add("password", password);
            myPost.Add("file", myPost.ImageToBase64(file));
            return myPost.GetResponse();
        }

        public string PostCaptcha(string username, string password, byte[] captchaImage)
        {
            WebPostRequest myPost = new WebPostRequest("http://captchatypers.com/Forms/FileUploadAndGetTextCD.ashx");
            myPost.Add("action", "UPLOADCAPTCHA");
            myPost.Add("username", username);
            myPost.Add("password", password);
            myPost.Add("file", Convert.ToBase64String(captchaImage));
            return myPost.GetResponse();
        }

        public string PostRecaptchaV2Captcha(string username, string password, byte[] captchaImage)
        {
            WebPostRequest myPost = new WebPostRequest("http://captchatypers.com/forms/UploadGoogleCaptcha.ashx");
            myPost.Add("action", "UPLOADCAPTCHA");
            myPost.Add("username", username);
            myPost.Add("password", password);
            myPost.Add("file", Convert.ToBase64String(captchaImage));
            return myPost.GetResponse();
        }
        
        public string PostUsernameAndPasswordCPT()
        {
            String result = String.Empty;
            try
            {
                WebPostRequest myPost = new WebPostRequest("http://captchaexperts.com/CheckLogin.ashx");
                myPost.Add("username", this._autoCaptchaServices.CPTUserName);
                myPost.Add("password", this._autoCaptchaServices.CPTPassword);
                result = myPost.GetResponse();
            }
            catch (Exception)
            {
                WebPostRequest myPost = new WebPostRequest("http://50.31.14.55/CheckLogin.ashx");
                myPost.Add("username", this._autoCaptchaServices.CPTUserName);
                myPost.Add("password", this._autoCaptchaServices.CPTPassword);
                result = myPost.GetResponse();
            }

            return result;
        }
        #endregion

        #region RD CPT captcha web request class
        private class WebPostRequest
        {
            WebRequest theRequest;
            HttpWebResponse theResponse;
            ArrayList theQueryData;

            public WebPostRequest(string url)
            {
                theRequest = WebRequest.Create(url);
                theRequest.Method = "POST";
                theQueryData = new ArrayList();
            }

            public void Add(string key, string value)
            {
                theQueryData.Add(String.Format("{0}={1}", key, HttpUtility.UrlEncode(value)));
            }


            public string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Convert Image to byte[]
                    image.Save(ms, format);
                    byte[] imageBytes = ms.ToArray();

                    // Convert byte[] to Base64 String                 
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }

            public string ImageToBase64(string file)
            {
                return Convert.ToBase64String(File.ReadAllBytes(file));
            }

            public Image Base64ToImage(string base64String)
            {
                // Convert Base64 String to byte[]
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0,
                  imageBytes.Length);

                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                Image image = Image.FromStream(ms, true);
                return image;
            }
            public string GetResponse()
            {
                // Set the encoding type
                theRequest.ContentType = "application/x-www-form-urlencoded";

                // Build a string containing all the parameters
                string Parameters = String.Join("&", (String[])theQueryData.ToArray(typeof(string)));
                theRequest.ContentLength = Parameters.Length;

                // We write the parameters into the request
                StreamWriter sw = new StreamWriter(theRequest.GetRequestStream());
                sw.Write(Parameters);
                sw.Close();

                // Execute the query
                theResponse = (HttpWebResponse)theRequest.GetResponse();
                StreamReader sr = new StreamReader(theResponse.GetResponseStream());
                return sr.ReadToEnd();
            }

        }
        #endregion

        #endregion


        public void abort()
        {
            try
            {
                this._wait.Set();
            }
            catch (Exception)
            {

            }
        }
    }
}
