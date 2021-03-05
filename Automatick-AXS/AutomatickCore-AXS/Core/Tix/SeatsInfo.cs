using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class SeatsInfo
    {
        public List<Offer> offers { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public string sectionID { get; set; }
        public string rowID { get; set; }
        public string rowLabel { get; set; }
        public string number { get; set; }
        public string priceLevelID { get; set; }
        public List<string> priceTypeIDs { get; set; }
        public string seatType { get; set; }
        public string sectionLabel { get; set; }
    }

    public class Quantity
    {
        public int min { get; set; }
        public int max { get; set; }
        public int selected { get; set; }
    }

    public class Offer
    {
        public string category { get; set; }
        public string offerGroupID { get; set; }
        public string offerID { get; set; }
        public string itemParentTypeID { get; set; }
        public string itemTypeID { get; set; }
        public List<Item> items { get; set; }
        public string eventID { get; set; }
        public Quantity quantity { get; set; }
        public string productID { get; set; }
    }
}
