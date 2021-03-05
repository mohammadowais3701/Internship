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
using System.ComponentModel;

namespace Automatick.Core
{
    class DeCaptchaService : IAutoCaptchaService
    {
        AutoCaptchaServices _autoCaptchaServices;
        Captcha _captcha = null;
        AutoResetEvent _wait = null;
        String Host = "api.decaptcher.com";
        String fileName = DateTime.Now.Ticks.ToString();
        String MajorID, MinorID;
        private bool ifRecap = false;

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

        public DeCaptchaService(AutoCaptchaServices autoCaptchaServices, Captcha captcha)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = captcha;
            this.CaptchaError = "";
            this._wait = new AutoResetEvent(false);

            if (!String.IsNullOrEmpty(this._autoCaptchaServices.DCUserName) && !String.IsNullOrEmpty(this._autoCaptchaServices.DCPassword) && !String.IsNullOrEmpty(this._autoCaptchaServices.DCPort))
            {
                this._autoCaptchaServices.retrieveDCUserNameAndPasswordThread(this._autoCaptchaServices);
            }
        }

        public DeCaptchaService(AutoCaptchaServices autoCaptchaServices, Captcha captcha, Boolean ifRecap)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = captcha;
            this.CaptchaError = "";
            this.ifRecap = ifRecap;
            this._wait = new AutoResetEvent(false);

