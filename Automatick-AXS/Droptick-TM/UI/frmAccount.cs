﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Automatick.Core;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;


namespace Automatick
{
    public partial class frmAccount : C1.Win.C1Ribbon.C1RibbonForm, IForm
    {
        bool firstLoad;
        String EmailRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        #region IAddForm Members

        public IMainForm MainForm
        {
            get;
            set;
        }

        public void load()
        {
            firstLoad = true;
            if (MainForm.AppStartUp.Accounts != null)
            {
                this.iAccountBindingSource.DataSource = this.MainForm.AppStartUp.Accounts;
                if (this.MainForm.AppStartUp.Accounts.Count != 0)
                {
                    AXSTicketAccount acc = (AXSTicketAccount)gvAccounts.CurrentRow.DataBoundItem;

                }
                if (this.MainForm.AppStartUp.Accounts.Count > 0)
                {
                    this.groupBox1.Enabled = true;
                    this.groupBox2.Enabled = true;
                    this.groupBox3.Enabled = true;
                }
            }
        }

        public void save()
        {
            if (MainForm.AppStartUp.Accounts != null)
            {
                this.MainForm.AppStartUp.SaveAccounts();
            }
        }

        public bool validate()
        {
            gvAccounts.EndEdit();
            if (this.MainForm.AppStartUp.Accounts != null)
            {
                foreach (ITicketAccount item in this.MainForm.AppStartUp.Accounts)
                {
                    if (String.IsNullOrEmpty(item.AccountName) || String.IsNullOrEmpty(item.EmailAddress))
                    {
                        MessageBox.Show("One or more account name or email is empty. Please review and resolve.");
                        return false;
                    }
                    if (String.IsNullOrEmpty(item.Country) || item.Country.Equals("Select Country"))
                    {
                        MessageBox.Show("Select Country For Account " + item.AccountName);
                        return false;
                    }
                    if (item.Country.ToLower().Equals("united states") || item.Country.ToLower().Equals("australia") || item.Country.ToLower().Equals("canada"))
                    {
                        if (String.IsNullOrEmpty(item.State))
                        {
                            MessageBox.Show("Select State For Account " + item.AccountName);
                            return false;
                        }
                    }
                    else
                    {
                        item.State = "";
                    }
                    if (String.IsNullOrEmpty(item.FirstName))
                    {
                        MessageBox.Show("Enter First Name for Account " + item.AccountName);
                        return false;
                    }

                    if (String.IsNullOrEmpty(item.LastName))
                    {
                        MessageBox.Show("Enter Last Name for Account " + item.AccountName);
                        return false;
                    }

                    if (string.IsNullOrEmpty(item.EmailAddress))
                    {
                        MessageBox.Show("Enter Email Address for Account " + item.AccountName);
                        return false;
                    }
                    else if (String.IsNullOrEmpty(item.ConfirmEmail))
                    {
                        MessageBox.Show("Enter Confirmation Email Address for Account " + item.AccountName);
                        return false;
                    }
                    else if (!Regex.IsMatch(this.Email.Text.ToString(), EmailRegex, RegexOptions.IgnoreCase))
                    {
                        MessageBox.Show("Email address is in invalid format. Please correct.");
                        return false;
                    }
                    else if (!Regex.IsMatch(this.ConfirmEmail.Text.ToString(), EmailRegex, RegexOptions.IgnoreCase))
                    {
                        MessageBox.Show("Confirmation Email address is in invalid format. Please correct.");
                        return false;
                    }
                    else if (!item.EmailAddress.Equals(item.ConfirmEmail))
                    {
                        MessageBox.Show("Email Address Mismatch " + item.AccountName);
                        return false;
                    }
                    if (String.IsNullOrEmpty(item.Address1))
                    {
                        MessageBox.Show("Enter Address1 For Account  " + item.AccountName);
                        return false;
                    }
                    if (String.IsNullOrEmpty(item.Town))
                    {
                        MessageBox.Show("Enter City Name for Account " + item.AccountName);
                        return false;
                    }

                    if (String.IsNullOrEmpty(item.PostCode))
                    {
                        MessageBox.Show("Enter Post Code for Account " + item.AccountName);
                        return false;
                    }

                    if (String.IsNullOrEmpty(item.Telephone))
                    {
                        MessageBox.Show("Enter Phone Number for Account " + item.AccountName);
                        return false;
                    }
                    else if (!(item.Telephone.All(Char.IsNumber)))
                    {
                        MessageBox.Show("Enter Correct Phone Number for Account " + item.AccountName);
                        return false;
                    }
                    if (String.IsNullOrEmpty(item.Mobile))
                    {
                    }
                    else if (!(item.Mobile.All(Char.IsNumber)))
                    {
                        MessageBox.Show("Enter Correct Mobile Number for Account " + item.AccountName);
                        return false;
                    }

                    //if (String.IsNullOrEmpty(item.Password))
                    //{
                    //    MessageBox.Show("Enter Password For Account " + item.AccountName);
                    //    return false;
                    //}

                    if (String.IsNullOrEmpty(item.CardType) || item.CardType.Equals("Select Card Type"))
                    {
                        MessageBox.Show("Select Credit Card Type For Account " + item.AccountName);
                        return false;
                    }
                    if (String.IsNullOrEmpty(item.CardNumber))
                    {
                        MessageBox.Show("Enter Card Number for Account " + item.AccountName);
                        return false;
                    }
                    else if (!(item.CardNumber.All(Char.IsNumber)))
                    {
                        MessageBox.Show("Enter Correct Card Number for Account " + item.AccountName);
                        return false;
                    }

                    if (String.IsNullOrEmpty(item.CCVNum))
                    {
                        MessageBox.Show("Enter CCV Number for Account " + item.AccountName);
                        return false;
                    }
                    else if (!(item.CCVNum.All(Char.IsNumber)))
                    {
                        MessageBox.Show("Enter Correct CCV Number for Account " + item.AccountName);
                        return false;
                    }

                    if (String.IsNullOrEmpty(item.ExpiryMonth) || item.ExpiryMonth.Equals("Month"))
                    {
                        MessageBox.Show("Select Expiry Month For Account " + item.AccountName);
                        return false;
                    }
                    if (String.IsNullOrEmpty(item.ExpiryYear) || item.ExpiryYear.Equals("Year"))
                    {
                        MessageBox.Show("Select Expiry Year For Account " + item.AccountName);
                        return false;
                    }


                }

            }
            return true;
        }

