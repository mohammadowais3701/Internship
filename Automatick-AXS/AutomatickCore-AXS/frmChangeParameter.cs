using Automatick.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Automatick
{
    public partial class frmChangeParameter : C1.Win.C1Ribbon.C1RibbonForm, ISelectDateForm
    {
        public bool ifTicketTypeBind = false;
        private System.Threading.Timer _timer;

        public BindingList<ITicketSearch> ParameterQueue
        {
            get;
            set;
        }

        public BindingSource tickettypes;

        Queue<Parameter> unmatchedParameters = null;

        public frmChangeParameter()
        {
            ParameterQueue = new BindingList<ITicketSearch>();
            unmatchedParameters = new Queue<Parameter>();
            ParameterQueue.AllowNew = true;            
            InitializeComponent();
           // if (!SingleInstance.isTicketAddWindowOpen) this.TopMost = true;
            tickettypes = new BindingSource();
            this.tMParameterBindingSource.DataSource = this.ParameterQueue;
           // _timer = new System.Threading.Timer(new TimerCallback(this.MakeTopMost), null, 0, 20 * 1000);

        }

        private void selectParameter(ITicketSearch tmpSearch)
        {
            try
            {
                if (tmpSearch != null)
                {
                    if (tmpSearch.Parameter.IfpriceLevelsBind)
                    {
                        Dictionary<String, Boolean> tt = new Dictionary<string, bool>();

                        foreach (AXSPriceLevel item in tmpSearch.Parameter.PriceLevels)
                        {
                            if (!tt.ContainsKey(item.PriceSecName))
                            {
                                tt.Add(item.PriceSecName, true);
                            }
                        }

                        if (!tt.ContainsKey("Empty PriceLevel"))
                        {
                            tt.Add("Empty PriceLevel", true);
                        }

                        tickettypes.DataSource=tt;
                        cbTicketType.DataSource = tickettypes;
                        cbTicketType.DisplayMember = "Key";
                        cbTicketType.ValueMember = "Value";                      

                    }                    

                    if ((this.unmatchedParameters != null) && (!this.unmatchedParameters.Contains(tmpSearch.Parameter)))
                    {
                        if (!tmpSearch.Parameter.IfShown)
                        {
                            this.unmatchedParameters.Enqueue(tmpSearch.Parameter);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private void tMParameterBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            try
            {
                if (tMParameterBindingSource.Count > 0)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            //this.Visible = true;
                            if (e.ListChangedType == ListChangedType.ItemAdded)
                            {
                                ITicketSearch tmpSearch = (ITicketSearch)tMParameterBindingSource.List[e.NewIndex];
                                selectParameter(tmpSearch);
                            }
                            if (!this.Focused && !SingleInstance.isTicketAddWindowOpen)
                            {
                                VisibleWin();
                                this.Activate();
                            }
                            if (!this.cbTicketType.Focused && !SingleInstance.isTicketAddWindowOpen)
                            {
                                VisibleWin();
                                this.cbTicketType.Focus();
                            }
                            this.lblCount.Text = "Remaining parameters : " + tMParameterBindingSource.Count;
                        }));
                    }
                    else
                    {
                        //this.Visible = true;
                        if (e.ListChangedType == ListChangedType.ItemAdded)
                        {
                            ITicketSearch tmpSearch = (ITicketSearch)tMParameterBindingSource.List[e.NewIndex];
                            selectParameter(tmpSearch);
                        }
                        if (!this.Focused && !SingleInstance.isTicketAddWindowOpen)
                        {
                            VisibleWin();
                            this.Activate();
                        }
                        if (!this.cbTicketType.Focused && !SingleInstance.isTicketAddWindowOpen)
                        {
                            VisibleWin();
                            this.cbTicketType.Focus();
                        }
                        this.lblCount.Text = "Remaining parameters : " + tMParameterBindingSource.Count;
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
            catch
            { }
        }

        public void showForm()
        {
            this.Show();
        }

        public void hideForm()
        {
            this.Hide();
        }        

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tMParameterBindingSource.Current != null)
                {   
                    ITicketSearch tmpSearch = ((ITicketSearch)this.tMParameterBindingSource.Current);
                    lock (this.ParameterQueue)
                    {
                        this.ParameterQueue.Remove(tmpSearch);
                    }
                    KeyValuePair<String, Boolean> selectedItem = (KeyValuePair<String, Boolean>)cbTicketType.SelectedItem;
                    if (selectedItem.Key.Contains("Empty Ticket Type") && !String.IsNullOrEmpty(txtManualTicketType.Text))
                    {
                        tmpSearch._CurrentParameter.PriceLevelString = txtManualTicketType.Text;
                        tmpSearch._CurrentParameter.TicketTypePasssword = txtPassword.Text;
                    }
                    else
                    {
                        tmpSearch._CurrentParameter.PriceLevelString = !selectedItem.Key.Contains("Empty Ticket Type") ? selectedItem.Key : String.Empty;
                        tmpSearch._CurrentParameter.TicketTypePasssword = txtPassword.Text;
                    }
                    txtPassword.Text = "";
                    txtManualTicketType.Text = String.Empty;
                    tmpSearch.Parameter.parameterentered.Set();

                    if (this.unmatchedParameters.Count > 0)
                    {
                        Parameter pm = this.unmatchedParameters.Dequeue();

                        if (pm != null)
                        {
                            if (!pm.IfShown)
                            {
                                this.lbloldParameter.Text = pm.InvalidParameter;
                                this.lblOldPassword.Text = pm.InvalidPassowrd;
                            }
                        }
                    }
                }

            }
            catch
            { 
            }
        }

        private void tMParameterBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        if (this.tMParameterBindingSource.Current != null)
                        {
                            ((ITicketSearch)this.tMParameterBindingSource.Current).Parameter.IfShown = true;
                            lblTicketName.Text = "Ticket Name:  " + ((ITicketSearch)this.tMParameterBindingSource.Current).TicketName;
                            this.lbloldParameter.Text = ((ITicketSearch)this.tMParameterBindingSource.Current).Parameter.InvalidParameter;
                            this.lblOldPassword.Text = ((ITicketSearch)this.tMParameterBindingSource.Current).Parameter.InvalidPassowrd;
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void frmChangeParameter_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            try
            {
                if (this._timer != null)
                {
                    this._timer.Change(Timeout.Infinite, Timeout.Infinite);
                    this._timer.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void cbTicketType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbTicketType.Text.Equals("Empty Ticket Type"))
                {
                    this.txtPassword.Enabled = true;
                    this.txtManualTicketType.Enabled = true;
                }
                else
                {
                    this.txtManualTicketType.Enabled = false;
                    this.txtManualTicketType.Clear();
                    KeyValuePair<String, Boolean> selectedItem = (KeyValuePair<String, Boolean>)cbTicketType.SelectedItem;
                    txtPassword.Enabled = selectedItem.Value;
                }
            }
            catch
            { }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tMParameterBindingSource.Current != null)
                {
                    ITicketSearch tmpSearch = ((ITicketSearch)this.tMParameterBindingSource.Current);
                    lock (this.ParameterQueue)
                    {
                        this.ParameterQueue.Remove(tmpSearch);
                    }
                    //KeyValuePair<String, Boolean> selectedItem = (KeyValuePair<String, Boolean>)cbTicketType.SelectedItem;
                    //tmpSearch._CurrentParameter.TicketTypeString = selectedItem.Key;
                    //tmpSearch._CurrentParameter.TicketTypePasssword = txtPassword.Text;
                    //txtPassword.Text = "";
                    tmpSearch.Parameter.parameterentered.Set();

                    if (this.unmatchedParameters.Count > 0)
                    {
                        Parameter pm = this.unmatchedParameters.Dequeue();

                        if (pm != null)
                        {
                            if (!pm.IfShown)
                            {
                                this.lbloldParameter.Text = pm.InvalidParameter;
                                this.lblOldPassword.Text = pm.InvalidPassowrd;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void frmChangeParameter_Load(object sender, EventArgs e)
        {
           // this.Location = new Point(SystemInformation.VirtualScreen.Width - this.Width - 50, SystemInformation.VirtualScreen.Height - this.Height - 50);
        }
        private void MakeTopMost(object obj)
        {
            try
            {
                if (((this.ParameterQueue != null) && (this.ParameterQueue.Count > 0)) && (!SingleInstance.isTicketAddWindowOpen))
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            if (this.MinimizeBox)
                            {
                                if (!SingleInstance.isTicketAddWindowOpen)
                                {
                                    this.WindowState = FormWindowState.Normal;
                                    this.Activate();
                                }
                            }

                            VisibleWin();
                            
                        }));
                    }
                    else
                    {
                        if (this.MinimizeBox)
                        {
                            if (!SingleInstance.isTicketAddWindowOpen)
                            {
                                this.WindowState = FormWindowState.Normal;        
                                this.Activate();
                            }
                        }

                        VisibleWin();                    
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        void VisibleWin()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    if (!this.Visible)
                        this.Visible = true;
                    this.TopMost = true;

                }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}
