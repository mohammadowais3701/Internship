using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class AXSTixPrices
    {
        public String PriceID
        {
            get;
            set;
        }

        public String Currency
        {
            get;
            set;
        }

        public int tax
        {
            get;
            set;
        }

        public double Price
        {
            get;
            set;
        }

        public List<int> Fees
        {
            get;
            set;
        }

        public string PriceLevelID { get; set; }
    }
}
