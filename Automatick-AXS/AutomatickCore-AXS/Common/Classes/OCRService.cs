using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace Automatick.Core
{
    class OCRService: IAutoCaptchaService
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

        public OCRService(AutoCaptchaServices autoCaptchaServices, Captcha captcha)
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
            try
            {
                this.CaptchaError = "";

                ImgSender sender = new ImgSender(this._autoCaptchaServices.OCRIP, Convert.ToInt32(this._autoCaptchaServices.OCRPort));
                this._captcha.CaptchaWords = sender.SendBytes(this._captcha.CaptchesBytes);
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
                this.CaptchaError = ex.Message;
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

        public Boolean reportBadCaptcha()
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
