using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Automatick.Core;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;

namespace Automatick
{
    public partial class frmTicket : C1.Win.C1Ribbon.C1RibbonForm, IAddTicket
    {
        private AccessRights.AccessList CapsiumServers = null;

        public frmTicket(IMainForm mainForm)
        {
            InitializeComponent();
            this.MainForm = mainForm;
        }

        public frmTicket(IMainForm mainForm, ITicket ticket)
        {
            InitializeComponent();
            this.MainForm = mainForm;
            this.Ticket = ticket;
        }

        private void frmTicket_Load(object sender, EventArgs e)
        {
            this.load();
            this.applyPermission();
            this.Ticket.ifCapsium = false;
        }
        private void applyPermission()
        {
            try
            {
                IEnumerable<AccessRights.AccessList> allControlsAccesses = this.MainForm.AppPermissions.AllAccessList.Where(p => p.form == this.Name);
                Boolean isWebAllowed = false;
                foreach (AccessRights.AccessList obj in allControlsAccesses)
                {

                    if (obj.name == AXSByPassWaitingRoom.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        AXSByPassWaitingRoom.Visible = access;
                        if (!access)
                        {
                            this.Ticket.Permission = false;
                            AXSByPassWaitingRoom.Checked = false;
                        }
                    }
                    else if (obj.name == this.chkMobile.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        chkMobile.Visible = access;
                        if (!access)
                        {
                            this.Ticket.isMobile = false;
                            this.chkMobile.Checked = false;
                        }
                    }
                    else if (obj.name == this.chkWeb.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        chkWeb.Visible = access;
                        isWebAllowed = access;
                        if (!access)
                        {
                            this.Ticket.isWeb = false;
                            this.chkWeb.Checked = false;
                        }
                    }
                    else if (obj.name == "chkEventko")
                    {
                        Boolean access = Boolean.Parse(obj.access);

                        if (!isWebAllowed && access)
                        {
                            chkWeb.Visible = access;
                            chkWeb.Checked = access;
                        }
                        if (!access)
                        {
                            this.Ticket.isEventko = false;
                        }
                    }
                }
                IEnumerable<AccessRights.AccessList> allControlsAccesses1 = this.MainForm.AppPermissions.AllAccessList.Where(p => p.form == "Core");
                foreach (AccessRights.AccessList obj in allControlsAccesses1)
                {
                    if (obj.name == AXSByPassWaitingRoom.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        AXSByPassWaitingRoom.Visible = access;
                        if (!access)
                        {
                            this.Ticket.Permission = false;
                            AXSByPassWaitingRoom.Checked = false;
                        }
                    }
                }
                AccessRights.AccessList accessObj = allControlsAccesses.FirstOrDefault(p => p.access.ToLower() == "true");
            }
            catch (Exception)
            {

            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.save();
        }

        #region IAddTicket Members
        public ITicket Ticket
        {
            get;
            set;
        }
        public IMainForm MainForm
        {
            get;
            set;
        }
        public Boolean validate()
        {
            Boolean result = false;

            if (MainForm.AppStartUp.Tickets.FirstOrDefault(p => p.TicketName == txtTicketName.Text.Trim()) != null)
            {
                ITicket tmpticket = MainForm.AppStartUp.Tickets.FirstOrDefault(p => p.TicketName == txtTicketName.Text.Trim());
                if (tmpticket != this.Ticket)
                {
                    result = false;
                    MessageBox.Show("Please provide the unique ticket name.");
                    txtTicketName.Focus();
                    return result;
                }
            }

            if (!String.IsNullOrEmpty(this.txtTicketURL.Text))
            {
                Match m = Regex.Match(this.txtTicketURL.Text, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
                if (m.Success)
                {
                    this.Ticket.constTicketURL = this.Ticket.URL = this.txtTicketURL.Text;

                    if (this.Ticket.URL.Contains("shop.axs.co.uk") || this.Ticket.URL.Contains("q.axs.co.uk"))
                    {
                        this.Ticket.IsUkEvent = true;
                    }

                    result = true;
                }
                else
                {
                    result = false;
                    MessageBox.Show("Invalid URL. Please provide the valid ticket URL.");
                    txtTicketURL.Focus();
                    return result;
                }
            }
            else
            {
                result = false;
                MessageBox.Show("Please provide the ticket URL.");
                txtTicketURL.Focus();
                return result;
            }

            if (this.Ticket.Parameters != null)
            {
                if (this.Ticket.Parameters.Count > 0)
                {
                    //TODO: validate all parameters. Info: Already handled in the cell validating event.
                }
                else
                {
                    result = false;
                    MessageBox.Show("Please provide at least one ticket parameter.");
                    return result;
                }
            }
            else
            {
                result = false;
                MessageBox.Show("Please provide at least one ticket parameter.");
                return result;
            }


            if (this.Ticket.URL.Contains("evenko.ca") && !this.MainForm.AppStartUp.GlobalSetting.ifEventko)
            {
                MessageBox.Show("Evenko events are not allowed");
                return false;
            }
            else if (this.Ticket.URL.Contains("axs.com") && (this.MainForm.AppStartUp.GlobalSetting.ifEventko && !this.MainForm.AppStartUp.GlobalSetting.IfWeb && !this.MainForm.AppStartUp.GlobalSetting.ifMobile))
            {
                MessageBox.Show("AXS events are not allowed");
                return false;
            }

            if (this.Ticket.URL.Contains("evenko.ca"))
            {
                this.Ticket.isEventko = this.chkWeb.Checked;
                this.Ticket.isWeb = false;
            }
            else if (this.Ticket.URL.Contains("axs.com"))
            {
                this.Ticket.isEventko = false;
                this.Ticket.isWeb = this.chkWeb.Checked;
            }

            this.Ticket.isMobile = this.chkMobile.Checked;

            if (!this.Ticket.isWeb && !this.Ticket.isMobile && !this.Ticket.isEventko)
            {
                if (!this.Ticket.URL.Contains("evenko.ca"))
                {
                    if (this.chkMobile.Visible && this.chkWeb.Visible)
                    {
                        MessageBox.Show("Please select an option from Web/Mobile to search.");
                    }
                    else if (this.chkMobile.Visible)
                    {
                        MessageBox.Show("Please select Mobile option to search.");
                    }
                    else if (this.chkWeb.Visible)
                    {
                        MessageBox.Show("Please select Web option to search.");
                    }
                    return false;
                }
                else
                {
                    this.Ticket.isEventko = true;
                }
            }


            if (this.rbSelectDeliveryOptionList.Checked)
            {
                this.iTicketDeliveryOptionBindingSource.EndEdit();
                IEnumerable<AXSDeliveryOption> selectedDeliveryOptions = this.MainForm.AppStartUp.TicketDeliveryOptions.Where(p => p.IfSelected == true);
                if (selectedDeliveryOptions != null)
                {
                    if (selectedDeliveryOptions.Count() > 0)
                    {
                        if (this.Ticket.SelectedDeliveryOptions == null)
                        {
                            this.Ticket.SelectedDeliveryOptions = new SortedBindingList.SortableBindingList<AXSDeliveryOption>();
                        }
                        this.Ticket.SelectedDeliveryOptions.Clear();

                        foreach (AXSDeliveryOption item in selectedDeliveryOptions)
                        {
                            this.Ticket.SelectedDeliveryOptions.Add(item);
                        }
                    }
                    else
                    {
                        result = false;
                        MessageBox.Show("Please select/mark at least one delivery option from the list.");
                        docParameters.SelectedTab = tabPageMoreParameters;
                        return result;
                    }
                }
                else
                {
                    result = false;
                    MessageBox.Show("Please select/mark at least one delivery option from the list.");
                    docParameters.SelectedTab = tabPageMoreParameters;
                    return result;
                }
            }

            if (this.rbSelectAccountList.Checked)
            {
                this.iTicketAccountBindingSource.EndEdit();
                IEnumerable<AXSTicketAccount> selectedAccounts = this.MainForm.AppStartUp.Accounts.Where(p => p.IfSelected == true);
                if (selectedAccounts != null)
                {
                    if (selectedAccounts.Count() > 0)
                    {
                        if (this.Ticket.SelectedAccounts == null)
                        {
                            this.Ticket.SelectedAccounts = new SortedBindingList.SortableBindingList<AXSTicketAccount>();
                        }
                        this.Ticket.SelectedAccounts.Clear();

                        foreach (AXSTicketAccount item in selectedAccounts)
                        {
                            this.Ticket.SelectedAccounts.Add(item);
                        }
                    }
                    else
                    {
                        result = false;
                        MessageBox.Show("Please select/mark at least one account from the list.");
                        docParameters.SelectedTab = tabPageMoreParameters;
                        return result;
                    }
                }
                else
                {
                    result = false;
                    MessageBox.Show("Please select/mark at least one account from the list.");
                    docParameters.SelectedTab = tabPageMoreParameters;
                    return result;
                }
            }

            try
            {
                List<TicketGroup> listGroup = this.MainForm.AppStartUp.Groups.ToList();
                TicketGroup defaultGroup = listGroup.FirstOrDefault(p => p.GroupName == "Default Group" && p.GroupId == "Default Group");
                this.Ticket.TicketGroup = defaultGroup;
            }
            catch (Exception)
            {

            }


            this.Ticket.Notes = this.txtNotes.Text;
            this.Ticket.DeliveryCountry = this.txtDeliveryCountry.Text;
            this.Ticket.DeliveryOption = this.txtDeliveryOption.Text;

            this.Ticket.ifSchedule = false;
            this.Ticket.ScheduleDateTime = DateTime.Now;
            this.Ticket.ScheduleRunningTime = 15;
            this.Ticket.NoOfSearches = 1;
            this.Ticket.ifAutoBuy = this.chkAutoBuy.Checked;
            this.Ticket.ifAutoBuyWitoutProxy = this.chkAutoBuyWitoutProxy.Checked;
            this.Ticket.ifDistributeInSearches = this.rbDistributeInSearches.Checked;
            this.Ticket.ifPlaySoundAlert = true;
            this.Ticket.ifRandomDelay = this.chkRandomDelay.Checked; ;
            this.Ticket.ifSelectDeliveryOptionAutoBuying = this.rbSelectDeliveryOptionAutoBuying.Checked;
            this.Ticket.ifSelectDeliveryOptionList = this.rbSelectDeliveryOptionList.Checked;
            this.Ticket.ifSelectAccountAutoBuying = this.rbSelectAccountAutoBuying.Checked;
            this.Ticket.ifSelectAccountList = this.rbSelectAccountList.Checked;
            this.Ticket.ifSendEmail = false;
            this.Ticket.ifUseFoundOnFirstAttempt = this.rbUseFoundOnFirstAttempt.Checked;
            this.Ticket.ifUseAvailableParameters = this.rbUseAvailableParameters.Checked;
            this.Ticket.ifUseProxies = true;
            this.Ticket.ResetSearchDelay = this.nudResetSearchDelay.Value; ;
            this.Ticket.StartUsingProxiesFrom = 1;
            this.Ticket.DelayForAutoBuy = this.Delay.Value;
            this.Ticket.Permission = this.AXSByPassWaitingRoom.Checked;

            this.Ticket.isWaitingEvent = this.chkIsWaiting.Checked;

            this.Ticket.Permission = this.AXSByPassWaitingRoom.Checked;

            String url = this.Ticket.URL;
            url = url.Replace(";;;slash;;;", "/");
            url = url.Replace("%2F", "/");
            String[] query = url.Split('&');

            foreach (var item in query)
            {
                if (item.Split('=')[0].ToLower().Equals("eventid"))
                {
                    this.Ticket.EventID = item.Split('=')[1];
                    break;
                }
                else if (item.Split('=')[0].ToLower().Equals("event"))
                {
                    this.Ticket.EventID = item.Split('=')[1];
                    break;
                }
            }

            this.Ticket.WR = HttpUtility.ParseQueryString((new Uri(this.Ticket.URL)).Query).Get("wr");

            if (string.IsNullOrEmpty(this.Ticket.WR))
            {
                if (this.Ticket.URL.Contains("/shop/"))
                {
                    String w = this.Ticket.URL.Substring(this.Ticket.URL.IndexOf("#") + 2);
                    string[] split = w.Split('/');
                    if (split[0].Contains("?"))
                    {
                        split = split[0].Split('?');
                        this.Ticket.WR = split[0];
                    }
                    else
                    {
                        this.Ticket.WR = split[0];
                    }
                }
            }

            if (string.IsNullOrEmpty(this.Ticket.WR))
            {
                String[] _query = this.Ticket.URL.Substring(this.Ticket.URL.IndexOf('?') + 1).Split('&');

                foreach (var item in _query)
                {
                    if (item.Split('=')[0].ToLower().Equals("wr"))
                    {
                        this.Ticket.WR = item.Split('=')[1];
                        break;
                    }
                }
            }

            if (!String.IsNullOrEmpty(this.txtTicketName.Text))
            {
                int index = -1;
                AXSTicket ticketToRemove = MainForm.AppStartUp.Tickets.FirstOrDefault(p => p.TicketName == this.Ticket.TicketName);

                if (!String.IsNullOrEmpty(this.Ticket.TicketName) && ticketToRemove != null)
                {
                    index = this.MainForm.AppStartUp.Tickets.IndexOf(ticketToRemove);
                    this.MainForm.AppStartUp.Tickets.Remove(ticketToRemove);
                }
                this.Ticket.DeleteTicket();
                this.Ticket.TicketName = this.txtTicketName.Text.Trim();

                if (index >= 0)
                {
                    this.MainForm.AppStartUp.Tickets.Insert(index, (AXSTicket)this.Ticket);
                }
                else
                {
                    this.MainForm.AppStartUp.Tickets.Add((AXSTicket)this.Ticket);
                }

                if (this.Ticket.isWaitingEvent && !String.IsNullOrEmpty(this.Ticket.WR))
                {
                    this.Ticket.sendURLToBrowser((AXSTicket)Ticket, this.MainForm.Lic, this.MainForm.AppStartUp.DefaultFileLocation);
                }

                this.Ticket.FileLocation = this.MainForm.AppStartUp.DefaultFileLocation;

                this.Ticket.SaveTicket();

                result = true;
            }
            else
            {
                result = false;
                MessageBox.Show("Please provide the ticket name");
                return result;
            }

            return result;
        }

        public void load()
        {
            if (this.Ticket != null)
            {
                this.Text = "Edit " + this.Ticket.TicketName;
            }
            else
            {
                this.Text = "Add new event";
            }
            if (this.Ticket == null)
            {
                this.Ticket = new AXSTicket();
                this.Ticket.ResetSearchDelay = 15;
            }

            this.txtTicketURL.Text = this.Ticket.URL;
            this.txtTicketName.Text = this.Ticket.TicketName;
            this.txtNotes.Text = this.Ticket.Notes;
            this.txtDeliveryCountry.Text = this.Ticket.DeliveryCountry;
            this.txtDeliveryOption.Text = this.Ticket.DeliveryOption;
            this.AXSByPassWaitingRoom.Checked = this.Ticket.Permission;
            this.chkAutoBuy.Checked = this.Ticket.ifAutoBuy;
            this.chkAutoBuyWitoutProxy.Checked = this.Ticket.ifAutoBuyWitoutProxy;
            this.rbDistributeInSearches.Checked = this.Ticket.ifDistributeInSearches;
            this.rbSelectDeliveryOptionAutoBuying.Checked = this.Ticket.ifSelectDeliveryOptionAutoBuying;
            this.rbSelectDeliveryOptionList.Checked = this.Ticket.ifSelectDeliveryOptionList;
            this.rbSelectAccountAutoBuying.Checked = this.Ticket.ifSelectAccountAutoBuying;
            this.rbSelectAccountList.Checked = this.Ticket.ifSelectAccountList;
            this.rbUseFoundOnFirstAttempt.Checked = this.Ticket.ifUseFoundOnFirstAttempt;
            this.rbUseAvailableParameters.Checked = this.Ticket.ifUseAvailableParameters;
            this.Delay.Value = this.Ticket.DelayForAutoBuy;
            this.nudResetSearchDelay.Value = this.Ticket.ResetSearchDelay < 15 ? 15 : this.Ticket.ResetSearchDelay;
            this.chkRandomDelay.Checked = this.Ticket.ifRandomDelay;
            this.chkIsWaiting.Checked = this.Ticket.isWaitingEvent;

            if (this.Ticket.Parameters != null)
            {
                this.iTicketParameterBindingSource.DataSource = this.Ticket.Parameters;
            }

            if (this.Ticket.TicketFoundCriterions != null)
            {
                this.iTicketFoundCriteriaBindingSource.DataSource = this.Ticket.TicketFoundCriterions;
            }

            if (!this.Ticket.isWeb && !this.Ticket.isMobile && !this.Ticket.isEventko)
            {
                this.Ticket.isWeb = true;
            }

            if (this.MainForm.AppStartUp.GlobalSetting.ifEventko && this.Ticket.isEventko)
            {
                this.chkWeb.Checked = true;
            }
            else if (this.MainForm.AppStartUp.GlobalSetting.IfWeb)
            {
                this.chkWeb.Checked = this.Ticket.isWeb;
            }

            this.chkMobile.Checked = this.Ticket.isMobile;

            this.populateDeliveryOptions();
            this.populateAccounts();
            this.selectedDeliveryOptionCheckChanged();
            this.selectedAccountCheckedChanged();
            this.autoBuyCheckChanged();
        }

        private void populateDeliveryOptions()
        {
            if (this.MainForm.AppStartUp.TicketDeliveryOptions != null)
            {
                this.MainForm.loadDeliveryOptions();
                if (this.Ticket.SelectedDeliveryOptions != null)
                {
                    if (this.Ticket.SelectedDeliveryOptions.Count > 0)
                    {
                        for (int i = 0; i < this.MainForm.AppStartUp.TicketDeliveryOptions.Count; i++)
                        {
                            this.MainForm.AppStartUp.TicketDeliveryOptions[i].IfSelected = false;
                        }

                        foreach (AXSDeliveryOption deliveryOption in this.Ticket.SelectedDeliveryOptions)
                        {
                            AXSDeliveryOption dopt = this.MainForm.AppStartUp.TicketDeliveryOptions.FirstOrDefault(p => p.DeliveryOptionId == deliveryOption.DeliveryOptionId && p.DeliveryOption == deliveryOption.DeliveryOption && p.DeliveryCountry == deliveryOption.DeliveryCountry);
                            if (dopt != null)
                            {
                                dopt.IfSelected = true;
                            }
                        }
                    }
                }
                this.iTicketDeliveryOptionBindingSource.DataSource = this.MainForm.AppStartUp.TicketDeliveryOptions;
            }
        }

        private void populateAccounts()
        {
            if (this.MainForm.AppStartUp.Accounts != null)
            {
                this.MainForm.loadAccounts();
                if (this.Ticket.SelectedAccounts != null)
                {
                    if (this.Ticket.SelectedAccounts.Count > 0)
                    {
                        for (int i = 0; i < this.MainForm.AppStartUp.Accounts.Count; i++)
                        {
                            this.MainForm.AppStartUp.Accounts[i].IfSelected = false;
                        }

                        foreach (AXSTicketAccount account in this.Ticket.SelectedAccounts)
                        {
                            AXSTicketAccount acc = this.MainForm.AppStartUp.Accounts.FirstOrDefault(p => p.EmailAddress == account.EmailAddress);
                            if (acc != null)
                            {
                                acc.IfSelected = true;
                                acc.BuyingLimit = account.BuyingLimit;
                            }
                        }
                    }
                }
                this.iTicketAccountBindingSource.DataSource = this.MainForm.AppStartUp.Accounts;
            }
        }

        public void onClosed()
        {
            GC.SuppressFinalize(this);
            //GC.Collect();
        }
        public void save()
        {
            if (this.validate())
            {
                this.Close();
            }
        }

        #endregion

        private void btnEmailSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmEmail frm = new frmEmail(this.MainForm);
            frm.ShowDialog();
        }

        private void btnAutoBuyOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            docParameters.SelectedTab = tabPageMoreParameters;
        }

