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

        public frmSelectAccount(ITicket ticket)
        {
            InitializeComponent();
            
            AXSTicket AXSTicket = (AXSTicket)ticket;

            try
            {
                int i = 0;
                foreach (AXSTicketAccount account in AXSTicket.AllTMAccounts)
                {
                    RadioButton rb = new RadioButton();

                    String strCount = "";
                    try
                    {
                        if (AXSTicket.BuyHistory.ContainsKey(account.EmailAddress))
                        {
                            strCount = " Bought = " + AXSTicket.BuyHistory[account.AccountEmail].ToString();
                            if (AXSTicket.BuyHistory[account.AccountEmail] >= account.BuyingLimit)
                            {
                                rb.ForeColor = System.Drawing.Color.OrangeRed;
                                rb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                            }
                        }
                    }
                    catch { }

                    rb.Text = account.AccountName + " (" + account.AccountEmail + ")" + strCount;
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
