using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Automatick.Core
{
    class BoloCaptchaService : IAutoCaptchaService
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

        public BoloCaptchaService(AutoCaptchaServices autoCaptchaServices, Captcha captcha)
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
                bool answered = false;
                ImageSenderForBolo sender = new ImageSenderForBolo(this._autoCaptchaServices.BOLOIP, Convert.ToInt32(this._autoCaptchaServices.BOLOPORT), null);
                this._captcha.CaptchaWords = sender.getAnswer(this._autoCaptchaServices.BOLOIP, Convert.ToInt32(this._autoCaptchaServices.BOLOPORT), this._captcha.CaptchesBytes, ref answered);
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
