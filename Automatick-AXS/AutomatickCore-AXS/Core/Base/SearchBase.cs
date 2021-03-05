using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automatick.Core
{
    public class SearchBase : ITicketSearch
    {
        #region Variables

        Boolean _ifStopping = false;
        System.Threading.Timer timeoutStopping = null;
        Thread StartThread = null;
        public AutoResetEvent captchaload = null;
        Boolean _ifCaptchaWaiting = false;
        AutoResetEvent _sleep = null;
        IAutoCaptchaService solveAutoCaptcha = null;
        private MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();
        frmSelectDeliveryOption _frmSelectDO = null;
        frmSelectAccount _frmSelectAccount = null;
        protected Proxy _proxy = null;
        public String _SelectedDeliveryOption = null;
        Boolean _ifRestarting = false;
        public string context = String.Empty;
        private String clearSessionURL = "http://trigger.pvms?cmd=clear&session=";

        ApplicationStartUp appStartup = new ApplicationStartUp(Application.StartupPath);

        protected BrowserSession _session = null;

        public BrowserSession Session
        {
            get { return _session; }
            set { _session = value; }
        }

        public Stopping stopping
        {
            get;
            set;
        }

        public ApplicationStartUp _AppStartUp
        {
            get { return appStartup; }
            set { appStartup = value; }
        }

        public String RecaptchaV2Key
        {
            get;
            set;
        }

        public String RecapToken
        {
            get;
            set;
        }

        #endregion

        #region ITicketSearch Members
        public System.Drawing.Bitmap FlagImage
        {
            get;
            set;
        }

        public ResetAfterStop resetAfterStop
        {
            get;
            set;
        }

        public Captcha Captcha
        {
            get;
            set;
        }

        public String Recaptcha
        {
            get;
            set;
        }

        public ITicketParameter _CurrentParameter
        {
            get;
            set;
        }

        public ITicket Ticket
        {
            get;
            set;
        }

        public String TicketName
        {
            get
            {
                String ticketName = String.Empty;
                if (this.Ticket != null)
                {
                    ticketName = this.Ticket.TicketName;
                }
                return ticketName;
            }
        }

        public Proxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        public String Section
        {
            get;
            set;
        }

        public String Row
        {
            get;
            set;
        }

        public String Seat
        {
            get;
            set;
        }

        public String TotalPrice
        {
            get;
            set;
        }

        public String Price
        {
            get;
            set;
        }

        public String Quantity
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

        public String TimeLeft
        {
            get;
            set;
        }

        public String MoreInfo
        {
            get;
            set;
        }

        public String Status
        {
            get;
            set;
        }

        public Boolean IfUseAutoCaptcha
        {
            get;
            set;
        }

        public Boolean IfUseProxy
        {
            get;
            set;
        }

        public Boolean IfWorking
        {
            get;
            set;
        }

        public Boolean IfResetSearch
        {
            get;
            set;
        }

        public SearchBase search
        {
            get;
            set;
        }

        public Boolean IfFound
        {
            get;
            set;
        }

        public Boolean IfAutoBuy
        {
            get;
            set;
        }

        public String LastURLForManualBuy
        {
            get;
            set;
        }

        public bool IsPresale
        {
            get { return _isPresale; }
            set { _isPresale = value; }
        }

        bool _stateWaiting = true;

        public bool StateWaiting
        {
            get { return _stateWaiting; }
            set { _stateWaiting = value; }
        }

        public String luminatiSessionId
        {
            get;
            set;
        }

        public void start()
        {
            this.resetAfterStop = null;
            this.IfWorking = true;
            this.MoreInfo = "";
            StartThread = new Thread(this.workerThread);
            StartThread.Priority = ThreadPriority.Highest;
            StartThread.SetApartmentState(ApartmentState.STA);
            StartThread.IsBackground = true;
            StartThread.Start();
        }

        public void stop()
        {
            try
            {
                if (this._ifStopping)
                {
                    return;
                }
                TimerCallback tcbTimeoutStopping = new TimerCallback(this.stoppingHandlerTimeout);
                timeoutStopping = new System.Threading.Timer(tcbTimeoutStopping, this, new TimeSpan(0, 0, 15), new TimeSpan(-1));

                this.IfWorking = false;

                this._ifStopping = true;
                this.TimeLeft = "";
                this.Section = "";
                this.Row = "";
                this.Seat = "";
                this.Price = "";
                this.Quantity = "";
                this.TimeLeft = "";
                this.Description = "";
                this.MoreInfo = "";
                changeStatus(TicketSearchStatus.StoppingStatus);
                //this.resetAfterStop = null;
                //this._ifRestarting = false;
                this.stopping = new Stopping(this.stoppingHandler);
                try
                {
                    if (this._session != null)
                    {
                        if (this._session.HTMLWeb != null)
                        {
                            if (this._session.HTMLWeb.Request != null)
                            {
                                this._session.HTMLWeb.Request.Abort();
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }

                AXSTicket ticket = (AXSTicket)this.Ticket;

                if (this._ifCaptchaWaiting)
                {
                    if (Interlocked.Read(ref ServerPortsPicker.ServerPortsPickerInstance.Requested) > 0)
                    {
                        Interlocked.Decrement(ref ServerPortsPicker.ServerPortsPickerInstance.Requested);
                    }

                    this._ifCaptchaWaiting = false;
                }

                if (this._proxy != null)
                {
                    if ((this._proxy.TheProxyType != Core.Proxy.ProxyType.Relay) && (ProxyPicker.ProxyPickerInstance.ProxyManager != null))
                    {
                        ProxyPicker.ProxyPickerInstance.ProxyManager.ReleaseProxy(this._proxy);
                    }
                    else if (ProxyPicker.ProxyPickerInstance.ProxyRelayManager != null)
                    {
                        #region Release Session from Server

                        ClearSessionFromServer();

                        #endregion

                        ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                    }
                    this._proxy = null;
                }

                if (ticket != null)
                {
                    if (ticket.CaptchaQueue != null)
                    {
                        lock (ticket.CaptchaQueue)
                        {
                            ticket.CaptchaQueue.Remove(this);
                        }
                    }

                    if (ticket.CaptchaBrowserQueue != null)
                    {
                        lock (ticket.CaptchaBrowserQueue)
                        {
                            ticket.CaptchaBrowserQueue.Remove(this);
                        }
                    }
                }

                if (this.Captcha != null)
                {
                    this.Captcha.captchaentered.Set();
                }

                if (this.captchaload != null)
                {
                    this.captchaload.Set();
                }

                if (this._frmSelectDO != null)
                {
                    if (!this._frmSelectDO.IsDisposed)
                    {
                        this._frmSelectDO.Close();
                    }
                    this._frmSelectDO = null;
                }

                if (this._frmSelectAccount != null)
                {
                    if (!this._frmSelectAccount.IsDisposed)
                    {
                        this._frmSelectAccount.Close();
                    }
                    this._frmSelectAccount = null;
                }

            }
            catch { }
            finally
            {
                this.MoreInfo = "";
                GC.SuppressFinalize(this);
            }
            //GC.Collect();
        }

        void stoppingHandlerTimeout(Object o)
        {
            if (this._ifStopping)
            {
                try
                {
                    if (this.StartThread != null)
                    {
                        this.StartThread.Abort();
                    }
                }
                catch (Exception)
                {

                }

                this.stoppingHandler();
            }
        }

        void stoppingHandler()
        {
            try
            {
                this.MoreInfo = "";
                changeStatus(TicketSearchStatus.StopStatus);

                this._ifStopping = false;
                this.stopping = null;
                sleep(500);
                _sleep.Close();

                GC.SuppressFinalize(_sleep);

                _sleep = null;
            }
            catch
            {

            }
            try
            {
                if (timeoutStopping != null)
                {
                    timeoutStopping.Dispose();
                    timeoutStopping = null;
                }
                if (this.Ticket != null)
                {
                    if (this._proxy != null)
                    {
                        if (this._proxy.TheProxyType != Core.Proxy.ProxyType.Relay)
                        {
                            if (ProxyPicker.ProxyPickerInstance.ProxyManager != null)
                            {
                                ProxyPicker.ProxyPickerInstance.ProxyManager.ReleaseProxy(this._proxy);
                            }
                        }
                        else
                        {
                            if (ProxyPicker.ProxyPickerInstance.ProxyRelayManager != null)
                            {
                                #region Release Session from Server

                                ClearSessionFromServer();

                                #endregion

                                ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                            }
                        }
                        this._proxy = null;
                    }


                    if (!this.Ticket.isRunning)
                    {
                        this.Dispose();
                    }
                }
            }
            catch (Exception)
            {

            }


            if (this.resetAfterStop != null)
            {
                this.resetAfterStop();
            }
        }

        private void sleep(int time)
        {
            try
            {
                if (_sleep != null)
                {
                    _sleep.WaitOne(time);
                }
            }
            catch (Exception)
            {

            }
        }

        public void restart()
        {
            if (this._ifStopping)
            {
                return;
            }
            this.MoreInfo = "";
            Thread thReset = new Thread(this.restartThread);
            thReset.SetApartmentState(ApartmentState.STA);
            thReset.Priority = ThreadPriority.Highest;
            thReset.IsBackground = true;
            thReset.Start();
        }

        void restartThread()
        {
            try
            {
                if (this.IfWorking)
                {
                    if (doc != null)
                    {
                        if (!this.isTix)
                        {
                            doc.LoadHtml(post(this, this.XmlUrl.Replace("/info/", "/bfox/") + "?methodName=webapi.clearOrder&calledFrom=releaseLocks&sessionKey=" + this.SessionKey + "&XMLRPC_callCounter=8", String.Format("<methodCall><methodName>webapi.clearOrder</methodName><params><param><value><string>{0}</string></value></param><param><value><array><data /></array></value></param></params></methodCall>", this.SessionKey)));
                        }
                        else if (this.isTix)
                        {
                            this._session.Payload = "{\"onsaleIDToResetTimers\":\"" + HttpUtility.UrlDecode(this.OnSaleUrl) + "\",\"isTimerExpired\":false}";

                            String strhtml = this._session.Delete("https://unified-api.axs.com/veritix/cart/v2?onsaleID=" + this.OnSaleUrl);
                        }
                    }
                    this.resetAfterStop = new ResetAfterStop(this.resetAfterStopHandler);
                    this.stop();
                    changeStatus(TicketSearchStatus.RestartingStatus);
                }
                else if (!this.IfWorking && !this._ifStopping)
                {
                    this.start();
                }
            }
            catch { }
        }

        public void autoBuy()
        {
            try
            {

                if (this.IfWorking && this.IfFound && this.Ticket.isRunning)
                {
                    this.IfAutoBuy = true;
                    this.currLog.BuyStatus = TicketsLog.AutoBuyStatus;
                    this.currLog.Account = (this._selectedAccountForAutoBuy != null) ? this._selectedAccountForAutoBuy.EmailAddress : "";
                    this.Ticket.tic_Logs.Add(this.currLog);
                }
            }
            catch (Exception)
            {

            }
        }

        public void autoBuyGuest()
        {
            try
            {

                if (this.IfWorking && this.IfFound && this.Ticket.isRunning)
                {
                    this.IfAutoBuy = true;
                    this.isGuest = true;
                    this.currLog.BuyStatus = TicketsLog.AutoBuyStatus;
                    this.currLog.Account = (this._selectedAccountForAutoBuy != null) ? this._selectedAccountForAutoBuy.EmailAddress : "";
                    this.Ticket.tic_Logs.Add(this.currLog);
                }
            }
            catch (Exception)
            {

            }
        }

        public void autoBuyWithoutProxy()
        {
            try
            {
                if (this._proxy != null)
                {

                    if (this._proxy.TheProxyType != Core.Proxy.ProxyType.Relay)
                    {
                        if (ProxyPicker.ProxyPickerInstance.ProxyManager != null)
                        {
                            ProxyPicker.ProxyPickerInstance.ProxyManager.ReleaseProxy(this._proxy);
                        }
                    }
                    else
                    {
                        if (ProxyPicker.ProxyPickerInstance.ProxyRelayManager != null)
                        {
                            #region Release Session from Server

                            ClearSessionFromServer();

                            #endregion

                            ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Logger.LoggerInstance.Add(new Log(ErrorType.EXCEPTION, this.Ticket.TicketID, this.Ticket.URL, ex.Message + ex.StackTrace));
            }
            this._proxy = null;

            if (this._session != null)
            {
                this._session.Proxy = null;
            }

            this.autoBuy();
        }

        private string ParseHtml(string html)
        {
            string json = string.Empty;

            try
            {
                int startindex = html.IndexOf("countdownFinishedText") - 2;

                int endIndex = html.IndexOf("DocumentTitle\":\"Queue-it\"");

                html = html.Replace(html.Substring(startindex, endIndex - startindex), "null");

                json = html.Replace("DocumentTitle\":\"Queue-it\"}", "");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return json;
        }

        private string ExtractLastLayoutVersion(string html)
        {
            string version = string.Empty;

            try
            {
                try
                {
                    version = html.Substring(html.IndexOf("lastLayoutVersion") + 19, (html.IndexOf("lastLayoutName") - 4) - (html.IndexOf("lastLayoutVersion") + 17));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }

                if (String.IsNullOrEmpty(version))
                {
                    try
                    {
                        version = html.Substring(html.IndexOf("layoutVersion") + 15, (html.IndexOf("messageFeed") - 7) - (html.IndexOf("layoutVersion") + 20));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }

                if (String.IsNullOrEmpty(version))
                {
                    try
                    {
                        version = html.Substring(html.IndexOf("layoutVersion") + 15, (html.IndexOf("updateInterval") - 4) - (html.IndexOf("layoutVersion") + 13));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return version;
        }

        private string ExtractLastLayoutName(string html)
        {
            string version = string.Empty;

            try
            {
                version = html.Substring(html.IndexOf("lastLayoutName") + 17, (html.IndexOf("\"},\"texts\":{\"header\":") - (html.IndexOf("lastLayoutName") + 17)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            if (String.IsNullOrEmpty(version))
            {
                try
                {
                    version = html.Substring(html.IndexOf("layout:") + 9, (html.IndexOf("layoutVersion") - (html.IndexOf("layout:") + 21)));

                }
                catch (Exception ex)
                {

                }
            }

            if (String.IsNullOrEmpty(version))
            {
                try
                {
                    version = html.Substring(html.IndexOf("layoutName") + 13, (html.IndexOf("layoutVersion") - (html.IndexOf("layoutName") + 16)));

                }
                catch (Exception ex)
                {

                }
            }

            return version;
        }

        void resetAfterStopHandler()
        {
            if (this.Ticket.isRunning && !this.IfWorking)
            {
                this.start();
            }
        }

        #endregion

        #region SharedMethods

        void stoppingHandlerTimeout(Object o)
        {
            if (this._ifStopping)
            {
                try
                {
                    if (this.StartThread != null)
                    {
                        this.StartThread.Abort();
                    }
                }
                catch (Exception)
                {

                }

                this.stoppingHandler();
            }
        }

        void stoppingHandler()
        {
            try
            {
                this.MoreInfo = "";
                changeStatus(TicketSearchStatus.StopStatus);

                this._ifStopping = false;
                this.stopping = null;
                sleep(500);
                _sleep.Close();

                GC.SuppressFinalize(_sleep);

                _sleep = null;
            }
            catch
            {

            }
            try
            {
                if (timeoutStopping != null)
                {
                    timeoutStopping.Dispose();
                    timeoutStopping = null;
                }
                if (this.Ticket != null)
                {
                    if (this._proxy != null)
                    {
                        if (this._proxy.TheProxyType != Core.Proxy.ProxyType.Relay)
                        {
                            if (ProxyPicker.ProxyPickerInstance.ProxyManager != null)
                            {
                                ProxyPicker.ProxyPickerInstance.ProxyManager.ReleaseProxy(this._proxy);
                            }
                        }
                        else
                        {
                            if (ProxyPicker.ProxyPickerInstance.ProxyRelayManager != null)
                            {
                                #region Release Session from Server

                                ClearSessionFromServer();

                                #endregion

                                ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                            }
                        }
                        this._proxy = null;
                    }


                    if (!this.Ticket.isRunning)
                    {
                        this.Dispose();
                    }
                }
            }
            catch (Exception)
            {

            }


            if (this.resetAfterStop != null)
            {
                this.resetAfterStop();
            }
        }

        private void sleep(int time)
        {
            try
            {
                if (_sleep != null)
                {
                    _sleep.WaitOne(time);
                }
            }
            catch (Exception)
            {

            }
        }

        public void restart()
        {
            if (this._ifStopping)
            {
                return;
            }
            this.MoreInfo = "";
            Thread thReset = new Thread(this.restartThread);
            thReset.SetApartmentState(ApartmentState.STA);
            thReset.Priority = ThreadPriority.Highest;
            thReset.IsBackground = true;
            thReset.Start();
        }

        void restartThread()
        {
            try
            {
                if (this.IfWorking)
                {
                    if (doc != null)
                    {
                        if (!this.isTix)
                        {
                            doc.LoadHtml(post(this, this.XmlUrl.Replace("/info/", "/bfox/") + "?methodName=webapi.clearOrder&calledFrom=releaseLocks&sessionKey=" + this.SessionKey + "&XMLRPC_callCounter=8", String.Format("<methodCall><methodName>webapi.clearOrder</methodName><params><param><value><string>{0}</string></value></param><param><value><array><data /></array></value></param></params></methodCall>", this.SessionKey)));
                        }
                        else if (this.isTix)
                        {
                            this._session.Payload = "{\"onsaleIDToResetTimers\":\"" + HttpUtility.UrlDecode(this.OnSaleUrl) + "\",\"isTimerExpired\":false}";

                            String strhtml = this._session.Delete("https://unified-api.axs.com/veritix/cart/v2?onsaleID=" + this.OnSaleUrl);
                        }
                    }
                    this.resetAfterStop = new ResetAfterStop(this.resetAfterStopHandler);
                    this.stop();
                    changeStatus(TicketSearchStatus.RestartingStatus);
                }
                else if (!this.IfWorking && !this._ifStopping)
                {
                    this.start();
                }
            }
            catch { }
        }

        protected void ClearSessionFromServer()
        {
            String result = String.Empty;

            try
            {
                HttpWebRequest webRequest = System.Net.HttpWebRequest.Create(clearSessionURL + this.Proxy.userName + "-context-" + context + "-session-" + this.Proxy.LuminatiSessionId) as System.Net.HttpWebRequest;
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.86 Safari/537.36";
                webRequest.KeepAlive = true;
                webRequest.Accept = "*/*";

                if (this.Proxy != null)
                {
                    if (this.Proxy.TheProxyType != Proxy.ProxyType.Custom)
                    {
                        webRequest.Timeout = 10000;
                    }

                    webRequest.Proxy = this.Proxy.toWebProxy(this.context);
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                try
                {
                    HttpWebResponse resp = null;
                    Encoding respenc = null;
                    var isGZipEncoding = false;

                    System.IO.Stream reqStream = null;

                    try
                    {
                        resp = webRequest.GetResponse() as HttpWebResponse;
                    }
                    catch (WebException we)
                    {
                        resp = (HttpWebResponse)we.Response;
                    }

                    if (resp != null)
                    {
                        if (!string.IsNullOrEmpty(resp.ContentEncoding))
                        {
                            isGZipEncoding = resp.ContentEncoding.ToLower().StartsWith("gzip") ? true : false;
                            if (!isGZipEncoding)
                            {
                                respenc = Encoding.GetEncoding(resp.ContentEncoding);
                            }
                        }

                        if (isGZipEncoding)
                        {
                            reqStream = new GZipStream(resp.GetResponseStream(), CompressionMode.Decompress);
                        }
                        else
                        {
                            reqStream = resp.GetResponseStream();
                        }

                        StreamReader sr = new StreamReader(reqStream);
                        result = sr.ReadToEnd();

                        if (!String.IsNullOrEmpty(result))
                        {
                            if (result.ToLower().Equals("ok"))
                            {
                                Debug.WriteLine("Proxy released from server too -- " + this.Proxy.LuminatiSessionId);
                            }
                        }
                    }
                    reqStream.Close();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected long UnixTimeNow()
        {
            TimeSpan _TimeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)_TimeSpan.TotalMilliseconds;
        }

        protected DateTime epoch2string(double epoch)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(epoch);
        }

        protected Boolean qualifies()
        {
            bool ifQualifies = false;
            string searchSection = this.Section;
            string searchRow = this.Row;
            int isearchRow = 0;


            try
            {
                if (this.Ticket.TicketFoundCriterions == null)
                {
                    ifQualifies = true;
                    return ifQualifies;
                }
                else if (this.Ticket.TicketFoundCriterions.Count <= 0)
                {
                    ifQualifies = true;
                    return ifQualifies;
                }

                foreach (FoundCriteria criteria in this.Ticket.TicketFoundCriterions)
                {

                    if (searchRow.Length == 1 && searchRow.Length == ((!String.IsNullOrEmpty(criteria.RowFrom)) ? criteria.RowFrom.Length : searchRow.Length) && searchRow.Length == ((!String.IsNullOrEmpty(criteria.RowTo)) ? criteria.RowTo.Length : searchRow.Length))
                    {
                        int iFirstRow = 0;
                        int iLastRow = 0;
                        //Below are the variable used to check whether the row contains numeric data or not.
                        bool bFirstRow = false;
                        bool bLastRow = false;
                        bool bSearchRow = false;

                        if (!String.IsNullOrEmpty(criteria.Section) && !String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo))
                        {
                            bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                            bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                            bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);

                            if (!bSearchRow)
                            {
                                isearchRow = Convert.ToInt32(Convert.ToChar(searchRow.ToLower()));
                            }
                            if (!bFirstRow || !bLastRow)
                            {
                                iFirstRow = Convert.ToInt32(Convert.ToChar(criteria.RowFrom.ToLower()));
                                iLastRow = Convert.ToInt32(Convert.ToChar(criteria.RowTo.ToLower()));
                            }
                            if ((bFirstRow && bLastRow) && (!bSearchRow))
                            {
                                ifQualifies = false;
                            }
                            else if (criteria.Section.ToLower() == searchSection.ToLower() && (isearchRow >= iFirstRow && isearchRow <= iLastRow))
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                        }
                        else if (String.IsNullOrEmpty(criteria.Section) && (!String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo)))
                        {
                            bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                            bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                            bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);

                            if (!bSearchRow)
                            {
                                isearchRow = Convert.ToInt32(Convert.ToChar(searchRow.ToLower()));
                            }
                            if (!bFirstRow || !bLastRow)
                            {
                                iFirstRow = Convert.ToInt32(Convert.ToChar(criteria.RowFrom.ToLower()));
                                iLastRow = Convert.ToInt32(Convert.ToChar(criteria.RowTo.ToLower()));
                            }
                            if ((bFirstRow && bLastRow) && (!bSearchRow))
                            {
                                ifQualifies = false;
                            }
                            else if (isearchRow >= iFirstRow && isearchRow <= iLastRow)
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                        }
                        else if (!String.IsNullOrEmpty(criteria.Section) && (String.IsNullOrEmpty(criteria.RowFrom) && String.IsNullOrEmpty(criteria.RowTo)))
                        {
                            if (criteria.Section.ToLower() == searchSection.ToLower())
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                        }
                    }
                    else if (searchRow.Length > 1 && searchRow.Length == ((!String.IsNullOrEmpty(criteria.RowFrom)) ? criteria.RowFrom.Length : searchRow.Length) && searchRow.Length == ((!String.IsNullOrEmpty(criteria.RowTo)) ? criteria.RowTo.Length : searchRow.Length))//&& searchRow.Length == criteria.RowFrom.Length && searchRow.Length == criteria.RowTo.Length)
                    {
                        try
                        {
                            int iFirstRow = 0;
                            int iLastRow = 0;
                            bool bFirstRow = false;
                            bool bLastRow = false;
                            bool bSearchRow = false;

                            if (!String.IsNullOrEmpty(criteria.Section) && !String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo))
                            {
                                bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                                bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                                bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);

                                if (!bSearchRow)
                                {
                                    isearchRow = Convert.ToInt32(searchRow.ToLower().ToCharArray()[0]);
                                }
                                if (!bFirstRow || !bLastRow)
                                {
                                    iFirstRow = Convert.ToInt32(criteria.RowFrom.ToLower().ToCharArray()[0]);
                                    iLastRow = Convert.ToInt32(criteria.RowTo.ToLower().ToCharArray()[0]);
                                }
                                if ((bFirstRow && bLastRow) && (!bSearchRow))
                                {
                                    ifQualifies = false;
                                }
                                else if (criteria.Section.ToLower() == searchSection.ToLower() && (isearchRow >= iFirstRow && isearchRow <= iLastRow))
                                {
                                    ifQualifies = true;
                                    return ifQualifies;
                                }
                            }
                            else if (String.IsNullOrEmpty(criteria.Section) && (!String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo)))
                            {

                                bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                                bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                                bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);

                                if (!bSearchRow)
                                {
                                    isearchRow = Convert.ToInt32(searchRow.ToLower().ToCharArray()[0]);
                                }
                                if (!bFirstRow || !bLastRow)
                                {
                                    iFirstRow = Convert.ToInt32(criteria.RowFrom.ToLower().ToCharArray()[0]);
                                    iLastRow = Convert.ToInt32(criteria.RowTo.ToLower().ToCharArray()[0]);
                                }
                                if ((bFirstRow && bLastRow) && (!bSearchRow))
                                {
                                    ifQualifies = false;
                                }
                                else if (isearchRow >= iFirstRow && isearchRow <= iLastRow)
                                {
                                    ifQualifies = true;
                                    return ifQualifies;
                                }
                            }
                            else if (!String.IsNullOrEmpty(criteria.Section) && (String.IsNullOrEmpty(criteria.RowFrom) && String.IsNullOrEmpty(criteria.RowTo)))
                            {
                                if (criteria.Section.ToLower() == searchSection.ToLower())
                                {
                                    ifQualifies = true;
                                    return ifQualifies;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            ifQualifies = false;
                        }
                    }
                    else
                    {
                        int iFirstRow = 0;
                        int iLastRow = 0;
                        //Below are the variable used to check whether the row contains numeric data or not.
                        bool bFirstRow = false;
                        bool bLastRow = false;
                        bool bSearchRow = false;

                        if (!String.IsNullOrEmpty(criteria.Section) && !String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo))
                        {

                            bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                            bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                            bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);


                            if (!bFirstRow || !bLastRow || !bSearchRow)
                            {
                                return false;
                            }
                            if ((bFirstRow && bLastRow) && (!bSearchRow))
                            {
                                ifQualifies = false;
                            }
                            else if (criteria.Section.ToLower() == searchSection.ToLower() && (isearchRow >= iFirstRow && isearchRow <= iLastRow))
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                            else
                            {
                                ifQualifies = false;
                            }
                        }
                        else if (String.IsNullOrEmpty(criteria.Section) && (!String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo)))
                        {
                            bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                            bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                            bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);

                            if (!bFirstRow || !bLastRow || !bSearchRow)
                            {
                                return false;
                            }
                            if ((bFirstRow && bLastRow) && (!bSearchRow))
                            {
                                ifQualifies = false;
                            }
                            else if (isearchRow >= iFirstRow && isearchRow <= iLastRow)
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                            else
                            {
                                ifQualifies = false;
                            }
                        }
                        else if (!String.IsNullOrEmpty(criteria.Section) && (String.IsNullOrEmpty(criteria.RowFrom) && String.IsNullOrEmpty(criteria.RowTo)))
                        {
                            if (criteria.Section.ToLower() == searchSection.ToLower())
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                            else
                            {
                                ifQualifies = false;
                            }
                        }
                        else
                        {
                            ifQualifies = false;
                        }

                    }
                }
            }
            catch (Exception)
            {
                ifQualifies = false;
            }

            return ifQualifies;
        }

        public void AddUpdateField(BrowserSession b, string key, string value)
        {
            if (!b.FormElements.ContainsKey(key))
            {
                b.FormElements.Add(key, value);
            }
            else
            {
                b.FormElements[key] = value;
            }
        }

        //private String getAlphaValue(String recaptcha_response_field)
        //{
        //    String alpha = "";
        //    if (recaptcha_response_field.Length == 0)
        //    {
        //        alpha = "1000";
        //    }
        //    else if (recaptcha_response_field.Length == 1)
        //    {
        //        alpha = "1000,125";
        //    }
        //    else if (recaptcha_response_field.Length == 2)
        //    {
        //        alpha = "1000,125,93";
        //    }
        //    else if (recaptcha_response_field.Length == 3)
        //    {
        //        alpha = "1000,125,93,266";
        //    }
        //    else if (recaptcha_response_field.Length == 4)
        //    {
        //        alpha = "1000,125,93,266,156";
        //    }
        //    else if (recaptcha_response_field.Length == 5)
        //    {
        //        alpha = "1000,125,93,266,156,500";
        //    }
        //    else if (recaptcha_response_field.Length == 6)
        //    {
        //        alpha = "1000,172,125,109,94,141,500";
        //    }
        //    else if (recaptcha_response_field.Length == 7)
        //    {
        //        alpha = "1000,219,94,94,125,109,94,500";
        //    }
        //    else if (recaptcha_response_field.Length == 8)
        //    {
        //        alpha = "1000,219,94,94,125,109,94,500,219";
        //    }
        //    else if (recaptcha_response_field.Length == 9)
        //    {
        //        alpha = "1000,219,94,94,125,109,94,500,219,93";
        //    }
        //    else if (recaptcha_response_field.Length == 10)
        //    {
        //        alpha = "1000,219,94,94,125,109,94,500,219,93,125";
        //    }
        //    else if (recaptcha_response_field.Length == 11)
        //    {
        //        alpha = "1000,219,94,94,125,109,94,500,219,93,125,100";
        //    }
        //    else if (recaptcha_response_field.Length == 12)
        //    {
        //        alpha = "1000,219,94,94,125,109,94,500,219,93,125,100,178";
        //    }
        //    else if (recaptcha_response_field.Length == 13)
        //    {
        //        alpha = "1000,219,94,94,125,109,94,500,219,93,125,100,178,340";
        //    }
        //    else if (recaptcha_response_field.Length == 14)
        //    {
        //        alpha = "1000,219,94,94,125,109,94,500,219,93,125,100,178,340,87";
        //    }
        //    else if (recaptcha_response_field.Length == 15)
        //    {
        //        alpha = "1000,219,94,94,125,109,94,500,219,93,125,100,178,340,87,112";
        //    }
        //    return alpha;
        //}

        //private void getLambdaValue(ref string str11, ref int attempt)
        //{
        //    if (this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("md5hexx(\"") != -1)
        //    {
        //        int startIndex = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("md5hexx(\"") + "md5hexx(\"".Length;
        //        int index = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("\"", startIndex);
        //        string str12 = this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(startIndex, index - startIndex);
        //        startIndex = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("a) == \"", startIndex);
        //        if (startIndex != -1)
        //        {
        //            startIndex += "a) == \"".Length;
        //            index = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("\"", startIndex);
        //            string str13 = this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(startIndex, index - startIndex);
        //            str11 = this.getLambdaVal(str12, str13, out attempt);
        //        }
        //    }
        //    else if (((this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("d5m_chars = \"") != -1) && (this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("d5m_str(") != -1)) && (this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("d5m_work()") != -1))
        //    {
        //        int num4 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("d5m_chars = \"") + "d5m_chars = \"".Length;
        //        int num5 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("\"", num4);
        //        string str14 = this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num4, num5 - num4);
        //        num4 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("d5m_str(");
        //        num4 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("var s = \"", num4) + "var s = \"".Length;
        //        num5 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("\"", num4);
        //        string str15 = this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num4, num5 - num4);
        //        num4 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("d5m_work()");
        //        num4 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("d5m_core(s)[", num4) + "d5m_core(s)[".Length;
        //        int num6 = -1;
        //        try
        //        {
        //            num6 = Convert.ToInt32(this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num4, 1));
        //        }
        //        catch (Exception)
        //        {
        //        }
        //        if (num6 != -1)
        //        {
        //            num4 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("== ", num4) + "== ".Length;
        //            num5 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf(")", num4);
        //            int num7 = -1;
        //            try
        //            {
        //                num7 = Convert.ToInt32(this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num4, num5 - num4));
        //            }
        //            catch (Exception)
        //            {
        //            }
        //            if (num7 != -1)
        //            {
        //                for (int i = 0; i < 0x989680; i++)
        //                {
        //                    StringBuilder builder = new StringBuilder(str15);
        //                    int num9 = i;
        //                    do
        //                    {
        //                        builder.Append(str14[num9 % str14.Length]);
        //                        num9 /= str14.Length;
        //                    }
        //                    while (num9 > 0);
        //                    byte[] buffer2 = this._md5.ComputeHash(Encoding.ASCII.GetBytes(builder.ToString()));
        //                    int num10 = buffer2[((num6 + 1) * 4) - 1];
        //                    num10 += (buffer2[((num6 + 1) * 4) - 2] & 1) * 0x100;
        //                    if (num10 == num7)
        //                    {
        //                        str11 = builder.ToString() + ":" + i.ToString();
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else if (this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("new OpticalTest(function()") != -1)
        //    {
        //        int num11 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("new OpticalTest(function()");
        //        num11 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("return \"", num11);
        //        if (num11 != -1)
        //        {
        //            num11 += "return \"".Length;
        //            int num12 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("\"", num11);
        //            str11 = this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num11, num12 - num11);
        //        }
        //    }
        //    else
        //    {
        //        attempt = 200;
        //        int num13 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("new OpticalTest(");
        //        if (num13 != -1)
        //        {
        //            num13 += "new OpticalTest(".Length;
        //            int num14 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf(",", num13);
        //            string str16 = this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num13, num14 - num13);
        //            num13 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("var " + str16 + " = function ()");
        //            if (num13 != -1)
        //            {
        //                num13 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("return \"", num13);
        //                if (num13 != -1)
        //                {
        //                    num13 += "return \"".Length;
        //                    num14 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("\"", num13);
        //                    str11 = this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num13, num14 - num13);
        //                }
        //            }
        //            else
        //            {
        //                num13 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("function " + str16 + " (a) { return ");
        //                if (num13 != -1)
        //                {
        //                    num13 += ("function " + str16 + " (a) { return ").Length;
        //                    num14 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("(", num13);
        //                    string str17 = this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num13, num14 - num13);
        //                    num13 += str17.Length + 1;
        //                    num14 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("(", num13);
        //                    string str18 = this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num13, num14 - num13);
        //                    num13 += str18.Length + 1;
        //                    num14 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf(")", num13);
        //                    int result2 = 0;
        //                    if (int.TryParse(this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num13, num14 - num13), out result2))
        //                    {
        //                        num13 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("function " + str18 + " (a) { return ") + ("function " + str18 + " (a) { return ").Length;
        //                        num14 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf(";", num13);
        //                        string expression = this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num13, num14 - num13);
        //                        result2 = this.processExpression(expression, result2);
        //                        num13 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf("function " + str17 + " (a) { return ") + ("function " + str17 + " (a) { return ").Length;
        //                        num14 = this._session.HtmlDocument.DocumentNode.InnerHtml.IndexOf(";", num13);
        //                        string str20 = this._session.HtmlDocument.DocumentNode.InnerHtml.Substring(num13, num14 - num13);
        //                        str11 = this.processExpression(str20, result2).ToString();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private string getLambdaVal(string md5start, string md5hash, out int attempt)
        //{
        //    string str = "0123456789abcdefghijklmnopqrstuvwxyz";
        //    attempt = 0;
        //    while (attempt < 0xf4240)
        //    {
        //        string str2 = string.Empty;
        //        int num = attempt;
        //        do
        //        {
        //            str2 = str[num % str.Length].ToString() + str2;
        //            num /= str.Length;
        //        }
        //        while (num != 0);
        //        if (this.computeMD5(md5start + str2, md5hash.Length) == md5hash)
        //        {
        //            return str2;
        //        }
        //        attempt++;
        //    }
        //    return string.Empty;
        //}

        //private string computeMD5(string data, int hashlen)
        //{
        //    lock (this._md5)
        //    {
        //        byte[] buffer = this._md5.ComputeHash(Encoding.ASCII.GetBytes(data));
        //        if (hashlen == 1)
        //        {
        //            return ((buffer[0] < 0x10) ? "0" : buffer[0].ToString("x")[0].ToString());
        //        }
        //        return ((buffer[0] < 0x10) ? ("0" + buffer[0].ToString("x")) : buffer[0].ToString("x"));
        //    }
        //}

        private int processExpression(string expression, int value)
        {
            while (expression.StartsWith("("))
            {
                expression = expression.Substring(1);
            }
            while (expression.EndsWith(")"))
            {
                expression = expression.Substring(0, expression.Length - 1);
            }
            if (expression.StartsWith("a + "))
            {
                int result = 0;
                if (int.TryParse(expression.Substring("a + ".Length), out result))
                {
                    return (value + result);
                }
                return value;
            }
            if (expression.StartsWith("a - "))
            {
                int num2 = 0;
                if (int.TryParse(expression.Substring("a - ".Length), out num2))
                {
                    return (value - num2);
                }
                return value;
            }
            if (expression.StartsWith("a * "))
            {
                int num3 = 0;
                if (int.TryParse(expression.Substring("a * ".Length), out num3))
                {
                    return (value * num3);
                }
                return value;
            }
            if (expression.StartsWith("a / "))
            {
                int num4 = 0;
                if (int.TryParse(expression.Substring("a / ".Length), out num4))
                {
                    return (value / num4);
                }
            }
            return value;
        }

        private string extractParameter(string code, string parameter)
        {
            int index = code.ToLower().IndexOf(parameter.ToLower());
            if (index == -1)
            {
                return null;
            }
            index += parameter.Length;
            index = code.ToLower().IndexOfAny(new char[] { '\'', '"' }, index);
            if (index == -1)
            {
                return null;
            }
            index++;
            int num2 = code.ToLower().IndexOfAny(new char[] { '\'', '"' }, index);
            if (num2 == -1)
            {
                return null;
            }
            return code.Substring(index, num2 - index);
        }

        private void ReleaseAccount(String accountEmail)
        {
            if (!String.IsNullOrEmpty(accountEmail))
            {
                if (this.Ticket.TicketAccountsInTransition != null)
                {
                    if (this.Ticket.TicketAccountsInTransition.Contains(accountEmail))
                    {
                        lock (this.Ticket.TicketAccountsInTransition)
                        {
                            this.Ticket.TicketAccountsInTransition.Remove(accountEmail);
                        }
                    }
                }
            }

        }

        private Boolean CheckingAccountAvailability(String accountEmail, int buyinglimit)
        {
            bool result = false;
            int totalBought = 0;
            try
            {
                if (this.Ticket.BuyHistory.ContainsKey(accountEmail))
                {
                    totalBought = this.Ticket.BuyHistory[accountEmail];
                }
                lock (this.Ticket.TicketAccountsInTransition)
                {
                    int count = this.Ticket.TicketAccountsInTransition.Count(p => p == accountEmail);
                    if ((count + totalBought) < buyinglimit)
                    {
                        this.Ticket.TicketAccountsInTransition.Add(accountEmail);
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public void changeStatus(String status)
        {
            try
            {
                if (this.IfWorking && this.Ticket.isRunning)
                {
                    this.Status = status;
                }
                else if (!this.IfWorking && this.Ticket.isRunning && this.resetAfterStop != null)
                {
                    this.Status = status;
                }
                else if (!this.IfWorking && this.Ticket.isRunning && this.stopping != null)
                {
                    this.Status = status;
                }

                if (this.Ticket.Searches.changeDelegate != null)
                {
                    this.Ticket.Searches.changeDelegate(this);
                }

                //if (this.Ticket.onChangeForGauge != null)
                //{
                //    this.Ticket.onChangeForGauge();
                //}

                if (this._proxy != null)
                {
                    if (_proxy.TheProxyType != Core.Proxy.ProxyType.Relay)
                    {
                        String strProxy = _proxy.ToString() + ", ";
                        if (!this.MoreInfo.StartsWith(strProxy))
                        {
                            Regex.CacheSize = 0;
                            Match m = Regex.Match(this.MoreInfo, @"(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.(\d)(\d)?(\d)?:(\d)(\d)?(\d)?(\d)?(\d)?(\d)?,");
                            if (m.Success)
                            {
                                this.MoreInfo = Regex.Replace(this.MoreInfo, @"(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.(\d)(\d)?(\d)?:(\d)(\d)?(\d)?(\d)?(\d)?(\d)?,", "");
                            }

                            this.MoreInfo = strProxy + this.MoreInfo;
                        }
                    }
                    else
                    {
                        String strProxy = _proxy.ToString() + ", ";
                        if (!this.MoreInfo.StartsWith(strProxy))
                        {
                            Regex.CacheSize = 0;
                            Match m = Regex.Match(this.MoreInfo, @"(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.(\d)(\d)?(\d)?:(\d)(\d)?(\d)?(\d)?(\d)?(\d)?,");
                            if (m.Success)
                            {
                                this.MoreInfo = Regex.Replace(this.MoreInfo, @"(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.(\d)(\d)?(\d)?:(\d)(\d)?(\d)?(\d)?(\d)?(\d)?,", "");
                            }

                            this.MoreInfo = strProxy + this.MoreInfo;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        protected HtmlAgilityPack.HtmlDocument getResponse(HttpWebRequest request, out string strHtml)
        {
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream receiveStream = null;
                    HtmlDocument h = new HtmlDocument();
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
                    return h;
                }
            }
            catch
            {
                strHtml = string.Empty;
                return null;
            }
        }

        protected String getResponse(HttpWebRequest request)
        {
            String strHtml = String.Empty;

            try
            {
                WebResponse wResp = request.GetResponse();
                HttpWebResponse response = wResp as HttpWebResponse;
                Stream receiveStream = null;
                //HtmlAgilityPack.HtmlDocument h = new HtmlAgilityPack.HtmlDocument();
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
                //h.LoadHtml(strHtml);

                wResp.Close();

                return strHtml;
                //}
            }
            catch
            {
                strHtml = string.Empty;
                return null;
            }
        }

        static string commaSeperated(string words)
        {
            try
            {
                int n = 0;
                int x = 0;
                string answer = string.Empty;

                if (words.Contains(" "))
                {
                    words = words.Replace(" ", String.Empty);
                }

                if (words.ToString().Contains("0"))
                {
                    answer += "0,";
                    n = Convert.ToInt32(words.Replace("0", String.Empty));
                }
                else
                {
                    n = Convert.ToInt32(words);
                }

                do
                {
                    x = n % 10;
                    string temp = n.ToString().Replace(x.ToString(), String.Empty);

                    if (!String.IsNullOrEmpty(temp))
                    {
                        n = Convert.ToInt32(temp);
                    }
                    else
                    {
                        n = 0;
                    }

                    answer += x.ToString() + ",";
                }
                while ((n % 10) > 0);

                return answer.TrimEnd(',');

            }
            catch
            {
                return words;
            }
        }

        string changeNumbers(string words)
        {
            try
            {
                // words = words.Trim().Replace(",", "").Replace("\\", "");
                string[] array = words.Trim().Split(',');
                string capwords = string.Empty;

                foreach (string c in array)
                {
                    switch (c)
                    {
                        case "1":
                            capwords += "0,";
                            break;
                        case "2":
                            capwords += "1,";
                            break;
                        case "3":
                            capwords += "2,";
                            break;
                        case "4":
                            capwords += "3,";
                            break;
                        case "5":
                            capwords += "4,";
                            break;
                        case "6":
                            capwords += "5,";
                            break;
                        case "7":
                            capwords += "6,";
                            break;
                        case "8":
                            capwords += "7,";
                            break;
                        case "9":
                            capwords += "8,";
                            break;
                        case "10":
                            capwords += "9,";
                            break;
                        case "11":
                            capwords += "10,";
                            break;
                        case "l2":
                            capwords += "11,";
                            break;
                        case "13":
                            capwords += "12,";
                            break;
                        case "14":
                            capwords += "13,";
                            break;
                        case "15":
                            capwords += "14,";
                            break;
                        case "16":
                            capwords += "15,";
                            break;
                        case "a":
                            capwords += "0,";
                            break;
                        case "b":
                            capwords += "1,";
                            break;
                        case "c":
                            capwords += "2,";
                            break;
                        case "d":
                            capwords += "3,";
                            break;
                        case "e":
                            capwords += "4,";
                            break;
                        case "f":
                            capwords += "5,";
                            break;
                        case "g":
                            capwords += "6,";
                            break;
                        case "h":
                            capwords += "7,";
                            break;
                        case "i":
                            capwords += "8,";
                            break;
                        case "j":
                            capwords += "9,";
                            break;
                        case "k":
                            capwords += "10,";
                            break;
                        case "l":
                            capwords += "11,";
                            break;
                        case "m":
                            capwords += "12,";
                            break;
                        case "n":
                            capwords += "13,";
                            break;
                        case "o":
                            capwords += "14,";
                            break;
                        case "p":
                            capwords += "15,";
                            break;
                    }
                }

                return capwords = capwords.TrimEnd(',');

            }
            catch
            {
                return words;
            }
        }

        #endregion

        public void workerThread()
        {
            try
            {
                while (this.IfWorking && this.Ticket.isRunning)
                {
                    try
                    {
                        if (this._frmSelectDO != null)
                        {
                            if (!this._frmSelectDO.IsDisposed)
                            {
                                this._frmSelectDO.Close();
                            }
                            this._frmSelectDO = null;
                        }

                        if (this._frmSelectAccount != null)
                        {
                            if (!this._frmSelectAccount.IsDisposed)
                            {
                                this._frmSelectAccount.Close();
                            }
                            this._frmSelectAccount = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        //  ProxyPicker.ProxyPickerInstance.RecheckProxyStatus(this.Proxy, ex.Message);
                    }

                    #region QueueIt handling
                    String strURL = this.Ticket.URL;

                    String strHTML = String.Empty;

                    String lastLayoutVersion = String.Empty;

                    String lastLayoutName = String.Empty;

                    BrowserSession queueSession = null;

                    if (strURL.Contains(".queueit") || strURL.Contains("q.axs.co.uk"))
                    {
                        queueSession = new BrowserSession();

                        try
                        {
                            String queueURL = strURL;

                            String[] values = queueURL.Split('&');

                            foreach (String item in values)
                            {
                                if (item.Split('=')[0].Equals("e"))
                                {
                                    strURL = values[0] + "&e=" + item.Split('=')[1];
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                        }

                        queueSession.HTMLWeb = new HtmlWeb();
                        queueSession.HTMLWeb.IfAllowAutoRedirect = false;

                        strHTML = queueSession.Get(strURL);

                        if (!queueSession.RedirectLocation.ToLower().Contains("tix") && !queueSession.RedirectLocation.ToLower().Contains("tickets") && !queueSession.RedirectLocation.ToLower().Contains("home.aspx"))
                        {
                            if (queueSession.RedirectLocation.Contains(".queueit") || strURL.Contains("q.axs.co.uk"))
                            {
                                strHTML = queueSession.Get(_session.RedirectLocation);
                            }

                            if (!String.IsNullOrEmpty(queueSession.RedirectLocation))
                            {
                                strHTML = queueSession.Get(_session.RedirectLocation);
                            }


                            if (!String.IsNullOrEmpty(queueSession.RedirectLocation))
                            {
                                strHTML = queueSession.Get(queueSession.RedirectLocation);
                            }
                            else
                            {
                                lastLayoutVersion = ExtractLastLayoutVersion(strHTML);

                                lastLayoutName = ExtractLastLayoutName(strHTML);

                                String queueID = String.Empty, eventID = String.Empty, customerID = String.Empty, cid = String.Empty, i = String.Empty;

                                String queueURL = queueSession.HTMLWeb.ResponseUri.Query.Substring(1);

                                String[] values = queueURL.Split('&');

                                foreach (String item in values)
                                {
                                    try
                                    {
                                        if (item.Split('=')[0].Equals("cid"))
                                        {
                                            cid = item.Split('=')[1];
                                        }
                                        else if (item.Split('=')[0].Equals("q"))
                                        {
                                            queueID = item.Split('=')[1];
                                        }
                                        else if (item.Split('=')[0].Equals("c"))
                                        {
                                            customerID = item.Split('=')[1];
                                        }
                                        else if (item.Split('=')[0].Equals("e"))
                                        {
                                            eventID = item.Split('=')[1];
                                        }
                                        else if (item.Split('?')[1].Contains("c="))
                                        {
                                            customerID = item.Split('?')[1].Split('=')[1];
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                    }
                                }

                                string redirectUrl = string.Empty;

                                this.MoreInfo += " Your queue Id is:" + queueID;

                                this.changeStatus(this.Status);

                                strHTML = queueSession.Get(queueSession.HTMLWeb.ResponseUri.AbsoluteUri);

                                if (queueSession.HTMLWeb.ResponseUri.AbsoluteUri.Contains("tix.axs") || queueSession.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("tickets.axs") || !queueSession.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("home.aspx"))
                                {
                                    this.Ticket.URL = queueSession.HTMLWeb.ResponseUri.AbsoluteUri;
                                }
                                else if (!queueSession.RedirectLocation.ToLower().Contains("tix.axs") && !queueSession.RedirectLocation.ToLower().Contains("tickets.axs") && !queueSession.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("home.aspx"))
                                {
                                    string isBeforeOrIdle = "True";

                                    string isclientRedayToRedirect = "(null)";

                                    do
                                    {
                                        try
                                        {
                                            string url = string.Empty;

                                            if (strURL.Contains("veritix.queue"))
                                            {
                                                url = queueSession.HTMLWeb.ResponseUri.Scheme + "://" + queueSession.HTMLWeb.ResponseUri.Host + "/v2queue/" + customerID + "/" + eventID + "/" + queueID + "/GetStatusV2?cid=" + cid + "&l=" + lastLayoutName;
                                            }
                                            else
                                            {
                                                url = this.Session.HTMLWeb.ResponseUri.Scheme + "://" + this.Session.HTMLWeb.ResponseUri.Host + "/queue/" + customerID + "/" + eventID + "/" + queueID + "/GetStatus?cid=" + cid + "&l=" + lastLayoutName;
                                            }
                                            String PostData = "{\"targetUrl\":\"\",\"customUrlParams\":\"\",\"layoutVersion\":" + lastLayoutVersion + ",\"layoutName\":\"" + lastLayoutName + "\",\"isClientRedayToRedirect\":\"" + isclientRedayToRedirect + "\",\"isBeforeOrIdle\":\"" + isBeforeOrIdle + "\"}";

                                            queueSession.Payload = PostData;

                                            queueSession._IfJSOn = true;

                                            String html = queueSession.Post(url);

                                            queueSession._IfJSOn = false;

                                            object json = null;

                                            JObject _json = null;

                                            try
                                            {
                                                json = JsonConvert.DeserializeObject(html);
                                            }

                                            catch (JsonReaderException ex)
                                            {
                                                try
                                                {
                                                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);

                                                    html = ParseHtml(html);

                                                    json = JsonConvert.DeserializeObject(html);

                                                    isclientRedayToRedirect = "True";

                                                    isBeforeOrIdle = "False";
                                                }
                                                catch (Exception e)
                                                {
                                                    Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);

                                                    isclientRedayToRedirect = "True";

                                                    isBeforeOrIdle = "False";
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                            }

                                            if (json != null)
                                            {
                                                _json = (JObject)json;

                                                if (_json["redirectUrl"] != null)
                                                {
                                                    redirectUrl = _json["redirectUrl"].ToString();
                                                }
                                                else if (_json["ticket"]["secondsToStart"] != null)
                                                {
                                                    if (!String.IsNullOrEmpty(_json["ticket"]["secondsToStart"].ToString()))
                                                    {
                                                        try
                                                        {
                                                            this.Status = "Queue will start in " + _json["ticket"]["secondsToStart"] + " seconds";

                                                            this.changeStatus(this.Status);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                                        }
                                                    }
                                                    else if (_json["ticket"]["queueNumber"] != null)
                                                    {
                                                        try
                                                        {
                                                            this.Status = "Your queue number is " + _json["ticket"]["queueNumber"];

                                                            isclientRedayToRedirect = "True";

                                                            isBeforeOrIdle = "False";

                                                            this.changeStatus(this.Status);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                                        }
                                                    }
                                                }
                                                else if (_json["ticket"]["queueNumber"] != null)
                                                {
                                                    try
                                                    {
                                                        this.Status = "Your queue number is " + _json["ticket"]["queueNumber"];

                                                        isclientRedayToRedirect = "True";

                                                        isBeforeOrIdle = "False";


                                                        this.changeStatus(this.Status);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                                    }
                                                }

                                                try
                                                {
                                                    if (String.IsNullOrEmpty(redirectUrl))
                                                    {
                                                        isBeforeOrIdle = _json["isBeforeOrIdle"].ToString();

                                                        if (isBeforeOrIdle.Equals("False"))
                                                        {
                                                            isclientRedayToRedirect = "True";
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                                }
                                            }
                                            else
                                            {
                                                isclientRedayToRedirect = "True";

                                                isBeforeOrIdle = "False";
                                            }

                                            queueSession._IfJSOn = false;

                                            if (!this.IfWorking || !this.Ticket.isRunning)
                                            {
                                                break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                            break;
                                        }

                                        if (String.IsNullOrEmpty(redirectUrl))
                                        {
                                            Thread.Sleep(10000);
                                        }

                                    } while (String.IsNullOrEmpty(redirectUrl));
                                }

                                if (!String.IsNullOrEmpty(redirectUrl))
                                {
                                    this.Ticket.URL = redirectUrl;
                                }
                            }
                        }
                        else
                        {
                            this.Ticket.URL = queueSession.RedirectLocation;
                        }
                    }
                    #endregion

                    if (this.Ticket.URL.Contains("home.aspx"))
                    {
                        if (this.appStartup.GlobalSetting.ifVeritix)
                        {
                            this.search = new VSSearch();
                            this.search.workerThread(); 
                        }
                        else
                        {
                            this.MoreInfo = "Veritix searches are not allowed.";
                            changeStatus("Not Allowed");
                        }
                    }
                    else if (new Uri(this.Ticket.URL).Host.Contains("evenko.ca") || new Uri(this.Ticket.URL).Host.Contains("axs") || new Uri(this.Ticket.URL).Host.Contains("tix.axs"))
                    {
                        if (this.appStartup.GlobalSetting.ifAXS)
                        {
                            this.search = new AXSSearch();
                            this.search.workerThread(); 
                        }
                        else
                        {
                            this.MoreInfo = "AXS searches are not allowed.";
                            changeStatus("Not Allowed");
                        }
                    }

                    if (this._proxy != null)
                    {
                        if (this._proxy.TheProxyType != Core.Proxy.ProxyType.Relay)
                        {
                            if (ProxyPicker.ProxyPickerInstance.ProxyManager != null)
                            {
                                ProxyPicker.ProxyPickerInstance.ProxyManager.ReleaseProxy(this._proxy);
                            }
                        }
                        else
                        {
                            if (ProxyPicker.ProxyPickerInstance.ProxyRelayManager != null)
                            {
                                #region Release Session from Server

                                ClearSessionFromServer();

                                #endregion

                                ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                            }
                        }
                        this._proxy = null;
                    }

                    this._proxy = null;
                    _tmEvent = null;

                    if (!this.IfWorking)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            try
            {
                if (_md5 != null)
                {
                    _md5.Clear();
                    GC.SuppressFinalize(_md5);
                }

                if (this._session != null)
                {
                    this._session.Dispose();
                    GC.SuppressFinalize(this._session);
                }

                if (this.search != null)
                {
                    this.search.Dispose();
                }

                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {

            }
        }
    }
}