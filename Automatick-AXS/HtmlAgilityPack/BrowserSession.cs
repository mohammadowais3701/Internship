using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace HtmlAgilityPack
{
    /// <summary>
    /// Browser Session
    /// </summary>
    /// 
    [Serializable]
    public class BrowserSession : ICloneable, IDisposable
    {
        public bool _IfJSOn = false;
        public String Payload = String.Empty;
        private bool _isPost;
        private HtmlDocument _htmlDoc;
        WebProxy _Proxy;

        public string EncryptedCredentials = "";
        /// <summary>
        /// s_fid cookie
        /// </summary>
        public String s_fid = "";

        /// <summary>
        /// mobileshopper cookie
        /// </summary>
        public String mobileshopper = "";

        /// <summary>
        /// JSESSIONID cookie
        /// </summary>
        public String JSESSIONID = "";

        /// <summary>
        /// JSESSIONID cookie
        /// </summary>
        public String TS01b4e73f = "";

        /// <summary>
        /// JSESSIONID cookie
        /// </summary>
        public String TS01aef1de = "";

        /// <summary>
        /// cy_track_user cookie
        /// </summary>
        public String cy_track_user = "";

        /// <summary>
        /// 3DSSTBIP cookie
        /// </summary>
        public String _3DSSTBIP = "";

        /// <summary>
        /// mobileid cookie
        /// </summary>
        public String mobileid = "";

        /// <summary>
        /// srv_id cookie
        /// </summary>
        public String srv_id = "";

        /// <summary>
        /// s_cc cookie
        /// </summary>
        public String s_cc = "";

        /// <summary>
        /// s_sq cookie
        /// </summary>
        public String s_sq = "";

        /// <summary>
        /// gpv_pn cookie
        /// </summary>
        public String gpv_pn = "";

        /// <summary>
        /// gpv_c7 cookie
        /// </summary>
        public String gpv_c7 = "";

        public Boolean isJson = false;

        /// <summary>
        /// SID cookie
        /// </summary>
        //public String SID = "";


        //public String SID = "";
        ///// <summary>
        ///// CID cookie
        ///// </summary>
        //public String CID = "";
        ///// <summary>
        ///// CMPS cookie
        ///// </summary>
        //public String CMPS = "";
        ///// <summary>
        ///// BID cookie
        ///// </summary>
        //public String BID = "";
        ///// <summary>
        ///// GEO_OUT cookie
        ///// </summary>
        //public String GEO_OUT = "";
        ///// <summary>
        ///// ORIGIN cookie
        ///// </summary>
        //public String ORIGIN = "";
        ///// <summary>
        ///// MARKET_ID cookie
        ///// </summary>
        //public String MARKET_ID = "";
        ///// <summary>
        ///// GEO_OMN cookie
        ///// </summary>
        //public String GEO_OMN = "";
        ///// <summary>
        ///// GEORAN cookie
        ///// </summary>
        //public String GEORAN = "";
        ///// <summary>
        ///// MARKET_NAME cookie
        ///// </summary>
        //public String MARKET_NAME = "";
        ///// <summary>
        ///// cq-tmol-hd cookie
        ///// </summary>
        //public String cq_tmol_hd="";
        ///// <summary>
        ///// MID cookie
        ///// </summary>
        //public String MID = "";
        ///// <summary>
        ///// MID_MD5 cookie
        ///// </summary>
        //public String MID_MD5 = "";
        ///// <summary>
        ///// MNAME cookie
        ///// </summary>
        //public String MNAME = "";
        ///// <summary>
        ///// SSIDC cookie
        ///// </summary>
        //public String SSIDC = "";
        ///// <summary>
        ///// NDMA cookie
        ///// </summary>
        //public String NDMA = "";
        ///// <summary>
        ///// NPDMA cookie
        ///// </summary>
        //public String NPDMA = "";
        ///// <summary>
        ///// PDIS cookie
        ///// </summary>
        //public String PDIS = "";
        ///// <summary>
        ///// SDIS cookie
        ///// </summary>
        //public String SDIS = "";
        ///// <summary>
        ///// Proxies
        ///// </summary>
        public WebProxy Proxy
        {
            get { return _Proxy; }
            set { _Proxy = value; }
        }

        /// <summary>
        /// Last URL
        /// </summary>
        public String LastURL
        {
            get;
            set;
        }

        /// <summary>
        /// Redirect Location in header
        /// </summary>
        public String RedirectLocation
        {
            get;
            set;
        }

        HtmlWeb _web;
        /// <summary>
        /// HTML web property
        /// </summary>
        public HtmlWeb HTMLWeb
        {
            get { return _web; }
            set { _web = value; }
        }
        /// <summary>
        /// HtmlDocument property
        /// </summary>
        public HtmlDocument HtmlDocument
        {
            get { return _htmlDoc; }
        }
        /// <summary>
        /// String cookies
        /// </summary>
        public String StrCookies
        {
            get;
            set;
        }

        public Boolean IsHCaptcha
        {
            get;
            set;
        }
        /// <summary>
        /// System.Net.CookieCollection. Provides a collection container for instances of Cookie class 
        /// </summary>
        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// Provide a key-value-pair collection of form elements 
        /// </summary>
        public FormElementCollection FormElements { get; set; }
        public Dictionary<String, String> _DISTILL = new Dictionary<string, string>();
        public Dictionary<string, string> _cookies = new Dictionary<string, string>();
        public string GetCookie()
        {

            StringBuilder builder = new StringBuilder();
            foreach (string str in _cookies.Keys)
            {
                if (str.StartsWith("BIGipServer"))
                {
                    builder.AppendFormat("{0}={1};", str, _cookies[str]);
                    //this.BIGipServerglobal_sovereign = _cookies[str];
                }
                else if (str == "SESSION_ID")
                {
                    builder.AppendFormat("{0}={1};", str, _cookies[str]);
                    // this.SESSION_ID = _cookies[str];
                }
                else if (str == "old_session_id")
                {
                    builder.AppendFormat("{0}={1};", str, _cookies[str]);
                }
                else
                {
                    builder.AppendFormat("{0}={1};", str, _cookies[str]);

                }
                //if (!str.StartsWith("BIGipServer"))
                //{
                //    builder.AppendFormat("{0}={1};", , this.BIGipServerglobal_sovereign);
                //}

                //builder.AppendFormat("JSESSIONID=o44bC89dZPd8kJ0p5zhw8TBs.ca01-cluster6-app73;");
            }

            foreach (var item in _DISTILL)
            {
                if (!_cookies.ContainsKey(item.Key))
                {
                    builder.AppendFormat("{0}={1};", item.Key, item.Value);
                }
            }
            return builder.ToString();

        }


        public void SetCookie(string setCookie, Boolean isDistill = false)
        {
            try
            {
                if (!string.IsNullOrEmpty(setCookie) && (setCookie.Contains("SESSION_ID") || setCookie.Contains("D_SID")))// || setCookie.Contains("BIGipServer")))
                {
                    StrCookies = setCookie;
                    try
                    {
                        setCookie = setCookie.Remove(setCookie.IndexOf("expires=") - 1, setCookie.IndexOf("GMT;") + "GMT;".Length - setCookie.IndexOf("expires=") + 1);
                    }
                    catch
                    { }
                    setCookie = Regex.Replace(setCookie, "path=/;,", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    setCookie = Regex.Replace(setCookie, "path=/,", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    setCookie = Regex.Replace(setCookie, "path=/;", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    setCookie = Regex.Replace(setCookie, "Max-Age(.*?);", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    setCookie = Regex.Replace(setCookie, "HttpOnly;", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    setCookie = Regex.Replace(setCookie, "Httponly,", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                    try
                    {
                        MatchCollection matchCookies = Regex.Matches(setCookie, "(.*?)=(.*?);");
                        _cookies = new Dictionary<string, string>();
                        //if (setCookie.Contains("old_session_id"))
                        //{
                        //    ifOldSessionId = true;
                        //}
                        //else if (isDistill)
                        //{
                        //    ifOldSessionId = false;
                        //}

                        foreach (Match mactchItem in matchCookies)
                        {
                            if (mactchItem.Value.EndsWith(";"))
                            {

                                String tmp = mactchItem.Value.TrimEnd(';');
                                int a = -1;
                                a = tmp.IndexOf("=");
                                String cookieName = "";
                                String cookieValue = "";
                                // if (!ifOldSessionId)
                                {
                                    if (a >= 0)
                                    {
                                        cookieName = tmp.Substring(0, a);
                                        cookieValue = tmp.Remove(0, a + 1);
                                    }
                                    if (!String.IsNullOrEmpty(cookieName))
                                    {
                                        _cookies.Add(cookieName.Trim(), cookieValue.Trim());

                                        if (isDistill && cookieName.Trim().StartsWith("D_"))
                                        {
                                            if (!_DISTILL.ContainsKey(cookieName.Trim()))
                                            {
                                                _DISTILL.Add(cookieName.Trim(), cookieValue.Trim());
                                            }
                                            else
                                            {
                                                _DISTILL[cookieName.Trim()] = cookieValue.Trim();
                                            }
                                        }
                                        else if (cookieName.Trim().ToLower().Contains("cluster1"))
                                        {
                                            if (!_DISTILL.ContainsKey(cookieName.Trim()))
                                            {
                                                _DISTILL.Add(cookieName.Trim(), cookieValue.Trim());
                                            }
                                            else
                                            {
                                                _DISTILL[cookieName.Trim()] = cookieValue.Trim();
                                            }
                                        }
                                    }
                                }
                            }

                            //if (ifOldSessionId)
                            {
                                string[] ab = setCookie.Split(' ');
                                foreach (string b in ab)
                                {
                                    try
                                    {
                                        string c = b.Replace("path=/,", "");

                                        if (isDistill)
                                        {
                                            if (!_DISTILL.ContainsKey(c.Split('=')[0].Trim()))
                                            {
                                                _DISTILL.Add(c.Split('=')[0].Trim(), c.Split('=')[1].Trim());
                                            }
                                            else
                                            {
                                                _DISTILL[c.Split('=')[0].Trim()] = c.Split('=')[1].Trim();
                                            }
                                        }

                                        if (!c.Contains("SESSION_ID"))
                                        {
                                            _cookies.Add(c.Split('=')[0], c.Split('=')[1]);
                                        }
                                        else if (c.Contains("SESSION_ID"))
                                        {
                                            _cookies.Add("SESSION_ID", c.Replace("SESSION_ID=", ""));
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }




                        //    ////string[] ab = setCookie.Split(' ');
                        //    ////if (!ifOldSessionId)
                        //    ////{
                        //    ////    foreach (string b in ab)
                        //    ////    {
                        //    ////        try
                        //    ////        {
                        //    ////            string c = b.Replace("path=/,", "");
                        //    ////            _cookies.Add(c.Split('=')[0], c.Split('=')[1]);
                        //    ////        }
                        //    ////        catch { }
                        //    ////    }
                        //    ////}
                        //    ////else
                        //    ////{
                        //    ////    if (ifOldSessionId)
                        //    ////    {
                        //    ////        foreach (string b in ab)
                        //    ////        {
                        //    ////            try
                        //    ////            {
                        //    ////                string c = b.Replace("path=/,", "");
                        //    ////                if (!c.Contains("SESSION_ID"))
                        //    ////                {
                        //    ////                    _cookies.Add(c.Split('=')[0], c.Split('=')[1]);
                        //    ////                }
                        //    ////                else if (c.Contains("SESSION_ID"))
                        //    ////                {
                        //    ////                    _cookies.Add("SESSION_ID", c.Replace("SESSION_ID=", ""));
                        //    ////                }
                        //    ////            }
                        //    ////            catch { }
                        //    ////        }

                        //    ////    }

                        //    ////}
                        //}


                        bool ifreferral_datExists = false;
                        bool ifreferral_idExists = false;
                        bool ifSESSION_IDExists = false;
                        bool ifclient_cookieExists = false;
                        bool ifBIGipServerglobalExists = false;
                        bool ifOldSessionIdExists = false;



                        //foreach (KeyValuePair<String, String> item in _cookies)
                        //{
                        //    if (item.Key == "referral_dat")
                        //    {
                        //        referral_dat = item.Value;
                        //        ifreferral_datExists = true;
                        //    }
                        //    else if (item.Key == "referral_id")
                        //    {
                        //        referral_id = item.Value;
                        //        ifreferral_idExists = true;
                        //    }
                        //    else if (item.Key.ToLower().Contains("bigipserver") && !String.IsNullOrEmpty(item.Value)) //bigipserverglobal
                        //    {
                        //        BIGipServerglobalName = item.Key;
                        //        BIGipServerglobalValue = item.Value;
                        //        ifBIGipServerglobalExists = true;
                        //    }
                        //    else if ((item.Key == "SESSION_ID" && !String.IsNullOrEmpty(item.Value)))
                        //    {
                        //        SESSION_ID = item.Value;
                        //        ifSESSION_IDExists = true;
                        //    }
                        //    else if (item.Key == "client_cookie")
                        //    {
                        //        client_cookie = item.Value;
                        //        ifclient_cookieExists = true;
                        //    }
                        //    else if ((item.Key == "old_session_id"))
                        //    {
                        //        OldSessionId = item.Value;
                        //        ifOldSessionIdExists = true;
                        //    }
                        //}

                        //if (!ifreferral_datExists && !String.IsNullOrEmpty(referral_dat))
                        //{
                        //    _cookies.Add("referral_dat", "referral_dat");
                        //}

                        //if (!ifreferral_idExists && !String.IsNullOrEmpty(referral_id))
                        //{
                        //    _cookies.Add(referral_id, referral_id);
                        //}

                        //if (!ifBIGipServerglobalExists && !String.IsNullOrEmpty(BIGipServerglobalValue))
                        //{
                        //    _cookies.Add(BIGipServerglobalName, BIGipServerglobalValue);
                        //}

                        //if (!ifSESSION_IDExists && !String.IsNullOrEmpty(SESSION_ID))
                        //{
                        //    _cookies.Add("SESSION_ID", SESSION_ID);
                        //}

                        //if (!ifclient_cookieExists && !String.IsNullOrEmpty(client_cookie))
                        //{
                        //    _cookies.Add("client_cookie", client_cookie);
                        //}

                        //if (!ifOldSessionIdExists && !String.IsNullOrEmpty(OldSessionId))
                        //{
                        //    _cookies.Add("old_session_id",OldSessionId);
                        //}


                    }
                    catch
                    {

                    }
                }
                else if (!string.IsNullOrEmpty(setCookie) && setCookie.Contains("JSESSIONID"))
                {
                    StrCookies = setCookie;
                    try
                    {
                        setCookie = setCookie.Remove(setCookie.IndexOf("expires=") - 1, setCookie.IndexOf("GMT;") + "GMT;".Length - setCookie.IndexOf("expires=") + 1);
                    }
                    catch
                    { }
                    setCookie = Regex.Replace(setCookie, "path=/;,", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    setCookie = Regex.Replace(setCookie, "path=/,", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    setCookie = Regex.Replace(setCookie, "path(.*?),", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    setCookie = Regex.Replace(setCookie, "path=/;", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    setCookie = Regex.Replace(setCookie, "Max-Age(.*?);", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                    setCookie = Regex.Replace(setCookie, "HttpOnly;", "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

                    try
                    {
                        MatchCollection matchCookies = Regex.Matches(setCookie, "(.*?)=(.*?);");

                        foreach (Match mactchItem in matchCookies)
                        {
                            if (mactchItem.Value.EndsWith(";"))
                            {
                                String tmp = mactchItem.Value.TrimEnd(';');
                                int a = -1;
                                a = tmp.IndexOf("=");
                                String cookieName = "";
                                String cookieValue = "";
                                if (a >= 0)
                                {
                                    cookieName = tmp.Substring(0, a);
                                    cookieValue = tmp.Remove(0, a + 1);
                                }

                                if (!String.IsNullOrEmpty(cookieName))
                                {
                                    //_cookies.Add(cookieName.Trim(), cookieValue.Trim());

                                    if (cookieName.Trim().StartsWith("JSESSION"))
                                    {
                                        this.JSESSIONID = cookieValue.Trim();

                                        //if (_DISTILL.ContainsKey("JSESSIONID"))
                                        //{
                                        //    _DISTILL["JSESSIONID"] = this.JSESSIONID;
                                        //}
                                        //else
                                        //{
                                        //    _DISTILL.Add("JSESSIONID", this.JSESSIONID);
                                        //}
                                    }
                                    else if (cookieName.Trim().Contains("cluster1"))
                                    {
                                        if (_DISTILL.ContainsKey(cookieName.Trim()))
                                        {
                                            _DISTILL[cookieName.Trim()] = cookieValue.Trim();
                                        }
                                        else
                                        {
                                            _DISTILL.Add(cookieName.Trim(), cookieValue.Trim());
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch
            {

            }
        }


        public String luminatiSessionId
        {
            get;
            set;
        }

        /// <summary>
        /// Makes a HTTP GET request to the given URL
        /// </summary>
        public string Get(string url)
        {
            url = validateAndMakeValidURL(url);
            this.LastURL = url;
            _isPost = false;
            if (_Proxy != null)
            {
                CreateWebRequestObject().Load(url, "GET", _Proxy, null);
            }
            else
            {
                CreateWebRequestObject().Load(url);
            }
            return _htmlDoc.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// Makes a HTTP GET request to the given URL
        /// </summary>
        public byte[] GetBinaryData(string url)
        {
            url = validateAndMakeValidURL(url);
            this.LastURL = url;
            _isPost = false;
            byte[] data = null;
            data = CreateWebRequestObject().LoadBinaryData(url, "GET", _Proxy, null);
            return data;
        }
        private string validateAndMakeValidURL(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute) && !url.StartsWith("http"))
            {
                if (this._web != null)
                {
                    if (this._web.ResponseUri != null)
                    {
                        url = this._web.ResponseUri.Scheme + "://" + this._web.ResponseUri.Host + url;
                    }
                }
            }
            return url;
        }

        /// <summary>
        /// Makes a HTTP POST request to the given URL
        /// </summary>
        public string Post(string url)
        {
            url = validateAndMakeValidURL(url);
            this.LastURL = url;
            _isPost = true;
            if (_Proxy != null)
            {
                CreateWebRequestObject().Load(url, "POST", _Proxy, null);
            }
            else
            {
                CreateWebRequestObject().Load(url, "POST");
            }

            return _htmlDoc.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// To make a HTTP Delete request to the given URL
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string Delete(string url)
        {
            String str = String.Empty;

            try
            {
                url = validateAndMakeValidURL(url);
                this.LastURL = url;
                _isPost = true;
                if (_Proxy != null)
                {
                    CreateWebRequestObject().Load(url, "Delete", _Proxy, null);
                }
                else
                {
                    CreateWebRequestObject().Load(url, "Delete");
                }

                return _htmlDoc.DocumentNode.InnerHtml;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return str;
        }

        /// <summary>
        /// Creates the HtmlWeb object and initializes all event handlers. 
        /// </summary>
        private HtmlWeb CreateWebRequestObject()
        {
            if (_web == null)
            {
                _web = new HtmlWeb();
            }

            _web.IsJson = this.isJson;

            _web.UseCookies = true;
            _web.PreRequest = new HtmlWeb.PreRequestHandler(OnPreRequest);
            _web.PostResponse = new HtmlWeb.PostResponseHandler(OnAfterResponse);
            _web.PreHandleDocument = new HtmlWeb.PreHandleDocumentHandler(OnPreHandleDocument);
            return _web;
        }

        /// <summary>
        /// Event handler for HtmlWeb.PreRequestHandler. Occurs before an HTTP request is executed.
        /// </summary>
        protected bool OnPreRequest(HttpWebRequest request)
        {
            if (!String.IsNullOrEmpty(this.luminatiSessionId))
            {
                request.ConnectionGroupName = this.luminatiSessionId;
            }

            if (!String.IsNullOrEmpty(this.FanSight) && !request.RequestUri.Host.Equals("acs-safekey.americanexpress.com") && !request.RequestUri.Host.Equals("www.securesuite.co.uk") && !request.RequestUri.AbsoluteUri.Contains("secure7.arcot.com"))
            {
                request.Headers.Add("FanSight-Tab", this.FanSight);
            }

            AddCookiesTo(request);               // Add cookies that were saved from previous requests
            if (_isPost) AddPostDataTo(request); // We only need to add post data on a POST request
            return true;
        }

        /// <summary>
        /// Event handler for HtmlWeb.PostResponseHandler. Occurs after a HTTP response is received
        /// </summary>
        protected void OnAfterResponse(HttpWebRequest request, HttpWebResponse response)
        {
            SaveCookiesFrom(response); // Save cookies for subsequent requests
            if (!String.IsNullOrEmpty(response.Headers["Location"]))
            {
                RedirectLocation = response.Headers["Location"];
            }
            else
            {
                RedirectLocation = "";
            }
            if (!String.IsNullOrEmpty(response.Headers["FanSight-Tab"]))
            {
                FanSight = response.Headers["FanSight-Tab"];
            }
            //else
            //{
            //    FanSight = "";
            //}
        }

        /// <summary>
        /// Event handler for HtmlWeb.PreHandleDocumentHandler. Occurs before a HTML document is handled
        /// </summary>
        protected void OnPreHandleDocument(HtmlDocument document)
        {
            SaveHtmlDocument(document);
        }

        /// <summary>
        /// Assembles the Post data and attaches to the request object
        /// </summary>
        private void AddPostDataTo(HttpWebRequest request)
        {
            string payload = string.Empty;
            //string payload="{\"eventCode\":\"STC140707\",\"acceptSplit\":\"0\",\"priceLevel\":\"\",\"qty\":\"1\"}";  // for hovannes CBN140302
            if (!String.IsNullOrEmpty(this.Payload))
            {
                payload = this.Payload;
                request.ContentType = "application/json";
            }
            else if (request.RequestUri.AbsoluteUri.Contains("json"))
            {
                payload = FormElementsToJsonString(FormElements);
                if (payload.Contains("plevel"))
                    payload = payload.Replace("plevel", "priceLevel");
                //string temp = "{\"dvf\":\"item-1-4\",\"item-1-1\":\"1\",\"intAmount\":\"7800\",\"selectedCCType\":\"VI\",\"amount\":\"78.00\",\"ccType\":\"VI\",\"ccNumber\":\"4640123456789112\",\"ccv\":\"436\",\"ccFirstName\":\"d.\",\"ccLastName\":\"walden\",\"ccExpiryMonth\":\"06\",\"ccExpiryYear\":\"15\",\"update_external_customer\":\"on\",\"updateExtCustomer\":1,\"action\":\"purchase\",\"emailAddress\":\"moizaj@yahoo.com\",\"emailAddress2\":\"moizaj@yahoo.com\",\"phone\":\"+9234455634566\",\"firstName\":\"d.\",\"lastName\":\"walden\",\"address1\":\"delhi\",\"address2\":\"delhi\",\"city\":\"delhi\",\"country\":\"IN\",\"region\":\"MHRS\",\"postCode\":\"40001\",\"amexEmailOptOut\":false,\"emailOptOut\":false}";
                //payload = temp;
                request.ContentType = "application/json";
            }
            else
            {
                payload = FormElements.AssemblePostPayload();
                request.ContentType = "application/x-www-form-urlencoded";
            }

            if (this._IfJSOn)
            {
                request.Accept = "application/json, text/javascript, */*; q=0.01";

                request.ContentType = "application/json";

                if (!request.RequestUri.AbsoluteUri.Contains("https://eu-west-1-verifycaptcha-api.queue-it.net/captchaverify"))
                {
                    request.Headers.Add("X-Requested-With", "XMLHttpRequest");

                    request.Headers.Add("Accept-Encoding", "gzip, deflate");

                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                }
            }

            if (request.RequestUri.AbsoluteUri.Contains("axs.com") && !IsHCaptcha)
            {
                request.ContentType = "application/json";
                //request.Accept = "application/json";
                //request.Headers.Add("origin", "https://tix.axs.com");
            }

            byte[] buff = Encoding.UTF8.GetBytes(payload.ToCharArray());
            request.ContentLength = buff.Length;

            if (this._web.ResponseUri != null && String.IsNullOrEmpty(request.Referer))
            {
                request.Referer = this._web.ResponseUri.AbsoluteUri;
            }

            System.IO.Stream reqStream = request.GetRequestStream();
            reqStream.Write(buff, 0, buff.Length);
        }

        public static string FormElementsToJsonString(FormElementCollection formElements)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            foreach (KeyValuePair<string, string> item in formElements)
            {

                json.Append(GetJsonString(item.Key));
                json.Append(":");

                if (item.Key == "updateExtCustomer" || item.Key == "emailOptOut" || item.Key == "amexEmailOptOut" || item.Key == "acceptSplit")
                    json.Append(item.Value);
                else
                    json.Append(GetJsonString(item.Value));

                json.Append(",");
            }
            json.Remove(json.ToString().LastIndexOf(','), 1);
            json.Append("}");
            return json.ToString();
        }

        public static string GetJsonString(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }

        public long UnixTimeNow(DateTime datetime)
        {
            TimeSpan _TimeSpan = (datetime - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)_TimeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// Add cookies to the request object
        /// </summary>
        private void AddCookiesTo(HttpWebRequest request)
        {
            Uri uri = request.RequestUri;

            if (!String.IsNullOrEmpty(StrCookies))
            {
                Regex.CacheSize = 0;
                request.CookieContainer = new System.Net.CookieContainer
                {
                    MaxCookieSize = 8192
                };
                // Uri uri = request.RequestUri;
                String tmpCookies = StrCookies;

                //

                tmpCookies = Regex.Replace(tmpCookies, "path=(.*?),", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "path=(.*?)(/?)", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "path=(.*?)(/?);", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "max-age=(.*?)(/?),", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "max-age=(.*?)(/?);", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "max-age=(.*?)(/?)", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "Version=(.*?);", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "Expires=(.*?),", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "\\d(\\d)?(-|\\s)\\w{3}(-|\\s)\\d(\\d)?(\\d)?(\\d)? \\d(\\d)?:\\d(\\d)?:\\d(\\d)? \\w{3};", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "\\d(\\d)?(-|\\s)\\w{3}(-|\\s)\\d(\\d)?(\\d)?(\\d)? \\d(\\d)?:\\d(\\d)?:\\d(\\d)? \\w{3},", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "domain=(.*?);", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "Secure(.*?);", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "HttpOnly(.*?),", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "HttpOnly(.*?);", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "Expires=\\w{3}, \\d(\\d)?(-|\\s)\\w{3}(-|\\s)\\d(\\d)?(\\d)?(\\d)? \\d(\\d)?:\\d(\\d)?:\\d(\\d)? \\w{3};", "", RegexOptions.IgnoreCase);

                MatchCollection matchCookies = Regex.Matches(tmpCookies, "(.*?)=(.*?);");
                foreach (Match mactchItem in matchCookies)
                {
                    if (mactchItem.Value.EndsWith(";"))
                    {
                        String tmp = mactchItem.Value.TrimEnd(';');
                        int a = -1;
                        a = tmp.IndexOf("=");
                        String cookieName = "";
                        String cookieValue = "";

                        if (a >= 0)
                        {
                            cookieName = tmp.Substring(0, a);
                            if (tmp.Contains(","))
                                tmp = tmp.Remove(tmp.IndexOf(','));
                            cookieValue = tmp.Remove(0, a + 1);
                        }
                        if (!String.IsNullOrEmpty(cookieName))
                        {
                            if (cookieName.Contains(";"))
                            {
                                cookieName = cookieName.Replace(";", String.Empty);
                            }
                            Cookie ck = new Cookie(cookieName.Trim(), cookieValue.Trim());
                            request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                        }
                    }
                }


                //Cookie ck00 = new Cookie("AMCV_B7B972315A1341150A495EFE%40AdobeOrg", "1075005958%7CMCIDTS%7C18291%7CMCMID%7C00352757215102649660812056037297267325%7CMCOPTOUT-1580300383s%7CNONE%7CvVersion%7C4.4.1");
                //request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck00);

                //Cookie ck0 = new Cookie("AMCVS_B7B972315A1341150A495EFE%40AdobeOrg", "1");
                //request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck0);

                Cookie ack = new Cookie("AMCV_B7B972315A1341150A495EFE%40AdobeOrg", "1075005958%7CMCIDTS%7C18306%7CMCMID%7C83045388051431756754518782686414686273%7CMCOPTOUT-1581622579s%7CNONE%7CvVersion%7C4.4.1");
                request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ack);

                ack = new Cookie("AMCVS_B7B972315A1341150A495EFE%40AdobeOrg", "1");
                request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ack);

                if (request.RequestUri.AbsoluteUri.Contains("https://unifiedapicommerce.us-prod0.axs.com/veritix/session/v2/"))
                {
                    Cookie ck1 = new Cookie("gpv_c7", "tix.axs.com%3A");
                    request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck1);

                    Cookie ck2 = new Cookie("gpv_pn", "tix.axs.com%3Acheckout%3Acaptcha");
                    request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck2);
                }
                else if (request.RequestUri.AbsoluteUri.Contains("https://unifiedapicommerce.us-prod0.axs.com/veritix/cart/v2/add-items"))
                {
                    Cookie ck1 = new Cookie("gpv_c7", "tix.axs.com%3Aselect%20tickets");
                    request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck1);

                    Cookie ck2 = new Cookie("gpv_pn", "tix.axs.com%3Aselection%3Aselect%20tickets%3A554987904%3A682");
                    request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck2);
                }


                Cookie ck3 = new Cookie("s_cc", "true");
                request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck3);

                Cookie ck4 = new Cookie("s_gnr7", UnixTimeNow(DateTime.Now) + "-New");
                request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck4);

                CookieCollection tmpCookiesColl = request.CookieContainer.GetCookies(new Uri(uri.Scheme + "://" + uri.Host + "/"));

               
                if (tmpCookiesColl != null)
                {
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "s_fid")
                        {
                            s_fid = item.Value;
                        }
                        else if (item.Name == "mobileshopper.session.id")
                        {
                            mobileshopper = item.Value;
                        }
                        else if (item.Name == "3DSSTBIP")
                        {
                            _3DSSTBIP = item.Value;
                        }
                        else if (item.Name == "cy_track_user")
                        {
                            cy_track_user = item.Value;
                        }
                        else if (item.Name == "JSESSIONID")
                        {
                            JSESSIONID = item.Value;
                        }
                        else if (item.Name == "TS01b4e73f")
                        {
                            TS01b4e73f = item.Value;
                        }
                        else if (item.Name == "TS01aef1de")
                        {
                            TS01aef1de = item.Value;
                        }
                        else if (item.Name == "mobileid")
                        {
                            mobileid = item.Value;
                        }
                        else if (item.Name == "srv_id")
                        {
                            srv_id = item.Value;
                        }
                        else if (item.Name == "s_cc")
                        {
                            s_cc = item.Value;
                        }
                        else if (item.Name == "s_sq")
                        {
                            s_sq = item.Value;
                        }
                        else if (item.Name == "gpv_pn")
                        {
                            gpv_pn = item.Value;
                        }
                        else if (item.Name == "gpv_c7")
                        {
                            gpv_c7 = item.Value;
                        }
                        else if (item.Name == "AWSALB")
                        {
                            AWSALB = item.Value;
                        }
                        else if (item.Name == "axs_ecomm")
                        {
                            axs_ecomm = item.Value;
                        }
                        else if (item.Name == "io")
                        {
                            io = item.Value;
                        }
                        else if (item.Name == "hap")
                        {
                            hap = item.Value;
                        }
                        else if (item.Name == "srv")
                        {
                            srv = item.Value;
                        }
                        else if (item.Name == "cf_clearance")
                        {
                            cf_clearance = item.Value;
                        }
                    }

                    bool ifMobileshopper = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "mobileshopper.session.id")
                        {
                            ifMobileshopper = true;
                            break;
                        }
                    }
                    if (!ifMobileshopper && !String.IsNullOrEmpty(mobileshopper))
                    {
                        Cookie ck = new Cookie("mobileshopper.session.id", mobileshopper);
                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }

                    bool ifio = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "io")
                        {
                            ifio = true;
                            break;
                        }
                    }
                    if (!ifio && !String.IsNullOrEmpty(io))
                    {
                        Cookie ck = new Cookie("io", io);
                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }

                    bool ifaxs_ecomm = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "axs_ecomm")
                        {
                            ifaxs_ecomm = true;
                            break;
                        }
                    }
                    if (!ifaxs_ecomm && !String.IsNullOrEmpty(axs_ecomm))
                    {
                        Cookie ck = new Cookie("axs_ecomm", axs_ecomm);
                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }
                    bool ifhap = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "hap")
                        {
                            ifhap = true;
                            break;
                        }
                    }
                    if (!ifhap && !String.IsNullOrEmpty(hap))
                    {
                        Cookie ck = new Cookie("hap", hap);
                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }
                    bool ifsrv = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "srv")
                        {
                            ifsrv = true;
                            break;
                        }
                    }
                    if (!ifsrv && !String.IsNullOrEmpty(srv))
                    {
                        Cookie ck = new Cookie("srv", srv);
                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }

                    bool ifAWSALB = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "AWSALB")
                        {
                            ifAWSALB = true;
                            break;
                        }
                    }
                    if (!ifAWSALB && !String.IsNullOrEmpty(AWSALB))
                    {
                        Cookie ck = new Cookie("AWSALB", AWSALB);
                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }

                    bool ifcy_track_user = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "cy_track_user")
                        {
                            ifcy_track_user = true;
                            break;
                        }
                    }
                    if (!ifcy_track_user && !String.IsNullOrEmpty(cy_track_user))
                    {
                        Cookie ck = new Cookie("cy_track_user", cy_track_user);

                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }

                    if ( !String.IsNullOrEmpty(cf_clearance))
                    {
                        Cookie ck = new Cookie("cf_clearance", cf_clearance);

                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }



                    bool ifJSESSIONID = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "JSESSIONID")
                        {
                            ifJSESSIONID = true;
                            break;
                        }
                    }
                    if (!ifJSESSIONID && !String.IsNullOrEmpty(JSESSIONID))
                    {
                        Cookie ck = new Cookie("JSESSIONID", JSESSIONID);

                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }



                    bool ifTS01b4e73f = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "TS01b4e73f")
                        {
                            ifTS01b4e73f = true;
                            break;
                        }
                    }
                    if (!ifTS01b4e73f && !String.IsNullOrEmpty(TS01b4e73f))
                    {
                        Cookie ck = new Cookie("TS01b4e73f", TS01b4e73f);

                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }

                    bool ifTS01aef1de = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "TS01aef1de")
                        {
                            ifTS01aef1de = true;
                            break;
                        }
                    }
                    if (!ifTS01aef1de && !String.IsNullOrEmpty(TS01aef1de))
                    {
                        Cookie ck = new Cookie("TS01aef1de", TS01aef1de);

                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }

                    bool if3DSSTBIP = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "3DSSTBIP")
                        {
                            if3DSSTBIP = true;
                            break;
                        }
                    }
                    if (!if3DSSTBIP && !String.IsNullOrEmpty(_3DSSTBIP))
                    {
                        Cookie ck = new Cookie("3DSSTBIP", _3DSSTBIP);

                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }

                    bool ifsrv_id = false;
                    if (!ifsrv_id && !String.IsNullOrEmpty(srv_id))
                    {
                        Cookie ck = new Cookie("srv_id", srv_id);
                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }

                    //    /////////////////
                    bool ifs_fid = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "s_fid")
                        {
                            ifs_fid = true;
                            break;
                        }
                    }
                    if (!ifs_fid && !String.IsNullOrEmpty(s_fid))
                    {
                        Cookie ck = new Cookie("s_fid", s_fid);
                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }

                    bool ifmobileid = false;
                    foreach (Cookie item in tmpCookiesColl)
                    {
                        if (item.Name == "mobileid")
                        {
                            ifmobileid = true;
                            break;
                        }
                    }
                    if (!ifmobileid && !String.IsNullOrEmpty(mobileid))
                    {
                        Cookie ck = new Cookie("mobileid", mobileid);
                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }
                }

                if (_DISTILL != null){

                    foreach (KeyValuePair<String, String> item in _DISTILL)
                    {
                        try
                        {
                            Cookie ck = new Cookie(item.Key.Trim(), item.Value.Trim());
                            request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                if (!String.IsNullOrEmpty(EncryptedCredentials))
                {
                    Cookie ck = new Cookie("axsid_user", EncryptedCredentials);
                    request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                }
            }
            else if (_DISTILL != null)
            {
                foreach (KeyValuePair<String, String> item in _DISTILL)
                {
                    try
                    {
                        Cookie ck = new Cookie(item.Key.Trim(), item.Value.Trim());
                        request.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), ck);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Get cookies for manual browsering
        /// </summary>
        public Dictionary<String, String> GetCookiesForManualBrowsing()
        {
            Dictionary<String, String> cookiesForManualBuy = new Dictionary<String, String>();
            if (!String.IsNullOrEmpty(StrCookies))
            {
                String tmpCookies = StrCookies;
                Regex.CacheSize = 0;
                //

                tmpCookies = Regex.Replace(tmpCookies, "path=(.*?),", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "path=(.*?)(/?)", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "\\d(\\d)?(-|\\s)\\w{3}(-|\\s)\\d(\\d)?(\\d)?(\\d)? \\d(\\d)?:\\d(\\d)?:\\d(\\d)? \\w{3},", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "domain=(.*?);", "", RegexOptions.IgnoreCase);

                tmpCookies = Regex.Replace(tmpCookies, "Expires=\\w{3}, \\d(\\d)?(-|\\s)\\w{3}(-|\\s)\\d(\\d)?(\\d)?(\\d)? \\d(\\d)?:\\d(\\d)?:\\d(\\d)? \\w{3};", "", RegexOptions.IgnoreCase);

                MatchCollection matchCookies = Regex.Matches(tmpCookies, "(.*?)=(.*?);");
                foreach (Match mactchItem in matchCookies)
                {
                    if (mactchItem.Value.EndsWith(";"))
                    {
                        String tmp = mactchItem.Value.TrimEnd(';');
                        int a = -1;
                        a = tmp.IndexOf("=");
                        String cookieName = "";
                        String cookieValue = "";

                        if (a >= 0)
                        {
                            cookieName = tmp.Substring(0, a);
                            cookieValue = tmp.Remove(0, a + 1);
                        }
                        if (!String.IsNullOrEmpty(cookieName))
                        {
                            if (!cookiesForManualBuy.ContainsKey(cookieName))
                            {
                                cookiesForManualBuy.Add(cookieName, cookieValue);
                            }
                        }
                    }
                }
                //

                if (cookiesForManualBuy != null)
                {
                    bool ifs_fid = false;
                    foreach (KeyValuePair<String, String> item in cookiesForManualBuy)
                    {
                        if (item.Key == "s_fid")
                        {
                            ifs_fid = true;
                            break;
                        }
                    }
                    if (!ifs_fid && !String.IsNullOrEmpty(s_fid))
                    {
                        if (!cookiesForManualBuy.ContainsKey("s_fid"))
                        {
                            cookiesForManualBuy.Add("s_fid", s_fid);
                        }
                    }
                    ///////////////////////////

                    bool ifmobileshopper = false;
                    bool ifsrv_id = false;
                    bool ifmobileid = false;
                    foreach (KeyValuePair<String, String> item in cookiesForManualBuy)
                    {
                        if (item.Key == "mobileshopper.session.id")
                        {
                            ifmobileshopper = true;
                            //break;
                        }
                        if (item.Key == "ifsrv_id")
                        {
                            ifsrv_id = true;
                            //break;
                        }
                        if (item.Key.Equals("mobileid"))
                        {
                            ifmobileid = true;
                        }
                    }

                    if (!ifmobileshopper && !String.IsNullOrEmpty(mobileshopper))
                    {
                        if (!cookiesForManualBuy.ContainsKey("mobileshopper.session.id"))
                        {
                            cookiesForManualBuy.Add("mobileshopper.session.id", mobileshopper);
                        }
                    }
                    if (!ifmobileid && !String.IsNullOrEmpty(mobileid))
                    {
                        if (!cookiesForManualBuy.ContainsKey("mobileid"))
                        {
                            cookiesForManualBuy.Add("mobileid", mobileid);
                        }
                    }
                    if (!ifsrv_id && !String.IsNullOrEmpty(srv_id))
                    {
                        if (!cookiesForManualBuy.ContainsKey("srv_id"))
                        {
                            cookiesForManualBuy.Add("srv_id", srv_id);
                        }
                    }
                }

            }

            return cookiesForManualBuy;
        }

        /// <summary>
        /// Saves cookies from the response object to the local CookieCollection object
        /// </summary>
        private void SaveCookiesFrom(HttpWebResponse response)
        {
            //if (response.Cookies.Count > 0)
            //{
            //    if (Cookies == null) Cookies = new CookieCollection();
            //    Cookies.Add(response.Cookies);
            //}
            if (!String.IsNullOrEmpty(response.Headers["Set-Cookie"]))
            {
                StrCookies = response.Headers["Set-Cookie"];
            }
        }

        /// <summary>
        /// Saves the form elements collection by parsing the HTML document
        /// </summary>
        private void SaveHtmlDocument(HtmlDocument document)
        {
            _htmlDoc = document;
            FormElements = new FormElementCollection(_htmlDoc);
        }

        #region ICloneable Members

        /// <summary>
        /// Clone of Browser Session
        /// </summary>
        /// <returns>Browser Session cloned object</returns>
        public object Clone()
        {
            try
            {
                HttpWebRequest tmpwebreq = this._web.Request;
                this._web.Request = null;

                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms, this);
                    ms.Position = 0;
                    this._web.Request = tmpwebreq;
                    BrowserSession bsCloned = (BrowserSession)formatter.Deserialize(ms);
                    bsCloned._web.Request = tmpwebreq;
                    bsCloned.Proxy = this.Proxy;

                    return bsCloned;
                }


            }
            catch (Exception)
            {

            }
            return null;

        }

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose()
        {
            try
            {
                //this.BID = null;
                //this.CID = null;
                //this.CMPS = null;
                //this.SID = null;
                this.Proxy = null;
                this.StrCookies = null;
                if (this._web != null)
                {
                    GC.SuppressFinalize(this._web);
                }
                this._web = null;
                if (this._htmlDoc != null)
                {
                    GC.SuppressFinalize(this._htmlDoc);
                }
                this._htmlDoc = null;
                if (this.FormElements != null)
                {
                    GC.SuppressFinalize(this.FormElements);
                }
                this.FormElements = null;
                if (this.Cookies != null)
                {
                    GC.SuppressFinalize(this.Cookies);
                }
                this.Cookies = null;
                GC.SuppressFinalize(this);
                //GC.Collect();
            }
            catch (Exception)
            {

            }
        }

        #endregion

        public string FanSight { get; set; }

        public string AWSALB { get; set; }

        public string axs_ecomm { get; set; }

        public string hap { get; set; }

        public string srv { get; set; }

        public string cf_clearance { get; set; }

        public string io { get; set; }
    }

    /// <summary>
    /// Represents a combined list and collection of Form Elements.
    /// </summary>
    /// 
    /// <summary>
    /// Represents a combined list and collection of Form Elements.
    /// </summary>
    /// 
    [Serializable]
    public class FormElementCollection : Dictionary<string, string>
    {
        /// <summary>
        /// Form structure
        /// </summary>
        public struct Form
        {
            /// <summary>
            /// Name property/attribute of form tag
            /// </summary>
            public String Name;
            /// <summary>
            /// Action property/attribute of form tag
            /// </summary>
            public String Action;
        }
        /// <summary>
        /// All forms in the webpage
        /// </summary>
        public Dictionary<String, Form> Forms
        {
            get;
            set;
        }
        /// <summary>
        /// Constructor of Form Collection
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public FormElementCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        /// <summary>
        /// Constructor. Parses the HtmlDocument to get all form input elements. 
        /// </summary>
        public FormElementCollection(HtmlDocument htmlDoc)
        {
            var inputs = htmlDoc.DocumentNode.Descendants("input");
            foreach (var element in inputs)
            {
                AddInputElement(element);
            }

            var menus = htmlDoc.DocumentNode.Descendants("select");
            foreach (var element in menus)
            {
                AddMenuElement(element);
            }

            var textareas = htmlDoc.DocumentNode.Descendants("textarea");
            foreach (var element in textareas)
            {
                AddTextareaElement(element);
            }

            this.Forms = new Dictionary<String, Form>();
            try
            {
                IEnumerable<HtmlNode> forms = htmlDoc.DocumentNode.Descendants("form");
                foreach (HtmlNode element in forms)
                {
                    Form frm = new Form();
                    string name = element.GetAttributeValue("name", "");
                    string action = element.GetAttributeValue("action", "");
                    frm.Name = name;
                    frm.Action = action;
                    if (!this.Forms.ContainsKey(name))
                    {
                        this.Forms.Add(name, frm);
                    }
                }
            }
            catch { }
        }
        private void AddInputElement(HtmlNode element)
        {
            string name = element.GetAttributeValue("name", "");
            string value = element.GetAttributeValue("value", "");
            string type = element.GetAttributeValue("type", "");

            if (string.IsNullOrEmpty(name)) return;

            switch (type.ToLower())
            {
                case "checkbox":
                case "radio":
                    if (!ContainsKey(name)) Add(name, "");
                    string isChecked = element.GetAttributeValue("checked", "unchecked");
                    if (!isChecked.Equals("unchecked")) this[name] = value;
                    break;
                default:
                    if (!ContainsKey(name) && !name.Equals("undefined")) Add(name, value);
                    break;
            }
        }

        private void AddMenuElement(HtmlNode element)
        {
            try
            {
                string name = element.GetAttributeValue("name", "");
                HtmlNodeCollection options = element.SelectNodes("option");

                if (string.IsNullOrEmpty(name)) return;



                // choose the first option as default
                if (options != null)
                {
                    HtmlNode firstOp = options[0];
                    string defaultValue = firstOp.GetAttributeValue("value", firstOp.InnerText);

                    if (!ContainsKey(name) && !name.Equals("undefined")) Add(name, defaultValue);

                    // check if any option is selected
                    foreach (HtmlNode option in options)
                    {
                        string selected = option.GetAttributeValue("selected", "notSelected");
                        if (!selected.Equals("notSelected"))
                        {
                            string selectedValue = option.GetAttributeValue("value", option.NextSibling.InnerText);
                            this[name] = selectedValue;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void AddTextareaElement(HtmlNode element)
        {
            string name = element.GetAttributeValue("name", "");
            if (string.IsNullOrEmpty(name)) return;
            if (!ContainsKey(name) && !name.Equals("undefined")) Add(name, element.InnerText);
        }
        /// <summary>
        /// Assembles all form elements and values to POST. Also html encodes the values.  
        /// </summary>
        public string AssemblePostPayload()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var element in this)
            {
                string value = System.Web.HttpUtility.UrlEncode(element.Value);
                if (element.Key.Contains("="))
                {
                    sb.Append("&" + System.Web.HttpUtility.UrlEncode(element.Key, Encoding.ASCII) + "=" + value);
                }
                else
                {
                    sb.Append("&" + element.Key + "=" + value);
                }
            }
            return sb.ToString().Substring(1);
        }
    }
}