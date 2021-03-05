using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class AXSTixPriceLevels
    {
        public String PriceLevelID
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public List<AXSTixSection> Sections
        {
            get;
            set;
        }

        public List<AXSTixPrices> Prices
        {
            get;
            set;
        }

        public AXSTixPriceLevels(String id, String name)
        {
            this.PriceLevelID = id;
            this.Name = name;
            this.Sections = new List<AXSTixSection>();
            this.Prices = new List<AXSTixPrices>();
        }
    }

    public class AXSTixResale
    {
        public String SectionLabel { get; set; }
        public String ID { get; set; }
        public String Price { get; set; }

        public AXSTixResale(String label,String id,String price)
        {
            this.SectionLabel = label;
            this.ID = id;
            this.Price = price;
        }
    }
}
