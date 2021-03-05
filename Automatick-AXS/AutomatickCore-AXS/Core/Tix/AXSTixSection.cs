using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class AXSTixSection
    {
        public String ID
        {
            get;
            set;
        }

        public String Label
        {
            get;
            set;
        }

        public int SeatsAvailable
        {
            get;
            set;
        }

        public List<String> PriceTypeID
        {
            get;
            set;
        }

        public AXSTixSection(String id, String label, int seats, List<String> priceTypeID = null)
        {
            this.ID = id;
            this.Label = label;
            this.SeatsAvailable = seats;
            this.PriceTypeID = priceTypeID;
        }
    }
}
