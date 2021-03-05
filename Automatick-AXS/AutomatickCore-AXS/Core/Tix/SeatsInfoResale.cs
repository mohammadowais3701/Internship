using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class SeatsInfoResale
    {
        public List<ResaleOffer> offers { get; set; }
    }

    public class ResaleItem
    {
        public string rowLabel { get; set; }
        public string sectionLabel { get; set; }
        public string id { get; set; }
        public string number { get; set; }
        public string displayOrder { get; set; }
        public string seatType { get; set; }
        public string priceCodeLabel { get; set; }
        public string priceLevelLabel { get; set; }
        public string neighborhoodLabel { get; set; }
        public string info1 { get; set; }
        public string info2 { get; set; }
        public string sellerName { get; set; }
        public double originalPrice { get; set; }
    }

    public class ResaleOffer
    {
        public string category { get; set; }
        public string offerID { get; set; }
        public string itemTypeID { get; set; }
        public List<ResaleItem> items { get; set; }
        public string eventID { get; set; }
        public string sellerNotes { get; set; }
        public List<int> purchasableQuantityList { get; set; }
        public string purchasableQuantityRule { get; set; }
        public string offerType { get; set; }
    }
}
