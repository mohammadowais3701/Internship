using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SortedBindingList;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public delegate void ChangeDelegate(ITicket ticket);
    public delegate void ChangeDelegateForGauge();
    public interface ITicket
    {
        String WR
        {
            get;
            set;
        }

        String EventID
        {
            get;
            set;
        }

        Boolean isServiceSelected
        {
            get;
            set;
        }

        Boolean isPasswordHandled
        {
            get;
            set;
        }

        Boolean isWrUpdated
        {
            get;
            set;
        }

        Boolean autobuytry2
        {
            get;
            set;
        }
        Boolean autobuytry3
        {
            get;
            set;
        }
        ChangeDelegate onStartSearching
        {
            get;
            set;
        }
        ChangeDelegate onFound
        {
            get;
            set;
        }
        ChangeDelegate onNotFound
        {
            get;
            set;
        }
        ChangeDelegate onChangeStartOrStop
        {
            get;
            set;
        }

        ChangeDelegateForGauge onChangeForGauge
        {
            get;
            set;
        }

        String TicketID
        {
            get;
            set;
        }

        String TicketName
        {
            get;
            set;
        }

        String URL
        {
            get;
            set;
        }
        String constTicketURL { get; set; }

        String OldURL
        {
            get;
            set;
        }

        String QueueURL
        {
            get;
            set;
        }

        String FileLocation
        {
            get;
            set;
        }

        String DeliveryOption
        {
            get;
            set;
        }

        String DeliveryCountry
        {
            get;
            set;
        }

        String Notes
        {
            get;
            set;
        }

        /// <summary>
        /// This property is for Droptick
        /// </summary>
        String TicketStatus
        {
            get;
            set;
        }
        decimal NoOfSearches
        {
            get;
            set;
        }

        decimal ResetSearchDelay
        {
            get;
            set;
        }

        decimal StartUsingProxiesFrom
        {
            get;
            set;
        }

        decimal DelayForAutoBuy
        {
            get;
            set;
        }

        decimal StartSolvingFromCaptcha
        {
            get;
            set;
        }

        decimal ScheduleRunningTime
        {
            get;
            set;
        }

        Boolean ifOpen
        {
            get;
            set;
        }

        Boolean IsUkEvent
        {
            get;
            set;
        }

        Boolean DoneSelection
        {
            get;
            set;
        }


        /// <summary>
        /// This property is for Droptick
        /// </summary>
        Boolean ifRun
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        Boolean ifContinousRun
        {
            get;
            set;
        }

        Boolean isRunning
        {
            get;
            set;
        }

        Boolean ifPesistSessionInEachSearch
        {
            get;
            set;

        }
        Boolean ifDistributeInSearches
        {
            get;
            set;
        }

        Boolean ifUseAvailableParameters
        {
            get;
            set;
        }

        Boolean ifUseFoundOnFirstAttempt
        {
            get;
            set;
        }

        Boolean ifAutoCaptcha
        {
            get;
            set;
        }

        Boolean ifBPCAutoCaptcha
        {
            get;
            set;
        }

        Boolean ifRDAutoCaptcha
        {
            get;
            set;
        }

        Boolean ifAntigateAutoCaptcha
        {
            get;
            set;
        }

        Boolean ifCPTAutoCaptcha
        {
            get;
            set;
        }

        Boolean ifDBCAutoCaptcha
        {
            get;
            set;
        }

        Boolean ifDCAutoCaptcha
        {
            get;
            set;
        }

        Boolean ifRDCAutoCaptcha
        {
            get;
            set;
        }

        Boolean ifOCR
        {
            get;
            set;
        }

        Boolean ifBoloOCR
        {
            get;
            set;
        }

        Boolean ifCAutoCaptcha
        {
            get;
            set;
        }

        Boolean ifAC1AutoCaptcha
        {
            get;
            set;
        }

        Boolean ifROCR
        {
            get;
            set;
        }

        Boolean ifSOCR
        {
            get;
            set;
        }

        Boolean if2C
        {
            get;
            set;
        }

        Boolean ifCaptchator
        {
            get;
            set;
        }

        Boolean ifPlaySoundAlert
        {
            get;
            set;
        }

        Boolean ifAutoBuy
        {
            get;
            set;
        }

        Boolean ifSendEmail
        {
            get;
            set;
        }

        Boolean ifAutoBuyWitoutProxy
        {
            get;
            set;
        }

        Boolean ifRandomDelay
        {
            get;
            set;
        }

        Boolean ifUseProxies
        {
            get;
            set;
        }

        Boolean ifUseProxiesInCaptchaSource
        {
            get;
            set;
        }

        Boolean ifSelectDeliveryOptionAutoBuying
        {
            get;
            set;
        }

        Boolean ifSelectDeliveryOptionList
        {
            get;
            set;
        }

        Boolean ifSelectAccountAutoBuying
        {
            get;
            set;
        }

        Boolean ifSelectAccountList
        {
            get;
            set;
        }

        Boolean ifSchedule
        {
            get;
            set;
        }

        Boolean isWaitingEvent
        {
            get;
            set;
        }

        Boolean ifCapsium
        {
            get;
            set;
        }

        int RunCount
        {
            get;
            set;
        }

        int FoundCount
        {
            get;
            set;
        }

        int countOf2
        {
            get;
            set;
        }

        int BuyCount
        {
            get;
            set;
        }

        int SoldoutCount
        {
            get;
            set;
        }

        Boolean isWeb
        {
            get;
            set;
        }

        Boolean isMobile
        {
            get;
            set;
        }

        Boolean isEventko
        {
            get;
            set;
        }

        DateTime ScheduleDateTime
        {
            get;
            set;
        }

        DateTime LastUsedDateTime
        {
            get;
            set;
        }

        ISearchForm TicketSearchWindow
        {
            get;
            set;
        }

        TicketGroup TicketGroup
        {
            get;
            set;
        }

        SortableBindingList<ITicketSearch> Searches
        {
            get;
            set;
        }

        SortableBindingList<AXSParameter> Parameters
        {
            get;
            set;
        }

        SortableBindingList<AXSDeliveryOption> SelectedDeliveryOptions
        {
            get;
            set;
        }

        SortableBindingList<AXSFoundCriteria> TicketFoundCriterions
        {
            get;
            set;
        }

        SortableBindingList<AXSTicketAccount> SelectedAccounts
        {
            get;
            set;
        }

        SortableBindingList<String> TicketAccountsInTransition
        {
            get;
            set;
        }
        Dictionary<String, int> BuyHistory
        {
            get;
            set;
        }

        System.Media.SoundPlayer SoundAlert
        {
            get;
        }

        AutoCaptchaServices AutoCaptchaServices
        {
            get;
            set;
        }
        EmailSetting Email
        {
            get;
            set;
        }

        GlobalSettings GlobalSetting
        {
            get;
            set;
        }
        string AgencyName
        {
            get;
            set;
        }

        string AgencyUrlCode
        {
            get;
            set;
        }

        string lang
        {
            get;
            set;
        }

        String SelectedDate
        {
            get;
            set;
        }
        String SelectedEventTime
        {
            get;
            set;
        }

        bool Permission
        {
            get;
            set;
        }
        bool ifChangeCountOf2
        {
            get;
            set;
        }
        List<String> VerifiedLotID
        {
            get;
            set;
        }

        SortableBindingList<TicketsLog> tic_Logs
        {
            get;
            set;
        }

        String MakeValidFileName();
        String MakeValidFileName(String ticketname);
        Boolean SaveTicket();
        Boolean DeleteTicket();
        AXSTicketAccount getNextAccount(Boolean isguest = false);
        Boolean setAgencyRequestUrl(AXSTicket Ticket);
        void sendURLToBrowser(AXSTicket Ticket, LicenseCore lic, String Location);
        Boolean setAgencyRequestUrl(AXSTicket Ticket, string wr);
        void start(System.Media.SoundPlayer soundAlert, SortableBindingList<AXSTicketAccount> allTMAccountsForSyncing, EmailSetting Email, GlobalSettings globalSetting);
        void start(BindingList<ITicketSearch> captchaQueue, BindingList<ITicketSearch> captchaBrowserQueue, System.Media.SoundPlayer soundAlert, AutoCaptchaServices autoCaptchaServices, SortableBindingList<AXSTicketAccount> allTMAccountsForSyncing, EmailSetting Email, GlobalSettings globalSetting);
        void stop();
        void changeSettingsDuringSearching(decimal lastSearches);
        String GetNextLotID();
        void SaveLotId(string lotId);
        void ReleaseLotId(string lotId);
        Object Clone();
    }
}
