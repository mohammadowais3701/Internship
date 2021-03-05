using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Automatick.Core
{
    public interface ICaptchaForm
    {
        BindingList<ITicketSearch> CaptchaQueue
        {
            get;
            set;
        }
        void showForm();
        void hideForm();
    }
}
