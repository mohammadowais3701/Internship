using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    [Serializable]
    public class GlobalSettings
    {
        public String Username
        {
            get;
            set;
        }
        public String Password
        {
            get;
            set;
        }

        public Boolean IfCheckProxiesStatus
        {
            get;
            set;
        }

        public Boolean IfEnableSpecialProxies
        {
            get;
            set;
        }
        public int ProxiesCount
        {
            get;
            set;
        }
        public string ProxiesCountry
        {
            get;
            set;
        }
        public Boolean CountryAny
        {
            get;
            set;
        }
        public Boolean CountryUS
        {
            get;
            set;
        }
        public Boolean CountryGB
        {
            get;
            set;
        }
        public Boolean CountryAU
        {
            get;
            set;
        }
        public Boolean CountryCA
        {
            get;
            set;
        }
        public Boolean CountryUSCA
        {
            get;
            set;
        }
        public Boolean SpecialProxies15
        {
            get;
            set;
        }
        public Boolean SpecialProxies50
        {
            get;
            set;
        }
        public Boolean SpecialProxies75
        {
            get;
            set;
        }
        public Boolean SpecialProxies100
        {
            get;
            set;
        }
        public Boolean SpecialProxies125
        {
            get;
            set;
        }
        public Boolean SpecialProxies150
        {
            get;
            set;
        }
        public Boolean IfUseSpecialProxies
        {
            get;
            set;
        }

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

        public Boolean IfWeb
        {
            get;
            set;
        }

        public Boolean ifMobile
        {
            get;
            set;
        }

        public Boolean ifEventko
        {
            get;
            set;
        }


        public Boolean ifEvenkoBucket { get; set; }

        public Boolean ifAXSBucket { get; set; }

        public Boolean ifAXSUKBucket { get; set; }

        public Boolean ifQueueITBucket { get; set; }

        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifRunOnSunday
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifRunOnMonday
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifRunOnTuesday
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifRunOnWednesday
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifRunOnThursday
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifRunOnFriday
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifRunOnSaturday
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifAutoCaptcha
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifBPCAutoCaptcha
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifDBCAutoCaptcha
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifRDAutoCaptcha
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifRDCAutoCaptcha
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifAntigateAutoCaptcha
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifCPTAutoCaptcha
        {
            get;
            set;
        }

        /// This property is for Droptick
        /// </summary>
        public Boolean ifDCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifCAutoCaptcha
        {
            get;
            set;
        }

        public Boolean ifAC1AutoCaptcha
        {
            get;
            set;
        }

        /// <summary>
        /// This property is for Droptick
        /// </summary>
        /// 
        public Boolean ifOCR
        {
            get;
            set;
        }

        public Boolean ifROCR
        {
            get;
            set;
        }

        public Boolean ifBoloOCR
        {
            get;
            set;
        }

        public Boolean ifSOCR
        {
            get;
            set;
        }

        public Boolean if2C
        {
            get;
            set;
        }

        public Boolean ifCaptchator
        {
            get;
            set;
        }
        /// This property is for Droptick
        /// </summary>
        public Boolean ifPlaySoundAlert
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifSendEmail
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifFoundExtendNumberOfSearches
        {
            get;
            set;
        }

        public Boolean Permission
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        /// This property is for Droptick
        /// </summary>
        public TimeSpan MonitoringTimeFrom
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public decimal MonitoringHours
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public decimal MaxConcurrentEventsToRun
        {
            get;
            set;
        }

        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public Boolean ifUseProxiesInCaptchaSource
        {
            get;
            set;
        }
        public decimal ExtendSearchesTo
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        public decimal RescheduleDelay
        {
            get;
            set;
        }
        public Boolean TokensCache5
        {
            get;
            set;
        }
        public Boolean TokensCache10
        {
            get;
            set;
        }
        public Boolean TokensCache25
        {
            get;
            set;
        }
        public Boolean TokensCache50
        {
            get;
            set;
        }
        public Boolean TokensCache75
        {
            get;
            set;
        }
        public Boolean TokensCache100
        {
            get;
            set;
        }
        public Boolean TokensCache150
        {
            get;
            set;
        }
        public Boolean TokensCache200
        {
            get;
            set;
        }
        public int TokensCount
        {
            get;
            set;
        }
        public Boolean CapsiumWorkers25
        {
            get;
            set;
        }
        public Boolean CapsiumWorker10
        {
            get;
            set;
        }
        public Boolean CapsiumWorkers30
        {
            get;
            set;
        }
        public Boolean CapsiumWorker50
        {
            get;
            set;
        }
        public Boolean CapsiumWorker75
        {
            get;
            set;
        }
        public Boolean CapsiumWorkers100
        {
            get;
            set;
        }
        public int CapsiumWorkersCount
        {
            get;
            set;
        }

        public Boolean SpecialCaptchaServices10
        {
            get;
            set;
        }
        public Boolean SpecialCaptchaServices20
        {
            get;
            set;
        }
        public Boolean SpecialCaptchaServices30
        {
            get;
            set;
        }
        public Boolean SpecialCaptchaServices40
        {
            get;
            set;
        }

        public Boolean SpecialCaptchaServices50
        {
            get;
            set;
        }

        public Boolean SpecialCaptchaServices75
        {
            get;
            set;
        }

        public Boolean SpecialCaptchaServices100
        {
            get;
            set;
        }
        public Boolean SpecialCaptchaServices200
        {
            get;
            set;
        }
        public int SpecialServicesWorkerCount
        {
            get;
            set;
        }

        public Boolean IfUseRelayProxies
        {
            get;
            set;
        }
        public Boolean IfUseSpecialRelayProxies
        {
            get;
            set;
        }
        public Boolean RelayProxies10Percent
        {
            get;
            set;
        }
        public Boolean RelayProxies25Percent
        {
            get;
            set;
        }
        public Boolean RelayProxies50Percent
        {
            get;
            set;
        }
        public Boolean RelayProxies100Percent
        {
            get;
            set;
        }
        public int RelayPercentCount
        {
            get;
            set;
        }

        public GlobalSettings()
        {

            this.IfCheckProxiesStatus = false;
            this.SpecialProxies50 = false;
            this.SpecialProxies75 = false;
            this.SpecialProxies100 = false;
            this.SpecialProxies125 = false;
            this.SpecialProxies150 = false;
            this.IfUseSpecialProxies = false;
            this.IfEnableSpecialProxies = true;
            this.ProxiesCountry = "US";
            //this.CountryUSCA = true; /////*******/////
            this.IfEnablePM = true;
            this.ifEnableISPIPUseage = false;
            this.ifAutoCaptcha = false;
            this.ifBPCAutoCaptcha = true;
            this.ifCPTAutoCaptcha = false;
            this.ifDBCAutoCaptcha = false;
            this.ifRDAutoCaptcha = false;
            this.ifAntigateAutoCaptcha = false;
            this.ifOCR = false;
            this.ifROCR = false;
            this.ifFoundExtendNumberOfSearches = false;
            this.Permission = false;
            this.ExtendSearchesTo = 1;
            this.ifPlaySoundAlert = true;
            this.ifRunOnMonday = true;
            this.ifRunOnTuesday = true;
            this.ifRunOnWednesday = true;
            this.ifRunOnThursday = true;
            this.ifRunOnFriday = true;
            this.ifRunOnSaturday = true;
            this.ifRunOnSunday = true;
            this.ifSendEmail = false;
            this.MaxConcurrentEventsToRun = 10;
            this.MonitoringHours = 8;
            this.MonitoringTimeFrom = new TimeSpan(9, 0, 0);
            this.RescheduleDelay = 5;
            this.IfWeb = true;
            this.ifMobile = false;
            this.ifEventko = false;
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.IfUseRelayProxies = false;
            this.IfUseSpecialRelayProxies = false;
            this.RelayProxies10Percent = false;
            this.RelayProxies25Percent = false;
            this.RelayProxies50Percent = false;
            this.RelayProxies100Percent = false;
        }
    }
}
