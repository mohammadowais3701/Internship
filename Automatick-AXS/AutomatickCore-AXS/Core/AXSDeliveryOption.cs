using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SortedBindingList;

namespace Automatick.Core
{
    [Serializable]
    public class AXSDeliveryOption : ITicketDeliveryOption
    {
        String _DeliveryOptionId="";
        public String DeliveryOptionId
        {
            get
            {
                return this._DeliveryOptionId;
            }
            set { this._DeliveryOptionId = value; }
        }
        public String DeliveryOption
        {
            get;
            set;
        }

        public String DeliveryCountry
        {
            get;
            set;
        }

        public Boolean IfSelected
        {
            get;
            set;
        }

        public AXSDeliveryOption()
        {
            this._DeliveryOptionId = UniqueKey.getUniqueKey();
            this.IfSelected = false;
            this.DeliveryCountry = "";
        }

        public void CheckAndCreateDefaultTicketDeliveryOptions(SortableBindingList<AXSDeliveryOption> TicketDeliveryOptions)
        {
            if (TicketDeliveryOptions != null)
            {
                if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Electronic Ticket" && p.DeliveryCountry == "UK") == null)
                {
                    AXSDeliveryOption tmdo = new AXSDeliveryOption();
                    tmdo._DeliveryOptionId = "Default";
                    tmdo.DeliveryCountry = "UK";
                    tmdo.DeliveryOption = "Electronic Ticket";
                    tmdo.IfSelected = true;
                    TicketDeliveryOptions.Add(tmdo);
                }
                if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Standard Mail" && p.DeliveryCountry == "UK") == null)
                {
                    AXSDeliveryOption tmdo = new AXSDeliveryOption();
                    tmdo._DeliveryOptionId = "Default";
                    tmdo.DeliveryCountry = "UK";
                    tmdo.DeliveryOption = "Standard Mail";
                   // TicketDeliveryOptions.Add(tmdo);
                }
                if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Will Call" && p.DeliveryCountry == "UK") == null)
                {
                    AXSDeliveryOption tmdo = new AXSDeliveryOption();
                    tmdo._DeliveryOptionId = "Default";
                    tmdo.DeliveryCountry = "UK";
                    tmdo.DeliveryOption = "Will Call";
                  //  TicketDeliveryOptions.Add(tmdo);
                }
            
            }

        }
    }
}
