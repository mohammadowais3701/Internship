using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class VSPriceLevel
    {
        public List<VSSection> Section
        {
            get;
            set;
        }
        public String PriceSecName
        {
            get;
            set;
        }
        public String ID
        {
            get;
            set;
        }
        public String Description
        {
            get;
            set;
        }
        public Decimal Price
        {
            get;
            set;
        }
        public Decimal MaxPrice
        {
            get;
            set;
        }
        public Decimal MinPrice
        {
            get;
            set;
        }
        public VSPriceLevel(String id, String priceSecName, String description, Decimal price, Decimal maxPrice, Decimal minPrice)
        {
            this.PriceSecName = priceSecName;
            this.ID = id;
            this.Description = description;
            this.Price = price;
            this.MaxPrice = maxPrice;
            this.MinPrice = minPrice;
            this.Section = new List<VSSection>();
        }
    }
}
