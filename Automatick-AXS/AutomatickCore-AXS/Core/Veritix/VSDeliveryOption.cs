using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SortedBindingList;

namespace Automatick.Core
{
    [Serializable]
    public class VSDeliveryOption : ITicketDeliveryOption
    {
        String _DeliveryOptionId="";
        public String DeliveryOptionId
        {
            get
            {
                return this._DeliveryOptionId;
            }
        }
        public String DeliveryOption
        {
            get;
            set;
        }

        //public String DeliveryCountry
        //{
        //    get;
        //    set;
        //}

        public Boolean IfSelected
        {
            get;
            set;
        }

        public VSDeliveryOption()
        {
            this._DeliveryOptionId = UniqueKey.getUniqueKey();
            this.IfSelected = false;
            //this.DeliveryCountry = "";
        }

        public void CheckAndCreateDefaultTicketDeliveryOptions(SortableBindingList<VSDeliveryOption> TicketDeliveryOptions)
        {
            if (TicketDeliveryOptions != null)
            {
                if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Will Call") == null)
                {
                    VSDeliveryOption tmdo = new VSDeliveryOption();
                    tmdo._DeliveryOptionId = "Default";
                    tmdo.DeliveryOption = "Will Call";
                    tmdo.IfSelected = true;
                    TicketDeliveryOptions.Add(tmdo);
                }
                if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Flash Seats") == null)
                {
                    VSDeliveryOption tmdo = new VSDeliveryOption();
                    tmdo._DeliveryOptionId = "Default";
                    tmdo.DeliveryOption = "Flash Seats";
                    tmdo.IfSelected = true;
                    TicketDeliveryOptions.Add(tmdo);
                }
                if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Standard Mail") == null)
                {
                    VSDeliveryOption tmdo = new VSDeliveryOption();
                    tmdo._DeliveryOptionId = "Default";
                    tmdo.DeliveryOption = "Standard Mail";
                    tmdo.IfSelected = true;
                    TicketDeliveryOptions.Add(tmdo);
                }

                if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Express Mail - Within 3 Bus. Days") == null)
                {
                    VSDeliveryOption tmdo = new VSDeliveryOption();
                    tmdo._DeliveryOptionId = "Default";
                    tmdo.DeliveryOption = "Express Mail - Within 3 Bus. Days";
                    tmdo.IfSelected = true;
                    TicketDeliveryOptions.Add(tmdo);
                }

                if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Express Mail - 2nd Day Evening") == null)
                {
                    VSDeliveryOption tmdo = new VSDeliveryOption();
                    tmdo._DeliveryOptionId = "Default";
                    tmdo.DeliveryOption = "Express Mail - 2nd Day Evening";
                    tmdo.IfSelected = true;
                    TicketDeliveryOptions.Add(tmdo);
                }

                if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Express Mail - 2nd Day Morning") == null)
                {
                    VSDeliveryOption tmdo = new VSDeliveryOption();
                    tmdo._DeliveryOptionId = "Default";
                    tmdo.DeliveryOption = "Express Mail - 2nd Day Morning";
                    tmdo.IfSelected = true;
                    TicketDeliveryOptions.Add(tmdo);
                }

                if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Express Mail - Saturday Delivery") == null)
                {
                    VSDeliveryOption tmdo = new VSDeliveryOption();
                    tmdo._DeliveryOptionId = "Default";
                    tmdo.DeliveryOption = "Express Mail - Saturday Delivery";
                    tmdo.IfSelected = true;
                    TicketDeliveryOptions.Add(tmdo);
                }

                if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Print At Home") == null)
                {
                    VSDeliveryOption tmdo = new VSDeliveryOption();
                    tmdo._DeliveryOptionId = "Default";
                    tmdo.DeliveryOption = "Print At Home";
                    tmdo.IfSelected = true;
                    TicketDeliveryOptions.Add(tmdo);
                }
            }
            //if (TicketDeliveryOptions != null)
            //{
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "TicketFast" && p.DeliveryCountry == "USA") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "USA";
            //        tmdo.DeliveryOption = "TicketFast";
            //        tmdo.IfSelected = true;
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "2 Business Day (Evening)" && p.DeliveryCountry == "USA") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "USA";
            //        tmdo.DeliveryOption = "2 Business Day (Evening)";
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "2 Business Day (Morning)" && p.DeliveryCountry == "USA") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "USA";
            //        tmdo.DeliveryOption = "2 Business Day (Morning)";
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "3 Business Day (Evening)" && p.DeliveryCountry == "USA") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "USA";
            //        tmdo.DeliveryOption = "3 Business Day (Evening)";
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Saturday Morning" && p.DeliveryCountry == "USA") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "USA";
            //        tmdo.DeliveryOption = "Saturday Morning";
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Standard Mail" && p.DeliveryCountry == "USA") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "USA";
            //        tmdo.DeliveryOption = "Standard Mail";
            //        tmdo.IfSelected = true;
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Retail Outlet" && p.DeliveryCountry == "USA") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "USA";
            //        tmdo.DeliveryOption = "Retail Outlet";
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "TicketFast" && p.DeliveryCountry == "Canada") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "Canada";
            //        tmdo.DeliveryOption = "TicketFast";
            //        tmdo.IfSelected = true;
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Standard Mail" && p.DeliveryCountry == "Canada") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "Canada";
            //        tmdo.DeliveryOption = "Standard Mail";
            //        tmdo.IfSelected = true;
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "TicketFast" && p.DeliveryCountry == "Great Britain") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "Great Britain";
            //        tmdo.DeliveryOption = "TicketFast";
            //        tmdo.IfSelected = true;
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Standard Post" && p.DeliveryCountry == "Great Britain") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "Great Britain";
            //        tmdo.DeliveryOption = "Standard Post";
            //        tmdo.IfSelected = true;
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Box Office" && p.DeliveryCountry == "Great Britain") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "Great Britain";
            //        tmdo.DeliveryOption = "Box Office";
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "TicketFast" && p.DeliveryCountry == "Other Country") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "Other Country";
            //        tmdo.DeliveryOption = "TicketFast";
            //        tmdo.IfSelected = true;
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Will Call" && p.DeliveryCountry == "Other Country") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "Other Country";
            //        tmdo.DeliveryOption = "Will Call";
            //        tmdo.IfSelected = true;
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Box Office" && p.DeliveryCountry == "Other Country") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "Other Country";
            //        tmdo.DeliveryOption = "Box Office";
            //        TicketDeliveryOptions.Add(tmdo);
            //    }
            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Overseas Mail" && p.DeliveryCountry == "Other Country") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "Other Country";
            //        tmdo.DeliveryOption = "Overseas Mail";
            //        TicketDeliveryOptions.Add(tmdo);
            //    }

            //    if (TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOption == "Print-at-Home" && p.DeliveryCountry == "USA") == null)
            //    {
            //        VSDeliveryOption tmdo = new VSDeliveryOption();
            //        tmdo._DeliveryOptionId = "Default";
            //        tmdo.DeliveryCountry = "USA";
            //        tmdo.DeliveryOption = "Print-at-Home";
            //        tmdo.IfSelected = true;
            //        TicketDeliveryOptions.Insert(0,tmdo);
            //    }
            //}

        }
    }
}
