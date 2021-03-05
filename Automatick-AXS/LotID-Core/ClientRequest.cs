using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LotID_Core
{
    public class ClientRequest
    {
        public String WR
        {
            get;
            set;
        }

        public String URL
        {
            get;
            set;
        }

        public String AppPrefix
        {
            get;
            set;
        }

        public String EventID
        {
            get;
            set;
        }

        public String referer
        {
            get;
            set;
        }

        public List<Cookie> cookies 
        { 
            get;
            set; 
        }
    }

    public class BrowserRequest
    {
        public String ProcessID
        {
            get;
            set;
        }

        public Boolean IsMinimized
        {
            get;
            set;
        }

        public String WaitingURL
        {
            get;
            set;
        }
    }
}
