using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class VSSection
    {
        public String ID
        {
            get;
            set;
        }
        public String SecName
        {
            get;
            set;
        }
        public VSSection(String id, String secName)
        {
            this.ID = id;
            this.SecName = secName;
        }
    }
}
