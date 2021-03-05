using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Automatick.Core
{
    public delegate void ResetAfterStop();
    public delegate void Stopping();
    public interface ITicketSearch : IDisposable
    {
        System.Drawing.Bitmap FlagImage
        {
            get;
            set;
        }
        ResetAfterStop resetAfterStop
        {
            get;
            set;
        }
        Stopping stopping
        {
            get;
            set;
        }
        ITicket Ticket
        {
            get;
            set;
        }

        String RecaptchaV2Key
        {
            get;
            set;
        }
        /// <summary>
        /// This property is for Droptick
        /// </summary>
        String TicketName
        {
            get;
        }

        Proxy Proxy
        {
            get;
            set;
        }

        Parameter Parameter
        {
            get;
            set;
        }
		
		Captcha Captcha
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

        Boolean isJSON
        {
            get;
            set;
        }

        Boolean isGuest
        {
            get;
            set;
        }

        Boolean isTix
        {
            get;
            set;
        }

        String OnSaleUrl
        {
            get;
            set;
        }

        String Section
        {
            get;
            set;
        }
        String Row
        {
            get;
            set;
        }
        String Seat
        {
            get;
            set;
        }
        String Recaptcha
        {
            get;
            set;
        }
        Boolean ifMap
        {
            get;
            set;
        }
        String TotalPrice
        {
            get;
            set;
        }
        String Price
        {
            get;
            set;
        }
        String Quantity
        {
            get;
            set;
        }
        String Description
        {
            get;
            set;
        }
        String TimeLeft
        {
            get;
            set;
        }
        String MoreInfo
        {
            get;
            set;
        }
        String Status
        {
            get;
            set;
        }
        Boolean IfUseAutoCaptcha
        {
            get;
            set;
        }
        Boolean IfUseProxy
        {
            get;
            set;
        }
        Boolean IfWorking
        {
            get;
            set;
        }
        Boolean IfResetSearch
        {
            get;
            set;
        }
        Boolean IfFound
        {
            get;
            set;
        }
        HtmlDocument doc
        {
            get;
            set;
        }
        string wRoom
        {
            get;
            set;
        }
        string SessionKey
        {
            get;
            set;
        }
        Boolean IfAutoBuy
        {
            get;
            set;
        }

        String LastURLForManualBuy
        {
            get;
            set;
        }
        String code
        {
            get;
            set;
        }
        Presale _presaleSearch
        {
            get;
            set;
        }
        ApplicationStartUp _AppStartUp
        {
            get;
            set;
        }


        ITicketParameter _CurrentParameter
        {
            get;
            set;
        }

        void start();
        void stop();
        void restart();
        void workerThread();
        void autoBuy();
        void autoBuyGuest();
        void autoBuyWithoutProxy();
        ITicketParameter getNextParameter();
        Boolean mapParameterIfAvaiable(ITicketParameter parameter);
    }
}
