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
        public frmSelectDeliveryOption(List<string> deliveryOptions, ITicket ticket)
        {
            this._ticket = ticket;
            
            InitializeComponent();

            int currYLocation = 10;
            bool ifFirst = true;

            foreach (String item in deliveryOptions)
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
            }

            
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
                        this._ticket.DeliveryCountry = rb.Tag.ToString().Replace("Customers in", "").Replace("Customers", "").Trim();
                        this._ticket.DeliveryOption = rb.Text;
                        this._ticket.SaveTicket();
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
    }
}
