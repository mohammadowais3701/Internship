using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Automatick.Core;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Automatick
{
    public partial class ucRecentItem : UserControl
    {
        private ITicket _ticket = null;
        private IMainForm _mainForm;
        public ITicket Ticket
        {
            get
            {
                return this._ticket;
            }
            set
            {
                this._ticket = value;
                load();
            }
        }

        public ucRecentItem(ITicket ticket, IMainForm mainForm)
        {
            InitializeComponent();
            this._ticket = ticket;
            this._mainForm = mainForm;
        }

        private void ucRecentItem_Load(object sender, EventArgs e)
        {
            load();
        }

        private void load()
        {
            if (this._ticket != null)
            {
                this.lblTicketName.Text = this._ticket.TicketName;
                _ticket.onChangeForGauge = new ChangeDelegateForGauge(this.onChangeHandlerForGauge);
            }

            onChangeHandlerForGauge();
        }

        private void onChangeHandlerForGauge()
        {
            try
            {

                mapFoundRate();
                mapBuyRate();
                Application.DoEvents();
            }
            catch (Exception ex)
            {

            }
        }

        private void mapFoundRate()
        {
            try
            {
                if (this._ticket != null)
                {
                    decimal RunCount = (decimal)this._ticket.RunCount;
                    decimal FoundCount = (decimal)this._ticket.FoundCount;
                    decimal Percentage = 0;

                    try
                    {
                        if (RunCount > 0)
                        {
                            Percentage = FoundCount / RunCount * 100;
                        }

                        this.txtFoundRate.Text = getFoundRate(Percentage);
                        this.rgFoundRate.Value = (int)Percentage;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                {
                    this.txtFoundRate.Text = "Very low";
                    this.rgFoundRate.Value = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void mapBuyRate()
        {
            try
            {
                if (this._ticket != null)
                {
                    decimal FoundCount = (decimal)this._ticket.FoundCount;
                    decimal BuyCount = (decimal)this._ticket.BuyCount;
                    decimal Percentage = 0;

                    if (FoundCount > 0)
                    {
                        Percentage = BuyCount / FoundCount * 100;
                    }

                    this.txtBuyRate.Text = getFoundRate(Percentage);
                    this.rgBuyRate.Value = (int)Percentage;
                }
                else
                {
                    this.txtBuyRate.Text = "Very low";
                    this.rgBuyRate.Value = 0;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private String getFoundRate(decimal percentage)
        {
            String result = "Very low";

            try
            {
                if (percentage >= 0 && percentage <= 20)
                {
                    result = "Very low";
                }
                else if (percentage > 20 && percentage <= 40)
                {
                    result = "Low";
                }
                else if (percentage > 40 && percentage <= 60)
                {
                    result = "Medium";
                }
                else if (percentage > 60 && percentage <= 80)
                {
                    result = "High";
                }
                else if (percentage > 80 && percentage <= 100)
                {
                    result = "Very high";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        private void onClick()
        {
            try
            {
                if (!this.Ticket.isRunning)
                {
                    frmFindTicket frm = new frmFindTicket(this.Ticket, this._mainForm);
                    if (!this.Ticket.isServiceSelected || !this.Ticket.ifCapsium)
                    {
                        if (_ticket.URL.Contains("bit.ly") || (_ticket.URL.Contains(".queue") || _ticket.URL.Contains("q.axs.co.uk")) || (this._mainForm.AppStartUp.GlobalSetting.ifEventko && _ticket.URL.Contains("evenko.ca")) || ((this._mainForm.AppStartUp.GlobalSetting.IfWeb || this._mainForm.AppStartUp.GlobalSetting.ifMobile) && _ticket.URL.Contains("axs.com")) || ((this._mainForm.AppStartUp.GlobalSetting.IfWeb || this._mainForm.AppStartUp.GlobalSetting.ifMobile) && _ticket.URL.Contains("shop.axs.co.uk")))
                        {
                            this._ticket.start(this._mainForm.CaptchaForm.CaptchaQueue, this._mainForm.CaptchaBrowserForm.CaptchaQueue, this._mainForm.AppStartUp.SoundAlert, this._mainForm.AppStartUp.AutoCaptchaService, this._mainForm.AppStartUp.Accounts, this._mainForm.AppStartUp.EmailSetting, this._mainForm.AppStartUp.GlobalSetting);
                            frm.Show();
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
                else
                {
                    frmFindTicket frm = (frmFindTicket)this._ticket.TicketSearchWindow;
                    frm.Activate();
                    frm.Focus();
                    if (frm.WindowState == FormWindowState.Minimized)
                    {
                        frm.WindowState = FormWindowState.Normal;
                    }
                }
                if (this._ticket.onChangeStartOrStop != null)
                {
                    this._ticket.onChangeStartOrStop(this._ticket);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void lblTicketName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                onClick();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void lblStop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (this.Ticket.isRunning)
                {
                    this.Ticket.stop();
                    if (this._ticket.onChangeStartOrStop != null)
                    {
                        this._ticket.onChangeStartOrStop(this._ticket);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void lblEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (!this.Ticket.isRunning)
                {
                    frmTicket frm = new frmTicket(this._mainForm, this._ticket);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void lblDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (!this.Ticket.isRunning)
                {
                    ITicket ticketToDelete = this.Ticket;

                    if (MessageBox.Show("Do you really want to delete \"" + ticketToDelete.TicketName + "\"?", "Are you Sure?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        frmMain mainForm = (frmMain)this._mainForm;
                        mainForm.AppStartUp.Tickets.Remove((AXSTicket)ticketToDelete);
                        ticketToDelete.DeleteTicket();
                        mainForm.populateTickets();
                        mainForm.populateRecentTickets();

                        // unschedule ticket on delete ticket
                        lock (mainForm._scheduledTickets)
                        {
                            if (mainForm._scheduledTickets.ContainsKey(ticketToDelete.TicketID))
                            {
                                if (mainForm._scheduledTickets[ticketToDelete.TicketID] != null)
                                {
                                    mainForm._scheduledTickets[ticketToDelete.TicketID].Dispose();
                                    GC.SuppressFinalize(mainForm._scheduledTickets[ticketToDelete.TicketID]);
                                }

                                mainForm._scheduledTickets.Remove(ticketToDelete.TicketID);
                            }
                        }

                        lock (mainForm._unscheduledTickets)
                        {
                            if (mainForm._unscheduledTickets.ContainsKey(ticketToDelete.TicketID))
                            {
                                if (mainForm._unscheduledTickets[ticketToDelete.TicketID] != null)
                                {
                                    mainForm._unscheduledTickets[ticketToDelete.TicketID].Dispose();
                                    GC.SuppressFinalize(mainForm._unscheduledTickets[ticketToDelete.TicketID]);
                                    //GC.Collect();
                                }

                                mainForm._unscheduledTickets.Remove(ticketToDelete.TicketID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void lblDuplicate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                AXSTicket ticket = (AXSTicket)_ticket.Clone();

                ticket.TicketID = UniqueKey.getUniqueKey();

                int i = 0;

                string ticketName = ticket.TicketName;

                do
                {
                    i++;
                    ticket.TicketName = ticketName + " - Copy(" + i + ")";
                    ticket.LastUsedDateTime = DateTime.Now;
                    ticket.isRunning = false;
                }
                while (File.Exists(this.Ticket.FileLocation + @"\Tickets\" + ticket.TicketName + ".tevent"));

                ticket.SaveTicket();

                this._mainForm.AppStartUp.Tickets.Add(ticket);

                this._mainForm.populateTickets();

                this._mainForm.populateRecentTickets();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}
