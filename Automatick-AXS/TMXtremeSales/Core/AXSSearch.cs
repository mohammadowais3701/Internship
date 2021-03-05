using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Threading;
using System.Net;
using System.IO;
using System.Drawing;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Xml;
using System.Windows.Forms;
namespace Automatick.Core
{
    [Serializable]
    public class AXSSearch : ITicketSearch
    {
        #region Variables
       
        private MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();        
        ITicketParameter _CurrentParameter = null;
        Boolean _ifRestarting = false;
        AXSTicketAccount _selectedAccountForAutoBuy = null;
        AXSEvent _tmEvent = null;
        Proxy _proxy = null;
        
        public AXSEvent TmEvent
        {
            get { return _tmEvent; }
        }
        #endregion

        #region ITicketSearch Members
        public System.Drawing.Bitmap FlagImage
        {
            get;
            set;
        }

        public ResetAfterStop resetAfterStop
        {
            get;
            set;
        }        

        public ITicket Ticket
        {
            get;
            set;
        }
        
        public Proxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        public String Section
        {
            get;
            set;
        }

        public String Row
        {
            get;
            set;
        }

        public String Seat
        {
            get;
            set;
        }

        public String Price
        {
            get;
            set;
        }

        public String Quantity
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

        public String TimeLeft
        {
            get;
            set;
        }

        public String MoreInfo
        {
            get;
            set;
        }

        public String Status
        {
            get;
            set;
        }

        public Boolean IfUseAutoCaptcha
        {
            get;
            set;
        }

        public Boolean IfUseProxy
        {
            get;
            set;
        }

        public Boolean IfWorking
        {
            get;
            set;
        }

        public Boolean IfResetSearch
        {
            get;
            set;
        }

        public Boolean IfFound
        {
            get;
            set;
        }
        public HtmlAgilityPack.HtmlDocument doc
        {
            get;
            set;
        }
        public string wRoom
        {
            get;
            set;
        }
        public Boolean IfAutoBuy
        {
            get;
            set;
        }
         public String SessionKey
        {
            get;
            set;
        }
        public String LastURLForManualBuy
        {
            get;
            set;
        }
        public String code
        {
            get;
            set;
        }
        bool _isPresale = false;

        public bool IsPresale
        {
            get { return _isPresale; }
            set { _isPresale = value; }
        }
        bool _stateWaiting = true;

        public bool StateWaiting
        {
            get { return _stateWaiting; }
            set { _stateWaiting = value; }
        }

        public void start()
        {
            this.resetAfterStop = null;
            this.IfWorking = true;
            this.MoreInfo = "";
            Thread th = new Thread(this.workerThread);
            th.Priority = ThreadPriority.Highest;
            th.SetApartmentState(ApartmentState.STA);
            th.IsBackground = true;
            th.Start();
        }

        public void stop()
        {
            try
            {
                if (this._ifRestarting)
                {
                    return;
                }

                this.TimeLeft = "";
                this.Section = "";
                this.Row = "";
                this.Seat = "";
                this.Price = "";
                this.Quantity = "";
                this.TimeLeft = "";
                this.Description = "";
                changeStatus(TicketSearchStatus.StopStatus);
                this.IfWorking = false;
                this.resetAfterStop = null;
                this._ifRestarting = false;

                AXSTicket ticket = (AXSTicket)this.Ticket;
              
                if (ProxyPicker.ProxyPickerInstance.ProxyManager != null)
                {
                    ProxyPicker.ProxyPickerInstance.ProxyManager.ReleaseProxy(this._proxy);
                }

                GC.SuppressFinalize(this);
            }
            catch { }
            GC.Collect();
        }

        public void restart()
        {
            this.MoreInfo = "";
            Thread thReset = new Thread(this.restartThread);
            thReset.SetApartmentState(ApartmentState.STA);
            thReset.Priority = ThreadPriority.Highest;
            thReset.IsBackground = true;
            thReset.Start();
        }
        void restartThread()
        {
            try
            {
                if (this.IfWorking)
                {
                    this.stop();
                    this._ifRestarting = true;
                    this.resetAfterStop = new ResetAfterStop(this.resetAfterStopHandler);
                    changeStatus(TicketSearchStatus.RestartingStatus);
                }
                else if (!this.IfWorking && !this._ifRestarting)
                {
                    this.start();
                }
            }
            catch { }
        }

        void retrying()
        {
            if (this.IfWorking && this.Ticket.isRunning)
            {
                this.IfFound = false;
                this.TimeLeft = "";
                this.Section = "";
                this.Row = "";
                this.Seat = "";
                this.Price = "";
                this.Quantity = "";
                this.TimeLeft = "";
                this.Description = "";

                if (ProxyPicker.ProxyPickerInstance.ProxyManager != null)
                {
                    ProxyPicker.ProxyPickerInstance.ProxyManager.ReleaseProxy(this._proxy);
                }

                int delay = (int)this.Ticket.ResetSearchDelay;
                if (this.Ticket.ifRandomDelay)
                {
                    Random rnd = new Random(0);
                    delay = rnd.Next(0, delay);
                }

                delay++;

                TimeSpan ts = new TimeSpan(0, 0, delay);

                this.TimeLeft = String.Format("{0:mm:ss}", ts).Remove(0, 3);
                while (ts.TotalSeconds > 0 && this.IfWorking && this.Ticket.isRunning)
                {
                    ts = ts.Subtract(new TimeSpan(0, 0, 1));
                    System.Threading.Thread.Sleep(1000);
                    this.TimeLeft = String.Format("{0:mm:ss}", ts).Remove(0, 3);
                    changeStatus(TicketSearchStatus.RetryingStatus);
                }
                this.TimeLeft = "";
            }
        }
        public void workerThread()
        {
            while (this.IfWorking && this.Ticket.isRunning)
            {
                changeStatus(TicketSearchStatus.SearchingStatus);
                this.IfFound = false;
                this.IfResetSearch = false;
                this.IfAutoBuy = false;
                this.TimeLeft = "";
                this.Section = "";
                this.Row = "";
                this.Seat = "";
                this.Price = "";
                this.Quantity = "";
                this.TimeLeft = "";
                this.Description = "";
                this.FlagImage = global::Automatick.Properties.Resources.Flag16Disable;
                this.FlagImage.Tag = false;
                                
                 if (( this.IfWorking && this.Ticket.isRunning)? this.processFirstPage() : false)
                {

                    if ((this.IfWorking && this.Ticket.isRunning) ? this.processFoundPage() : false)
                    {
                        if ((this.IfWorking && this.Ticket.isRunning) ? this.processDeliveryPage() : false)
                        {
                            if (this.IfWorking && this.Ticket.isRunning)
                            {
                                this.processAutoBuyPage();
                            }
                        }
                    }
                    
                }

                if (!this.Ticket.ifPesistSessionInEachSearch)
                {
                    
                }

                GC.Collect();

                retrying();

                if (!this.IfWorking)
                {
                    break;
                }
            }

            if (ProxyPicker.ProxyPickerInstance.ProxyManager != null)
            {
                ProxyPicker.ProxyPickerInstance.ProxyManager.ReleaseProxy(this._proxy);
            }

            if (this.resetAfterStop != null)
            {
                this.resetAfterStop();
            }
        }

        public void autoBuy()
        {
            try
            {

                if (this.IfWorking && this.IfFound && this.Ticket.isRunning)
                {
                    this.IfAutoBuy = true;
                }
            }
            catch (Exception)
            {

            }
        }

        public void autoBuyWithoutProxy()
        {
            if (ProxyPicker.ProxyPickerInstance.ProxyManager != null)
            {
                ProxyPicker.ProxyPickerInstance.ProxyManager.ReleaseProxy(this._proxy);
            }
            this._proxy = null;
            this.autoBuy();
        }

    

      

