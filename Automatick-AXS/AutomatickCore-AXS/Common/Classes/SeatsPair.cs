using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public struct SeatsPair
    {
        public String SectionID { get; set; }
        public String RowID { get; set; }
        public String Row { get; set; }
        public String PriceLevelID { get; set; }
        public String SeatType { get; set; }
        public String sectionLabel { get; set; }

        public List<int> Seats { get; set; }
    }
}
