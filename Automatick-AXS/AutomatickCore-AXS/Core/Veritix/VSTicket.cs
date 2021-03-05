using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SortedBindingList;
using System.ComponentModel;
using System.Threading;

namespace Automatick.Core
{
    [Serializable]
    public class VSTicket : ITicket,ICloneable
    {
        internal int CurrentParameterIndex = 0;
        internal int CurrentAvailableParameterIndex = 0;
        internal int CurrentFoundParameterIndex = 0;
        internal Boolean IfSelectDeliveryWindowOpen = false;
        private int _currentSelectedAccountIndex = 0;
        private int _currentAccountIndex = 0;
        private decimal _switchProxiesFrom = 1;
        System.Media.SoundPlayer _soundAlert = null;
        Boolean _ifCapsium = false;
        SortableBindingList<VSTicketAccount> _allTMAccounts = null;

        public SortableBindingList<VSTicketAccount> AllTMAccounts
        {
            get { return _allTMAccounts; }
        }

        public VSTicket()
        {
            this._TicketID = UniqueKey.getUniqueKey();
            this.NoOfSearches = 1;
            this.ifAutoBuy = false;
            this.ifAutoBuyWitoutProxy = false;
            this.ifAutoCaptcha = true;
            this.ifBPCAutoCaptcha = false;
            this.ifCPTAutoCaptcha = false;
            this.ifDBCAutoCaptcha = false;
            this.ifRDAutoCaptcha = true;
            this.ifDCAutoCaptcha = false;
            this.ifOCR = false;
            this.ifDistributeInSearches = true;
            this.ifPlaySoundAlert = true;
            this.ifRandomDelay = false;
            this.ifSchedule = false;
            this.ifAntigateCaptcha = false;
            this.if2CAutoCaptcha = false;
            this.ifSelectDeliveryOptionAutoBuying = true;
            this.ifSelectDeliveryOptionList = false;
            this.ifSelectAccountAutoBuying = true;
            this.ifSelectAccountList = false;
            this.ifSendEmail = false;
            this.ifUseGoogle = true;
            this.ifUseFoundOnFirstAttempt = false;
            this.ifUseAvailableParameters = false;
            this.ifUseProxies = true;
            this.ifUseRecaptcha = false;
            this.ifUseSolveMedia = false;
            this.ResetSearchDelay = 3;
            this.ifRDCAutoCaptcha = false;
            this.Parameters = new SortableBindingList<VSParameter>();
            VSParameter parameter = new VSParameter();
            this.Parameters.Add(parameter);

            //TODO: add here multiple presale work

            this.PresaleCodes = new SortableBindingList<VSMultiplePresaleCode>();
            VSMultiplePresaleCode presaleCodes = new VSMultiplePresaleCode();
            this.PresaleCodes.Add(presaleCodes);

            this.ifPresale = false;

            this.ScheduleDateTime = DateTime.Now.AddHours(1);
            this.ScheduleRunningTime = 15;
            this.SelectedDeliveryOptions = new SortableBindingList<VSDeliveryOption>();
            this.StartUsingProxiesFrom = 1;
            this.SwitchProxiesFrom = 1;
            this.StartSolvingFromCaptcha = 1;
            this.TicketFoundCriterions = new SortableBindingList<VSFoundCriteria>();
            this.TicketGroup = null;
            this.SelectedAccounts = new SortableBindingList<VSTicketAccount>();
            this.TicketAccountsInTransition = new SortableBindingList<String>();
            this.ifRun = true;
            this.ifExtractLink = false;
            this.ifContinousRun = false;
            this.ifUseProxiesInCaptchaSource = false;
            this.ifProcessToFinalPage = false;
            this.ifAlwaysAskDeliveryOptionOnBuying = false;
        }
        #region ITicket Members
        private String _TicketID = "";

        public bool isServiceSelected
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

        public SortableBindingList<TicketsLog> tic_Logs
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Veritix 
        /// </summary>
        public Boolean ifExtractLink
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Veritix 
        /// </summary>
        public Boolean ifPresale
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

