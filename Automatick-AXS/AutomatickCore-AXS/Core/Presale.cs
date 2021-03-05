using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using System.Threading;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Automatick.Core
{
    [Serializable]
    public class Presale
    {

        #region Members
        private AXSSearch _search;
        private string wr = string.Empty;
        private string lotId = string.Empty;
        private string wrNew = string.Empty;
        public Boolean ifAvailable = true;

        #endregion

        #region Properties
        public string Wr
        {
            get { return wr; }
            set { wr = value; }
        }

        public string LotId
        {
            get { return lotId; }
            set { lotId = value; }
        }

        public AXSSearch search
        {
            get { return _search; }
            set { _search = value; }
        }
        #endregion

        #region Constructors
        public Presale(AXSSearch search)
        {
            this.search = search;
        }

        public Presale(AXSSearch search, string wRoom)
        {
            this.search = search;
            this.wrNew = wRoom;
        }
        #endregion

        #region Methods
        private Boolean enterWaitingRoom(AXSSearch search)
        {
            bool ifValidated = false;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            if (string.IsNullOrEmpty(this.wrNew))
            {
                this.wr = HttpUtility.ParseQueryString((new Uri(search.Ticket.URL)).Query).Get("wr");

                if (string.IsNullOrEmpty(wr))
                {
                    if (search.Ticket.URL.Contains("/shop/") || search.Ticket.URL.Contains("/#/"))
                    {
                        String w = search.Ticket.URL.Substring(search.Ticket.URL.IndexOf("#") + 2);
                        string[] split = w.Split('/');
                        if (split[0].Contains("?"))
                        {
                            split = split[0].Split('?');
                            wr = split[0];
                        }
                        else
                        {
                            wr = split[0];
                        }
                    }
                }

                if (string.IsNullOrEmpty(this.wr))
                {
                    String[] query = search.Ticket.URL.Substring(search.Ticket.URL.IndexOf('?') + 1).Split('&');

                    foreach (var item in query)
                    {
                        if (item.Split('=')[0].ToLower().Equals("wr"))
                        {
                            wr = item.Split('=')[1];
                            break;
                        }
                    }
                }
            }
            else
                this.wr = this.wrNew;

            this.wr = this.wrNew;

            try
            {
                doc.LoadHtml(post(search, "https://" + new Uri(this.search.Ticket.URL).Host + "/xmlrpc?methodName=enternl", String.Format("<methodCall><methodName>enternl</methodName><params><param><value><string>{0}</string></value></param></params></methodCall>", this.wr), "application/x-www-form-urlencoded"));
                HtmlNode lotIdNode = doc.DocumentNode.SelectSingleNode("//name[text() = 'lotId']");

                if (lotIdNode != null)
                {
                    this.lotId = lotIdNode.NextSibling.ChildNodes[0].InnerHtml;
                    ifValidated = true;
                }
            }
            catch (Exception)
            {
                ifValidated = false;
            }

            return ifValidated;
        }

        private Boolean WaitForTurn(AXSSearch search, string wr, string lotId)
        {
            bool ifValidated = false;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            //if (search.StateWaiting == true)
            {
                try
                {
                    if (string.IsNullOrEmpty(this.wrNew))
                    {
                        wr = HttpUtility.ParseQueryString((new Uri(search.Ticket.URL)).Query).Get("wr");

                        if (string.IsNullOrEmpty(wr))
                        {
                            if (search.Ticket.URL.Contains("/shop/") || search.Ticket.URL.Contains("/#/"))
                            {
                                String w = search.Ticket.URL.Substring(search.Ticket.URL.IndexOf("#") + 2);
                                string[] split = w.Split('/');
                                if (split[0].Contains("?"))
                                {
                                    split = split[0].Split('?');
                                    wr = split[0];
                                }
                                else
                                {
                                    wr = split[0];
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(wr))
                        {
                            String[] query = search.Ticket.URL.Substring(search.Ticket.URL.IndexOf('?') + 1).Split('&');

                            foreach (var item in query)
                            {
                                if (item.Split('=')[0].ToLower().Equals("wr"))
                                {
                                    wr = item.Split('=')[1];
                                    break;
                                }
                            }
                        }
                    }
                    else
                        wr = this.wrNew;

                    this.Wr = wr;

                    doc.LoadHtml(post(search, String.Format("https://" + new Uri(this.search.Ticket.URL).Host + "/xmlrpc?methodName=getPhase&lotID={0}&wr={1}", this.lotId, this.wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>{1}</string></value></param></params></methodCall>", this.wr, this.lotId), "text/xml"));
                    HtmlNode enterPauseNode = doc.DocumentNode.SelectSingleNode("//name[text() = 'enterPause']");
                    HtmlNode comeBackInNode = doc.DocumentNode.SelectSingleNode("//name[text() = 'comeBackIn']");

                    HtmlNodeCollection nodehash = doc.DocumentNode.SelectNodes("//member/name");
                    Boolean checkHash = false;
                    if (nodehash != null)
                    {
                        foreach (HtmlNode item in nodehash)
                        {
                            if (item.InnerHtml.Contains("hashts"))
                            {
                                checkHash = true;
                                if (item.NextSibling.FirstChild == null)
                                {
                                    search.HashTS = item.NextSibling.NextSibling.FirstChild.InnerText.Trim();
                                }
                                else
                                {
                                    search.HashTS = item.NextSibling.FirstChild.InnerText.Trim();
                                }
                            }
                            else if (item.InnerHtml.Contains("hash"))
                            {
                                checkHash = true;

                                if (!GoodProxies._goodProxyList.Contains(search.Proxy))
                                {
                                    GoodProxies._goodProxyList.Add(search.Proxy);
                                }

                                if (search.Proxy != null)
                                {
                                    search.Proxy.ProxySortOrder = Proxy.ProxyPriority.FirstPage;
                                }

                                if (item.NextSibling.FirstChild == null)
                                {
                                    search.Hash = item.NextSibling.NextSibling.FirstChild.InnerText.Trim();
                                }
                                else
                                {
                                    search.Hash = item.NextSibling.FirstChild.InnerText.Trim();
                                }
                            }
                            else if (item.InnerHtml.Contains("recaptcha"))
                            {
                                if (item.NextSibling.FirstChild == null)
                                {
                                    search.Recaptcha = item.NextSibling.NextSibling.FirstChild.InnerText.Trim();
                                }
                                else
                                {
                                    search.Recaptcha = item.NextSibling.FirstChild.InnerText.Trim();
                                }
                            }
                        }
                    }

                    if ((enterPauseNode != null) && (comeBackInNode != null))
                    {
                        this.search.changeStatus(TicketSearchStatus.WaitingRoomStatus);

                        int enterPause = 0;
                        int comeBackIn = 0;

                        if (enterPauseNode.NextSibling.ChildNodes.Count > 0)
                        {
                            enterPause = Convert.ToInt32(enterPauseNode.NextSibling.ChildNodes[0].InnerHtml);
                            comeBackIn = Convert.ToInt32(comeBackInNode.NextSibling.ChildNodes[0].InnerHtml);
                        }
                        else
                        {
                            enterPause = Convert.ToInt32(enterPauseNode.NextSibling.NextSibling.ChildNodes[0].InnerHtml);
                            comeBackIn = Convert.ToInt32(comeBackInNode.NextSibling.NextSibling.ChildNodes[0].InnerHtml);
                        }
                        if (enterPause < 60)
                        {
                            search.StateWaiting = false;
                            enterPause *= 1000;
                            //if (!search.Ticket.Permission)
                            if (!checkHash)
                            {
                                //Thread.Sleep(enterPause);
                                //using (AutoResetEvent sleep = new AutoResetEvent(false))
                                //{
                                //    sleep.WaitOne(enterPause);
                                //}

                                //this.search.Ticket.ReleaseLotId(lotId);
                                ifAvailable = false;
                                return false;
                                //WaitForTurn(search, this.wr, this.lotId);
                            }

                        }
                        else if (comeBackIn < 60)
                        {
                            search.StateWaiting = false;
                            comeBackIn *= 1000;
                            //if (!search.Ticket.Permission)
                            if (!checkHash)
                            {
                                //Thread.Sleep(comeBackIn);
                                //using (AutoResetEvent sleep = new AutoResetEvent(false))
                                //{
                                //    sleep.WaitOne(comeBackIn);
                                //}

                                //this.search.Ticket.ReleaseLotId(lotId);
                                ifAvailable = false;
                                return false;
                                //WaitForTurn(search, this.wr, this.lotId);
                            }
                        }
                        else
                        {
                            enterPause *= 1000;
                            search.StateWaiting = true;
                            //if (!search.Ticket.Permission)
                            if (!checkHash)
                            {
                                //Thread.Sleep(enterPause);
                                //using (AutoResetEvent sleep = new AutoResetEvent(false))
                                //{
                                //    sleep.WaitOne(enterPause);
                                //}

                                //this.search.Ticket.ReleaseLotId(lotId);
                                ifAvailable = false;
                                return false;
                                //WaitForTurn(search, this.wr, this.lotId);
                            }

                        }

                        this.search.Ticket.SaveLotId(lotId);
                        ifValidated = true;
                    }
                    else if (checkHash)
                    {
                        ifValidated = true;
                    }
                    else
                    {
                        ifValidated = false;
                    }
                }
                catch (Exception)
                {
                    ifValidated = false;
                }

            }
            //else
            //{
            //    return true;
            //}
            return ifValidated;
        }

        private Boolean processWaitingRoom(AXSSearch search)
        {
            bool ifValidated = false;
            string wr = string.Empty;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            try
            {
                if (string.IsNullOrEmpty(this.wrNew))
                {
                    wr = HttpUtility.ParseQueryString((new Uri(search.Ticket.URL)).Query).Get("wr");

                    if (string.IsNullOrEmpty(wr))
                    {
                        if (search.Ticket.URL.Contains("EVENKO#"))
                        {
                            String w = search.Ticket.URL.Substring(search.Ticket.URL.IndexOf("#") + 1);
                            string[] split = w.Split('/');
                            if (split[0].Contains("?"))
                            {
                                split = split[0].Split('?');
                                wr = split[0];
                            }
                            else
                            {
                                wr = split[0];
                            }
                        }
                        else if (search.Ticket.URL.Contains("/shop/") || search.Ticket.URL.Contains("/#/"))
                        {
                            String w = search.Ticket.URL.Substring(search.Ticket.URL.IndexOf("#") + 2);
                            string[] split = w.Split('/');
                            if (split[0].Contains("?"))
                            {
                                split = split[0].Split('?');
                                wr = split[0];
                            }
                            else
                            {
                                wr = split[0];
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(wr))
                    {
                        String[] query = search.Ticket.URL.Substring(search.Ticket.URL.IndexOf('?') + 1).Split('&');

                        foreach (var item in query)
                        {
                            if (item.Split('=')[0].ToLower().Equals("wr"))
                            {
                                wr = item.Split('=')[1];
                                break;
                            }
                        }
                    }
                }
                else
                    wr = this.wrNew;

                this.Wr = wr;

                if (!search.Ticket.URL.Contains("/shop/") && !search.Ticket.URL.Contains("/#/"))
                {
                    if (search.isWeb || search.isEventko)
                    {
                        doc.LoadHtml(post(search, String.Format("https://" + new Uri(this.search.Ticket.URL).Host + "/xmlrpc?methodName=getPhase&lotID=noLotId_7&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>noLotId_7</string></value></param></params></methodCall>", wr), "text/xml"));
                    }
                    else
                    {
                        doc.LoadHtml(post(search, String.Format("https://" + new Uri(this.search.Ticket.URL).Host + "/xmlrpc?methodName=getPhase&lotId=NoLotId&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>NoLotId</string></value></param></params></methodCall>", wr), "application/x-www-form-urlencoded; charset=UTF-8"));
                    }
                }
                else
                {
                    doc.LoadHtml(post(search, String.Format("https://" + new Uri(this.search.Ticket.URL).Host + "/xmlrpc?methodName=getPhase&lotID=noLotId&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>noLotId</string></value></param></params></methodCall>", wr), "text/xml"));//"application/x-www-form-urlencoded"));
                }

                HtmlNode enterPauseNode = doc.DocumentNode.SelectSingleNode("//name[text() = 'enterPause']");

                HtmlNodeCollection nodehash = doc.DocumentNode.SelectNodes("//member/name");
                if (nodehash != null)
                {
                    foreach (HtmlNode item in nodehash)
                    {
                        if (item.InnerHtml.Contains("hashts"))
                        {
                            if (!GoodProxies._goodProxyList.Contains(search.Proxy))
                            {
                                GoodProxies._goodProxyList.Add(search.Proxy);
                            }

                            if (search.Proxy != null)
                            {
                                search.Proxy.ProxySortOrder = Proxy.ProxyPriority.FirstPage;
                            }

                            if (item.NextSibling.FirstChild != null)
                            {
                                search.HashTS = item.NextSibling.FirstChild.InnerText.Trim();
                            }
                            else
                            {
                                search.HashTS = item.NextSibling.NextSibling.FirstChild.InnerText.Trim();
                            }
                        }
                        else if (item.InnerHtml.Contains("hash"))
                        {
                            if (item.NextSibling.FirstChild != null)
                            {
                                search.Hash = item.NextSibling.FirstChild.InnerText.Trim();
                            }
                            else
                            {
                                search.Hash = item.NextSibling.NextSibling.FirstChild.InnerText.Trim();
                            }
                        }
                        else if (item.InnerHtml.Contains("recaptcha"))
                        {
                            if (item.NextSibling.FirstChild != null)
                            {
                                search.Recaptcha = item.NextSibling.FirstChild.InnerText.Trim();
                            }
                            else
                            {
                                search.Recaptcha = item.NextSibling.NextSibling.FirstChild.InnerText.Trim();
                            }
                        }
                        else if (item.InnerHtml.Contains("info_path"))
                        {
                            if (item.NextSibling.FirstChild != null)
                            {
                                search.XmlUrl = item.NextSibling.FirstChild.InnerText.Trim();
                            }
                            else
                            {
                                search.XmlUrl = item.NextSibling.NextSibling.FirstChild.InnerText.Trim();
                            }
                        }
                    }
                }

                if ((enterPauseNode != null))
                {
                    int enterPause = 0;

                    if (enterPauseNode.NextSibling.ChildNodes.Count > 0)
                    {
                        enterPause = Convert.ToInt32(enterPauseNode.NextSibling.ChildNodes[0].InnerHtml);
                    }
                    else
                    {
                        enterPause = Convert.ToInt32(enterPauseNode.NextSibling.NextSibling.ChildNodes[0].InnerHtml);
                    }
                    enterPause *= 1000;
                    //if (!search.Ticket.Permission)
                    {
                        this.search.changeStatus(TicketSearchStatus.WaitingRoomStatus);
                        //Thread.Sleep(enterPause);
                        using (AutoResetEvent sleep = new AutoResetEvent(false))
                        {
                            sleep.WaitOne(3000);
                        }
                    }
                    ifValidated = true;
                }
                else
                {
                    ifValidated = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ifValidated = false;
            }

            return ifValidated;
        }

        private Boolean processTixWaitingRoom(AXSSearch search)
        {
            bool ifValidated = false;
            string wr = string.Empty;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            Boolean socketEnabled = false;
            Newtonsoft.Json.Linq.JObject obj = null;

            try
            {
                //if (!search.Ticket.URL.Contains("/shop/"))
                //{
                //    if (search.isWeb || search.isEventko)
                //    {
                //        doc.LoadHtml(post(search, String.Format("https://" + new Uri(this.search.Ticket.URL).Host + "/xmlrpc?methodName=getPhase&lotID=noLotId_7&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>noLotId_7</string></value></param></params></methodCall>", wr), "text/xml"));
                //    }
                //    else
                //    {
                //        doc.LoadHtml(post(search, String.Format("https://" + new Uri(this.search.Ticket.URL).Host + "/xmlrpc?methodName=getPhase&lotId=NoLotId&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>NoLotId</string></value></param></params></methodCall>", wr), "application/x-www-form-urlencoded; charset=UTF-8"));
                //    }
                //}
                //else
                //{
                //    doc.LoadHtml(post(search, String.Format("https://" + new Uri(this.search.Ticket.URL).Host + "/xmlrpc?methodName=getPhase&lotID=noLotId&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>noLotId</string></value></param></params></methodCall>", wr), "text/xml"));//"application/x-www-form-urlencoded"));
                //}

                this.search.Session.Payload = "";

                if (this.search.Ticket.IsUkEvent) //if (this.search.Ticket.URL.Contains("shop.axs.co.uk"))
                {
                    this.search.Session.HTMLWeb.origin = "https://shop.axs.co.uk";
                }

                String str = string.Empty;

                if (!this.search.Ticket.IsUkEvent) //if (!this.search.Ticket.URL.Contains("shop.axs.co.uk") && !this.search.Ticket.URL.Contains("q.axs.co.uk"))
                {
                    str = this.search.Session.Get("https://unifiedapicommerce.us-prod0.axs.com/veritix/pre-flow/v2/" + this.search.OnSaleUrl + "/phase?reservation=false");
                }
                else
                {
                    str = this.search.Session.Get("https://unifiedapicommerce.axs.co.uk/veritix/pre-flow/v2/" + this.search.OnSaleUrl + "/phase?reservation=false");
                }

                if (!String.IsNullOrEmpty(str))
                {
                    try
                    {
                        obj = Newtonsoft.Json.Linq.JObject.Parse(str);
                    }
                    catch
                    {
                        #region coverup patch
                        Match match = Regex.Match(str, "\"recaptchaEnabled\":(.*?)}");
                        if (match.Success)
                        {
                            String value = match.Value;

                            value = value.Replace("recaptchaEnabled", String.Empty).Replace("}", String.Empty).Replace(":", String.Empty).Replace("\"", String.Empty);

                            this.search.Recaptcha = Convert.ToString(value);
                        }



                        match = Regex.Match(str, "\"socketEnabled\":(.*?),");
                        if (match.Success)
                        {
                            String value = match.Value;

                            value = value.Replace("socketEnabled", String.Empty).Replace(":", String.Empty).Replace("\"", String.Empty).Replace(",", String.Empty);

                            socketEnabled = Convert.ToBoolean(value);
                        }



                        match = Regex.Match(str, "\"es5Flow\":(.*?),");
                        if (match.Success)
                        {
                            String value = match.Value;

                            value = value.Replace("es5Flow", String.Empty).Replace(":", String.Empty).Replace("\"", String.Empty).Replace(",", String.Empty);

                            if (value.Contains("3D") || value.Contains("2D"))
                            {
                                this.search.ifMap = true;
                            }
                        }
                        #endregion
                    }

                    if (obj != null)
                    {
                        if (obj.Property("recaptcha") != null)
                        {
                            Newtonsoft.Json.Linq.JObject recaptcha = (Newtonsoft.Json.Linq.JObject)obj["recaptcha"];

                            if (recaptcha.Property("enabled") != null)
                            {
                                this.search.Recaptcha = Convert.ToString(recaptcha["enabled"]);
                            }
                        }
                        else if (obj.Property("recaptchaEnabled") != null)
                        {
                            string recaptcha = obj["recaptchaEnabled"].ToString();

                            this.search.Recaptcha = Convert.ToString(recaptcha);
                        }


                        try
                        {
                            if (obj.Property("socketEnabled") != null)
                            {
                                socketEnabled = Convert.ToBoolean(obj["socketEnabled"]);
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        if (obj.Property("mmcMapID") != null)
                        {
                            if (obj.Property("es5Flow") != null)
                            {
                                if (obj["es5Flow"].ToString().Contains("3D") || obj["es5Flow"].ToString().Contains("2D"))
                                {
                                    this.search.ifMap = true;
                                }
                            }
                        }
                    }

                    ifValidated = true;
                }

                if (socketEnabled)
                {
                    try
                    {
                        try
                        {
                            Match match = Regex.Match(str, "\"referenceNumber\":(.*?),");
                            if (match.Success)
                            {
                                String value = match.Value;

                                value = value.Replace("referenceNumber", String.Empty).Replace("\"", String.Empty).Replace(",", String.Empty).TrimStart(':');

                                this.search.SessionKey = value;
                            }
                        }
                        catch { }

                        str = this.search.Session.Get("https://unifiedapicommerce.us-prod0.axs.com/socket.io/?transport=polling");

                        if (this.search.Session.HTMLWeb.StatusCode == HttpStatusCode.NotFound)
                        {
                            str = this.search.Session.Get("https://unifiedapicommerce.axs.co.uk/socket.io/?EIO=3&transport=websocket");
                        }
                    }
                    catch (Exception ex)
                    {
                        str = this.search.Session.Get("https://unifiedapicommerce.axs.co.uk/socket.io/?EIO=3&transport=websocket");
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                ifValidated = false;
            }

            return ifValidated;
        }

        public Boolean startPresale()
        {
            bool ifValidated = false;

            try
            {
                if (this.search.isTix)
                {
                    if (processTixWaitingRoom(this.search))
                    {
                        //if (enterWaitingRoom(this.search))
                        {
                            //this.LotId = this.search.Ticket.GetNextLotID();  //"sHWHHEq0Psc=";//

                            ////if (string.IsNullOrEmpty(this.LotId))
                            ////    this.LotId = LotIDPicker.LotIDInstance.GetNextLotID();

                            //if (!string.IsNullOrEmpty(this.LotId))
                            //{
                            //    if (this.LotId.Contains(" "))
                            //    {
                            //        this.LotId = this.LotId.Replace(' ', '+');
                            //    }
                            //    //search.MoreInfo += this.LotId;
                            //    //search.changeStatus(TicketSearchStatus.WaitingRoomStatus);
                            //    if (WaitForTurn(this.search, this.wr, this.lotId))
                            //    {
                            //        ifValidated = true;
                            //    }
                            //}
                            //else
                            //{
                            //    ifAvailable = false;
                            //    ifValidated = false;
                            //}
                        }

                        if (!this.search.Ticket.IsUkEvent && String.IsNullOrEmpty(this.search.SessionKey)) // if (!this.search.Ticket.URL.Contains("shop.axs.co.uk/") && !this.search.Ticket.URL.Contains("q.axs.co.uk/"))
                        {
                            JObject objsess = JObject.Parse(this.search.Session.HtmlDocument.DocumentNode.InnerText);
                            if (objsess != null)
                            {
                                if (objsess.Property("referenceNumber") != null)
                                {
                                    this.search.SessionKey = objsess["referenceNumber"].ToString();
                                    this.search.SessionKey = this.search.SessionKey.Replace(":", "%3A");
                                }
                            }
                        }
                    }

                    if ((search.Recaptcha != null) && (search.Recaptcha.ToLower().Contains("true")))
                    {
                        ifValidated = search.processCaptchaPage();
                    }
                }
                else
                {
                    if (processWaitingRoom(this.search))
                    {
                        //if (enterWaitingRoom(this.search))
                        {
                            this.LotId = this.search.Ticket.GetNextLotID();  //"sHWHHEq0Psc=";//

                            //if (string.IsNullOrEmpty(this.LotId))
                            //    this.LotId = LotIDPicker.LotIDInstance.GetNextLotID();

                            if (!string.IsNullOrEmpty(this.LotId))
                            {
                                if (this.LotId.Contains(" "))
                                {
                                    this.LotId = this.LotId.Replace(' ', '+');
                                }
                                //search.MoreInfo += this.LotId;
                                //search.changeStatus(TicketSearchStatus.WaitingRoomStatus);
                                if (WaitForTurn(this.search, this.wr, this.lotId))
                                {
                                    ifValidated = true;
                                }
                            }
                            else
                            {
                                ifAvailable = false;
                                ifValidated = false;
                            }
                        }
                    }

                    if (this.search.isWeb || this.search.isJSON || this.search.isEventko)
                    {
                        if ((search.Recaptcha != null) && (search.Recaptcha.ToLower().Contains("enable")))
                        {
                            ifValidated = search.processCaptchaPage();
                        }
                    }
                }
            }
            catch (Exception)
            {
                ifValidated = false;
            }

            return ifValidated;
        }

        public static String post(AXSSearch search, string url, string postdata, string contentType)
        {
            String Result = String.Empty;
            try
            {
                Encoding respenc = null;
                var isGZipEncoding = false;

                string URL = url;
                System.Net.HttpWebRequest webRequest = System.Net.HttpWebRequest.Create(URL) as System.Net.HttpWebRequest;
                webRequest.Method = "POST";
                webRequest.ContentType = contentType;
                webRequest.Timeout = 60000 * 2;

                webRequest.Headers.Add("X-Requested-With", "ShockwaveFlash/23.0.0.166");
                webRequest.Accept = "*/*";// "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";   //"*/*";

                if (search.isWeb || search.isJSON || search.isEventko)
                {
                    webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";
                }
                else
                {
                    webRequest.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 8_0 like Mac OS X) AppleWebKit/600.1.3 (KHTML, like Gecko) Version/8.0 Mobile/12A4345d Safari/600.1.4";
                }

                webRequest.KeepAlive = true;
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                //webRequest.Timeout = 10000;//60000 * 2;
                //webRequest.Referer = search.Ticket.URL;

                if (!search.isJSON && !search.isWeb && !search.isEventko)
                {
                    String tempUrl = search.Ticket.URL.Substring(search.Ticket.URL.IndexOf('?'));
                    webRequest.Referer = "https://axsmobile.eventshopper.com/mobilewroom/" + tempUrl;
                    webRequest.Headers.Add("Origin", "https://axsmobile.eventshopper.com");
                }
                else
                {
                    webRequest.Referer = search.Ticket.URL;
                    webRequest.Headers.Add("Origin", "https://" + new Uri(search.Ticket.URL).Host);
                }
                //webRequest.Headers.Add("Cookie", "_ga=GA1.2.1883910939.1469809617; s_fid=3828F044A5FE97D5-09B797F8FB98C5C0; s_gnr30=1469809638188-New");

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
                    if (search.Proxy.TheProxyType == Proxy.ProxyType.Relay)
                    {
                        webRequest.Proxy = search.Proxy.toWebProxy(search.context);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(search.Proxy.LuminatiSessionId))
                        {
                            webRequest.ConnectionGroupName = search.Proxy.LuminatiSessionId;
                        }

                        webRequest.Proxy = search.Proxy.toWebProxy();
                    }
                }

                System.Net.ServicePointManager.Expect100Continue = false;
                webRequest.ServicePoint.Expect100Continue = false;
                webRequest.KeepAlive = true;

                Stream reqStream = webRequest.GetRequestStream();
                string postData = postdata;
                byte[] postArray = Encoding.ASCII.GetBytes(postData);
                reqStream.Write(postArray, 0, postArray.Length);
                reqStream.Close();
                HttpWebResponse resp = null;

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

                    ////
                    //Stream reqStream = webRequest.GetRequestStream();


                    if (isGZipEncoding)
                    {
                        reqStream = new GZipStream(resp.GetResponseStream(), CompressionMode.Decompress);
                    }
                    else
                    {
                        reqStream = resp.GetResponseStream();
                        //  xdoc.Load(resp.GetResponseStream());


                    }

                    StreamReader sr = new StreamReader(reqStream);
                    Result = sr.ReadToEnd();
                }
                reqStream.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return Result;
        }

        private static bool IsGZipEncoding(string contentEncoding)
        {
            return contentEncoding.ToLower().StartsWith("gzip");
        }

        #endregion
    }
}
