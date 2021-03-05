namespace Automatick
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Net;
    using System.IO;
    using System.Threading;
    using SortedBindingList;
    using System.Text.RegularExpressions;
    using System.Linq;
    using System.Collections.Generic;
    using Automatick.Core;
    public class frmProxies : C1.Win.C1Ribbon.C1RibbonForm
    {
        private IContainer components;
        private IMainForm _mainform;
        private DataGridView ProxiesGrid;
        private C1.Win.C1Input.C1Button button_proxyFromFile;
        private C1.Win.C1Input.C1Button btnVerifyProxies;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private DataGridViewTextBoxColumn addressDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn portDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn UserName;
        private DataGridViewTextBoxColumn Password;
        private DataGridViewTextBoxColumn ProxyVerification;
        private CheckBox chkStatus;
        public BindingSource ProxiesSource;

        delegate void ResetBinding();
        void resetBinding()
        {
            try
            {
                ProxiesSource.ResetBindings(false);

                foreach (Proxy item in this._mainform.AppStartUp.Proxies)
                {
                    if (item.ProxyStatus == Proxy.proxyVerifying)
                    {
                        button_proxyFromFile.Enabled = false;
                        ProxiesGrid.ReadOnly = true;
                        ProxiesGrid.AllowUserToAddRows = false;
                        ProxiesGrid.AllowUserToDeleteRows = false;
                        btnVerifyProxies.Enabled = false;
                        break;
                    }
                    else
                    {
                        button_proxyFromFile.Enabled = true;
                        ProxiesGrid.ReadOnly = false;
                        ProxiesGrid.AllowUserToAddRows = true;
                        ProxiesGrid.AllowUserToDeleteRows = true;
                        btnVerifyProxies.Enabled = true;
                    }
                }

            }
            catch (Exception)
            {

            }

        }
        public frmProxies(IMainForm mainForm)
        {
            this._mainform = mainForm;
            this.InitializeComponent();
            this.chkStatus.Checked = this._mainform.AppStartUp.GlobalSetting.IfCheckProxiesStatus;
            this._mainform.AppStartUp.Proxies.Sort(this._mainform.AppStartUp.Proxies.OrderBy(p => p.TheProxyType));
            this.ProxiesSource.DataSource = this._mainform.AppStartUp.Proxies;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmProxies_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this._mainform.AppStartUp.Proxies.Count == 1)
            {
                if (string.IsNullOrEmpty(this._mainform.AppStartUp.Proxies[0].Address) || string.IsNullOrEmpty(this._mainform.AppStartUp.Proxies[0].Port))
                {
                    this._mainform.AppStartUp.Proxies.Clear();
                }
            }
            this._mainform.AppStartUp.SaveProxies();
            this._mainform.AppStartUp.GlobalSetting.IfCheckProxiesStatus = this.chkStatus.Checked;
            this._mainform.AppStartUp.SaveGlobalSettings();

            GC.SuppressFinalize(this);
            //GC.Collect();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProxies));
            this.ProxiesGrid = new System.Windows.Forms.DataGridView();
            this.addressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.portDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProxyVerification = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProxiesSource = new System.Windows.Forms.BindingSource(this.components);
            this.button_proxyFromFile = new C1.Win.C1Input.C1Button();
            this.btnVerifyProxies = new C1.Win.C1Input.C1Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ProxiesGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProxiesSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ProxiesGrid
            // 
            this.ProxiesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProxiesGrid.AutoGenerateColumns = false;
            this.ProxiesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ProxiesGrid.BackgroundColor = System.Drawing.Color.White;
            this.ProxiesGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ProxiesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProxiesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.addressDataGridViewTextBoxColumn,
            this.portDataGridViewTextBoxColumn,
            this.UserName,
            this.Password,
            this.ProxyVerification});
            this.ProxiesGrid.DataSource = this.ProxiesSource;
            this.ProxiesGrid.GridColor = System.Drawing.Color.LightGray;
            this.ProxiesGrid.Location = new System.Drawing.Point(12, 59);
            this.ProxiesGrid.Name = "ProxiesGrid";
            this.ProxiesGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.ProxiesGrid.RowHeadersWidth = 25;
            this.ProxiesGrid.Size = new System.Drawing.Size(610, 363);
            this.ProxiesGrid.TabIndex = 1;
            this.ProxiesGrid.VirtualMode = true;
            this.ProxiesGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ProxiesGrid_CellFormatting);
            this.ProxiesGrid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.ProxiesGrid_DataBindingComplete);
            this.ProxiesGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.ProxiesGrid_DataError);
            this.ProxiesGrid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.ProxiesGrid_UserDeletingRow);
            // 
            // addressDataGridViewTextBoxColumn
            // 
            this.addressDataGridViewTextBoxColumn.DataPropertyName = "Address";
            this.addressDataGridViewTextBoxColumn.HeaderText = "Address";
            this.addressDataGridViewTextBoxColumn.Name = "addressDataGridViewTextBoxColumn";
            // 
            // portDataGridViewTextBoxColumn
            // 
            this.portDataGridViewTextBoxColumn.DataPropertyName = "Port";
            this.portDataGridViewTextBoxColumn.HeaderText = "Port";
            this.portDataGridViewTextBoxColumn.Name = "portDataGridViewTextBoxColumn";
            // 
            // UserName
            // 
            this.UserName.DataPropertyName = "UserName";
            this.UserName.HeaderText = "User ID";
            this.UserName.Name = "UserName";
            // 
            // Password
            // 
            this.Password.DataPropertyName = "Password";
            this.Password.HeaderText = "Password";
            this.Password.Name = "Password";
            // 
            // ProxyVerification
            // 
            this.ProxyVerification.DataPropertyName = "ProxyStatus";
            this.ProxyVerification.HeaderText = "Proxy Status";
            this.ProxyVerification.Name = "ProxyVerification";
            this.ProxyVerification.ReadOnly = true;
            // 
            // ProxiesSource
            // 
            this.ProxiesSource.DataSource = typeof(Automatick.Core.Proxy);
            // 
            // button_proxyFromFile
            // 
            this.button_proxyFromFile.Location = new System.Drawing.Point(12, 9);
            this.button_proxyFromFile.Name = "button_proxyFromFile";
            this.button_proxyFromFile.Size = new System.Drawing.Size(296, 23);
            this.button_proxyFromFile.TabIndex = 3;
            this.button_proxyFromFile.Text = "Load Proxies From File";
            this.button_proxyFromFile.UseVisualStyleBackColor = true;
            this.button_proxyFromFile.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.button_proxyFromFile.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.button_proxyFromFile.Click += new System.EventHandler(this.button_proxyFromFile_Click);
            // 
            // btnVerifyProxies
            // 
            this.btnVerifyProxies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVerifyProxies.Location = new System.Drawing.Point(326, 9);
            this.btnVerifyProxies.Name = "btnVerifyProxies";
            this.btnVerifyProxies.Size = new System.Drawing.Size(296, 23);
            this.btnVerifyProxies.TabIndex = 4;
            this.btnVerifyProxies.Text = "Verify Proxies";
            this.btnVerifyProxies.UseVisualStyleBackColor = true;
            this.btnVerifyProxies.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnVerifyProxies.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnVerifyProxies.Click += new System.EventHandler(this.btnVerifyProxies_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 492);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Verified : Your proxy is worknig fine";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 444);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(235, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Blocked : Your proxy is blocked and not working";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 460);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(307, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Not Working : Your proxy is either blocked or non working proxy";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 476);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(245, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Time Out: Proxies timed out due to being very slow";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 428);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(311, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Not Verified : Please click verify button to check the proxy status";
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new System.Drawing.Point(15, 38);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(120, 17);
            this.chkStatus.TabIndex = 11;
            this.chkStatus.Text = "Update proxy status";
            this.chkStatus.UseVisualStyleBackColor = true;
            // 
            // frmProxies
            // 
            this.ClientSize = new System.Drawing.Size(634, 512);
            this.Controls.Add(this.chkStatus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnVerifyProxies);
            this.Controls.Add(this.button_proxyFromFile);
            this.Controls.Add(this.ProxiesGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmProxies";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proxies";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Silver;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmProxies_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmProxies_FormClosed);
            this.Load += new System.EventHandler(this.frmProxies_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProxiesGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProxiesSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button_proxyFromFile_Click(object sender, EventArgs e)
        {
            LoadProxiesFromFile();
        }
        public void LoadProxiesFromFile()
        {
            OpenFileDialog dialogOpen = new OpenFileDialog();
            dialogOpen.Filter = "Text files (*.txt)|*.txt";
            dialogOpen.Title = "Open an TXT File";
            if (dialogOpen.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr;
                SortableBindingList<Proxy> lstProxy = new SortableBindingList<Proxy>();
                try
                {
                    sr = new System.IO.StreamReader(dialogOpen.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("The process cannot access the file '" + dialogOpen.FileName + "' because it is being used by another process.");
                    return;
                }

                string strLine;

                do
                {
                    strLine = sr.ReadLine();
                    if (String.IsNullOrEmpty(strLine))
                    {
                        break;
                    }
                    else
                    {
                        String[] strItem = strLine.Split(':');
                        if (strItem.Length >= 2)
                        {
                            Proxy p = new Proxy();
                            p.Address = strItem[0].Trim();
                            p.Port = strItem[1].Trim();
                            if (strItem.Length > 2)
                            {
                                p.UserName = strItem[2].Trim();
                                p.Password = strItem[3].Trim();
                            }
                            lstProxy.Add(p);
                        }
                    }

                } while (true);
                if (lstProxy.Count > 0)
                {
                    IEnumerable<Proxy> proxiesToRemove = this._mainform.AppStartUp.Proxies.Where(p => p.TheProxyType == Proxy.ProxyType.Custom);
                    try
                    {
                        Proxy[] pArr = proxiesToRemove.ToArray();
                        for (int i = pArr.Length - 1; i >= 0; i--)
                        {
                            this._mainform.AppStartUp.Proxies.Remove(pArr[i]);
                        }
                    }
                    catch { }
                    foreach (Proxy item in lstProxy)
                    {
                        this._mainform.AppStartUp.Proxies.Add(item);
                    }
                }
                //this.mainform.AppStartUp.Proxies = lstProxy;
                this.ProxiesSource.DataSource = this._mainform.AppStartUp.Proxies;
            }
        }

        private void btnVerifyProxies_Click(object sender, EventArgs e)
        {
           
                button_proxyFromFile.Enabled = false;
                ProxiesGrid.ReadOnly = true;
                ProxiesGrid.AllowUserToAddRows = false;
                ProxiesGrid.AllowUserToDeleteRows = false;

                btnVerifyProxies.Enabled = false;
                Thread th = new Thread(VerifyAll);
                th.IsBackground = true;
                th.Priority = ThreadPriority.Highest;
                th.Start();
                ProxiesSource.ResetBindings(false);
            
          
        }

        private void VerifyAll()
        {
            foreach (Proxy item in this._mainform.AppStartUp.Proxies)
            {
                item.ProxyStatus = Proxy.proxyWaiting;
            }
            foreach (Proxy item in this._mainform.AppStartUp.Proxies)
            {
                item.ProxyStatus = Proxy.proxyVerifying;
                verifyProxies(item);
            }
            //
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    button_proxyFromFile.Enabled = true;
                    ProxiesGrid.ReadOnly = false;
                    ProxiesGrid.AllowUserToAddRows = true;
                    ProxiesGrid.AllowUserToDeleteRows = true;
                    btnVerifyProxies.Enabled = true;
                }));
            }
            catch { }
        }

        void verifyProxies(object obj)
        {
            // IOrderedEnumerable<AXSTicket> lastedUsedTicketsDesc = this._mainform.AppStartUp.Tickets.OrderByDescending(p => p.LastUsedDateTime);
            Proxy proxy = obj as Proxy;
            int i = ProxiesSource.IndexOf(proxy);
            if (i >= 0)
            {
                try
                {
                    ProxiesSource.ResetItem(i);
                }
                catch
                { }
            }

            try
            {
                WebRequest request = WebRequest.Create("http://www.axs.com/");
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                if (proxy.TheProxyType != Proxy.ProxyType.ISPIP)
                {
                    request.Proxy = new WebProxy("http://" + proxy.Address + ":" + proxy.Port.ToString());
                    if (!String.IsNullOrEmpty(proxy.UserName) && !String.IsNullOrEmpty(proxy.Password))
                    {
                        request.Proxy.Credentials = new NetworkCredential(proxy.UserName, proxy.Password);
                    }
                }
                request.Timeout = 10000;// 60000 * 2;
                proxy.ProxyResponseTime = 0;
                TimeSpan ts = DateTime.Now.TimeOfDay;
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            string responseFromServer = "";

                            char[] buff = new char[5000];
                            reader.ReadBlock(buff, 0, 5000);
                            responseFromServer = new string(buff);

                            responseFromServer = responseFromServer.ToLower();
                            if (responseFromServer.Contains("403 Forbidden".ToLower()) || responseFromServer.Contains("Forbidden".ToLower()) || responseFromServer.Contains("You don't have permission to access".ToLower()))
                            {
                                proxy.ProxyStatus = Proxy.proxyBlocked;
                            }
                            else if (responseFromServer.Contains("<script type=\"text/javascript\">".ToLower()))
                            {
                                proxy.ProxyStatus = Proxy.proxyVerified;

                                ts = DateTime.Now.TimeOfDay.Subtract(ts);

                                if (ts.Seconds > 0)
                                {
                                    proxy.ProxyResponseTime = ts.Seconds;
                                }
                            }
                            else
                            {
                                proxy.ProxyStatus = Proxy.proxyNotWork;
                            }

                            i = ProxiesSource.IndexOf(proxy);
                            if (i >= 0)
                            {
                                ProxiesSource.ResetItem(i);
                            }

                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("unable to connect to the remote server"))
                {
                    proxy.ProxyStatus = Proxy.proxyNotWork;
                }
                else if (ex.Message.ToLower().Contains("the operation has timed out"))
                {
                    proxy.ProxyStatus = Proxy.proxyTimeOut;
                }
                else
                {
                    proxy.ProxyStatus = Proxy.proxyNotWork;
                }

            }

            i = ProxiesSource.IndexOf(proxy);
            if (i >= 0)
            {
                ProxiesSource.ResetItem(i);
            }
        }

        private void frmProxies_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Proxy item in this._mainform.AppStartUp.Proxies)
            {
                if (item.ProxyStatus == Proxy.proxyVerifying)
                {
                    e.Cancel = true;
                    break;
                }
            }
            if (e.Cancel)
            {
                MessageBox.Show("You are not allowed to closed the form during verfication process.", "Please wait!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void frmProxies_Load(object sender, EventArgs e)
        {
            if (ProxyPicker.ProxyPickerInstance.LicPPM.IsValidated)
            {
                if (this._mainform.AppStartUp.GlobalSetting.IfEnablePM)
                {
                    btnVerifyProxies.Visible = false;
                }
                else
                {
                    btnVerifyProxies.Visible = true;
                }
            }
            else
            {
                btnVerifyProxies.Visible = true;
            }

        }

        private void ProxiesGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (ProxiesGrid.Columns[e.ColumnIndex].DataPropertyName == "Address")
                {
                    if (ProxiesGrid.Rows[e.RowIndex].DataBoundItem != null)
                    {
                        if (ProxiesGrid.Rows[e.RowIndex].DataBoundItem.GetType() == typeof(Proxy))
                        {
                            Proxy p = (Proxy)ProxiesGrid.Rows[e.RowIndex].DataBoundItem;
                            if (p.TheProxyType == Proxy.ProxyType.MyIP || p.TheProxyType == Proxy.ProxyType.Luminati || p.TheProxyType == Proxy.ProxyType.Relay)
                            {
                                ProxiesGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.SteelBlue;
                                ProxiesGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                                //gvSellers.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif,(float)8.25, FontStyle.Bold);
                                ProxiesGrid.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                                ProxiesGrid.Rows[e.RowIndex].ReadOnly = true;

                                // Regex r = new Regex(@"(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.(\d)(\d)?(\d)?");
                                // Regex r2 = new Regex(@"(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.(\d)(\d)?(\d)?.");
                                //Match m2 = r2.Match(p.Address);
                                //  if (m2.Success)
                                //  {
                                //     e.Value = r.Replace(p.Address, m2.Value + "***");
                                //  }
                                e.Value = "***.***.***.***";

                            }
                            //else
                            //{
                            //    ProxiesGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                            //    ProxiesGrid.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                            //    ProxiesGrid.Rows[e.RowIndex].ReadOnly = false;
                            //}
                        }
                    }
                }
                else if (ProxiesGrid.Columns[e.ColumnIndex].DataPropertyName == "Port")
                {
                    if (ProxiesGrid.Rows[e.RowIndex].DataBoundItem != null)
                    {
                        if (ProxiesGrid.Rows[e.RowIndex].DataBoundItem.GetType() == typeof(Proxy))
                        {
                            Proxy p = (Proxy)ProxiesGrid.Rows[e.RowIndex].DataBoundItem;
                            if (p.TheProxyType == Proxy.ProxyType.MyIP || p.TheProxyType == Proxy.ProxyType.Luminati || p.TheProxyType == Proxy.ProxyType.Relay)
                            {

                                e.Value = "****";
                            }
                        }
                    }
                }
                else if ((ProxiesGrid.Columns[e.ColumnIndex].DataPropertyName == "UserName") || (ProxiesGrid.Columns[e.ColumnIndex].DataPropertyName == "Password"))
                {
                    if (ProxiesGrid.Rows[e.RowIndex].DataBoundItem != null)
                    {
                        if (ProxiesGrid.Rows[e.RowIndex].DataBoundItem.GetType() == typeof(Proxy))
                        {
                            Proxy p = (Proxy)ProxiesGrid.Rows[e.RowIndex].DataBoundItem;
                            if (p.TheProxyType == Proxy.ProxyType.MyIP || p.TheProxyType == Proxy.ProxyType.Luminati || p.TheProxyType == Proxy.ProxyType.Relay)
                            {

                                e.Value = "**********";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void ProxiesGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row.DataBoundItem.GetType() == typeof(Proxy))
                {
                    Proxy p = (Proxy)e.Row.DataBoundItem;
                    if (p.TheProxyType == Proxy.ProxyType.MyIP || p.TheProxyType == Proxy.ProxyType.Relay || p.TheProxyType == Proxy.ProxyType.Luminati)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void ProxiesGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }
        }

        private void ProxiesGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }
    }
}

