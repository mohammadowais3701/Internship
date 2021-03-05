using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace Automatick.Core
{
    [Serializable]
    public class Proxy
    {
        public enum ProxyType
        {
            MyIP = 0,
            Custom = 1,
            ISPIP = 2,
            Luminati = 3,
            Relay = 4,
            All = 5
        }

        public enum ProxyPriority
        {
            TicketFound = 0,
            FirstPage = 1,
            Use = 2,
            Default = 3
        }

        public const string proxyNotVerified = "Not Verified";
        public const string proxyNotWork = "Not Working";
        public const string proxyVerified = "Verified";
        public const string proxyBlocked = "Blocked";
        public const string proxyTimeOut = "Time Out";
        public const string proxyVerifying = "Verifying";
        public const string proxyWaiting = "Waiting";
        private String address = string.Empty;
        private String port = string.Empty;
        String _proxyStatus = string.Empty;
        int _ProxyResponseTime = 0;
        DateTime _ExpiryTime = DateTime.Now;
        ProxyType _ProxyType = ProxyType.Custom;
        public String userName = string.Empty;
        String password = string.Empty;
        String _luminatiSessionId = String.Empty;
        Boolean _IfLuminatiProxy = false;
        String _luminatiProxiesCountry = String.Empty;
        private int _failCount = 0;
        ProxyPriority _ProxySortOrder = ProxyPriority.Default;
        private String reportingPort = String.Empty;
        public String ReportingPort
        {
            get { return reportingPort; }
            set { reportingPort = value; }
        }

        public ProxyPriority ProxySortOrder
        {
            get { return _ProxySortOrder; }
            set { _ProxySortOrder = value; }
        }

        public int FailCount
        {
            get { return _failCount; }
            set { _failCount = value; }
        }

        public String LuminatiProxiesCountry
        {
            get { return _luminatiProxiesCountry; }
        }

        public Boolean IfLuminatiProxy
        {
            get { return _IfLuminatiProxy; }
        }

        public Boolean IfValidLuminatiProxy
        {
            get
            {
                Boolean result = false;
                IPAddress ip = null;
                if (IPAddress.TryParse(this.address, out ip))
                {
                    if (ip != null)
                    {
                        result = true;
                    }
                }

                return result;
            }
        }
        public Proxy()
        {
            _proxyStatus = proxyNotVerified;
            _ProxyResponseTime = 0;
            _ProxyType = ProxyType.Custom;
            //this.address = "127.0.0.1";

            _luminatiProxiesCountry = String.Empty;
            _IfLuminatiProxy = false;
        }

        public Proxy(string luminatiCountry)
        {
            _proxyStatus = proxyNotVerified;
            _ProxyResponseTime = 0;
            _ProxyType = ProxyType.Luminati;

            _luminatiProxiesCountry = luminatiCountry;
            _IfLuminatiProxy = true;
            generateLuminatiSessionId();
        }

        public String ProxyStatus
        {
            get { return _proxyStatus; }
            set
            {
                _proxyStatus = value;
            }
        }

        public String setSessionIdInBrowserSession()
        {
            try
            {

                this.generateLuminatiSessionId();

                if (this.TheProxyType != ProxyType.Relay)
                {
                    if (!String.IsNullOrEmpty(this._luminatiSessionId))
                    {
                        return this._luminatiSessionId;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public void generateLuminatiSessionId()
        {
            try
            {
                this._luminatiSessionId = UniqueKey.getUniqueKey();
            }
            catch (Exception)
            {

            }
        }
        public override string ToString()
        {
            string strToString = "";
            try
            {
                strToString = (this.Address + ":" + this.Port);
                if (this.TheProxyType != ProxyType.Custom)
                {
                    if (this.TheProxyType == ProxyType.Luminati)
                    {
                        //strToString = "127.0.0.1:8082";
                        strToString = "SP";
                    }
                    else if (this.TheProxyType == ProxyType.Relay)
                    {
                        //strToString = "127.0.0.1:8081";
                        strToString = "R";
                    }
                    else
                    {
                        //strToString = "127.0.0.1:8081";
                        strToString = "PM";
                    }
                }

            }
            catch (Exception)
            {

            }


            return strToString;
        }

        public string ToStringForManualBuy(String context = null)
        {
            String proxy = "";
            try
            {
                if (!String.IsNullOrEmpty(this.Address) && !String.IsNullOrEmpty(this.Port.ToString()))
                {
                    proxy = this.Address + ":" + this.Port.ToString();

                    if (!String.IsNullOrEmpty(this.userName) && !String.IsNullOrEmpty(this.Password) && (this.TheProxyType == ProxyType.MyIP || this.TheProxyType == ProxyType.Relay || this.TheProxyType == ProxyType.Luminati))
                    {
                        if (this.TheProxyType != ProxyType.Relay)
                        {
                            proxy = proxy + ":" + this.UserName + ":" + this.Password;
                        }
                        else if (this.TheProxyType == ProxyType.Relay)
                        {
                            proxy = proxy + ":" + this.userName + "-context-" + context + "-session-" + this._luminatiSessionId + ":" + this.Password;
                        }
                    }
                }
            }
            catch (Exception)
            {
                proxy = "";
            }
            return proxy;
        }

        public string Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;
            }
        }

        public String Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }
        public String UserName
        {
            get
            {
                if (this._IfLuminatiProxy)
                {
                    String luminatUserName = String.Empty;
                    if (this._luminatiProxiesCountry != "any")
                    {
                        luminatUserName = userName + "-country-" + this._luminatiProxiesCountry + "-session-" + this._luminatiSessionId;
                    }
                    else
                    {
                        luminatUserName = userName + "-session-" + this._luminatiSessionId;
                    }

                    return luminatUserName;
                }
                else
                {
                    return userName;
                }
            }
            set { userName = value; }
        }

        public String Password
        {
            get { return password; }
            set { password = value; }
        }
        public String LuminatiSessionId
        {
            get { return _luminatiSessionId; }
            set { _luminatiSessionId = value; }
        }

        public int ProxyResponseTime
        {
            get { return _ProxyResponseTime; }
            set
            {
                _ProxyResponseTime = value;
            }
        }
        public DateTime LastUsed
        {
            get;
            set;
        }
        public ProxyType TheProxyType
        {
            get { return _ProxyType; }
            set { _ProxyType = value; }
        }

        public WebProxy toWebProxy(String context = null)
        {
            WebProxy webProxy = null;
            if (this.TheProxyType != ProxyType.Relay)
            {
                if (this.TheProxyType != ProxyType.ISPIP)
                {
                    webProxy = new WebProxy("http://" + this.address + ":" + this.port);
                    if (!String.IsNullOrEmpty(this.UserName) && !String.IsNullOrEmpty(this.Password))
                    {
                        webProxy.Credentials = new NetworkCredential(this.UserName, this.Password);
                    }
                }
            }
            else
            {
                if (this.TheProxyType == ProxyType.Relay)
                {
                    webProxy = new WebProxy("http://" + this.address + ":" + this.port);

                    if (String.IsNullOrEmpty(this._luminatiSessionId)) this.generateLuminatiSessionId();

                    if (!String.IsNullOrEmpty(this.userName) && !String.IsNullOrEmpty(this.Password))
                    {
                        webProxy.Credentials = new NetworkCredential(this.userName + "-context-" + context + "-session-" + this._luminatiSessionId, this.Password);
                    }
                }
            }
            return webProxy;
        }

        public void incrementFailCount()
        {
            try
            {
                Interlocked.Increment(ref this._failCount);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }

}