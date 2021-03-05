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
using Automatick.Logging;
using System.Diagnostics;
using TCPClient;
using System.Threading.Tasks;
using PushClientLibrary;
using SAPipeLibrary;
using System.Net;
using GeneralBucket;
using Newtonsoft.Json;
using System.IO;


namespace Automatick
{
    public partial class frmMain : C1RibbonForm, IMainForm
    {
        int captchatorWorker = 10;
        private String _lastSelectTicketId = String.Empty;
        private int _recentTicketsListLimit = 8;
        ProcessesWorker _processesWorker = null;
        public Dictionary<String, System.Threading.Timer> _scheduledTickets = null;
        public Dictionary<String, System.Threading.Timer> _unscheduledTickets = null;
        private ApplicationPermissions _appPermissions;
        String LICENSING_SERVER_IP = "mainserver.ticketpeers.com";

        private Boolean IsRunning = false;
        private ManualResetEvent mre = null;

        TixToxProxyListener proxyListner = null;
        private LicenseCore _lic;

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

        public frmMain(ApplicationPermissions appPermissions, LicenseCore lic)
        {
            this._appPermissions = appPermissions;
            this._lic = lic;
            InitializeComponent();
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
            try
            {
                this._scheduledTickets = new Dictionary<String, System.Threading.Timer>();
                this._unscheduledTickets = new Dictionary<String, System.Threading.Timer>();
                _processesWorker = new ProcessesWorker(this);

                CaptchaBrowserForm = new frmCaptchaBrowser();
                CaptchaBrowserForm.showForm();
                CaptchaBrowserForm.hideForm();

                CaptchaForm = new frmCaptcha();
                CaptchaForm.showForm();
                CaptchaForm.hideForm();

                AppStartUp = new ApplicationStartUp(Application.StartupPath);
                AppStartUp.LoadGlobalSettings();
                loadAutoCaptchaServices();
                loadTickets();
                loadAccounts();
                loadDeliveryOptions();
                loadEmails();
                loadGroups();
                loadProxies();
                loadlog();
                populateRecentTickets();
                ProxyPicker.createProxyPickerInstance(this);

                if (Automatick.Logging.Logger.LoggerInstance != null)
                {
                    Automatick.Logging.Logger.LoggerInstance.CreateLogFolder(Application.StartupPath);
                    Automatick.Logging.Logger.LoggerInstance.DeleteLogFile();
                }

                Captchator.createCaptchatorInstance(AppStartUp.AutoCaptchaService, captchatorWorker);
                AppPermissions.ApplyPemissions(this);

                this.setRelayProxies();
                this.setSpecialProxiesValues();
                this.setTokenstValues();
                this.SetSpecialCaptchaServicesCount();

                LotIDPicker.createLotIDInstance(this, this._lic);
                //LotIDPicker.LotIDInstance.setLotIDs();

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

                this.customBuildCheck();
                GoodProxies.Add(null);

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

                mre = new ManualResetEvent(false);

                Thread th = new Thread(GarbageCollectorTimer);
                th.SetApartmentState(ApartmentState.STA);
                th.Priority = ThreadPriority.Normal;
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GarbageCollectorTimer()
        {
            try
            {
                while (IsRunning)
                {
                    try
                    {
                        LicenseMessage.SendMessage("getAAXMagic");
                        GC.Collect();
                        mre.WaitOne(1000 * 45 * 1);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message + " " + ex.StackTrace);
                    }
                }
            }
            catch (Exception)
            {

            }
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
                            this.AppStartUp.GlobalSetting.RelayProxies25Percent = Boolean.Parse(obj.access);
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

        private void setTokenstValues()
        {
            try
            {
                IEnumerable<AccessList> allControlsAccesses = this.AppPermissions.AllAccessList.Where(p => p.form == this.Name);

                foreach (AccessList obj in allControlsAccesses)
                {
                    try
                    {
                        if (obj.name == "TokensCache5")
                        {
                            this.AppStartUp.GlobalSetting.TokensCache5 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "TokensCache10")
                        {
                            this.AppStartUp.GlobalSetting.TokensCache10 = Boolean.Parse(obj.access);
                        }
                        if (obj.name == "TokensCache25")
                        {
                            this.AppStartUp.GlobalSetting.TokensCache25 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "TokensCache50")
                        {
                            this.AppStartUp.GlobalSetting.TokensCache50 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "TokensCache75")
                        {
                            this.AppStartUp.GlobalSetting.TokensCache75 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "TokensCache100")
                        {
                            this.AppStartUp.GlobalSetting.TokensCache100 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "TokensCache150")
                        {
                            this.AppStartUp.GlobalSetting.TokensCache150 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "TokensCache200")
                        {
                            this.AppStartUp.GlobalSetting.TokensCache200 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "CapsiumWorkers10")
                        {
                            this.AppStartUp.GlobalSetting.CapsiumWorker10 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "CapsiumWorkers30")
                        {
                            this.AppStartUp.GlobalSetting.CapsiumWorkers30 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "CapsiumWorkers50")
                        {
                            this.AppStartUp.GlobalSetting.CapsiumWorker50 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "CapsiumWorkers75")
                        {
                            this.AppStartUp.GlobalSetting.CapsiumWorker75 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "CapsiumWorkers100")
                        {
                            this.AppStartUp.GlobalSetting.CapsiumWorkers100 = Boolean.Parse(obj.access);
                        }

                    }
                    catch { }
                }

                if (this.AppStartUp.GlobalSetting.TokensCache5)
                {
                    this.AppStartUp.GlobalSetting.TokensCount = 5;
                }
                else if (!this.AppStartUp.GlobalSetting.TokensCache5 && this.AppStartUp.GlobalSetting.TokensCache10)
                {
                    this.AppStartUp.GlobalSetting.TokensCount = 10;
                }
                else if (!this.AppStartUp.GlobalSetting.TokensCache5 && !this.AppStartUp.GlobalSetting.TokensCache10 && this.AppStartUp.GlobalSetting.TokensCache25)
                {
                    this.AppStartUp.GlobalSetting.TokensCount = 25;
                }
                else if (!this.AppStartUp.GlobalSetting.TokensCache5 && !this.AppStartUp.GlobalSetting.TokensCache10 && !this.AppStartUp.GlobalSetting.TokensCache25 && this.AppStartUp.GlobalSetting.TokensCache50)
                {
                    this.AppStartUp.GlobalSetting.TokensCount = 50;
                }
                else if (!this.AppStartUp.GlobalSetting.TokensCache5 && !this.AppStartUp.GlobalSetting.TokensCache10 && !this.AppStartUp.GlobalSetting.TokensCache25 && !this.AppStartUp.GlobalSetting.TokensCache50 && this.AppStartUp.GlobalSetting.TokensCache75)
                {
                    this.AppStartUp.GlobalSetting.TokensCount = 75;
                }
                else if (!this.AppStartUp.GlobalSetting.TokensCache5 && !this.AppStartUp.GlobalSetting.TokensCache10 && !this.AppStartUp.GlobalSetting.TokensCache25 && !this.AppStartUp.GlobalSetting.TokensCache50 && !this.AppStartUp.GlobalSetting.TokensCache75 && this.AppStartUp.GlobalSetting.TokensCache100)
                {
                    this.AppStartUp.GlobalSetting.TokensCount = 100;
                }
                else if (!this.AppStartUp.GlobalSetting.TokensCache5 && !this.AppStartUp.GlobalSetting.TokensCache10 && !this.AppStartUp.GlobalSetting.TokensCache25 && !this.AppStartUp.GlobalSetting.TokensCache50 && !this.AppStartUp.GlobalSetting.TokensCache75 && !this.AppStartUp.GlobalSetting.TokensCache100 && this.AppStartUp.GlobalSetting.TokensCache150)
                {
                    this.AppStartUp.GlobalSetting.TokensCount = 150;
                }
                else if (!this.AppStartUp.GlobalSetting.TokensCache5 && !this.AppStartUp.GlobalSetting.TokensCache10 && !this.AppStartUp.GlobalSetting.TokensCache25 && !this.AppStartUp.GlobalSetting.TokensCache50 && !this.AppStartUp.GlobalSetting.TokensCache75 && !this.AppStartUp.GlobalSetting.TokensCache100 && !this.AppStartUp.GlobalSetting.TokensCache150 && this.AppStartUp.GlobalSetting.TokensCache200)
                {
                    this.AppStartUp.GlobalSetting.TokensCount = 200;
                }
                else
                {
                    this.AppStartUp.GlobalSetting.TokensCount = 0;
                }
                try
                {
                    if (this.AppStartUp.GlobalSetting.CapsiumWorker10)
                    {
                        this.AppStartUp.GlobalSetting.CapsiumWorkersCount = 10;
                    }
                    else if (this.AppStartUp.GlobalSetting.CapsiumWorkers30 && !this.AppStartUp.GlobalSetting.CapsiumWorker50 && !this.AppStartUp.GlobalSetting.CapsiumWorker75 && !this.AppStartUp.GlobalSetting.CapsiumWorkers100)
                    {
                        this.AppStartUp.GlobalSetting.CapsiumWorkersCount = 30;
                    }
                    else if (this.AppStartUp.GlobalSetting.CapsiumWorker50 && !this.AppStartUp.GlobalSetting.CapsiumWorker75 && !this.AppStartUp.GlobalSetting.CapsiumWorkers100)
                    {
                        this.AppStartUp.GlobalSetting.CapsiumWorkersCount = 50;
                    }
                    else if (this.AppStartUp.GlobalSetting.CapsiumWorker75 && !this.AppStartUp.GlobalSetting.CapsiumWorker50 && !this.AppStartUp.GlobalSetting.CapsiumWorkers100)
                    {
                        this.AppStartUp.GlobalSetting.CapsiumWorkersCount = 75;
                    }
                    else if (this.AppStartUp.GlobalSetting.CapsiumWorkers100)
                    {
                        this.AppStartUp.GlobalSetting.CapsiumWorkersCount = 100;
                    }
                    else
                    {
                        this.AppStartUp.GlobalSetting.CapsiumWorkersCount = 0;
                    }
                }
                catch (Exception e)
                {
                    this.AppStartUp.GlobalSetting.CapsiumWorkersCount = 0;
                    Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
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

        public void SetSpecialCaptchaServicesCount()
        {
            try
            {
                IEnumerable<AccessList> allControlsAccesses = this.AppPermissions.AllAccessList.Where(p => p.form == this.Name);

                foreach (AccessList obj in allControlsAccesses)
                {
                    try
                    {
                        if (obj.name == "SpecialCaptchaServices10")
                        {
                            this.AppStartUp.GlobalSetting.SpecialCaptchaServices10 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "SpecialCaptchaServices20")
                        {
                            this.AppStartUp.GlobalSetting.SpecialCaptchaServices20 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "SpecialCaptchaServices30")
                        {
                            this.AppStartUp.GlobalSetting.SpecialCaptchaServices30 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "SpecialCaptchaServices40")
                        {
                            this.AppStartUp.GlobalSetting.SpecialCaptchaServices40 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "SpecialCaptchaServices50")
                        {
                            this.AppStartUp.GlobalSetting.SpecialCaptchaServices50 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "SpecialCaptchaServices75")
                        {
                            this.AppStartUp.GlobalSetting.SpecialCaptchaServices75 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "SpecialCaptchaServices100")
                        {
                            this.AppStartUp.GlobalSetting.SpecialCaptchaServices100 = Boolean.Parse(obj.access);
                        }
                        else if (obj.name == "SpecialCaptchaServices200")
                        {
                            this.AppStartUp.GlobalSetting.SpecialCaptchaServices200 = Boolean.Parse(obj.access);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }

                try
                {
                    if (this.AppStartUp.GlobalSetting.SpecialCaptchaServices10)
                    {
                        this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount = 10;
                    }
                    else if (this.AppStartUp.GlobalSetting.SpecialCaptchaServices20 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices30 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices40 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices50 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices75 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices100 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices200)
                    {
                        this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount = 20;
                    }
                    else if (this.AppStartUp.GlobalSetting.SpecialCaptchaServices30 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices40 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices50 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices75 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices100 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices200)
                    {
                        this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount = 30;
                    }
                    else if (this.AppStartUp.GlobalSetting.SpecialCaptchaServices40 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices50 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices75 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices100 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices200)
                    {
                        this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount = 40;
                    }
                    else if (this.AppStartUp.GlobalSetting.SpecialCaptchaServices50 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices75 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices100 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices200)
                    {
                        this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount = 50;
                    }
                    else if (this.AppStartUp.GlobalSetting.SpecialCaptchaServices50 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices75 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices100 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices200)
                    {
                        this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount = 50;
                    }
                    else if (this.AppStartUp.GlobalSetting.SpecialCaptchaServices75 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices50 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices100 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices200)
                    {
                        this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount = 75;
                    }
                    else if (this.AppStartUp.GlobalSetting.SpecialCaptchaServices100 && !this.AppStartUp.GlobalSetting.SpecialCaptchaServices200)
                    {
                        this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount = 100;
                    }
                    else if (this.AppStartUp.GlobalSetting.SpecialCaptchaServices200)
                    {
                        this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount = 200;
                    }
                    else
                    {
                        this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount = 5;
                    }
                }
                catch (Exception e)
                {
                    this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount = 0;
                    Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        public void loadTickets()
        {
            try
            {
                AppStartUp.LoadTickets();
                populateTickets();

                scheduleTickets();
            }
            catch (Exception ex)
            {

            }
        }

        public void loadlog()
        {
            AppStartUp.LoadLogs();
        }

        public void scheduleTickets()
        {
            try
            {
                if (this.AppStartUp != null)
                {
                    if (this.AppStartUp.Tickets != null)
                    {
                        for (int i = 0; i < this.AppStartUp.Tickets.Count; i++)
                        {
                            if (this.AppStartUp.Tickets[i].ifSchedule)
                            {
                                DateTime currentTime = DateTime.Now;
                                // this.AppStartUp.Tickets[i].ScheduleDateTime = this.AppStartUp.Tickets[i].ScheduleDateTime.AddSeconds(-1 * this.AppStartUp.Tickets[i].ScheduleDateTime.Second);

                                TimeSpan ts = (TimeSpan)(this.AppStartUp.Tickets[i].ScheduleDateTime - currentTime);

                                if (ts.TotalSeconds < -1)
                                {
                                    this.AppStartUp.Tickets[i].ifSchedule = false;
                                    continue;
                                }
                                System.Threading.TimerCallback tcb = this.runScheduledTicket;
                                System.Threading.Timer scheduleTimer = new System.Threading.Timer(tcb, this.AppStartUp.Tickets[i], ts, new TimeSpan(-1));

                                lock (this._scheduledTickets)
                                {
                                    if (this._scheduledTickets.ContainsKey(this.AppStartUp.Tickets[i].TicketID))
                                    {
                                        this._scheduledTickets[this.AppStartUp.Tickets[i].TicketID].Dispose();
                                        GC.SuppressFinalize(this._scheduledTickets[this.AppStartUp.Tickets[i].TicketID]);

                                        this._scheduledTickets[this.AppStartUp.Tickets[i].TicketID] = scheduleTimer;
                                    }
                                    else
                                    {
                                        this._scheduledTickets.Add(this.AppStartUp.Tickets[i].TicketID, scheduleTimer);
                                    }
                                }
                            }
                            else if (this._scheduledTickets.ContainsKey(this.AppStartUp.Tickets[i].TicketID))
                            {
                                lock (this._scheduledTickets)
                                {
                                    if (this._scheduledTickets[this.AppStartUp.Tickets[i].TicketID] != null)
                                    {
                                        this._scheduledTickets[this.AppStartUp.Tickets[i].TicketID].Dispose();
                                        GC.SuppressFinalize(this._scheduledTickets[this.AppStartUp.Tickets[i].TicketID]);
                                    }

                                    this._scheduledTickets.Remove(this.AppStartUp.Tickets[i].TicketID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void runScheduledTicket(object ticket)
        {
            try
            {
                if (ticket != null)
                {
                    AXSTicket ticketToRun = (AXSTicket)ticket;
                    if (ticketToRun.ifSchedule)
                    {
                        ticketToRun.ifSchedule = false;
                        ticketToRun.SaveTicket();

                        if (!ticketToRun.isRunning)
                        {
                            DateTime currentTime = DateTime.Now;
                            TimeSpan ts = (TimeSpan)(currentTime.AddMinutes((int)ticketToRun.ScheduleRunningTime) - currentTime);

                            System.Threading.TimerCallback tcb = this.stopScheduledTicket;
                            System.Threading.Timer unscheduleTimer = new System.Threading.Timer(tcb, ticketToRun, ts, new TimeSpan(-1));

                            lock (this._unscheduledTickets)
                            {
                                if (this._unscheduledTickets.ContainsKey(ticketToRun.TicketID))
                                {
                                    this._unscheduledTickets[ticketToRun.TicketID].Dispose();
                                    GC.SuppressFinalize(this._unscheduledTickets[ticketToRun.TicketID]);

                                    this._unscheduledTickets[ticketToRun.TicketID] = unscheduleTimer;
                                }
                                else
                                {
                                    this._unscheduledTickets.Add(ticketToRun.TicketID, unscheduleTimer);
                                }
                            }

                            if (this.InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate()
                                {
                                    frmFindTicket frm = new frmFindTicket(ticketToRun, this);
                                    if (!ticketToRun.isServiceSelected)
                                    {
                                        if (ticketToRun.URL.Contains("bit.ly") || (ticketToRun.URL.Contains(".queue") || ticketToRun.URL.Contains("q.axs.co.uk")) || (this.AppStartUp.GlobalSetting.ifEventko && ticketToRun.URL.Contains("evenko.ca")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("axs.com")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("shop.axs.co.uk")))
                                        {
                                            ticketToRun.start(this.CaptchaForm.CaptchaQueue, this.CaptchaBrowserForm.CaptchaQueue, this.AppStartUp.SoundAlert, this.AppStartUp.AutoCaptchaService, this.AppStartUp.Accounts, this.AppStartUp.EmailSetting, this.AppStartUp.GlobalSetting);
                                            frm.Show();
                                        }
                                        else
                                        {
                                            MessageBox.Show("You are not allowed to run this event");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(Notification.CSAlert);
                                    }

                                }));
                            }
                            else
                            {
                                frmFindTicket frm = new frmFindTicket(ticketToRun, this);
                                if (!ticketToRun.isServiceSelected)
                                {
                                    if ((this.AppStartUp.GlobalSetting.ifEventko && ticketToRun.URL.Contains("evenko.ca")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("axs.com")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("shop.axs.co.uk")))
                                    {
                                        ticketToRun.start(this.CaptchaForm.CaptchaQueue, this.CaptchaBrowserForm.CaptchaQueue, this.AppStartUp.SoundAlert, this.AppStartUp.AutoCaptchaService, this.AppStartUp.Accounts, this.AppStartUp.EmailSetting, this.AppStartUp.GlobalSetting);
                                        frm.Show();
                                    }
                                    else
                                    {
                                        MessageBox.Show("You are not allowed to run this event");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(Notification.CSAlert);
                                }
                            }
                        }
                    }

                    lock (this._scheduledTickets)
                    {
                        if (this._scheduledTickets.ContainsKey(ticketToRun.TicketID))
                        {
                            if (this._scheduledTickets[ticketToRun.TicketID] != null)
                            {
                                this._scheduledTickets[ticketToRun.TicketID].Dispose();
                                GC.SuppressFinalize(this._scheduledTickets[ticketToRun.TicketID]);
                                //GC.Collect();
                            }

                            this._scheduledTickets.Remove(ticketToRun.TicketID);
                        }
                    }
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
                                ticketToStop.stop();
                            }));
                        }
                        else
                        {
                            ticketToStop.stop();
                        }
                    }

                    lock (this._unscheduledTickets)
                    {
                        if (this._unscheduledTickets.ContainsKey(ticketToStop.TicketID))
                        {
                            if (this._unscheduledTickets[ticketToStop.TicketID] != null)
                            {
                                this._unscheduledTickets[ticketToStop.TicketID].Dispose();
                                GC.SuppressFinalize(this._unscheduledTickets[ticketToStop.TicketID]);
                                //GC.Collect();
                            }

                            this._unscheduledTickets.Remove(ticketToStop.TicketID);
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
            filterAndPopulateTickets();
        }

        public void populateTickets(SortableBindingList<AXSTicket> filteredTickets)
        {
            try
            {
                rgAllTickets.Items.ClearAndDisposeItems();
                //GC.Collect();

                if (filteredTickets != null)
                {
                    foreach (ITicket pair in filteredTickets)
                    {
                        C1.Win.C1Ribbon.RibbonGalleryItem rgItem = new RibbonGalleryItem();
                        rgItem.LargeImage = ((System.Drawing.Image)global::Automatick.Properties.Resources.ticket3);
                        rgItem.Text = pair.TicketName;
                        for (int i = pair.TicketName.Length; i <= 61; i++)
                        {
                            rgItem.Text += " ";
                        }
                        rgItem.FontPadding = FontPadding.Yes;
                        rgItem.VerticalLayout = false;
                        rgItem.Tag = pair;
                        this.rgAllTickets.Items.Add(rgItem);
                    }

                    if (rgAllTickets.SelectedIndex <= -1 && rgAllTickets.Items.Count > 0)
                    {
                        AXSTicket lastSelectedTicket = filteredTickets.FirstOrDefault(p => p.TicketID == this._lastSelectTicketId);
                        if (lastSelectedTicket != null && !String.IsNullOrEmpty(this._lastSelectTicketId))
                        {
                            rgAllTickets.SelectedIndex = filteredTickets.IndexOf(lastSelectedTicket);
                        }
                        else
                        {
                            rgAllTickets.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        rbRunTicket.Enabled = false;
                        rbEditTicket.Enabled = false;
                        rbDeleteTicket.Enabled = false;
                    }
                }
                else
                {
                    rbRunTicket.Enabled = false;
                    rbEditTicket.Enabled = false;
                    rbDeleteTicket.Enabled = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void loadGroups()
        {
            AppStartUp.LoadGroups();
            populateGroups();
        }

        public void populateGroups()
        {
            try
            {
                rcbGroups.Items.ClearAndDisposeItems();

                if (this.AppStartUp.Groups != null)
                {
                    foreach (TicketGroup pair in this.AppStartUp.Groups)
                    {
                        try
                        {
                            C1.Win.C1Ribbon.RibbonButton rItem = new RibbonButton();
                            rItem.SmallImage = ((System.Drawing.Image)global::Automatick.Properties.Resources._1341900838_services);
                            rItem.SmallImage = null;
                            rItem.LargeImage = null;
                            rItem.Text = pair.GroupName;
                            rItem.Tag = pair;
                            rItem.Name = pair.GroupName.Replace(" ", "_");

                            if (pair.GroupId == "Default Group")
                            {
                                if (rcbGroups.Items.Contains("All_Groups") && pair.GroupName == "Default Group")
                                {
                                    this.rcbGroups.Items.Insert(1, rItem);
                                }
                                else
                                {
                                    this.rcbGroups.Items.Insert(0, rItem);
                                }
                            }
                            else
                            {
                                this.rcbGroups.Items.Add(rItem);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                        }
                    }
                    this.rcbGroups.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
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
            try
            {
                if (flpRecentTickets != null)
                {
                    if (flpRecentTickets.Controls != null)
                    {
                        IOrderedEnumerable<AXSTicket> lastedUsedTicketsDesc = this.AppStartUp.Tickets.OrderByDescending(p => p.LastUsedDateTime);
                        IEnumerable<AXSTicket> lastedUsedTickets = null;
                        if (lastedUsedTicketsDesc != null)
                        {
                            if (lastedUsedTicketsDesc.Count() > this._recentTicketsListLimit)
                            {
                                lastedUsedTickets = lastedUsedTicketsDesc.Take(this._recentTicketsListLimit);
                            }
                            else
                            {
                                lastedUsedTickets = lastedUsedTicketsDesc;
                            }
                        }

                        if (lastedUsedTickets != null)
                        {
                            IEnumerable<AXSTicket> lastedUsedTicketsRunning = null;
                            IEnumerable<AXSTicket> lastedUsedTicketsNotRunning = null;

                            lastedUsedTicketsRunning = lastedUsedTickets.Where(p => p.isRunning == true);
                            lastedUsedTicketsNotRunning = lastedUsedTickets.Where(p => p.isRunning == false);

                            int renderEvents = 0;
                            if (lastedUsedTicketsRunning != null)
                            {
                                for (int i = 0; i < lastedUsedTicketsRunning.Count(); i++, renderEvents++)
                                {
                                    AXSTicket ticket = lastedUsedTicketsRunning.ElementAt(i);
                                    ucRecentItem ri = null;
                                    if (flpRecentTickets.Controls.Count > renderEvents)
                                    {
                                        if (flpRecentTickets.Controls[renderEvents] != null)
                                        {
                                            ri = (ucRecentItem)flpRecentTickets.Controls[renderEvents];
                                            ri.Ticket = ticket;
                                        }
                                        else
                                        {
                                            ri = new ucRecentItem(ticket, this);
                                            flpRecentTickets.Controls.Add(ri);
                                        }
                                    }
                                    else
                                    {
                                        ri = new ucRecentItem(ticket, this);
                                        flpRecentTickets.Controls.Add(ri);
                                    }
                                    if (ri != null)
                                    {
                                        ri.lblEdit.Visible = false;
                                        ri.lblDelete.Visible = false;
                                        ri.lblStop.Visible = true;
                                    }
                                }
                            }

                            if (lastedUsedTicketsNotRunning != null)
                            {
                                for (int i = 0; i < lastedUsedTicketsNotRunning.Count(); i++, renderEvents++)
                                {
                                    AXSTicket ticket = lastedUsedTicketsNotRunning.ElementAt(i);
                                    ucRecentItem ri = null;
                                    if (flpRecentTickets.Controls.Count > renderEvents)
                                    {
                                        if (flpRecentTickets.Controls[renderEvents] != null)
                                        {
                                            ri = (ucRecentItem)flpRecentTickets.Controls[renderEvents];
                                            ri.Ticket = ticket;
                                        }
                                        else
                                        {
                                            ri = new ucRecentItem(ticket, this);
                                            flpRecentTickets.Controls.Add(ri);
                                        }
                                    }
                                    else
                                    {
                                        ri = new ucRecentItem(ticket, this);
                                        flpRecentTickets.Controls.Add(ri);
                                    }
                                    if (ri != null)
                                    {
                                        ri.lblEdit.Visible = true;
                                        ri.lblDelete.Visible = true;
                                        ri.lblStop.Visible = false;
                                    }
                                }
                            }

                            int controlsCount = flpRecentTickets.Controls.Count;
                            if (renderEvents < controlsCount)
                            {
                                for (int i = controlsCount - 1; i >= renderEvents; i--)
                                {
                                    Control ctrl = flpRecentTickets.Controls[i];
                                    flpRecentTickets.Controls.Remove(ctrl);
                                    ctrl.Dispose();
                                    GC.SuppressFinalize(ctrl);
                                }
                                //GC.Collect();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void rbRunTicket_Click(object sender, EventArgs e)
        {
            try
            {
                if (rgAllTickets.SelectedItem != null)
                {
                    if (rgAllTickets.SelectedItem.Tag != null)
                    {
                        AXSTicket ticketToRun = ((AXSTicket)rgAllTickets.SelectedItem.Tag);
                        if (!ticketToRun.isRunning)
                        {
                            rbEditTicket.Enabled = false;
                            rbDeleteTicket.Enabled = false;
                            this.rbRunTicket.LargeImage = global::Automatick.Properties.Resources._1348035001_stop;
                            this.rbRunTicket.Text = "Abort";

                            frmFindTicket frm = new frmFindTicket(ticketToRun, this);
                            if (!ticketToRun.isServiceSelected)
                            {
                                if (ticketToRun.URL.Contains("bit.ly") || (ticketToRun.URL.Contains(".queue") || ticketToRun.URL.Contains("q.axs.co.uk")) || (this.AppStartUp.GlobalSetting.ifEventko && ticketToRun.URL.Contains("evenko.ca")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("axs.com")) || ((this.AppStartUp.GlobalSetting.IfWeb || this.AppStartUp.GlobalSetting.ifMobile) && ticketToRun.URL.Contains("shop.axs.co.uk")))
                                {
                                    ticketToRun.start(this.CaptchaForm.CaptchaQueue, this.CaptchaBrowserForm.CaptchaQueue, this.AppStartUp.SoundAlert, this.AppStartUp.AutoCaptchaService, this.AppStartUp.Accounts, this.AppStartUp.EmailSetting, this.AppStartUp.GlobalSetting);
                                    frm.Show();
                                }
                                else
                                {
                                    MessageBox.Show("You are not allowed to run this event");
                                }
                            }
                            else
                            {
                                MessageBox.Show(Notification.CSAlert);
                            }
                        }
                        else
                        {
                            rbEditTicket.Enabled = true;
                            rbDeleteTicket.Enabled = true;
                            this.rbRunTicket.LargeImage = global::Automatick.Properties.Resources._1348034999_continue;
                            this.rbRunTicket.Text = "Run";
                            ticketToRun.stop();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void rbAddTicket_Click(object sender, EventArgs e)
        {
            frmTicket frm = new frmTicket(this);
            frm.ShowDialog();
        }

        private void rbEditTicket_Click(object sender, EventArgs e)
        {
            if (rgAllTickets.SelectedItem != null)
            {
                frmTicket frm = new frmTicket(this, (ITicket)rgAllTickets.SelectedItem.Tag);
                frm.ShowDialog();
            }

        }

        private void rbDeleteTicket_Click(object sender, EventArgs e)
        {
            try
            {
                if (rgAllTickets.SelectedItem != null)
                {
                    if (rgAllTickets.SelectedItem.Tag != null)
                    {
                        ITicket ticketToDelete = ((ITicket)rgAllTickets.SelectedItem.Tag);

                        if (MessageBox.Show("Do you really want to delete \"" + ticketToDelete.TicketName + "\"?", "Are you Sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            this.AppStartUp.Tickets.Remove((AXSTicket)ticketToDelete);
                            ticketToDelete.DeleteTicket();
                            this.populateTickets();
                            this.populateRecentTickets();

                            // unschedule ticket on delete ticket
                            lock (this._scheduledTickets)
                            {
                                if (this._scheduledTickets.ContainsKey(ticketToDelete.TicketID))
                                {
                                    if (this._scheduledTickets[ticketToDelete.TicketID] != null)
                                    {
                                        this._scheduledTickets[ticketToDelete.TicketID].Dispose();
                                        GC.SuppressFinalize(this._scheduledTickets[ticketToDelete.TicketID]);
                                    }

                                    this._scheduledTickets.Remove(ticketToDelete.TicketID);
                                }
                            }

                            lock (this._unscheduledTickets)
                            {
                                if (this._unscheduledTickets.ContainsKey(ticketToDelete.TicketID))
                                {
                                    if (this._unscheduledTickets[ticketToDelete.TicketID] != null)
                                    {
                                        this._unscheduledTickets[ticketToDelete.TicketID].Dispose();
                                        GC.SuppressFinalize(this._unscheduledTickets[ticketToDelete.TicketID]);
                                        //GC.Collect();
                                    }

                                    this._unscheduledTickets.Remove(ticketToDelete.TicketID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void rgAllTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rgAllTickets.SelectedIndex > -1)
                {
                    rbRunTicket.Enabled = true;
                    rbEditTicket.Enabled = true;
                    rbDeleteTicket.Enabled = true;
                    if (rgAllTickets.SelectedItem.Tag != null)
                    {
                        ITicket ticketToCheck = ((ITicket)rgAllTickets.SelectedItem.Tag);
                        this._lastSelectTicketId = ticketToCheck.TicketID;
                        if (ticketToCheck.isRunning)
                        {
                            rbEditTicket.Enabled = false;
                            rbDeleteTicket.Enabled = false;
                            this.rbRunTicket.LargeImage = global::Automatick.Properties.Resources._1348035001_stop;
                            this.rbRunTicket.Text = "Abort";
                        }
                        else
                        {
                            rbEditTicket.Enabled = true;
                            rbDeleteTicket.Enabled = true;
                            this.rbRunTicket.LargeImage = global::Automatick.Properties.Resources._1348034999_continue;
                            this.rbRunTicket.Text = "Run";
                        }
                    }
                }
                else
                {
                    rbRunTicket.Enabled = false;
                    rbEditTicket.Enabled = false;
                    rbDeleteTicket.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void onChangeHandlerStartOrStop(ITicket ticket)
        {
            try
            {
                if (rgAllTickets.SelectedIndex > -1)
                {
                    rbRunTicket.Enabled = true;
                    rbEditTicket.Enabled = true;
                    rbDeleteTicket.Enabled = true;
                    if (rgAllTickets.SelectedItem.Tag != null)
                    {
                        ITicket ticketToCheck = ((ITicket)rgAllTickets.SelectedItem.Tag);
                        if (ticket == ticketToCheck)
                        {
                            if (ticketToCheck.isRunning)
                            {
                                rbEditTicket.Enabled = false;
                                rbDeleteTicket.Enabled = false;
                                this.rbRunTicket.LargeImage = global::Automatick.Properties.Resources._1348035001_stop;
                                this.rbRunTicket.Text = "Abort";
                            }
                            else
                            {
                                rbEditTicket.Enabled = true;
                                rbDeleteTicket.Enabled = true;
                                this.rbRunTicket.LargeImage = global::Automatick.Properties.Resources._1348034999_continue;
                                this.rbRunTicket.Text = "Run";
                            }
                        }
                    }
                }
                else
                {
                    rbRunTicket.Enabled = false;
                    rbEditTicket.Enabled = false;
                    rbDeleteTicket.Enabled = false;
                }

                populateRecentTickets();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void rbEmail_Click(object sender, EventArgs e)
        {
            frmEmail frm = new frmEmail(this);
            frm.ShowDialog();
        }

        private void rbManageGroup_Click(object sender, EventArgs e)
        {
            frmGroup frm = new frmGroup(this);
            frm.ShowDialog();
        }

        private void rbDeliveryOptions_Click(object sender, EventArgs e)
        {
            frmDeliveryOption frm = new frmDeliveryOption(this);
            frm.ShowDialog();
        }

        private void rcbGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterAndPopulateTickets();
        }

        private void rcbFilter_ChangeCommitted(object sender, EventArgs e)
        {
            filterAndPopulateTickets();
        }

        private void filterAndPopulateTickets()
        {
            try
            {
                if (rcbGroups.SelectedIndex > -1 && rcbGroups.SelectedItem != null)
                {
                    if (rcbGroups.SelectedItem.Tag != null)
                    {
                        TicketGroup group = (TicketGroup)rcbGroups.SelectedItem.Tag;
                        SortableBindingList<AXSTicket> filteredTickets = null;
                        if (!String.IsNullOrEmpty(rcbFilter.Text.Trim()))
                        {
                            filteredTickets = this.AppStartUp.filterTickets(group, rcbFilter.Text);
                        }
                        else
                        {
                            filteredTickets = this.AppStartUp.filterTickets(group);
                        }
                        this.populateTickets(filteredTickets);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
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
                    SAPushClient.stop();
                    ServerAutomationPiperServer.Stop();
                    this.proxyListner.Stop();
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
                    this.AppStartUp.SaveLogs();
                    try
                    {
                        LotIDPicker.LotIDInstance.closeClient();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }

                try
                {
                    if (this._processesWorker != null)
                    {
                        this._processesWorker.stop();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }
            catch (Exception)
            {

            }
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

                    if (this.AppStartUp.Proxies.Count > 0)
                    {
                        Proxy p = this.AppStartUp.Proxies.First(q => q.TheProxyType == Proxy.ProxyType.ISPIP);
                        if (p != null)
                        {
                            this.AppStartUp.Proxies.Remove(p);
                        }
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
                LicenseCore lic = new LicenseCore("AAX", Application.StartupPath + @"\lic.lic", false);

                if (!lic.ValidateLicense())
                {
                    //AuthtokenPicker.AuthtokenPickerInstance.Validated();
                    this.Close();
                }
                else
                {
                    //AuthtokenPicker.AuthtokenPickerInstance.ReInitialize(lic);
                }
                if (!this.AppStartUp.GlobalSetting.IfUseSpecialProxies)
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

        public void stopTixTox()
        {
            try
            {
                if (this.AppStartUp.GlobalSetting.TokensCount <= 0)
                {
                    if (_processesWorker != null)
                    {
                        _processesWorker.stop();
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public async void startTixTox()
        {
            try
            {
                AccessRights.AccessList CapsiumServers = this.AppPermissions.AllAccessList.Single(p => p.form == "CapsiumServers");

                await Task.Run(() =>
                {
                    if (ValidationMessage.TixToxInstance != null && (CapsiumServers != null))
                    {
                        ValidationMessage.TixToxInstance.GLOBAL_SERVICE_PORT = CONFIG.AAX_BUCKET_SERVICE_PORT;
                        ValidationMessage.TixToxInstance.ConvertAutoCaptchaService(JsonConvert.SerializeObject(this.AppStartUp.AutoCaptchaService));
                        ValidationMessage.TixToxInstance.TokensCount = this.AppStartUp.GlobalSetting.TokensCount;
                        ValidationMessage.TixToxInstance.CapsiumWorkersCount = this.AppStartUp.GlobalSetting.TokensCount;
                        ValidationMessage.TixToxInstance.SpecialServicesWorkerCount = this.AppStartUp.GlobalSetting.SpecialServicesWorkerCount;
                        ValidationMessage.TixToxInstance.BucketVersion = "AXS Bucket Version 4.3.8";
                        ValidationMessage.TixToxInstance.Configs.Clear();
                        //ValidationMessage.TixToxInstance.Configs.Add(new Configuration() { sitecaptchakey = "6LcxLC4UAAAAALApc8RfaPkUJ-YEtnapN5xAnZ37", sitehost = "tickets.evenko.ca", region = "EVENKO", key = "EVENKO" + TCPClient.UniqueKey.getUniqueKey(), isrecap = true, recapUIText = "EVENKO Tokens" });
                        //ValidationMessage.TixToxInstance.save(this.AppStartUp.DefaultBucketFile);

                        if (this.AppStartUp.GlobalSetting.ifEvenkoBucket)
                        {
                            ValidationMessage.TixToxInstance.Configs.Add(new Configuration()
                            {
                                sitecaptchakey = "6LcxLC4UAAAAALApc8RfaPkUJ-YEtnapN5xAnZ37",
                                sitehost = "tickets.evenko.ca",
                                region = "EVENKO",
                                key = "EVENKO" + TCPClient.UniqueKey.getUniqueKey(),
                                isrecap = true,
                                recapUIText = "EVENKO Tokens"
                            });
                        }

                        if (this.AppStartUp.GlobalSetting.ifAXSBucket)
                        {
                            ValidationMessage.TixToxInstance.Configs.Add(new Configuration()
                            {
                                sitecaptchakey = "6Lejv2AUAAAAAC2ga_dkzgFadQvGnUbuJW_FgsvC",//"6LexTBoTAAAAAESv_PtNKgDQM7ZP9KOKedZUbYay",//CONFIG.AAX_CAPTCHA_KEY,// 6Lejv2AUAAAAAC2ga_dkzgFadQvGnUbuJW_FgsvC
                                sitehost = "tix.axs.com",// CONFIG.AAX_CAPTCHA_HOST,
                                region = CONFIG.AAX_WEB,
                                key = CONFIG.AAX_WEB + TCPClient.UniqueKey.getUniqueKey(),
                                isrecap = true,
                                recapUIText = "AXS Tokens"
                            });

                            ValidationMessage.TixToxInstance.Configs.Add(new Configuration()
                            {
                                sitecaptchakey = "6LexTBoTAAAAAESv_PtNKgDQM7ZP9KOKedZUbYay",//CONFIG.AAX_CAPTCHA_KEY,// 6Lejv2AUAAAAAC2ga_dkzgFadQvGnUbuJW_FgsvC
                                sitehost = "tix.axs.com",// CONFIG.AAX_CAPTCHA_HOST,
                                region = "axsWEBv1",//CONFIG.AAX_WEB,
                                key = "axsWEBV1" + TCPClient.UniqueKey.getUniqueKey(),
                                isrecap = true,
                                recapUIText = "AXS Tokens V1"
                            });

                            ValidationMessage.TixToxInstance.Configs.Add(new Configuration()
                            {
                                sitecaptchakey = "33f96e6a-38cd-421b-bb68-7806e1764460",//CONFIG.AAX_CAPTCHA_KEY,// 6Lejv2AUAAAAAC2ga_dkzgFadQvGnUbuJW_FgsvC
                                sitehost = "tix.axs.com",// CONFIG.AAX_CAPTCHA_HOST,
                                region = "axsWEBCF",//CONFIG.AAX_WEB,
                                key = "axsWEBCF" + TCPClient.UniqueKey.getUniqueKey(),
                                isrecap = true,
                                iscfCaptcha = true,
                                recapUIText = "AXS CF Tokens"
                            });
                        }

                        if (this.AppStartUp.GlobalSetting.ifQueueITBucket)
                        {
                            GeneralBucket.ValidationMessage.TixToxInstance.Configs.Add(new Configuration()
                            {
                                sitecaptchakey = "6Lc9sScUAAAAALTk003eM2ytnYGGKQaQa7usPKwo",
                                sitehost = "shop.axs.com",
                                region = "QUEUE_V1",
                                key = "QUEUE_V1" + TCPClient.UniqueKey.getUniqueKey(),
                                isrecap = true,
                                recapUIText = "Queue Recaptcha V1"
                            });

                            //GeneralBucket.ValidationMessage.TixToxInstance.Configs.Add(new Configuration()
                            //{
                            //    sitecaptchakey = "6LePTyoUAAAAADPttQg1At44EFCygqxZYzgleaKp",
                            //    sitehost = "shop.axs.com",
                            //    region = "QUEUE_V2",
                            //    key = "QUEUE_V2" + TCPClient.UniqueKey.getUniqueKey(),
                            //    isrecap = true,
                            //    recapUIText = "Queue Recaptcha V2"
                            //});
                        }

                        // axs uk bucket
                        if (this.AppStartUp.GlobalSetting.ifAXSUKBucket)
                        {
                            ValidationMessage.TixToxInstance.Configs.Add(new Configuration()
                            {
                                sitecaptchakey = "6Lfq-DcUAAAAAB6ONN3R_TL3NP4-8sYIlkNVEJ7n",//CONFIG.AAX_CAPTCHA_KEY,
                                sitehost = "shop.axs.co.uk",
                                region = "axsUK",//CONFIG.AAX_WEB,
                                key = "axsUK" + TCPClient.UniqueKey.getUniqueKey(),
                                isrecap = true,
                                recapUIText = "AXS UK Tokens"
                            });
                        }

                        ValidationMessage.TixToxInstance.save(this.AppStartUp.DefaultBucketFile);
                    }
                    else
                    {
                        Debug.WriteLine("Invalid License");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void customBuildCheck()
        {
            try
            {
                AccessRights.AccessList CapsiumServers = AppPermissions.AllAccessList.Single(p => p.form == "CapsiumServers");

                AccessRights.AccessList EVENKO = this.AppPermissions.AllAccessList.Single(p => p.form == "EvenkoBucket");
                this.AppStartUp.GlobalSetting.ifEvenkoBucket = Convert.ToBoolean(EVENKO.access);

                AccessRights.AccessList AXS = this.AppPermissions.AllAccessList.Single(p => p.form == "AXSBucket");
                this.AppStartUp.GlobalSetting.ifAXSBucket = Convert.ToBoolean(AXS.access);

                AccessRights.AccessList QueueIT = this.AppPermissions.AllAccessList.Single(p => p.form == "QueueITBucket");
                this.AppStartUp.GlobalSetting.ifQueueITBucket = Convert.ToBoolean(QueueIT.access);

                AccessRights.AccessList AXSuk = this.AppPermissions.AllAccessList.Single(p => p.form == "AXSUKBucket");
                this.AppStartUp.GlobalSetting.ifAXSUKBucket = Convert.ToBoolean(AXSuk.access);

                ServerAutomationPiperServer.start(IPAddress.Parse("127.0.0.1"), 1237);

                if (this.AppStartUp.GlobalSetting.TokensCount > 0 && (Boolean.Parse(CapsiumServers.access)))
                {
                    TixToxShowStartButton();
                    //startProxyListner();
                }
                else
                {
                    try
                    {
                        Task.Run(() =>
                        {
                            ImageMerger.createImageMerger();
                            Captchator.createCaptchatorInstance(AppStartUp.AutoCaptchaService, captchatorWorker);
                        });

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }

                try
                {
                    Task.Run(() =>
                    {
                        SAPushClient.initialize(LICENSING_SERVER_IP, 44000, new CommandProcessor());
                        LicenseMessage.RegisterLicense(ValidationMessage.TixToxInstance.HardDiskSerial, ValidationMessage.TixToxInstance.LicenseCode, ValidationMessage.TixToxInstance.ProcessorID, ValidationMessage.TixToxInstance.AppPrefix);
                        LicenseMessage.SendMessage("RelayServer");
                        LicenseMessage.SendMessage("getThrottleValue");
                        LicenseMessage.SendMessage("getCapsiumServers");
                        LicenseMessage.SendMessage("C1");
                        LicenseMessage.SendMessage("PercentRelay");
                        LicenseMessage.SendMessage("getAAXMagic");
                    });
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Debug.WriteLine(ex.Message);
            }
        }

        public void TixToxShowStartButton()
        {
            try
            {
                rbStartAndStopTixTox.Visible = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }

        private async void rbStartAndStopTixTox_Click(object sender, EventArgs e)
        {
            try
            {

                rbStartAndStopTixTox.Enabled = false;

                await Task.Run(() =>
                {
                    if (applyPermission())
                    {
                        this.startTixTox();

                        if (this.rbStartAndStopTixTox.Text == "Start Token Generator")
                        {
                            this._processesWorker.start();


                            if (this.InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate()
                                {
                                    this.rbStartAndStopTixTox.SmallImage = global::Automatick.Properties.Resources.stopb16;
                                    this.rbStartAndStopTixTox.Text = "Stop Token Generator";
                                }));
                            }


                        }
                        else if (this.rbStartAndStopTixTox.Text == "Stop Token Generator")
                        {
                            this._processesWorker.exit();

                            if (this.InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate()
                                {
                                    this.rbStartAndStopTixTox.SmallImage = global::Automatick.Properties.Resources.playb16;
                                    this.rbStartAndStopTixTox.Text = "Start Token Generator";


                                }));
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(Notification.CSAlert);
                    }
                });


            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);
            }

            rbStartAndStopTixTox.Enabled = true;
        }

        public bool applyPermission()
        {
            bool ifAnySelected = true;

            try
            {

                IEnumerable<AccessRights.AccessList> allControlsAccesses = this.AppPermissions.AllAccessList.Where(p => p.form == "frmTicket");//"frmTicket");

                foreach (AccessRights.AccessList obj in allControlsAccesses)
                {
                    if (obj.name == "rbBPCAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifBPCAutoCaptcha = false;
                        }
                    }
                    else if (obj.name == "rbRDAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifRDAutoCaptcha = false;
                        }
                    }
                    else if (obj.name == "rbRDCAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifRDCAutoCaptcha = false;
                        }
                    }
                    else if (obj.name == "rbCPTAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);

                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifCPTAutoCaptcha = false;

                        }
                    }
                    else if (obj.name == "rbDBCAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);

                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifDBCAutoCaptcha = false;

                        }
                    }
                    else if (obj.name == "rbDCAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifDCAutoCaptcha = false;
                        }
                    }
                    else if (obj.name == "rbCAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifCAutoCaptcha = false;

                        }
                    }
                    else if (obj.name == "rbNOCRAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifOCR = false;

                        }
                    }
                    else if (obj.name == "rbRAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifROCR = false;
                        }
                    }
                    else if (obj.name == "rbBoloAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifBoloOCR = false;
                        }
                    }
                    else if (obj.name == "rb2CAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.if2CAutoCaptcha = false;
                        }
                    }
                    else if (obj.name == "rbCRAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifCRAutoCaptcha = false;
                        }
                    }
                    else if (obj.name == "rbNOCRAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifNoCaptchaOCRAutoCaptcha = false;
                        }
                    }
                    else if (obj.name == "rbCaptchatorAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifCaptchator = false;
                        }
                    }
                    else if (obj.name == "rbAAutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifAntigate = false;
                        }
                    }
                    else if (obj.name == "rbAC1AutoCaptcha")
                    {
                        Boolean access = Boolean.Parse(obj.access);

                        if (!access)
                        {
                            this.AppStartUp.AutoCaptchaService.ifAC1AutoCaptcha = false;
                        }
                    }
                }

                if (!this.AppStartUp.AutoCaptchaService.ifBPCAutoCaptcha && !this.AppStartUp.AutoCaptchaService.ifCPTAutoCaptcha && !this.AppStartUp.AutoCaptchaService.ifDBCAutoCaptcha && !this.AppStartUp.AutoCaptchaService.ifRDAutoCaptcha && !this.AppStartUp.AutoCaptchaService.ifDCAutoCaptcha && !this.AppStartUp.AutoCaptchaService.ifCAutoCaptcha && !this.AppStartUp.AutoCaptchaService.ifOCR && !this.AppStartUp.AutoCaptchaService.ifROCR
                 && !this.AppStartUp.AutoCaptchaService.ifBoloOCR && !this.AppStartUp.AutoCaptchaService.if2CAutoCaptcha && !this.AppStartUp.AutoCaptchaService.ifCRAutoCaptcha && !this.AppStartUp.AutoCaptchaService.ifNoCaptchaOCRAutoCaptcha && !this.AppStartUp.AutoCaptchaService.ifCaptchator && !this.AppStartUp.AutoCaptchaService.ifAntigate && !this.AppStartUp.AutoCaptchaService.ifAC1AutoCaptcha && !this.AppStartUp.AutoCaptchaService.ifRDCAutoCaptcha)
                {
                    ifAnySelected = false;
                }
                else
                {
                    ifAnySelected = true;
                }
            }
            catch (Exception ex)
            {
            }

            return ifAnySelected;
        }

        public void startProxyListner()
        {
            try
            {
                this.proxyListner = new TixToxProxyListener("127.0.0.1", 8014);
                this.proxyListner.start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

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

        private void ribbonMain_RibbonEvent(object sender, RibbonEventArgs e)
        {

        }
    }
}
