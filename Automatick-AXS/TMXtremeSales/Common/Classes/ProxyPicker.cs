using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SortedBindingList;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace Automatick.Core
{
    public class ProxyPicker
    {
        static ProxyPicker _proxyPicker = null;
        ApplicationStartUp _appStartUp = null;
        int _currentProxyIndex = 0;
        private LicenseCorePPM _licPPM = null;
        private ProxyManager.ProxyManager _proxyManager = null;        
        frmMain _mainForm = null;


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

        public static void createProxyPickerInstance(frmMain mainForm)
        {
            _proxyPicker = new ProxyPicker(mainForm);
        }

        private ProxyPicker(frmMain mainForm)
        {
            this._mainForm = mainForm;
            this._appStartUp = mainForm.AppStartUp;
            this.InitializePPM();
        }

        public Proxy getNextProxy(ITicketSearch search)
        {
            Proxy proxy = null;
                        
            if (this._appStartUp.Proxies!= null)
            {
                if (this._appStartUp.Proxies.Count > 0)
                {
                    if (_appStartUp.GlobalSetting.IfEnablePM && _licPPM.IsValidated)
                    {
                        this._proxyManager.ReleaseProxy(search.Proxy);
                        proxy = this._proxyManager.getNextProxy();
                    }
                    else
                    {
                        for (int i = 0; i < this._appStartUp.Proxies.Count; i++)
                        {
                            if (this._currentProxyIndex >= this._appStartUp.Proxies.Count)
                            {
                                this._currentProxyIndex = 0;
                            }

                            proxy = this._appStartUp.Proxies[_currentProxyIndex];
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
                    }                    
                }
            }

            return proxy;
        }

        public Boolean ifSearchAllowed(ITicketSearch search)
        {
            Boolean result = false;
            if (_appStartUp.GlobalSetting.IfEnablePM && search.IfUseProxy && this.LicPPM != null)
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

        internal void ValidatePPM(Boolean ifShowMessage)
        {
            if (_licPPM.ValidateLicense())
            {
                if (_proxyManager != null)
                {
                    _proxyManager.Dispose();
                    _proxyManager = null;
                }
                _proxyManager = new ProxyManager.ProxyManager(_licPPM.PPMLicenseID, this._appStartUp.Proxies);

                _mainForm.chkEnableProxyManager.Visible = true;
                _mainForm.chkEnableISPProxyUsage.Visible = true;
                _mainForm.rbRegisterPM.Visible = false;

                if (_appStartUp.GlobalSetting.ifEnableISPIPUseage)
                {
                    _mainForm.chkEnableISPProxyUsage.Checked = true;                    
                }
                else
                {
                    _mainForm.chkEnableISPProxyUsage.Checked = false;
                }
                _mainForm.enableISPProxyUsage();

                if (_appStartUp.GlobalSetting.IfEnablePM)
                {
                    _mainForm.chkEnableProxyManager.Checked = true;
                }
                else
                {
                    _mainForm.chkEnableProxyManager.Checked = false;
                }
                _mainForm.enableProxyManager();

                if (ifShowMessage)
                {
                    MessageBox.Show("You request have been approved.", "Congratulations!");
                }

            }
            else
            {
                _mainForm.chkEnableProxyManager.Visible = false;
                _mainForm.chkEnableISPProxyUsage.Visible = false;
                _mainForm.rbRegisterPM.Visible = true;
            }
        }

        internal void ValidatePPM()
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

                _mainForm.chkEnableProxyManager.Visible = true;
                _mainForm.chkEnableISPProxyUsage.Visible = true;
                _mainForm.rbRegisterPM.Visible = false;

                if (_appStartUp.GlobalSetting.ifEnableISPIPUseage)
                {
                    _mainForm.chkEnableISPProxyUsage.Checked = true;
                }
                else
                {
                    _mainForm.chkEnableISPProxyUsage.Checked = false;
                }

                if (_appStartUp.GlobalSetting.IfEnablePM)
                {
                    _mainForm.chkEnableProxyManager.Checked = true;
                }
                else
                {
                    _mainForm.chkEnableProxyManager.Checked = false;
                }


            }
            else
            {
                _mainForm.chkEnableProxyManager.Visible = false;
                _mainForm.chkEnableISPProxyUsage.Visible = false;
                _mainForm.rbRegisterPM.Visible = true;
            }
        }

    }
}
