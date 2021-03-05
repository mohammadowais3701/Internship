using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class AXSSection
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
        public String EventDates
        {
            get;
            set;
        }
        public List<AXSPriceLevel> PriceLevels
        {
            get;
            set;
        }
        public AXSSection(String eventTypeCode, String eventCode, String eventDates,List<AXSPriceLevel> priceLevels)
        {
            this.EventTypeCode = eventTypeCode;
            this.EventCode = eventCode;
            this.EventDates = eventDates;
            this.PriceLevels = priceLevels;
           
            
        }
    }
}
