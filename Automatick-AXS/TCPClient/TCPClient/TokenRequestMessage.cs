using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCPClient;

namespace Automatick.Core
{
    public class TokenRequestMessage
    {
        private String type;
        private ClientProxy proxy;
        private String command;
        private List<ClientProxy> proxies;

        public ClientProxy Proxy
        {
            get { return proxy; }
            set { proxy = value; }
        }
        public String Type
        {
            get { return type; }
            set { type = value; }
        }
        public String Command
        {
            get { return command; }
            set { command = value; }
        }

        public List<ClientProxy> Proxies
        {
            get { return proxies; }
            set { proxies = value; }
        }
    }
}
