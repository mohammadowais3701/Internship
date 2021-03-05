using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.IO.Compression;

namespace Automatick.Core
{
    public class BypassAutoCaptchaService : IAutoCaptchaService
    {
        AutoCaptchaServices _autoCaptchaServices;
        Captcha _captcha = null;
        String task_id = "-1";
        AutoResetEvent _wait = null;
        Boolean IfRecapV2 = false;

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

        public BypassAutoCaptchaService(AutoCaptchaServices autoCaptchaServices, Captcha captcha, Boolean ifRecap)
        {
            this._autoCaptchaServices = autoCaptchaServices;
            this._captcha = captcha;
            this.CaptchaError = "";
            this.IfRecapV2 = ifRecap;
            this._wait = new AutoResetEvent(false);
        }

        public BypassAutoCaptchaService(AutoCaptchaServices autoCaptchaServices, Captcha captcha)
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
                if (!IfRecapV2)
                {
                    this.CaptchaError = "";
                    task_id = "-1";

                    // base64 encode it
                    string im = Convert.ToBase64String(_captcha.CaptchesBytes);

                    // submit captcha to server
                    string captchaResult = WebPost("http://nanosofttek.0x0023.com/upload.php", new string[] {
                "key", this._autoCaptchaServices.BPCKey.Trim(),
                "file", im,
                "submit", "Submit",
                "gen_task_id", "1",
                "base64_code", "1", 
                "vendor_key", "33bdeffd43e4ba073c27bed0e1e6f17c"});
                    Dictionary<String, String> dict = Split(captchaResult);

                    if (dict.ContainsKey("TaskId"))
                    {
                        task_id = dict["TaskId"];
                    }

                    if (dict.ContainsKey("Value"))
                    {
                        _captcha.CaptchaWords = dict["Value"];
                    }

                    if (!dict.ContainsKey("TaskId") || !dict.ContainsKey("Value"))
                    {
                        this.CaptchaError = captchaResult;
                    }
                }
                else
                {
                    this._captcha.CaptchaWords = this.BPCPost();
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
                //Feedback(false);
                refund();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        #region ByPassCaptcha methods

        private string WebPost(string url, params string[] ps)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ServicePoint.ConnectionLimit = 1000;
                request.Proxy = WebRequest.DefaultWebProxy;
                string str = "";
                for (int i = 0; i + 1 < ps.Length; i += 2)
                {
                    str += UrlEncode(ps[i]) + "=" + UrlEncode(ps[i + 1]) + "&";
                }
                if (str.EndsWith("&"))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] data = Encoding.ASCII.GetBytes(str);
                request.ContentLength = data.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);

