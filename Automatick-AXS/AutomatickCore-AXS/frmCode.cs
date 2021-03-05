using Automatick.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automatick
{
    public partial class frmCode : C1.Win.C1Ribbon.C1RibbonForm
    {
        String code = String.Empty;

        public frmCode(String TopMost, String CardNumber, String PhoneNumber, String Parameters)
        {
            InitializeComponent();

            this.textBox1.Text = String.Empty;

            this.Text = TopMost; //"Enter Code for " + accountName+", Qty: ";
            this.lblRowSectionv.Text = Parameters;//"Account name : " + _selectedAccount.AccountName;
            this.lblCardNumber.Text = CardNumber;//"Phone No : " + _selectedAccount.Telephone;

            this.lblPhone.Text = PhoneNumber;

            //if (this._search != null)
            //{
            //    this.lblRowSection.Text = String.Format("Section = {0}, Row = {1}, Price = {2}", String.IsNullOrEmpty(this._search.Section) ? "--" : this._search.Section, String.IsNullOrEmpty(this._search.Row) ? "--" : this._search.Row, String.IsNullOrEmpty(this._search.Price) ? "--" : this._search.Price);
            //}
            //else
            //{
            //    this.lblRowSection.Text = "";
            //}
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                this.code = this.textBox1.Text.Trim();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            this.Close();
        }

        public String promptCode()
        {
            this.ShowDialog();
            return code;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.code = String.Empty;
            this.Close();
        }

        private void frmCode_Load(object sender, EventArgs e)
        {

        }
    }
}
