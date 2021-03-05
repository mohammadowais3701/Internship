using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Automatick.Core
{
    [Serializable]
    public class VSEvent
    {
        private BrowserSession session = null;
        int minQty = 0, maxQty = 0, qtyStep = 1;
        Dictionary<String, String> formElement = null;

        public VSTicket Ticket
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
        public String EventLocation
        {
            get;
            set;
        }
        public String EventDateTime
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
                    if (this.HasTicketTypes)
                    {
                        result = false;
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
        public HtmlDocument HTML
        {
            get;
            set;
        }
        public String V_EDP
        {
            get;
            set;
        }
        public String URLToPost
        {
            get;
            set;
        }
        public String V_For_SecLocAdd
        {
            get;
            set;
        }
        public List<VSTicketType> TicketTypes
        {
            get;
            set;
        }
        public Boolean HasTicketTypes
        {
            get
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
        }

        public string EventTarget
        {
            get;
            set;
        }

        public List<VSSection> Sections
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
        public List<VSSection> Locations
        {
            get;
            set;
        }
        public Boolean HasLocations
        {
            get
            {
                if (Locations != null)
                {
                    if (Locations.Count > 0)
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
        public List<VSSection> Additionals
        {
            get;
            set;
        }
        public Boolean HasAdditionals
        {
            get
            {
                if (Additionals != null)
                {
                    if (Additionals.Count > 0)
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
        public int TicketLimitPerAccountForAutoBuy
        {
            get;
            set;
        }
        JObject JSONdata
        {
            get;
            set;
        }

        VSSearch _search = null;

        VSSearch Search
        {
            get
            {
                return this._search;
            }
        }

        public Boolean PriceLevelMatched
        {
            get;
            set;
        }
        public Boolean SectionMatched
        {
            get;
            set;
        }
        public Boolean TicketTypeMatched
        {
            get;
            set;
        }
        public Boolean IsBestAvailable
        {
            get;
            set;
        }
        public Boolean NoSeatsAvailable
        {
            get;
            set;
        }

        public VSEvent(VSSearch search, VSTicket ticket)
        {
            this._search = search;
            this.session = this.Search.Session;
            this.HTML = this.session.HtmlDocument;
            this.Ticket = ticket;
            //Sections = new List<VSSection>();
            //Locations = new List<VSSection>();
            //Additionals = new List<VSSection>();
            TicketTypes = new List<VSTicketType>();
            if (parseEventParams())
            {
                IfTicketAlive = true;
            }
            else
            {
                IfTicketAlive = false;
            }

        }
        private Boolean parseEventParams()
        {
            Boolean result = false;
            Regex.CacheSize = 0;
            String url = this.Ticket.URL;

            if (this.session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("home.aspx"))
            {
                String html = this.session.Get(this.session.HTMLWeb.ResponseUri.AbsoluteUri);
            }

            foreach (var item in this.session.FormElements)
            {
                try
                {
                    if (item.Key.Contains("ddlQuantity"))
                    {
                        result = ddlQuantityFormat();
                        break;
                    }
                    if (item.Key.Contains("ddlOfferQuantity"))
                    {
                        result = offerDdlQuantity();
                        break;
                    }
                    if (item.Key.Contains("txtQty"))
                    {
                        result = txtQuantity();
                        break;
                    }

                    if (item.Key.Contains("ucQuantityAndPrice$cboPriceCode"))
                    {
                        result = pareseTicketsAndPriceLevels();
                        break;
                    }

                    if (item.Key.Contains("ucQuantityAndPrice:cboPriceCode"))
                    {
                        result = pareseTicketsAndPriceLevels(true);
                        break;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + Environment.NewLine + ex.Message);
                }
            }

            return result;
        }

        public Boolean pareseTicketsAndPriceLevels()
        {
            Boolean result = false;

            try
            {
                HtmlNode node = this.HTML.DocumentNode.SelectSingleNode("//div[@id='pnlOptions']");

                if (node != null)
                {

                    // HTML.Load(node.InnerHtml);

                    string ticketTypeString = this.HTML.DocumentNode.SelectSingleNode("//td[@class='quantity-price-row-description']").InnerText.Trim();

                    minQty = Convert.ToInt32(this.HTML.DocumentNode.SelectSingleNode("//span[@id='m_c_ucPickASection_ucQuantityAndPrice_cvQtySum']").Attributes["min"].Value);

                    maxQty = Convert.ToInt32(this.HTML.DocumentNode.SelectSingleNode("//span[@id='m_c_ucPickASection_ucQuantityAndPrice_cvQtySum']").Attributes["max"].Value);

                    qtyStep = Convert.ToInt32(this.HTML.DocumentNode.SelectSingleNode("//span[@id='m_c_ucPickASection_ucQuantityAndPrice_cvQtySum']").Attributes["increment"].Value);

                    HtmlNode formElementKey = this.HTML.DocumentNode.SelectSingleNode("//select[contains(@name,'PickASection$ucQuantityAndPrice$cboPriceCode')]");


                    VSTicketType typeTicket = new VSTicketType(ticketTypeString, ticketTypeString, minQty, maxQty, qtyStep, formElementKey.Attributes["name"].Value.Trim());

                    this.TicketTypes.Add(typeTicket);

                    if (String.IsNullOrEmpty(this.Search._CurrentParameter.TicketTypeString))
                    {
                        if (String.IsNullOrEmpty(this.Search.Price) && String.IsNullOrEmpty(this.Search.Section))
                        {
                            this.IsBestAvailable = true;
                            TicketTypes[0].HasPriceLevel = parsePriceLevel(TicketTypes[0], true);
                            this.TicketTypeMatched = true;
                            result = true;
                        }
                    }
                    else
                    {
                        foreach (VSTicketType ticketType in TicketTypes)
                        {
                            if (this.Search._CurrentParameter.ExactMatch ? ticketType.Description.ToLower() == this.Search._CurrentParameter.TicketTypeString.ToLower() : ticketType.Description.ToLower().Contains(this.Search._CurrentParameter.TicketTypeString.ToLower()))
                            {
                                this.TicketTypeMatched = true;
                                ticketType.HasPriceLevel = this.parsePriceLevel(ticketType, true);
                                break;

                            }
                        }
                    }
                }
                else
                {
                    result = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return result;
        }

        public Boolean pareseTicketsAndPriceLevels(bool ischange = false)
        {
            Boolean result = false;

            try
            {
                HtmlNode node = this.HTML.DocumentNode.SelectSingleNode("//div[@id='pnlOptions']");

                if (node != null)
                {

                    // HTML.Load(node.InnerHtml);

                    string ticketTypeString = this.HTML.DocumentNode.SelectSingleNode("//td[@class='quantity-price-row-description']").InnerText.Trim();

                    try
                    {

                        HtmlNodeCollection quantititycollection = this.HTML.DocumentNode.SelectNodes("//select[contains(@name,'PickASection:ucQuantityAndPrice:cboPriceCode')]//option");

                        if (quantititycollection != null && quantititycollection.Count() > 1)
                        {
                            minQty = Convert.ToInt32(quantititycollection[0].Attributes["value"].Value);

                            qtyStep = Convert.ToInt32(quantititycollection[1].Attributes["value"].Value) - Convert.ToInt32(quantititycollection[0].Attributes["value"].Value);

                            maxQty = Convert.ToInt32(quantititycollection[quantititycollection.Count - 1].Attributes["value"].Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        minQty = 1;
                        maxQty = 8;
                        qtyStep = 1;
                    }

                    HtmlNode formElementKey = this.HTML.DocumentNode.SelectSingleNode("//select[contains(@name,'PickASection:ucQuantityAndPrice:cboPriceCode')]");


                    VSTicketType typeTicket = new VSTicketType(ticketTypeString, ticketTypeString, minQty, maxQty, qtyStep, formElementKey.Attributes["name"].Value.Trim());

                    this.TicketTypes.Add(typeTicket);

                    if (String.IsNullOrEmpty(this.Search._CurrentParameter.TicketTypeString))
                    {
                        this.IsBestAvailable = true;
                        TicketTypes[0].HasPriceLevel = parsePriceLevel(TicketTypes[0], true);
                        this.TicketTypeMatched = true;
                        result = true;
                    }
                    else
                    {
                        foreach (VSTicketType ticketType in TicketTypes)
                        {
                            if (this.Search._CurrentParameter.ExactMatch ? ticketType.Description.ToLower() == this.Search._CurrentParameter.TicketTypeString.ToLower() : ticketType.Description.ToLower().Contains(this.Search._CurrentParameter.TicketTypeString.ToLower()))
                            {
                                this.TicketTypeMatched = true;
                                ticketType.HasPriceLevel = this.parsePriceLevel(ticketType, true);
                                break;

                            }
                        }
                    }
                }
                else
                {
                    result = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return result;
        }

        public Boolean parsePriceLevel(VSTicketType ticketType)
        {
            VSTicketType type = ticketType;
            Boolean result = false;

            //TODO: if chk karo pricelevel aur section null hain to dosri post padege
            if (String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && String.IsNullOrEmpty(this.Search._CurrentParameter.Section) && String.IsNullOrEmpty(this.Search._CurrentParameter.Neighbourhood) && this.Search._CurrentParameter.PriceMin == null && this.Search._CurrentParameter.PriceMax == null)
            {
                //session.FormElements.Remove("m:c:g:_ctl2:qg:p:_ctl1:chkProduct");
                //session.FormElements.Remove("m:c:btnPriceRange");

                HtmlNode node = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'text-left ticket-checkbox')]//input");

                if (node != null)
                {
                    if (this.session.FormElements.ContainsKey(node.Attributes["name"].Value))
                    {

                        this.session.FormElements[node.Attributes["name"].Value] = "on";
                    }

                    this.EventTarget = System.Net.WebUtility.HtmlDecode(node.Attributes["onclick"].Value).Replace("javascript:setTimeout('__doPostBack(\\", "").Replace("&quot;", "").Replace("\\',\\'\\')', 0)", "");

                    if (this.session.FormElements.ContainsKey("__EVENTTARGET"))
                    {
                        this.session.FormElements["__EVENTTARGET"] = this.EventTarget;
                    }
                }

                if (this.Search._CurrentParameter.Quantity >= (type.MinQuantity) && this.Search._CurrentParameter.Quantity <= type.MaxQuantity)
                {
                    if (this.session.FormElements.ContainsKey(ticketType.QuantityFormElementKey))
                    {
                        this.session.FormElements[ticketType.QuantityFormElementKey] = this.Search._CurrentParameter.Quantity.ToString();
                    }
                }
                IsBestAvailable = true;
                return true;
            }
            else //if (this.Search._CurrentParameter.Quantity >= (type.MinQuantity) && this.Search._CurrentParameter.Quantity <= type.MaxQuantity)
            {
                session.FormElements.Remove("m:c:g:_ctl2:qg:p:_ctl1:chkProduct");
                session.FormElements.Remove("m:c:btnNBA");

                if (this.Search._CurrentParameter.Quantity >= (type.MinQuantity) && this.Search._CurrentParameter.Quantity <= type.MaxQuantity)
                {
                    if (this.session.FormElements.ContainsKey(ticketType.QuantityFormElementKey))
                    {
                        this.session.FormElements[ticketType.QuantityFormElementKey] = this.Search._CurrentParameter.Quantity.ToString();
                    }
                }
                else
                {
                    this.TicketTypeMatched = false;
                    return false;
                }
            }

            HtmlNode htmlNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'text-left ticket-checkbox')]//input");

            if (htmlNode != null)
            {
                if (this.session.FormElements.ContainsKey(htmlNode.Attributes["name"].Value))
                {

                    this.session.FormElements[htmlNode.Attributes["name"].Value] = "on";
                }

                this.EventTarget = System.Net.WebUtility.HtmlDecode(htmlNode.Attributes["onclick"].Value).Replace("javascript:setTimeout('__doPostBack(\\", "").Replace("&quot;", "").Replace("\\',\\'\\')', 0)", "");

                if (this.session.FormElements.ContainsKey("__EVENTTARGET"))
                {
                    this.session.FormElements["__EVENTTARGET"] = this.EventTarget;
                }
            }

            if (this.session.FormElements.ContainsKey("m$c$g$ctl02$qg$btnNBAFlex"))
            {
                this.session.FormElements.Remove("m$c$g$ctl02$qg$btnNBAFlex");
            }

            if (this.session.FormElements.ContainsKey("m$c$g$ctl01$qg$p$ctl01$btnNBA"))
            {
                this.session.FormElements.Remove("m$c$g$ctl01$qg$p$ctl01$btnNBA");
            }

            if (this.session.FormElements.ContainsKey("m$c$g$ctl02$qg$p$ctl01$chkProduct"))
            {
                this.session.FormElements.Remove("m$c$g$ctl02$qg$p$ctl01$chkProduct");
            }

            if (this.session.FormElements.ContainsKey("__EVENTTARGET"))
            {
                this.session.FormElements["__EVENTTARGET"] = this.EventTarget;
            }


            //HtmlNode documentNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//a[contains(@class,'btn-default-link link-blue flex-item-grow')]");



            String html = this.session.Post(this.session.HTMLWeb.ResponseUri.AbsoluteUri);

            //TODO: add neighborhood parameter working here

            HtmlNodeCollection _neighborhood = session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlNeighbourhood')]/option");

            type.Neighborhoods.Clear();

            if (_neighborhood != null)
            {
                foreach (HtmlNode neighbour in _neighborhood)
                {
                    try
                    {
                        String descNeighbour = neighbour.NextSibling.InnerHtml;
                        String[] nameNeighbour = descNeighbour.Split('-');

                        VSNeighborhood neighbourhood = new VSNeighborhood(neighbour.Attributes["value"].Value, nameNeighbour[0], neighbour.ParentNode.Attributes["name"].Value);

                        if (!neighbourhood.ID.Equals("-1") || !neighbourhood.Description.ToLower().Contains("best avail"))
                        {
                            AddUpdateField(this.session, neighbourhood.FormElementKey, neighbourhood.ID);
                            AddUpdateField(this.session, "__EVENTTARGET", neighbourhood.FormElementKey);
                            this.session.FormElements.Remove("m$c$g$ctl01$prg$p$ctl01$btnFindTickets");

                            this.session.FormElements.Remove("m:c:btnChangeQty");
                            this.session.FormElements.Remove("m:c:btnReserve");
                            this.session.Post(session.HTMLWeb.ResponseUri.AbsoluteUri);
                        }

                        HtmlNodeCollection _price = session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlPriceLevel')]/option");

                        if (_price != null)
                        {
                            foreach (HtmlNode tmp in _price)
                            {
                                try
                                {
                                    String desc = tmp.NextSibling.InnerHtml;
                                    String[] name = desc.Split('-');
                                    Decimal price = 0, minPrice = 0, maxPrice = 0;

                                    //String priceRange = session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td").InnerText;

                                    if (name.Length > 1)
                                    {
                                        price = Convert.ToDecimal(name[name.Length - 1].Split('$')[1]);
                                        maxPrice = price;
                                        minPrice = price;
                                    }
                                    else
                                    {
                                        HtmlNode minMaxNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td");
                                        String[] minMax = minMaxNode.InnerText.Replace("\n", "").Trim().Split('$');
                                        minPrice = Convert.ToDecimal(minMax[1].Replace("Max", ""));
                                        maxPrice = Convert.ToDecimal(minMax[2]);
                                    }

                                    //foreach (String item in this.session.FormElements.Keys)
                                    //{
                                    //    if (item.Contains("ddlPriceLevel"))
                                    //    {
                                    //        this.session.FormElements[item] = tmp.Attributes["value"].Value;
                                    //        break;
                                    //    }
                                    //}
                                    VSPriceLevel _priceLevel = new VSPriceLevel(tmp.Attributes["value"].Value, name[0], desc, price, maxPrice, minPrice);
                                    //getSectionByPriceLevel(_priceLevel);
                                    neighbourhood.PriceLevels.Add(_priceLevel);
                                    result = true;
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            type.Neighborhoods.Add(neighbourhood);

                        }
                        else
                        {
                            this.NoSeatsAvailable = true;
                            result = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }

                if (type.Neighborhoods.Count() > 0)
                {

                    AddUpdateField(this.Search.Session, type.Neighborhoods[0].FormElementKey, type.Neighborhoods[0].ID);
                    this.session.FormElements.Remove("m:c:btnChangeQty");
                    this.session.FormElements.Remove("m:c:btnReserve");
                      this.session.FormElements.Remove("m$c$g$ctl01$prg$p$ctl01$btnFindTickets");
                    if (this.session.FormElements.ContainsKey("__EVENTTARGET"))

                    {
                        this.session.FormElements["__EVENTTARGET"] = this.EventTarget;
                    }
                    this.session.Post(session.HTMLWeb.ResponseUri.AbsoluteUri);

                    getPriceAndSections(type);
                }
                else
                {
                    HtmlNodeCollection _price = session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlPriceLevel')]/option");

                    if (_price != null)
                    {
                        foreach (HtmlNode tmp in _price)
                        {
                            try
                            {
                                String desc = tmp.NextSibling.InnerHtml;
                                String[] name = desc.Split('-');
                                Decimal price = 0, minPrice = 0, maxPrice = 0;

                                //String priceRange = session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td").InnerText;

                                if (name.Length > 1)
                                {
                                    price = Convert.ToDecimal(name[name.Length - 1].Split('$')[1]);
                                    maxPrice = price;
                                    minPrice = price;
                                }
                                else
                                {
                                    HtmlNode minMaxNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td");
                                    if (minMaxNode == null)
                                    {
                                        minMaxNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@class = 'pricerangebox3']/p");
                                    }
                                    if (minMaxNode != null)
                                    {
                                        String[] minMax = minMaxNode.InnerText.Replace("\n", "").Trim().Split('$');
                                        minPrice = Convert.ToDecimal(minMax[1].Replace("Max", ""));
                                        maxPrice = Convert.ToDecimal(minMax[2]);
                                    }
                                }

                                //foreach (String item in this.session.FormElements.Keys)
                                //{
                                //    if (item.Contains("ddlPriceLevel"))
                                //    {
                                //        this.session.FormElements[item] = tmp.Attributes["value"].Value;
                                //        break;
                                //    }
                                //}
                                VSPriceLevel _priceLevel = new VSPriceLevel(tmp.Attributes["value"].Value, name[0], desc, price, maxPrice, minPrice);
                                //getSectionByPriceLevel(_priceLevel);
                                type.PriceLevels.Add(_priceLevel);
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                            }
                        }
                        getPriceAndSections(type);
                    }
                    else
                    {
                        this.NoSeatsAvailable = true;
                        result = false;
                    }
                }
            }
            else
            {
                HtmlNodeCollection _price = session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlPriceLevel')]/option");

                if (_price != null)
                {
                    foreach (HtmlNode tmp in _price)
                    {
                        try
                        {
                            String desc = tmp.NextSibling.InnerHtml;
                            String[] name = desc.Split('-');
                            Decimal price = 0, minPrice = 0, maxPrice = 0;

                            //String priceRange = session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td").InnerText;

                            if (name.Length > 1)
                            {
                                String value = String.Empty;

                                if (name[name.Length - 1].Contains("$"))
                                {
                                    value = name[name.Length - 1].Split('$')[1];
                                }
                                else
                                {
                                    value = name[name.Length - 1].Split(';')[1];
                                }
                                price = Convert.ToDecimal(value);
                                maxPrice = price;
                                minPrice = price;
                            }
                            else
                            {
                                HtmlNode minMaxNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td");
                                if (minMaxNode == null)
                                {
                                    minMaxNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@class = 'pricerangebox3']/p");
                                }
                                if (minMaxNode != null)
                                {
                                    String[] minMax;

                                    if (minMaxNode.InnerText.Replace("\n", "").Contains('$'))
                                    {
                                        minMax = minMaxNode.InnerText.Replace("\n", "").Trim().Split('$');
                                    }
                                    else
                                    {
                                        minMax = minMaxNode.InnerText.Replace("\n", "").Trim().Split(';');
                                    }

                                    minPrice = Convert.ToDecimal(minMax[1].Replace("Max", ""));
                                    maxPrice = Convert.ToDecimal(minMax[2]);
                                }
                            }

                            //foreach (String item in this.session.FormElements.Keys)
                            //{
                            //    if (item.Contains("ddlPriceLevel"))
                            //    {
                            //        this.session.FormElements[item] = tmp.Attributes["value"].Value;
                            //        break;
                            //    }
                            //}
                            VSPriceLevel _priceLevel = new VSPriceLevel(tmp.Attributes["value"].Value, name[0], desc, price, maxPrice, minPrice);
                            //getSectionByPriceLevel(_priceLevel);
                            type.PriceLevels.Add(_priceLevel);
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                        }
                    }
                    getPriceAndSections(type);
                }
                else
                {
                    this.NoSeatsAvailable = true;
                    result = false;
                }
            }
            return result;
        }

        public Boolean parsePriceLevelAndDates(VSTicketType ticketType)
        {
            VSTicketType type = ticketType;
            Boolean result = false;

            //TODO: if chk karo pricelevel aur section null hain to dosri post padege
            if (String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && String.IsNullOrEmpty(this.Search._CurrentParameter.Section) && String.IsNullOrEmpty(this.Search._CurrentParameter.Neighbourhood) && this.Search._CurrentParameter.PriceMin == null && this.Search._CurrentParameter.PriceMax == null)
            {
                session.FormElements.Remove("m:c:g:_ctl2:qg:p:_ctl1:chkProduct");
                session.FormElements.Remove("m:c:btnPriceRange");

                if (this.Search._CurrentParameter.Quantity >= (type.MinQuantity) && this.Search._CurrentParameter.Quantity <= type.MaxQuantity)
                {
                    if (this.session.FormElements.ContainsKey(ticketType.QuantityFormElementKey))
                    {
                        this.session.FormElements[ticketType.QuantityFormElementKey] = this.Search._CurrentParameter.Quantity.ToString();
                    }
                }

                if (this.session.FormElements.ContainsKey(ticketType.DateId))
                {
                    this.session.FormElements[ticketType.DateId] = "on";
                }
                IsBestAvailable = true;
                return true;
            }
            else //if (this.Search._CurrentParameter.Quantity >= (type.MinQuantity) && this.Search._CurrentParameter.Quantity <= type.MaxQuantity)
            {
                session.FormElements.Remove("m:c:g:_ctl2:qg:p:_ctl1:chkProduct");
                session.FormElements.Remove("m:c:btnNBA");

                if (this.session.FormElements.ContainsKey(ticketType.DateId))
                {
                    this.session.FormElements[ticketType.DateId] = "on";
                }

                if (this.Search._CurrentParameter.Quantity >= (type.MinQuantity) && this.Search._CurrentParameter.Quantity <= type.MaxQuantity)
                {
                    if (this.session.FormElements.ContainsKey(ticketType.QuantityFormElementKey))
                    {
                        this.session.FormElements[ticketType.QuantityFormElementKey] = this.Search._CurrentParameter.Quantity.ToString();
                    }
                }
                else
                {
                    this.TicketTypeMatched = false;
                    return false;
                }
            }



            if (this.session.FormElements.ContainsKey("m$c$g$ctl02$qg$btnNBAFlex"))
            {
                this.session.FormElements.Remove("m$c$g$ctl02$qg$btnNBAFlex");
            }

            if (this.session.FormElements.ContainsKey("m$c$g$ctl01$qg$p$ctl01$btnNBA"))
            {
                this.session.FormElements.Remove("m$c$g$ctl01$qg$p$ctl01$btnNBA");
            }

            if (this.session.FormElements.ContainsKey("m$c$g$ctl02$qg$p$ctl01$chkProduct"))
            {
                this.session.FormElements.Remove("m$c$g$ctl02$qg$p$ctl01$chkProduct");
            }

            if (this.session.FormElements.ContainsKey("__EVENTTARGET"))
            {
                this.session.FormElements["__EVENTTARGET"] = this.EventTarget;
            }


            //HtmlNode documentNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//a[contains(@class,'btn-default-link link-blue flex-item-grow')]");

            string keyToRemove = string.Empty;

            foreach (var item in this.session.FormElements)
            {
                if (item.Value.Equals("Find Tickets"))
                {
                    keyToRemove = item.Key;
                    break;
                }

            }

            if (!String.IsNullOrEmpty(keyToRemove))
            {
                this.session.FormElements.Remove(keyToRemove);
            }

            String html = this.session.Post(this.session.HTMLWeb.ResponseUri.AbsoluteUri);


            //TODO: add neighborhood parameter working here

            HtmlNodeCollection _neighborhood = session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlNeighbourhood')]/option");

            type.Neighborhoods.Clear();

            if (_neighborhood != null)
            {
                foreach (HtmlNode neighbour in _neighborhood)
                {
                    String descNeighbour = neighbour.NextSibling.InnerHtml;
                    String[] nameNeighbour = descNeighbour.Split('-');

                    VSNeighborhood neighbourhood = new VSNeighborhood(neighbour.Attributes["value"].Value, nameNeighbour[0], neighbour.ParentNode.Attributes["name"].Value);

                    if (!neighbourhood.ID.Equals("-1") || !neighbourhood.Description.ToLower().Contains("best avail"))
                    {
                        AddUpdateField(this.Search.Session, neighbourhood.FormElementKey, neighbourhood.ID);
                        this.session.FormElements.Remove("m:c:btnChangeQty");
                        this.session.FormElements.Remove("m:c:btnReserve");
                        this.session.Post(session.HTMLWeb.ResponseUri.AbsoluteUri);
                    }

                    HtmlNodeCollection _price = session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlPriceLevel')]/option");

                    if (_price != null)
                    {
                        foreach (HtmlNode tmp in _price)
                        {
                            String desc = tmp.NextSibling.InnerHtml;
                            String[] name = desc.Split('-');
                            Decimal price = 0, minPrice = 0, maxPrice = 0;

                            //String priceRange = session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td").InnerText;

                            if (name.Length > 1)
                            {
                                price = Convert.ToDecimal(name[name.Length - 1].Split('$')[1]);
                                maxPrice = price;
                                minPrice = price;
                            }
                            else
                            {
                                HtmlNode minMaxNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td");
                                String[] minMax = minMaxNode.InnerText.Replace("\n", "").Trim().Split('$');
                                minPrice = Convert.ToDecimal(minMax[1].Replace("Max", ""));
                                maxPrice = Convert.ToDecimal(minMax[2]);
                            }

                            //foreach (String item in this.session.FormElements.Keys)
                            //{
                            //    if (item.Contains("ddlPriceLevel"))
                            //    {
                            //        this.session.FormElements[item] = tmp.Attributes["value"].Value;
                            //        break;
                            //    }
                            //}
                            VSPriceLevel _priceLevel = new VSPriceLevel(tmp.Attributes["value"].Value, name[0], desc, price, maxPrice, minPrice);
                            //getSectionByPriceLevel(_priceLevel);
                            neighbourhood.PriceLevels.Add(_priceLevel);
                            result = true;
                        }
                        type.Neighborhoods.Add(neighbourhood);

                    }
                    else
                    {
                        this.NoSeatsAvailable = true;
                        result = false;
                    }
                }

                AddUpdateField(this.Search.Session, type.Neighborhoods[0].FormElementKey, type.Neighborhoods[0].ID);
                this.session.FormElements.Remove("m:c:btnChangeQty");
                this.session.FormElements.Remove("m:c:btnReserve");
                this.session.Post(session.HTMLWeb.ResponseUri.AbsoluteUri);

                getPriceAndSections(type);
            }
            else
            {
                HtmlNodeCollection _price = session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlPriceLevel')]/option");

                if (_price != null)
                {
                    foreach (HtmlNode tmp in _price)
                    {
                        try
                        {
                            String desc = tmp.NextSibling.InnerHtml;
                            String[] name = desc.Split('-');
                            Decimal price = 0, minPrice = 0, maxPrice = 0;

                            //String priceRange = session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td").InnerText;

                            if (name.Length > 1)
                            {
                                price = Convert.ToDecimal(name[name.Length - 1].Split('$')[1]);
                                maxPrice = price;
                                minPrice = price;
                            }
                            else
                            {
                                HtmlNode minMaxNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td");
                                if (minMaxNode == null)
                                {
                                    minMaxNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@class = 'pricerangebox3']/p");
                                }
                                if (minMaxNode != null)
                                {
                                    String[] minMax = minMaxNode.InnerText.Replace("\n", "").Trim().Split('$');
                                    minPrice = Convert.ToDecimal(minMax[1].Replace("Max", ""));
                                    maxPrice = Convert.ToDecimal(minMax[2]);
                                }
                            }

                            //foreach (String item in this.session.FormElements.Keys)
                            //{
                            //    if (item.Contains("ddlPriceLevel"))
                            //    {
                            //        this.session.FormElements[item] = tmp.Attributes["value"].Value;
                            //        break;
                            //    }
                            //}
                            VSPriceLevel _priceLevel = new VSPriceLevel(tmp.Attributes["value"].Value, name[0], desc, price, maxPrice, minPrice);
                            //getSectionByPriceLevel(_priceLevel);
                            type.PriceLevels.Add(_priceLevel);
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                        }
                    }
                    getPriceAndSections(type);
                }
                else
                {
                    this.NoSeatsAvailable = true;
                    result = false;
                }
            }
            return result;
        }

        public Boolean parsePriceLevel(VSTicketType ticketType, Boolean isnewEvents = false)
        {
            VSTicketType type = ticketType;
            Boolean result = false;

            //TODO: if chk karo pricelevel aur section null hain to dosri post padege
            if (String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && String.IsNullOrEmpty(this.Search._CurrentParameter.Section) && String.IsNullOrEmpty(this.Search._CurrentParameter.Neighbourhood) && this.Search._CurrentParameter.PriceMin == null && this.Search._CurrentParameter.PriceMax == null)
            {
                session.FormElements.Remove("m:c:g:_ctl2:qg:p:_ctl1:chkProduct");
                session.FormElements.Remove("m:c:btnPriceRange");

                if (this.Search._CurrentParameter.Quantity >= (type.MinQuantity) && this.Search._CurrentParameter.Quantity <= type.MaxQuantity)
                {
                    if (this.session.FormElements.ContainsKey(ticketType.QuantityFormElementKey))
                    {
                        this.session.FormElements[ticketType.QuantityFormElementKey] = this.Search._CurrentParameter.Quantity.ToString();
                    }
                }
                IsBestAvailable = true;
                return true;
            }
            else //if (this.Search._CurrentParameter.Quantity >= (type.MinQuantity) && this.Search._CurrentParameter.Quantity <= type.MaxQuantity)
            {
                session.FormElements.Remove("m:c:g:_ctl2:qg:p:_ctl1:chkProduct");
                session.FormElements.Remove("m:c:btnNBA");

                if (this.Search._CurrentParameter.Quantity >= (type.MinQuantity) && this.Search._CurrentParameter.Quantity <= type.MaxQuantity)
                {
                    if (this.session.FormElements.ContainsKey(ticketType.QuantityFormElementKey))
                    {
                        this.session.FormElements[ticketType.QuantityFormElementKey] = this.Search._CurrentParameter.Quantity.ToString();
                    }
                }
                else
                {
                    this.TicketTypeMatched = false;
                    return false;
                }
            }

            if (this.session.FormElements.ContainsKey("m$c$g$ctl02$qg$btnNBAFlex"))
            {
                this.session.FormElements.Remove("m$c$g$ctl02$qg$btnNBAFlex");
            }

            if (this.session.FormElements.ContainsKey("m$c$g$ctl01$qg$p$ctl01$btnNBA"))
            {
                this.session.FormElements.Remove("m$c$g$ctl01$qg$p$ctl01$btnNBA");
            }

            if (this.session.FormElements.ContainsKey("m$c$g$ctl02$qg$p$ctl01$chkProduct"))
            {
                this.session.FormElements.Remove("m$c$g$ctl02$qg$p$ctl01$chkProduct");
            }

            if (this.session.FormElements.ContainsKey("__EVENTTARGET"))
            {
                this.session.FormElements["__EVENTTARGET"] = this.EventTarget;
            }


            //HtmlNode documentNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//a[contains(@class,'btn-default-link link-blue flex-item-grow')]");


            if (!String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel))
            {


                HtmlNodeCollection _price = session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@name,'QuantityAndPrice$cboPriceLevels')]/option[contains(@class,'priceLevel')]");

                if (_price != null)
                {
                    foreach (HtmlNode tmp in _price)
                    {
                        try
                        {
                            if (this.Search._CurrentParameter.ExactMatch && tmp.NextSibling.InnerText.ToLower().Equals(this.Search._CurrentParameter.PriceLevel.ToLower()))
                            {
                                if (this.session.FormElements.ContainsKey("m$c$ucPickASection$ucQuantityAndPrice$cboPriceLevels"))
                                {
                                    this.session.FormElements["m$c$ucPickASection$ucQuantityAndPrice$cboPriceLevels"] = tmp.Attributes["value"].Value;
                                }
                                else
                                {
                                    this.session.FormElements.Add("m$c$ucPickASection$ucQuantityAndPrice$cboPriceLevels", tmp.Attributes["value"].Value);
                                }

                                result = true;
                                break;
                            }

                            else if (!this.Search._CurrentParameter.ExactMatch && tmp.NextSibling.InnerText.ToLower().Contains(this.Search._CurrentParameter.PriceLevel.ToLower()))
                            {
                                if (this.session.FormElements.ContainsKey("m$c$ucPickASection$ucQuantityAndPrice$cboPriceLevels"))
                                {
                                    this.session.FormElements["m$c$ucPickASection$ucQuantityAndPrice$cboPriceLevels"] = tmp.Attributes["value"].Value;
                                }
                                else
                                {
                                    this.session.FormElements.Add("m$c$ucPickASection$ucQuantityAndPrice$cboPriceLevels", tmp.Attributes["value"].Value);
                                }

                                result = true;
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                        }
                    }
                    //getPriceAndSections(type);
                }
                else
                {
                    _price = session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@name,'QuantityAndPrice:cboPriceLevels')]/option[contains(@class,'priceLevel')]");

                    if (_price != null)
                    {
                        foreach (HtmlNode tmp in _price)
                        {
                            try
                            {
                                if (this.Search._CurrentParameter.ExactMatch && tmp.NextSibling.InnerText.ToLower().Equals(this.Search._CurrentParameter.PriceLevel.ToLower()))
                                {
                                    if (this.session.FormElements.ContainsKey("m:c:ucPickASection:ucQuantityAndPrice:cboPriceLevels"))
                                    {
                                        this.session.FormElements["m:c:ucPickASection:ucQuantityAndPrice:cboPriceLevels"] = tmp.Attributes["value"].Value;
                                    }
                                    else
                                    {
                                        this.session.FormElements.Add("m:c:ucPickASection:ucQuantityAndPrice:cboPriceLevels", tmp.Attributes["value"].Value);
                                    }

                                    result = true;
                                    break;
                                }

                                else if (!this.Search._CurrentParameter.ExactMatch && tmp.NextSibling.InnerText.ToLower().Contains(this.Search._CurrentParameter.PriceLevel.ToLower()))
                                {
                                    if (this.session.FormElements.ContainsKey("m:c:ucPickASection:ucQuantityAndPrice:cboPriceLevels"))
                                    {
                                        this.session.FormElements["m:c:ucPickASection:ucQuantityAndPrice:cboPriceLevels"] = tmp.Attributes["value"].Value;
                                    }
                                    else
                                    {
                                        this.session.FormElements.Add("m:c:ucPickASection:ucQuantityAndPrice:cboPriceLevels", tmp.Attributes["value"].Value);
                                    }

                                    result = true;
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                            }
                        }
                        //getPriceAndSections(type);
                    }
                    else
                    {
                        // this.NoSeatsAvailable = true;
                        // result = false;
                    }
                }
            }

            return result;
        }

        private void getPriceAndSections(VSTicketType type)
        {
            Boolean ifPriceStringMatch = false, ifSectionStringMatch = false, ifNeighborhoodsStringMatch = false;

            if (type.Neighborhoods.Count > 0)
            {
                if (!String.IsNullOrEmpty(this.Search._CurrentParameter.Neighbourhood))
                {
                    foreach (VSNeighborhood neighbour in type.Neighborhoods)
                    {
                        if (!ifNeighborhoodsStringMatch && (this.Search._CurrentParameter.ExactMatch ? neighbour.Description.ToLower() == this.Search._CurrentParameter.Neighbourhood.ToLower() : neighbour.Description.ToLower().Contains(this.Search._CurrentParameter.Neighbourhood.ToLower())))
                        {
                            ifNeighborhoodsStringMatch = true;

                            AddUpdateField(this.Search.Session, neighbour.FormElementKey, neighbour.ID);
                            this.session.FormElements.Remove("m$c$g$ctl01$prg$p$ctl01$btnFindTickets");
                            this.session.FormElements.Remove("m:c:btnChangeQty");
                            this.session.FormElements.Remove("m:c:btnReserve");
                            AddUpdateField(session, "__EVENTTARGET", this.EventTarget);
                            this.session.Post(session.HTMLWeb.ResponseUri.AbsoluteUri);

                            if (!String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && !String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                            {
                                foreach (VSPriceLevel priceLevel in neighbour.PriceLevels)
                                {
                                    if (!ifPriceStringMatch && (this.Search._CurrentParameter.ExactMatch ? priceLevel.Description.ToLower() == this.Search._CurrentParameter.PriceLevel.ToLower() : priceLevel.Description.ToLower().Contains(this.Search._CurrentParameter.PriceLevel.ToLower())))
                                    {
                                        ifPriceStringMatch = true;
                                        this.PriceLevelMatched = true;
                                        if (session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel"))
                                        {
                                            AddUpdateField(this.session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", priceLevel.ID);
                                        }
                                        else if (session.FormElements.ContainsKey("m$c$g$ctl01$prg$p$ctl01$ddlPriceLevel"))
                                        {
                                            AddUpdateField(this.session, "m$c$g$ctl01$prg$p$ctl01$ddlPriceLevel", priceLevel.ID);
                                        }

                                        this.getSectionByPriceLevel(priceLevel);
                                        for (int j = 0; j < priceLevel.Section.Count; j++)
                                        {
                                            if (!ifSectionStringMatch && (this.Search._CurrentParameter.ExactMatch ? priceLevel.Section[j].SecName.ToLower() == this.Search._CurrentParameter.Section.ToLower() : priceLevel.Section[j].SecName.ToLower().Contains(this.Search._CurrentParameter.Section.ToLower())))
                                            {
                                                ifSectionStringMatch = true;
                                                this.SectionMatched = true;

                                                if (this.Search._CurrentParameter.PriceMax == null && this.Search._CurrentParameter.PriceMin == null)
                                                {
                                                    if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlSections"))
                                                    {
                                                        AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", priceLevel.Section[j].ID);
                                                    }
                                                    else if (this.session.FormElements.ContainsKey("m$c$g$ctl01$prg$p$ctl01$ddlSections"))
                                                    {
                                                        AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlSections", priceLevel.Section[j].ID);
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    //TODO: match price with max nd min quantity
                                                    VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, neighbour);
                                                    if (value != null)
                                                    {
                                                        if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlSections"))
                                                        {
                                                            AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[j].ID);
                                                        }
                                                        else if (this.session.FormElements.ContainsKey("m$c$g$ctl01$prg$p$ctl01$ddlSections"))
                                                        {
                                                            AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", priceLevel.Section[j].ID);
                                                        }
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        ////TODO: If no price match
                                                        this.Search.MoreInfo = "Price range does not exists. Please review the minimum and maximum price range.";
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                        if (ifSectionStringMatch)
                                        {
                                            break;
                                        }
                                    }
                                }
                                if (!ifSectionStringMatch && ifPriceStringMatch)
                                {
                                    this.Search.MoreInfo = "\"" + this.Search._CurrentParameter.Section + "\" " + TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                    return;
                                }
                                if (!ifPriceStringMatch)
                                {
                                    this.Search.MoreInfo = "\"" + this.Search._CurrentParameter.PriceLevel + "\" " + TicketSearchStatus.MoreInfoPriceLevelNotMatch;
                                    return;
                                }
                            }
                            else if (String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                            {
                                //TODO: Match parameter if price level and section both values are not null

                                if (this.Search._CurrentParameter.PriceMax == null && this.Search._CurrentParameter.PriceMin == null)
                                {
                                    AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", neighbour.PriceLevels[0].ID);
                                    this.getSectionByPriceLevel(neighbour.PriceLevels[0]);
                                    AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", neighbour.PriceLevels[0].Section[0].ID);
                                    this.PriceLevelMatched = this.SectionMatched = true;
                                }
                                else
                                {
                                    //TODO: match price with max nd min quantity
                                    VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, neighbour);
                                    if (value != null)
                                    {
                                        AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", value.ID);
                                        this.getSectionByPriceLevel(value);
                                        AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);
                                        this.PriceLevelMatched = this.SectionMatched = true;
                                        //break;
                                    }
                                    else
                                    {
                                        //TODO: If no price match
                                        this.Search.MoreInfo = "Price range does not exists. Please review the minimum and maximum price range.";
                                        return;
                                    }
                                }
                            }
                            else if (String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) || String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                            {
                                //TODO: Match parameter if price level or section both values are not null
                                if (!String.IsNullOrEmpty(this.Search._CurrentParameter.Section) && String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel))
                                {
                                    if (this.Search._CurrentParameter.PriceMax != null && this.Search._CurrentParameter.PriceMin != null)
                                    {
                                        VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, neighbour);
                                        this.getSectionByPriceLevel(value);
                                        this.PriceLevelMatched = true;

                                        if (value != null)
                                        {
                                            foreach (VSSection section in neighbour.PriceLevels[0].Section)
                                            {
                                                if (!ifSectionStringMatch && (this.Search._CurrentParameter.ExactMatch ? section.SecName.ToLower() == this.Search._CurrentParameter.Section.ToLower() : section.SecName.ToLower().Contains(this.Search._CurrentParameter.Section.ToLower())))
                                                {
                                                    ifSectionStringMatch = true;
                                                    this.SectionMatched = true;
                                                    AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", neighbour.PriceLevels[0].ID);
                                                    AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);
                                                    break;
                                                }
                                            }
                                            if (!ifSectionStringMatch)
                                            {
                                                this.Search.MoreInfo = TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.getSectionByPriceLevel(neighbour.PriceLevels[0]);
                                        this.PriceLevelMatched = true;
                                        foreach (VSSection section in neighbour.PriceLevels[0].Section)
                                        {
                                            if (!ifSectionStringMatch && (this.Search._CurrentParameter.ExactMatch ? section.SecName.ToLower() == this.Search._CurrentParameter.Section.ToLower() : section.SecName.ToLower().Contains(this.Search._CurrentParameter.Section.ToLower())))
                                            {
                                                ifSectionStringMatch = true;
                                                this.SectionMatched = true;
                                                AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", neighbour.PriceLevels[0].ID);
                                                AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);
                                                break;
                                            }
                                        }
                                        if (!ifSectionStringMatch)
                                        {
                                            this.Search.MoreInfo = TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                            return;
                                        }
                                    }
                                    //}
                                }
                                else if (!String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                                {
                                    for (int j = 0; j < neighbour.PriceLevels.Count; j++)
                                    {
                                        if (!ifPriceStringMatch && (this.Search._CurrentParameter.ExactMatch ? neighbour.PriceLevels[j].Description.ToLower() == this.Search._CurrentParameter.PriceLevel.ToLower() : neighbour.PriceLevels[j].Description.ToLower().Contains(this.Search._CurrentParameter.PriceLevel.ToLower())))
                                        {
                                            ifPriceStringMatch = true;
                                            this.PriceLevelMatched = true;

                                            AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", neighbour.PriceLevels[j].ID);

                                            this.getSectionByPriceLevel(neighbour.PriceLevels[j]);

                                            if (this.Search._CurrentParameter.PriceMax == null && this.Search._CurrentParameter.PriceMin == null)
                                            {
                                                AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", neighbour.PriceLevels[j].Section[0].ID);
                                                this.SectionMatched = true;
                                                break;
                                            }
                                            else
                                            {
                                                //TODO: match price with max nd min quantity
                                                VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, neighbour);
                                                if (value != null)
                                                {
                                                    if (value == neighbour.PriceLevels[j])
                                                    {
                                                        ifPriceStringMatch = true;
                                                        AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);
                                                        this.SectionMatched = true;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        //TODO: If no price match
                                                        this.Search.MoreInfo = "Price range does not match with the PriceLevel.";
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    //TODO: If no price match
                                                    this.Search.MoreInfo = "Price range does not match.";
                                                    return;
                                                }
                                            }

                                        }
                                    }
                                    if (!ifPriceStringMatch)
                                    {
                                        this.Search.MoreInfo = TicketSearchStatus.MoreInfoPriceLevelNotMatch;
                                        return;
                                    }
                                }
                            }
                        }
                        if (ifNeighborhoodsStringMatch)
                        {
                            break;
                        }
                    }
                    if (!ifNeighborhoodsStringMatch)
                    {
                        this.Search.MoreInfo = "\"" + this.Search._CurrentParameter.Neighbourhood + "\" " + TicketSearchStatus.MoreInfoNeighbourhoodNotMatch;
                        return;
                    }
                }
                else
                {
                    //if (!ifNeighborhoodsStringMatch && (this.Search._CurrentParameter.ExactMatch ? type.Neighborhoods[0].Description.ToLower() == this.Search._CurrentParameter.Neighbourhood.ToLower() : type.Neighborhoods[0].Description.ToLower().Contains(this.Search._CurrentParameter.Neighbourhood.ToLower())))
                    {
                        ifNeighborhoodsStringMatch = true;

                        if (!String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && !String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                        {
                            foreach (VSPriceLevel priceLevel in type.Neighborhoods[0].PriceLevels)
                            {
                                if (!ifPriceStringMatch && (this.Search._CurrentParameter.ExactMatch ? priceLevel.Description.ToLower() == this.Search._CurrentParameter.PriceLevel.ToLower() : priceLevel.Description.ToLower().Contains(this.Search._CurrentParameter.PriceLevel.ToLower())))
                                {
                                    ifPriceStringMatch = true;
                                    this.PriceLevelMatched = true;
                                    AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", priceLevel.ID);

                                    this.getSectionByPriceLevel(priceLevel);
                                    for (int j = 0; j < priceLevel.Section.Count; j++)
                                    {
                                        if (!ifSectionStringMatch && (this.Search._CurrentParameter.ExactMatch ? priceLevel.Section[j].SecName.ToLower() == this.Search._CurrentParameter.Section.ToLower() : priceLevel.Section[j].SecName.ToLower().Contains(this.Search._CurrentParameter.Section.ToLower())))
                                        {
                                            ifSectionStringMatch = true;
                                            this.SectionMatched = true;

                                            if (this.Search._CurrentParameter.PriceMax == null && this.Search._CurrentParameter.PriceMin == null)
                                            {
                                                //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", priceLevel.Section[j].ID);
                                                break;
                                            }
                                            else
                                            {
                                                //TODO: match price with max nd min quantity
                                                VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, type.Neighborhoods[0]);
                                                if (value != null)
                                                {
                                                    //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[j].ID);
                                                    break;
                                                }
                                                else
                                                {
                                                    ////TODO: If no price match
                                                    this.Search.MoreInfo = "Price range does not exists. Please review the minimum and maximum price range.";
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    if (ifSectionStringMatch)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (!ifSectionStringMatch && ifPriceStringMatch)
                            {
                                this.Search.MoreInfo = "\"" + this.Search._CurrentParameter.Section + "\" " + TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                return;
                            }
                            if (!ifPriceStringMatch)
                            {
                                this.Search.MoreInfo = "\"" + this.Search._CurrentParameter.PriceLevel + "\" " + TicketSearchStatus.MoreInfoPriceLevelNotMatch;
                                return;
                            }
                        }
                        else if (String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                        {
                            //TODO: Match parameter if price level and section both values are not null

                            if (this.Search._CurrentParameter.PriceMax == null && this.Search._CurrentParameter.PriceMin == null)
                            {
                                AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.Neighborhoods[0].PriceLevels[0].ID);
                                this.getSectionByPriceLevel(type.Neighborhoods[0].PriceLevels[0]);
                                //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", type.Neighborhoods[0].PriceLevels[0].Section[0].ID);
                                this.PriceLevelMatched = this.SectionMatched = true;
                            }
                            else
                            {
                                //TODO: match price with max nd min quantity
                                VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, type.Neighborhoods[0]);
                                if (value != null)
                                {
                                    AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", value.ID);
                                    this.getSectionByPriceLevel(value);
                                    //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);
                                    this.PriceLevelMatched = this.SectionMatched = true;
                                    //break;
                                }
                                else
                                {
                                    //TODO: If no price match
                                    this.Search.MoreInfo = "Price range does not exists. Please review the minimum and maximum price range.";
                                    return;
                                }
                            }
                        }
                        else if (String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) || String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                        {
                            //TODO: Match parameter if price level or section both values are not null
                            if (!String.IsNullOrEmpty(this.Search._CurrentParameter.Section) && String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel))
                            {
                                if (this.Search._CurrentParameter.PriceMax != null && this.Search._CurrentParameter.PriceMin != null)
                                {
                                    VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, type.Neighborhoods[0]);
                                    this.getSectionByPriceLevel(value);
                                    this.PriceLevelMatched = true;

                                    if (value != null)
                                    {

                                        ifSectionStringMatch = true;
                                        this.SectionMatched = true;
                                        AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.Neighborhoods[0].PriceLevels[0].ID);
                                        //foreach (VSSection section in type.Neighborhoods[0].PriceLevels[0].Section)
                                        //{
                                        //    if (!ifSectionStringMatch && (this.Search._CurrentParameter.ExactMatch ? section.SecName.ToLower() == this.Search._CurrentParameter.Section.ToLower() : section.SecName.ToLower().Contains(this.Search._CurrentParameter.Section.ToLower())))
                                        //    {
                                        //        ifSectionStringMatch = true;
                                        //        this.SectionMatched = true;
                                        //        AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.Neighborhoods[0].PriceLevels[0].ID);
                                        //        //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);
                                        //        break;
                                        //    }
                                        //}
                                        if (!ifSectionStringMatch)
                                        {
                                            this.Search.MoreInfo = TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    this.getSectionByPriceLevel(type.Neighborhoods[0].PriceLevels[0]);
                                    this.PriceLevelMatched = true;
                                    foreach (VSSection section in type.Neighborhoods[0].PriceLevels[0].Section)
                                    {
                                        if (!ifSectionStringMatch && (this.Search._CurrentParameter.ExactMatch ? section.SecName.ToLower() == this.Search._CurrentParameter.Section.ToLower() : section.SecName.ToLower().Contains(this.Search._CurrentParameter.Section.ToLower())))
                                        {
                                            ifSectionStringMatch = true;
                                            this.SectionMatched = true;
                                            AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.Neighborhoods[0].PriceLevels[0].ID);
                                            //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);
                                            break;
                                        }
                                    }
                                    if (!ifSectionStringMatch)
                                    {
                                        this.Search.MoreInfo = TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                        return;
                                    }
                                }
                                //}
                            }
                            else if (!String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                            {
                                for (int j = 0; j < type.Neighborhoods[0].PriceLevels.Count; j++)
                                {
                                    if (!ifPriceStringMatch && (this.Search._CurrentParameter.ExactMatch ? type.Neighborhoods[0].PriceLevels[j].Description.ToLower() == this.Search._CurrentParameter.PriceLevel.ToLower() : type.Neighborhoods[0].PriceLevels[j].Description.ToLower().Contains(this.Search._CurrentParameter.PriceLevel.ToLower())))
                                    {
                                        ifPriceStringMatch = true;
                                        this.PriceLevelMatched = true;

                                        AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.Neighborhoods[0].PriceLevels[j].ID);

                                        this.getSectionByPriceLevel(type.Neighborhoods[0].PriceLevels[j]);

                                        if (this.Search._CurrentParameter.PriceMax == null && this.Search._CurrentParameter.PriceMin == null)
                                        {
                                            //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", type.Neighborhoods[0].PriceLevels[j].Section[0].ID);
                                            this.SectionMatched = true;
                                            break;
                                        }
                                        else
                                        {
                                            //TODO: match price with max nd min quantity
                                            VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, type.Neighborhoods[0]);
                                            if (value != null)
                                            {
                                                if (value == type.Neighborhoods[0].PriceLevels[j])
                                                {
                                                    ifPriceStringMatch = true;
                                                    //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);
                                                    this.SectionMatched = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    //TODO: If no price match
                                                    this.Search.MoreInfo = "Price range does not match with the PriceLevel.";
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                //TODO: If no price match
                                                this.Search.MoreInfo = "Price range does not match.";
                                                return;
                                            }
                                        }

                                    }
                                }
                                if (!ifPriceStringMatch)
                                {
                                    this.Search.MoreInfo = TicketSearchStatus.MoreInfoPriceLevelNotMatch;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && !String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                {
                    foreach (VSPriceLevel priceLevel in type.PriceLevels)
                    {
                        if (!ifPriceStringMatch && (this.Search._CurrentParameter.ExactMatch ? priceLevel.Description.ToLower() == this.Search._CurrentParameter.PriceLevel.ToLower() : priceLevel.Description.ToLower().Contains(this.Search._CurrentParameter.PriceLevel.ToLower())))
                        {
                            ifPriceStringMatch = true;
                            this.PriceLevelMatched = true;

                            if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel"))
                            {
                                AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", priceLevel.ID);
                            }
                            else
                            {
                                AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlPriceLevel", priceLevel.ID);
                            }

                            this.getSectionByPriceLevel(priceLevel);
                            for (int j = 0; j < priceLevel.Section.Count; j++)
                            {
                                if (!ifSectionStringMatch && (this.Search._CurrentParameter.ExactMatch ? priceLevel.Section[j].SecName.ToLower() == this.Search._CurrentParameter.Section.ToLower() : priceLevel.Section[j].SecName.ToLower().Contains(this.Search._CurrentParameter.Section.ToLower())))
                                {
                                    ifSectionStringMatch = true;
                                    this.SectionMatched = true;

                                    if (this.Search._CurrentParameter.PriceMax == null && this.Search._CurrentParameter.PriceMin == null)
                                    {

                                        if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlSections"))
                                        {
                                            AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", priceLevel.Section[j].ID);
                                        }
                                        else
                                        {
                                            AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlSections", priceLevel.Section[j].ID);
                                        }

                                        break;
                                    }
                                    else
                                    {
                                        //TODO: match price with max nd min quantity
                                        VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, type);
                                        if (value != null)
                                        {
                                            // AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[j].ID);

                                            if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlSections"))
                                            {
                                                AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", priceLevel.Section[j].ID);
                                            }
                                            else
                                            {
                                                AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlSections", priceLevel.Section[j].ID);
                                            }

                                            break;
                                        }
                                        else
                                        {
                                            ////TODO: If no price match
                                            this.Search.MoreInfo = "Price range does not exists. Please review the minimum and maximum price range.";
                                            return;
                                        }
                                    }
                                }
                            }
                            if (ifSectionStringMatch)
                            {
                                break;
                            }
                        }
                    }
                    if (!ifSectionStringMatch && ifPriceStringMatch)
                    {
                        this.Search.MoreInfo = "\"" + this.Search._CurrentParameter.Section + "\" " + TicketSearchStatus.MoreInfoSectionStringNotMatch;
                        return;
                    }
                    if (!ifPriceStringMatch)
                    {
                        this.Search.MoreInfo = "\"" + this.Search._CurrentParameter.PriceLevel + "\" " + TicketSearchStatus.MoreInfoPriceLevelNotMatch;
                        return;
                    }
                }
                else if (String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                {
                    //TODO: Match parameter if price level and section both values are not null

                    if (this.Search._CurrentParameter.PriceMax == null && this.Search._CurrentParameter.PriceMin == null)
                    {
                        if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel"))
                        {
                            AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.PriceLevels[0].ID);
                        }
                        else
                        {
                            AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlPriceLevel", type.PriceLevels[0].ID);
                        }


                        // AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.PriceLevels[0].ID);

                        this.getSectionByPriceLevel(type.PriceLevels[0]);

                        // AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", type.PriceLevels[0].Section[0].ID);

                        if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlSections"))
                        {
                            AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", type.PriceLevels[0].Section[0].ID);
                        }
                        else
                        {
                            AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlSections", type.PriceLevels[0].Section[0].ID);
                        }


                        this.PriceLevelMatched = this.SectionMatched = true;
                    }
                    else
                    {
                        //TODO: match price with max nd min quantity
                        VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, type);
                        if (value != null)
                        {
                            //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", value.ID);

                            if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel"))
                            {
                                AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", value.ID);
                            }
                            else
                            {
                                AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlPriceLevel", value.ID);
                            }

                            this.getSectionByPriceLevel(value);

                            // AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);

                            if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlSections"))
                            {
                                AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);
                            }
                            else
                            {
                                AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlSections", value.Section[0].ID);
                            }

                            this.PriceLevelMatched = this.SectionMatched = true;
                            //break;
                        }
                        else
                        {
                            //TODO: If no price match
                            this.Search.MoreInfo = "Price range does not exists. Please review the minimum and maximum price range.";
                            return;
                        }
                    }
                }
                else if (String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) || String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                {
                    //TODO: Match parameter if price level or section both values are not null
                    if (!String.IsNullOrEmpty(this.Search._CurrentParameter.Section) && String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel))
                    {
                        if (this.Search._CurrentParameter.PriceMax != null && this.Search._CurrentParameter.PriceMin != null)
                        {
                            VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, type);
                            this.getSectionByPriceLevel(value);
                            this.PriceLevelMatched = true;

                            if (value != null)
                            {
                                foreach (VSSection section in type.PriceLevels[0].Section)
                                {
                                    if (!ifSectionStringMatch && (this.Search._CurrentParameter.ExactMatch ? section.SecName.ToLower() == this.Search._CurrentParameter.Section.ToLower() : section.SecName.ToLower().Contains(this.Search._CurrentParameter.Section.ToLower())))
                                    {
                                        ifSectionStringMatch = true;
                                        this.SectionMatched = true;
                                        //   AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.PriceLevels[0].ID);

                                        if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel"))
                                        {
                                            AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.PriceLevels[0].ID);
                                        }
                                        else
                                        {
                                            AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlPriceLevel", type.PriceLevels[0].ID);
                                        }

                                        if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlSections"))
                                        {
                                            AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);
                                        }
                                        else
                                        {
                                            AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlSections", section.ID);
                                        }

                                        //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);
                                        break;
                                    }
                                }
                                if (!ifSectionStringMatch)
                                {
                                    this.Search.MoreInfo = TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            this.getSectionByPriceLevel(type.PriceLevels[0]);
                            this.PriceLevelMatched = true;
                            foreach (VSSection section in type.PriceLevels[0].Section)
                            {
                                if (!ifSectionStringMatch && (this.Search._CurrentParameter.ExactMatch ? section.SecName.ToLower() == this.Search._CurrentParameter.Section.ToLower() : section.SecName.ToLower().Contains(this.Search._CurrentParameter.Section.ToLower())))
                                {
                                    ifSectionStringMatch = true;
                                    this.SectionMatched = true;
                                    //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.PriceLevels[0].ID);
                                    //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);

                                    if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel"))
                                    {
                                        AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.PriceLevels[0].ID);
                                    }
                                    else
                                    {
                                        AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlPriceLevel", type.PriceLevels[0].ID);
                                    }

                                    if (this.session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlSections"))
                                    {
                                        AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", section.ID);
                                    }
                                    else
                                    {
                                        AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlSections", section.ID);
                                    }

                                    break;
                                }
                            }
                            if (!ifSectionStringMatch)
                            {
                                this.Search.MoreInfo = TicketSearchStatus.MoreInfoSectionStringNotMatch;
                                return;
                            }
                        }
                        //}
                    }
                    else if (!String.IsNullOrEmpty(this.Search._CurrentParameter.PriceLevel) && String.IsNullOrEmpty(this.Search._CurrentParameter.Section))
                    {
                        for (int j = 0; j < type.PriceLevels.Count; j++)
                        {
                            if (!ifPriceStringMatch && (this.Search._CurrentParameter.ExactMatch ? type.PriceLevels[j].Description.ToLower() == this.Search._CurrentParameter.PriceLevel.ToLower() : type.PriceLevels[j].Description.ToLower().Contains(this.Search._CurrentParameter.PriceLevel.ToLower())))
                            {
                                ifPriceStringMatch = true;
                                this.PriceLevelMatched = true;

                                if (session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel"))
                                {
                                    AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlPriceLevel", type.PriceLevels[j].ID);
                                }
                                else
                                {
                                    AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlPriceLevel", type.PriceLevels[j].ID);
                                }

                                this.getSectionByPriceLevel(type.PriceLevels[j]);

                                if (this.Search._CurrentParameter.PriceMax == null && this.Search._CurrentParameter.PriceMin == null)
                                {
                                    if (session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlSections"))
                                    {
                                        AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", type.PriceLevels[j].Section[0].ID);
                                    }
                                    else
                                    {
                                        AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlSections", type.PriceLevels[j].Section[0].ID);
                                    }

                                    //AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", type.PriceLevels[j].Section[0].ID);
                                    this.SectionMatched = true;
                                    break;
                                }
                                else
                                {
                                    //TODO: match price with max nd min quantity
                                    VSPriceLevel value = this.Search.getPriceLevel(this.Search._CurrentParameter, type);
                                    if (value != null)
                                    {
                                        if (value == type.PriceLevels[j])
                                        {
                                            ifPriceStringMatch = true;
                                            // AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);

                                            if (session.FormElements.ContainsKey("m:c:g:_ctl1:prg:p:_ctl1:ddlSections"))
                                            {
                                                AddUpdateField(this.Search.Session, "m:c:g:_ctl1:prg:p:_ctl1:ddlSections", value.Section[0].ID);
                                            }
                                            else
                                            {
                                                AddUpdateField(this.Search.Session, "m$c$g$ctl01$prg$p$ctl01$ddlSections", value.Section[0].ID);
                                            }

                                            this.SectionMatched = true;
                                            break;
                                        }
                                        else
                                        {
                                            //TODO: If no price match
                                            this.Search.MoreInfo = "Price range does not match with the PriceLevel.";
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        //TODO: If no price match
                                        this.Search.MoreInfo = "Price range does not match.";
                                        return;
                                    }
                                }

                            }
                        }
                        if (!ifPriceStringMatch)
                        {
                            this.Search.MoreInfo = TicketSearchStatus.MoreInfoPriceLevelNotMatch;
                            return;
                        }
                    }
                }
            }
        }

        private Boolean ddlQuantityFormat()
        {
            Boolean result = false;
            try
            {
                HtmlNode htmlNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//div[contains(@id,'pnlGroup_0')]");

                if (htmlNode != null)
                {
                    HTML.LoadHtml(htmlNode.InnerHtml);

                    HtmlNodeCollection type = this.HTML.DocumentNode.SelectNodes("//table[@class='pricecodebox']/tr");

                    if (type != null)
                    {
                        foreach (HtmlNode item in type)
                        {
                            if (item.InnerHtml.Contains("<em>"))
                            {
                                string ticketType = item.SelectSingleNode("td[1]").InnerHtml.Substring(0, item.SelectSingleNode("td[1]").InnerHtml.IndexOf("<em>")).Replace("&nbsp;", "").Replace("\n", "");


                                HtmlNodeCollection quantityRange = item.SelectNodes("td[2]");

                                foreach (HtmlNode node in quantityRange)
                                {
                                    HtmlNodeCollection gtyRange = quantityRange[0].SelectNodes("select/option");

                                    minQty = int.Parse(gtyRange[0].Attributes["value"].Value);
                                    if (minQty == 0)
                                    {
                                        minQty += 1;
                                    }
                                    maxQty = int.Parse(gtyRange[gtyRange.Count - 1].Attributes["value"].Value);
                                    qtyStep = int.Parse(gtyRange[1].Attributes["value"].Value) - int.Parse(gtyRange[0].Attributes["value"].Value);

                                    //if (String.IsNullOrEmpty(this.Search._CurrentParameter.TicketTypeString))
                                    {
                                        VSTicketType typeTicket = new VSTicketType(ticketType, ticketType, minQty, maxQty, qtyStep, quantityRange[0].SelectSingleNode("select").Attributes["name"].Value);


                                        TicketTypes.Add(typeTicket);

                                    }
                                }

                                result = true;
                            }
                        }
                        if (String.IsNullOrEmpty(this.Search._CurrentParameter.TicketTypeString))
                        {
                            this.TicketTypeMatched = true;
                            TicketTypes[0].HasPriceLevel = this.parsePriceLevel(TicketTypes[0]);
                        }
                        else
                        {
                            foreach (VSTicketType ticketType in TicketTypes)
                            {
                                if (this.Search._CurrentParameter.ExactMatch ? ticketType.Description.ToLower() == this.Search._CurrentParameter.TicketTypeString.ToLower() : ticketType.Description.ToLower().Contains(this.Search._CurrentParameter.TicketTypeString.ToLower()))
                                {
                                    this.TicketTypeMatched = true;
                                    ticketType.HasPriceLevel = this.parsePriceLevel(ticketType);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    HtmlNodeCollection multipleDates = this.session.HtmlDocument.DocumentNode.SelectNodes("//div[@class='qtygrp__panel']");

                    if (multipleDates != null)
                    {

                        foreach (var item in multipleDates)
                        {
                            try
                            {
                                string ticketTitle = item.SelectSingleNode(".//span[contains(@class,'qtygrp__panel-info-heading')]").InnerText;
                                string ticketType = item.SelectSingleNode(".//span[contains(@id,'eventInfoSmall_lblOfferName')]").InnerText;

                                ticketType = ticketType.Replace("&nbsp;", "");

                                HtmlNodeCollection quantityRange = item.SelectNodes(".//select[contains(@id,'ddlQuantity')]//option");

                                if (quantityRange != null)
                                {
                                    minQty = int.Parse(quantityRange[0].Attributes["value"].Value);

                                    maxQty = int.Parse(quantityRange[quantityRange.Count - 1].Attributes["value"].Value);

                                    if (quantityRange.Count > 2)
                                    {
                                        qtyStep = int.Parse(quantityRange[quantityRange.Count - 1].Attributes["value"].Value) - int.Parse(quantityRange[quantityRange.Count - 2].Attributes["value"].Value);
                                    }
                                    else
                                    {
                                        qtyStep = minQty;
                                    }
                                    string DateName = item.SelectSingleNode(".//span[contains(@id,'lblDisplayDate')]").InnerText;
                                    string dateId = string.Empty;

                                    try
                                    {
                                        dateId = item.SelectSingleNode(".//div[contains(@class,'checkbox__container')]//input").Attributes["name"].Value;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                                    }


                                    HtmlNode quantityFormElementKey = item.SelectSingleNode(".//select[contains(@id,'ddlQuantity')]");


                                    VSTicketType typeTicket = new VSTicketType(ticketType, ticketType, minQty, maxQty, qtyStep, quantityFormElementKey.Attributes["name"].Value, DateName, dateId, ticketTitle);


                                    TicketTypes.Add(typeTicket);

                                    result = true;


                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                            }
                        }

                        HtmlNode _eventTarget = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//a[contains(@class,'btn-default-link link-blue flex-item-grow')]");

                        if (_eventTarget != null)
                        {
                            try
                            {
                                this.EventTarget = _eventTarget.Attributes["href"].Value;

                                this.EventTarget = this.EventTarget.Replace("javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(", "").Replace("&quot;", "").Replace(", , true, , , false, true))", "");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                            }

                        }

                        if (String.IsNullOrEmpty(this.Search._CurrentParameter.TicketTypeTitle))
                        {

                            if (String.IsNullOrEmpty(this.Search._CurrentParameter.TicketTypeString))
                            {
                                foreach (VSTicketType ticketType in TicketTypes)
                                {
                                    if (!String.IsNullOrEmpty(this._search._CurrentParameter.Date))
                                    {
                                        if (this.Search._CurrentParameter.ExactMatch ? ticketType.DateName.ToLower() == this.Search._CurrentParameter.Date.ToLower() : ticketType.DateName.ToLower().Contains(this.Search._CurrentParameter.Date.ToLower()))
                                        {
                                            this.TicketTypeMatched = true;
                                            ticketType.HasPriceLevel = this.parsePriceLevelAndDates(ticketType);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        this.TicketTypeMatched = true;
                                        ticketType.HasPriceLevel = this.parsePriceLevelAndDates(ticketType);
                                        break;
                                    }
                                }
                            }
                            else
                            {

                                foreach (VSTicketType ticketType in TicketTypes)
                                {
                                    // if (this.Search._CurrentParameter.ExactMatch ? ticketType.Title.ToLower() == this.Search._CurrentParameter.TicketTypeTitle.ToLower() : ticketType.Title.ToLower().Contains(this.Search._CurrentParameter.TicketTypeTitle.ToLower()))
                                    {
                                        if (this.Search._CurrentParameter.ExactMatch ? ticketType.Description.ToLower() == this.Search._CurrentParameter.TicketTypeString.ToLower() : ticketType.Description.ToLower().Contains(this.Search._CurrentParameter.TicketTypeString.ToLower()))
                                        {
                                            if (!String.IsNullOrEmpty(this._search._CurrentParameter.Date))
                                            {
                                                if (this.Search._CurrentParameter.ExactMatch ? ticketType.DateName.ToLower() == this.Search._CurrentParameter.Date.ToLower() : ticketType.DateName.ToLower().Contains(this.Search._CurrentParameter.Date.ToLower()))
                                                {
                                                    this.TicketTypeMatched = true;
                                                    ticketType.HasPriceLevel = this.parsePriceLevelAndDates(ticketType);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                this.TicketTypeMatched = true;
                                                ticketType.HasPriceLevel = this.parsePriceLevelAndDates(ticketType);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {


                            foreach (VSTicketType ticketType in TicketTypes)
                            {
                                if (this.Search._CurrentParameter.ExactMatch ? ticketType.Title.ToLower() == this.Search._CurrentParameter.TicketTypeTitle.ToLower() : ticketType.Title.ToLower().Contains(this.Search._CurrentParameter.TicketTypeTitle.ToLower()))
                                {

                                    if (!String.IsNullOrEmpty(this.Search._CurrentParameter.TicketTypeString))
                                    {
                                        if (this.Search._CurrentParameter.ExactMatch ? ticketType.Description.ToLower() == this.Search._CurrentParameter.TicketTypeString.ToLower() : ticketType.Description.ToLower().Contains(this.Search._CurrentParameter.TicketTypeString.ToLower()))
                                        {
                                            if (!String.IsNullOrEmpty(this._search._CurrentParameter.Date))
                                            {
                                                if (this.Search._CurrentParameter.ExactMatch ? ticketType.DateName.ToLower() == this.Search._CurrentParameter.Date.ToLower() : ticketType.DateName.ToLower().Contains(this.Search._CurrentParameter.Date.ToLower()))
                                                {
                                                    this.TicketTypeMatched = true;
                                                    ticketType.HasPriceLevel = this.parsePriceLevelAndDates(ticketType);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                this.TicketTypeMatched = true;
                                                ticketType.HasPriceLevel = this.parsePriceLevelAndDates(ticketType);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!String.IsNullOrEmpty(this._search._CurrentParameter.Date))
                                        {
                                            if (this.Search._CurrentParameter.ExactMatch ? ticketType.DateName.ToLower() == this.Search._CurrentParameter.Date.ToLower() : ticketType.DateName.ToLower().Contains(this.Search._CurrentParameter.Date.ToLower()))
                                            {
                                                this.TicketTypeMatched = true;
                                                ticketType.HasPriceLevel = this.parsePriceLevelAndDates(ticketType);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            this.TicketTypeMatched = true;
                                            ticketType.HasPriceLevel = this.parsePriceLevelAndDates(ticketType);
                                            break;
                                        }
                                    }
                                }
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            if (!result)
            {
                HtmlNode _eventTarget = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//a[contains(@class,'btn-default-link link-blue flex-item-grow')]");

                if (_eventTarget != null)
                {
                    try
                    {
                        this.EventTarget = _eventTarget.Attributes["href"].Value;

                        this.EventTarget = this.EventTarget.Replace("javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(", "").Replace("&quot;", "").Replace(", , true, , , false, true))", "");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }

                }

                String html = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class,'qtygrp__price-list-item')]").InnerHtml;
                HTML.LoadHtml(html);



                HtmlNodeCollection type = this.HTML.DocumentNode.SelectNodes("//div[@class='qtygrp__price-list-item-code']");

                if (type != null)
                {
                    foreach (HtmlNode item in type)
                    {
                        try
                        {

                            string ticketType = item.SelectSingleNode(".//span[@class='price-text bold']").InnerHtml;

                            minQty = int.Parse(item.SelectSingleNode(".//span[contains(@id,'lblMin')]").InnerHtml);

                            maxQty = int.Parse(item.SelectSingleNode(".//span[contains(@id,'lblMax')]").InnerHtml);

                            HtmlNodeCollection quantityRange = HTML.DocumentNode.SelectNodes(".//div[@class='qtygrp__price-list-item-qty']");

                            if (quantityRange != null)
                            {
                                foreach (HtmlNode node in quantityRange)
                                {
                                    HtmlNodeCollection gtyRange = quantityRange[0].SelectNodes("select/option");

                                    minQty = int.Parse(gtyRange[0].Attributes["value"].Value);
                                    if (minQty == 0)
                                    {
                                        minQty += 1;
                                    }
                                    maxQty = int.Parse(gtyRange[gtyRange.Count - 1].Attributes["value"].Value);
                                    qtyStep = int.Parse(gtyRange[1].Attributes["value"].Value) - int.Parse(gtyRange[0].Attributes["value"].Value);

                                    //if (String.IsNullOrEmpty(this.Search._CurrentParameter.TicketTypeString))
                                    {
                                        VSTicketType typeTicket = new VSTicketType(ticketType, ticketType, minQty, maxQty, qtyStep, quantityRange[0].SelectSingleNode("select").Attributes["name"].Value);


                                        TicketTypes.Add(typeTicket);

                                    }
                                }
                            }

                            result = true;

                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                        }
                    }
                    if (String.IsNullOrEmpty(this.Search._CurrentParameter.TicketTypeString))
                    {
                        this.TicketTypeMatched = true;
                        TicketTypes[0].HasPriceLevel = this.parsePriceLevel(TicketTypes[0]);
                    }
                    else
                    {
                        foreach (VSTicketType ticketType in TicketTypes)
                        {
                            if (this.Search._CurrentParameter.ExactMatch ? ticketType.Description.ToLower() == this.Search._CurrentParameter.TicketTypeString.ToLower() : ticketType.Description.ToLower().Contains(this.Search._CurrentParameter.TicketTypeString.ToLower()))
                            {
                                this.TicketTypeMatched = true;
                                ticketType.HasPriceLevel = this.parsePriceLevel(ticketType);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        private Boolean offerDdlQuantity()
        {
            Boolean result = false;
            String ticketType = String.Empty;

            HtmlNodeCollection type = this.HTML.DocumentNode.SelectNodes("//table[@class='pricecodebox']/tr/td[1]");

            if (type != null)
            {
                foreach (HtmlNode item in type)
                {
                    if (item.InnerHtml.Contains("<em>"))
                    {
                        ticketType = item.InnerHtml.Substring(0, item.InnerHtml.IndexOf("<em>")).Replace("&nbsp;", "").Replace("\n", "");

                        HtmlNodeCollection quantityRange = this.HTML.DocumentNode.SelectNodes("//select[contains(@id,'OfferQuantity')]/option");

                        //HtmlNodeCollection gtyRange = quantityRange[0].SelectNodes("option");

                        minQty = int.Parse(quantityRange[0].Attributes["value"].Value);
                        if (minQty == 0)
                        {
                            minQty += 1;
                        }

                        maxQty = int.Parse(quantityRange[quantityRange.Count - 1].Attributes["value"].Value);
                        qtyStep = int.Parse(quantityRange[1].Attributes["value"].Value) - int.Parse(quantityRange[0].Attributes["value"].Value);
                        //String node = this.HTML.DocumentNode.SelectSingleNode("//select[contains(@id,'OfferQuantity')]").Attributes["name"].Value;
                        VSTicketType typeTicket = new VSTicketType(ticketType, ticketType, minQty, maxQty, qtyStep, this.HTML.DocumentNode.SelectSingleNode("//select[contains(@id,'OfferQuantity')]").Attributes["name"].Value);
                        //this.parsePriceLevel(typeTicket);
                        TicketTypes.Add(typeTicket);
                    }
                }

                if (String.IsNullOrEmpty(this.Search._CurrentParameter.TicketTypeString))
                {
                    this.TicketTypeMatched = true;
                    TicketTypes[0].HasPriceLevel = this.parsePriceLevel(TicketTypes[0]);
                }
                else
                {
                    foreach (VSTicketType typeTicket in TicketTypes)
                    {
                        if (this.Search._CurrentParameter.ExactMatch ? typeTicket.Description.ToLower() == this.Search._CurrentParameter.TicketTypeString.ToLower() : typeTicket.Description.ToLower().Contains(this.Search._CurrentParameter.TicketTypeString.ToLower()))
                        {
                            this.TicketTypeMatched = true;
                            typeTicket.HasPriceLevel = this.parsePriceLevel(typeTicket);
                            break;
                        }
                    }
                }

            }
            else
            {
                HtmlNode _eventTarget = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//a[contains(@class,'btn-default-link link-blue flex-item-grow')]");

                if (_eventTarget != null)
                {
                    try
                    {
                        this.EventTarget = _eventTarget.Attributes["href"].Value;

                        this.EventTarget = this.EventTarget.Replace("javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(", "").Replace("&quot;", "").Replace(", , true, , , false, true))", "");

                        this.EventTarget = this.EventTarget.Replace("javascript:__doPostBack(", "");

                        this.EventTarget = this.EventTarget.Replace("&#39;", "");

                        this.EventTarget = this.EventTarget.Replace(",)", "");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                    }

                }

                if (this.session.FormElements.ContainsKey("m$c$ddlOfferQuantity"))
                {
                    this.session.FormElements["m$c$ddlOfferQuantity"] = this._search._CurrentParameter.Quantity.ToString();

                }
                else
                {
                    this.session.FormElements.Add("m$c$ddlOfferQuantity", this._search._CurrentParameter.Quantity.ToString());
                }

                this.session.FormElements.Remove("m$c$g$ctl01$qg$btnNBAFlex");

                AddUpdateField(this.session, "__EVENTTARGET", this.EventTarget);

                this.session.Post(this.session.HTMLWeb.ResponseUri.AbsoluteUri);

                HtmlNodeCollection _price = session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlPriceLevel')]/option");

                VSTicketType _ticketType = null;

                if (_price != null)
                {
                    this.TicketTypeMatched = true;
                    foreach (HtmlNode tmp in _price)
                    {
                        try
                        {
                            String desc = tmp.NextSibling.InnerHtml;
                            String[] name = desc.Split('-');
                            Decimal price = 0, minPrice = 0, maxPrice = 0;

                            //String priceRange = session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td").InnerText;

                            if (name.Length > 1)
                            {
                                price = Convert.ToDecimal(name[name.Length - 1].Split('$')[1]);
                                maxPrice = price;
                                minPrice = price;
                            }
                            else
                            {
                                HtmlNode minMaxNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//table[@class = 'pricerangebox3']/tr/td");
                                if (minMaxNode == null)
                                {
                                    minMaxNode = this.session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@class = 'pricerangebox3']/p");
                                }
                                if (minMaxNode != null)
                                {
                                    String[] minMax = minMaxNode.InnerText.Replace("\n", "").Trim().Split('$');
                                    minPrice = Convert.ToDecimal(minMax[1].Replace("Max", ""));
                                    maxPrice = Convert.ToDecimal(minMax[2]);
                                }
                            }

                            //foreach (String item in this.session.FormElements.Keys)
                            //{
                            //    if (item.Contains("ddlPriceLevel"))
                            //    {
                            //        this.session.FormElements[item] = tmp.Attributes["value"].Value;
                            //        break;
                            //    }
                            //}

                            _ticketType = new VSTicketType("", "", 1, 8, 1, "");
                            this.TicketTypes.Add(_ticketType);
                            VSPriceLevel _priceLevel = new VSPriceLevel(tmp.Attributes["value"].Value, name[0], desc, price, maxPrice, minPrice);
                            //getSectionByPriceLevel(_priceLevel);
                            _ticketType.PriceLevels.Add(_priceLevel);
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                        }
                    }
                    this.IsBestAvailable = true;

                    getPriceAndSections(_ticketType);
                    result = true;
                }

                else
                {
                    this.session.Post(this.session.HTMLWeb.ResponseUri.AbsoluteUri);

                    this.TicketTypes.Add(new VSTicketType("","",1,1,1,""));
                    this.IsBestAvailable = true;
                    this.TicketTypeMatched = true;
                    //this.TicketTypes.Add(new VSTicketType());
                    result = true;
                }

            }
            return result;
        }

        private Boolean txtQuantity()
        {
            Boolean result = false;
            String ticketType = String.Empty;

            HtmlNodeCollection type = this.HTML.DocumentNode.SelectNodes("//table[@class='pricecodebox']/tr");

            if (type != null)
            {
                foreach (HtmlNode item in type)
                {
                    if (item.InnerHtml.Contains("<em>"))
                    {
                        //ticketType = item.InnerHtml.Substring(0, item.InnerHtml.IndexOf("<i>")).Replace("&nbsp;", "").Replace("\n", "");

                        ticketType = item.SelectSingleNode("td[1]").InnerHtml.Substring(0, item.SelectSingleNode("td[1]").InnerHtml.IndexOf("<em>")).Replace("&nbsp;", "").Replace("\n", "");
                        minQty = int.Parse(item.SelectSingleNode("td[1]/i/span[contains(@id,'Min')]").InnerText);
                        maxQty = int.Parse(item.SelectSingleNode("td[1]/i/span[contains(@id,'Max')]").InnerText);

                        VSTicketType typeTicket = new VSTicketType(ticketType, ticketType, minQty, maxQty, qtyStep, item.SelectSingleNode("td[2]/input").Attributes["name"].Value);
                        //this.parsePriceLevel(typeTicket);
                        TicketTypes.Add(typeTicket);

                        result = true;
                    }
                }

                if (String.IsNullOrEmpty(this.Search._CurrentParameter.TicketTypeString))
                {
                    this.TicketTypeMatched = true;
                    TicketTypes[0].HasPriceLevel = this.parsePriceLevel(TicketTypes[0]);
                }
                else
                {
                    foreach (VSTicketType typeTicket in TicketTypes)
                    {
                        if (this.Search._CurrentParameter.ExactMatch ? typeTicket.Description.ToLower() == this.Search._CurrentParameter.TicketTypeString.ToLower() : typeTicket.Description.ToLower().Contains(this.Search._CurrentParameter.TicketTypeString.ToLower()))
                        {
                            this.TicketTypeMatched = true;
                            typeTicket.HasPriceLevel = this.parsePriceLevel(typeTicket);
                            break;
                        }
                    }
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        private void getSectionByPriceLevel(VSPriceLevel _priceLevel)
        {
            this.session.FormElements.Remove("mc:btnChangeQty");
            this.session.FormElements.Remove("m:c:btnReserve");
            this.session.FormElements.Remove("m$c$g$ctl01$prg$btnFindTicketsFlex");
            this.session.FormElements.Remove("m$c$g$ctl01$prg$p$ctl01$btnFindTickets");
            this.session.FormElements.Remove("m:c:btnChangeQty");

            AddUpdateField(session, "__EVENTTARGET", "m$c$g$ctl01$prg$p$ctl01$ddlPriceLevel");

            VSPriceLevel priceLevel = _priceLevel;

            {
                this.session.Post(session.HTMLWeb.ResponseUri.AbsoluteUri);
            }

            if (!session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("pricerange"))
            {
                this.session.Post(session.HTMLWeb.ResponseUri.AbsoluteUri);
            }
            HtmlNodeCollection _options = session.HtmlDocument.DocumentNode.SelectNodes("//select[contains(@id,'ddlSections')]/option");

            if (_options != null)
            {
                foreach (HtmlNode tmp in _options)
                {
                    _priceLevel.Section.Add(new VSSection(tmp.Attributes["value"].Value, tmp.NextSibling.InnerHtml));
                }
            }
        }


        public void parsePriceLevels()
        {
            BrowserSession session = new BrowserSession();
            try
            {

            }
            catch (Exception ex)
            {
            }
        }

        public void AddUpdateField(BrowserSession b, string key, string value)
        {
            if (!b.FormElements.ContainsKey(key))
            {
                b.FormElements.Add(key, value);
            }
            else
            {
                b.FormElements[key] = value;
            }
        }

        public void Dispose()
        {
            try
            {
                if (this.TicketTypes != null)
                {
                    this.TicketTypes.Clear();
                    this.TicketTypes = null;
                }

                if (this.Sections != null)
                {
                    this.Sections.Clear();
                    this.Sections = null;
                }

                if (this.Locations != null)
                {
                    this.Locations.Clear();
                    this.Locations = null;
                }

                if (this.Additionals != null)
                {
                    this.Additionals.Clear();
                    this.Additionals = null;
                }

                this.HTML = null;

                GC.SuppressFinalize(this);
                GC.Collect();
            }
            catch (Exception)
            {

            }
        }
    }
}