                WebResponse response = request.GetResponse();
                Stream sStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(sStream);
                string retContent = reader.ReadToEnd();
                reader.Close();
                response.Close();
                newStream.Close();
                return retContent;
            }
            catch
            {
                return "";
            }
        }

        private string UrlEncode(string str)
        {
            if (str == null) return "";

            Encoding enc = Encoding.ASCII;
            StringBuilder result = new StringBuilder();

            foreach (char symbol in str)
            {
                byte[] bs = enc.GetBytes(new char[] { symbol });
                for (int i = 0; i < bs.Length; i++)
                {
                    byte b = bs[i];
                    if (b >= 48 && b < 58 || b >= 65 && b < 65 + 26 || b >= 97 && b < 97 + 26) // decode non numalphabet
                    {
                        result.Append(Encoding.ASCII.GetString(bs, i, 1));
                    }
                    else
                    {
                        result.Append('%' + String.Format("{0:X2}", (int)b));
                    }
                }
            }

            return result.ToString();
        }

        private Dictionary<string, string> Split(string con)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (con == null) return ret;
            string[] lines = con.Split('\n');
            foreach (string line in lines)
            {
                string x = line.Trim();
                int sidx = x.IndexOf(' ');
                string name = "";
                string value = "";
                if (sidx >= 0)
                {
                    name = x.Substring(0, sidx);
                    value = x.Substring(sidx + 1).Trim();
                }
                else
                {
                    name = x;
                    value = "";
                }
                if (name != "") ret[name] = value;
            }
            return ret;
        }

        private void Feedback(bool is_input_correct)
        {
            String result = WebPost("http://nanosofttek.0x0023.com/check_value.php", new string[] {
                "key", this._autoCaptchaServices.BPCKey.Trim(),
                "task_id", task_id,
                "cv", (is_input_correct ? "1" : "0"),
                "submit", "Submit"});

        }
        public String BPCPost()
        {

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("user", this.AutoCaptchaServices.BPCUserName);
            postParameters.Add("password", this.AutoCaptchaServices.BPCPassword);
            postParameters.Add("vendor", "33bdeffd43e4ba073c27bed0e1e6f17c");
            postParameters.Add("base64_code", "1");
            postParameters.Add("file", Convert.ToBase64String(this._captcha.CaptchesBytes));
            postParameters.Add("what", this._captcha.Question);
            string postURL = @"http://api.bypassrecaptcha.com/submit.php";

            string userAgent = "Mozilla/5.0 (Linux; U; Android 4.0.4; en-gb; GT-I9300 Build/IMM76D) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);

            string fullResponse = String.Empty;
            if (webResponse != null)
            {
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                fullResponse = responseReader.ReadToEnd();
                if (fullResponse.Contains("TaskId"))
                {
                    task_id = fullResponse.Replace("TaskId", "").Trim();
                    webResponse.Close();

                    string body = @"user=" + this.AutoCaptchaServices.BPCUserName + "&password=" + this.AutoCaptchaServices.BPCPassword + "&task_id=" + task_id;
                    string url = @"http://api.bypassrecaptcha.com/get_answer.php";
                    int retry = 0;

                    do
                    {
                        fullResponse = GetResponse(url, body);
                        System.Threading.Thread.Sleep(5000);
                        retry++;

                        if (fullResponse.ToLower().Contains("answer"))
                        {
                            break;
                        }

                        if (retry > 60) break;
                    }
                    while (!(fullResponse.Length > 8));

                    string result = string.Empty;

                    if (fullResponse.ToLower().Contains("answer"))
                    {
                        //return result = getresult(fullResponse.ToLower().Replace("answer", "").Trim());
                        result = getresult(fullResponse.ToLower().Replace("answer", "").Trim());

                        if (!String.IsNullOrEmpty(fullResponse) && String.IsNullOrEmpty(result))
                        {
                            result = fullResponse.ToLower().Replace("answer", "").Trim();
                        }
                        return result;
                    }
                    else
                    {
                        this.CaptchaError = fullResponse;
                        return result;
                    }
                }
                else
                {
                    // this.CaptchaError = fullResponse;
                    webResponse.Close();
                }

            }
            else
            {
                // this.CaptchaError = FormUpload.GetErrorMessage();
            }
            return fullResponse;
        }


        private string GetResponse(string url, string body)
        {
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.0.4; en-gb; GT-I9300 Build/IMM76D) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en;q=0.5");
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Referer = "http://bypassrecaptcha.com/api_test.php";
                request.KeepAlive = true;
                request.ContentType = "application/x-www-form-urlencoded";

                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();
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

        string getresult(string num)
        {
            //string num = "100000011";
            char[] arr = num.ToCharArray();
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
                    finalResult += count.ToString();
                }
                count++;
            }
            return finalResult;
        }

        public void refund()
        {
            try
            {
                //-- refund  string 
                string url = @"http://api.bypassrecaptcha.com/refund.php";
                string body = @"user=" + this.AutoCaptchaServices.BPCUserName + "&password=" + this.AutoCaptchaServices.BPCPassword + "&task_id=" + task_id;
                string fullResponse = GetResponse(url, body);
            }
            catch
            { }
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
