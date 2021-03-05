using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SortedBindingList;
using System.ComponentModel;
using System.Web;
using HtmlAgilityPack;
using System.Threading;
using System.Data.SqlClient;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using TCPClient;
using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Automatick.Core
{
    [Serializable]
    public class TicketsLog
    {

        public const String NotBuyStatus = "Not bought";
        public const String AutoBuyStatus = "Automatically bought";
        public const String ManualBuyStatus = "Manually bought";
        String _TicketName;
        String _Section;
        String _Row;
        String _Seat;
        String _Price;
        String _Account;
        DateTime _FoundDateTime;
        String _MoreInfo;
        String _BuyStatus;

        public String MoreInfo
        {
            get { return _MoreInfo; }
            set { _MoreInfo = value; }
        }

        public String TicketName
        {
            get { return _TicketName; }
            set { _TicketName = value; }
        }
        public String Section
        {
            get { return _Section; }
            set { _Section = value; }
        }
        public String Row
        {
            get { return _Row; }
            set { _Row = value; }
        }
        public String Seat
        {
            get { return _Seat; }
            set { _Seat = value; }
        }
        public String Price
        {
            get { return _Price; }
            set { _Price = value; }
        }

        public String Account
        {
            get { return _Account; }
            set { _Account = value; }
        }

        public DateTime FoundDateTime
        {
            get { return _FoundDateTime; }
            set { _FoundDateTime = value; }
        }

        public String BuyStatus
        {
            get { return _BuyStatus; }
            set { _BuyStatus = value; }
        }

        public TicketsLog()
        {         
            _BuyStatus = NotBuyStatus;
        }
    }

    [Serializable]
    public class AXSTicket : ITicket
    {
        internal int CurrentParameterIndex = 0;
        internal int CurrentAvailableParameterIndex = 0;
        internal int CurrentFoundParameterIndex = 0;
        internal Boolean IfSelectDeliveryWindowOpen = false;
        private int _currentSelectedAccountIndex = 0;
        private int _currentAccountIndex = 0;
        private int usingGetNextLotIdMethod = 0;
        private int currentLotIDIndex = 0;
        private String ipAddress = "axs1.ticketpeers.com";
        private int port = 44500;
        public bool ifCapsium { get; set; }

        public String constTicketURL { get; set; }
        System.Media.SoundPlayer _soundAlert = null;
        SortableBindingList<AXSTicketAccount> _allTMAccounts = null;
        public Boolean DoneSelection
        {
            get;
            set;
        }

        public SortableBindingList<AXSTicketAccount> AllTMAccounts
        {
            get { return _allTMAccounts; }
        }

        public Boolean isPasswordHandled
        {
            get;
            set;
        }

        public Boolean isWrUpdated
        {
            get;
            set;
        }

        public SortableBindingList<TicketsLog> tic_Logs
        {
            get;
            set;
        }

        public AXSTicket()
        {
            this.WR = String.Empty;
            this.EventID = String.Empty;
            this.DoneSelection = false;
            this.autobuytry2 = false;
            this.autobuytry3 = false;
            this._TicketID = UniqueKey.getUniqueKey();
            this.ifCapsium = true;
            this.NoOfSearches = 1;
            this.ifAutoBuy = false;
            this.ifAutoBuyWitoutProxy = false;
            this.ifDistributeInSearches = true;
            this.ifPlaySoundAlert = true;
            this.ifRandomDelay = false;
            this.ifSchedule = false;
            this.ifSelectDeliveryOptionAutoBuying = true;
            this.ifSelectDeliveryOptionList = false;
            this.ifSelectAccountAutoBuying = true;
            this.ifSelectAccountList = false;
            this.ifSendEmail = false;
            this.ifUseFoundOnFirstAttempt = false;
            this.ifUseAvailableParameters = false;
            this.ifUseProxies = true;
            this.ResetSearchDelay = 15;
            this.Parameters = new SortableBindingList<AXSParameter>();
            AXSParameter parameter = new AXSParameter();
            this.Parameters.Add(parameter);
            this.ScheduleDateTime = DateTime.Now.AddHours(1);
            this.ScheduleRunningTime = 15;
            this.SelectedDeliveryOptions = new SortableBindingList<AXSDeliveryOption>();
            this.StartUsingProxiesFrom = 1;
            this.StartSolvingFromCaptcha = 1;
            this.DelayForAutoBuy = 30;
            this.TicketFoundCriterions = new SortableBindingList<AXSFoundCriteria>();
            this.TicketGroup = null;
            this.SelectedAccounts = new SortableBindingList<AXSTicketAccount>();
            this.TicketAccountsInTransition = new SortableBindingList<String>();
            this.AgencyName = "";
            this.AgencyUrlCode = "";
            this.ifRun = true;
            this.ifContinousRun = false;
            this.ifChangeCountOf2 = true;
            this.countOf2 = 0;
            this.ifOpen = false;
            VerifiedLotID = new List<string>();
            this.isWaitingEvent = true;

            this.ifUseProxiesInCaptchaSource = true;
            this.ifAutoCaptcha = true;
            this.ifBPCAutoCaptcha = false;
            this.ifCPTAutoCaptcha = false;
            this.ifDBCAutoCaptcha = false;
            this.ifRDAutoCaptcha = false;
            this.ifAntigateAutoCaptcha = false;
            this.ifDCAutoCaptcha = false;
            this.ifCAutoCaptcha = false;
            this.ifOCR = false;
            this.ifBoloOCR = false;
            this.ifSOCR = false;
            this.if2C = false;
            this.ifAC1AutoCaptcha = true;
            this.ifRDCAutoCaptcha = false;
            this.isWeb = true;
            this.isMobile = false;
            this.isEventko = false;
            this.isWrUpdated = false;
            this.isPasswordHandled = false;
        }
        #region ITicket Members
        private String _TicketID = "";

        public String WR
        {
            get;
            set;
        }

        public String EventID
        {
            get;
            set;
        }
        public Boolean isServiceSelected
        {
            get;
            set;
        }
        public ChangeDelegate onStartSearching
        {
            get;
            set;
        }
        public ChangeDelegate onFound
        {
            get;
            set;
        }
        public ChangeDelegate onNotFound
        {
            get;
            set;
        }
        public ChangeDelegate onChangeStartOrStop
        {
            get;
            set;
        }

        public ChangeDelegateForGauge onChangeForGauge
        {
            get;
            set;
        }

        public bool autobuytry2
        {
            get;
            set;
        }
        public bool autobuytry3
        {
            get;
            set;
        }
        public String TicketID
        {
            get { return this._TicketID; }
            set { this._TicketID = value; }
        }

        public String TicketName
        {
            get;
            set;
        }

        public String URL
        {
            get;
            set;
        }

        public String OldURL
        {
            get;
            set;
        }
        public String QueueURL
        {
            get;
            set;
        }

        public String FileLocation
        {
            get;
            set;
        }

        public String DeliveryOption
        {
            get;
            set;
        }

        public String DeliveryCountry
        {
            get;
            set;
        }

        public String Notes
        {
            get;
            set;
        }

        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public String TicketStatus
        {
            get;
            set;
        }
        public decimal NoOfSearches
        {
            get;
            set;
        }

        public decimal ResetSearchDelay
        {
            get;
            set;
        }

        public decimal StartUsingProxiesFrom
        {
            get;
            set;
        }

        public decimal DelayForAutoBuy
        {
            get;
            set;
        }
        public decimal StartSolvingFromCaptcha
        {
            get;
            set;
        }

        public decimal ScheduleRunningTime
        {
            get;
            set;
        }

        public Boolean ifOpen
        {
            get;
            set;
        }

        public Boolean IsUkEvent
        {
            get;
            set;
        }

        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifRun
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifContinousRun
        {
            get;
            set;
        }

        public Boolean ifChangeCountOf2
        {
            get;
            set;
        }

        public Boolean ifPesistSessionInEachSearch
        {
            get;
            set;

        }

        public Boolean isRunning
        {
            get;
            set;
        }

        public Boolean ifDBCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifDCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifOCR
        {
            get;
            set;
        }

        public Boolean ifROCR
        {
            get;
            set;
        }

        public Boolean ifBoloOCR
        {
            get;
            set;
        }

        public Boolean ifSOCR
        {
            get;
            set;
        }

        public Boolean if2C
        {
            get;
            set;
        }

        public Boolean ifCaptchator
        {
            get;
            set;
        }

        public Boolean ifAC1AutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifBPCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifRDAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifRDCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifAntigateAutoCaptcha
        {
            get;
            set;
        }
        public Boolean ifCPTAutoCaptcha
        {
            get;
            set;
        }     
        

        public Boolean ifDistributeInSearches
        {
            get;
            set;
        }

        public Boolean ifUseAvailableParameters
        {
            get;
            set;
        }

        public Boolean ifUseFoundOnFirstAttempt
        {
            get;
            set;
        }

        public Boolean ifPlaySoundAlert
        {
            get;
            set;
        }

        public Boolean ifAutoBuy
        {
            get;
            set;
        }

        public Boolean ifSendEmail
        {
            get;
            set;
        }

        public Boolean ifAutoBuyWitoutProxy
        {
            get;
            set;
        }

        public Boolean ifRandomDelay
        {
            get;
            set;
        }

        public Boolean ifUseProxies
        {
            get;
            set;
        }

        public Boolean ifUseProxiesInCaptchaSource
        {
            get;
            set;
        }

        public Boolean ifSelectDeliveryOptionAutoBuying
        {
            get;
            set;
        }

        public Boolean ifSelectDeliveryOptionList
        {
            get;
            set;
        }

        public Boolean ifSelectAccountAutoBuying
        {
            get;
            set;
        }

        public Boolean ifSelectAccountList
        {
            get;
            set;
        }

        public Boolean ifSchedule
        {
            get;
            set;
        }

        public Boolean isWaitingEvent
        {
            get;
            set;
        }

        public Boolean isWeb
        {
            get;
            set;
        }

        public Boolean isMobile
        {
            get;
            set;
        }

        public Boolean isEventko
        {
            get;
            set;
        }

        public int RunCount
        {
            get;
            set;
        }

        public int FoundCount
        {
            get;
            set;
        }

        public int countOf2
        {
            get;
            set;
        }

        public int BuyCount
        {
            get;
            set;
        }

        public int SoldoutCount
        {
            get;
            set;
        }

        public DateTime ScheduleDateTime
        {
            get;
            set;
        }

        public DateTime LastUsedDateTime
        {
            get;
            set;
        }

        public ISearchForm TicketSearchWindow
        {
            get;
            set;
        }

        public TicketGroup TicketGroup
        {
            get;
            set;
        }

        public SortableBindingList<ITicketSearch> Searches
        {
            get;
            set;
        }

        public BindingList<ITicketSearch> CaptchaQueue
        {
            get;
            set;
        }

        public BindingList<ITicketSearch> CaptchaBrowserQueue
        {
            get;
            set;
        }

        public BindingList<ITicketSearch> ParameterQueue
        {
            get;
            set;
        }

        public AutoCaptchaServices AutoCaptchaServices
        {
            get;
            set;
        }

        public ISelectDateForm SelectDateForm
        {
            get;
            set;
        }

        public SortableBindingList<AXSParameter> Parameters
        {
            get;
            set;
        }

        public SortableBindingList<AXSDeliveryOption> SelectedDeliveryOptions
        {
            get;
            set;
        }

        public SortableBindingList<AXSFoundCriteria> TicketFoundCriterions
        {
            get;
            set;
        }

        public SortableBindingList<AXSTicketAccount> SelectedAccounts
        {
            get;
            set;
        }

        public SortableBindingList<String> TicketAccountsInTransition
        {
            get;
            set;
        }

        public Dictionary<String, int> BuyHistory
        {
            get;
            set;
        }

        public System.Media.SoundPlayer SoundAlert
        {
            get { return _soundAlert; }
        }

        public List<String> VerifiedLotID
        {
            get;
            set;
        }

        public EmailSetting Email
        {
            get;
            set;
        }

        public GlobalSettings GlobalSetting
        {
            get;
            set;
        }
        public string AgencyName
        {
            get;
            set;
        }

        public string AgencyUrlCode
        {
            get;
            set;
        }

        public string lang
        {
            get;
            set;
        }
        public bool Permission
        {
            get;
            set;
        }
        public String SelectedDate
        {
            get;
            set;
        }
        public String SelectedEventTime
        {
            get;
            set;
        }
        public String MakeValidFileName()
        {
            String invalidChars = Regex.Escape(new String(Path.GetInvalidFileNameChars()));
            String invalidReStr = String.Format(@"[{0}]+", invalidChars);
            String file = Regex.Replace(TicketName, invalidReStr, "");
            if (file.Length > 250)
            {
                file = file.Substring(0, 250);
            }
            return file;
        }
        public String MakeValidFileName(String ticketname)
        {
            String invalidChars = Regex.Escape(new String(Path.GetInvalidFileNameChars()));
            String invalidReStr = String.Format(@"[{0}]+", invalidChars);
            String file = Regex.Replace(ticketname, invalidReStr, "");
            if (file.Length > 250)
            {
                file = file.Substring(0, 250);
            }
            return file;
        }

        private Boolean verifyTicketFile()
        {
            Boolean result = false;
            try
            {

                FileStream serializationStream = null;
                AXSTicket ticket = null;
                try
                {
                    String fileName = FileLocation + @"\Tickets\" + MakeValidFileName() + ".tevent";
                    if (File.Exists(fileName))
                    {
                        serializationStream = new FileStream(fileName, FileMode.Open);
                        BinaryFormatter formatter = new BinaryFormatter();
                        ticket = (AXSTicket)formatter.Deserialize(serializationStream);
                        ticket.FileLocation = this.FileLocation;
                    }
                    result = true;
                }
                catch (Exception)
                {
                    result = false;
                }
                finally
                {
                    if (serializationStream != null)
                    {
                        serializationStream.Close();
                        serializationStream.Dispose();
                    }

                    //GC.Collect();
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
        public bool SaveTicket()
        {
            try
            {
                int iretryAttempt = 0;
            Retry:
                SortableBindingList<ITicketSearch> tmpAllSearches = this.Searches;
                ChangeDelegate tmpChangeDelegate = this.onChangeStartOrStop;
                ChangeDelegate tmpstartSearchingDelegate = this.onStartSearching;
                ChangeDelegate tmpFoundDelegate = this.onFound;
                ChangeDelegate tmpNotFoundDelegate = this.onNotFound;
                ChangeDelegateForGauge tmpChangeDelegateForGauge = this.onChangeForGauge;
                System.Media.SoundPlayer tmpSoundAlert = this._soundAlert;
                EmailSetting tmpEmail = this.Email;
                GlobalSettings tmpGlobalSetting = this.GlobalSetting;

                ISearchForm tmpTicketSearchWindow = this.TicketSearchWindow;

                this.Searches = null;
                this.TicketSearchWindow = null;
                this.onChangeStartOrStop = null;
                this.onChangeForGauge = null;
                this.onFound = null;
                this.onNotFound = null;
                this._soundAlert = null;
                this.Email = null;
                this.GlobalSetting = null;
                this.onStartSearching = null;
                this.SelectDateForm = null;

                if (String.IsNullOrEmpty(this._TicketID))
                {
                    this._TicketID = UniqueKey.getUniqueKey();
                }

                this.LastUsedDateTime = DateTime.Now;

                this.saveLog();

                if (!Directory.Exists(FileLocation + @"\Tickets\"))
                {
                    Directory.CreateDirectory(FileLocation + @"\Tickets\");
                }
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream serializationStream = new FileStream(FileLocation + @"\Tickets\" + MakeValidFileName() + ".tevent", FileMode.Create);
                    formatter.Serialize(serializationStream, this);
                    serializationStream.Close();
                    serializationStream.Dispose();
                    //GC.Collect();
                }
                catch (IOException ioex)
                {
                    if (iretryAttempt < 5)
                    {
                        iretryAttempt++;
                        Thread.Sleep(10);
                        goto Retry;
                    }
                }
                catch
                {
                    if (iretryAttempt < 5)
                    {
                        iretryAttempt++;
                        Thread.Sleep(10);
                        goto Retry;
                    }
                }

                if (!verifyTicketFile())
                {
                    if (iretryAttempt < 10)
                    {
                        iretryAttempt++;
                        Thread.Sleep(10);
                        goto Retry;
                    }
                }

                this.Searches = tmpAllSearches;
                this.TicketSearchWindow = tmpTicketSearchWindow;
                this.onChangeStartOrStop = tmpChangeDelegate;
                this.onChangeForGauge = tmpChangeDelegateForGauge;
                this.onFound = tmpFoundDelegate;
                this.onNotFound = tmpNotFoundDelegate;
                this._soundAlert = tmpSoundAlert;
                this.Email = tmpEmail;
                this.GlobalSetting = tmpGlobalSetting;
                this.onStartSearching = tmpstartSearchingDelegate;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteTicket()
        {
            try
            {
                if (File.Exists(FileLocation + @"\Tickets\" + MakeValidFileName(TicketName) + ".tevent"))
                {
                    File.Delete(FileLocation + @"\Tickets\" + MakeValidFileName(TicketName) + ".tevent");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        String XmlUrl = string.Empty;
        Boolean ifHashGenerated = false;

        public void sendURLToBrowser(AXSTicket Ticket, LicenseCore lic, String Location)
        {
            try
            {
                AXSParameter parameter = null;
                if (this.isWaitingEvent) //&& !this.URL.Contains("/shop/"))
                {
                    if (!processWaitingRoom())
                    {
                        if (!ifHashGenerated && !this.isPasswordHandled)
                        {
                            parameter = this.Parameters.FirstOrDefault(pred => pred.TicketTypePasssword != null && !pred.TicketTypePasssword.Equals(String.Empty));

                            if (parameter != null)
                            {
                                if (!String.IsNullOrEmpty(parameter.TicketTypePasssword))
                                {
                                    #region Password Event handling
                                    try
                                    {
                                        String wr = HttpUtility.ParseQueryString((new Uri(this.URL)).Query).Get("wr");
                                        if (this.URL.Contains("wroom.centrebell.ca"))
                                        {
                                            this.lang = HttpUtility.ParseQueryString((new Uri(this.URL)).Query).Get("lang");
                                        }

                                        if (string.IsNullOrEmpty(wr))
                                        {
                                            if (URL.Contains("/shop/") || URL.Contains("/#/"))
                                            {
                                                String w = URL.Substring(URL.IndexOf("#") + 2);
                                                string[] split = w.Split('/');
                                                if (split[0].Contains("?"))
                                                {
                                                    split = split[0].Split('?');
                                                    wr = split[0];
                                                }
                                                else
                                                {
                                                    wr = split[0];
                                                }
                                            }
                                        }

                                        if (string.IsNullOrEmpty(wr))
                                        {
                                            try
                                            {
                                                string[] breakforWroom = URL.Split('?');
                                                string[] split = breakforWroom[1].Split('&');

                                                foreach (var item in split)
                                                {
                                                    if (item.Contains("wr="))
                                                    {
                                                        wr = item.Replace("wr=", "");
                                                        break;
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                            }
                                        }



                                        HtmlDocument doc = null;
                                        string strNew = MD5Hash(wr + parameter.TicketTypePasssword + "90f22b2e");
                                        //doc.LoadHtml(post(this.XmlUrl.Replace("/info/", "/bfox/") + "?methodName=session.get&serverStr=" + this.XmlUrl.Replace("/info/", "/bfox/"), String.Format("<methodCall><methodName>session.get</methodName><params><param><value><string>{0}</string></value></param><param><value><array><data><value><string>accessQty</string></value><value><string>searchRules</string></value><value><string>options</string></value></data></array></value></param></params></methodCall>", key)));

                                        String parentIssCode = String.Empty;
                                        if (!URL.ToLower().Contains("/shop/") && !!URL.ToLower().Contains("/#/"))
                                        {
                                            int counter = 0;

                                        Retry:
                                            doc = new HtmlDocument();
                                            doc.LoadHtml(post(Ticket.XmlUrl + "?methodName=showshop.jumpW&wroom=" + wr + "&lang=en&pc=" + parameter.TicketTypePasssword, String.Format("<methodCall><methodName>showshop.jumpW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>{1}</string></value></param><param><value><string>{2}</string></value></param><param><value><string>.promoGO:28:13:1</string></value></param></params></methodCall>", wr, parameter.TicketTypePasssword, strNew)));

                                            parentIssCode = doc.DocumentNode.SelectSingleNode("//methodresponse").InnerText.Trim();
                                            if (!String.IsNullOrEmpty(parentIssCode))
                                            {
                                                this.URL = this.URL.Replace(wr, parentIssCode);
                                                isWrUpdated = true;
                                                this.WR = parentIssCode;
                                                isPasswordHandled = true;
                                            }
                                            else
                                            {
                                                counter++;

                                                if (counter <= 10)
                                                {
                                                    goto Retry;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            String str = MD5Hash(wr + parameter.TicketTypePasssword + "90f22b2e");

                                            String resutl = post(Ticket.XmlUrl + "showshop.jumpW/" + wr + "/" + parameter.TicketTypePasssword + "/" + str + "?resp=json", null, "application/json");
                                            JObject obj = JObject.Parse(resutl);

                                            parentIssCode = obj["result"].ToString();

                                            if (!String.IsNullOrEmpty(parentIssCode))
                                            {
                                                this.URL = this.URL.Replace(wr, parentIssCode);
                                                this.WR = parentIssCode;
                                                isWrUpdated = true;
                                                isPasswordHandled = true;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex.Message);
                                    }
                                    #endregion
                                }
                            }
                        }
                    }

                    if (!ifHashGenerated)
                    {
                        using (TcpClient client = new TcpClient("mainserver.ticketpeers.com", port))
                        {
                            NetworkStream stream = client.GetStream();

                            ClientRequst clientReq = new ClientRequst();
                            clientReq.AppPrefix = lic.AppPreFix;
                            clientReq.URL = checkO2Link(Ticket.URL);
                            clientReq.LicenseID = ServerPortsPicker.ServerPortsPickerInstance.LicenseCode;

                            if (parameter != null)
                            {
                                clientReq.Password = parameter.TicketTypePasssword;
                            }

                            clientReq.WR = this.WR;// HttpUtility.ParseQueryString((new Uri(this.URL)).Query).Get("wr");
                            clientReq.EventID = this.EventID;// HttpUtility.ParseQueryString((new Uri(this.URL)).Query).Get("eventid");

                            byte[] buffer = Encoding.UTF8.GetBytes(TCPEncryptor.Encrypt(JsonConvert.SerializeObject(clientReq)) + "<EOF>");

                            stream.Write(buffer, 0, buffer.Length);
                            stream.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            this.FileLocation = Location;
            this.SaveTicket();
        }

        private Boolean processWaitingRoom()
        {
            bool ifValidated = false;
            string wr = string.Empty;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            try
            {
                proxy = ProxyPicker.ProxyPickerInstance.getNextProxy(null);

                //if (string.IsNullOrEmpty(this.wrNew))
                {
                    wr = HttpUtility.ParseQueryString((new Uri(URL)).Query).Get("wr");

                    if (string.IsNullOrEmpty(wr))
                    {
                        if (this.URL.Contains("/shop/") || this.URL.Contains("/#/"))
                        {
                            String w = URL.Substring(URL.IndexOf("#") + 2);
                            string[] split = w.Split('/');
                            if (split[0].Contains("?"))
                            {
                                split = split[0].Split('?');
                                wr = split[0];
                            }
                            else
                            {
                                wr = split[0];
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(wr))
                    {
                        String[] query = URL.Substring(URL.IndexOf('?') + 1).Split('&');

                        foreach (var item in query)
                        {
                            if (item.Split('=')[0].ToLower().Equals("wr"))
                            {
                                wr = item.Split('=')[1];
                                break;
                            }
                        }
                    }
                }

            Retry:
                if (!URL.Contains("/shop/") && !URL.Contains("/#/"))
                {
                    if (this.isWeb || this.isEventko)
                    {
                        doc.LoadHtml(post(String.Format("https://" + new Uri(this.URL).Host + "/xmlrpc?methodName=getPhase&lotID=noLotId_7&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>noLotId_7</string></value></param></params></methodCall>", wr), "text/xml"));
                    }
                    else
                    {
                        doc.LoadHtml(post(String.Format("https://" + new Uri(this.URL).Host + "/xmlrpc?methodName=getPhase&lotId=NoLotId&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>NoLotId</string></value></param></params></methodCall>", wr), "application/x-www-form-urlencoded; charset=UTF-8"));
                    }
                }
                else
                {
                    doc.LoadHtml(post(String.Format("https://" + new Uri(this.URL).Host + "/xmlrpc?methodName=getPhase&lotID=noLotId&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>noLotId</string></value></param></params></methodCall>", wr), "text/xml"));//"application/x-www-form-urlencoded"));
                }

                HtmlNode enterPauseNode = doc.DocumentNode.SelectSingleNode("//name[text() = 'enterPause']");

                HtmlNodeCollection nodehash = doc.DocumentNode.SelectNodes("//member/name");
                if (nodehash != null)
                {
                    foreach (HtmlNode item in nodehash)
                    {
                        if (item.InnerHtml.Contains("hashts"))
                        {
                            ifHashGenerated = true;
                        }
                        else if (item.InnerHtml.Contains("hash"))
                        {
                            ifHashGenerated = true;
                        }
                        if (item.InnerHtml.Contains("info_path"))
                        {
                            if (item.NextSibling.FirstChild != null)
                            {
                                XmlUrl = item.NextSibling.FirstChild.InnerText.Trim();
                            }
                            else
                            {
                                XmlUrl = item.NextSibling.NextSibling.FirstChild.InnerText.Trim();
                            }
                        }
                    }
                }

                if ((enterPauseNode != null))
                {
                    int enterPause = 0;

                    if (enterPauseNode.NextSibling.ChildNodes.Count > 0)
                    {
                        enterPause = Convert.ToInt32(enterPauseNode.NextSibling.ChildNodes[0].InnerHtml);
                    }
                    else
                    {
                        enterPause = Convert.ToInt32(enterPauseNode.NextSibling.NextSibling.ChildNodes[0].InnerHtml);
                    }
                    enterPause *= 1000;

                    ifValidated = true;
                }
                else
                {
                    ifValidated = false;
                }

                if (proxy != null)
                {
                    if (proxy.TheProxyType != Core.Proxy.ProxyType.Relay)
                    {
                        if (ProxyPicker.ProxyPickerInstance.ProxyManager != null)
                        {
                            ProxyPicker.ProxyPickerInstance.ProxyManager.ReleaseProxy(proxy);
                        }
                    }
                    else
                    {
                        if (ProxyPicker.ProxyPickerInstance.ProxyRelayManager != null)
                        {
                            #region Release Session from Server

                            ClearSessionFromServer();

                            #endregion

                            ProxyPicker.ProxyPickerInstance.ProxyRelayManager.ReleaseProxy(proxy);
                        }
                    }
                }
            }
            catch (Exception)
            {
                ifValidated = false;
            }

            return ifValidated;
        }

        void ClearSessionFromServer()
        {
            String result = String.Empty;

            try
            {
                HttpWebRequest webRequest = System.Net.HttpWebRequest.Create("http://trigger.pvms?cmd=clear&session=" + proxy.userName + "-context-" + "axsus" + "-session-" + proxy.LuminatiSessionId) as System.Net.HttpWebRequest;
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:59.0) Gecko/20100101 Firefox/59.0";
                webRequest.KeepAlive = true;
                webRequest.Accept = "*/*";

                if (proxy != null)
                {
                    if (proxy.TheProxyType != Proxy.ProxyType.Custom)
                    {
                        webRequest.Timeout = 10000;
                    }

                    webRequest.Proxy = proxy.toWebProxy("axsus");
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
                                Debug.WriteLine("Proxy released from server too -- " + proxy.LuminatiSessionId);
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

        public static String MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public string checkO2Link(String O2link)
        {
            string link = O2link;
            try
            {
                if (O2link.Contains("o2priority"))
                {
                    if (!O2link.Contains("/shop/") && !O2link.Contains("/#/"))
                    {
                        string remove = O2link.Substring(0, O2link.IndexOf('?'));
                        O2link = O2link.Replace(remove, "https://axsmobile.eventshopper.com/mobilewroom/");
                        link = O2link; 
                    }
                }
            }
            catch
            { }
            return link;
        }

        public bool setAgencyRequestUrl(AXSTicket Ticket)
        {
            bool ifValidated = false;
            string wr = string.Empty;
            Boolean ifJson = false;
            string jsonStr = string.Empty;

            return ifValidated;
        }

        public bool setAgencyRequestUrl(AXSTicket Ticket, string wr)
        {
            bool ifValidated = false;

            return ifValidated;
        }

        public String post(string url, string postdata)
        {
            string URL = url; string Result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.Expect100Continue = false;
            request.ServicePoint.Expect100Continue = false;
            request.KeepAlive = true;

            if (url.Contains("jumpW"))
            {
                if (!this.URL.Contains("/shop/") && !this.URL.Contains("/#/"))
                {
                    //webRequest.Headers.Add("X-Requested-With", "ShockwaveFlash/21.0.0.182");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
                    request.ContentType = "text/html";
                    request.Accept = "*/*";
                }
                else
                {
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
                    request.ContentType = "application/json";
                    request.Accept = "*/*";
                    request.Referer = "https://" + new Uri(this.URL).Host + "/shop/";
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    request.Headers.Add("Origin", "https://" + new Uri(this.URL).Host);
                }
            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            if (proxy != null)
            {
                if (proxy.TheProxyType == Proxy.ProxyType.Relay)
                {
                    string context = "";

                    if (new Uri(this.URL).Host.Contains("o2priority"))
                    {
                        context = "axsuk";
                    }
                    else
                    {
                        context = "axsus";
                    }
                    request.Proxy = proxy.toWebProxy(context);
                }
                else  request.Proxy = proxy.toWebProxy();
            }

            HttpWebResponse resp = null;
            Encoding respenc = null;
            var isGZipEncoding = false;
            Stream stream = null;

            if (!String.IsNullOrEmpty(postdata))
            {
                request.Method = "POST";
                string body = postdata;
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
            }
            else
            {
                request.Method = "GET";
            }

            try
            {
                resp = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException we)
            {
                resp = (HttpWebResponse)we.Response;
                //ProxyPicker.ProxyPickerInstance.RecheckProxyStatus(search.Proxy, we.Message);
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

                ////
                //Stream reqStream = webRequest.GetRequestStream();


                if (isGZipEncoding)
                {
                    stream = new GZipStream(resp.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    stream = resp.GetResponseStream();
                    //  xdoc.Load(resp.GetResponseStream());


                }


                //reqStream.Write(postArray, 0, postArray.Length);

                //StreamReader sr = new StreamReader(new GZipStream((webRequest.GetResponse().GetResponseStream()), CompressionMode.Compress));
                StreamReader sr = new StreamReader(stream);
                Result = sr.ReadToEnd();
            }
            stream.Close();
            return Result;
        }

        Proxy proxy = null;

        public String post(string url, string postdata, String contentType)
        {
            string URL = url; string Result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.Expect100Continue = false;
            request.ServicePoint.Expect100Continue = false;
            request.KeepAlive = true;

            if (url.Contains("jumpW"))
            {
                if (!this.URL.Contains("/shop/"))
                {
                    //webRequest.Headers.Add("X-Requested-With", "ShockwaveFlash/21.0.0.182");
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
                    request.ContentType = "text/html";
                    request.Accept = "*/*";
                }
                else
                {
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:48.0) Gecko/20100101 Firefox/48.0";
                    request.ContentType = "application/json";
                    request.Accept = "*/*";
                    request.Referer = "https://" + new Uri(this.URL).Host + "/shop/";
                    request.Headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
                    request.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    request.Headers.Add("Origin", "https://" + new Uri(this.URL).Host);
                }
            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            if (!String.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }

            if (proxy != null)
            {
                if (proxy.TheProxyType == Proxy.ProxyType.Relay)
                {
                    string context = "";
                    if (new Uri(this.URL).Host.Contains("o2priority"))
                    {
                        context = "axsuk";
                    }
                    else
                    {
                        context = "axsus";
                    }
                    request.Proxy = proxy.toWebProxy(context);
                }
                else
                {
                    request.Proxy = proxy.toWebProxy();
                }
            }

            HttpWebResponse resp = null;
            Encoding respenc = null;
            var isGZipEncoding = false;
            Stream stream = null;

            if (!String.IsNullOrEmpty(postdata))
            {
                request.Method = "POST";
                string body = postdata;
                byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = postBytes.Length;
                stream = request.GetRequestStream();
                stream.Write(postBytes, 0, postBytes.Length);
            }
            else
            {
                request.Method = "GET";
            }

            try
            {
                resp = request.GetResponse() as HttpWebResponse;
            }
            catch (WebException we)
            {
                resp = (HttpWebResponse)we.Response;
                //ProxyPicker.ProxyPickerInstance.RecheckProxyStatus(search.Proxy, we.Message);
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

                ////
                //Stream reqStream = webRequest.GetRequestStream();


                if (isGZipEncoding)
                {
                    stream = new GZipStream(resp.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    stream = resp.GetResponseStream();
                    //  xdoc.Load(resp.GetResponseStream());


                }


                //reqStream.Write(postArray, 0, postArray.Length);

                //StreamReader sr = new StreamReader(new GZipStream((webRequest.GetResponse().GetResponseStream()), CompressionMode.Compress));
                StreamReader sr = new StreamReader(stream);
                Result = sr.ReadToEnd();
            }
            stream.Close();
            return Result;
        }

        public void changeSettingsDuringSearching(decimal lastSearches)
        {
            if (this.NoOfSearches > lastSearches)
            {
                for (int i = 0; i < (this.NoOfSearches - lastSearches); i++)
                {
                    if (Searches != null)
                    {
                        AXSSearch search = new AXSSearch(this);

                        if (this.URL.Contains("evenko.ca"))
                        {
                            search.isEventko = true;
                        }
                        else if (this.isWeb && this.isMobile)
                        {
                            if (i % 2 == 0)
                            {
                                search.isWeb = true;
                                search.isMobile = false;
                            }
                            else
                            {
                                search.isWeb = false;
                                search.isMobile = true;
                            }
                        }
                        else
                        {
                            if (this.isWeb)
                            {
                                search.isWeb = true;
                                search.isMobile = false;
                            }
                            else if (this.isMobile)
                            {
                                search.isWeb = false;
                                search.isMobile = true;
                            }
                            else
                            {
                                search.isWeb = true;
                                search.isMobile = false;
                            }
                        }

                        //search.start();
                        this.Searches.Add(search);
                    }
                }
            }
            else if (this.NoOfSearches < lastSearches)
            {
                if (Searches != null)
                {
                    for (int i = ((int)lastSearches - 1); i >= this.NoOfSearches; i--)
                    {
                        ITicketSearch search = this.Searches[i];
                        search.stop();
                        this.Searches.Remove(search);
                        search.Dispose();
                    }
                }
            }

            try
            {
                for (int i = 0; i < this.Searches.Count; i++)
                {
                    ITicketSearch search = this.Searches[i];

                    search.IfUseAutoCaptcha = false;
                    if (i >= ((int)this.StartSolvingFromCaptcha - 1) && this.ifAutoCaptcha)
                    {
                        search.IfUseAutoCaptcha = true;
                    }

                    search.IfUseProxy = false;
                    if (i >= ((int)this.StartUsingProxiesFrom - 1) && this.ifUseProxies)
                    {
                        search.IfUseProxy = true;
                    }

                    if (!search.IfWorking)
                    {
                        search.start();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void start(System.Media.SoundPlayer soundAlert, SortableBindingList<AXSTicketAccount> allTMAccountsForSyncing, EmailSetting email, GlobalSettings globalSetting)
        {
            Regex.CacheSize = 0;
            this.isRunning = true;
            this._soundAlert = soundAlert;
            Searches = new SortableBindingList<ITicketSearch>();
            this.CurrentParameterIndex = 0;
            this.LastUsedDateTime = DateTime.Now;
            this.IfSelectDeliveryWindowOpen = false;
            this._allTMAccounts = allTMAccountsForSyncing;
            this.Email = email;
            this.syncSelectedAccounts();
            this.GlobalSetting = globalSetting;
            string FileName = FileLocation + @"\Tickets\" + MakeValidFileName() + "Log.tlog";

            this.tic_Logs = new SortableBindingList<TicketsLog>();
            if (File.Exists(FileName))
            {
                this.tic_Logs.Load(FileName);
            }

            if (this.TicketAccountsInTransition == null)
            {
                this.TicketAccountsInTransition = new SortableBindingList<String>();
            }


            SelectDateForm = new frmSelectDate();
            SelectDateForm.showForm();
            SelectDateForm.hideForm();
            this.ParameterQueue = SelectDateForm.ParameterQueue;


            if (this.TicketAccountsInTransition.Count > 0)
            {
                this.TicketAccountsInTransition.Clear();
            }
            for (int i = 0; i < this.Parameters.Count; i++)
            {
                if (this.Parameters[i].DateTimeString == "mm/dd/yyyy" || String.IsNullOrEmpty(this.Parameters[i].DateTimeString))
                {
                    this.DoneSelection = false;
                }
                else
                {
                    this.SelectedDate = this.Parameters[i].DateTimeString;
                    this.SelectedEventTime = this.Parameters[i].EventTime;
                }
                this.Parameters[i].IfAvailable = false;
                this.Parameters[i].IfFound = false;
            }

            for (int i = 0; i < this.NoOfSearches; i++)
            {
                AXSSearch search = new AXSSearch(this);


                search.IfUseProxy = false;
                if (i >= ((int)this.StartUsingProxiesFrom - 1) && this.ifUseProxies)
                {
                    search.IfUseProxy = true;
                }

                search.start();
                this.Searches.Add(search);
            }

            if (this.onChangeStartOrStop != null)
            {
                this.onChangeStartOrStop(this);
            }
        }

        public void start(BindingList<ITicketSearch> captchaQueue, BindingList<ITicketSearch> captchaBrowserQueue, System.Media.SoundPlayer soundAlert, AutoCaptchaServices autoCaptchaServices, SortableBindingList<AXSTicketAccount> allTMAccountsForSyncing, EmailSetting email, GlobalSettings globalSetting)
        {
            Regex.CacheSize = 0;
            this.isRunning = true;
            this._soundAlert = soundAlert;
            Searches = new SortableBindingList<ITicketSearch>();
            this.CurrentParameterIndex = 0;
            this.LastUsedDateTime = DateTime.Now;
            this.IfSelectDeliveryWindowOpen = false;
            this._allTMAccounts = allTMAccountsForSyncing;
            this.Email = email;
            this.syncSelectedAccounts();
            this.GlobalSetting = globalSetting;
            this.AutoCaptchaServices = autoCaptchaServices;

            this.CaptchaQueue = captchaQueue;
            this.CaptchaBrowserQueue = captchaBrowserQueue;
            string FileName = FileLocation + @"\Tickets\" + MakeValidFileName() + "Log.tlog";

            this.tic_Logs = new SortableBindingList<TicketsLog>();
            if (File.Exists(FileName))
            {
                this.tic_Logs.Load(FileName);
            }

            if (this.TicketAccountsInTransition == null)
            {
                this.TicketAccountsInTransition = new SortableBindingList<String>();
            }


            SelectDateForm = new frmSelectDate();
            SelectDateForm.showForm();
            SelectDateForm.hideForm();
            this.ParameterQueue = SelectDateForm.ParameterQueue;

            if (VerifiedLotID == null)
            {
                VerifiedLotID = new List<string>();
            }
            VerifiedLotID = LotIDPicker.LotIDInstance.getEventsLots(WR,EventID);

            if (this.TicketAccountsInTransition.Count > 0)
            {
                this.TicketAccountsInTransition.Clear();
            }
            for (int i = 0; i < this.Parameters.Count; i++)
            {
                if (this.Parameters[i].DateTimeString == "mm/dd/yyyy" || String.IsNullOrEmpty(this.Parameters[i].DateTimeString))
                {
                    this.DoneSelection = false;
                }
                else
                {
                    this.SelectedDate = this.Parameters[i].DateTimeString;
                    this.SelectedEventTime = this.Parameters[i].EventTime;
                }
                this.Parameters[i].IfAvailable = false;
                this.Parameters[i].IfFound = false;
            }

            if (!globalSetting.ifMobile)
            {
                this.isMobile = false;
            }

            if (!globalSetting.IfWeb)
            {
                this.isWeb = false;
            }

            if (!globalSetting.ifEventko)
            {
                this.isEventko = false;
            }

            for (int i = 0; i < this.NoOfSearches; i++)
            {
                AXSSearch search = new AXSSearch(this);

                if (this.URL.Contains("evenko.ca"))
                {
                    search.isEventko = true;
                }
                else if (this.isWeb && this.isMobile)
                {
                    if (i % 2 == 0)
                    {
                        search.isWeb = true;
                        search.isMobile = false;
                    }
                    else
                    {
                        search.isWeb = false;
                        search.isMobile = true;
                    }
                }
                else
                {
                    if (this.isWeb)
                    {
                        search.isWeb = true;
                        search.isMobile = false;
                    }
                    else if (this.isMobile)
                    {
                        search.isWeb = false;
                        search.isMobile = true;
                    }
                    else
                    {
                        search.isWeb = true;
                        search.isMobile = false;
                    }
                }

                search.IfUseProxy = false;
                if (i >= ((int)this.StartUsingProxiesFrom - 1) && this.ifUseProxies)
                {
                    search.IfUseProxy = true;
                }

                search.IfUseAutoCaptcha = false;
                if (i >= ((int)this.StartSolvingFromCaptcha - 1) && this.ifAutoCaptcha)
                {
                    search.IfUseAutoCaptcha = true;
                }

                search.start();
                this.Searches.Add(search);
            }

            if (this.onChangeStartOrStop != null)
            {
                this.onChangeStartOrStop(this);
            }
        }

        private void stopThreadHandler(object o)
        {
            try
            {
                if (o != null)
                {
                    SortableBindingList<ITicketSearch> tmpSearches = (SortableBindingList<ITicketSearch>)o;
                    ITicketSearch[] tmpArrayToStop = new ITicketSearch[tmpSearches.Count];
                    tmpSearches.CopyTo(tmpArrayToStop, 0);
                    tmpSearches.Clear();

                    //for (int i = 0; i < tmpArrayToStop.Length; i++)
                    //{
                    //    try
                    //    {
                    //        ((AXSSearch)tmpArrayToStop[i]).IfWorking = false;
                    //        ((AXSSearch)tmpArrayToStop[i]).captchaload.Set();
                    //    }
                    //    catch { }
                    //}

                    for (int i = 0; i < tmpArrayToStop.Length; i++)
                    {
                        try
                        {
                            ((AXSSearch)tmpArrayToStop[i]).IfWorking = false;
                            //((AXSSearch)tmpArrayToStop[i]).captchaload.Set();
                            tmpArrayToStop[i].stop();
                            //tmpArrayToStop[i].Dispose();
                        }
                        catch { }
                    }
                    try
                    {
                        if (this.ParameterQueue != null)
                        {
                            this.ParameterQueue.Clear();
                        }
                    }
                    catch { }
                    tmpArrayToStop = null;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                try
                {
                    this.SaveTicket();
                    GC.Collect();
                }
                catch (Exception)
                {

                }
            }
        }

        public void stop()
        {
            try
            {
                this.isRunning = false;

                if (this.TicketSearchWindow != null)
                {
                    this.TicketSearchWindow.closeForm();
                }
                if (this.onChangeStartOrStop != null)
                {
                    this.onChangeStartOrStop(this);
                    this.onChangeStartOrStop = null;
                }

                if (this.onChangeForGauge != null)
                {
                    this.onChangeForGauge();
                    this.onChangeForGauge = null;
                }

                this.SelectedDate = string.Empty;
                this.SelectedEventTime = string.Empty;

                Thread thStop = new Thread(new ParameterizedThreadStart(this.stopThreadHandler));
                thStop.Priority = ThreadPriority.Highest;
                thStop.SetApartmentState(ApartmentState.STA);
                thStop.IsBackground = true;
                thStop.Start(this.Searches);

                //this.stopThreadHandler();
                // this.SaveTicket();
            }
            catch { }
        }

        private void syncSelectedAccounts()
        {
            if (_allTMAccounts != null && this.SelectedAccounts != null)
            {
                if (_allTMAccounts.Count > 0)
                {
                    for (int i = this.SelectedAccounts.Count - 1; i >= 0; i--)
                    {
                        AXSTicketAccount syncAccount = _allTMAccounts.FirstOrDefault(p => p.EmailAddress == this.SelectedAccounts[i].EmailAddress);
                        if (syncAccount != null)
                        {
                            syncAccount.IfSelected = this.SelectedAccounts[i].IfSelected;
                            syncAccount.BuyingLimit = this.SelectedAccounts[i].BuyingLimit;
                            this.SelectedAccounts[i] = syncAccount;
                        }
                        else
                        {
                            this.SelectedAccounts.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    this.SelectedAccounts = new SortableBindingList<AXSTicketAccount>();
                }
            }
            else
            {
                this.SelectedAccounts = new SortableBindingList<AXSTicketAccount>();
            }

            this.SaveTicket();
        }

        public AXSTicketAccount getNextAccount(Boolean isguest = false)
        {
            AXSTicketAccount account = null;

            if (this.ifSelectAccountList)
            {
                if (this.SelectedAccounts != null)
                {
                    if (this.SelectedAccounts.Count > 0)
                    {
                        int loginAcc = 0;

                        if (!isguest)
                        {
                            loginAcc = this.SelectedAccounts.Count(pred => pred.GroupName.Equals("login"));
                        }

                        for (int i = 0; i < this.SelectedAccounts.Count; i++)
                        {
                            if (this._currentSelectedAccountIndex >= this.SelectedAccounts.Count)
                            {
                                this._currentSelectedAccountIndex = 0;
                            }

                            account = this.SelectedAccounts[_currentSelectedAccountIndex];

                            _currentSelectedAccountIndex++;

                            if (account == null)
                            {
                                continue;
                            }
                            else
                            {
                                if (!isguest && loginAcc == 0 ? true : (isguest == account.GroupName.Equals("guest")))
                                {
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (this._allTMAccounts != null)
                {
                    if (this._allTMAccounts.Count > 0)
                    {
                        int loginAcc = 0;

                        if (!isguest)
                        {
                            loginAcc = this._allTMAccounts.Count(pred => pred.GroupName.Equals("login"));
                        }

                        for (int i = 0; i < this._allTMAccounts.Count; i++)
                        {
                            if (this._currentAccountIndex >= this._allTMAccounts.Count)
                            {
                                this._currentAccountIndex = 0;
                            }

                            account = this._allTMAccounts[_currentAccountIndex];

                            _currentAccountIndex++;

                            if (account == null)
                            {
                                continue;
                            }
                            else
                            {
                                if (!isguest && loginAcc == 0 ? true : (isguest == account.GroupName.Equals("guest")))
                                {
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }

            return account;
        }

        public String GetNextLotID()
        {
            string lotId = string.Empty;

            try
            {
                if (this.VerifiedLotID != null && this.VerifiedLotID.Count > 0)
                {
                    lock (this.VerifiedLotID)
                    {
                        if (this.currentLotIDIndex >= this.VerifiedLotID.Count)
                        {
                            this.currentLotIDIndex = 0;
                        }

                        lotId = this.VerifiedLotID[currentLotIDIndex];
                        this.currentLotIDIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
            }


            return lotId;
        }

        public void SaveLotId(string lotId)
        {
            try
            {
                if (0 == Interlocked.Exchange(ref usingGetNextLotIdMethod, 1))
                {
                    if (VerifiedLotID == null)
                        VerifiedLotID = new List<string>();

                    if (!VerifiedLotID.Contains(lotId))
                        VerifiedLotID.Add(lotId);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                try
                {
                    //Release the lock
                    Interlocked.Exchange(ref usingGetNextLotIdMethod, 0);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void ReleaseLotId(string lotId)
        {
            try
            {
                if (0 == Interlocked.Exchange(ref usingGetNextLotIdMethod, 1))
                {
                    if (VerifiedLotID != null && VerifiedLotID.Count > 0)
                    {
                        if (VerifiedLotID.Contains(lotId))
                            VerifiedLotID.Remove(lotId);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                try
                {
                    //Release the lock
                    Interlocked.Exchange(ref usingGetNextLotIdMethod, 0);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void saveLog()
        {
            try
            {
                if (this.tic_Logs != null)
                {
                    if (this.tic_Logs.Count > 0)
                    {
                        string FileName = FileLocation + @"\Tickets\" + MakeValidFileName() + "Log.tlog";
                        this.tic_Logs.Save(FileName);
                    }
                }
            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex.Message);
            }
        }

        public Object Clone()
        {
            try
            {
                return this.MemberwiseClone();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        #endregion
    }
}
