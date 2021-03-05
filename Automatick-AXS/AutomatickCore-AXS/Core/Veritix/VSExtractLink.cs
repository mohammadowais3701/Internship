using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automatick.Core
{
    public class VSExtractLink
    {
        #region Variables

        public BrowserSession session = null;
        Dictionary<String, String> dicOfTicket = new Dictionary<String, String>();
        
        String _link;
        bool isShow = false;

        #endregion

        public VSExtractLink()
        {
            try
            {
                session = new BrowserSession();
            }
            catch
            { }
        }

        public Dictionary<String, String> LinkExtract(object s)
        {

            try
            {
                String url = (String)s;
                if (!url.Contains("http://") && !url.Contains("https://"))
                {
                    url = "http://" + url;
                }

                String response = session.Get(url);

                //if (session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("theqarena.com/events/"))
                //{
                //    if (!ExtractQareneLink(session))
                //    { MessageBox.Show("Link not found"); }
                //}
                if (session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("houstontoyotacenter.com/events/"))
                {
                    if (!ExtractHoustonToyota(session))
                    { 
                        //System.Windows.Forms.MessageBox.Show("Link not found"); 
                    }

                }
                else if (session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("tickethorse.com/event/"))
                {
                    if (!ExtractTicketHorse(session))// && !this.frmAddTick.ifStopCall)
                    {
                        //System.Windows.Forms.MessageBox.Show("Link not found"); 
                    }
                }
                else if (session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("smithstix.com/events/item/root/"))
                {
                    if (!ExtractSmithstix(session))
                    {
                        //System.Windows.Forms.MessageBox.Show("Link not found"); 
                    }
                }
                else if (session.HTMLWeb.ResponseUri.AbsoluteUri.ToLower().Contains("boisestatetickets.com/event/"))
                {
                    if (!ExtractBoiseStateTickets(session))
                    {
                        //MessageBox.Show("Link not found");
                    }
                }
                else
                {
                    //MessageBox.Show("Invaild Url");
                }
            }
            catch
            {

            }
            finally
            {

            }
            return dicOfTicket;
        }

        public String getLink(String url)
        {
            String Link = String.Empty;
            String response = this.session.Get(url);

            if (this.session.LocationURL.ToLower().Contains("ticketingcentral"))
            {
                Link = this.session.LocationURL;
            }
            else
            {
                Link = url;
            }
            return Link;

        }

        #region HoustonToyota

        public bool ExtractHoustonToyota(BrowserSession session)
        {
            try
            {
                String name = session.HtmlDocument.DocumentNode.SelectSingleNode("//span[@class='date']").InnerText + session.HtmlDocument.DocumentNode.SelectSingleNode("//span[@class='time']").InnerText;

                HtmlNode node = session.HtmlDocument.DocumentNode.SelectSingleNode("//a[@class='tickets']");
                if (node != null)
                {
                    this.session.HTMLWeb.IfAllowAutoRedirect = false;
                    this.session.Get(node.Attributes["href"].Value);
                    this.session.HTMLWeb.IfAllowAutoRedirect = true;
                    if (!String.IsNullOrEmpty(this.session.LocationURL))
                    {
                        dicOfTicket.Add(name, this.session.LocationURL);
                    }
                    else
                    {
                        dicOfTicket.Add(name, node.Attributes["href"].Value);
                    }
                }

                HtmlNodeCollection multiplelinks = session.HtmlDocument.DocumentNode.SelectNodes("//div[@class='description']//h3");
                if (multiplelinks != null)
                {
                    name = String.Empty;
                    foreach (HtmlNode subNode in multiplelinks)
                    {
                        HtmlNode anchor = subNode.SelectSingleNode("a");
                        name = anchor.InnerText;
                        this.session.HTMLWeb.IfAllowAutoRedirect = false;
                        this.session.Get(anchor.Attributes["href"].Value);
                        this.session.HTMLWeb.IfAllowAutoRedirect = true;
                        if (!String.IsNullOrEmpty(this.session.LocationURL))
                        {
                            dicOfTicket.Add(name, this.session.LocationURL);
                        }
                        else
                        {
                            dicOfTicket.Add(name, anchor.Attributes["href"].Value);
                        }
                    }

                }

                if (session.HtmlDocument.DocumentNode.SelectNodes("//p[@class='buytickets']") != null)
                {
                    ExtractAllAnchors(session);
                }
                #region dont delete

                ////else
                ////{

                ////    HtmlNodeCollection multiLink = session.HtmlDocument.DocumentNode.SelectNodes("//span[@class='specialoffer']");
                ////    if (multiLink != null)
                ////        foreach (HtmlNode multitem in multiLink)
                ////        {
                ////            HtmlNode item = multitem.SelectSingleNode("div");
                ////            if (item.SelectSingleNode("p/em") != null)
                ////            {
                ////                key = removeChars(item.SelectSingleNode("p/em").InnerText);
                ////            }
                ////            else
                ////            {
                ////                key = removeChars(item.SelectSingleNode("p").InnerText);
                ////            }
                ////            HtmlNodeCollection anchorList = item.SelectNodes("p/a");
                ////            if ((anchorList == null) || (key.Contains("American Express")))  //only in the case of american express b/c of </p> end tag is missing in original html
                ////            {
                ////                anchorList = item.SelectNodes("a");//("a")
                ////                if (item.SelectNodes("p/strong/a") != null)
                ////                {
                ////                    anchorList = item.SelectNodes("p/strong/a");
                ////                }
                ////                if (key.Contains("American Express"))
                ////                {
                ////                    ExtractAmerican(session);
                ////                    anchorList = null;
                ////                }
                ////                key = removeChars(multitem.SelectSingleNode("a/span[@class='style2']").InnerText);   //for creating key
                ////            }
                ////            if (anchorList != null)
                ////                if (anchorList.Count == 1)
                ////                {
                ////                    link = anchorList[0].Attributes["href"].Value.Replace("&amp;", "&");
                ////                    if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                ////                    {
                ////                        if (!dicTypeOfTicket.ContainsKey(key))
                ////                            dicTypeOfTicket.Add(key, link);
                ////                    }

                ////                }
                ////                else if (anchorList.Count > 1)
                ////                {
                ////                    foreach (HtmlNode anchor in anchorList)
                ////                    {
                ////                        if (!anchor.InnerText.Contains("Click Here for Tickets"))
                ////                            key = anchor.InnerText;
                ////                        link = anchor.Attributes["href"].Value.Replace("&amp;", "&");
                ////                        if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                ////                        {
                ////                            if (!key.Contains("Accessible"))
                ////                                if (!dicTypeOfTicket.ContainsKey(key))
                ////                                    dicTypeOfTicket.Add(key, link);
                ////                        }

                ////                    }

                ////                }
                ////        }

                ////    HtmlNode singleLink = session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@id='content']//font//a");//div[@id='content']//font//a");//p[@class='buytickets']
                ////    #region rough
                ////    // HtmlNodeCollection singleLink = session.HtmlDocument.DocumentNode.SelectNodes("//div[@id='content']//font//a");
                ////    //foreach (HtmlNode item in singleLink)
                ////    //{
                ////    //   

                ////    //key = item.InnerText.Replace("Click Here", "").Replace("-", "");
                ////    #endregion
                ////    key = "Admissions";
                ////    link = singleLink.Attributes["href"].Value.Replace("&amp;", "&");  //for multi-link
                ////    if (dicTypeOfTicket.Count < 1)
                ////    {
                ////        Link = link;
                ////        return true;
                ////    }
                ////    else if (dicTypeOfTicket.Count >= 1)
                ////    {
                ////        if (!dicTypeOfTicket.ContainsKey(key))  // events not longer onsale 
                ////            dicTypeOfTicket.Add(key, link);

                ////    }
                ////}
                #endregion
                
            }
            catch
            { }
            return false;
        }

        public void ExtractAllAnchors(BrowserSession session)
        {

            String key = "", link = "";
            String strAmer = "American Express";
            int count = 1;
            String pref = "Preferred Seating";
            String Seat = "Seating";
            bool IsAmericanExpress = false;
            bool title = false;
            String strtitle = "";
            HtmlNodeCollection p = session.HtmlDocument.DocumentNode.SelectNodes("//div[@class='maincontent']//font//a");
            foreach (HtmlNode a in p)
            {
                key = ""; link = "";
                if ((a.Attributes["href"].Value != null))
                {
                    key = removeChars(a.InnerText);
                    if (key.Contains(strAmer)) IsAmericanExpress = true;
                    link = a.Attributes["href"].Value.Replace("&amp;", "&");
                    if (link.Contains('#'))
                    {
                        title = true;
                        strtitle = key;
                    }
                }
                if (IsAmericanExpress && key.Contains("Click Here")) // because of ending tags missing in original html
                {
                    if (count <= 1)
                    {
                        key = strAmer + " " + pref; count++;
                    }
                    else if (count > 1)
                    {
                        key = strAmer + " " + Seat;
                        count = 1;
                        IsAmericanExpress = false;
                    }

                }
                else if (key.Contains("Click Here") && (title))
                {
                    key = strtitle;
                    title = false;
                }

                if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                {
                    if (!key.Contains("more information"))
                        if (!dicOfTicket.ContainsKey(key) && (link != "#") && (!link.Contains("flashseats")))  // events not longer onsale 

                            dicOfTicket.Add(key, link);
                }

            }



            //handheldhid[0].NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.Attributes["href"].Value
            //String eliminate = text.Substring(0, text.IndexOf("</div>"));
            //text = text.Replace(eliminate, "");


        }

        public bool ExtractAmerican(BrowserSession session)
        {
            try
            {
                String key = "", link = "";
                HtmlNodeCollection multiLink = session.HtmlDocument.DocumentNode.SelectNodes("//span[@class='specialoffer']");
                int n = multiLink.Count;
                HtmlNode item = multiLink[n - 1];

                HtmlNodeCollection p = item.SelectNodes("div/p");
                HtmlNodeCollection p3 = item.SelectNodes("div/p[3]");
                //item = item.SelectSingleNode("div");
                //HtmlNodeCollection p2 = item.SelectNodes("p");
                key = removeChars(p[1].InnerText);
                //if (p[2].SelectSingleNode("img/a")!=null)
                //link = p[2].SelectSingleNode("a").Attributes["href"].Value;
                // HtmlNodeCollection anchorList = item.SelectNodes("p/a");
                foreach (HtmlNode a in p)
                {

                    if (a.SelectSingleNode("a") != null)
                    {
                        //do something here

                        link = a.SelectSingleNode("a").Attributes["href"].Value.Replace("&amp;", "&");

                    }
                    if (key == "")
                        key = removeChars(a.InnerText);
                    if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                    {
                        if (!dicOfTicket.ContainsKey(key))
                        {
                            dicOfTicket.Add(key, link);
                            key = "";
                        }
                    }
                }
                return true;
            }
            catch { }

            return false;
        }

        #endregion

        #region TicketHorse

        public bool ExtractTicketHorse(BrowserSession session)
        {
            String link = "";
            String key = "";
            HtmlNodeCollection multilink = session.HtmlDocument.DocumentNode.SelectNodes("//div[@id='event_middle']/ul");
            do
            {
                if (multilink != null)
                {
                    foreach (HtmlNode multitem in multilink)
                    {
                        HtmlNodeCollection li = multitem.SelectNodes("li");//liBuyUrl
                        foreach (HtmlNode litem in li)
                        {
                            if (litem.Attributes.Contains("id"))
                            {
                                if (litem.Attributes["id"].Value == "Name")
                                {
                                    key = litem.InnerText;
                                }
                                if (litem.Attributes["id"].Value.Contains("liBuyUrl"))
                                {
                                    link = litem.FirstChild.Attributes["href"].Value;
                                }
                                if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                                {
                                    if (!dicOfTicket.ContainsKey(key))
                                        dicOfTicket.Add(key, link);
                                }
                            }
                        }
                    }
                }
                HtmlNodeCollection singleLink = session.HtmlDocument.DocumentNode.SelectNodes("//div[@id='eventInfoTop']/ul/li");
                foreach (HtmlNode li in singleLink)
                {
                    if (li.Attributes.Contains("id"))
                    {
                        String id = li.Attributes["id"].Value;
                        if (String.IsNullOrEmpty(li.FirstChild.InnerText))
                            key = li.FirstChild.InnerText;
                        if (id.Contains("liBuyTix"))
                        {
                            link = li.FirstChild.Attributes["href"].Value;
                            key = session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@id='eventInfoTop']/ul/li[@class='buyOption1']").InnerText;

                            if (dicOfTicket.Count == 0)
                            {
                                _link = link;
                                return true;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                                {
                                    if (!dicOfTicket.ContainsKey(key))
                                        dicOfTicket.Add(key, link);
                                }
                            }
                        }
                    }
                }
                if (dicOfTicket.Count < 1)
                {
                    //break;
                }
            }
            while (dicOfTicket.Count < 1 );// && !frmAddTick.ifStopCall);
            if (dicOfTicket.Count == 1)
            {
                foreach (KeyValuePair<String, String> value in dicOfTicket)
                {
                    _link = value.Value; return true; 
                }
            }
            if (dicOfTicket.Count > 1)
            {
                return true;
                //this.frmAddTick.ifExtractLink = true;            //---from save & start, multiple links exist
            }
            //else
            //{
            //    this.frmAddTick.ifExtractLink = false;
            //}
            //if (!this.frmAddTick.ifStopCall)
            //{
            //    if (this.frmAddTick.ifExtractLink)
            //    {
            //        if (bindwithCombo())
            //        {
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        this.frmAddTick.ifExtractLink = false;      //----from save & start,do not open Find window
            //        MessageBox.Show("Kindly use Extract Parameter for this link");
            //        return true;
            //    }
            //}
            return false;
        }

        #endregion

        #region smithstix

        public bool ExtractSmithstix(BrowserSession session)
        {
            try
            {
                String link = "";
                String key = "";
                HtmlNode anchor;
                HtmlNodeCollection multilink = session.HtmlDocument.DocumentNode.SelectNodes("//div[@class='offer']");
                if (multilink != null)
                    foreach (HtmlNode multitem in multilink)
                    {
                        HtmlNodeCollection anchorList = multitem.SelectNodes("a");
                        HtmlNode item = anchorList[1];

                        key = item.SelectSingleNode("h3").InnerText;

                        if (!String.IsNullOrEmpty(key))
                        {
                            link = item.Attributes["href"].Value;
                            if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                            {
                                if (!dicOfTicket.ContainsKey(key))
                                    dicOfTicket.Add(key, link);
                            }
                        }

                    }

                HtmlNode singlelink = session.HtmlDocument.DocumentNode.SelectSingleNode("//div[@class='sale_buttons']");
                if (singlelink != null)
                {
                    anchor = singlelink.SelectSingleNode("a");
                    if (anchor.Attributes.Contains("href"))
                    {
                        link = anchor.Attributes["href"].Value;
                        if (dicOfTicket.Count == 0)
                        {
                            if (String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                            {
                                // if (!dicTypeOfTicket.ContainsKey(key))
                                dicOfTicket.Add("GA", link);
                            }
                            return true;
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                            {
                                // if (!dicTypeOfTicket.ContainsKey(key))
                                dicOfTicket.Add("GA", link);
                            }
                        }
                    }
                }
            }
            catch
            { }
            return false;
        }

        #endregion

        #region boisestatetickets

        public bool ExtractBoiseStateTickets(BrowserSession session)
        {
            try
            {
                string link = "";
                string key = "";
                //HtmlNode anchor;
                HtmlNodeCollection singleLink = session.HtmlDocument.DocumentNode.SelectNodes("//div[@class='related-events']/ul[@class='related-events']/li");
                if(singleLink != null)
                {
                foreach (HtmlNode li in singleLink)
                {
                    HtmlNodeCollection nodes = li.SelectNodes("div");

                    foreach (HtmlNode item in nodes)
                    {
                        if (item.Attributes["class"].Value.ToLower().Contains("buy-now"))
                        {
                            link = item.SelectSingleNode("a").Attributes["href"].Value;
                            if (!link.ToLower().Contains("ticketingcentral"))
                            {
                                link = String.Empty;
                            }
                        }
                        else if (item.Attributes["class"].Value.ToLower().Contains("related-event-text"))
                        {
                            String name = item.SelectSingleNode("p[@class='related-event-title']").InnerText;
                            String date = item.SelectSingleNode("p[2]").InnerText;
                            key = name + " --- " + date;
                        }

                        
                    }

                    if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                    {
                        if (!dicOfTicket.ContainsKey(key))
                            dicOfTicket.Add(key, link);
                    }
                    else if (String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                    {
                        if (!dicOfTicket.ContainsKey(key))
                            dicOfTicket.Add("GA", link);
                    }
                }
            }
                else
                {
                    HtmlNodeCollection buyLink = session.HtmlDocument.DocumentNode.SelectNodes("//div[@class = 'event_bottom']/div");
                    if (buyLink != null)
                    {
                        if (buyLink[1].SelectSingleNode("a").Attributes["class"].Value.ToLower().Contains("orange"))
                        {
                            link = buyLink[1].SelectSingleNode("a").Attributes["href"].Value;
                            if (!link.ToLower().Contains("ticketingcentral"))
                            {
                                link = String.Empty;
                            }
                        }
                    }
                    if (String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(link))
                    {
                        if (!dicOfTicket.ContainsKey(key))
                            dicOfTicket.Add("GA", link);
                    }
                }
                if (dicOfTicket.Count > 0)
                {
                    return true;
                }
            }
            catch { }
            return false;

        }

        #endregion


        #region General functions


        public String removeChars(String input)
        {
            input = input.Trim().Replace(";", "");
            input = input.Replace('\r', ' ').Replace('\n', ' ');
            input = input.Replace("\t", "");
            input = input.Replace(",", "");
            input = input.Replace("&nbsp;", "");
            input = input.Replace("&amp;", "&");
            input = input.Replace("&reg", " ");
            input = input.Replace(":", "");
            input = input.Replace("&#", " ");
            // input = input.Replace("&rsaquo;", "›");

            return input;
        }
        #endregion
    }
}
