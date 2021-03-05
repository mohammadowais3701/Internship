using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Net;

namespace Automatick.Core
{
    [Serializable]
    public class Proxy
    {
        public enum ProxyType
        {
            MyIP = 0,
            Custom = 1,
            ISPIP = 2
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
        String userName = string.Empty;
        String password = string.Empty;

        public Proxy()
        {
            _proxyStatus = proxyNotVerified;
            _ProxyResponseTime = 0;
            _ProxyType = ProxyType.Custom;
        }

        public String ProxyStatus
        {
            get { return _proxyStatus; }
            set
            {
                _proxyStatus = value;
            }
        }

        public override string ToString()
        {
            string strToString = "";
            try
            {
                strToString = (this.Address + ":" + this.Port);
                if (this.TheProxyType == ProxyType.MyIP)
                {
                    strToString = "127.0.0.1:8081";
                }
            }
            catch (Exception)
            {

            }


            return strToString;
        }

        public string ToStringForManualBuy()
        {
            String proxy = "";
            try
            {
                if (!String.IsNullOrEmpty(this.Address) && !String.IsNullOrEmpty(this.Port.ToString()))
                {
                    proxy = this.Address + ":" + this.Port.ToString();
                   
                    if (!String.IsNullOrEmpty(this.UserName) && !String.IsNullOrEmpty(this.Password) && this.TheProxyType == ProxyType.MyIP)
                    {
                        proxy = proxy + ":" + this.UserName + ":" + this.Password;
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
            get { return userName; }
            set { userName = value; }
        }

        public String Password
        {
            get { return password; }
            set { password = value; }
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

        public WebProxy toWebProxy()
        {
            WebProxy webProxy = null;
            if (this.TheProxyType != ProxyType.ISPIP)
            {
                webProxy = new WebProxy("http://" + this.address + ":" + this.port);
                if (!String.IsNullOrEmpty(this.userName) && !String.IsNullOrEmpty(this.password))
                {
                    webProxy.Credentials = new NetworkCredential(this.userName, this.password);
                }
            }
            return webProxy;
        }

    }

}