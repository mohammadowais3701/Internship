using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SortedBindingList;
using System.Web;
using System.Diagnostics;

namespace Automatick.Core
{
    [Serializable]
    public class ApplicationStartUp
    {
        String _DefaultLocation;
        SortableBindingList<AXSTicket> _Tickets;
        SortableBindingList<Proxy> _Proxies;
        SortableBindingList<AXSTicketAccount> _Accounts;
        SortableBindingList<TicketGroup> _Groups;
        SortableBindingList<AXSDeliveryOption> _TicketDeliveryOptions;
        EmailSetting _EmailSetting;
        System.Media.SoundPlayer _SoundAlert;
        AutoCaptchaServices _AutoCaptchaServices;
        Dictionary<String, String> _Countries = new Dictionary<string, string>();
        Dictionary<String, String> US_States = new Dictionary<string, string>();
        Dictionary<String, String> Canada_States = new Dictionary<string, string>();
        Dictionary<String, String> Australia_States = new Dictionary<string, string>();
        List<string> _LotID = new List<string>();
        String _DefaultBucketFile;
        public GlobalSettings GlobalSetting
        {
            get;
            set;
        }
        public System.Media.SoundPlayer SoundAlert
        {
            get { return _SoundAlert; }
        }

        public SortableBindingList<AXSDeliveryOption> TicketDeliveryOptions
        {
            get { return this._TicketDeliveryOptions; }
            set { this._TicketDeliveryOptions = value; }
        }

        public SortableBindingList<TicketGroup> Groups
        {
            get { return this._Groups; }
            set { this._Groups = value; }
        }

        public SortableBindingList<AXSTicketAccount> Accounts
        {
            get { return _Accounts; }
            set { _Accounts = value; }
        }

        public SortableBindingList<Proxy> Proxies
        {
            get { return _Proxies; }
            set { _Proxies = value; }
        }

        public SortableBindingList<AXSTicket> Tickets
        {
            get { return _Tickets; }
            set { _Tickets = value; }
        }

        public EmailSetting EmailSetting
        {
            get { return _EmailSetting; }
            set { _EmailSetting = value; }
        }

        public AutoCaptchaServices AutoCaptchaService
        {
            get { return this._AutoCaptchaServices; }
            set { this._AutoCaptchaServices = value; }
        }


        public Dictionary<String, String> Countries
        {
            get { return _Countries; }
            set { _Countries = value; }
        }
        public Dictionary<String, String> _US_States
        {
            get { return US_States; }
            set { US_States = value; }
        }
        public Dictionary<String, String> _Canada_States
        {
            get { return Canada_States; }
            set { Canada_States = value; }
        }
        public Dictionary<String, String> _Australia_States
        {
            get { return Australia_States; }
            set { Australia_States = value; }
        }
        public String DefaultFileLocation
        {
            get { return _DefaultLocation; }
        }
        public List<String> LotID
        {
            get { return _LotID; }
            set { _LotID = value; }
        }

        public String DefaultBucketFile
        {
            get { return _DefaultBucketFile; }
        }

