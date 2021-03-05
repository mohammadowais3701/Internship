using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1Ribbon;
using Automatick.Core;
using SortedBindingList;
using System.Collections;
using AccessRights;
using System.Threading;
using System.Diagnostics;
using PushClientLibrary;
using TCPClient;
using System.Threading.Tasks;
using GeneralBucket;

namespace Automatick
{
    public partial class frmMain : C1RibbonForm, IMainForm
    {
        int captchatorWorker = 10;
        private Dictionary<String, System.Threading.Timer> _pendingScheduledTickets = null;
        private Dictionary<String, System.Threading.Timer> _runningScheduledTickets = null;
        private SortableBindingList<AXSSearch> _allSearches = null;
        private ApplicationPermissions _appPermissions;
        private Boolean _ifMonitoring = true;
        private LicenseCore _lic;
        String LICENSING_SERVER_IP = "mainserver.ticketpeers.com";

        private Boolean IsRunning = false;
        private ManualResetEvent mre = null;

        public LicenseCore Lic
        {
            get { return _lic; }
            set { _lic = value; }
        }
        public ApplicationPermissions AppPermissions
        {
            get
            {
                return this._appPermissions;
            }
        }

        public ApplicationStartUp AppStartUp
        {
            get;
            set;
        }

        public Boolean chkEnableProxyManagerVisible
        {
            get
            {
                return chkEnableProxyManager.Visible;
            }
            set
            {
                chkEnableProxyManager.Visible = value;
            }
        }
        public Boolean chkEnableISPProxyUsageVisible
        {
            get
            {
                return chkEnableISPProxyUsage.Visible;
            }
            set
            {
                chkEnableISPProxyUsage.Visible = value;
            }
        }
        public Boolean rbRegisterPMVisible
        {
            get
            {
                return rbRegisterPM.Visible;
            }
            set
            {
                rbRegisterPM.Visible = value;
            }
        }

        public Boolean chkEnableISPProxyUsageChecked
        {
            get
            {
                return chkEnableISPProxyUsage.Checked;
            }
            set
            {
                chkEnableISPProxyUsage.Checked = value;
            }
        }
        public Boolean chkEnableProxyManagerChecked
        {
            get
            {
                return chkEnableProxyManager.Checked;
            }
            set
            {
                chkEnableProxyManager.Checked = value;
            }
        }

        public Boolean chkEnableLuminatiProxiesChecked
        {
            get
            {
                return chkEnableLuminatiProxies.Checked;
            }
            set
            {
                chkEnableLuminatiProxies.Checked = value;
            }
        }

        public ICaptchaForm CaptchaForm
        {
            get;
            set;
        }

        public ICaptchaForm CaptchaBrowserForm
        {
            get;
            set;
        }

        public frmMain(ApplicationPermissions appPermissions, LicenseCore lic)
        {
            this._appPermissions = appPermissions;
            this._lic = lic;
            InitializeComponent();
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Show();
                    WindowState = FormWindowState.Normal;
                }
                else
                {
                    WinApi.ShowToFront(this.Handle);
                }
            }
            base.WndProc(ref message);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this._ifMonitoring = false;
            this._pendingScheduledTickets = new Dictionary<String, System.Threading.Timer>();
            this._runningScheduledTickets = new Dictionary<String, System.Threading.Timer>();

            this.ribbonTabSearches.Visible = false;
            this.rgForceStartAndStop.Visible = false;
            this.btnForceRun.Visible = false;
            this.btnForceStop.Visible = false;

            _allSearches = new SortableBindingList<AXSSearch>();
            this.tMSearchBindingSource.DataSource = this._allSearches;


            CaptchaBrowserForm = new frmCaptchaBrowser();
            CaptchaBrowserForm.showForm();
            CaptchaBrowserForm.hideForm();

            CaptchaForm = new frmCaptcha();
            CaptchaForm.showForm();
            CaptchaForm.hideForm();

            AppStartUp = new ApplicationStartUp(Application.StartupPath);

            AppStartUp.LoadGlobalSettings();
            loadMonitoringOptions();
            loadAutoCaptchaServices();
            loadTickets();
            loadAccounts();
            loadDeliveryOptions();
            loadEmails();
            loadGroups();
            loadProxies();

            ProxyPicker.createProxyPickerInstance(this);
            Captchator.createCaptchatorInstance(AppStartUp.AutoCaptchaService, captchatorWorker);
            LotIDPicker.createLotIDInstance(this, this._lic);
            //LotIDPicker.LotIDInstance.setLotIDs();

            try
            {
                for (int i = 0; i < this.AppStartUp.Tickets.Count; i++)
                {
                    this.AppStartUp.Tickets[i].TicketStatus = "";
                }
            }
            catch (Exception)
            {
            }
            //this.AppStartUp.GlobalSetting.IfUseSpecialProxies = false;
            //ProxyPicker.ProxyPickerInstance.ValidatePPM(false);
            this.setRelayProxies();
            this.setSpecialProxiesValues();

            try
            {
                Boolean allowed = false;
                if (this.AppStartUp.GlobalSetting.IfUseRelayProxies || this.AppStartUp.GlobalSetting.IfUseSpecialRelayProxies)
                {
                    allowed = true;
                }

                ProxyPicker.ProxyPickerInstance.ProxyRelayManager = new ProxyManager.RelayProxyManager(this.AppStartUp.Proxies, this.AppStartUp.GlobalSetting.Username, this.AppStartUp.GlobalSetting.Password, allowed);

                ProxyPicker.ProxyPickerInstance.RelayCounter = this.AppStartUp.GlobalSetting.RelayPercentCount;
            }
            catch (Exception ex)
            {
            }

            Task.Run(() =>
            {

                SAPushClient.initialize(LICENSING_SERVER_IP, 44000, new CommandProcessor());
                LicenseMessage.RegisterLicense(ValidationMessage.TixToxInstance.HardDiskSerial, ValidationMessage.TixToxInstance.LicenseCode, ValidationMessage.TixToxInstance.ProcessorID, ValidationMessage.TixToxInstance.AppPrefix);
                LicenseMessage.SendMessage("RelayServer");
                LicenseMessage.SendMessage("getThrottleValue");
                LicenseMessage.SendMessage("C1");
                LicenseMessage.SendMessage("PercentRelay");
                LicenseMessage.SendMessage("getAAXMagic");
            });

            try
            {
                this.rbTxtPassword.Text = this.AppStartUp.GlobalSetting.Password = "system";
                this.rbTxtUsername.Text = this.AppStartUp.GlobalSetting.Username = "system";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            if (!this.AppStartUp.GlobalSetting.IfUseSpecialProxies)
            {
                this.chkEnableLuminatiProxies.Visible = false;
                // this.cmbProxiesCountry.Visible = false;
                ProxyPicker.ProxyPickerInstance.ValidatePPM(false);
            }
            else
            {
                this.rbRegisterPM.Visible = false;
                this.chkEnableLuminatiProxies.Visible = true;
                //this.cmbProxiesCountry.Visible = true;
                ProxyPicker.ProxyPickerInstance.ValidatePPM(false);
                enableSpecialProxies();
            }

            this.applyPermission();

            ImageMerger.createImageMerger();

            AccessRights.AccessList isWeb = AppPermissions.AllAccessList.Single(p => p.form == "isWeb");
            AccessRights.AccessList isMobile = AppPermissions.AllAccessList.Single(p => p.form == "isMobile");
            AccessRights.AccessList isEventko = AppPermissions.AllAccessList.Single(p => p.form == "isEventko");

            if (isWeb != null)
            {
                AppStartUp.GlobalSetting.IfWeb = Boolean.Parse(isWeb.access);
            }

            if (isMobile != null)
            {
                AppStartUp.GlobalSetting.ifMobile = Boolean.Parse(isMobile.access);
            }

            if (isEventko != null)
            {
                AppStartUp.GlobalSetting.ifEventko = Boolean.Parse(isEventko.access);
            }

            // GC collector timer
            this.IsRunning = true;
            GoodProxies.Add(null);

            mre = new ManualResetEvent(false);

            Thread th = new Thread(GarbageCollectorTimer);
            th.SetApartmentState(ApartmentState.STA);
            th.Priority = ThreadPriority.Normal;
            th.IsBackground = true;
            th.Start();
        }

