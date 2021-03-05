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
    public partial class frmSelectDeliveryOption : C1.Win.C1Ribbon.C1RibbonForm
    {
        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        ITicket _ticket = null;
        ITicketSearch _search = null;
        ITicketDeliveryOption _selectedDeliveryOption = null;

        public frmSelectDeliveryOption(List<string> deliveryOptions, ITicket ticket, ITicketSearch search,Dictionary<string,string> DeliveryOptions)
        {
            int count = 0;
            try
            {
                this._ticket = ticket;
                this._search = search;
                InitializeComponent();

                int currYLocation = 10;
                bool ifFirst = true;

                foreach (String item in deliveryOptions)
                {
                    //if (count==0 || item.ToLower().Contains("electronic"))
                    {
                        RadioButton rb = new RadioButton();
                        rb.Name = "rb" + currYLocation.ToString();
                        rb.Text = item;
                        rb.Tag = item;
                        rb.AutoSize = true;
                        rb.Location = new Point(20, currYLocation);
                        if (ifFirst)
                        {
                            rb.Checked = true;
                            ifFirst = false;
                        }
                        pnlOptions.Controls.Add(rb);
                        currYLocation = currYLocation + 25;
                        //rb.Checked = true;
                    }
                    //count++;
                }
                this.Text = "Select delivery option for " + this._ticket.TicketName;
                this.lblTicketName.Text = "Event name : " + this._ticket.TicketName;
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
            { }

        }

        public frmSelectDeliveryOption(Dictionary<String, HtmlNode> deliveryOption, ITicket ticket, ITicketSearch search)
        {
            try
            {
                this._ticket = ticket;
                this._search = search;

                //this._countryWiseDeliveryOptions = countryWiseDeliveryOptions;
                InitializeComponent();

                int currYLocation = 10;
                bool ifFirst = true;
                foreach (KeyValuePair<String, HtmlNode> item in deliveryOption)
                {
                    RadioButton rb = new RadioButton();
                    rb.Name = "rb" + currYLocation.ToString();
                    rb.Text = item.Key;
                    rb.Tag = item.Key;
                    rb.AutoSize = true;
                    rb.Location = new Point(20, currYLocation);
                    if (ifFirst)
                    {
                        rb.Checked = true;
                        ifFirst = false;
                    }
                    pnlOptions.Controls.Add(rb);
                    currYLocation = currYLocation + 25;

                }
                this.Text = "Select delivery option for " + this._ticket.TicketName;
                this.lblTicketName.Text = "Event name : " + this._ticket.TicketName;
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
            { }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            foreach (Control item in pnlOptions.Controls)
            {
                if (item.GetType() == typeof(RadioButton))
                {
                    RadioButton rb = (RadioButton)item;
                    if (rb.Checked)
                    {
                        if ((!this._search.isTix) && (this._search.isWeb || this._search.isJSON || this._search.isEventko))
                        {
                            this._ticket.DeliveryCountry = rb.Tag.ToString().Replace("Customers in", "").Replace("Customers", "").Trim();
                            this._ticket.DeliveryOption = rb.Text;
                            this._selectedDeliveryOption = new AXSDeliveryOption();
                            this._selectedDeliveryOption.DeliveryOption = rb.Text;
                            this._ticket.SaveTicket();
                        }
                        else
                        {
                            this._ticket.DeliveryOption = rb.Text;
                            this._ticket.SaveTicket();
                            this._selectedDeliveryOption = new AXSDeliveryOption();
                            this._selectedDeliveryOption.DeliveryOption = rb.Text;
                            this._ticket.SaveTicket(); 
                        }
                        break;
                    }
                }
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._ticket.DeliveryCountry = String.Empty;
            this._ticket.DeliveryOption = String.Empty;
            this._ticket.SaveTicket();

            this.Close();
        }

        public ITicketDeliveryOption promptDeliveryOption()
        {
            this.ShowDialog();
            return this._selectedDeliveryOption;
        }
    }
}
