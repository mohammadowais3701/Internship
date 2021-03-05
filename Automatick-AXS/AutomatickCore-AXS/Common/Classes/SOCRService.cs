using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace Automatick.Core
{
    class SOCRService : IAutoCaptchaService
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

        public SOCRService(AutoCaptchaServices autoCaptchaServices, Captcha captcha)
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
               
                string sqlCommand = string.Empty;
                System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

                try
                {
                    clientSocket.Connect(this._autoCaptchaServices.SOCRIP, int.Parse(this._autoCaptchaServices.SOCRPort));
                    NetworkStream serverStream = clientSocket.GetStream();

                    //  while (true)
                    {
                        byte[] inStream = new byte[10025];
                        byte[] outStream = System.Text.Encoding.ASCII.GetBytes((this._autoCaptchaServices.SOCRCaptchaURL + "$"));
                        serverStream.Write(outStream, 0, outStream.Length);
                        //   serverStream.Flush();
                        serverStream.Read(inStream, 0, clientSocket.ReceiveBufferSize);
                        string data = System.Text.Encoding.UTF8.GetString(inStream);
                        this._captcha.CaptchaWords = data.Substring(0, data.IndexOf("$"));
                    }
                }
                catch (Exception ex)
                {
                    clientSocket.Close();
                    // returndata = ex.Message;
                }
            
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