        private void setRelayProxies()
        {
            try
            {
                IEnumerable<AccessList> allControlsAccesses = this.AppPermissions.AllAccessList.Where(p => p.form == this.Name);

                foreach (AccessList obj in allControlsAccesses)
                {
                    try
                    {
                        if (obj.name == "IfUseRelayProxies")
                        {
                            this.AppStartUp.GlobalSetting.IfUseRelayProxies = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "IfUseSpecialRelayProxies")
                        {
                            this.AppStartUp.GlobalSetting.IfUseSpecialRelayProxies = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "RelayProxies10Percent")
                        {
                            this.AppStartUp.GlobalSetting.RelayProxies10Percent = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "RelayProxies25Percent")
                        {
                            this.AppStartUp.GlobalSetting.SpecialProxies125 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "RelayProxies50Percent")
                        {
                            this.AppStartUp.GlobalSetting.RelayProxies50Percent = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "RelayProxies100Percent")
                        {
                            this.AppStartUp.GlobalSetting.RelayProxies100Percent = Boolean.Parse(obj.access);
                        }


                    }
                    catch { }
                }

                if (this.AppStartUp.GlobalSetting.IfUseRelayProxies || this.AppStartUp.GlobalSetting.IfUseSpecialRelayProxies)
                {
                    if (this.AppStartUp.GlobalSetting.RelayProxies10Percent)
                    {
                        this.AppStartUp.GlobalSetting.RelayPercentCount = 10;
                    }
                    else if (!this.AppStartUp.GlobalSetting.RelayProxies10Percent && this.AppStartUp.GlobalSetting.RelayProxies25Percent)
                    {
                        this.AppStartUp.GlobalSetting.RelayPercentCount = 25;
                    }
                    else if (!this.AppStartUp.GlobalSetting.RelayProxies10Percent && !this.AppStartUp.GlobalSetting.RelayProxies25Percent && this.AppStartUp.GlobalSetting.RelayProxies50Percent)
                    {
                        this.AppStartUp.GlobalSetting.RelayPercentCount = 50;
                    }
                    else if (!this.AppStartUp.GlobalSetting.RelayProxies10Percent && !this.AppStartUp.GlobalSetting.RelayProxies25Percent && !this.AppStartUp.GlobalSetting.RelayProxies50Percent && this.AppStartUp.GlobalSetting.RelayProxies100Percent)
                    {
                        this.AppStartUp.GlobalSetting.RelayPercentCount = 100;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void GarbageCollectorTimer()
        {
            try
            {
                while (IsRunning)
                {
                    LicenseMessage.SendMessage("getAAXMagic");
                    GC.Collect();
                    mre.WaitOne(1000 * 45 * 1);
                }
            }
            catch (Exception)
            {

            }
        }

        private void setSpecialProxiesValues()
        {
            IEnumerable<AccessList> allControlsAccesses = this.AppPermissions.AllAccessList.Where(p => p.form == this.Name);
            foreach (AccessList obj in allControlsAccesses)
            {
                try
                {
                    if (obj.name == "IfUseSpecialProxies")
                    {
                        this.AppStartUp.GlobalSetting.IfUseSpecialProxies = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxies150")
                    {
                        this.AppStartUp.GlobalSetting.SpecialProxies150 = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxies125")
                    {
                        this.AppStartUp.GlobalSetting.SpecialProxies125 = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxies100")
                    {
                        this.AppStartUp.GlobalSetting.SpecialProxies100 = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxies75")
                    {
                        this.AppStartUp.GlobalSetting.SpecialProxies75 = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxies50")
                    {
                        this.AppStartUp.GlobalSetting.SpecialProxies50 = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxies15")
                    {
                        this.AppStartUp.GlobalSetting.SpecialProxies15 = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxiesAny")
                    {
                        this.AppStartUp.GlobalSetting.CountryAny = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxiesUS")
                    {
                        this.AppStartUp.GlobalSetting.CountryUS = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxiesGB")
                    {
                        this.AppStartUp.GlobalSetting.CountryGB = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxiesAU")
                    {
                        this.AppStartUp.GlobalSetting.CountryAU = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxiesCA")
                    {
                        this.AppStartUp.GlobalSetting.CountryCA = Boolean.Parse(obj.access);
                    }
                    else if (obj.name == "SpecialProxiesUSCA")
                    {
                        this.AppStartUp.GlobalSetting.CountryUSCA = Boolean.Parse(obj.access);
                    }

                }
                catch { }
            }

            if (this.AppStartUp.GlobalSetting.SpecialProxies150)
            {
                this.AppStartUp.GlobalSetting.ProxiesCount = 150;
            }
            else if (!this.AppStartUp.GlobalSetting.SpecialProxies150 && this.AppStartUp.GlobalSetting.SpecialProxies125)
            {
                this.AppStartUp.GlobalSetting.ProxiesCount = 125;
            }
            else if (!this.AppStartUp.GlobalSetting.SpecialProxies150 && !this.AppStartUp.GlobalSetting.SpecialProxies125 && this.AppStartUp.GlobalSetting.SpecialProxies100)
            {
                this.AppStartUp.GlobalSetting.ProxiesCount = 100;
            }
            else if (!this.AppStartUp.GlobalSetting.SpecialProxies150 && !this.AppStartUp.GlobalSetting.SpecialProxies125 && !this.AppStartUp.GlobalSetting.SpecialProxies100 && this.AppStartUp.GlobalSetting.SpecialProxies75)
            {
                this.AppStartUp.GlobalSetting.ProxiesCount = 75;
            }
            else if (!this.AppStartUp.GlobalSetting.SpecialProxies150 && !this.AppStartUp.GlobalSetting.SpecialProxies125 && !this.AppStartUp.GlobalSetting.SpecialProxies100 && !this.AppStartUp.GlobalSetting.SpecialProxies75 && this.AppStartUp.GlobalSetting.SpecialProxies50)
            {
                this.AppStartUp.GlobalSetting.ProxiesCount = 50;
            }
            else if (!this.AppStartUp.GlobalSetting.SpecialProxies150 && !this.AppStartUp.GlobalSetting.SpecialProxies125 && !this.AppStartUp.GlobalSetting.SpecialProxies100 && !this.AppStartUp.GlobalSetting.SpecialProxies75 && !this.AppStartUp.GlobalSetting.SpecialProxies50 && this.AppStartUp.GlobalSetting.SpecialProxies15)
            {
                this.AppStartUp.GlobalSetting.ProxiesCount = 15;
            }
            else
            {
                this.AppStartUp.GlobalSetting.ProxiesCount = 25;
            }

            ProxyPicker.ProxyPickerInstance.globalproxycount = this.AppStartUp.GlobalSetting.ProxiesCount;

            if (this.AppStartUp.GlobalSetting.CountryGB)
            {
                this.AppStartUp.GlobalSetting.ProxiesCountry = "gb";
            }
            else if (this.AppStartUp.GlobalSetting.CountryAU)
            {
                this.AppStartUp.GlobalSetting.ProxiesCountry = "au";
            }
            else if (this.AppStartUp.GlobalSetting.CountryAny)
            {
                this.AppStartUp.GlobalSetting.ProxiesCountry = "any";
            }
            else if (this.AppStartUp.GlobalSetting.CountryCA)
            {
                this.AppStartUp.GlobalSetting.ProxiesCountry = "ca";
            }
            else
            {
                this.AppStartUp.GlobalSetting.ProxiesCountry = "us";
            }
        }

        private void loadMonitoringOptions()
        {
            if (AppStartUp.GlobalSetting != null)
            {
                this.chkMonday.Checked = this.AppStartUp.GlobalSetting.ifRunOnMonday;
                this.chkTuesday.Checked = this.AppStartUp.GlobalSetting.ifRunOnTuesday;
                this.chkWednesday.Checked = this.AppStartUp.GlobalSetting.ifRunOnWednesday;
                this.chkThursday.Checked = this.AppStartUp.GlobalSetting.ifRunOnThursday;
                this.chkFriday.Checked = this.AppStartUp.GlobalSetting.ifRunOnFriday;
                this.chkSaturday.Checked = this.AppStartUp.GlobalSetting.ifRunOnSaturday;
                this.chkSunday.Checked = this.AppStartUp.GlobalSetting.ifRunOnSunday;
                this.tMonitoringTime.Value = this.AppStartUp.GlobalSetting.MonitoringTimeFrom;
                this.nudMonitoringHours.Value = this.AppStartUp.GlobalSetting.MonitoringHours;
                this.chkSendEmail.Checked = this.AppStartUp.GlobalSetting.ifSendEmail;
                this.chkPlayMusic.Checked = this.AppStartUp.GlobalSetting.ifPlaySoundAlert;
                this.chkExtendSearches.Checked = this.AppStartUp.GlobalSetting.ifFoundExtendNumberOfSearches;
                this.AXSByPassWaitingRoom.Checked = this.AppStartUp.GlobalSetting.Permission;
                this.nudExtendSearchesUpto.Value = this.AppStartUp.GlobalSetting.ExtendSearchesTo;
                this.nudMaxConcurrentEventsToRun.Value = this.AppStartUp.GlobalSetting.MaxConcurrentEventsToRun;
                this.nudRescheduleDelay.Value = this.AppStartUp.GlobalSetting.RescheduleDelay;

                this.chkAutoCaptcha.Checked = this.AppStartUp.GlobalSetting.ifAutoCaptcha;

                if (this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbBPCAutoCaptcha;
                }
                else if (this.AppStartUp.GlobalSetting.ifRDAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbRDAutoCaptcha;
                }
                else if (this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbRDCAutoCaptcha;
                }
                else if (this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbCPTAutoCaptcha;
                }
                else if (this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbDBCAutoCaptcha;
                }
                else if (this.AppStartUp.GlobalSetting.ifDCAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbDCAutoCaptcha;
                }
                else if (this.AppStartUp.GlobalSetting.ifOCR)
                {
                    this.cbCaptchaService.SelectedItem = rbOCR;
                }
                else if (this.AppStartUp.GlobalSetting.if2C)
                {
                    this.cbCaptchaService.SelectedItem = rb2CAutoCaptcha;
                }
                else if (this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbAC1AutoCaptcha;
                }
                else if (this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbAAutoCaptcha;
                }

                extendSeachesCheckedChanged();
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        public void loadTickets()
        {
            AppStartUp.LoadTickets();
            populateTickets();
        }

        public void scheduleTickets()
        {
            if (this.AppStartUp != null)
            {
                if (this.AppStartUp.Tickets != null && this._ifMonitoring)
                {
                    for (int i = 0; i < this.AppStartUp.Tickets.Count; i++)
                    {
                        if (this.AppStartUp.Tickets[i].ifSchedule)
                        {
                            setScheduledTimer(this.AppStartUp.Tickets[i]);
                        }
                        else if (this._pendingScheduledTickets.ContainsKey(this.AppStartUp.Tickets[i].TicketID))
                        {
                            unsetScheduledTimer(this.AppStartUp.Tickets[i]);
                        }
                    }
                }
            }
        }

        public void runScheduledTicket(object ticket)
        {
            try
            {
                if (ticket != null)
                {
                    AXSTicket ticketToRun = (AXSTicket)ticket;
                    int countRunningTickets = this._runningScheduledTickets.Count;
                    if (this.AppStartUp.GlobalSetting.MaxConcurrentEventsToRun > 0 ? countRunningTickets < this.AppStartUp.GlobalSetting.MaxConcurrentEventsToRun : true)
                    {
                        if (ticketToRun.ifSchedule && ticketToRun.ifRun)
                        {
                            ticketToRun.StartSolvingFromCaptcha = 1;
                            ticketToRun.StartUsingProxiesFrom = 1;
                            ticketToRun.ifUseProxies = true;
                            ticketToRun.ifSendEmail = this.AppStartUp.GlobalSetting.ifSendEmail;
                            ticketToRun.ifPlaySoundAlert = this.AppStartUp.GlobalSetting.ifPlaySoundAlert;

                            ticketToRun.ifBPCAutoCaptcha = this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha;
                            ticketToRun.ifRDAutoCaptcha = this.AppStartUp.GlobalSetting.ifRDAutoCaptcha;
                            ticketToRun.ifRDCAutoCaptcha = this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha;
                            ticketToRun.ifCPTAutoCaptcha = this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha;
                            ticketToRun.ifDBCAutoCaptcha = this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha;
                            ticketToRun.ifDCAutoCaptcha = this.AppStartUp.GlobalSetting.ifDCAutoCaptcha;
                            ticketToRun.ifOCR = this.AppStartUp.GlobalSetting.ifOCR;
                            ticketToRun.ifCAutoCaptcha = this.AppStartUp.GlobalSetting.ifCAutoCaptcha;
                            ticketToRun.ifROCR = this.AppStartUp.GlobalSetting.ifROCR;
                            ticketToRun.ifBoloOCR = this.AppStartUp.GlobalSetting.ifBoloOCR;
                            ticketToRun.ifSOCR = this.AppStartUp.GlobalSetting.ifSOCR;
                            ticketToRun.if2C = this.AppStartUp.GlobalSetting.if2C;
                            ticketToRun.ifCaptchator = this.AppStartUp.GlobalSetting.ifCaptchator;
                            ticketToRun.ifAC1AutoCaptcha = this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha;
                            ticketToRun.ifAntigateAutoCaptcha = this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha;

                            ticketToRun.ifSchedule = false;
                            ticketToRun.NoOfSearches = 1;
                            ticketToRun.SaveTicket();

                            if (!ticketToRun.isRunning)
                            {
                                DateTime currentTime = DateTime.Now;
                                TimeSpan ts = (TimeSpan)(currentTime.AddMinutes((int)ticketToRun.ScheduleRunningTime) - currentTime);

                                System.Threading.TimerCallback tcb = this.stopScheduledTicket;
                                System.Threading.Timer unscheduleTimer = new System.Threading.Timer(tcb, ticketToRun, ts, new TimeSpan(-1));

                                lock (this._runningScheduledTickets)
                                {
                                    if (this._runningScheduledTickets.ContainsKey(ticketToRun.TicketID))
                                    {
                                        this._runningScheduledTickets[ticketToRun.TicketID].Dispose();
                                        GC.SuppressFinalize(this._runningScheduledTickets[ticketToRun.TicketID]);

                                        this._runningScheduledTickets[ticketToRun.TicketID] = unscheduleTimer;
                                    }
                                    else
                                    {
                                        this._runningScheduledTickets.Add(ticketToRun.TicketID, unscheduleTimer);
                                    }
                                }

                                if (this.InvokeRequired)
                                {
                                    this.Invoke(new MethodInvoker(delegate()
                                    {
                                        ticketToRun.onChangeStartOrStop = new ChangeDelegate(this.onChangeHandlerStartOrStop);
                                        ticketToRun.onStartSearching = new ChangeDelegate(this.onStartSearchingHandler);
                                        ticketToRun.onFound = new ChangeDelegate(this.onFoundHandler);
                                        ticketToRun.onNotFound = new ChangeDelegate(this.onNotFoundHandler);
                                        if ((ticketToRun.URL.Contains(".queue") || ticketToRun.URL.Contains("q.axs.co.uk")) || (this.AppStartUp.GlobalSetting.ifEventko && ticketToRun.URL.Contains("evenko.ca")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("axs.com")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("shop.axs.co.uk")))
                                        {
                                            ticketToRun.start(this.CaptchaForm.CaptchaQueue, this.CaptchaBrowserForm.CaptchaQueue, this.AppStartUp.SoundAlert, this.AppStartUp.AutoCaptchaService, this.AppStartUp.Accounts, this.AppStartUp.EmailSetting, this.AppStartUp.GlobalSetting);

                                            if (ticketToRun.Searches != null)
                                            {
                                                ticketToRun.Searches.changeDelegate = new SortedBindingList.SortableBindingList<ITicketSearch>.dlgOnChange(this.changeDelegateHandler);
                                                lock (this._allSearches)
                                                {
                                                    foreach (AXSSearch search in ticketToRun.Searches)
                                                    {
                                                        if (!this._allSearches.Contains(search))
                                                        {
                                                            this._allSearches.Add(search);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("You are not allowed to run this event");
                                        }

                                    }));
                                }
                                else
                                {
                                    ticketToRun.onChangeStartOrStop = new ChangeDelegate(this.onChangeHandlerStartOrStop);
                                    ticketToRun.onStartSearching = new ChangeDelegate(this.onStartSearchingHandler);
                                    ticketToRun.onFound = new ChangeDelegate(this.onFoundHandler);
                                    ticketToRun.onNotFound = new ChangeDelegate(this.onNotFoundHandler);

                                    //ticketToRun.start(this.AppStartUp.SoundAlert, this.AppStartUp.Accounts, this.AppStartUp.EmailSetting, this.AppStartUp.GlobalSetting);
                                    if ((ticketToRun.URL.Contains(".queue") || ticketToRun.URL.Contains("q.axs.co.uk")) || (this.AppStartUp.GlobalSetting.ifEventko && ticketToRun.URL.Contains("evenko.ca")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("axs.com")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("shop.axs.co.uk")))
                                    {
                                        ticketToRun.start(this.CaptchaForm.CaptchaQueue, this.CaptchaBrowserForm.CaptchaQueue, this.AppStartUp.SoundAlert, this.AppStartUp.AutoCaptchaService, this.AppStartUp.Accounts, this.AppStartUp.EmailSetting, this.AppStartUp.GlobalSetting);

                                        if (ticketToRun.Searches != null)
                                        {
                                            ticketToRun.Searches.changeDelegate = new SortedBindingList.SortableBindingList<ITicketSearch>.dlgOnChange(this.changeDelegateHandler);
                                            lock (this._allSearches)
                                            {
                                                foreach (AXSSearch search in ticketToRun.Searches)
                                                {
                                                    if (!this._allSearches.Contains(search))
                                                    {
                                                        this._allSearches.Add(search);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("You are not allowed to run this event");
                                    }
                                }
                            }
                        }

                        unsetScheduledTimer(ticketToRun);
                        changeTicketStatus(ticketToRun, "Searching for tickets");
                    }
                    else
                    {
                        extendScheduledTicket(ticketToRun, 10);
                        this.onChangeHandlerStartOrStop(ticketToRun);
                    }

                }
            }
            catch (Exception)
            {

            }
        }

        public void forceRunTicket(object ticket)
        {
            try
            {
                if (ticket != null)
                {
                    AXSTicket ticketToRun = (AXSTicket)ticket;
                    int countRunningTickets = this._runningScheduledTickets.Count;
                    if (ticketToRun.ifSchedule && ticketToRun.ifRun)
                    {
                        ticketToRun.StartSolvingFromCaptcha = 1;
                        ticketToRun.StartUsingProxiesFrom = 1;
                        ticketToRun.ifUseProxies = true;
                        ticketToRun.ifAutoCaptcha = this.AppStartUp.GlobalSetting.ifAutoCaptcha;
                        ticketToRun.ifBPCAutoCaptcha = this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha;
                        ticketToRun.ifDBCAutoCaptcha = this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha;
                        ticketToRun.ifRDAutoCaptcha = this.AppStartUp.GlobalSetting.ifRDAutoCaptcha;
                        ticketToRun.ifRDCAutoCaptcha = this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha;
                        ticketToRun.ifAntigateAutoCaptcha = this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha;
                        ticketToRun.ifCPTAutoCaptcha = this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha;
                        ticketToRun.ifDCAutoCaptcha = this.AppStartUp.GlobalSetting.ifDCAutoCaptcha;
                        ticketToRun.ifCAutoCaptcha = this.AppStartUp.GlobalSetting.ifCAutoCaptcha;
                        ticketToRun.ifAC1AutoCaptcha = this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha;
                        ticketToRun.ifOCR = this.AppStartUp.GlobalSetting.ifOCR;
                        ticketToRun.ifROCR = this.AppStartUp.GlobalSetting.ifROCR;
                        ticketToRun.ifBoloOCR = this.AppStartUp.GlobalSetting.ifBoloOCR;
                        ticketToRun.ifSOCR = this.AppStartUp.GlobalSetting.ifSOCR;
                        ticketToRun.if2C = this.AppStartUp.GlobalSetting.if2C;
                        ticketToRun.ifSendEmail = this.AppStartUp.GlobalSetting.ifSendEmail;
                        ticketToRun.ifPlaySoundAlert = this.AppStartUp.GlobalSetting.ifPlaySoundAlert;


                        ticketToRun.ifSchedule = false;
                        ticketToRun.NoOfSearches = 1;
                        ticketToRun.SaveTicket();

                        if (!ticketToRun.isRunning)
                        {
                            DateTime currentTime = DateTime.Now;
                            TimeSpan ts = (TimeSpan)(currentTime.AddMinutes((int)ticketToRun.ScheduleRunningTime) - currentTime);

                            System.Threading.TimerCallback tcb = this.stopScheduledTicket;
                            System.Threading.Timer unscheduleTimer = new System.Threading.Timer(tcb, ticketToRun, ts, new TimeSpan(-1));

                            lock (this._runningScheduledTickets)
                            {
                                if (this._runningScheduledTickets.ContainsKey(ticketToRun.TicketID))
                                {
                                    this._runningScheduledTickets[ticketToRun.TicketID].Dispose();
                                    GC.SuppressFinalize(this._runningScheduledTickets[ticketToRun.TicketID]);

                                    this._runningScheduledTickets[ticketToRun.TicketID] = unscheduleTimer;
                                }
                                else
                                {
                                    this._runningScheduledTickets.Add(ticketToRun.TicketID, unscheduleTimer);
                                }
                            }

                            if (this.InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate()
                                {
                                    ticketToRun.onChangeStartOrStop = new ChangeDelegate(this.onChangeHandlerStartOrStop);
                                    ticketToRun.onStartSearching = new ChangeDelegate(this.onStartSearchingHandler);
                                    ticketToRun.onFound = new ChangeDelegate(this.onFoundHandler);
                                    ticketToRun.onNotFound = new ChangeDelegate(this.onNotFoundHandler);


                                    if (ticketToRun.URL.Contains("bit.ly") || (ticketToRun.URL.Contains(".queue") || ticketToRun.URL.Contains("q.axs.co.uk")) || (this.AppStartUp.GlobalSetting.ifEventko && ticketToRun.URL.Contains("evenko.ca")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("axs.com")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("shop.axs.co.uk")))
                                    {
                                        ticketToRun.start(this.CaptchaForm.CaptchaQueue, this.CaptchaBrowserForm.CaptchaQueue, this.AppStartUp.SoundAlert, this.AppStartUp.AutoCaptchaService, this.AppStartUp.Accounts, this.AppStartUp.EmailSetting, this.AppStartUp.GlobalSetting);

                                        if (ticketToRun.Searches != null)
                                        {
                                            ticketToRun.Searches.changeDelegate = new SortedBindingList.SortableBindingList<ITicketSearch>.dlgOnChange(this.changeDelegateHandler);
                                            foreach (AXSSearch search in ticketToRun.Searches)
                                            {
                                                if (!this._allSearches.Contains(search))
                                                {
                                                    this._allSearches.Add(search);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("You are not allowed to run this event");
                                    }

                                }));
                            }
                            else
                            {
                                ticketToRun.onChangeStartOrStop = new ChangeDelegate(this.onChangeHandlerStartOrStop);
                                ticketToRun.onStartSearching = new ChangeDelegate(this.onStartSearchingHandler);
                                ticketToRun.onFound = new ChangeDelegate(this.onFoundHandler);
                                ticketToRun.onNotFound = new ChangeDelegate(this.onNotFoundHandler);
                                if ((ticketToRun.URL.Contains(".queue") || ticketToRun.URL.Contains("q.axs.co.uk")) || (this.AppStartUp.GlobalSetting.ifEventko && ticketToRun.URL.Contains("evenko.ca")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("axs.com")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("shop.axs.co.uk")))
                                {
                                    ticketToRun.start(this.CaptchaForm.CaptchaQueue, this.CaptchaBrowserForm.CaptchaQueue, this.AppStartUp.SoundAlert, this.AppStartUp.AutoCaptchaService, this.AppStartUp.Accounts, this.AppStartUp.EmailSetting, this.AppStartUp.GlobalSetting);

                                    if (ticketToRun.Searches != null)
                                    {
                                        ticketToRun.Searches.changeDelegate = new SortedBindingList.SortableBindingList<ITicketSearch>.dlgOnChange(this.changeDelegateHandler);
                                        foreach (AXSSearch search in ticketToRun.Searches)
                                        {
                                            if (!this._allSearches.Contains(search))
                                            {
                                                this._allSearches.Add(search);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You are not allowed to run this event");
                                }
                            }
                        }
                    }

                    unsetScheduledTimer(ticketToRun);
                    changeTicketStatus(ticketToRun, "Searching for tickets");

                }
            }
            catch (Exception)
            {

            }
        }

        public void stopScheduledTicket(object ticket)
        {
            try
            {
                if (ticket != null)
                {
                    AXSTicket ticketToStop = (AXSTicket)ticket;
                    if (ticketToStop.isRunning)
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new MethodInvoker(delegate()
                            {
                                //lock (this._allSearches)
                                {
                                    foreach (AXSSearch search in ticketToStop.Searches)
                                    {
                                        if (this._allSearches.Contains(search))
                                        {
                                            this._allSearches.Remove(search);
                                            tMSearchBindingSource.ResetBindings(false);
                                        }
                                    }
                                }

                                ticketToStop.stop();
                                reScheduleTicket(ticketToStop);
                                ticketToStop.isRunning = false;
                                this.onChangeHandlerStartOrStop(ticketToStop);
                            }));
                        }
                        else
                        {
                            foreach (AXSSearch search in ticketToStop.Searches)
                            {
                                if (this._allSearches.Contains(search))
                                {
                                    this._allSearches.Remove(search);
                                    tMSearchBindingSource.ResetBindings(false);
                                }
                            }
                            ticketToStop.stop();
                            reScheduleTicket(ticketToStop);
                            ticketToStop.isRunning = false;
                            this.onChangeHandlerStartOrStop(ticketToStop);
                        }
                    }

                    lock (this._runningScheduledTickets)
                    {
                        if (this._runningScheduledTickets.ContainsKey(ticketToStop.TicketID))
                        {
                            if (this._runningScheduledTickets[ticketToStop.TicketID] != null)
                            {
                                this._runningScheduledTickets[ticketToStop.TicketID].Dispose();
                                GC.SuppressFinalize(this._runningScheduledTickets[ticketToStop.TicketID]);
                                GC.Collect();
                            }

                            this._runningScheduledTickets.Remove(ticketToStop.TicketID);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void populateTickets()
        {
            this.tMTicketBindingSource.DataSource = AppStartUp.Tickets;
            if (this.AppStartUp.Tickets != null)
            {
                if (this.AppStartUp.Tickets.Count > 0)
                {
                    rbRunTicket.Enabled = true;
                }
                else
                {
                    rbRunTicket.Enabled = false;
                }
            }
            else
            {
                rbRunTicket.Enabled = false;
            }
        }

        public void loadGroups()
        {
            AppStartUp.LoadGroups();
        }

        public void populateGroups()
        {
            throw new Exception("Not allowed in Droptick");
        }

        public void loadProxies()
        {
            AppStartUp.LoadProxies();
        }

        public void loadAccounts()
        {
            AppStartUp.LoadAccounts();
        }

        public void loadEmails()
        {
            AppStartUp.LoadEmails();
        }

        public void loadDeliveryOptions()
        {
            AppStartUp.LoadDeliveryOptions();
        }

        public void loadAutoCaptchaServices()
        {
            AppStartUp.LoadAutoCaptchaServices();
        }

        public void populateRecentTickets()
        {
            throw new Exception("Not allowed in Droptick");
        }

        private void rbRunTicket_Click(object sender, EventArgs e)
        {

            if (this._ifMonitoring)
            {
                this._ifMonitoring = false;
                this.rbRunTicket.LargeImage = global::Automatick.Properties.Resources._1348034999_continue;
                this.rbRunTicket.Text = "Monitor";

                this.ribbonTabSearches.Visible = false;

                this.rgForceStartAndStop.Visible = false;
                this.btnForceRun.Visible = false;
                this.btnForceStop.Visible = false;
                //

                DateTime dt = DateTime.Now;
                for (int i = 0; i < this.AppStartUp.Tickets.Count; i++)
                {
                    changeTicketStatus(this.AppStartUp.Tickets[i], "Stopping please wait...");
                    stopScheduledTicket(this.AppStartUp.Tickets[i]);
                    unsetScheduledTimer(this.AppStartUp.Tickets[i]);
                }

                //
            }
            else
            {
                this._ifMonitoring = true;
                this.rbRunTicket.LargeImage = global::Automatick.Properties.Resources._1348035001_stop;
                this.rbRunTicket.Text = "Stop";

                this.ribbonTabSearches.Visible = true;
                //

                this._pendingScheduledTickets.Clear();
                this._runningScheduledTickets.Clear();
                this._allSearches.Clear();

                DateTime dt = DateTime.Now.AddSeconds(1);
                for (int i = 0; i < this.AppStartUp.Tickets.Count; i++)
                {
                    scheduleTicket(this.AppStartUp.Tickets[i], dt);
                    if (this.AppStartUp.Tickets[i].ifRun)
                    {
                        dt = dt.AddMilliseconds(50);
                    }
                }

                //

                gvEvents_SelectionChanged(gvEvents, null);
            }
        }

        private void extendScheduledTicket(AXSTicket ticket, int timeInSeconds)
        {
            if (ticket.ifRun && this._ifMonitoring)
            {
                DateTime dt = DateTime.Now.AddSeconds(timeInSeconds);
                dt = calculateNextScheduleTime(dt);

                ticket.ScheduleDateTime = dt;
                ticket.ifSchedule = true;

                setScheduledTimer(ticket);
            }
            else
            {
                ticket.ifSchedule = false;

                unsetScheduledTimer(ticket);

                changeTicketStatus(ticket, "Not scheduled");
            }

            ticket.SaveTicket();
        }

        private void reScheduleTicket(AXSTicket ticket)
        {
            if (ticket.ifRun && this._ifMonitoring)
            {
                DateTime dt = DateTime.Now.AddMinutes((int)this.AppStartUp.GlobalSetting.RescheduleDelay).AddSeconds(10);
                IEnumerable<AXSTicket> filterTickets = this.AppStartUp.Tickets.Where(p => p.ifSchedule && p.ifRun && !p.isRunning);
                DateTime maxScheduleTime = dt;

                if (filterTickets != null)
                {
                    if (filterTickets.Count() > 0)
                    {
                        maxScheduleTime = filterTickets.Max(p => p.ScheduleDateTime);
                        maxScheduleTime = maxScheduleTime.AddSeconds(10);
                    }
                }

                if (dt < maxScheduleTime && this.AppStartUp.GlobalSetting.MaxConcurrentEventsToRun > 0)
                {
                    dt = calculateNextScheduleTime(maxScheduleTime);
                }
                else
                {
                    dt = calculateNextScheduleTime(dt);
                }

                ticket.ScheduleDateTime = dt;
                ticket.ifSchedule = true;

                setScheduledTimer(ticket);
            }
            else
            {
                ticket.ifSchedule = false;

                unsetScheduledTimer(ticket);
            }

            ticket.SaveTicket();
        }

        private void scheduleTicket(AXSTicket ticket, DateTime dt)
        {
            if (ticket.ifRun && this._ifMonitoring)
            {
                dt = calculateNextScheduleTime(dt);
                ticket.ScheduleDateTime = dt;
                ticket.ifSchedule = true;

                setScheduledTimer(ticket);

                //changeTicketStatus(ticket, "Scheduled");
                String strStatus = "Scheduled at " + ticket.ScheduleDateTime.ToLongTimeString();
                if (ticket.ScheduleDateTime.Date != DateTime.Now.Date)
                {
                    strStatus = strStatus + " on " + ticket.ScheduleDateTime.ToLongDateString();
                }
                changeTicketStatus(ticket, strStatus);
            }
            else
            {
                ticket.ifSchedule = false;

                unsetScheduledTimer(ticket);

                changeTicketStatus(ticket, "Not scheduled");
            }

            ticket.SaveTicket();
        }

        private void unsetScheduledTimer(AXSTicket ticket)
        {
            if (this._pendingScheduledTickets.ContainsKey(ticket.TicketID))
            {
                lock (this._pendingScheduledTickets)
                {
                    if (this._pendingScheduledTickets[ticket.TicketID] != null)
                    {
                        this._pendingScheduledTickets[ticket.TicketID].Dispose();
                        GC.SuppressFinalize(this._pendingScheduledTickets[ticket.TicketID]);
                        //GC.Collect();
                    }

                    this._pendingScheduledTickets.Remove(ticket.TicketID);
                }
            }
            changeTicketStatus(ticket, "");
        }

        private void setScheduledTimer(AXSTicket ticket)
        {
            DateTime currentTime = DateTime.Now;

            TimeSpan ts = (TimeSpan)(ticket.ScheduleDateTime - currentTime);
            System.Threading.TimerCallback tcb = this.runScheduledTicket;
            System.Threading.Timer scheduleTimer = new System.Threading.Timer(tcb, ticket, ts, new TimeSpan(-1));

            lock (this._pendingScheduledTickets)
            {
                if (this._pendingScheduledTickets.ContainsKey(ticket.TicketID))
                {
                    this._pendingScheduledTickets[ticket.TicketID].Dispose();
                    GC.SuppressFinalize(this._pendingScheduledTickets[ticket.TicketID]);
                    //GC.Collect();

                    this._pendingScheduledTickets[ticket.TicketID] = scheduleTimer;
                }
                else
                {
                    this._pendingScheduledTickets.Add(ticket.TicketID, scheduleTimer);
                }
            }
        }

        private DateTime calculateNextScheduleTime(DateTime dt)
        {
            Boolean ifAllowedToday = false;
            if (this.AppStartUp.GlobalSetting.ifRunOnMonday && dt.DayOfWeek == DayOfWeek.Monday)
            {
                ifAllowedToday = true;
            }
            else if (this.AppStartUp.GlobalSetting.ifRunOnTuesday && dt.DayOfWeek == DayOfWeek.Tuesday)
            {
                ifAllowedToday = true;
            }
            else if (this.AppStartUp.GlobalSetting.ifRunOnWednesday && dt.DayOfWeek == DayOfWeek.Wednesday)
            {
                ifAllowedToday = true;
            }
            else if (this.AppStartUp.GlobalSetting.ifRunOnThursday && dt.DayOfWeek == DayOfWeek.Thursday)
            {
                ifAllowedToday = true;
            }
            else if (this.AppStartUp.GlobalSetting.ifRunOnFriday && dt.DayOfWeek == DayOfWeek.Friday)
            {
                ifAllowedToday = true;
            }
            else if (this.AppStartUp.GlobalSetting.ifRunOnSaturday && dt.DayOfWeek == DayOfWeek.Saturday)
            {
                ifAllowedToday = true;
            }
            else if (this.AppStartUp.GlobalSetting.ifRunOnSunday && dt.DayOfWeek == DayOfWeek.Sunday)
            {
                ifAllowedToday = true;
            }

            if (ifAllowedToday && dt.TimeOfDay >= this.AppStartUp.GlobalSetting.MonitoringTimeFrom && dt.TimeOfDay <= this.AppStartUp.GlobalSetting.MonitoringTimeFrom.Add(new TimeSpan((int)this.AppStartUp.GlobalSetting.MonitoringHours, 0, 0)))
            {
                dt = dt.AddSeconds(2);
            }
            else if (ifAllowedToday && dt.TimeOfDay < this.AppStartUp.GlobalSetting.MonitoringTimeFrom)
            {
                dt = dt.AddTicks(this.AppStartUp.GlobalSetting.MonitoringTimeFrom.Ticks - dt.TimeOfDay.Ticks).AddSeconds(2);
            }
            else if (!ifAllowedToday || dt.TimeOfDay > this.AppStartUp.GlobalSetting.MonitoringTimeFrom.Add(new TimeSpan((int)this.AppStartUp.GlobalSetting.MonitoringHours, 0, 0)))
            {
                for (int i = 1; i <= 7; i++)
                {
                    if (this.AppStartUp.GlobalSetting.ifRunOnMonday && dt.AddDays(i).DayOfWeek == DayOfWeek.Monday)
                    {
                        dt = dt.AddDays(i);
                        break;
                    }
                    else if (this.AppStartUp.GlobalSetting.ifRunOnTuesday && dt.AddDays(i).DayOfWeek == DayOfWeek.Tuesday)
                    {
                        dt = dt.AddDays(i);
                        break;
                    }
                    else if (this.AppStartUp.GlobalSetting.ifRunOnWednesday && dt.AddDays(i).DayOfWeek == DayOfWeek.Wednesday)
                    {
                        dt = dt.AddDays(i);
                        break;
                    }
                    else if (this.AppStartUp.GlobalSetting.ifRunOnThursday && dt.AddDays(i).DayOfWeek == DayOfWeek.Thursday)
                    {
                        dt = dt.AddDays(i);
                        break;
                    }
                    else if (this.AppStartUp.GlobalSetting.ifRunOnFriday && dt.AddDays(i).DayOfWeek == DayOfWeek.Friday)
                    {
                        dt = dt.AddDays(i);
                        break;
                    }
                    else if (this.AppStartUp.GlobalSetting.ifRunOnSaturday && dt.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
                    {
                        dt = dt.AddDays(i);
                        break;
                    }
                    else if (this.AppStartUp.GlobalSetting.ifRunOnSunday && dt.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                    {
                        dt = dt.AddDays(i);
                        break;
                    }
                }

                dt = dt.AddTicks(this.AppStartUp.GlobalSetting.MonitoringTimeFrom.Ticks - dt.TimeOfDay.Ticks).AddSeconds(2);
            }
            return dt;
        }

        private void rbAddTicket_Click(object sender, EventArgs e)
        {
            frmTicket frm = new frmTicket(this);
            frm.ShowDialog();

            loadTicket(frm.Ticket);
        }

        private void rbEditTicket_Click(object sender, EventArgs e)
        {
            if (gvEvents.SelectedRows != null)
            {
                if (gvEvents.SelectedRows.Count > 0)
                {
                    if (gvEvents.SelectedRows[0].DataBoundItem != null)
                    {
                        AXSTicket ticketToEdit = ((AXSTicket)gvEvents.SelectedRows[0].DataBoundItem);

                        frmTicket frm = new frmTicket(this, ticketToEdit);
                        frm.ShowDialog();

                        loadTicket(ticketToEdit);
                    }
                }
            }

            if (this.AppStartUp.Tickets != null)
            {
                if (this.AppStartUp.Tickets.Count > 0)
                {
                    rbRunTicket.Enabled = true;
                }
                else
                {
                    rbRunTicket.Enabled = false;
                }
            }
            else
            {
                rbRunTicket.Enabled = false;
            }
        }

        private void loadTicket(ITicket ticket)
        {
            if (ticket != null)
            {
                if (!String.IsNullOrEmpty(ticket.TicketName))
                {
                    AXSTicket ticketFromFile = this.AppStartUp.loadTicketByName(ticket.MakeValidFileName(ticket.TicketName));
                    if (ticketFromFile != null)
                    {
                        stopScheduledTicket(ticket);
                        unsetScheduledTimer((AXSTicket)ticket);

                        int index = this.AppStartUp.Tickets.IndexOf((AXSTicket)ticket);

                        if (index > -1)
                        {
                            this.AppStartUp.Tickets[index] = ticketFromFile;
                        }
                        else
                        {
                            this.AppStartUp.Tickets.Add(ticketFromFile);
                        }

                        this.tMTicketBindingSource.ResetBindings(true);


                        if (this._ifMonitoring)
                        {
                            index = this.AppStartUp.Tickets.IndexOf(ticketFromFile);
                            if (index > -1)
                            {
                                ticketFromFile = this.AppStartUp.Tickets[index];
                                if (ticketFromFile.isRunning && ticketFromFile.ifRun)
                                {
                                    ticketFromFile.stop();
                                    ticketFromFile.ifSchedule = true;
                                    runScheduledTicket(ticketFromFile);
                                }
                                else
                                {
                                    ticketFromFile.stop();
                                    scheduleTicket(ticketFromFile, DateTime.Now.AddSeconds(2));
                                }
                            }
                        }
                    }
                }
            }

            if (this.AppStartUp.Tickets != null)
            {
                if (this.AppStartUp.Tickets.Count > 0)
                {
                    rbRunTicket.Enabled = true;
                }
                else
                {
                    rbRunTicket.Enabled = false;
                }
            }
            else
            {
                rbRunTicket.Enabled = false;
            }
        }

        private void rbDeleteTicket_Click(object sender, EventArgs e)
        {
            if (gvEvents.SelectedRows != null)
            {
                if (gvEvents.SelectedRows.Count > 0)
                {
                    if (gvEvents.SelectedRows[0].DataBoundItem != null)
                    {
                        ITicket ticketToDelete = ((ITicket)gvEvents.SelectedRows[0].DataBoundItem);

                        if (MessageBox.Show("Do you really want to delete \"" + ticketToDelete.TicketName + "\"?", "Are you Sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (ticketToDelete.isRunning)
                            {
                                foreach (AXSSearch search in ticketToDelete.Searches)
                                {
                                    if (this._allSearches.Contains(search))
                                    {
                                        this._allSearches.Remove(search);
                                    }
                                }
                                ticketToDelete.stop();
                            }

                            this.AppStartUp.Tickets.Remove((AXSTicket)ticketToDelete);
                            ticketToDelete.DeleteTicket();
                            this.populateTickets();
                            // unschedule ticket on delete ticket
                            lock (this._pendingScheduledTickets)
                            {
                                if (this._pendingScheduledTickets.ContainsKey(ticketToDelete.TicketID))
                                {
                                    if (this._pendingScheduledTickets[ticketToDelete.TicketID] != null)
                                    {
                                        this._pendingScheduledTickets[ticketToDelete.TicketID].Dispose();
                                        GC.SuppressFinalize(this._pendingScheduledTickets[ticketToDelete.TicketID]);
                                    }

                                    this._pendingScheduledTickets.Remove(ticketToDelete.TicketID);
                                }
                            }

                            lock (this._runningScheduledTickets)
                            {
                                if (this._runningScheduledTickets.ContainsKey(ticketToDelete.TicketID))
                                {
                                    if (this._runningScheduledTickets[ticketToDelete.TicketID] != null)
                                    {
                                        this._runningScheduledTickets[ticketToDelete.TicketID].Dispose();
                                        GC.SuppressFinalize(this._runningScheduledTickets[ticketToDelete.TicketID]);
                                        GC.Collect();
                                    }

                                    this._runningScheduledTickets.Remove(ticketToDelete.TicketID);
                                }
                            }
                        }
                    }
                }
            }

            if (this.AppStartUp.Tickets != null)
            {
                if (this.AppStartUp.Tickets.Count > 0)
                {
                    rbRunTicket.Enabled = true;
                }
                else
                {
                    rbRunTicket.Enabled = false;
                }
            }
            else
            {
                rbRunTicket.Enabled = false;
            }
        }

        private void changeTicketStatus(ITicket ticket, String status)
        {
            try
            {
                if (ticket != null)
                {
                    lock (this.tMTicketBindingSource)
                    {
                        ticket.TicketStatus = status;
                        int index = this.tMTicketBindingSource.IndexOf(ticket);
                        if (index > -1)
                        {
                            this.tMTicketBindingSource.ResetItem(index);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        public void onStartSearchingHandler(ITicket ticket)
        {
            try
            {
                if (ticket != null)
                {
                    if (ticket.Searches != null)
                    {
                        if (ticket.Searches.Count > 0)
                        {
                            if (ticket.Searches.Count == 1)
                            {
                                changeTicketStatus(ticket, "Searching for tickets");
                            }
                            else if (ticket.Searches.Count > 1)
                            {
                                int foundCount = ticket.Searches.Count(p => p.IfFound);
                                if (foundCount < ticket.Searches.Count)
                                {
                                    changeTicketStatus(ticket, String.Format("Searching {0} of {1} tickets", ticket.Searches.Count - foundCount, ticket.Searches.Count));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void onFoundHandler(ITicket ticket)
        {
            try
            {
                if (ticket != null)
                {
                    if (ticket.Searches != null)
                    {
                        if (ticket.Searches.Count > 0)
                        {
                            if (this.AppStartUp.GlobalSetting.ifFoundExtendNumberOfSearches)
                            {
                                decimal lastNumberOfSearches = ticket.NoOfSearches;

                                ticket.StartSolvingFromCaptcha = 1;
                                ticket.StartUsingProxiesFrom = 1;
                                ticket.ifUseProxies = true;
                                ticket.ifAutoCaptcha = this.AppStartUp.GlobalSetting.ifAutoCaptcha;
                                ticket.ifBPCAutoCaptcha = this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha;
                                ticket.ifDBCAutoCaptcha = this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha;
                                ticket.ifRDAutoCaptcha = this.AppStartUp.GlobalSetting.ifRDAutoCaptcha;
                                ticket.ifRDCAutoCaptcha = this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha;
                                ticket.ifAntigateAutoCaptcha = this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha;
                                ticket.ifCPTAutoCaptcha = this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha;
                                ticket.ifDCAutoCaptcha = this.AppStartUp.GlobalSetting.ifDCAutoCaptcha;
                                ticket.ifCAutoCaptcha = this.AppStartUp.GlobalSetting.ifCAutoCaptcha;
                                ticket.ifAC1AutoCaptcha = this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha;
                                ticket.ifOCR = this.AppStartUp.GlobalSetting.ifOCR;
                                ticket.ifROCR = this.AppStartUp.GlobalSetting.ifROCR;
                                ticket.ifBoloOCR = this.AppStartUp.GlobalSetting.ifBoloOCR;
                                ticket.ifSOCR = this.AppStartUp.GlobalSetting.ifSOCR;
                                ticket.if2C = this.AppStartUp.GlobalSetting.if2C;
                                ticket.ifSendEmail = this.AppStartUp.GlobalSetting.ifSendEmail;
                                ticket.ifPlaySoundAlert = this.AppStartUp.GlobalSetting.ifPlaySoundAlert;

                                ticket.NoOfSearches = this.AppStartUp.GlobalSetting.ExtendSearchesTo;

                                ticket.changeSettingsDuringSearching(lastNumberOfSearches);

                                if (ticket.NoOfSearches != lastNumberOfSearches)
                                {
                                    ticket.Searches.changeDelegate = null;
                                    if (ticket.Searches != null)
                                    {
                                        ticket.Searches.changeDelegate = new SortedBindingList.SortableBindingList<ITicketSearch>.dlgOnChange(this.changeDelegateHandler);
                                        foreach (AXSSearch search in ticket.Searches)
                                        {
                                            if (!this._allSearches.Contains(search))
                                            {
                                                this._allSearches.Add(search);
                                            }
                                            //if (this.InvokeRequired)
                                            //{
                                            //    this.Invoke(new MethodInvoker(delegate
                                            //                                        {
                                            //                                            this.tMSearchBindingSource.ResetBindings(false);
                                            //                                        }));
                                            //}
                                            //else
                                            //{
                                            //    this.tMSearchBindingSource.ResetBindings(false);
                                            //}
                                        }
                                    }

                                }

                                if (ticket.Searches.Count == 1)
                                {
                                    changeTicketStatus(ticket, "Ticket found!");
                                }
                                else if (ticket.Searches.Count > 1)
                                {
                                    int foundCount = ticket.Searches.Count(p => p.IfFound);
                                    changeTicketStatus(ticket, String.Format("{0} of {1} tickets found!", foundCount, ticket.Searches.Count));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void onNotFoundHandler(ITicket ticket)
        {
            try
            {
                if (!ticket.ifContinousRun)
                {
                    stopScheduledTicket(ticket);
                }
            }
            catch (Exception)
            {

            }
        }

        public void onChangeHandlerStartOrStop(ITicket ticket)
        {
            try
            {
                if (ticket != null)
                {
                    if (ticket.isRunning)
                    {
                        changeTicketStatus(ticket, "Searching for tickets");
                    }
                    else if (ticket.ifRun && ticket.ifSchedule)
                    {
                        String strStatus = "Scheduled at " + ticket.ScheduleDateTime.ToLongTimeString();
                        if (ticket.ScheduleDateTime.Date != DateTime.Now.Date)
                        {
                            strStatus = strStatus + " on " + ticket.ScheduleDateTime.ToLongDateString();
                        }
                        changeTicketStatus(ticket, strStatus);
                    }
                    else
                    {
                        changeTicketStatus(ticket, "");
                    }
                    gvEvents_SelectionChanged(gvEvents, null);
                }
            }
            catch (Exception)
            {

            }
        }

        void changeDelegateHandler(ITicketSearch search)
        {
            try
            {
                if (search != null)
                {
                    int index = this.tMSearchBindingSource.IndexOf(search);
                    if (index > -1)
                    {
                        this.tMSearchBindingSource.ResetItem(index);
                    }
                    try
                    {
                        DataGridViewRow row = gvFindTickets.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.DataBoundItem == search);
                        if (row != null)
                        {
                            if (search.Status == TicketSearchStatus.RetryingStatus && (search.MoreInfo.Contains("Sold out") || search.MoreInfo.Contains(TicketSearchStatus.MoreInfoSiteUnavailable) || search.MoreInfo.Contains("Ticket not found!")))
                            {
                                row.DefaultCellStyle.BackColor = System.Drawing.Color.OrangeRed;
                                row.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
                            }
                            else if (search.IfFound)
                            {
                                row.DefaultCellStyle.BackColor = System.Drawing.Color.PaleGreen;
                                row.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
                            }
                            else
                            {
                                row.DefaultCellStyle.BackColor = Color.White; //System.Drawing.SystemColors.Window;
                                row.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch { }
        }

        private void rbEmail_Click(object sender, EventArgs e)
        {
            frmEmail frm = new frmEmail(this);
            frm.ShowDialog();
        }

        private void rbDeliveryOptions_Click(object sender, EventArgs e)
        {
            frmDeliveryOption frm = new frmDeliveryOption(this);
            frm.ShowDialog();
        }

        private void filterAndPopulateTickets()
        {
            throw new Exception("Not allowed in Droptick");
        }

        private void rcbFilter_KeyUp(object sender, KeyEventArgs e)
        {
            filterAndPopulateTickets();
        }

        private void rbAutoCaptcha_Click(object sender, EventArgs e)
        {
            frmAutoCaptchaService frm = new frmAutoCaptchaService(this);
            frm.ShowDialog();
        }

        private void rbAccounts_Click(object sender, EventArgs e)
        {
            frmAccount frm = new frmAccount(this);
            frm.ShowDialog();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                try
                {
                    this.IsRunning = false;

                    if (mre != null)
                    {
                        mre.Set();
                        mre.Close();
                    }
                }
                catch (Exception)
                {

                }

                if (this.AppStartUp != null)
                {
                    if (this.AppStartUp.Tickets != null)
                    {
                        for (int i = 0; i < this.AppStartUp.Tickets.Count; i++)
                        {
                            if (this.AppStartUp.Tickets[i].isRunning)
                            {
                                this.AppStartUp.Tickets[i].stop();
                                this.AppStartUp.Tickets[i].SaveTicket();
                            }
                        }
                    }
                    this.AppStartUp.SaveGlobalSettings();
                    try
                    {
                        LotIDPicker.LotIDInstance.closeClient();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }
            }
            catch (Exception)
            {

            }

            //ServerPortsPicker.ServerPortsPickerInstance.DisposeTimerForJWTToken();
        }

        private void rbProxies_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.AppStartUp.Proxies != null && ProxyPicker.ProxyPickerInstance.LicPPM != null)
                {
                    if (this.AppStartUp.Proxies.Count > 0 && ProxyPicker.ProxyPickerInstance.LicPPM.IsValidated)
                    {
                        int cntVerifyingProxies = this.AppStartUp.Proxies.Where(p => p.ProxyStatus == Proxy.proxyVerifying || p.ProxyStatus == Proxy.proxyWaiting).Count();
                        if (cntVerifyingProxies > 0)
                        {
                            if (MessageBox.Show("Proxy Manager is currently verifying the proxies in background." + Environment.NewLine + "Please try again later." + Environment.NewLine + "Remaining proxies for verfication = " + cntVerifyingProxies.ToString(), "Proxy Manager is busy!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information) == DialogResult.Retry)
                            {
                                rbProxies_Click(sender, e);
                            }
                            return;
                        }
                    }
                }

                frmProxies frm = new frmProxies(this);
                frm.ShowDialog();
            }
            catch (Exception)
            {
            }
        }

        private void rbRegisterPM_Click(object sender, EventArgs e)
        {
            ProxyPicker.ProxyPickerInstance.LicPPM.RegisterPPM();
            ProxyPicker.ProxyPickerInstance.ValidatePPM(true);
        }

        private void chkEnableProxyManager_CheckedChanged(object sender, EventArgs e)
        {
            enableProxyManager();
        }

        public void enableProxyManager()
        {
            try
            {
                if (chkEnableProxyManager.Checked)
                {
                    chkEnableProxyManager.Text = "Disable Proxy Manager";
                    this.AppStartUp.GlobalSetting.IfEnablePM = true;
                    chkEnableISPProxyUsage.Enabled = true;
                    ProxyPicker.ProxyPickerInstance.ProxyManager.Load();
                }
                else
                {
                    chkEnableProxyManager.Text = "Enable Proxy Manager";
                    this.AppStartUp.GlobalSetting.IfEnablePM = false;
                    chkEnableISPProxyUsage.Checked = false;
                    chkEnableISPProxyUsage.Enabled = false;
                    ProxyPicker.ProxyPickerInstance.ProxyManager.Unload();
                }

            }
            catch { }
            finally { this.AppStartUp.SaveGlobalSettings(); }
        }

        private void chkEnableISPProxyUsage_CheckedChanged(object sender, EventArgs e)
        {
            enableISPProxyUsage();
        }

        private void chkEnableLuminatiProxies_CheckedChanged(object sender, EventArgs e)
        {
            enableSpecialProxies();
        }

        public void enableSpecialProxies()
        {
            try
            {
                if (this.chkEnableLuminatiProxies.Checked)
                {
                    //  this.cmbProxiesCountry.Enabled = true;
                    this.AppStartUp.GlobalSetting.IfEnableSpecialProxies = true;
                    ProxyPicker.ProxyPickerInstance.ProxyManager.Load();
                }
                else
                {
                    // this.cmbProxiesCountry.Enabled = false;
                    this.AppStartUp.GlobalSetting.IfEnableSpecialProxies = false;
                    ProxyPicker.ProxyPickerInstance.ProxyManager.Unload();

                }

            }
            catch { }
            finally { this.AppStartUp.SaveGlobalSettings(); }
        }

        public void enableISPProxyUsage()
        {
            try
            {
                if (chkEnableISPProxyUsage.Checked)
                {
                    chkEnableISPProxyUsage.Text = "Disable ISP IP usage";
                    this.AppStartUp.GlobalSetting.ifEnableISPIPUseage = true;
                    Proxy p = new Proxy();
                    p.Address = "ISP";
                    p.Port = "ISPPORT";
                    p.TheProxyType = Proxy.ProxyType.ISPIP;
                    p.ProxyStatus = Proxy.proxyWaiting;
                    if (this.AppStartUp.Proxies != null)
                    {
                        int cnt = this.AppStartUp.Proxies.Where(pr => pr.Address == "ISP" && pr.Port == "ISPPORT").Count();
                        if (cnt <= 0)
                        {
                            this.AppStartUp.Proxies.Add(p);
                            Thread th = new Thread(new ParameterizedThreadStart(ProxyPicker.ProxyPickerInstance.ProxyManager.verifyProxies));
                            th.IsBackground = true;
                            th.SetApartmentState(ApartmentState.STA);
                            th.Priority = ThreadPriority.Highest;
                            th.Start(p);
                        }
                    }

                }
                else
                {
                    chkEnableISPProxyUsage.Text = "Enable ISP IP usage";
                    this.AppStartUp.GlobalSetting.ifEnableISPIPUseage = false;

                    Proxy p = this.AppStartUp.Proxies.First(q => q.TheProxyType == Proxy.ProxyType.ISPIP);
                    if (p != null)
                    {
                        this.AppStartUp.Proxies.Remove(p);
                    }
                }
            }
            catch { }
            finally
            {
                this.AppStartUp.SaveGlobalSettings();
            }
        }

        private void timerValidateLicense_Tick(object sender, EventArgs e)
        {
            try
            {
                LicenseCore lic = new LicenseCore("DAX", Application.StartupPath + @"\lic.lic", false);

                if (!lic.ValidateLicense())
                {
                    //AuthtokenPicker.AuthtokenPickerInstance.Validated();
                    this.Close();
                }
                else
                {
                    //AuthtokenPicker.AuthtokenPickerInstance.ReInitialize(lic);
                }
                //if (!this.AppStartUp.GlobalSetting.IfUseSpecialProxies)
                {
                    ProxyPicker.ProxyPickerInstance.ValidatePPM();
                }
                // ProxyPicker.ProxyPickerInstance.ValidatePPM();

            }
            catch (Exception)
            {
                try
                {
                    Process p = Process.GetCurrentProcess();
                    if (p != null)
                    {
                        p.Kill();
                    }
                }
                catch (Exception)
                {

                }

            }
        }

        private void rbShowHideBar_Click(object sender, EventArgs e)
        {
            if (ribbonMain.Minimized)
            {
                ribbonMain.Minimized = false;
                this.rbShowHideBar.SmallImage = global::Automatick.Properties.Resources._1354172637_navigate_up;
                this.rbShowHideBar.ToolTip = "Minimize bar";
            }
            else
            {
                ribbonMain.Minimized = true;
                this.rbShowHideBar.SmallImage = global::Automatick.Properties.Resources._1354172644_navigate_down;
                this.rbShowHideBar.ToolTip = "Maximize bar";
            }
        }

        private void gvEvents_SelectionChanged(object sender, EventArgs e)
        {
            if (gvEvents.SelectedRows != null)
            {
                if (gvEvents.SelectedRows.Count > 0)
                {
                    rbEditTicket.Enabled = true;
                    rbDeleteTicket.Enabled = true;
                    if (gvEvents.SelectedRows[0].DataBoundItem != null)
                    {
                        ITicket ticketToCheck = ((ITicket)gvEvents.SelectedRows[0].DataBoundItem);
                        if (ticketToCheck.isRunning && this._ifMonitoring)
                        {
                            this.rgForceStartAndStop.Visible = true;
                            this.btnForceRun.Visible = false;
                            this.btnForceStop.Visible = true;
                        }
                        else if (!ticketToCheck.isRunning && this._ifMonitoring)
                        {
                            this.rgForceStartAndStop.Visible = true;
                            this.btnForceRun.Visible = true;
                            this.btnForceStop.Visible = false;
                        }
                        else
                        {
                            this.rgForceStartAndStop.Visible = false;
                            this.btnForceRun.Visible = false;
                            this.btnForceStop.Visible = false;
                        }
                    }
                }
                else
                {
                    this.rgForceStartAndStop.Visible = false;
                    this.btnForceRun.Visible = false;
                    this.btnForceStop.Visible = false;
                }
            }
            else
            {
                this.rgForceStartAndStop.Visible = false;
                this.btnForceRun.Visible = false;
                this.btnForceStop.Visible = false;
            }
        }

        private void gvEvents_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (gvEvents.CurrentCell.ValueType == typeof(decimal))
                {
                    decimal value;
                    if (!decimal.TryParse(e.FormattedValue.ToString(), out value))
                    {
                        gvEvents.CurrentCell.Value = (decimal)1;
                    }
                    else if (gvEvents.CurrentCell.OwningColumn.Name == "ScheduleRunningTime" && value <= 0)
                    {
                        gvEvents.CurrentCell.Value = (decimal)1;
                    }
                    else if (gvEvents.CurrentCell.OwningColumn.Name == "TimeInterval" && value <= 0)
                    {
                        gvEvents.CurrentCell.Value = (decimal)1;
                    }
                }
            }
        }

        private void gvEvents_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }
        }

        private void gvEvents_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (gvEvents.CurrentCell != null)
                {
                    if (gvEvents.CurrentCell.OwningColumn.Name == "ifRunDataGridViewCheckBoxColumn" || gvEvents.CurrentCell.OwningColumn.Name == "ifContinousRunDataGridViewCheckBoxColumn" || gvEvents.CurrentCell.OwningColumn.Name == "ScheduleRunningTime")
                    {
                        AXSTicket ticketToSave = ((AXSTicket)gvEvents.CurrentCell.OwningRow.DataBoundItem);

                        if (this._ifMonitoring)
                        {
                            if (!ticketToSave.ifRun)
                            {
                                stopScheduledTicket(ticketToSave);
                            }

                            scheduleTicket(ticketToSave, DateTime.Now.AddSeconds(2));
                        }

                        ticketToSave.SaveTicket();
                    }
                }
            }
            catch
            {

            }
        }

        private void rbApplyToAllEvents_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.AppStartUp.Tickets.Count; i++)
                {
                    this.AppStartUp.Tickets[i].ifRun = chkRun.Checked;
                    this.AppStartUp.Tickets[i].ifRandomDelay = chkRandomDelay.Checked;
                    this.AppStartUp.Tickets[i].ResetSearchDelay = nudSearchDelay.Value;
                    this.AppStartUp.Tickets[i].ifContinousRun = chkContinuously.Checked;
                    this.AppStartUp.Tickets[i].ScheduleRunningTime = nudRunningTime.Value;
                    this.AppStartUp.Tickets[i].SaveTicket();
                }
                this.tMTicketBindingSource.ResetBindings(true);
            }
            catch (Exception)
            {

            }
        }

        private void chkAutoCaptcha_CheckedChanged(object sender, EventArgs e)
        {
            autoCaptchaCheckedChanged();
        }

        private void autoCaptchaCheckedChanged()
        {
            if (chkAutoCaptcha.Checked)
            {
                cbCaptchaService.Enabled = true;
            }
            else
            {
                cbCaptchaService.Enabled = false;
            }
        }

        private void chkExtendSearches_CheckedChanged(object sender, EventArgs e)
        {
            extendSeachesCheckedChanged();
        }

        private void extendSeachesCheckedChanged()
        {
            if (chkExtendSearches.Checked)
            {
                nudExtendSearchesUpto.Enabled = true;
            }
            else
            {
                nudExtendSearchesUpto.Enabled = false;
            }
        }

        private void rbSave_Click(object sender, EventArgs e)
        {
            saveMonitoringOptions();
        }

        private void saveMonitoringOptions()
        {
            try
            {
                if (AppStartUp.GlobalSetting != null)
                {
                    if (!(this.chkMonday.Checked || this.chkTuesday.Checked || this.chkWednesday.Checked || this.chkThursday.Checked || this.chkFriday.Checked || this.chkSaturday.Checked || this.chkSunday.Checked))
                    {
                        MessageBox.Show("Please select atlease one day for monitoring.");
                        return;
                    }
                    this.AppStartUp.GlobalSetting.ifRunOnMonday = this.chkMonday.Checked;
                    this.AppStartUp.GlobalSetting.ifRunOnTuesday = this.chkTuesday.Checked;
                    this.AppStartUp.GlobalSetting.ifRunOnWednesday = this.chkWednesday.Checked;
                    this.AppStartUp.GlobalSetting.ifRunOnThursday = this.chkThursday.Checked;
                    this.AppStartUp.GlobalSetting.ifRunOnFriday = this.chkFriday.Checked;
                    this.AppStartUp.GlobalSetting.ifRunOnSaturday = this.chkSaturday.Checked;
                    this.AppStartUp.GlobalSetting.ifRunOnSunday = this.chkSunday.Checked;

                    this.AppStartUp.GlobalSetting.MonitoringTimeFrom = this.tMonitoringTime.Value;
                    this.AppStartUp.GlobalSetting.MonitoringHours = this.nudMonitoringHours.Value;

                    this.AppStartUp.GlobalSetting.ifAutoCaptcha = this.chkAutoCaptcha.Checked;

                    if (this.cbCaptchaService.SelectedItem != null)
                    {
                        if (this.cbCaptchaService.SelectedItem.Text == "BPC")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = true;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "RD")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = true;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "CPT")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = true;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "DBC")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = true;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "DC")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = true;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "O")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = true;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "C")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = true;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "RC")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = true;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "B")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = true;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "S")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = true;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "2C")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = true;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "CTR")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = true;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "A")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = true;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "C1")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = true;
                        }
                        else if (this.cbCaptchaService.SelectedItem.Text == "RDC")
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = true;
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifOCR = false;
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            this.AppStartUp.GlobalSetting.ifROCR = false;
                            this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            this.AppStartUp.GlobalSetting.ifSOCR = false;
                            this.AppStartUp.GlobalSetting.if2C = false;
                            this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        }
                    }
                    else
                    {
                        this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = true;
                        this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                        this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                        this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                        this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                        this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                        this.AppStartUp.GlobalSetting.ifOCR = false;
                        this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                        this.AppStartUp.GlobalSetting.ifROCR = false;
                        this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                        this.AppStartUp.GlobalSetting.ifSOCR = false;
                        this.AppStartUp.GlobalSetting.if2C = false;
                        this.AppStartUp.GlobalSetting.ifCaptchator = false;
                        this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                        this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                    }

                    this.AppStartUp.GlobalSetting.ifSendEmail = this.chkSendEmail.Checked;
                    this.AppStartUp.GlobalSetting.ifPlaySoundAlert = this.chkPlayMusic.Checked;
                    this.AppStartUp.GlobalSetting.ifFoundExtendNumberOfSearches = this.chkExtendSearches.Checked;
                    this.AppStartUp.GlobalSetting.Permission = this.AXSByPassWaitingRoom.Checked;
                    this.AppStartUp.GlobalSetting.ExtendSearchesTo = this.nudExtendSearchesUpto.Value;

                    this.AppStartUp.GlobalSetting.MaxConcurrentEventsToRun = this.nudMaxConcurrentEventsToRun.Value;
                    this.AppStartUp.GlobalSetting.RescheduleDelay = this.nudRescheduleDelay.Value;
                }

                this.AppStartUp.SaveGlobalSettings();

                if (this._ifMonitoring)
                {
                    IEnumerable<AXSTicket> filterTickets = this.AppStartUp.Tickets.Where(p => p.isRunning);
                    if (filterTickets != null)
                    {
                        if (filterTickets.Count() > 0)
                        {
                            for (int i = 0; i < filterTickets.Count(); i++)
                            {
                                AXSTicket ticket = filterTickets.ElementAt(i);
                                decimal lastNumberOfSearches = ticket.NoOfSearches;

                                ticket.StartSolvingFromCaptcha = 1;
                                ticket.StartUsingProxiesFrom = 1;
                                ticket.ifUseProxies = true;
                                ticket.ifSendEmail = this.AppStartUp.GlobalSetting.ifSendEmail;
                                ticket.ifPlaySoundAlert = this.AppStartUp.GlobalSetting.ifPlaySoundAlert;

                                ticket.ifBPCAutoCaptcha = this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha;
                                ticket.ifRDAutoCaptcha = this.AppStartUp.GlobalSetting.ifRDAutoCaptcha;
                                ticket.ifRDCAutoCaptcha = this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha;
                                ticket.ifCPTAutoCaptcha = this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha;
                                ticket.ifDBCAutoCaptcha = this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha;
                                ticket.ifDCAutoCaptcha = this.AppStartUp.GlobalSetting.ifDCAutoCaptcha;
                                ticket.ifOCR = this.AppStartUp.GlobalSetting.ifOCR;
                                ticket.ifCAutoCaptcha = this.AppStartUp.GlobalSetting.ifCAutoCaptcha;
                                ticket.ifROCR = this.AppStartUp.GlobalSetting.ifROCR;
                                ticket.ifBoloOCR = this.AppStartUp.GlobalSetting.ifBoloOCR;
                                ticket.ifSOCR = this.AppStartUp.GlobalSetting.ifSOCR;
                                ticket.if2C = this.AppStartUp.GlobalSetting.if2C;
                                ticket.ifCaptchator = this.AppStartUp.GlobalSetting.ifCaptchator;
                                ticket.ifAC1AutoCaptcha = this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha;
                                ticket.ifAntigateAutoCaptcha = this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha;

                                ticket.changeSettingsDuringSearching(lastNumberOfSearches);

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void grvFindTickets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvFindTickets.CurrentCell is DataGridViewImageCell)
            {
                DataGridViewImageCell flagCell = (DataGridViewImageCell)gvFindTickets.CurrentCell;
                ITicketSearch search = (ITicketSearch)gvFindTickets.CurrentCell.OwningRow.DataBoundItem;
                if (((Boolean)search.FlagImage.Tag))
                {
                    search.FlagImage = global::Automatick.Properties.Resources.Flag16Disable;
                    search.FlagImage.Tag = false;
                }
                else
                {
                    search.FlagImage = global::Automatick.Properties.Resources.Flag_red;
                    search.FlagImage.Tag = true;
                }

                changeDelegateHandler(search);
            }
        }

        private void ribbonMain_SelectedTabChanged(object sender, EventArgs e)
        {
            if (ribbonMain.SelectedTab == ribbonTabSearches)
            {
                gvEvents.Visible = false;
                gvFindTickets.Visible = true;
                gvFindTickets.Dock = DockStyle.Fill;
                gvFindTickets.BringToFront();
            }
            else
            {
                gvFindTickets.Visible = false;
                gvEvents.Visible = true;
                gvEvents.Dock = DockStyle.Fill;
                gvEvents.BringToFront();
            }
        }

        private void rbAutoBuy_Click(object sender, EventArgs e)
        {
            if (this.tMSearchBindingSource.Current != null)
            {
                if (this.tMSearchBindingSource.Current is ITicketSearch)
                {
                    ITicketSearch search = (ITicketSearch)this.tMSearchBindingSource.Current;
                    search.autoBuy();
                }
            }
        }

        private void rbAutoBuyGuest_Click(object sender, EventArgs e)
        {
            if (this.tMSearchBindingSource.Current != null)
            {
                if (this.tMSearchBindingSource.Current is ITicketSearch)
                {
                    ITicketSearch search = (ITicketSearch)this.tMSearchBindingSource.Current;
                    search.autoBuyGuest();
                }
            }
        }

        private void rbAutoBuyWithoutProxy_Click(object sender, EventArgs e)
        {
            if (this.tMSearchBindingSource.Current != null)
            {
                if (this.tMSearchBindingSource.Current is ITicketSearch)
                {
                    ITicketSearch search = (ITicketSearch)this.tMSearchBindingSource.Current;
                    search.autoBuyWithoutProxy();
                }
            }
        }

        private void gvEvents_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (gvEvents.CurrentCell != null && gvEvents.IsCurrentCellDirty)
                {
                    if (gvEvents.CurrentCell.OwningColumn.Name == "ifRunDataGridViewCheckBoxColumn" || gvEvents.CurrentCell.OwningColumn.Name == "ifContinousRunDataGridViewCheckBoxColumn")
                    {
                        gvEvents.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
                }
            }
            catch
            {

            }
        }

        private void btnForceRun_Click(object sender, EventArgs e)
        {
            if (this._ifMonitoring)
            {
                if (gvEvents.SelectedRows != null)
                {
                    if (gvEvents.SelectedRows.Count > 0)
                    {
                        if (gvEvents.SelectedRows[0].DataBoundItem != null)
                        {
                            AXSTicket ticket = ((AXSTicket)gvEvents.SelectedRows[0].DataBoundItem);

                            if (ticket.isRunning)
                            {
                                ticket.stop();
                            }
                            ticket.ifRun = true;
                            ticket.ifSchedule = true;
                            forceRunTicket(ticket);
                            int index = -1;
                            index = this.tMTicketBindingSource.IndexOf(ticket);
                            if (index > -1)
                            {
                                this.tMTicketBindingSource.ResetItem(index);
                            }
                        }
                    }
                }
            }
        }

        private void applyPermission()
        {
            try
            {
                Boolean ifAutoCaptchaSelectedPreviously = this.AppStartUp.GlobalSetting.ifAutoCaptcha;

                IEnumerable<AccessRights.AccessList> allControlsAccesses = this.AppPermissions.AllAccessList.Where(p => p.form == this.Name);
                foreach (AccessRights.AccessList obj in allControlsAccesses)
                {
                    if (obj.name == rbBPCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbBPCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(rbBPCAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbRDAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbRDAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(rbRDAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbRDCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbRDCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(rbRDCAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbAC1AutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbAC1AutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(rbAC1AutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbAAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbAAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(rbAAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbCPTAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbCPTAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(rbCPTAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbDBCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbDBCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(rbDBCAutoCaptcha);
                        }

                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbDCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbDCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(rbDCAutoCaptcha);
                        }

                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(rbCAutoCaptcha);
                        }

                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifCAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbOCR.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbOCR.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifOCR = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(rbOCR);
                        }

                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifOCR = false;
                            }
                        }
                    }
                    else if (obj.name == this.rbRAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        this.rbRAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifROCR = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(this.rbRAutoCaptcha);
                        }

                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifROCR = false;
                            }
                        }
                    }
                    else if (obj.name == this.rbBoloAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        this.rbBoloAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(this.rbBoloAutoCaptcha);
                        }

                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifBoloOCR = false;
                            }
                        }
                    }
                    else if (obj.name == this.rbSOCR.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        this.rbSOCR.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifSOCR = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(this.rbSOCR);
                        }

                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifSOCR = false;
                            }
                        }
                    }
                    else if (obj.name == this.rb2CAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        this.rb2CAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.if2C = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(this.rb2CAutoCaptcha);
                        }

                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.if2C = false;
                            }
                        }
                    }
                    else if (obj.name == this.rbCaptchatorAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        this.rbCaptchatorAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            if (cbCaptchaService.SelectedItem != null)
                            {
                                if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                                {
                                    this.AppStartUp.GlobalSetting.ifCaptchator = false;
                                    cbCaptchaService.SelectedItem = null;
                                }
                            }

                            cbCaptchaService.Items.Remove(rbCaptchatorAutoCaptcha);
                        }

                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.AppStartUp.GlobalSetting.ifCaptchator = false;
                            }
                        }
                    }
                    else if (obj.name == chkExtendSearches.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        chkExtendSearches.Visible = access;
                        if (!access)
                        {
                            this.AppStartUp.GlobalSetting.ifFoundExtendNumberOfSearches = false;
                            chkExtendSearches.Checked = false;
                        }
                    }
                    else if (obj.name == nudExtendSearchesUpto.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        nudExtendSearchesUpto.Visible = access;
                        if (!access)
                        {
                            this.AppStartUp.GlobalSetting.ExtendSearchesTo = 1;
                            nudExtendSearchesUpto.Value = 1;
                        }
                    }
                    else if (obj.name == AXSByPassWaitingRoom.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        AXSByPassWaitingRoom.Visible = access;
                        if (!access)
                        {
                            this.AppStartUp.GlobalSetting.Permission = false;
                            AXSByPassWaitingRoom.Checked = false;
                        }
                    }
                }
                IEnumerable<AccessRights.AccessList> allControlsAccesses1 = this.AppPermissions.AllAccessList.Where(p => p.form == "Core");
                foreach (AccessRights.AccessList obj in allControlsAccesses1)
                {


                    if (obj.name == AXSByPassWaitingRoom.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        AXSByPassWaitingRoom.Visible = access;
                        if (!access)
                        {
                            this.AppStartUp.GlobalSetting.Permission = false;
                            AXSByPassWaitingRoom.Checked = false;
                        }
                    }
                }
                AccessRights.AccessList accessObj = allControlsAccesses.FirstOrDefault(p => p.access.ToLower() == "true" && (p.name == "rbBPCAutoCaptcha" || p.name == "rbDBCAutoCaptcha" || p.name == "rbRDAutoCaptcha" || p.name == "rbRDCAutoCaptcha" || p.name == "rbAAutoCaptcha" || p.name == "rbCPTAutoCaptcha" || p.name == "rbDCAutoCaptcha" || p.name == "rbCAutoCaptcha" || p.name == "rbOCR" || p.name == "rbRAutoCaptcha"
                   || p.name == "rbBoloAutoCaptcha" || p.name == "rbSOCR" || p.name == "rb2CAutoCaptcha" || p.name == "rbCaptchatorAutoCaptcha" || p.name == "rbAC1AutoCaptcha"));

                if (accessObj != null)
                {
                    if (!this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha && !this.AppStartUp.GlobalSetting.ifRDAutoCaptcha && !this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha && !this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha && !this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha && !this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha && !this.AppStartUp.GlobalSetting.ifDCAutoCaptcha && !this.AppStartUp.GlobalSetting.ifCAutoCaptcha && !this.AppStartUp.GlobalSetting.ifROCR
                        && !this.AppStartUp.GlobalSetting.ifBoloOCR && !this.AppStartUp.GlobalSetting.ifSOCR && !this.AppStartUp.GlobalSetting.if2C && !this.AppStartUp.GlobalSetting.ifCaptchator && !this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha)
                    {
                        if (accessObj.name == rbBPCAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifBPCAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbBPCAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbRDAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifRDAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbRDAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbRDCAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifRDCAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbRDCAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbAC1AutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbAC1AutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbAAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifAntigateAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbAAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbCPTAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifCPTAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbCPTAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbDBCAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifDBCAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbDBCAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbDCAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifDCAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbDCAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbCAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifCAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbCAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbOCR.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifOCR = true;
                            cbCaptchaService.SelectedItem = rbOCR;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rbRAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifROCR = true;
                            cbCaptchaService.SelectedItem = this.rbRAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rbBoloAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifBoloOCR = true;
                            cbCaptchaService.SelectedItem = this.rbBoloAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rbSOCR.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifSOCR = true;
                            cbCaptchaService.SelectedItem = this.rbSOCR;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rb2CAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.if2C = true;
                            cbCaptchaService.SelectedItem = this.rb2CAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rbCaptchatorAutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifCaptchator = true;
                            cbCaptchaService.SelectedItem = this.rbCaptchatorAutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rbAC1AutoCaptcha.Name)
                        {
                            this.AppStartUp.GlobalSetting.ifAC1AutoCaptcha = true;
                            cbCaptchaService.SelectedItem = this.rbAC1AutoCaptcha;
                            this.AppStartUp.GlobalSetting.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                    }
                }
                else
                {
                    this.chkAutoCaptcha.Enabled = false;
                    this.chkAutoCaptcha.Checked = false;
                    this.AppStartUp.GlobalSetting.ifAutoCaptcha = false;
                }

                this.AppStartUp.SaveGlobalSettings();
            }
            catch (Exception)
            {

            }
        }
        private void btnForceStop_Click(object sender, EventArgs e)
        {
            if (this._ifMonitoring)
            {
                if (gvEvents.SelectedRows != null)
                {
                    if (gvEvents.SelectedRows.Count > 0)
                    {
                        if (gvEvents.SelectedRows[0].DataBoundItem != null)
                        {
                            AXSTicket ticket = ((AXSTicket)gvEvents.SelectedRows[0].DataBoundItem);


                            ticket.ifRun = false;
                            ticket.ifSchedule = false;
                            stopScheduledTicket(ticket);
                            unsetScheduledTimer(ticket);
                            if (ticket.isRunning)
                            {
                                ticket.stop();
                            }
                            ticket.SaveTicket();
                            int index = -1;
                            index = this.tMTicketBindingSource.IndexOf(ticket);
                            if (index > -1)
                            {
                                this.tMTicketBindingSource.ResetItem(index);
                            }
                        }
                    }
                }
            }
        }

        public void startTixTox() { }
        public void customBuildCheck() { }
        public void stopTixTox() { }

        private void rbBtnSaveProxySettings_Click(object sender, EventArgs e)
        {
            try
            {
                this.AppStartUp.GlobalSetting.Password = this.rbTxtPassword.Text.Trim();
                this.AppStartUp.GlobalSetting.Username = this.rbTxtUsername.Text.Trim();

                for (int i = this.AppStartUp.Proxies.Count - 1; i >= 0; i--)
                {
                    if (this.AppStartUp.Proxies[i].TheProxyType == Proxy.ProxyType.Relay)
                    {
                        this.AppStartUp.Proxies[i].userName = this.AppStartUp.GlobalSetting.Username;
                        this.AppStartUp.Proxies[i].Password = this.AppStartUp.GlobalSetting.Password;
                    }
                }

                this.AppStartUp.SaveProxies();
                this.AppStartUp.SaveGlobalSettings();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ribbonButton4_Click(object sender, EventArgs e)
        {
            frmReport frm = new frmReport(this);
            frm.ShowDialog();
        }
    }
}
