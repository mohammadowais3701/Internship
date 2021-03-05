using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Threading;
using System.Net;
using System.IO;
using System.Drawing;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.IO.Compression;
using System.Net.Sockets;
using Newtonsoft.Json;
using TCPClient;
using System.Threading.Tasks;
using CaptchaFactory;

namespace Automatick.Core
{
    [Serializable]
    public class VSSearch : SearchBase
    {
        public AutoResetEvent captchaload = new AutoResetEvent(false);
        Cookie AYAH_Cookie = null;

        public CancellationTokenSource cancToken = new CancellationTokenSource();

        public String AVS_CAPTCHA_KEY = "6Lfq-DcUAAAAAB6ONN3R_TL3NP4-8sYIlkNVEJ7n";//"6LewTf8SAAAAAGcyTzf1kbZwfPvJ30KJGycl93ua";
        public const string context = "avsus";
        public static object locker = new object();

        public Boolean AxsAccountRequired
        {
            get;
            set;
        }

        TicketsLog currLog = null;

        #region Variables
        private BrowserSession _session = null;
        private MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();
        public ITicketParameter _CurrentParameter = null;
        Boolean _ifStopping = false;
        VSTicketAccount _selectedAccountForAutoBuy = null;
        VSEvent _tmEvent = null;
        Proxy _proxy = null;

        public VSMultiplePresaleCode presaleCode
        {
            get;
            set;
        }

