using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SortedBindingList;

namespace Automatick.Core
{
    public interface ITicketDeliveryOption
    {
        String DeliveryOptionId
        {
            get;
        }

        String DeliveryOption
        {
            get;
            set;
        }

        String DeliveryCountry
        {
            get;
            set;
        }

        Boolean IfSelected
        {
            get;
            set;
        }

    }
}