        protected Boolean processFirstPage()
        {
            #region presale
            Presale _presaleSearch = new Presale(this);

            if (_presaleSearch.startPresale())
            {
                this._isPresale = true;
            }
            else
            {
                this._isPresale = false;
            }
            #endregion
            Boolean result = false;
            String strHTML = String.Empty;
            try
            {
                if (this._CurrentParameter == null)
                {
                    _CurrentParameter = this.getNextParameter();
                }

                //Switch to next parameter if max bought exceeds
                if (this._CurrentParameter != null)
                {
                    if (this._CurrentParameter.MaxBought != null)
                    {
                        if (this._CurrentParameter.Bought >= this._CurrentParameter.MaxBought && this._CurrentParameter.MaxBought > 0)
                        {
                            this._CurrentParameter = null;
                            return false;
                        }
                    }
                }

                if (this._CurrentParameter != null)
                {
                    this.MoreInfo = "Searching quantity:" + this._CurrentParameter.Quantity;
                    if (this._CurrentParameter.PriceMin != null && this._CurrentParameter.PriceMax != null)
                    {
                        this.MoreInfo += ", price:" + this._CurrentParameter.PriceMin.ToString() + " - " + this._CurrentParameter.PriceMax.ToString();
                    }
                    if (!String.IsNullOrEmpty(this._CurrentParameter.PriceLevelString))
                    {
                        this.MoreInfo += ", ticket type:" + this._CurrentParameter.PriceLevelString;
                    }
                    if (!String.IsNullOrEmpty(this._CurrentParameter.TicketTypePasssword))
                    {
                        this.MoreInfo += ", password:" + this._CurrentParameter.TicketTypePasssword;
                    }
                }
                
                String strURL = this.Ticket.URL;

                if (this.Ticket.ifPesistSessionInEachSearch)
                {
                    if (this._proxy == null && this.Ticket.ifUseProxies && this.IfUseProxy)
                    {
                        _proxy = ProxyPicker.ProxyPickerInstance.getNextProxy(this);
                    }
                    Thread.Sleep(1000);
                    this.doc = new HtmlAgilityPack.HtmlDocument();
                }
                else
                {
                    this._proxy = null;
                    //Proxy
                    if (this.Ticket.ifUseProxies && this.IfUseProxy)
                    {
                        _proxy = ProxyPicker.ProxyPickerInstance.getNextProxy(this);
                    }
                    
                  
                    this.doc = new HtmlAgilityPack.HtmlDocument();
                }                

                changeStatus(TicketSearchStatus.SearchingStatus);

                this._selectedAccountForAutoBuy = null;

                //Proxy
               
               
             

                if (ProxyPicker.ProxyPickerInstance.ifSearchAllowed(this))
                 {
                    string[] breakforWroom = strURL.Split('?');
                    string[] split = breakforWroom[1].Split('&');
                    this.wRoom = split[0].Replace("wr=", "");
                   

                   
                  //  this.processWaitingPage();

                    
                    this._tmEvent = new AXSEvent(this.doc, (AXSSearch)this);
                   
                   
                   
                        lock (this.Ticket)
                        {
                            this.Ticket.RunCount++;
                        }

                        changeStatus(TicketSearchStatus.FirstPageStatus);

                        if (this.mapParameterIfAvaiable(_CurrentParameter))
                        {
                            _CurrentParameter.IfAvailable = true;
                          
                            result = true;
                        }
                        else
                        {
                            result = false;
                            if (this.Ticket.ifUseFoundOnFirstAttempt || this.Ticket.ifUseAvailableParameters)
                            {
                                this.MoreInfo = TicketSearchStatus.MoreInfoParamterNotMatch;
                                _CurrentParameter = null;
                            }
                        }
                    
                    
                }
                else
                {
                    this.MoreInfo = TicketSearchStatus.MoreInfoProxyNotAvaiable;
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        protected Boolean qualifies()
        {
            bool ifQualifies = false;
            string searchSection = this.Section;
            string searchRow = this.Row;
            int isearchRow = 0;


            try
            {
                if (this.Ticket.TicketFoundCriterions == null)
                {
                    ifQualifies = true;
                    return ifQualifies;
                }
                else if (this.Ticket.TicketFoundCriterions.Count <= 0)
                {
                    ifQualifies = true;
                    return ifQualifies;
                }

                foreach (AXSFoundCriteria criteria in this.Ticket.TicketFoundCriterions)
                {

                    if (searchRow.Length == 1 && searchRow.Length == ((!String.IsNullOrEmpty(criteria.RowFrom)) ? criteria.RowFrom.Length : searchRow.Length) && searchRow.Length == ((!String.IsNullOrEmpty(criteria.RowTo)) ? criteria.RowTo.Length : searchRow.Length))
                    {
                        int iFirstRow = 0;
                        int iLastRow = 0;
                        //Below are the variable used to check whether the row contains numeric data or not.
                        bool bFirstRow = false;
                        bool bLastRow = false;
                        bool bSearchRow = false;

                        if (!String.IsNullOrEmpty(criteria.Section) && !String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo))
                        {
                            bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                            bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                            bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);

                            if (!bSearchRow)
                            {
                                isearchRow = Convert.ToInt32(Convert.ToChar(searchRow.ToLower()));
                            }
                            if (!bFirstRow || !bLastRow)
                            {
                                iFirstRow = Convert.ToInt32(Convert.ToChar(criteria.RowFrom.ToLower()));
                                iLastRow = Convert.ToInt32(Convert.ToChar(criteria.RowTo.ToLower()));
                            }
                            if ((bFirstRow && bLastRow) && (!bSearchRow))
                            {
                                ifQualifies = false;
                            }
                            else if (criteria.Section.ToLower() == searchSection.ToLower() && (isearchRow >= iFirstRow && isearchRow <= iLastRow))
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                        }
                        else if (String.IsNullOrEmpty(criteria.Section) && (!String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo)))
                        {
                            bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                            bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                            bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);

                            if (!bSearchRow)
                            {
                                isearchRow = Convert.ToInt32(Convert.ToChar(searchRow.ToLower()));
                            }
                            if (!bFirstRow || !bLastRow)
                            {
                                iFirstRow = Convert.ToInt32(Convert.ToChar(criteria.RowFrom.ToLower()));
                                iLastRow = Convert.ToInt32(Convert.ToChar(criteria.RowTo.ToLower()));
                            }
                            if ((bFirstRow && bLastRow) && (!bSearchRow))
                            {
                                ifQualifies = false;
                            }
                            else if (isearchRow >= iFirstRow && isearchRow <= iLastRow)
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                        }
                        else if (!String.IsNullOrEmpty(criteria.Section) && (String.IsNullOrEmpty(criteria.RowFrom) && String.IsNullOrEmpty(criteria.RowTo)))
                        {
                            if (criteria.Section.ToLower() == searchSection.ToLower())
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                        }
                    }
                    else if (searchRow.Length > 1 && searchRow.Length == criteria.RowFrom.Length && searchRow.Length == criteria.RowTo.Length)
                    {
                        try
                        {
                            int iFirstRow = 0;
                            int iLastRow = 0;
                            bool bFirstRow = false;
                            bool bLastRow = false;
                            bool bSearchRow = false;

                            if (!String.IsNullOrEmpty(criteria.Section) && !String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo))
                            {
                                bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                                bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                                bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);

                                if (!bSearchRow)
                                {
                                    isearchRow = Convert.ToInt32(searchRow.ToLower().ToCharArray()[0]);
                                }
                                if (!bFirstRow || !bLastRow)
                                {
                                    iFirstRow = Convert.ToInt32(criteria.RowFrom.ToLower().ToCharArray()[0]);
                                    iLastRow = Convert.ToInt32(criteria.RowTo.ToLower().ToCharArray()[0]);
                                }
                                if ((bFirstRow && bLastRow) && (!bSearchRow))
                                {
                                    ifQualifies = false;
                                }
                                else if (criteria.Section.ToLower() == searchSection.ToLower() && (isearchRow >= iFirstRow && isearchRow <= iLastRow))
                                {
                                    ifQualifies = true;
                                    return ifQualifies;
                                }
                            }
                            else if (String.IsNullOrEmpty(criteria.Section) && (!String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo)))
                            {

                                bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                                bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                                bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);

                                if (!bSearchRow)
                                {
                                    isearchRow = Convert.ToInt32(searchRow.ToLower().ToCharArray()[0]);
                                }
                                if (!bFirstRow || !bLastRow)
                                {
                                    iFirstRow = Convert.ToInt32(criteria.RowFrom.ToLower().ToCharArray()[0]);
                                    iLastRow = Convert.ToInt32(criteria.RowTo.ToLower().ToCharArray()[0]);
                                }
                                if ((bFirstRow && bLastRow) && (!bSearchRow))
                                {
                                    ifQualifies = false;
                                }
                                else if (isearchRow >= iFirstRow && isearchRow <= iLastRow)
                                {
                                    ifQualifies = true;
                                    return ifQualifies;
                                }
                            }
                            else if (!String.IsNullOrEmpty(criteria.Section) && (String.IsNullOrEmpty(criteria.RowFrom) && String.IsNullOrEmpty(criteria.RowTo)))
                            {
                                if (criteria.Section.ToLower() == searchSection.ToLower())
                                {
                                    ifQualifies = true;
                                    return ifQualifies;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            ifQualifies = false;
                        }
                    }
                    else
                    {
                        int iFirstRow = 0;
                        int iLastRow = 0;
                        //Below are the variable used to check whether the row contains numeric data or not.
                        bool bFirstRow = false;
                        bool bLastRow = false;
                        bool bSearchRow = false;

                        if (!String.IsNullOrEmpty(criteria.Section) && !String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo))
                        {

                            bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                            bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                            bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);


