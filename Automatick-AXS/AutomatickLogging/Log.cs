using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Automatick.Logging
{   
    [Serializable]
    public class Log
    {
        #region Members
        private String _LogId;        
        private DateTime _DateTime;        
        private String _TicketID;        
        private String _TicketURL;       
        private String _ErrorMessage;
        private ErrorType _ErrorType;
        private Boolean _IfHtml;

        public Boolean IfHtml
        {
            get { return _IfHtml; }
            set { _IfHtml = value; }
        }
        public DateTime DateTime
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }
        public String TicketID
        {
            get { return _TicketID; }
            set { _TicketID = value; }
        }
        public String TicketURL
        {
            get { return _TicketURL; }
            set { _TicketURL = value; }
        }
        public String ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; 
            }
        }
        public ErrorType LogLevel
        {
            get { return _ErrorType; }
            set { _ErrorType = value; }
        }
        public String LogId
        {
            get { return _LogId; }
            set { _LogId = value; }
        }
        #endregion

        #region Constructor
        public Log(ErrorType logLevel, String ticketID, String ticketURL, String errorMessage)
        {           
            this.DateTime = DateTime.Now;
            this.LogId = UniqueKey.getUniqueKey();
            this.LogLevel = logLevel;
            this.TicketID = ticketID;
            this.TicketURL = ticketURL;
            this.ErrorMessage = errorMessage;
            this.IfHtml = false;
        }

        public Log(ErrorType logLevel, String ticketID, String ticketURL, String errorMessage,Boolean ifHtml)
        {
            this.DateTime = DateTime.Now;
            this.LogId = UniqueKey.getUniqueKey();
            this.LogLevel = logLevel;
            this.TicketID = ticketID;
            this.TicketURL = ticketURL;
            this.ErrorMessage = errorMessage;
            this.IfHtml = ifHtml;
        } 
        #endregion        
    }    
    public class logMessages
    {
        //-----------Messages for error log
        //---Naming convention use is message Class.FunctionName
        public const String ErrorLogEventExpire = "This event no longer exists";
        public const String ErrorLogErrorLogTMEventSection = "Sections not found in  TMEvent.ParseJson function";
        public const String ErrorLogMapParameter = "Value cannot be null in mapParameters";
        public const String ErrorLogCurrentParameter = "Value cannot be null in CurrentParameter";

        public const String ErrorLogFirstPageStatus = "First Page Loading Fail";
        public const String ErrorLogCaptchaPageStatus = "Captcha Page Loading Fail";
        public const String ErrorLogRefreshPageStatus = "Refresh Page Loading Fail";
        public const String ErrorLogDeliveryPageStatus = "Delivery Page Loading Fail";
        public const String ErrorLogSigninPageStatus = "Sign in Fail";
        public const String ErrorLogPaymentPageStatus = "Payment Page Loading Fail";
        public const String ErrorLogVerificationPageStatus = "Verification Page Loading Fail";
        public const String ErrorLogConfirmationPagetStatus = "Confirmation Page Loading Fail";
        public const String ErrorLogCreatePageStatus = "Create Page Loading Fail";
        public const String ErrorLogFoundPageStatus = "Unable to Find Ticket";
    }
    public enum ErrorType : int
    {
        LOGICAL = 0,
        EXCEPTION = 1,
    }  
}
