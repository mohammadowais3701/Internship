using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
    public class TokenBucket
    {
        string _recaptoken;
        string _error;
        DateTime _expiretime;
        ClientProxy _proxy;

        public ClientProxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }


        public DateTime Expiretime
        {
            get { return _expiretime; }
            set { _expiretime = value; }
        }

        public string Recaptoken
        {
            get { return _recaptoken; }
            set { _recaptoken = value; }
        }

        public string Error
        {
            get { return _error; }
            set { _error = value; }
        }
    }
}
