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
using System.Web;
using System.Globalization;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Automatick
{
    public partial class frmTicket : C1.Win.C1Ribbon.C1RibbonForm, IAddTicket
    {
        Boolean ifRunEvent = false;
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

            try
            {
                CapsiumServers = this.MainForm.AppPermissions.AllAccessList.Single(p => p.form == "CapsiumServers");

                if (CapsiumServers != null)
                {
                    string _controlName = CapsiumServers.name;
                    string _controlVisibility = CapsiumServers.access;

                    if (!String.IsNullOrEmpty(CapsiumServers.name) && Boolean.Parse(CapsiumServers.access))
                    {
                        this.pnlCaptchaService.Visible = false;
                        this.chkUseProxiesInCaptchaSource.Visible = false;
                        this.Ticket.ifCapsium = true;
                    }
                    else
                    {
                        this.pnlCaptchaService.Visible = true;
                        this.chkUseProxiesInCaptchaSource.Visible = false;
                        this.Ticket.ifCapsium = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        private void applyPermission()
        {
            try
            {
                Boolean ifAutoCaptchaSelectedPreviously = this.Ticket.ifAutoCaptcha;
                Boolean isWebAllowed = false;

                IEnumerable<AccessRights.AccessList> allControlsAccesses = this.MainForm.AppPermissions.AllAccessList.Where(p => p.form == this.Name);
                foreach (AccessRights.AccessList obj in allControlsAccesses)
                {
                    if (obj.name == rbBPCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbBPCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifBPCAutoCaptcha = false;
                            rbBPCAutoCaptcha.Checked = false;
                            rbBPCAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbAC1AutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbAC1AutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifAC1AutoCaptcha = false;
                            rbAC1AutoCaptcha.Checked = false;
                            rbAC1AutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbRDAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbRDAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifRDAutoCaptcha = false;
                            rbRDAutoCaptcha.Checked = false;
                            rbRDAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbRDCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbRDCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifRDCAutoCaptcha = false;
                            rbRDCAutoCaptcha.Checked = false;
                            rbRDCAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbAAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbAAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifAntigateAutoCaptcha = false;
                            rbAAutoCaptcha.Checked = false;
                            rbAAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbCPTAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbCPTAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifCPTAutoCaptcha = false;
                            rbCPTAutoCaptcha.Checked = false;
                            rbCPTAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbDBCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbDBCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifDBCAutoCaptcha = false;
                            rbDBCAutoCaptcha.Checked = false;
                            rbDBCAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbDCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbDCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifDCAutoCaptcha = false;
                            rbDCAutoCaptcha.Checked = false;
                            rbDCAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifCAutoCaptcha = false;
                            rbCAutoCaptcha.Checked = false;
                            rbCAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbOCR.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbOCR.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifOCR = false;
                            rbOCR.Checked = false;
                            rbOCR.Enabled = false;
                        }
                    }
                    else if (obj.name == this.rbRAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbRAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifROCR = false;
                            rbRAutoCaptcha.Checked = false;
                            rbRAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == this.rbBoloAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbBoloAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifBoloOCR = false;
                            rbBoloAutoCaptcha.Checked = false;
                            rbBoloAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == this.rbSOCR.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        this.rbSOCR.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifSOCR = false;
                            rbSOCR.Checked = false;
                            rbSOCR.Enabled = false;
                        }
                    }
                    else if (obj.name == this.rb2CAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        this.rb2CAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.if2C = false;
                            rb2CAutoCaptcha.Checked = false;
                            rb2CAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == this.rbCaptchatorAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        this.rbCaptchatorAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifCaptchator = false;
                            rbCaptchatorAutoCaptcha.Checked = false;
                            rbCaptchatorAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == chkPersistSession.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        chkPersistSession.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifPesistSessionInEachSearch = false;
                            chkPersistSession.Checked = false;
                        }
                    }
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
                        this.chkWeb.Visible = access;
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

                    if (obj.name == chkPersistSession.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        chkPersistSession.Visible = access;
                        if (!access)
                        {
                            this.Ticket.ifPesistSessionInEachSearch = false;
                            chkPersistSession.Checked = false;
                        }
                    }
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

                if (accessObj != null)
                {
                    if (!this.Ticket.ifBPCAutoCaptcha && !this.Ticket.ifCPTAutoCaptcha && !this.Ticket.ifDBCAutoCaptcha && !this.Ticket.ifRDAutoCaptcha && !this.Ticket.ifRDCAutoCaptcha && !this.Ticket.ifAntigateAutoCaptcha && !this.Ticket.ifDCAutoCaptcha && !this.Ticket.ifCAutoCaptcha && !this.Ticket.ifOCR && !this.Ticket.ifROCR
                         && !this.Ticket.ifBoloOCR && !this.Ticket.ifSOCR && !this.Ticket.if2C && !this.Ticket.ifCaptchator && !this.Ticket.ifAC1AutoCaptcha)
                    {

                        if (accessObj.name == rbRDAutoCaptcha.Name)
                        {
                            this.Ticket.ifRDAutoCaptcha = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbRDAutoCaptcha.Checked = true;
                            this.rbRDAutoCaptcha.Enabled = true;
                        }
                        if (accessObj.name == rbRDCAutoCaptcha.Name)
                        {
                            this.Ticket.ifRDCAutoCaptcha = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbRDCAutoCaptcha.Checked = true;
                            this.rbRDCAutoCaptcha.Enabled = true;
                        }
                        if (accessObj.name == rbAAutoCaptcha.Name)
                        {
                            this.Ticket.ifAntigateAutoCaptcha = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbAAutoCaptcha.Checked = true;
                            this.rbAAutoCaptcha.Enabled = true;
                        }
                        if (accessObj.name == rbAC1AutoCaptcha.Name)
                        {
                            this.Ticket.ifAC1AutoCaptcha = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbAC1AutoCaptcha.Checked = true;
                            this.rbAC1AutoCaptcha.Enabled = true;
                        }
                        if (accessObj.name == rbBPCAutoCaptcha.Name)
                        {
                            this.Ticket.ifBPCAutoCaptcha = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbBPCAutoCaptcha.Checked = true;
                            this.rbBPCAutoCaptcha.Enabled = true;

                        }
                        else if (accessObj.name == rbCPTAutoCaptcha.Name)
                        {
                            this.Ticket.ifCPTAutoCaptcha = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbCPTAutoCaptcha.Checked = true;
                            this.rbCPTAutoCaptcha.Enabled = true;

                        }
                        else if (accessObj.name == rbDBCAutoCaptcha.Name)
                        {
                            this.Ticket.ifDBCAutoCaptcha = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbDBCAutoCaptcha.Checked = true;
                            this.rbDBCAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == rbDCAutoCaptcha.Name)
                        {
                            this.Ticket.ifDCAutoCaptcha = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbDCAutoCaptcha.Checked = true;
                            this.rbDCAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == rbCAutoCaptcha.Name)
                        {
                            this.Ticket.ifCAutoCaptcha = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbCAutoCaptcha.Checked = true;
                            this.rbCAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == rbOCR.Name)
                        {
                            this.Ticket.ifOCR = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbOCR.Checked = true;
                            this.rbOCR.Enabled = true;
                        }
                        else if (accessObj.name == this.rbRAutoCaptcha.Name)
                        {
                            this.Ticket.ifROCR = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbRAutoCaptcha.Checked = true;
                            this.rbRAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == this.rbBoloAutoCaptcha.Name)
                        {
                            this.Ticket.ifBoloOCR = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbBoloAutoCaptcha.Checked = true;
                            this.rbBoloAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == this.rbSOCR.Name)
                        {
                            this.Ticket.ifSOCR = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbSOCR.Checked = true;
                            this.rbSOCR.Enabled = true;
                        }
                        else if (accessObj.name == this.rb2CAutoCaptcha.Name)
                        {
                            this.Ticket.if2C = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rb2CAutoCaptcha.Checked = true;
                            this.rb2CAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == this.rbCaptchatorAutoCaptcha.Name)
                        {
                            this.Ticket.ifCaptchator = true;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbCaptchatorAutoCaptcha.Checked = true;
                            this.rbCaptchatorAutoCaptcha.Enabled = true;
                        }
                    }
                }
                else
                {
                    this.chkAutoCaptcha.Enabled = false;
                    this.chkAutoCaptcha.Checked = false;
                    this.Ticket.ifAutoCaptcha = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            ifRunEvent = false;
            this.save();
        }

        private void btnSaveandStart_Click(object sender, EventArgs e)
        {
            ifRunEvent = true;
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

            try
            {
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
                        this.Ticket.constTicketURL = this.Ticket.URL = this.Ticket.QueueURL = this.txtTicketURL.Text;

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

                if (this.chkSchedule.Checked)
                {
                    if (this.dtpScheduleDateTime.Value <= DateTime.Now)
                    {
                        result = false;
                        MessageBox.Show("Schedule date time must be greater than current date time.");
                        docParameters.SelectedTab = tabPageMoreParameters;
                        this.dtpScheduleDateTime.Focus();
                        return result;
                    }
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

                if (this.cbGroup.SelectedItem != null)
                {
                    if (this.cbGroup.SelectedItem is TicketGroup)
                    {
                        this.Ticket.TicketGroup = (TicketGroup)this.cbGroup.SelectedItem;
                    }
                }

                this.Ticket.Notes = this.txtNotes.Text;
                this.Ticket.DeliveryCountry = this.txtDeliveryCountry.Text;
                this.Ticket.DeliveryOption = this.txtDeliveryOption.Text;


                this.Ticket.ifSchedule = this.chkSchedule.Checked;
                this.Ticket.ScheduleDateTime = this.dtpScheduleDateTime.Value;
                this.Ticket.ScheduleRunningTime = this.nudScheduleRunningTime.Value;
                this.Ticket.NoOfSearches = this.nudNoOfSearches.Value;
                this.Ticket.ifAutoBuy = this.chkAutoBuy.Checked;
                this.Ticket.ifAutoBuyWitoutProxy = this.chkAutoBuyWitoutProxy.Checked;
                this.Ticket.ifAutoCaptcha = this.chkAutoCaptcha.Checked;
                this.Ticket.ifBPCAutoCaptcha = this.rbBPCAutoCaptcha.Checked;
                this.Ticket.ifCPTAutoCaptcha = this.rbCPTAutoCaptcha.Checked;
                this.Ticket.ifDBCAutoCaptcha = this.rbDBCAutoCaptcha.Checked;
                this.Ticket.ifRDAutoCaptcha = this.rbRDAutoCaptcha.Checked;
                this.Ticket.ifRDCAutoCaptcha = this.rbRDCAutoCaptcha.Checked;
                this.Ticket.ifAntigateAutoCaptcha = this.rbAAutoCaptcha.Checked;
                this.Ticket.ifAC1AutoCaptcha = this.rbAC1AutoCaptcha.Checked;
                this.Ticket.ifOCR = this.rbOCR.Checked;
                this.Ticket.ifROCR = this.rbRAutoCaptcha.Checked;
                this.Ticket.ifBoloOCR = this.rbBoloAutoCaptcha.Checked;
                this.Ticket.ifCAutoCaptcha = this.rbCAutoCaptcha.Checked;
                this.Ticket.ifDCAutoCaptcha = this.rbDCAutoCaptcha.Checked;
                this.Ticket.ifSOCR = this.rbSOCR.Checked;
                this.Ticket.if2C = this.rb2CAutoCaptcha.Checked;
                this.Ticket.ifCaptchator = this.rbCaptchatorAutoCaptcha.Checked;
                this.Ticket.ifDistributeInSearches = this.rbDistributeInSearches.Checked;
                this.Ticket.ifPlaySoundAlert = this.chkPlaySoundAlert.Checked;
                this.Ticket.ifRandomDelay = this.chkRandomDelay.Checked;
                this.Ticket.ifSelectDeliveryOptionAutoBuying = this.rbSelectDeliveryOptionAutoBuying.Checked;
                this.Ticket.ifSelectDeliveryOptionList = this.rbSelectDeliveryOptionList.Checked;
                this.Ticket.ifSelectAccountAutoBuying = this.rbSelectAccountAutoBuying.Checked;
                this.Ticket.ifSelectAccountList = this.rbSelectAccountList.Checked;
                this.Ticket.ifSendEmail = this.chkSendEmail.Checked;
                this.Ticket.ifUseFoundOnFirstAttempt = this.rbUseFoundOnFirstAttempt.Checked;
                this.Ticket.ifUseAvailableParameters = this.rbUseAvailableParameters.Checked;
                this.Ticket.ifUseProxies = this.chkUseProxies.Checked;
                this.Ticket.ResetSearchDelay = this.nudResetSearchDelay.Value;
                this.Ticket.StartUsingProxiesFrom = this.nudStartUsingProxiesFrom.Value;
                this.Ticket.DelayForAutoBuy = this.Delay.Value;
                this.Ticket.StartSolvingFromCaptcha = this.nudStartSolvingFromCaptcha.Value;
                this.Ticket.ifPesistSessionInEachSearch = this.chkPersistSession.Checked;
                this.Ticket.Permission = this.AXSByPassWaitingRoom.Checked;
                this.Ticket.ifUseProxiesInCaptchaSource = this.chkUseProxiesInCaptchaSource.Checked;
                this.Ticket.isWaitingEvent = this.chkIsWaiting.Checked;

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
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public void load()
        {
            try
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
                }

                this.txtTicketURL.Text = this.Ticket.URL;
                this.txtTicketName.Text = this.Ticket.TicketName;
                this.txtNotes.Text = this.Ticket.Notes;
                this.txtDeliveryCountry.Text = this.Ticket.DeliveryCountry;
                this.txtDeliveryOption.Text = this.Ticket.DeliveryOption;
                this.Delay.Value = this.Ticket.DelayForAutoBuy;
                this.nudNoOfSearches.Value = this.Ticket.NoOfSearches;
                this.chkPersistSession.Checked = this.Ticket.ifPesistSessionInEachSearch;
                this.AXSByPassWaitingRoom.Checked = this.Ticket.Permission;
                this.chkAutoBuy.Checked = this.Ticket.ifAutoBuy;
                this.chkAutoBuyWitoutProxy.Checked = this.Ticket.ifAutoBuyWitoutProxy;
                this.chkAutoCaptcha.Checked = this.Ticket.ifAutoCaptcha;
                this.rbBPCAutoCaptcha.Checked = this.Ticket.ifBPCAutoCaptcha;
                this.rbCPTAutoCaptcha.Checked = this.Ticket.ifCPTAutoCaptcha;
                this.rbDBCAutoCaptcha.Checked = this.Ticket.ifDBCAutoCaptcha;
                this.rbRDAutoCaptcha.Checked = this.Ticket.ifRDAutoCaptcha;
                this.rbRDCAutoCaptcha.Checked = this.Ticket.ifRDCAutoCaptcha;
                this.rbAAutoCaptcha.Checked = this.Ticket.ifAntigateAutoCaptcha;
                this.rbAC1AutoCaptcha.Checked = this.Ticket.ifAC1AutoCaptcha;
                this.rbOCR.Checked = this.Ticket.ifOCR;
                this.rbRAutoCaptcha.Checked = this.Ticket.ifROCR;
                this.rbBoloAutoCaptcha.Checked = this.Ticket.ifBoloOCR;
                this.rbDCAutoCaptcha.Checked = this.Ticket.ifDCAutoCaptcha;
                this.rbCAutoCaptcha.Checked = this.Ticket.ifCAutoCaptcha;
                this.rbSOCR.Checked = this.Ticket.ifSOCR;
                this.rb2CAutoCaptcha.Checked = this.Ticket.if2C;
                this.rbCaptchatorAutoCaptcha.Checked = this.Ticket.ifCaptchator;
                this.rbDistributeInSearches.Checked = this.Ticket.ifDistributeInSearches;
                this.chkPlaySoundAlert.Checked = this.Ticket.ifPlaySoundAlert;
                this.chkRandomDelay.Checked = this.Ticket.ifRandomDelay;
                this.chkSchedule.Checked = this.Ticket.ifSchedule;
                this.dtpScheduleDateTime.Value = this.Ticket.ScheduleDateTime;
                this.nudScheduleRunningTime.Value = this.Ticket.ScheduleRunningTime;
                this.rbSelectDeliveryOptionAutoBuying.Checked = this.Ticket.ifSelectDeliveryOptionAutoBuying;
                this.rbSelectDeliveryOptionList.Checked = this.Ticket.ifSelectDeliveryOptionList;
                this.rbSelectAccountAutoBuying.Checked = this.Ticket.ifSelectAccountAutoBuying;
                this.rbSelectAccountList.Checked = this.Ticket.ifSelectAccountList;
                this.chkSendEmail.Checked = this.Ticket.ifSendEmail;
                this.rbUseFoundOnFirstAttempt.Checked = this.Ticket.ifUseFoundOnFirstAttempt;
                this.rbUseAvailableParameters.Checked = this.Ticket.ifUseAvailableParameters;
                this.chkUseProxies.Checked = this.Ticket.ifUseProxies;
                this.nudResetSearchDelay.Value = this.Ticket.ResetSearchDelay < 15 ? 15 : this.Ticket.ResetSearchDelay;
                this.nudStartUsingProxiesFrom.Value = this.Ticket.StartUsingProxiesFrom;
                this.nudStartSolvingFromCaptcha.Value = this.Ticket.StartSolvingFromCaptcha;
                this.chkUseProxiesInCaptchaSource.Checked = this.Ticket.ifUseProxiesInCaptchaSource;
                this.chkIsWaiting.Checked = this.Ticket.isWaitingEvent;

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

                if (this.Ticket.Parameters != null)
                {
                    this.iTicketParameterBindingSource.DataSource = this.Ticket.Parameters;
                }

                if (this.Ticket.TicketFoundCriterions != null)
                {
                    this.iTicketFoundCriteriaBindingSource.DataSource = this.Ticket.TicketFoundCriterions;
                }
                if (!String.IsNullOrEmpty(this.Ticket.URL))
                {
                    //if (!this.Ticket.setAgencyRequestUrl((AXSTicket)this.Ticket))
                    //{
                    //    throw new Exception("Agency Inner Class exception");
                    //}
                }

                this.populateGroups();
                this.populateDeliveryOptions();
                this.populateAccounts();
                this.autoCaptchaCheckChanged();
                this.selectedDeliveryOptionCheckChanged();
                this.selectedAccountCheckedChanged();
                this.scheduleCheckChanged();
                this.sendEmailCheckChanged();
                this.autoBuyCheckChanged();
                this.useProxiesCheckedChanged();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void populateDeliveryOptions()
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void populateAccounts()
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void populateGroups()
        {
            try
            {
                if (this.MainForm.AppStartUp.Groups != null)
                {
                    List<TicketGroup> listGroup = this.MainForm.AppStartUp.Groups.ToList();
                    TicketGroup removeGroup = listGroup.FirstOrDefault(p => p.GroupName == "All Groups" && p.GroupId == "Default Group");
                    listGroup.Remove(removeGroup);

                    this.cbGroup.DisplayMember = "GroupName";
                    this.cbGroup.ValueMember = "GroupId";
                    this.cbGroup.DataSource = listGroup;
                    if (this.Ticket.TicketGroup != null)
                    {
                        TicketGroup group = listGroup.FirstOrDefault(p => p.GroupName == this.Ticket.TicketGroup.GroupName && p.GroupId == this.Ticket.TicketGroup.GroupId);
                        if (group != null)
                        {
                            this.cbGroup.SelectedItem = group;
                        }
                        else
                        {
                            TicketGroup defaultGroup = listGroup.FirstOrDefault(p => p.GroupName == "Default Group" && p.GroupId == "Default Group");
                            this.cbGroup.SelectedItem = defaultGroup;
                            this.Ticket.TicketGroup = defaultGroup;
                        }

                    }
                    else
                    {
                        TicketGroup defaultGroup = listGroup.FirstOrDefault(p => p.GroupName == "Default Group" && p.GroupId == "Default Group");
                        this.cbGroup.SelectedItem = defaultGroup;
                        this.Ticket.TicketGroup = defaultGroup;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public void onClosed()
        {
            try
            {
                this.MainForm.loadTickets();
                this.MainForm.populateRecentTickets();
                GC.SuppressFinalize(this);
                //GC.Collect();

                if (this.Ticket != null)
                {
                    if (this.Ticket is AXSTicket && this.ifRunEvent)
                    {
                        AXSTicket ticket = this.MainForm.AppStartUp.Tickets.FirstOrDefault(p => p.TicketID == this.Ticket.TicketID);
                        if (ticket != null)
                        {
                            frmFindTicket frm = new frmFindTicket(ticket, this.MainForm);
                            if (!ticket.isServiceSelected)
                            {
                                if (ticket.URL.Contains("bit.ly") || (ticket.URL.Contains(".queue") || ticket.URL.Contains("q.axs.co.uk")) || (this.MainForm.AppStartUp.GlobalSetting.ifEventko && ticket.URL.Contains("evenko.ca")) || ((this.MainForm.AppStartUp.GlobalSetting.IfWeb || this.MainForm.AppStartUp.GlobalSetting.ifMobile) && ticket.URL.Contains("axs.com")) || ((this.MainForm.AppStartUp.GlobalSetting.IfWeb || this.MainForm.AppStartUp.GlobalSetting.ifMobile) && ticket.URL.Contains("shop.axs.co.uk")))
                                {
                                    frm.TopMost = true;
                                    ticket.start(this.MainForm.CaptchaForm.CaptchaQueue, this.MainForm.CaptchaBrowserForm.CaptchaQueue, this.MainForm.AppStartUp.SoundAlert, this.MainForm.AppStartUp.AutoCaptchaService, this.MainForm.AppStartUp.Accounts, this.MainForm.AppStartUp.EmailSetting, this.MainForm.AppStartUp.GlobalSetting);
                                    frm.Show();
                                }
                                else if (ticket.URL.Contains(".aspx"))
                                {
                                    MessageBox.Show("Please use this on Veritix app.");
                                }
                                else
                                {
                                    MessageBox.Show("You are not allowed to run this event");
                                }
                            }
                            else
                            {
                                MessageBox.Show(Notification.CSAlert);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        public void save()
        {
            try
            {
                if (this.validate())
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        private void btnGroup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmGroup frm = new frmGroup(this.MainForm);
            frm.ShowDialog();
            this.populateGroups();
        }

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

        private void chkAutoCaptcha_CheckStateChanged(object sender, EventArgs e)
        {
            this.autoCaptchaCheckChanged();
        }

        private void autoCaptchaCheckChanged()
        {
            try
            {
                if (chkAutoCaptcha.Checked)
                {
                    nudStartSolvingFromCaptcha.Enabled = true;
                    pnlCaptchaService.Enabled = true;
                    lblStartSolvingFrom.Enabled = true;
                }
                else
                {
                    nudStartSolvingFromCaptcha.Enabled = false;
                    pnlCaptchaService.Enabled = false;
                    lblStartSolvingFrom.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
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

        private void chkSchedule_CheckStateChanged(object sender, EventArgs e)
        {
            this.scheduleCheckChanged();
        }

        private void scheduleCheckChanged()
        {
            try
            {
                if (chkSchedule.Checked)
                {
                    dtpScheduleDateTime.Enabled = true;
                    lblRunningTime.Enabled = true;
                    nudScheduleRunningTime.Enabled = true;
                    lblMinutes.Enabled = true;
                }
                else
                {
                    dtpScheduleDateTime.Enabled = false;
                    lblRunningTime.Enabled = false;
                    nudScheduleRunningTime.Enabled = false;
                    lblMinutes.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void chkSendEmail_CheckStateChanged(object sender, EventArgs e)
        {
            this.sendEmailCheckChanged();
        }

        private void sendEmailCheckChanged()
        {
            try
            {
                if (chkSendEmail.Checked)
                {
                    btnEmailSettings.Enabled = true;
                }
                else
                {
                    btnEmailSettings.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void chkAutoBuy_CheckedChanged(object sender, EventArgs e)
        {
            this.autoBuyCheckChanged();
        }

        private void autoBuyCheckChanged()
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void frmTicket_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.onClosed();
        }

        private void gvParameters_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
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
                        #region Min Max
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
                        #endregion
                    }

                    if (!String.IsNullOrEmpty(e.FormattedValue.ToString()))
                    {
                        #region Date Time
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
                                MessageBox.Show("Date is in invalid format. Please correct.(e.g. 12/01/2013 or 01/12/2013");
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
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void gvParameters_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (iTicketParameterBindingSource.Count <= 1)
                {
                    e.Cancel = true;
                    MessageBox.Show("There must be one parameter row defined, therefore you could not deleted this parameter row.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void rbSelectAccountAutoBuying_CheckedChanged(object sender, EventArgs e)
        {
            this.selectedAccountCheckedChanged();
        }

        private void selectedAccountCheckedChanged()
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void gvAccounts_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void btnManageAccounts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAccount frm = new frmAccount(this.MainForm);
            frm.ShowDialog();
            this.populateAccounts();
        }

        private void chkUseProxies_CheckStateChanged(object sender, EventArgs e)
        {
            this.useProxiesCheckedChanged();
        }

        private void useProxiesCheckedChanged()
        {
            if (chkUseProxies.Checked)
            {
                nudStartUsingProxiesFrom.Enabled = true;
                lblStartUsingProxiesFrom.Enabled = true;
                chkUseProxiesInCaptchaSource.Enabled = true;
            }
            else
            {
                nudStartUsingProxiesFrom.Enabled = false;
                lblStartUsingProxiesFrom.Enabled = false;
                chkUseProxiesInCaptchaSource.Enabled = false;
            }
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
                            //  gvParameters.CurrentCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
                            //  gvParameters.CurrentCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
