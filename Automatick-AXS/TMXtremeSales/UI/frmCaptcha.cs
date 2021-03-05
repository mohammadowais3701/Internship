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
    public partial class frmCaptcha : C1.Win.C1Ribbon.C1RibbonForm, ICaptchaForm
    {
        public BindingList<ITicketSearch> CaptchaQueue
        {
            get;
            set;
        }

        public frmCaptcha()
        {
            CaptchaQueue = new BindingList<ITicketSearch>();
            CaptchaQueue.AllowNew = true;
            CaptchaQueue.ListChanged += new ListChangedEventHandler(CaptchaQueue_ListChanged);
            InitializeComponent();
            this.tMSearchBindingSource.DataSource = this.CaptchaQueue;
        }

        private void frmCaptcha_Load(object sender, EventArgs e)
        {
            this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width - 2, SystemInformation.VirtualScreen.Height - this.Height - 32);
        }

        private void tMSearchBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            try
            {
                if (tMSearchBindingSource.Count > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            this.Visible = true;
                            if (!this.Focused)
                            {
                                this.Activate();
                            }
                            if (!this.txtCaptchaText.Focused)
                            {
                                this.txtCaptchaText.Focus();
                            }
                            this.lblRemainingCaptcha.Text = "Remaining captcha : " + tMSearchBindingSource.Count;
                        }));
                    }
                    else
                    {
                        this.Visible = true;

                        if (!this.Focused)
                        {
                            this.Activate();
                        }
                        if (!this.txtCaptchaText.Focused)
                        {
                            this.txtCaptchaText.Focus();
                        }
                        this.lblRemainingCaptcha.Text = "Remaining captcha : " + tMSearchBindingSource.Count;
                    }
                }
                else
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate { this.Visible = false; }));
                    }
                    else
                    {
                        this.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        void CaptchaQueue_ListChanged(object sender, ListChangedEventArgs e)
        {

        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (this.tMSearchBindingSource.Current != null)
            {
                this.pbCaptcha.Image = null;

                AXSSearch tmpSearch = ((AXSSearch)this.tMSearchBindingSource.Current);
                lock (this.CaptchaQueue)
                {
                    this.CaptchaQueue.Remove(tmpSearch);
                }
                tmpSearch.Captcha.CaptchaWords = txtCaptchaText.Text;
                tmpSearch.Captcha.CaptchaImage.Dispose();
                this.txtCaptchaText.Text = "";
                tmpSearch.Captcha.captchaentered.Set();
            }
        }

        private void tMSearchBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.tMSearchBindingSource.Current != null)
                {
                    AXSSearch tmpSearch = ((AXSSearch)this.tMSearchBindingSource.Current);
                    if (tmpSearch.Captcha.CaptchaImage == null)
                    {
                        lock (this.CaptchaQueue)
                        {
                            this.CaptchaQueue.Remove(tmpSearch);
                        }
                        tmpSearch.Captcha.captchaentered.Set();
                        return;
                    }

                    this.pbCaptcha.Image = ((AXSSearch)this.tMSearchBindingSource.Current).Captcha.CaptchaImage;
                    try
                    {
                        Bitmap b = ((AXSSearch)this.tMSearchBindingSource.Current).Captcha.CaptchaImage;

                        if (b.Size.Height > 100)
                        {
                            if (this.pbCaptcha.InvokeRequired)
                            {
                                this.pbCaptcha.Invoke(new MethodInvoker(delegate
                                {
                                    this.pbCaptcha.Size = new Size(this.Width - 20, this.Height - 85);
                                }));
                            }
                            else
                            {
                                this.pbCaptcha.Size = new Size(this.Width - 20, this.Height - 85);
                            }

                        }
                        else
                        {
                            try
                            {
                                if (this.pbCaptcha.InvokeRequired)
                                {
                                    this.pbCaptcha.Invoke(new MethodInvoker(delegate
                                    {
                                        this.pbCaptcha.Size = new Size(this.Width - 20, this.Height - 175);
                                    }));
                                }
                                else
                                {
                                    this.pbCaptcha.Size = new Size(this.Width - 20, this.Height - 175);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void frmCaptcha_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }


        public void showForm()
        {
            this.Show();
        }

        public void hideForm()
        {
            this.Hide();
        }
    }
}