        int _indexOfLastSelectedAccount = 0;
        Boolean _ifManualBuyProcessToFinalPage = false;
        frmSelectDeliveryOption _frmSelectDO = null;
        frmSelectAccount _frmSelectAccount = null;
        AutoResetEvent _sleep = null;
        IAutoCaptchaService solveAutoCaptcha = null;
        System.Threading.Timer timeoutStopping = null;
        private bool IncreaseTimer = false;
        private string PostData;
        public VSEvent TmEvent
        {
            get { return _tmEvent; }
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
        public Stopping stopping
        {
            get;
            set;
        }
        public BrowserSession Session
        {
            get { return _session; }
        }

        public Captcha Captcha
        {
            get;
            set;
        }

        public ITicket Ticket
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
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

        public String PresaleCode
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

        public Boolean IfNewCard
        {
            get;
            set;
        }

        public String SolvedCaptchaResponse
        {
            get;
            set;
        }
        public String LastURLForManualBuy
        {
            get;
            set;
        }

        public int SearchCycle
        {
            get;
            set;
        }

        public FormElementCollection LastFormElementsToPost
        {
            get;
            set;
        }

        public void start()
        {
            _sleep = new AutoResetEvent(false);
            this.resetAfterStop = null;
            this.IfWorking = true;
            this.MoreInfo = "";
            Thread th = new Thread(this.workerThread);
            th.Priority = ThreadPriority.Lowest;
            th.SetApartmentState(ApartmentState.MTA);
            th.IsBackground = true;
            th.Start();
        }

        void stoppingHandlerTimeout(Object o)
        {
            if (this._ifStopping)
            {
                this.stoppingHandler();
            }
        }
        public void stop()
        {

            try
            {

                if (this._ifStopping)
                {
                    return;
                }
                try
                {
                    if (cancToken != null)
                    {
                        cancToken.Cancel();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }

                TimerCallback tcbTimeoutStopping = new TimerCallback(this.stoppingHandlerTimeout);
                timeoutStopping = new Timer(tcbTimeoutStopping, this, new TimeSpan(0, 0, 15), new TimeSpan(-1));

                this.TimeLeft = "";
                this.Section = "";
                this.Row = "";
                this.Seat = "";
                this.Price = "";
                this.Quantity = "";
                this.TimeLeft = "";
                this.PresaleCode = "";
                this.Description = "";
                //changeStatus(TicketSearchStatus.StopStatus);
                changeStatus(TicketSearchStatus.StoppingStatus);
                this.IfWorking = false;

                this._ifStopping = true;
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

                VSTicket ticket = (VSTicket)this.Ticket;
                if (ticket != null)
                {
                    if (ticket.CaptchaQueue != null)
                    {
                        lock (ticket.CaptchaQueue)
                        {
                            ticket.CaptchaQueue.Remove(this);
                        }
                    }
                }

                if (this.Captcha != null)
                {
                    this.Captcha.captchaentered.Set();
                }

                try
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
                            ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                            this.ClearSessionFromServer();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
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

                if (_sleep != null)
                {
                    _sleep.Set();
                }

                if (solveAutoCaptcha != null)
                {
                    solveAutoCaptcha.abort();
                }

            }
            catch { }
            finally
            {
                GC.SuppressFinalize(this);
            }
            GC.Collect();

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
            thReset.Priority = ThreadPriority.Lowest;
            thReset.IsBackground = true;
            thReset.Start();
        }
        void restartThread()
        {
            try
            {
                if (this.IfWorking)
                {
                    this.resetAfterStop = new ResetAfterStop(this.resetAfterStopHandler);
                    this.stop();
                    //changeStatus(TicketSearchStatus.RestartingStatus);
                }
                else if (!this.IfWorking && !this._ifStopping)
                {
                    this.start();
                }
            }
            catch { }
        }

        private void sleep(int time)
        {
            try
            {
                if (_sleep == null)
                {
                    _sleep = new AutoResetEvent(false);
                }

                if (_sleep != null)
                {
                    _sleep.WaitOne(time);
                }
            }
            catch (Exception)
            {

            }
        }

        void retrying()
        {
            try
            {
                if (this.IfWorking && this.Ticket.isRunning)
                {
                    this.IfFound = false;
                    this.TimeLeft = "";
                    this.Section = "";
                    this.Row = "";
                    this.Seat = "";
                    this.Price = "";
                    this.Quantity = "";
                    this.PresaleCode = "";
                    this.TimeLeft = "";
                    this.Description = "";

                    //if (this.IfUseProxy)
                    //{
                    //    if (this.SearchCycle < this.Ticket.SwitchProxiesFrom)
                    //    {
                    //        this.SearchCycle++;
                    //    }
                    //}

                    try
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
                                ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                                this.ClearSessionFromServer();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }

                    int delay = (int)this.Ticket.ResetSearchDelay;
                    if (this.Ticket.ifRandomDelay)
                    {
                        Random rnd = new Random(0);
                        delay = rnd.Next(0, delay);
                    }

                    // delay++;

                    TimeSpan ts = new TimeSpan(0, 0, delay);

                    if (this.Status == TicketSearchStatus.SearchingStatus)
                    {
                        this.TimeLeft = "";
                    }
                    else
                    {
                        // this.TimeLeft = String.Format("{0:mm:ss}", ts).Remove(0, 3);
                        this.TimeLeft = String.Format("{0}:{1}", System.Math.Truncate(ts.TotalMinutes).ToString(), System.Math.Truncate(Convert.ToDouble(ts.Seconds)).ToString());
                    }

                    if (this.Status == TicketSearchStatus.SearchingStatus)
                    {
                        this.TimeLeft = "";
                        this.MoreInfo = "";
                        changeStatus(TicketSearchStatus.SearchingStatus);
                    }
                    else
                    {
                        this.TimeLeft = String.Format("{0:mm:ss}", ts.ToString()).Remove(0, 3);
                        this.MoreInfo = "";
                        changeStatus(TicketSearchStatus.RetryingStatus);
                    }

                    while (ts.TotalSeconds > 0 && this.IfWorking && this.Ticket.isRunning)
                    {
                        ts = ts.Subtract(new TimeSpan(0, 0, 1));
                        //ystem.Threading.Thread.Sleep(1000);
                        sleep(1000);

                        if (this.Status == TicketSearchStatus.SearchingStatus)
                        {
                            this.TimeLeft = "";
                            changeStatus(TicketSearchStatus.SearchingStatus);
                        }
                        else
                        {
                            this.TimeLeft = String.Format("{0:mm:ss}", ts.ToString()).Remove(0, 3);
                            changeStatus(TicketSearchStatus.RetryingStatus);
                        }
                    }
                    this.TimeLeft = "";
                    //System.Threading.Thread.Sleep(1000);
                    sleep(1000);

                }
            }
            catch
            {
            }
        }

        public override async void workerThread()
        {
            while (this.IfWorking && this.Ticket.isRunning)
            {
                this.IfFound = false;
                this.IfResetSearch = false;
                this.IfAutoBuy = false;
                this.TimeLeft = "";
                this.Section = "";
                this.Row = "";
                this.Seat = "";
                this.Price = "";
                this.Quantity = "";
                this.TimeLeft = "";
                this.PresaleCode = "";
                this.Description = "";
                this.FlagImage = global::Automatick.Properties.Resources.Flag16Disable;
                this.FlagImage.Tag = false;
                this._ifManualBuyProcessToFinalPage = false;
                this.currLog = new TicketsLog();
                this.currLog.TicketName = this.Ticket.TicketName;

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
                catch (Exception)
                {

                }

                //if (this.Ticket.ifPresale)
                //{
                //    try
                //    {
                //        presaleCode = VSMultiplePresaleCode.GetNextPresale(this.Ticket.PresaleCodes);
                //        this.PresaleCode = presaleCode.PresaleCode;
                //    }
                //    catch (Exception)
                //    {

                //    }
                //}

                if ((this.IfWorking && this.Ticket.isRunning) ? this.processFirstPage() : false)
                {
                    if ((this.IfWorking && this.Ticket.isRunning) ? await this.processCaptchaPage() : false)
                    {
                        solveAutoCaptcha = null;
                        if ((this.IfWorking && this.Ticket.isRunning) ? this.processFoundPage() : false)
                        {
                            //if ((this.IfWorking && this.Ticket.isRunning) ? this.processDeliveryPage() : false)
                            {
                                // if ((this.IfWorking && this.Ticket.isRunning) ? this.processCreateSignInPage() : false)
                                {
                                    if ((this.IfWorking && this.Ticket.isRunning) ? this.processSignInPage() : false)
                                    {
                                        if ((this.IfWorking && this.Ticket.isRunning) ? this.processUpdateSecurityQuestion() : false)
                                        {
                                            if ((this.IfWorking && this.Ticket.isRunning) ? this.processPaymentPage() : false)
                                            {
                                                if (this.IfWorking && this.Ticket.isRunning)
                                                {
                                                    this.processVerificationPage();
                                                }

                                                if (this.IfWorking && this.Ticket.isRunning)
                                                {
                                                    this.processConfirmationPage();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    solveAutoCaptcha = null;
                }

                if (this.IfUseProxy)
                {
                    if (this.SearchCycle < (this.Ticket.SwitchProxiesFrom - 1))
                    {
                        this.SearchCycle++;
                    }
                }

                sleep(2000);

                if (this._selectedAccountForAutoBuy != null)
                {
                    ReleaseAccount(this._selectedAccountForAutoBuy.AccountEmail);
                }

                if (this._session != null && !this.Ticket.ifPesistSessionInEachSearch)
                {
                    try
                    {
                        this._session.Dispose();
                        GC.SuppressFinalize(this._session);
                        this._session = null;
                    }
                    catch { }
                }

                if (this.Ticket.ifPresale)
                {
                    VSMultiplePresaleCode.ReleasePresaleCode(this.Ticket.PresaleCodes, this.PresaleCode);
                }

                if (this._tmEvent != null)
                {
                    this._tmEvent.Dispose();
                    this._tmEvent = null;
                }

                retrying();

                if (!this.IfWorking)
                {
                    break;
                }
            }

            try
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
                        ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                        this.ClearSessionFromServer();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }


            if (this.stopping != null)
            {
                this.stopping();
            }


            //_sleep.Close();
        }

        public void autoBuy(Boolean newCard = false)
        {
            try
            {
                if (this.IfWorking && this.IfFound && this.Ticket.isRunning)
                {
                    this.currLog.BuyStatus = TicketsLog.AutoBuyStatus;
                    this.currLog.Account = (this._selectedAccountForAutoBuy != null) ? this._selectedAccountForAutoBuy.AccountEmail : "";
                    this.Ticket.tic_Logs.Add(this.currLog);
                    this.IfAutoBuy = true;
                    this.IfNewCard = newCard;
                }
            }
            catch (Exception)
            {

            }
        }

        public void autoBuyWithoutProxy(Boolean newCard = false)
        {
            try
            {
                try
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
                            ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                            this.ClearSessionFromServer();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            catch (Exception)
            {

            }

            this._proxy = null;
            this._session.Proxy = null;
            this.autoBuy(newCard);
        }

        private void openManualTicketBrowser()
        {
            try
            {

                if (this.IfWorking && this.IfFound && this.Ticket.isRunning)//&& !this.IfAutoBuy)
                {
                    PostData = string.Empty;
                    String dataFileName = "ManualFile" + DateTime.Now.ToString("yyMMddhhmmssfffff") + ".txt";
                    using (StreamWriter writer = new StreamWriter(this.Ticket.FileLocation + @"\" + dataFileName))
                    {

                        if (String.IsNullOrEmpty(this.LastURLForManualBuy))
                        {
                            if (this._session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("deliverymethod"))
                            {
                                this.LastURLForManualBuy = this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("DeliveryMethod", "SignIn");
                            }
                        }

                        this.currLog.BuyStatus = TicketsLog.ManualBuyStatus;
                        this.currLog.Account = (this._selectedAccountForAutoBuy != null) ? this._selectedAccountForAutoBuy.AccountEmail : "";
                        if (!this.Ticket.tic_Logs.Contains(this.currLog))
                        {
                            this.Ticket.tic_Logs.Add(this.currLog);
                        }

                        writer.WriteLine(this.LastURLForManualBuy);
                        writer.WriteLine(this.PostData);
                        if (this._proxy != null)
                        {
                            //writer.WriteLine(this._session.Proxy.Address.Host + ":" + this._session.Proxy.Address.Port);
                            writer.WriteLine(this._proxy.ToStringForManualBuy());
                        }
                        else
                        {
                            writer.WriteLine(String.Empty);
                        }



                        Dictionary<String, String> dicCookies = this._session.GetCookiesForManualBrowsing();

                        if (dicCookies != null)
                        {
                            foreach (KeyValuePair<String, String> cookie in dicCookies)
                            {
                                writer.WriteLine(cookie.Key.Trim());
                                writer.WriteLine(cookie.Value);
                            }
                        }
                    }

                    Process process = new Process();
                    process.StartInfo.Arguments = dataFileName;
                    process.StartInfo.FileName = this.Ticket.FileLocation + @"\TicketBrowser.exe";
                    process.StartInfo.WorkingDirectory = this.Ticket.FileLocation;
                    process.EnableRaisingEvents = true;
                    process.Start();

                    lock (this.Ticket)
                    {
                        this.Ticket.BuyCount++;
                        if (_selectedAccountForAutoBuy != null)
                        {
                            if (this.Ticket.BuyHistory.ContainsKey(_selectedAccountForAutoBuy.AccountEmail))
                            {
                                this.Ticket.BuyHistory[_selectedAccountForAutoBuy.AccountEmail] += 1;
                                this.Ticket.SaveTicket();
                            }
                            else
                            {
                                this.Ticket.BuyHistory.Add(_selectedAccountForAutoBuy.AccountEmail, 1);
                                this.Ticket.SaveTicket();
                            }
                        }
                    }

                    if (this._CurrentParameter != null)
                    {
                        if (this._CurrentParameter.Bought == null)
                        {
                            this._CurrentParameter.Bought = 0;
                        }
                        this._CurrentParameter.Bought++;
                    }

                    this.restart();

                    this.Ticket.SaveTicket();
                }
            }
            catch (Exception)
            {

            }
        }

        public void manualBuy()
        {
            if (this.Ticket.ifProcessToFinalPage)
            {
                _ifManualBuyProcessToFinalPage = true;
                this.autoBuy();
            }
            else
            {
                openManualTicketBrowser();
            }
        }

        public void manualBuyWithoutProxy()
        {
            if (this.Ticket.ifProcessToFinalPage)
            {
                _ifManualBuyProcessToFinalPage = true;
                this.autoBuyWithoutProxy();
            }
            else
            {
                try
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
                            ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                            this.ClearSessionFromServer();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }

                this._proxy = null;
                this._session.Proxy = null;
                openManualTicketBrowser();
            }
        }

        protected Boolean processFirstPage()
        {
            Boolean result = false;
            String strHTML = String.Empty;
            String lastLayoutVersion = String.Empty;
            String lastLayoutName = String.Empty;


            try
            {
                if (this._CurrentParameter == null)
                {
                    _CurrentParameter = this.getNextParameter();
                }

                //Switch to next parameter if max bought exceeds
                if (this._CurrentParameter != null)
                {
                    if (this._CurrentParameter.MaxBought != null)
                    {
                        if (this._CurrentParameter.Bought >= this._CurrentParameter.MaxBought && this._CurrentParameter.MaxBought > 0)
                        {
                            this._CurrentParameter = null;
                            return false;
                        }
                    }
                }

                if (this._CurrentParameter != null)
                {
                    this.MoreInfo = "Searching quantity:" + this._CurrentParameter.Quantity;
                    if (this._CurrentParameter.PriceMin != null && this._CurrentParameter.PriceMax != null)
                    {
                        this.MoreInfo += ", price:" + this._CurrentParameter.PriceMin.ToString() + " - " + this._CurrentParameter.PriceMax.ToString();
                    }
                    if (!String.IsNullOrEmpty(this._CurrentParameter.TicketTypeString))
                    {
                        this.MoreInfo += ", ticket type:" + this._CurrentParameter.TicketTypeString;
                    }
                    if (!String.IsNullOrEmpty(this._CurrentParameter.TicketTypePasssword))
                    {
                        this.MoreInfo += ", password:" + this._CurrentParameter.TicketTypePasssword;
                    }
                }

                changeStatus(TicketSearchStatus.SearchingStatus);

                String strURL = this.Ticket.URL;

                if (this.Ticket.ifPesistSessionInEachSearch && this._session != null)
                {
                    if (this._proxy == null && this.Ticket.ifUseProxies && this.IfUseProxy)
                    {
                        if (this.SearchCycle >= (this.Ticket.SwitchProxiesFrom - 1))
                        {
                            _proxy = ProxyPicker.ProxyPickerInstance.getNextProxy(this);
                        }
                    }
                    //Thread.Sleep(1000);
                    sleep(1000);

                }
                else
                {
                    // this._proxy = null;
                    changeStatus(TicketSearchStatus.SearchingStatus);
                    //Proxy
                    if (this.Ticket.ifUseProxies && this.IfUseProxy)
                    {
                        if (this.SearchCycle >= (this.Ticket.SwitchProxiesFrom - 1))
                        {
                            _proxy = ProxyPicker.ProxyPickerInstance.getNextProxy(this);
                        }
                    }

                    this._session = new BrowserSession();
                }

                changeStatus(TicketSearchStatus.SearchingStatus);

                if (this.Ticket.onStartSearching != null)
                {
                    this.Ticket.onStartSearching(this.Ticket);
                }

                this._selectedAccountForAutoBuy = null;

                //Proxy

                if (_proxy != null)
                {
                    if (_proxy.IfLuminatiProxy)
                    {
                        //if (_authservicedevicetoken.Proxy != null)
                        //{
                        //    _proxy = _authservicedevicetoken.Proxy;
                        //}

                        _proxy.setSessionIdInBrowserSession(this._session);
                    }

                    else if (_proxy.TheProxyType == Core.Proxy.ProxyType.Relay)
                    {
                        _proxy.setSessionIdInBrowserSession(this._session);
                    }

                    this._session.Proxy = _proxy.toWebProxy(context);

                    //this._session.luminatiSessionId = this._proxy.LuminatiSessionId;
                    //this._session.Proxy = _proxy.toWebProxy();
                }
                else
                {
                    this._session.Proxy = null;
                }

                //TODO: call getnextpresale here

                if (this.Ticket.ifPresale)
                {
                    try
                    {
                        //VSMultiplePresaleCode.ReleasePresaleCode(this.Ticket.PresaleCodes, this.PresaleCode);
                        //presaleCode = VSMultiplePresaleCode.GetNextPresale(this.Ticket.PresaleCodes);
                        this.PresaleCode = presaleCode.PresaleCode;
                    }
                    catch (Exception)
                    {

                    }
                }

                if (ProxyPicker.ProxyPickerInstance.ifSearchAllowed(this))
                {
                    //https://axseu.queue-it.net?c=axseu&e=o2l181127ven
                    if (!strURL.Contains(".queue") && !strURL.Contains("q.axs.co.uk"))
                    {
                        strHTML = this._session.Get(strURL);
                        Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                    }
                    else
                    {
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

                        strHTML = this._session.Get(strURL);

                        Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                        if (!this._session.LocationURL.ToLower().Contains("home.aspx"))
                        {
                            if (this._session.LocationURL.Contains(".queue") || strURL.Contains("q.axs.co.uk"))
                            {
                                strHTML = this._session.Get(this._session.LocationURL);
                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            }

                            if (!String.IsNullOrEmpty(this._session.LocationURL))
                            {
                                strHTML = this._session.Get(this._session.LocationURL);
                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            }


                            if (!String.IsNullOrEmpty(this._session.LocationURL))
                            {
                                strHTML = this._session.Get(this._session.LocationURL);
                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            }
                            else
                            {


                                lastLayoutVersion = ExtractLastLayoutVersion(strHTML);

                                lastLayoutName = ExtractLastLayoutName(strHTML);

                                String queueID = String.Empty, eventID = String.Empty, customerID = String.Empty, cid = String.Empty, i = String.Empty;

                                String queueURL = this.Session.HTMLWeb.ResponseUri.Query.Substring(1);

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

                                strHTML = this._session.Get(this.Session.HTMLWeb.ResponseUri.AbsoluteUri);

                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                                if (!this._session.LocationURL.ToLower().Contains("home.aspx"))
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
                                                url = this.Session.HTMLWeb.ResponseUri.Scheme + "://" + this.Session.HTMLWeb.ResponseUri.Host + "/v2queue/" + customerID + "/" + eventID + "/" + queueID + "/GetStatusV2?cid=" + cid + "&l=" + lastLayoutName;
                                            }
                                            else
                                            {
                                                url = this.Session.HTMLWeb.ResponseUri.Scheme + "://" + this.Session.HTMLWeb.ResponseUri.Host + "/queue/" + customerID + "/" + eventID + "/" + queueID + "/GetStatus?cid=" + cid + "&l=" + lastLayoutName;
                                            }
                                            PostData = "{\"targetUrl\":\"\",\"customUrlParams\":\"\",\"layoutVersion\":" + lastLayoutVersion + ",\"layoutName\":\"" + lastLayoutName + "\",\"isClientRedayToRedirect\":\"" + isclientRedayToRedirect + "\",\"isBeforeOrIdle\":\"" + isBeforeOrIdle + "\"}";

                                            this.Session.HTMLWeb.PostData = PostData;

                                            this.Session._IfJSOn = true;

                                            String html = this.Session.Post(url);

                                            //html = File.ReadAllText("json.txt");

                                            this.Session._IfJSOn = false;

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

                                            this.Session._IfJSOn = false;

                                            if (!this.IfWorking || !this.Ticket.isRunning)
                                            {
                                                return false;
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
                                    this.Session.Get(redirectUrl);

                                    Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                                }

                                #region CommentOut

                                //if (!this.Session.LocationURL.ToLower().Contains("home.aspx"))
                                //{
                                //    string locationURL = string.Empty;

                                //    string url = this.Session.HTMLWeb.ResponseUri.Scheme + "://" + this.Session.HTMLWeb.ResponseUri.Host + "/queue/" + customerID + "/" + eventID + "/" + queueID + "/getforecast?cid=" + cid + "&l=" + lastLayoutName;

                                //    do
                                //    {
                                //        Thread.Sleep(2000);

                                //        JObject _json = null;

                                //        this.Session._IfJSOn = true;

                                //        PostData = "{\"targetUrl\":\"\",\"layoutVersion\":" + lastLayoutVersion + ",\"layoutName\":\"" + lastLayoutName + "\",\"isClientRedayToRedirect\":true}";

                                //        this.Session.HTMLWeb.PostData = PostData;

                                //        string html = this.Session.Post(url);

                                //        this.Session._IfJSOn = false;

                                //        try
                                //        {
                                //            try
                                //            {
                                //                _json = JObject.Parse(html);
                                //            }
                                //            catch (Exception)
                                //            {
                                //            }

                                //            if (_json != null)
                                //            {
                                //                if (_json["redirectUrl"] != null)
                                //                {
                                //                    if (_json["redirectUrl"].ToString().ToLower().Contains("home.aspx"))
                                //                    {
                                //                        locationURL = _json["redirectUrl"].ToString(); ;
                                //                    }
                                //                }
                                //                else if (_json["ticket"]["queueNumber"] != null)
                                //                {
                                //                    this.Status = "Your queue number is " + _json["ticket"]["queueNumber"];

                                //                    this.changeStatus(this.Status);
                                //                }

                                //            }

                                //        }
                                //        catch
                                //        { }

                                //        if (!this.IfWorking || !this.Ticket.isRunning)
                                //        {
                                //            return false;
                                //        }
                                //    }
                                //    while (String.IsNullOrEmpty(locationURL));

                                //    if (!String.IsNullOrEmpty(locationURL))
                                //    {
                                //        this.Session.Get(locationURL);
                                //    }

                                //}


                                //}
                                //else
                                //{
                                //    strHTML = this.Session.Get(this.Session.HTMLWeb.ResponseUri.AbsoluteUri);

                                //    if (!this.Session.LocationURL.ToLower().Contains("home.aspx"))
                                //    {
                                //        string locationURL = string.Empty;

                                //        string url = this.Session.HTMLWeb.ResponseUri.Scheme + "://" + this.Session.HTMLWeb.ResponseUri.Host + "/queue/" + customerID + "/" + eventID + "/" + queueID + "/getforecast?cid=" + cid + "&l=" + lastLayoutName;

                                //        do
                                //        {
                                //            Thread.Sleep(2000);

                                //            JObject _json = null;

                                //            this.Session._IfJSOn = true;

                                //            PostData = "{\"targetUrl\":\"\",\"layoutVersion\":" + lastLayoutVersion + ",\"layoutName\":\"" + lastLayoutName + "\",\"isClientRedayToRedirect\":true}";

                                //            this.Session.HTMLWeb.PostData = PostData;

                                //            string html = this.Session.Post(url);

                                //            this.Session._IfJSOn = false;

                                //            try
                                //            {
                                //                Match m = Regex.Match(html, "\"redirectUrl\":(.*?),", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                                //                if (m.Success)
                                //                {
                                //                    //string value = m.Value;
                                //                    try
                                //                    {
                                //                        _json = JObject.Parse(html);
                                //                    }
                                //                    catch (Exception)
                                //                    {
                                //                    }
                                //                }
                                //                if (_json != null)
                                //                {
                                //                    if (_json["redirectUrl"] != null)
                                //                    {
                                //                        if (_json["redirectUrl"].ToString().ToLower().Contains("home.aspx"))
                                //                        {
                                //                            locationURL = _json["redirectUrl"].ToString(); ;
                                //                        }
                                //                    }
                                //                    else if (_json["ticket"]["queueNumber"] != null)
                                //                    {
                                //                        this.Status = "Your queue number is:" + _json["ticket"]["queueNumber"];

                                //                        this.changeStatus(this.Status);
                                //                    }

                                //                }

                                //            }
                                //            catch
                                //            { }

                                //            if (!this.IfWorking || !this.Ticket.isRunning)
                                //            {
                                //                return false;
                                //            }
                                //        }
                                //        while (String.IsNullOrEmpty(locationURL));


                                //        if (!String.IsNullOrEmpty(locationURL))
                                //        {
                                //            this.Session.Get(locationURL);
                                //        }

                                //    }
                                //} 
                                #endregion
                            }
                        }
                    }

                    this._session._IfJSOn = false;

                    this._session.HTMLWeb.PostData = string.Empty;

                    if (this._session.LocationURL.ToLower().Contains("home.aspx"))
                    {
                        if (!String.IsNullOrEmpty(this._session.LocationURL))
                        {
                            strHTML = this._session.Get(this._session.LocationURL);
                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                        }

                        if (!String.IsNullOrEmpty(this._session.LocationURL))
                        {
                            strHTML = this._session.Get(this._session.LocationURL);
                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                        }
                        if (!String.IsNullOrEmpty(this._session.LocationURL))
                        {
                            strHTML = this._session.Get(this._session.LocationURL);
                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                        }

                        if (strHTML.Contains("c_lblAXSAccount1"))
                        {
                            this.AxsAccountRequired = true;

                            this._selectedAccountForAutoBuy = this.selectTicketAccount();

                            if (this._selectedAccountForAutoBuy != null)
                            {
                                AddUpdateField(this._session, "m:c:txtExistingUserId", this._selectedAccountForAutoBuy.AccountEmail);

                                AddUpdateField(this._session, "m:c:txtPassword", this._selectedAccountForAutoBuy.AccountPassword);

                                AddUpdateField(this._session, "m:c:btnContinue", "Continue");

                                strHTML = this._session.Post(this._session.LastURL);

                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                                if (!String.IsNullOrEmpty(this._session.RedirectLocation))
                                {
                                    this._session.Get(this._session.RedirectLocation);

                                    Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                                }

                                if (this._session.LocationURL.Contains("UpdateSecurityQuestion.aspx"))
                                {
                                    AddUpdateField(this._session, "m:c:btnSkip", "Skip This Time");

                                    strHTML = this.Session.Post(this._session.LocationURL);

                                    Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                                }

                                if (!String.IsNullOrEmpty(this._session.RedirectLocation))
                                {
                                    strHTML = this._session.Get(this._session.RedirectLocation);

                                    Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                                }
                                if (!String.IsNullOrEmpty(this._session.RedirectLocation))
                                {
                                    strHTML = this._session.Get(this._session.RedirectLocation);

                                    Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                                }
                                if (!String.IsNullOrEmpty(this._session.RedirectLocation))
                                {
                                    strHTML = this._session.Get(this._session.RedirectLocation);

                                    Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                                }
                            }
                            else
                            {
                                this.MoreInfo = TicketSearchStatus.MoreInfoAccountNotAvailable;
                                this.AxsAccountRequired = false;
                                result = false;
                                return result;
                            }
                        }
                        else
                        {
                            this.AxsAccountRequired = false;
                        }
                    }
                    else if (this._session.LocationURL.ToLower().Contains("queue-it"))
                    {
                        do
                        {
                            strHTML = this._session.Get(this._session.LocationURL);
                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            //Thread.Sleep(100);
                        }
                        while (!String.IsNullOrEmpty(this.Session.LocationURL));
                    }

                    if (!String.IsNullOrEmpty(this.Session.LocationURL))
                    {
                        strHTML = this._session.Get(this._session.LocationURL);
                        Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                    }

                    this._session.HTMLWeb.IfAllowAutoRedirect = true;

                    if (this._session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("restrictions.aspx"))
                    {
                        if (this._session.FormElements.Keys.Contains("m:c:txtPromo"))
                        {
                            if (presaleCode != null)
                            {
                                AddUpdateField(this._session, "m:c:txtPromo", presaleCode.PresaleCode);
                            }
                        }
                        else if (this._session.FormElements.Keys.Contains("m:c:txtPasscode"))
                        {
                            if (presaleCode != null)
                            {
                                AddUpdateField(this._session, "m:c:txtPasscode", presaleCode.PresaleCode);
                            }
                        }
                        else if (this._session.FormElements.Keys.Contains("m$c$txtPromo"))
                        {
                            if (presaleCode != null)
                            {
                                AddUpdateField(this._session, "m$c$txtPromo", presaleCode.PresaleCode);
                            }
                        }
                        else if (this._session.FormElements.Keys.Contains("m$c$txtPasscode"))
                        {
                            if (presaleCode != null)
                            {
                                AddUpdateField(this._session, "m$c$txtPasscode", presaleCode.PresaleCode);
                            }
                        }

                        else
                        {
                            return false;
                        }
                        strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                        if (this._session.FormElements.ContainsKey("m:c:txtPasscode"))
                        {
                            if (presaleCode != null)
                            {
                                AddUpdateField(this._session, "m:c:txtPasscode", presaleCode.PresaleCode);
                            }

                            strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                        }


                        if (this._session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("apperror.aspx"))
                        {
                            if (this.Session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@id='Table1']") != null)
                            {
                                this.MoreInfo = this.Session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@id='Table1']/tr/td").InnerText;
                            }
                            else if (this.Session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@id='Table1']/h4") != null)
                            {
                                this.MoreInfo = this.Session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@id='Table1']/h4").InnerText.Replace("\n", String.Empty);
                            }
                            //this.MoreInfo = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//table[contains(@id, 'Table1')]/tr/td").InnerText;
                            return false;
                        }

                    }

                    if (this._session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("pickasection.aspx"))
                    {

                        if (this._session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@id='AYAH']") != null)
                        {
                            //BrowserSession captchaSession = new BrowserSession();
                            //String token = "";
                            //if (this._session.HtmlDocument.DocumentNode.InnerHtml.Contains("$(function ()"))
                            //{
                            //    token = this._session.HtmlDocument.DocumentNode.InnerHtml;
                            //    token = token.Substring(token.LastIndexOf("var urls ="), 106);
                            //    token = token.Replace("var urls = ", String.Empty).Remove(0, 1);
                            //    string start = token.Remove(token.IndexOf('+')).Replace("'", String.Empty);
                            //    string end = token.Substring(token.IndexOf('?'), token.Length - token.IndexOf('?')).Replace("'+", String.Empty); ;
                            //    token = String.Concat(start, end).Replace(";", String.Empty);
                            //    //section.ParameterKey.Add("token", token);
                            //}

                            //String response = captchaSession.Get(token);
                            //AYAH_Cookie = captchaSession.Cookies["AYAH_Cookie"];
                        }

                        //if (this.Ticket.URL.Contains("ticketingcentral.com"))
                        //{
                        //    strHTML = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("PickASection", "Quantity"));
                        //}
                        //else
                        //{
                        strHTML = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                        Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                        // }

                        //strHTML = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("PickASection", "Quantity"));
                    }
                    //else if (this._session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("OfferInfo.aspx"))
                    //{
                    //    if (strHTML.ToLower().Contains("the offer is no longer on sale."))
                    //    {

                    //    }
                    //}

                    //ProxyPicker.ProxyPickerInstance.RecheckProxyStatus(this.Proxy, strHTML);

                    this._session._IfJSOn = false;

                    this._session.HTMLWeb.PostData = string.Empty;

                    if (this._session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("offerinfo.aspx"))
                    {
                        this.MoreInfo = "Ticket is not on-sale.";
                        return false;
                    }

                    processCaptchaPage();

                    changeStatus(TicketSearchStatus.FirstPageStatus);

                    //if (this._tmEvent == null || this._tmEvent.HasTicketTypes)
                    {
                        this._tmEvent = new VSEvent(this, (VSTicket)this.Ticket);
                    }
                    if (_tmEvent.IsExpired)
                    {
                        this.MoreInfo = String.Empty;
                        this.MoreInfo = TicketSearchStatus.MoreInfoTicketOutDated;
                    }
                    if (!this._tmEvent.TicketTypeMatched)
                    {
                        this.MoreInfo = "Ticket Type does not match.";
                        return false;
                    }
                    if (!this._tmEvent.IsBestAvailable)
                    {
                        if (!this._tmEvent.PriceLevelMatched)
                        {
                            if (this._tmEvent.NoSeatsAvailable)
                            {
                                this.MoreInfo = TicketSearchStatus.MoreInfoQuantityNotMatch + " " + this._CurrentParameter.Quantity + " is not valid quantity for this parameter.";
                            }
                            return false;
                        }
                        if (!this._tmEvent.SectionMatched)
                        {
                            //this.MoreInfo = "Price range does not match with the PriceLevel.";
                            return false;
                        }
                    }
                    if (_tmEvent.HasTicketTypes || this._tmEvent.IsBestAvailable)
                    {
                        lock (this.Ticket)
                        {
                            this.Ticket.RunCount++;
                        }

                        //changeStatus(TicketSearchStatus.FirstPageStatus);

                        //if (this.mapParameterIfAvaiable(_CurrentParameter))
                        {
                            if (!this.IfWorking || !this.Ticket.isRunning)
                            {
                                return false;
                            }

                            this._session._IfJSOn = false;
                            this._session.HTMLWeb.PostData = string.Empty;

                            _CurrentParameter.IfAvailable = true;

                            this.Session.HTMLWeb.IfAllowAutoRedirect = false;

                            strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                            if (!String.IsNullOrEmpty(this.Session.LocationURL))
                            {
                                this._session.HTMLWeb.Referer = this._session.LastURL;
                                strHTML = this._session.Get(this._session.LocationURL);
                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            }

                            HtmlNode errorNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@id='m_c_lblReserveSeatsError']");

                            if (errorNode != null)
                            {
                                this.MoreInfo = errorNode.InnerText.Trim();

                                this.changeStatus("Retrying..");
                                Thread.Sleep(3000);
                                result = false;
                            }
                            else
                            {
                                result = true;
                            }
                        }
                        if (!this.Session.HTMLWeb.ResponseUri.AbsolutePath.ToLower().Contains("reserveseats.aspx") && !this.Session.HTMLWeb.ResponseUri.AbsolutePath.ToLower().Contains("pickaseat.aspx") && !this.Session.HTMLWeb.ResponseUri.AbsolutePath.ToLower().Contains("deliverymethod.aspx"))
                        {
                            result = false;
                            if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
                            {
                                this.MoreInfo = TicketSearchStatus.MoreInfoParamterNotMatch;
                                _CurrentParameter = null;
                            }
                        }
                    }
                    else // If Price levels does not exist in the event first page then show message.
                    {
                        //this.MoreInfo = TicketSearchStatus.MoreInfoEventNotAvaiable;
                        //this.MoreInfo = "";
                        result = false;
                    }
                }
                else
                {
                    //this.MoreInfo = TicketSearchStatus.MoreInfoProxyNotAvaiable;
                    this.MoreInfo = "";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                //Note: do not uncomment. 
                //ProxyPicker.ProxyPickerInstance.RecheckProxyStatus(this.Proxy, ex.Message);
            }
            return result;
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

        //protected Boolean processCaptchaPageAjax()
        //{
        //    String strHTML;
        //    String strRecaptchaCreate, strChallenge = "", strServer, uRL;
        //    String code = "";
        //    String verify_psig;

        //    byte[] imgBytes;

        //    try
        //    {
        //        if (this.Ticket.ifUseSolveMedia && this._tmEvent.EventAjax.TMAJAXCaptchaObj.Recaptcha_type == "solve_media")
        //        {
        //            Boolean ifCaptchaFailed = false;
        //            do
        //            {
        //                if (!this.IfWorking || !this.Ticket.isRunning)
        //                {
        //                    return false;
        //                }

        //                WebProxy tmpProxy = this._session.Proxy;
        //                BrowserSession bsClone = this._session;

        //                if (this.Ticket.ifUseProxiesInCaptchaSource)
        //                {
        //                    bsClone.Proxy = this._session.Proxy;
        //                }
        //                else
        //                {
        //                    bsClone.Proxy = null;
        //                }

        //                if (!this.IfWorking || !this.Ticket.isRunning)
        //                {
        //                    return false;
        //                }

        //                /*************MAGIC************/
        //                //string html = Encoding.ASCII.GetString(wc.DownloadData("https://api-secure.solvemedia.com/papi/challenge.ajax"));
        //                string html = bsClone.Get("http://api.solvemedia.com/papi/challenge.ajax");
        //                string magic = html.Substring(html.IndexOf("magic:") + "magic:".Length, html.IndexOf(",", html.IndexOf("magic:") + "magic:".Length) - (html.IndexOf("magic:") + "magic:".Length));
        //                magic = magic.Replace("'", " ").Trim();
        //                /******************************/

        //                /***********chalstamp:*********/
        //                string ct = html.Substring(html.IndexOf("chalstamp:") + "chalstamp:".Length, html.IndexOf(",", html.IndexOf("chalstamp:") + "chalstamp:".Length) - (html.IndexOf("chalstamp:") + "chalstamp:".Length));
        //                ct = ct.Trim();
        //                /******************************/

        //                String extraParams = this._tmEvent.EventAjax.pageHints;

        //                var unixTime = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        //                string ts = unixTime.TotalSeconds.ToString().Substring(0, unixTime.TotalSeconds.ToString().IndexOf('.'));

        //                JObject obj = JObject.Parse(extraParams);

        //                JToken j = obj.First.First;

        //                string event_id = (string)j["evtid"];

        //                string artist = (string)j["artist"];
        //                if (artist == null)
        //                {
        //                    artist = "";
        //                }
        //                artist = artist.Replace(' ', '_');

        //                JArray ctg = (JArray)j["ctg"];

        //                List<string> ctgList = new List<string>();

        //                for (int i = 0; i < ctg.Count; i++)
        //                {
        //                    ctgList.Add(ctg[i].ToString().Replace('\"', ' ').Trim().Replace(' ', '_'));
        //                }

        //                string venue = (string)j["venue"];
        //                if (venue == null)
        //                {
        //                    venue = "";
        //                }

        //                venue = venue.Replace(' ', '_');

        //                string venueid = (string)j["venueid"];
        //                if (venueid == null)
        //                {
        //                    venueid = "";
        //                }

        //                string str2 = this._tmEvent.EventAjax.Key + ";f=_ACPuzzleUtil.callbacks%5B0%5D;l=en;t=img;s=300x150;c=js,swf11,swf11.7,swf,h5c,h5ct,svg,h5v,v/h264,v/ogg,v/webm,h5a,a/ogg,ua/firefox,ua/firefox21,os/nt,os/nt6.1,fwv/Nbw.Jg.nbmn3,jslib/jquery,jslib/jqueryui,jslib/proto;am=" + magic + ";ca=ajax;h=evtid:" + event_id + " artist:" + artist + " ";
        //                foreach (string tmp in ctgList)
        //                {
        //                    str2 += "ctg:" + tmp + " ";
        //                }

        //                if (!String.IsNullOrEmpty(venue))
        //                {
        //                    str2 += "venue:" + venue + ";ts=" + ts + ";ct=" + ct + ";th=custom;r=0.8632257031762814";
        //                }
        //                else if (!String.IsNullOrEmpty(venueid))
        //                {
        //                    str2 += "venueid:" + venueid + ";ts=" + ts + ";ct=" + ct + ";th=custom;r=0.8632257031762814";
        //                }

        //                str2 = str2.Replace("[", "%5B").Replace("]", "%5D").Replace(":", "%3A").Replace(" ", "%20");

        //                if (!this.IfWorking || !this.Ticket.isRunning)
        //                {
        //                    return false;
        //                }

        //                code = bsClone.Get("http://api.solvemedia.com/papi/_challenge.js?k=" + str2);

        //                string solveMediaChallenge = this.extractParameter(code, "\"chid\"");
        //                if (solveMediaChallenge == null)
        //                {
        //                    return false;
        //                }
        //                string imageURL = @"http://api.solvemedia.com/papi/media?c=";
        //                uRL = imageURL + solveMediaChallenge;

        //                if (!this.IfWorking || !this.Ticket.isRunning)
        //                {
        //                    return false;
        //                }

        //                imgBytes = bsClone.GetBinaryData(uRL);

        //                this._session.Proxy = tmpProxy;

        //                if (!this.IfWorking || !this.Ticket.isRunning)
        //                {
        //                    return false;
        //                }


        //                this.Captcha = new Captcha(imgBytes);

        //                if (this.Ticket.ifAutoCaptcha && this.IfUseAutoCaptcha)
        //                {
        //                    changeStatus(TicketSearchStatus.ResolvingCaptchaStatus);

        //                    if (this.Ticket.ifDBCAutoCaptcha)
        //                    {
        //                        solveAutoCaptcha = new DeathByCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }
        //                    else if (this.Ticket.ifRDAutoCaptcha)
        //                    {
        //                        solveAutoCaptcha = new RDCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }
        //                    else if (this.Ticket.ifCPTAutoCaptcha)
        //                    {
        //                        solveAutoCaptcha = new CPTCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }                            
        //                    else if (this.Ticket.ifDCAutoCaptcha)
        //                    {
        //                        solveAutoCaptcha = new DeCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }
        //                    else if (this.Ticket.ifOCR)
        //                    {
        //                        solveAutoCaptcha = new OCRService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }
        //                    else
        //                    {
        //                        // By Default use bypass auto captcha
        //                        solveAutoCaptcha = new BypassAutoCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }

        //                    if (this.IfWorking && this.Ticket.isRunning && solveAutoCaptcha != null)
        //                    {
        //                        solveAutoCaptcha.solve();
        //                    }
        //                    else
        //                    {
        //                        return false;
        //                    }

        //                    if (!String.IsNullOrEmpty(solveAutoCaptcha.CaptchaError))
        //                    {
        //                        //this.MoreInfo = TicketSearchStatus.MoreInfoAutoCaptchaError + solveAutoCaptcha.CaptchaError;
        //                    }
        //                    else
        //                    {
        //                        this.MoreInfo = "";
        //                        if (this._CurrentParameter != null)
        //                        {
        //                            this.MoreInfo = "Searching quantity:" + this._CurrentParameter.Quantity;
        //                            if (this._CurrentParameter.PriceMin != null && this._CurrentParameter.PriceMax != null)
        //                            {
        //                                this.MoreInfo += ", price:" + this._CurrentParameter.PriceMin.ToString() + " - " + this._CurrentParameter.PriceMax.ToString();
        //                            }
        //                            if (!String.IsNullOrEmpty(this._CurrentParameter.TicketTypeString))
        //                            {
        //                                this.MoreInfo += ", ticket type:" + this._CurrentParameter.TicketTypeString;
        //                            }
        //                            if (!String.IsNullOrEmpty(this._CurrentParameter.TicketTypePasssword))
        //                            {
        //                                this.MoreInfo += ", password:" + this._CurrentParameter.TicketTypePasssword;
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    changeStatus(TicketSearchStatus.ManualCaptchaStatus);

        //                    TMTicket tmp = ((TMTicket)this.Ticket);
        //                    lock (tmp.CaptchaQueue)
        //                    {
        //                        tmp.CaptchaQueue.Add(this);
        //                    }

        //                    this.Captcha.captchaentered.WaitOne();
        //                }

        //                changeStatus(TicketSearchStatus.CaptchaResolvedStatus);

        //                if (this.Captcha.CaptchaImage != null)
        //                {
        //                    this.Captcha.CaptchaImage.Dispose();
        //                    GC.SuppressFinalize(this.Captcha.CaptchaImage);
        //                    this.Captcha.CaptchaImage = null;
        //                }

        //                if (!this.IfWorking || !this.Ticket.isRunning)
        //                {
        //                    return false;
        //                }


        //                string resultCaptcha = bsClone.Get(this._tmEvent.EventAjax.Action + "?callback=bbaresvcb&v=" + this._tmEvent.EventAjax.TMAJAXCaptchaObj.Continue + "&adcopy_challenge=" + solveMediaChallenge + "&adcopy_response=" + this.Captcha.CaptchaWords.Replace(" ", "+") + "&_=" + UnixTimeNow().ToString());

        //                if (!String.IsNullOrEmpty(resultCaptcha))
        //                {
        //                    TMAJAXCaptchaResponse tMAJAXCaptchaResponse = new TMAJAXCaptchaResponse(resultCaptcha);
        //                    if (!tMAJAXCaptchaResponse.IsFailed)
        //                    {
        //                        ifCaptchaFailed = false;
        //                        //this.processRefreshPage();
        //                        ///////////////////
        //                        changeStatus(TicketSearchStatus.RefreshPageStatus);
        //                        sleep(1000);
        //                        if (!this.IfWorking || !this.Ticket.isRunning)
        //                        {
        //                            return false;
        //                        }
        //                        //resultCaptcha = bsClone.Get(this._tmEvent.EventAjax.Action + "?v=" + tMAJAXCaptchaResponse.Continue + "&callback=bbaresvcb&_=" + UnixTimeNow().ToString());
        //                        resultCaptcha = this._session.Get(this._tmEvent.EventAjax.Action + "?v=" + tMAJAXCaptchaResponse.Continue + "&callback=bbaresvcb&_=" + UnixTimeNow().ToString());
        //                        tMAJAXCaptchaResponse.parseJSON(resultCaptcha);

        //                        while (String.IsNullOrEmpty(bsClone.RedirectLocation) && !String.IsNullOrEmpty(resultCaptcha) && !String.IsNullOrEmpty(tMAJAXCaptchaResponse.Continue) && !tMAJAXCaptchaResponse.IsErrorOrNotFound)
        //                        {
        //                            if (!this.IfWorking || !this.Ticket.isRunning)
        //                            {
        //                                return false;
        //                            }

        //                            sleep(1000);

        //                            resultCaptcha = this._session.Get(this._tmEvent.EventAjax.Action + "?v=" + tMAJAXCaptchaResponse.Continue + "&callback=bbaresvcb&queue_token=" + tMAJAXCaptchaResponse.Queue_token + "&_=" + UnixTimeNow().ToString());

        //                            tMAJAXCaptchaResponse.parseJSON(resultCaptcha);
        //                        }

        //                        if (!this.IfWorking || !this.Ticket.isRunning)
        //                        {
        //                            return false;
        //                        }

        //                        ///////////////////
        //                        if (!String.IsNullOrEmpty(bsClone.RedirectLocation) && !tMAJAXCaptchaResponse.IsErrorOrNotFound)
        //                        {
        //                            ifCaptchaFailed = false;
        //                            //resultCaptcha = bsClone.Get(bsClone.RedirectLocation);
        //                            resultCaptcha = this._session.Get(bsClone.RedirectLocation);
        //                            tMAJAXCaptchaResponse = new TMAJAXCaptchaResponse(resultCaptcha);

        //                            this._session.FormElements.Clear();
        //                            this._session.FormElements.Add("v", tMAJAXCaptchaResponse.Continue);

        //                            this.setManualURL(this._tmEvent.EventAjax.Checkout, this._session.FormElements);

        //                            try
        //                            {
        //                                Regex.CacheSize = 0;
        //                                Match m = Regex.Match(resultCaptcha.Replace("\n", ""), "bbaresvcb\\((.*?)\\);", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        //                                String subStr = m.Value;
        //                                subStr = subStr.Replace("bbaresvcb(", "");
        //                                subStr = subStr.Replace(");", "");
        //                                subStr = subStr.Trim();

        //                                JObject JSONdataTMP = JObject.Parse(subStr);

        //                                if (JSONdataTMP != null)
        //                                {
        //                                    if (JSONdataTMP["data"] != null)
        //                                    {
        //                                        if (JSONdataTMP["data"]["cart"] != null)
        //                                        {
        //                                            if (JSONdataTMP["data"]["cart"]["secnames"] != null && !String.IsNullOrEmpty(this._CurrentParameter.Section))
        //                                            {
        //                                                IEnumerable<JToken> secNames = JSONdataTMP["data"]["cart"]["secnames"].Children();
        //                                                if (secNames != null)
        //                                                {
        //                                                    if (secNames.Count() > 0)
        //                                                    {
        //                                                        Boolean ifMatched = false;
        //                                                        foreach (JToken item in secNames)
        //                                                        {
        //                                                            if (item != null)
        //                                                            {
        //                                                                if (item.ToString() == this._CurrentParameter.Section || item.ToString().Contains(this._CurrentParameter.Section))
        //                                                                {
        //                                                                    ifMatched = true;
        //                                                                    break;
        //                                                                }
        //                                                            }
        //                                                        }

        //                                                        if (!ifMatched)
        //                                                        {
        //                                                            if (_CurrentParameter != null)
        //                                                            {
        //                                                                _CurrentParameter.IfFound = false;
        //                                                                this.MoreInfo = "Sold out with Qty:" + _CurrentParameter.Quantity;
        //                                                                if (!String.IsNullOrEmpty(_CurrentParameter.Section))
        //                                                                {
        //                                                                    this.MoreInfo += ", Sec:" + _CurrentParameter.Section;
        //                                                                }
        //                                                                if (_CurrentParameter.PriceMin > 0)
        //                                                                {
        //                                                                    this.MoreInfo += ", Min Price:" + _CurrentParameter.PriceMin;
        //                                                                }
        //                                                                if (_CurrentParameter.PriceMax > 0)
        //                                                                {
        //                                                                    this.MoreInfo += ", Max Price:" + _CurrentParameter.PriceMax;
        //                                                                }
        //                                                                if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypeString))
        //                                                                {
        //                                                                    this.MoreInfo += ", Type:" + _CurrentParameter.TicketTypeString;
        //                                                                }
        //                                                                if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypePasssword))
        //                                                                {
        //                                                                    this.MoreInfo += ", Passsword:" + _CurrentParameter.TicketTypePasssword;
        //                                                                }

        //                                                                if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
        //                                                                {
        //                                                                    _CurrentParameter = null;
        //                                                                }
        //                                                            }

        //                                                            //if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
        //                                                            //{
        //                                                            //    _CurrentParameter = null;
        //                                                            //}

        //                                                            //_CurrentParameter.IfFound = false;

        //                                                            //this.MoreInfo = "Section does not match in the found ticket.";

        //                                                            if (this.Ticket.onNotFound != null)
        //                                                            {
        //                                                                this.Ticket.onNotFound(this.Ticket);
        //                                                            }
        //                                                            return false;
        //                                                        }
        //                                                    }
        //                                                }
        //                                            }
        //                                            if (JSONdataTMP["data"]["cart"]["charges"] != null && _CurrentParameter.PriceMin != null && _CurrentParameter.PriceMax != null)
        //                                            {
        //                                                if (JSONdataTMP["data"]["cart"]["charges"] != null)
        //                                                {
        //                                                    IEnumerable<JToken> charges = JSONdataTMP["data"]["cart"]["charges"].Children();
        //                                                    if (charges != null)
        //                                                    {
        //                                                        if (charges.Count() >0)
        //                                                        {
        //                                                            decimal foundPrice = 0;
        //                                                            for (int i = 0; i < charges.Count(); i++)
        //                                                            {
        //                                                                if (JSONdataTMP["data"]["cart"]["charges"][i]["cost"] != null)
        //                                                                {
        //                                                                    foundPrice += decimal.Parse(JSONdataTMP["data"]["cart"]["charges"][i]["cost"].ToString());
        //                                                                }
        //                                                            }

        //                                                            if (foundPrice >0)
        //                                                            {
        //                                                                if (!( foundPrice >= (decimal)_CurrentParameter.PriceMin && foundPrice <= (decimal)_CurrentParameter.PriceMax))
        //                                                                {
        //                                                                    if (_CurrentParameter != null)
        //                                                                    {
        //                                                                        _CurrentParameter.IfFound = false;
        //                                                                        this.MoreInfo = "Sold out with Qty:" + _CurrentParameter.Quantity;
        //                                                                        if (!String.IsNullOrEmpty(_CurrentParameter.Section))
        //                                                                        {
        //                                                                            this.MoreInfo += ", Sec:" + _CurrentParameter.Section;
        //                                                                        }
        //                                                                        if (_CurrentParameter.PriceMin > 0)
        //                                                                        {
        //                                                                            this.MoreInfo += ", Min Price:" + _CurrentParameter.PriceMin;
        //                                                                        }
        //                                                                        if (_CurrentParameter.PriceMax > 0)
        //                                                                        {
        //                                                                            this.MoreInfo += ", Max Price:" + _CurrentParameter.PriceMax;
        //                                                                        }
        //                                                                        if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypeString))
        //                                                                        {
        //                                                                            this.MoreInfo += ", Type:" + _CurrentParameter.TicketTypeString;
        //                                                                        }
        //                                                                        if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypePasssword))
        //                                                                        {
        //                                                                            this.MoreInfo += ", Passsword:" + _CurrentParameter.TicketTypePasssword;
        //                                                                        }

        //                                                                        if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
        //                                                                        {
        //                                                                            _CurrentParameter = null;
        //                                                                        }
        //                                                                    }

        //                                                                    //if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
        //                                                                    //{
        //                                                                    //    _CurrentParameter = null;
        //                                                                    //}

        //                                                                    //_CurrentParameter.IfFound = false;

        //                                                                    //this.MoreInfo = "Price range does not match in the found ticket.";

        //                                                                    if (this.Ticket.onNotFound != null)
        //                                                                    {
        //                                                                        this.Ticket.onNotFound(this.Ticket);
        //                                                                    }
        //                                                                    return false;
        //                                                                }
        //                                                            }
        //                                                        }
        //                                                    }                                                            
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                }


        //                                GC.SuppressFinalize(JSONdataTMP);
        //                                JSONdataTMP = null;

        //                            }
        //                            catch { }

        //                            if (!this.IfWorking || !this.Ticket.isRunning)
        //                            {
        //                                return false;
        //                            }

        //                            changeStatus(TicketSearchStatus.RedirectingToReviewPageStatus);
        //                            sleep(1000);

        //                            resultCaptcha = this._session.Post(this._tmEvent.EventAjax.Checkout);

        //                            ifCaptchaFailed = false;
        //                            return true;
        //                        }
        //                        else
        //                        {
        //                            //if (_CurrentParameter != null && !tMAJAXCaptchaResponse.IsErrorOrNotFound)
        //                            if (_CurrentParameter != null)
        //                            {
        //                                _CurrentParameter.IfFound = false;
        //                                this.MoreInfo = "Sold out with Qty:" + _CurrentParameter.Quantity;
        //                                if (!String.IsNullOrEmpty(_CurrentParameter.Section))
        //                                {
        //                                    this.MoreInfo += ", Sec:" + _CurrentParameter.Section;
        //                                }
        //                                if (_CurrentParameter.PriceMin > 0)
        //                                {
        //                                    this.MoreInfo += ", Min Price:" + _CurrentParameter.PriceMin;
        //                                }
        //                                if (_CurrentParameter.PriceMax > 0)
        //                                {
        //                                    this.MoreInfo += ", Max Price:" + _CurrentParameter.PriceMax;
        //                                }
        //                                if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypeString))
        //                                {
        //                                    this.MoreInfo += ", Type:" + _CurrentParameter.TicketTypeString;
        //                                }
        //                                if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypePasssword))
        //                                {
        //                                    this.MoreInfo += ", Passsword:" + _CurrentParameter.TicketTypePasssword;
        //                                }

        //                                if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
        //                                {
        //                                    _CurrentParameter = null;
        //                                }
        //                            }
        //                            //else if (tMAJAXCaptchaResponse.IsErrorOrNotFound)
        //                            //{
        //                            //    _CurrentParameter.IfFound = false;
        //                            //    this.MoreInfo = "No tickets match your search or there was an error with search.";
        //                            //}
        //                            else
        //                            {
        //                                _CurrentParameter.IfFound = false;
        //                                this.MoreInfo = "Ticket not found";
        //                            }

        //                            if (this.Ticket.onNotFound != null)
        //                            {
        //                                this.Ticket.onNotFound(this.Ticket);
        //                            }
        //                            return false;
        //                        }
        //                        //resultCaptcha = bsClone.Get(this._tmEvent.EventAjax.Action + "?v=" + tMAJAXCaptchaResponse.Continue + "&callback=bbaresvcb&_=" + UnixTimeNow().ToString());
        //                    }
        //                    else
        //                    {
        //                        ifCaptchaFailed = true;
        //                    }
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            } while (ifCaptchaFailed && this.Ticket.isRunning && this.IfWorking);

        //        }
        //        else //Google
        //        {
        //            //Bypass Solvemedia captcha
        //            //string resultCaptcha = bsClone.Get(this._tmEvent.EventAjax.Action + "?callback=bbaresvcb&v=" + this._tmEvent.EventAjax.TMAJAXCaptchaObj.Recaptcha_force + "&_=" + UnixTimeNow().ToString());

        //            Boolean ifCaptchaFailed = false;
        //            do
        //            {

        //                if (!this.IfWorking || !this.Ticket.isRunning)
        //                {
        //                    return false;
        //                }

        //                WebProxy tmpProxy = this._session.Proxy;
        //                BrowserSession bsCloneGoogle = this._session;

        //                //Bypass solvemedia captcha
        //                int retryCnt = 0;
        //                while (this._tmEvent.EventAjax.TMAJAXCaptchaObj.Recaptcha_type == "solve_media" && retryCnt < 5)
        //                {
        //                    strHTML = bsCloneGoogle.Get(this._tmEvent.EventAjax.Action + "?callback=bbaresvcb&v=" + this._tmEvent.EventAjax.TMAJAXCaptchaObj.Recaptcha_force + "&_=" + UnixTimeNow().ToString());
        //                    this._tmEvent.EventAjax.TMAJAXCaptchaObj = new TMAJAXCaptcha(strHTML);
        //                    retryCnt++;
        //                }

        //                if (this._tmEvent.EventAjax.TMAJAXCaptchaObj.Recaptcha_type == "solve_media" || this._tmEvent.EventAjax.TMAJAXCaptchaObj.Recaptcha_type != "recaptcha")
        //                {
        //                    return false;
        //                }

        //                if (this.Ticket.ifUseProxiesInCaptchaSource)
        //                {
        //                    bsCloneGoogle.Proxy = this._session.Proxy;
        //                }
        //                else
        //                {
        //                    bsCloneGoogle.Proxy = null;
        //                }


        //                changeStatus(TicketSearchStatus.CaptchaPageStatus);


        //                strRecaptchaCreate = this._tmEvent.EventAjax.TMAJAXCaptchaObj.Recaptcha_public_key;
        //                strChallenge = this._tmEvent.EventAjax.TMAJAXCaptchaObj.Recaptcha_params;
        //                if (String.IsNullOrEmpty(strRecaptchaCreate) || String.IsNullOrEmpty(strChallenge))
        //                {
        //                    return false;
        //                }

        //                if (!this.IfWorking || !this.Ticket.isRunning)
        //                {
        //                    return false;
        //                }

        //                if (this.Ticket.ifUseGoogle)
        //                {
        //                    //code = Encoding.ASCII.GetString(wc.DownloadData("http://www.google.com/recaptcha/api/challenge?k=" + strRecaptchaCreate + "&ajax=1&cachestop=" + new Random().Next().ToString() + "&" + strChallenge)); ;
        //                    code = Encoding.ASCII.GetString(bsCloneGoogle.GetBinaryData("http://www.google.com/recaptcha/api/challenge?k=" + strRecaptchaCreate + "&ajax=1&cachestop=" + new Random().Next().ToString() + "&" + strChallenge)); ;
        //                }
        //                else
        //                {
        //                    //code = Encoding.ASCII.GetString(wc.DownloadData("http://api.recaptcha.net/challenge?k=" + strRecaptchaCreate + "&ajax=1&cachestop=" + new Random().Next().ToString() + "&" + strChallenge)); ;
        //                    code = Encoding.ASCII.GetString(bsCloneGoogle.GetBinaryData("http://api.recaptcha.net/challenge?k=" + strRecaptchaCreate + "&ajax=1&cachestop=" + new Random().Next().ToString() + "&" + strChallenge)); ;
        //                }

        //                if (!this.IfWorking || !this.Ticket.isRunning)
        //                {
        //                    return false;
        //                }

        //                strChallenge = this.extractParameter(code, "challenge");
        //                strServer = this.extractParameter(code, "server");
        //                uRL = strServer + "image?c=" + strChallenge;
        //                if (!this.IfWorking || !this.Ticket.isRunning)
        //                {
        //                    return false;
        //                }

        //                //imgBytes = wc.DownloadData(uRL);
        //                imgBytes = bsCloneGoogle.GetBinaryData(uRL);

        //                this._session.Proxy = tmpProxy;

        //                this.Captcha = new Captcha(imgBytes);

        //                if (this.Ticket.ifAutoCaptcha && this.IfUseAutoCaptcha)
        //                {
        //                    changeStatus(TicketSearchStatus.ResolvingCaptchaStatus);

        //                    if (this.Ticket.ifDBCAutoCaptcha)
        //                    {
        //                        solveAutoCaptcha = new DeathByCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }
        //                    else if (this.Ticket.ifRDAutoCaptcha)
        //                    {
        //                        solveAutoCaptcha = new RDCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }
        //                    else if (this.Ticket.ifCPTAutoCaptcha)
        //                    {
        //                        solveAutoCaptcha = new CPTCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }
        //                    else if (this.Ticket.ifDCAutoCaptcha)
        //                    {
        //                        solveAutoCaptcha = new DeCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }
        //                    else if (this.Ticket.ifOCR)
        //                    {
        //                        solveAutoCaptcha = new OCRService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }
        //                    else
        //                    {
        //                        // By Default use bypass auto captcha
        //                        solveAutoCaptcha = new BypassAutoCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
        //                    }

        //                    if (this.IfWorking && this.Ticket.isRunning && solveAutoCaptcha != null)
        //                    {
        //                        solveAutoCaptcha.solve();
        //                    }
        //                    else
        //                    {
        //                        return false;
        //                    }

        //                    if (!String.IsNullOrEmpty(solveAutoCaptcha.CaptchaError))
        //                    {
        //                        //this.MoreInfo = TicketSearchStatus.MoreInfoAutoCaptchaError + solveAutoCaptcha.CaptchaError;
        //                    }
        //                    else
        //                    {
        //                        this.MoreInfo = "";
        //                        if (this._CurrentParameter != null)
        //                        {
        //                            this.MoreInfo = "Searching quantity:" + this._CurrentParameter.Quantity;
        //                            if (this._CurrentParameter.PriceMin != null && this._CurrentParameter.PriceMax != null)
        //                            {
        //                                this.MoreInfo += ", price:" + this._CurrentParameter.PriceMin.ToString() + " - " + this._CurrentParameter.PriceMax.ToString();
        //                            }
        //                            if (!String.IsNullOrEmpty(this._CurrentParameter.TicketTypeString))
        //                            {
        //                                this.MoreInfo += ", ticket type:" + this._CurrentParameter.TicketTypeString;
        //                            }
        //                            if (!String.IsNullOrEmpty(this._CurrentParameter.TicketTypePasssword))
        //                            {
        //                                this.MoreInfo += ", password:" + this._CurrentParameter.TicketTypePasssword;
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    changeStatus(TicketSearchStatus.ManualCaptchaStatus);

        //                    TMTicket tmp = ((TMTicket)this.Ticket);
        //                    lock (tmp.CaptchaQueue)
        //                    {
        //                        tmp.CaptchaQueue.Add(this);
        //                    }

        //                    this.Captcha.captchaentered.WaitOne();
        //                }

        //                changeStatus(TicketSearchStatus.CaptchaResolvedStatus);

        //                if (this.Captcha.CaptchaImage != null)
        //                {
        //                    this.Captcha.CaptchaImage.Dispose();
        //                    GC.SuppressFinalize(this.Captcha.CaptchaImage);
        //                    this.Captcha.CaptchaImage = null;
        //                }

        //                if (!this.IfWorking || !this.Ticket.isRunning)
        //                {
        //                    return false;
        //                }


        //                string resultCaptcha = bsCloneGoogle.Get(strServer + "ajaxverify?c=" + strChallenge + "&response=" + this.Captcha.CaptchaWords);


        //                if (!String.IsNullOrEmpty(resultCaptcha))
        //                {
        //                    if (Regex.Match(resultCaptcha, "is_correct(:?)true").Success)
        //                    {
        //                        verify_psig = this.extractParameter(resultCaptcha, "verify_psig");

        //                        if (!this.IfWorking || !this.Ticket.isRunning)
        //                        {
        //                            return false;
        //                        }

        //                        resultCaptcha = bsCloneGoogle.Get(this._tmEvent.EventAjax.Action + "?v=" + this._tmEvent.EventAjax.TMAJAXCaptchaObj.Continue + "&callback=bbaresvcb&verify_psig=" + verify_psig + "&_=" + UnixTimeNow().ToString());

        //                        TMAJAXCaptchaResponse tMAJAXCaptchaResponse = new TMAJAXCaptchaResponse(resultCaptcha);
        //                        if (!tMAJAXCaptchaResponse.IsFailed)
        //                        {
        //                            ifCaptchaFailed = false;

        //                            //this.processRefreshPage();
        //                            ///////////////////
        //                            changeStatus(TicketSearchStatus.RefreshPageStatus);
        //                            sleep(1000);

        //                            if (!this.IfWorking || !this.Ticket.isRunning)
        //                            {
        //                                return false;
        //                            }

        //                            //resultCaptcha = bsClone.Get(this._tmEvent.EventAjax.Action + "?v=" + tMAJAXCaptchaResponse.Continue + "&callback=bbaresvcb&_=" + UnixTimeNow().ToString());
        //                            resultCaptcha = this._session.Get(this._tmEvent.EventAjax.Action + "?v=" + tMAJAXCaptchaResponse.Continue + "&callback=bbaresvcb&_=" + UnixTimeNow().ToString());

        //                            tMAJAXCaptchaResponse.parseJSON(resultCaptcha);

        //                            while (String.IsNullOrEmpty(bsCloneGoogle.RedirectLocation) && !String.IsNullOrEmpty(resultCaptcha) && !String.IsNullOrEmpty(tMAJAXCaptchaResponse.Continue) && !tMAJAXCaptchaResponse.IsErrorOrNotFound)
        //                            {
        //                                if (!this.IfWorking || !this.Ticket.isRunning)
        //                                {
        //                                    return false;
        //                                }

        //                                sleep(1000);

        //                                resultCaptcha = this._session.Get(this._tmEvent.EventAjax.Action + "?v=" + tMAJAXCaptchaResponse.Continue + "&callback=bbaresvcb&queue_token="+ tMAJAXCaptchaResponse.Queue_token +"&_=" + UnixTimeNow().ToString());

        //                                tMAJAXCaptchaResponse.parseJSON(resultCaptcha);
        //                            }

        //                            if (!this.IfWorking || !this.Ticket.isRunning)
        //                            {
        //                                return false;
        //                            }
        //                            ///////////////////
        //                            if (!String.IsNullOrEmpty(bsCloneGoogle.RedirectLocation) && !tMAJAXCaptchaResponse.IsErrorOrNotFound)
        //                            {
        //                                ifCaptchaFailed = false;
        //                                //resultCaptcha = bsClone.Get(bsClone.RedirectLocation);
        //                                resultCaptcha = this._session.Get(bsCloneGoogle.RedirectLocation);
        //                                tMAJAXCaptchaResponse = new TMAJAXCaptchaResponse(resultCaptcha);

        //                                this._session.FormElements.Clear();
        //                                this._session.FormElements.Add("v", tMAJAXCaptchaResponse.Continue);

        //                                this.setManualURL(this._tmEvent.EventAjax.Checkout, this._session.FormElements);


        //                                try
        //                                {
        //                                    Regex.CacheSize = 0;
        //                                    Match m = Regex.Match(resultCaptcha.Replace("\n", ""), "bbaresvcb\\((.*?)\\);", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        //                                    String subStr = m.Value;
        //                                    subStr = subStr.Replace("bbaresvcb(", "");
        //                                    subStr = subStr.Replace(");", "");
        //                                    subStr = subStr.Trim();

        //                                    JObject JSONdataTMP = JObject.Parse(subStr);

        //                                    if (JSONdataTMP != null)
        //                                    {
        //                                        if (JSONdataTMP["data"] != null)
        //                                        {
        //                                            if (JSONdataTMP["data"]["cart"] != null)
        //                                            {
        //                                                if (JSONdataTMP["data"]["cart"]["secnames"] != null && !String.IsNullOrEmpty(this._CurrentParameter.Section))
        //                                                {
        //                                                    IEnumerable<JToken> secNames = JSONdataTMP["data"]["cart"]["secnames"].Children();
        //                                                    if (secNames != null)
        //                                                    {
        //                                                        if (secNames.Count() > 0)
        //                                                        {
        //                                                            Boolean ifMatched = false;
        //                                                            foreach (JToken item in secNames)
        //                                                            {
        //                                                                if (item != null)
        //                                                                {
        //                                                                    if (item.ToString() == this._CurrentParameter.Section || item.ToString().Contains(this._CurrentParameter.Section))
        //                                                                    {
        //                                                                        ifMatched = true;
        //                                                                        break;
        //                                                                    }                                                                            
        //                                                                }                                                                        
        //                                                            }

        //                                                            if (!ifMatched)
        //                                                            {
        //                                                                if (_CurrentParameter != null)
        //                                                                {
        //                                                                    _CurrentParameter.IfFound = false;
        //                                                                    this.MoreInfo = "Sold out with Qty:" + _CurrentParameter.Quantity;
        //                                                                    if (!String.IsNullOrEmpty(_CurrentParameter.Section))
        //                                                                    {
        //                                                                        this.MoreInfo += ", Sec:" + _CurrentParameter.Section;
        //                                                                    }
        //                                                                    if (_CurrentParameter.PriceMin > 0)
        //                                                                    {
        //                                                                        this.MoreInfo += ", Min Price:" + _CurrentParameter.PriceMin;
        //                                                                    }
        //                                                                    if (_CurrentParameter.PriceMax > 0)
        //                                                                    {
        //                                                                        this.MoreInfo += ", Max Price:" + _CurrentParameter.PriceMax;
        //                                                                    }
        //                                                                    if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypeString))
        //                                                                    {
        //                                                                        this.MoreInfo += ", Type:" + _CurrentParameter.TicketTypeString;
        //                                                                    }
        //                                                                    if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypePasssword))
        //                                                                    {
        //                                                                        this.MoreInfo += ", Passsword:" + _CurrentParameter.TicketTypePasssword;
        //                                                                    }

        //                                                                    if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
        //                                                                    {
        //                                                                        _CurrentParameter = null;
        //                                                                    }
        //                                                                }

        //                                                                //if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
        //                                                                //{
        //                                                                //    _CurrentParameter = null;
        //                                                                //}

        //                                                                //_CurrentParameter.IfFound = false;

        //                                                                //this.MoreInfo = "Section does not match in the found ticket.";

        //                                                                if (this.Ticket.onNotFound != null)
        //                                                                {
        //                                                                    this.Ticket.onNotFound(this.Ticket);
        //                                                                }
        //                                                                return false;
        //                                                            }


        //                                                        }
        //                                                    }
        //                                                }
        //                                                if (JSONdataTMP["data"]["cart"]["charges"] != null && _CurrentParameter.PriceMin != null && _CurrentParameter.PriceMax != null)
        //                                                {
        //                                                    if (JSONdataTMP["data"]["cart"]["charges"] != null)
        //                                                    {
        //                                                        IEnumerable<JToken> charges = JSONdataTMP["data"]["cart"]["charges"].Children();
        //                                                        if (charges != null)
        //                                                        {
        //                                                            if (charges.Count() > 0)
        //                                                            {
        //                                                                decimal foundPrice = 0;
        //                                                                for (int i = 0; i < charges.Count(); i++)
        //                                                                {
        //                                                                    if (JSONdataTMP["data"]["cart"]["charges"][i]["cost"] != null)
        //                                                                    {
        //                                                                        foundPrice += decimal.Parse(JSONdataTMP["data"]["cart"]["charges"][i]["cost"].ToString());
        //                                                                    }
        //                                                                }

        //                                                               if (foundPrice > 0)
        //                                                                {
        //                                                                    if (!(foundPrice >= (decimal)_CurrentParameter.PriceMin && foundPrice <= (decimal)_CurrentParameter.PriceMax))
        //                                                                    {
        //                                                                        if (_CurrentParameter != null)
        //                                                                        {
        //                                                                            _CurrentParameter.IfFound = false;
        //                                                                            this.MoreInfo = "Sold out with Qty:" + _CurrentParameter.Quantity;
        //                                                                            if (!String.IsNullOrEmpty(_CurrentParameter.Section))
        //                                                                            {
        //                                                                                this.MoreInfo += ", Sec:" + _CurrentParameter.Section;
        //                                                                            }
        //                                                                            if (_CurrentParameter.PriceMin > 0)
        //                                                                            {
        //                                                                                this.MoreInfo += ", Min Price:" + _CurrentParameter.PriceMin;
        //                                                                            }
        //                                                                            if (_CurrentParameter.PriceMax > 0)
        //                                                                            {
        //                                                                                this.MoreInfo += ", Max Price:" + _CurrentParameter.PriceMax;
        //                                                                            }
        //                                                                            if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypeString))
        //                                                                            {
        //                                                                                this.MoreInfo += ", Type:" + _CurrentParameter.TicketTypeString;
        //                                                                            }
        //                                                                            if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypePasssword))
        //                                                                            {
        //                                                                                this.MoreInfo += ", Passsword:" + _CurrentParameter.TicketTypePasssword;
        //                                                                            }

        //                                                                            if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
        //                                                                            {
        //                                                                                _CurrentParameter = null;
        //                                                                            }
        //                                                                        }

        //                                                                        //if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
        //                                                                        //{
        //                                                                        //    _CurrentParameter = null;
        //                                                                        //}

        //                                                                        //_CurrentParameter.IfFound = false;

        //                                                                        //this.MoreInfo = "Price range does not match in the found ticket.";

        //                                                                        if (this.Ticket.onNotFound != null)
        //                                                                        {
        //                                                                            this.Ticket.onNotFound(this.Ticket);
        //                                                                        }
        //                                                                        return false;
        //                                                                    }
        //                                                                }
        //                                                            }
        //                                                        }
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                    }

        //                                    GC.SuppressFinalize(JSONdataTMP);
        //                                    JSONdataTMP = null;
        //                                }
        //                                catch { }

        //                                if (!this.IfWorking || !this.Ticket.isRunning)
        //                                {
        //                                    return false;
        //                                }

        //                                changeStatus(TicketSearchStatus.RedirectingToReviewPageStatus);
        //                                sleep(1000);

        //                                resultCaptcha = this._session.Post(this._tmEvent.EventAjax.Checkout);

        //                                ifCaptchaFailed = false;
        //                                return true;
        //                            }
        //                            else
        //                            {
        //                              //  if (_CurrentParameter != null && !tMAJAXCaptchaResponse.IsErrorOrNotFound)                                     
        //                                if (_CurrentParameter != null)
        //                                {
        //                                    _CurrentParameter.IfFound = false;
        //                                    this.MoreInfo = "Sold out with Qty:" + _CurrentParameter.Quantity;
        //                                    if (!String.IsNullOrEmpty(_CurrentParameter.Section))
        //                                    {
        //                                        this.MoreInfo += ", Sec:" + _CurrentParameter.Section;
        //                                    }
        //                                    if (_CurrentParameter.PriceMin > 0)
        //                                    {
        //                                        this.MoreInfo += ", Min Price:" + _CurrentParameter.PriceMin;
        //                                    }
        //                                    if (_CurrentParameter.PriceMax > 0)
        //                                    {
        //                                        this.MoreInfo += ", Max Price:" + _CurrentParameter.PriceMax;
        //                                    }
        //                                    if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypeString))
        //                                    {
        //                                        this.MoreInfo += ", Type:" + _CurrentParameter.TicketTypeString;
        //                                    }
        //                                    if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypePasssword))
        //                                    {
        //                                        this.MoreInfo += ", Passsword:" + _CurrentParameter.TicketTypePasssword;
        //                                    }

        //                                    if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
        //                                    {
        //                                        _CurrentParameter = null;
        //                                    }
        //                                }
        //                                //else if (tMAJAXCaptchaResponse.IsErrorOrNotFound)
        //                                //{
        //                                //     _CurrentParameter.IfFound = false;
        //                                //     this.MoreInfo = "No tickets match your search or there was an error with search.";
        //                                //}
        //                                else
        //                                {
        //                                    _CurrentParameter.IfFound = false;
        //                                    this.MoreInfo = "Ticket not found";
        //                                }

        //                                if (this.Ticket.onNotFound != null)
        //                                {
        //                                    this.Ticket.onNotFound(this.Ticket);
        //                                }
        //                                return false;
        //                            }
        //                            //resultCaptcha = bsClone.Get(this._tmEvent.EventAjax.Action + "?v=" + tMAJAXCaptchaResponse.Continue + "&callback=bbaresvcb&_=" + UnixTimeNow().ToString());
        //                        }
        //                        else
        //                        {
        //                            ifCaptchaFailed = true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ifCaptchaFailed = true;
        //                    }
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            } while (ifCaptchaFailed && this.Ticket.isRunning && this.IfWorking);
        //        }
        //    }
        //    catch
        //    { 

        //    }
        //    return false;
        //}

        System.Threading.Timer timer = null;

        protected async Task<Boolean> processCaptchaPage()
        {
            int retryCounter = 0;
            Boolean result = false;

            try
            {
                String strHTML;
                String strRecaptchaCreate, strChallenge = "", strServer, uRL;
                String code = "";
                String verify_psig, recaptcha_response_field;
                String delta, alpha, strLambda = "";

                int OCRCount = 0;
                int attempt = 0;
                WebClient webClient = null;
                byte[] imgBytes;
                Boolean ifReallySolveMediaCaptha = false;

                //if (this._tmEvent.IsAjax)
            //{
            //    return processCaptchaPageAjax();
            //}

            SolveCaptcha:

                ifReallySolveMediaCaptha = false;

                do
                {

                    changeStatus(TicketSearchStatus.CaptchaLoadingStatus);

                    webClient = new WebClient();

                    if (this.Session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("sessionexpire"))
                    {

                    }
                    HtmlNode _captchaNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//img[@id='imgSecurityWord']");

                    String token = String.Empty;

                    #region Are  u human captcha
                    //if (_captchaNode == null)
                    //{


                    //    BrowserSession captchaSession = new BrowserSession();

                    //    if (this._session.HtmlDocument.DocumentNode.InnerHtml.Contains("$(function ()"))
                    //    {
                    //        token = this._session.HtmlDocument.DocumentNode.InnerHtml;
                    //        token = token.Substring(token.LastIndexOf("var urls ="), 106);
                    //        token = token.Replace("var urls = ", String.Empty).Remove(0, 1);
                    //        string start = token.Remove(token.IndexOf('+')).Replace("'", String.Empty);
                    //        string end = token.Substring(token.IndexOf('?'), token.Length - token.IndexOf('?')).Replace("'+", String.Empty); ;
                    //        token = String.Concat(start, end).Replace(";", String.Empty);
                    //        //section.ParameterKey.Add("token", token);
                    //    }

                    //    if (captchaSession.Cookies != null)
                    //    {
                    //        captchaSession.Cookies = new CookieCollection();
                    //        captchaSession.Cookies.Add(AYAH_Cookie);
                    //    }

                    //    String response = captchaSession.Get(token);
                    //    response = new WebClient().DownloadString(token);

                    //    if (response.Contains("sessionSecret: function()"))
                    //    {
                    //        //token = response.Substring(response.LastIndexOf("sessionSecret: function()"), 200);
                    //        token = response.Substring(response.LastIndexOf("sessionSecret: function()"), response.IndexOf("},"));
                    //        //token = token.Replace("var urls = ", String.Empty).Remove(0, 1);
                    //        token = token.Substring(token.IndexOf("input.value")).Split(';')[0].Replace("\"", String.Empty).Replace("input.value = ", String.Empty);
                    //    }

                    //    //    Dictionary<String, String> postParameters = new Dictionary<string, string>();
                    //    //    foreach (KeyValuePair<String, String> item in this._session.FormElements)
                    //    //    {
                    //    //        postParameters.Add(item.Key, item.Value);
                    //    //    }

                    //    //if (captchaSession.Cookies != null)
                    //    //{
                    //    //    captchaSession.Cookies = new CookieCollection();
                    //    //    captchaSession.Cookies.Add(AYAH_Cookie);
                    //    //}
                    //    //captchaSession.Cookies.Add(AYAH_Cookie);
                    //    captchaSession.HTMLWeb.Referer = this._session.HTMLWeb.ResponseUri.AbsoluteUri;

                    //    response = captchaSession.Get("https://ws.areyouahuman.com/ws/chooseGame/0/1/" + token + "/0-0?campaign=none&campaignmessage=OPT_OUT");
                    //    ////https://ws.areyouahuman.com/ws/chooseGame/0/1/DVcWxM7CQeb5mrZ6OOsKV5EbefmCAcwnFxCpyiOv3tjGq/0-0?campaign=none&campaignmessage=OPT_OUT

                    //    // this._session.FormElements.Remove("m:c:_ctl24:btnBigStaticCancelled");
                    //    //    //this._session.Cookies.Add(new Cookie("__qca", "P0-1799455356-1369894479068"));
                    //    //captchaSession.FormElements.Clear();
                    //    //    //AddUpdateField(this._session, "bluecava", "FPv5.adQnJvd3NlcjpXaW4zMjo6RmlyZWZveDoyMTozMzQwOnNzdjE6:MjA4NTI0MDIzMjU3ODMwNjk3MTM2Nzg3:Dg9AWF4URldBQVxYVg4SBxcHExNTWVtYVllWUg8WRUZVHw0VBw0MUEkXSV5aWUsKEFhMQUIOHx1ERUIZWl9FU1pWR1IYVFdaHQIIBAIbAQAcVFxZX1ZCRkteX0cUF0BaXgpLRVNXVQ8RQkdSS1ZCQFwVDw9VRAYLURBRUQ8WcVhSShcJBFoOCkkXWFcLFXldU0gaCw5CDmp+fn1DTENiU0hCVEBCF1dVWFVbQQ4bRgwPHUUJBBxZCAUYUg0KVBheVg0admJhEgwPWwsLSBNZUgQVZUpGUhoJDkYGQFxfXl1EXAkYTg0MGUkJDRxfCQQYUQ4EVhJdVA8RZlxaXWlfWFwVDw9fCQRHEllcCBB7VlRAV0EVBg9GCBQEAQMKGE4JDh9ICw4bWQwPHVYJBFAQX10KE3VZWUxEEA4EXAwIQBJaVggVflxeQkoVDw9ACXlQV15bTBJych5yWlRFV11ZGnhbVVtXVVEbc1xfUEBdUVwfc1tTWV9FRRV2X1RFVlZWEn5dQh51XlVAU1tWbWNzGnhHUEFXXVFDUxx5R1NWWVETZkdWVkBAV0tSX0cadkpWUFlbFWZNQFdAV0FDUV1XGnhFWFJaG3lFW1FUFXBVXEZaURl2SlpRWhl1XVJVXBR2QFlZWRJ3dR5yQFxWVBNzb2sbcEFfVlQXdUJdUFkYcUBaU1kXdlJCRFZAHXJEXllbEmJXQFxQVVYTf2EXelxcUhV2Q1pXWxhjZ2IUdEBdUV4TZ1teW1xUUxl6Yh90VktcV0JOXF5YVRJ8XlEXflJTUxV1UEdXWV8bcFFMVFxTc1pWHndWTVtRQ0oXCAAadV1bXhB1YR52VUBfW1sXa1JeRRlxcx90UkpbW14YZlNaQxJ1cBVzXV5ZGntSQ11XRVwXf2QYdl1aVFddQVBTFHFcV1pcUFdSUkoXe2R7GXBbVF1dWxV6bB9yWV1YX1oWemwXcFxZVlkYcl1XXVteGH5kFnpYX1dTWUtSVhx6WlZbXlsTf2EXaFxDQlxFEXBZWkhFV0NLUFYYcl1cWRV2VkdZR0xWHXFZWFNaU14Yel5QEGFHS1lSFHFfWVJEWVZaURhkS11aWl4UBx5xQFRTVFZJFnFWX1cWfmx0HnJKXEZVXlxaURV1V19UGntFXlJSQFlOHnJKWkVVXF5aUxV5XUQcdEtYRlJaW1FWZ2B7GXBGRUFbEmZUSlpAQhl6ZR90QkBDXV4YZllRRFFbHnZWVFpSRFAbclJaXlpFWxB0XFVcRB5wU1leXlxCWFBWXxNwdRR0U1xRRkZbEH9nHnZWVVFCX1gbclJbVUpeUxB1VEZcHHFSXFFWSlIcdVhERVZaW1lFHnNdW0ZVRUAfcVBZTEZCTxV0VF1CQkpOEndXQVpdUx5wV1tDTUFJFmpUWVxZW1pYXVsUdlpdXF5WQBl0V19fWFdWEX5iG3tYX1lbFWFVXkETf2Ybe1xeRVZbUEAadFdZQURZW0ZdUR5wXVpHXUEQdFVWUlgadFdHQlVKRV5VRFcTdVpDUFpTFntYXVcadFdHQlVKRV5VRFcTdVpDUFpTFnVeVltCG3tYQFJdWR53X0BXW1QXdlZHGnpYQ1dfVm1ncRx7WkdGWVdBEntSTx9zWUxFWFZEF3ZSRRB6VF5AWVEfcVpCSlpVRBl5VEQWdH0bcV9NR1tRQhJ9V0IXe2piGnpYREFfUkoXfFVPFXVGVVdYHnZYTUFZU0sXf1ZBF2xiYBx7QEBYShJ+ZhlzWUZeZlxZWR9yVk5eVhx8c3lVWR9gcBlzUV9cU1deUGZmdBRzXVt7XVNZQFMfdlpDTV4cclZDRF51X10bd1JKXF9VHHdXRVRFXFpRWBlkUkFfR0wXe2R7GXdYVUJbU1tDFHZeUUtWR1ZERBh6Zhx9R1NHEHBcXlEXcWdzGnxFUEAWc11aWxBxYXEYdUBSQRV7UVRYQhl+ZXAackpWQRB1UFZdRV8Te2F0FHZDQktWX1RTW1cXd1RdRkFVHHdGUUdYS1pRY2l0HXZDR1BSX1lZGXRVXlVgXVtQFHVVWlBPEWdfQ1ReXFcUc11bRF5aVV1DGH5kFnVeVltCG35YQERdGXRGUVxYXlxZGHRfQlFeUhN0WFdcHnZKVFxfXFtdEnJYTFtZVRlzVF5fG35FU15TWVtaEHVcRl1eWxN0U1ReEXBZWVwbdEJZW1lYWVwTdVpDUFpTFnFSUEVPG35FU15TWVtaEHVcRl1eWxN9U11eRF4acUpWXFtUXFwUd11HWlxUGH5VUlBCXBN1WFZTHnZKVFxfYkdWWlkbfkFVU0peUGZmdBRxQFVdRkZNXFcTYVZFUUNEGn9FVF1VXxhkUUJRRUYUfWYfdVRVSlpfWlgbdlJSQl9eHndZR1NZX1xXHnJWTUdRW1AbdlZZRV9eUxx/XFVdHHVaXlkXa1JeRRl6ZR9xXlRbEmNZW0EUfWYTcVpZXFZeRVxTHXRfW1QXYVFWRhJ5ZBJ2SkEXe1xeUlxZQlZSF3pYXlQUcltYXBJgU1tEGGZcQktWEXFZW1wbdVlUWRJnUVxAEmBbTEFRFntYXVcWdFdZVlVWRldQHHVaQV1WFHRcWUxUVEBCUkoXf2QYcEpAQlMTcVpZXFZeRVxTHXRZQlxOEn9UURJnREtfVxlwV0ZUTxlkRVxDQxRwR1xRWB5zRV5aX3ZfXR93Q1dQQkZeG39CXFdLQFp3WFcfelRSTEdVWEpUWURTXlRSQBxwVEBYX0UTYVpbUVcQf01WXVpVG3BWQEJRW1VAX1wfelxQUBNkWU5SQxNiUkBDHnlVRVNXRB56X0VFUV1EFnRjEWBeVlxYRRxxW1RbQl9SXhVlV15RWBV+Q1pFYmh0HnlLXl1bXFMTYlpDWR96V0paWF1TYmh0HnpXXldGXVNdHn9CUVBVFnBjch99VlFjWxxzVF5dXlVSHn5WSkdZXVgbeltbUkoXZ3kUfl1QU1paU1tQbWNzGnJYWlpaVhR8QFlLQVdaEHtncRl8TV1DQlVSQxNlVEpeQkQUeVNbEGd6HnlWTFtRGnVSVF9XQFlTV1UUeVdCVVxaXxV6bB98X1VOZGN1G3RCUVlcVBJ2QltUWkEbdEZTX11WEXBXW1ReVUJZRVpNHH5GUVxTWRNzWVdEXl9TG3RCUVlcVBJyUUoffkBUUVdRFnFWX1dBRVFDW15fGX5BU1tXUxVkWV1DGnVCUlpSVhhkU15LFWZNQFdEQFxDXUEcekxUWFdXF2tWXEMYYFxdU11XVxl6WVReU01YHX5XXllZVkJZFXVwHH9SXlJCVhN3WU1fWFAaellZVVFUGX9VQl5WRkEbdVJEQ0tWEX5iF2tUQFlIQRJ3UUJaRlRbSx99U1BFSFwael1eQElXFWd9HH9aUUdYS1xWQhl/WF5XW1lOUxx1XFFGX0FcVEEXcltVWF5/VFoaelFUQF9LWlRAEHhbV1tQcFZZFmx+HX5fVEpYQV9eQRJ6VUUTZlReGH9FUxV6WFBEWEtYVEQYZVpVV0FjUxl6UVBCWUpYV0cWZFlZQRBrUEBdVh5+W1ZFV0BfUE0XZVJfF3RSHn1RVkBbQ11VRhViUVRYQ0sbfFpVRVdEXVZMFWtVeFdaHnheW0FfRVZRRRNvVnBSWxBtfB55WVFBXUZYXkcQb1AXc1JfQ1Ebf1lWUn5dZR5+W1tQdFplG3xPRXEaelFZVXxRYG18e2FwYRl6UV1XelBibnt9ZHtkH3VAQXAYfVtBW1RaFH5ZRFBWXBNwXkBSVhx1XEFAQlNfHnhYXFZCWBl5Xh0WBQgbf19WUl1YWVNdEndWUUdZGnRYX1xCTkhSEnNXR0FdRlMff1pYVHFfRFhZHX5lF39YRlhRVh55YxJ+W1tUUFwce2oXfkZCW1dYWRx1ZhJkd11HWlxUFH5jFml6WF1VX1cbf2MYZ1dSVUBWXFZSGGBRWEoXYlZEXl4bf2MYZ1dSVUBWXFZSGGBAU1peUF9CThR6YRBtfBJzX0ZbW1YbdWcQc0FDQ1Iaem4XcF9UXB55SVNdX1RFGGdVTk0bf1JEXFFEW10Ue1tVV1NBUxVyVlRCV09SVR94XllQU0JZFWFbXFtXHnteSl5RWlgXZHoaeWteX2NNWx56SVNfUxl4e2EQdxlySUdTWVxSVhx3WVYUdVxUXlxEUBNkU0FDEX5iG3dZS0gUZVNYUVFWEmZUSlpAQhl6ZR9mVlRWRllWWhJ4WVxcRkxHXR9gV0lOQ0ZFG2hWQFNQWFdaRB5jV0dHXUdFVxVnVEFGUkxCUxBsXEZYWVxUEnhjFGNcV1dDUFRTWV1DEnNQUEBbW1dWHmVbWUpSX1VbHWN7XlZQflltGWJ5WVxUflxiFXZIQnsbYVxZRRhlW1NQVEBQHGJBW0ZDUV1RGmtWUEVfG2pWVVUYfEZVXFtQHmdWTlpVGmtYUlhBUlRbHmJXVllDVV5fEnZYVldVWEpSVR9kWFtcRVVUWRJxSEZBUxV1V19UGmtYVR9lVlNcU1wYeFNeUV5fUxlkW0FZRk0XfGcWdVdbVhxrUFVbVRJ+U0dcXUEcZVxQXlYWZ0peXEQUZldTX1cTYVZFUUNEGmpSVlxTF21+HmNdUl1REGd6EnleX1tEGmpSVlxTF21+EmNdWFtWX15XHmZSX1xVFmx+EWBTWlFbW1dQQR5nVVVcVxVicRNjT1RVXl8aZFBYXFFKFXBVXlVfUxlkUFxHVVhFVRNxWExfW1MUZlpGRUZaHmZeVXtVXxVkWF5GW1FRW1VcFXNGUVBaURlkUV5AWlBRWFZSF3lFU1JRVhJyWUpWVhlkUV5jQ1cbYlpbZE1ZH3VAQXAYY1lWRlZffl9fQRlnQ1pYQxRkXFFIFXtgcx5gRlBZW1pcGmpOXVVXUlYbYUlVV11YHGZSWlpaWR9kU1RHREAWZFlZQRBxYXEYZFteV0YXdlZHFmtYXFJYG2xeX1VLFXxRRxJhXVhWVhNyV1VDWFAaY1FaV0MYe1dDEGBcX1RZGHB1Gm1eXFZFF3ZSRRBqWl9VXhJwa2cbbFpdU0oXf1ZBF2pYX1FWFXVGVVdYHmFeVVZDFndSRhNkWFVWXBBsYGAYZEBSVlxDUVxeV1UXcEFXVVFUHmRKUFBBU1pWRhV6ax9kQ1dQUB9iQBh0V14YeGYYZEUTcVBZGH5kFnpYX1dTWUtSVhxsQhJ3VVwTf2EXe1xeUlxZQlZSF31PRkJZFXBbXFYfZ0dTTRNkT0lSQlZCQ1FZVRxtQUFVUVofZFRZUR9mU0tTUF1XG25eWFFBVB5iWVxWQBV/WV1UFnBjch9gXk5WXlRRGWRYUVZaX1xFGGBTRFBHRR9gRVFZVlEUYldWVFtdVUYbb1pUUxl7UEdfWRRgW15fUVtaV0EfZVxZX1dZWF5EEQEaYFFZVVRRW1VHEAEPHUMJBBxACAUYWA0KGFsJDlMYXFYJEn5SXFJCWVRVRRsJDVoIC0gXW1QFF35VXlVGU1JSGg0MQAdSXx5jZAQYRA4EGkIKDB1aDAkYWw0MVRleVQ4UZ1RCVVlWRhAKDFsNDkUXUVcNFHdWXFYUCQRBDHFuchJnWUZWYVRRXUdJFklbRFRfWQQYRA4EGkIKDEITW1EKGmVVREpeXl0UCQRBDAEJGwMaABwHDhpBBg8fRgcLQRNfUwUVdFlUUHxVXVcRDAlBBl1ARVBDVEBXUV1DSx5cWV4IH0QNDhpHBg8fXwcLWA0KRxheVg0ae1NZVRANDkMJclJGVxFjfBoWZ1RWRlZXR18UY3cTBRViCgIMGU8JDRxGCQRHEllcCBBiVUBAW1pZGg0MQAcGAR0EBhYFHAEJCR1CDg4cQgsLSBNZUgQVd1paUnZWX1UaCw5CDlxDWEUFFldcWgUYRw0KGEgJDh9RCw5dDg5DElxTBRF+V1RSEw0KQQZ9U0ZZFXZRQF5cS1hSVkcQYlZYXVhfQxgAHAAWBwMEHgMCDhpBBg8fRgcLQRNfUwUVZFVKRltbXhANDkMJCQMeBAgZAx0HBgQYRA4EGkIKDEITW1EKGnVZWlx5UF5TFQYLRA5WRXZRQF5cS39WTlIBGF1bXQ8ZQQYLHUAGCR1dDg5aDAlHGFpUCxt5UF5TFQYLRA5/Wl1TXFcTZ0VTWUdVChZBDw8ZRwYLQhBRUQ8WZldBQVxYVhEOCk8JAB0FGQoGHAEMAA4bRgwPHUUJBEMQX10KE3VfW115U11dFwwIRgxdQnJYV1RcU2xHVVJCUgsZVlxUCR1CDg4cQgsLF1oOClAJDUMWXlwKEH5ZWFcWDg5FDGZeVEVVRFVeVltCF2hbR1cVfFwIH0QNDhpHBg9AFlBTDBFgUkpEW19WFwwIRgwGHAQZCQMEBwgZAQ8ZQQYLHUAGCUIUWVYOEHNeVFZ+V1RSEw0KQQZZQlNMR14aVF5fDhpBBg8fRgcLHloICxdUDAxbFVtQDRB0V1tSSlJcFAcLWA0KRxheVg0aYEFRQnNUV1tDGg0MQAd6XklfW1RWHQUWBRIcZ1tdVlpASxN+YhkBHwENF0pBCAIJGwIdEHVWUV5YFwEABwkHAAMHF35eQFVeWkobAgMdAgkYTg0MGUkJDUMWXlwKEHFIRXxVXVcRDAlBBn1VQkpUUENTCxdBDAwXRQwIQBJaVggVeUNAdVZTVH1XWl0VDAxOC39bSltfXlQLF0UOChZHDw9GF1FTDxJ5RUJiVUBAW1pZGg0MQAcCHwMWH29eXFRXQkEdDB1FDAkYSA0MRhleVQ4UeGt0YmUaCw5CDmVaXFFYT0AQeG0XBx0ECxdBDAwXRQwIQBJaVggVaF9RQl9YQ14UCQRBDGdRWwEGDB1FDAkYSA0MRhleVQ4UZV1RV0JKXFxTYFNHWhcJBEUOXk1DQUAMGBdAQR5ZR1dNX0dSWkBaWV0eVVZaHkRFGFtfXV9LUHVVXVccAxoGF1pABwkOAQUFBQoOX3xvQX1EBmppRwdEaHoDbEFVZ3gFfnFxWglLUwRxQV9yVAkYTg0MGUkJDUMWXlwKEHZUVEFcZldBQVxYVhEOCk8JAAIYABYAAgAEGkQKDB1DDAlHGFpUCxtkRlVwXlRSfl9ZUVdQEgwPRAsGBBxGCAUYQQ0KGFEJDh9bCw5XEFtXDxd9a3ZeUVBZVBEIC1EJDkAYXFYJEnxSX1AVBg9GCHRWRVsYe3dwAHUEGkQKDB1DDAlHGFpUCxthUF9DUhoJDkYGBBwABAAFCwAHDAMIDgEOBwACCxdBDAwXRQwIH1sNDlwJBEMQX10KE31XWl0VDAxOC39VRFodfnpwCQN1ChZBDw8ZRwYLQhBRUQ8WZlNfR1AVBg9GCAkZBQACBQEDBggJDAIHAgcCCgkYTg0MGUkJDRxfCQQYUQ4EVhJdVA8RYVxbTlZCWlBQWUd1WFVHXV5dW0ZHEgwPWwsLSBNZUgQVZ1ZERFFYXBIGCUQKARwDDhpBBg8fRgcLHloIC1EJDkAYXFYJEmRWQEZeV10SCAVBDwEYBxYEAgIKAw4bRgwPHUUJBBxZCAVeDw9GF1FTDxJuUEBHWV1dEAsLTg0CGAkZAgMDBQsLHUYGCR1EDg4cWwsLUQ0MRhleVQ4UYV1FQVlXWxAKDEQNABsHFgAADwsEDRxACQQYQg4EGlsKDFsNDkUXUVcNFG9SQ0BfWFYVDAxOCwAaABwAAwUHDQ8fQAcLHkMICxdeDAxRCw5EEFtXDxdhXUFDX1ZZEw0KQQYEHAAWAQICAgYPHUMJBBxACAUYWA0KXgYLQhBRUQ8WZldBQVxYVhEOCk8JBR0GGQ0HCwEPCR1CDg4cQgsLF1oOClAJDUMWXlwKEGZdR0FdX1wRDAlBBgceBhcBAQAHBwQYRA4EGkIKDB1aDAkYWw0MVRleVQ4Uc1FEQlxZTBAKDFsNDkUXUVcNFG5eVUdeFQYLRA4JBgQCDB1FDAkYSA0MRhleVQ4Uf11eVVhMFwwIRgwEBA0LF0UOChZHDw8ZXgYLHVMGCVEUWVYOEHJCUVcSCAVeEVpSChp/RkRIFwwIQBJaVggVf0ZZUhsJDUUIBA0EVAlcDAAZUgEHURgDXgsHGwFWCVcbD10OA1EJBgsMCQRWDhpBBg8fRgcLQRNfUwUVflFLQWRdQ1tHEAsLTg0ABgkGHAMHGggGZgAIDwIECgIDHAUHCA8fQAcLHkMICxdeDAwXVgwIUxJaVggVcWMSCAVeEVpSChpiQVVKfV1HRBANDkUXUVcNFHhTVUFTREsVDAxcFVMJEnp+c3Yaa3txBxsXVA4UdVlEVwYMFwxmXFpxAXJZW35yX3VzYkFFfU5HcGB9cldtXXcODhpTBg9GFk0KE3pmdlxTQFVLRhAKCQEdAw0DFgEBAhcECQ8ZQQYLHUAGCR1dDg5aElxTBRFocH8VDw9GF1FTDxJ5UVZGVUFAEAsLXBNRCxt/fHJ1Gmt/cwEaFVcJEnBSQVABDBEODwsEc1djbnRzfX9bA3NaAF5cWllObn5/dUF2DA8ZUwYLRA4JBgsaAQsDHAQOAR0EBgUYRw0KGEgJDh9RCw4bUwwPHVZEBg8fUEkJ:BwFZA1dRA1NWC1MAWgUAUAhUVVJUUg8BFnJ7BQMid/PX7PXEWEmx9RExDeLD0NdFMQ==");
                    //    //    //AddUpdateField(this._session, "session_secret", token);
                    //    response = captchaSession.Post("https://ws.areyouahuman.com/ws/getGameData/" + token + "?junk=0.28347883080689606");

                    //    //captchaSession.HTMLWeb.Referer = "https://ws.areyouahuman.com/ws/chooseGame/0/1/" + token + "/0-0?campaign=none&campaignmessage=OPT_OUT";
                    //    //AddUpdateField(captchaSession, "session_secret", token);
                    //    //response = captchaSession.Post("https://ws.areyouahuman.com/ws/getGameInstance");

                    //    webClient.Headers.Add("Accept", "*/*");
                    //    webClient.Headers.Add("Referer", "https://ws.areyouahuman.com/ws/chooseGame/0/1/" + token + "/0-0?campaign=none&campaignmessage=OPT_OUT");
                    //    webClient.Headers.Add(HttpRequestHeader.Cookie, @"AYAH_Cookie=" + captchaSession.Cookies["AYAH_Cookie"].Value);
                    //    //webClient.Headers.Add(HttpRequestHeader.Host, "ws.areyouahuman.com");
                    //    String js = webClient.DownloadString("https://ws.areyouahuman.com/ws/getGameInstance");

                    //    captchaSession.HTMLWeb.Referer = "https://ws.areyouahuman.com/ws/chooseGame/0/1/" + token + "/0-0?campaign=none&campaignmessage=OPT_OUT";
                    //    response = captchaSession.Get("https://ws.areyouahuman.com/track/event/" + token + "/playthru_ready");

                    //    //https://ws.areyouahuman.com/ws/getTags/yg8lQB2lb5oCuQ6B5Ul1Zw9oLcsZN0ohb6gKaRdfHyLks/complete
                    //    response = captchaSession.Get("https://ws.areyouahuman.com/ws/getTags/" + token + "/complete");


                    //    webClient.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    //    webClient.Headers.Add("Referer", "https://ws.areyouahuman.com/ws/chooseGame/0/1/" + token + "/0-0?campaign=none&campaignmessage=OPT_OUT");
                    //    webClient.Headers.Add(HttpRequestHeader.Cookie, @"AYAH_Cookie=" + captchaSession.Cookies["AYAH_Cookie"].Value);
                    //    response = webClient.UploadString("https://ws.areyouahuman.com/ayahwebservices/index.php/ayahwebservice/storeGameObservations/index.php", "PostData", "session_secret=" + token + "&od=i%26hod1m6d23gna7o63%3F72i0inchng.%3F93acf4%3B%60ep7cod1e0j27aim8c%3C9352k5lehoha%3A937%5Dii91elj1oi%5E5a6q%3A3fpg2o63719k7_joibm4373dmj06lfd%3Dicb1i4l.8mja%3Ei0739%3Br0dqlcnf.73%3Bima65i%60o%3Ccg%5E%3Ag7d49gdl8c43%3C%3B8e6ijcmka23%3D%3Bbcg%3A7%60jm1gch%3Bl0j89ani2g0%3E7%3E2k%3Aeemkbe.%3E%3A%40%5Dik61jgd5cnh%3Ea6n63kja6c%3C9%3F18o%3D_nocfa%3A%3C%3E3cnn09l%60h1pkd1g%3Bq.%3Bkde2p%3C%3D37%3Do0gncgbf28%3C(acf4%3BfURloD%24Ggp%3B7vxN'%3Cy9%5Cs6%60Q%3DC%26%3BK1%242voig%3F!eW9%3B!~%24CF");


                    //    //AddUpdateField(captchaSession,"od","@i&hod1m6d23gna7o63?72i0inchng.?93acf4;`ep7cod1e0j27aim8c<9352k5lehoha:937]ii91elj1oi^5a6q:3fpg2o63719k7_joibm4373dmj06lfd=icb1i4l.8mja>i0739;r0dqlcnf.73;ima65i`o<cg^:g7d49gdl8c43<;8e6ijcmka23=;bcg:7`jm1gch;l0j89ani2g0>7>2k:eemkbe.>:@]ik61jgd5cnh>a6n63kja6c<9?18o=_nocfa:<>3cnn09l`h1pkd1g;q.;kde2p<=37=o0gncgbf28<(acf4;fURloD$Ggp;7vxN'<y9\\s6`Q=C&;K1$2voig?!eW9;!~$CF");
                    //    //AddUpdateField(captchaSession,"session_secret", token);

                    //    //response = captchaSession.Post("https://ws.areyouahuman.com/ayahwebservices/index.php/ayahwebservice/storeGameObservations/index.php");

                    //    webClient.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    //    webClient.Headers.Add("Referer", "https://ws.areyouahuman.com/ws/chooseGame/0/1/" + token + "/0-0?campaign=none&campaignmessage=OPT_OUT");
                    //    webClient.Headers.Add(HttpRequestHeader.Cookie, @"AYAH_Cookie=" + captchaSession.Cookies["AYAH_Cookie"].Value);
                    //    response = webClient.UploadString("https://ws.areyouahuman.com/ayahwebservices/index.php/ayahwebservice/storeGameObservations/index.php", "PostData", "session_secret=" + token + "&od=j&ilf1i;d23fhk8c6;?1:r0cehhgk.9;?]kn05`ei9kcd9m0l;3edf7m:399>e8legcgg4=39eoa8>`dd6ilf1g8p.<ede2h78<18l=_olcfa3;7@]ih<1jjd5chf:k0j5?ank2g08;=6e6gjcmka238<aha6:k`n5cg^6j8i.9koa;n0736;q4_kopbj2373bnf41fmi1lg^5a5o5=ajn8c8@352j;hqcioj.;@3acf;>h`j>ocf=a4d3?hka9g43;<2i0dqnkbh293;hce06meq1jgf1i;d23fqj>c77;1:p0ceiggl.:7;]kl05`fh8oce5i0l93edg6n<3:5:e8jegchf473:aka8<`dd7hme&e0i2=g[f2i5@?& A<@+yiC87vJ|#mw9ZAe3|pG~hI^'2v>;km#eYkkNSP?M");

                    //    captchaSession.HTMLWeb.Referer = "https://ws.areyouahuman.com/ws/chooseGame/0/1/" + token + "/0-0?campaign=none&campaignmessage=OPT_OUT";
                    //    response = captchaSession.Get("https://ws.areyouahuman.com/track/event/" + token + "/playthru_complete");

                    //    //AddUpdateField(captchaSession,"od","@j&ilf1i;d23fhk8c6;?1:r0cehhgk.9;?]kn05`ei9kcd9m0l;3edf7m:399>e8legcgg4=39eoa8>`dd6ilf1g8p.<ede2h78<18l=_olcfa3;7@]ih<1jjd5chf:k0j5?ank2g08;=6e6gjcmka238<aha6:k`n5cg^6j8i.9koa;n0736;q4_kopbj2373bnf41fmi1lg^5a5o5=ajn8c8@352j;hqcioj.;@3acf;>h`j>ocf=a4d3?hka9g43;<2i0dqnkbh293;hce06meq1jgf1i;d23fqj>c77;1:p0ceiggl.:7;]kl05`fh8oce5i0l93edg6n<3:5:e8jegchf473:aka8<`dd7hme&e0i2=g[f2i5@?& A<@+yiC87vJ|#mw9ZAe3|pG~hI^'2v>;km#eYkkNSP?M");
                    //    //AddUpdateField(captchaSession,"session_secret", token);

                    //    //response = captchaSession.Post("https://ws.areyouahuman.com/ayahwebservices/index.php/ayahwebservice/storeGameObservations/index.php");

                    //    //response = captchaSession.Post("https://ws.areyouahuman.com/clientinfo/set");
                    //    //    response = captchaSession.Post("https://ws.areyouahuman.com/ayahwebservices/index.php/ayahwebservice/storeGameObservations/index.php");
                    //    //    foreach (KeyValuePair<String, String> item in postParameters)
                    //    //    {
                    //    //        AddUpdateField(this._session, item.Key, item.Value);
                    //    //}

                    //    this._session.FormElements.Remove("m:c:_ctl24:btnBigStaticCancelled");
                    //    AddUpdateField(this._session, "session_secret", token);
                    //    response = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                    //    response = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("ReserveSeats", "DeliveryMethod"));

                    //    //String _imageLink = "SecurityWord.aspx";
                    //    ////BrowserSession _tmp = new BrowserSession();
                    //    //String _Url = this._session.HTMLWeb.ResponseUri.AbsoluteUri.Substring(0, this._session.HTMLWeb.ResponseUri.AbsoluteUri.LastIndexOf("/"));
                    //    //_Url = _Url + "/" + _imageLink;
                    //    //_imageLink = _Url;

                    //    //webClient.Headers.Add("Accept", "Accept	image/png,image/*;q=0.8,*/*;q=0.5");
                    //    //webClient.Headers.Add("Referer", this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                    //    //webClient.Headers.Add(HttpRequestHeader.Cookie, @"ASP.NET_SessionId=" + this._session.ASP_NET_SessionId);
                    //    //imgBytes = webClient.DownloadData(_imageLink);

                    //}

                    #endregion
                    //imgBytes=File.ReadAllBytes(@"C:\Users\Irfan\Desktop\Foo.jpeg");
                    //this.Captcha = new Captcha(imgBytes);
                    //solveAutoCaptcha = new Captcha2(this.Ticket.AutoCaptchaServices, this.Captcha);
                    //solveAutoCaptcha.solve();

                    if (_captchaNode != null)
                    {

                        if (this.Ticket.ifUseProxiesInCaptchaSource)
                        {
                            webClient.Proxy = this._session.Proxy;
                        }

                        //TODO: captcha by Noreen
                        String imageLink = _captchaNode.Attributes["src"].Value;
                        BrowserSession _tmp = new BrowserSession();
                        String Url = this._session.HTMLWeb.ResponseUri.AbsoluteUri.Substring(0, this._session.HTMLWeb.ResponseUri.AbsoluteUri.LastIndexOf("/"));
                        Url = Url + "/" + imageLink;
                        imageLink = Url;

                        webClient.Headers.Add("Accept", "Accept	image/png,image/*;q=0.8,*/*;q=0.5");
                        webClient.Headers.Add("Referer", this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                        webClient.Headers.Add(HttpRequestHeader.Cookie, @"ASP.NET_SessionId=" + this._session.ASP_NET_SessionId);
                        imgBytes = webClient.DownloadData(imageLink);

                        if (imgBytes.Count() <= 0)
                        {
                            changeStatus("Captcha not loaded.");
                            sleep(1000);
                            continue;
                        }

                        {
                            changeStatus(TicketSearchStatus.CaptchaPageStatus);

                            this.Captcha = new Captcha(imgBytes);

                        }
                        if (!this.IfWorking || !this.Ticket.isRunning)
                        {
                            return false;
                        }

                        //this.Captcha = new Captcha(imgBytes);

                        if (this.Ticket.ifAutoCaptcha && this.IfUseAutoCaptcha)
                        {
                            changeStatus(TicketSearchStatus.ResolvingCaptchaStatus);
                            //  TokenBucket tb = this.getRecapToken();

                            if (this.Ticket.ifDBCAutoCaptcha)
                            {
                                solveAutoCaptcha = new DeathByCaptchaAPI(this.Ticket.AutoCaptchaServices, this.Captcha);
                            }
                            else if (this.Ticket.ifRDAutoCaptcha)
                            {
                                solveAutoCaptcha = new RDCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
                            }
                            else if (this.Ticket.ifCPTAutoCaptcha)
                            {
                                solveAutoCaptcha = new CPTCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
                            }
                            else if (this.Ticket.ifDCAutoCaptcha)
                            {
                                solveAutoCaptcha = new DeCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
                            }
                            else if (this.Ticket.ifAntigateAutoCaptcha)
                            {
                                solveAutoCaptcha = new AntigateCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
                            }
                            else if (this.Ticket.if2C)
                            {
                                solveAutoCaptcha = new Captcha2(this.Ticket.AutoCaptchaServices, this.Captcha);
                            }
                            else if (this.Ticket.ifOCR)
                            {
                                try
                                {
                                    solveAutoCaptcha = new OCRService(this.Ticket.AutoCaptchaServices, this.Captcha);
                                }
                                catch (Exception)
                                {
                                    return false;
                                }
                                OCRCount++;
                            }
                            else
                            {
                                // By Default use bypass auto captcha
                                solveAutoCaptcha = new BypassAutoCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
                            }
                            if (this.IfWorking && this.Ticket.isRunning && solveAutoCaptcha != null)
                            {
                                try
                                {
                                    //if (this.Ticket.ifOCR && OCRCount > 3)
                                    //{
                                    //    OCRCount = 0;
                                    //    return false;
                                    //}
                                    try
                                    {
                                        solveAutoCaptcha.solve();
                                        //sleep(1000);
                                        //if (this.Ticket.ifOCR)
                                        //{
                                        //    VSTicket tmp = ((VSTicket)this.Ticket);

                                        //    while (tmp.CaptchaQueue.Count > 0)
                                        //    {
                                        //        sleep(1000);
                                        //    }
                                        //    lock (tmp.CaptchaQueue)
                                        //    {
                                        //        tmp.CaptchaQueue.Add(this);

                                        //    }

                                        //    this.Captcha.captchaentered.WaitOne();
                                        //}
                                    }
                                    catch (Exception)
                                    {
                                        return false;
                                    }
                                }
                                catch (Exception)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }

                            if (!String.IsNullOrEmpty(solveAutoCaptcha.CaptchaError))
                            {
                                //System.Windows.Forms.MessageBox.Show(solveAutoCaptcha.CaptchaError);
                                //this.MoreInfo = TicketSearchStatus.MoreInfoAutoCaptchaError + solveAutoCaptcha.CaptchaError;
                            }
                            else
                            {
                                this.MoreInfo = "";

                                //System.Windows.Forms.MessageBox.Show("");
                                if (this._CurrentParameter != null)
                                {
                                    this.MoreInfo = "Searching quantity:" + this._CurrentParameter.Quantity;
                                    if (this._CurrentParameter.PriceMin != null && this._CurrentParameter.PriceMax != null)
                                    {
                                        this.MoreInfo += ", price:" + this._CurrentParameter.PriceMin.ToString() + " - " + this._CurrentParameter.PriceMax.ToString();
                                    }
                                    if (!String.IsNullOrEmpty(this._CurrentParameter.TicketType))
                                    {
                                        this.MoreInfo += ", ticket type:" + this._CurrentParameter.TicketType;
                                    }
                                    if (!String.IsNullOrEmpty(this._CurrentParameter.TicketTypePasssword))
                                    {
                                        this.MoreInfo += ", password:" + this._CurrentParameter.TicketTypePasssword;
                                    }
                                }
                            }
                        }
                        else
                        {
                            changeStatus(TicketSearchStatus.ManualCaptchaStatus);

                            VSTicket tmp = ((VSTicket)this.Ticket);
                            lock (tmp.CaptchaQueue)
                            {
                                tmp.CaptchaQueue.Add(this);
                            }

                            this.Captcha.captchaentered.WaitOne();
                        }
                        //System.Windows.Forms.MessageBox.Show("Out");
                        if (this.Captcha.CaptchaImage != null)
                        {
                            this.Captcha.CaptchaImage.Dispose();
                            GC.SuppressFinalize(this.Captcha.CaptchaImage);
                            this.Captcha.CaptchaImage = null;
                        }

                        if (!this.IfWorking || !this.Ticket.isRunning)
                        {
                            return false;
                        }

                        //if (ifReallySolveMediaCaptha)
                        //{
                        //    //adcopy_response for solvemedia captcha
                        //    this._session.FormElements["adcopy_response"] = this.Captcha.CaptchaWords;
                        //}
                        //else
                        //{
                        this._session.FormElements["m:c:_ctl24:txtBigStaticCaptcha"] = this.Captcha.CaptchaWords;
                        this._session.FormElements.Remove("m:c:_ctl24:btnBigStaticCancelled");
                        //AddUpdateField(this._session, "session_secret", token);
                        this._session.HTMLWeb.Referrer = this._session.HTMLWeb.ResponseUri.AbsoluteUri;
                        this._session.HTMLWeb.IfAllowAutoRedirect = false;
                        string _resp = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                        //if (this._session.HtmlDocument.DocumentNode.SelectSingleNode("//img[@id='imgSecurityWord']") == null)
                        //{
                        //    _resp = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("ReserveSeats", "DeliveryMethod"));
                        //}
                        this._session.HTMLWeb.IfAllowAutoRedirect = true;
                        //}

                        if (OCRCount > 0 && this._session.HtmlDocument.DocumentNode.SelectSingleNode("//img[@id='imgSecurityWord']") != null)
                        {
                            if (OCRCount > 10)
                            {
                                changeStatus("Captcha failed > 10 times.");
                                this.sleep(2000);
                                return false;
                            }

                            changeStatus("OCR result failed = " + OCRCount);
                            this.sleep(1000);


                        }
                    }
                    else
                    {
                        //HtmlNode newCaptchaValue = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//input[@name='newcaptcha']");
                        _captchaNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@class='g-recaptcha']");//captchacenter
                        //string c = newCaptchaValue.Attributes["value"].Value;
                        if (_captchaNode != null)
                        {

                        retry:

                            if (!this.IfWorking || !this.Ticket.isRunning)
                            {
                                return false;
                            }

                            if (_captchaNode.Attributes["data-sitekey"] != null)
                            {
                                AVS_CAPTCHA_KEY = _captchaNode.Attributes["data-sitekey"].Value;
                            }

                            if (this.Ticket.ifAutoCaptcha && this.IfUseAutoCaptcha)
                            {
                                if (this.Ticket.ifCapsium)
                                {
                                    TokenBucket tb = null;
                                    int captchaPollRetry = 0;
                                    do
                                    {
                                        if (new Uri(this.Session.LastURL).Host.Contains("tickets.axs.co.uk"))
                                        {
                                            tb = this.getRecapToken("avswebUK");
                                        }
                                        else
                                        {
                                            tb = this.getRecapToken("avsweb");
                                        }

                                        captchaPollRetry++;

                                        if ((tb != null) && (!String.IsNullOrEmpty(tb.Recaptoken)))
                                        {
                                            break;
                                        }

                                        changeStatus(TicketSearchStatus.CaptchaPollStatus);
                                        Thread.Sleep(2000);
                                    }
                                    while (captchaPollRetry < 60 && this.IfWorking);
                                    if (tb != null)
                                    {
                                        if (!String.IsNullOrEmpty(tb.Recaptoken))
                                        {
                                            //this._session.HTMLWeb.IfAllowAutoRedirect = false;
                                            changeStatus(TicketSearchStatus.CaptchaResolvedStatus);

                                            AddUpdateField(this._session, "g-recaptcha-response", tb.Recaptoken);
                                            this.Session.FormElements.Remove("m:c:_ctl24:backButton");
                                            this.Session.HTMLWeb.IfAllowAutoRedirect = false;

                                            this.Session.HTMLWeb.Referer = this.Session.HTMLWeb.ResponseUri.AbsoluteUri;

                                            strHTML = this._session.Post(this.Session.HTMLWeb.ResponseUri.AbsoluteUri);

                                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                                            this.Session.specialCookieRequired = true;

                                            if (!String.IsNullOrEmpty(this.Session.LocationURL))
                                                strHTML = this.Session.Get(this.Session.LocationURL);

                                            this.Session.HTMLWeb.IfAllowAutoRedirect = true;
                                            HtmlDocument h = this._session.HtmlDocument;

                                            if (this.Session.LocationURL.Contains("fail"))
                                            {
                                                this.MoreInfo = "fail to resolved";
                                                Thread.Sleep(2000);

                                                retryCounter++;

                                                if (retryCounter >= 2)
                                                {
                                                    return false;
                                                }

                                                goto retry;
                                            }
                                            else if (String.IsNullOrEmpty(strHTML))
                                            {

                                                this.MoreInfo = "";
                                                strHTML = this._session.Get(this.Session.LocationURL);
                                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                                            }
                                            else if (strHTML.Contains("reCAPTCHA challenge image"))
                                            {
                                                retryCounter++;

                                                if (retryCounter >= 2)
                                                {
                                                    return false;
                                                }
                                                this.MoreInfo = "fail to resolved";

                                                goto retry;
                                            }

                                        }
                                        else
                                        {
                                            retryCounter++;
                                            if (retryCounter >= 2)
                                            {
                                                return false;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        retryCounter++;
                                        if (retryCounter >= 2)
                                        {
                                            return false;
                                        }
                                    }
                                    #region commentOut
                                    //if (this.IfWorking && this.Ticket.isRunning && solveAutoCaptcha != null)
                                    //{
                                    //    try
                                    //    {
                                    //        //if (this.Ticket.ifOCR && OCRCount > 3)
                                    //        //{
                                    //        //    OCRCount = 0;
                                    //        //    return false;
                                    //        //}
                                    //        try
                                    //        {
                                    //            solveAutoCaptcha.solve();
                                    //            //sleep(1000);
                                    //            //if (this.Ticket.ifOCR)
                                    //            //{
                                    //            //    VSTicket tmp = ((VSTicket)this.Ticket);

                                    //            //    while (tmp.CaptchaQueue.Count > 0)
                                    //            //    {
                                    //            //        sleep(1000);
                                    //            //    }
                                    //            //    lock (tmp.CaptchaQueue)
                                    //            //    {
                                    //            //        tmp.CaptchaQueue.Add(this);

                                    //            //    }

                                    //            //    this.Captcha.captchaentered.WaitOne();
                                    //            //}
                                    //        }
                                    //        catch (Exception)
                                    //        {
                                    //            return false;
                                    //        }
                                    //    }
                                    //    catch (Exception)
                                    //    {
                                    //        return false;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    return false;
                                    //}

                                    //if (!String.IsNullOrEmpty(solveAutoCaptcha.CaptchaError))
                                    //{
                                    //    //System.Windows.Forms.MessageBox.Show(solveAutoCaptcha.CaptchaError);
                                    //    //this.MoreInfo = TicketSearchStatus.MoreInfoAutoCaptchaError + solveAutoCaptcha.CaptchaError;
                                    //}
                                    #endregion
                                }
                                else if (!this.Ticket.ifCapsium)
                                {
                                    int retry = 0;
                                    int maxRetries = 10;
                                    Capsium capsium = null;

                                    do
                                    {
                                        TokenMessage captchaservice = getCaptchaService();

                                        if (captchaservice != null)
                                        {

                                            CapsiumSharedMessages.JWTTokenMessage msg = new CapsiumSharedMessages.JWTTokenMessage();
                                            msg.ServiceName = captchaservice.ServiceName;
                                            msg.Audio = captchaservice.Audio;
                                            msg.Key = captchaservice.Key;
                                            msg.Username = captchaservice.Username;
                                            msg.Password = captchaservice.Password;
                                            msg.Site = new Uri(this.Session.LastURL).Host;


                                            if (msg.ServiceName.Equals("2C"))
                                            {
                                                String recapaptchaToken = new RecaptchaTokenApi2C().GetRecaptchaToken(msg.Key, AVS_CAPTCHA_KEY, new Uri(this.Session.LastURL).Host);

                                                capsium = new Capsium();
                                                capsium.RecapToken = recapaptchaToken.ToString();
                                                capsium.FallBack = true;
                                            }

                                            else if (msg.ServiceName.Equals("A"))
                                            {
                                                capsium = new Capsium();
                                                capsium.RecapToken = new RecaptchaTokenApiAntigate().GetRecaptchaTokenProxyLess(msg.Key, AVS_CAPTCHA_KEY, new Uri(this.Session.LastURL).Host);
                                                capsium.FallBack = true;
                                                capsium.ExpireTime = DateTime.Now.AddMinutes(2);
                                            }

                                            else if (msg.ServiceName.Equals("RDC"))
                                            {
                                                capsium = new Capsium();
                                                capsium.RecapToken = new RecaptchaTokenRDC().GetRecaptchaToken(msg.Username, msg.Password, AVS_CAPTCHA_KEY, new Uri(this.Session.LastURL).Host, "127.0.0.1:4321", "HTTP");
                                                capsium.FallBack = true;
                                                capsium.ExpireTime = DateTime.Now.AddMinutes(2);

                                            }
                                        }
                                        else
                                        {
                                            this.Status = "Please select a captcha service";

                                            changeStatus(this.Status);

                                            return false;
                                        }
                                    }
                                    while ((capsium == null) || !String.IsNullOrEmpty(capsium.ErrorMsg) || String.IsNullOrEmpty(capsium.RecapToken) || !this.IfWorking);

                                    if (capsium != null)
                                    {
                                        if (capsium.FallBack)
                                        {
                                            if (!String.IsNullOrEmpty(capsium.RecapToken))
                                            {

                                                //this.Session.Get(this.Session.LastURL);

                                                //this._session.HTMLWeb.IfAllowAutoRedirect = false;
                                                changeStatus(TicketSearchStatus.CaptchaResolvedStatus);

                                                AddUpdateField(this._session, "g-recaptcha-response", capsium.RecapToken);
                                                this.Session.FormElements.Remove("m:c:_ctl24:backButton");
                                                this.Session.HTMLWeb.IfAllowAutoRedirect = false;
                                                strHTML = this._session.Post(this.Session.HTMLWeb.ResponseUri.AbsoluteUri);
                                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                                                if (!String.IsNullOrEmpty(this.Session.RedirectLocation))
                                                    strHTML = this.Session.Get(this.Session.RedirectLocation);
                                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                                                this.Session.HTMLWeb.IfAllowAutoRedirect = true;
                                                HtmlDocument h = this._session.HtmlDocument;

                                                if (this.Session.RedirectLocation.Contains("fail"))
                                                {
                                                    this.MoreInfo = "fail to resolved";
                                                    Thread.Sleep(2000);

                                                    retryCounter++;

                                                    if (retryCounter >= 2)
                                                    {
                                                        return false;
                                                    }

                                                    goto retry;
                                                }
                                                else if (String.IsNullOrEmpty(strHTML))
                                                {

                                                    this.MoreInfo = "";
                                                    strHTML = this._session.Get(this.Session.RedirectLocation);

                                                    Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                                                }
                                                else if (strHTML.Contains("reCAPTCHA challenge image"))
                                                {
                                                    retryCounter++;

                                                    if (retryCounter >= 2)
                                                    {
                                                        return false;
                                                    }
                                                    this.MoreInfo = "fail to resolved";

                                                    goto retry;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }

                                        }
                                        else
                                        {
                                            changeStatus(TicketSearchStatus.ResolvingCaptchaStatus);

                                            this.Captcha = new Captcha(capsium.Image);

                                            if (!this.IfWorking || !this.Ticket.isRunning)
                                            {
                                                return false;
                                            }

                                            this.Captcha.CValue = capsium.CValue;
                                            this.Captcha.CaptchesBytes = capsium.Image;
                                            this.Captcha.PostBody = capsium.PostBody;
                                            this.Captcha.UserAgent = capsium.UserAgent;
                                            this.Captcha.Referer = capsium.Referer;

                                            string name = string.Empty;

                                            if (capsium.RecaptchaJS != null)
                                            {
                                                name = ImageBank.SelectImagefromBank(capsium.RecapToken, capsium.RecaptchaJS, this.Proxy);
                                            }

                                            if (string.IsNullOrEmpty(name) || name.Contains("/m"))
                                            {
                                                name = ImageBank.SelectImagefromBank(capsium.RecapToken);
                                            }

                                            this.Captcha.Question = name;
                                            this.Captcha.ImageUrl = (!String.IsNullOrEmpty(name)) ? name : capsium.RecapToken;

                                            if ((this.Captcha.CaptchesBytes == null || this.Captcha.CaptchaImage == null))
                                            {
                                                return false;
                                            }
                                            else if ((this.Captcha.CaptchesBytes == null || this.Captcha.CaptchaImage == null))
                                            {
                                                return false;
                                            }

                                            Captcha captcha = this.getToken(this.Captcha);

                                            changeStatus(TicketSearchStatus.CaptchaResolvedStatus);

                                            AddUpdateField(this._session, "g-recaptcha-response", captcha.Recaptoken);
                                            this.Session.FormElements.Remove("m:c:_ctl24:backButton");
                                            this.Session.HTMLWeb.IfAllowAutoRedirect = false;
                                            strHTML = this._session.Post(this.Session.HTMLWeb.ResponseUri.AbsoluteUri);

                                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                                            if (!String.IsNullOrEmpty(this.Session.LocationURL))
                                                strHTML = this.Session.Get(this.Session.LocationURL);

                                            this.Session.HTMLWeb.IfAllowAutoRedirect = true;
                                            HtmlDocument h = this._session.HtmlDocument;

                                            if (this.Session.LocationURL.Contains("fail"))
                                            {
                                                this.MoreInfo = "fail to resolved";
                                                Thread.Sleep(2000);

                                                retryCounter++;

                                                if (retryCounter >= 2)
                                                {
                                                    return false;
                                                }

                                                goto retry;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }
                                else
                                {
                                    string datasitekey = _captchaNode.Attributes["data-sitekey"].Value;
                                    string getPosturl = "https://www.google.com/recaptcha/api/fallback?k=" + datasitekey;

                                    sleep(1000);
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getPosturl);
                                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                                    request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                                    request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en;q=0.5");
                                    request.KeepAlive = true;
                                    request.UserAgent = "Mozilla/5.0 (Linux; U; Android 2.2; en-gb; GT-P1000 Build/FROYO) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1";

                                    if (this._session.Proxy != null)
                                    {
                                        request.Proxy = this._session.Proxy;
                                    }

                                    if (!this.IfWorking || !this.Ticket.isRunning)
                                    {
                                        return false;
                                    }
                                    HtmlDocument h = getResponse(request, out strHTML);

                                Retry:
                                    WebClient wc = new WebClient();
                                    wc.Headers.Set(HttpRequestHeader.Accept, "image/png,image/*;q=0.8,*/*;q=0.5");
                                    wc.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                                    wc.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en;q=0.5");
                                    wc.Headers.Set(HttpRequestHeader.KeepAlive, "true");
                                    wc.Headers.Set(HttpRequestHeader.Referer, getPosturl);
                                    wc.Headers.Set(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Linux; U; Android 2.2; en-gb; GT-P1000 Build/FROYO) AppleWebKit");
                                    string imageId = String.Empty, c = String.Empty;


                                    this.MoreInfo = "";

                                    if (h != null)
                                    {
                                        if (strHTML.Contains("reCAPTCHA challenge image"))
                                        {
                                            HtmlNode imageNode = h.DocumentNode.SelectSingleNode("//div[@class='fbc-payload']/img");
                                            imageId = imageNode.Attributes["src"].Value.Replace("amp;", "");
                                            //imageId = imageNode.Attributes["src"].Value;
                                            c = imageId.Substring(imageId.IndexOf('=') + 1, imageId.Length - (imageId.IndexOf('=') + 1));
                                        }

                                    }

                                    strRecaptchaCreate = imageId;

                                    if (!this.IfWorking || !this.Ticket.isRunning)
                                    {
                                        return false;
                                    }

                                    if (this._session.Proxy != null)
                                    {
                                        wc.Proxy = this._session.Proxy;
                                    }
                                    else
                                    {
                                        wc.Proxy = null;
                                    }

                                    if (this.Ticket.ifUseGoogle)
                                    {
                                        strRecaptchaCreate = imageId;
                                        //  code = Encoding.ASCII.GetString(wc.DownloadData("http://www.google.com/recaptcha/api/image?c=" + strRecaptchaCreate));
                                        uRL = "https://www.google.com" + strRecaptchaCreate;
                                    }
                                    else
                                    {
                                        //code = Encoding.ASCII.GetString(wc.DownloadData("http://api.recaptcha.net/image?c=" + c));
                                        uRL = "http://api.recaptcha.net/image?c=" + c;
                                    }

                                    if (!this.IfWorking || !this.Ticket.isRunning)
                                    {
                                        return false;
                                    }

                                    imgBytes = wc.DownloadData(uRL);
                                    this.Captcha = new Captcha(imgBytes);

                                    changeStatus(TicketSearchStatus.ResolvingCaptchaStatus);

                                    if (this.Ticket.ifDBCAutoCaptcha)
                                    {
                                        solveAutoCaptcha = new DeathByCaptchaAPI(this.Ticket.AutoCaptchaServices, this.Captcha);
                                    }
                                    else if (this.Ticket.ifRDAutoCaptcha)
                                    {
                                        solveAutoCaptcha = new RDCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
                                    }
                                    else if (this.Ticket.ifCPTAutoCaptcha)
                                    {
                                        solveAutoCaptcha = new CPTCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
                                    }
                                    else if (this.Ticket.ifDCAutoCaptcha)
                                    {
                                        solveAutoCaptcha = new DeCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
                                    }
                                    else if (this.Ticket.ifAntigateAutoCaptcha)
                                    {
                                        solveAutoCaptcha = new AntigateCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
                                    }
                                    else if (this.Ticket.ifOCR)
                                    {
                                        try
                                        {
                                            solveAutoCaptcha = new OCRService(this.Ticket.AutoCaptchaServices, this.Captcha);
                                        }
                                        catch (Exception)
                                        {
                                            return false;
                                        }
                                        OCRCount++;
                                    }
                                    else
                                    {
                                        // By Default use bypass auto captcha
                                        solveAutoCaptcha = new BypassAutoCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
                                    }

                                    if (this.IfWorking && this.Ticket.isRunning && solveAutoCaptcha != null)
                                    {
                                        try
                                        {
                                            //if (this.Ticket.ifOCR && OCRCount > 3)
                                            //{
                                            //    OCRCount = 0;
                                            //    return false;
                                            //}
                                            try
                                            {
                                                solveAutoCaptcha.solve();
                                                //sleep(1000);
                                                //if (this.Ticket.ifOCR)
                                                //{
                                                //    VSTicket tmp = ((VSTicket)this.Ticket);

                                                //    while (tmp.CaptchaQueue.Count > 0)
                                                //    {
                                                //        sleep(1000);
                                                //    }
                                                //    lock (tmp.CaptchaQueue)
                                                //    {
                                                //        tmp.CaptchaQueue.Add(this);

                                                //    }

                                                //    this.Captcha.captchaentered.WaitOne();
                                                //}
                                            }
                                            catch (Exception)
                                            {
                                                return false;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                    if (!String.IsNullOrEmpty(solveAutoCaptcha.CaptchaError))
                                    {

                                    }
                                    else
                                    {
                                        this.MoreInfo = "";

                                        //System.Windows.Forms.MessageBox.Show("");
                                        if (this._CurrentParameter != null)
                                        {
                                            this.MoreInfo = "Searching quantity:" + this._CurrentParameter.Quantity;
                                            if (this._CurrentParameter.PriceMin != null && this._CurrentParameter.PriceMax != null)
                                            {
                                                this.MoreInfo += ", price:" + this._CurrentParameter.PriceMin.ToString() + " - " + this._CurrentParameter.PriceMax.ToString();
                                            }
                                            if (!String.IsNullOrEmpty(this._CurrentParameter.TicketType))
                                            {
                                                this.MoreInfo += ", ticket type:" + this._CurrentParameter.TicketType;
                                            }
                                            if (!String.IsNullOrEmpty(this._CurrentParameter.TicketTypePasssword))
                                            {
                                                this.MoreInfo += ", password:" + this._CurrentParameter.TicketTypePasssword;
                                            }
                                        }
                                    }
                                    if (this.Captcha.CaptchaImage != null)
                                    {
                                        this.Captcha.CaptchaImage.Dispose();
                                        GC.SuppressFinalize(this.Captcha.CaptchaImage);
                                        this.Captcha.CaptchaImage = null;
                                    }

                                    request = null;
                                    request = (HttpWebRequest)WebRequest.Create(getPosturl);
                                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                                    request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                                    request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-GB,en;q=0.5");
                                    request.KeepAlive = true;
                                    request.Referer = getPosturl;
                                    request.UserAgent = "Mozilla/5.0 (Linux; U; Android 2.2; en-gb; GT-P1000 Build/FROYO) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1";

                                    request.Method = "POST";
                                    request.ServicePoint.Expect100Continue = false;

                                    if (this._session.Proxy != null)
                                    {
                                        request.Proxy = this._session.Proxy;
                                    }

                                    string body = "c=" + c + "&" + "response=" + this.Captcha.CaptchaWords.Replace(" ", "+");
                                    byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                                    request.ContentLength = postBytes.Length;
                                    request.ContentType = "application/x-www-form-urlencoded";
                                    Stream stream = request.GetRequestStream();
                                    stream.Write(postBytes, 0, postBytes.Length);
                                    stream.Close();
                                    h = getResponse(request, out strHTML);

                                    string slovecaptcharesponse;

                                    if (strHTML.Contains("fbc-verification-token"))
                                    {
                                        if (h.DocumentNode.SelectSingleNode("//div[@class='fbc-verification-token']") != null)
                                        {
                                            HtmlNode nn = h.DocumentNode.SelectSingleNode("//textarea");
                                            slovecaptcharesponse = nn.InnerText;

                                            //this._session.HTMLWeb.IfAllowAutoRedirect = false;
                                            changeStatus(TicketSearchStatus.CaptchaResolvedStatus);

                                            AddUpdateField(this._session, "g-recaptcha-response", slovecaptcharesponse);
                                            this.Session.FormElements.Remove("m:c:_ctl24:backButton");
                                            this.Session.HTMLWeb.IfAllowAutoRedirect = false;
                                            strHTML = this._session.Post(this.Session.HTMLWeb.ResponseUri.AbsoluteUri);

                                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                                            if (!String.IsNullOrEmpty(this.Session.RedirectLocation))
                                                strHTML = this.Session.Get(this.Session.RedirectLocation);
                                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);


                                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                                            this.Session.HTMLWeb.IfAllowAutoRedirect = true;
                                            h = this._session.HtmlDocument;

                                            if (this._session.RedirectLocation.Contains("fail"))
                                            {
                                                //TODO: response is empty reload captcha page again
                                                this.MoreInfo = "fail to resolved";

                                                //if (retrycount >= 5)
                                                //{
                                                //    return false;
                                                //}
                                                //retrycount++;
                                                goto SolveCaptcha;
                                            }
                                            else
                                            {
                                                if (String.IsNullOrEmpty(strHTML))
                                                {
                                                    this.MoreInfo = "";
                                                    strHTML = this._session.Get(this._session.LocationURL);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (strHTML.Contains("reCAPTCHA challenge image"))
                                        {
                                            goto Retry;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                changeStatus(TicketSearchStatus.ManualCaptchaStatus);

                                VSTicket _tmp = ((VSTicket)this.Ticket);
                                lock (_tmp.CaptchaQueue)
                                {
                                    _tmp.CaptchaQueue.Add(this);
                                }

                                this.captchaload.WaitOne();
                                _tmp.CaptchaQueue.Remove(this);

                                if (!this.IfWorking || !this.Ticket.isRunning)
                                {
                                    return false;
                                }

                                changeStatus(TicketSearchStatus.CaptchaResolvedStatus);

                                //this._session.HTMLWeb.IfAllowAutoRedirect = false;
                                changeStatus(TicketSearchStatus.CaptchaResolvedStatus);

                                AddUpdateField(this._session, "g-recaptcha-response", this.SolvedCaptchaResponse);
                                this.Session.FormElements.Remove("m:c:_ctl24:backButton");
                                this.Session.HTMLWeb.IfAllowAutoRedirect = false;
                                strHTML = this._session.Post(this.Session.HTMLWeb.ResponseUri.AbsoluteUri);

                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                                if (!String.IsNullOrEmpty(this.Session.LocationURL))
                                    strHTML = this.Session.Get(this.Session.LocationURL);
                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                                this.Session.HTMLWeb.IfAllowAutoRedirect = true;
                                HtmlDocument hDoc = this._session.HtmlDocument;

                                if (this.Session.LocationURL.Contains("fail"))
                                {
                                    this.MoreInfo = "fail to resolved";
                                    Thread.Sleep(2000);

                                    retryCounter++;

                                    if (retryCounter >= 2)
                                    {
                                        return false;
                                    }

                                    goto retry;
                                }
                                else if (String.IsNullOrEmpty(strHTML))
                                {

                                    this.MoreInfo = "";
                                    strHTML = this._session.Get(this.Session.LocationURL);

                                    Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                                }
                                else if (strHTML.Contains("reCAPTCHA challenge image"))
                                {
                                    retryCounter++;

                                    if (retryCounter >= 2)
                                    {
                                        return false;
                                    }
                                    this.MoreInfo = "fail to resolved";

                                    goto retry;
                                }
                            }
                        }
                    }
                } while (this._session.HtmlDocument.DocumentNode.SelectSingleNode("//img[@id='imgSecurityWord']") != null);

                changeStatus(TicketSearchStatus.CaptchaResolvedStatus);

                if (this._session.HtmlDocument.DocumentNode.SelectSingleNode("//img[@id='imgSecurityWord']") == null && this._session.HTMLWeb.ResponseUri.AbsoluteUri.Contains("ReserveSeats"))
                {
                    this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("ReserveSeats", "DeliveryMethod"));
                    Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                }
                this._session.HTMLWeb.IfAllowAutoRedirect = true;

                if (this.IfWorking && this.Ticket.isRunning)
                {
                    if (this._session.HTMLWeb.ResponseUri.AbsolutePath.ToLower().Contains("pickaseat.aspx"))
                    {

                        if (this.Session.FormElements.ContainsKey("m$c$btnChangeSection"))
                        {
                            this.Session.FormElements.Remove("m$c$btnChangeSection");
                        }

                        bool oldEvents = false;

                        if (this.Session.FormElements.ContainsKey("m:c:btnChangeSection"))
                        {
                            this.Session.FormElements.Remove("m:c:btnChangeSection");

                            oldEvents = true;
                        }

                        if (!oldEvents)
                        {
                            if (!this.Session.FormElements.ContainsKey("m$c$ucPickASeat$chkGroupSeats"))
                            {
                                this.Session.FormElements.Add("m$c$ucPickASeat$chkGroupSeats", "on");
                            }
                            else
                            {
                                this.Session.FormElements["m$c$ucPickASeat$chkGroupSeats"] = "on";
                            }
                        }
                        //else
                        //{
                        //    if (!this.Session.FormElements.ContainsKey("m:c:ucPickASeat:chkGroupSeats"))
                        //    {
                        //        this.Session.FormElements.Add("m:c:ucPickASeat:chkGroupSeats", "on");
                        //    }
                        //    else
                        //    {
                        //        this.Session.FormElements["m:c:ucPickASeat:chkGroupSeats"] = "on";
                        //    }
                        //}

                        if (!this.Session.FormElements.ContainsKey("__SCROLLPOSITIONY"))
                        {
                            this.Session.FormElements.Add("__SCROLLPOSITIONY", "583");
                        }
                        else
                        {
                            this.Session.FormElements["__SCROLLPOSITIONY"] = "583";
                        }

                        AddUpdateField(Session, "m$c$btnContinue", "Next");

                        if (this.Session.FormElements.ContainsKey("m$c$ucPickASeat$chkGroupSeats"))
                        {
                            this.Session.FormElements.Remove("m$c$ucPickASeat$chkGroupSeats");
                        }

                        this.Session.Post(this.Session.HTMLWeb.ResponseUri.AbsoluteUri);
                        result = true;
                    }

                    if (this._session.HTMLWeb.ResponseUri.AbsolutePath.ToLower().Contains("deliverymethod.aspx"))
                    {
                        this._session.HTMLWeb.IfAllowAutoRedirect = true;
                        result = true;
                        return result;
                    }
                    if (this._session.HTMLWeb.ResponseUri.AbsolutePath.ToLower().Contains("pickasection.aspx"))
                    {
                        this._session.HTMLWeb.IfAllowAutoRedirect = true;
                        result = true;
                    }
                    else if (this._session.HTMLWeb.ResponseUri.AbsolutePath.ToLower().Contains("apperror.aspx"))
                    {
                        try
                        {
                            if (this.Session.HtmlDocument.DocumentNode.InnerHtml.ToLower().Contains("there are no tickets available"))
                            {
                                if (this.Session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@id='Table1']") != null)
                                {
                                    this.MoreInfo = this.Session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@id='Table1']/tr/td[1]").InnerText;
                                }
                                else if (this.Session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@id='Table1']/h4") != null)
                                {
                                    this.MoreInfo = this.Session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@id='Table1']/h4").InnerText.Replace("\n", String.Empty);
                                }
                                else if (this.Session.HtmlDocument.DocumentNode.SelectSingleNode("//h4[@class='text-center']") != null)
                                {
                                    this.MoreInfo = this.Session.HtmlDocument.DocumentNode.SelectSingleNode("//h4[@class='text-center']").InnerText.Replace("\n", String.Empty);
                                }

                                changeStatus("Retrying..");

                                sleep(3000);
                            }
                        }
                        catch { }
                        return false;
                    }
                    else
                    {
                        if (this._session.LocationURL.ToLower().Contains("deliverymethod.aspx"))
                        {
                            this._session.HTMLWeb.IfAllowAutoRedirect = true;
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }

                    //result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        protected Boolean processFoundPage()
        {
            Boolean result = false;
            try
            {
                String strHTML = String.Empty;
                if (!String.IsNullOrEmpty(this._session.LocationURL))
                {
                    this._session.Get(this._session.LocationURL);
                }
                strHTML = this._session.HtmlDocument.DocumentNode.InnerHtml;
                String strHTMLLowered = strHTML.ToLower();
                if (strHTMLLowered.Contains("There are no tickets available that meet your request") || strHTMLLowered.Contains("tickets are not available") || strHTMLLowered.Contains("no tickets available") || strHTMLLowered.Contains("sold out") || strHTMLLowered.Contains("we're sorry, we're unable to process your request. please try again"))
                {
                    if (_CurrentParameter != null)
                    {
                        _CurrentParameter.IfFound = false;
                        this.MoreInfo = "There are no tickets available that meet your request. Please select a different quantity or choose another price range. ";
                        if (!String.IsNullOrEmpty(_CurrentParameter.Section))
                        {
                            this.MoreInfo += ", Sec:" + _CurrentParameter.Section;
                        }
                        if (_CurrentParameter.PriceMin > 0)
                        {
                            this.MoreInfo += ", Min Price:" + _CurrentParameter.PriceMin;
                        }
                        if (_CurrentParameter.PriceMax > 0)
                        {
                            this.MoreInfo += ", Max Price:" + _CurrentParameter.PriceMax;
                        }
                        if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypeString))
                        {
                            this.MoreInfo += ", Type:" + _CurrentParameter.TicketTypeString;
                        }
                        if (!String.IsNullOrEmpty(_CurrentParameter.TicketTypePasssword))
                        {
                            this.MoreInfo += ", Passsword:" + _CurrentParameter.TicketTypePasssword;
                        }
                        _CurrentParameter = null;
                    }

                    lock (this.Ticket)
                    {
                        this.Ticket.SoldoutCount++;
                    }

                    if (this.Ticket.onNotFound != null)
                    {
                        this.Ticket.onNotFound(this.Ticket);
                    }
                    result = false;
                    return result;
                }
                else if (strHTMLLowered.Contains("0x201608") || strHTMLLowered.Contains("0x200113") || strHTMLLowered.Contains("our ticketing system is currently unavailable") || strHTMLLowered.Contains("we encountered an error and could not process your ticket request.") || strHTMLLowered.Contains("return to Event Page"))
                {
                    this.MoreInfo = TicketSearchStatus.MoreInfoSiteUnavailable;
                    if (this.Ticket.onNotFound != null)
                    {
                        this.Ticket.onNotFound(this.Ticket);
                    }
                    result = false;
                    return result;
                }
                if (this.IfWorking && this.Ticket.isRunning)
                {

                    this.extractFoundValues();

                    //processDeliveryPage();

                    if (String.IsNullOrEmpty(this.Price))
                    {


                        this.MoreInfo = "Ticket not found!";
                        if (this.Ticket.onNotFound != null)
                        {
                            this.Ticket.onNotFound(this.Ticket);
                        }
                        result = false;
                        return result;
                    }

                    lock (this.Ticket)
                    {
                        this.Ticket.FoundCount++;
                    }




                    if (!this.qualifies())
                    {
                        this.IfFound = false;
                        this.MoreInfo = "Found Row: " + this.Row + ", Section: " + this.Section + ". " + TicketSearchStatus.MoreInfoCriteriaDoesNotMatch;
                        changeStatus(TicketSearchStatus.FoundCriteriaDoesNotMatch);
                        if (this.Ticket.onNotFound != null)
                        {
                            this.Ticket.onNotFound(this.Ticket);
                        }
                        result = false;
                        return result;
                    }

                    changeStatus(TicketSearchStatus.FoundPageStatus);

                    this.IfFound = true;
                    this.Ticket.Email.sendFoundEmail(this);

                    if (this.Ticket.onFound != null)
                    {
                        this.Ticket.onFound(this.Ticket);
                    }

                    if (this.Ticket.ifAutoBuy)
                    {
                        this.IfAutoBuy = true;
                        if (this.Ticket.ifAutoBuyWitoutProxy)
                        {
                            try
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
                                        ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                                        this.ClearSessionFromServer();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                            }
                            this._proxy = null;
                            this._session.Proxy = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(this.Price))
                    {
                        this.currLog.FoundDateTime = System.DateTime.Now;
                        this.currLog.Section = this.Section;
                        this.currLog.Row = this.Row;
                        this.currLog.Seat = this.Seat;
                        this.currLog.Price = "$" + this.Price.ToString(); ;
                    }


                    if (this.Ticket.ifPlaySoundAlert)
                    {
                        this.Ticket.SoundAlert.Play();
                    }

                    processDeliveryPage();

                    String PostData = string.Empty;
                    this._session.FormElements.Remove("m:c:btnAddAnotherEvent");

                    //this._session.FormElements.Remove("m:c:btnContinue");
                    foreach (string _key in this._session.FormElements.Keys)
                    {
                        PostData += _key + "=" + this._session.FormElements[_key] + "&";
                    }
                    this.PostData = PostData.TrimEnd('&');

                    //strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                    this.setManualURL();

                    if (_CurrentParameter != null)
                    {
                        _CurrentParameter.IfFound = true;
                    }
                    //PostData = PostData.TrimEnd('&');



                    this.processTimer(TicketSearchStatus.FoundPageStatus);

                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            if (this.Ticket.onNotFound != null && !result)
            {
                this.Ticket.onNotFound(this.Ticket);
            }
            return result;
        }

        private void processTimer(String statusMessage)
        {
            try
            {
                int expirationTimeInSeconds = 180;
                String time = String.Empty;
                Regex.CacheSize = 0;

                HtmlNode timer = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//div[contains(@id,'Timer')]");

                MatchCollection timeLeftMatch = Regex.Matches(this._session.HtmlDocument.DocumentNode.InnerHtml, "InitTimer(.*?);", RegexOptions.IgnoreCase);
                if (timeLeftMatch != null && timeLeftMatch.Count > 0)
                {
                    foreach (Match item in timeLeftMatch)
                    {
                        if (item.Value.StartsWith("InitTimer("))
                        {
                            time = item.Value;
                            break;
                        }
                    }
                }

                if (String.IsNullOrEmpty(time))
                {
                    HtmlNode timeNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("/html[1]/body[1]/script[10]");
                    if (timeNode != null)
                    {
                        if (String.IsNullOrEmpty(timeNode.InnerHtml))
                        {
                            timeNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("/html[1]/body[1]/script[14]");
                        }
                        if (!timeNode.InnerHtml.Contains("InitTimer"))
                        {
                            timeNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("/html[1]/body[1]/script[12]");
                        }
                    }

                    if (timeNode == null)
                    {
                        timeNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("/html[1]/body[1]/script[9]");
                        if (timeNode != null)
                        {
                            if (!timeNode.InnerHtml.Contains("InitTimer"))
                            {
                                timeNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("/html[1]/body[1]/script[11]");
                            }
                        }
                        else
                        {
                            //if (!timeNode.InnerHtml.Contains("InitTimer"))
                            {
                                timeNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("/html[1]/body[1]/script[11]");
                            }
                        }
                    }

                    if (statusMessage == TicketSearchStatus.PaymentPageStatus && timeNode == null)
                    {
                        timeNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("/html[1]/body[1]/script[8]");
                    }
                    time = timeNode.InnerHtml;
                }

                expirationTimeInSeconds = int.Parse(time.Substring(time.IndexOf("InitTimer(") + "InitTimer(".Length, time.IndexOf(',') - (time.IndexOf("InitTimer(") + "InitTimer(".Length)));

                DateTime dateTimeFound = DateTime.Now;
                TimeSpan ts = dateTimeFound.AddSeconds(expirationTimeInSeconds) - dateTimeFound;
                // this.TimeLeft = String.Format("{0:mm:ss}", ts).Remove(0, 3);
                this.TimeLeft = String.Format("{0}:{1}", System.Math.Truncate(ts.TotalMinutes).ToString(), System.Math.Truncate(Convert.ToDouble(ts.Seconds)).ToString());
                while (ts.TotalSeconds > 1 && this.IfWorking && this.Ticket.isRunning)
                {
                    ts = ts.Subtract(new TimeSpan(0, 0, 1));
                    this.sleep(1000);
                    //  this.TimeLeft = String.Format("{0:mm:ss}", ts).Remove(0, 3);
                    this.TimeLeft = String.Format("{0}:{1}", System.Math.Truncate(ts.TotalMinutes).ToString(), System.Math.Truncate(Convert.ToDouble(ts.Seconds)).ToString());
                    changeStatus(statusMessage);

                    if (Convert.ToInt32(ts.TotalSeconds) < 15)
                    {
                        IncreaseTimer = true;
                        break;
                    }

                    if (this.IfAutoBuy)
                    {
                        IncreaseTimer = false;
                        break;
                    }

                }

                if (this.IfAutoBuy)
                {
                    this.MoreInfo = TicketSearchStatus.MoreInfoBuyingInProgress;
                    changeStatus(statusMessage);
                }
                this.TimeLeft = String.Empty;
            }
            catch
            { }
        }

        protected Boolean processDeliveryPage()
        {
            Boolean result = false;
            try
            {
                String strHTML = String.Empty;
                //strHTML = this._session.Get(this._session.LocationURL);
                HtmlAgilityPack.FormElementCollection.Form frmDeliveryOpts;
                if (this._session.FormElements.Forms.ContainsKey("") || this._session.FormElements.Forms.ContainsKey("aspnetForm"))
                {
                    if (this._session.FormElements.Forms.ContainsKey("aspnetForm"))
                    {
                        frmDeliveryOpts = this._session.FormElements.Forms["aspnetForm"];
                    }
                    else
                    {
                        frmDeliveryOpts = this._session.FormElements.Forms[""];
                    }

                    if (!frmDeliveryOpts.Action.Contains("DeliveryMethod.aspx?t="))
                    {
                        return false;
                    }
                    //if (this._session.FormElements.ContainsKey("recaptcha"))
                    //{
                    //    this._session.FormElements.Remove("recaptcha");
                    //}

                    //Thread.Sleep(1000);
                    ////strHTML = this._session.Post(frmDeliveryOpts.Action);
                    //Thread.Sleep(1000);
                    //this.processRefreshPage();

                    //this.setManualURL();

                    //changeStatus(TicketSearchStatus.DeliveryPageStatus);

                    //this.processTimer(TicketSearchStatus.DeliveryPageStatus);

                    Dictionary<String, String> countryWiseDeliveryOptions = extractDeliveryOptions();
                    String selectedDeliveryOption = null;

                    selectedDeliveryOption = selectDeliveryOption(countryWiseDeliveryOptions);

                    if (selectedDeliveryOption == null && this.IfAutoBuy)
                    {
                        this.Ticket.DeliveryOption = "";
                        //this.Ticket.DeliveryCountry = "";

                        selectedDeliveryOption = askDeliveryOptionWhenEmpty(countryWiseDeliveryOptions);
                    }

                    if (selectedDeliveryOption != null && this.IfWorking && this.Ticket.isRunning)
                    {
                        String selectedDOPTValue = String.Empty;

                        //if (selectedDeliveryOption. .Contains("value"))
                        //{
                        //    selectedDOPTValue = selectedDeliveryOption.Attributes["value"].Value;
                        //}

                        if (countryWiseDeliveryOptions.ContainsKey(selectedDeliveryOption))
                        {
                            selectedDOPTValue = selectedDeliveryOption;
                        }

                        if (!String.IsNullOrEmpty(selectedDOPTValue) && this.IfWorking && this.Ticket.isRunning)
                        {
                            foreach (var item in this._session.FormElements.Keys)
                            {
                                if (item.Contains("ddMOD"))
                                {
                                    this._session.FormElements[item] = selectedDOPTValue;
                                    break;
                                }
                            }

                            //Thread.Sleep(1000);
                            this._session.FormElements.Remove("m:c:btnAddAnotherEvent");
                            this._session.FormElements.Remove("m:c:btnContinue");


                            this._session.HTMLWeb.IfAllowAutoRedirect = true;
                            strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            //Thread.Sleep(1000);
                            ////this.processRefreshPage();
                            //this.setManualURL();

                            result = true;
                        }
                        else
                        {
                            if (this.IfWorking)
                            {
                                this.MoreInfo = TicketSearchStatus.MoreInfoDeliveryOptionDoesNotMatch;
                            }

                            result = false;
                        }
                    }
                    else
                    {
                        if (this.IfWorking)
                        {
                            this.MoreInfo = TicketSearchStatus.MoreInfoDeliveryOptionDoesNotMatch;
                        }

                        result = false;
                    }
                }
                else if (this.Session.HTMLWeb.ResponseUri.AbsoluteUri.Contains("ticketing.axs.com"))
                {
                    Dictionary<String, String> countryWiseDeliveryOptions = extractDeliveryOptions();
                    String selectedDeliveryOption = null;

                    selectedDeliveryOption = selectDeliveryOption(countryWiseDeliveryOptions);

                    if (selectedDeliveryOption == null && this.IfAutoBuy)
                    {
                        this.Ticket.DeliveryOption = "";
                        //this.Ticket.DeliveryCountry = "";

                        selectedDeliveryOption = askDeliveryOptionWhenEmpty(countryWiseDeliveryOptions);
                    }

                    if (selectedDeliveryOption != null && this.IfWorking && this.Ticket.isRunning)
                    {
                        String selectedDOPTValue = String.Empty;

                        //if (selectedDeliveryOption. .Contains("value"))
                        //{
                        //    selectedDOPTValue = selectedDeliveryOption.Attributes["value"].Value;
                        //}

                        if (countryWiseDeliveryOptions.ContainsKey(selectedDeliveryOption))
                        {
                            selectedDOPTValue = selectedDeliveryOption;
                        }

                        if (!String.IsNullOrEmpty(selectedDOPTValue) && this.IfWorking && this.Ticket.isRunning)
                        {
                            foreach (var item in this._session.FormElements.Keys)
                            {
                                if (item.Contains("ddMOD"))
                                {
                                    this._session.FormElements[item] = selectedDOPTValue;
                                    break;
                                }
                            }

                            //Thread.Sleep(1000);
                            this._session.FormElements.Remove("m:c:btnAddAnotherEvent");
                            this._session.FormElements.Remove("m:c:btnContinue");


                            this._session.HTMLWeb.IfAllowAutoRedirect = true;
                            strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            //Thread.Sleep(1000);
                            ////this.processRefreshPage();
                            //this.setManualURL();

                            result = true;
                        }
                        else
                        {
                            if (this.IfWorking)
                            {
                                this.MoreInfo = TicketSearchStatus.MoreInfoDeliveryOptionDoesNotMatch;
                            }

                            result = false;
                        }
                    }
                    else
                    {
                        if (this.IfWorking)
                        {
                            this.MoreInfo = TicketSearchStatus.MoreInfoDeliveryOptionDoesNotMatch;
                        }

                        result = false;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        protected Boolean processCreateSignInPage()
        {
            Boolean result = false;
            String strHtml = String.Empty;
            try
            {
                strHtml = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("DeliveryMethod", "SignIn"));
                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                //strHtml = this._session.Post();
                HtmlAgilityPack.FormElementCollection.Form frmSign_in;
                if (this._session.FormElements.Forms.ContainsKey("create_acct_link"))
                {
                    result = true;
                }
                else if (this._session.FormElements.Forms.ContainsKey("sign_in"))
                {
                    frmSign_in = this._session.FormElements.Forms["sign_in"];

                    changeStatus(TicketSearchStatus.CreatePageStatus);

                    this.processTimer(TicketSearchStatus.CreatePageStatus);

                    if (this.IfWorking && this.Ticket.isRunning)
                    {

                        for (int i = this._session.FormElements.Count - 1; i >= 0; i--)
                        {
                            if (this._session.FormElements.ElementAt(i).Key != "v")
                            {
                                this._session.FormElements.Remove(this._session.FormElements.ElementAt(i).Key);

                            }
                        }
                        Thread.Sleep(1000);
                        strHtml = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                    }

                    result = true;
                }
                else if (this._session.FormElements.Forms.ContainsKey("billForm") || this._session.FormElements.Forms.ContainsKey("changeDeliveryMethod"))
                {
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        protected Boolean processSignInPage()
        {
            Boolean result = false;
            String strHTML = String.Empty;
            try
            {
                Thread.Sleep(2000);

                if (this.IfAutoBuy || this.IncreaseTimer)
                {
                    //if (this.IncreaseTimer)
                    {
                        if (!this._session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("signin.aspx"))
                        {
                            strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                            if (this.Session.HTMLWeb.ResponseUri.AbsoluteUri.Contains("InLineOffers.aspx"))
                            {
                                strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                            }
                        }
                    }

                    //if (AxsAccountRequired)
                    //{
                    //    return true;
                    //}

                    //else
                    //{
                    //    strHTML = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("DeliveryMethod", "SignIn"));
                    //}
                    HtmlAgilityPack.FormElementCollection.Form frmSign_in;
                    if (this._session.FormElements.Forms.ContainsKey("") || this._session.FormElements.Forms.ContainsKey("aspnetForm"))
                    {
                        if (this._session.FormElements.Forms.ContainsKey("aspnetForm"))
                        {
                            frmSign_in = this._session.FormElements.Forms["aspnetForm"];
                        }
                        else
                        {
                            frmSign_in = this._session.FormElements.Forms[""];
                        }

                        if (this._session.FormElements.ContainsKey("recaptcha"))
                        {
                            this._session.FormElements.Remove("recaptcha");
                        }
                        //this.setManualURL();

                        this.processTimer(TicketSearchStatus.SigninPageStatus);

                        if (this.IfWorking && this.Ticket.isRunning)
                        {
                            changeStatus(TicketSearchStatus.SigninPageStatus);

                            this._selectedAccountForAutoBuy = null;

                            lock (this.Ticket)
                            {
                                if (this.Ticket.BuyHistory == null)
                                {
                                    this.Ticket.BuyHistory = new Dictionary<String, int>();
                                }
                            }

                            this._selectedAccountForAutoBuy = this.selectTicketAccount();

                            if (this._selectedAccountForAutoBuy != null && this.IfWorking && this.Ticket.isRunning)
                            {
                                this.currLog.Account = (this._selectedAccountForAutoBuy != null) ? this._selectedAccountForAutoBuy.AccountEmail : "";

                                if (this.IfAutoBuy)
                                {
                                    this.MoreInfo = "Buying: " + this._selectedAccountForAutoBuy.AccountEmail;
                                }
                                else
                                {
                                    this.MoreInfo = "Signing with " + this._selectedAccountForAutoBuy.AccountEmail;
                                }
                                changeStatus(TicketSearchStatus.SigninPageStatus);

                                if (this._session.FormElements.ContainsKey("m:c:txtExistingUserId"))
                                {
                                    this._session.FormElements["m:c:txtExistingUserId"] = this._selectedAccountForAutoBuy.AccountEmail;
                                }
                                else if (this._session.FormElements.ContainsKey("m$c$txtExistingUserId"))
                                {
                                    this._session.FormElements["m$c$txtExistingUserId"] = this._selectedAccountForAutoBuy.AccountEmail;
                                }
                                else
                                {
                                    this._session.FormElements.Add("email_address", this._selectedAccountForAutoBuy.AccountEmail);
                                }

                                if (this._session.FormElements.ContainsKey("m:c:txtPassword"))
                                {
                                    this._session.FormElements["m:c:txtPassword"] = this._selectedAccountForAutoBuy.AccountPassword;
                                }
                                else if (this._session.FormElements.ContainsKey("m$c$txtPassword"))
                                {
                                    this._session.FormElements["m$c$txtPassword"] = this._selectedAccountForAutoBuy.AccountPassword;
                                }
                                else
                                {
                                    this._session.FormElements.Add("password", this._selectedAccountForAutoBuy.AccountPassword);
                                }

                                //HtmlNodeCollection nodesForV = this._session.HtmlDocument.DocumentNode.SelectNodes(@"//input[@name='v']");

                                //if (nodesForV.Count >= 2)
                                //{
                                //    if (nodesForV[1].Attributes["value"] != null)
                                //    {
                                //        String v = nodesForV[1].Attributes["value"].Value;
                                //        if (this._session.FormElements.ContainsKey("v"))
                                //        {
                                //            this._session.FormElements["v"] = v;
                                //        }
                                //        else
                                //        {
                                //            this._session.FormElements.Add("v", v);
                                //        }
                                //    }
                                //}
                                //Thread.Sleep(1000);
                                this.Session.HTMLWeb.IfAllowAutoRedirect = true;
                                strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                                //strHTML = this._session.Get(this._session.LocationURL);
                                //strHTML = this._session.Get(this._session.LocationURL);

                                //Thread.Sleep(1000);
                                //this.processRefreshPage();
                                //this._session.FormElements["m:c:txtPassword"] = this._selectedAccountForAutoBuy.AccountPassword;
                                //strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                                if (strHTML.ToLower().Contains("email address/login or password is invalid"))
                                {
                                    this.MoreInfo = TicketSearchStatus.MoreInfoAccountIncorrect;
                                    result = false;
                                }
                                else if (strHTML.ToLower().Contains("session has expired"))
                                {
                                    this.MoreInfo = "Your session has expired. Please try again.";
                                    result = false;
                                }
                                else
                                {
                                    //this.setManualURL();

                                    if (this.IfAutoBuy)
                                    {
                                        this.MoreInfo = "Buying: " + this._selectedAccountForAutoBuy.AccountEmail;
                                    }
                                    else
                                    {
                                        this.MoreInfo = "Signed in with " + this._selectedAccountForAutoBuy.AccountEmail;
                                    }
                                    Thread.Sleep(1000);
                                    result = true;
                                }
                            }
                            else
                            {
                                this.MoreInfo = TicketSearchStatus.MoreInfoAccountNotAvailable;
                                result = false;
                            }
                        }
                        else
                        {
                            this.MoreInfo = TicketSearchStatus.MoreInfoHoldTimeExpired;
                            this.processTimer(TicketSearchStatus.SigninPageStatus);
                            result = false;
                        }
                    }
                    else if (this._session.FormElements.Forms.ContainsKey("billForm") || this._session.FormElements.Forms.ContainsKey("changeDeliveryMethod"))
                    {
                        result = true;
                    }
                    else if (this.Session.HTMLWeb.ResponseUri.AbsoluteUri.Contains("axs.com"))
                    {

                        lock (this.Ticket)
                        {
                            if (this.Ticket.BuyHistory == null)
                            {
                                this.Ticket.BuyHistory = new Dictionary<String, int>();
                            }
                        }

                        this.AxsAccountRequired = true;

                        this._selectedAccountForAutoBuy = this.selectTicketAccount();

                        if (this._selectedAccountForAutoBuy != null)
                        {
                            AddUpdateField(this._session, "m$c$txtExistingUserId", this._selectedAccountForAutoBuy.AccountEmail);

                            AddUpdateField(this._session, "m$c$txtPassword", this._selectedAccountForAutoBuy.AccountPassword);

                            AddUpdateField(this._session, "m$c$btnContinue", "Sign In");

                            strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                            Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);

                            if (!String.IsNullOrEmpty(this._session.RedirectLocation))
                            {
                                this._session.Get(this._session.RedirectLocation);
                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            }

                            if (this._session.LocationURL.Contains("UpdateSecurityQuestion.aspx"))
                            {
                                AddUpdateField(this._session, "m:c:btnSkip", "Skip This Time");

                                strHTML = this.Session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            }

                            if (!String.IsNullOrEmpty(this._session.RedirectLocation))
                            {
                                strHTML = this._session.Get(this._session.RedirectLocation);

                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            }
                            if (!String.IsNullOrEmpty(this._session.RedirectLocation))
                            {
                                strHTML = this._session.Get(this._session.RedirectLocation);

                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            }
                            if (!String.IsNullOrEmpty(this._session.RedirectLocation))
                            {
                                strHTML = this._session.Get(this._session.RedirectLocation);

                                Distill.checkForDistilIdentificationBlock(this.Session, this.Ticket.AutoCaptchaServices, this.Ticket, this);
                            }

                            if (strHTML.Contains("Please fill-in/correct the information below before proceeding."))
                            {
                                this.MoreInfo = TicketSearchStatus.MoreInfoAccountIncorrect;
                                result = false;

                                changeStatus(TicketSearchStatus.RetryingStatus);

                                Thread.Sleep(3000);
                                return result;
                            }


                            result = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        #region Recaptcha V2
        public Captcha getToken(object resource)
        {
            Captcha captcha = (Captcha)resource;
            IAutoCaptchaService services = null;

            try
            {
                HttpWebRequest request = null;
                String getPosturl = String.Format(@"https://www.google.com/recaptcha/api2/userverify?k={0}", AVS_CAPTCHA_KEY);



                request = (HttpWebRequest)WebRequest.Create(getPosturl);

                request.Headers.Set(HttpRequestHeader.AcceptLanguage, "en-us");
                request.UserAgent = captcha.UserAgent;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                request.KeepAlive = true;
                request.Headers.Add("DNT", @"1");
                request.Headers.Set(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                request.Method = "POST";
                request.ServicePoint.Expect100Continue = false;

                try
                {
                    if (this.Proxy != null)
                        request.Proxy = this.Proxy.toWebProxy();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

                if (this.Ticket.ifDBCAutoCaptcha)
                {
                    Bitmap dummyImage;
                    this.Captcha.CaptchesBytes = ImageMerger.ImageMergerInstance.getMergeImage(this.Captcha.CaptchesBytes, this.Captcha.ImageUrl, out dummyImage);
                    this.Captcha.CaptchaImage = dummyImage;
                    solveAutoCaptcha = new DeathByCaptchaAPI(this.Ticket.AutoCaptchaServices, this.Captcha);
                }
                else if (this.Ticket.ifRDAutoCaptcha)
                {
                    Bitmap dummyImage;
                    this.Captcha.CaptchesBytes = ImageMerger.ImageMergerInstance.getMergeImage(this.Captcha.CaptchesBytes, this.Captcha.ImageUrl, out dummyImage);
                    this.Captcha.CaptchaImage = dummyImage;
                    solveAutoCaptcha = new RDCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
                }
                else if (this.Ticket.ifCPTAutoCaptcha)
                {
                    Bitmap dummyImage;
                    this.Captcha.CaptchesBytes = ImageMerger.ImageMergerInstance.getMergeImage(this.Captcha.CaptchesBytes, this.Captcha.ImageUrl, out dummyImage);
                    this.Captcha.CaptchaImage = dummyImage;
                    solveAutoCaptcha = new CPTCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
                }
                else if (this.Ticket.ifDCAutoCaptcha)
                {
                    //Bitmap dummyImage;
                    //this.Captcha.CaptchesBytes = ImageMerger.ImageMergerInstance.getMergeImage(this.Captcha.CaptchesBytes, this.Captcha.ImageUrl, out dummyImage);
                    //this.Captcha.CaptchaImage = dummyImage;
                    solveAutoCaptcha = new DeCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
                }
                else if (this.Ticket.ifOCR)
                {
                    Bitmap dummyImage;
                    this.Captcha.CaptchesBytes = ImageMerger.ImageMergerInstance.getMergeImage(this.Captcha.CaptchesBytes, this.Captcha.ImageUrl, out dummyImage);
                    this.Captcha.CaptchaImage = dummyImage;
                    solveAutoCaptcha = new OCRService(this.Ticket.AutoCaptchaServices, this.Captcha);
                }
                else if (this.Ticket.if2CAutoCaptcha)
                {
                    Bitmap dummyImage;
                    this.Captcha.CaptchesBytes = ImageMerger.ImageMergerInstance.getMergeImage(this.Captcha.CaptchesBytes, this.Captcha.ImageUrl, out dummyImage);
                    this.Captcha.CaptchaImage = dummyImage;
                    solveAutoCaptcha = new Captcha2(this.Ticket.AutoCaptchaServices, this.Captcha, true);
                }
                else if (this.Ticket.ifAntigateCaptcha)
                {
                    solveAutoCaptcha = new AntigateCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
                }
                else
                {
                    // By Default use bypass auto captcha
                    solveAutoCaptcha = new BypassAutoCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
                }
                if (this.IfWorking && this.Ticket.isRunning && solveAutoCaptcha != null)
                {
                    solveAutoCaptcha.solve();
                }

                //Captchawork need to be done 

                if (!this.Ticket.ifAntigateCaptcha && !this.Ticket.ifDCAutoCaptcha)
                {
                    captcha.CaptchaWords = commaSeperated(this.Captcha.CaptchaWords);
                    captcha.CaptchaWords = changeNumbers(this.Captcha.CaptchaWords);
                }
                Debug.WriteLine(captcha.CaptchaWords);

                string strHTML = String.Empty;

                String requestBody = captcha.PostBody;
                int indexOfResponse = requestBody.IndexOf("response=") + "response=".Length;
                int indexOfAmp = requestBody.IndexOf('&', indexOfResponse);

                Byte[] captchawords = Encoding.UTF8.GetBytes("{\"response\":\"" + captcha.CaptchaWords + "\"}");
                string body = requestBody = requestBody.Insert(indexOfResponse, Convert.ToBase64String(captchawords).Replace("=", "")); //for post words                        

                //body = body.Substring(0, body.IndexOf("&bg"));

                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);

                request.ContentLength = postBytes.Length;
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                Stream stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
                stream.Close();

                strHTML = getResponse(request);

                if (strHTML.Contains("uvresp") && !strHTML.Contains("rresp"))
                {
                    string remove = strHTML.Substring(0, strHTML.IndexOf("["));
                    strHTML = strHTML.Replace(remove, "");

                    if (!String.IsNullOrEmpty((JArray.Parse(strHTML)[1]).ToString()))
                    {
                        captcha.Recaptoken = (JArray.Parse(strHTML)[1]).ToString();
                        captcha.Expiretime = DateTime.Now.AddMinutes(2);

                    }
                    else
                    {

                        //if ((captchaWorkshop != null) && (services != null))
                        //{
                        //    captchaWorkshop.reportBadCaptcha(services);
                        //}
                    }
                }
                else
                {
                    //services.Statistic.incrementFailed();

                    //if ((captchaWorkshop != null) && (services != null))
                    //{
                    //    captchaWorkshop.reportBadCaptcha(services);
                    //}

                    String date = DateTime.Now.Ticks.ToString();

                    Debug.WriteLine("unsolved");
                }
            }
            catch (Exception e)
            {

                Debug.WriteLine(e.Message);

                if (e.Message.Contains("Empty captcha solution"))
                {
                    // captchaWorkshop.reportBadCaptcha(services);
                }
            }

            return captcha;
        }


        #endregion

        protected TokenMessage getCaptchaService()
        {
            TokenMessage msg = new TokenMessage();

            if (this.Ticket.ifDBCAutoCaptcha)
            {
                msg.Username = this.Ticket.AutoCaptchaServices.DBCUserName;
                msg.Password = this.Ticket.AutoCaptchaServices.DBCPassword;
                msg.ServiceName = "DBC";
                return msg;
                //  solveAutoCaptcha = new DeathByCaptchaAPI(this.Ticket.AutoCaptchaServices, this.Captcha);
            }
            else if (this.Ticket.ifRDAutoCaptcha)
            {
                msg.Username = this.Ticket.AutoCaptchaServices.RDUserName;
                msg.Password = this.Ticket.AutoCaptchaServices.RDPassword;
                msg.ServiceName = "RD";
                return msg;
                //solveAutoCaptcha = new RDCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
            }
            else if (this.Ticket.ifRDCAutoCaptcha)
            {
                msg.Username = this.Ticket.AutoCaptchaServices.RDCUserName;
                msg.Password = this.Ticket.AutoCaptchaServices.RDCPassword;
                msg.ServiceName = "RDC";
                return msg;
                //solveAutoCaptcha = new RDCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
            }
            else if (this.Ticket.ifCPTAutoCaptcha)
            {
                msg.Username = this.Ticket.AutoCaptchaServices.CPTUserName;
                msg.Password = this.Ticket.AutoCaptchaServices.CPTPassword;
                msg.ServiceName = "CPT";
                return msg;
                //solveAutoCaptcha = new CPTCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
            }
            else if (this.Ticket.ifDCAutoCaptcha)
            {
                msg.Username = this.Ticket.AutoCaptchaServices.DCUserName;
                msg.Password = this.Ticket.AutoCaptchaServices.DCPassword;
                msg.Port = this.Ticket.AutoCaptchaServices.DCPort;
                msg.ServiceName = "DC";
                return msg;
                //solveAutoCaptcha = new DeCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
            }
            else if (this.Ticket.ifOCR)
            {
                msg.Host = this.Ticket.AutoCaptchaServices.OCRIP;
                msg.Port = this.Ticket.AutoCaptchaServices.OCRPort;
                msg.ServiceName = "OCR";
                return msg;
                //solveAutoCaptcha = new OCRService(this.Ticket.AutoCaptchaServices, this.Captcha);
            }
            //else if (this.Ticket.ifCAutoCaptcha)
            //{
            //    msg.Host = this.Ticket.AutoCaptchaServices.CHost;
            //    msg.Port = this.Ticket.AutoCaptchaServices.CPort;
            //    msg.Username = this.Ticket.AutoCaptchaServices.CUserName;
            //    msg.Password = this.Ticket.AutoCaptchaServices.CPassword;
            //    msg.ServiceName = "C";
            //    //solveAutoCaptcha = new CustomCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
            //}
            //else if (this.Ticket.ifROCR)
            //{
            //    msg.Ip = this.Ticket.AutoCaptchaServices.ROCRIP;
            //    msg.Port = this.Ticket.AutoCaptchaServices.ROCRPort;
            //    msg.Username = this.Ticket.AutoCaptchaServices.ROCRUsername;
            //    msg.Password = this.Ticket.AutoCaptchaServices.ROCRPassword;
            //    msg.ServiceName = "ROCR";
            //    //solveAutoCaptcha = new ROCRService(this.Ticket.AutoCaptchaServices, this.Captcha);
            //}
            //else if (this.Ticket.ifBoloOCR)
            //{
            //    msg.Ip = this.Ticket.AutoCaptchaServices.BOLOIP;
            //    msg.Port = this.Ticket.AutoCaptchaServices.BOLOPORT;
            //    msg.ServiceName = "BOLO";
            //    //solveAutoCaptcha = new BoloCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha);
            //}
            //else if (this.Ticket.ifSOCR)
            //{
            //    msg.Ip = this.Ticket.AutoCaptchaServices.SOCRIP;
            //    msg.Port = this.Ticket.AutoCaptchaServices.SOCRPort;
            //    msg.Username = this.Ticket.AutoCaptchaServices.SOCRCaptchaURL;

            //    msg.ServiceName = "SOCR";
            //    //solveAutoCaptcha = new SOCRService(this.Ticket.AutoCaptchaServices, this.Captcha);
            //}
            else if (this.Ticket.if2CAutoCaptcha)
            {
                msg.Key = this.Ticket.AutoCaptchaServices.C2Key;
                msg.ServiceName = "2C";
                return msg;
                //solveAutoCaptcha = new Captcha2(this.Ticket.AutoCaptchaServices, this.Captcha, true);
            }
            else if (this.Ticket.ifAntigateCaptcha)
            {
                msg.Key = this.Ticket.AutoCaptchaServices.AntigateKey;
                msg.ServiceName = "A";
                return msg;
                //solveAutoCaptcha = new AntigateCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
            }
            else if (this.Ticket.ifAntigateCaptcha)
            {
                msg.Key = this.Ticket.AutoCaptchaServices.AntigateKey;
                msg.ServiceName = "A";
                return msg;
                //solveAutoCaptcha = new AntigateCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
            }
            else if (this.Ticket.ifAC1AutoCaptcha)
            {
                msg.Key = CapsiumLicensingOperation.LicesningPickerInstance.AC1Key;
                msg.ServiceName = "AC1";
                msg.Audio = true;
                return msg;
                //solveAutoCaptcha = new AntigateCaptchaService(this.Ticket.AutoCaptchaServices, this.Captcha, true);
            }
            return null;
        }

        private void setManualURL()
        {
            try
            {
                //this._session.HTMLWeb.Referer = "https://www8.ticketingcentral.com/V2/ReserveSeats.aspx?t=6D1A89FA8D06B5525A185CB";
                //String strhtml = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("DeliveryMethod", "SignIn"));
                //this._session.HTMLWeb.Referer = "https://www8.ticketingcentral.com/V2/ReserveSeats.aspx?t=6D1A89FA8D06B5525A185CB";
                //strhtml = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                //AddUpdateField(this._session, "m:c:txtExistingUserId", "midhatzehra110@gmail.com");
                //AddUpdateField(this._session, "m:c:txtPassword", "nanosoft");
                //strhtml = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                if (this._session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("deliverymethod"))
                {
                    this.LastURLForManualBuy = this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("DeliveryMethod", "SignIn");
                }
                else
                {
                    //if (this._ifManualBuyProcessToFinalPage)
                    //{
                    this.LastURLForManualBuy = this._session.HTMLWeb.ResponseUri.AbsoluteUri;
                    //}
                    if (this._session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("waitpage"))
                    {
                        this.LastURLForManualBuy = this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("WaitPage", "Process");
                    }
                }
                //}
            }
            catch (Exception)
            {

            }
        }

        private string validateAndMakeValidURL(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute) && !url.StartsWith("http"))
            {
                if (this._session.HTMLWeb != null)
                {
                    if (this._session.HTMLWeb.ResponseUri != null)
                    {
                        url = this._session.HTMLWeb.ResponseUri.Scheme + "://" + this._session.HTMLWeb.ResponseUri.Host + url;
                    }
                }
            }
            return url;
        }
        private void setManualURL(String url, FormElementCollection formElements)
        {
            try
            {
                this.LastURLForManualBuy = validateAndMakeValidURL(url);
                this.LastFormElementsToPost = formElements;
            }
            catch (Exception)
            {

            }
        }

        private VSTicketAccount selectTicketAccount()
        {
            VSTicketAccount selectedAccount = null;
            try
            {
                if (this.Ticket.ifSelectAccountAutoBuying)
                {
                    if (this.IfAutoBuy)
                    {
                        _frmSelectAccount = new frmSelectAccount(this.Ticket, this);
                        selectedAccount = (VSTicketAccount)_frmSelectAccount.promptAccount();
                        _frmSelectAccount.Dispose();
                        _frmSelectAccount = null;
                        if (selectedAccount == null)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        //If not of auto buying then select account from all accounts list.
                        VSTicket ticket = (VSTicket)this.Ticket;
                        if (ticket.AllTMAccounts != null)
                        {
                            if (ticket.AllTMAccounts.Count > 0)
                            {
                                try
                                {
                                    VSTicketAccount account = this.Ticket.getNextAccount(this.IfNewCard);
                                    if (CheckingAccountAvailability(account.AccountEmail, account.BuyingLimit))
                                    {
                                        selectedAccount = account;
                                    }
                                    else
                                    {
                                        selectedAccount = null;
                                    }
                                }
                                catch
                                {
                                    selectedAccount = null;
                                }
                            }
                            else
                            {
                                selectedAccount = null;
                            }
                        }
                        else
                        {
                            selectedAccount = null;
                        }
                    }
                }
                else
                {
                    if (this.Ticket.SelectedAccounts != null)
                    {
                        if (this.Ticket.SelectedAccounts.Count > 0)
                        {
                            try
                            {
                                VSTicketAccount account = this.Ticket.getNextAccount(this.IfNewCard);
                                if (CheckingAccountAvailability(account.AccountEmail, account.BuyingLimit))
                                {
                                    selectedAccount = account;
                                }
                                else
                                {
                                    selectedAccount = null;
                                }
                            }
                            catch
                            {
                                selectedAccount = null;
                            }
                        }
                        else
                        {
                            selectedAccount = null;
                        }
                    }
                    else
                    {
                        selectedAccount = null;
                    }
                }
                // if no account found for sign in then retry one more time here.
                if (selectedAccount == null)
                {
                    try
                    {
                        VSTicketAccount account = this.Ticket.getNextAccount(this.IfNewCard);
                        if (CheckingAccountAvailability(account.AccountEmail, account.BuyingLimit))
                        {
                            selectedAccount = account;
                        }
                        else
                        {
                            selectedAccount = null;
                        }
                    }
                    catch
                    {
                        selectedAccount = null;
                    }
                }

            }
            catch
            {
                selectedAccount = null;
            }

            return selectedAccount;
        }

        protected Boolean processVerificationPage()
        {
            Boolean result = false;
            try
            {
                String strHTML = String.Empty;
                HtmlAgilityPack.FormElementCollection.Form frmVerification;
                if (this._session.FormElements.Forms.Count > 0 && this._session.HtmlDocument.DocumentNode.InnerHtml.Contains("Credit Card Verification") && this.IfWorking && this.Ticket.isRunning)
                {
                    frmVerification = this._session.FormElements.Forms[""];

                    if (this._session.FormElements.ContainsKey("recaptcha"))
                    {
                        this._session.FormElements.Remove("recaptcha");
                    }

                    Thread.Sleep(1000);

                    changeStatus(TicketSearchStatus.VerificationPageStatus);

                    this.processTimer(TicketSearchStatus.VerificationPageStatus);

                    //Thread.Sleep(1000);
                    sleep(1000);

                    strHTML = this._session.Post(frmVerification.Action);
                    //Thread.Sleep(1000);
                    sleep(1000);

                    this.processRefreshPage();

                    String urlToRedirect = String.Empty;
                    HtmlNode hnodeBank = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//iframe[@name='bank']");
                    if (hnodeBank != null)
                    {
                        //Thread.Sleep(1000);
                        sleep(1000);

                        urlToRedirect = hnodeBank.Attributes["src"].Value;
                        strHTML = this._session.Get(urlToRedirect);
                        //Thread.Sleep(1000);
                        sleep(1000);

                        this.processRefreshPage();
                        //Thread.Sleep(1000);
                        sleep(1000);


                        frmVerification = this._session.FormElements.Forms[""];

                        strHTML = this._session.Post(frmVerification.Action);
                        //Thread.Sleep(1000);
                        sleep(1000);

                        this.processRefreshPage();
                        //Thread.Sleep(1000);
                        sleep(1000);


                        if (this._session.FormElements.Forms.ContainsKey("no_js_form")) //Handling for Visa card verification.
                        {
                            frmVerification = this._session.FormElements.Forms["no_js_form"];
                            if (this._session.FormElements.ContainsKey("submit"))
                            {
                                this._session.FormElements.Remove("submit");
                            }
                            this._session.FormElements["user_action"] = "activate";
                            //this._session.FormElements["ssn3"] = "7584";
                            //this._session.FormElements["cvv"] = "438";
                            //this._session.FormElements["exp_month"] = "01";

                            Thread.Sleep(2000);
                            //this._session.FormElements["exp_year"] = "13";
                            String urlToPost = this._session.HTMLWeb.ResponseUri.AbsoluteUri;
                            int ilastIndexOf = urlToPost.LastIndexOf("/");
                            if (ilastIndexOf > -1 && this.IfWorking && this.Ticket.isRunning)
                            {
                                urlToPost = urlToPost.Remove(ilastIndexOf);
                                ilastIndexOf = frmVerification.Action.LastIndexOf("/");
                                if (ilastIndexOf > -1 && this.IfWorking && this.Ticket.isRunning)
                                {
                                    String tmpurlToPost = frmVerification.Action.Remove(0, ilastIndexOf);
                                    urlToPost = urlToPost + tmpurlToPost;
                                    strHTML = this._session.Post(urlToPost);
                                    //Thread.Sleep(1000);
                                    sleep(1000);

                                    this.processRefreshPage();
                                    //Thread.Sleep(1000);
                                    sleep(1000);


                                    if (this._session.FormElements.Forms.ContainsKey("pa_res_form") && this.IfWorking && this.Ticket.isRunning)
                                    {
                                        urlToRedirect = this._session.FormElements.Forms["pa_res_form"].Action;

                                        strHTML = this._session.Post(urlToRedirect);
                                        //Thread.Sleep(1000);
                                        sleep(1000);

                                        this.processRefreshPage();

                                        result = true;
                                    }
                                }
                            }
                        }
                        else if (this._session.FormElements.Forms.ContainsKey("form1")) //Handling for master card verification
                        {
                            frmVerification = this._session.FormElements.Forms["form1"];
                            if (this._session.FormElements.ContainsKey("submit"))
                            {
                                this._session.FormElements.Remove("submit");
                            }
                            this._session.FormElements["user_action"] = "activate";
                            //this._session.FormElements["ssn3"] = "7584";
                            //this._session.FormElements["cvv"] = "438";
                            //this._session.FormElements["exp_month"] = "01";
                            //this._session.FormElements["exp_year"] = "13";
                            String urlToPost = this._session.HTMLWeb.ResponseUri.AbsoluteUri;
                            int ilastIndexOf = urlToPost.LastIndexOf("/");
                            if (ilastIndexOf > -1 && this.IfWorking && this.Ticket.isRunning)
                            {
                                urlToPost = urlToPost.Remove(ilastIndexOf);
                                ilastIndexOf = frmVerification.Action.LastIndexOf("/");
                                if (ilastIndexOf > -1 && this.IfWorking && this.Ticket.isRunning)
                                {
                                    String tmpurlToPost = frmVerification.Action.Remove(0, ilastIndexOf);
                                    urlToPost = urlToPost + tmpurlToPost;
                                    strHTML = this._session.Post(urlToPost);
                                    //Thread.Sleep(1000);
                                    sleep(1000);

                                    this.processRefreshPage();
                                    //Thread.Sleep(1000);
                                    sleep(1000);


                                    if (this._session.FormElements.Forms.ContainsKey("pa_res_form") && this.IfWorking && this.Ticket.isRunning)
                                    {
                                        urlToRedirect = this._session.FormElements.Forms["pa_res_form"].Action;

                                        strHTML = this._session.Post(urlToRedirect);
                                        //Thread.Sleep(1000);
                                        sleep(1000);

                                        this.processRefreshPage();
                                        sleep(1000);

                                        if (strHTML.Contains(".auth_complete_callback(") && this.IfWorking && this.Ticket.isRunning)
                                        {
                                            int iStart = strHTML.IndexOf("auth_complete_callback({ uri:") + "auth_complete_callback({ uri:".Length;
                                            int iEnd = strHTML.IndexOf("});", iStart);
                                            if (iEnd > iStart)
                                            {
                                                urlToRedirect = strHTML.Substring(iStart, iEnd - iStart);
                                                urlToRedirect = urlToRedirect.Trim();
                                                urlToRedirect = urlToRedirect.Trim('\"');
                                                strHTML = this._session.Get(urlToRedirect);

                                                sleep(1000);

                                                this.processRefreshPage();
                                                result = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //string text = File.ReadAllText("autobuyrequest.txt");

                    //Session.HtmlDocument.LoadHtml(text);

                    HtmlNode scriptNode = this.Session.HtmlDocument.DocumentNode.SelectSingleNode("//script[contains(@language,'javascript')]");

                    if (scriptNode != null)
                    {
                        int index = scriptNode.InnerText.IndexOf("window.location.href='") + 22;

                        int endIndex = scriptNode.InnerText.IndexOf(", 300);") - 2;

                        string url = scriptNode.InnerText.Substring(index, endIndex - index);

                        if (!String.IsNullOrEmpty(url))
                        {
                            this.Session.Get(url);
                            result = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        protected Boolean processConfirmationPage()
        {
            Boolean result = false;
            try
            {
                String strHTML = String.Empty;
                strHTML = this._session.HtmlDocument.DocumentNode.InnerHtml;
                strHTML = strHTML.ToLower();



                if (strHTML.Contains("your order number is") || strHTML.ToLower().Contains("your confirmation number is") || strHTML.ToLower().Contains("your order confirmation number is"))
                {
                    result = true;
                    this.MoreInfo = TicketSearchStatus.MoreInfoTicketBought;

                    try
                    {
                        HtmlNode nodeOrderNumber = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//span[@class='messageText']");
                        if (nodeOrderNumber != null)
                        {
                            this.MoreInfo = this.MoreInfo + " Your order number is " + nodeOrderNumber.InnerText;

                            this.currLog.OrderNumber = nodeOrderNumber.InnerText;

                            Thread.Sleep(2000);
                        }
                        else
                        {
                            nodeOrderNumber = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//span[@class='order-confirmation-number']");
                            if (nodeOrderNumber != null)
                            {
                                this.MoreInfo = this.MoreInfo + " Your Confirmation Number is " + nodeOrderNumber.InnerText;
                                this.currLog.OrderNumber = nodeOrderNumber.InnerText;

                                this.currLog.OrderNumber = nodeOrderNumber.InnerText;

                                Thread.Sleep(2000);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        this.MoreInfo = TicketSearchStatus.MoreInfoTicketBought;
                    }

                    changeStatus(TicketSearchStatus.ConfirmationPagetStatus);

                    if (this._CurrentParameter != null)
                    {
                        if (this._CurrentParameter.Bought == null)
                        {
                            this._CurrentParameter.Bought = 0;
                        }
                        this._CurrentParameter.Bought++;
                    }

                    lock (this.Ticket)
                    {
                        this.Ticket.BuyCount++;

                        if (this.Ticket.BuyHistory.ContainsKey(_selectedAccountForAutoBuy.AccountEmail))
                        {
                            this.Ticket.BuyHistory[_selectedAccountForAutoBuy.AccountEmail] += 1;
                            this.Ticket.SaveTicket();
                        }
                        else
                        {
                            this.Ticket.BuyHistory.Add(_selectedAccountForAutoBuy.AccountEmail, 1);
                            this.Ticket.SaveTicket();
                        }
                    }
                }
                else if (strHTML.Contains("order details:"))
                {
                    result = true;
                    this.MoreInfo = TicketSearchStatus.MoreInfoTicketBought;

                    try
                    {
                        HtmlNode nodeOrderNumber = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//ul[@class='order-list']/li/a");
                        if (nodeOrderNumber != null)
                        {
                            this.currLog.OrderNumber = nodeOrderNumber.InnerText.Replace("&nbsp;", " ");
                            this.MoreInfo = this.MoreInfo + " " + nodeOrderNumber.InnerText.Replace("&nbsp;", " ");

                            this.currLog.OrderNumber = nodeOrderNumber.InnerText.Replace("&nbsp;", " ");
                        }
                    }
                    catch (Exception)
                    {
                        this.MoreInfo = TicketSearchStatus.MoreInfoTicketBought;
                    }

                    changeStatus(TicketSearchStatus.ConfirmationPagetStatus);

                    if (this._CurrentParameter != null)
                    {
                        if (this._CurrentParameter.Bought == null)
                        {
                            this._CurrentParameter.Bought = 0;
                        }
                        this._CurrentParameter.Bought++;
                    }

                    lock (this.Ticket)
                    {
                        this.Ticket.BuyCount++;

                        if (this.Ticket.BuyHistory.ContainsKey(_selectedAccountForAutoBuy.AccountEmail))
                        {
                            this.Ticket.BuyHistory[_selectedAccountForAutoBuy.AccountEmail] += 1;
                            this.Ticket.SaveTicket();
                        }
                        else
                        {
                            this.Ticket.BuyHistory.Add(_selectedAccountForAutoBuy.AccountEmail, 1);
                            this.Ticket.SaveTicket();
                        }
                    }
                }
                else if (strHTML.Contains("we limit the rate of web page requests that can be made by individual users in any given time period") || strHTML.Contains("your web page requests have exceeded these limits and your access has been temporarily disabled"))
                {
                    result = true;
                    this.MoreInfo = TicketSearchStatus.MoreInfoTicketMayBeBought;

                    changeStatus(TicketSearchStatus.ConfirmationPagetStatus);

                    if (this._CurrentParameter != null)
                    {
                        if (this._CurrentParameter.Bought == null)
                        {
                            this._CurrentParameter.Bought = 0;
                        }
                        this._CurrentParameter.Bought++;
                    }

                    lock (this.Ticket)
                    {
                        this.Ticket.BuyCount++;

                        if (this.Ticket.BuyHistory.ContainsKey(_selectedAccountForAutoBuy.AccountEmail))
                        {
                            this.Ticket.BuyHistory[_selectedAccountForAutoBuy.AccountEmail] += 1;
                            this.Ticket.SaveTicket();
                        }
                        else
                        {
                            this.Ticket.BuyHistory.Add(_selectedAccountForAutoBuy.AccountEmail, 1);
                            this.Ticket.SaveTicket();
                        }
                    }
                }
                else if (strHTML.Contains("there are problems with your submission"))
                {
                    HtmlNode nodeErrorMessage = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@class='container-neutralZone']/ul");
                    String errorMessage = String.Empty;
                    if (nodeErrorMessage != null)
                    {
                        errorMessage = nodeErrorMessage.InnerText;
                        errorMessage = Regex.Replace(errorMessage, "<!--(.*?)-->", "");
                        errorMessage = errorMessage.Replace("\n", " ").Trim();
                    }

                    this.MoreInfo = TicketSearchStatus.MoreInfoBuyingFailed + errorMessage;
                    changeStatus(TicketSearchStatus.ConfirmationPagetStatus);

                    result = false;
                }
                else if (strHTML.Contains("transaction could not be completed with the credit card information entered"))
                {
                    this.MoreInfo = TicketSearchStatus.MoreInfoBuyingFailed + "Transaction could not be completed with the credit card information entered.";
                    changeStatus(TicketSearchStatus.ConfirmationPagetStatus);

                    result = false;
                }
                else if (strHTML.Contains("your timer has expired"))
                {
                    this.MoreInfo = TicketSearchStatus.MoreInfoBuyingFailed + "Your timer has expired and the tickets in your shopping cart have been released.";
                    changeStatus(TicketSearchStatus.ConfirmationPagetStatus);

                    result = false;
                }
                else
                {
                    if (this._session.HTMLWeb.ResponseUri.AbsoluteUri.Contains("/TransactionError.aspx?"))
                    {
                        this.MoreInfo = TicketSearchStatus.MoreInfoBuyingFailed + "Transaction Error.";
                        changeStatus(TicketSearchStatus.ConfirmationPagetStatus);
                    }
                    else
                    {
                        this.MoreInfo = TicketSearchStatus.MoreInfoBuyingFailed;
                        changeStatus(TicketSearchStatus.ConfirmationPagetStatus);
                    }
                    result = false;
                }

                sleep(2000);

            }
            catch
            {
                result = false;
            }

            return result;
        }

        protected Boolean processUpdateSecurityQuestion()
        {
            Boolean result = false;

            try
            {
                FormElementCollection.Form frmBillForm;
                if (this._session.FormElements.Forms.ContainsKey("") || this._session.FormElements.Forms.ContainsKey("aspnetForm"))
                {
                    if (this._session.FormElements.Forms.ContainsKey("aspnetForm"))
                    {
                        frmBillForm = this._session.FormElements.Forms["aspnetForm"];
                    }
                    else
                    {
                        frmBillForm = this._session.FormElements.Forms[""];
                    }

                    if (frmBillForm.Action.Contains("PaymentInfo.aspx?t="))
                    {
                        result = true;
                    }
                    else if (frmBillForm.Action.Contains("UpdateSecurityQuestion.aspx?t="))
                    {
                        changeStatus("Security Page loaded");

                        for (int i = this._session.FormElements.Count - 1; i >= 0; i--)
                        {
                            String key = this._session.FormElements.ElementAt(i).Key;

                            if (key.Contains("buttonUpdate"))
                            {
                                this._session.FormElements.Remove(key);
                            }
                            else
                            {
                                continue;
                            }
                        }

                        String strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                        if (this._session.FormElements.Forms.ContainsKey("aspnetForm"))
                        {
                            frmBillForm = this._session.FormElements.Forms["aspnetForm"];
                        }
                        else
                        {
                            frmBillForm = this._session.FormElements.Forms[""];
                        }

                        if (frmBillForm.Action.Contains("PaymentInfo.aspx?t="))
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        protected Boolean processPaymentPage()
        {
            Boolean result = false;
            try
            {
                Thread.Sleep(2000);

                String strHTML = String.Empty;
                HtmlAgilityPack.FormElementCollection.Form frmBillForm;

                if (this.IncreaseTimer)
                {
                    strHTML = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                    this.processTimer(TicketSearchStatus.PaymentPageStatus);
                }

                if (this._session.FormElements.Forms.ContainsKey("") || this._session.FormElements.Forms.ContainsKey("aspnetForm"))
                {
                    if (this._session.FormElements.Forms.ContainsKey("aspnetForm"))
                    {
                        frmBillForm = this._session.FormElements.Forms["aspnetForm"];
                    }
                    else
                    {
                        frmBillForm = this._session.FormElements.Forms[""];
                    }

                    if (!frmBillForm.Action.Contains("PaymentInfo.aspx?t="))
                    {
                        return false;
                    }

                    if (this._session.FormElements.ContainsKey("recaptcha"))
                    {
                        this._session.FormElements.Remove("recaptcha");
                    }

                    changeStatus(TicketSearchStatus.PaymentPageStatus);

                    Boolean cardFound = false;

                    if (this.IfAutoBuy || _ifManualBuyProcessToFinalPage)
                    {
                        try
                        {
                            this.MoreInfo = "Buying: " + this._selectedAccountForAutoBuy.AccountEmail;
                            changeStatus(TicketSearchStatus.PaymentPageStatus);

                            if (!this.IfNewCard)
                            {
                                HtmlNodeCollection _cards = this._session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlCardList')]/option");

                                if (_cards != null)
                                {
                                    foreach (HtmlNode card in _cards)
                                    {
                                        try
                                        {
                                            if (card.NextSibling.InnerHtml.EndsWith(this._selectedAccountForAutoBuy.CardLastDigits))
                                            {
                                                if (this._session.FormElements.ContainsKey("m$c$ddlCardList"))
                                                {
                                                    AddUpdateField(this._session, "m$c$ddlCardList", card.Attributes["value"].Value);
                                                }
                                                else
                                                {
                                                    AddUpdateField(this._session, "m:c:ddlCardList", card.Attributes["value"].Value);
                                                }
                                                cardFound = true;
                                                break;
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                cardFound = true;

                                #region Select Delivery DropDown post

                                HtmlNodeCollection _ddCountry = this._session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddCountry')]/option");

                                if (_ddCountry != null)
                                {
                                    Boolean matched = false;

                                    foreach (HtmlNode country in _ddCountry)
                                    {
                                        try
                                        {
                                            if (country.NextSibling.InnerHtml.ToLower().Contains(this._selectedAccountForAutoBuy.Country.ToLower()))
                                            {
                                                if (this._session.FormElements.ContainsKey("m$c$BillAddress$ddCountry"))
                                                {
                                                    if (String.IsNullOrEmpty(this._session.FormElements["m$c$BillAddress$ddCountry"]) || !this._session.FormElements["m$c$BillAddress$ddCountry"].ToLower().Contains(this._selectedAccountForAutoBuy.Country.ToLower()))
                                                    {
                                                        AddUpdateField(this._session, "m$c$BillAddress$ddCountry", country.Attributes["value"].Value);
                                                        this._session.FormElements["__EVENTTARGET"] = "m$c$BillAddress$ddCountry";
                                                        matched = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (String.IsNullOrEmpty(this._session.FormElements["m:c:BillAddress$ddCountry"]) || !this._session.FormElements["m$c$BillAddress$ddCountry"].ToLower().Contains(this._selectedAccountForAutoBuy.Country.ToLower()))
                                                    {
                                                        AddUpdateField(this._session, "m:c:BillAddress$ddCountry", country.Attributes["value"].Value);
                                                        this._session.FormElements["__EVENTTARGET"] = "m:c:BillAddress$ddCountry";
                                                        matched = true;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }

                                    if (matched)
                                    {
                                        this._session.FormElements.Remove("m:c:btnProcessOrder");
                                        this._session.FormElements.Remove("m$c$btnProcessOrder");

                                        strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                                    }
                                }

                                #endregion

                                #region Insurance Callback

                                HtmlNode insuranceNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//input[contains(@name,'nsuranceChoice')]");

                                if (insuranceNode != null)
                                {
                                    HtmlNodeCollection insuranceNodes = this._session.HtmlDocument.DocumentNode.SelectNodes("//input[contains(@name,'nsuranceChoice')]");

                                    if (insuranceNodes != null)
                                    {
                                        foreach (HtmlNode item in insuranceNodes)
                                        {
                                            if (item.Attributes.Contains("value"))
                                            {
                                                if (item.Attributes["value"].Value.ToLower().Contains("no"))
                                                {
                                                    this._session.FormElements[item.Attributes["name"].Value] = item.Attributes["value"].Value;
                                                    this._session.FormElements.Remove("m:c:btnProcessOrder");
                                                    this._session.FormElements.Remove("m$c$btnProcessOrder");

                                                    this._session.FormElements["__EVENTTARGET"] = item.Attributes["name"].Value;

                                                    strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                #endregion

                                for (int i = this._session.FormElements.Count - 1; i >= 0; i--)
                                {
                                    String key = this._session.FormElements.ElementAt(i).Key;

                                    try
                                    {
                                        if (key.Contains("__EVENTTARGET") || key.Contains("__EVENTARGUMENT") || key.Contains("__LASTFOCUS") || key.Contains("__VIEWSTATE") || key.Contains("__VIEWSTATEGENERATOR") || key.Contains("__EVENTVALIDATION"))
                                        {
                                            continue;
                                        }
                                        else if (key.Contains("__SCROLLPOSITIONX") || key.Contains("__SCROLLPOSITIONY"))
                                        {
                                            if (key.Contains("__SCROLLPOSITIONY"))
                                            {
                                                this._session.FormElements[key] = new Random().Next(1100, 1150).ToString();
                                            }
                                            continue;
                                        }
                                        else if (key.Contains("encryptedData"))
                                        {
                                            String encryptedData = String.Empty;
                                            String publicKey = "2#10001#a94964d56cdae46f6d38c3d98f38b215b35d2e1342b4adf26508efc25b41d9f4c8c362a60f6e0e73339e9d8cb3e8428cf8912f505d674199507bbc1f03cb7d08f1c79c03c6c68df70576c949552078c4c33d0d16881a4ee3d4230e4d8e8746c975e6bdd0607f3b5a332070812eabb4aef95f02fbff4ec0d108003d1813a5bb0ea2760f0c322c91526de54739193de670e8f11eef8090b88127e006b91fdcd351cc3372e3149889b533a068734fec8607baea9477c9b65fc1a6a843c45800f9f3c4598f76e66863ce9c9f37b00bd048960a5a3dcccbcdaa33b23f01c7e2ea83364813c9012d389b544738d62c9dfe812d96b33b2d6ac673be7f766370c03de447";
                                            String header = "{\"alg\":\"RSA1_5\",\"enc\":\"A256GCM\",\"kid\":\"2\",\"com.worldpay.apiVersion\":\"1.0\",\"com.worldpay.libVersion\":\"1.0.1\",\"com.worldpay.channel\":\"javascript\"}";

                                            Match m = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, @"Worldpay.setPublicKey(.*?);");

                                            try
                                            {
                                                if (m.Success)
                                                {
                                                    String value = m.Value;
                                                    value = value.Replace("Worldpay.setPublicKey('", String.Empty);
                                                    value = value.Replace("');", String.Empty);
                                                    publicKey = value;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.WriteLine(ex.Message);
                                            }

                                            String script = global::Automatick.Properties.Resources.WorldPay;
                                            String month = String.Empty;

                                            if (this._session.HTMLWeb.ResponseUri.Host.Contains(".com"))
                                            {
                                                if (this._selectedAccountForAutoBuy.ExpiryMonth.StartsWith("0"))
                                                {
                                                    month = this._selectedAccountForAutoBuy.ExpiryMonth.Substring(1);
                                                }
                                                else
                                                {
                                                    month = this._selectedAccountForAutoBuy.ExpiryMonth;
                                                }
                                            }
                                            else
                                            {
                                                month = this._selectedAccountForAutoBuy.ExpiryMonth;
                                            }

                                            var cse = "{\"cvc\": \"" + this._selectedAccountForAutoBuy.CVV2 + "\",\"cardHolderName\":\"" + this._selectedAccountForAutoBuy.AccountName + "\",\"cardNumber\": \"" + this._selectedAccountForAutoBuy.CardNumber + "\",\"expiryMonth\": \"" + month + "\",\"expiryYear\":\"" + this._selectedAccountForAutoBuy.ExpiryYear + "\"}";

                                            try
                                            {
                                                using (ScriptEngine engine = new ScriptEngine("jscript"))
                                                {
                                                    ParsedScript parsed = engine.Parse(script);

                                                    encryptedData = parsed.CallMethod("encrypt", new object[] { cse, header, publicKey }).ToString();
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }

                                            this._session.FormElements[key] = encryptedData;
                                            continue;
                                        }
                                        else if (key.Contains("shaHash"))
                                        {
                                            this._session.FormElements[key] = getSHA256Hash(this._selectedAccountForAutoBuy.CardNumber.Trim());
                                            continue;
                                        }
                                        else if (key.Contains("md5Hash"))
                                        {
                                            String md5 = getMD5Hash(this._selectedAccountForAutoBuy.CardNumber.Trim());
                                            String truncated = String.Empty;

                                            for (var index = 0; index < md5.Length; index = index + 4)
                                            {
                                                truncated += md5[index];
                                                truncated += md5[index + 1];
                                            }

                                            this._session.FormElements[key] = truncated;
                                            continue;
                                        }
                                        else if (key.Contains("pan"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.CardNumber.Trim().Substring(0, 6) + '|' + this._selectedAccountForAutoBuy.CardNumber.Trim().Substring(this._selectedAccountForAutoBuy.CardNumber.Trim().Length - 4);
                                            continue;
                                        }
                                        else if (key.Contains("CardNo"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.CardNumber.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("NameOnCard"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.AccountName.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("ExpMonth"))
                                        {
                                            if (this._session.HTMLWeb.ResponseUri.Host.Contains(".com"))
                                            {
                                                if (this._selectedAccountForAutoBuy.ExpiryMonth.StartsWith("0"))
                                                {
                                                    this._session.FormElements[key] = this._selectedAccountForAutoBuy.ExpiryMonth.Substring(1);
                                                }
                                                else
                                                {
                                                    this._session.FormElements[key] = this._selectedAccountForAutoBuy.ExpiryMonth;
                                                }
                                            }
                                            else
                                            {
                                                this._session.FormElements[key] = this._selectedAccountForAutoBuy.ExpiryMonth;
                                            }
                                            continue;
                                        }
                                        else if (key.Contains("ExpYear"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.ExpiryYear.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("VerificationNo"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.CVV2.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("PhoneNo"))
                                        {
                                            if (this._session.HTMLWeb.ResponseUri.Host.Contains(".com"))
                                            {
                                                this._session.FormElements[key] = "(" + this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(0, 3) + ") " + this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(3, 3) + "-" + this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(6);
                                            }
                                            else
                                            {
                                                this._session.FormElements[key] = this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(0, 3) + "-" + this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(3, 3) + "-" + this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(6);
                                            }
                                        }
                                        else if (key.Contains("CardList"))
                                        {
                                            continue;
                                        }
                                        else if (key.Contains("Add1"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.AddressLine1.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("Add2"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.AddressLine2.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("City"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.City.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("Province"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.Province.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("State"))
                                        {
                                            if (this._selectedAccountForAutoBuy.Country.ToLower().Equals("united states"))
                                            {
                                                var _key = this.Ticket.GlobalSetting.US_States.FirstOrDefault(pred => pred.Value.ToLower().Equals(this._selectedAccountForAutoBuy.Province.Trim().ToLower()));

                                                try
                                                {
                                                    if (!String.IsNullOrEmpty(_key.Key))
                                                    {
                                                        this._session.FormElements[key] = _key.Key;
                                                    }
                                                    else
                                                    {
                                                        this._session.FormElements[key] = this._selectedAccountForAutoBuy.Province.Trim();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    this._session.FormElements[key] = this._selectedAccountForAutoBuy.Province.Trim();
                                                }
                                            }
                                            else
                                            {
                                                this._session.FormElements[key] = this._selectedAccountForAutoBuy.Province.Trim();
                                            }
                                            continue;
                                        }
                                        else if (key.Contains("txtZip"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.PostCode.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("Country"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.Country.ToUpper();
                                            continue;
                                        }
                                        else if (key.Contains("btnProcessOrder"))
                                        {
                                            continue;
                                        }
                                        else if (key.Contains("chkAcceptTerms"))
                                        {
                                            this._session.FormElements[key] = "on";
                                            continue;
                                        }
                                        else if (key.Contains("ddMOD"))
                                        {
                                            this._session.FormElements.Remove(key);
                                            continue;
                                        }
                                        else if (key.Contains("chkThirdParty"))
                                        {
                                            this._session.FormElements.Remove(key);
                                            continue;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex.Message);
                                    }
                                }
                            }
                        }
                        catch { }

                        if (cardFound && this.IfWorking && this.Ticket.isRunning)
                        {
                            if (_ifManualBuyProcessToFinalPage)
                            {
                                this._session.FormElements.Remove("m:c:btnProcessOrder");
                                this._session.FormElements.Remove("m$c$btnProcessOrder");

                                PostData = string.Empty;

                                foreach (string _key in this._session.FormElements.Keys)
                                {
                                    PostData += _key + "=" + this._session.FormElements[_key] + "&";
                                }
                                this.PostData = PostData.TrimEnd('&');
                                setManualURL();
                                try
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
                                            ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                                            this.ClearSessionFromServer();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                }
                                this.openManualTicketBrowser();
                                this.MoreInfo = "Transferred to manual browsing.";
                                this._ifManualBuyProcessToFinalPage = false;
                                result = false;
                            }
                            else if (this.IfAutoBuy)
                            {
                                if (!this.IfNewCard)
                                {
                                    this._session.FormElements.Remove("m:c:btnProcessOrder");
                                    this._session.FormElements.Remove("m$c$btnProcessOrder");

                                    strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                                }

                                for (int i = 0; i < this._session.FormElements.Count; i++)
                                {
                                    if (this._session.FormElements.ElementAt(i).Key.Contains("chkAcceptTerms"))
                                    {
                                        AddUpdateField(this._session, this._session.FormElements.ElementAt(i).Key, "on");
                                    }
                                    else if (this._session.FormElements.ElementAt(i).Key.Contains("txtVerificationNo"))
                                    {
                                        AddUpdateField(this._session, this._session.FormElements.ElementAt(i).Key, this._selectedAccountForAutoBuy.CVV2);
                                    }
                                    else if (this._session.FormElements.ElementAt(i).Key.EndsWith("insuranceChoices"))
                                    {
                                        AddUpdateField(this._session, this._session.FormElements.ElementAt(i).Key, "insuranceChoiceNo");
                                    }
                                }

                                foreach (string _key in this._session.FormElements.Keys)
                                {
                                    PostData += _key + "=" + this._session.FormElements[_key] + "&";
                                }
                                this.PostData = PostData.TrimEnd('&');

                                strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                                result = true;

                                return result;
                            }
                        }
                        else
                        {
                            this.MoreInfo = TicketSearchStatus.MoreInfoNoCreditCardInfoMatch;
                            result = false;
                        }
                    }
                    else
                    {
                        this.MoreInfo = "Time expired.";
                        result = false;
                    }
                }
                else if (this.Session.HTMLWeb.ResponseUri.AbsoluteUri.Contains("axs.com"))
                {
                    if (!this.Session.HTMLWeb.ResponseUri.AbsoluteUri.Contains("axs.com"))
                    {
                        if (this._session.FormElements.Forms.ContainsKey("aspnetForm"))
                        {
                            frmBillForm = this._session.FormElements.Forms["aspnetForm"];
                        }
                        else
                        {
                            frmBillForm = this._session.FormElements.Forms[""];
                        }

                        if (!frmBillForm.Action.Contains("PaymentInfo.aspx?t="))
                        {
                            return false;
                        }
                    }

                    if (this._session.FormElements.ContainsKey("recaptcha"))
                    {
                        this._session.FormElements.Remove("recaptcha");
                    }

                    changeStatus(TicketSearchStatus.PaymentPageStatus);

                    Boolean cardFound = false;

                    if (this.IfAutoBuy || _ifManualBuyProcessToFinalPage)
                    {
                        try
                        {
                            this.MoreInfo = "Buying: " + this._selectedAccountForAutoBuy.AccountEmail;
                            changeStatus(TicketSearchStatus.PaymentPageStatus);

                            if (!this.IfNewCard)
                            {
                                HtmlNodeCollection _cards = this._session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlCardList')]/option");

                                if (_cards != null)
                                {
                                    foreach (HtmlNode card in _cards)
                                    {
                                        try
                                        {
                                            if (card.NextSibling.InnerHtml.EndsWith(this._selectedAccountForAutoBuy.CardLastDigits))
                                            {
                                                if (this._session.FormElements.ContainsKey("m$c$ddlCardList"))
                                                {
                                                    AddUpdateField(this._session, "m$c$ddlCardList", card.Attributes["value"].Value);
                                                }
                                                else
                                                {
                                                    AddUpdateField(this._session, "m:c:ddlCardList", card.Attributes["value"].Value);
                                                }
                                                cardFound = true;
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                cardFound = true;

                                #region Select Delivery DropDown post

                                HtmlNodeCollection _ddCountry = this._session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddCountry')]/option");

                                if (_ddCountry != null)
                                {
                                    Boolean matched = false;

                                    foreach (HtmlNode country in _ddCountry)
                                    {
                                        try
                                        {
                                            if (country.NextSibling.InnerHtml.ToLower().Contains(this._selectedAccountForAutoBuy.Country.ToLower()))
                                            {
                                                if (this._session.FormElements.ContainsKey("m$c$BillAddress$ddCountry"))
                                                {
                                                    if (String.IsNullOrEmpty(this._session.FormElements["m$c$BillAddress$ddCountry"]) || !this._session.FormElements["m$c$BillAddress$ddCountry"].ToLower().Contains(this._selectedAccountForAutoBuy.Country.ToLower()))
                                                    {
                                                        AddUpdateField(this._session, "m$c$BillAddress$ddCountry", country.Attributes["value"].Value);
                                                        this._session.FormElements["__EVENTTARGET"] = "m$c$BillAddress$ddCountry";
                                                        matched = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (String.IsNullOrEmpty(this._session.FormElements["m:c:BillAddress$ddCountry"]) || !this._session.FormElements["m$c$BillAddress$ddCountry"].ToLower().Contains(this._selectedAccountForAutoBuy.Country.ToLower()))
                                                    {
                                                        AddUpdateField(this._session, "m:c:BillAddress$ddCountry", country.Attributes["value"].Value);
                                                        this._session.FormElements["__EVENTTARGET"] = "m:c:BillAddress$ddCountry";
                                                        matched = true;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }

                                    if (matched)
                                    {
                                        this._session.FormElements.Remove("m:c:btnProcessOrder");
                                        this._session.FormElements.Remove("m$c$btnProcessOrder");

                                        strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                                    }
                                }

                                #endregion

                                #region Insurance Callback

                                HtmlNode insuranceNode = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//input[contains(@name,'nsuranceChoice')]");

                                if (insuranceNode != null)
                                {
                                    HtmlNodeCollection insuranceNodes = this._session.HtmlDocument.DocumentNode.SelectNodes("//input[contains(@name,'nsuranceChoice')]");

                                    if (insuranceNodes != null)
                                    {
                                        foreach (HtmlNode item in insuranceNodes)
                                        {
                                            if (item.Attributes.Contains("value"))
                                            {
                                                if (item.Attributes["value"].Value.ToLower().Contains("no"))
                                                {
                                                    this._session.FormElements[item.Attributes["name"].Value] = item.Attributes["value"].Value;
                                                    this._session.FormElements.Remove("m:c:btnProcessOrder");
                                                    this._session.FormElements.Remove("m$c$btnProcessOrder");

                                                    this._session.FormElements["__EVENTTARGET"] = item.Attributes["name"].Value;

                                                    strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                #endregion

                                for (int i = this._session.FormElements.Count - 1; i >= 0; i--)
                                {
                                    String key = this._session.FormElements.ElementAt(i).Key;

                                    try
                                    {
                                        if (key.Contains("__EVENTTARGET") || key.Contains("__EVENTARGUMENT") || key.Contains("__LASTFOCUS") || key.Contains("__VIEWSTATE") || key.Contains("__VIEWSTATEGENERATOR") || key.Contains("__EVENTVALIDATION"))
                                        {
                                            continue;
                                        }
                                        else if (key.Contains("__SCROLLPOSITIONX") || key.Contains("__SCROLLPOSITIONY"))
                                        {
                                            if (key.Contains("__SCROLLPOSITIONY"))
                                            {
                                                this._session.FormElements[key] = new Random().Next(1100, 1150).ToString();
                                            }
                                            continue;
                                        }
                                        else if (key.Contains("encryptedData"))
                                        {
                                            String encryptedData = String.Empty;
                                            String publicKey = "2#10001#a94964d56cdae46f6d38c3d98f38b215b35d2e1342b4adf26508efc25b41d9f4c8c362a60f6e0e73339e9d8cb3e8428cf8912f505d674199507bbc1f03cb7d08f1c79c03c6c68df70576c949552078c4c33d0d16881a4ee3d4230e4d8e8746c975e6bdd0607f3b5a332070812eabb4aef95f02fbff4ec0d108003d1813a5bb0ea2760f0c322c91526de54739193de670e8f11eef8090b88127e006b91fdcd351cc3372e3149889b533a068734fec8607baea9477c9b65fc1a6a843c45800f9f3c4598f76e66863ce9c9f37b00bd048960a5a3dcccbcdaa33b23f01c7e2ea83364813c9012d389b544738d62c9dfe812d96b33b2d6ac673be7f766370c03de447";
                                            String header = "{\"alg\":\"RSA1_5\",\"enc\":\"A256GCM\",\"kid\":\"2\",\"com.worldpay.apiVersion\":\"1.0\",\"com.worldpay.libVersion\":\"1.0.1\",\"com.worldpay.channel\":\"javascript\"}";

                                            Match m = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, @"Worldpay.setPublicKey(.*?);");

                                            try
                                            {
                                                if (m.Success)
                                                {
                                                    String value = m.Value;
                                                    value = value.Replace("Worldpay.setPublicKey('", String.Empty);
                                                    value = value.Replace("');", String.Empty);
                                                    publicKey = value;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.WriteLine(ex.Message);
                                            }

                                            String script = global::Automatick.Properties.Resources.WorldPay;
                                            String month = String.Empty;

                                            if (this._session.HTMLWeb.ResponseUri.Host.Contains(".com"))
                                            {
                                                if (this._selectedAccountForAutoBuy.ExpiryMonth.StartsWith("0"))
                                                {
                                                    month = this._selectedAccountForAutoBuy.ExpiryMonth.Substring(1);
                                                }
                                                else
                                                {
                                                    month = this._selectedAccountForAutoBuy.ExpiryMonth;
                                                }
                                            }
                                            else
                                            {
                                                month = this._selectedAccountForAutoBuy.ExpiryMonth;
                                            }

                                            var cse = "{\"cvc\": \"" + this._selectedAccountForAutoBuy.CVV2 + "\",\"cardHolderName\":\"" + this._selectedAccountForAutoBuy.AccountName + "\",\"cardNumber\": \"" + this._selectedAccountForAutoBuy.CardNumber + "\",\"expiryMonth\": \"" + month + "\",\"expiryYear\":\"" + this._selectedAccountForAutoBuy.ExpiryYear + "\"}";

                                            try
                                            {
                                                using (ScriptEngine engine = new ScriptEngine("jscript"))
                                                {
                                                    ParsedScript parsed = engine.Parse(script);

                                                    encryptedData = parsed.CallMethod("encrypt", new object[] { cse, header, publicKey }).ToString();
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }

                                            this._session.FormElements[key] = encryptedData;
                                            continue;
                                        }
                                        else if (key.Contains("shaHash"))
                                        {
                                            this._session.FormElements[key] = getSHA256Hash(this._selectedAccountForAutoBuy.CardNumber.Trim());
                                            continue;
                                        }
                                        else if (key.Contains("md5Hash"))
                                        {
                                            String md5 = getMD5Hash(this._selectedAccountForAutoBuy.CardNumber.Trim());
                                            String truncated = String.Empty;

                                            for (var index = 0; index < md5.Length; index = index + 4)
                                            {
                                                truncated += md5[index];
                                                truncated += md5[index + 1];
                                            }

                                            this._session.FormElements[key] = truncated;
                                            continue;
                                        }
                                        else if (key.Contains("pan"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.CardNumber.Trim().Substring(0, 6) + '|' + this._selectedAccountForAutoBuy.CardNumber.Trim().Substring(this._selectedAccountForAutoBuy.CardNumber.Trim().Length - 4);
                                            continue;
                                        }
                                        else if (key.Contains("CardNo"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.CardNumber.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("NameOnCard"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.AccountName.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("ExpMonth"))
                                        {
                                            if (this._session.HTMLWeb.ResponseUri.Host.Contains(".com"))
                                            {
                                                if (this._selectedAccountForAutoBuy.ExpiryMonth.StartsWith("0"))
                                                {
                                                    this._session.FormElements[key] = this._selectedAccountForAutoBuy.ExpiryMonth.Substring(1);
                                                }
                                                else
                                                {
                                                    this._session.FormElements[key] = this._selectedAccountForAutoBuy.ExpiryMonth;
                                                }
                                            }
                                            else
                                            {
                                                this._session.FormElements[key] = this._selectedAccountForAutoBuy.ExpiryMonth;
                                            }
                                            continue;
                                        }
                                        else if (key.Contains("ExpYear"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.ExpiryYear.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("VerificationNo"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.CVV2.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("PhoneNo"))
                                        {
                                            if (this._session.HTMLWeb.ResponseUri.Host.Contains(".com"))
                                            {
                                                this._session.FormElements[key] = "(" + this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(0, 3) + ") " + this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(3, 3) + "-" + this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(6);
                                            }
                                            else
                                            {
                                                this._session.FormElements[key] = this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(0, 3) + "-" + this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(3, 3) + "-" + this._selectedAccountForAutoBuy.PhoneNumber.Replace(" ", String.Empty).Substring(6);
                                            }
                                        }
                                        else if (key.Contains("CardList"))
                                        {
                                            continue;
                                        }
                                        else if (key.Contains("Add1"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.AddressLine1.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("Add2"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.AddressLine2.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("City"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.City.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("Province"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.Province.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("State"))
                                        {
                                            if (this._selectedAccountForAutoBuy.Country.ToLower().Equals("united states"))
                                            {
                                                var _key = this.Ticket.GlobalSetting.US_States.FirstOrDefault(pred => pred.Value.ToLower().Equals(this._selectedAccountForAutoBuy.Province.Trim().ToLower()));

                                                try
                                                {
                                                    if (!String.IsNullOrEmpty(_key.Key))
                                                    {
                                                        this._session.FormElements[key] = _key.Key;
                                                    }
                                                    else
                                                    {
                                                        this._session.FormElements[key] = this._selectedAccountForAutoBuy.Province.Trim();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    this._session.FormElements[key] = this._selectedAccountForAutoBuy.Province.Trim();
                                                }
                                            }
                                            else
                                            {
                                                this._session.FormElements[key] = this._selectedAccountForAutoBuy.Province.Trim();
                                            }
                                            continue;
                                        }
                                        else if (key.Contains("txtZip"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.PostCode.Trim();
                                            continue;
                                        }
                                        else if (key.Contains("Country"))
                                        {
                                            this._session.FormElements[key] = this._selectedAccountForAutoBuy.Country.ToUpper();
                                            continue;
                                        }
                                        else if (key.Contains("btnProcessOrder"))
                                        {
                                            continue;
                                        }
                                        else if (key.Contains("chkAcceptTerms"))
                                        {
                                            this._session.FormElements[key] = "on";
                                            continue;
                                        }
                                        else if (key.Contains("ddMOD"))
                                        {
                                            this._session.FormElements.Remove(key);
                                            continue;
                                        }
                                        else if (key.Contains("chkThirdParty"))
                                        {
                                            this._session.FormElements.Remove(key);
                                            continue;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex.Message);
                                    }
                                }
                            }
                        }
                        catch { }

                        if (cardFound && this.IfWorking && this.Ticket.isRunning)
                        {
                            if (_ifManualBuyProcessToFinalPage)
                            {
                                this._session.FormElements.Remove("m$c$btnProcessOrder");
                                this._session.FormElements.Remove("m:c:btnProcessOrder");

                                foreach (string _key in this._session.FormElements.Keys)
                                {
                                    PostData += _key + "=" + this._session.FormElements[_key] + "&";
                                }
                                this.PostData = PostData.TrimEnd('&');
                                setManualURL();
                                try
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
                                            ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(this._proxy);
                                            this.ClearSessionFromServer();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                }
                                this.openManualTicketBrowser();
                                this.MoreInfo = "Transferred to manual browsing.";
                                this._ifManualBuyProcessToFinalPage = false;
                                result = false;
                            }
                            else if (this.IfAutoBuy)
                            {
                                if (!this.IfNewCard)
                                {
                                    this._session.FormElements.Remove("m$c$btnProcessOrder");
                                    this._session.FormElements.Remove("m:c:btnProcessOrder");

                                    strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                                }

                                for (int i = 0; i < this._session.FormElements.Count; i++)
                                {
                                    if (this._session.FormElements.ElementAt(i).Key.Contains("chkAcceptTerms"))
                                    {
                                        AddUpdateField(this._session, this._session.FormElements.ElementAt(i).Key, "on");
                                    }
                                    else if (this._session.FormElements.ElementAt(i).Key.Contains("txtVerificationNo"))
                                    {
                                        AddUpdateField(this._session, this._session.FormElements.ElementAt(i).Key, this._selectedAccountForAutoBuy.CVV2);
                                    }
                                    else if (this._session.FormElements.ElementAt(i).Key.EndsWith("insuranceChoices"))
                                    {
                                        AddUpdateField(this._session, this._session.FormElements.ElementAt(i).Key, "insuranceChoiceNo");
                                    }
                                }

                                foreach (string _key in this._session.FormElements.Keys)
                                {
                                    PostData += _key + "=" + this._session.FormElements[_key] + "&";
                                }
                                this.PostData = PostData.TrimEnd('&');

                                this.Session.HTMLWeb.IfAllowAutoRedirect = false;

                                strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                                if (!String.IsNullOrEmpty(this.Session.LocationURL))
                                {
                                    this._session.Get(this.Session.LocationURL);

                                }
                                this.Session.HTMLWeb.IfAllowAutoRedirect = true;

                                result = true;
                            }
                        }
                        else
                        {
                            this.MoreInfo = TicketSearchStatus.MoreInfoNoCreditCardInfoMatch;
                            result = false;
                        }
                    }
                    else
                    {
                        this.MoreInfo = "Time expired.";
                        result = false;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        private String selectDeliveryOption(Dictionary<String, String> countryWiseDeliveryOptions)
        {
            String selectedDeliveryOption = null;

            try
            {
                if (this.Ticket.ifSelectDeliveryOptionAutoBuying)
                {
                    if (this.Ticket.ifAlwaysAskDeliveryOptionOnBuying)// && this.IfAutoBuy )
                    {
                        ITicketDeliveryOption _selectedDeliveryOption = null;
                        _frmSelectDO = new frmSelectDeliveryOption(countryWiseDeliveryOptions, this.Ticket, this);
                        _selectedDeliveryOption = _frmSelectDO.promptDeliveryOption();
                        _frmSelectDO.Dispose();
                        _frmSelectDO = null;
                        if (_selectedDeliveryOption != null)
                        {
                            if (!string.IsNullOrEmpty(_selectedDeliveryOption.DeliveryOption))
                            {
                                //if (!string.IsNullOrEmpty(_selectedDeliveryOption.DeliveryCountry))
                                //{
                                if (String.IsNullOrEmpty(this.Ticket.DeliveryOption))//|| String.IsNullOrEmpty(this.Ticket.DeliveryCountry))
                                {
                                    this.Ticket.DeliveryOption = _selectedDeliveryOption.DeliveryOption;
                                }

                                //    foreach (KeyValuePair<String, HtmlNodeCollection> item in countryWiseDeliveryOptions)
                                //    {
                                //        if (item.Key.ToLower().Contains(_selectedDeliveryOption.DeliveryCountry.ToLower()))
                                //        {
                                //            foreach (HtmlNode ele in item.Value)
                                //            {
                                //                HtmlAttributeCollection prop = ele.Attributes;
                                //                if (prop["data-md"].Value.ToLower().Contains(_selectedDeliveryOption.DeliveryOption.ToLower()))
                                //                {
                                //                    selectedDeliveryOption = ele;
                                //                    break;
                                //                }
                                //            }
                                //        }
                                //        if (selectedDeliveryOption != null)
                                //        {
                                //            break;
                                //        }
                                //    }
                                //}
                                //else
                                //{
                                foreach (KeyValuePair<String, String> item in countryWiseDeliveryOptions)
                                {
                                    //foreach (KeyValuePair<String, String> ele in item.Value)
                                    //{
                                    //HtmlAttributeCollection prop = ele.Attributes;
                                    if (item.Value.ToLower().Contains(_selectedDeliveryOption.DeliveryOption.ToLower()))
                                    {
                                        selectedDeliveryOption = item.Key;
                                        break;
                                    }
                                    //}

                                    if (selectedDeliveryOption != null)
                                    {
                                        break;
                                    }
                                }
                                //}
                            }
                            //}
                        }
                        else
                        {
                            selectedDeliveryOption = askDeliveryOptionWhenEmpty(countryWiseDeliveryOptions);
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(this.Ticket.DeliveryOption))
                        {
                            foreach (KeyValuePair<String, String> item in countryWiseDeliveryOptions)
                            {
                                if (item.Value.ToLower().Contains(this.Ticket.DeliveryOption.ToLower()))
                                {
                                    selectedDeliveryOption = item.Key;
                                    break;
                                }
                                //}

                                if (selectedDeliveryOption != null)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            selectedDeliveryOption = askDeliveryOptionWhenEmpty(countryWiseDeliveryOptions);
                        }
                        // else if (this.Ticket.SelectedDeliveryOptions != null)
                        //{
                        //    IEnumerable<EMDeliveryOption> selectedDeliveryOptions = this.Ticket.SelectedDeliveryOptions.Where(p => p.IfSelected == true);

                        //    foreach (EMDeliveryOption dopt in selectedDeliveryOptions)
                        //    {
                        //        //HtmlNodeCollection selectedCountryDopts = null;
                        //        //if (countryWiseDeliveryOptions.ContainsKey(dopt.DeliveryCountry))
                        //        //{
                        //        //    selectedCountryDopts = countryWiseDeliveryOptions[dopt.DeliveryCountry];
                        //        //}

                        //        //if (selectedCountryDopts != null)
                        //        //{
                        //        foreach (KeyValuePair<String, String> item in countryWiseDeliveryOptions)
                        //            {
                        //        //HtmlAttributeCollection prop = ele.Attributes;
                        //        if (item.Value.Contains(dopt.DeliveryOption))
                        //        {
                        //            selectedDeliveryOption = dopt.DeliveryOption;
                        //            break;
                        //        }
                        //        }
                        //        //}
                        //        //else
                        //        //{
                        //        //    continue;
                        //        //}

                        //        if (selectedDeliveryOption != null)
                        //        {
                        //            break;
                        //        }
                        //    }
                        //}
                    }
                }
                else if (this.Ticket.SelectedDeliveryOptions != null)
                {
                    IEnumerable<VSDeliveryOption> selectedDeliveryOptions = this.Ticket.SelectedDeliveryOptions.Where(p => p.IfSelected == true);

                    foreach (VSDeliveryOption dopt in selectedDeliveryOptions)
                    {
                        //HtmlNodeCollection selectedCountryDopts = null;
                        //if (countryWiseDeliveryOptions.ContainsKey(dopt.DeliveryCountry))
                        //{
                        //    selectedCountryDopts = countryWiseDeliveryOptions[dopt.DeliveryCountry];
                        //}

                        //if (selectedCountryDopts != null)
                        //{
                        foreach (KeyValuePair<String, String> item in countryWiseDeliveryOptions)
                        {
                            //HtmlAttributeCollection prop = ele.Attributes;
                            if (item.Value.Contains(dopt.DeliveryOption))
                            {
                                selectedDeliveryOption = item.Key;
                                break;
                            }
                        }
                        //}
                        //else
                        //{
                        //    continue;
                        //}

                        if (selectedDeliveryOption != null)
                        {
                            break;
                        }
                    }
                }
            }
            catch
            {

            }
            return selectedDeliveryOption;
        }

        private String askDeliveryOptionWhenEmpty(Dictionary<String, String> countryWiseDeliveryOptions)
        {
            String selectedDeliveryOption = null;
            //if (this.IfAutoBuy && (String.IsNullOrEmpty(this.Ticket.DeliveryOption) || String.IsNullOrEmpty(this.Ticket.DeliveryCountry)))
            if ((String.IsNullOrEmpty(this.Ticket.DeliveryOption))) //|| String.IsNullOrEmpty(this.Ticket.DeliveryCountry)))
            {
                VSTicket tick = (VSTicket)this.Ticket;
                if (!tick.IfSelectDeliveryWindowOpen)
                {
                    ITicketDeliveryOption _selectedDeliveryOption = null;
                    tick.IfSelectDeliveryWindowOpen = true;
                    _frmSelectDO = new frmSelectDeliveryOption(countryWiseDeliveryOptions, this.Ticket, this);
                    //_frmSelectDO.ShowDialog();
                    _selectedDeliveryOption = _frmSelectDO.promptDeliveryOption();
                    tick.IfSelectDeliveryWindowOpen = false;
                    _frmSelectDO.Dispose();
                    _frmSelectDO = null;
                    if (_selectedDeliveryOption != null)
                    {
                        this.Ticket.DeliveryOption = _selectedDeliveryOption.DeliveryOption;
                        //this.Ticket.DeliveryCountry = _selectedDeliveryOption.DeliveryCountry;
                    }
                }
                else
                {
                    while (String.IsNullOrEmpty(this.Ticket.DeliveryOption) && tick.IfSelectDeliveryWindowOpen)
                    {
                        if ((!this.IfWorking || !this.Ticket.isRunning))
                        {
                            break;
                        }
                        Thread.Sleep(500);
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.Ticket.DeliveryOption))
            {
                //if (!string.IsNullOrEmpty(this.Ticket.DeliveryCountry))
                //{
                //    foreach (KeyValuePair<String, String> item in countryWiseDeliveryOptions)
                //    {
                //        if (item.Key.ToLower().Contains(this.Ticket.DeliveryCountry.ToLower()))
                //        {
                //            foreach (HtmlNode ele in item.Value)
                //            {
                //                HtmlAttributeCollection prop = ele.Attributes;
                //                if (item.Value.ToLower().Contains(this.Ticket.DeliveryOption.ToLower()))
                //                {
                //                    selectedDeliveryOption = item.Key;
                //                    break;
                //                }
                //            }
                //        }
                //        if (selectedDeliveryOption != null)
                //        {
                //            break;
                //        }
                //    }
                //}
                //else
                //{
                foreach (KeyValuePair<String, String> item in countryWiseDeliveryOptions)
                {
                    //foreach (HtmlNode ele in item.Value)
                    //{
                    //    HtmlAttributeCollection prop = ele.Attributes;
                    if (item.Value.ToLower().Contains(this.Ticket.DeliveryOption.ToLower()))
                    {
                        selectedDeliveryOption = item.Key;
                        break;
                    }
                    //}

                    if (selectedDeliveryOption != null)
                    {
                        break;
                    }
                }
                //}
            }
            return selectedDeliveryOption;
        }

        private Dictionary<String, String> extractDeliveryOptions()
        {
            Dictionary<String, String> countryWiseDeliveryOptions = null;

            try
            {
                HtmlAgilityPack.HtmlDocument hdoc = this._session.HtmlDocument;
                HtmlAgilityPack.HtmlNodeCollection hNodes = null;
                hNodes = hdoc.DocumentNode.SelectNodes("//table[@class='cartproductbox']/tr/td/select/option");

                if (hNodes == null)
                {
                    hNodes = hdoc.DocumentNode.SelectNodes("//div[contains(@class,'cartproductbox')]/div[contains(@class,'deliverymethod')]//select/option");
                }

                if (hNodes == null)
                {
                    hNodes = hdoc.DocumentNode.SelectNodes("//select[contains(@class,'axs-select mod-select')]//option");
                }

                if (hNodes != null)
                {
                    countryWiseDeliveryOptions = new Dictionary<String, String>();

                    //HtmlAgilityPack.HtmlNodeCollection hNodesHead = hdoc.DocumentNode.SelectNodes("//div[@class='do-head']/ul/li/a");
                    foreach (HtmlAgilityPack.HtmlNode item in hNodes)
                    {
                        try
                        {
                            //HtmlAgilityPack.HtmlNode item = hNodes.FirstOrDefault(p => p.Attributes["id"].Value == itemHead.Attributes["href"].Value.Replace("#", ""));
                            //if (item != null)
                            //{
                            String hNodeDeliveriesId = item.Attributes["value"].Value;
                            String hNodeDeliveries = item.NextSibling.InnerText.Trim();

                            //if (hNodeDeliveriesFirst != null)
                            //{
                            //if (hNodeDeliveriesFirst.Count > 0)
                            //{
                            //    countryWiseDeliveryOptions.Add(itemHead.InnerText, hNodeDeliveriesFirst);
                            //}
                            //}

                            countryWiseDeliveryOptions.Add(hNodeDeliveriesId, hNodeDeliveries);

                            //if (hNodeDeliveries != null)
                            //{
                            //if (hNodeDeliveries.Count > 0)
                            //{
                            //    if (countryWiseDeliveryOptions.ContainsKey(itemHead.InnerText))
                            //    {
                            //        for (int i = 0; i < hNodeDeliveries.Count; i++)
                            //        {
                            //            countryWiseDeliveryOptions[itemHead.InnerText].Add(hNodeDeliveries[i]);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        countryWiseDeliveryOptions.Add(itemHead.InnerText, hNodeDeliveries);
                            //    }
                            //}
                            //}
                            //}
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {
                countryWiseDeliveryOptions = null;
            }

            //try
            //{
            //    HtmlAgilityPack.HtmlDocument hdoc = this._session.HtmlDocument;
            //    HtmlAgilityPack.HtmlNodeCollection hNodes = null;
            //    hNodes = hdoc.DocumentNode.SelectNodes("//table[@class='cartproductbox']/tr/td/select/option");

            //    if (hNodes != null)
            //    {
            //        countryWiseDeliveryOptions = new Dictionary<String, String>();

            //        HtmlAgilityPack.HtmlNodeCollection hNodesHead = hdoc.DocumentNode.SelectNodes("//div[@class='do-head']/ul/li/a");
            //        foreach (HtmlAgilityPack.HtmlNode itemHead in hNodesHead)
            //        {
            //            try
            //            {
            //                HtmlAgilityPack.HtmlNode item = hNodes.FirstOrDefault(p => p.Attributes["id"].Value == itemHead.Attributes["href"].Value.Replace("#", ""));
            //                if (item != null)
            //                {
            //                    HtmlAgilityPack.HtmlNodeCollection hNodeDeliveriesFirst = item.SelectNodes("table/tr/td/input[@name='vv_delivery_choice']");
            //                    HtmlAgilityPack.HtmlNodeCollection hNodeDeliveries = item.SelectNodes("div/table/tr/td/input[@name='vv_delivery_choice']");

            //                    if (hNodeDeliveriesFirst != null)
            //                    {
            //                        if (hNodeDeliveriesFirst.Count > 0)
            //                        {
            //                            countryWiseDeliveryOptions.Add(itemHead.InnerText, hNodeDeliveriesFirst);
            //                        }
            //                    }

            //                    if (hNodeDeliveries != null)
            //                    {
            //                        if (hNodeDeliveries.Count > 0)
            //                        {
            //                            if (countryWiseDeliveryOptions.ContainsKey(itemHead.InnerText))
            //                            {
            //                                for (int i = 0; i < hNodeDeliveries.Count; i++)
            //                                {
            //                                    countryWiseDeliveryOptions[itemHead.InnerText].Add(hNodeDeliveries[i]);
            //                                }
            //                            }
            //                            else
            //                            {
            //                                countryWiseDeliveryOptions.Add(itemHead.InnerText, hNodeDeliveries);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            catch (Exception)
            //            {

            //            }
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    countryWiseDeliveryOptions = null;
            //}

            return countryWiseDeliveryOptions;
        }

        private Dictionary<String, String> extractDeliveryOptions(bool axsEvent)
        {
            Dictionary<String, String> countryWiseDeliveryOptions = null;

            try
            {
                HtmlAgilityPack.HtmlDocument hdoc = this._session.HtmlDocument;
                HtmlAgilityPack.HtmlNodeCollection hNodes = null;
                hNodes = hdoc.DocumentNode.SelectNodes("//table[@class='cartproductbox']/tr/td/select/option");

                if (hNodes == null)
                {
                    hNodes = hdoc.DocumentNode.SelectNodes("//div[contains(@class,'cartproductbox')]/div[contains(@class,'deliverymethod')]//select/option");
                }

                if (hNodes != null)
                {
                    countryWiseDeliveryOptions = new Dictionary<String, String>();

                    //HtmlAgilityPack.HtmlNodeCollection hNodesHead = hdoc.DocumentNode.SelectNodes("//div[@class='do-head']/ul/li/a");
                    foreach (HtmlAgilityPack.HtmlNode item in hNodes)
                    {
                        try
                        {
                            //HtmlAgilityPack.HtmlNode item = hNodes.FirstOrDefault(p => p.Attributes["id"].Value == itemHead.Attributes["href"].Value.Replace("#", ""));
                            //if (item != null)
                            //{
                            String hNodeDeliveriesId = item.Attributes["value"].Value;
                            String hNodeDeliveries = item.NextSibling.InnerText.Trim();

                            //if (hNodeDeliveriesFirst != null)
                            //{
                            //if (hNodeDeliveriesFirst.Count > 0)
                            //{
                            //    countryWiseDeliveryOptions.Add(itemHead.InnerText, hNodeDeliveriesFirst);
                            //}
                            //}

                            countryWiseDeliveryOptions.Add(hNodeDeliveriesId, hNodeDeliveries);

                            //if (hNodeDeliveries != null)
                            //{
                            //if (hNodeDeliveries.Count > 0)
                            //{
                            //    if (countryWiseDeliveryOptions.ContainsKey(itemHead.InnerText))
                            //    {
                            //        for (int i = 0; i < hNodeDeliveries.Count; i++)
                            //        {
                            //            countryWiseDeliveryOptions[itemHead.InnerText].Add(hNodeDeliveries[i]);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        countryWiseDeliveryOptions.Add(itemHead.InnerText, hNodeDeliveries);
                            //    }
                            //}
                            //}
                            //}
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {
                countryWiseDeliveryOptions = null;
            }

            //try
            //{
            //    HtmlAgilityPack.HtmlDocument hdoc = this._session.HtmlDocument;
            //    HtmlAgilityPack.HtmlNodeCollection hNodes = null;
            //    hNodes = hdoc.DocumentNode.SelectNodes("//table[@class='cartproductbox']/tr/td/select/option");

            //    if (hNodes != null)
            //    {
            //        countryWiseDeliveryOptions = new Dictionary<String, String>();

            //        HtmlAgilityPack.HtmlNodeCollection hNodesHead = hdoc.DocumentNode.SelectNodes("//div[@class='do-head']/ul/li/a");
            //        foreach (HtmlAgilityPack.HtmlNode itemHead in hNodesHead)
            //        {
            //            try
            //            {
            //                HtmlAgilityPack.HtmlNode item = hNodes.FirstOrDefault(p => p.Attributes["id"].Value == itemHead.Attributes["href"].Value.Replace("#", ""));
            //                if (item != null)
            //                {
            //                    HtmlAgilityPack.HtmlNodeCollection hNodeDeliveriesFirst = item.SelectNodes("table/tr/td/input[@name='vv_delivery_choice']");
            //                    HtmlAgilityPack.HtmlNodeCollection hNodeDeliveries = item.SelectNodes("div/table/tr/td/input[@name='vv_delivery_choice']");

            //                    if (hNodeDeliveriesFirst != null)
            //                    {
            //                        if (hNodeDeliveriesFirst.Count > 0)
            //                        {
            //                            countryWiseDeliveryOptions.Add(itemHead.InnerText, hNodeDeliveriesFirst);
            //                        }
            //                    }

            //                    if (hNodeDeliveries != null)
            //                    {
            //                        if (hNodeDeliveries.Count > 0)
            //                        {
            //                            if (countryWiseDeliveryOptions.ContainsKey(itemHead.InnerText))
            //                            {
            //                                for (int i = 0; i < hNodeDeliveries.Count; i++)
            //                                {
            //                                    countryWiseDeliveryOptions[itemHead.InnerText].Add(hNodeDeliveries[i]);
            //                                }
            //                            }
            //                            else
            //                            {
            //                                countryWiseDeliveryOptions.Add(itemHead.InnerText, hNodeDeliveries);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            catch (Exception)
            //            {

            //            }
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    countryWiseDeliveryOptions = null;
            //}

            return countryWiseDeliveryOptions;
        }

        public void extractFoundValues()
        {
            Regex.CacheSize = 0;
            HtmlNodeCollection detailsRow = this._session.HtmlDocument.DocumentNode.SelectNodes("//table[contains(@class,'cartdetailbox')]");

            if (detailsRow == null)
            {
                detailsRow = this._session.HtmlDocument.DocumentNode.SelectNodes("//div[contains(@class,'cartdetailbox')]");
            }



            //int quantity = 0;

            if (detailsRow != null)
            {
                foreach (HtmlNode itemRow in detailsRow)
                {
                    HtmlNodeCollection details = itemRow.SelectNodes("tr/td");

                    if (details == null)
                    {
                        details = itemRow.SelectNodes("div");
                    }

                    //MatchCollection foundHTMLAreaMatches = Regex.Matches(this._session.HtmlDocument.DocumentNode.InnerHtml, "<div class=\"shipLabelLeft\"(.*?)>(.*?)((\n)*(.*?))*</div>((\n)*(.*?))*<div class=\"shipValueRight\"(.*?)>(.*?)((\n)*(.*?))*</div>", RegexOptions.IgnoreCase);
                    if (details != null)
                    {
                        if (details.Count > 0)
                        {
                            foreach (HtmlNode item in details)
                            {
                                if (item.SelectSingleNode("span").Attributes["id"].Value.ToLower().Contains("section"))
                                {
                                    if (String.IsNullOrEmpty(this.Section))
                                    {
                                        this.Section = item.SelectSingleNode("span").InnerText;
                                    }
                                    else if (!this.Section.ToLower().Contains(item.SelectSingleNode("span").Attributes["id"].Value.ToLower()))
                                    {
                                        this.Section += ", " + item.SelectSingleNode("span").InnerText;
                                    }
                                }
                                else if (item.SelectSingleNode("span").Attributes["id"].Value.ToLower().Contains("row"))
                                {
                                    //this.Row = item.SelectSingleNode("span").InnerText;
                                    if (String.IsNullOrEmpty(this.Row))
                                    {
                                        this.Row = item.SelectSingleNode("span").InnerText;
                                    }
                                    else if (!this.Row.ToLower().Contains(item.SelectSingleNode("span").Attributes["id"].Value.ToLower()))
                                    {
                                        this.Row += ", " + item.SelectSingleNode("span").InnerText;
                                    }
                                }
                                else if (item.SelectSingleNode("span").Attributes["id"].Value.ToLower().Contains("seats"))
                                {
                                    //this.Seat = item.SelectSingleNode("span").InnerText;
                                    if (String.IsNullOrEmpty(this.Seat))
                                    {
                                        this.Seat = item.SelectSingleNode("span").InnerText;
                                    }
                                    else if (!this.Seat.ToLower().Contains(item.SelectSingleNode("span").Attributes["id"].Value.ToLower()))
                                    {
                                        this.Seat += ", " + item.SelectSingleNode("span").InnerText;
                                    }

                                }
                                else if (item.SelectSingleNode("span").Attributes["id"].Value.ToLower().Contains("restriction"))
                                {
                                    //this.Description += "Restriction: " + item.SelectSingleNode("span").InnerText + ", ";
                                    if (String.IsNullOrEmpty(this.Description))
                                    {
                                        this.Description += "Restriction: " + item.SelectSingleNode("span").InnerText + ", ";
                                    }
                                    else if (!this.Description.ToLower().Contains(item.SelectSingleNode("span").Attributes["id"].Value.ToLower()))
                                    {
                                        this.Description += ", " + "Restriction: " + item.SelectSingleNode("span").InnerText;
                                    }
                                }
                                else if (item.SelectSingleNode("span").Attributes["id"].Value.ToLower().Contains("neighberhood"))
                                {
                                    //this.Description += "Neighberhood: " + item.SelectSingleNode("span").InnerText + " ";
                                    if (String.IsNullOrEmpty(this.Description))
                                    {
                                        this.Description += "Neighberhood: " + item.SelectSingleNode("span").InnerText + " ";
                                    }
                                    else if (!this.Description.ToLower().Contains(item.SelectSingleNode("span").Attributes["id"].Value.ToLower()))
                                    {
                                        this.Description += ", " + "Neighberhood: " + item.SelectSingleNode("span").InnerText;
                                    }
                                }
                                else if (item.SelectSingleNode("span").Attributes["id"].Value.ToLower().Contains("pricecode"))
                                {
                                    //this.MoreInfo = "Type: " + item.SelectSingleNode("span").InnerText;
                                    if (String.IsNullOrEmpty(this.MoreInfo))
                                    {
                                        this.MoreInfo = "Type: " + item.SelectSingleNode("span").InnerText;
                                    }
                                    else if (!this.MoreInfo.ToLower().Contains(item.SelectSingleNode("span").Attributes["id"].Value.ToLower()))
                                    {
                                        this.MoreInfo += ", " + "Type: " + item.SelectSingleNode("span").InnerText;
                                    }
                                }
                                else if (item.SelectSingleNode("span").Attributes["id"].Value.ToLower().Contains("seatcount"))
                                {
                                    //this.Quantity = item.SelectSingleNode("span").InnerText;
                                    if (String.IsNullOrEmpty(this.Quantity))
                                    {
                                        this.Quantity = item.SelectSingleNode("span").InnerText;
                                    }
                                    else
                                    {
                                        int qty = Convert.ToInt32(item.SelectSingleNode("span").InnerText) + Convert.ToInt32(this.Quantity);
                                        this.Quantity = qty.ToString();
                                    }
                                }
                                else if (item.SelectSingleNode("span").Attributes["id"].Value.ToLower().Contains("price"))
                                {
                                    //this.Price = item.SelectSingleNode("span").InnerText;
                                    if (String.IsNullOrEmpty(this.Price))
                                    {
                                        this.Price = item.SelectSingleNode("span").InnerText;
                                    }
                                    else if (!this.Price.ToLower().Contains(item.SelectSingleNode("span").Attributes["id"].Value.ToLower()))
                                    {
                                        this.Price += ", " + item.SelectSingleNode("span").InnerText;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                try
                {

                    HtmlNode details = this._session.HtmlDocument.DocumentNode.SelectSingleNode("//tr[contains(@class,'cart__box-item-detail')]");

                    if (details != null)
                    {
                        this.Quantity = details.SelectSingleNode(".//span[contains(@id,'lblSeatCount')]").InnerText;

                        this.Description = details.SelectSingleNode(".//span[contains(@id,'lblPriceCode')]").InnerText;

                        this.Price = details.SelectSingleNode(".//span[@id='m_c_Cart1_rptOfferItems_ctl01_rptZoneItems_ctl01_rptDetails_ctl01_lblPrice']").InnerText;

                        this.Seat = details.SelectSingleNode(".//span[contains(@id,'lblSeats')]").InnerText;

                        this.Section = details.SelectSingleNode(".//span[contains(@id,'lblSection')]").InnerText;

                        this.Row = details.SelectSingleNode(".//span[contains(@id,'lblRow')]").InnerText;

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }

            }
        }

        protected Boolean processRefreshPage()
        {
            Boolean result = false;
            String urlToGet = "";
            try
            {
                changeStatus(TicketSearchStatus.RefreshPageStatus);

                Regex.CacheSize = 0;
                Match mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "http-equiv=\"refresh\"", RegexOptions.IgnoreCase);
                if (!mRefresh.Success)
                {
                    mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "http-equiv=refresh", RegexOptions.IgnoreCase);
                }
                while (mRefresh.Success && this.IfWorking && this.Ticket.isRunning)
                {
                    mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "url=(.*?)\">", RegexOptions.IgnoreCase);
                    if (mRefresh.Success)
                    {
                        urlToGet = mRefresh.Value.Replace("url=", "").Replace("\">", "");

                        int wait = 1;
                        //Don't delete the code below ************************
                        //int wait = 4;                        
                        //mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "http-equiv=\"refresh\"(\\s+)content=\"(\\d+);", RegexOptions.IgnoreCase);
                        //if (mRefresh.Success)
                        //{
                        //    mRefresh = Regex.Match(mRefresh.Value, "\\d+");
                        //    if (mRefresh.Success)
                        //    {
                        //        wait = int.Parse(mRefresh.Value);
                        //    }
                        //}

                        DateTime dateTimeFound = DateTime.Now;
                        TimeSpan ts = dateTimeFound.AddSeconds(wait) - dateTimeFound;

                        while (ts.TotalSeconds > 1 && this.IfWorking && this.Ticket.isRunning)
                        {
                            ts = ts.Subtract(new TimeSpan(0, 0, 1));
                            //System.Threading.Thread.Sleep(1000);
                            sleep(1000);

                            changeStatus(TicketSearchStatus.RefreshPageStatus);
                        }


                        this._session.HTMLWeb.IfAllowAutoRedirect = true;
                        String html = this._session.Get(urlToGet);
                        this._session.HTMLWeb.IfAllowAutoRedirect = false;
                    }
                    //

                    mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "http-equiv=\"refresh\"", RegexOptions.IgnoreCase);
                    if (!mRefresh.Success)
                    {
                        mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "http-equiv=refresh", RegexOptions.IgnoreCase);
                    }
                }

                if (this.IfWorking && this.Ticket.isRunning)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        protected Boolean processWaitingPage()
        {
            Boolean result = false;
            String urlToGet = "";
            try
            {
                Regex.CacheSize = 0;
                Match mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "http-equiv=\"refresh\"", RegexOptions.IgnoreCase);
                if (!mRefresh.Success)
                {
                    mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "http-equiv=refresh", RegexOptions.IgnoreCase);
                }
                while (mRefresh.Success && this.IfWorking && this.Ticket.isRunning)
                {
                    changeStatus(TicketSearchStatus.QueuePageStatus);

                    int wait = 1;
                    //Don't delete the code below ************************
                    //int wait = 4;                    
                    //mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "http-equiv=\"refresh\"(\\s+)content=\"(\\d+);", RegexOptions.IgnoreCase);
                    //if (mRefresh.Success)
                    //{
                    //    mRefresh = Regex.Match(mRefresh.Value, "\\d+");
                    //    if (mRefresh.Success)
                    //    {
                    //        wait = int.Parse(mRefresh.Value);
                    //    }
                    //}

                    DateTime dateTimeFound = DateTime.Now;
                    TimeSpan ts = dateTimeFound.AddSeconds(wait) - dateTimeFound;

                    while (ts.TotalSeconds > 1 && this.IfWorking && this.Ticket.isRunning)
                    {
                        ts = ts.Subtract(new TimeSpan(0, 0, 1));
                        //System.Threading.Thread.Sleep(1000);
                        sleep(1000);

                        changeStatus(TicketSearchStatus.QueuePageStatus);
                    }

                    changeStatus(TicketSearchStatus.RefreshingQueuePageStatus);

                    //urlToGet = this._session.HTMLWeb.ResponseUri.AbsoluteUri;
                    urlToGet = this.Ticket.URL;
                    this._session.HTMLWeb.IfAllowAutoRedirect = true;
                    String html = this._session.Get(urlToGet);
                    this._session.HTMLWeb.IfAllowAutoRedirect = false;

                    //                    
                    mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "http-equiv=\"refresh\"", RegexOptions.IgnoreCase);
                    if (!mRefresh.Success)
                    {
                        mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "http-equiv=refresh", RegexOptions.IgnoreCase);
                    }
                }

                if (this.IfWorking && this.Ticket.isRunning)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public ITicketParameter getNextParameter()
        {
            ITicketParameter parameter = null;
            VSTicket ticket = (VSTicket)this.Ticket;

            //lock (this.Ticket)
            {
                //lock (this.Ticket.Parameters)
                {
                    if (this.Ticket.ifUseFoundOnFirstAttempt)
                    {
                        IEnumerable<VSParameter> tmpParamters = this.Ticket.Parameters.Where(p => p.IfFound == true);
                        if (tmpParamters != null)
                        {
                            if (ticket.CurrentFoundParameterIndex >= tmpParamters.Count())
                            {
                                ticket.CurrentFoundParameterIndex = 0;
                            }
                            parameter = tmpParamters.ElementAtOrDefault(ticket.CurrentFoundParameterIndex);
                            if (parameter != null)
                            {
                                ticket.CurrentFoundParameterIndex++;
                            }
                        }
                    }
                    else if (this.Ticket.ifUseAvailableParameters)
                    {
                        IEnumerable<VSParameter> tmpParamters = this.Ticket.Parameters.Where(p => p.IfAvailable == true);
                        if (tmpParamters != null)
                        {
                            if (ticket.CurrentAvailableParameterIndex >= tmpParamters.Count())
                            {
                                ticket.CurrentAvailableParameterIndex = 0;
                            }
                            parameter = tmpParamters.ElementAtOrDefault(ticket.CurrentAvailableParameterIndex);
                            if (parameter != null)
                            {
                                ticket.CurrentAvailableParameterIndex++;
                            }
                        }
                    }

                    if (parameter == null)
                    {
                        if (ticket.CurrentParameterIndex >= this.Ticket.Parameters.Count)
                        {
                            ticket.CurrentParameterIndex = 0;
                        }

                        parameter = this.Ticket.Parameters[ticket.CurrentParameterIndex];

                        ticket.CurrentParameterIndex++;
                    }
                }
            }
            return parameter;
        }

        public VSPriceLevel getPriceLevel(ITicketParameter parameter, VSTicketType ticketType)
        {
            VSPriceLevel selectedPriceLevel = null;

            //If user provides the price range then find the right price level and return the price level.
            if (parameter.PriceMin != null && parameter.PriceMax != null && ticketType.PriceLevels != null)
            {
                SortedList<decimal, VSPriceLevel> sortedPriceLevels = new SortedList<decimal, VSPriceLevel>();



                if (String.IsNullOrEmpty(parameter.PriceLevel))
                {
                    foreach (VSPriceLevel priceLevel in ticketType.PriceLevels)
                    {
                        if (!sortedPriceLevels.ContainsKey(priceLevel.Price))
                        {
                            if (priceLevel.Price != 0)
                            {
                                sortedPriceLevels.Add(priceLevel.Price, priceLevel);
                            }
                        }
                    }
                    if (parameter.MaxToMin)
                    {
                        IOrderedEnumerable<KeyValuePair<decimal, VSPriceLevel>> sortedPriceLevelsDesc = sortedPriceLevels.OrderByDescending(p => p.Key);
                        foreach (KeyValuePair<decimal, VSPriceLevel> priceLevel in sortedPriceLevelsDesc)
                        {
                            if (priceLevel.Key >= parameter.PriceMin && priceLevel.Key <= parameter.PriceMax)
                            {
                                selectedPriceLevel = priceLevel.Value;
                                break;
                            }
                            //if (ticketType.PriceLevels[0].MinPrice >= parameter.PriceMin && ticketType.PriceLevels[0].MaxPrice <= parameter.PriceMax)
                            //{
                            //    selectedPriceLevel = ticketType.PriceLevels[0];
                            //}
                        }
                    }
                    else // If user wants to find the right price level from min total price to max total price.
                    {
                        //foreach (VSPriceLevel priceLevel in ticketType.PriceLevels)
                        //{
                        //if (ticketType.PriceLevels[0].MinPrice >= parameter.PriceMin && ticketType.PriceLevels[0].MaxPrice <= parameter.PriceMax)
                        //{
                        //    selectedPriceLevel = ticketType.PriceLevels[0];
                        //}
                        //}
                        foreach (KeyValuePair<decimal, VSPriceLevel> priceLevel in sortedPriceLevels)
                        {
                            if (priceLevel.Key >= parameter.PriceMin && priceLevel.Key <= parameter.PriceMax)
                            {
                                selectedPriceLevel = priceLevel.Value;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    //If user provides the price range then find the right price level and return the price level.
                    if (parameter.PriceMin != null && parameter.PriceMax != null && ticketType.PriceLevels != null)
                    {
                        foreach (VSPriceLevel priceLevel in ticketType.PriceLevels)
                        {
                            if (!sortedPriceLevels.ContainsKey(priceLevel.Price))
                            {
                                if (priceLevel.Price != 0)
                                {
                                    sortedPriceLevels.Add(priceLevel.Price, priceLevel);
                                }
                            }
                        }
                        //SortedList<decimal, VSPriceLevel> sortedPriceLevels = new SortedList<decimal, VSPriceLevel>();

                        //foreach (VSPriceLevel priceLevel in ticketType.PriceLevels)
                        //{
                        //    if (!sortedPriceLevels.ContainsKey(priceLevel.Price))
                        //    {
                        //        sortedPriceLevels.Add(priceLevel.Price, priceLevel);
                        //    }
                        //}

                        // If user wants to find the right price level from max total price to min total price.
                        if (parameter.MaxToMin)
                        {
                            ////IOrderedEnumerable<KeyValuePair<decimal, VSPriceLevel>> sortedPriceLevelsDesc = sortedPriceLevels.OrderByDescending(p => p.Key);
                            //foreach (VSPriceLevel priceLevel in ticketType.PriceLevels)
                            //{
                            //    if (priceLevel.MinPrice >= parameter.PriceMin && priceLevel.MaxPrice <= parameter.PriceMax)
                            //    {
                            //        selectedPriceLevel = priceLevel;
                            //        break;
                            //    }
                            //}
                            IOrderedEnumerable<KeyValuePair<decimal, VSPriceLevel>> sortedPriceLevelsDesc = sortedPriceLevels.OrderByDescending(p => p.Key);
                            foreach (KeyValuePair<decimal, VSPriceLevel> priceLevel in sortedPriceLevelsDesc)
                            {
                                if (priceLevel.Key >= parameter.PriceMin && priceLevel.Key <= parameter.PriceMax)
                                {
                                    selectedPriceLevel = priceLevel.Value;
                                    break;
                                }
                                //if (ticketType.PriceLevels[0].MinPrice >= parameter.PriceMin && ticketType.PriceLevels[0].MaxPrice <= parameter.PriceMax)
                                //{
                                //    selectedPriceLevel = ticketType.PriceLevels[0];
                                //}
                            }
                        }
                        else // If user wants to find the right price level from min total price to max total price.
                        {
                            //foreach (VSPriceLevel priceLevel in ticketType.PriceLevels)
                            //{
                            //    if (priceLevel.MinPrice >= parameter.PriceMin && priceLevel.MaxPrice <= parameter.PriceMax)
                            //    {
                            //        selectedPriceLevel = priceLevel;
                            //        break;
                            //    }
                            //}
                            foreach (KeyValuePair<decimal, VSPriceLevel> priceLevel in sortedPriceLevels)
                            {
                                if (priceLevel.Key >= parameter.PriceMin && priceLevel.Key <= parameter.PriceMax)
                                {
                                    selectedPriceLevel = priceLevel.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
                //else // if user does not provide the price level then male the empty price level and return it.
                //{
                //    //selectedPriceLevel = new VSPriceLevel(String.Empty, 0, 0, String.Empty, String.Empty, String.Empty);
                //}
            }
            return selectedPriceLevel;
        }

        public VSPriceLevel getPriceLevel(ITicketParameter parameter, VSNeighborhood neighbour)
        {
            VSPriceLevel selectedPriceLevel = null;

            //If user provides the price range then find the right price level and return the price level.
            if (parameter.PriceMin != null && parameter.PriceMax != null && neighbour.PriceLevels != null)
            {
                SortedList<decimal, VSPriceLevel> sortedPriceLevels = new SortedList<decimal, VSPriceLevel>();



                if (String.IsNullOrEmpty(parameter.PriceLevel))
                {
                    foreach (VSPriceLevel priceLevel in neighbour.PriceLevels)
                    {
                        if (!sortedPriceLevels.ContainsKey(priceLevel.Price))
                        {
                            if (priceLevel.Price != 0)
                            {
                                sortedPriceLevels.Add(priceLevel.Price, priceLevel);
                            }
                        }
                    }
                    if (parameter.MaxToMin)
                    {
                        IOrderedEnumerable<KeyValuePair<decimal, VSPriceLevel>> sortedPriceLevelsDesc = sortedPriceLevels.OrderByDescending(p => p.Key);
                        foreach (KeyValuePair<decimal, VSPriceLevel> priceLevel in sortedPriceLevelsDesc)
                        {
                            if (priceLevel.Key >= parameter.PriceMin && priceLevel.Key <= parameter.PriceMax)
                            {
                                selectedPriceLevel = priceLevel.Value;
                                break;
                            }
                            //if (ticketType.PriceLevels[0].MinPrice >= parameter.PriceMin && ticketType.PriceLevels[0].MaxPrice <= parameter.PriceMax)
                            //{
                            //    selectedPriceLevel = ticketType.PriceLevels[0];
                            //}
                        }
                    }
                    else // If user wants to find the right price level from min total price to max total price.
                    {
                        //foreach (VSPriceLevel priceLevel in ticketType.PriceLevels)
                        //{
                        //if (ticketType.PriceLevels[0].MinPrice >= parameter.PriceMin && ticketType.PriceLevels[0].MaxPrice <= parameter.PriceMax)
                        //{
                        //    selectedPriceLevel = ticketType.PriceLevels[0];
                        //}
                        //}
                        foreach (KeyValuePair<decimal, VSPriceLevel> priceLevel in sortedPriceLevels)
                        {
                            if (priceLevel.Key >= parameter.PriceMin && priceLevel.Key <= parameter.PriceMax)
                            {
                                selectedPriceLevel = priceLevel.Value;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    //If user provides the price range then find the right price level and return the price level.
                    if (parameter.PriceMin != null && parameter.PriceMax != null && neighbour.PriceLevels != null)
                    {
                        foreach (VSPriceLevel priceLevel in neighbour.PriceLevels)
                        {
                            if (!sortedPriceLevels.ContainsKey(priceLevel.Price))
                            {
                                if (priceLevel.Price != 0)
                                {
                                    sortedPriceLevels.Add(priceLevel.Price, priceLevel);
                                }
                            }
                        }
                        //SortedList<decimal, VSPriceLevel> sortedPriceLevels = new SortedList<decimal, VSPriceLevel>();

                        //foreach (VSPriceLevel priceLevel in ticketType.PriceLevels)
                        //{
                        //    if (!sortedPriceLevels.ContainsKey(priceLevel.Price))
                        //    {
                        //        sortedPriceLevels.Add(priceLevel.Price, priceLevel);
                        //    }
                        //}

                        // If user wants to find the right price level from max total price to min total price.
                        if (parameter.MaxToMin)
                        {
                            ////IOrderedEnumerable<KeyValuePair<decimal, VSPriceLevel>> sortedPriceLevelsDesc = sortedPriceLevels.OrderByDescending(p => p.Key);
                            //foreach (VSPriceLevel priceLevel in ticketType.PriceLevels)
                            //{
                            //    if (priceLevel.MinPrice >= parameter.PriceMin && priceLevel.MaxPrice <= parameter.PriceMax)
                            //    {
                            //        selectedPriceLevel = priceLevel;
                            //        break;
                            //    }
                            //}
                            IOrderedEnumerable<KeyValuePair<decimal, VSPriceLevel>> sortedPriceLevelsDesc = sortedPriceLevels.OrderByDescending(p => p.Key);
                            foreach (KeyValuePair<decimal, VSPriceLevel> priceLevel in sortedPriceLevelsDesc)
                            {
                                if (priceLevel.Key >= parameter.PriceMin && priceLevel.Key <= parameter.PriceMax)
                                {
                                    selectedPriceLevel = priceLevel.Value;
                                    break;
                                }
                                //if (ticketType.PriceLevels[0].MinPrice >= parameter.PriceMin && ticketType.PriceLevels[0].MaxPrice <= parameter.PriceMax)
                                //{
                                //    selectedPriceLevel = ticketType.PriceLevels[0];
                                //}
                            }
                        }
                        else // If user wants to find the right price level from min total price to max total price.
                        {
                            //foreach (VSPriceLevel priceLevel in ticketType.PriceLevels)
                            //{
                            //    if (priceLevel.MinPrice >= parameter.PriceMin && priceLevel.MaxPrice <= parameter.PriceMax)
                            //    {
                            //        selectedPriceLevel = priceLevel;
                            //        break;
                            //    }
                            //}
                            foreach (KeyValuePair<decimal, VSPriceLevel> priceLevel in sortedPriceLevels)
                            {
                                if (priceLevel.Key >= parameter.PriceMin && priceLevel.Key <= parameter.PriceMax)
                                {
                                    selectedPriceLevel = priceLevel.Value;
                                    break;
                                }
                            }
                        }
                    }
                }
                //else // if user does not provide the price level then male the empty price level and return it.
                //{
                //    //selectedPriceLevel = new VSPriceLevel(String.Empty, 0, 0, String.Empty, String.Empty, String.Empty);
                //}
            }
            return selectedPriceLevel;
        }

        public Boolean mapParameterIfAvaiable(ITicketParameter parameter)
        {
            Boolean result = false;
            String strHTML = String.Empty;

            if (this._session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("home.aspx"))
            {
                if (!String.IsNullOrEmpty(this.Session.LocationURL))
                {
                    this._session.Get(this._session.LocationURL);
                }
            }

            if (this._session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("reserveseats.aspx"))
            {
                strHTML = this._session.Get(this._session.HTMLWeb.ResponseUri.AbsoluteUri.Replace("ReserveSeats", "PriceRange"));
            }
            if (this._session.FormElements.Keys.Contains("m:c:btnReserve"))
            {
                this._session.FormElements.Remove("m:c:btnReserve");
                strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
            }
            if (this._tmEvent != null && parameter != null && this._session != null && this._session.FormElements != null)
            {
                // If the Ticket Types exists in the first page then proceed further otherwise show message and keep searching.
                if (this._tmEvent.HasTicketTypes)
                {
                    this._session.FormElements.Remove("m:c:btnNBA");

                    // If user provides the Search String or Ticket Type String. Then Select price type according to parameter.
                    if (!String.IsNullOrEmpty(parameter.TicketTypeString))
                    {
                        Boolean ifTicketTypeStringMatch = false;
                        for (int i = 0; i < this._tmEvent.TicketTypes.Count; i++)
                        {
                            // Check if user mark checed on Exact Match or not and find for the provided Search String or Ticket Type String.
                            if (!ifTicketTypeStringMatch && (parameter.ExactMatch ? this._tmEvent.TicketTypes[i].Description.ToLower() == parameter.TicketTypeString.ToLower() : this._tmEvent.TicketTypes[i].Description.ToLower().Contains(parameter.TicketTypeString.ToLower())))
                            {
                                ifTicketTypeStringMatch = true;
                                // Select quantity from the matching Search String or Ticket Type String.
                                if (parameter.Quantity >= (this._tmEvent.TicketTypes[i].MinQuantity) && parameter.Quantity <= this._tmEvent.TicketTypes[i].MaxQuantity)
                                {
                                    if (parameter.Quantity % this._tmEvent.TicketTypes[i].QuantityStep == 0)
                                    {
                                        String key = this._tmEvent.TicketTypes[i].QuantityFormElementKey;
                                        this._session.FormElements[key] = parameter.Quantity.ToString();
                                        strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                                        HtmlNodeCollection _price = this._session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlPriceLevel')]/option");
                                        if (_price != null)
                                        {
                                            Boolean ifPriceStringMatch = false, ifSectionStringMatch = false;

                                            //If both the sections and price levels are provided
                                            if (!String.IsNullOrEmpty(parameter.PriceLevel) && !String.IsNullOrEmpty(parameter.Section))
                                            {
                                                foreach (VSPriceLevel priceLevel in this._tmEvent.TicketTypes[i].PriceLevels)
                                                {
                                                    if (!ifPriceStringMatch && (parameter.ExactMatch ? priceLevel.Description.ToLower() == parameter.PriceLevel.ToLower() : priceLevel.Description.ToLower().Contains(parameter.PriceLevel.ToLower())))
                                                    {
                                                        ifPriceStringMatch = true;

                                                        for (int j = 0; j < priceLevel.Section.Count; j++)
                                                        {
                                                            if (!ifSectionStringMatch && (parameter.ExactMatch ? priceLevel.Section[j].SecName.ToLower() == parameter.Section.ToLower() : priceLevel.Section[j].SecName.ToLower().Contains(parameter.Section.ToLower())))
                                                            {
                                                                ifSectionStringMatch = true;
                                                                AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", priceLevel.ID);
                                                                this._session.FormElements.Remove("m:c:btnChangeQty");
                                                                this._session.FormElements.Remove("m:c:btnReserve");
                                                                this._session.Post(_session.HTMLWeb.ResponseUri.AbsoluteUri);

                                                                if (parameter.PriceMax == null && parameter.PriceMin == null)
                                                                {
                                                                    //AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", priceLevel.ID);
                                                                    AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", priceLevel.Section[j].ID);
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    //TODO: match price with max nd min quantity
                                                                    VSPriceLevel value = getPriceLevel(parameter, this._tmEvent.TicketTypes[0]);
                                                                    if (value != null)
                                                                    {
                                                                        //AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", value.ID);
                                                                        AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        ////TODO: If no price match
                                                                        //this.MoreInfo = "Price range does not exists. Please review the minimum and maximum price range.";
                                                                        //return false;
                                                                    }
                                                                }

                                                            }
                                                        }

                                                    }
                                                }
                                                if (!ifSectionStringMatch && ifPriceStringMatch)
                                                {
                                                    this.MoreInfo = "\"" + parameter.Section + "\" " + TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                                    return false;
                                                }
                                                if (!ifPriceStringMatch)
                                                {
                                                    this.MoreInfo = "\"" + parameter.PriceLevel + "\" " + TicketSearchStatus.MoreInfoPriceLevelNotMatch;
                                                    return false;
                                                }
                                            }
                                            else if (String.IsNullOrEmpty(parameter.PriceLevel) && String.IsNullOrEmpty(parameter.Section))
                                            {
                                                //TODO: Match parameter if price level and section both values are not null

                                                if (parameter.PriceMax == null && parameter.PriceMin == null)
                                                {
                                                    AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", this._tmEvent.TicketTypes[i].PriceLevels[0].ID);
                                                    AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", this._tmEvent.TicketTypes[i].PriceLevels[0].Section[0].ID);
                                                    break;
                                                }
                                                else
                                                {
                                                    //TODO: match price with max nd min quantity
                                                    VSPriceLevel value = getPriceLevel(parameter, this._tmEvent.TicketTypes[i]);
                                                    if (value != null)
                                                    {
                                                        AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", value.ID);
                                                        AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        //TODO: If no price match
                                                        this.MoreInfo = "Price range does not exists. Please review the minimum and maximum price range.";
                                                        return false;
                                                    }
                                                }
                                            }
                                            else if (String.IsNullOrEmpty(parameter.PriceLevel) || String.IsNullOrEmpty(parameter.Section))
                                            {
                                                //TODO: Match parameter if price level or section both values are not null
                                                if (!String.IsNullOrEmpty(parameter.Section) && String.IsNullOrEmpty(parameter.PriceLevel))
                                                {
                                                    if (parameter.PriceMax != null && parameter.PriceMin != null)
                                                    {
                                                        VSPriceLevel value = getPriceLevel(parameter, this._tmEvent.TicketTypes[i]);
                                                        //if (value == this._tmEvent.TicketTypes[i].PriceLevels[0])
                                                        //{
                                                        if (value != null)
                                                        {
                                                            foreach (VSSection section in this._tmEvent.TicketTypes[i].PriceLevels[0].Section)
                                                            {
                                                                if (!ifSectionStringMatch && (parameter.ExactMatch ? section.SecName.ToLower() == parameter.Section.ToLower() : section.SecName.ToLower().Contains(parameter.Section.ToLower())))
                                                                {
                                                                    ifSectionStringMatch = true;
                                                                    AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", this._tmEvent.TicketTypes[i].PriceLevels[0].ID);
                                                                    AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);
                                                                    break;
                                                                }
                                                            }
                                                            if (!ifSectionStringMatch)
                                                            {
                                                                this.MoreInfo = TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                                                return false;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        foreach (VSSection section in this._tmEvent.TicketTypes[i].PriceLevels[0].Section)
                                                        {
                                                            if (!ifSectionStringMatch && (parameter.ExactMatch ? section.SecName.ToLower() == parameter.Section.ToLower() : section.SecName.ToLower().Contains(parameter.Section.ToLower())))
                                                            {
                                                                ifSectionStringMatch = true;
                                                                AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", this._tmEvent.TicketTypes[i].PriceLevels[0].ID);
                                                                AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);
                                                                break;
                                                            }
                                                        }
                                                        if (!ifSectionStringMatch)
                                                        {
                                                            this.MoreInfo = TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                                            return false;
                                                        }
                                                    }
                                                    //}
                                                }
                                                else if (!String.IsNullOrEmpty(parameter.PriceLevel) && String.IsNullOrEmpty(parameter.Section))
                                                {
                                                    for (int j = 0; j < this._tmEvent.TicketTypes[i].PriceLevels.Count; j++)
                                                    {
                                                        if (!ifPriceStringMatch && (parameter.ExactMatch ? this._tmEvent.TicketTypes[i].PriceLevels[j].Description.ToLower() == parameter.PriceLevel.ToLower() : this._tmEvent.TicketTypes[i].PriceLevels[j].Description.ToLower().Contains(parameter.PriceLevel.ToLower())))
                                                        {
                                                            AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", this._tmEvent.TicketTypes[i].PriceLevels[j].ID);
                                                            this._session.FormElements.Remove("m:c:btnChangeQty");
                                                            this._session.FormElements.Remove("m:c:btnReserve");
                                                            this._session.Post(_session.HTMLWeb.ResponseUri.AbsoluteUri);

                                                            if (parameter.PriceMax == null && parameter.PriceMin == null)
                                                            {
                                                                //AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", this._tmEvent.TicketTypes[i].PriceLevels[j].ID);
                                                                AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", this._tmEvent.TicketTypes[i].PriceLevels[j].Section[0].ID);
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                //TODO: match price with max nd min quantity
                                                                VSPriceLevel value = getPriceLevel(parameter, this._tmEvent.TicketTypes[i]);
                                                                if (value != null)
                                                                {
                                                                    if (value == this._tmEvent.TicketTypes[i].PriceLevels[j])
                                                                    {
                                                                        ifPriceStringMatch = true;
                                                                        //AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", value.ID);
                                                                        AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);
                                                                        break;
                                                                    }
                                                                    else
                                                                    {
                                                                        //TODO: If no price match
                                                                        //this.MoreInfo = "Price range does not match with the PriceLevel.";
                                                                        //return false;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //TODO: If no price match
                                                                    //this.MoreInfo = "Price range does not match.";
                                                                    //return false;
                                                                }
                                                            }

                                                        }
                                                    }
                                                    if (!ifPriceStringMatch)
                                                    {
                                                        this.MoreInfo = TicketSearchStatus.MoreInfoPriceLevelNotMatch;
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    this._session.FormElements.Remove("m:c:btnChangeQty");
                                                    //strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                                                    return true;
                                                }
                                            }
                                            else    //TODO: if no pricelevel and section are provided
                                            {

                                            }
                                        }
                                    }
                                    else// Show more info that quantity does not match.
                                    {
                                        this.MoreInfo = TicketSearchStatus.MoreInfoQuantityNotMatch + " " + parameter.Quantity + " is not valid quantity for this parameter.";
                                        return false;
                                    }
                                }
                                else// Show more info that quantity does not match.
                                {
                                    this.MoreInfo = TicketSearchStatus.MoreInfoQuantityNotMatch + " Allowed quantity is " + (this._tmEvent.TicketTypes[i].MinQuantity) + " to " + this._tmEvent.TicketTypes[i].MaxQuantity;
                                    return false;
                                }

                                ifTicketTypeStringMatch = true;
                            }
                            else // Put zero "0" in other price types or search strings for making the post request.
                            {
                                //this._session.FormElements.Add("w_ext_qty." + this._tmEvent.TicketTypes[i].V, "0");
                                //this._session.FormElements.Add("w_vv_ext_price." + this._tmEvent.TicketTypes[i].V, "");
                            }
                        }

                        // If Search String or Price Type String does not match then show message.
                        if (!ifTicketTypeStringMatch)
                        {
                            this.MoreInfo = "\"" + parameter.TicketTypeString + "\" " + TicketSearchStatus.MoreInfoTicketTypeStringNotMatch;
                            return false;
                        }
                    }
                    // If user provides the only Password and not Search String or Ticket Type String. Then select first available password field according to parameter and add it to the post request.
                    else if (!String.IsNullOrEmpty(parameter.TicketTypePasssword) && String.IsNullOrEmpty(parameter.TicketTypeString))
                    {
                        Boolean ifTicketTypePasswordFound = false;
                        for (int i = 0; i < this._tmEvent.TicketTypes.Count; i++)
                        {
                            //If the ticket type contains password or special offer field then add the password in the post request.
                            //if (!String.IsNullOrEmpty(this._tmEvent.TicketTypes[i].PasswordFormName) && !ifTicketTypePasswordFound)
                            //{
                            //    this._session.FormElements.Add(this._tmEvent.TicketTypes[i].PasswordFormName, parameter.TicketTypePasssword);

                            //    // Select quantity from the matching Ticket Type Password.
                            //    if (parameter.Quantity >= (this._tmEvent.TicketTypes[i].MinQuantity + 1) && parameter.Quantity <= this._tmEvent.TicketTypes[i].MaxQuantity)
                            //    {
                            //        if (parameter.Quantity % this._tmEvent.TicketTypes[i].QuantityStep == 0)
                            //        {
                            //            this._session.FormElements.Add("w_ext_qty." + this._tmEvent.TicketTypes[i].V, parameter.Quantity.ToString());
                            //        }
                            //        else// Show more info that quantity does not match.
                            //        {
                            //            this.MoreInfo = TicketSearchStatus.MoreInfoQuantityNotMatch + " " + parameter.Quantity + " is not valid quantity for this parameter.";
                            //            return false;
                            //        }
                            //    }
                            //    else// Show more info that quantity does not match.
                            //    {
                            //        this.MoreInfo = TicketSearchStatus.MoreInfoQuantityNotMatch + " Allowed quantity is " + (this._tmEvent.TicketTypes[i].MinQuantity + 1) + " to " + this._tmEvent.TicketTypes[i].MaxQuantity;
                            //        return false;
                            //    }

                            //    VSPriceLevel selectedPriceLevel = null;

                            //    // Find and get price level and map it to the post request.
                            //    selectedPriceLevel = getPriceLevel(parameter, this._tmEvent.TicketTypes[i].PriceLevels);

                            //    // If price level is null then it means that price level does not match according to the parameter.
                            //    if (selectedPriceLevel != null)
                            //    {
                            //        this._session.FormElements.Add("w_vv_ext_price." + this._tmEvent.TicketTypes[i].V, selectedPriceLevel.Section.V);
                            //    }
                            //    else // Show more info that price level does not match.
                            //    {
                            //        this.MoreInfo = TicketSearchStatus.MoreInfoPriceLevelNotMatch;
                            //        return false;
                            //    }

                            //    ifTicketTypePasswordFound = true;
                            //}
                            //else // Put zero "0" in other price types or search strings for making the post request.
                            //{
                            //    this._session.FormElements.Add("w_ext_qty." + this._tmEvent.TicketTypes[i].V, "0");
                            //    this._session.FormElements.Add("w_vv_ext_price." + this._tmEvent.TicketTypes[i].V, "");
                            //}
                        }

                        // If Password does not find then show message.
                        if (!ifTicketTypePasswordFound)
                        {
                            this.MoreInfo = TicketSearchStatus.MoreInfoTicketTypePasswordNotExist;
                            return false;
                        }
                    }
                    else // If user does not provide the Search String or Ticket Type String. Then Select first price type automatically.
                    {
                        if (this._tmEvent.TicketTypes.Count >= 1)
                        {
                            // Select quantity from the first Search String or Ticket Type String.
                            if (parameter.Quantity >= (this._tmEvent.TicketTypes[0].MinQuantity) && parameter.Quantity <= this._tmEvent.TicketTypes[0].MaxQuantity)
                            {
                                if (parameter.Quantity % this._tmEvent.TicketTypes[0].QuantityStep == 0)
                                {
                                    //this._session.FormElements.Add("w_ext_qty." + this._tmEvent.TicketTypes[0].V, parameter.Quantity.ToString());
                                    String key = this._tmEvent.TicketTypes[0].QuantityFormElementKey;
                                    this._session.FormElements[key] = parameter.Quantity.ToString();
                                    strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);

                                    //VSPriceLevel selectedPriceLevel = null;
                                    HtmlNodeCollection _price = this._session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlPriceLevel')]/option");
                                    if (_price != null)
                                    {
                                        Boolean ifPriceStringMatch = false, ifSectionStringMatch = false;

                                        //If both the sections and price levels are provided
                                        if (!String.IsNullOrEmpty(parameter.PriceLevel) && !String.IsNullOrEmpty(parameter.Section))
                                        {
                                            foreach (VSPriceLevel priceLevel in this._tmEvent.TicketTypes[0].PriceLevels)
                                            {

                                                if (!ifPriceStringMatch && (parameter.ExactMatch ? priceLevel.Description.ToLower() == parameter.PriceLevel.ToLower() : priceLevel.Description.ToLower().Contains(parameter.PriceLevel.ToLower())))
                                                {
                                                    //ifPriceStringMatch = true;
                                                    for (int j = 0; j < priceLevel.Section.Count; j++)
                                                    {
                                                        if (!ifSectionStringMatch && (parameter.ExactMatch ? priceLevel.Section[j].SecName.ToLower() == parameter.Section.ToLower() : priceLevel.Section[j].SecName.ToLower().Contains(parameter.Section.ToLower())))
                                                        {
                                                            ifSectionStringMatch = true;
                                                            ifPriceStringMatch = true;

                                                            AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", priceLevel.ID);
                                                            this._session.FormElements.Remove("m:c:btnChangeQty");
                                                            this._session.FormElements.Remove("m:c:btnReserve");
                                                            this._session.Post(_session.HTMLWeb.ResponseUri.AbsoluteUri);

                                                            if (parameter.PriceMax == null && parameter.PriceMin == null)
                                                            {
                                                                //AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", priceLevel.ID);
                                                                AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", priceLevel.Section[j].ID);
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                //TODO: match price with max nd min quantity
                                                                VSPriceLevel value = getPriceLevel(parameter, this._tmEvent.TicketTypes[0]);

                                                                if (value != null)
                                                                {
                                                                    if (value == priceLevel)
                                                                    {
                                                                        //AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", value.ID);
                                                                        AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[j].ID);
                                                                        break;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //TODO: If no price match
                                                                    this.MoreInfo = "Price range does not exists. Please review the minimum and maximum price range.";
                                                                    return false;
                                                                }
                                                            }

                                                        }
                                                    }

                                                }
                                            }
                                            if (!ifSectionStringMatch && ifPriceStringMatch)
                                            {
                                                this.MoreInfo = "\"" + parameter.Section + "\" " + TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                                return false;
                                            }
                                            if (!ifPriceStringMatch)
                                            {
                                                this.MoreInfo = "\"" + parameter.PriceLevel + "\" " + TicketSearchStatus.MoreInfoPriceLevelNotMatch;
                                                return false;
                                            }
                                        }
                                        else if (String.IsNullOrEmpty(parameter.PriceLevel) && String.IsNullOrEmpty(parameter.Section))
                                        {
                                            //TODO: Match parameter if price level and section both values are not null
                                            if (parameter.PriceMax == null && parameter.PriceMin == null)
                                            {
                                                AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", this._tmEvent.TicketTypes[0].PriceLevels[0].ID);
                                                AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", this._tmEvent.TicketTypes[0].PriceLevels[0].Section[0].ID);
                                            }
                                            else
                                            {
                                                //TODO: match price with max nd min quantity
                                                VSPriceLevel value = getPriceLevel(parameter, this._tmEvent.TicketTypes[0]);
                                                if (value != null)
                                                {
                                                    AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", value.ID);
                                                    AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);

                                                }
                                                else
                                                {
                                                    //TODO: If no price match
                                                    this.MoreInfo = "Price range does not exists. Please review the minimum and maximum price range.";
                                                    return false;
                                                }
                                            }
                                        }
                                        else if (String.IsNullOrEmpty(parameter.PriceLevel) || String.IsNullOrEmpty(parameter.Section))
                                        {
                                            //TODO: Match parameter if price level or section both values are not null
                                            if (!String.IsNullOrEmpty(parameter.Section) && String.IsNullOrEmpty(parameter.PriceLevel))
                                            {
                                                if (parameter.PriceMax != null && parameter.PriceMin != null)
                                                {
                                                    VSPriceLevel value = getPriceLevel(parameter, this._tmEvent.TicketTypes[0]);
                                                    //if (value == this._tmEvent.TicketTypes[i].PriceLevels[0])
                                                    //{
                                                    if (value != null)
                                                    {
                                                        foreach (VSSection section in this._tmEvent.TicketTypes[0].PriceLevels[0].Section)
                                                        {
                                                            if (!ifSectionStringMatch && (parameter.ExactMatch ? section.SecName.ToLower() == parameter.Section.ToLower() : section.SecName.ToLower().Contains(parameter.Section.ToLower())))
                                                            {
                                                                ifSectionStringMatch = true;
                                                                AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", this._tmEvent.TicketTypes[0].PriceLevels[0].ID);
                                                                AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);
                                                                break;
                                                            }
                                                        }
                                                        if (!ifSectionStringMatch)
                                                        {
                                                            this.MoreInfo = TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                                            return false;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (VSSection section in this._tmEvent.TicketTypes[0].PriceLevels[0].Section)
                                                    {
                                                        if (!ifSectionStringMatch && (parameter.ExactMatch ? section.SecName.ToLower() == parameter.Section.ToLower() : section.SecName.ToLower().Contains(parameter.Section.ToLower())))
                                                        {
                                                            ifSectionStringMatch = true;
                                                            AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", this._tmEvent.TicketTypes[0].PriceLevels[0].ID);
                                                            AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);
                                                            break;
                                                        }
                                                    }
                                                    if (!ifSectionStringMatch)
                                                    {
                                                        this.MoreInfo = TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                                        return false;
                                                    }
                                                }
                                                //}
                                            }
                                            else if (!String.IsNullOrEmpty(parameter.PriceLevel) && String.IsNullOrEmpty(parameter.Section))
                                            {
                                                for (int j = 0; j < this._tmEvent.TicketTypes[0].PriceLevels.Count; j++)
                                                {
                                                    if (!ifPriceStringMatch && (parameter.ExactMatch ? this._tmEvent.TicketTypes[0].PriceLevels[j].Description.ToLower() == parameter.PriceLevel.ToLower() : this._tmEvent.TicketTypes[0].PriceLevels[j].Description.ToLower().Contains(parameter.PriceLevel.ToLower())))
                                                    {
                                                        AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", this._tmEvent.TicketTypes[0].PriceLevels[j].ID);
                                                        this._session.FormElements.Remove("m:c:btnChangeQty");
                                                        this._session.FormElements.Remove("m:c:btnReserve");
                                                        this._session.Post(_session.HTMLWeb.ResponseUri.AbsoluteUri);

                                                        if (parameter.PriceMax == null && parameter.PriceMin == null)
                                                        {
                                                            ifPriceStringMatch = true;
                                                            //AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", this._tmEvent.TicketTypes[0].PriceLevels[j].ID);
                                                            AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", this._tmEvent.TicketTypes[0].PriceLevels[j].Section[0].ID);
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            //TODO: match price with max nd min quantity
                                                            VSPriceLevel value = getPriceLevel(parameter, this._tmEvent.TicketTypes[0]);
                                                            if (value != null)
                                                            {
                                                                if (value == this._tmEvent.TicketTypes[0].PriceLevels[j])
                                                                {
                                                                    ifPriceStringMatch = true;
                                                                    //AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", value.ID);
                                                                    AddUpdateField(this._session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    //TODO: If no price match



                                                                    //this.MoreInfo = "Price range does not match with the PriceLevel.";
                                                                    //return false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //TODO: If no price match
                                                                //this.MoreInfo = "Price range does not match.";
                                                                //return false;
                                                            }
                                                        }

                                                    }
                                                }
                                                if (!ifPriceStringMatch)
                                                {
                                                    this.MoreInfo = TicketSearchStatus.MoreInfoPriceLevelNotMatch;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                this._session.FormElements.Remove("m:c:btnChangeQty");
                                                //strHTML = this._session.Post(this._session.HTMLWeb.ResponseUri.AbsoluteUri);
                                                return true;
                                            }
                                        }
                                        else    //TODO: if no pricelevel and section are provided
                                        {

                                        }
                                    }
                                }
                                else// Show more info that quantity does not match.
                                {
                                    this.MoreInfo = TicketSearchStatus.MoreInfoQuantityNotMatch + " " + parameter.Quantity + " is not valid quantity for this parameter.";
                                    return false;
                                }
                            }
                            else// Show more info that quantity does not match.
                            {
                                this.MoreInfo = TicketSearchStatus.MoreInfoQuantityNotMatch + " Allowed quantity is " + (this._tmEvent.TicketTypes[0].MinQuantity) + " to " + this._tmEvent.TicketTypes[0].MaxQuantity;
                                return false;
                            }
                        }

                        // Put zero "0" in other price types or search strings for making the post request.
                        for (int i = 1; i < this._tmEvent.TicketTypes.Count; i++)
                        {
                            //this._session.FormElements.Add("w_ext_qty." + this._tmEvent.TicketTypes[i].V, "0");
                            //this._session.FormElements.Add("w_vv_ext_price." + this._tmEvent.TicketTypes[i].V, "");
                        }
                    }
                }
                else // If Price levels does not exist in the event first page then show message.
                {
                    this.MoreInfo = TicketSearchStatus.MoreInfoEventNotAvaiable;
                    return false;
                }

                //// If the Sections exists in the first page then proceed further.
                //if (this._tmEvent.HasSections)
                //{
                //    // If user provides the Section String then find and select the matching section otherwise show message and keep searching.
                //    if (!String.IsNullOrEmpty(parameter.Section))
                //    {
                //        Boolean ifSectionStringMatch = false;

                //        // Find and get section and map it to the post request.
                //        foreach (VSSection section in this._tmEvent.Sections)
                //        {
                //            // If user defined section match on keyword or exactly then select it and map it to the post request.
                //            //if (section.Description.ToLower().Contains(parameter.Section.ToLower()) || section.Description.ToLower() == parameter.Section.ToLower())
                //            //{
                //            //    this._session.FormElements.Add("w_vv_loc_2." + this._tmEvent.V_For_SecLocAdd, section.V);
                //            //    ifSectionStringMatch = true;
                //            //    break;
                //            //}
                //        }

                //        // If Sections String does not match then show message.
                //        if (!ifSectionStringMatch)
                //        {
                //            this.MoreInfo = "\"" + parameter.Section + "\" " + TicketSearchStatus.MoreInfoSectionStringNotMatch;
                //            return false;
                //        }
                //    }
                //    else // If user does not provide the Section String then make empty section automatically.
                //    {
                //        this._session.FormElements.Add("w_vv_loc_2." + this._tmEvent.V_For_SecLocAdd, "");
                //    }
                //}

                //// If the Locations exists in the first page then proceed further.
                //if (this._tmEvent.HasLocations)
                //{
                //    // If user provides the Locations String then find and select the matching location otherwise show message and keep searching.
                //    if (!String.IsNullOrEmpty(parameter.PriceLevel))
                //    {
                //        Boolean ifLocationStringMatch = false;

                //        // Find and get Location and map it to the post request.
                //        foreach (VSSection location in this._tmEvent.Locations)
                //        {
                //            // If user defined Location match on keyword or exactly then select it and map it to the post request.
                //            //if (location.Description.ToLower().Contains(parameter.Location.ToLower()) || location.Description.ToLower() == parameter.Location.ToLower())
                //            //{
                //            //    this._session.FormElements.Add("w_vv_loc_3." + this._tmEvent.V_For_SecLocAdd, location.V);
                //            //    ifLocationStringMatch = true;
                //            //    break;
                //            //}
                //        }

                //        // If Location String does not match then show message.
                //        if (!ifLocationStringMatch)
                //        {
                //            this.MoreInfo = "\"" + parameter.PriceLevel + "\" " + TicketSearchStatus.MoreInfoLocationStringNotMatch;
                //            return false;
                //        }
                //    }
                //    else // If user does not provide the Location String then make empty Location automatically.
                //    {
                //        this._session.FormElements.Add("w_vv_loc_3." + this._tmEvent.V_For_SecLocAdd, "");
                //    }
                //}

                //Mark parameter is available.
                parameter.IfAvailable = true;

                // If all parameters get fulfil then make result = true.
                result = true;
            }
            return result;
        }

        void resetAfterStopHandler()
        {
            if (this.Ticket.isRunning)
            {
                this.start();
            }

            //this._ifRestarting = false;
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
            }
            catch (Exception)
            {

            }

            if (this.resetAfterStop != null)
            {
                this.resetAfterStop();
            }
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

        #endregion

        #region Other Supporting Methods

        public TokenBucket getRecapToken(string region)
        {
            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 8016))
                {
                    NetworkStream stream = client.GetStream();
                    TCPEncryptor encryptor = new TCPEncryptor();
                    TokenRequestMessage msg = new TokenRequestMessage();
                    msg.Command = "getrecapTokens";
                    msg.Type = region;
                    byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(msg)) + "<EOF>");
                    stream.Write(buffer, 0, buffer.Length);
                    string message = Msg.ReadMessage(stream);
                    message = TCPEncryptor.Decrypt(message);
                    TokenBucket Tokens = JsonConvert.DeserializeObject<TokenBucket>(message);
                    return Tokens;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        private String getMD5Hash(String data)
        {
            string md5 = String.Empty;

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            MD5CryptoServiceProvider md5string = new MD5CryptoServiceProvider();
            byte[] md5hash = md5string.ComputeHash(bytes);

            foreach (byte x in md5hash)
            {
                md5 += String.Format("{0:x2}", x);
            }

            return md5;
        }

        private String getSHA256Hash(String data)
        {
            string hashString = string.Empty;

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }

            return hashString;
        }

        #endregion

        #region Constructor

        public VSSearch()
        {
            this.FlagImage = global::Automatick.Properties.Resources.Flag16Disable;
            this.FlagImage.Tag = false;
        }

        public VSSearch(VSTicket ticket)
        {
            this.Ticket = ticket;
            this.FlagImage = global::Automatick.Properties.Resources.Flag16Disable;
            this.FlagImage.Tag = false;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                if (_md5 != null)
                {
                    _md5.Clear();
                    GC.SuppressFinalize(_md5);
                    //_md5 = null;
                }
                if (this._session != null)
                {
                    this._session.Dispose();
                    GC.SuppressFinalize(this._session);
                }

                if (this._tmEvent != null)
                {
                    //this._tmEvent.Dispose();
                    this._tmEvent = null;
                }

                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {

            }
        }

        #endregion
    }
}
