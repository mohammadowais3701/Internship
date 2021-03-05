using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Threading;
using System;

namespace Automatick.Core
{
    class CustomCaptchaService : IAutoCaptchaService
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

        public CustomCaptchaService(AutoCaptchaServices autoCaptchaServices, Captcha captcha)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = captcha;
            this.CaptchaError = "";
            this._wait = new AutoResetEvent(false);

            if (!String.IsNullOrEmpty(this._autoCaptchaServices.CUserName) && !String.IsNullOrEmpty(this._autoCaptchaServices.CPassword) && !String.IsNullOrEmpty(this._autoCaptchaServices.CHost) 
                && !String.IsNullOrEmpty(this._autoCaptchaServices.CPort))
            {
                this._autoCaptchaServices.retrieveCUserNameAndPasswordThread(this._autoCaptchaServices);
            }
        }

        public CustomCaptchaService(AutoCaptchaServices autoCaptchaServices)
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
                captchaResult = this.PostCaptcha(this._autoCaptchaServices.CUserName, this._autoCaptchaServices.CPassword,this._autoCaptchaServices.CHost, this._autoCaptchaServices.CPort, this._captcha.CaptchesBytes);
                if (!String.IsNullOrEmpty(captchaResult))
                {
                    this._captcha.CaptchaWords = captchaResult;
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
                uint res = this.GetloadSystem(this._autoCaptchaServices.CUserName, this._autoCaptchaServices.CPassword, this._autoCaptchaServices.CPort);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        #region DCaptcha methods

        public double GetBalance(string username, string password, string port)
        {
            double balance;
            DecaptcherLib.Decaptcher.Balance(this._autoCaptchaServices.CHost, Convert.ToUInt32(port), username, password, out balance);
            return balance;
        }

        public uint GetloadSystem(string username, string password, string port)
        {
            uint load;
            DecaptcherLib.Decaptcher.SystemDecaptcherLoad(this._autoCaptchaServices.CHost, Convert.ToUInt32(port), username, password, out load);
            return load;
        }

        public string PostCaptcha(string username, string password, string host,string port, byte[] captchaImage)
        {
            uint _p_pict_to = 0;
            uint _p_pict_type = 0;
            uint _major_id = 0;
            uint _minor_id = 0;
            uint _port = Convert.ToUInt32(port);
            string answer_captcha;
            //Send captcha to Decaptcher
            int ret = DecaptcherLib.Decaptcher.RecognizePicture(host, _port, username, password, captchaImage, out _p_pict_to, out _p_pict_type, out answer_captcha, out _major_id, out _minor_id);
            return answer_captcha;
        }

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
