using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Automatick.Core;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Automatick
{
    public partial class frmFindTicket : C1.Win.C1Ribbon.C1RibbonForm, IForm, ISearchForm
    {
        ITicket Ticket = null;
        Boolean IfShowMessage = true;
        private AccessRights.AccessList CapsiumServers = null;

        public frmFindTicket(ITicket ticket, IMainForm mainForm)
        {
            try
            {
                this.Ticket = ticket;
                this.Ticket.TicketSearchWindow = this;
                this.MainForm = mainForm;

                if (this.MainForm != null)
                {
                    Ticket.onChangeStartOrStop = new ChangeDelegate(this.MainForm.onChangeHandlerStartOrStop);
                }
                InitializeComponent();

                if (applyPermission())
                {
                    this.Ticket.isServiceSelected = false;
                }
                else
                {
                    this.Ticket.isServiceSelected = true;
                }
            }
            catch
            {

            }
        }

        private void frmFindTicket_Load(object sender, EventArgs e)
        {
            this.chkPersistSession.Visible = false;
            this.load();


            //TODO: Apply permission for this form here


            try
            {
                CapsiumServers = this.MainForm.AppPermissions.AllAccessList.Single(p => p.form == "CapsiumServers");

                if (CapsiumServers != null)
                {
                    string _controlName = CapsiumServers.name;
                    string _controlVisibility = CapsiumServers.access;
                    if (!String.IsNullOrEmpty(CapsiumServers.name) && Boolean.Parse(CapsiumServers.access))
                    {
                        this.cbCaptchaService.Visible = false;
                        this.chkUseProxiesInCaptchaSource.Visible = false;
                        this.Ticket.ifCapsium = true;
                    }
                    else
                    {
                        this.cbCaptchaService.Visible = true;
                        this.chkUseProxiesInCaptchaSource.Visible = false;
                        this.Ticket.ifCapsium = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private Boolean applyPermission()
        {
            try
            {
                if (cbCaptchaService != null) cbCaptchaService.Text = string.Empty;

                Boolean ifAutoCaptchaSelectedPreviously = this.Ticket.ifAutoCaptcha;

                IEnumerable<AccessRights.AccessList> allControlsAccesses = this.MainForm.AppPermissions.AllAccessList.Where(p => p.form == this.Name);
                foreach (AccessRights.AccessList obj in allControlsAccesses)
                {
                    if (obj.name == rbBPCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbBPCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbBPCAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbRDAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbRDAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbRDAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifRDAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbRDCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbRDCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbRDCAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifRDCAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbAC1AutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbAC1AutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbAC1AutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbAAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbAAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbAAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifAntigateAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbCPTAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbCPTAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbCPTAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifCPTAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbDBCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbDBCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbDBCAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifDBCAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbDCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbDCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbDCAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifDCAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbCAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbCAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifCAutoCaptcha = false;
                            }
                        }
                    }
                    else if (obj.name == rbOCR.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbOCR.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbOCR);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifOCR = false;
                            }
                        }
                    }
                    else if (obj.name == this.rbRAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbRAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbRAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifROCR = false;
                            }
                        }
                    }
                    else if (obj.name == this.rbBoloAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbBoloAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbBoloAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifBoloOCR = false;
                            }
                        }
                    }
                    else if (obj.name == this.rbSOCR.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        this.rbSOCR.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbSOCR);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifSOCR = false;
                            }
                        }
                    }
                    else if (obj.name == this.rb2CAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        this.rb2CAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rb2CAutoCaptcha);
                        }
                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.if2C = false;
                            }
                        }
                    }
                    else if (obj.name == this.rbCaptchatorAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        this.rbCaptchatorAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            cbCaptchaService.Items.Remove(rbCaptchatorAutoCaptcha);
                        }

                        if (cbCaptchaService.SelectedItem != null)
                        {
                            if (cbCaptchaService.SelectedItem.Name == obj.name && !access)
                            {
                                this.Ticket.ifCaptchator = false;
                            }
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

                AccessRights.AccessList accessObj = allControlsAccesses.FirstOrDefault(p => p.access.ToLower() == "true" && (p.name == "rbBPCAutoCaptcha" || p.name == "rbDBCAutoCaptcha" || p.name == "rbRDAutoCaptcha" || p.name == "rbCPTAutoCaptcha" || p.name == "rbDCAutoCaptcha" || p.name == "rbCAutoCaptcha" || p.name == "rbOCR" || p.name == "rbRAutoCaptcha" || p.name == "rbBoloAutoCaptcha" || p.name == "rbAAutoCaptcha" || p.name == "rb2CAutoCaptcha" || p.name == "rbCaptchatorAutoCaptcha" || p.name == "rbAC1AutoCaptcha"));

                if (accessObj != null)
                {
                    if (!this.Ticket.ifBPCAutoCaptcha && !this.Ticket.ifCPTAutoCaptcha && !this.Ticket.ifRDAutoCaptcha && !this.Ticket.ifAntigateAutoCaptcha && !this.Ticket.ifDBCAutoCaptcha && !this.Ticket.ifDCAutoCaptcha && !this.Ticket.ifCAutoCaptcha && !this.Ticket.ifOCR && !this.Ticket.ifROCR
                         && !this.Ticket.ifBoloOCR && !this.Ticket.ifSOCR && !this.Ticket.if2C && !this.Ticket.ifCaptchator && !this.Ticket.ifAC1AutoCaptcha)
                    {
                        if (accessObj.name == rbBPCAutoCaptcha.Name)
                        {
                            this.Ticket.ifBPCAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbBPCAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbAC1AutoCaptcha.Name)
                        {
                            this.Ticket.ifAC1AutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbAC1AutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbRDAutoCaptcha.Name)
                        {
                            this.Ticket.ifRDAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbRDAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbAAutoCaptcha.Name)
                        {
                            this.Ticket.ifAntigateAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbAAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbCPTAutoCaptcha.Name)
                        {
                            this.Ticket.ifCPTAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbCPTAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbDBCAutoCaptcha.Name)
                        {
                            this.Ticket.ifDBCAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbDBCAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbDCAutoCaptcha.Name)
                        {
                            this.Ticket.ifDCAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbDCAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbCAutoCaptcha.Name)
                        {
                            this.Ticket.ifCAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbCAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rbOCR.Name)
                        {
                            this.Ticket.ifOCR = true;
                            cbCaptchaService.SelectedItem = rbOCR;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rbRAutoCaptcha.Name)
                        {
                            this.Ticket.ifROCR = true;
                            cbCaptchaService.SelectedItem = this.rbRAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rbBoloAutoCaptcha.Name)
                        {
                            this.Ticket.ifBoloOCR = true;
                            cbCaptchaService.SelectedItem = this.rbBoloAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rbSOCR.Name)
                        {
                            this.Ticket.ifSOCR = true;
                            cbCaptchaService.SelectedItem = this.rbSOCR;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rb2CAutoCaptcha.Name)
                        {
                            this.Ticket.if2C = true;
                            cbCaptchaService.SelectedItem = this.rb2CAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == this.rbCaptchatorAutoCaptcha.Name)
                        {
                            this.Ticket.ifCaptchator = true;
                            cbCaptchaService.SelectedItem = this.rbCaptchatorAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                        else if (accessObj.name == rbRDCAutoCaptcha.Name)
                        {
                            this.Ticket.ifRDCAutoCaptcha = true;
                            cbCaptchaService.SelectedItem = rbRDCAutoCaptcha;
                            this.Ticket.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                        }
                    }
                }
                else
                {
                    this.chkAutoCaptcha.Enabled = false;
                    this.chkAutoCaptcha.Checked = false;
                    this.Ticket.ifAutoCaptcha = false;
                }


                this.Ticket.SaveTicket();
            }
            catch (Exception)
            {

            }

            CapsiumServers = this.MainForm.AppPermissions.AllAccessList.Single(p => p.form == "CapsiumServers");

            if ((CapsiumServers != null) && !(Boolean.Parse(CapsiumServers.access)))
            {

                if (this.Ticket.ifBPCAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbBPCAutoCaptcha;
                }
                else if (this.Ticket.ifRDAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbRDAutoCaptcha;
                }
                else if (this.Ticket.ifCPTAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbCPTAutoCaptcha;
                }
                else if (this.Ticket.ifDBCAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbDBCAutoCaptcha;
                }
                else if (this.Ticket.ifDCAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbDCAutoCaptcha;
                }
                else if (this.Ticket.ifOCR)
                {
                    this.cbCaptchaService.SelectedItem = rbOCR;
                }
                else if (this.Ticket.ifROCR)
                {
                    this.cbCaptchaService.SelectedItem = rbRAutoCaptcha;
                }
                else if (this.Ticket.ifBoloOCR)
                {
                    this.cbCaptchaService.SelectedItem = rbBoloAutoCaptcha;
                }
                else if (this.Ticket.ifCAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbCAutoCaptcha;
                }
                else if (this.Ticket.if2C)
                {
                    this.cbCaptchaService.SelectedItem = rb2CAutoCaptcha;
                }
                else if (this.Ticket.ifAntigateAutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbAAutoCaptcha;
                }
                else if (this.Ticket.ifCaptchator)
                {
                    this.cbCaptchaService.SelectedItem = rbCaptchatorAutoCaptcha;
                }
                else if (this.Ticket.ifAC1AutoCaptcha)
                {
                    this.cbCaptchaService.SelectedItem = rbAC1AutoCaptcha;
                }

                if ((cbCaptchaService != null) && (cbCaptchaService.SelectedItem != null))
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
                return true;
            }
        }

        private void frmFindTicket_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.IfShowMessage)
                {
                    if (MessageBox.Show("Do you really want to stop and close searches?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        this.Ticket.TicketSearchWindow = null;
                        this.Ticket.stop();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void frmFindTicket_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.onClosed();
        }

        public void closeForm()
        {
            try
            {
                this.IfShowMessage = false;
                this.Close();
            }
            catch (Exception ex)
            {
                 Debug.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }

        private void grv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }
        }

        void changeDelegateHandler(ITicketSearch search)
        {
            try
            {
                if (search != null)
                {
                    int index = this.AXSSearchBindingSource.IndexOf(search);
                    if (index > -1)
                    {
                        this.AXSSearchBindingSource.ResetItem(index);
                        try
                        {
                            DataGridViewRow row = grvFindTickets.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.DataBoundItem == search);
                            if (row != null)
                            {
                                if (search.Status == TicketSearchStatus.RetryingStatus && (search.MoreInfo.Contains("Sold out") || search.MoreInfo.Contains(TicketSearchStatus.MoreInfoSiteUnavailable) || search.MoreInfo.Contains("Ticket not found!")))
                                {
                                    row.DefaultCellStyle.BackColor = System.Drawing.Color.OrangeRed;
                                    row.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
                                }
                                else if (search.IfFound)
                                {
                                    row.DefaultCellStyle.BackColor = System.Drawing.Color.PaleGreen;
                                    row.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
                                }
                                else
                                {
                                    row.DefaultCellStyle.BackColor = Color.White; //System.Drawing.SystemColors.Window;
                                    row.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch { }
        }

        private void timerFocusScreen_Tick(object sender, EventArgs e)
        {
            //this.Activate();
            this.TopMost = false;
            timerFocusScreen.Stop();
            timerFocusScreen.Dispose();
        }

        private void rbStopSelectedSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.AXSSearchBindingSource.Current != null)
                {
                    if (this.AXSSearchBindingSource.Current is ITicketSearch)
                    {
                        ITicketSearch search = (ITicketSearch)this.AXSSearchBindingSource.Current;
                        search.stop();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void rbResetSelectedSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.AXSSearchBindingSource.Current != null)
                //{
                //    if (this.AXSSearchBindingSource.Current is ITicketSearch)
                //    {
                //        ITicketSearch search = (ITicketSearch)this.AXSSearchBindingSource.Current;
                //        search.restart();
                //    }
                //}

                if (this.grvFindTickets.SelectedRows != null && this.grvFindTickets.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in this.grvFindTickets.SelectedRows)
                    {
                        try
                        {
                            ITicketSearch search = (ITicketSearch)row.DataBoundItem;
                            //search.restart();
                            Task.Factory.StartNew(search.restart);
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }



        private void rbClose_Click(object sender, EventArgs e)
        {
            try
            {
                List<AXSSearch> rows = new List<AXSSearch>();
                foreach (DataGridViewRow row in this.grvFindTickets.SelectedRows)
                {
                    try
                    {
                        AXSSearch search = (AXSSearch)row.DataBoundItem;
                        //rows.Add(search);
                        Task.Factory.StartNew(search.stop);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message + " " + ex.StackTrace);
                    }
                }

                //if (this.grvFindTickets.SelectedRows != null && this.grvFindTickets.SelectedRows.Count > 0)
                //{
                //    foreach (DataGridViewRow row in this.grvFindTickets.SelectedRows)
                //    {
                //        try
                //        {
                //            ITicketSearch search = (ITicketSearch)row.DataBoundItem;
                //            search.stop();
                //        }
                //        catch { }
                //    }
                //}
            }
            catch
            {
            }
            //if (this.AXSSearchBindingSource.Current != null)
            //{
            //    if (this.AXSSearchBindingSource.Current is ITicketSearch)
            //    {
            //        ITicketSearch search = (ITicketSearch)this.AXSSearchBindingSource.Current;
            //        search.stop();
            //    }
            //}

        }

        private void rbResetAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you really want to restart all searches?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < this.Ticket.Searches.Count; i++)
                    {
                        try
                        {
                            ITicketSearch search = this.Ticket.Searches[i];

                            search.IfUseAutoCaptcha = false;
                            if (i >= ((int)this.Ticket.StartSolvingFromCaptcha - 1) && this.Ticket.ifAutoCaptcha)
                            {
                                search.IfUseAutoCaptcha = true;
                            }

                            search.IfUseProxy = false;
                            if (i >= ((int)this.Ticket.StartUsingProxiesFrom - 1) && this.Ticket.ifUseProxies)
                            {
                                search.IfUseProxy = true;
                            }

                            search.restart();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public IMainForm MainForm
        {
            get;
            set;
        }

        public bool validate()
        {
            throw new NotImplementedException();
        }

        public void load()
        {
            try
            {
                if (this.Ticket != null)
                {
                    if (this.Ticket.Searches != null)
                    {
                        this.Ticket.Searches.changeDelegate = new SortedBindingList.SortableBindingList<ITicketSearch>.dlgOnChange(this.changeDelegateHandler);
                        this.AXSSearchBindingSource.DataSource = this.Ticket.Searches;
                    }
                    this.Text = "Finding tickets for " + this.Ticket.TicketName;
                    this.Ticket.ResetSearchDelay = this.Ticket.ResetSearchDelay < 15 ? 15 : this.Ticket.ResetSearchDelay;

                    this.chkAutoCaptcha.Checked = this.Ticket.ifAutoCaptcha;
                    this.nudStartSolvingCaptchaFrom.Value = this.Ticket.StartSolvingFromCaptcha;
                    if (this.Ticket.ifBPCAutoCaptcha)
                    {
                        this.cbCaptchaService.SelectedItem = rbBPCAutoCaptcha;
                    }
                    else if (this.Ticket.ifRDAutoCaptcha)
                    {
                        this.cbCaptchaService.SelectedItem = rbRDAutoCaptcha;
                    }
                    else if (this.Ticket.ifRDCAutoCaptcha)
                    {
                        this.cbCaptchaService.SelectedItem = rbRDCAutoCaptcha;
                    }
                    else if (this.Ticket.ifAC1AutoCaptcha)
                    {
                        this.cbCaptchaService.SelectedItem = rbAC1AutoCaptcha;
                    }
                    else if (this.Ticket.ifAntigateAutoCaptcha)
                    {
                        this.cbCaptchaService.SelectedItem = rbAAutoCaptcha;
                    }
                    else if (this.Ticket.ifCPTAutoCaptcha)
                    {
                        this.cbCaptchaService.SelectedItem = rbCPTAutoCaptcha;
                    }
                    else if (this.Ticket.ifDBCAutoCaptcha)
                    {
                        this.cbCaptchaService.SelectedItem = rbDBCAutoCaptcha;
                    }
                    else if (this.Ticket.ifDCAutoCaptcha)
                    {
                        this.cbCaptchaService.SelectedItem = rbDCAutoCaptcha;
                    }
                    else if (this.Ticket.ifCAutoCaptcha)
                    {
                        this.cbCaptchaService.SelectedItem = rbCAutoCaptcha;
                    }
                    else if (this.Ticket.ifOCR)
                    {
                        this.cbCaptchaService.SelectedItem = rbOCR;
                    }
                    else if (this.Ticket.ifROCR)
                    {
                        this.cbCaptchaService.SelectedItem = rbRAutoCaptcha;
                    }
                    else if (this.Ticket.ifBoloOCR)
                    {
                        this.cbCaptchaService.SelectedItem = rbBoloAutoCaptcha;
                    }
                    else if (this.Ticket.ifSOCR)
                    {
                        this.cbCaptchaService.SelectedItem = rbSOCR;
                    }
                    else if (this.Ticket.if2C)
                    {
                        this.cbCaptchaService.SelectedItem = this.rb2CAutoCaptcha;
                    }
                    else if (this.Ticket.ifCaptchator)
                    {
                        this.cbCaptchaService.SelectedItem = this.rbCaptchatorAutoCaptcha;
                    }

                    this.chkPlayMusic.Checked = this.Ticket.ifPlaySoundAlert;
                    this.chkPersistSession.Checked = this.Ticket.ifPesistSessionInEachSearch;
                    this.AXSByPassWaitingRoom.Checked = this.Ticket.Permission;
                    this.nudNumberOfSearches.Value = this.Ticket.NoOfSearches;
                    this.chkAutoBuy.Checked = this.Ticket.ifAutoBuy;
                    this.chkSendEmail.Checked = this.Ticket.ifSendEmail;
                    this.chkUseProxies.Checked = this.Ticket.ifUseProxies;
                    this.nudStartUsingProxiesFrom.Value = this.Ticket.StartUsingProxiesFrom;
                    this.chkUseProxiesInCaptchaSource.Checked = this.Ticket.ifUseProxiesInCaptchaSource;
                }
                populateAutoCaptchaSettings();
                populateUseProxiesSettings();

            }
            catch (Exception ex)
            {

            }
        }

        public void save()
        {
            throw new NotImplementedException();
        }

        public void onClosed()
        {
            try
            {
                this.MainForm.AppStartUp.SaveLogs();
                GC.SuppressFinalize(this);
                //GC.Collect();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }

        private void rbShowHideBar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rMain.Minimized)
                {
                    rMain.Minimized = false;
                    this.rbShowHideBar.SmallImage = global::Automatick.Properties.Resources._1354172637_navigate_up;
                    this.rbShowHideBar.ToolTip = "Minimize actions bar";
                }
                else
                {
                    rMain.Minimized = true;
                    this.rbShowHideBar.SmallImage = global::Automatick.Properties.Resources._1354172644_navigate_down;
                    this.rbShowHideBar.ToolTip = "Maximize actions bar";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void chkAutoCaptcha_CheckedChanged(object sender, EventArgs e)
        {
            populateAutoCaptchaSettings();
        }

        private void populateAutoCaptchaSettings()
        {
            try
            {
                if (chkAutoCaptcha.Checked)
                {
                    cbCaptchaService.Enabled = true;
                    nudStartSolvingCaptchaFrom.Enabled = true;
                }
                else
                {
                    cbCaptchaService.Enabled = false;
                    nudStartSolvingCaptchaFrom.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void rbSaveAndChange_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to change and save the settings?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (this.Ticket != null)
                    {
                        decimal lastNumberOfSearches = this.Ticket.NoOfSearches;
                        this.Ticket.ifAutoCaptcha = this.chkAutoCaptcha.Checked;
                        this.Ticket.StartSolvingFromCaptcha = this.nudStartSolvingCaptchaFrom.Value;
                        if (this.cbCaptchaService.SelectedItem != null)
                        {
                            if (this.cbCaptchaService.SelectedItem.Text == "BPC")
                            {
                                this.Ticket.ifBPCAutoCaptcha = true;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "RD")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = true;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "A")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifAntigateAutoCaptcha = true;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "CPT")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = true;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "DBC")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = true;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "DC")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = true;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "O")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = true;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "C")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = true;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "RC")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = true;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "B")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = true;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "S")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = true;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "2C")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = true;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "CTR")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifCaptchator = true;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "C1")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = false;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifCaptchator = false;
                                this.Ticket.ifAC1AutoCaptcha = true;
                            }
                            else if (this.cbCaptchaService.SelectedItem.Text == "RDC")
                            {
                                this.Ticket.ifBPCAutoCaptcha = false;
                                this.Ticket.ifRDAutoCaptcha = false;
                                this.Ticket.ifRDCAutoCaptcha = true;
                                this.Ticket.ifCPTAutoCaptcha = false;
                                this.Ticket.ifDBCAutoCaptcha = false;
                                this.Ticket.ifDCAutoCaptcha = false;
                                this.Ticket.ifOCR = false;
                                this.Ticket.ifCAutoCaptcha = false;
                                this.Ticket.ifROCR = false;
                                this.Ticket.ifBoloOCR = false;
                                this.Ticket.ifSOCR = false;
                                this.Ticket.if2C = false;
                                this.Ticket.ifAntigateAutoCaptcha = false;
                                this.Ticket.ifCaptchator = false;
                                this.Ticket.ifAC1AutoCaptcha = false;
                            }
                        }
                        else
                        {
                            this.Ticket.ifBPCAutoCaptcha = true;
                            this.Ticket.ifRDAutoCaptcha = false;
                            this.Ticket.ifRDCAutoCaptcha = false;
                            this.Ticket.ifCPTAutoCaptcha = false;
                            this.Ticket.ifDBCAutoCaptcha = false;
                            this.Ticket.ifDCAutoCaptcha = false;
                            this.Ticket.ifOCR = false;
                            this.Ticket.ifCAutoCaptcha = false;
                            this.Ticket.ifROCR = false;
                            this.Ticket.ifBoloOCR = false;
                            this.Ticket.ifSOCR = false;
                            this.Ticket.if2C = false;
                            this.Ticket.ifAntigateAutoCaptcha = false;
                            this.Ticket.ifCaptchator = false;
                            this.Ticket.ifAC1AutoCaptcha = false;
                        }

                        this.Ticket.ifPlaySoundAlert = this.chkPlayMusic.Checked;
                        this.Ticket.ifPesistSessionInEachSearch = this.chkPersistSession.Checked;
                        this.Ticket.Permission = this.AXSByPassWaitingRoom.Checked;
                        this.Ticket.Permission = this.AXSByPassWaitingRoom.Checked;
                        this.Ticket.NoOfSearches = this.nudNumberOfSearches.Value;
                        this.Ticket.ifAutoBuy = this.chkAutoBuy.Checked;
                        this.Ticket.ifSendEmail = this.chkSendEmail.Checked;
                        this.Ticket.ifUseProxies = this.chkUseProxies.Checked;
                        this.Ticket.StartUsingProxiesFrom = this.nudStartUsingProxiesFrom.Value;
                        this.Ticket.ifUseProxiesInCaptchaSource = this.chkUseProxiesInCaptchaSource.Checked;


                        lock (this.Ticket)
                        {
                            this.Ticket.SaveTicket();
                        }

                        if (this.Ticket.NoOfSearches != lastNumberOfSearches)
                        {
                            this.Ticket.Searches.changeDelegate = null;
                            if (this.Ticket.Searches != null)
                            {
                                this.Ticket.Searches.changeDelegate = new SortedBindingList.SortableBindingList<ITicketSearch>.dlgOnChange(this.changeDelegateHandler);
                                this.AXSSearchBindingSource.DataSource = this.Ticket.Searches;
                            }
                        }

                        this.Ticket.changeSettingsDuringSearching(lastNumberOfSearches);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
        }

        private void grvFindTickets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grvFindTickets.CurrentCell is DataGridViewImageCell)
                {
                    DataGridViewImageCell flagCell = (DataGridViewImageCell)grvFindTickets.CurrentCell;
                    ITicketSearch search = (ITicketSearch)grvFindTickets.CurrentCell.OwningRow.DataBoundItem;
                    if (((Boolean)search.FlagImage.Tag))
                    {
                        search.FlagImage = global::Automatick.Properties.Resources.Flag16Disable;
                        search.FlagImage.Tag = false;
                    }
                    else
                    {
                        search.FlagImage = global::Automatick.Properties.Resources.Flag_red;
                        search.FlagImage.Tag = true;
                    }

                    changeDelegateHandler(search);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void rbAutoBuy_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.AXSSearchBindingSource.Current != null)
                //{
                //    if (this.AXSSearchBindingSource.Current is ITicketSearch)
                //    {
                //        ITicketSearch search = (ITicketSearch)this.AXSSearchBindingSource.Current;
                //        search.autoBuy();
                //    }
                //}

                if (this.grvFindTickets.SelectedRows != null && this.grvFindTickets.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in this.grvFindTickets.SelectedRows)
                    {
                        try
                        {
                            ITicketSearch search = (ITicketSearch)row.DataBoundItem;
                            search.autoBuy();
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void rbAutoBuyGuest_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.AXSSearchBindingSource.Current != null)
                //{
                //    if (this.AXSSearchBindingSource.Current is ITicketSearch)
                //    {
                //        ITicketSearch search = (ITicketSearch)this.AXSSearchBindingSource.Current;
                //        search.autoBuyGuest();
                //    }
                //}

                if (this.grvFindTickets.SelectedRows != null && this.grvFindTickets.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in this.grvFindTickets.SelectedRows)
                    {
                        try
                        {
                            ITicketSearch search = (ITicketSearch)row.DataBoundItem;
                            search.autoBuyGuest();
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void chkUseProxies_CheckedChanged(object sender, EventArgs e)
        {
            populateUseProxiesSettings();
        }

        private void populateUseProxiesSettings()
        {
            try
            {
                if (chkUseProxies.Checked)
                {
                    chkUseProxiesInCaptchaSource.Enabled = true;
                    nudStartUsingProxiesFrom.Enabled = true;
                }
                else
                {
                    chkUseProxiesInCaptchaSource.Checked = false;
                    chkUseProxiesInCaptchaSource.Enabled = false;
                    nudStartUsingProxiesFrom.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }



        private void rbAutoBuyWithoutProxy_Click(object sender, EventArgs e)
        {
            //if (this.AXSSearchBindingSource.Current != null)
            //{
            //    if (this.AXSSearchBindingSource.Current is ITicketSearch)
            //    {
            //        ITicketSearch search = (ITicketSearch)this.AXSSearchBindingSource.Current;
            //        search.autoBuyWithoutProxy();
            //    }
            //}

            try
            {
                if (this.grvFindTickets.SelectedRows != null && this.grvFindTickets.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in this.grvFindTickets.SelectedRows)
                    {
                        try
                        {
                            ITicketSearch search = (ITicketSearch)row.DataBoundItem;
                            search.autoBuyWithoutProxy();
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }
        private void grvFindTickets_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }
        }
    }
}
