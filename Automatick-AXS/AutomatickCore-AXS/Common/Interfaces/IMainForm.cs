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
        ICaptchaForm CaptchaForm
        {
            get;
            set;
        }
        ICaptchaForm CaptchaBrowserForm
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

        Boolean chkEnableLuminatiProxiesChecked
        {
            get;
            set;
        }

        LicenseCore Lic
        {
            get;
            set;
        }

        void startTixTox();
        void customBuildCheck();
        void stopTixTox();
        void loadTickets();
        void loadGroups();
        void loadProxies();
        void loadAccounts();
        void loadEmails();
        void loadDeliveryOptions();
        void loadAutoCaptchaServices();
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
