using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Automatick.Core;
using System.Text.RegularExpressions;
using System.Linq;

namespace Automatick
{
    public partial class frmEmail : C1.Win.C1Ribbon.C1RibbonForm, IForm
    {
        #region IAddForm Members

        public IMainForm MainForm
        {
            get;
            set;
        }

        public void load()
        {
            if (MainForm.AppStartUp.EmailSetting != null)
            {
                this.txtEmail.Text = this.MainForm.AppStartUp.EmailSetting.EmailAddress;
                this.txtPassword.Text = this.MainForm.AppStartUp.EmailSetting.EmailPassword;
                this.txtSMTP.Text = this.MainForm.AppStartUp.EmailSetting.SMTPServer;
                this.nudPort.Value = this.MainForm.AppStartUp.EmailSetting.SMTPPort;
                this.cbIsSSL.Checked = this.MainForm.AppStartUp.EmailSetting.IsSSLRequired;
                this.emailBindingSource.DataSource = this.MainForm.AppStartUp.EmailSetting.EmailAddresses;
            }
        }

        public void save()
        {
            if (MainForm.AppStartUp.EmailSetting != null)
            {
                this.MainForm.AppStartUp.EmailSetting.EmailAddress = this.txtEmail.Text;
                this.MainForm.AppStartUp.EmailSetting.EmailPassword = this.txtPassword.Text;
                this.MainForm.AppStartUp.EmailSetting.SMTPServer = this.txtSMTP.Text;
                this.MainForm.AppStartUp.EmailSetting.SMTPPort = this.nudPort.Value;
                this.MainForm.AppStartUp.EmailSetting.IsSSLRequired = this.cbIsSSL.Checked;
                this.MainForm.AppStartUp.SaveEmails();
            }
        }

        public bool validate()
        {
            //throw new NotImplementedException();
            return true;
        }

        public void onClosed()
        {
            this.MainForm.loadEmails();
            GC.SuppressFinalize(this);
            //GC.Collect();
        }
        #endregion                

        String EmailRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public frmEmail(IMainForm mainForm)
        {
            this.MainForm = mainForm;
            InitializeComponent();
        }

        private void frmEmail_Load(object sender, EventArgs e)
        {
            this.load();
        }

        private void frmEmail_FormClosed(object sender, FormClosedEventArgs e)
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
        
        private void gvBackpageReply_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                if (!Regex.IsMatch(e.FormattedValue.ToString(), EmailRegex, RegexOptions.IgnoreCase))
                {
                    MessageBox.Show("Email address is in invalid format. Please correct.");
                    e.Cancel = true;
                }
                else
                {
                    Email email = this.MainForm.AppStartUp.EmailSetting.EmailAddresses.FirstOrDefault(p => !String.IsNullOrEmpty(p.EmailAddress) ? p.EmailAddress == e.FormattedValue.ToString() : false );
                    if (email != emailBindingSource.Current  && email != null)
                    {
                        MessageBox.Show("Email address already exists. Please provide the unique email address.");
                        e.Cancel = true;
                    }
                }
            }
            else if (emailBindingSource.Current != null)
            {
                Email email = (Email)emailBindingSource.Current;
                if (String.IsNullOrEmpty(email.EmailAddress))
                {
                    emailBindingSource.Remove(email);
                    emailBindingSource.CancelEdit();
                }
            }
        }
    }
}