        private void btnManageDeliveryOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDeliveryOption frm = new frmDeliveryOption(this.MainForm);
            frm.ShowDialog();
            this.populateDeliveryOptions();
        }


        private void rbSelectDeliveryOptionAutoBuying_CheckedChanged(object sender, EventArgs e)
        {
            this.selectedDeliveryOptionCheckChanged();
        }

        private void selectedDeliveryOptionCheckChanged()
        {
            if (rbSelectDeliveryOptionAutoBuying.Checked)
            {
                txtDeliveryOption.Enabled = true;
                lblDeliveryOption.Enabled = true;
                txtDeliveryCountry.Enabled = true;
                lblDeliveryCountry.Enabled = true;
                btnManageDeliveryOptions.Enabled = false;
                gvDeliveryOptions.Enabled = false;
            }
            else
            {
                txtDeliveryOption.Enabled = false;
                lblDeliveryOption.Enabled = false;
                txtDeliveryCountry.Enabled = false;
                lblDeliveryCountry.Enabled = false;
                btnManageDeliveryOptions.Enabled = true;
                gvDeliveryOptions.Enabled = true;
            }
        }

        private void chkAutoBuy_CheckedChanged(object sender, EventArgs e)
        {
            this.autoBuyCheckChanged();
        }

        private void autoBuyCheckChanged()
        {
            if (chkAutoBuy.Checked)
            {
                btnAutoBuyOptions.Enabled = true;
                chkAutoBuyWitoutProxy.Enabled = true;
            }
            else
            {
                btnAutoBuyOptions.Enabled = false;
                chkAutoBuyWitoutProxy.Enabled = false;
            }
        }

