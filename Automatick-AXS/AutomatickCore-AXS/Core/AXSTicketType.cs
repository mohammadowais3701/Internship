using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class AXSTicketType
    
    {
       
        public int MinQuantity
        {
            get;
            set;
        }
        public String PasswordFormName
        {
            get;
            set;
        }
        
        public List<AXSPriceLevel> PriceLevels
        {
            get;
            set;
        }
        public String Description
        {
            get;
            set;
        }
        public int MaxQuantity
        {
            get;
            set;
        }
        public int QuantityStep
        {
            get;
            set;
        }
        public Dictionary<String, String> AcceptSplit
        {
            get;
            set;
        }

        
        public AXSTicketType(string description, int quantityStep, int maxQuantity)
        {
            PriceLevels = new List<AXSPriceLevel>();
            this.Description = description;
            this.QuantityStep = quantityStep;
            this.MaxQuantity = maxQuantity;
        }
    }
}
