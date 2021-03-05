using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccessRights;

namespace Automatick.Core
{
    public interface IMainForm
    {
        ApplicationPermissions AppPermissions
        {
            get;
        }

        ApplicationStartUp AppStartUp
        {
            get;
            set;
        }

        Boolean chkEnableProxyManagerVisible
        {
            get;
            set;
        }
        Boolean chkEnableISPProxyUsageVisible
        {
            get;
            set;
        }
        Boolean rbRegisterPMVisible
        {
            get;
            set;
        }
        Boolean chkEnableISPProxyUsageChecked
        {
            get;
            set;
        }
        Boolean chkEnableProxyManagerChecked
        {
            get;
            set;
        }
        void loadTickets();
        void loadGroups();
        void loadProxies();
        void loadAccounts();
        void loadEmails();
        void loadDeliveryOptions();
        void populateTickets();
        void populateGroups();
        void populateRecentTickets();
        void onChangeHandlerStartOrStop(ITicket ticket);
        void scheduleTickets();
        void runScheduledTicket(object ticket);
        void stopScheduledTicket(object ticket);
        void enableProxyManager();
        void enableISPProxyUsage();
    }
}
