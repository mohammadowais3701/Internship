using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    public interface IAddTicket
    {
        ITicket Ticket
        {
            get;
            set;
        }
        IMainForm MainForm
        {
            get;
            set;
        }
        Boolean validate();
        void load();
        void save();
        void onClosed();
    }
}
