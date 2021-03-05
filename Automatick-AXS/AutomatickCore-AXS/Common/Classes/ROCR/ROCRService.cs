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
    class ROCRService : IAutoCaptchaService
    {
        AutoCaptchaServices _autoCaptchaServices;
        Captcha _captcha = null;
        AutoResetEvent _wait = null;

        public String CaptchaError
        {
            get;
            set;
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

        public ROCRService(AutoCaptchaServices autoCaptchaServices, Captcha captcha)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = captcha;
            this.CaptchaError = "";
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
            String name = @"\img" + UniqueKey.getUniqueKey() + ".png";
            try
            {
                this.CaptchaError = "";

                //ImgSender sender = new ImgSender(this._autoCaptchaServices.BOLO.IP, Convert.ToInt32(this._autoCaptchaServices.BOLO.Port));
                //this._captcha.CaptchaWords = sender.SendBytes(this._captcha.CaptchesBytes);


                Bitmap bm = new Bitmap(this._captcha.CaptchaImage);
                bm.Save(Application.StartupPath + name, System.Drawing.Imaging.ImageFormat.Png);
                this._captcha.CaptchaWords = this.BreakCoral(name);

                if (this._captcha.CaptchaWords != null)
                {
                    //result = true;
                }
                else
                {
                    // result = false;
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

        public String BreakCoral(string filename)
        {
            // Read file data
            FileStream fs = new FileStream(Application.StartupPath + filename, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();

            postParameters.Add("pict", new FormUpload.FileParameter(data, "imgvWYl1VrObn.png", "image/png"));
            //postParameters.Add("username", "demo_81");
            //postParameters.Add("password", "Ff8bzj16");
            postParameters.Add("username", this.AutoCaptchaServices.ROCRUsername);
            postParameters.Add("password", this.AutoCaptchaServices.ROCRPassword);

            // Create request and receive response
            //string postURL = "http://188.226.138.13:8081/";
            string postURL = "http://" + this.AutoCaptchaServices.ROCRIP + ":" + this.AutoCaptchaServices.ROCRPort + "/";

            string userAgent = "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);

            // Process response
            string fullResponse = String.Empty;
            if (webResponse != null)
            {
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                fullResponse = responseReader.ReadToEnd();
                fullResponse = fullResponse.Replace("0|0|0|0|0|", "");
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

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
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
