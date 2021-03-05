using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SortedBindingList;
using System.ComponentModel;
using System.Web;
using HtmlAgilityPack;

namespace Automatick.Core
{
    [Serializable]
    public class AXSTicket:ITicket
    {
        internal int CurrentParameterIndex = 0;
        internal int CurrentAvailableParameterIndex = 0;
        internal int CurrentFoundParameterIndex = 0;
        internal Boolean IfSelectDeliveryWindowOpen = false;
        System.Media.SoundPlayer _soundAlert = null;
        SortableBindingList<AXSTicketAccount> _allTMAccounts = null;
        
        
        public SortableBindingList<AXSTicketAccount> AllTMAccounts
        {
            get { return _allTMAccounts; }
        }

        public AXSTicket()
        {
           
            this._TicketID = UniqueKey.getUniqueKey();
            this.NoOfSearches = 4;
            this.ifAutoBuy = false;
            this.ifAutoBuyWitoutProxy = false;
            this.ifDistributeInSearches = true;
            this.ifPlaySoundAlert = true;
            this.ifRandomDelay = false;
            this.ifSchedule = false;
            this.ifSelectDeliveryOptionAutoBuying = true;
            this.ifSelectDeliveryOptionList = false;
            this.ifSelectAccountAutoBuying = true;
            this.ifSelectAccountList = false;
            this.ifSendEmail = false;
            this.ifUseFoundOnFirstAttempt = false;
            this.ifUseAvailableParameters = false;
            this.ifUseProxies = true;
            this.ResetSearchDelay = 3;
            this.Parameters = new SortableBindingList<AXSParameter>();
            AXSParameter parameter = new AXSParameter();
            this.Parameters.Add(parameter);
            this.ScheduleDateTime = DateTime.Now.AddHours(1);
            this.ScheduleRunningTime = 15;
            this.SelectedDeliveryOptions = new SortableBindingList<AXSDeliveryOption>();
            this.StartUsingProxiesFrom = 1;
            this.StartSolvingFromCaptcha = 1;
            this.TicketFoundCriterions = new SortableBindingList<AXSFoundCriteria>();
            this.TicketGroup = null;
            this.SelectedAccounts = new SortableBindingList<AXSTicketAccount>();
            this.AgencyName = "";
            this.AgencyUrlCode = "";
            this.XmlUrl = "";
        }
        #region ITicket Members
        private String _TicketID = "";
        public ChangeDelegate onChangeStartOrStop
        {
            get;
            set;
        }

        public ChangeDelegateForGauge onChangeForGauge
        {
            get;
            set;
        }

        public String TicketID
        {
            get { return this._TicketID; }
        }

        public String TicketName
        {
            get;
            set;
        }

        public String URL
        {
            get;
            set;
        }

        public String FileLocation
        {
            get;
            set;
        }

        public String DeliveryOption
        {
            get;
            set;
        }

        public String DeliveryCountry
        {
            get;
            set;
        }

        public String Notes
        {
            get;
            set;
        }

        public decimal NoOfSearches
        {
            get;
            set;
        }

        public decimal ResetSearchDelay
        {
            get;
            set;
        }

        public decimal StartUsingProxiesFrom
        {
            get;
            set;
        }

        public decimal StartSolvingFromCaptcha
        {
            get;
            set;
        }

        public decimal ScheduleRunningTime
        {
            get;
            set;
        }

        public Boolean ifPesistSessionInEachSearch
        {
            get;
            set;

        }

        public Boolean isRunning
        {
            get;
            set;
        }

        public Boolean ifDistributeInSearches
        {
            get;
            set;
        }

        public Boolean ifUseAvailableParameters
        {
            get;
            set;
        }

        public Boolean ifUseFoundOnFirstAttempt
        {
            get;
            set;
        }


        public Boolean ifPlaySoundAlert
        {
            get;
            set;
        }