        public ApplicationStartUp(String defaultLocation)
        {
            _DefaultLocation = defaultLocation;
            _DefaultBucketFile = defaultLocation + @"\TixToxSetting.txt";
            _Tickets = new SortableBindingList<AXSTicket>();
            if (!Directory.Exists(_DefaultLocation + @"\Tickets\"))
            {
                Directory.CreateDirectory(_DefaultLocation + @"\Tickets\");
            }
            _SoundAlert = new System.Media.SoundPlayer(global::Automatick.Properties.Resources.TicketFound);
            LoadCountries();
            LoadStates();
        }

        public void LoadTickets()
        {
            try
            {
                List<AXSTicket> runningTickets = this._Tickets.ToList();
                this._Tickets.Clear();

                foreach (string str in Directory.GetFiles(_DefaultLocation + @"\Tickets\", "*.tevent"))
                {
                    string ticketName = str.Substring(str.LastIndexOf('\\') + 1);
                    ticketName = ticketName.Substring(0, ticketName.LastIndexOf('.'));
                    AXSTicket ticket = loadTicketByName(ticketName);
                    if (ticket != null)
                    {
                        try
                        {
                            String url = ticket.URL;
                            url = url.Replace(";;;slash;;;", "/");
                            url = url.Replace("%2F", "/");
                            String[] query = url.Split('&');

                            foreach (var item in query)
                            {
                                if (item.Split('=')[0].ToLower().Equals("eventid"))
                                {
                                    ticket.EventID = item.Split('=')[1];
                                    break;
                                }
                                else if (item.Split('=')[0].ToLower().Equals("event"))
                                {
                                    ticket.EventID = item.Split('=')[1];
                                    break;
                                }
                            }

                            if (String.IsNullOrEmpty(ticket.EventID))
                            {
                                ticket.EventID = String.Empty;
                            }

                            ticket.WR = HttpUtility.ParseQueryString((new Uri(ticket.URL)).Query).Get("wr");

                            if (string.IsNullOrEmpty(ticket.WR))
                            {
                                if (ticket.URL.Contains("/shop/") || ticket.URL.Contains("/#/"))
                                {
                                    String w = ticket.URL.Substring(ticket.URL.IndexOf("#") + 2);
                                    string[] split = w.Split('/');
                                    if (split[0].Contains("?"))
                                    {
                                        split = split[0].Split('?');
                                        ticket.WR = split[0];
                                    }
                                    else
                                    {
                                        ticket.WR = split[0];
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(ticket.WR))
                            {
                                String[] _query = ticket.URL.Substring(ticket.URL.IndexOf('?') + 1).Split('&');

                                foreach (var item in _query)
                                {
                                    if (item.Split('=')[0].ToLower().Equals("wr"))
                                    {
                                        ticket.WR = item.Split('=')[1];
                                        break;
                                    }
                                }
                            }

                            if ((runningTickets != null) && (runningTickets.Count > 0))
                            {
                                AXSTicket tmpTicket = runningTickets.FirstOrDefault(p => p.TicketID == ticket.TicketID);
                                if (tmpTicket != null)
                                {
                                    if (tmpTicket.isRunning)
                                    {
                                        ticket = tmpTicket;
                                    }
                                }
                            }
                            else
                            {
                                ticket.isRunning = false;
                                ticket.SaveTicket();
                            }

                            this._Tickets.Add(ticket);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ticket.URL + " -> " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public AXSTicket loadTicketByName(string ticketName)
        {
            FileStream serializationStream = null;
            AXSTicket ticket = null;
            try
            {
                if (!ticketName.EndsWith(".tevent"))
                {
                    ticketName += ".tevent";
                }
                String fileName = _DefaultLocation + @"\Tickets\" + ticketName;
                if (File.Exists(fileName))
                {
                    serializationStream = new FileStream(fileName, FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();
                    ticket = (AXSTicket)formatter.Deserialize(serializationStream);
                    ticket.FileLocation = this.DefaultFileLocation;
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                if (serializationStream != null)
                {
                    serializationStream.Close();
                    serializationStream.Dispose();
                    //GC.Collect();
                }
            }
            return ticket;
        }

        public void LoadProxies()
        {
            string FileName = _DefaultLocation + @"\Tickets\proxy.tproxy";

            if (File.Exists(FileName))
            {
                _Proxies = new SortableBindingList<Proxy>();
                _Proxies.Load(FileName);
            }
            else
            {
                _Proxies = new SortableBindingList<Proxy>();
            }
            if (this._Proxies != null)
            {
                if (this._Proxies.Count > 0)
                {
                    for (int i = this._Proxies.Count - 1; i >= 0; i--)
                    {
                        if (this._Proxies[i].TheProxyType == Proxy.ProxyType.MyIP || this._Proxies[i].TheProxyType == Proxy.ProxyType.Luminati || this._Proxies[i].TheProxyType == Proxy.ProxyType.Relay)
                        {
                            this._Proxies.Remove(this._Proxies[i]);
                        }
                    }
                }
            }
        }

        public void LoadGlobalSettings()
        {
            string FileName = _DefaultLocation + @"\Tickets\globalSettings.settings";
            GlobalSettings gs = null;
            try
            {
                if (File.Exists(FileName))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (FileStream stream = new FileStream(FileName, FileMode.Open))
                    {
                        // Deserialize data list items
                        gs = (GlobalSettings)formatter.Deserialize(stream);
                    }
                }
            }
            catch (Exception)
            {
                gs = null;
            }

            if (gs == null)
            {
                gs = new GlobalSettings();
            }

            this.GlobalSetting = gs;
        }

        public void LoadLogs()
        {
            string FileName = _DefaultLocation + @"\Logs\" + DateTime.Now.ToString("MM-dd-yyyy") + ".dat";
            if (File.Exists(FileName))
            {
                Automatick.Logging.Logger.LoggerInstance.Load(FileName);
            }
        }


        public void LoadAccounts()
        {
            string FileName = _DefaultLocation + @"\Tickets\accounts.taccounts";

            if (File.Exists(FileName))
            {
                _Accounts = new SortableBindingList<AXSTicketAccount>();
                _Accounts.Load(FileName);

                try
                {
                    for (int i = this._Accounts.Count - 1; i >= 0; i--)
                    {
                        if (String.IsNullOrEmpty(this._Accounts[i].GroupName))
                        {
                            this._Accounts[i].GroupName = "guest";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                _Accounts = new SortableBindingList<AXSTicketAccount>();
            }
        }

        public void LoadEmails()
        {
            string FileName = _DefaultLocation + @"\Tickets\email.temail";

            if (File.Exists(FileName))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(FileName, FileMode.Open))
                {
                    // Deserialize object                    
                    this._EmailSetting = (EmailSetting)formatter.Deserialize(stream);
                }
            }
            else
            {
                this._EmailSetting = new EmailSetting();
            }
        }

        public void LoadGroups()
        {
            string FileName = _DefaultLocation + @"\Tickets\groups.tgroup";

            this._Groups = new SortableBindingList<TicketGroup>();

            if (File.Exists(FileName))
            {
                this._Groups.Load(FileName);
            }

            TicketGroup tg = new TicketGroup();
            tg.CheckAndCreateDefaultTicketGroup(this._Groups);
        }

        public void LoadDeliveryOptions()
        {
            string FileName = _DefaultLocation + @"\Tickets\DeliveryOptions.tDeliveryOption";

            this._TicketDeliveryOptions = new SortableBindingList<AXSDeliveryOption>();

            if (File.Exists(FileName))
            {
                this._TicketDeliveryOptions.Load(FileName);
            }

            AXSDeliveryOption tdop = new AXSDeliveryOption();
            tdop.CheckAndCreateDefaultTicketDeliveryOptions(this._TicketDeliveryOptions);
        }

        public void LoadCountries()
        {
            _Countries.Add("CA", "Canada"); _Countries.Add("US", "United States"); _Countries.Add("AR", "Argentina");
            _Countries.Add("AU", "Australia"); _Countries.Add("BE", "Belgium"); _Countries.Add("BR", "Brazil");
            _Countries.Add("BW", "Botswana"); _Countries.Add("AT", "Austria"); _Countries.Add("CN", "China");
            _Countries.Add("CL", "Chile"); _Countries.Add("HR", "Croatia"); _Countries.Add("DK", "Denmark");
            _Countries.Add("CR", "Costa Rica"); _Countries.Add("HU", "Hungary"); _Countries.Add("FR", "France");
            _Countries.Add("EE", "Estonia"); _Countries.Add("FI", "Finland"); _Countries.Add("HK", "Hong Kong");
            _Countries.Add("GE", "Georgia"); _Countries.Add("DE", "Germany"); _Countries.Add("GR", "Greece");
            _Countries.Add("IS", "Iceland"); _Countries.Add("ID", "Indonesia"); _Countries.Add("IE", "Ireland");
            _Countries.Add("IL", "Israel"); _Countries.Add("IT", "Italy"); _Countries.Add("JM", "Jamaica");
            _Countries.Add("JP", "Japan"); _Countries.Add("KR", "Korea, Republic Of"); _Countries.Add("LT", "Lithuania");
            _Countries.Add("MX", "Mexico"); _Countries.Add("MC", "Monaco"); _Countries.Add("NL", "Netherlands");
            _Countries.Add("NZ", "New Zealand"); _Countries.Add("NO", "Norway"); _Countries.Add("PA", "Panama");
            _Countries.Add("PH", "Philippines"); _Countries.Add("PL", "Poland"); _Countries.Add("PT", "Portugal");
            _Countries.Add("RU", "Russian Federation"); _Countries.Add("RS", "Serbia"); _Countries.Add("ZA", "South Africa");
            _Countries.Add("SI", "Slovenia"); _Countries.Add("SG", "Singapore"); _Countries.Add("SE", "Sweden");
            _Countries.Add("ES", "Spain"); _Countries.Add("CH", "Switzerland"); _Countries.Add("TR", "Turkey");
            _Countries.Add("GB", "United Kingdom"); _Countries.Add("ZW", "Zimbabwe");
        }

        public void LoadStates()
        {
            Canada_States.Add("AB", "Alberta"); Canada_States.Add("BC", "British Columbia"); Canada_States.Add("MB", "Manitoba");
            Canada_States.Add("NB", "New Brunswick"); Canada_States.Add("NL", "Newfoundland and Labrador"); Canada_States.Add("NT", "Northwest Territories");
            Canada_States.Add("NS", "Nova Scotia"); Canada_States.Add("NU", "Nunavut"); Canada_States.Add("ON", "Ontario");
            Canada_States.Add("PE", "Prince Edward Island"); Canada_States.Add("QC", "Quebec"); Canada_States.Add("SK", "Saskatchewan");
            Canada_States.Add("YT", "Yukon");

            Australia_States.Add("ACT", "Capital Territory"); Australia_States.Add("NSW", "New South Wales"); Australia_States.Add("NT", "Northern Territory");
            Australia_States.Add("QLD", "Queensland"); Australia_States.Add("SA", "South Australia"); Australia_States.Add("TAS", "Tasmania");
            Australia_States.Add("VIC", "Victoria"); Australia_States.Add("WA", "Western Australia");

            US_States.Add("AL", "Alabama"); US_States.Add("AK", "Alaska"); US_States.Add("AZ", "Arizona");
            US_States.Add("AR", "Arkansas"); US_States.Add("CA", "California"); US_States.Add("CO", "Colorado");
            US_States.Add("CT", "Connecticut"); US_States.Add("DE", "Delaware"); US_States.Add("FL", "Florida");
            US_States.Add("GA", "Georgia"); US_States.Add("HI", "Hawaii"); US_States.Add("ID", "Idaho");
            US_States.Add("IL", "Illinois"); US_States.Add("IN", "Indiana"); US_States.Add("IA", "Iowa");
            US_States.Add("KS", "Kansas"); US_States.Add("KY", "Kentucky"); US_States.Add("LA", "Louisiana");
            US_States.Add("ME", "Maine"); US_States.Add("MD", "Maryland"); US_States.Add("MA", "Massachusetts");
            US_States.Add("MI", "Michigan"); US_States.Add("MN", "Minnesota"); US_States.Add("MS", "Mississippi");
            US_States.Add("MO", "Missouri"); US_States.Add("MT", "Montana"); US_States.Add("NE", "Nebraska");
            US_States.Add("NV", "Nevada"); US_States.Add("NH", "New Hampshire"); US_States.Add("NJ", "New Jersey");
            US_States.Add("NM", "New Mexico"); US_States.Add("NY", "New York"); US_States.Add("NC", "North Carolina");
            US_States.Add("ND", "North Dakota"); US_States.Add("OH", "Ohio"); US_States.Add("OK", "Oklahoma");
            US_States.Add("OR", "Oregon"); US_States.Add("PA", "Pennsylvania"); US_States.Add("RI", "Rhode Island");
            US_States.Add("SC", "South Carolina"); US_States.Add("SD", "South Dakota"); US_States.Add("TN", "Tennessee");
            US_States.Add("TX", "Texas"); US_States.Add("UT", "Utah"); US_States.Add("VT", "Vermont");
            US_States.Add("VA", "Virginia"); US_States.Add("WA", "Washington"); US_States.Add("WV", "West Virginia");
            US_States.Add("WI", "Wisconsin"); US_States.Add("WY", "Wyoming");

        }

        public void LoadAutoCaptchaServices()
        {
            string FileName = _DefaultLocation + @"\Tickets\autocaptchaservices.tacs";

            if (File.Exists(FileName))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(FileName, FileMode.Open))
                {
                    // Deserialize object                    
                    this._AutoCaptchaServices = (AutoCaptchaServices)formatter.Deserialize(stream);
                }
            }
            else
            {
                this._AutoCaptchaServices = new AutoCaptchaServices();
            }

            try
            {
                if (this.Tickets != null)
                {
                    IEnumerable<AXSTicket> runningTickets = this.Tickets.Where(p => p.isRunning);
                    if (runningTickets != null)
                    {
                        if (runningTickets.Count() > 0)
                        {
                            for (int i = 0; i < runningTickets.Count(); i++)
                            {
                                runningTickets.ElementAt(i).AutoCaptchaServices = this._AutoCaptchaServices;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void SaveTickets()
        {
            if (this._Tickets != null)
            {
                if (this._Tickets.Count > 0)
                {
                    foreach (ITicket ticket in this._Tickets)
                    {
                        ticket.SaveTicket();
                    }
                }
            }
        }

        public void SaveAccounts()
        {
            string FileName = _DefaultLocation + @"\Tickets\accounts.taccounts";
            _Accounts.Save(FileName);
        }

        public void SaveProxies()
        {
            string FileName = _DefaultLocation + @"\Tickets\proxy.tproxy";
            _Proxies.Save(FileName);
        }

        public void SaveEmails()
        {
            string FileName = _DefaultLocation + @"\Tickets\email.temail";
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, this._EmailSetting);
            }
        }

        public void SaveGlobalSettings()
        {
            string FileName = _DefaultLocation + @"\Tickets\globalSettings.settings";
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, this.GlobalSetting);
            }
        }

        public void SaveGroups()
        {
            string FileName = _DefaultLocation + @"\Tickets\groups.tgroup";
            if (this.Groups != null)
            {
                this.Groups.Save(FileName);
            }
        }

        public void SaveDeliveryOptions()
        {
            string FileName = _DefaultLocation + @"\Tickets\DeliveryOptions.tDeliveryOption";
            if (this.TicketDeliveryOptions != null)
            {
                this.TicketDeliveryOptions.Save(FileName);
            }
        }

        public void SaveAutoCaptchaServices()
        {
            string FileName = _DefaultLocation + @"\Tickets\autocaptchaservices.tacs";
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, this._AutoCaptchaServices);
            }
        }

        public void SaveLogs()
        {
            Automatick.Logging.Logger.LoggerInstance.SaveLogFile();
        }

        public SortableBindingList<AXSTicket> filterTickets(TicketGroup group)
        {
            SortableBindingList<AXSTicket> filteredList = null;
            if (group.GroupName == "All Groups")
            {
                filteredList = this._Tickets;
            }
            else
            {
                if (this._Tickets != null)
                {
                    if (this._Tickets.Count > 0)
                    {
                        IEnumerable<AXSTicket> filteredTickets = this._Tickets.Where(p => p.TicketGroup.GroupId == group.GroupId);
                        filteredList = new SortableBindingList<AXSTicket>();
                        if (filteredTickets != null)
                        {
                            if (filteredTickets.Count() > 0)
                            {
                                foreach (AXSTicket item in filteredTickets)
                                {
                                    filteredList.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            return filteredList;
        }

        public SortableBindingList<AXSTicket> filterTickets(TicketGroup group, String keyword)
        {
            SortableBindingList<AXSTicket> filteredList = this.filterTickets(group);

            if (!String.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim();
            }

            if (filteredList != null && !String.IsNullOrEmpty(keyword))
            {
                IEnumerable<AXSTicket> filteredTickets = filteredList.Where(p => p.TicketName.ToLower().Contains(keyword.ToLower()));
                filteredList = new SortableBindingList<AXSTicket>();
                if (filteredTickets != null)
                {
                    if (filteredTickets.Count() > 0)
                    {
                        foreach (AXSTicket item in filteredTickets)
                        {
                            filteredList.Add(item);
                        }
                    }
                }
            }

            return filteredList;
        }
    }
}
