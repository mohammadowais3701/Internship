using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace Automatick.Core
{
    public delegate void ResetAfterStop();
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
        ITicket Ticket
        {
            get;
            set;
        }

        Proxy Proxy
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
        void start();
        void stop();
        void restart();
        void workerThread();
        void autoBuy();
        void autoBuyWithoutProxy();
        ITicketParameter getNextParameter();
        Boolean mapParameterIfAvaiable(ITicketParameter parameter);
       
    }
}
