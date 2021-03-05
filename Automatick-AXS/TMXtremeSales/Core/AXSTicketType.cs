using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class AXSTicketType
    {
        public String V
        {
            get;
            set;
        }
        public String Description
        {
            get;
            set;
        }
        public int MinQuantity
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
        public String PasswordFormName
        {
            get;
            set;
        }
        public String PasswordPrompt
        {
            get;
            set;
        }
        public List<AXSPriceLevel> PriceLevels
        {
            get;
            set;
        }

        public AXSTicketType(String v, String description, int minQuantity, int maxQuantity, int quantityStep)
        {
            PriceLevels = new List<AXSPriceLevel>();
            this.V = v;
            this.Description = description;
            this.MinQuantity = minQuantity;
            this.MaxQuantity = maxQuantity;
            this.QuantityStep = quantityStep;
            this.PasswordFormName = String.Empty;
            this.PasswordPrompt = String.Empty;
        }
    }
}
