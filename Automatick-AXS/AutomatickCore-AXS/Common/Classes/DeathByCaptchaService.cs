using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace Automatick.Core
{
    public class DeathByCaptchaService : IAutoCaptchaService
    {
        AutoCaptchaServices _autoCaptchaServices;
        Captcha _captcha = null;
        DeathByCaptcha.HttpClient deathByCaptchaClient = null;
        DeathByCaptcha.Captcha deathByCaptchaResult = null;
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

        public DeathByCaptchaService(AutoCaptchaServices autoCaptchaServices, Captcha captcha)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = captcha;
            this.CaptchaError = "";
            this._wait = new AutoResetEvent(false);
        }

        void solveThreadHandler()
        {
            try
            {
                this.CaptchaError = "";

                deathByCaptchaClient = new DeathByCaptcha.HttpClient(
                        this._autoCaptchaServices.DBCUserName.Trim(),
                        this._autoCaptchaServices.DBCPassword.Trim());

                deathByCaptchaResult = deathByCaptchaClient.Decode(this._captcha.CaptchesBytes);

                if (deathByCaptchaResult.Solved)
                {
                    this._captcha.CaptchaWords = deathByCaptchaResult.Text;
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
            catch (Exception ex)
            {
                result = false;
                this.CaptchaError = ex.Message;
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
                if (deathByCaptchaClient != null && deathByCaptchaResult != null)
                {
                    if (deathByCaptchaResult.Solved)
                    {                        
                        deathByCaptchaClient.Report(deathByCaptchaResult);
                    }
                }

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
