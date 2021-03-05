using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
    public class StokenRequest
    {

        public String Command
        {
            get;
            set;
        }
        public String Stoken
        {
            get;
            set;
        }

        public Boolean IsStokenRequired
        {
            get;
            set;
        }

    }
}