        public void onClosed()
        {
            this.MainForm.loadAccounts();
            GC.SuppressFinalize(this);
            //GC.Collect();
        }
        #endregion

        public frmAccount(IMainForm mainForm)
        {
            this.MainForm = mainForm;
            InitializeComponent();
        }

        private void frmAccount_Load(object sender, EventArgs e)
        {
            this.load();


        }

        private void frmAccount_FormClosed(object sender, FormClosedEventArgs e)
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

        private void gvAccounts_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            ITicketAccount account;
            if (!String.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                if (!Regex.IsMatch(e.FormattedValue.ToString(), EmailRegex, RegexOptions.IgnoreCase) && e.ColumnIndex == 3)
                {
                    MessageBox.Show("Email address is in invalid format. Please correct.");
                    e.Cancel = true;
                }

            }
            else if (iAccountBindingSource.Current != null)
            {
                account = (ITicketAccount)iAccountBindingSource.Current;
                if (String.IsNullOrEmpty(account.AccountName) && String.IsNullOrEmpty(account.EmailAddress))
                {
                    iAccountBindingSource.Remove(account);
                    iAccountBindingSource.CancelEdit();
                }
            }


        }

        private void gvAccounts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }
        }


        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogOpen = new OpenFileDialog();
            dialogOpen.Filter = "Text files (*.txt)|*.txt";
            dialogOpen.Title = "Import accounts from text file";
            dialogOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialogOpen.FileName = "Automatick-Accounts.txt";
            if (dialogOpen.ShowDialog() == DialogResult.OK)
            {
                if (this.MainForm.AppStartUp.Accounts == null)
                {
                    this.MainForm.AppStartUp.Accounts = new SortedBindingList.SortableBindingList<AXSTicketAccount>();
                }

                StreamReader sr = new StreamReader(dialogOpen.FileName);
                String strLine = "";
                do
                {
                    strLine = sr.ReadLine();
                    String[] splitted = strLine.Split(',');
                    AXSTicketAccount account = new AXSTicketAccount();
                    if (splitted.Length >= 1)
                    {
                        account.AccountName = splitted[0];
                    }
                    if (splitted.Length >= 2)
                    {
                        account.Country = splitted[1];
                    }
                    if (splitted.Length >= 3)
                    {
                        account.State = splitted[2];
                    }
                    if (splitted.Length >= 4)
                    {
                        account.FirstName = splitted[3];
                    }
                    if (splitted.Length >= 5)
                    {
                        account.LastName = splitted[4];
                    }
                    if (splitted.Length >= 6)
                    {
                        account.EmailAddress = splitted[5];
                    }
                    if (splitted.Length >= 7)
                    {
                        account.ConfirmEmail = splitted[6];
                    }
                    if (splitted.Length >= 8)
                    {
                        account.Password = splitted[7];
                    }
                    if (splitted.Length >= 9)
                    {
                        account.Address1 = splitted[8];
                    }
                    if (splitted.Length >= 10)
                    {
                        account.Address2 = splitted[9];
                    }
                    if (splitted.Length >= 11)
                    {
                        account.Town = splitted[10];
                    }
                    if (splitted.Length >= 12)
                    {
                        account.PostCode = splitted[11];
                    }
                    if (splitted.Length >= 13)
                    {
                        account.Telephone = splitted[12];
                    }
                    if (splitted.Length >= 14)
                    {
                        account.Mobile = splitted[13];
                    }
                    if (splitted.Length >= 15)
                    {
                        account.CardType = splitted[14];
                    }
                    if (splitted.Length >= 16)
                    {
                        account.CardNumber = splitted[15];
                    }
                    if (splitted.Length >= 17)
                    {
                        account.CCVNum = splitted[16];
                    }
                    if (splitted.Length >= 18)
                    {
                        account.ExpiryMonth = splitted[17];
                    }
                    if (splitted.Length >= 19)
                    {
                        account.ExpiryYear = splitted[18];
                    }


                    this.MainForm.AppStartUp.Accounts.Add(account);

                } while (!sr.EndOfStream);

                this.iAccountBindingSource.DataSource = this.MainForm.AppStartUp.Accounts;
            }
            this.DialogResult = DialogResult.None;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text files (*.txt)|*.txt";
            saveDialog.Title = "Save accounts into text file";
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveDialog.FileName = "Automatick-Accounts.txt";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                if (!String.IsNullOrEmpty(saveDialog.FileName))
                {
                    if (this.MainForm.AppStartUp.Accounts != null)
                    {
                        if (this.MainForm.AppStartUp.Accounts.Count() > 0)
                        {
                            StreamWriter sr = new StreamWriter(saveDialog.FileName, false);
                            foreach (AXSTicketAccount item in this.MainForm.AppStartUp.Accounts)
                            {
                                String strAccount = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18}", item.AccountName, item.Country, item.State, item.FirstName, item.LastName, item.EmailAddress, item.ConfirmEmail, item.Password, item.Address1, item.Address2, item.Town, item.PostCode, item.Telephone, item.Mobile, item.CardType, item.CardNumber, item.CCVNum, item.ExpiryMonth, item.ExpiryYear);
                                sr.WriteLine(strAccount);
                            }
                            sr.Close();
                        }
                    }
                }
            }
            this.DialogResult = DialogResult.None;
        }


        BindingSource _States;
        //private void Country_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    AXSTicketAccount acc = new AXSTicketAccount();
        //    if (!firstLoad)
        //    {
        //        if(gvAccounts.CurrentRow!=null)
        //        {
        //        acc = (AXSTicketAccount)gvAccounts.CurrentRow.DataBoundItem;
        //        if (acc != null)
        //        {
        //            for (int k = 0; k < this.MainForm.AppStartUp.Accounts.Count; k++)
        //            {
        //                if (!String.IsNullOrEmpty(acc.AccountName))
        //                {
        //                    if (acc.AccountName == this.MainForm.AppStartUp.Accounts[k].AccountName)
        //                    {
        //                        try
        //                        {

        //                            acc = this.MainForm.AppStartUp.Accounts[k];
        //                            if (acc != null)
        //                            {
        //                                //acc.Country = this.Country.SelectedItem.ToString();

        //                                if (acc.Country.ToLower().Trim() == "canada")
        //                                {
        //                                    _States = new BindingSource();
        //                                    _States.DataSource = this.MainForm.AppStartUp._Canada_States;
        //                                    States.DisplayMember = "Value";
        //                                    States.ValueMember = "Key";
        //                                    States.DataSource = _States;
        //                                    this.States.Visible = true;



        //                                    this.label4.Visible = true;
        //                                }
        //                                else if (acc.Country.ToLower().Trim() == "united states")
        //                                {
        //                                    _States = new BindingSource();
        //                                    _States.DataSource = this.MainForm.AppStartUp._US_States;
        //                                    States.DisplayMember = "Value";
        //                                    States.ValueMember = "Key";
        //                                    States.DataSource = _States;
        //                                    this.States.Visible = true;


        //                                    this.label4.Visible = true;
        //                                }
        //                                else if (acc.Country.ToLower().Trim() == "australia")
        //                                {
        //                                    _States = new BindingSource();
        //                                    _States.DataSource = this.MainForm.AppStartUp._Australia_States;
        //                                    States.DisplayMember = "Value";
        //                                    States.ValueMember = "Key";
        //                                    States.DataSource = _States;
        //                                    this.States.Visible = true;

        //                                    this.label4.Visible = true;
        //                                }
        //                                else
        //                                {
        //                                    this.States.Visible = false;
        //                                    this.label4.Visible = false;
        //                                }
        //                                //this.States.Text = acc.State;
        //                                break;
        //                            }

        //                        }
        //                        catch
        //                        {
        //                            try
        //                            {
        //                                acc = this.MainForm.AppStartUp.Accounts[0];
        //                            }
        //                            catch { }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }

        //    }
        //    else
        //    {
        //        firstLoad = false;
        //    }
        //     if (this.Country.Text.ToLower().Trim() == "canada")
        //        {
        //            _States = new BindingSource();
        //            _States.DataSource = this.MainForm.AppStartUp._Canada_States;
        //            States.DisplayMember = "Value";
        //            States.ValueMember = "Key";
        //            States.DataSource = _States;
        //            this.States.Visible = true;
        //            this.label4.Visible = true;
        //        }
        //        else if (this.Country.Text.ToLower().Trim() == "united states")
        //        {
        //            _States = new BindingSource();
        //            _States.DataSource = this.MainForm.AppStartUp._US_States;
        //            States.DisplayMember = "Value";
        //            States.ValueMember = "Key";
        //            States.DataSource = _States;
        //            this.States.Visible = true;
        //            this.label4.Visible = true;
        //        }
        //        else if (this.Country.Text.ToLower().Trim() == "australia")
        //        {
        //            _States = new BindingSource();
        //            _States.DataSource = this.MainForm.AppStartUp._Australia_States;
        //            States.DisplayMember = "Value";
        //            States.ValueMember = "Key";
        //            States.DataSource = _States;
        //            this.States.Visible = true;
        //            this.label4.Visible = true;
        //        }
        //        else
        //        {
        //            this.States.Visible = false;
        //            this.label4.Visible = false;
        //        }

        //    if (acc != null)
        //    {
        //        if(!String.IsNullOrEmpty(acc.State))
        //        {
        //        this.States.Text = acc.State;
        //        }
        //    }
        //}

        private void States_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                foreach (AXSTicketAccount acc in this.MainForm.AppStartUp.Accounts)
                {
                    if (this.txtAccName.Text == acc.AccountName)
                    {
                        KeyValuePair<string, string> item = new KeyValuePair<string, string>();
                        item = (KeyValuePair<string, string>)this.States.SelectedItem;
                        acc.State = item.Value;
                        this.States.Enabled = false;
                    }
                }
            }
            catch
            {
            }

        }

        private void Country_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                foreach (AXSTicketAccount acc in this.MainForm.AppStartUp.Accounts)
                {
                    if (this.txtAccName.Text == acc.AccountName)
                    {
                        acc.Country = this.Country.SelectedItem.ToString();
                        if (acc.Country.ToLower() == "canada")
                        {
                            _States = new BindingSource();
                            _States.DataSource = this.MainForm.AppStartUp._Canada_States;
                            States.DisplayMember = "Value";
                            States.ValueMember = "Key";
                            States.DataSource = _States;
                            this.States.Visible = true;
                            this.States.Enabled = true;
                            this.label4.Visible = true;


                        }
                        else if (acc.Country.ToLower() == "united states")
                        {
                            _States = new BindingSource();
                            _States.DataSource = this.MainForm.AppStartUp._US_States;
                            States.DisplayMember = "Value";
                            States.ValueMember = "Key";
                            States.DataSource = _States;
                            this.States.Visible = true;
                            this.States.Enabled = true;
                            this.label4.Visible = true;

                        }
                        else if (acc.Country.ToLower() == "australia")
                        {
                            _States = new BindingSource();
                            _States.DataSource = this.MainForm.AppStartUp._Australia_States;
                            States.DisplayMember = "Value";
                            States.ValueMember = "Key";
                            States.DataSource = _States;
                            this.States.Visible = true;
                            this.States.Enabled = true;
                            this.label4.Visible = true;

                        }
                        else
                        {
                            this.States.Enabled = false;
                            this.States.Visible = false;
                            this.label4.Visible = false;
                            acc.State = "";
                        }
                    }
                }
            }
            catch
            {
            }
        }


        private void gvAccounts_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (gvAccounts.CurrentRow != null)
                {
                    int index = gvAccounts.CurrentRow.Index;
                    if (gvAccounts.CurrentCell.ColumnIndex == 0)
                    {
                        if (gvAccounts.CurrentCell.Value == null)
                        {
                            this.Country.SelectedIndex = 0;
                            // this.States.SelectedIndex = 0;
                            this.comboBox1.SelectedIndex = 0;
                            this.comboBox2.SelectedIndex = 0;
                            this.comboBox3.SelectedIndex = 0;
                        }

                    }
                    if (gvAccounts[0, index].Value != null)
                    {
                        groupBox1.Enabled = true;
                        groupBox2.Enabled = true;
                        groupBox3.Enabled = true;
                    }
                    else
                    {
                        groupBox1.Enabled = false;
                        groupBox2.Enabled = false;
                        groupBox3.Enabled = false;
                    }
                }
            }
            catch
            {
            }

        }



        private void gvAccounts_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (gvAccounts.CurrentRow != null)
                {
                    int index = gvAccounts.CurrentRow.Index;
                    int count = this.MainForm.AppStartUp.Accounts.Count() - 1;
                    if (String.IsNullOrEmpty(this.MainForm.AppStartUp.Accounts[count].AccountName))
                    {
                        this.groupBox1.Enabled = false;
                        this.groupBox2.Enabled = false;
                        this.groupBox3.Enabled = false;
                    }
                }
            }
            catch
            {
            }


        }

        private void gvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (gvAccounts.CurrentRow != null)
                {
                    AXSTicketAccount acc = new AXSTicketAccount();
                    acc = (AXSTicketAccount)gvAccounts.CurrentRow.DataBoundItem;
                    if (acc != null)
                    {
                        if (String.IsNullOrEmpty(acc.Country))
                        {
                            this.Country.SelectedIndex = 0;
                        }
                        if (string.IsNullOrEmpty(acc.State))
                        {

                        }
                        if (String.IsNullOrEmpty(acc.CardType))
                        {
                            this.comboBox1.SelectedIndex = 0;
                        }
                        if (String.IsNullOrEmpty(acc.ExpiryMonth))
                        {
                            this.comboBox3.SelectedIndex = 0;
                        }
                        if (String.IsNullOrEmpty(acc.ExpiryYear))
                        {
                            this.comboBox2.SelectedIndex = 0;
                        }
                    }
                    if (gvAccounts[0, gvAccounts.CurrentRow.Index].Value != null)
                    {
                        this.groupBox1.Enabled = true;
                        this.groupBox2.Enabled = true;
                        this.groupBox3.Enabled = true;
                    }
                    else
                    {
                        this.groupBox1.Enabled = false;
                        this.groupBox2.Enabled = false;
                        this.groupBox3.Enabled = false;
                    }
                }
            }
            catch
            {
            }
        }


    }

      

       
       

    
         

        //BindingSource _States;
        //private void Country_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    AXSTicketAccount acc =new AXSTicketAccount();
        //    if (!firstLoad)
        //    {
        //        acc = (AXSTicketAccount)gvAccounts.CurrentRow.DataBoundItem;
        //        for (int k = 0; k < this.MainForm.AppStartUp.Accounts.Count; k++)
        //        {
        //            if (acc.AccountName == this.MainForm.AppStartUp.Accounts[k].AccountName)
        //            {
        //                try
        //                {

        //                    acc = this.MainForm.AppStartUp.Accounts[k + 1];
        //                    if (acc.Country.ToLower().Trim() == "canada")
        //                    {
        //                        _States = new BindingSource();
        //                        _States.DataSource = this.MainForm.AppStartUp._Canada_States;
        //                        States.DisplayMember = "Value";
        //                        States.ValueMember = "Key";
        //                        States.DataSource = _States;
        //                        this.States.Visible = true;
        //                        this.label4.Visible = true;
        //                    }
        //                    else if (acc.Country.ToLower().Trim() == "united states")
        //                    {
        //                        _States = new BindingSource();
        //                        _States.DataSource = this.MainForm.AppStartUp._US_States;
        //                        States.DisplayMember = "Value";
        //                        States.ValueMember = "Key";
        //                        States.DataSource = _States;
        //                        this.States.Visible = true;
        //                        this.label4.Visible = true;
        //                    }
        //                    else if (acc.Country.ToLower().Trim() == "australia")
        //                    {
        //                        _States = new BindingSource();
        //                        _States.DataSource = this.MainForm.AppStartUp._Australia_States;
        //                        States.DisplayMember = "Value";
        //                        States.ValueMember = "Key";
        //                        States.DataSource = _States;
        //                        this.States.Visible = true;
        //                        this.label4.Visible = true;
        //                    }
        //                    else
        //                    {
        //                        this.States.Visible = false;
        //                        this.label4.Visible = false;
        //                    }
        //                    this.States.Text = acc.State;
        //                    break;


        //                }
        //                catch
        //                {
        //                    try
        //                    {
        //                        acc = this.MainForm.AppStartUp.Accounts[0];
        //                    }
        //                    catch { }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        firstLoad = false;  
        //    }
        //        if (this.Country.Text.ToLower().Trim() == "canada")
        //        {
        //            _States = new BindingSource();
        //            _States.DataSource = this.MainForm.AppStartUp._Canada_States;
        //            States.DisplayMember = "Value";
        //            States.ValueMember = "Key";
        //            States.DataSource = _States;
        //            this.States.Visible = true;
        //            this.label4.Visible = true;
        //        }
        //        else if (this.Country.Text.ToLower().Trim() == "united states")
        //        {
        //            _States = new BindingSource();
        //            _States.DataSource = this.MainForm.AppStartUp._US_States;
        //            States.DisplayMember = "Value";
        //            States.ValueMember = "Key";
        //            States.DataSource = _States;
        //            this.States.Visible = true;
        //            this.label4.Visible = true;
        //        }
        //        else if (this.Country.Text.ToLower().Trim() == "australia")
        //        {
        //            _States = new BindingSource();
        //            _States.DataSource = this.MainForm.AppStartUp._Australia_States;
        //            States.DisplayMember = "Value";
        //            States.ValueMember = "Key";
        //            States.DataSource = _States;
        //            this.States.Visible = true;
        //            this.label4.Visible = true;
        //        }
        //        else
        //        {
        //            this.States.Visible = false;
        //            this.label4.Visible = false;
        //        }
        //        this.States.Text = acc.State;
            

            
           
        //}

    
       

        //private void gvAccounts_RowEnter(object sender, DataGridViewCellEventArgs e)
        //{
        //    try
        //    {
        //        AXSTicketAccount acc = (AXSTicketAccount)gvAccounts.CurrentRow.DataBoundItem;
        //        this.States.Text = acc.State;
        //    }
        //    catch
        //    {
        //    }
        //}        
   
}