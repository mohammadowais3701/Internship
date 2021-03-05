using Automatick.Core;
namespace Automatick
{
    partial class frmMain
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.ribbonMain = new C1.Win.C1Ribbon.C1Ribbon();
            this.ribbonApplicationMenu1 = new C1.Win.C1Ribbon.RibbonApplicationMenu();
            this.ribbonConfigToolBar1 = new C1.Win.C1Ribbon.RibbonConfigToolBar();
            this.RibbonStyleMenu = new C1.Win.C1Ribbon.RibbonMenu();
            this.ribbonButton1 = new C1.Win.C1Ribbon.RibbonButton();
            this.ribbonButton2 = new C1.Win.C1Ribbon.RibbonButton();
            this.ribbonButton3 = new C1.Win.C1Ribbon.RibbonButton();
            this.ribbonQat1 = new C1.Win.C1Ribbon.RibbonQat();
            this.ribbonTabEvents = new C1.Win.C1Ribbon.RibbonTab();
            this.rgTicketsActions = new C1.Win.C1Ribbon.RibbonGroup();
            this.rbRunTicket = new C1.Win.C1Ribbon.RibbonButton();
            this.rbAddTicket = new C1.Win.C1Ribbon.RibbonButton();
            this.rbEditTicket = new C1.Win.C1Ribbon.RibbonButton();
            this.rbDeleteTicket = new C1.Win.C1Ribbon.RibbonButton();
            this.rgTickets = new C1.Win.C1Ribbon.RibbonGroup();
            this.rgAllTickets = new C1.Win.C1Ribbon.RibbonGallery();
            this.ribbonSeparator1 = new C1.Win.C1Ribbon.RibbonSeparator();
            this.rcbFilter = new C1.Win.C1Ribbon.RibbonTextBox();
            this.rcbGroups = new C1.Win.C1Ribbon.RibbonComboBox();
            this.rbManageGroup = new C1.Win.C1Ribbon.RibbonButton();
            this.rbConfigurationsAndSettings = new C1.Win.C1Ribbon.RibbonGroup();
            this.rbAccounts = new C1.Win.C1Ribbon.RibbonButton();
            this.rbDeliveryOptions = new C1.Win.C1Ribbon.RibbonButton();
            this.rbAutoCaptcha = new C1.Win.C1Ribbon.RibbonButton();
            this.rbEmail = new C1.Win.C1Ribbon.RibbonButton();
            this.ribbonButton4 = new C1.Win.C1Ribbon.RibbonButton();
            this.rbProxy = new C1.Win.C1Ribbon.RibbonGroup();
            this.rbTxtUsername = new C1.Win.C1Ribbon.RibbonTextBox();
            this.rbTxtPassword = new C1.Win.C1Ribbon.RibbonTextBox();
            this.ribbonLabel1 = new C1.Win.C1Ribbon.RibbonLabel();
            this.rbBtnSaveProxySettings = new C1.Win.C1Ribbon.RibbonButton();
            this.rgProxyManager = new C1.Win.C1Ribbon.RibbonGroup();
            this.rbProxies = new C1.Win.C1Ribbon.RibbonButton();
            this.rbRegisterPM = new C1.Win.C1Ribbon.RibbonButton();
            this.chkEnableProxyManager = new C1.Win.C1Ribbon.RibbonCheckBox();
            this.chkEnableISPProxyUsage = new C1.Win.C1Ribbon.RibbonCheckBox();
            this.chkEnableLuminatiProxies = new C1.Win.C1Ribbon.RibbonCheckBox();
            this.rbStartAndStopTixTox = new C1.Win.C1Ribbon.RibbonButton();
            this.rgAutomatickLogo = new C1.Win.C1Ribbon.RibbonGroup();
            this.btnAutomatickLogo = new C1.Win.C1Ribbon.RibbonButton();
            this.flpRecentTickets = new System.Windows.Forms.FlowLayoutPanel();
            this.timerValidateLicense = new System.Windows.Forms.Timer(this.components);
            this.c1TaskDialog1 = new C1.Win.C1Win7Pack.C1TaskDialog(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonMain)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonMain
            // 
            this.ribbonMain.AllowMinimize = false;
            this.ribbonMain.ApplicationMenuHolder = this.ribbonApplicationMenu1;
            this.ribbonMain.ConfigToolBarHolder = this.ribbonConfigToolBar1;
            this.ribbonMain.Location = new System.Drawing.Point(0, 0);
            this.ribbonMain.Name = "ribbonMain";
            this.ribbonMain.QatHolder = this.ribbonQat1;
            this.ribbonMain.Size = new System.Drawing.Size(990, 152);
            this.ribbonMain.Tabs.Add(this.ribbonTabEvents);
            this.ribbonMain.RibbonEvent += new C1.Win.C1Ribbon.RibbonEventHandler(this.ribbonMain_RibbonEvent);
            // 
            // ribbonApplicationMenu1
            // 
            this.ribbonApplicationMenu1.LargeImage = global::Automatick.Properties.Resources.Automatic321;
            this.ribbonApplicationMenu1.Name = "ribbonApplicationMenu1";
            // 
            // ribbonConfigToolBar1
            // 
            this.ribbonConfigToolBar1.Items.Add(this.RibbonStyleMenu);
            this.ribbonConfigToolBar1.Name = "ribbonConfigToolBar1";
            this.ribbonConfigToolBar1.Visible = false;
            // 
            // RibbonStyleMenu
            // 
            this.RibbonStyleMenu.Items.Add(this.ribbonButton1);
            this.RibbonStyleMenu.Items.Add(this.ribbonButton2);
            this.RibbonStyleMenu.Items.Add(this.ribbonButton3);
            this.RibbonStyleMenu.Name = "RibbonStyleMenu";
            this.RibbonStyleMenu.Text = "Style";
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.Name = "ribbonButton1";
            this.ribbonButton1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.SmallImage")));
            this.ribbonButton1.Text = "Button";
            // 
            // ribbonButton2
            // 
            this.ribbonButton2.Name = "ribbonButton2";
            this.ribbonButton2.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.SmallImage")));
            this.ribbonButton2.Text = "Button";
            // 
            // ribbonButton3
            // 
            this.ribbonButton3.Name = "ribbonButton3";
            this.ribbonButton3.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.SmallImage")));
            this.ribbonButton3.Text = "Button";
            // 
            // ribbonQat1
            // 
            this.ribbonQat1.Name = "ribbonQat1";
            this.ribbonQat1.Visible = false;
            // 
            // ribbonTabEvents
            // 
            this.ribbonTabEvents.Groups.Add(this.rgTicketsActions);
            this.ribbonTabEvents.Groups.Add(this.rgTickets);
            this.ribbonTabEvents.Groups.Add(this.rbConfigurationsAndSettings);
            this.ribbonTabEvents.Groups.Add(this.rbProxy);
            this.ribbonTabEvents.Groups.Add(this.rgProxyManager);
            this.ribbonTabEvents.Groups.Add(this.rgAutomatickLogo);
            this.ribbonTabEvents.Name = "ribbonTabEvents";
            this.ribbonTabEvents.Text = "Events";
            // 
            // rgTicketsActions
            // 
            this.rgTicketsActions.Items.Add(this.rbRunTicket);
            this.rgTicketsActions.Items.Add(this.rbAddTicket);
            this.rgTicketsActions.Items.Add(this.rbEditTicket);
            this.rgTicketsActions.Items.Add(this.rbDeleteTicket);
            this.rgTicketsActions.Name = "rgTicketsActions";
            this.rgTicketsActions.Text = "Events actions";
            // 
            // rbRunTicket
            // 
            this.rbRunTicket.Enabled = false;
            this.rbRunTicket.LargeImage = global::Automatick.Properties.Resources._1348034999_continue;
            this.rbRunTicket.Name = "rbRunTicket";
            this.rbRunTicket.ShortcutKeyDisplayString = "Alt+R";
            this.rbRunTicket.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            this.rbRunTicket.Text = "Run";
            this.rbRunTicket.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageAboveText;
            this.rbRunTicket.Click += new System.EventHandler(this.rbRunTicket_Click);
            // 
            // rbAddTicket
            // 
            this.rbAddTicket.Name = "rbAddTicket";
            this.rbAddTicket.ShortcutKeyDisplayString = "Alt+A";
            this.rbAddTicket.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.rbAddTicket.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbAddTicket.SmallImage")));
            this.rbAddTicket.Text = "Add";
            this.rbAddTicket.Click += new System.EventHandler(this.rbAddTicket_Click);
            // 
            // rbEditTicket
            // 
            this.rbEditTicket.Enabled = false;
            this.rbEditTicket.Name = "rbEditTicket";
            this.rbEditTicket.ShortcutKeyDisplayString = "Alt+E";
            this.rbEditTicket.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
            this.rbEditTicket.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbEditTicket.SmallImage")));
            this.rbEditTicket.Text = "Edit";
            this.rbEditTicket.Click += new System.EventHandler(this.rbEditTicket_Click);
            // 
            // rbDeleteTicket
            // 
            this.rbDeleteTicket.Enabled = false;
            this.rbDeleteTicket.Name = "rbDeleteTicket";
            this.rbDeleteTicket.ShortcutKeyDisplayString = "Alt+D";
            this.rbDeleteTicket.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
            this.rbDeleteTicket.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbDeleteTicket.SmallImage")));
            this.rbDeleteTicket.Text = "Delete";
            this.rbDeleteTicket.Click += new System.EventHandler(this.rbDeleteTicket_Click);
            // 
            // rgTickets
            // 
            this.rgTickets.Items.Add(this.rgAllTickets);
            this.rgTickets.Items.Add(this.ribbonSeparator1);
            this.rgTickets.Items.Add(this.rcbFilter);
            this.rgTickets.Items.Add(this.rcbGroups);
            this.rgTickets.Items.Add(this.rbManageGroup);
            this.rgTickets.Name = "rgTickets";
            this.rgTickets.Text = "Events";
            // 
            // rgAllTickets
            // 
            this.rgAllTickets.ItemSize = new System.Drawing.Size(325, 30);
            this.rgAllTickets.Name = "rgAllTickets";
            this.rgAllTickets.SupportedGroupSizing = C1.Win.C1Ribbon.SupportedGroupSizing.TextAlwaysVisible;
            this.rgAllTickets.Text = "Events";
            this.rgAllTickets.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
            this.rgAllTickets.VisibleItems = 1;
            this.rgAllTickets.SelectedIndexChanged += new System.EventHandler(this.rgAllTickets_SelectedIndexChanged);
            // 
            // ribbonSeparator1
            // 
            this.ribbonSeparator1.Name = "ribbonSeparator1";
            // 
            // rcbFilter
            // 
            this.rcbFilter.Label = "Filter";
            this.rcbFilter.LabelWidth = 30;
            this.rcbFilter.Name = "rcbFilter";
            this.rcbFilter.TextAreaWidth = 132;
            this.rcbFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.rcbFilter_KeyUp);
            this.rcbFilter.ChangeCommitted += new System.EventHandler(this.rcbFilter_ChangeCommitted);
            // 
            // rcbGroups
            // 
            this.rcbGroups.DropDownStyle = C1.Win.C1Ribbon.RibbonComboBoxStyle.DropDownList;
            this.rcbGroups.Label = "Group";
            this.rcbGroups.LabelWidth = 30;
            this.rcbGroups.Name = "rcbGroups";
            this.rcbGroups.TextAreaWidth = 120;
            this.rcbGroups.SelectedIndexChanged += new System.EventHandler(this.rcbGroups_SelectedIndexChanged);
            // 
            // rbManageGroup
            // 
            this.rbManageGroup.Name = "rbManageGroup";
            this.rbManageGroup.ShortcutKeyDisplayString = "Alt+G";
            this.rbManageGroup.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.G)));
            this.rbManageGroup.SmallImage = global::Automatick.Properties.Resources._1350471720_category;
            this.rbManageGroup.Text = "Manage groups";
            this.rbManageGroup.Click += new System.EventHandler(this.rbManageGroup_Click);
            // 
            // rbConfigurationsAndSettings
            // 
            this.rbConfigurationsAndSettings.Items.Add(this.rbAccounts);
            this.rbConfigurationsAndSettings.Items.Add(this.rbDeliveryOptions);
            this.rbConfigurationsAndSettings.Items.Add(this.rbAutoCaptcha);
            this.rbConfigurationsAndSettings.Items.Add(this.rbEmail);
            this.rbConfigurationsAndSettings.Items.Add(this.ribbonButton4);
            this.rbConfigurationsAndSettings.Name = "rbConfigurationsAndSettings";
            this.rbConfigurationsAndSettings.Text = "Configurations && settings";
            // 
            // rbAccounts
            // 
            this.rbAccounts.Name = "rbAccounts";
            this.rbAccounts.SmallImage = global::Automatick.Properties.Resources._1324639284_Edit_Male_User_16;
            this.rbAccounts.Text = "Accounts";
            this.rbAccounts.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
            this.rbAccounts.Click += new System.EventHandler(this.rbAccounts_Click);
            // 
            // rbDeliveryOptions
            // 
            this.rbDeliveryOptions.Name = "rbDeliveryOptions";
            this.rbDeliveryOptions.SmallImage = global::Automatick.Properties.Resources._1350465055_box_16;
            this.rbDeliveryOptions.Text = "Delivery options";
            this.rbDeliveryOptions.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
            this.rbDeliveryOptions.Click += new System.EventHandler(this.rbDeliveryOptions_Click);
            // 
            // rbAutoCaptcha
            // 
            this.rbAutoCaptcha.LargeImage = global::Automatick.Properties.Resources._1324649434_Flag1_Green24;
            this.rbAutoCaptcha.Name = "rbAutoCaptcha";
            this.rbAutoCaptcha.SmallImage = global::Automatick.Properties.Resources._1324649434_Flag1_Green16;
            this.rbAutoCaptcha.Text = "Auto captcha";
            this.rbAutoCaptcha.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
            this.rbAutoCaptcha.Click += new System.EventHandler(this.rbAutoCaptcha_Click);
            // 
            // rbEmail
            // 
            this.rbEmail.Name = "rbEmail";
            this.rbEmail.SmallImage = global::Automatick.Properties.Resources._1333455860_email_open16;
            this.rbEmail.Text = "Email setting";
            this.rbEmail.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
            this.rbEmail.Click += new System.EventHandler(this.rbEmail_Click);
            // 
            // ribbonButton4
            // 
            this.ribbonButton4.LargeImage = global::Automatick.Properties.Resources._1341905378_gnome_mime_application_vnd_lotus_1_2_3;
            this.ribbonButton4.Name = "ribbonButton4";
            this.ribbonButton4.SmallImage = global::Automatick.Properties.Resources._1341905376_gnome_mime_application_vnd_lotus_1_2_3;
            this.ribbonButton4.Text = "Ticket Logs";
            this.ribbonButton4.Click += new System.EventHandler(this.ribbonButton4_Click);
            // 
            // rbProxy
            // 
            this.rbProxy.Items.Add(this.rbTxtUsername);
            this.rbProxy.Items.Add(this.rbTxtPassword);
            this.rbProxy.Items.Add(this.ribbonLabel1);
            this.rbProxy.Items.Add(this.rbBtnSaveProxySettings);
            this.rbProxy.Name = "rbProxy";
            this.rbProxy.Text = "Proxy";
            this.rbProxy.Visible = false;
            // 
            // rbTxtUsername
            // 
            this.rbTxtUsername.Label = "Username";
            this.rbTxtUsername.Name = "rbTxtUsername";
            // 
            // rbTxtPassword
            // 
            this.rbTxtPassword.Label = "Password ";
            this.rbTxtPassword.Name = "rbTxtPassword";
            this.rbTxtPassword.PasswordChar = '*';
            // 
            // ribbonLabel1
            // 
            this.ribbonLabel1.Name = "ribbonLabel1";
            // 
            // rbBtnSaveProxySettings
            // 
            this.rbBtnSaveProxySettings.Name = "rbBtnSaveProxySettings";
            this.rbBtnSaveProxySettings.Click += new System.EventHandler(this.rbBtnSaveProxySettings_Click);
            // 
            // rgProxyManager
            // 
            this.rgProxyManager.Items.Add(this.rbProxies);
            this.rgProxyManager.Items.Add(this.rbRegisterPM);
            this.rgProxyManager.Items.Add(this.chkEnableProxyManager);
            this.rgProxyManager.Items.Add(this.chkEnableISPProxyUsage);
            this.rgProxyManager.Items.Add(this.chkEnableLuminatiProxies);
            this.rgProxyManager.Items.Add(this.rbStartAndStopTixTox);
            this.rgProxyManager.Name = "rgProxyManager";
            this.rgProxyManager.Text = "Proxy manager";
            // 
            // rbProxies
            // 
            this.rbProxies.LargeImage = global::Automatick.Properties.Resources._1324639344_network;
            this.rbProxies.Name = "rbProxies";
            this.rbProxies.SmallImage = global::Automatick.Properties.Resources._1324639344_network16;
            this.rbProxies.Text = "Proxies";
            this.rbProxies.Click += new System.EventHandler(this.rbProxies_Click);
            // 
            // rbRegisterPM
            // 
            this.rbRegisterPM.Name = "rbRegisterPM";
            this.rbRegisterPM.SmallImage = global::Automatick.Properties.Resources._1347983536_arrow_up_right_16;
            this.rbRegisterPM.Text = "Register PM";
            this.rbRegisterPM.Visible = false;
            this.rbRegisterPM.Click += new System.EventHandler(this.rbRegisterPM_Click);
            // 
            // chkEnableProxyManager
            // 
            this.chkEnableProxyManager.Name = "chkEnableProxyManager";
            this.chkEnableProxyManager.Text = "Enable proxy manager";
            this.chkEnableProxyManager.Visible = false;
            this.chkEnableProxyManager.CheckedChanged += new System.EventHandler(this.chkEnableProxyManager_CheckedChanged);
            // 
            // chkEnableISPProxyUsage
            // 
            this.chkEnableISPProxyUsage.Name = "chkEnableISPProxyUsage";
            this.chkEnableISPProxyUsage.Text = "Enable ISP proxy usage";
            this.chkEnableISPProxyUsage.Visible = false;
            this.chkEnableISPProxyUsage.CheckedChanged += new System.EventHandler(this.chkEnableISPProxyUsage_CheckedChanged);
            // 
            // chkEnableLuminatiProxies
            // 
            this.chkEnableLuminatiProxies.Checked = true;
            this.chkEnableLuminatiProxies.Name = "chkEnableLuminatiProxies";
            this.chkEnableLuminatiProxies.Text = "Enable Special Proxies";
            this.chkEnableLuminatiProxies.CheckedChanged += new System.EventHandler(this.chkEnableLuminatiProxies_CheckedChanged);
            // 
            // rbStartAndStopTixTox
            // 
            this.rbStartAndStopTixTox.Name = "rbStartAndStopTixTox";
            this.rbStartAndStopTixTox.SmallImage = global::Automatick.Properties.Resources.playb16;
            this.rbStartAndStopTixTox.Text = "Start Token Generator";
            this.rbStartAndStopTixTox.Visible = false;
            this.rbStartAndStopTixTox.Click += new System.EventHandler(this.rbStartAndStopTixTox_Click);
            // 
            // rgAutomatickLogo
            // 
            this.rgAutomatickLogo.Items.Add(this.btnAutomatickLogo);
            this.rgAutomatickLogo.Name = "rgAutomatickLogo";
            // 
            // btnAutomatickLogo
            // 
            this.btnAutomatickLogo.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutomatickLogo.ForeColorInner = System.Drawing.Color.DarkGray;
            this.btnAutomatickLogo.Name = "btnAutomatickLogo";
            this.btnAutomatickLogo.SmallImage = global::Automatick.Properties.Resources.Automatick_Logo4;
            this.btnAutomatickLogo.Text = "Automated ticket machine";
            this.btnAutomatickLogo.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageAboveText;
            // 
            // flpRecentTickets
            // 
            this.flpRecentTickets.AutoScroll = true;
            this.flpRecentTickets.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(60)))));
            this.flpRecentTickets.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.flpRecentTickets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpRecentTickets.Location = new System.Drawing.Point(0, 152);
            this.flpRecentTickets.Name = "flpRecentTickets";
            this.flpRecentTickets.Size = new System.Drawing.Size(990, 443);
            this.flpRecentTickets.TabIndex = 3;
            // 
            // timerValidateLicense
            // 
            this.timerValidateLicense.Enabled = true;
            this.timerValidateLicense.Interval = 1800000;
            this.timerValidateLicense.Tick += new System.EventHandler(this.timerValidateLicense_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 595);
            this.Controls.Add(this.flpRecentTickets);
            this.Controls.Add(this.ribbonMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(998, 591);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Automatick AXS - Version 6.2.1";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2007Silver;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private C1.Win.C1Ribbon.C1Ribbon ribbonMain;
        private C1.Win.C1Ribbon.RibbonApplicationMenu ribbonApplicationMenu1;
        private C1.Win.C1Ribbon.RibbonConfigToolBar ribbonConfigToolBar1;
        private C1.Win.C1Ribbon.RibbonQat ribbonQat1;
        private C1.Win.C1Ribbon.RibbonTab ribbonTabEvents;
        private C1.Win.C1Ribbon.RibbonGroup rgTickets;
        private C1.Win.C1Ribbon.RibbonMenu RibbonStyleMenu;
        private C1.Win.C1Ribbon.RibbonButton ribbonButton1;
        private C1.Win.C1Ribbon.RibbonButton ribbonButton2;
        private C1.Win.C1Ribbon.RibbonButton ribbonButton3;
        private C1.Win.C1Ribbon.RibbonGallery rgAllTickets;
        private C1.Win.C1Ribbon.RibbonGroup rgTicketsActions;
        private C1.Win.C1Ribbon.RibbonButton rbAddTicket;
        private C1.Win.C1Ribbon.RibbonButton rbEditTicket;
        private C1.Win.C1Ribbon.RibbonButton rbDeleteTicket;
        private C1.Win.C1Ribbon.RibbonButton rbRunTicket;
        private C1.Win.C1Ribbon.RibbonGroup rbConfigurationsAndSettings;
        private C1.Win.C1Ribbon.RibbonButton rbAccounts;
        private C1.Win.C1Ribbon.RibbonButton rbDeliveryOptions;
        private C1.Win.C1Ribbon.RibbonComboBox rcbGroups;
        private C1.Win.C1Ribbon.RibbonTextBox rcbFilter;
        private C1.Win.C1Ribbon.RibbonButton rbManageGroup;
        private System.Windows.Forms.FlowLayoutPanel flpRecentTickets;
        private C1.Win.C1Ribbon.RibbonButton rbEmail;
        private C1.Win.C1Ribbon.RibbonGroup rgProxyManager;
        private C1.Win.C1Ribbon.RibbonButton rbProxies;
        private C1.Win.C1Ribbon.RibbonSeparator ribbonSeparator1;
        private C1.Win.C1Ribbon.RibbonGroup rgAutomatickLogo;
        private C1.Win.C1Ribbon.RibbonButton btnAutomatickLogo;
        internal C1.Win.C1Ribbon.RibbonButton rbRegisterPM;
        internal C1.Win.C1Ribbon.RibbonCheckBox chkEnableProxyManager;
        internal C1.Win.C1Ribbon.RibbonCheckBox chkEnableISPProxyUsage;
        private System.Windows.Forms.Timer timerValidateLicense;
        private C1.Win.C1Ribbon.RibbonButton rbAutoCaptcha;
        private C1.Win.C1Win7Pack.C1TaskDialog c1TaskDialog1;
        private C1.Win.C1Ribbon.RibbonCheckBox chkEnableLuminatiProxies;
        private C1.Win.C1Ribbon.RibbonButton rbStartAndStopTixTox;
        private C1.Win.C1Ribbon.RibbonGroup rbProxy;
        private C1.Win.C1Ribbon.RibbonTextBox rbTxtUsername;
        private C1.Win.C1Ribbon.RibbonTextBox rbTxtPassword;
        private C1.Win.C1Ribbon.RibbonLabel ribbonLabel1;
        private C1.Win.C1Ribbon.RibbonButton rbBtnSaveProxySettings;
        private C1.Win.C1Ribbon.RibbonButton ribbonButton4;
    }
}

