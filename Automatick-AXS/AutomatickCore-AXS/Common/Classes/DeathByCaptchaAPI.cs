using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using DeathByCaptcha;
using System.Threading.Tasks;
using System.Collections;


namespace Automatick.Core
{
    public class DeathByCaptchaAPI : IAutoCaptchaService
    {
        AutoCaptchaServices _autoCaptchaServices;
        Captcha _captcha = null;
        DeathByCaptchaClient DBC = null;
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

        public DeathByCaptchaAPI(AutoCaptchaServices autoCaptchaServices)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this.CaptchaError = "";
            this._wait = new AutoResetEvent(false);
        }

        public DeathByCaptchaAPI(AutoCaptchaServices autoCaptchaServices, Captcha captcha)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = captcha;
            this.CaptchaError = "";
            this._wait = new AutoResetEvent(false);

            if (!String.IsNullOrEmpty(this._autoCaptchaServices.DBCUserName) && !String.IsNullOrEmpty(this._autoCaptchaServices.DBCPassword))
            {
                DeathByCaptchaClient.login(this._autoCaptchaServices.DBCUserName, this._autoCaptchaServices.DBCPassword);
            }
        }

        void solveThreadHandler()
        {
            try
            {
                this.CaptchaError = "";
                deathByCaptchaResult = DeathByCaptchaClient.Decode(this._captcha.CaptchesBytes, this._captcha.Question);// deathByCaptchaClient.Decode(this._captcha.CaptchesBytes);

                if (deathByCaptchaResult.Solved)
                {
                    this._captcha.CaptchaWords = deathByCaptchaResult.Text;
                }
            }
            catch (System.Exception)
            {
                // DeathByCaptchaClient.client = (Client)new SocketClient(this._autoCaptchaServices.DBCUserName.Trim(), this._autoCaptchaServices.DBCPassword.Trim());
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

                if (this._wait == null)
                {
                    new AutoResetEvent(false);
                }

                this._wait.WaitOne();

                result = true;
            }
            catch (System.Exception ex)
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
                DeathByCaptchaClient.Report(deathByCaptchaResult);
                result = true;
            }
            catch (System.Exception)
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
            catch (System.Exception)
            {

            }
        }
    }

    public class DeathByCaptchaClient
    {
        public static Client client = null;

        public static void login(string DBCUserName, string DBCPassword)
        {
            Task.Run(() =>
            {
                try
                {
                    bool isloginfail = false;
                    int retries = 0;

                    do
                    {
                        try
                        {
                            DeathByCaptchaClient.client = (Client)new SocketClient(DBCUserName.Trim(), DBCPassword.Trim());

                            if (DeathByCaptchaClient.client.Balance < 1)
                            {
                                isloginfail = true;
                            }
                        }
                        catch
                        {
                            isloginfail = true;
                        }

                        Thread.Sleep(300);
                        retries++;
                    }
                    while (isloginfail && retries < 2);

                }
                catch
                {
                }
            });
        }

        public static DeathByCaptcha.Captcha Decode(object o, string banner_text)
        {
            byte[] CaptchesBytes = (byte[])o;

            // Put your CAPTCHA image file name, file object, stream or vector
            // of bytes here:
            // DeathByCaptcha.Captcha captcha = client.Upload(CaptchesBytes);//(captchaFileName);

            DeathByCaptcha.Captcha captcha = client.Decode(CaptchesBytes, Client.DefaultTimeout,
                  new Hashtable(){
                    { "type", 3 },
                    {"banner_text", "Select all images with "+ banner_text}                    
                });

            captcha.Text = captcha.Text.Replace("[", "").Replace("]", "");

            if (null != captcha)
            {
                // Poll for the CAPTCHA status until it's solved.
                // Wait at least a few seconds between poll or you'll get
                // banned as abuser.
                while (captcha.Uploaded && !captcha.Solved)
                {
                    System.Threading.Thread.Sleep(Client.DefaultPollInterval * 1000);
                    captcha = client.GetCaptcha(captcha.Id);
                }

                if (captcha.Solved)
                {
                    Console.WriteLine("CAPTCHA solved: {0}",
                                       captcha.Text);

                    // Report an incorrectly solved CAPTCHA.  Make sure the
                    // CAPTCHA was in fact incorrectly solved, do not just
                    // report them all or at random, or you might be banned
                    // as abuser.
                }
                else
                {
                    Console.WriteLine("CAPTCHA was not solved");
                }
            }
            else
            {
                Console.WriteLine("CAPTCHA was not uploaded");
            }

            return captcha;
        }

        public static void Report(DeathByCaptcha.Captcha captcha)
        {
            if (true /* put your CAPTCHA correctness check here */)
            {
                if (client.Report(captcha))
                {
                    //Console.WriteLine("CAPTCHA {0} reported as incorrectly solved",
                    //                  captchaFileName);
                }
                else
                {
                    Console.WriteLine("Failed reporting as incorrectly solved");
                }
            }
        }
    }
}