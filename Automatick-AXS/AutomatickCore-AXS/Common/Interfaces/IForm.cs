﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    public interface IForm
    {
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
