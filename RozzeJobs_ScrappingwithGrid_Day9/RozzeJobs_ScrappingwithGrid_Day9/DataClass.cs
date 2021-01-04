using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozzeJobs_ScrappingwithGrid_Day9
{
    class DataClass
    {

        public int jobid
        {
            get;
            set;
        }
        public string title
        {
            get;
            set;

        }
        public string description { get; set; }

        public string displayDate { get; set; }

        public string link { get; set; }

        public string city { get; set; }

        public string country { get; set; }
    }
}