        public Boolean ifAutoBuy
        {
            get;
            set;
        }

        public Boolean ifSendEmail
        {
            get;
            set;
        }

        public Boolean ifAutoBuyWitoutProxy
        {
            get;
            set;
        }

        public Boolean ifRandomDelay
        {
            get;
            set;
        }

        public Boolean ifUseProxies
        {
            get;
            set;
        }

        public Boolean ifSelectDeliveryOptionAutoBuying
        {
            get;
            set;
        }

        public Boolean ifSelectDeliveryOptionList
        {
            get;
            set;
        }

        public Boolean ifSelectAccountAutoBuying
        {
            get;
            set;
        }

        public Boolean ifSelectAccountList
        {
            get;
            set;
        }

        public Boolean ifSchedule
        {
            get;
            set;
        }

        public int RunCount
        {
            get;
            set;
        }

        public int FoundCount
        {
            get;
            set;
        }

        public int BuyCount
        {
            get;
            set;
        }

        public int SoldoutCount
        {
            get;
            set;
        }

        public DateTime ScheduleDateTime
        {
            get;
            set;
        }

        public DateTime LastUsedDateTime
        {
            get;
            set;
        }

        public object TicketSearchWindow
        {
            get;
            set;
        }

        public TicketGroup TicketGroup
        {
            get;
            set;
        }

        public SortableBindingList<ITicketSearch> Searches
        {
            get;
            set;
        }

      

        public SortableBindingList<AXSParameter> Parameters
        {
            get;
            set;
        }

        public SortableBindingList<AXSDeliveryOption> SelectedDeliveryOptions
        {
            get;
            set;
        }

        public SortableBindingList<AXSFoundCriteria> TicketFoundCriterions
        {
            get;
            set;
        }

        public SortableBindingList<AXSTicketAccount> SelectedAccounts
        {
            get;
            set;
        }

        public Dictionary<String, int> BuyHistory
        {
            get;
            set;
        }

        public System.Media.SoundPlayer SoundAlert
        {
            get { return _soundAlert; }
        }

    
        public EmailSetting Email
        {
            get;
            set;
        }

        public GlobalSettings GlobalSetting
        {
            get;
            set;
        }
        public string AgencyName
        {
            get;
            set;
        }

        public string AgencyUrlCode
        {
            get;
            set;
        }

        public string XmlUrl
        {
            get;
            set;
        }

        public bool Permission
        {
            get;
            set;
        }
        public String MakeValidFileName()
        {
            String invalidChars = Regex.Escape(new String(Path.GetInvalidFileNameChars()));
            String invalidReStr = String.Format(@"[{0}]+", invalidChars);
            String file = Regex.Replace(TicketName, invalidReStr, "");
            if (file.Length > 250)
            {
                file = file.Substring(0, 250);
            }
            return file;
        }

