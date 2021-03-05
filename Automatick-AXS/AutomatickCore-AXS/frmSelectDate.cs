using Automatick.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Automatick
{
    public partial class frmSelectDate : C1.Win.C1Ribbon.C1RibbonForm, ISelectDateForm
    {
        public bool ifTicketTypeBind = false;

        public BindingList<ITicketSearch> ParameterQueue
        {
            get;
            set;
        }

        public BindingSource eventDates;

        public frmSelectDate()
        {
            InitializeComponent();
            ParameterQueue = new BindingList<ITicketSearch>();
            ParameterQueue.AllowNew = true;
            eventDates = new BindingSource();
            this.axsEventDatesBindingSource.DataSource = this.ParameterQueue;
        
        }


        public void showForm()
        {
            this.Show();
        }

        public void hideForm()
        {
            this.Hide();
        }

        private void frmSelectDate_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.axsEventDatesBindingSource.Current != null)
            {
                ITicketSearch tmpSearch = ((ITicketSearch)this.axsEventDatesBindingSource.Current);
                lock (this.ParameterQueue)
                {
                    this.ParameterQueue.Remove(tmpSearch);
                }

                tmpSearch.Parameter.parameterentered.Set();

            }

        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.axsEventDatesBindingSource.Current != null)
                {
                    ITicketSearch tmpSearch = ((ITicketSearch)this.axsEventDatesBindingSource.Current);
                    tmpSearch.Ticket.DoneSelection = true;
                    lock (this.ParameterQueue)
                    {
                        this.ParameterQueue.Remove(tmpSearch);
                    }
                    KeyValuePair<String, String> selectedItem = (KeyValuePair<String, String>)this.cmbEventDates.SelectedItem;
                    CultureInfo provider = new CultureInfo("en-US");
                    DateTime dt = Convert.ToDateTime(selectedItem.Value.ToString(), provider);
                    string Eventdates = dt.ToString("MM/dd/yyyy");
                    DateTime time = Convert.ToDateTime(selectedItem.Value.Trim());
                    tmpSearch._CurrentParameter.DateTimeString = Eventdates;
                    tmpSearch._CurrentParameter.EventTime = time.ToShortTimeString();
                    tmpSearch.Ticket.SelectedDate = Eventdates;
                    tmpSearch.Ticket.SelectedEventTime = time.ToShortTimeString();
                    tmpSearch.Parameter.parameterentered.Set();

          
                }

            }
            catch
            {
            }
        }

        private void selectParameter(ITicketSearch tmpSearch)
        {
            try
            {
                if (tmpSearch != null)
                {
                    if (tmpSearch.Parameter.IfEventDatesBind)
                    {
                        Dictionary<String, String> tt = new Dictionary<string, string>();

                        foreach (AXSSection item in tmpSearch.Parameter.EventDates)
                        {
                            if (!tt.ContainsKey(item.EventDates))
                            {
                                String formatted;
                                CultureInfo provider = new CultureInfo("en-US");
                                DateTime dt = Convert.ToDateTime(item.EventDates.ToString(), provider);
                                string Eventdates = dt.ToString("MM/dd/yyyy");
                                DateTime time = Convert.ToDateTime(item.EventDates.Trim());
                                formatted = Eventdates + " " + time.ToShortTimeString();
                                tt.Add(formatted, item.EventDates);
                            }
                        }


                        eventDates.DataSource = tt;
                        this.cmbEventDates.DataSource = eventDates;
                        this.cmbEventDates.DisplayMember = "Key";
                        this.cmbEventDates.ValueMember = "Value";

                    }

              
                }
            }
            catch (Exception e)
            {

            }
        }

        private void axsEventDatesBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (axsEventDatesBindingSource.Count > 0)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        this.Visible = true;
                        if (e.ListChangedType == ListChangedType.ItemAdded)
                        {
                            ITicketSearch tmpSearch = (ITicketSearch)axsEventDatesBindingSource.List[e.NewIndex];
                            selectParameter(tmpSearch);
                        }
                        if (!this.Focused)
                        {
                            this.Activate();
                        }
                        if (!this.cmbEventDates.Focused)
                        {
                            this.cmbEventDates.Focus();
                        }
                        
                    }));
                }
                else
                {
                    this.Visible = true;
                    if (e.ListChangedType == ListChangedType.ItemAdded)
                    {
                        ITicketSearch tmpSearch = (ITicketSearch)axsEventDatesBindingSource.List[e.NewIndex];
                        selectParameter(tmpSearch);
                    }
                    if (!this.Focused)
                    {
                        this.Activate();
                    }
                    if (!this.cmbEventDates.Focused)
                    {
                        this.cmbEventDates.Focus();
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

        private void axsEventDatesBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    if (this.axsEventDatesBindingSource.Current != null)
                    {
                        ((ITicketSearch)this.axsEventDatesBindingSource.Current).Parameter.IfShown = true;
                        lblTicketName.Text = "Ticket Name:  " + ((ITicketSearch)this.axsEventDatesBindingSource.Current).TicketName;
                    
                    }
                }));
            }
        }        

   
    }
}
