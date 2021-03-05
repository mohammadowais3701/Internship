using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class Email
    {
        String _EmailId = "";
        public Boolean IsSelected
        {
            get;
            set;
        }

        public String EmailId
        {
            get
            {
                return this._EmailId;
            }
        }

        public String EmailAddress
        {
            get;
            set;
        }

        public Email()
        {
            this._EmailId = UniqueKey.getUniqueKey();
        }
    }
}
