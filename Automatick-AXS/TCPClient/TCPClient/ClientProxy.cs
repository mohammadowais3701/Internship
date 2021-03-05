using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;

namespace TCPClient
{
    [Serializable]
    public class ClientProxy
    {
        private String host;
        private String port;
        private String username;
        private String password;
        private String command;
        private String error;
        String _luminatiSessionId = String.Empty;
        ProxyType _proxyType;

        public String Host
        {
            get { return host; }
            set { host = value; }
        }

        public String Port
        {
            get { return port; }
            set { port = value; }
        }

        public String Username
        {
            get { return username; }
            set { username = value; }
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
        public  ProxyType TypeProxy
        {
            get { return _proxyType; }
            set { _proxyType = value; }
        }
        public String Command
        {
            get { return command; }
            set { command = value; }
        }
        public String Error
        {
            get { return error; }
            set { error = value; }
        }

        public ClientProxy()
        {

        }
        public ClientProxy(String _error)
        {
            this.error = _error;
        }

        public ClientProxy(String host, String port, String username, String password)
        {
            this.host = host;
            this.port = port;
            this.username = username;
            this.password = password;
        }

        public WebProxy toWebProxy()
        {
            WebProxy webProxy = null;
            webProxy = new WebProxy("http://" + this.host + ":" + this.port);
            if (!String.IsNullOrEmpty(this.username) && !String.IsNullOrEmpty(this.password))
            {
                webProxy.Credentials = new NetworkCredential(this.username, this.password);
            }

            return webProxy;
        }

        public bool IfValidLuminatiProxy { get; set; }
    }

    public enum ProxyType
    {
        MyIP = 0,
        Custom = 1,
        ISPIP = 2,
        Luminati = 3
    }
}