        public decimal SwitchProxiesFrom
        {
            get
            {
                if (_switchProxiesFrom < 1)
                {
                    _switchProxiesFrom = 1;
                }
                return _switchProxiesFrom;
            }
            set
            {
                _switchProxiesFrom = value;
            }
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

        public Boolean ifPesistSessionInEachSearch
        {
            get;
            set;

        }

        public Boolean ifAlwaysAskDeliveryOptionOnBuying
        {
            get;
            set;

        }

        public Boolean isRunning
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

        public Boolean ifUseGoogle
        {
            get;
            set;
        }

        public Boolean ifUseRecaptcha
        {
            get;
            set;
        }

        public Boolean ifUseSolveMedia
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

        public Boolean ifCPTAutoCaptcha
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

        public Boolean ifOCR
        {
            get;
            set;
        }
        public Boolean ifAntigateCaptcha
        {
            get;
            set;
        }

        public Boolean ifAC1AutoCaptcha
        {
            get;
            set;
        }
        public Boolean if2CAutoCaptcha
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

        public Boolean ifProcessToFinalPage
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

        public Boolean WaitingRoomByPassed
        {
            get;
            set;
        }

        public String WaitingRoomByPassURL
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

        /// <summary>
        /// This property is for Veritix only
        /// </summary>
        public int MaximumPresaleCount
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

        public SortableBindingList<VSParameter> Parameters
        {
            get;
            set;
        }

        public SortableBindingList<VSDeliveryOption> SelectedDeliveryOptions
        {
            get;
            set;
        }

        public SortableBindingList<VSFoundCriteria> TicketFoundCriterions
        {
            get;
            set;
        }

        public SortableBindingList<VSTicketAccount> SelectedAccounts
        {
            get;
            set;
        }


        public SortableBindingList<String> TicketAccountsInTransition
        {
            get;
            set;
        }

        /// <summary>
        /// This property is for Veritix only
        /// </summary>
        public SortableBindingList<VSMultiplePresaleCode> PresaleCodes
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

        public AutoCaptchaServices AutoCaptchaServices
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

        public Boolean ifCapsium
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
                VSTicket ticket = null;
                try
                {
                    String fileName = FileLocation + @"\Tickets\" + MakeValidFileName() + ".tevent";
                    if (File.Exists(fileName))
                    {
                        serializationStream = new FileStream(fileName, FileMode.Open);
                        BinaryFormatter formatter = new BinaryFormatter();
                        ticket = (VSTicket)formatter.Deserialize(serializationStream);
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

                    GC.Collect();
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

                SortableBindingList<ITicketSearch> tmpAllSearches = this.Searches;
                BindingList<ITicketSearch> tmpCaptchaQueue = this.CaptchaQueue;
                ChangeDelegate tmpChangeDelegate = this.onChangeStartOrStop;
                ChangeDelegate tmpstartSearchingDelegate = this.onStartSearching;
                ChangeDelegate tmpFoundDelegate = this.onFound;
                ChangeDelegate tmpNotFoundDelegate = this.onNotFound;
                ChangeDelegateForGauge tmpChangeDelegateForGauge = this.onChangeForGauge;
                System.Media.SoundPlayer tmpSoundAlert = this._soundAlert;
                AutoCaptchaServices tmpAutoCaptchaServices = this.AutoCaptchaServices;
                EmailSetting tmpEmail = this.Email;
                GlobalSettings tmpGlobalSetting = this.GlobalSetting;

                ISearchForm tmpTicketSearchWindow = this.TicketSearchWindow;

            Retry:
                this.CaptchaQueue = null;
                this.Searches = null;
                this.TicketSearchWindow = null;
                this.onChangeStartOrStop = null;
                this.onChangeForGauge = null;
                this.onFound = null;
                this.onNotFound = null;
                this._soundAlert = null;
                this.AutoCaptchaServices = null;
                this.Email = null;
                this.GlobalSetting = null;
                this.onStartSearching = null;

                if (String.IsNullOrEmpty(this._TicketID))
                {
                    this._TicketID = UniqueKey.getUniqueKey();
                }

                this.LastUsedDateTime = DateTime.Now;

                this.SaveLog();

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
                    GC.Collect();
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
                this.CaptchaQueue = tmpCaptchaQueue;
                this.TicketSearchWindow = tmpTicketSearchWindow;
                this.onChangeStartOrStop = tmpChangeDelegate;
                this.onChangeForGauge = tmpChangeDelegateForGauge;
                this.onFound = tmpFoundDelegate;
                this.onNotFound = tmpNotFoundDelegate;
                this._soundAlert = tmpSoundAlert;
                this.AutoCaptchaServices = tmpAutoCaptchaServices;
                this.Email = tmpEmail;
                this.GlobalSetting = tmpGlobalSetting;
                this.onStartSearching = tmpstartSearchingDelegate;

                if (this.CaptchaQueue != null)
                {
                    IEnumerable<ITicketSearch> tmpCaptchaQueu = this.CaptchaQueue.Where(p => p.Ticket.TicketID == this.TicketID);
                    // this.CaptchaQueue = (BindingList < ITicketSearch >) this.CaptchaQueue.Except(tmpCaptchaQueu);

                    BindingList<ITicketSearch> bb = new BindingList<ITicketSearch>();

                    List<ITicketSearch> ss = new List<ITicketSearch>();
                    foreach (ITicketSearch item in tmpCaptchaQueu)
                    {
                        ss.Add(item);
                    }

                    foreach (ITicketSearch item in ss)
                    {
                        this.CaptchaQueue.Remove(item);
                    }

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public void SaveLog()
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

        public void changeSettingsDuringSearching(decimal lastSearches)
        {
            if (this.NoOfSearches > lastSearches)
            {
                for (int i = 0; i < (this.NoOfSearches - lastSearches); i++)
                {
                    if (Searches != null)
                    {
                        VSSearch search = new VSSearch(this);
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

                    if (this.ifPresale)
                    {
                        try
                        {
                            search.presaleCode = VSMultiplePresaleCode.GetNextPresale(this.PresaleCodes);
                        }
                        catch (Exception)
                        {

                        }
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

        public void start(BindingList<ITicketSearch> captchaQueue, System.Media.SoundPlayer soundAlert, AutoCaptchaServices autoCaptchaServices, SortableBindingList<VSTicketAccount> allTMAccountsForSyncing, EmailSetting email, GlobalSettings globalSetting)
        {
            Regex.CacheSize = 0;
            this.isRunning = true;
            this.CaptchaQueue = captchaQueue;
            this._soundAlert = soundAlert;
            Searches = new SortableBindingList<ITicketSearch>();

            this.CurrentParameterIndex = 0;
            this.LastUsedDateTime = DateTime.Now;
            this.AutoCaptchaServices = autoCaptchaServices;
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

            if (this.ifAutoCaptcha && this.ifRDAutoCaptcha)
            {
                this.AutoCaptchaServices.retrieveNewRDUserNameAndPassword();
            }
            else if (this.ifAutoCaptcha && this.ifCPTAutoCaptcha)
            {
                this.AutoCaptchaServices.retrieveNewCPTUserNameAndPassword();
            }

            if (this.TicketAccountsInTransition == null)
            {
                this.TicketAccountsInTransition = new SortableBindingList<String>();
            }

            if (this.TicketAccountsInTransition.Count > 0)
            {
                this.TicketAccountsInTransition.Clear();
            }



            for (int i = 0; i < this.Parameters.Count; i++)
            {
                this.Parameters[i].IfAvailable = false;
                this.Parameters[i].IfFound = false;
            }

            for (int i = 0; i < this.NoOfSearches; i++)
            {
                VSSearch search = new VSSearch(this);
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

                if (i >= ((int)this.StartUsingProxiesFrom - 1) && this.ifUseProxies && i >= ((int)this.SwitchProxiesFrom - 1))
                {
                    search.SearchCycle = 1;
                }

                if (this.ifPresale)
                {
                    try
                    {
                        search.presaleCode = VSMultiplePresaleCode.GetNextPresale(this.PresaleCodes);
                    }
                    catch (Exception)
                    {

                    }
                }

                search.start();
                this.Searches.Add(search);
            }

            if (this.onChangeStartOrStop != null)
            {
                this.onChangeStartOrStop(this);
            }
        }

        public void start(BindingList<ITicketSearch> captchaQueue, System.Media.SoundPlayer soundAlert, AutoCaptchaServices autoCaptchaServices, SortableBindingList<VSTicketAccount> allTMAccountsForSyncing, EmailSetting email, GlobalSettings globalSetting, AccessRights.AccessList capsiumServers)
        {
            Regex.CacheSize = 0;
            this.isRunning = true;
            this.CaptchaQueue = captchaQueue;
            this._soundAlert = soundAlert;
            Searches = new SortableBindingList<ITicketSearch>();

            this.CurrentParameterIndex = 0;
            this.LastUsedDateTime = DateTime.Now;
            this.AutoCaptchaServices = autoCaptchaServices;
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

            if (this.ifAutoCaptcha && this.ifRDAutoCaptcha)
            {
                this.AutoCaptchaServices.retrieveNewRDUserNameAndPassword();
            }
            else if (this.ifAutoCaptcha && this.ifCPTAutoCaptcha)
            {
                this.AutoCaptchaServices.retrieveNewCPTUserNameAndPassword();
            }

            if (this.TicketAccountsInTransition == null)
            {
                this.TicketAccountsInTransition = new SortableBindingList<String>();
            }

            if (this.TicketAccountsInTransition.Count > 0)
            {
                this.TicketAccountsInTransition.Clear();
            }

            if (capsiumServers != null)
            {
                if (!String.IsNullOrEmpty(capsiumServers.name) && Boolean.Parse(capsiumServers.access))
                {
                    this._ifCapsium = true;
                }
            }

            for (int i = 0; i < this.Parameters.Count; i++)
            {
                this.Parameters[i].IfAvailable = false;
                this.Parameters[i].IfFound = false;
            }

            for (int i = 0; i < this.NoOfSearches; i++)
            {
                VSSearch search = new VSSearch(this);
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

                if (i >= ((int)this.StartUsingProxiesFrom - 1) && this.ifUseProxies && i >= ((int)this.SwitchProxiesFrom - 1))
                {
                    search.SearchCycle = 1;
                }

                if (this.ifPresale)
                {
                    try
                    {
                        search.presaleCode = VSMultiplePresaleCode.GetNextPresale(this.PresaleCodes);
                    }
                    catch (Exception)
                    {

                    }
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
                    for (int i = 0; i < tmpArrayToStop.Length; i++)
                    {
                        try
                        {
                            ((VSSearch)tmpArrayToStop[i]).IfWorking = false;
                            ((VSSearch)tmpArrayToStop[i]).captchaload.Set();
                        }
                        catch { }
                    }
                    for (int i = 0; i < tmpArrayToStop.Length; i++)
                    {
                        try
                        {
                            tmpArrayToStop[i].stop();
                            tmpArrayToStop[i].Dispose();
                        }
                        catch { }
                    }
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
                    //this.SaveTicket();
                    GC.Collect();
                }
                catch (Exception)
                {

                }
            }
        }

        public object Clone()
        {
            try
            {
                return this.MemberwiseClone();
            }
            catch (Exception)
            {

            }
            return null;
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

                Thread thStop = new Thread(new ParameterizedThreadStart(this.stopThreadHandler));
                thStop.Priority = ThreadPriority.Normal;
                thStop.SetApartmentState(ApartmentState.STA);
                thStop.IsBackground = true;
                thStop.Start(this.Searches);

                //this.stopThreadHandler(this.Searches);

                this.SaveTicket();
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
                        VSTicketAccount syncAccount = _allTMAccounts.FirstOrDefault(p => p.AccountEmail == this.SelectedAccounts[i].AccountEmail);
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
                    this.SelectedAccounts = new SortableBindingList<VSTicketAccount>();
                }
            }
            else
            {
                this.SelectedAccounts = new SortableBindingList<VSTicketAccount>();
            }

            this.SaveTicket();
        }

        public VSTicketAccount getNextAccount(Boolean newCard = false)
        {
            VSTicketAccount account = null;

            if (this.ifSelectAccountList)
            {
                if (this.SelectedAccounts != null)
                {
                    if (this.SelectedAccounts.Count > 0)
                    {
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
                                if (newCard == account.GroupName.Equals("new"))
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
                                if (newCard == account.GroupName.Equals("new"))
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
        #endregion
    }
}
