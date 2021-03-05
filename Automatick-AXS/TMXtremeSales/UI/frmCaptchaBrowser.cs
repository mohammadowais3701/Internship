using Automatick.Core;
using Awesomium.Core;
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
    public partial class frmCaptchaBrowser : Form, IResourceInterceptor, ICaptchaForm
    {
        int i = 0;
        string cookies = string.Empty;
        AXSSearch tmpSearch = null;
        String default_us_Key = "6LexTBoTAAAAAESv_PtNKgDQM7ZP9KOKedZUbYay";

        public BindingList<ITicketSearch> CaptchaQueue
        {
            get;
            set;
        }

        public frmCaptchaBrowser()
        {
            //InitializeComponent();
            CaptchaQueue = new BindingList<ITicketSearch>();
            CaptchaQueue.AllowNew = true;
            CaptchaQueue.ListChanged += new ListChangedEventHandler(CaptchaQueue_ListChanged);
            //this.tMSearchBindingSource.DataSource = this.CaptchaQueue;
        }

        private void frmLoginBrowserAwesomium_Load(object sender, EventArgs e)
        {
            this.initializeResource();
            this.wbaCaptcha.Source = new Uri("http://localhost:8086/?" + default_us_Key);
            this.wbaCaptcha.Refresh();
        }

        public void initializeResource()
        {
            // We demonstrate the use of a resource interceptor.
            if (WebCore.ResourceInterceptor == null)
                WebCore.ResourceInterceptor = this;
        }

        public bool OnFilterNavigation(NavigationRequest request)
        {
            return false;
        }

        public ResourceResponse OnRequest(ResourceRequest request)
        {
            return null;
        }

        private void tMSearchBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (this.tMSearchBindingSource.Current != null)
            {
                AXSSearch _tmpSearch = (AXSSearch)this.tMSearchBindingSource.Current;

                try
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            this.wbaCaptcha.Source = new Uri("http://localhost:8086/?" + _tmpSearch.RecaptchaV2Key);
                            this.wbaCaptcha.Refresh();
                        }));
                    }
                    else
                    {
                        this.wbaCaptcha.Source = new Uri("http://localhost:8086/?" + _tmpSearch.RecaptchaV2Key);
                        this.wbaCaptcha.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void CaptchaQueue_ListChanged(object sender, ListChangedEventArgs e)
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
                            if (!this.wbaCaptcha.Focused)
                            {
                                this.wbaCaptcha.Focus();
                            }
                        }));
                    }
                    else
                    {
                        this.Visible = true;

                        if (!this.Focused)
                        {
                            this.Activate();
                        }
                        if (!this.wbaCaptcha.Focused)
                        {
                            this.wbaCaptcha.Focus();
                        }
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

        private void Awesomium_Windows_Forms_WebControl_LoadingFrameComplete(object sender, FrameEventArgs e)
        {
            if (wbaCaptcha.IsDocumentReady)
            {
                dynamic document = (JSObject)wbaCaptcha.ExecuteJavascriptWithResult("document");
                String HTML = wbaCaptcha.ExecuteJavascriptWithResult("document.documentElement.innerHTML");
                if (wbaCaptcha.IsDocumentReady)
                {
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(HTML);

                    if (doc.DocumentNode.SelectSingleNode("//input[@id='g-captcha-response']") != null)
                    {
                        lock (this.CaptchaQueue)
                        {
                            AXSSearch _tmpSearch = _tmpSearch = ((AXSSearch)this.tMSearchBindingSource.Current); ;
                            try
                            {
                                if (doc != null && this.tMSearchBindingSource.Current != null)
                                {
                                    HtmlAgilityPack.HtmlNode nodeCaptchaResponse = doc.DocumentNode.SelectSingleNode("//input[@id='g-captcha-response']");
                                    if (nodeCaptchaResponse != null)
                                    {
                                        _tmpSearch.RecapToken = nodeCaptchaResponse.Attributes["value"].Value.Replace("\"", "").Replace("g-recaptcha-response=", "");

                                    }
                                    this.CaptchaQueue.Remove(tmpSearch);
                                    ((AXSTicket)_tmpSearch.Ticket).CaptchaBrowserQueue.Remove(_tmpSearch);

                                    _tmpSearch.captchaload.Set();
                                }
                            }
                            catch (Exception ex)
                            {
                                this.CaptchaQueue.Remove(tmpSearch);
                                ((AXSTicket)_tmpSearch.Ticket).CaptchaBrowserQueue.Remove(_tmpSearch);

                                _tmpSearch.captchaload.Set();
                            }
                        }
                    }
                }
            }

        }

        public void showForm()
        {
            this.Show();
        }

        public void hideForm()
        {
            this.Hide();
        }

        public void closeForm()
        {
        }
    }
}
