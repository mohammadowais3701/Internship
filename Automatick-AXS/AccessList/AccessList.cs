using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccessRights
{
    [Serializable]
    public class AccessList
    {
        #region Members
        public String name;
        public String form;
        public String access;
        #endregion

        #region Contstructor
        public AccessList()
        {
        }

        public AccessList(String name, String form, String access)
        {
            this.name = name;
            this.form = form;
            this.access = access;
        }
        #endregion
    }   
}
