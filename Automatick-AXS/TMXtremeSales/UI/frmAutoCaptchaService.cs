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
            applyPermission();

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

                this.rbCaptchatorAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifCaptchator;
                this.rbBPCAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifBPCAutoCaptcha;
                this.rbCPTAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifCPTAutoCaptcha;
                this.rbDBCAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifDBCAutoCaptcha;
                this.rbRDAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifRDAutoCaptcha;
                this.rbRDCAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifRDCAutoCaptcha;
                this.rbCAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifCAutoCaptcha;
                this.rbNOCRAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifOCR;
                this.rbRAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifROCR;
                this.rbDCAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifDCAutoCaptcha;
                this.rbBoloAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifBoloOCR;
                this.rb2CAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.if2CAutoCaptcha;
                this.rbCRAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifCRAutoCaptcha;
                this.rbNOCRAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifNoCaptchaOCRAutoCaptcha;
                this.rbAAutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifAntigate;
                this.rbAC1AutoCaptcha.Checked = this.MainForm.AppStartUp.AutoCaptchaService.ifAC1AutoCaptcha;
            }
        }

        private void applyPermission()
        {
            try
            {
                Boolean ifAutoCaptchaSelectedPreviously = this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha;

                IEnumerable<AccessRights.AccessList> allControlsAccesses = this.MainForm.AppPermissions.AllAccessList.Where(p => p.form == "frmTicket");//"frmTicket");
                foreach (AccessRights.AccessList obj in allControlsAccesses)
                {
                    if (obj.name == rbBPCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbBPCAutoCaptcha.Visible = access;
                        grpBPC.Visible = access;

                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifBPCAutoCaptcha = false;
                            rbBPCAutoCaptcha.Checked = false;
                            rbBPCAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbRDAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbRDAutoCaptcha.Visible = access;
                        grpRD.Visible = access;
                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifRDAutoCaptcha = false;
                            rbRDAutoCaptcha.Checked = false;
                        }
                    }
                    else if (obj.name == rbRDCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbRDCAutoCaptcha.Visible = access;
                        grpRDC.Visible = access;
                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifRDCAutoCaptcha = false;
                            rbRDCAutoCaptcha.Checked = false;
                        }
                    }
                    else if (obj.name == rbCPTAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        grpCPT.Visible = access;
                        rbCPTAutoCaptcha.Visible = access;
                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifCPTAutoCaptcha = false;
                            rbCPTAutoCaptcha.Checked = false;
                            rbCPTAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbDBCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbDBCAutoCaptcha.Visible = access;
                        grpDBC.Visible = access;

                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifDBCAutoCaptcha = false;
                            rbDBCAutoCaptcha.Checked = false;
                            rbDBCAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbDCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbDCAutoCaptcha.Visible = access;
                        grpDC.Visible = access;

                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifDCAutoCaptcha = false;
                            rbDCAutoCaptcha.Checked = false;
                            rbDCAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbCAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbCAutoCaptcha.Visible = access;
                        grpC.Visible = access;

                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifCAutoCaptcha = false;
                            rbCAutoCaptcha.Checked = false;
                            rbCAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbNOCRAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbNOCRAutoCaptcha.Visible = access;
                        

                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifOCR = false;
                            rbNOCRAutoCaptcha.Checked = false;
                            rbNOCRAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == this.rbRAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbRAutoCaptcha.Visible = access;
                        grpR.Visible = access;

                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifROCR = false;
                            rbRAutoCaptcha.Checked = false;
                            rbRAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == this.rbBoloAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbBoloAutoCaptcha.Visible = access;
                        grpBolo.Visible = access;

                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifBoloOCR = false;
                            rbBoloAutoCaptcha.Checked = false;
                            rbBoloAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == this.rb2CAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rb2CAutoCaptcha.Visible = access;
                        grp2C.Visible = access;

                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.if2CAutoCaptcha = false;
                            rb2CAutoCaptcha.Checked = false;
                            rb2CAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbCRAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbCRAutoCaptcha.Visible = access;
                        
                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifCRAutoCaptcha = false;
                            rbCRAutoCaptcha.Checked = false;
                            rbCRAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbNOCRAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbNOCRAutoCaptcha.Visible = access;
                       

                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifNoCaptchaOCRAutoCaptcha = false;
                            rbNOCRAutoCaptcha.Checked = false;
                            rbNOCRAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbCaptchatorAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbCaptchatorAutoCaptcha.Visible = access;

                        grpCaptchator.Visible = access;

                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifCaptchator = false;
                            rbCaptchatorAutoCaptcha.Checked = false;
                            rbCaptchatorAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbAAutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbAAutoCaptcha.Visible = access;
                        grpAntigate.Visible = access;

                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAntigate = false;
                            rbAAutoCaptcha.Checked = false;
                            rbAAutoCaptcha.Enabled = false;
                        }
                    }
                    else if (obj.name == rbAC1AutoCaptcha.Name)
                    {
                        Boolean access = Boolean.Parse(obj.access);
                        rbAC1AutoCaptcha.Visible = access;
                        grpAC1.Visible = access;
                        if (!access)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAC1AutoCaptcha = false;
                            rbAC1AutoCaptcha.Checked = false;
                            rbAC1AutoCaptcha.Enabled = false;
                        }
                    }
                }

                AccessRights.AccessList accessObj = allControlsAccesses.FirstOrDefault(p => p.access.ToLower() == "true");

                if (accessObj != null)
                {
                    if (!this.MainForm.AppStartUp.AutoCaptchaService.ifBPCAutoCaptcha && !this.MainForm.AppStartUp.AutoCaptchaService.ifCPTAutoCaptcha && !this.MainForm.AppStartUp.AutoCaptchaService.ifDBCAutoCaptcha && !this.MainForm.AppStartUp.AutoCaptchaService.ifRDAutoCaptcha && !this.MainForm.AppStartUp.AutoCaptchaService.ifDCAutoCaptcha && !this.MainForm.AppStartUp.AutoCaptchaService.ifCAutoCaptcha && !this.MainForm.AppStartUp.AutoCaptchaService.ifOCR && !this.MainForm.AppStartUp.AutoCaptchaService.ifROCR
                       && !this.MainForm.AppStartUp.AutoCaptchaService.ifBoloOCR && !this.MainForm.AppStartUp.AutoCaptchaService.if2CAutoCaptcha && !this.MainForm.AppStartUp.AutoCaptchaService.ifCRAutoCaptcha && !this.MainForm.AppStartUp.AutoCaptchaService.ifNoCaptchaOCRAutoCaptcha && !this.MainForm.AppStartUp.AutoCaptchaService.ifCaptchator && !this.MainForm.AppStartUp.AutoCaptchaService.ifAntigate && !this.MainForm.AppStartUp.AutoCaptchaService.ifAC1AutoCaptcha && !this.MainForm.AppStartUp.AutoCaptchaService.ifRDCAutoCaptcha)
                    {

                        if (accessObj.name == rbRDAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifRDAutoCaptcha = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            // this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbRDAutoCaptcha.Checked = true;
                        }
                        if (accessObj.name == rbRDCAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifRDCAutoCaptcha = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            // this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbRDCAutoCaptcha.Checked = true;
                        }

                        if (accessObj.name == rbBPCAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifBPCAutoCaptcha = true;
                            // this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            // this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbBPCAutoCaptcha.Checked = true;
                            this.rbBPCAutoCaptcha.Enabled = true;

                        }
                        else if (accessObj.name == rbCPTAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifCPTAutoCaptcha = true;
                            //     this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            //   this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbCPTAutoCaptcha.Checked = true;
                            this.rbCPTAutoCaptcha.Enabled = true;

                        }
                        else if (accessObj.name == rbDBCAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifDBCAutoCaptcha = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            //  this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbDBCAutoCaptcha.Checked = true;
                            this.rbDBCAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == rbDCAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifDCAutoCaptcha = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            //  this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbDCAutoCaptcha.Checked = true;
                            this.rbDCAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == rbCAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifCAutoCaptcha = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            //   this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbCAutoCaptcha.Checked = true;
                            this.rbCAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == rbNOCRAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifOCR = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            //  this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbNOCRAutoCaptcha.Checked = true;
                            this.rbNOCRAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == this.rbRAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifROCR = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            // this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbRAutoCaptcha.Checked = true;
                            this.rbRAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == this.rbBoloAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifBoloOCR = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            // this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbBoloAutoCaptcha.Checked = true;
                            this.rbBoloAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == this.rb2CAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.if2CAutoCaptcha = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            // this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rb2CAutoCaptcha.Checked = true;
                            this.rb2CAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == rbCRAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifCRAutoCaptcha = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            //  this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbCRAutoCaptcha.Checked = true;
                            this.rbCRAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == rbNOCRAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifNoCaptchaOCRAutoCaptcha = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            //  this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbNOCRAutoCaptcha.Checked = true;
                            this.rbNOCRAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == rbCaptchatorAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifCaptchator = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            //  this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbNOCRAutoCaptcha.Checked = true;
                            this.rbNOCRAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == rbAAutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAntigate = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            //  this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbAAutoCaptcha.Checked = true;
                            this.rbAAutoCaptcha.Enabled = true;
                        }
                        else if (accessObj.name == rbAC1AutoCaptcha.Name)
                        {
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAC1AutoCaptcha = true;
                            this.MainForm.AppStartUp.AutoCaptchaService.ifAutoCaptcha = ifAutoCaptchaSelectedPreviously;
                            //  this.chkAutoCaptcha.Checked = ifAutoCaptchaSelectedPreviously;
                            this.rbAC1AutoCaptcha.Checked = true;
                            this.rbAC1AutoCaptcha.Enabled = true;
                        }
                    }
                }
                else
                {
                    //this.rbDBCAutoCaptcha.Enabled = false;
                    //this.rbCPTAutoCaptcha.Enabled = false;
                    //this.rbBPCAutoCaptcha.Enabled = false;
                    //this.chkAutoCaptcha.Enabled = false;
                    //this.chkAutoCaptcha.Checked = false;
                    //this.Ticket.ifAutoCaptcha = false;
                }
            }
            catch (Exception)
            {

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
                this.MainForm.AppStartUp.AutoCaptchaService.NewCPTPassword = String.Empty;
                this.MainForm.AppStartUp.AutoCaptchaService.NewCPTUserName = String.Empty;

                this.MainForm.AppStartUp.AutoCaptchaService.NewRDUserName = String.Empty;
                this.MainForm.AppStartUp.AutoCaptchaService.NewRDPassword = String.Empty;

                this.MainForm.AppStartUp.AutoCaptchaService.CTRIP = this.txtCTIP.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.CTRPort = this.txtCTPort.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.CTRUserName = this.txtCTUsername.Text;
                this.MainForm.AppStartUp.AutoCaptchaService.CTRPassword = this.txtCTPassword.Text;

                this.MainForm.AppStartUp.AutoCaptchaService.ifCaptchator = this.rbCaptchatorAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifBPCAutoCaptcha = this.rbBPCAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifCPTAutoCaptcha = this.rbCPTAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifDBCAutoCaptcha = this.rbDBCAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifRDAutoCaptcha = this.rbRDAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifRDCAutoCaptcha = this.rbRDCAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifCAutoCaptcha = this.rbCAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifOCR = this.rbNOCRAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifDCAutoCaptcha = this.rbDCAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifROCR = this.rbRAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifBoloOCR = this.rbBoloAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.if2CAutoCaptcha = this.rb2CAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifCRAutoCaptcha = this.rbCRAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifNoCaptchaOCRAutoCaptcha = this.rbNOCRAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifAntigate = this.rbAAutoCaptcha.Checked;
                this.MainForm.AppStartUp.AutoCaptchaService.ifAC1AutoCaptcha = this.rbAC1AutoCaptcha.Checked;

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

            try
            {
                AccessRights.AccessList CapsiumServers = this.MainForm.AppPermissions.AllAccessList.Single(p => p.form == "CapsiumServers");

                if (CapsiumServers != null)
                {
                    string _controlName = CapsiumServers.name;
                    string _controlVisibility = CapsiumServers.access;
                    if (Boolean.Parse(CapsiumServers.access))
                    {
                        this.grBCaptchaSettings.Visible = true;
                    }
                    else
                    {
                        this.grBCaptchaSettings.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
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
                this.MainForm.stopTixTox();
                this.MainForm.customBuildCheck();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}