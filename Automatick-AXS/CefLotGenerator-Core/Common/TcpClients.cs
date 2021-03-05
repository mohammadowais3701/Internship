using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LotGenerator_Core
{
    public class TcpClients
    {
        public TcpClient Client
        {
            get;
            set;
        }

        public String DateTime
        {
            get;
            set;
        }

        public String Key
        {
            get;
            set;
        }

        public String IP
        {
            get;
            set;
        }
    }
}
