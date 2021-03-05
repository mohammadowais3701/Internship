using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    public interface ISelectDateForm
    {
        BindingList<ITicketSearch> ParameterQueue
        {
            get;
            set;
        }
        void showForm();
        void hideForm();
    }
}
