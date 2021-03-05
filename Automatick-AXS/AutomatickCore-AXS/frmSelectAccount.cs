using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using Automatick.Core;


namespace Automatick
{
    public partial class frmSelectAccount : C1.Win.C1Ribbon.C1RibbonForm
    {
        ITicketAccount _selectedAccount = null;
        ITicketSearch _search = null;
        public frmSelectAccount(ITicket ticket, ITicketSearch search)
        {
            InitializeComponent();
            this._search = search;
            AXSTicket AXSTicket = (AXSTicket)ticket;

            try
            {
                int loginAcc = 0;

                if (!search.isGuest)
                {
                    loginAcc = AXSTicket.AllTMAccounts.Count(pred => pred.GroupName.Equals("login"));
                }

                int i = 0;
                foreach (AXSTicketAccount account in AXSTicket.AllTMAccounts)
                {
                    if (!search.isGuest && loginAcc == 0 ? true : (this._search.isGuest == account.GroupName.Equals("guest")))
                    {
                        RadioButton rb = new RadioButton();

                        String strCount = "";
                        try
                        {
                            if (AXSTicket.BuyHistory.ContainsKey(account.EmailAddress))
                            {
                                strCount = " Bought = " + AXSTicket.BuyHistory[account.EmailAddress].ToString();
                                if (AXSTicket.BuyHistory[account.EmailAddress] >= account.BuyingLimit)
                                {
                                    rb.ForeColor = System.Drawing.Color.OrangeRed;
                                    rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                }
                            }
                        }
                        catch { }

                        rb.Text = account.AccountName + " (" + account.EmailAddress + ")" + strCount;
                        rb.Name = account.EmailAddress.Replace("@", "").Replace(".", "") + i.ToString();
                        rb.AutoSize = true;
                        rb.Tag = account;
                        rb.Location = new Point(15, i + 10);
                        pnlAccounts.Controls.Add(rb);

                        if (i == 0)
                        {
                            rb.Checked = true;
                        }
                        i = i + 25;
                    }
                }
                this.Text = "Select account for " + AXSTicket.TicketName;
                this.lblTicketName.Text = "Event name : " + AXSTicket.TicketName;
                if (this._search != null)
                {
                    this.lblRowSection.Text = String.Format("Section = {0}, Row = {1}, Price = {2}", String.IsNullOrEmpty(this._search.Section) ? "--" : this._search.Section, String.IsNullOrEmpty(this._search.Row) ? "--" : this._search.Row, String.IsNullOrEmpty(this._search.Price) ? "--" : this._search.Price);
                }
                else
                {
                    this.lblRowSection.Text = "";
                }
            }
            catch
            {
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            foreach (Control item in pnlAccounts.Controls)
            {
                if (item.GetType() == typeof(RadioButton))
                {
                    RadioButton rb = (RadioButton)item;
                    if (rb.Checked)
                    {
                        this._selectedAccount = (ITicketAccount)rb.Tag;
                        break;
                    }
                }
            }
            this.Close();
        }

        public ITicketAccount promptAccount()
        {
            this.ShowDialog();
            return this._selectedAccount;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._selectedAccount = null;
            this.Close();
        }
    }
}
