using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SortedBindingList;

namespace Automatick.Core
{
    [Serializable]
    public class TicketGroup
    {
        String _GroupId = "";
        public String GroupId
        {
            get 
            {
                return this._GroupId;
            }
        }

        public String GroupName
        {
            get;
            set;
        }
        
        public TicketGroup()
        {
            this._GroupId = UniqueKey.getUniqueKey();
        }

        public void CheckAndCreateDefaultTicketGroup(SortableBindingList<TicketGroup> Groups)
        {
            TicketGroup tg = new TicketGroup();
                        
            tg._GroupId = "Default Group";
            tg.GroupName = "Default Group";

            if (Groups.FirstOrDefault(p => p.GroupId == "Default Group" && p.GroupName=="Default Group") == null)
            {
                Groups.Insert(0, tg);
            }

            tg = new TicketGroup();
            tg._GroupId = "Default Group";
            tg.GroupName = "All Groups";

            if (Groups.FirstOrDefault(p => p.GroupId == "Default Group" && p.GroupName == "All Groups") == null)
            {
                Groups.Insert(0, tg);
            }
        }
        public override string ToString()
        {
            return this.GroupName;
        }
    }
}