        private void frmTicket_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.onClosed();
        }

        private void gvParameters_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (gvParameters.CurrentCell.ValueType == typeof(Int32))
                {
                    int value;
                    if (!int.TryParse(e.FormattedValue.ToString(), out value))
                    {
                        gvParameters.CurrentCell.Value = 1;
                    }
                    else if (gvParameters.CurrentCell.OwningColumn.Name == "quantity" && value <= 0)
                    {
                        gvParameters.CurrentCell.Value = 1;
                    }
                }
                else if (gvParameters.CurrentCell.ValueType == typeof(Int32?) && gvParameters.CurrentRow.DataBoundItem != null)
                {
                    int value;
                    if (!int.TryParse(e.FormattedValue.ToString(), out value))
                    {
                        gvParameters.CurrentCell.Value = null;
                        if (gvParameters.CurrentCell.OwningColumn.Name == "priceMin")
                        {
                            AXSParameter parameter = (AXSParameter)gvParameters.CurrentRow.DataBoundItem;
                            gvParameters.CurrentCell.Value = null;
                            parameter.PriceMax = null;
                        }
                        if (gvParameters.CurrentCell.OwningColumn.Name == "priceMax")
                        {
                            AXSParameter parameter = (AXSParameter)gvParameters.CurrentRow.DataBoundItem;
                            gvParameters.CurrentCell.Value = null;
                            parameter.PriceMin = null;
                        }
                    }
                    else if (gvParameters.CurrentCell.OwningColumn.Name == "priceMin")
                    {
                        if (gvParameters.CurrentRow.DataBoundItem is AXSParameter)
                        {
                            AXSParameter parameter = (AXSParameter)gvParameters.CurrentRow.DataBoundItem;
                            if (parameter.PriceMax == null)
                            {
                                parameter.PriceMax = value + 1;
                            }
                            else if (parameter.PriceMax.HasValue)
                            {
                                if (value <= 0)
                                {
                                    gvParameters.CurrentCell.Value = null;
                                    parameter.PriceMax = null;
                                }
                                else if (parameter.PriceMax.Value < value)
                                {
                                    parameter.PriceMax = value + 1;
                                }
                            }
                        }
                    }
                    else if (gvParameters.CurrentCell.OwningColumn.Name == "priceMax")
                    {
                        if (gvParameters.CurrentRow.DataBoundItem is AXSParameter)
                        {
                            AXSParameter parameter = (AXSParameter)gvParameters.CurrentRow.DataBoundItem;
                            if (parameter.PriceMin == null)
                            {
                                parameter.PriceMin = value - 1;
                            }
                            else if (parameter.PriceMin.HasValue)
                            {
                                if (value <= 0)
                                {
                                    gvParameters.CurrentCell.Value = null;
                                    parameter.PriceMin = null;
                                }
                                else if (parameter.PriceMin.Value > value)
                                {
                                    parameter.PriceMin = value - 1;
                                }
                            }
                        }
                    }
                }


                if (!String.IsNullOrEmpty(e.FormattedValue.ToString()))
                {


                    AXSParameter parameter = (AXSParameter)gvParameters.CurrentRow.DataBoundItem;
                    if (e.ColumnIndex == 3)
                    {
                        try
                        {
                            if (e.FormattedValue.ToString() != "mm/dd/yyyy")
                            {
                                CultureInfo provider = new CultureInfo("en-US");
                                DateTime dt = DateTime.ParseExact(e.FormattedValue.ToString(), "MM/dd/yyyy", provider);
                                //DateTime dt = Convert.ToDateTime(e.FormattedValue.ToString());
                                //DateTime dt2 = DateTime.Parse(parameter.DateTimeString, culture);
                                parameter.DateTimeString = dt.ToString("MM/dd/yyyy");//String.Format("{0:MM/dd/yyyy}", dt);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Date is in invalid format. Please correct.(e.g. 12/01/2013 or 01/12/2013)");
                            e.Cancel = true;

                        }
                    }
                    if (e.ColumnIndex == 4)
                    {
                        try
                        {
                            if (e.FormattedValue.ToString() != "hh:mm")
                            {
                                parameter.EventTime = dtSelect.Value.ToShortTimeString().ToString();
                                gvParameters.CurrentCell.Value = parameter.EventTime;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Time is in invalid format.");
                            e.Cancel = true;

                        }
                    }
                }
            }

        }

        private void gvParameters_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (iTicketParameterBindingSource.Count <= 1)
            {
                e.Cancel = true;
                MessageBox.Show("There must be one parameter row defined, therefore you could not deleted this parameter row.");
            }
        }

        private void rbSelectAccountAutoBuying_CheckedChanged(object sender, EventArgs e)
        {
            this.selectedAccountCheckedChanged();
        }

        private void selectedAccountCheckedChanged()
        {
            if (rbSelectAccountAutoBuying.Checked)
            {
                this.gvAccounts.Enabled = false;
                this.btnManageAccounts.Enabled = false;
            }
            else
            {
                this.gvAccounts.Enabled = true;
                this.btnManageAccounts.Enabled = true;
            }
        }

        private void gvAccounts_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (gvAccounts.CurrentCell.ValueType == typeof(Int32))
                {
                    int value;
                    if (!int.TryParse(e.FormattedValue.ToString(), out value))
                    {
                        gvAccounts.CurrentCell.Value = 1;
                    }
                    else if (gvAccounts.CurrentCell.OwningColumn.Name == "BuyingLimit" && value <= 0)
                    {
                        gvAccounts.CurrentCell.Value = 0;
                    }
                }
            }
        }

        private void btnManageAccounts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAccount frm = new frmAccount(this.MainForm);
            frm.ShowDialog();
            this.populateAccounts();
        }



        private void gvParameters_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (gvParameters.Columns[e.ColumnIndex].DataPropertyName == "DateTimeString")
                {
                    if (String.IsNullOrEmpty(Convert.ToString(e.Value)))
                    {
                        gvParameters.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "mm/dd/yyyy";
                        // gvParameters.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gvParameters.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                        gvParameters.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Gray;
                    }
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (gvParameters.Columns[e.ColumnIndex].DataPropertyName == "EventTime")
                {
                    if (String.IsNullOrEmpty(Convert.ToString(e.Value)))
                    {
                        gvParameters.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "hh:mm";
                        // gvParameters.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gvParameters.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                        gvParameters.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Gray;
                    }
                }
            }
            catch (Exception)
            {

            }
        }


        private void gvParameters_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (gvParameters.Columns[e.ColumnIndex].DataPropertyName == "DateTimeString")
                {
                    mcSelectDate.Visible = true;
                    IContainerControl c = gvParameters.GetContainerControl();
                    Rectangle r = gvParameters.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    mcSelectDate.Top = r.Top + r.Height + 1;
                    mcSelectDate.Left = r.Left;

                }
                else
                {
                    mcSelectDate.Visible = false;
                }
                try
                {
                    if (gvParameters.Columns[e.ColumnIndex].DataPropertyName == "DateTimeString")
                    {
                        string cellValue = Convert.ToString(((DataGridView)sender).CurrentCell.FormattedValue);

                        if (!String.IsNullOrEmpty(cellValue))
                        {
                            if (cellValue == "mm/dd/yyyy")
                            {
                                gvParameters.CurrentCell.Value = "";
                                //gvParameters.CurrentCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                gvParameters.CurrentCell.Style.BackColor = Color.White;
                                gvParameters.CurrentCell.Style.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }

                if (gvParameters.Columns[e.ColumnIndex].DataPropertyName == "EventTime")
                {
                    dtSelect.Visible = true;
                    IContainerControl c = gvParameters.GetContainerControl();
                    Rectangle r = gvParameters.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    dtSelect.Top = r.Top;// +r.Height;// +1;
                    dtSelect.Left = r.Left;
                    this.dtSelect.Size = r.Size;

                }
                else
                {
                    dtSelect.Visible = false;
                }
                try
                {

                    if (gvParameters.Columns[e.ColumnIndex].DataPropertyName == "EventTime")
                    {
                        string cellValue = Convert.ToString(((DataGridView)sender).CurrentCell.FormattedValue);

                        if (!String.IsNullOrEmpty(cellValue))
                        {
                            if (cellValue == "hh:mm")
                            {
                                gvParameters.CurrentCell.Value = "";
                                // gvParameters.CurrentCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                gvParameters.CurrentCell.Style.BackColor = Color.White;
                                gvParameters.CurrentCell.Style.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            catch (Exception)
            {

            }
        }

        private void gvParameters_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (gvParameters.CurrentCell.ColumnIndex == 4)
            {
                //dtSelect.Visible = false;
            }
            else if (gvParameters.CurrentCell.ColumnIndex == 3)
            {
                //mcSelectDate.Visible = false;
            }
        }

        private void mcSelectDate_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (gvParameters.CurrentCell.ColumnIndex == 3)
            {
                gvParameters.CurrentCell.Value = e.End.ToString("MM/dd/yyyy");
            }

            //   if (gvParameters.Columns[e.ColumnIndex].DataPropertyName == "DateTimeString" && mcSelectDate.Visible)
            {
                mcSelectDate.Visible = false;
            }

        }

        private void gvParameters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            //if (gvParameters.Columns[e.ColumnIndex].DataPropertyName == "DateTimeString" && mcSelectDate.Visible)
            //{
            //    mcSelectDate.Visible = false;
            //}
            try
            {

                if (gvParameters.Columns[e.ColumnIndex].DataPropertyName == "DateTimeString")
                {
                    string cellValue = Convert.ToString(((DataGridView)sender).CurrentCell.FormattedValue);

                    if (!String.IsNullOrEmpty(cellValue))
                    {

                        // gvParameters.CurrentCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        gvParameters.CurrentCell.Style.BackColor = Color.White;
                        gvParameters.CurrentCell.Style.ForeColor = Color.Black;


                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {

                if (gvParameters.Columns[e.ColumnIndex].DataPropertyName == "EventTime")
                {
                    string cellValue = Convert.ToString(((DataGridView)sender).CurrentCell.FormattedValue);

                    if (!String.IsNullOrEmpty(cellValue))
                    {

                        gvParameters.CurrentCell.Style.BackColor = Color.White;
                        gvParameters.CurrentCell.Style.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void dtSelect_Leave(object sender, EventArgs e)
        {
            if (gvParameters.CurrentCell.ColumnIndex == 4)
            {
                gvParameters.CurrentCell.Value = Convert.ToString(((DateTimePicker)sender).Value.ToShortTimeString());

                dtSelect.Visible = false;
            }
        }

        private void dtSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (gvParameters.CurrentCell.ColumnIndex == 4 && (e.KeyChar == (Char)8 || e.KeyChar == (Char)Keys.Delete))
            {
                dtSelect.Visible = false;
                gvParameters.CurrentCell.Value = "";

            }
        }

        private void gvParameters_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.C && (e.KeyData & Keys.Control) == Keys.Control)
                {
                    if (this.gvParameters.SelectedRows.Count == 1)
                    {
                        int selectedIndex = this.gvParameters.CurrentRow.Index;

                        AXSParameter currentParameter = (AXSParameter)this.iTicketParameterBindingSource.Current;
                        if (currentParameter != null && !String.IsNullOrEmpty(currentParameter.PriceLevelString))
                        {
                            int calculatedIndex = selectedIndex + 1;

                            if (calculatedIndex < this.gvParameters.Rows.Count - 1)
                            {
                                while (true)
                                {
                                    try
                                    {
                                        if (calculatedIndex < this.gvParameters.Rows.Count - 1)
                                        {
                                            AXSParameter newlyAddedParameter = (AXSParameter)this.gvParameters.Rows[calculatedIndex].DataBoundItem;
                                            if ((newlyAddedParameter != null && String.IsNullOrEmpty(newlyAddedParameter.PriceLevelString)) || newlyAddedParameter.PriceLevelString != currentParameter.PriceLevelString)
                                            {
                                                newlyAddedParameter.PriceLevelString = currentParameter.PriceLevelString;
                                                break;
                                            }
                                            else
                                            {
                                                calculatedIndex++;
                                            }
                                        }
                                        else
                                        {
                                            AXSParameter newlyAddedParameter = new AXSParameter();
                                            newlyAddedParameter.PriceLevelString = currentParameter.PriceLevelString;
                                            this.Ticket.Parameters.Add(newlyAddedParameter);

                                            break;
                                        }
                                    }
                                    catch { break; }
                                }
                            }
                            else
                            {
                                /** Adding a new row if there is 
                                 ** no row below the current row.
                                 **/
                                AXSParameter newlyAddedParameter = new AXSParameter();
                                newlyAddedParameter.PriceLevelString = currentParameter.PriceLevelString;
                                this.Ticket.Parameters.Add(newlyAddedParameter);
                            }

                            //this.Ticket.Parameters.Insert(this.gvParameters.SelectedRows[0].Index, newlyAddedParameter);
                            //this.Ticket.Parameters.Add(newlyAddedParameter);
                            this.iTicketParameterBindingSource.ResetBindings(true);
                        }
                    }
                }
            }
            catch { }
        }

        private void ResetParameterBindingSource(int index)
        {
            try
            {
                if (index > -1)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate()
                        {
                            this.iTicketParameterBindingSource.ResetItem(index);
                        }));
                    }
                    else
                    {
                        this.iTicketParameterBindingSource.ResetItem(index);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void gvParameters_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (gvParameters.CurrentCell.OwningColumn.Name == "LowestPrice")
                {
                    if (gvParameters.CurrentRow.DataBoundItem is AXSParameter)
                    {
                        string cellValue = Convert.ToString(((DataGridView)sender).CurrentCell.Displayed);

                        Boolean check = Boolean.Parse(cellValue);

                        if (check)
                        {
                            AXSParameter parameter = (AXSParameter)gvParameters.CurrentRow.DataBoundItem;
                            parameter.TopPrice = false;
                            parameter.PriceMax = null;
                            parameter.PriceMin = null;

                            int cellIndex = e.RowIndex;

                            ResetParameterBindingSource(cellIndex);
                        }
                    }
                }
                else if (gvParameters.CurrentCell.OwningColumn.Name == "TopPrice")
                {
                    if (gvParameters.CurrentRow.DataBoundItem is AXSParameter)
                    {
                        string cellValue = Convert.ToString(((DataGridView)sender).CurrentCell.Displayed);

                        Boolean check = Boolean.Parse(cellValue);

                        if (check)
                        {
                            AXSParameter parameter = (AXSParameter)gvParameters.CurrentRow.DataBoundItem;
                            parameter.LowestPrice = false;
                            parameter.PriceMax = null;
                            parameter.PriceMin = null;

                            int cellIndex = e.RowIndex;

                            ResetParameterBindingSource(cellIndex);
                        }
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
