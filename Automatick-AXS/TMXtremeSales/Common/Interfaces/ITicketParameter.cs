using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    public interface ITicketParameter
    {
        String DateTimeString
        {
            get;
            set;
        }
        String PriceLevelString
        {
            get;
            set;
        }

        String TicketTypePasssword
        {
            get;
            set;
        }

        String Section
        {
            get;
            set;
        }

        String Location
        {
            get;
            set;
        }

        String Additional
        {
            get;
            set;
        }

        int? MaxBought
        {
            get;
            set;
        }

        int? Bought
        {
            get;
            set;
        }

        int? PriceMin
        {
            get;
            set;
        }

        int? PriceMax
        {
            get;
            set;
        }

        int Quantity
        {
            get;
            set;
        }

        Boolean MaxToMin
        {
            get;
            set;
        }

        Boolean ExactMatch
        {
            get;
            set;
        }

        Boolean IfAvailable
        {
            get;
            set;
        }

        Boolean IfFound
        {
            get;
            set;
        }
    }
}
