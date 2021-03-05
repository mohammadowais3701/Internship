using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    [Serializable]
    public class MultiEvent
    {
        public String EventTypeCode
        {
            get;
            set;
        }

        public String EventCode
        {
            get;
            set;
        }

        public String EventDate
        {
            get;
            set;
        }

        public String EventTime
        {
            get;
            set;
        }

        public Boolean isSoldOut
        {
            get;
            set;
        }
        public List<AXSPriceLevel> PriceLevels
        {
            get;
            set;
        }
        public MultiEvent(String eventTypeCode, String eventCode, String eventDate, String eventTime)
        {
            this.EventTypeCode = eventTypeCode;
            this.EventCode = eventCode;
            this.EventDate = eventDate;
            this.EventTime = eventTime;
        }

        public MultiEvent()
        { }
    }
}
