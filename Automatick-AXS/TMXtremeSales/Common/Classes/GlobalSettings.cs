using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class GlobalSettings
    {
        public Boolean IfEnablePM
        {
            get;
            set;
        }

        public Boolean ifEnableISPIPUseage
        {
            get;
            set;
        }

        public GlobalSettings()
        {
            this.IfEnablePM = true;
            this.ifEnableISPIPUseage = false;
        }
    }
}