        public bool SaveTicket()
        {
            try
            {
                SortableBindingList<ITicketSearch> tmpAllSearches = this.Searches;
                ChangeDelegate tmpChangeDelegate = this.onChangeStartOrStop;
                ChangeDelegateForGauge tmpChangeDelegateForGauge = this.onChangeForGauge;
                System.Media.SoundPlayer tmpSoundAlert = this._soundAlert;
                EmailSetting tmpEmail = this.Email;
                GlobalSettings tmpGlobalSetting = this.GlobalSetting;

                object tmpTicketSearchWindow = this.TicketSearchWindow;
                this.Searches = null;
                this.TicketSearchWindow = null;
                this.onChangeStartOrStop = null;
                this.onChangeForGauge = null;
                this._soundAlert = null;
                this.Email = null;
                this.GlobalSetting= null;
                if (String.IsNullOrEmpty(this._TicketID))
                {
                    this._TicketID = UniqueKey.getUniqueKey();
                }

                this.LastUsedDateTime = DateTime.Now;

                if (!Directory.Exists(FileLocation + @"\Tickets\"))
                {
                    Directory.CreateDirectory(FileLocation + @"\Tickets\");
                }
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream serializationStream = new FileStream(FileLocation + @"\Tickets\" + MakeValidFileName() + ".tevent", FileMode.Create);
                formatter.Serialize(serializationStream, this);
                serializationStream.Close();
                this.Searches = tmpAllSearches;
                this.TicketSearchWindow = tmpTicketSearchWindow;
                this.onChangeStartOrStop = tmpChangeDelegate;
                this.onChangeForGauge = tmpChangeDelegateForGauge;
                this._soundAlert = tmpSoundAlert;
                this.Email = tmpEmail;
                this.GlobalSetting = tmpGlobalSetting;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteTicket()
        {
            try
            {
                if (File.Exists(FileLocation + @"\Tickets\" + TicketName + ".tevent"))
                {
                    File.Delete(FileLocation + @"\Tickets\" + TicketName + ".tevent");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool setAgencyRequestUrl(AXSTicket Ticket)
        {
            bool ifValidated = false;
            string wr = string.Empty;

            wr = HttpUtility.ParseQueryString((new Uri(this.URL)).Query).Get("wr");

            if (!string.IsNullOrEmpty(wr))
            {
                try
                {
                    HtmlAgilityPack.HtmlDocument hDoc = new HtmlAgilityPack.HtmlDocument();
                    hDoc.LoadHtml(post(String.Format("http://tickets.axs.com/xmlrpc?methodName=getPhase&lotID=noLotId_7&wr={0}", wr), String.Format("<methodCall><methodName>getPhase</methodName><params><param><value><string>{0}</string></value></param><param><value><string>noLotId_7</string></value></param></params></methodCall>", wr)));
                    HtmlNode requestUrlNode = hDoc.DocumentNode.SelectSingleNode("//member/name[text() = 'info_path']");

                    if (requestUrlNode == null)
                    {
                        ifValidated = false;
                    }
                    else
                    {
                        try
                        {
                            string requestUrl = requestUrlNode.NextSibling.ChildNodes[0].InnerHtml;
                            int index = requestUrl.IndexOf(".");
                            string agencyRequestCode = requestUrl.Substring(0, index).Replace("https://", "").Replace("http://", "");
                            Ticket.AgencyUrlCode = agencyRequestCode;
                            Ticket.XmlUrl = requestUrl;
                            Ticket.AgencyName = "";
                            ifValidated = true;
                        }
                        catch (Exception)
                        {
                            ifValidated = false;
                        }
                    }
                }
                catch (Exception)
                {
                    ifValidated = false;
                }
            }
            else
            {
                ifValidated = false;
            }

            return ifValidated;
        }
        public static String post(string url, string postdata)
        {
            string URL = url;
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(URL);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            Stream reqStream = webRequest.GetRequestStream();
            string postData = postdata;
            byte[] postArray = Encoding.ASCII.GetBytes(postData);
            reqStream.Write(postArray, 0, postArray.Length);
            reqStream.Close();
            StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream());
            string Result = sr.ReadToEnd();

            return Result;
        }
        public void changeSettingsDuringSearching(decimal lastSearches)
        {
            if (this.NoOfSearches > lastSearches)
            {                
                for (int i = 0; i < (this.NoOfSearches - lastSearches); i++)
                {
                    if (Searches != null)
                    {
                        AXSSearch search = new AXSSearch(this);
                        //search.start();
                        this.Searches.Add(search);
                    }                    
                }          
            }
            else if (this.NoOfSearches < lastSearches)
            {
                if (Searches != null)
                {
                    for (int i = ((int)lastSearches - 1) ; i >= this.NoOfSearches; i--)
                    {
                        ITicketSearch search = this.Searches[i];
                        search.stop();
                        this.Searches.Remove(search);                        
                        search.Dispose();
                    }
                }
            }

            try
            {
                for (int i = 0; i < this.Searches.Count; i++)
                {
                    ITicketSearch search = this.Searches[i];              

                    search.IfUseProxy = false;
                    if (i >= ((int)this.StartUsingProxiesFrom - 1) && this.ifUseProxies)
                    {
                        search.IfUseProxy = true;
                    }

                    if (!search.IfWorking)
                    {
                        search.start();
                    }                    
                }
            }
            catch (Exception)
            {

            }
        }

        public void start(System.Media.SoundPlayer soundAlert,  SortableBindingList<AXSTicketAccount> allTMAccountsForSyncing, EmailSetting email,GlobalSettings globalSetting)
        {
            Regex.CacheSize = 0;
            this.isRunning = true;
            this._soundAlert = soundAlert;
            Searches = new SortableBindingList<ITicketSearch>();
            this.CurrentParameterIndex = 0;
            this.LastUsedDateTime = DateTime.Now;
            this.IfSelectDeliveryWindowOpen = false;
            this._allTMAccounts = allTMAccountsForSyncing;
            this.Email = email;
            this.syncSelectedAccounts();
            this.GlobalSetting = globalSetting;


            for (int i = 0; i < this.Parameters.Count; i++)
            {
                this.Parameters[i].IfAvailable = false;
                this.Parameters[i].IfFound = false;
            }

            for (int i = 0; i < this.NoOfSearches; i++)
            {
                AXSSearch search = new AXSSearch(this);
               

                search.IfUseProxy = false;
                if (i >= ((int)this.StartUsingProxiesFrom - 1) && this.ifUseProxies)
                {
                    search.IfUseProxy = true;
                }

                search.start();
                this.Searches.Add(search);
            }

            if (this.onChangeStartOrStop != null)
            {
                this.onChangeStartOrStop(this);
            } 
        }

        public void stop()
        {
            this.isRunning = false;
            try
            {
                if (this.Searches != null)
                {
                    for (int i = 0; i < this.Searches.Count; i++)
                    {
                        try
                        {
                            this.Searches[i].stop();
                            this.Searches[i].Dispose();
                        }
                        catch { }
                    }
                    this.Searches.Clear();
                }

                if (this.TicketSearchWindow != null)
                {
                    if (this.TicketSearchWindow.GetType() == typeof(frmFindTicket))
                    {
                        frmFindTicket frm = (frmFindTicket)this.TicketSearchWindow;
                        frm.closeForm();
                    }
                }
                if (this.onChangeStartOrStop != null)
                {
                    this.onChangeStartOrStop(this);
                    this.onChangeStartOrStop = null;
                }

                if (this.onChangeForGauge != null)
                {
                    this.onChangeForGauge();
                    this.onChangeForGauge = null;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                try
                {
                    this.SaveTicket();
                    GC.Collect();
                }
                catch (Exception)
                {

                }
            }
        }

        private void syncSelectedAccounts()
        {
            if (_allTMAccounts != null && this.SelectedAccounts != null)
            {
                if (_allTMAccounts.Count>0)
                {
                    for (int i = this.SelectedAccounts.Count - 1; i >= 0; i--)
                    {
                        AXSTicketAccount syncAccount = _allTMAccounts.FirstOrDefault(p => p.AccountEmail == this.SelectedAccounts[i].AccountEmail);
                        if (syncAccount != null)
                        {
                            syncAccount.IfSelected = this.SelectedAccounts[i].IfSelected;
                            syncAccount.BuyingLimit = this.SelectedAccounts[i].BuyingLimit;
                            this.SelectedAccounts[i] = syncAccount;
                        }
                        else
                        {
                            this.SelectedAccounts.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    this.SelectedAccounts = new SortableBindingList<AXSTicketAccount>();
                }                
            }
            else
            {
                this.SelectedAccounts = new SortableBindingList<AXSTicketAccount>();
            }

            this.SaveTicket();
        }
        #endregion
    }
}
