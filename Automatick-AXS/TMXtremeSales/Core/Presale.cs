using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using System.Threading;
using System.IO;

namespace Automatick.Core
{
    [Serializable]
    class Presale
    {

        #region Members
        private AXSSearch _search;
        private string wr = string.Empty;
        private string lotId = string.Empty;
        
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
        #endregion

        #region Methods
        private Boolean enterWaitingRoom(AXSSearch search)
        {
            bool ifValidated = false;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            this.wr = HttpUtility.ParseQueryString((new Uri(search.Ticket.URL)).Query).Get("wr");

            try
            {
                doc.LoadHtml(post(search, "http://tickets.axs.com/xmlrpc?methodName=enternl", String.Format("<methodCall><methodName>enternl</methodName><params><param><value><string>{0}</string></value></param></params></methodCall>", this.wr)));
                HtmlNode lotIdNode = doc.DocumentNode.SelectSingleNode("//name[text() = 'lotId']");

                if (lotIdNode != null)
                {
                    this.lotId = lotIdNode.NextSibling.ChildNodes[0].InnerHtml;
                    ifValidated = true;
                }
            }
            catch (Exception )
            {
                ifValidated = false;
            }

            return ifValidated;
        }

        private Boolean WaitForTurn(AXSSearch search, string wr, string lotId)
        {
            bool ifValidated = false;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            if (search.StateWaiting == true)
            {
                
                   
                        try
                        {
                            doc.LoadHtml(post(search, String.Format("http://tickets.axs.com/xmlrpc?methodName=getPhase&lotId={0}&wr={1}", this.lotId, this.wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>{1}</string></value></param></params></methodCall>", this.wr, this.lotId)));

                            HtmlNode enterPauseNode = doc.DocumentNode.SelectSingleNode("//name[text() = 'enterPause']");
                            HtmlNode comeBackInNode = doc.DocumentNode.SelectSingleNode("//name[text() = 'comeBackIn']");

                            if ((enterPauseNode != null) && (comeBackInNode != null))
                            {
                                int enterPause = Convert.ToInt32(enterPauseNode.NextSibling.ChildNodes[0].InnerHtml);
                                int comeBackIn = Convert.ToInt32(comeBackInNode.NextSibling.ChildNodes[0].InnerHtml);

                                if (enterPause < 60)
                                {
                                    search.StateWaiting = false;
                                    enterPause *= 1000;
                                    if (search.Ticket.Permission)
                                    {
                                        Thread.Sleep(enterPause);
                                        WaitForTurn(search, this.wr, this.lotId);
                                    }

                                }
                                else if (comeBackIn < 60)
                                {
                                    search.StateWaiting = false;
                                    comeBackIn *= 1000;
                                    if (search.Ticket.Permission)
                                    {
                                        Thread.Sleep(comeBackIn);
                                        WaitForTurn(search, this.wr, this.lotId);
                                    }

                                }
                                else
                                {
                                    enterPause *= 1000;
                                    search.StateWaiting = true;
                                    if (search.Ticket.Permission)
                                    {
                                        Thread.Sleep(enterPause);
                                        WaitForTurn(search, this.wr, this.lotId);
                                    }

                                }

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
            else
            {
                return true;
            }
            return ifValidated;
        }

        private Boolean processWaitingRoom(AXSSearch search)
        {
            bool ifValidated = false;
            string wr = string.Empty;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
           
              
                    try
                    {
                        wr = HttpUtility.ParseQueryString((new Uri(search.Ticket.URL)).Query).Get("wr");

                        doc.LoadHtml(post(search, String.Format("http://tickets.axs.com/xmlrpc?methodName=getPhase&lotID=noLotId_7&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>noLotId_7</string></value></param></params></methodCall>", wr)));

                        HtmlNode enterPauseNode = doc.DocumentNode.SelectSingleNode("//name[text() = 'enterPause']");

                        if ((enterPauseNode != null))
                        {
                            int enterPause = Convert.ToInt32(enterPauseNode.NextSibling.ChildNodes[0].InnerHtml);
                            enterPause *= 1000;
                            if (search.Ticket.Permission)
                            {
                                Thread.Sleep(enterPause);
                            }
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
              


            

            return ifValidated;
        }

        public Boolean startPresale()
        {
            bool ifValidated = false;

            try
            {
                if (processWaitingRoom(this.search))
                {
                    if (enterWaitingRoom(this.search))
                    {
                        if (WaitForTurn(this.search, this.wr, this.lotId))
                        {
                            ifValidated = true;
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
        #endregion
    }
}
