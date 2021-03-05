using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class AXSPriceLevel
    {
        public AXSSection Section
        {
            get;
            set;
        }
        public String PriceSecName
        {
            get;
            set;
        }
        public String PriceSectionNumber
        {
            get;
            set;
        }
        public decimal TotalPrice
        {
            get;
            set;
        }

        public Boolean ifChecked
        {
            get;
            set;
        }
        public AXSPriceLevel(String priceSecName, String priceSecNumber,decimal totalPrice)
        {
            this.PriceSecName = priceSecName;
            this.PriceSectionNumber = priceSecNumber;
            this.TotalPrice = totalPrice;
            this.ifChecked = false;
            
        }
     
    }
}
