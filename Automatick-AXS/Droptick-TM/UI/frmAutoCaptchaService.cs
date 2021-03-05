using Automatick.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automatick
{
    public partial class frmAutoCaptchaService : C1.Win.C1Ribbon.C1RibbonForm, IForm
    {
        #region IAddForm Members

        public IMainForm MainForm
        {
            get;
            set;
        }

        public void load()
        {
            if (MainForm.AppStartUp.AutoCaptchaService != null)
            {
                this.txtBPCKey.Text = this.MainForm.AppStartUp.AutoCaptchaService.BPCKey;
                this.txtBPCUsername.Text = this.MainForm.AppStartUp.AutoCaptchaService.BPCUserName;
                this.txtBPCPassword.Text = this.MainForm.AppStartUp.AutoCaptchaService.BPCPassword;

                this.txtDBCUsername.Text = this.MainForm.AppStartUp.AutoCaptchaService.DBCUserName;
                this.txtDBCPassword.Text = this.MainForm.AppStartUp.AutoCaptchaService.DBCPassword;
                this.txtRDCUsername.Text = this.MainForm.AppStartUp.AutoCaptchaService.RDCUserName;
                this.txtRDCPassword.Text = this.MainForm.AppStartUp.AutoCaptchaService.RDCPassword;

                this.txtRDUsername.Text = this.MainForm.AppStartUp.AutoCaptchaService.RDUserName;
                this.txtRDPassword.Text = this.MainForm.AppStartUp.AutoCaptchaService.RDPassword;

                this.txtCPTUsername.Text = this.MainForm.AppStartUp.AutoCaptchaService.CPTUserName;
                this.txtCPTPassword.Text = this.MainForm.AppStartUp.AutoCaptchaService.CPTPassword;

                this.txtDCUsername.Text = this.MainForm.AppStartUp.AutoCaptchaService.DCUserName;
                this.txtDCPassword.Text = this.MainForm.AppStartUp.AutoCaptchaService.DCPassword;
                this.txtDCPort.Text = this.MainForm.AppStartUp.AutoCaptchaService.DCPort;

                this.txtOCRIP.Text = this.MainForm.AppStartUp.AutoCaptchaService.OCRIP;
                this.txtOCRPort.Text = this.MainForm.AppStartUp.AutoCaptchaService.OCRPort;

                this.txtCUsername.Text = this.MainForm.AppStartUp.AutoCaptchaService.CUserName;
                this.txtCPassword.Text = this.MainForm.AppStartUp.AutoCaptchaService.CPassword;
                this.txtCIP.Text = this.MainForm.AppStartUp.AutoCaptchaService.CHost;
                this.txtCPort.Text = this.MainForm.AppStartUp.AutoCaptchaService.CPort;

                this.txtRUsername.Text = this.MainForm.AppStartUp.AutoCaptchaService.ROCRUsername;
                this.txtRPassword.Text = this.MainForm.AppStartUp.AutoCaptchaService.ROCRPassword;
                this.txtRIP.Text = this.MainForm.AppStartUp.AutoCaptchaService.ROCRIP;
                this.txtRPort.Text = this.MainForm.AppStartUp.AutoCaptchaService.ROCRPort;

                this.txtBoloIP.Text = this.MainForm.AppStartUp.AutoCaptchaService.BOLOIP;
                this.txtBoloPort.Text = this.MainForm.AppStartUp.AutoCaptchaService.BOLOPORT;

                this.txtSOCRIP.Text = this.MainForm.AppStartUp.AutoCaptchaService.SOCRIP;
                this.txtSOCRPort.Text = this.MainForm.AppStartUp.AutoCaptchaService.SOCRPort;

                this.txtC2Key.Text = this.MainForm.AppStartUp.AutoCaptchaService.C2Key;
                this.txtAntigateKey.Text = this.MainForm.AppStartUp.AutoCaptchaService.AntigateKey;
                this.txtAC1Key.Text = ServerPortsPicker.ServerPortsPickerInstance.AC1Credential;// this.MainForm.AppStartUp.AutoCaptchaService.AC1Key;

                this.txtCTIP.Text = this.MainForm.AppStartUp.AutoCaptchaService.CTRIP = "64.71.77.133";
                this.txtCTPort.Text = this.MainForm.AppStartUp.AutoCaptchaService.CTRPort = "101";
                this.txtCTUsername.Text = this.MainForm.AppStartUp.AutoCaptchaService.CTRUserName;
                this.txtCTPassword.Text = this.MainForm.AppStartUp.AutoCaptchaService.CTRPassword;
            }
        }

        public void save()
        {
            if (MainForm.AppStartUp.AutoCaptchaService != null)
            {
                this.MainForm.AppStartUp.AutoCaptchaService.BPCKey = this.txtBPCKey.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.BPCUserName = this.txtBPCUsername.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.BPCPassword = this.txtBPCPassword.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.DBCUserName = this.txtDBCUsername.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.DBCPassword = this.txtDBCPassword.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.RDUserName = this.txtRDUsername.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.RDPassword = this.txtRDPassword.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.RDCUserName = this.txtRDCUsername.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.RDCPassword = this.txtRDCPassword.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.CPTUserName = this.txtCPTUsername.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.CPTPassword = this.txtCPTPassword.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.DCUserName = this.txtDCUsername.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.DCPassword = this.txtDCPassword.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.DCPort = this.txtDCPort.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.OCRIP = this.txtOCRIP.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.OCRPort = this.txtOCRPort.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.CUserName = this.txtCUsername.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.CPassword = this.txtCPassword.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.CHost = this.txtCIP.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.CPort = this.txtCPort.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.ROCRUsername = this.txtRUsername.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.ROCRPassword = this.txtRPassword.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.ROCRIP = this.txtRIP.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.ROCRPort = this.txtRPort.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.BOLOIP = this.txtBoloIP.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.BOLOPORT = this.txtBoloPort.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.SOCRIP = this.txtSOCRIP.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.SOCRPort = this.txtSOCRPort.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.C2Key = this.txtC2Key.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.AntigateKey = this.txtAntigateKey.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.AC1Key = this.txtAC1Key.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.CTRIP = this.txtCTIP.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.CTRPort = this.txtCTPort.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.CTRUserName = this.txtCTUsername.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.CTRPassword = this.txtCTPassword.Text;

                this.MainForm.AppStartUp.SaveAutoCaptchaServices();
            }
        }

        public bool validate()
        {
            return true;
        }

        public void onClosed()
        {
            this.MainForm.loadAutoCaptchaServices();
            GC.SuppressFinalize(this);
            //GC.Collect();
        }
        #endregion

        public frmAutoCaptchaService(IMainForm mainForm)
        {
            this.MainForm = mainForm;
            InitializeComponent();
        }

        private void frmAutoCaptchaService_Load(object sender, EventArgs e)
        {
            this.load();
            this.MainForm.AppPermissions.ApplyPemissions(this);
        }

        private void frmAutoCaptchaService_FormClosed(object sender, FormClosedEventArgs e)
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
    }
}
