using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotGenerator_Core
{
    public class BrowserRequest
    {
        public String ID
        {
            get;
            set;
        }

        public String Command
        {
            get;
            set;
        }

        public String URL
        {
            get;
            set;
        }

        public String LotID
        {
            get;
            set;
        }

        public String AppPrefix
        {
            get;
            set;
        }

        public int BrowserCount
        {
            get;
            set;
        }

        public String Proxy
        {
            get;
            set;
        }
    }
}
