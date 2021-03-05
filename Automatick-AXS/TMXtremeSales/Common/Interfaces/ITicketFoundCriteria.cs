using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    public interface ITicketFoundCriteria
    {
        String RowFrom
        {
            get;
            set;
        }

        String RowTo
        {
            get;
            set;
        }

        String Section
        {
            get;
            set;
        }
    }
}
