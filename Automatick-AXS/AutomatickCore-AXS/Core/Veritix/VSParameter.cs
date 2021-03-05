using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class VSParameter : ITicketParameter
    {
        public String TicketTypeTitle
        {
            get;
            set;
        }

        public String TicketTypeString
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

        public String PriceLevel
        {
            get;
            set;
        }

        public String Neighbourhood
        {
            get;
            set;
        }

        //public String Additional
        //{
        //    get;
        //    set;
        //}

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

        public string Date
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

        public VSParameter()
        {
            this.Quantity = 1;
        }
    }
}
