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
using System.IO.Compression;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Automatick.Core
{
    public class ReCaptcha2C : IAutoCaptchaService
    {
        AutoCaptchaServices _autoCaptchaServices;
        String captchaKey;
        String host;
        AutoResetEvent _wait = null;
        String task_id = "-1";
        String RDLastImageID=string.Empty;
        String RecapToken;
        Captcha captcha;

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

        public ReCaptcha2C(AutoCaptchaServices autoCaptchaServices,Captcha captcha, string key, string host)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this.captchaKey = key;
            this.CaptchaError = "";
            this.host = host;
            this.captcha = captcha;
            this._wait = new AutoResetEvent(false);
        }

        public ReCaptcha2C(AutoCaptchaServices autoCaptchaServices, string host)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this.host = host;
            this.CaptchaError = "";
            this._wait = new AutoResetEvent(false);
        }

        public ReCaptcha2C(AutoCaptchaServices autoCaptchaServices)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this.CaptchaError = "";
            this._wait = new AutoResetEvent(false);
        }

        void solveThreadHandler()
        {
            try
            {
                this.CaptchaError = "";
                String captchaResult = "";
                //captchaResult = this.PostCaptcha(this._autoCaptchaServices.C2Key, this._captcha.CaptchesBytes);
                captchaResult = this.PostCaptcha(this._autoCaptchaServices.C2Key, this.captchaKey,this.host);

                if (!String.IsNullOrEmpty(captchaResult))
                {
                    string[] splitted = captchaResult.Split('|');
                    if (splitted.Length >= 2)
                    {
                        this.captcha.CaptchaWords = splitted[1];
                        RDLastImageID = splitted[0];
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

                if (this._wait == null)
                {
                    this._wait = new AutoResetEvent(false);
                }

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

        public Boolean solve(string host)
        {
            this.host = host;
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
                    this._wait = new AutoResetEvent(false);
                }

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

        string PostCaptcha(string key, string captchaKey,string host)
        {
            string responseFromServer = string.Empty;

            try
            {

            HttpWebRequest request = WebRequest.CreateHttp("http://2captcha.com/in.php");
            request.Method = "POST";

            //http://2captcha.com/in.php?key=9d5450a0c631278c266cfbaf904551fd&method=userrecaptcha&googlekey=6LexTBoTAAAAAESv_PtNKgDQM7ZP9KOKedZUbYay&pageurl=tickets.axs.com

            byte[] body = ASCIIEncoding.UTF8.GetBytes("method=base64&key=" + key + "&method=userrecaptcha&googlekey="+captchaKey +"&pageurl="+host);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = body.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(body, 0, body.Length);
            dataStream.Close();

            using (WebResponse response = request.GetResponse())
            {
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Display the content.

                if (responseFromServer.ToLower().Contains("ok"))
                {
                    string captcha_id = responseFromServer.Split('|')[1];

                    string fullResponse = string.Empty;
                    string url = @"http://2captcha.com/res.php?key=" + key + "&action=get&id=" + captcha_id;
                    int retry = 0;

                    do
                    {
                        fullResponse = GetResponse(url);

                        if (fullResponse.ToLower().Contains("ok"))
                        {
                            break;
                        }
                        System.Threading.Thread.Sleep(5000);
                        retry++;

                        if (retry > 60) break;
                    }
                    while (!fullResponse.ToLower().Contains("ok"));

                    string result = string.Empty;

                    if (fullResponse.ToLower().Contains("ok"))
                    {
                        return result = captcha_id + "|" + fullResponse.Split('|')[1];
                    }
                    else
                    {
                        this.CaptchaError = fullResponse;
                        return result;
                    }

                }
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return responseFromServer;
        }

        private string GetResponse(string url)
        {
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.0.4; en-gb; GT-I9300 Build/IMM76D) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en;q=0.5");
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.KeepAlive = true;
                request.ContentType = "application/x-www-form-urlencoded";

                request.Method = "GET";
                request.ServicePoint.Expect100Continue = false;

                //byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                //request.ContentLength = postBytes.Length;
                //Stream stream = request.GetRequestStream();
                //stream.Write(postBytes, 0, postBytes.Length);
                //stream.Close();
                string responsetext;

                return responsetext = getResponse(request);


            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError) response = (HttpWebResponse)e.Response;
                else return null;
            }
            catch (Exception)
            {
                if (response != null) response.Close();
                return null;
            }

            return null;
        }

        protected string getResponse(HttpWebRequest request)
        {
            string strHtml;
            try
            {

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream receiveStream = null;
                    HtmlAgilityPack.HtmlDocument h = new HtmlAgilityPack.HtmlDocument();
                    Encoding respenc = null;
                    var isGZipEncoding = false;
                    if (!string.IsNullOrEmpty(response.ContentEncoding))
                    {
                        if (response.ContentEncoding.ToLower().StartsWith("gzip")) isGZipEncoding = true;

                        if (!isGZipEncoding)
                        {
                            respenc = Encoding.GetEncoding(response.ContentEncoding);
                        }
                    }

                    if (isGZipEncoding)
                    {
                        receiveStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                    }
                    else
                    {
                        receiveStream = response.GetResponseStream();
                    }
                    var reader = new StreamReader(receiveStream);
                    strHtml = reader.ReadToEnd();
                    h.LoadHtml(strHtml);
                    return strHtml;
                }
            }
            catch
            {
                strHtml = string.Empty;
                return null;
            }
        }

        public Boolean reportBadCaptcha()
        {
            Boolean result = false;
            try
            {
                if (!String.IsNullOrEmpty(RDLastImageID))
                {

                    string res = this.SetBadImage(this._autoCaptchaServices.C2Key, RDLastImageID);
                }

                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public string SetBadImage(string key, string captcha_id)
        {
            string url = @"http://2captcha.com/res.php?key=" + key + "&action=reportbad&id=" + captcha_id;
            string fullResponse = GetResponse(url);
            return fullResponse;
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
