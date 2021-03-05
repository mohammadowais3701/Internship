using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SortedBindingList;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace Automatick.Core
{
    public class ProxyPicker
    {
        static ProxyPicker _proxyPicker = null;
        ApplicationStartUp _appStartUp = null;
        int _currentProxyIndex = 0;
        private LicenseCorePPM _licPPM = null;
        private ProxyManager.ProxyManager _proxyManager = null;
        private ProxyManager.ProxyManager _proxyRelayManager = null;

        public ProxyManager.ProxyManager ProxyRelayManager
        {
            get { return _proxyRelayManager; }
            set { _proxyRelayManager = value; }
        }
        IMainForm _mainForm = null;
        System.Threading.Timer _recheckProxiesStatusTimer = null;
        int relayCounter = 0;

        public long globalproxycount = 0;
        public long globalproxyusage = 0;

        public int RelayCounter
        {
            get { return relayCounter; }
            set { relayCounter = value; }
        }
        ProxySelector selector = null;

        public ProxySelector Selector
        {
            get { return selector; }
            set { selector = value; }
        }

        public LicenseCorePPM LicPPM
        {
            get { return _licPPM; }
            set { _licPPM = value; }
        }

        public ProxyManager.ProxyManager ProxyManager
        {
            get { return _proxyManager; }
            set { _proxyManager = value; }
        }

        public ApplicationStartUp AppStartUp
        {
            get { return _appStartUp; }
            set { _appStartUp = value; }
        }

        public static ProxyPicker ProxyPickerInstance
        {
            get
            {
                if (_proxyPicker == null)
                {
                    throw new Exception("Proxy picker Object not created");
                }
                return _proxyPicker;
            }
        }

        public static void createProxyPickerInstance(IMainForm mainForm)
        {
            _proxyPicker = new ProxyPicker(mainForm);
        }

        private ProxyPicker(IMainForm mainForm)
        {
            this._mainForm = mainForm;
            this._appStartUp = mainForm.AppStartUp;
            this.InitializePPM();
            Dictionary<Proxy.ProxyType, int> select = new Dictionary<Proxy.ProxyType, int>();
            select.Add(Proxy.ProxyType.All, 100);
            selector = new ProxySelector(select);
        }

        public Proxy getNextProxy(ITicketSearch search)
        {

            Proxy proxy = null;

            if (this._appStartUp.Proxies != null)
            {
                if (this._appStartUp.Proxies.Count > 0)
                {
                    Proxy.ProxyType type = selector.SelectProxy();

                    if (search != null)
                    {
                        if (search.Proxy != null)
                        {
                            if (search.Proxy.TheProxyType == Proxy.ProxyType.Relay)
                            {
                                this._proxyRelayManager.ReleaseProxy(search.Proxy);
                            }
                            else if (search.Proxy.TheProxyType == Proxy.ProxyType.Luminati)
                            {
                                this._proxyManager.ReleaseProxy(search.Proxy);
                            }
                            else if (search.Proxy.TheProxyType == Proxy.ProxyType.MyIP)
                            {
                                this._proxyManager.ReleaseProxy(search.Proxy);
                            }
                        }
                    }

                    if ((this._appStartUp.GlobalSetting.IfUseSpecialProxies && this._appStartUp.GlobalSetting.IfEnableSpecialProxies) || (this._appStartUp.GlobalSetting.IfUseRelayProxies || this._appStartUp.GlobalSetting.IfUseSpecialRelayProxies))
                    {
                        try
                        {
                            if (type == Proxy.ProxyType.All && ((this._appStartUp.GlobalSetting.IfUseSpecialProxies && this._appStartUp.GlobalSetting.IfEnableSpecialProxies)))
                            {
                                proxy = this._proxyManager.getNextProxy();
                            }
                            else
                            {
                                proxy = this._proxyRelayManager.getNextProxy();
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else if (_appStartUp.GlobalSetting.IfEnablePM && _licPPM.IsValidated && !this._appStartUp.GlobalSetting.IfUseSpecialProxies)
                    {
                        proxy = this._proxyManager.getNextProxy();

                        try
                        {
                            if (_recheckProxiesStatusTimer == null)
                            {
                                _recheckProxiesStatusTimer = new System.Threading.Timer(this.recheckProxiesStatusTimerCallBackHandler, null, 10000, 30000); //30 * 60000

                            }
                        }
                        catch (Exception)
                        {

                        }
                    }

                    if (proxy == null)
                    {
                        List<Proxy> _lstProxies = this._appStartUp.Proxies.Where(pred => pred.TheProxyType != Proxy.ProxyType.Relay && pred.TheProxyType != Proxy.ProxyType.Luminati && pred.TheProxyType != Proxy.ProxyType.MyIP).ToList();
                        for (int i = 0; i < _lstProxies.Count; i++)
                        {
                            if (this._currentProxyIndex >= _lstProxies.Count)
                            {
                                this._currentProxyIndex = 0;
                            }

                            proxy = _lstProxies[_currentProxyIndex];

                            _currentProxyIndex++;

                            if (proxy == null)
                            {
                                continue;
                            }
                            else if (proxy.ProxyStatus == Proxy.proxyBlocked || proxy.ProxyStatus == Proxy.proxyNotWork)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }

                        try
                        {
                            if (this._recheckProxiesStatusTimer != null)
                            {
                                this._recheckProxiesStatusTimer.Dispose();
                                this._recheckProxiesStatusTimer = null;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }

            return proxy;
        }

        int coun = 0;

        public Proxy getNextProxyForTixTox()
        {
            Proxy selectedProxy = null;

            try
            {
                if ((this._appStartUp.Proxies != null) && (this._appStartUp.Proxies.Count > 0))
                {
                    string content = string.Empty;
                    foreach (Proxy p in this._appStartUp.Proxies)
                    {
                        content += p.Address + ":" + p.Port + ":" + p.UserName + ":" + p.Password + Environment.NewLine;
                    }
                    //File.WriteAllText(@"p.txt", content);

                    selectedProxy = this._appStartUp.Proxies[coun];//this._proxyManager.getNextProxyForTixTox();

                    //if (coun < this._appStartUp.Proxies.Count())
                    //{ Interlocked.Increment(ref coun); }
                    //else coun = 0;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return selectedProxy;
        }

        public Boolean ifSearchAllowed(ITicketSearch search)
        {
            Boolean result = false;
            if (_appStartUp.GlobalSetting.IfEnableSpecialProxies && search.IfUseProxy && _appStartUp.GlobalSetting.IfUseSpecialProxies)
            {
                if (search.Proxy == null)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else if (_appStartUp.GlobalSetting.IfEnablePM && search.IfUseProxy && this.LicPPM != null)
            {
                if (this.LicPPM.IsValidated && search.IfUseProxy && search.Proxy == null)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        internal void InitializePPM()
        {
            try
            {
                string PpmPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TB";
                if (!Directory.Exists(PpmPath))
                {
                    Directory.CreateDirectory(PpmPath);
                }
                PpmPath = PpmPath + "\\PM";
                if (!Directory.Exists(PpmPath))
                {
                    Directory.CreateDirectory(PpmPath);
                }
                _licPPM = new LicenseCorePPM(PpmPath + @"\lic.lic", true);
            }
            catch { }
        }

        public void ValidatePPM(Boolean ifShowMessage)
        {
            if (this._appStartUp.GlobalSetting.IfUseSpecialProxies)
            {
                if (_proxyManager != null)
                {
                    _proxyManager.Dispose();
                    _proxyManager = null;
                }

                _proxyManager = new ProxyManager.LuminatiProxyManager(this._appStartUp.Proxies, this.AppStartUp.GlobalSetting.ProxiesCount, this._appStartUp.GlobalSetting.ProxiesCountry, this._appStartUp.GlobalSetting.CountryUSCA);

                if (_appStartUp.GlobalSetting.IfEnableSpecialProxies)
                {
                    _mainForm.chkEnableLuminatiProxiesChecked = true;
                }
                else
                {
                    _mainForm.chkEnableLuminatiProxiesChecked = false;
                }
            }
            else
            {
                if (_licPPM.ValidateLicense())
                {
                    if (_proxyManager != null)
                    {
                        _proxyManager.Dispose();
                        _proxyManager = null;
                    }
                    _proxyManager = new ProxyManager.ProxyManager(_licPPM.PPMLicenseID, this._appStartUp.Proxies);

                    _mainForm.chkEnableProxyManagerVisible = true;
                    _mainForm.chkEnableISPProxyUsageVisible = true;
                    _mainForm.rbRegisterPMVisible = false;

                    if (_appStartUp.GlobalSetting.ifEnableISPIPUseage)
                    {
                        _mainForm.chkEnableISPProxyUsageChecked = true;
                    }
                    else
                    {
                        _mainForm.chkEnableISPProxyUsageChecked = false;
                    }
                    _mainForm.enableISPProxyUsage();

                    if (_appStartUp.GlobalSetting.IfEnablePM)
                    {
                        _mainForm.chkEnableProxyManagerChecked = true;
                    }
                    else
                    {
                        _mainForm.chkEnableProxyManagerChecked = false;
                    }
                    _mainForm.enableProxyManager();

                    if (ifShowMessage)
                    {
                        MessageBox.Show("You request have been approved.", "Congratulations!");
                    }
                }
                else
                {
                    _mainForm.chkEnableProxyManagerVisible = false;
                    _mainForm.chkEnableISPProxyUsageVisible = false;
                    _mainForm.rbRegisterPMVisible = true;
                }
            }
        }

        public void ValidatePPM()
        {
            if (this._appStartUp.GlobalSetting.IfUseSpecialProxies)
            {
                if (_proxyManager != null)
                {
                    _proxyManager.Dispose();
                    _proxyManager = null;
                }
                _proxyManager = new ProxyManager.LuminatiProxyManager(this._appStartUp.Proxies, this.AppStartUp.GlobalSetting.ProxiesCount, this.AppStartUp.GlobalSetting.ProxiesCountry, this._appStartUp.GlobalSetting.CountryUSCA);
            }
            else
            {
                if (_licPPM.ValidateLicense())
                {
                    if (_proxyManager != null)
                    {
                        _proxyManager.Dispose();
                        _proxyManager = null;
                    }
                    if (_proxyManager == null)
                    {
                        _proxyManager = new ProxyManager.ProxyManager(_licPPM.PPMLicenseID, this.AppStartUp.Proxies);
                    }

                    _mainForm.chkEnableProxyManagerVisible = true;
                    _mainForm.chkEnableISPProxyUsageVisible = true;
                    _mainForm.rbRegisterPMVisible = false;

                    if (_appStartUp.GlobalSetting.ifEnableISPIPUseage)
                    {
                        _mainForm.chkEnableISPProxyUsageChecked = true;
                    }
                    else
                    {
                        _mainForm.chkEnableISPProxyUsageChecked = false;
                    }

                    if (_appStartUp.GlobalSetting.IfEnablePM)
                    {
                        _mainForm.chkEnableProxyManagerChecked = true;
                    }
                    else
                    {
                        _mainForm.chkEnableProxyManagerChecked = false;
                    }


                }
                else
                {
                    _mainForm.chkEnableProxyManagerVisible = false;
                    _mainForm.chkEnableISPProxyUsageVisible = false;
                    _mainForm.rbRegisterPMVisible = true;
                }
            }

        }

        void recheckProxiesStatusTimerCallBackHandler(Object stateInfo)
        {
            //Thread th = new Thread(new ParameterizedThreadStart(getNewMyIpsFromServer));
            //th.IsBackground = true;
            //th.SetApartmentState(ApartmentState.STA);
            //th.Priority = ThreadPriority.Highest;
            //th.Start(this.Proxies);
            try
            {
                IEnumerable<Proxy> notWorkingProxies = this._appStartUp.Proxies.Where(p => p.ProxyStatus != Proxy.proxyVerified);
                if (notWorkingProxies != null)
                {
                    if (notWorkingProxies.Count() > 0)
                    {
                        foreach (Proxy proxy in notWorkingProxies)
                        {
                            if (this.ProxyManager != null)
                            {
                                this.ProxyManager.verifyProxies(proxy);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        public bool RecheckProxyStatus(Proxy proxy, String message)
        {
            bool result = false;
            try
            {
                if (this.AppStartUp.GlobalSetting.IfCheckProxiesStatus)
                {

                    if (!String.IsNullOrEmpty(message))
                    {
                        if (proxy != null)
                        {
                            if (proxy.TheProxyType == Proxy.ProxyType.Custom)
                            {
                                if (message.ToLower().Contains("unable to connect to the remote server"))
                                {
                                    proxy.ProxyStatus = Proxy.proxyNotWork;
                                }
                                else if (message.ToLower().Contains("the operation has timed out"))
                                {
                                    proxy.ProxyStatus = Proxy.proxyTimeOut;
                                }
                                else if (message.Contains("403 Forbidden".ToLower()) || message.Contains("You don't have permission to access".ToLower()))
                                {
                                    proxy.ProxyStatus = Proxy.proxyBlocked;
                                }
                                else if (message.Contains("<script type=\"text/javascript\">".ToLower()))
                                {
                                    proxy.ProxyStatus = Proxy.proxyVerified;
                                }
                                else
                                {
                                    proxy.ProxyStatus = Proxy.proxyNotWork;
                                }
                                result = true;
                            }
                            else if ((proxy.TheProxyType == Proxy.ProxyType.Luminati && this._appStartUp.GlobalSetting.IfUseSpecialProxies) || proxy.TheProxyType == Proxy.ProxyType.Relay)
                            {
                                //if (message.ToLower().Contains("unable to connect to the remote server"))
                                //{
                                //    proxy.ProxyStatus = Proxy.proxyNotWork;
                                //}
                                //else if (message.ToLower().Contains("the operation has timed out"))
                                //{
                                //    proxy.ProxyStatus = Proxy.proxyTimeOut;
                                //}
                                //else if (message.Contains("403 Forbidden".ToLower()) || message.Contains("You don't have permission to access".ToLower()))
                                //{
                                //    proxy.ProxyStatus = Proxy.proxyBlocked;
                                //}
                                //else if (message.Contains("<script type=\"text/javascript\">".ToLower()))
                                //{
                                //    proxy.ProxyStatus = Proxy.proxyVerified;
                                //}
                                //else
                                //{
                                //    proxy.ProxyStatus = Proxy.proxyNotWork;
                                //}
                                result = true;
                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }

                }
                else
                {
                    result = true;
                }
            }
            catch { }
            return result;
        }
    }
}
