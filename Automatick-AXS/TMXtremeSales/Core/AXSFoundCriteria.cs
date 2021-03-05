using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class AXSFoundCriteria : ITicketFoundCriteria
    {
        public String RowFrom
        {
            get;
            set;
        }

        public String RowTo
        {
            get;
            set;
        }

        public String Section
        {
            get;
            set;
        }
        
        public AXSFoundCriteria()
        {

        }
    }
}
