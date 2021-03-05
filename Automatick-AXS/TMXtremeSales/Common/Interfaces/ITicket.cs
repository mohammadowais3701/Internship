using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SortedBindingList;
using System.ComponentModel;

namespace Automatick.Core
{
    public delegate void ChangeDelegate(ITicket ticket);
    public delegate void ChangeDelegateForGauge();
    public interface ITicket
    {
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

        object TicketSearchWindow
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
        
        Dictionary<String, int> BuyHistory
        {
            get;
            set;
        }

        System.Media.SoundPlayer SoundAlert
        {
            get;
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

        string XmlUrl
        {
            get;
            set;
        }
      bool Permission
        {
            get;
            set;
        }
        String MakeValidFileName();
        Boolean SaveTicket();
        Boolean DeleteTicket();
        Boolean setAgencyRequestUrl(AXSTicket Ticket);
        void start(System.Media.SoundPlayer soundAlert, SortableBindingList<AXSTicketAccount> allTMAccountsForSyncing, EmailSetting Email, GlobalSettings globalSetting);
        void stop();
        void changeSettingsDuringSearching(decimal lastSearches);
    }
}
