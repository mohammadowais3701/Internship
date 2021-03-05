using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.IO;

namespace Automatick.Core
{
    [Serializable]
    public class AXSEvent
    {

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
        public AXSEvent(HtmlDocument xml, AXSSearch search)
        {
            this.XML = xml;
            this.Search = search;
            Sections = new List<AXSSection>();
            PriceLevels = new List<AXSPriceLevel>();
            
         
          
                IfTicketAlive = true;
                if (parseXML())
                {
                    IfTicketAlive = true;
                }
              
                else
                {
                    IfTicketAlive = false;
                }
           
        }
    

        private Boolean parseXML()
        {
            try 
            {
                int i;
                this.XML.LoadHtml(post(this.Search, this.Search.Ticket.XmlUrl + "?methodName=showshop.seriesInfoW&wroom=" + this.Search.wRoom + "&lang=en&ver=$Name:%20eventShopperV3_V3_1Cd%20$", String.Format("<methodCall><methodName>showshop.seriesInfoW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param></params></methodCall>", this.Search.wRoom)));
                this.XML.LoadHtml(this.XML.DocumentNode.InnerHtml.Replace("<param>", ""));

                HtmlNodeCollection eventTypeCodeCollection = this.XML.DocumentNode.SelectNodes("//methodresponse/params/value/array/data/value/struct/member/name[text() = 'eventTypeCode']");
                HtmlNodeCollection eventCodeCollection = this.XML.DocumentNode.SelectNodes("//methodresponse/params/value/array/data/value/struct/member/name[text() = 'eventCode']");
                HtmlNodeCollection eventDatesCollection = this.XML.DocumentNode.SelectNodes("//methodresponse/params/value/array/data/value/struct/member/name[text() = 'date']");

               
                if(eventTypeCodeCollection.Count==eventCodeCollection.Count)
                {
                    if(eventTypeCodeCollection.Count==eventDatesCollection.Count)
                    {
                    for (i =0;i<eventTypeCodeCollection.Count;i++)
                    {
                        PriceLevels = new List<AXSPriceLevel>();
                        string eventTypeCode = eventTypeCodeCollection[i].NextSibling.NextSibling.ChildNodes[0].InnerHtml;
                        string eventCode = eventCodeCollection[i].NextSibling.NextSibling.ChildNodes[0].InnerHtml;
                        string eventDate = eventDatesCollection[i].NextSibling.NextSibling.ChildNodes[0].InnerHtml;

                        
                        AXSSection section=new AXSSection(eventTypeCode,eventCode,eventDate,null);
                        this.Sections.Add(section);
                    }
                    }
                }
               
                foreach (AXSSection section in this.Sections)
                {
                    this.XML.LoadHtml(post(this.Search, this.Search.Ticket.XmlUrl + "?methodName=showshop.priceTableW&wroom=" + this.Search.wRoom + "&lang=en", String.Format("<methodCall><methodName>showshop.priceTableW</methodName><params><param><value><string>{0}</string></value></param><param><value><string>en</string></value></param></params></methodCall>", this.Search.wRoom)));
                    this.XML.LoadHtml(this.XML.DocumentNode.InnerHtml.Replace("<param>", ""));
                    PriceLevels = new List<AXSPriceLevel>();
                    string priceLevelQuery = "//name[text() = '" + section.EventTypeCode + "']";
                    HtmlNode priceLevels = this.XML.DocumentNode.SelectSingleNode(priceLevelQuery);

                    this.XML = new HtmlAgilityPack.HtmlDocument();
                    this.XML.LoadHtml(priceLevels.NextSibling.NextSibling.OuterHtml);
                    HtmlNodeCollection allPriceLevels = this.XML.DocumentNode.SelectNodes("/value/array/data/value[2]/array/data/value/array/data");
                   // HtmlDocument hdoc = new HtmlDocument();
                   // hdoc.LoadHtml(priceLevels.NextSibling.NextSibling.OuterHtml);
                    HtmlNodeCollection allPrices =this.XML.DocumentNode.SelectNodes("/value/array/data/value[4]/array/data/value/array/data");
                    string mos = allPriceLevels[0].SelectNodes("value")[0].InnerHtml;
                    Dictionary<string, string> pLevels = new Dictionary<string, string>();
                    for (int k = 0; k < allPriceLevels.Count; k++)
                    {
                        string priceLevelNumber = allPriceLevels[k].SelectNodes("value/string")[0].InnerHtml;
                        string priceLevelName = allPriceLevels[k].SelectNodes("value/string")[1].InnerHtml;
                        string priceTotal = allPrices[k].SelectNodes("value/int")[2].InnerHtml;
                        priceTotal=priceTotal.Insert((priceTotal.Length-2),".");
                        decimal price = Convert.ToDecimal(priceTotal);
                        AXSPriceLevel priceLevel = new AXSPriceLevel(priceLevelName, priceLevelNumber,price);
                        this.PriceLevels.Add(priceLevel);

                    }
                    section.PriceLevels = this.PriceLevels;
                }
                return true;
            }
            catch
            {
                return false;
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

