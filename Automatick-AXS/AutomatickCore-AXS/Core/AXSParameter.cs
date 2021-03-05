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

        public String EventTime
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

        public String TicketType
        {
            get;
            set;
        }

        public Boolean AcceptSplit
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

        public String OfferName
        {
            get;
            set;
        }

        public Boolean LowestPrice
        {
            get;
            set;
        }

        public Boolean TopPrice
        {
            get;
            set;
        }

        public Boolean GetResaleTix
        {
            get;
            set;
        }

        public AXSParameter()
        {
            this.Quantity = 1;
        }

        public AXSParameter getSharedObject()
        {
            AXSParameter parameter = new AXSParameter();

            try
            {
                parameter.DateTimeString = this.DateTimeString;
                parameter.EventTime = this.EventTime;
                parameter.PriceLevelString = this.PriceLevelString;
                parameter.TicketTypePasssword = this.TicketTypePasssword;
                parameter.TicketType = this.TicketType;
                parameter.AcceptSplit = this.AcceptSplit;
                parameter.PriceMin = this.PriceMin;
                parameter.PriceMax = this.PriceMax;
                parameter.Quantity = this.Quantity;
                parameter.MaxToMin = this.MaxToMin;
                parameter.ExactMatch = this.ExactMatch;
                parameter.IfAvailable = this.IfAvailable;
                parameter.IfFound = this.IfFound;
                parameter.OfferName = this.OfferName;
                parameter.IfAvailable = this.IfAvailable;
                parameter.LowestPrice = this.LowestPrice;
                parameter.TopPrice = this.TopPrice;
                parameter.GetResaleTix = this.GetResaleTix;
            }
            catch { parameter = null; }

            return parameter;
        }
    }
}