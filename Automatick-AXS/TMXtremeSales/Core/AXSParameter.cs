using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class AXSParameter : ITicketParameter
    {
        public String DateTimeString
        {
            get;
            set;
        }

        public String PriceLevelString
        {
            get;
            set;
        }

        public String TicketTypePasssword
        {
            get;
            set;
        }

        public String Section
        {
            get;
            set;
        }

        public String Location
        {
            get;
            set;
        }

        public String Additional
        {
            get;
            set;
        }

        public int? MaxBought
        {
            get;
            set;
        }

        public int? Bought
        {
            get;
            set;
        }

        public int? PriceMin
        {
            get;
            set;
        }

        public int? PriceMax
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }

        public Boolean MaxToMin
        {
            get;
            set;
        }

        public Boolean ExactMatch
        {
            get;
            set;
        }

        public Boolean IfAvailable
        {
            get;
            set;
        }

        public Boolean IfFound
        {
            get;
            set;
        }

        public AXSParameter()
        {
            this.Quantity = 1;
        }
    }
}
