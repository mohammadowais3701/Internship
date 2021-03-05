using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SortedBindingList;

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

  

        public String DefaultFileLocation
        {
            get { return _DefaultLocation; }
        }

        public ApplicationStartUp(String defaultLocation)
        {
            _DefaultLocation = defaultLocation;
            _Tickets = new SortableBindingList<AXSTicket>();
            if (!Directory.Exists(_DefaultLocation + @"\Tickets\"))
            {
                Directory.CreateDirectory(_DefaultLocation + @"\Tickets\");
            }
            _SoundAlert = new System.Media.SoundPlayer(global::Automatick.Properties.Resources.TicketFound);
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
                        if (runningTickets != null)
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
                        this._Tickets.Add(ticket);
                    }
                }
            }
            catch (Exception)
            {

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

        public void LoadAccounts()
        {
            string FileName = _DefaultLocation + @"\Tickets\accounts.taccounts";

            if (File.Exists(FileName))
            {
                _Accounts = new SortableBindingList<AXSTicketAccount>();
                _Accounts.Load(FileName);
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

      

        public void SaveTickets()
        {
            if (this._Tickets != null)
            {
                if (this._Tickets.Count>0)
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

            if (filteredList!= null && !String.IsNullOrEmpty(keyword))
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