            if (!String.IsNullOrEmpty(this._autoCaptchaServices.DCUserName) && !String.IsNullOrEmpty(this._autoCaptchaServices.DCPassword) && !String.IsNullOrEmpty(this._autoCaptchaServices.DCPort))
            {
                this._autoCaptchaServices.retrieveDCUserNameAndPasswordThread(this._autoCaptchaServices);
            }
        }

        public DeCaptchaService(AutoCaptchaServices autoCaptchaServices)
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
                    captchaResult = this.PostCaptcha(this._autoCaptchaServices.DCUserName, this._autoCaptchaServices.DCPassword, this._autoCaptchaServices.DCPort, this._captcha.CaptchesBytes); 
                }
                else
                {
                    captchaResult = this.RePostCaptcha(this._autoCaptchaServices.DCUserName, this._autoCaptchaServices.DCPassword, this._captcha.Question, this._captcha.CaptchesBytes);
                }

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
                uint res = this.GetloadSystem(this._autoCaptchaServices.DCUserName, this._autoCaptchaServices.DCPassword,this._autoCaptchaServices.DCPort);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public Boolean report()
        {
            try
            {
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("function", "picture_bad2");
                postParameters.Add("username", this._autoCaptchaServices.DCUserName);
                postParameters.Add("password", this._autoCaptchaServices.DCPassword);
                postParameters.Add("major_id", MajorID);
                postParameters.Add("minor_id", MinorID);

                // Create request and receive response

                string postURL = "http://poster.de-captcher.com/";


                string userAgent = "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);

                // Process response
                if (webResponse != null)
                {
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    string fullResponse = responseReader.ReadToEnd();
                    webResponse.Close();
                    if (fullResponse.ToLower().Contains("|"))
                    {
                        //-- dont know what's it return

                        return true;


                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        #region DCaptcha methods

        public double GetBalance(string username, string password,string port)
        {
            double balance;
            DecaptcherLib.Decaptcher.Balance(Host, Convert.ToUInt32(port), username, password, out balance);
            return balance;
        }

        public uint GetloadSystem(string username, string password,string port)
        {
            uint load;
            DecaptcherLib.Decaptcher.SystemDecaptcherLoad(Host, Convert.ToUInt32(port), password, password, out load);
            return load;
        }

        public string PostCaptcha(string username, string password,string port ,byte[] captchaImage)
        {
            uint _p_pict_to = 0;
            uint _p_pict_type = 0;
            uint _major_id = 0;
            uint _minor_id = 0;
            uint _port=Convert.ToUInt32(port);
            string answer_captcha;
            //Send captcha to Decaptcher
            int ret=  DecaptcherLib.Decaptcher.RecognizePicture(Host, _port, username, password, captchaImage, out _p_pict_to, out _p_pict_type, out answer_captcha, out _major_id, out _minor_id);
            return answer_captcha;
        }

        public string RePostCaptcha(string username, string password, string question, byte[] CaptchesBytes)
        {
            try
            {
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("function", "picture2");
                postParameters.Add("username", username);
                postParameters.Add("password", password);
                postParameters.Add("text1", question);
                postParameters.Add("pict_type", "82");

                Bitmap captchaImage = getCaptchaImage(CaptchesBytes);

                if (captchaImage.Height > 300)
                {
                    int picCount = 16;
                    byte[][] bytesReal = new byte[picCount][];
                    var imgarray = new System.Drawing.Image[16];
                    var img = getCaptchaImage(CaptchesBytes);

                    int Id = 0;

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            try
                            {
                                Id++;
                                var index = i * 4 + j;
                                imgarray[index] = new Bitmap(150, 150);
                                var graphics = Graphics.FromImage(imgarray[index]);
                                graphics.DrawImage(img, new Rectangle(0, 0, 150, 150), new Rectangle(i * 150, j * 150, 150, 150), GraphicsUnit.Pixel);
                                graphics.Dispose();
                                bytesReal[i] = (byte[])(TypeDescriptor.GetConverter(imgarray[index]).ConvertTo(imgarray[index], typeof(byte[])));
                                postParameters.Add("pict" + Id, new FormUpload.FileParameter(bytesReal[i], "imgvWYl1VrOb" + Id + ".jpeg", "image/jpeg"));
                                //  File.WriteAllBytes(@"" + fileName + index + ".bmp", bytesReal[i]);
                            }
                            catch
                            {

                            }

                        }
                    }
                }
                else
                {
                    int picCount = 9;
                    byte[][] bytesReal = new byte[picCount][];
                    var imgarray = new System.Drawing.Image[9];
                    var img = getCaptchaImage(CaptchesBytes);// Image.FromFile(@"test.jpg");

                    int Id = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Id++;
                            var index = i * 3 + j;
                            imgarray[index] = new Bitmap(100, 100);
                            var graphics = Graphics.FromImage(imgarray[index]);
                            graphics.DrawImage(img, new Rectangle(0, 0, 100, 100), new Rectangle(i * 100, j * 100, 100, 100), GraphicsUnit.Pixel);
                            graphics.Dispose();
                            bytesReal[i] = (byte[])(TypeDescriptor.GetConverter(imgarray[index]).ConvertTo(imgarray[index], typeof(byte[])));
                            postParameters.Add("pict" + Id, new FormUpload.FileParameter(bytesReal[i], "imgvWYl1VrOb" + Id + ".jpeg", "image/jpeg"));
                            //  File.WriteAllBytes(@"" + fileName + index + ".bmp", bytesReal[i]);

                        }
                    }
                }

                // Create request and receive response

                string postURL = "http://poster.de-captcher.com/";
                
                string userAgent = "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.153 Safari/537.36";
                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);
                captchaImage.Dispose();
                // Process response
                if (webResponse != null)
                {
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    string fullResponse = responseReader.ReadToEnd();
                    if (fullResponse.ToLower().Contains("|"))
                    {
                        string[] result = fullResponse.Split('|');
                        string captchaResult;

                        if (result.Count() > 5)
                        {
                            string ResultCode = result[0];
                            MajorID = result[1];
                            MinorID = result[2];
                            string Type = result[3];
                            string Timeout = result[4];
                            string Text = result[5];
                            captchaResult = getresult(Text);
                            return captchaResult;
                        }
                    }
                    webResponse.Close();
                }
                else
                {

                }
                
                return "";
            }
            catch
            {

            }
            return null;
        }

        static string getresult(string num)
        {
            //string num = "100000011";
            num = num.Replace(" ", "").Trim();
            char[] arr = num.Trim().ToCharArray();
            string finalResult = String.Empty; ;
            int count = 0;
            if (arr.Length == 8)
            {
                count = 1;
            }
            foreach (Char item in arr)
            {
                if (item == '1')
                {
                    finalResult += count.ToString() + ",";
                }
                count++;
            }

            finalResult = finalResult.TrimEnd(',');

            return finalResult;
        }

        private void deleteImageFiles(int picCount, string name)
        {

            for (int j = 0; j < picCount; j++)
            {
                try
                {
                    File.Delete(name + (j).ToString() + ".bmp");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message + e.StackTrace);
                }
            }
        }

        public Bitmap getCaptchaImage(byte[] captchaImagebytes)
        {
            Bitmap captchaImage = null;

            try
            {
                using (MemoryStream ms = new MemoryStream(captchaImagebytes))
                {
                    captchaImage = new Bitmap(ms);
                }
            }
            catch
            {
                captchaImage = null;
            }

            return captchaImage;
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