                            if (!bFirstRow || !bLastRow || !bSearchRow)
                            {
                                return false;
                            }
                            if ((bFirstRow && bLastRow) && (!bSearchRow))
                            {
                                ifQualifies = false;
                            }
                            else if (criteria.Section.ToLower() == searchSection.ToLower() && (isearchRow >= iFirstRow && isearchRow <= iLastRow))
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                            else
                            {
                                ifQualifies = false;
                            }
                        }
                        else if (String.IsNullOrEmpty(criteria.Section) && (!String.IsNullOrEmpty(criteria.RowFrom) && !String.IsNullOrEmpty(criteria.RowTo)))
                        {
                            bSearchRow = int.TryParse(searchRow.Trim(), out isearchRow);
                            bFirstRow = int.TryParse(criteria.RowFrom.Trim(), out iFirstRow);
                            bLastRow = int.TryParse(criteria.RowTo.Trim(), out iLastRow);

                            if (!bFirstRow || !bLastRow || !bSearchRow)
                            {
                                return false;
                            }
                            if ((bFirstRow && bLastRow) && (!bSearchRow))
                            {
                                ifQualifies = false;
                            }
                            else if (isearchRow >= iFirstRow && isearchRow <= iLastRow)
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                            else
                            {
                                ifQualifies = false;
                            }
                        }
                        else if (!String.IsNullOrEmpty(criteria.Section) && (String.IsNullOrEmpty(criteria.RowFrom) && String.IsNullOrEmpty(criteria.RowTo)))
                        {
                            if (criteria.Section.ToLower() == searchSection.ToLower())
                            {
                                ifQualifies = true;
                                return ifQualifies;
                            }
                            else
                            {
                                ifQualifies = false;
                            }
                        }
                        else
                        {
                            ifQualifies = false;
                        }

                    }
                }
            }
            catch (Exception)
            {
                ifQualifies = false;
            }

            return ifQualifies;
        }


        string timeToExpire;
        protected Boolean processFoundPage()
        {
            Boolean result = false;
            try
            {
                HtmlNode totalPrice = doc.DocumentNode.SelectSingleNode("/methodresponse[1]/params[1]/value[1]/struct[1]/member[8]/value[1]/struct[1]/member[1]");
                /******************************************************/
                //Parse the cart
                /******************************************************/
                if (totalPrice != null)
                {
                    this.Price = totalPrice.InnerText.Replace("totalPrice", "").Trim().Replace("\n", "");///methodresponse[1]/params[1]/value[1]/struct[1]/member[8]/value[1]/struct[1]/member[1]/value[2]
                    if (this.Price.Contains("GBP"))
                    {
                        this.Price = this.Price.Insert(this.Price.Length - 5, ".");
                        this.Price = this.Price.Insert(this.Price.Length - 3, " ");
                    }
                    if (this.Price.Contains("USD"))
                    {
                        this.Price = this.Price.Insert(this.Price.Length - 5, ".");
                        this.Price = this.Price.Insert(this.Price.Length - 3, " ");
                    }
                  
                    timeToExpire = doc.DocumentNode.SelectSingleNode("//member/name[text() = 'lockttl']").NextSibling.NextSibling.ChildNodes[0].InnerHtml;
                    HtmlAgilityPack.HtmlDocument hdoc = new HtmlAgilityPack.HtmlDocument();
                    hdoc.LoadHtml(doc.DocumentNode.SelectSingleNode("//member/name[text() = 'seats']").ParentNode.OuterHtml);
                    this.Quantity = this._CurrentParameter.Quantity.ToString();
                    for (int i = Convert.ToInt32(this._CurrentParameter.Quantity); i >= 1; i--)
                    {
                        HtmlNode seats = hdoc.DocumentNode.SelectSingleNode("/member/value/array/data/value[" + i.ToString() + "]/array/data/value[4]/string");
                        if (seats != null)
                        {
                            string cart_seats = seats.InnerHtml;
                            try
                            {
                                string section = hdoc.DocumentNode.SelectSingleNode("/member/value/array/data/value[" + i.ToString() + "]/array/data/value[4]/string").InnerText;
                                if (this.Section != section)
                                {
                                    try
                                    {
                                        this.Section += hdoc.DocumentNode.SelectSingleNode("/member/value/array/data/value[" + i.ToString() + "]/array/data/value[4]/string").InnerText;
                                    }
                                    catch { }
                                }
                            }
                            catch { }
                            try
                            {
                                string Row = hdoc.DocumentNode.SelectSingleNode("/member/value/array/data/value[" + i.ToString() + "]/array/data/value[5]/string").InnerText;
                                if (this.Row != Row)
                                {
                                    try
                                    {
                                        this.Row += hdoc.DocumentNode.SelectSingleNode("/member/value/array/data/value[" + i.ToString() + "]/array/data/value[5]/string").InnerText;
                                    }
                                    catch { }
                                }
                            }
                            catch { }
                            try
                            {
                                string seat = hdoc.DocumentNode.SelectSingleNode("/member/value/array/data/value[" + i.ToString() + "]/array/data/value[6]/string").InnerText;
                                if (this.Seat != seat)
                                {
                                    try
                                    {
                                        this.Seat += hdoc.DocumentNode.SelectSingleNode("/member/value/array/data/value[" + i.ToString() + "]/array/data/value[6]/string").InnerText + ",";
                                    }
                                    catch { }
                                }
                            }
                            catch { }
                        }

                        else
                        {
                            changeStatus("Seats Not Available");
                            lock (this.Ticket)
                            {
                                this.Ticket.SoldoutCount++;
                            }
                            this.retrying();
                            result = false;
                        }
                        int l = this.Seat.Length;
                        this.Seat = this.Seat.Substring(0, (l - 1));

                        if (this.IfWorking && this.Ticket.isRunning)
                        {

                            lock (this.Ticket)
                            {
                                this.Ticket.FoundCount++;
                            }

                            if (!this.qualifies())
                            {
                                this.IfFound = false;
                                this.MoreInfo = "Found Row: " + this.Row + ", Section: " + this.Section + ". " + TicketSearchStatus.MoreInfoCriteriaDoesNotMatch;
                                changeStatus(TicketSearchStatus.FoundCriteriaDoesNotMatch);
                                result = false;
                                return result;
                            }


                        }

                        changeStatus(TicketSearchStatus.FoundPageStatus);
                        this.IfFound = true;
                        this.Ticket.Email.sendFoundEmail(this);

                    }
                    if (this.Ticket.ifAutoBuy)
                    {
                        this.IfAutoBuy = true;
                        if (this.Ticket.ifAutoBuyWitoutProxy)
                        {
                            if (ProxyPicker.ProxyPickerInstance.ProxyManager != null)
                            {
                                ProxyPicker.ProxyPickerInstance.ProxyManager.ReleaseProxy(this._proxy);
                            }
                            this._proxy = null;
                       
                        }


                    }
                    if (this.Ticket.ifPlaySoundAlert == true && this.Status == TicketSearchStatus.FoundPageStatus)
                    {
                        this.Ticket.SoundAlert.Play();
                    }

                    if (_CurrentParameter != null)
                    {
                        _CurrentParameter.IfFound = true;
                    }

                    this.processTimer(TicketSearchStatus.FoundPageStatus);
                    result = true;

                }
                else
                {
                    changeStatus("Seats Not Available");
                    //Thread.Sleep(100);
                    lock (this.Ticket)
                    {
                        this.Ticket.SoldoutCount++;
                    }
                    result = false;

                }

            }

            catch
            {
                result = false;
            }


            return result;
        }



        protected Boolean processDeliveryPage()
        {
            Boolean result = false;
            try
            {
                String strHTML = String.Empty;
                                
                    this.processRefreshPage();


                    changeStatus(TicketSearchStatus.DeliveryPageStatus);

                    this.processTimer(TicketSearchStatus.DeliveryPageStatus);

                   List<string> countryWiseDeliveryOptions = extractDeliveryOptions();
                    

                    selectDeliveryOption(countryWiseDeliveryOptions, this.Ticket);

                    result = true;

                      
                
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }


        protected Boolean processRefreshPage()
        {
            Boolean result = false;
           
            try
            {
                changeStatus(TicketSearchStatus.RefreshPageStatus);

                        int wait = 1;
                        //Don't delete the code below ************************
                        //int wait = 4;                        
                        //mRefresh = Regex.Match(this._session.HtmlDocument.DocumentNode.InnerHtml, "http-equiv=\"refresh\"(\\s+)content=\"(\\d+);", RegexOptions.IgnoreCase);
                        //if (mRefresh.Success)
                        //{
                        //    mRefresh = Regex.Match(mRefresh.Value, "\\d+");
                        //    if (mRefresh.Success)
                        //    {
                        //        wait = int.Parse(mRefresh.Value);
                        //    }
                        //}

                        DateTime dateTimeFound = DateTime.Now;
                        TimeSpan ts = dateTimeFound.AddSeconds(wait) - dateTimeFound;

                        while (ts.TotalSeconds > 1 && this.IfWorking && this.Ticket.isRunning)
                        {
                            ts = ts.Subtract(new TimeSpan(0, 0, 1));
                            System.Threading.Thread.Sleep(1000);
                            changeStatus(TicketSearchStatus.RefreshPageStatus);
                        }


                     
                    //

                   
                

                if (this.IfWorking && this.Ticket.isRunning)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }


        private void processTimer(String statusMessage)
        {

            int expirationTimeInSeconds =Convert.ToInt32(timeToExpire);

            
        
            DateTime dateTimeFound = DateTime.Now;
            TimeSpan ts = dateTimeFound.AddSeconds(expirationTimeInSeconds) - dateTimeFound;
            this.TimeLeft = String.Format("{0:mm:ss}", ts).Remove(0, 3);
            while (ts.TotalSeconds > 1 && this.IfWorking && this.Ticket.isRunning && !this.IfAutoBuy)
            {
                ts = ts.Subtract(new TimeSpan(0, 0, 1));
                System.Threading.Thread.Sleep(1000);
                this.TimeLeft = String.Format("{0:mm:ss}", ts).Remove(0, 3);
                changeStatus(statusMessage);
            }

            if (this.IfAutoBuy)
            {
                this.MoreInfo = TicketSearchStatus.MoreInfoBuyingInProgress;
                changeStatus(statusMessage);
            }
            this.TimeLeft = String.Empty;
          
           
        }



        void processAutoBuyPage()
        {
            if (this.IfAutoBuy && this.IfWorking && this.Ticket.isRunning)
            {
                 this._selectedAccountForAutoBuy = null;

                        lock (this.Ticket)
                        {
                            if (this.Ticket.BuyHistory == null)
                            {
                                this.Ticket.BuyHistory = new Dictionary<String, int>();
                            }
                        }

                        this._selectedAccountForAutoBuy = this.selectTicketAccount();

                        if (this._selectedAccountForAutoBuy != null)
                        {
                            try
                            {
                                this.MoreInfo = "Buying: " + this._selectedAccountForAutoBuy.AccountEmail;
                                string firstSixDigits = this._selectedAccountForAutoBuy.CardNumber.Substring(0, 6);
                                string lastFourDigits = this._selectedAccountForAutoBuy.CardNumber.Remove(0, this._selectedAccountForAutoBuy.CardNumber.Length - 4);
                                string pmCode = string.Empty;
                                string cardTypeNum = string.Empty;
                                this.Price = this.Price.Replace(".", "");
                                if (this._selectedAccountForAutoBuy.CardType == "AMEX")
                                {
                                    pmCode = "AMEX";
                                    cardTypeNum = "003";
                                }
                                else if (this._selectedAccountForAutoBuy.CardType == "Master Card")
                                {
                                    pmCode = "MC";
                                    cardTypeNum = "002";
                                }
                                else if (this._selectedAccountForAutoBuy.CardType == "Visa")
                                {
                                    pmCode = "VI";
                                    cardTypeNum = "001";
                                }
                                string currency = String.Empty;
                                if (this.Price.Contains("GBP"))
                                {
                                    currency = "GBP";
                                    this.Price = this.Price.Replace("GBP", "").Trim();
                                }
                                else if (this.Price.Contains("USD"))
                                {
                                    currency = "USD";
                                    this.Price = this.Price.Replace("USD", "").Trim();
                                }
                                string postData = String.Empty;
                                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                                XmlDocument xDoc = new XmlDocument();
                                if (currency == "GBP")
                                {
                                    postData = "<methodCall><methodName>webapi.amodsell</methodName><params><param><value><string>" + this.SessionKey + "</string></value></param><param><struct><member><name>customer</name><value><struct><member><name>extCustId</name><value><string></string></value></member><member><name>primaryId</name><value><string></string></value></member><member><name>firstName</name><value><string>" + this._selectedAccountForAutoBuy.FirstName + "</string></value></member><member><name>lastName</name><value><string>" + this._selectedAccountForAutoBuy.LastName + "</string></value></member><member><name>preferredLanguage</name><value><string>en</string></value></member><member><name>type</name><value><string></string></value></member><member><name>emailOptOut</name><value><boolean>1</boolean></value></member><member/><member><name>updateExtCustomer</name><value><boolean>1</boolean></value></member></struct></value></member><member><name>emails</name><value><array><data><value><struct><member><name>emailId</name><value><string></string></value></member><member><name>primaryId</name><value><string></string></value></member><member><name>ownerId</name><value><string></string></value></member><member><name>emailAddress</name><value><string>" + this._selectedAccountForAutoBuy.EmailAddress + "</string></value></member><member><name>emailAddress2</name><value><string>" + this._selectedAccountForAutoBuy.ConfirmEmail + "</string></value></member><member><name>emailType</name><value><string>Primary</string></value></member></struct></value></data></array></value></member><member><name>addresses</name><value><array><data><value><struct><member><name>addressId</name><value><string></string></value></member><member><name>ownerId</name><value><string></string></value></member><member><name>address1</name><value><string>" + this._selectedAccountForAutoBuy.Address1 + "</string></value></member><member><name>address2</name><value><string>" + this._selectedAccountForAutoBuy.Address2 + "</string></value></member><member><name>city</name><value><string>" + this._selectedAccountForAutoBuy.Town + "</string></value></member><member><name>countryCode</name><value><string>GB</string></value></member><member><name>regionCode</name><value><string></string></value></member><member><name>postCode</name><value><string>" + this._selectedAccountForAutoBuy.PostCode + "</string></value></member><member><name>addressType</name><value><string>Billing</string></value></member><member><name>primaryId</name><value><string></string></value></member><member><name>primary</name><value><string>1</string></value></member></struct></value></data></array></value></member><member><name>phones</name><value><array><data><value><struct><member><name>phoneId</name><value><string></string></value></member><member><name>ownerId</name><value><string></string></value></member><member><name>primaryId</name><value><string></string></value></member><member><name>primary</name><value><string>1</string></value></member><member><name>phoneNumber</name><value><string>" + this._selectedAccountForAutoBuy.Telephone + "</string></value></member><member><name>phoneType</name><value><string>Day</string></value></member></struct></value><value><struct><member><name>phoneId</name><value><string></string></value></member><member><name>ownerId</name><value><string></string></value></member><member><name>primaryId</name><value><string></string></value></member><member><name>primary</name><value><string>0</string></value></member><member><name>phoneNumber</name><value><string>" + this._selectedAccountForAutoBuy.Mobile + "</string></value></member><member><name>phoneType</name><value><string>Home</string></value></member></struct></value></data></array></value></member></struct></param><param><value><array><data><value><value><array><data><value><int>1</int></value><value><int>1</int></value><value><int>1</int></value></data></array></value></value><value><value><array><data><value><int>1</int></value><value><int>2</int></value><value><int>1</int></value></data></array></value></value><value><value><array><data><value><int>1</int></value><value><int>3</int></value><value><int>1</int></value></data></array></value></value><value><value><array><data><value><int>1</int></value><value><int>4</int></value><value><int>1</int></value></data></array></value></value><value><value><array><data><value><int>1</int></value><value><int>5</int></value><value><int>0</int></value></data></array></value></value></data></array></value></param><param><value><array><data><value><struct><member><name>primaryId</name><value><string></string></value></member><member><name>countryCode</name><value><string>GB</string></value></member><member><name>firstSixDigits</name><value><string>" + firstSixDigits + "</string></value></member><member><name>lastFourDigits</name><value><string>" + lastFourDigits + "</string></value></member><member><name>terminalId</name><value><string></string></value></member><member><name>cardNumber</name><value><string>" + this._selectedAccountForAutoBuy.CardNumber + "</string></value></member><member><name>cardToken</name><value><string></string></value></member><member><name>emailOptOut</name><value><boolean>1</boolean></value></member><member /><member><name>noAddrFlag</name><value><string></string></value></member><member><name>docNumber</name><value><string></string></value></member><member><name>city</name><value><string>" + this._selectedAccountForAutoBuy.Town + "</string></value></member><member><name>regionCode</name><value><string></string></value></member><member><name>phoneNumber</name><value><string>" + this._selectedAccountForAutoBuy.Telephone + "</string></value></member><member><name>address1</name><value><string>" + this._selectedAccountForAutoBuy.Address1 + "</string></value></member><member><name>address2</name><value><string>" + this._selectedAccountForAutoBuy.Address2 + "</string></value></member><member><name>paymentType</name><value><string></string></value></member><member><name>pmCode</name><value><string>" + pmCode + "</string></value></member><member><name>firstName</name><value><string>" + this._selectedAccountForAutoBuy.FirstName + "</string></value></member><member><name>lastName</name><value><string>" + this._selectedAccountForAutoBuy.LastName + "</string></value></member><member><name>ccv</name><value><string>" + this._selectedAccountForAutoBuy.CCVNum + "</string></value></member><member><name>expMonth</name><value><string>" + this._selectedAccountForAutoBuy.ExpiryMonth + "</string></value></member><member><name>expYear</name><value><string>" + this._selectedAccountForAutoBuy.ExpiryYear + "</string></value></member><member><name>amount</name><value><array><data><value><int>" + this.Price + "</int></value><value><string>GBP</string></value></data></array></value></member><member><name>postCode</name><value><string>" + this._selectedAccountForAutoBuy.PostCode + "</string></value></member></struct></value></data></array></value></param><param><value><array><data><value><string>customer</string></value><value><string>emails</string></value><value><string>addresses</string></value><value><string>phones</string></value><value><string>payments</string></value><value><string>Order</string></value><value><string>lockttl</string></value></data></array></value></param><param><value><string>REGULAR</string></value></param><param><value><array><data><value><struct><member><name>extCustId</name><value><string></string></value></member><member><name>emailAddress</name><value><string>" + this._selectedAccountForAutoBuy.EmailAddress + "</string></value></member><member><name>address1</name><value><string>" + this._selectedAccountForAutoBuy.Address1 + "</string></value></member><member><name>address2</name><value><string>" + this._selectedAccountForAutoBuy.Address2 + "</string></value></member><member><name>city</name><value><string>" + this._selectedAccountForAutoBuy.Town + "</string></value></member><member><name>countryCode</name><value><string>GB</string></value></member><member><name>regionCode</name><value><string></string></value></member><member><name>postCode</name><value><string>" + this._selectedAccountForAutoBuy.PostCode + "</string></value></member><member><name>phoneNumber</name><value><string>" + this._selectedAccountForAutoBuy.Telephone + "</string></value></member><member><name>currency</name><value><string>GBP</string></value></member><member><name>pmCode</name><value><string>" + pmCode + "</string></value></member><member><name>cardType</name><value><string>" + cardTypeNum + "</string></value></member><member><name>firstName</name><value><string>" + this._selectedAccountForAutoBuy.FirstName + "</string></value></member><member><name>lastName</name><value><string>" + this._selectedAccountForAutoBuy.LastName + "</string></value></member><member><name>cardToken</name><value><string></string></value></member><member><name>cardNumber</name><value><string>" + this._selectedAccountForAutoBuy.CardNumber + "</string></value></member><member><name>expMonth</name><value><string>" + this._selectedAccountForAutoBuy.ExpiryMonth + "</string></value></member><member><name>expYear</name><value><string>" + this._selectedAccountForAutoBuy.ExpiryYear + "</string></value></member><member><name>ccv</name><value><string>" + this._selectedAccountForAutoBuy.CCVNum + "</string></value></member><member><name>firstSixDigits</name><value><string>" + firstSixDigits + "</string></value></member><member><name>lastFourDigits</name><value><string>" + lastFourDigits + "</string></value></member><member><name>icsReplyCode</name><value><string></string></value></member><member><name>icsReplyFlag</name><value><string></string></value></member><member><name>icsReplyMsg</name><value><string></string></value></member><member><name>icsRequestId</name><value><string></string></value></member></struct></value></data></array></value></param><param><value><string>sc:false,Ase:" + this.code + ",t:1,s,rs:ff,fok:false,fok:false,fok:false,fok:false,fok:false,rs:s,rs:ff,fok:false,fok:true,</string></value></param></params></methodCall>";
                                }
                                else if (currency == "USD")
                                {
                                    postData = "<methodCall><methodName>webapi.amodsell</methodName><params><param><value><string>" + this.SessionKey + "</string></value></param><param><struct><member><name>customer</name><value><struct><member><name>extCustId</name><value><string></string></value></member><member><name>primaryId</name><value><string></string></value></member><member><name>firstName</name><value><string>" + this._selectedAccountForAutoBuy.FirstName + "</string></value></member><member><name>lastName</name><value><string>" + this._selectedAccountForAutoBuy.LastName + "</string></value></member><member><name>preferredLanguage</name><value><string>en</string></value></member><member><name>type</name><value><string></string></value></member><member><name>emailOptOut</name><value><boolean>1</boolean></value></member><member /><member><name>updateExtCustomer</name><value><boolean>1</boolean></value></member></struct></value></member><member><name>emails</name><value><array><data><value><struct><member><name>emailId</name><value><string></string></value></member><member><name>primaryId</name><value><string></string></value></member><member><name>ownerId</name><value><string></string></value></member><member><name>emailAddress</name><value><string>" + this._selectedAccountForAutoBuy.EmailAddress + "</string></value></member><member><name>emailAddress2</name><value><string>" + this._selectedAccountForAutoBuy.ConfirmEmail + "</string></value></member><member><name>emailType</name><value><string>Primary</string></value></member></struct></value></data></array></value></member><member><name>addresses</name><value><array><data><value><struct><member><name>addressId</name><value><string></string></value></member><member><name>ownerId</name><value><string></string></value></member><member><name>address1</name><value><string>" + this._selectedAccountForAutoBuy.Address1 + "</string></value></member><member><name>address2</name><value><string>" + this._selectedAccountForAutoBuy.Address2 + "</string></value></member><member><name>city</name><value><string>" + this._selectedAccountForAutoBuy.Town + "</string></value></member><member><name>countryCode</name><value><string>GB</string></value></member><member><name>regionCode</name><value><string></string></value></member><member><name>postCode</name><value><string>" + this._selectedAccountForAutoBuy.PostCode + "</string></value></member><member><name>addressType</name><value><string>Billing</string></value></member><member><name>primaryId</name><value><string></string></value></member><member><name>primary</name><value><string>1</string></value></member></struct></value></data></array></value></member><member><name>phones</name><value><array><data><value><struct><member><name>phoneId</name><value><string></string></value></member><member><name>ownerId</name><value><string></string></value></member><member><name>primaryId</name><value><string></string></value></member><member><name>primary</name><value><string>1</string></value></member><member><name>phoneNumber</name><value><string>" + this._selectedAccountForAutoBuy.Telephone + "</string></value></member><member><name>phoneType</name><value><string>Day</string></value></member></struct></value><value><struct><member><name>phoneId</name><value><string></string></value></member><member><name>ownerId</name><value><string></string></value></member><member><name>primaryId</name><value><string></string></value></member><member><name>primary</name><value><string>0</string></value></member><member><name>phoneNumber</name><value><string>" + this._selectedAccountForAutoBuy.Mobile + "</string></value></member><member><name>phoneType</name><value><string>Home</string></value></member></struct></value></data></array></value></member></struct></param><param><value><array><data><value><value><array><data><value><int>1</int></value><value><int>1</int></value><value><int>1</int></value></data></array></value></value><value><value><array><data><value><int>1</int></value><value><int>2</int></value><value><int>1</int></value></data></array></value></value><value><value><array><data><value><int>1</int></value><value><int>3</int></value><value><int>1</int></value></data></array></value></value><value><value><array><data><value><int>1</int></value><value><int>4</int></value><value><int>0</int></value></data></array></value></value><value><value><array><data><value><int>1</int></value><value><int>5</int></value><value><int>0</int></value></data></array></value></value></data></array></value></param><param><value><array><data><value><struct><member><name>primaryId</name><value><string></string></value></member><member><name>countryCode</name><value><string>GB</string></value></member><member><name>firstSixDigits</name><value><string>" + firstSixDigits + "</string></value></member><member><name>lastFourDigits</name><value><string>" + lastFourDigits + "</string></value></member><member><name>terminalId</name><value><string></string></value></member><member><name>cardNumber</name><value><string>" + this._selectedAccountForAutoBuy.CardNumber + "</string></value></member><member><name>cardToken</name><value><string></string></value></member><member><name>emailOptOut</name><value><boolean>1</boolean></value></member><member /><member><name>noAddrFlag</name><value><string></string></value></member><member><name>docNumber</name><value><string></string></value></member><member><name>city</name><value><string>" + this._selectedAccountForAutoBuy.Town + "</string></value></member><member><name>regionCode</name><value><string></string></value></member><member><name>phoneNumber</name><value><string>" + this._selectedAccountForAutoBuy.Telephone + "</string></value></member><member><name>address1</name><value><string>" + this._selectedAccountForAutoBuy.Address1 + "</string></value></member><member><name>address2</name><value><string>" + this._selectedAccountForAutoBuy.Address2 + "</string></value></member><member><name>paymentType</name><value><string></string></value></member><member><name>pmCode</name><value><string>" + pmCode + "</string></value></member><member><name>firstName</name><value><string>" + this._selectedAccountForAutoBuy.FirstName + "</string></value></member><member><name>lastName</name><value><string>" + this._selectedAccountForAutoBuy.LastName + "</string></value></member><member><name>ccv</name><value><string>" + this._selectedAccountForAutoBuy.CCVNum + "</string></value></member><member><name>expMonth</name><value><string>" + this._selectedAccountForAutoBuy.ExpiryMonth + "</string></value></member><member><name>expYear</name><value><string>" + this._selectedAccountForAutoBuy.ExpiryYear + "</string></value></member><member><name>amount</name><value><array><data><value><int>" + this.Price + "</int></value><value><string>USD</string></value></data></array></value></member><member><name>postCode</name><value><string>" + this._selectedAccountForAutoBuy.PostCode + "</string></value></member></struct></value></data></array></value></param><param><value><array><data><value><string>customer</string></value><value><string>emails</string></value><value><string>addresses</string></value><value><string>phones</string></value><value><string>payments</string></value><value><string>Order</string></value><value><string>lockttl</string></value></data></array></value></param><param><value><string>REGULAR</string></value></param><param><value><array><data><value><struct><member><name>extCustId</name><value><string></string></value></member><member><name>emailAddress</name><value><string>" + this._selectedAccountForAutoBuy.EmailAddress + "</string></value></member><member><name>address1</name><value><string>" + this._selectedAccountForAutoBuy.Address1 + "</string></value></member><member><name>address2</name><value><string>" + this._selectedAccountForAutoBuy.Address2 + "</string></value></member><member><name>city</name><value><string>" + this._selectedAccountForAutoBuy.Town + "</string></value></member><member><name>countryCode</name><value><string>GB</string></value></member><member><name>regionCode</name><value><string></string></value></member><member><name>postCode</name><value><string>" + this._selectedAccountForAutoBuy.PostCode + "</string></value></member><member><name>phoneNumber</name><value><string>" + this._selectedAccountForAutoBuy.Telephone + "</string></value></member><member><name>currency</name><value><string>USD</string></value></member><member><name>pmCode</name><value><string>" + pmCode + "</string></value></member><member><name>cardType</name><value><string>" + cardTypeNum + "</string></value></member><member><name>firstName</name><value><string>" + this._selectedAccountForAutoBuy.FirstName + "</string></value></member><member><name>lastName</name><value><string>" + this._selectedAccountForAutoBuy.LastName + "</string></value></member><member><name>cardToken</name><value><string></string></value></member><member><name>cardNumber</name><value><string>" + this._selectedAccountForAutoBuy.CardNumber + "</string></value></member><member><name>expMonth</name><value><string>" + this._selectedAccountForAutoBuy.ExpiryMonth + "</string></value></member><member><name>expYear</name><value><string>" + this._selectedAccountForAutoBuy.ExpiryYear + "</string></value></member><member><name>ccv</name><value><string>" + this._selectedAccountForAutoBuy.CCVNum + "</string></value></member><member><name>firstSixDigits</name><value><string>" + firstSixDigits + "</string></value></member><member><name>lastFourDigits</name><value><string>" + lastFourDigits + "</string></value></member><member><name>icsReplyCode</name><value><string></string></value></member><member><name>icsReplyFlag</name><value><string></string></value></member><member><name>icsReplyMsg</name><value><string></string></value></member><member><name>icsRequestId</name><value><string></string></value></member></struct></value></data></array></value></param><param><value><string>sc:false,se:" + this.code + ",t:1,s,rs:ff,fok:true,</string></value></param></params></methodCall>";
                                }
                                xDoc.InnerXml = postData;
                                doc.LoadHtml(post(this, this.Ticket.XmlUrl.Replace("/info/", "/bfox/") + "?methodName=webapi.amodsell&sessionKey=" + this.SessionKey + "&XMLRPC_callCounter=59", xDoc.InnerXml));
                                if (this._CurrentParameter != null)
                                {
                                    if (this._CurrentParameter.Bought == null)
                                    {
                                        this._CurrentParameter.Bought = 0;
                                    }
                                    this._CurrentParameter.Bought++;
                                }

                                lock (this.Ticket)
                                {
                                    this.Ticket.BuyCount++;

                                    if (this.Ticket.BuyHistory.ContainsKey(this._selectedAccountForAutoBuy.EmailAddress))
                                    {
                                        this.Ticket.BuyHistory[this._selectedAccountForAutoBuy.EmailAddress] += 1;
                                        this.Ticket.SaveTicket();
                                    }
                                    else
                                    {
                                        this.Ticket.BuyHistory.Add(this._selectedAccountForAutoBuy.EmailAddress, 1);
                                        this.Ticket.SaveTicket();
                                    }
                                }
                            }
                            catch 
                            {
                              
                            }
                        }
                        else
                        {
                            this.MoreInfo = TicketSearchStatus.MoreInfoAccountNotAvailable;
                            
                        }

            }
        }


     
                 
          
        

      

        private AXSTicketAccount selectTicketAccount()
        {
            AXSTicketAccount selectedAccount = null;
            try
            {
                if (this.Ticket.ifSelectAccountAutoBuying)
                {
                    frmSelectAccount selectAccount = new frmSelectAccount(this.Ticket);
                    selectedAccount = (AXSTicketAccount)selectAccount.promptAccount();
                    selectAccount.Dispose();
                }
                else
                {
                    if (this.Ticket.SelectedAccounts != null)
                    {
                        if (this.Ticket.SelectedAccounts.Count > 0)
                        {
                            foreach (AXSTicketAccount account in this.Ticket.SelectedAccounts)
                            {
                                if (account.IfSelected)
                                {
                                    selectedAccount = account;
                                }
                            }
                        }
                        else
                        {
                            selectedAccount = null;
                        }
                    }
                    else
                    {
                        selectedAccount = null;
                    }
                }
            }
            catch
            {
                selectedAccount = null;
            }

            return selectedAccount;
        }

      
        private void selectDeliveryOption(List<String> deliveryOptions,ITicket Ticket)
        {


            try
            {
                if (this.Ticket.ifSelectDeliveryOptionAutoBuying)
                {

                    if (this.IfAutoBuy && (String.IsNullOrEmpty(this.Ticket.DeliveryOption) || String.IsNullOrEmpty(this.Ticket.DeliveryCountry)))
                    {
                        AXSTicket tick = (AXSTicket)this.Ticket;
                        if (!tick.IfSelectDeliveryWindowOpen)
                        {
                            tick.IfSelectDeliveryWindowOpen = true;
                            frmSelectDeliveryOption frm = new frmSelectDeliveryOption(deliveryOptions, this.Ticket);
                            frm.ShowDialog();
                            tick.IfSelectDeliveryWindowOpen = false;
                            frm.Dispose();
                        }
                        else
                        {
                            while (String.IsNullOrEmpty(this.Ticket.DeliveryOption) && tick.IfSelectDeliveryWindowOpen && this.IfAutoBuy)
                            {
                                if ((!this.IfWorking || !this.Ticket.isRunning))
                                {
                                    break;
                                }
                                Thread.Sleep(500);
                            }
                        }
                    }

                }
                else
                {
                    if (this.Ticket.SelectedDeliveryOptions != null)
                    {
                        foreach (AXSDeliveryOption dopt in this.Ticket.SelectedDeliveryOptions)
                        {
                            if (deliveryOptions.Contains(dopt.DeliveryOption))
                            {
                                this.Ticket.DeliveryCountry = dopt.DeliveryOption;
                                this.Ticket.DeliveryOption = dopt.DeliveryOption;
                                this.Ticket.SaveTicket();
                            }
                        }
                    }
                }
            }
            catch
            {

            }

            
        }

        private List<String> extractDeliveryOptions()
        {
            List<string> countryWiseDeliveryOptions = new List<string>();

            try
            {
                HtmlAgilityPack.HtmlDocument deliveryoptions = new HtmlAgilityPack.HtmlDocument();
                    deliveryoptions.LoadHtml(this.doc.DocumentNode.SelectSingleNode("//name[text() = 'label']").ParentNode.OuterHtml);

                    HtmlNodeCollection hnodes = this.doc.DocumentNode.SelectNodes("//name[text()='label']");
                    foreach (HtmlNode item in hnodes)
                    {
                        string node = item.ParentNode.OuterHtml.Replace("<member><name>label</name><value><string>","");
                        node = node.Replace("<member>\n<name>label</name>\n<value><string>", "").Replace("</string></value>\n</member>","");
                        countryWiseDeliveryOptions.Add(node);
                    }

            }
            catch (Exception)
            {
                countryWiseDeliveryOptions = null;
            }

            return countryWiseDeliveryOptions;
        }
      

        

        

        public ITicketParameter getNextParameter()
        {
            ITicketParameter parameter = null;
            AXSTicket ticket = (AXSTicket)this.Ticket;

            if (this.Ticket.ifUseFoundOnFirstAttempt)
            {
                IEnumerable<AXSParameter> tmpParamters = this.Ticket.Parameters.Where(p => p.IfFound == true);
                if (tmpParamters != null)
                {
                    if (ticket.CurrentFoundParameterIndex >= tmpParamters.Count())
                    {
                        ticket.CurrentFoundParameterIndex = 0;
                    }
                    parameter = tmpParamters.ElementAtOrDefault(ticket.CurrentFoundParameterIndex);
                    if (parameter != null)
                    {
                        ticket.CurrentFoundParameterIndex++;
                    }
                }
            }
            else if (this.Ticket.ifUseAvailableParameters)
            {
                IEnumerable<AXSParameter> tmpParamters = this.Ticket.Parameters.Where(p => p.IfAvailable == true);
                if (tmpParamters != null)
                {
                    if (ticket.CurrentAvailableParameterIndex >= tmpParamters.Count())
                    {
                        ticket.CurrentAvailableParameterIndex = 0;
                    }
                    parameter = tmpParamters.ElementAtOrDefault(ticket.CurrentAvailableParameterIndex);
                    if (parameter != null)
                    {
                        ticket.CurrentAvailableParameterIndex++;
                    }
                }
            }

            if (parameter == null)
            {
                if (ticket.CurrentParameterIndex >= this.Ticket.Parameters.Count)
                {
                    ticket.CurrentParameterIndex = 0;
                }

                parameter = this.Ticket.Parameters[ticket.CurrentParameterIndex];

                ticket.CurrentParameterIndex++;
            }
            return parameter;
        }
        protected AXSPriceLevel getPriceLevel(ITicketParameter parameter, List<AXSPriceLevel> PriceLevels)
        {
            AXSPriceLevel selectedPriceLevel = null;

            //If user provides the price range then find the right price level and return the price level.
            if (parameter.PriceMin != null && parameter.PriceMax != null && PriceLevels != null)
            {
                SortedList<string, decimal> sortedPriceLevels = new SortedList<string, decimal>();

                foreach (AXSPriceLevel priceLevel in PriceLevels)
                {
                    sortedPriceLevels.Add(priceLevel.PriceSecName, priceLevel.TotalPrice);
                }

                // If user wants to find the right price level from max total price to min total price.
                if (parameter.MaxToMin)
                {
                    IOrderedEnumerable<KeyValuePair<string, decimal>> sortedPriceLevelsDesc = sortedPriceLevels.OrderByDescending(p => p.Value);
                    foreach (KeyValuePair<string, decimal> priceLevel in sortedPriceLevelsDesc)
                    {
                        if (priceLevel.Value >= parameter.PriceMin && priceLevel.Value <= parameter.PriceMax)
                        {
                            foreach (AXSPriceLevel item in PriceLevels)
                            {
                                if (item.PriceSecName == priceLevel.Key)
                                {
                                    selectedPriceLevel = item;
                                }
                            }
                            break;
                        }
                    }
                }
                else // If user wants to find the right price level from min total price to max total price.
                {
                    foreach (KeyValuePair<string, decimal> priceLevel in sortedPriceLevels)
                    {
                        if (priceLevel.Value >= parameter.PriceMin && priceLevel.Value <= parameter.PriceMax)
                        {
                            foreach (AXSPriceLevel item in PriceLevels)
                            {
                                if (item.PriceSecName == priceLevel.Key)
                                {
                                    selectedPriceLevel = item;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            else // if user does not provide the price level then male the empty price level and return it.
            {
                selectedPriceLevel = new AXSPriceLevel(String.Empty, String.Empty,0);
            }

            return selectedPriceLevel;
        }
        public Boolean mapParameterIfAvaiable(ITicketParameter parameter)
        {
           
            Boolean ifTicketTypeStringMatch = false;
            Boolean result = false;
            AXSPriceLevel selectedPriceLevel = null;
            this.doc.LoadHtml(post(this, this.Ticket.XmlUrl + "?methodName=showshop.seriesInfoW&wroom=" + this.wRoom + "&lang=en&ver=$Name:%20eventShopperV3_V3_1Cd%20$", String.Format("<methodCall><methodName>showshop.seriesInfoW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param></params></methodCall>", this.wRoom)));
            string parentIssCode = this.doc.DocumentNode.SelectSingleNode("//name[text() = 'parentIssCode']").NextSibling.NextSibling.ChildNodes[0].InnerHtml;
            if (this._tmEvent != null && parameter != null)
            {
                // If the Ticket Types exists in the first page then proceed further otherwise show message and keep searching.
                if (this._tmEvent.HasSections)
                {
                    // If user provides the Search String or Ticket Type String. Then Select price type according to parameter.
    
                        foreach (AXSSection item in this._tmEvent.Sections)
                        {
                            if (!String.IsNullOrEmpty(parameter.DateTimeString))
                            {
                                DateTime dt = Convert.ToDateTime(item.EventDates);
                                DateTime dt1 = Convert.ToDateTime(parameter.DateTimeString);
                                item.EventDates = String.Format("{0:MM/dd/yyyy}", dt);
                                parameter.DateTimeString = String.Format("{0:MM/dd/yyyy}", dt1);
                                // Check if user mark checed on Exact Match or not and find for the provided Search String or Ticket Type String.
                                if (parameter.ExactMatch ? item.EventDates.ToLower() == parameter.DateTimeString.ToLower() : item.EventDates.ToLower().Contains(parameter.DateTimeString.ToLower()))
                                {
                                    this.code = item.EventCode;
                                    if (!String.IsNullOrEmpty(parameter.PriceLevelString))
                                    {
                                        // Select priceLevel from the matching DateTime Search String or Ticket Type String.
                                        foreach (AXSPriceLevel priceLevel in item.PriceLevels)
                                        {
                                            if (parameter.ExactMatch ? priceLevel.PriceSecName.ToLower() == parameter.PriceLevelString.ToLower() : priceLevel.PriceSecName.ToLower().Contains(parameter.PriceLevelString.ToLower()))
                                            {
                                                // Find and get price level and map it to the post request.
                                                if (this._CurrentParameter.PriceMin != null && this._CurrentParameter.PriceMax != null)
                                                {
                                                    selectedPriceLevel = getPriceLevel(parameter, item.PriceLevels);
                                                }
                                                else
                                                {
                                                    selectedPriceLevel = priceLevel;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (AXSPriceLevel priceLevel in item.PriceLevels)
                                        {
                                            // Find and get price level and map it to the post request.
                                            if (this._CurrentParameter.PriceMin != null && this._CurrentParameter.PriceMax != null)
                                            {
                                                selectedPriceLevel = getPriceLevel(parameter, item.PriceLevels);
                                            }
                                            else
                                            {
                                                selectedPriceLevel = priceLevel;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                    this.code = item.EventCode;
                                    if (!String.IsNullOrEmpty(parameter.PriceLevelString))
                                    {
                                        // Select priceLevel from the matching DateTime Search String or Ticket Type String.
                                        foreach (AXSPriceLevel priceLevel in item.PriceLevels)
                                        {
                                            if (parameter.ExactMatch ? priceLevel.PriceSecName.ToLower() == parameter.PriceLevelString.ToLower() : priceLevel.PriceSecName.ToLower().Contains(parameter.PriceLevelString.ToLower()))
                                            {
                                                // Find and get price level and map it to the post request.
                                                if (this._CurrentParameter.PriceMin != null && this._CurrentParameter.PriceMax != null)
                                                {
                                                    selectedPriceLevel = getPriceLevel(parameter, item.PriceLevels);
                                                }
                                                else
                                                {
                                                    selectedPriceLevel = priceLevel;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (AXSPriceLevel priceLevel in item.PriceLevels)
                                        {
                                            // Find and get price level and map it to the post request.
                                            if (this._CurrentParameter.PriceMin != null && this._CurrentParameter.PriceMax != null)
                                            {
                                                selectedPriceLevel = getPriceLevel(parameter, item.PriceLevels);
                                            }
                                            else
                                            {
                                                selectedPriceLevel = priceLevel;
                                            }
                                            break;
                                        }
                                    }
                                    break;
                            }
                    }
                    if (parentIssCode != string.Empty)
                    {
                        this.SessionKey = createSessionKey(parentIssCode, this);
                    }
                    else
                    {
                        this.SessionKey = createSessionKey(this.wRoom, this);
                    }
                    ifTicketTypeStringMatch = true;
                    doc.LoadHtml(post(this, this.Ticket.XmlUrl + "?methodName=showshop.availW&wroom=" + this.wRoom, String.Format("<methodCall><methodName>showshop.availW</methodName><params><param><value><string>{0}</string></value></param></params></methodCall>", this.wRoom)));
                    doc.LoadHtml(doc.DocumentNode.SelectSingleNode("//name[text() = 'eventavail_ss']").NextSibling.NextSibling.InnerHtml);
                    string sectionQuery = "//struct/member/name[text() = '" + this.code + "']";
                    doc.LoadHtml(doc.DocumentNode.SelectSingleNode(sectionQuery).NextSibling.NextSibling.InnerHtml);
                    HtmlNodeCollection sections = doc.DocumentNode.SelectNodes("//value/string");
                    List<string> filteredSections = new List<string>();

                    foreach (HtmlNode tmp in sections)
                    {
                        filteredSections.Add(tmp.InnerHtml);
                    }
                    doc = new HtmlAgilityPack.HtmlDocument();

                    doc.LoadHtml(post(this, this.Ticket.XmlUrl + "?methodName=showshop.seriesInfoW&wroom=" + this.wRoom + "&lang=en&ver=$Name:%20eventShopperV3_V3_1Cd%20$", String.Format("<methodCall><methodName>showshop.seriesInfoW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param></params></methodCall>", this.wRoom)));

                    doc.LoadHtml(doc.DocumentNode.SelectSingleNode("//member/name[text() = 'subsections']").ParentNode.OuterHtml);
                    HtmlNodeCollection crossSections = doc.DocumentNode.SelectNodes("/member/value/struct/member/name");

                    List<string> crossSectionsList = new List<string>();

                    foreach (HtmlNode tmp in crossSections)
                    {
                        crossSectionsList.Add(tmp.InnerHtml);
                    }

                    HtmlNodeCollection plevels = doc.DocumentNode.SelectNodes("/member/value/struct/member/value/struct/member/name[text() ='plevel']");
                    List<string> plevelList = new List<string>();

                    foreach (HtmlNode tmp in plevels)
                    {
                        plevelList.Add(tmp.NextSibling.NextSibling.ChildNodes[0].InnerHtml);
                    }

                    List<AXSPriceLevel> PriceLevelObjectList = new List<AXSPriceLevel>();

                    if (crossSectionsList.Count == plevelList.Count)
                    {
                        for (int i = 0; i < crossSectionsList.Count; i++)
                        {
                            PriceLevelObjectList.Add(new AXSPriceLevel(crossSectionsList[i], plevelList[i], 0));
                        }
                    }
                    filteredSections = filteredSections.Intersect(crossSectionsList).ToList();

                    PriceLevelObjectList.RemoveAll(p => !filteredSections.Contains(p.PriceSecName));

                    XmlDocument xDoc = new XmlDocument();
                    XmlElement el = (XmlElement)xDoc.AppendChild(xDoc.CreateElement("methodCall"));

                    el.AppendChild(xDoc.CreateElement("methodName")).InnerText = "webapi.searchSeats2";
                    XmlElement Params = (XmlElement)el.AppendChild(xDoc.CreateElement("params"));

                    XmlElement par = (XmlElement)Params.AppendChild(xDoc.CreateElement("param"));
                    XmlElement value = (XmlElement)par.AppendChild(xDoc.CreateElement("value"));

                    value.AppendChild(xDoc.CreateElement("string")).InnerText = this.SessionKey;

                    par = (XmlElement)Params.AppendChild(xDoc.CreateElement("param"));
                    value = (XmlElement)par.AppendChild(xDoc.CreateElement("value"));

                    value.AppendChild(xDoc.CreateElement("string")).InnerText = this.code;

                    par = (XmlElement)Params.AppendChild(xDoc.CreateElement("param"));
                    value = (XmlElement)par.AppendChild(xDoc.CreateElement("value"));

                    value.AppendChild(xDoc.CreateElement("int")).InnerText = this._CurrentParameter.Quantity.ToString(); //quantity of seats

                    par = (XmlElement)Params.AppendChild(xDoc.CreateElement("param"));
                    value = (XmlElement)par.AppendChild(xDoc.CreateElement("value"));

                    value.AppendChild(xDoc.CreateElement("array")).InnerText = "<data />";

                    par = (XmlElement)Params.AppendChild(xDoc.CreateElement("param"));
                    value = (XmlElement)par.AppendChild(xDoc.CreateElement("value"));

                    value.AppendChild(xDoc.CreateElement("array")).InnerText = "<data />";

                    par = (XmlElement)Params.AppendChild(xDoc.CreateElement("param"));
                    value = (XmlElement)par.AppendChild(xDoc.CreateElement("value"));

                    value.AppendChild(xDoc.CreateElement("array")).InnerText = "<data />";

                    /******************************************************/
                    //Now we add the subsection to the xml, in which we want
                    //our seats to be searched.
                    /******************************************************/
                    par = (XmlElement)Params.AppendChild(xDoc.CreateElement("param"));
                    value = (XmlElement)par.AppendChild(xDoc.CreateElement("value"));

                    XmlElement arr = (XmlElement)value.AppendChild(xDoc.CreateElement("array"));
                    XmlElement data = (XmlElement)arr.AppendChild(xDoc.CreateElement("data"));

                    /******************************************************/
                    //change the price level number accordingly.
                    //1 = Floor standing , 2 = GA etc.
                    /******************************************************/
                    if (!String.IsNullOrEmpty(this._CurrentParameter.PriceLevelString)&&this._CurrentParameter.PriceLevelString.ToLower().Contains("best"))
                    {
                        PriceLevelObjectList = PriceLevelObjectList.Where(p => p.PriceSectionNumber == "0").ToList();
                    }
                    else
                    {
                        PriceLevelObjectList = PriceLevelObjectList.Where(p => p.PriceSectionNumber == selectedPriceLevel.PriceSectionNumber).ToList();
                    }

                    foreach (AXSPriceLevel tmp in PriceLevelObjectList)
                    {
                        XmlElement tVal = (XmlElement)data.AppendChild(xDoc.CreateElement("value"));
                        tVal.AppendChild(xDoc.CreateElement("string")).InnerText = tmp.PriceSecName;
                    }
                    if ((!String.IsNullOrEmpty(this._CurrentParameter.PriceLevelString)&&this._CurrentParameter.PriceLevelString.ToLower().Contains("best") && PriceLevelObjectList.Count == 0) || PriceLevelObjectList.Count != 0)
                    {
                        par = (XmlElement)Params.AppendChild(xDoc.CreateElement("param"));
                        value = (XmlElement)par.AppendChild(xDoc.CreateElement("value"));

                        value.AppendChild(xDoc.CreateElement("boolean")).InnerText = "0";

                        /******************************************************/
                        //Now include the session key
                        /******************************************************/
                        par = (XmlElement)Params.AppendChild(xDoc.CreateElement("param"));
                        value = (XmlElement)par.AppendChild(xDoc.CreateElement("value"));

                        value.AppendChild(xDoc.CreateElement("string")).InnerText = this.SessionKey;

                        /******************************************************/
                        //Now fake the btnConfirm counter
                        /******************************************************/
                        par = (XmlElement)Params.AppendChild(xDoc.CreateElement("param"));
                        value = (XmlElement)par.AppendChild(xDoc.CreateElement("value"));

                        value.AppendChild(xDoc.CreateElement("string")).InnerText = ".btnConfirm:84:13:1";
                        doc.LoadHtml(post(this, this.Ticket.XmlUrl.Replace("/info/", "/bfox/") + "?methodName=webapi.searchSeats2&serverStr=" + this.Ticket.XmlUrl.Replace("/info/", "/bfox/") + "&sessionKey=" + this.SessionKey + "&XMLRPC_callCounter=5&cc=.btnConfirm:84:13:1", xDoc.InnerXml));
                        result = true;



                    }

                }
                if (!ifTicketTypeStringMatch)
                {
                    this.MoreInfo = "\"" + parameter.PriceLevelString + "\" " + TicketSearchStatus.MoreInfoTicketTypeStringNotMatch;
                    return false;
                }
               
            }
            else
            {

            }

                    // If Search String or Price Type String does not match then show message.


                
            
                // If user provides the only Password and not Search String or Ticket Type String. Then select first available password field according to parameter and add it to the post request.

            
            return result;
        }
        void resetAfterStopHandler()
        {
            this.start();
            this._ifRestarting = false;
        }
        

       
       

       


       

      

        private void changeStatus(String status)
        {
            try
            {
                if (this.IfWorking && this.Ticket.isRunning)
                {
                    this.Status = status;
                }
                else if (!this.IfWorking && this.Ticket.isRunning && this.resetAfterStop != null)
                {
                    this.Status = status;
                }

                if (this.Ticket.Searches.changeDelegate != null)
                {
                    this.Ticket.Searches.changeDelegate(this);
                }

                if (this.Ticket.onChangeForGauge != null)
                {
                    this.Ticket.onChangeForGauge();
                }

                if (this._proxy != null)
                {
                    String strProxy = _proxy.ToString() + ", ";
                    if (!this.MoreInfo.StartsWith(strProxy))
                    {
                        this.MoreInfo = strProxy + this.MoreInfo;
                    }                    
                }
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region Constructor
        public AXSSearch(AXSTicket ticket)
        {
            this.Ticket = ticket;
            this.FlagImage = global::Automatick.Properties.Resources.Flag16Disable;
            this.FlagImage.Tag = false;
        }
        #endregion

     

        public void Dispose()
        {
            try
            {
                if (_md5 != null)
                {
                    _md5.Clear();
                    GC.SuppressFinalize(_md5);
                    //_md5 = null;
                }
               
                GC.SuppressFinalize(this);
                GC.Collect();
            }
            catch (Exception)
            {

            }
        }

       public String createSessionKey(string parentIssCode, AXSSearch search)
        {
            if (search._CurrentParameter.TicketTypePasssword == string.Empty)
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                if (this.Ticket.XmlUrl.Contains("las.eventshopper.com"))
                {
                    doc.LoadHtml(post(search, this.Ticket.XmlUrl.Replace("/info/", "/bfox/") + "?methodName=webapi.sessionCreateW&serverStr=" + this.Ticket.XmlUrl.Replace("/info/", "/bfox/") + "&wrLotID=noLotId&esVer=eventShopperV3_V3_1Cd", String.Format("<methodCall><methodName>webapi.sessionCreateW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param><param><value><string></string></value></param><param><value><string>https://tickets.axs.com/eventShopperV3.html?wr={0}&amp;lang=en&amp;skin=axs_colosseumv3&amp;preFill=1&amp;fbShareURL=www.axs.com%2Fevents%2F3044%2Fjerry-seinfeld%3Fu%3D231473%26ref%3Devs_fb&amp;src=AEGAXS1_WMAIN&amp;event=3044&amp;event=3044</string></value></param><param><value><string>noLotId</string></value></param><param><value><string>formPrefilled:false</string></value></param><param><value><string>eventShopperV3_V3_1Cd</string></value></param><param><value><string>https://tickets.axs.com/eventShopperV3.html?wr={0}&amp;lang=en&amp;skin=axs_colosseumv3&amp;preFill=1&amp;fbShareURL=www.axs.com%2Fevents%2F3044%2Fjerry-seinfeld%3Fu%3D231473%26ref%3Devs_fb&amp;src=AEGAXS1_WMAIN&amp;event=3044&amp;event=3044</string></value></param></params></methodCall>", search.wRoom)));
                    return doc.DocumentNode.SelectSingleNode("//name[text() = 'sk']").NextSibling.NextSibling.ChildNodes[0].InnerHtml;
                }
                else
                {
                    doc.LoadHtml(post(search, this.Ticket.XmlUrl.Replace("/info/", "/bfox/") + "?methodName=webapi.sessionCreateW&serverStr=" + this.Ticket.XmlUrl.Replace("/info/", "/bfox/") + "&wrLotID=noLotId&esVer=eventShopperV3_V3_1Cd", String.Format("<methodCall><methodName>webapi.sessionCreateW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param><param><value><string></string></value></param><param><value><string>https://tickets.axs.com/eventShopperV3Invite.html?wr={1}&amp;skin=verizontheatre</string></value></param><param><value><string>noLotId</string></value></param><param><value><string>formPrefilled:false</string></value></param><param><value><string>eventShopperV3_V3_1Da</string></value></param><param><value><string>https://tickets.axs.com/eventShopperV3Invite.html?wr={1}amp;skin=verizontheatre</string></value></param></params></methodCall>", parentIssCode, this.wRoom)));
                    return doc.DocumentNode.SelectSingleNode("//name[text() = 'sk']").NextSibling.NextSibling.ChildNodes[0].InnerHtml;
                }
            }
            else
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(post(search, this.Ticket.XmlUrl.Replace("/info/", "/bfox/") + "?methodName=webapi.sessionCreateW&serverStr=" + this.Ticket.XmlUrl.Replace("/info/", "/bfox/") + "&wrLotID=noLotId&esVer=eventShopperV3_V3_1Cd", String.Format("<methodCall><methodName>webapi.sessionCreateW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param><param><value><string>{2}</string></value></param><param><value><string>https://tickets.axs.com/eventShopperV3Invite.html?wr={1}&amp;skin=verizontheatre</string></value></param><param><value><string>noLotId</string></value></param><param><value><string>formPrefilled:false</string></value></param><param><value><string>eventShopperV3_V3_1Da</string></value></param><param><value><string>https://tickets.axs.com/eventShopperV3Invite.html?wr={1}amp;skin=verizontheatre</string></value></param></params></methodCall>", parentIssCode, this.wRoom, search._CurrentParameter.TicketTypePasssword)));

                return doc.DocumentNode.SelectSingleNode("//name[text() = 'sk']").NextSibling.NextSibling.ChildNodes[0].InnerHtml;
            }
        }
        public static String post(AXSSearch search, string url, string postdata)
        {

            string URL = url;
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(URL);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            if (search.Proxy != null)
            {
                if (!String.IsNullOrEmpty(search.Proxy.Address.Trim()) && !String.IsNullOrEmpty(search.Proxy.Port.Trim()))
                {
                    webRequest.Proxy = new System.Net.WebProxy(search.Proxy.Address.Trim(), int.Parse(search.Proxy.Port.Trim()));
                }
                if (!String.IsNullOrEmpty(search.Proxy.UserName.Trim()) && !String.IsNullOrEmpty(search.Proxy.Password.Trim()))
                {
                    webRequest.Proxy.Credentials = new System.Net.NetworkCredential(search.Proxy.UserName.Trim(), search.Proxy.Password.Trim());
                }
            }
            Stream reqStream = webRequest.GetRequestStream();
            string postData = postdata;
            byte[] postArray = Encoding.ASCII.GetBytes(postData);
            reqStream.Write(postArray, 0, postArray.Length);
            reqStream.Close();
            StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream());
            string Result = sr.ReadToEnd();

            return Result;


        }
       
    }
}
