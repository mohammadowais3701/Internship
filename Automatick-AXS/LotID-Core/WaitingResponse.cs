using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotID_Core
{
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

        public Boolean isHashGenerated
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
