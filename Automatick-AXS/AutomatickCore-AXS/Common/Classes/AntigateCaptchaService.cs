using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Automatick.Core
{
    public class AntigateCaptchaService : IAutoCaptchaService
    {
        AutoCaptchaServices _autoCaptchaServices;
        Captcha _captcha = null;
        AutoResetEvent _wait = null;
        WebProxy proxy = null;
        private WebProxy _proxy;
        string id;

        string fullResponse = String.Empty;
        private bool ifRecap = false;

        public string CaptchaError
        {
            get;
            set;
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

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


        public AntigateCaptchaService(AutoCaptchaServices autoCaptchaServices)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this.CaptchaError = "";
            this._wait = new AutoResetEvent(false);
        }

        public AntigateCaptchaService(AutoCaptchaServices autoCaptchaServices, Captcha captcha)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = captcha;
            this.CaptchaError = "";
            this._wait = new AutoResetEvent(false);
        }

        public AntigateCaptchaService(AutoCaptchaServices autoCaptchaServices, Captcha captcha, Boolean ifRecap)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = captcha;
            this.CaptchaError = "";
            this.ifRecap = ifRecap;
            this._wait = new AutoResetEvent(false);
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

        void solveThreadHandler()
        {
            //Boolean result = false;
            String name = String.Empty;
            try
            {
                this.CaptchaError = "";

                if (ifRecap)
                {
                    name = @"\img" + UniqueKey.getUniqueKey() + ".png";
                    Bitmap bm = new Bitmap(this._captcha.CaptchaImage);
                    bm.Save(Application.StartupPath + name, System.Drawing.Imaging.ImageFormat.Jpeg);
                    this._captcha.CaptchaWords = this.PostCaptcha(name, this._captcha.Question);
                }
                else
                {
                    name = @"\img" + UniqueKey.getUniqueKey() + ".png";
                    Bitmap bm = new Bitmap(this._captcha.CaptchaImage);
                    bm.Save(Application.StartupPath + name, System.Drawing.Imaging.ImageFormat.Jpeg);
                    this._captcha.CaptchaWords = this.BreakCoral(name);
                }
            }
            catch (Exception ex)
            {
                //result = false;
                if (String.IsNullOrEmpty(this.CaptchaError))
                    this.CaptchaError = ex.Message;
            }
            finally
            {
                try
                {
                    this._wait.Set();
                }
                catch { }
                if (File.Exists(Application.StartupPath + name))
                {
                    try
                    {
                        File.Delete(Application.StartupPath + name);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        String ID = String.Empty;

        public String BreakCoral(string filename)
        {
            // Read file data

            FileStream fs = new FileStream(Application.StartupPath + filename, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("method", "post");
            postParameters.Add("key", this.AutoCaptchaServices.AntigateKey);
            postParameters.Add("file", new FormUpload.FileParameter(data, "imgvWYl1VrObn.png", "image/png"));

            // Create request and receive response

            string postURL = "http://antigate.com/in.php";

            string userAgent = "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);

            // Process response
            if (webResponse != null)
            {
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                fullResponse = responseReader.ReadToEnd();
                if (fullResponse.ToLower().Contains("ok"))
                {
                    ID = fullResponse.Replace("OK|", "");
                    HtmlAgilityPack.BrowserSession session = new HtmlAgilityPack.BrowserSession();

                    do
                    {
                        fullResponse = session.Get("http://antigate.com/res.php?key=" + this.AutoCaptchaServices.AntigateKey + "&action=get&id=" + ID);
                        fullResponse = fullResponse.Replace("OK|", "");
                        Thread.Sleep(1000);
                    }
                    while (fullResponse.Contains("CAPCHA_NOT_READY"));                    
                }
                else
                {
                    this.CaptchaError = fullResponse;
                    fullResponse = "";
                }

                webResponse.Close();


            }
            else
            {
                this.CaptchaError = FormUpload.GetErrorMessage();
            }


            return fullResponse;
        }

        public String PostCaptcha(string filename, String question)
        {
            // Read file data

            FileStream fs = new FileStream(Application.StartupPath + filename, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("method", "post");
            postParameters.Add("type", "recaptcha2");//	
            postParameters.Add("comment", question);
            postParameters.Add("key", this.AutoCaptchaServices.AntigateKey);

            postParameters.Add("file", new FormUpload.FileParameter(data, "imgvWYl1VrObn.png", "image/png"));

            // Create request and receive response

            string postURL = "http://antigate.com/in.php";

            string userAgent = "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);

            // Process response
            if (webResponse != null)
            {
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                fullResponse = responseReader.ReadToEnd();
                if (fullResponse.ToLower().Contains("ok"))
                {
                    id = fullResponse.Replace("OK|", "");
                    HtmlAgilityPack.BrowserSession session = new HtmlAgilityPack.BrowserSession();

                    do
                    {
                        fullResponse = session.Get("http://antigate.com/res.php?key=" + this.AutoCaptchaServices.AntigateKey + "&action=get&id=" + this.Id);
                        fullResponse = fullResponse.Replace("OK|", "");
                        Thread.Sleep(1000);
                    }
                    while (fullResponse.Contains("CAPCHA_NOT_READY"));

                    fullResponse = changeNumbers(fullResponse);
                }
                else
                {
                    this.CaptchaError = fullResponse;
                    fullResponse = "";
                }

                webResponse.Close();


            }
            else
            {
                this.CaptchaError = FormUpload.GetErrorMessage();
            }


            return fullResponse;
        }

        public bool reportBadCaptcha()
        {
            Boolean result = false;
            try
            {
                HtmlAgilityPack.BrowserSession session = new HtmlAgilityPack.BrowserSession();
                string response = session.Get("http://antigate.com/res.php?key=" + this.AutoCaptchaServices.AntigateKey + "&action=reportbad&id=" + this.Id);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }


        string changeNumbers(string words)
        {
            try
            {
                // words = words.Trim().Replace(",", "").Replace("\\", "");
                string[] array = words.Trim().Split(',');
                string capwords = string.Empty;

                foreach (string c in array)
                {
                    switch (c)
                    {
                        case "1":
                            capwords += "0,";
                            break;
                        case "2":
                            capwords += "1,";
                            break;
                        case "3":
                            capwords += "2,";
                            break;
                        case "4":
                            capwords += "3,";
                            break;
                        case "5":
                            capwords += "4,";
                            break;
                        case "6":
                            capwords += "5,";
                            break;
                        case "7":
                            capwords += "6,";
                            break;
                        case "8":
                            capwords += "7,";
                            break;
                        case "9":
                            capwords += "8,";
                            break;
                        case "10":
                            capwords += "9,";
                            break;
                        case "11":
                            capwords += "10,";
                            break;
                        case "l2":
                            capwords += "11,";
                            break;
                        case "13":
                            capwords += "12,";
                            break;
                        case "14":
                            capwords += "13,";
                            break;
                        case "15":
                            capwords += "14,";
                            break;
                        case "16":
                            capwords += "15,";
                            break;
                        case "a":
                            capwords += "0,";
                            break;
                        case "b":
                            capwords += "1,";
                            break;
                        case "c":
                            capwords += "2,";
                            break;
                        case "d":
                            capwords += "3,";
                            break;
                        case "e":
                            capwords += "4,";
                            break;
                        case "f":
                            capwords += "5,";
                            break;
                        case "g":
                            capwords += "6,";
                            break;
                        case "h":
                            capwords += "7,";
                            break;
                        case "i":
                            capwords += "8,";
                            break;
                        case "j":
                            capwords += "9,";
                            break;
                        case "k":
                            capwords += "10,";
                            break;
                        case "l":
                            capwords += "11,";
                            break;
                        case "m":
                            capwords += "12,";
                            break;
                        case "n":
                            capwords += "13,";
                            break;
                        case "o":
                            capwords += "14,";
                            break;
                        case "p":
                            capwords += "15,";
                            break;
                    }
                }

                return capwords = capwords.TrimEnd(',');

            }
            catch
            {
                return words;
            }
        }

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
