using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotIdGenerator
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
    }

    public class WaitingResponse
    {
        public Boolean isMouseClicked
        {
            get;
            set;
        }

        public Boolean isWaiting
        {
            get;
            set;
        }

        public String LotID
        {
            get;
            set;
        }
    }

}
