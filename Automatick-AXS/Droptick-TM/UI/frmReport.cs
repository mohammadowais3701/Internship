using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1Ribbon;
using System.IO;
using Automatick.Core;
using SortedBindingList;
using System.Runtime.Serialization.Formatters.Binary;

namespace Automatick
{
    public partial class frmReport : C1RibbonForm
    {
        List<TicketsLog> all_Logs = new List<TicketsLog>();
        public IMainForm MainForm
        {
            get;
            set;
        }

        public frmReport(IMainForm mainForm)
        {
            InitializeComponent();
            this.MainForm = mainForm;
            foreach (string str in Directory.GetFiles(this.MainForm.AppStartUp.DefaultFileLocation + @"\Tickets\", "*.tlog"))
            {
                string reportName = str;
                SortableBindingList<TicketsLog> ticketLogs=new SortableBindingList<TicketsLog>();
                ticketLogs.Load(reportName);
                foreach (TicketsLog item in ticketLogs)
                {
                    all_Logs.Add(item);
                }
               
            }
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            this.ticketLogsBindingSource.DataSource = all_Logs;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string str in Directory.GetFiles(this.MainForm.AppStartUp.DefaultFileLocation + @"\Tickets\", "*.tlog"))
                {
                    string reportName = str;
                    if (File.Exists(reportName))
                    {
                        try
                        {
                            File.Delete(reportName);
                        }
                        catch
                        {
                        }
                       
                    }
                }
                foreach (AXSTicket item in this.MainForm.AppStartUp.Tickets)
                {
                    if (item.tic_Logs != null)
                    {
                        if (item.tic_Logs.Count > 0)
                        {
                            item.tic_Logs.Clear();
                        }
                    }
                }
                this.ticketLogsBindingSource.Clear();
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();

            fd.Filter = "csv files (*.csv)|*.csv";
            if (fd.ShowDialog() == DialogResult.OK)
            {

                FileStream filewriter = new FileStream(fd.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter writer = new StreamWriter(filewriter);
                String contents = "TicketName,Section,Row,Seat,Price,Account,FoundDateTime,BuyStatus" + Environment.NewLine;
                try
                {
                    //writer.WriteLine(contents.TrimEnd(new char[] { '\r', '\n' }));
                    if (this.all_Logs != null)
                    {
                        if (this.all_Logs.Count != 0)
                        {
                            foreach (TicketsLog item in this.all_Logs)
                            {
                                contents += item.TicketName.Replace(",", "") + "," + item.Section.Replace(",", "") + "," + item.Row.Replace(",", "") + "," + item.Seat.Replace(",", "") + "," + item.Price.Replace(",", "") + "," + item.Account.Replace(",", "") + "," + item.FoundDateTime.ToString().Replace(",", "") + "," + item.BuyStatus.Replace(",", "");
                                contents += Environment.NewLine;
                            }
                            writer.Write(contents.TrimEnd(new char[] { '\r', '\n' }));
                            writer.Close();
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }


             
    }
}
