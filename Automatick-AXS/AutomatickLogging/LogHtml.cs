using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Logging
{
    public class LogHtml
    {
        #region Members
        String _LicenceID;
        String _TicketID;
        String _DateTime;
        String _Html;
        String _FileName;


        public String LicenceID
        {
            get { return _LicenceID; }
            set { _LicenceID = value; }
        }

        public String TicketID
        {
            get { return _TicketID; }
            set { _TicketID = value; }
        }

        public String Datetime
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }

        public String Html
        {
            get { return _Html; }
            set { _Html = value; }
        }

        public String FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }
        #endregion

        #region Constructor

        public LogHtml(String html)
        {
            Html = html;
        }

        public LogHtml(String licenceId, String ticketId, String html)
        {
            this.Datetime = DateTime.Now.ToString("MM-dd-yyyy"); ;
            this.LicenceID = licenceId;
            this.TicketID = ticketId;
            this.Html = html;
        }

        #endregion

        public String MakeFileName()
        {
            return this.FileName = this.LicenceID + "_" + this.TicketID + "_" + this.Datetime;
        }

        public new void Add(LogHtml obj)
        {

            //if (!ExistsInList(obj))
            //{
            //    _logHtmlList.Add(obj);
            //    base.Add(obj);
            //    Write();
            //}
        }

        ~LogHtml()
        { }
    }
}
