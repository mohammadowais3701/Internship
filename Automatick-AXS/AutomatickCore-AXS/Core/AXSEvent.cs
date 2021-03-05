using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.IO;
using System.IO.Compression;
using System.Globalization;
using System.Net;
using Automatick.Logging;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Automatick.Core
{
    [Serializable]
    public class AXSEvent
    {
        public Boolean ifJson
        {
            get;
            set;
        }
        public AXSSearch Search
        {
            get;
            set;
        }
        public Boolean IfTicketAlive
        {
            get;
            set;
        }
        public String EventId
        {
            get;
            set;
        }
        public String EventName
        {
            get;
            set;
        }
        public String EventVenue
        {
            get;
            set;
        }

        public String EventDateTime
        {
            get;
            set;
        }
        public int NoOfTicket
        {
            get;
            set;
        }
        public int MaxNoOfTicket
        {
            get;
            set;
        }

        public Boolean IsExpired
        {
            get
            {
                Boolean result = false;
                try
                {
                    if (!String.IsNullOrEmpty(EventDateTime))
                    {
                        DateTime dt = DateTime.Parse(EventDateTime);
                        if (dt < DateTime.Now)
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        result = true;
                    }
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            }
        }

        public HtmlDocument XML
        {
            get;
            set;
        }

        public List<AXSSection> Sections
        {
            get;
            set;
        }
        public Boolean HasSections
        {
            get
            {
                if (Sections != null)
                {
                    if (Sections.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public List<AXSPriceLevel> PriceLevels
        {
            get;
            set;
        }

        public Boolean HasTixPL
        {
            get
            {
                if (TixPriceLevels != null)
                {
                    if (TixPriceLevels.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public List<AXSTicketType> TicketTypes
        {
            get;
            set;
        }

        public List<AXSTixPriceLevels> TixPriceLevels
        {
            get;
            set;
        }

        public List<AXSTixPriceTypes> TixTicketTypes
        {
            get;
            set;
        }

        public List<AXSTixResale> ResaleTickets
        {
            get;
            set;
        }

        public Boolean HasResaleTickets
        {
            get
            {
                if (!this.Search.isTix)
                {
                    if (ResaleTickets != null)
                    {
                        if (ResaleTickets.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (ResaleTickets != null)
                    {
                        if (ResaleTickets.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

        }

        public Boolean HasTicketTypes
        {
            get
            {
                if (!this.Search.isTix)
                {
                    if (TicketTypes != null)
                    {
                        if (TicketTypes.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (TixTicketTypes != null)
                    {
                        if (TixTicketTypes.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public Dictionary<string, int> quantityList
        {
            get;
            set;
        }

        public Dictionary<String, String> offerIds { get; set; }

        public int TicketLimitPerAccountForAutoBuy
        {
            get;
            set;
        }

        public string SectionCode
        {
            get;
            set;
        }

        public Boolean is3dsecureEnabled { get; set; }

        public AXSEvent(HtmlDocument xml, AXSSearch search)
        {
            this.XML = xml;
            this.Search = search;
            Sections = new List<AXSSection>();
            PriceLevels = new List<AXSPriceLevel>();
            this.TicketTypes = new List<AXSTicketType>();

            IfTicketAlive = true;
            if (parseEvent())
            {
                IfTicketAlive = true;
            }
            else
            {
                IfTicketAlive = false;
            }
        }

        private Boolean parseEvent()
        {
            try
            {
                Boolean result = false;
                string jsonStr = string.Empty;

                if (this.Search.isTix)
                {
                    result = parseTix();
                }
                else
                {
                    int i;
                    if (this.Search.Ticket.URL.Contains("wroom.centrebell.ca"))
                    {
                        this.XML.LoadHtml(Search.post(this.Search, this.Search.XmlUrl + "?methodName=showshop.seriesInfoW&wroom=" + this.Search.wRoom + "&lang=+this.Search.Ticket.lang+&ver=3.0.54.15", String.Format("<methodCall><methodName>showshop.seriesInfoW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>{1}</string></value></param></params></methodCall>", this.Search.wRoom, this.Search.Ticket.lang)));
                    }
                    else if (this.Search.Ticket.URL.Contains("/shop/") || this.Search.Ticket.URL.Contains("/#/"))
                    {
                        //if (!string.IsNullOrEmpty(jsonStr))
                        {
                            this.ifJson = true;
                        }
                    }
                    else
                    {
                        if (!Search.isMobile)
                        {
                            this.XML.LoadHtml(this.Search.post(this.Search, this.Search.XmlUrl + "?methodName=showshop.seriesInfoW&wroom=" + this.Search.wRoom + "&lang=en", String.Format("<methodCall><methodName>showshop.seriesInfoW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param></params></methodCall>", this.Search.wRoom)));
                        }
                    }


                    if (this.ifJson)
                    {
                        ExtractInfo();
                        result = parseJSON();
                    }
                    else
                    {
                        if (this.Search.isWeb || this.Search.isEventko)
                        {
                            //ExtractInfo();
                            result = parseXML();
                        }
                        else if (this.Search.isMobile)
                        {
                            result = parseMobile();
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.LoggerInstance.Add(new Log(ErrorType.EXCEPTION, this.Search.Ticket.TicketID, this.Search.Ticket.URL, ex.Message + ex.StackTrace));
                return false;
            }
        }

        private void ExtractInfo()
        {
            try
            {
                String strHtml = get(this.Search, "https://tickets.axs.com/goa/applications/EventShopper5/schemas/2.2release/cascades/" + this.Search.wRoom + "/configs/");

                if (!String.IsNullOrEmpty(strHtml))
                {
                    JObject obj = JObject.Parse(strHtml);

                    if (obj.Property("AXSLogin") != null)
                    {
                        JObject axsLogin = (JObject)obj["AXSLogin"];

                        if (axsLogin.Property("skipAsGuest") != null)
                        {
                            JObject guest = (JObject)axsLogin["skipAsGuest"];

                            if (guest.Property("enabled") != null)
                            {
                                this.Search.SkipAsGuest = Convert.ToBoolean(guest["enabled"]);
                            }
                        }

                        if (axsLogin.Property("production") != null)
                        {
                            JObject guest = (JObject)axsLogin["production"];

                            if (guest.Property("clientSecret") != null)
                            {
                                this.Search.Client_Secret = Convert.ToString(guest["clientSecret"]);
                            }
                            if (guest.Property("clientID") != null)
                            {
                                this.Search.Client_ID = Convert.ToString(guest["clientID"]);
                            }
                        }
                    }

                    if (obj.Property("extra_api") != null)
                    {
                        JObject axsLogin = (JObject)obj["extra_api"];

                        if (axsLogin.Property("axs") != null)
                        {
                            JObject guest = (JObject)axsLogin["axs"];

                            if (guest.Property("ExtraUrlParams") != null)
                            {
                                JObject ExtraUrlParams = (JObject)guest["ExtraUrlParams"];

                                if (ExtraUrlParams.Property("access_token") != null)
                                {
                                    this.Search.AccessToken = Convert.ToString(ExtraUrlParams["access_token"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private Boolean parseMobile()
        {
            Boolean result = false;
            int qtyStep = 0;

            try
            {
                #region Mobile old url working
                //HtmlDocument docPage = new HtmlDocument();
                //docPage.LoadHtml(this.XML);

                HtmlNodeCollection nodeQuantity = this.XML.DocumentNode.SelectNodes("//select[@id='qty']/option");
                if (nodeQuantity != null)
                {
                    this.TicketLimitPerAccountForAutoBuy = nodeQuantity.Count - 1;
                    quantityList = new Dictionary<string, int>();

                    foreach (HtmlNode qunatity in nodeQuantity)
                    {
                        if (Convert.ToInt32(qunatity.Attributes["Value"].Value.Trim()) > 0)
                        {
                            try
                            {
                                quantityList.Add(qunatity.Attributes["Value"].Value.Trim(), Convert.ToInt32(qunatity.NextSibling.InnerText.Replace("\n", "").Trim()));
                                if (qtyStep == 0)
                                    qtyStep = Convert.ToInt32(qunatity.NextSibling.InnerText.Replace("\n", "").Trim());

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                else
                {
                    // <fieldset data-role="controlgroup" data-type="horizontal">
                    nodeQuantity = this.XML.DocumentNode.SelectNodes("//fieldset[@data-type='horizontal']/input[@type='radio']");
                    if (nodeQuantity != null)
                    {
                        this.TicketLimitPerAccountForAutoBuy = nodeQuantity.Count - 1;
                        quantityList = new Dictionary<string, int>();

                        foreach (HtmlNode qunatity in nodeQuantity)
                        {
                            if (Convert.ToInt32(qunatity.Attributes["Value"].Value.Trim()) > 0)
                            {
                                try
                                {
                                    //<label for="qty-1">1</label>
                                    HtmlNode quantityLabel = qunatity.SelectSingleNode("//label[@for='" + qunatity.Attributes["id"].Value.Trim() + "']");

                                    quantityList.Add(qunatity.Attributes["Value"].Value.Trim(), Convert.ToInt32(quantityLabel.InnerText.Replace("\n", "").Trim()));
                                    if (qtyStep == 0)
                                        qtyStep = Convert.ToInt32(quantityLabel.InnerText.Replace("\n", "").Trim());

                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                }

                AXSTicketType ticketType = new AXSTicketType("", qtyStep, TicketLimitPerAccountForAutoBuy);

                try
                {
                    HtmlNodeCollection nodeSplitValues = this.XML.DocumentNode.SelectNodes("//input[@name='acceptSplit']");
                    if (nodeSplitValues != null)
                    {
                        ticketType.AcceptSplit = new Dictionary<string, string>();
                        foreach (HtmlNode split in nodeSplitValues)
                        {
                            ticketType.AcceptSplit.Add(split.Attributes["id"].Value.Trim(), split.Attributes["value"].Value.Trim());
                        }
                    }

                    try
                    {
                        HtmlNodeCollection nodePrice = this.XML.DocumentNode.SelectNodes("//select[@id='plevelselect']/option");
                        //HtmlNodeCollection nodePrice = docPage.DocumentNode.SelectNodes("//ul[@id='priceElementList']/li/span[@class='price-level-range']");
                        if (nodePrice != null)
                        {
                            foreach (HtmlNode itemPrice in nodePrice)
                            {
                                string pLevelValue = string.Empty, pLevelName = string.Empty, tempPrice = string.Empty;
                                decimal price = 0;

                                //Old method for extracting Price level Value
                                //pLevelValue = itemPrice.Attributes["Value"].Value.Trim();

                                //New method for extracting Price level Value
                                if (itemPrice.Attributes.Contains("jspricelevelcode"))
                                    pLevelValue = itemPrice.Attributes["jspricelevelcode"].Value.Trim();
                                else
                                    pLevelValue = string.Empty;

                                //Old method for extracting Price level Name
                                //if (itemPrice.NextSibling.InnerText.Contains("["))
                                //    pLevelName = itemPrice.NextSibling.InnerText.Remove(itemPrice.NextSibling.InnerText.IndexOf('[')).Replace("\n", "").Trim();

                                //New method for extracting Price level Name
                                if (itemPrice.Attributes.Contains("jsname"))
                                    pLevelName = itemPrice.Attributes["jsname"].Value.Trim();
                                else
                                    pLevelName = "Best available";

                                //New method for extracting Price Digits
                                if (itemPrice.Attributes.Contains("jsminprice"))
                                {
                                    tempPrice = itemPrice.Attributes["jsminprice"].Value.Trim();
                                    if (!string.IsNullOrEmpty(tempPrice))
                                        price = Convert.ToDecimal(tempPrice);
                                }
                                AXSPriceLevel pLevel = new AXSPriceLevel(pLevelValue, pLevelName, price);
                                ticketType.PriceLevels.Add(pLevel);
                            }
                        }
                        else
                        {
                            nodePrice = this.XML.DocumentNode.SelectNodes("//ul[@id='priceElementList']/li");
                            if (nodePrice != null)
                            {
                                foreach (HtmlNode itemPrice in nodePrice)
                                {
                                    string pLevelValue = string.Empty, pLevelName = string.Empty, tempPrice = string.Empty;
                                    decimal price = 0;

                                    pLevelValue = itemPrice.Attributes["jspricelevelcode"].Value.Trim();
                                    HtmlNode nodePriceLevelName = itemPrice.SelectSingleNode("span[@class='price-level-range']/strong");
                                    if (nodePriceLevelName != null)
                                    {
                                        pLevelName = nodePriceLevelName.InnerHtml.Trim();
                                    }

                                    HtmlNode nodePriceItem = itemPrice.SelectSingleNode(".//span[@class='monetary-amount']");
                                    if (nodePriceItem != null)
                                    {
                                        // remain work for - sign
                                        price = Convert.ToDecimal(nodePriceItem.InnerHtml.Trim());

                                        if (!nodePriceItem.InnerHtml.Contains("Sold Out"))
                                        {
                                            if (nodePriceItem.InnerHtml.Contains("-"))
                                                price = Convert.ToDecimal(nodePriceItem.InnerHtml.Trim().Remove(tempPrice.IndexOf('-')));
                                            else
                                                price = Convert.ToDecimal(nodePriceItem.InnerHtml.Trim());
                                        }
                                        else
                                        {
                                            pLevelName = pLevelName + nodePriceItem.InnerHtml.Trim();
                                        }
                                    }

                                    AXSPriceLevel pLevel = new AXSPriceLevel(pLevelValue, pLevelName, price);
                                    ticketType.PriceLevels.Add(pLevel);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    this.TicketTypes.Add(ticketType);
                    result = true;


                    HtmlNode nodeEventName = this.XML.DocumentNode.SelectSingleNode("//ul[@data-role='listview']/li/h3");
                    this.EventName = (nodeEventName != null) ? nodeEventName.InnerText.Replace("\n", "").Trim() : null;

                    HtmlNodeCollection nodeEventDateVenue = this.XML.DocumentNode.SelectNodes("//ul[@data-role='listview']/li//div");
                    if (nodeEventDateVenue != null)
                    {
                        foreach (HtmlNode nodes in nodeEventDateVenue)
                        {
                            DateTime dateTime;
                            if (DateTime.TryParse(nodes.InnerText.Replace("\n", "").Trim(), out dateTime))
                                this.EventDateTime = nodes.InnerText.Replace("\n", "").Trim();
                            else
                                this.EventVenue += nodes.InnerText.Replace("\n", "").Trim() + " ";
                        }
                    }
                }
                catch
                {
                }
                #endregion
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return result;
        }

        private Boolean parseXML()
        {
            Boolean result = false;
            String jsonStr = String.Empty;
            int i = 0;

            try
            {
                #region XML parsing
                //ProxyPicker.ProxyPickerInstance.RecheckProxyStatus(Search.Proxy, this.XML.DocumentNode.InnerHtml);
                this.XML.LoadHtml(this.XML.DocumentNode.InnerHtml.Replace("<param>", ""));

                #region work for section
                HtmlNodeCollection eventTypeCodeCollection = this.XML.DocumentNode.SelectNodes("//methodresponse/params/value/array/data/value/struct/member/name[text() = 'eventTypeCode']");
                HtmlNodeCollection eventCodeCollection = this.XML.DocumentNode.SelectNodes("//methodresponse/params/value/array/data/value/struct/member/name[text() = 'eventCode']");
                HtmlNodeCollection eventDatesCollection = this.XML.DocumentNode.SelectNodes("//methodresponse/params/value/array/data/value/struct/member/name[text() = 'date']");


                if ((eventTypeCodeCollection != null) && (eventTypeCodeCollection.Count == eventCodeCollection.Count))
                {
                    for (i = 0; i < eventTypeCodeCollection.Count; i++)
                    {
                        PriceLevels = new List<AXSPriceLevel>();
                        string eventTypeCode = eventTypeCodeCollection[i].NextSibling.NextSibling.ChildNodes[0].InnerHtml;
                        string eventCode = eventCodeCollection[i].NextSibling.NextSibling.ChildNodes[0].InnerHtml;
                        string eventDate = eventDatesCollection[i].NextSibling.NextSibling.ChildNodes[0].InnerHtml;


                        AXSSection section = new AXSSection(eventTypeCode, eventCode, eventDate, null);
                        this.Sections.Add(section);

                        this.Search.Sections.Add(section);
                    }
                }

                if (this.Sections != null && this.Sections.Count > 1)
                {
                    if (this.Search._CurrentParameter.DateTimeString == "mm/dd/yyyy" || String.IsNullOrEmpty(this.Search._CurrentParameter.DateTimeString))
                    {
                        if (!String.IsNullOrEmpty(this.Search.Ticket.SelectedDate))
                        {
                            this.Search._CurrentParameter.DateTimeString = this.Search.Ticket.SelectedDate;
                            this.Search._CurrentParameter.EventTime = this.Search.Ticket.SelectedEventTime;
                        }
                        else
                        {
                            this.Search.getExtractedDatesForm(this.Search);
                        }
                    }
                }
                #endregion

                #region third req
                decimal price = 0;
                if (this.Search._CurrentParameter != null)
                {
                    if (!String.IsNullOrEmpty(this.Search._CurrentParameter.DateTimeString))
                    {
                        if (this.Search._CurrentParameter.DateTimeString != "mm/dd/yyyy")
                        {
                            CultureInfo provider = new CultureInfo("en-US");
                            DateTime dt1 = DateTime.ParseExact(this.Search._CurrentParameter.DateTimeString.ToString(), "MM/dd/yyyy", provider);
                            this.Search._CurrentParameter.DateTimeString = dt1.ToString("MM/dd/yyyy");
                            foreach (AXSSection section in this.Sections)
                            {
                                DateTime dt = Convert.ToDateTime(section.EventDates.ToString(), provider);
                                string Eventdates = dt.ToString("MM/dd/yyyy");
                                if (this.Search._CurrentParameter.DateTimeString.Trim() == Eventdates.Trim())
                                {
                                    if (!String.IsNullOrEmpty(this.Search._CurrentParameter.EventTime) && this.Search._CurrentParameter.EventTime != "hh:mm")
                                    {
                                        DateTime time = Convert.ToDateTime(section.EventDates.Trim());

                                        if (this.Search._CurrentParameter.EventTime.Trim() == time.ToShortTimeString())
                                        {
                                            SectionCode = section.EventCode;

                                            if (this.Search.Ticket.URL.Contains("wroom.centrebell.ca"))
                                            {
                                                this.XML.LoadHtml(Search.post(this.Search, this.Search.XmlUrl + "?methodName=showshop.priceTableW&wroom=" + this.Search.wRoom + "&lang=" + this.Search.Ticket.lang, String.Format("<methodCall><methodName>showshop.priceTableW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>{1}</string></value></param></params></methodCall>", this.Search.wRoom, this.Search.Ticket.lang)));

                                            }
                                            else if (this.Search.Ticket.URL.Contains("/shop/") || this.Search.Ticket.URL.Contains("/#/"))
                                            {
                                                jsonStr = get(this.Search, this.Search.XmlUrl + "showshop.priceTableW/" + this.Search.wRoom + "/en?resp=json");
                                                this.XML.LoadHtml(jsonStr);
                                            }
                                            else
                                            {
                                                this.XML.LoadHtml(Search.post(this.Search, this.Search.XmlUrl + "?methodName=showshop.priceTableW&wroom=" + this.Search.wRoom + "&lang=en", String.Format("<methodCall><methodName>showshop.priceTableW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param></params></methodCall>", this.Search.wRoom)));
                                            }
                                            this.XML.LoadHtml(this.XML.DocumentNode.InnerHtml.Replace("<param>", ""));
                                            PriceLevels = new List<AXSPriceLevel>();
                                            string priceLevelQuery = "//name[text() = '" + section.EventTypeCode + "']";
                                            HtmlNode priceLevels = this.XML.DocumentNode.SelectSingleNode(priceLevelQuery);

                                            this.XML = new HtmlAgilityPack.HtmlDocument();
                                            this.XML.LoadHtml(priceLevels.NextSibling.NextSibling.OuterHtml);
                                            HtmlNodeCollection allPriceLevels = this.XML.DocumentNode.SelectNodes("/value/array/data/value[2]/array/data/value/array/data");
                                            // HtmlDocument hdoc = new HtmlDocument();
                                            // hdoc.LoadHtml(priceLevels.NextSibling.NextSibling.OuterHtml);
                                            HtmlNodeCollection allPrices = this.XML.DocumentNode.SelectNodes("/value/array/data/value[4]/array/data/value/array/data");
                                            string mos = allPriceLevels[0].SelectNodes("value")[0].InnerHtml;
                                            Dictionary<string, string> pLevels = new Dictionary<string, string>();

                                            for (int k = 0; k < allPriceLevels.Count; k++)
                                            {
                                                try
                                                {
                                                    string priceLevelNumber = allPriceLevels[k].SelectNodes("value/string")[0].InnerHtml;
                                                    string priceLevelName = allPriceLevels[k].SelectNodes("value/string")[1].InnerHtml;

                                                    try
                                                    {
                                                        string priceTotal = allPrices[k].SelectNodes("value/int")[2].InnerHtml;
                                                        priceTotal = priceTotal.Insert((priceTotal.Length - 2), ".");
                                                        price = Convert.ToDecimal(priceTotal);
                                                    }
                                                    catch { }
                                                    AXSPriceLevel priceLevel = new AXSPriceLevel(priceLevelName, priceLevelNumber, price);
                                                    this.PriceLevels.Add(priceLevel);
                                                }
                                                catch { }

                                            }
                                            section.PriceLevels = this.PriceLevels;
                                        }

                                    }
                                    else
                                    {
                                        if (this.Search.Ticket.URL.Contains("wroom.centrebell.ca"))
                                        {
                                            this.XML.LoadHtml(Search.post(this.Search, this.Search.XmlUrl + "?methodName=showshop.priceTableW&wroom=" + this.Search.wRoom + "&lang=" + this.Search.Ticket.lang, String.Format("<methodCall><methodName>showshop.priceTableW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>{1}</string></value></param></params></methodCall>", this.Search.wRoom, this.Search.Ticket.lang)));
                                        }
                                        else
                                        {
                                            this.XML.LoadHtml(Search.post(this.Search, this.Search.XmlUrl + "?methodName=showshop.priceTableW&wroom=" + this.Search.wRoom + "&lang=en", String.Format("<methodCall><methodName>showshop.priceTableW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param></params></methodCall>", this.Search.wRoom)));
                                        }
                                        this.XML.LoadHtml(this.XML.DocumentNode.InnerHtml.Replace("<param>", ""));
                                        PriceLevels = new List<AXSPriceLevel>();
                                        string priceLevelQuery = "//name[text() = '" + section.EventTypeCode + "']";
                                        HtmlNode priceLevels = this.XML.DocumentNode.SelectSingleNode(priceLevelQuery);

                                        this.XML = new HtmlAgilityPack.HtmlDocument();
                                        this.XML.LoadHtml(priceLevels.NextSibling.NextSibling.OuterHtml);
                                        HtmlNodeCollection allPriceLevels = this.XML.DocumentNode.SelectNodes("/value/array/data/value[2]/array/data/value/array/data");
                                        // HtmlDocument hdoc = new HtmlDocument();
                                        // hdoc.LoadHtml(priceLevels.NextSibling.NextSibling.OuterHtml);
                                        HtmlNodeCollection allPrices = this.XML.DocumentNode.SelectNodes("/value/array/data/value[4]/array/data/value/array/data");
                                        string mos = allPriceLevels[0].SelectNodes("value")[0].InnerHtml;
                                        Dictionary<string, string> pLevels = new Dictionary<string, string>();
                                        for (int k = 0; k < allPriceLevels.Count; k++)
                                        {
                                            try
                                            {
                                                string priceLevelNumber = allPriceLevels[k].SelectNodes("value/string")[0].InnerHtml;
                                                string priceLevelName = allPriceLevels[k].SelectNodes("value/string")[1].InnerHtml;

                                                try
                                                {
                                                    string priceTotal = allPrices[k].SelectNodes("value/int")[2].InnerHtml;
                                                    priceTotal = priceTotal.Insert((priceTotal.Length - 2), ".");
                                                    price = Convert.ToDecimal(priceTotal);
                                                }
                                                catch { }
                                                AXSPriceLevel priceLevel = new AXSPriceLevel(priceLevelName, priceLevelNumber, price);
                                                this.PriceLevels.Add(priceLevel);
                                            }
                                            catch { }

                                        }
                                        section.PriceLevels = this.PriceLevels;
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (this.Search.Ticket.URL.Contains("wroom.centrebell.ca"))
                            {
                                this.XML.LoadHtml(Search.post(this.Search, this.Search.XmlUrl + "?methodName=showshop.priceTableW&wroom=" + this.Search.wRoom + "&lang=" + this.Search.Ticket.lang, String.Format("<methodCall><methodName>showshop.priceTableW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>{1}</string></value></param></params></methodCall>", this.Search.wRoom, this.Search.Ticket.lang)));
                            }
                            else
                            {
                                this.XML.LoadHtml(Search.post(this.Search, this.Search.XmlUrl + "?methodName=showshop.priceTableW&wroom=" + this.Search.wRoom + "&lang=en", String.Format("<methodCall><methodName>showshop.priceTableW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param></params></methodCall>", this.Search.wRoom)));
                            }
                            this.XML.LoadHtml(this.XML.DocumentNode.InnerHtml.Replace("<param>", ""));
                            PriceLevels = new List<AXSPriceLevel>();
                            string priceLevelQuery = "//name[text() = '" + this.Sections[0].EventTypeCode + "']";
                            HtmlNode priceLevels = this.XML.DocumentNode.SelectSingleNode(priceLevelQuery);

                            this.XML = new HtmlAgilityPack.HtmlDocument();
                            this.XML.LoadHtml(priceLevels.NextSibling.NextSibling.OuterHtml);
                            HtmlNodeCollection allPriceLevels = this.XML.DocumentNode.SelectNodes("/value/array/data/value[2]/array/data/value/array/data");

                            //if (allPriceLevels == null)
                            //{
                            //    this.XML.LoadHtml(Search.post(this.Search, this.Search.XmlUrl + "?methodName=showshop.priceTableW&wroom=" + this.Search.wRoom + "&lang=en", String.Format("<methodCall><methodName>showshop.priceTableW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param></params></methodCall>", this.Search.wRoom)));
                            //    this.XML.LoadHtml(this.XML.DocumentNode.InnerHtml.Replace("<param>", ""));
                            //    priceLevelQuery = "//name[text() = 'bcs2dayprespkg']";
                            //    priceLevels = this.XML.DocumentNode.SelectSingleNode(priceLevelQuery);

                            //    this.XML = new HtmlAgilityPack.HtmlDocument();
                            //    this.XML.LoadHtml(priceLevels.NextSibling.NextSibling.OuterHtml);
                            //    allPriceLevels = this.XML.DocumentNode.SelectNodes("/value/array/data/value[2]/array/data/value/array/data");
                            //}
                            HtmlNodeCollection allPrices = this.XML.DocumentNode.SelectNodes("/value/array/data/value[4]/array/data/value/array/data");
                            string mos = allPriceLevels[0].SelectNodes("value")[0].InnerHtml;
                            Dictionary<string, string> pLevels = new Dictionary<string, string>();
                            for (int k = 0; k < allPriceLevels.Count; k++)
                            {
                                try
                                {
                                    string priceLevelNumber = allPriceLevels[k].SelectNodes("value/string")[0].InnerHtml;
                                    string priceLevelName = allPriceLevels[k].SelectNodes("value/string")[1].InnerHtml;

                                    try
                                    {
                                        string priceTotal = allPrices[k].SelectNodes("value/int")[2].InnerHtml;
                                        priceTotal = priceTotal.Insert((priceTotal.Length - 2), ".");
                                        price = Convert.ToDecimal(priceTotal);
                                    }
                                    catch { }
                                    AXSPriceLevel priceLevel = new AXSPriceLevel(priceLevelName, priceLevelNumber, price);
                                    this.PriceLevels.Add(priceLevel);
                                }
                                catch { }

                            }
                            this.Sections[0].PriceLevels = this.PriceLevels;

                        }
                    }
                    else
                    {
                        if (this.Search.Ticket.URL.Contains("wroom.centrebell.ca"))
                        {
                            this.XML.LoadHtml(Search.post(this.Search, this.Search.XmlUrl + "?methodName=showshop.priceTableW&wroom=" + this.Search.wRoom + "&lang=" + this.Search.Ticket.lang, String.Format("<methodCall><methodName>showshop.priceTableW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>{1}</string></value></param></params></methodCall>", this.Search.wRoom, this.Search.Ticket.lang)));
                        }
                        if (this.Search.Ticket.URL.Contains("/shop/") || this.Search.Ticket.URL.Contains("/#/"))
                        {
                            jsonStr = get(this.Search, this.Search.XmlUrl + "showshop.priceTableW/" + this.Search.wRoom + "/en?resp=json");
                            this.XML.LoadHtml(jsonStr);
                        }
                        else
                        {
                            this.XML.LoadHtml(Search.post(this.Search, this.Search.XmlUrl + "?methodName=showshop.priceTableW&wroom=" + this.Search.wRoom + "&lang=en", String.Format("<methodCall><methodName>showshop.priceTableW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param></params></methodCall>", this.Search.wRoom)));
                        }

                        this.XML.LoadHtml(this.XML.DocumentNode.InnerHtml.Replace("<param>", ""));
                        PriceLevels = new List<AXSPriceLevel>();
                        string priceLevelQuery = "//name[text() = '" + this.Sections[0].EventTypeCode + "']";
                        HtmlNode priceLevels = this.XML.DocumentNode.SelectSingleNode(priceLevelQuery);

                        this.XML = new HtmlAgilityPack.HtmlDocument();
                        this.XML.LoadHtml(priceLevels.NextSibling.NextSibling.OuterHtml);
                        HtmlNodeCollection allPriceLevels = this.XML.DocumentNode.SelectNodes("/value/array/data/value[2]/array/data/value/array/data");

                        HtmlNodeCollection allPrices = this.XML.DocumentNode.SelectNodes("/value/array/data/value[4]/array/data/value/array/data");
                        string mos = allPriceLevels[0].SelectNodes("value")[0].InnerHtml;
                        Dictionary<string, string> pLevels = new Dictionary<string, string>();
                        for (int k = 0; k < allPriceLevels.Count; k++)
                        {
                            try
                            {
                                string priceLevelNumber = allPriceLevels[k].SelectNodes("value/string")[0].InnerHtml;
                                string priceLevelName = allPriceLevels[k].SelectNodes("value/string")[1].InnerHtml;

                                try
                                {
                                    string priceTotal = allPrices[k].SelectNodes("value/int")[2].InnerHtml;
                                    priceTotal = priceTotal.Insert((priceTotal.Length - 2), ".");
                                    price = Convert.ToDecimal(priceTotal);
                                }
                                catch { }
                                AXSPriceLevel priceLevel = new AXSPriceLevel(priceLevelName, priceLevelNumber, price);
                                this.PriceLevels.Add(priceLevel);
                            }
                            catch { }

                        }
                        this.Sections[0].PriceLevels = this.PriceLevels;
                    }

                }
                #endregion

                #endregion

                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }

        private Boolean parseJSON()
        {
            Boolean result = false;
            String jsonStr = String.Empty;
            int i = 0;

            try
            {
                #region json Parsing

                jsonStr = get(this.Search, this.Search.XmlUrl + "showshop.getOnsaleInfo/" + this.Search.wRoom + "/en?resp=json");
                JObject jsonObj = JObject.Parse(jsonStr);

                JProperty ResultP = jsonObj.Property("result");
                string str = ResultP.Value.ToString();

                JToken outer = JToken.Parse(str);

                List<string> eventTypeCodeList = new List<string>();
                List<string> eventCodeList = new List<string>();
                List<string> eventDatesList = new List<string>();

                foreach (JToken item in outer)
                {
                    if (item.Previous != null)
                    {
                        JObject inner = JObject.Parse(item.ToString());

                        JProperty typeCode = inner.Property("eventTypeCode");
                        eventTypeCodeList.Add(typeCode.Value.ToString());

                        JProperty eventcode = inner.Property("eventCode");
                        eventCodeList.Add(eventcode.Value.ToString());

                        JProperty date = inner.Property("date");
                        eventDatesList.Add(date.Value.ToString());

                    }
                }

                if (eventTypeCodeList.Count == eventCodeList.Count)
                {
                    if (eventTypeCodeList.Count == eventDatesList.Count)
                    {
                        for (i = 0; i < eventTypeCodeList.Count; i++)
                        {
                            string eventTypeCode = eventTypeCodeList[i];
                            string eventCode = eventCodeList[i];
                            string eventDate = eventDatesList[i];

                            AXSSection section = new AXSSection(eventTypeCode, eventCode, eventDate, null);
                            this.Sections.Add(section);

                            this.Search.Sections.Add(section);
                        }
                    }
                }

                if (this.Sections != null && this.Sections.Count > 1)
                {
                    if (this.Search._CurrentParameter.DateTimeString == "mm/dd/yyyy" || String.IsNullOrEmpty(this.Search._CurrentParameter.DateTimeString))
                    {
                        if (!String.IsNullOrEmpty(this.Search.Ticket.SelectedDate))
                        {
                            this.Search._CurrentParameter.DateTimeString = this.Search.Ticket.SelectedDate;
                            this.Search._CurrentParameter.EventTime = this.Search.Ticket.SelectedEventTime;
                        }
                        else
                        {
                            this.Search.getExtractedDatesForm(this.Search);
                        }
                    }
                }

                #region third req
                decimal price = 0;
                if (this.Search._CurrentParameter != null)
                {
                    if (!String.IsNullOrEmpty(this.Search._CurrentParameter.DateTimeString))
                    {
                        if (this.Search._CurrentParameter.DateTimeString != "mm/dd/yyyy")
                        {
                            CultureInfo provider = new CultureInfo("en-US");
                            DateTime dt1 = DateTime.ParseExact(this.Search._CurrentParameter.DateTimeString.ToString(), "MM/dd/yyyy", provider);
                            this.Search._CurrentParameter.DateTimeString = dt1.ToString("MM/dd/yyyy");

                            foreach (AXSSection section in this.Sections)
                            {
                                DateTime dt = Convert.ToDateTime(section.EventDates.ToString(), provider);
                                string Eventdates = dt.ToString("MM/dd/yyyy");
                                if (this.Search._CurrentParameter.DateTimeString.Trim() == Eventdates.Trim())
                                {
                                    if (!String.IsNullOrEmpty(this.Search._CurrentParameter.EventTime) && this.Search._CurrentParameter.EventTime != "hh:mm")
                                    {
                                        DateTime time = Convert.ToDateTime(section.EventDates.Trim());

                                        if (this.Search._CurrentParameter.EventTime.Trim() == time.ToShortTimeString())
                                        {
                                            SectionCode = section.EventCode;

                                            if (this.Search.Ticket.URL.Contains("/shop/") || this.Search.Ticket.URL.Contains("/#/"))
                                            {
                                                #region pricelevels
                                                jsonStr = get(this.Search, this.Search.XmlUrl + "showshop.priceTableW/" + this.Search.wRoom + "/en?resp=json");

                                                List<string> Totalprices = new List<string>();

                                                PriceLevels = new List<AXSPriceLevel>();

                                                JObject priceObj = JObject.Parse(jsonStr);

                                                JProperty priceProp = priceObj.Property("result");
                                                string priceStr = priceProp.Value.ToString();

                                                JToken outerPrice = JToken.Parse(priceStr);

                                                try
                                                {
                                                    IEnumerable<JToken> pricyProducts = outerPrice.SelectToken(section.EventTypeCode);
                                                    int count = 0;

                                                    foreach (JToken item in pricyProducts)
                                                    {
                                                        if (item.HasValues)
                                                        {
                                                            if (count.Equals(1)) //count.Equals(2)
                                                            {
                                                                #region pricelevel
                                                                JArray array = JArray.Parse(item.ToString());

                                                                foreach (JArray arr in array)
                                                                {
                                                                    string priceLevelNumber = arr[0].ToString();
                                                                    string priceLevelName = arr[1].ToString();//arr[2].ToString();

                                                                    AXSPriceLevel priceLevel = new AXSPriceLevel(priceLevelName, priceLevelNumber, price);
                                                                    this.PriceLevels.Add(priceLevel);
                                                                }
                                                                #endregion
                                                            }
                                                            else if (count.Equals(3))
                                                            {
                                                                JArray array = JArray.Parse(item.ToString());
                                                                foreach (JArray arr in array)
                                                                {
                                                                    string p = arr[4].ToString();

                                                                    p = p.Insert((p.Length - 2), ".");
                                                                    //priceTotal = Convert.ToDecimal(priceTotal);
                                                                    Totalprices.Add(p);
                                                                }
                                                            }
                                                        }

                                                        count++;
                                                    }

                                                    if (PriceLevels.Count == Totalprices.Count)
                                                    {
                                                        for (int l = 0; l < Totalprices.Count; l++)
                                                        {
                                                            PriceLevels[l].TotalPrice = Convert.ToDecimal(Totalprices[l]);
                                                        }
                                                    }

                                                    section.PriceLevels = this.PriceLevels;
                                                }
                                                catch
                                                {
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    else if (this.Search.Ticket.URL.Contains("/shop/") || this.Search.Ticket.URL.Contains("/#/"))
                                    {
                                        #region same as above  for price levels

                                        SectionCode = section.EventCode;

                                        jsonStr = get(this.Search, this.Search.XmlUrl + "showshop.priceTableW/" + this.Search.wRoom + "/en?resp=json");

                                        List<string> Totalprices = new List<string>();

                                        PriceLevels = new List<AXSPriceLevel>();

                                        JObject priceObj = JObject.Parse(jsonStr);

                                        JProperty priceProp = priceObj.Property("result");
                                        string priceStr = priceProp.Value.ToString();

                                        JToken outerPrice = JToken.Parse(priceStr);

                                        try
                                        {
                                            IEnumerable<JToken> pricyProducts = outerPrice.SelectToken(section.EventTypeCode);
                                            int count = 0;

                                            foreach (JToken item in pricyProducts)
                                            {
                                                if (item.HasValues)
                                                {
                                                    if (count.Equals(1))
                                                    {
                                                        #region pricelevel
                                                        JArray array = JArray.Parse(item.ToString());

                                                        foreach (JArray arr in array)
                                                        {
                                                            string priceLevelNumber = arr[0].ToString();
                                                            string priceLevelName = arr[1].ToString();

                                                            AXSPriceLevel priceLevel = new AXSPriceLevel(priceLevelName, priceLevelNumber, price);
                                                            this.PriceLevels.Add(priceLevel);
                                                        }
                                                        #endregion
                                                    }
                                                    else if (count.Equals(3))
                                                    {
                                                        JArray array = JArray.Parse(item.ToString());
                                                        foreach (JArray arr in array)
                                                        {
                                                            string p = arr[4].ToString();

                                                            p = p.Insert((p.Length - 2), ".");
                                                            //priceTotal = Convert.ToDecimal(priceTotal);
                                                            Totalprices.Add(p);
                                                        }
                                                    }
                                                }

                                                count++;
                                            }

                                            if (PriceLevels.Count == Totalprices.Count)
                                            {
                                                for (int l = 0; l < Totalprices.Count; l++)
                                                {
                                                    PriceLevels[l].TotalPrice = Convert.ToDecimal(Totalprices[l]);
                                                }
                                            }

                                            section.PriceLevels = this.PriceLevels;
                                        }
                                        catch
                                        {
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                        else
                        {
                            #region same as above  for price levels
                            if (this.Search.Ticket.URL.Contains("/shop/") || this.Search.Ticket.URL.Contains("/#/"))
                            {
                                jsonStr = get(this.Search, this.Search.XmlUrl + "showshop.priceTableW/" + this.Search.wRoom + "/en?resp=json");

                                SectionCode = this.Search.Sections[0].EventCode;

                                List<string> Totalprices = new List<string>();

                                PriceLevels = new List<AXSPriceLevel>();

                                JObject priceObj = JObject.Parse(jsonStr);

                                JProperty priceProp = priceObj.Property("result");
                                string priceStr = priceProp.Value.ToString();

                                JToken outerPrice = JToken.Parse(priceStr);

                                try
                                {
                                    IEnumerable<JToken> pricyProducts = outerPrice.SelectToken(Sections[0].EventTypeCode);
                                    int count = 0;

                                    foreach (JToken item in pricyProducts)
                                    {
                                        if (item.HasValues)
                                        {
                                            if (count.Equals(1)) //(count.Equals(2))
                                            {
                                                #region pricelevel
                                                JArray array = JArray.Parse(item.ToString());

                                                foreach (JArray arr in array)
                                                {
                                                    string priceLevelNumber = arr[0].ToString();
                                                    string priceLevelName = arr[1].ToString(); //arr[2].ToString();

                                                    AXSPriceLevel priceLevel = new AXSPriceLevel(priceLevelName, priceLevelNumber, price);
                                                    this.PriceLevels.Add(priceLevel);
                                                }
                                                #endregion
                                            }
                                            else if (count.Equals(3))
                                            {
                                                JArray array = JArray.Parse(item.ToString());
                                                foreach (JArray arr in array)
                                                {
                                                    string p = arr[4].ToString();

                                                    p = p.Insert((p.Length - 2), ".");
                                                    //priceTotal = Convert.ToDecimal(priceTotal);
                                                    Totalprices.Add(p);
                                                }
                                            }
                                        }

                                        count++;
                                    }

                                    if (PriceLevels.Count == Totalprices.Count)
                                    {
                                        for (int l = 0; l < Totalprices.Count; l++)
                                        {
                                            PriceLevels[l].TotalPrice = Convert.ToDecimal(Totalprices[l]);
                                        }
                                    }

                                    this.Sections[0].PriceLevels = this.PriceLevels;
                                }
                                catch
                                {
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region same as above  for price levels
                        if (this.Search.Ticket.URL.Contains("/shop/") || this.Search.Ticket.URL.Contains("/#/"))
                        {
                            jsonStr = get(this.Search, this.Search.XmlUrl + "showshop.priceTableW/" + this.Search.wRoom + "/en?resp=json");

                            SectionCode = this.Search.Sections[0].EventCode;
                            List<string> Totalprices = new List<string>();

                            PriceLevels = new List<AXSPriceLevel>();

                            JObject priceObj = JObject.Parse(jsonStr);

                            JProperty priceProp = priceObj.Property("result");
                            string priceStr = priceProp.Value.ToString();

                            JToken outerPrice = JToken.Parse(priceStr);

                            try
                            {
                                IEnumerable<JToken> pricyProducts = outerPrice.SelectToken(Sections[0].EventTypeCode);
                                int count = 0;

                                foreach (JToken item in pricyProducts)
                                {
                                    if (item.HasValues)
                                    {
                                        if (count.Equals(1))
                                        {
                                            #region pricelevel
                                            JArray array = JArray.Parse(item.ToString());

                                            foreach (JArray arr in array)
                                            {
                                                string priceLevelNumber = arr[0].ToString();
                                                string priceLevelName = arr[1].ToString();

                                                AXSPriceLevel priceLevel = new AXSPriceLevel(priceLevelName, priceLevelNumber, price);
                                                this.PriceLevels.Add(priceLevel);
                                            }
                                            #endregion
                                        }
                                        else if (count.Equals(3))
                                        {
                                            JArray array = JArray.Parse(item.ToString());
                                            foreach (JArray arr in array)
                                            {
                                                string p = arr[4].ToString();

                                                p = p.Insert((p.Length - 2), ".");
                                                //priceTotal = Convert.ToDecimal(priceTotal);
                                                Totalprices.Add(p);
                                            }
                                        }
                                    }

                                    count++;
                                }

                                if (PriceLevels.Count == Totalprices.Count)
                                {
                                    for (int l = 0; l < Totalprices.Count; l++)
                                    {
                                        PriceLevels[l].TotalPrice = Convert.ToDecimal(Totalprices[l]);
                                    }
                                }

                                this.Sections[0].PriceLevels = this.PriceLevels;
                            }
                            catch
                            {
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                #endregion

                result = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }

        private Boolean parseTix()
        {
            Boolean result = false;
            String jsonStr = String.Empty;
            int i = 0;

            try
            {
                #region json Parsing
                if (this.Search.Ticket.IsUkEvent) // if (this.Search.Ticket.URL.Contains("shop.axs.co.uk/") || this.Search.Ticket.URL.Contains("q.axs.co.uk/"))
                {
                    jsonStr = this.Search.Session.Get("https://unifiedapicommerce.axs.co.uk/veritix/onsale/v2/" + this.Search.OnSaleUrl + "?sessionID=" + this.Search.SessionKey + "&locale=en-GB"); ;
                }
                else
                {
                    jsonStr = this.Search.Session.Get("https://unifiedapicommerce.us-prod0.axs.com/veritix/onsale/v2/" + this.Search.OnSaleUrl + "?sessionID=" + this.Search.SessionKey + "&locale=en-US"); ;
                }
                jsonStr = Regex.Replace(jsonStr, "<.*?>", String.Empty);

                JObject jsonObj = JObject.Parse(jsonStr);

                if (jsonObj.Property("onsaleInformation") != null)
                {
                    JObject onsaleInfo = (JObject)jsonObj["onsaleInformation"];

                    if (onsaleInfo.Property("is3dsecureEnabled") != null)
                    {
                        string dSecure = onsaleInfo["is3dsecureEnabled"].ToString();

                        try
                        {
                            is3dsecureEnabled = Boolean.Parse(dSecure);
                        }
                        catch { }
                    }

                    if (onsaleInfo.Property("onsaleID") != null)
                    {
                        this.Search.OnSaleUrl = System.Web.HttpUtility.UrlEncode(Convert.ToString(onsaleInfo["onsaleID"]));
                    }

                    if (onsaleInfo.Property("meta") != null)
                    {
                        if (((JObject)onsaleInfo["meta"]).Property("offerID") != null)
                        {
                            this.Search.OfferID = onsaleInfo["meta"]["offerID"].ToString();
                        }
                        if (((JObject)onsaleInfo["meta"]).Property("contextID") != null)
                        {
                            this.Search.contextID = onsaleInfo["meta"]["contextID"].ToString();
                        }
                    }

                    if (onsaleInfo.Property("events") != null)
                    {
                        JArray events = (JArray)onsaleInfo["events"];

                        foreach (JObject item in events)
                        {
                            if (item.Property("eventID") != null)
                            {
                                string eventTypeCode = String.Empty;
                                string eventCode = Convert.ToString(item["eventID"]);
                                string eventDate = Convert.ToString(item["date"]);


                                AXSSection section = new AXSSection(eventTypeCode, eventCode, eventDate, null);

                                if (item.Property("ticketSearchCriteria") != null)
                                {
                                    try
                                    {
                                        JArray ticketsearchCriteria = (JArray)item["ticketSearchCriteria"];

                                        if (((JObject)(ticketsearchCriteria[0])).Property("quantity") != null)
                                        {
                                            if (((JObject)ticketsearchCriteria[0]["quantity"]).Property("min") != null)
                                            {
                                                this.NoOfTicket = Convert.ToInt32(ticketsearchCriteria[0]["quantity"]["min"].ToString());
                                            }
                                            if (((JObject)ticketsearchCriteria[0]["quantity"]).Property("max") != null)
                                            {
                                                this.MaxNoOfTicket = Convert.ToInt32(ticketsearchCriteria[0]["quantity"]["max"].ToString());
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                this.Sections.Add(section);

                                this.Search.Sections.Add(section);

                                if (this.Sections != null && this.Sections.Count > 1)
                                {
                                    if (this.Search._CurrentParameter.DateTimeString == "mm/dd/yyyy" || String.IsNullOrEmpty(this.Search._CurrentParameter.DateTimeString))
                                    {
                                        if (!String.IsNullOrEmpty(this.Search.Ticket.SelectedDate))
                                        {
                                            this.Search._CurrentParameter.DateTimeString = this.Search.Ticket.SelectedDate;
                                            this.Search._CurrentParameter.EventTime = this.Search.Ticket.SelectedEventTime;
                                        }
                                        else
                                        {
                                            //this.Search.getExtractedDatesForm(this.Search);
                                        }
                                    }
                                }
                                else
                                {
                                    this.Search.EventID = this.Search.Sections[i].EventCode;
                                }
                            }
                        }
                    }
                }

                if (jsonObj.Property("configuration") != null)
                {
                    JObject configuration = (JObject)jsonObj["configuration"];

                    if (configuration.Property("APIs") != null)
                    {
                        JObject APIs = (JObject)configuration["APIs"];

                        if (APIs.Property("axs") != null)
                        {
                            JObject axs = (JObject)APIs["axs"];

                            if (axs.Property("login") != null)
                            {
                                JObject login = (JObject)axs["login"];

                                if (login.Property("production") != null)
                                {
                                    JObject production = (JObject)login["production"];

                                    if (production.Property("clientID") != null)
                                    {
                                        this.Search.Client_ID = Convert.ToString(production["clientID"]);
                                    }

                                    if (production.Property("clientSecret") != null)
                                    {
                                        this.Search.Client_Secret = Convert.ToString(production["clientSecret"]);
                                    }
                                }

                                if (login.Property("skipAsGuest") != null)
                                {
                                    JObject skipAsGuest = (JObject)login["skipAsGuest"];

                                    if (skipAsGuest.Property("enabled") != null)
                                    {
                                        this.Search.SkipAsGuest = Convert.ToBoolean(skipAsGuest["enabled"]);
                                    }
                                }
                            }
                        }
                    }
                }

                if (this.Search.Ticket.IsUkEvent) //if (this.Search.Ticket.URL.Contains("shop.axs.co.uk/") || this.Search.Ticket.URL.Contains("q.axs.co.uk/"))
                {
                    jsonStr = this.Search.Session.Get("https://unifiedapicommerce.axs.co.uk/veritix/inventory/V2/" + this.Search.OnSaleUrl + "/price?sessionID=" + this.Search.SessionKey + "&eventID=" + this.Search.EventID + "&locale=en-GB&getSections=true&grouped=true&includeSoldOuts=false");
                }
                else
                {
                    jsonStr = this.Search.Session.Get("https://unifiedapicommerce.us-prod0.axs.com/veritix/inventory/V2/" + this.Search.OnSaleUrl + "/price?sessionID=" + this.Search.SessionKey + "&eventID=" + this.Search.EventID + "&locale=en-US&getSections=true&grouped=true&includeSoldOuts=false");

                    if (jsonStr.ToLower().Equals("unauthorized") || jsonStr.Equals("Session Expired"))
                    {
                        jsonStr = this.Search.Session.Get("https://unifiedapicommerce.us-prod0.axs.com/veritix/inventory/V2/" + this.Search.OnSaleUrl + "/price?locale=en-US&getSections=true&grouped=true&includeSoldOuts=false&includeDynamicPrice=true");
                    }
                }

                if (jsonStr.ToLower().Equals("unauthorized") || jsonStr.Equals("Session Expired"))
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(this.Search.Ticket.OldURL))
                        {
                            this.Search.MoreInfo = jsonObj.ToString();

                            if (!String.IsNullOrEmpty(this.Search.Ticket.constTicketURL))
                            {
                                this.Search.Ticket.URL = this.Search.Ticket.constTicketURL;
                            }
                        }
                    }
                    catch (Exception ex)
                    { }

                    return false;
                }

                jsonStr = Regex.Replace(jsonStr, "<.*?>", String.Empty);

                JObject obj = JObject.Parse(jsonStr);

                this.TixPriceLevels = new List<AXSTixPriceLevels>();
                this.TixTicketTypes = new List<AXSTixPriceTypes>();
                this.ResaleTickets = new List<AXSTixResale>();

                if (obj.Property("offerPrices") != null)
                {

                    try
                    {
                        offerIds = new Dictionary<string, string>();
                        JArray jOfferPrices = JArray.Parse(obj["offerPrices"].ToString());
                        foreach (JToken item in jOfferPrices)
                        {
                            try
                            {
                                String id = item["offerID"].ToString();
                                String groupID = item["offerGroupID"].ToString();

                                offerIds.Add(id, groupID);
                            }
                            catch { }
                        }
                    }
                    catch { }


                    string sectionResponse = string.Empty;
                    JObject allSections = null;

                    try
                    {

                        if (this.Search.Ticket.IsUkEvent) //if (this.Search.Ticket.URL.Contains("shop.axs.co.uk/") || this.Search.Ticket.URL.Contains("q.axs.co.uk/"))
                        {
                            sectionResponse = this.Search.Session.Get("https://unifiedapicommerce.axs.co.uk/veritix/inventory/V2/" + this.Search.OnSaleUrl + "/sections");

                            allSections = JObject.Parse(sectionResponse);
                        }
                        else
                        {
                            sectionResponse = this.Search.Session.Get("https://unifiedapicommerce.us-prod0.axs.com/veritix/inventory/V2/" + this.Search.OnSaleUrl + "/sections");

                            allSections = JObject.Parse(sectionResponse);
                        }
                    }
                    catch (Exception ex)
                    {
                    }


                    int index = 0;
                    while (true)
                    {
                        try
                        {
                            JArray eventPrices = (JArray)obj["offerPrices"][index]["zonePrices"];
                            index++;

                            foreach (JObject item in eventPrices)
                            {
                                if (Convert.ToString(item["eventID"]).Equals(this.Search.EventID))
                                {
                                    if (item.Property("priceLevels") != null)
                                    {
                                        JArray priceLevels = (JArray)item["priceLevels"];

                                        if (priceLevels.HasValues)
                                        {
                                            #region Tickets
                                            foreach (JObject pricelevel in priceLevels)
                                            {
                                                String id = Convert.ToString(pricelevel["priceLevelID"]);
                                                String name = Convert.ToString(pricelevel["label"]);
                                                AXSTixPriceLevels price = new AXSTixPriceLevels(id, name);

                                                if (pricelevel.Property("availability") != null)
                                                {
                                                    JObject availability = (JObject)pricelevel["availability"];

                                                    if (availability.Property("sections") != null)
                                                    {
                                                        JArray sections = (JArray)availability["sections"];

                                                        if (sections.Count > 0)
                                                        {
                                                            foreach (JObject section in sections)
                                                            {
                                                                String _id = Convert.ToString(section["id"]);
                                                                String _name = Convert.ToString(section["label"]);
                                                                int seat = Convert.ToInt32(section["amount"]);
                                                                AXSTixSection _section = new AXSTixSection(_id, _name, seat);
                                                                price.Sections.Add(_section);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            foreach (KeyValuePair<string,JToken> section in allSections)
                                                            {
                                                                try
                                                                {

                                                                    List<string> PriceTypeIDs = new List<string>();

                                                                    JArray plevels = (JArray)section.Value["priceLevels"];


                                                                    JArray prices = (JArray)section.Value["availability"]["prices"];

                                                                    foreach (var pri in prices)
                                                                    {
                                                                        JArray priceTypes = (JArray)pri["priceTypes"];

                                                                        foreach (JValue pricttype in priceTypes)
                                                                        {
                                                                            PriceTypeIDs.Add(pricttype.ToString());
                                                                        }
                                                                    }

                                                                    foreach (JValue plev in plevels)
                                                                    {
                                                                        try
                                                                        {
                                                                            if (plev.ToString().Equals(id))
                                                                            {
                                                                                String _id = Convert.ToString(section.Value["sectionID"]);
                                                                                String _name = Convert.ToString(section.Value["sectionLabel"]);
                                                                                int seat = Convert.ToInt32(section.Value["availability"]["total"]);
                                                                                AXSTixSection _section = new AXSTixSection(_id, _name, seat, PriceTypeIDs);
                                                                                price.Sections.Add(_section);
                                                                            }
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                                                        }
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                                                }

                                                            }

                                                        }
                                                    }
                                                }

                                                if (pricelevel.Property("prices") != null)
                                                {
                                                    JArray prices = (JArray)pricelevel["prices"];

                                                    foreach (JObject _price in prices)
                                                    {
                                                        try
                                                        {
                                                            AXSTixPrices _prices = new AXSTixPrices();
                                                            _prices.PriceID = Convert.ToString(_price["priceID"]);
                                                            _prices.Currency = Convert.ToString(_price["currency"]);

                                                            _prices.tax = Convert.ToInt32(_price["tax"]);
                                                            string strprice = _price["base"].ToString();

                                                            _prices.Price = Convert.ToDouble(strprice.Insert(strprice.Length - 2, "."));

                                                            _prices.PriceLevelID = _price["priceTypeID"].ToString();

                                                            if (_price.Property("fees") != null)
                                                            {
                                                                _prices.Fees = new List<int>();
                                                                foreach (JObject fee in _price["fees"])
                                                                {
                                                                    _prices.Fees.Add(Convert.ToInt32(fee["value"]));
                                                                }
                                                            }

                                                            price.Prices.Add(_prices);
                                                        }
                                                        catch
                                                        { }
                                                    }
                                                }

                                                this.TixPriceLevels.Add(price);

                                                result = true;
                                            }
                                            #endregion
                                        }
                                    }

                                    if (item.Property("priceTypes") != null)
                                    {
                                        JArray types = (JArray)item["priceTypes"];

                                        foreach (JObject type in types)
                                        {
                                            AXSTixPriceTypes priceType = new AXSTixPriceTypes();
                                            priceType.ID = Convert.ToString(type["priceTypeID"]);
                                            priceType.Label = Convert.ToString(type["label"]);

                                            this.TixTicketTypes.Add(priceType);

                                            result = true;

                                        }
                                    }


                                    try
                                    {
                                        if (item.Property("resalePrices") != null)
                                        {
                                            String arr = item["resalePrices"].ToString();
                                            JObject oj = JObject.Parse(arr);

                                            // JArray resaleSections = (JArray)item["resalePrices"];

                                                foreach (var rs in oj)
                                            {
                                                try
                                                {
                                                    String seclbl = rs.Key;

                                                    foreach (var kv in rs.Value)
                                                    {
                                                        try
                                                        {
                                                            String[] Splitted = kv.ToString().Replace("\"", "").Split(':');

                                                            AXSTixResale tixResale = new AXSTixResale(seclbl, Splitted[0], Splitted[1]);

                                                            this.ResaleTickets.Add(tixResale);

                                                            result = true;


                                                        }
                                                        catch { }
                                                    }
                                                }
                                                catch { }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine(ex.Message);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }

        public static string get(AXSSearch search, string URL)
        {
            string result = string.Empty;

            try
            {

                if (URL.Contains("httpss"))
                {
                    URL = URL.Replace("httpss", "https");
                }

                HttpWebRequest webRequest = System.Net.HttpWebRequest.Create(URL) as System.Net.HttpWebRequest;
                webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0";
                webRequest.KeepAlive = true;
                if (new Uri(search.Ticket.URL).Host.Contains("shop.axs.co.uk"))
                {
                    webRequest.Headers.Add("Origin", "https://shop.axs.co.uk");
                    //webRequest.Referer = "https://" + new Uri(search.Ticket.URL).Host + "/";
                }
                else if (new Uri(search.Ticket.URL).Host.Contains("tickets.evenko"))
                {
                    webRequest.Headers.Add("Origin", "https://tickets.evenko.ca");
                }
                else if (!new Uri(search.Ticket.URL).Host.Contains("tix.axs.com"))
                {
                    webRequest.Headers.Add("Origin", "https://tickets.axs.com");
                }
                else
                {
                    webRequest.Headers.Add("Origin", "https://tix.axs.com");
                }

                webRequest.Accept = "*/*";

                if (URL.Contains("json"))
                {
                    webRequest.ContentType = "application/json";
                    webRequest.Headers.Add("Accept-Encoding", "gzip, deflate, sdch");
                    webRequest.Referer = "https://" + new Uri(search.Ticket.URL).Host + "/shop/";
                    webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                }

                if (new Uri(search.Ticket.URL).Host.Contains("tix.axs.com"))
                {
                    webRequest.ContentType = "application/json";
                    webRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                    webRequest.Referer = "https://" + new Uri(search.Ticket.URL).Host + "/";
                    webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                }

                if (!String.IsNullOrEmpty(search.EncryptedCredentials))
                {
                    Uri uri = webRequest.RequestUri;
                    Cookie cookie = new Cookie("axsid_user", search.EncryptedCredentials);
                    webRequest.CookieContainer = new CookieContainer();
                    webRequest.CookieContainer.Add(new Uri(uri.Scheme + "://" + uri.Host + "/"), cookie);
                }


                if (ServicePointManager.MaxServicePoints < 1000)
                {
                    ServicePointManager.MaxServicePoints = 1000;
                }

                if (ServicePointManager.DefaultConnectionLimit < 1000)
                {
                    ServicePointManager.DefaultConnectionLimit = 1000;
                }

                if (search.Proxy != null)
                {
                    if (search.Proxy.TheProxyType == Proxy.ProxyType.Luminati)
                    {
                        if (!String.IsNullOrEmpty(search.Proxy.LuminatiSessionId))
                        {
                            webRequest.ConnectionGroupName = search.Proxy.LuminatiSessionId;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(search.AuthorizarionHeader))
                {
                    webRequest.Headers.Set(HttpRequestHeader.Authorization, search.AuthorizarionHeader);
                }

                if (search.Proxy != null)
                {
                    if (search.Proxy.TheProxyType != Proxy.ProxyType.Custom)
                    {
                        webRequest.Timeout = 10000;
                    }

                    if (search.Proxy.TheProxyType == Proxy.ProxyType.Relay)
                    {
                        webRequest.Proxy = search.Proxy.toWebProxy(search.context);
                    }
                    else
                    {
                        webRequest.Proxy = search.Proxy.toWebProxy();
                    }
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                try
                {
                    HttpWebResponse resp = null;
                    Encoding respenc = null;
                    var isGZipEncoding = false;

                    //string postData = postdata.Replace("&lt;", "<").Replace("&gt;", ">");
                    //byte[] postArray = Encoding.ASCII.GetBytes(postData);
                    //webRequest.ContentLength = postArray.Length;
                    System.IO.Stream reqStream = null;//webRequest.GetRequestStream();
                    // reqStream.Write(postArray, 0, postArray.Length);

                    try
                    {
                        resp = webRequest.GetResponse() as HttpWebResponse;
                    }
                    catch (WebException we)
                    {
                        resp = (HttpWebResponse)we.Response;
                        ProxyPicker.ProxyPickerInstance.RecheckProxyStatus(search.Proxy, we.Message);
                    }

                    if (resp != null)
                    {
                        if (!string.IsNullOrEmpty(resp.ContentEncoding))
                        {
                            isGZipEncoding = resp.ContentEncoding.ToLower().StartsWith("gzip") ? true : false;
                            if (!isGZipEncoding)
                            {
                                respenc = Encoding.GetEncoding(resp.ContentEncoding);
                            }
                        }

                        if (isGZipEncoding)
                        {
                            reqStream = new GZipStream(resp.GetResponseStream(), CompressionMode.Decompress);
                        }
                        else
                        {
                            reqStream = resp.GetResponseStream();
                        }

                        StreamReader sr = new StreamReader(reqStream);
                        result = sr.ReadToEnd();
                    }
                    reqStream.Close();

                    return result;
                }
                catch (Exception ex)
                {
                    //Logger.LoggerInstance.Add(new Log(ErrorType.EXCEPTION, this.Ticket.TicketID, this.Ticket.URL, ex.Message + ex.StackTrace));
                    return result;
                }
            }
            catch
            {
                return result;
            }
        }

        private bool IsGZipEncoding(string contentEncoding)
        {
            return contentEncoding.ToLower().StartsWith("gzip");
        }

        //public static String post(AXSSearch search, string url, string postdata)
        //{
        //    System.Net.ServicePointManager.Expect100Continue = false;
        //    string URL = url;
        //    System.Net.WebRequest webRequest = System.Net.WebRequest.Create(URL) as System.Net.HttpWebRequest;
        //    webRequest.Method = "POST";
        //    webRequest.ContentType = "application/x-www-form-urlencoded";
        //    webRequest.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
        //    webRequest.Timeout = 5000;//60000 * 2;
        //    if (search.Proxy != null)
        //    {
        //        if (!String.IsNullOrEmpty(search.Proxy.Address.Trim()) && !String.IsNullOrEmpty(search.Proxy.Port.Trim()))
        //        {
        //            webRequest.Proxy = new System.Net.WebProxy(search.Proxy.Address.Trim(), int.Parse(search.Proxy.Port.Trim()));
        //        }
        //        if (!String.IsNullOrEmpty(search.Proxy.UserName.Trim()) && !String.IsNullOrEmpty(search.Proxy.Password.Trim()))
        //        {
        //            webRequest.Proxy.Credentials = new System.Net.NetworkCredential(search.Proxy.UserName.Trim(), search.Proxy.Password.Trim());
        //        }
        //    }
        //    ////

        //    HttpWebResponse resp =null;
        //    Encoding respenc = null;
        //    var isGZipEncoding = false;

        //    string postData = postdata;
        //    byte[] postArray = Encoding.ASCII.GetBytes(postData);

        //    webRequest.ContentLength = postArray.Length;
        //    System.IO.Stream reqStream = webRequest.GetRequestStream();
        //    reqStream.Write(postArray, 0, postArray.Length);

        //    try
        //    {
        //        resp = webRequest.GetResponse() as HttpWebResponse;
        //    }
        //    catch (WebException we)
        //    {                
        //        resp = (HttpWebResponse)we.Response;

        //    }
        //    catch (Exception)
        //    {

        //    }            

        //    if (!string.IsNullOrEmpty(resp.ContentEncoding))
        //    {
        //        isGZipEncoding = resp.ContentEncoding.ToLower().StartsWith("gzip") ? true : false;
        //        if (!isGZipEncoding)
        //        {
        //            respenc = Encoding.GetEncoding(resp.ContentEncoding);
        //        }
        //    }
        //    ////
        //    //Stream reqStream = webRequest.GetRequestStream();


        //    if (isGZipEncoding)
        //    {
        //        reqStream = new GZipStream(resp.GetResponseStream(), CompressionMode.Decompress);
        //    }
        //    else
        //    {
        //        reqStream = resp.GetResponseStream();
        //    }


        //    //reqStream.Write(postArray, 0, postArray.Length);

        //    //StreamReader sr = new StreamReader(new GZipStream((webRequest.GetResponse().GetResponseStream()), CompressionMode.Compress));
        //    StreamReader sr = new StreamReader(reqStream);
        //    string Result = sr.ReadToEnd();
        //    reqStream.Close();
        //    return Result;
        //}

    }
}

