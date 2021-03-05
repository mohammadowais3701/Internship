using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    public class VSNeighborhood
    {
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
        public String FormElementKey
        {
            get;
            set;
        }

        public Boolean HasPriceLevel
        {
            get
            {
                if (PriceLevels != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            { }
        }

        public List<VSPriceLevel> PriceLevels
        {
            get;
            set;
        }

        public VSNeighborhood(String id, String description, String quantityFormElementKey)
        {
            PriceLevels = new List<VSPriceLevel>();
            this.ID = id;
            this.Description = description;
            this.FormElementKey = quantityFormElementKey;
        }
    }
}
