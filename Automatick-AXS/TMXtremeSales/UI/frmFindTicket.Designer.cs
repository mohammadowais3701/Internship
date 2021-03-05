namespace Automatick
{
    partial class frmFindTicket
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
            catch { }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFindTicket));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.rMain = new C1.Win.C1Ribbon.C1Ribbon();
            this.ribbonApplicationMenu1 = new C1.Win.C1Ribbon.RibbonApplicationMenu();
            this.ribbonConfigToolBar1 = new C1.Win.C1Ribbon.RibbonConfigToolBar();
            this.rbShowHideBar = new C1.Win.C1Ribbon.RibbonButton();
            this.ribbonQat1 = new C1.Win.C1Ribbon.RibbonQat();
            this.ribbonTab1 = new C1.Win.C1Ribbon.RibbonTab();
            this.ribbonGroup21 = new C1.Win.C1Ribbon.RibbonGroup();
            this.rbAutoBuy = new C1.Win.C1Ribbon.RibbonButton();
            this.rbAutoBuyGuest = new C1.Win.C1Ribbon.RibbonButton();
            this.rbAutoBuyWithoutProxy = new C1.Win.C1Ribbon.RibbonButton();
            this.ribbonGroup1 = new C1.Win.C1Ribbon.RibbonGroup();
            this.rbResetAll = new C1.Win.C1Ribbon.RibbonButton();
            this.rbResetSelectedSearch = new C1.Win.C1Ribbon.RibbonButton();
            this.rbClose = new C1.Win.C1Ribbon.RibbonButton();
            this.ribbonGroup2 = new C1.Win.C1Ribbon.RibbonGroup();
            this.chkSendEmail = new C1.Win.C1Ribbon.RibbonCheckBox();
            this.chkAutoCaptcha = new C1.Win.C1Ribbon.RibbonCheckBox();
            this.cbCaptchaService = new C1.Win.C1Ribbon.RibbonComboBox();
            this.rbBPCAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbRDAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbCPTAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbDBCAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbDCAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbOCR = new C1.Win.C1Ribbon.RibbonButton();
            this.rbCAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbRAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbBoloAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbSOCR = new C1.Win.C1Ribbon.RibbonButton();
            this.rb2CAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbAAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbCaptchatorAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbAC1AutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbRDCAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.nudStartSolvingCaptchaFrom = new C1.Win.C1Ribbon.RibbonNumericBox();
            this.ribbonSeparator1 = new C1.Win.C1Ribbon.RibbonSeparator();
            this.chkUseProxies = new C1.Win.C1Ribbon.RibbonCheckBox();
            this.nudStartUsingProxiesFrom = new C1.Win.C1Ribbon.RibbonNumericBox();
            this.chkUseProxiesInCaptchaSource = new C1.Win.C1Ribbon.RibbonCheckBox();
            this.ribbonSeparator11 = new C1.Win.C1Ribbon.RibbonSeparator();
            this.chkAutoBuy = new C1.Win.C1Ribbon.RibbonCheckBox();
            this.chkPlayMusic = new C1.Win.C1Ribbon.RibbonCheckBox();
            this.AXSByPassWaitingRoom = new C1.Win.C1Ribbon.RibbonCheckBox();
            this.nudNumberOfSearches = new C1.Win.C1Ribbon.RibbonNumericBox();
            this.rbSaveAndChange = new C1.Win.C1Ribbon.RibbonButton();
            this.chkPersistSession = new C1.Win.C1Ribbon.RibbonCheckBox();
            this.ribbonToolBar1 = new C1.Win.C1Ribbon.RibbonToolBar();
            this.ribbonLabel1 = new C1.Win.C1Ribbon.RibbonLabel();
            this.timerFocusScreen = new System.Windows.Forms.Timer(this.components);
            this.grvFindTickets = new System.Windows.Forms.DataGridView();
            this.Flag = new System.Windows.Forms.DataGridViewImageColumn();
            this.sectionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seatDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeLeftDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MoreInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AXSSearchBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.rMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvFindTickets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AXSSearchBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // rMain
            // 
            this.rMain.ApplicationMenuHolder = this.ribbonApplicationMenu1;
            this.rMain.ConfigToolBarHolder = this.ribbonConfigToolBar1;
            this.rMain.Location = new System.Drawing.Point(0, 0);
            this.rMain.Name = "rMain";
            this.rMain.QatHolder = this.ribbonQat1;
            this.rMain.Size = new System.Drawing.Size(892, 152);
            this.rMain.Tabs.Add(this.ribbonTab1);
            // 
            // ribbonApplicationMenu1
            // 
            this.ribbonApplicationMenu1.Name = "ribbonApplicationMenu1";
            this.ribbonApplicationMenu1.Visible = false;
            // 
            // ribbonConfigToolBar1
            // 
            this.ribbonConfigToolBar1.Items.Add(this.rbShowHideBar);
            this.ribbonConfigToolBar1.Name = "ribbonConfigToolBar1";
            // 
            // rbShowHideBar
            // 
            this.rbShowHideBar.Name = "rbShowHideBar";
            this.rbShowHideBar.SmallImage = global::Automatick.Properties.Resources._1354172637_navigate_up;
            this.rbShowHideBar.ToolTip = "Minimize actions bar";
            this.rbShowHideBar.Click += new System.EventHandler(this.rbShowHideBar_Click);
            // 
            // ribbonQat1
            // 
            this.ribbonQat1.Name = "ribbonQat1";
            this.ribbonQat1.Visible = false;
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Groups.Add(this.ribbonGroup21);
            this.ribbonTab1.Groups.Add(this.ribbonGroup1);
            this.ribbonTab1.Groups.Add(this.ribbonGroup2);
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "Actions";
            // 
            // ribbonGroup21
            // 
            this.ribbonGroup21.Items.Add(this.rbAutoBuy);
            this.ribbonGroup21.Items.Add(this.rbAutoBuyGuest);
            this.ribbonGroup21.Items.Add(this.rbAutoBuyWithoutProxy);
            this.ribbonGroup21.Name = "ribbonGroup21";
            this.ribbonGroup21.Text = "Buying Options";
            // 
            // rbAutoBuy
            // 
            this.rbAutoBuy.Name = "rbAutoBuy";
            this.rbAutoBuy.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbAutoBuy.SmallImage")));
            this.rbAutoBuy.SupportedGroupSizing = C1.Win.C1Ribbon.SupportedGroupSizing.TextAlwaysVisible;
            this.rbAutoBuy.Text = "Auto buy";
            this.rbAutoBuy.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
            this.rbAutoBuy.Click += new System.EventHandler(this.rbAutoBuy_Click);
            // 
            // rbAutoBuyGuest
            // 
            this.rbAutoBuyGuest.Name = "rbAutoBuyGuest";
            this.rbAutoBuyGuest.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbAutoBuyGuest.SmallImage")));
            this.rbAutoBuyGuest.SupportedGroupSizing = C1.Win.C1Ribbon.SupportedGroupSizing.TextAlwaysVisible;
            this.rbAutoBuyGuest.Text = "Guest Autobuy";
            this.rbAutoBuyGuest.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
            this.rbAutoBuyGuest.Click += new System.EventHandler(this.rbAutoBuyGuest_Click);
            // 
            // rbAutoBuyWithoutProxy
            // 
            this.rbAutoBuyWithoutProxy.Name = "rbAutoBuyWithoutProxy";
            this.rbAutoBuyWithoutProxy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.rbAutoBuyWithoutProxy.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbAutoBuyWithoutProxy.SmallImage")));
            this.rbAutoBuyWithoutProxy.SupportedGroupSizing = C1.Win.C1Ribbon.SupportedGroupSizing.TextAlwaysVisible;
            this.rbAutoBuyWithoutProxy.Text = "Auto buy w/o proxy";
            this.rbAutoBuyWithoutProxy.Click += new System.EventHandler(this.rbAutoBuyWithoutProxy_Click);
            // 
            // ribbonGroup1
            // 
            this.ribbonGroup1.Items.Add(this.rbResetAll);
            this.ribbonGroup1.Items.Add(this.rbResetSelectedSearch);
            this.ribbonGroup1.Items.Add(this.rbClose);
            this.ribbonGroup1.Name = "ribbonGroup1";
            this.ribbonGroup1.Text = "Searches Options";
            // 
            // rbResetAll
            // 
            this.rbResetAll.Name = "rbResetAll";
            this.rbResetAll.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbResetAll.SmallImage")));
            this.rbResetAll.SupportedGroupSizing = C1.Win.C1Ribbon.SupportedGroupSizing.TextAlwaysVisible;
            this.rbResetAll.Text = "Restart all";
            this.rbResetAll.Click += new System.EventHandler(this.rbResetAll_Click);
            // 
            // rbResetSelectedSearch
            // 
            this.rbResetSelectedSearch.Name = "rbResetSelectedSearch";
            this.rbResetSelectedSearch.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbResetSelectedSearch.SmallImage")));
            this.rbResetSelectedSearch.SupportedGroupSizing = C1.Win.C1Ribbon.SupportedGroupSizing.TextAlwaysVisible;
            this.rbResetSelectedSearch.Text = "Restart";
            this.rbResetSelectedSearch.Click += new System.EventHandler(this.rbResetSelectedSearch_Click);
            // 
            // rbClose
            // 
            this.rbClose.Name = "rbClose";
            this.rbClose.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbClose.SmallImage")));
            this.rbClose.SupportedGroupSizing = C1.Win.C1Ribbon.SupportedGroupSizing.TextAlwaysVisible;
            this.rbClose.Text = "Stop";
            this.rbClose.Click += new System.EventHandler(this.rbClose_Click);
            // 
            // ribbonGroup2
            // 
            this.ribbonGroup2.Items.Add(this.chkSendEmail);
            this.ribbonGroup2.Items.Add(this.chkAutoCaptcha);
            this.ribbonGroup2.Items.Add(this.cbCaptchaService);
            this.ribbonGroup2.Items.Add(this.nudStartSolvingCaptchaFrom);
            this.ribbonGroup2.Items.Add(this.ribbonSeparator1);
            this.ribbonGroup2.Items.Add(this.chkUseProxies);
            this.ribbonGroup2.Items.Add(this.nudStartUsingProxiesFrom);
            this.ribbonGroup2.Items.Add(this.chkUseProxiesInCaptchaSource);
            this.ribbonGroup2.Items.Add(this.ribbonSeparator11);
            this.ribbonGroup2.Items.Add(this.chkAutoBuy);
            this.ribbonGroup2.Items.Add(this.chkPlayMusic);
            this.ribbonGroup2.Items.Add(this.AXSByPassWaitingRoom);
            this.ribbonGroup2.Items.Add(this.nudNumberOfSearches);
            this.ribbonGroup2.Items.Add(this.rbSaveAndChange);
            this.ribbonGroup2.Items.Add(this.chkPersistSession);
            this.ribbonGroup2.Items.Add(this.ribbonToolBar1);
            this.ribbonGroup2.Name = "ribbonGroup2";
            this.ribbonGroup2.Text = "Settings";
            // 
            // chkSendEmail
            // 
            this.chkSendEmail.Name = "chkSendEmail";
            this.chkSendEmail.Text = "Send email";
            // 
            // chkAutoCaptcha
            // 
            this.chkAutoCaptcha.Name = "chkAutoCaptcha";
            this.chkAutoCaptcha.Text = "Auto captcha";
            this.chkAutoCaptcha.CheckedChanged += new System.EventHandler(this.chkAutoCaptcha_CheckedChanged);
            // 
            // cbCaptchaService
            // 
            this.cbCaptchaService.DropDownStyle = C1.Win.C1Ribbon.RibbonComboBoxStyle.DropDownList;
            this.cbCaptchaService.Items.Add(this.rbBPCAutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbRDAutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbCPTAutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbDBCAutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbDCAutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbOCR);
            this.cbCaptchaService.Items.Add(this.rbCAutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbRAutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbBoloAutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbSOCR);
            this.cbCaptchaService.Items.Add(this.rb2CAutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbAAutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbCaptchatorAutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbAC1AutoCaptcha);
            this.cbCaptchaService.Items.Add(this.rbRDCAutoCaptcha);
            this.cbCaptchaService.Label = "Captcha service";
            this.cbCaptchaService.LabelWidth = 90;
            this.cbCaptchaService.MaxLength = 300;
            this.cbCaptchaService.Name = "cbCaptchaService";
            this.cbCaptchaService.TextAreaWidth = 30;
            // 
            // rbBPCAutoCaptcha
            // 
            this.rbBPCAutoCaptcha.Name = "rbBPCAutoCaptcha";
            this.rbBPCAutoCaptcha.Text = "BPC";
            // 
            // rbRDAutoCaptcha
            // 
            this.rbRDAutoCaptcha.Name = "rbRDAutoCaptcha";
            this.rbRDAutoCaptcha.Text = "RD";
            // 
            // rbCPTAutoCaptcha
            // 
            this.rbCPTAutoCaptcha.Name = "rbCPTAutoCaptcha";
            this.rbCPTAutoCaptcha.Text = "CPT";
            // 
            // rbDBCAutoCaptcha
            // 
            this.rbDBCAutoCaptcha.Name = "rbDBCAutoCaptcha";
            this.rbDBCAutoCaptcha.Text = "DBC";
            // 
            // rbDCAutoCaptcha
            // 
            this.rbDCAutoCaptcha.Name = "rbDCAutoCaptcha";
            this.rbDCAutoCaptcha.Text = "DC";
            // 
            // rbOCR
            // 
            this.rbOCR.Name = "rbOCR";
            this.rbOCR.Text = "O";
            // 
            // rbCAutoCaptcha
            // 
            this.rbCAutoCaptcha.Name = "rbCAutoCaptcha";
            this.rbCAutoCaptcha.Text = "C";
            // 
            // rbRAutoCaptcha
            // 
            this.rbRAutoCaptcha.Name = "rbRAutoCaptcha";
            this.rbRAutoCaptcha.Text = "RC";
            // 
            // rbBoloAutoCaptcha
            // 
            this.rbBoloAutoCaptcha.Name = "rbBoloAutoCaptcha";
            this.rbBoloAutoCaptcha.Text = "B";
            // 
            // rbSOCR
            // 
            this.rbSOCR.Name = "rbSOCR";
            this.rbSOCR.Text = "S";
            // 
            // rb2CAutoCaptcha
            // 
            this.rb2CAutoCaptcha.Name = "rb2CAutoCaptcha";
            this.rb2CAutoCaptcha.Text = "2C";
            // 
            // rbAAutoCaptcha
            // 
            this.rbAAutoCaptcha.Name = "rbAAutoCaptcha";
            this.rbAAutoCaptcha.Text = "A";
            // 
            // rbCaptchatorAutoCaptcha
            // 
            this.rbCaptchatorAutoCaptcha.Name = "rbCaptchatorAutoCaptcha";
            this.rbCaptchatorAutoCaptcha.Text = "CTR";
            // 
            // rbAC1AutoCaptcha
            // 
            this.rbAC1AutoCaptcha.Name = "rbAC1AutoCaptcha";
            this.rbAC1AutoCaptcha.Text = "C1";
            // 
            // rbRDCAutoCaptcha
            // 
            this.rbRDCAutoCaptcha.Name = "rbRDCAutoCaptcha";
            this.rbRDCAutoCaptcha.Text = "RDC";
            // 
            // nudStartSolvingCaptchaFrom
            // 
            this.nudStartSolvingCaptchaFrom.Label = "Start solving from";
            this.nudStartSolvingCaptchaFrom.LabelWidth = 90;
            this.nudStartSolvingCaptchaFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStartSolvingCaptchaFrom.Name = "nudStartSolvingCaptchaFrom";
            this.nudStartSolvingCaptchaFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudStartSolvingCaptchaFrom.TextAreaWidth = 25;
            this.nudStartSolvingCaptchaFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ribbonSeparator1
            // 
            this.ribbonSeparator1.Name = "ribbonSeparator1";
            // 
            // chkUseProxies
            // 
            this.chkUseProxies.Name = "chkUseProxies";
            this.chkUseProxies.Text = "Use proxies";
            this.chkUseProxies.CheckedChanged += new System.EventHandler(this.chkUseProxies_CheckedChanged);
            // 
            // nudStartUsingProxiesFrom
            // 
            this.nudStartUsingProxiesFrom.Label = "Start proxies from";
            this.nudStartUsingProxiesFrom.LabelWidth = 90;
            this.nudStartUsingProxiesFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStartUsingProxiesFrom.Name = "nudStartUsingProxiesFrom";
            this.nudStartUsingProxiesFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudStartUsingProxiesFrom.TextAreaWidth = 20;
            this.nudStartUsingProxiesFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkUseProxiesInCaptchaSource
            // 
            this.chkUseProxiesInCaptchaSource.Name = "chkUseProxiesInCaptchaSource";
            this.chkUseProxiesInCaptchaSource.Text = "Use proxy in captcha";
            // 
            // ribbonSeparator11
            // 
            this.ribbonSeparator11.Name = "ribbonSeparator11";
            // 
            // chkAutoBuy
            // 
            this.chkAutoBuy.Name = "chkAutoBuy";
            this.chkAutoBuy.Text = "Automatically buy";
            // 
            // chkPlayMusic
            // 
            this.chkPlayMusic.Name = "chkPlayMusic";
            this.chkPlayMusic.Text = "Play alert sound ";
            // 
            // AXSByPassWaitingRoom
            // 
            this.AXSByPassWaitingRoom.Name = "AXSByPassWaitingRoom";
            this.AXSByPassWaitingRoom.Text = "Bypass Waiting Room";
            this.AXSByPassWaitingRoom.Visible = false;
            // 
            // nudNumberOfSearches
            // 
            this.nudNumberOfSearches.Label = "Searches";
            this.nudNumberOfSearches.LabelWidth = 90;
            this.nudNumberOfSearches.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumberOfSearches.Name = "nudNumberOfSearches";
            this.nudNumberOfSearches.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudNumberOfSearches.TextAreaWidth = 20;
            this.nudNumberOfSearches.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // rbSaveAndChange
            // 
            this.rbSaveAndChange.Name = "rbSaveAndChange";
            this.rbSaveAndChange.SmallImage = global::Automatick.Properties.Resources._1347971328_save16;
            this.rbSaveAndChange.SupportedGroupSizing = C1.Win.C1Ribbon.SupportedGroupSizing.TextAlwaysVisible;
            this.rbSaveAndChange.Text = "Save and change";
            this.rbSaveAndChange.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
            this.rbSaveAndChange.Click += new System.EventHandler(this.rbSaveAndChange_Click);
            // 
            // chkPersistSession
            // 
            this.chkPersistSession.Name = "chkPersistSession";
            this.chkPersistSession.Text = "Persist search session";
            this.chkPersistSession.Visible = false;
            // 
            // ribbonToolBar1
            // 
            this.ribbonToolBar1.Name = "ribbonToolBar1";
            // 
            // ribbonLabel1
            // 
            this.ribbonLabel1.Name = "ribbonLabel1";
            this.ribbonLabel1.Text = " ";
            // 
            // timerFocusScreen
            // 
            this.timerFocusScreen.Enabled = true;
            this.timerFocusScreen.Interval = 1000;
            this.timerFocusScreen.Tick += new System.EventHandler(this.timerFocusScreen_Tick);
            // 
            // grvFindTickets
            // 
            this.grvFindTickets.AllowUserToAddRows = false;
            this.grvFindTickets.AllowUserToDeleteRows = false;
            this.grvFindTickets.AllowUserToResizeRows = false;
            this.grvFindTickets.AutoGenerateColumns = false;
            this.grvFindTickets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grvFindTickets.BackgroundColor = System.Drawing.Color.White;
            this.grvFindTickets.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.grvFindTickets.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grvFindTickets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Flag,
            this.sectionDataGridViewTextBoxColumn,
            this.rowDataGridViewTextBoxColumn,
            this.seatDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.Price,
            this.descriptionDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.timeLeftDataGridViewTextBoxColumn,
            this.MoreInfo});
            this.grvFindTickets.DataSource = this.AXSSearchBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grvFindTickets.DefaultCellStyle = dataGridViewCellStyle2;
            this.grvFindTickets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grvFindTickets.GridColor = System.Drawing.Color.White;
            this.grvFindTickets.Location = new System.Drawing.Point(0, 152);
            this.grvFindTickets.Name = "grvFindTickets";
            this.grvFindTickets.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grvFindTickets.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grvFindTickets.RowHeadersVisible = false;
            this.grvFindTickets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grvFindTickets.Size = new System.Drawing.Size(892, 233);
            this.grvFindTickets.StandardTab = true;
            this.grvFindTickets.TabIndex = 13;
            this.grvFindTickets.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvFindTickets_CellClick);
            this.grvFindTickets.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grvFindTickets_DataError);
            // 
            // Flag
            // 
            this.Flag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Flag.DataPropertyName = "FlagImage";
            this.Flag.FillWeight = 3F;
            this.Flag.HeaderText = "";
            this.Flag.MinimumWidth = 20;
            this.Flag.Name = "Flag";
            this.Flag.ReadOnly = true;
            this.Flag.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Flag.Width = 25;
            // 
            // sectionDataGridViewTextBoxColumn
            // 
            this.sectionDataGridViewTextBoxColumn.DataPropertyName = "Section";
            this.sectionDataGridViewTextBoxColumn.FillWeight = 7F;
            this.sectionDataGridViewTextBoxColumn.HeaderText = "Section";
            this.sectionDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.sectionDataGridViewTextBoxColumn.Name = "sectionDataGridViewTextBoxColumn";
            this.sectionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rowDataGridViewTextBoxColumn
            // 
            this.rowDataGridViewTextBoxColumn.DataPropertyName = "Row";
            this.rowDataGridViewTextBoxColumn.FillWeight = 5F;
            this.rowDataGridViewTextBoxColumn.HeaderText = "Row";
            this.rowDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.rowDataGridViewTextBoxColumn.Name = "rowDataGridViewTextBoxColumn";
            this.rowDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // seatDataGridViewTextBoxColumn
            // 
            this.seatDataGridViewTextBoxColumn.DataPropertyName = "Seat";
            this.seatDataGridViewTextBoxColumn.FillWeight = 6F;
            this.seatDataGridViewTextBoxColumn.HeaderText = "Seat";
            this.seatDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.seatDataGridViewTextBoxColumn.Name = "seatDataGridViewTextBoxColumn";
            this.seatDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.FillWeight = 7F;
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Price.DefaultCellStyle = dataGridViewCellStyle1;
            this.Price.FillWeight = 10F;
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.FillWeight = 15F;
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.FillWeight = 15F;
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // timeLeftDataGridViewTextBoxColumn
            // 
            this.timeLeftDataGridViewTextBoxColumn.DataPropertyName = "TimeLeft";
            this.timeLeftDataGridViewTextBoxColumn.FillWeight = 7F;
            this.timeLeftDataGridViewTextBoxColumn.HeaderText = "TimeLeft";
            this.timeLeftDataGridViewTextBoxColumn.MinimumWidth = 50;
            this.timeLeftDataGridViewTextBoxColumn.Name = "timeLeftDataGridViewTextBoxColumn";
            this.timeLeftDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // MoreInfo
            // 
            this.MoreInfo.DataPropertyName = "MoreInfo";
            this.MoreInfo.FillWeight = 23F;
            this.MoreInfo.HeaderText = "MoreInfo";
            this.MoreInfo.MinimumWidth = 50;
            this.MoreInfo.Name = "MoreInfo";
            this.MoreInfo.ReadOnly = true;
            // 
            // AXSSearchBindingSource
            // 
            this.AXSSearchBindingSource.DataSource = typeof(Automatick.Core.AXSSearch);
            // 
            // frmFindTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 385);
            this.Controls.Add(this.grvFindTickets);
            this.Controls.Add(this.rMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 193);
            this.Name = "frmFindTicket";
            this.Text = "Find Tickets";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2007Silver;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFindTicket_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFindTicket_FormClosed);
            this.Load += new System.EventHandler(this.frmFindTicket_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvFindTickets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AXSSearchBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private C1.Win.C1Ribbon.C1Ribbon rMain;
        private C1.Win.C1Ribbon.RibbonApplicationMenu ribbonApplicationMenu1;
        private C1.Win.C1Ribbon.RibbonConfigToolBar ribbonConfigToolBar1;
        private C1.Win.C1Ribbon.RibbonQat ribbonQat1;
        private C1.Win.C1Ribbon.RibbonTab ribbonTab1;
        private C1.Win.C1Ribbon.RibbonGroup ribbonGroup1;
        private C1.Win.C1Ribbon.RibbonButton rbResetSelectedSearch;
        private C1.Win.C1Ribbon.RibbonGroup ribbonGroup2;
        private C1.Win.C1Ribbon.RibbonButton rbClose;
        private C1.Win.C1Ribbon.RibbonButton rbSaveAndChange;
        private C1.Win.C1Ribbon.RibbonToolBar ribbonToolBar1;
        private C1.Win.C1Ribbon.RibbonCheckBox chkAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonComboBox cbCaptchaService;
        private C1.Win.C1Ribbon.RibbonButton rbBPCAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonButton rbRDAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonButton rbCPTAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonButton rbDBCAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonNumericBox nudStartSolvingCaptchaFrom;
        private C1.Win.C1Ribbon.RibbonSeparator ribbonSeparator1;
        private C1.Win.C1Ribbon.RibbonCheckBox chkPlayMusic;
        private System.Windows.Forms.Timer timerFocusScreen;
        private C1.Win.C1Ribbon.RibbonButton rbShowHideBar;
        private C1.Win.C1Ribbon.RibbonCheckBox chkPersistSession;
        private C1.Win.C1Ribbon.RibbonCheckBox chkUseProxiesInCaptchaSource;
        private C1.Win.C1Ribbon.RibbonCheckBox chkSendEmail;
        private C1.Win.C1Ribbon.RibbonCheckBox chkAutoBuy;

        private C1.Win.C1Ribbon.RibbonCheckBox AXSByPassWaitingRoom;
        private System.Windows.Forms.DataGridView grvFindTickets;
        private System.Windows.Forms.BindingSource AXSSearchBindingSource;
        private C1.Win.C1Ribbon.RibbonCheckBox chkUseProxies;
        private C1.Win.C1Ribbon.RibbonNumericBox nudStartUsingProxiesFrom;
        private C1.Win.C1Ribbon.RibbonNumericBox nudNumberOfSearches;
        private C1.Win.C1Ribbon.RibbonGroup ribbonGroup21;
        private C1.Win.C1Ribbon.RibbonButton rbAutoBuy;
        private C1.Win.C1Ribbon.RibbonButton rbAutoBuyGuest;
        private C1.Win.C1Ribbon.RibbonButton rbAutoBuyWithoutProxy;
        private C1.Win.C1Ribbon.RibbonButton rbResetAll;
        private C1.Win.C1Ribbon.RibbonLabel ribbonLabel1;
        private C1.Win.C1Ribbon.RibbonButton rbDCAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonButton rbOCR;
        private C1.Win.C1Ribbon.RibbonButton rbCAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonButton rbRAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonButton rbBoloAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonButton rbSOCR;
        private System.Windows.Forms.DataGridViewImageColumn Flag;
        private System.Windows.Forms.DataGridViewTextBoxColumn sectionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seatDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeLeftDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MoreInfo;
        private C1.Win.C1Ribbon.RibbonButton rb2CAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonButton rbAAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonButton rbCaptchatorAutoCaptcha;
        private C1.Win.C1Ribbon.RibbonButton rbAC1AutoCaptcha;
        private C1.Win.C1Ribbon.RibbonSeparator ribbonSeparator11;
        private C1.Win.C1Ribbon.RibbonButton rbRDCAutoCaptcha;
    }
}
