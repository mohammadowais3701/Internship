using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Automatick.Core;
using System.Linq;
namespace Automatick
{
    public partial class frmDeliveryOption : C1.Win.C1Ribbon.C1RibbonForm, IForm
    {
        #region IAddForm Members

        public IMainForm MainForm
        {
            get;
            set;
        }

        public void load()
        {
            if (MainForm.AppStartUp.TicketDeliveryOptions != null)
            {
                this.iTicketDeliveryOptionBindingSource.DataSource = this.MainForm.AppStartUp.TicketDeliveryOptions;
            }
        }

        public void save()
        {
            if (MainForm.AppStartUp.TicketDeliveryOptions != null)
            {
                this.MainForm.AppStartUp.SaveDeliveryOptions();
            }
        }

        public bool validate()
        {
            //throw new NotImplementedException();
            gvDeliveryOptions.EndEdit();
            if (this.MainForm.AppStartUp.TicketDeliveryOptions != null)
            {
                foreach (ITicketDeliveryOption item in this.MainForm.AppStartUp.TicketDeliveryOptions)
                {                    
                    if (String.IsNullOrEmpty(item.DeliveryOption) || String.IsNullOrEmpty(item.DeliveryCountry))
                    {
                        MessageBox.Show("One or more delivery option or delivery country is empty. Please review and resolve.");
                        return false;
                    }
                }
                foreach (ITicketDeliveryOption item in this.MainForm.AppStartUp.TicketDeliveryOptions)
                {
                    int count = this.MainForm.AppStartUp.TicketDeliveryOptions.Count(p => p.DeliveryCountry == item.DeliveryCountry && p.DeliveryOption == item.DeliveryOption);

                    if (count >=2)
                    {
                        MessageBox.Show("One or more delivery option with delivery country is duplicated. Please review and resolve.");
                        return false;
                    }
                }
            }
            return true;
        }

        public void onClosed()
        {
            this.MainForm.loadDeliveryOptions();
            GC.SuppressFinalize(this);
            //GC.Collect();
        }
        #endregion                

        public frmDeliveryOption(IMainForm mainForm)
        {
            this.MainForm = mainForm;
            InitializeComponent();
        }

        private void frmDeliveryOption_Load(object sender, EventArgs e)
        {
            this.load();
        }

        private void frmDeliveryOption_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.onClosed();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                this.save();                
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvDeliveryOptions_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                ITicketDeliveryOption DeliveryOption = this.MainForm.AppStartUp.TicketDeliveryOptions.FirstOrDefault(p => (!String.IsNullOrEmpty(p.DeliveryOption) ? p.DeliveryOption == e.FormattedValue.ToString() : false) && (!String.IsNullOrEmpty(p.DeliveryCountry) ? p.DeliveryCountry == gvDeliveryOptions.Rows[e.RowIndex].Cells[2].Value.ToString() : false));
                if (DeliveryOption != iTicketDeliveryOptionBindingSource.Current && DeliveryOption != null)
                {
                    MessageBox.Show("Delivery option already exists with the same country. Please provide the unique delivery option.");
                    e.Cancel = true;
                }
                else if (iTicketDeliveryOptionBindingSource.Current != null)
                {
                    DeliveryOption = (ITicketDeliveryOption)iTicketDeliveryOptionBindingSource.Current;
                    if (DeliveryOption.DeliveryOptionId == "Default" && e.FormattedValue.ToString() != DeliveryOption.DeliveryOption && e.ColumnIndex == 1)
                    {
                        e.Cancel = true;
                        MessageBox.Show("You cannot edit the predefined Delivery Option.", "Operation not allowed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gvDeliveryOptions.CancelEdit();
                        gvDeliveryOptions.EndEdit();
                    }
                    else if (DeliveryOption.DeliveryOptionId == "Default" && e.FormattedValue.ToString() != DeliveryOption.DeliveryCountry && e.ColumnIndex == 2)
                    {
                        e.Cancel = true;
                        MessageBox.Show("You cannot edit the predefined Delivery Option.", "Operation not allowed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gvDeliveryOptions.CancelEdit();
                        gvDeliveryOptions.EndEdit();
                    }
                }
            }
            else if (iTicketDeliveryOptionBindingSource.Current != null)
            {
                ITicketDeliveryOption DeliveryOption = (ITicketDeliveryOption)iTicketDeliveryOptionBindingSource.Current;
                if (DeliveryOption.DeliveryOptionId == "Default" && e.FormattedValue.ToString() != DeliveryOption.DeliveryOption && e.ColumnIndex == 1)
                {
                    e.Cancel = true;
                    MessageBox.Show("You cannot edit the predefined Delivery Option.", "Operation not allowed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gvDeliveryOptions.CancelEdit();
                    gvDeliveryOptions.EndEdit();
                }
                else if (DeliveryOption.DeliveryOptionId == "Default" && e.FormattedValue.ToString() != DeliveryOption.DeliveryCountry && e.ColumnIndex == 2)
                {
                    e.Cancel = true;
                    MessageBox.Show("You cannot edit the predefined Delivery Option.", "Operation not allowed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gvDeliveryOptions.CancelEdit();
                    gvDeliveryOptions.EndEdit();
                }                
            }
        }

        private void gvDeliveryOptions_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row.DataBoundItem.GetType() == typeof(AXSDeliveryOption))
                {
                    ITicketDeliveryOption p = (ITicketDeliveryOption)e.Row.DataBoundItem;
                    if (p.DeliveryOptionId == "Default")
                    {
                        e.Cancel = true;
                        MessageBox.Show("You cannot delete the predefined Delivery Option.", "Operation not allowed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {

            }
        }

        private void gvDeliveryOptions_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }
        }

        private void gvDeliveryOptions_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (iTicketDeliveryOptionBindingSource.Current != null && (e.ColumnIndex ==1 ||e.ColumnIndex==2))
                {
                    ITicketDeliveryOption DeliveryOption = (ITicketDeliveryOption)iTicketDeliveryOptionBindingSource.Current;
                    if (DeliveryOption.DeliveryOptionId == "Default")
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception)
            {

            }
        }        
    }
}
