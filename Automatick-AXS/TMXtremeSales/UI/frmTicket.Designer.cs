namespace Automatick
{
    partial class frmTicket
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTicket));
            this.txtTicketName = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtTicketURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new C1.Win.C1Input.C1Button();
            this.btnSaveandStart = new C1.Win.C1Input.C1Button();
            this.c1CommandHolder1 = new C1.Win.C1Command.C1CommandHolder();
            this.c1Command1 = new C1.Win.C1Command.C1Command();
            this.c1CommandControl1 = new C1.Win.C1Command.C1CommandControl();
            this.c1CommandControl2 = new C1.Win.C1Command.C1CommandControl();
            this.c1CommandControl3 = new C1.Win.C1Command.C1CommandControl();
            this.c1CommandControl4 = new C1.Win.C1Command.C1CommandControl();
            this.docParameters = new C1.Win.C1Command.C1DockingTab();
            this.tabPageParameter = new C1.Win.C1Command.C1DockingTabPage();
            this.pnlOnTicketsFound = new System.Windows.Forms.Panel();
            this.btnAutoBuyOptions = new System.Windows.Forms.LinkLabel();
            this.btnEmailSettings = new System.Windows.Forms.LinkLabel();
            this.chkSendEmail = new System.Windows.Forms.CheckBox();
            this.chkAutoBuyWitoutProxy = new System.Windows.Forms.CheckBox();
            this.chkAutoBuy = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkPlaySoundAlert = new System.Windows.Forms.CheckBox();
            this.pnlAutoCaptcha = new System.Windows.Forms.Panel();
            this.pnlCaptchaService = new System.Windows.Forms.Panel();
            this.rbRDCAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rbAC1AutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rbCaptchatorAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rbAAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rb2CAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rbSOCR = new System.Windows.Forms.RadioButton();
            this.rbBoloAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rbRAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rbCAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rbOCR = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.rbDCAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rbDBCAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rbCPTAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rbRDAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.rbBPCAutoCaptcha = new System.Windows.Forms.RadioButton();
            this.pnlCaptchaSolving = new System.Windows.Forms.Panel();
            this.nudStartSolvingFromCaptcha = new System.Windows.Forms.NumericUpDown();
            this.lblStartSolvingFrom = new System.Windows.Forms.Label();
            this.chkAutoCaptcha = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlSearchParameters = new System.Windows.Forms.Panel();
            this.dtSelect = new System.Windows.Forms.DateTimePicker();
            this.mcSelectDate = new System.Windows.Forms.MonthCalendar();
            this.gvParameters = new System.Windows.Forms.DataGridView();
            this.iTicketParameterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pnlSearchParametersOptions = new System.Windows.Forms.Panel();
            this.rbDistributeInSearches = new System.Windows.Forms.RadioButton();
            this.rbUseFoundOnFirstAttempt = new System.Windows.Forms.RadioButton();
            this.rbUseAvailableParameters = new System.Windows.Forms.RadioButton();
            this.pnlParamters = new System.Windows.Forms.Panel();
            this.chkMobile = new System.Windows.Forms.CheckBox();
            this.chkWeb = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkUseProxies = new System.Windows.Forms.CheckBox();
            this.nudNoOfSearches = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.chkRandomDelay = new System.Windows.Forms.CheckBox();
            this.nudResetSearchDelay = new System.Windows.Forms.NumericUpDown();
            this.Delay = new System.Windows.Forms.NumericUpDown();
            this.nudStartUsingProxiesFrom = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStartUsingProxiesFrom = new System.Windows.Forms.Label();
            this.tabPageMoreParameters = new C1.Win.C1Command.C1DockingTabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnManageAccounts = new System.Windows.Forms.LinkLabel();
            this.gvAccounts = new System.Windows.Forms.DataGridView();
            this.ifSelectedDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.accountNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BuyingLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iTicketAccountBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rbSelectAccountList = new System.Windows.Forms.RadioButton();
            this.rbSelectAccountAutoBuying = new System.Windows.Forms.RadioButton();
            this.gvFindingCriteria = new System.Windows.Forms.DataGridView();
            this.rowFromDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowToDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sectionDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iTicketFoundCriteriaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label14 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblMinutes = new System.Windows.Forms.Label();
            this.nudScheduleRunningTime = new System.Windows.Forms.NumericUpDown();
            this.chkSchedule = new System.Windows.Forms.CheckBox();
            this.lblRunningTime = new System.Windows.Forms.Label();
            this.dtpScheduleDateTime = new System.Windows.Forms.DateTimePicker();
            this.label18 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pnlDeliveryOptions = new System.Windows.Forms.Panel();
            this.btnManageDeliveryOptions = new System.Windows.Forms.LinkLabel();
            this.gvDeliveryOptions = new System.Windows.Forms.DataGridView();
            this.ifSelectedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.deliveryOptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deliveryCountryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iTicketDeliveryOptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.txtDeliveryCountry = new System.Windows.Forms.TextBox();
            this.txtDeliveryOption = new System.Windows.Forms.TextBox();
            this.lblDeliveryCountry = new System.Windows.Forms.Label();
            this.lblDeliveryOption = new System.Windows.Forms.Label();
            this.rbSelectDeliveryOptionList = new System.Windows.Forms.RadioButton();
            this.rbSelectDeliveryOptionAutoBuying = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.cbGroup = new System.Windows.Forms.ComboBox();
            this.btnGroup = new System.Windows.Forms.LinkLabel();
            this.chkPersistSession = new System.Windows.Forms.CheckBox();
            this.AXSByPassWaitingRoom = new System.Windows.Forms.CheckBox();
            this.chkIsWaiting = new System.Windows.Forms.CheckBox();
            this.chkUseProxiesInCaptchaSource = new System.Windows.Forms.CheckBox();
            this.Bought = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxBought = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTimeStringDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GetResaleTix = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.OfferName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceLevelStringDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exactMatchDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ticketTypePassswordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LowestPrice = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TopPrice = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.priceMinDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceMaxDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxToMin = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.c1CommandHolder1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.docParameters)).BeginInit();
            this.docParameters.SuspendLayout();
            this.tabPageParameter.SuspendLayout();
            this.pnlOnTicketsFound.SuspendLayout();
            this.pnlAutoCaptcha.SuspendLayout();
            this.pnlCaptchaService.SuspendLayout();
            this.pnlCaptchaSolving.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartSolvingFromCaptcha)).BeginInit();
            this.pnlSearchParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvParameters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTicketParameterBindingSource)).BeginInit();
            this.pnlSearchParametersOptions.SuspendLayout();
            this.pnlParamters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNoOfSearches)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudResetSearchDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Delay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartUsingProxiesFrom)).BeginInit();
            this.tabPageMoreParameters.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTicketAccountBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFindingCriteria)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTicketFoundCriteriaBindingSource)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScheduleRunningTime)).BeginInit();
            this.pnlDeliveryOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDeliveryOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTicketDeliveryOptionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTicketName
            // 
            this.txtTicketName.BackColor = System.Drawing.Color.GhostWhite;
            this.txtTicketName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTicketName.Location = new System.Drawing.Point(109, 46);
            this.txtTicketName.MaxLength = 50;
            this.txtTicketName.Name = "txtTicketName";
            this.txtTicketName.Size = new System.Drawing.Size(300, 20);
            this.txtTicketName.TabIndex = 1;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(7, 50);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(69, 13);
            this.label20.TabIndex = 260;
            this.label20.Text = "Ticket name:";
            // 
            // txtTicketURL
            // 
            this.txtTicketURL.BackColor = System.Drawing.Color.GhostWhite;
            this.txtTicketURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTicketURL.Location = new System.Drawing.Point(109, 12);
            this.txtTicketURL.Name = "txtTicketURL";
            this.txtTicketURL.Size = new System.Drawing.Size(660, 20);
            this.txtTicketURL.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(7, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 259;
            this.label1.Text = "Ticket URL/Link:";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(756, 522);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnSave.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveandStart
            // 
            this.btnSaveandStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveandStart.Location = new System.Drawing.Point(648, 522);
            this.btnSaveandStart.Name = "btnSaveandStart";
            this.btnSaveandStart.Size = new System.Drawing.Size(102, 23);
            this.btnSaveandStart.TabIndex = 5;
            this.btnSaveandStart.Text = "S&ave && Start";
            this.btnSaveandStart.UseVisualStyleBackColor = true;
            this.btnSaveandStart.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnSaveandStart.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnSaveandStart.Click += new System.EventHandler(this.btnSaveandStart_Click);
            // 
            // c1CommandHolder1
            // 
            this.c1CommandHolder1.Commands.Add(this.c1Command1);
            this.c1CommandHolder1.Commands.Add(this.c1CommandControl1);
            this.c1CommandHolder1.Commands.Add(this.c1CommandControl2);
            this.c1CommandHolder1.Commands.Add(this.c1CommandControl3);
            this.c1CommandHolder1.Commands.Add(this.c1CommandControl4);
            this.c1CommandHolder1.Owner = this;
            // 
            // c1Command1
            // 
            this.c1Command1.Name = "c1Command1";
            this.c1Command1.ShortcutText = "";
            this.c1Command1.Text = "New Command";
            // 
            // c1CommandControl1
            // 
            this.c1CommandControl1.Name = "c1CommandControl1";
            this.c1CommandControl1.ShortcutText = "";
            // 
            // c1CommandControl2
            // 
            this.c1CommandControl2.Name = "c1CommandControl2";
            this.c1CommandControl2.ShortcutText = "";
            // 
            // c1CommandControl3
            // 
            this.c1CommandControl3.Name = "c1CommandControl3";
            this.c1CommandControl3.ShortcutText = "";
            // 
            // c1CommandControl4
            // 
            this.c1CommandControl4.Name = "c1CommandControl4";
            this.c1CommandControl4.ShortcutText = "";
            // 
            // docParameters
            // 
            this.docParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.docParameters.Controls.Add(this.tabPageParameter);
            this.docParameters.Controls.Add(this.tabPageMoreParameters);
            this.docParameters.Location = new System.Drawing.Point(10, 73);
            this.docParameters.Name = "docParameters";
            this.docParameters.Size = new System.Drawing.Size(848, 436);
            this.docParameters.TabIndex = 3;
            this.docParameters.TabLook = C1.Win.C1Command.ButtonLookFlags.Text;
            this.docParameters.TabsSpacing = 2;
            this.docParameters.TabStyle = C1.Win.C1Command.TabStyleEnum.WindowsXP;
            this.docParameters.VisualStyle = C1.Win.C1Command.VisualStyle.WindowsXP;
            this.docParameters.VisualStyleBase = C1.Win.C1Command.VisualStyle.WindowsXP;
            // 
            // tabPageParameter
            // 
            this.tabPageParameter.BackColor = System.Drawing.Color.White;
            this.tabPageParameter.Controls.Add(this.pnlOnTicketsFound);
            this.tabPageParameter.Controls.Add(this.pnlAutoCaptcha);
            this.tabPageParameter.Controls.Add(this.pnlSearchParameters);
            this.tabPageParameter.Controls.Add(this.pnlSearchParametersOptions);
            this.tabPageParameter.Controls.Add(this.pnlParamters);
            this.tabPageParameter.Location = new System.Drawing.Point(2, 25);
            this.tabPageParameter.Name = "tabPageParameter";
            this.tabPageParameter.Size = new System.Drawing.Size(842, 407);
            this.tabPageParameter.TabIndex = 0;
            this.tabPageParameter.Text = "Parameters";
            // 
            // pnlOnTicketsFound
            // 
            this.pnlOnTicketsFound.BackColor = System.Drawing.Color.White;
            this.pnlOnTicketsFound.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOnTicketsFound.Controls.Add(this.btnAutoBuyOptions);
            this.pnlOnTicketsFound.Controls.Add(this.btnEmailSettings);
            this.pnlOnTicketsFound.Controls.Add(this.chkSendEmail);
            this.pnlOnTicketsFound.Controls.Add(this.chkAutoBuyWitoutProxy);
            this.pnlOnTicketsFound.Controls.Add(this.chkAutoBuy);
            this.pnlOnTicketsFound.Controls.Add(this.label9);
            this.pnlOnTicketsFound.Controls.Add(this.chkPlaySoundAlert);
            this.pnlOnTicketsFound.Location = new System.Drawing.Point(469, 332);
            this.pnlOnTicketsFound.Name = "pnlOnTicketsFound";
            this.pnlOnTicketsFound.Size = new System.Drawing.Size(370, 74);
            this.pnlOnTicketsFound.TabIndex = 3;
            // 
            // btnAutoBuyOptions
            // 
            this.btnAutoBuyOptions.ActiveLinkColor = System.Drawing.Color.SteelBlue;
            this.btnAutoBuyOptions.AutoSize = true;
            this.btnAutoBuyOptions.LinkColor = System.Drawing.Color.SteelBlue;
            this.btnAutoBuyOptions.Location = new System.Drawing.Point(297, 31);
            this.btnAutoBuyOptions.Name = "btnAutoBuyOptions";
            this.btnAutoBuyOptions.Size = new System.Drawing.Size(68, 13);
            this.btnAutoBuyOptions.TabIndex = 6;
            this.btnAutoBuyOptions.TabStop = true;
            this.btnAutoBuyOptions.Text = "(Buy options)";
            this.btnAutoBuyOptions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnAutoBuyOptions_LinkClicked);
            // 
            // btnEmailSettings
            // 
            this.btnEmailSettings.ActiveLinkColor = System.Drawing.Color.SteelBlue;
            this.btnEmailSettings.AutoSize = true;
            this.btnEmailSettings.LinkColor = System.Drawing.Color.SteelBlue;
            this.btnEmailSettings.Location = new System.Drawing.Point(77, 55);
            this.btnEmailSettings.Name = "btnEmailSettings";
            this.btnEmailSettings.Size = new System.Drawing.Size(113, 13);
            this.btnEmailSettings.TabIndex = 3;
            this.btnEmailSettings.TabStop = true;
            this.btnEmailSettings.Text = "(Email settings and list)";
            this.btnEmailSettings.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnEmailSettings_LinkClicked);
            // 
            // chkSendEmail
            // 
            this.chkSendEmail.AutoSize = true;
            this.chkSendEmail.BackColor = System.Drawing.Color.Transparent;
            this.chkSendEmail.Location = new System.Drawing.Point(3, 53);
            this.chkSendEmail.Name = "chkSendEmail";
            this.chkSendEmail.Size = new System.Drawing.Size(78, 17);
            this.chkSendEmail.TabIndex = 2;
            this.chkSendEmail.Text = "Send email";
            this.chkSendEmail.UseVisualStyleBackColor = false;
            this.chkSendEmail.CheckStateChanged += new System.EventHandler(this.chkSendEmail_CheckStateChanged);
            // 
            // chkAutoBuyWitoutProxy
            // 
            this.chkAutoBuyWitoutProxy.AutoSize = true;
            this.chkAutoBuyWitoutProxy.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoBuyWitoutProxy.Location = new System.Drawing.Point(193, 53);
            this.chkAutoBuyWitoutProxy.Name = "chkAutoBuyWitoutProxy";
            this.chkAutoBuyWitoutProxy.Size = new System.Drawing.Size(173, 17);
            this.chkAutoBuyWitoutProxy.TabIndex = 5;
            this.chkAutoBuyWitoutProxy.Text = "Automatically buy without proxy";
            this.chkAutoBuyWitoutProxy.UseVisualStyleBackColor = false;
            // 
            // chkAutoBuy
            // 
            this.chkAutoBuy.AutoSize = true;
            this.chkAutoBuy.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoBuy.Location = new System.Drawing.Point(193, 29);
            this.chkAutoBuy.Name = "chkAutoBuy";
            this.chkAutoBuy.Size = new System.Drawing.Size(108, 17);
            this.chkAutoBuy.TabIndex = 4;
            this.chkAutoBuy.Text = "Automatically buy";
            this.chkAutoBuy.UseVisualStyleBackColor = false;
            this.chkAutoBuy.CheckedChanged += new System.EventHandler(this.chkAutoBuy_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(3, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(118, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "When ticket has found:";
            // 
            // chkPlaySoundAlert
            // 
            this.chkPlaySoundAlert.AutoSize = true;
            this.chkPlaySoundAlert.BackColor = System.Drawing.Color.Transparent;
            this.chkPlaySoundAlert.Location = new System.Drawing.Point(3, 29);
            this.chkPlaySoundAlert.Name = "chkPlaySoundAlert";
            this.chkPlaySoundAlert.Size = new System.Drawing.Size(104, 17);
            this.chkPlaySoundAlert.TabIndex = 1;
            this.chkPlaySoundAlert.Text = "Play alert sound ";
            this.chkPlaySoundAlert.UseVisualStyleBackColor = false;
            // 
            // pnlAutoCaptcha
            // 
            this.pnlAutoCaptcha.BackColor = System.Drawing.Color.White;
            this.pnlAutoCaptcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAutoCaptcha.Controls.Add(this.pnlCaptchaService);
            this.pnlAutoCaptcha.Controls.Add(this.pnlCaptchaSolving);
            this.pnlAutoCaptcha.Location = new System.Drawing.Point(5, 332);
            this.pnlAutoCaptcha.Name = "pnlAutoCaptcha";
            this.pnlAutoCaptcha.Size = new System.Drawing.Size(458, 75);
            this.pnlAutoCaptcha.TabIndex = 5;
            // 
            // pnlCaptchaService
            // 
            this.pnlCaptchaService.BackColor = System.Drawing.Color.Transparent;
            this.pnlCaptchaService.Controls.Add(this.rbRDCAutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rbAC1AutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rbCaptchatorAutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rbAAutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rb2CAutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rbSOCR);
            this.pnlCaptchaService.Controls.Add(this.rbBoloAutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rbRAutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rbCAutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rbOCR);
            this.pnlCaptchaService.Controls.Add(this.label7);
            this.pnlCaptchaService.Controls.Add(this.rbDCAutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rbDBCAutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rbCPTAutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rbRDAutoCaptcha);
            this.pnlCaptchaService.Controls.Add(this.rbBPCAutoCaptcha);
            this.pnlCaptchaService.Location = new System.Drawing.Point(0, 32);
            this.pnlCaptchaService.Name = "pnlCaptchaService";
            this.pnlCaptchaService.Padding = new System.Windows.Forms.Padding(112, 0, 0, 0);
            this.pnlCaptchaService.Size = new System.Drawing.Size(751, 21);
            this.pnlCaptchaService.TabIndex = 9;
            // 
            // rbRDCAutoCaptcha
            // 
            this.rbRDCAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbRDCAutoCaptcha.Location = new System.Drawing.Point(696, 0);
            this.rbRDCAutoCaptcha.Name = "rbRDCAutoCaptcha";
            this.rbRDCAutoCaptcha.Size = new System.Drawing.Size(51, 21);
            this.rbRDCAutoCaptcha.TabIndex = 18;
            this.rbRDCAutoCaptcha.TabStop = true;
            this.rbRDCAutoCaptcha.Text = "RDC";
            this.rbRDCAutoCaptcha.UseVisualStyleBackColor = true;
            this.rbRDCAutoCaptcha.Visible = false;
            // 
            // rbAC1AutoCaptcha
            // 
            this.rbAC1AutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbAC1AutoCaptcha.Location = new System.Drawing.Point(647, 0);
            this.rbAC1AutoCaptcha.Name = "rbAC1AutoCaptcha";
            this.rbAC1AutoCaptcha.Size = new System.Drawing.Size(49, 21);
            this.rbAC1AutoCaptcha.TabIndex = 17;
            this.rbAC1AutoCaptcha.TabStop = true;
            this.rbAC1AutoCaptcha.Text = "C1";
            this.rbAC1AutoCaptcha.UseVisualStyleBackColor = true;
            // 
            // rbCaptchatorAutoCaptcha
            // 
            this.rbCaptchatorAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbCaptchatorAutoCaptcha.Location = new System.Drawing.Point(598, 0);
            this.rbCaptchatorAutoCaptcha.Name = "rbCaptchatorAutoCaptcha";
            this.rbCaptchatorAutoCaptcha.Size = new System.Drawing.Size(49, 21);
            this.rbCaptchatorAutoCaptcha.TabIndex = 16;
            this.rbCaptchatorAutoCaptcha.TabStop = true;
            this.rbCaptchatorAutoCaptcha.Text = "CTR";
            this.rbCaptchatorAutoCaptcha.UseVisualStyleBackColor = true;
            this.rbCaptchatorAutoCaptcha.Visible = false;
            // 
            // rbAAutoCaptcha
            // 
            this.rbAAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbAAutoCaptcha.Location = new System.Drawing.Point(563, 0);
            this.rbAAutoCaptcha.Name = "rbAAutoCaptcha";
            this.rbAAutoCaptcha.Size = new System.Drawing.Size(35, 21);
            this.rbAAutoCaptcha.TabIndex = 15;
            this.rbAAutoCaptcha.TabStop = true;
            this.rbAAutoCaptcha.Text = "A";
            this.rbAAutoCaptcha.UseVisualStyleBackColor = true;
            this.rbAAutoCaptcha.Visible = false;
            // 
            // rb2CAutoCaptcha
            // 
            this.rb2CAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rb2CAutoCaptcha.Location = new System.Drawing.Point(522, 0);
            this.rb2CAutoCaptcha.Name = "rb2CAutoCaptcha";
            this.rb2CAutoCaptcha.Size = new System.Drawing.Size(41, 21);
            this.rb2CAutoCaptcha.TabIndex = 14;
            this.rb2CAutoCaptcha.TabStop = true;
            this.rb2CAutoCaptcha.Text = "2C";
            this.rb2CAutoCaptcha.UseVisualStyleBackColor = true;
            this.rb2CAutoCaptcha.Visible = false;
            // 
            // rbSOCR
            // 
            this.rbSOCR.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbSOCR.Location = new System.Drawing.Point(486, 0);
            this.rbSOCR.Name = "rbSOCR";
            this.rbSOCR.Size = new System.Drawing.Size(36, 21);
            this.rbSOCR.TabIndex = 13;
            this.rbSOCR.TabStop = true;
            this.rbSOCR.Text = "S";
            this.rbSOCR.UseVisualStyleBackColor = true;
            this.rbSOCR.Visible = false;
            // 
            // rbBoloAutoCaptcha
            // 
            this.rbBoloAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbBoloAutoCaptcha.Location = new System.Drawing.Point(449, 0);
            this.rbBoloAutoCaptcha.Name = "rbBoloAutoCaptcha";
            this.rbBoloAutoCaptcha.Size = new System.Drawing.Size(37, 21);
            this.rbBoloAutoCaptcha.TabIndex = 12;
            this.rbBoloAutoCaptcha.TabStop = true;
            this.rbBoloAutoCaptcha.Text = "B";
            this.rbBoloAutoCaptcha.UseVisualStyleBackColor = true;
            this.rbBoloAutoCaptcha.Visible = false;
            // 
            // rbRAutoCaptcha
            // 
            this.rbRAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbRAutoCaptcha.Location = new System.Drawing.Point(404, 0);
            this.rbRAutoCaptcha.Name = "rbRAutoCaptcha";
            this.rbRAutoCaptcha.Size = new System.Drawing.Size(45, 21);
            this.rbRAutoCaptcha.TabIndex = 11;
            this.rbRAutoCaptcha.TabStop = true;
            this.rbRAutoCaptcha.Text = "RC";
            this.rbRAutoCaptcha.UseVisualStyleBackColor = true;
            this.rbRAutoCaptcha.Visible = false;
            // 
            // rbCAutoCaptcha
            // 
            this.rbCAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbCAutoCaptcha.Location = new System.Drawing.Point(370, 0);
            this.rbCAutoCaptcha.Name = "rbCAutoCaptcha";
            this.rbCAutoCaptcha.Size = new System.Drawing.Size(34, 21);
            this.rbCAutoCaptcha.TabIndex = 10;
            this.rbCAutoCaptcha.TabStop = true;
            this.rbCAutoCaptcha.Text = "C";
            this.rbCAutoCaptcha.UseVisualStyleBackColor = true;
            this.rbCAutoCaptcha.Visible = false;
            // 
            // rbOCR
            // 
            this.rbOCR.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbOCR.Location = new System.Drawing.Point(336, 0);
            this.rbOCR.Name = "rbOCR";
            this.rbOCR.Size = new System.Drawing.Size(34, 21);
            this.rbOCR.TabIndex = 9;
            this.rbOCR.TabStop = true;
            this.rbOCR.Text = "O";
            this.rbOCR.UseVisualStyleBackColor = true;
            this.rbOCR.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(4, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Captcha Services";
            // 
            // rbDCAutoCaptcha
            // 
            this.rbDCAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbDCAutoCaptcha.Location = new System.Drawing.Point(296, 0);
            this.rbDCAutoCaptcha.Name = "rbDCAutoCaptcha";
            this.rbDCAutoCaptcha.Size = new System.Drawing.Size(40, 21);
            this.rbDCAutoCaptcha.TabIndex = 7;
            this.rbDCAutoCaptcha.TabStop = true;
            this.rbDCAutoCaptcha.Text = "DC";
            this.rbDCAutoCaptcha.UseVisualStyleBackColor = true;
            this.rbDCAutoCaptcha.Visible = false;
            // 
            // rbDBCAutoCaptcha
            // 
            this.rbDBCAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbDBCAutoCaptcha.Location = new System.Drawing.Point(249, 0);
            this.rbDBCAutoCaptcha.Name = "rbDBCAutoCaptcha";
            this.rbDBCAutoCaptcha.Size = new System.Drawing.Size(47, 21);
            this.rbDBCAutoCaptcha.TabIndex = 6;
            this.rbDBCAutoCaptcha.TabStop = true;
            this.rbDBCAutoCaptcha.Text = "DBC";
            this.rbDBCAutoCaptcha.UseVisualStyleBackColor = true;
            this.rbDBCAutoCaptcha.Visible = false;
            // 
            // rbCPTAutoCaptcha
            // 
            this.rbCPTAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbCPTAutoCaptcha.Location = new System.Drawing.Point(201, 0);
            this.rbCPTAutoCaptcha.Name = "rbCPTAutoCaptcha";
            this.rbCPTAutoCaptcha.Size = new System.Drawing.Size(48, 21);
            this.rbCPTAutoCaptcha.TabIndex = 4;
            this.rbCPTAutoCaptcha.TabStop = true;
            this.rbCPTAutoCaptcha.Text = "CPT";
            this.rbCPTAutoCaptcha.UseVisualStyleBackColor = true;
            this.rbCPTAutoCaptcha.Visible = false;
            // 
            // rbRDAutoCaptcha
            // 
            this.rbRDAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbRDAutoCaptcha.Location = new System.Drawing.Point(158, 0);
            this.rbRDAutoCaptcha.Name = "rbRDAutoCaptcha";
            this.rbRDAutoCaptcha.Size = new System.Drawing.Size(43, 21);
            this.rbRDAutoCaptcha.TabIndex = 3;
            this.rbRDAutoCaptcha.TabStop = true;
            this.rbRDAutoCaptcha.Text = "RD";
            this.rbRDAutoCaptcha.UseVisualStyleBackColor = true;
            this.rbRDAutoCaptcha.Visible = false;
            // 
            // rbBPCAutoCaptcha
            // 
            this.rbBPCAutoCaptcha.Dock = System.Windows.Forms.DockStyle.Left;
            this.rbBPCAutoCaptcha.Location = new System.Drawing.Point(112, 0);
            this.rbBPCAutoCaptcha.Name = "rbBPCAutoCaptcha";
            this.rbBPCAutoCaptcha.Size = new System.Drawing.Size(46, 21);
            this.rbBPCAutoCaptcha.TabIndex = 2;
            this.rbBPCAutoCaptcha.TabStop = true;
            this.rbBPCAutoCaptcha.Text = "BPC";
            this.rbBPCAutoCaptcha.UseVisualStyleBackColor = true;
            this.rbBPCAutoCaptcha.Visible = false;
            // 
            // pnlCaptchaSolving
            // 
            this.pnlCaptchaSolving.BackColor = System.Drawing.Color.Transparent;
            this.pnlCaptchaSolving.Controls.Add(this.nudStartSolvingFromCaptcha);
            this.pnlCaptchaSolving.Controls.Add(this.lblStartSolvingFrom);
            this.pnlCaptchaSolving.Controls.Add(this.chkAutoCaptcha);
            this.pnlCaptchaSolving.Controls.Add(this.label5);
            this.pnlCaptchaSolving.Location = new System.Drawing.Point(0, 2);
            this.pnlCaptchaSolving.Name = "pnlCaptchaSolving";
            this.pnlCaptchaSolving.Size = new System.Drawing.Size(421, 25);
            this.pnlCaptchaSolving.TabIndex = 0;
            // 
            // nudStartSolvingFromCaptcha
            // 
            this.nudStartSolvingFromCaptcha.Location = new System.Drawing.Point(277, 3);
            this.nudStartSolvingFromCaptcha.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStartSolvingFromCaptcha.Name = "nudStartSolvingFromCaptcha";
            this.nudStartSolvingFromCaptcha.Size = new System.Drawing.Size(47, 20);
            this.nudStartSolvingFromCaptcha.TabIndex = 3;
            this.nudStartSolvingFromCaptcha.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudStartSolvingFromCaptcha.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblStartSolvingFrom
            // 
            this.lblStartSolvingFrom.AutoSize = true;
            this.lblStartSolvingFrom.BackColor = System.Drawing.Color.Transparent;
            this.lblStartSolvingFrom.Location = new System.Drawing.Point(184, 7);
            this.lblStartSolvingFrom.Name = "lblStartSolvingFrom";
            this.lblStartSolvingFrom.Size = new System.Drawing.Size(88, 13);
            this.lblStartSolvingFrom.TabIndex = 2;
            this.lblStartSolvingFrom.Text = "Start solving from";
            // 
            // chkAutoCaptcha
            // 
            this.chkAutoCaptcha.AutoSize = true;
            this.chkAutoCaptcha.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoCaptcha.Checked = true;
            this.chkAutoCaptcha.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoCaptcha.Location = new System.Drawing.Point(113, 6);
            this.chkAutoCaptcha.Name = "chkAutoCaptcha";
            this.chkAutoCaptcha.Size = new System.Drawing.Size(73, 17);
            this.chkAutoCaptcha.TabIndex = 1;
            this.chkAutoCaptcha.Text = "Automatic";
            this.chkAutoCaptcha.UseVisualStyleBackColor = false;
            this.chkAutoCaptcha.CheckStateChanged += new System.EventHandler(this.chkAutoCaptcha_CheckStateChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(5, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Captcha solving";
            // 
            // pnlSearchParameters
            // 
            this.pnlSearchParameters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearchParameters.Controls.Add(this.dtSelect);
            this.pnlSearchParameters.Controls.Add(this.mcSelectDate);
            this.pnlSearchParameters.Controls.Add(this.gvParameters);
            this.pnlSearchParameters.Location = new System.Drawing.Point(5, 3);
            this.pnlSearchParameters.Name = "pnlSearchParameters";
            this.pnlSearchParameters.Size = new System.Drawing.Size(834, 235);
            this.pnlSearchParameters.TabIndex = 4;
            // 
            // dtSelect
            // 
            this.dtSelect.CustomFormat = "HH:mm";
            this.dtSelect.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtSelect.Location = new System.Drawing.Point(417, 50);
            this.dtSelect.Name = "dtSelect";
            this.dtSelect.ShowUpDown = true;
            this.dtSelect.Size = new System.Drawing.Size(87, 20);
            this.dtSelect.TabIndex = 3;
            this.dtSelect.Value = new System.DateTime(2016, 9, 21, 16, 46, 0, 0);
            this.dtSelect.Visible = false;
            this.dtSelect.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtSelect_KeyPress);
            this.dtSelect.Leave += new System.EventHandler(this.dtSelect_Leave);
            // 
            // mcSelectDate
            // 
            this.mcSelectDate.Location = new System.Drawing.Point(178, 41);
            this.mcSelectDate.MaxSelectionCount = 1;
            this.mcSelectDate.Name = "mcSelectDate";
            this.mcSelectDate.ShowToday = false;
            this.mcSelectDate.TabIndex = 2;
            this.mcSelectDate.Visible = false;
            this.mcSelectDate.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.mcSelectDate_DateSelected);
            // 
            // gvParameters
            // 
            this.gvParameters.AllowUserToResizeRows = false;
            this.gvParameters.AutoGenerateColumns = false;
            this.gvParameters.BackgroundColor = System.Drawing.Color.White;
            this.gvParameters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvParameters.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.gvParameters.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Bought,
            this.MaxBought,
            this.quantityDataGridViewTextBoxColumn,
            this.dateTimeStringDataGridViewTextBoxColumn,
            this.EventTime,
            this.GetResaleTix,
            this.OfferName,
            this.priceLevelStringDataGridViewTextBoxColumn,
            this.exactMatchDataGridViewCheckBoxColumn,
            this.ticketTypePassswordDataGridViewTextBoxColumn,
            this.LowestPrice,
            this.TopPrice,
            this.priceMinDataGridViewTextBoxColumn,
            this.priceMaxDataGridViewTextBoxColumn,
            this.MaxToMin});
            this.gvParameters.DataSource = this.iTicketParameterBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvParameters.DefaultCellStyle = dataGridViewCellStyle3;
            this.gvParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvParameters.GridColor = System.Drawing.Color.LightGray;
            this.gvParameters.Location = new System.Drawing.Point(0, 0);
            this.gvParameters.MultiSelect = false;
            this.gvParameters.Name = "gvParameters";
            this.gvParameters.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvParameters.RowHeadersVisible = false;
            this.gvParameters.RowHeadersWidth = 20;
            this.gvParameters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvParameters.Size = new System.Drawing.Size(832, 233);
            this.gvParameters.TabIndex = 0;
            this.gvParameters.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gvParameters_CellBeginEdit);
            this.gvParameters.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvParameters_CellClick);
            this.gvParameters.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvParameters_CellEndEdit);
            this.gvParameters.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gvParameters_CellFormatting);
            this.gvParameters.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvParameters_CellLeave);
            this.gvParameters.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gvParameters_CellValidating);
            this.gvParameters.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gvParameters_UserDeletingRow);
            this.gvParameters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gvParameters_KeyDown);
            // 
            // iTicketParameterBindingSource
            // 
            this.iTicketParameterBindingSource.DataSource = typeof(Automatick.Core.ITicketParameter);
            // 
            // pnlSearchParametersOptions
            // 
            this.pnlSearchParametersOptions.BackColor = System.Drawing.Color.White;
            this.pnlSearchParametersOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearchParametersOptions.Controls.Add(this.rbDistributeInSearches);
            this.pnlSearchParametersOptions.Controls.Add(this.rbUseFoundOnFirstAttempt);
            this.pnlSearchParametersOptions.Controls.Add(this.rbUseAvailableParameters);
            this.pnlSearchParametersOptions.Location = new System.Drawing.Point(5, 242);
            this.pnlSearchParametersOptions.Name = "pnlSearchParametersOptions";
            this.pnlSearchParametersOptions.Size = new System.Drawing.Size(458, 88);
            this.pnlSearchParametersOptions.TabIndex = 0;
            // 
            // rbDistributeInSearches
            // 
            this.rbDistributeInSearches.AutoSize = true;
            this.rbDistributeInSearches.Location = new System.Drawing.Point(10, 6);
            this.rbDistributeInSearches.Name = "rbDistributeInSearches";
            this.rbDistributeInSearches.Size = new System.Drawing.Size(285, 17);
            this.rbDistributeInSearches.TabIndex = 0;
            this.rbDistributeInSearches.TabStop = true;
            this.rbDistributeInSearches.Text = "Distribute parameters according to number of searches.";
            this.rbDistributeInSearches.UseVisualStyleBackColor = true;
            // 
            // rbUseFoundOnFirstAttempt
            // 
            this.rbUseFoundOnFirstAttempt.AutoSize = true;
            this.rbUseFoundOnFirstAttempt.Location = new System.Drawing.Point(10, 52);
            this.rbUseFoundOnFirstAttempt.Name = "rbUseFoundOnFirstAttempt";
            this.rbUseFoundOnFirstAttempt.Size = new System.Drawing.Size(324, 17);
            this.rbUseFoundOnFirstAttempt.TabIndex = 2;
            this.rbUseFoundOnFirstAttempt.TabStop = true;
            this.rbUseFoundOnFirstAttempt.Text = "Use only found parameters on first attempt for searching tickets.";
            this.rbUseFoundOnFirstAttempt.UseVisualStyleBackColor = true;
            // 
            // rbUseAvailableParameters
            // 
            this.rbUseAvailableParameters.AutoSize = true;
            this.rbUseAvailableParameters.Location = new System.Drawing.Point(10, 29);
            this.rbUseAvailableParameters.Name = "rbUseAvailableParameters";
            this.rbUseAvailableParameters.Size = new System.Drawing.Size(325, 17);
            this.rbUseAvailableParameters.TabIndex = 1;
            this.rbUseAvailableParameters.TabStop = true;
            this.rbUseAvailableParameters.Text = "Use available parameters in event page while searching tickets.";
            this.rbUseAvailableParameters.UseVisualStyleBackColor = true;
            // 
            // pnlParamters
            // 
            this.pnlParamters.BackColor = System.Drawing.Color.White;
            this.pnlParamters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlParamters.Controls.Add(this.chkMobile);
            this.pnlParamters.Controls.Add(this.chkWeb);
            this.pnlParamters.Controls.Add(this.label4);
            this.pnlParamters.Controls.Add(this.chkUseProxies);
            this.pnlParamters.Controls.Add(this.nudNoOfSearches);
            this.pnlParamters.Controls.Add(this.label3);
            this.pnlParamters.Controls.Add(this.chkRandomDelay);
            this.pnlParamters.Controls.Add(this.nudResetSearchDelay);
            this.pnlParamters.Controls.Add(this.Delay);
            this.pnlParamters.Controls.Add(this.nudStartUsingProxiesFrom);
            this.pnlParamters.Controls.Add(this.label2);
            this.pnlParamters.Controls.Add(this.lblStartUsingProxiesFrom);
            this.pnlParamters.Location = new System.Drawing.Point(469, 242);
            this.pnlParamters.Name = "pnlParamters";
            this.pnlParamters.Size = new System.Drawing.Size(370, 88);
            this.pnlParamters.TabIndex = 1;
            // 
            // chkMobile
            // 
            this.chkMobile.AutoSize = true;
            this.chkMobile.BackColor = System.Drawing.Color.Transparent;
            this.chkMobile.Location = new System.Drawing.Point(249, 24);
            this.chkMobile.Name = "chkMobile";
            this.chkMobile.Size = new System.Drawing.Size(57, 17);
            this.chkMobile.TabIndex = 10;
            this.chkMobile.Text = "Mobile";
            this.chkMobile.UseVisualStyleBackColor = false;
            this.chkMobile.Visible = false;
            // 
            // chkWeb
            // 
            this.chkWeb.AutoSize = true;
            this.chkWeb.BackColor = System.Drawing.Color.Transparent;
            this.chkWeb.Location = new System.Drawing.Point(249, 4);
            this.chkWeb.Name = "chkWeb";
            this.chkWeb.Size = new System.Drawing.Size(49, 17);
            this.chkWeb.TabIndex = 9;
            this.chkWeb.Text = "Web";
            this.chkWeb.UseVisualStyleBackColor = false;
            this.chkWeb.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Delay For Auto Buy";
            // 
            // chkUseProxies
            // 
            this.chkUseProxies.AutoSize = true;
            this.chkUseProxies.BackColor = System.Drawing.Color.Transparent;
            this.chkUseProxies.Location = new System.Drawing.Point(249, 64);
            this.chkUseProxies.Name = "chkUseProxies";
            this.chkUseProxies.Size = new System.Drawing.Size(81, 17);
            this.chkUseProxies.TabIndex = 7;
            this.chkUseProxies.Text = "Use proxies";
            this.chkUseProxies.UseVisualStyleBackColor = false;
            this.chkUseProxies.CheckStateChanged += new System.EventHandler(this.chkUseProxies_CheckStateChanged);
            // 
            // nudNoOfSearches
            // 
            this.nudNoOfSearches.Location = new System.Drawing.Point(196, 1);
            this.nudNoOfSearches.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNoOfSearches.Name = "nudNoOfSearches";
            this.nudNoOfSearches.Size = new System.Drawing.Size(47, 20);
            this.nudNoOfSearches.TabIndex = 3;
            this.nudNoOfSearches.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudNoOfSearches.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(3, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Reset search delay (in seconds)";
            // 
            // chkRandomDelay
            // 
            this.chkRandomDelay.AutoSize = true;
            this.chkRandomDelay.BackColor = System.Drawing.Color.Transparent;
            this.chkRandomDelay.Location = new System.Drawing.Point(249, 44);
            this.chkRandomDelay.Name = "chkRandomDelay";
            this.chkRandomDelay.Size = new System.Drawing.Size(73, 17);
            this.chkRandomDelay.TabIndex = 6;
            this.chkRandomDelay.Text = "Randomly";
            this.chkRandomDelay.UseVisualStyleBackColor = false;
            // 
            // nudResetSearchDelay
            // 
            this.nudResetSearchDelay.Location = new System.Drawing.Point(196, 22);
            this.nudResetSearchDelay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudResetSearchDelay.Name = "nudResetSearchDelay";
            this.nudResetSearchDelay.Size = new System.Drawing.Size(47, 20);
            this.nudResetSearchDelay.TabIndex = 4;
            this.nudResetSearchDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudResetSearchDelay.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // Delay
            // 
            this.Delay.Location = new System.Drawing.Point(196, 64);
            this.Delay.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.Delay.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.Delay.Name = "Delay";
            this.Delay.Size = new System.Drawing.Size(47, 20);
            this.Delay.TabIndex = 5;
            this.Delay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Delay.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // nudStartUsingProxiesFrom
            // 
            this.nudStartUsingProxiesFrom.Location = new System.Drawing.Point(196, 43);
            this.nudStartUsingProxiesFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStartUsingProxiesFrom.Name = "nudStartUsingProxiesFrom";
            this.nudStartUsingProxiesFrom.Size = new System.Drawing.Size(47, 20);
            this.nudStartUsingProxiesFrom.TabIndex = 5;
            this.nudStartUsingProxiesFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudStartUsingProxiesFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Number of searches";
            // 
            // lblStartUsingProxiesFrom
            // 
            this.lblStartUsingProxiesFrom.AutoSize = true;
            this.lblStartUsingProxiesFrom.BackColor = System.Drawing.Color.Transparent;
            this.lblStartUsingProxiesFrom.Location = new System.Drawing.Point(3, 45);
            this.lblStartUsingProxiesFrom.Name = "lblStartUsingProxiesFrom";
            this.lblStartUsingProxiesFrom.Size = new System.Drawing.Size(161, 13);
            this.lblStartUsingProxiesFrom.TabIndex = 2;
            this.lblStartUsingProxiesFrom.Text = "Start using proxies from search #";
            // 
            // tabPageMoreParameters
            // 
            this.tabPageMoreParameters.BackColor = System.Drawing.Color.White;
            this.tabPageMoreParameters.Controls.Add(this.panel2);
            this.tabPageMoreParameters.Controls.Add(this.panel4);
            this.tabPageMoreParameters.Controls.Add(this.panel3);
            this.tabPageMoreParameters.Controls.Add(this.pnlDeliveryOptions);
            this.tabPageMoreParameters.Location = new System.Drawing.Point(2, 25);
            this.tabPageMoreParameters.Name = "tabPageMoreParameters";
            this.tabPageMoreParameters.Size = new System.Drawing.Size(842, 407);
            this.tabPageMoreParameters.TabBackColor = System.Drawing.Color.White;
            this.tabPageMoreParameters.TabBackColorSelected = System.Drawing.Color.White;
            this.tabPageMoreParameters.TabIndex = 1;
            this.tabPageMoreParameters.Text = "More Parameters";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.txtNotes);
            this.panel2.Location = new System.Drawing.Point(383, 333);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(456, 71);
            this.panel2.TabIndex = 267;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.White;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(3, 5);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(38, 13);
            this.label17.TabIndex = 2;
            this.label17.Text = "Notes:";
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.BackColor = System.Drawing.Color.White;
            this.txtNotes.Location = new System.Drawing.Point(6, 21);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(442, 44);
            this.txtNotes.TabIndex = 261;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnManageAccounts);
            this.panel4.Controls.Add(this.gvAccounts);
            this.panel4.Controls.Add(this.rbSelectAccountList);
            this.panel4.Controls.Add(this.rbSelectAccountAutoBuying);
            this.panel4.Controls.Add(this.gvFindingCriteria);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Location = new System.Drawing.Point(383, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(456, 324);
            this.panel4.TabIndex = 266;
            // 
            // btnManageAccounts
            // 
            this.btnManageAccounts.ActiveLinkColor = System.Drawing.Color.SteelBlue;
            this.btnManageAccounts.AutoSize = true;
            this.btnManageAccounts.LinkColor = System.Drawing.Color.SteelBlue;
            this.btnManageAccounts.Location = new System.Drawing.Point(349, 163);
            this.btnManageAccounts.Name = "btnManageAccounts";
            this.btnManageAccounts.Size = new System.Drawing.Size(99, 13);
            this.btnManageAccounts.TabIndex = 289;
            this.btnManageAccounts.TabStop = true;
            this.btnManageAccounts.Text = "(Manage accounts)";
            this.btnManageAccounts.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnManageAccounts_LinkClicked);
            // 
            // gvAccounts
            // 
            this.gvAccounts.AllowUserToAddRows = false;
            this.gvAccounts.AllowUserToDeleteRows = false;
            this.gvAccounts.AllowUserToResizeRows = false;
            this.gvAccounts.AutoGenerateColumns = false;
            this.gvAccounts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvAccounts.BackgroundColor = System.Drawing.Color.White;
            this.gvAccounts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvAccounts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAccounts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ifSelectedDataGridViewCheckBoxColumn1,
            this.accountNameDataGridViewTextBoxColumn,
            this.BuyingLimit});
            this.gvAccounts.DataSource = this.iTicketAccountBindingSource;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvAccounts.DefaultCellStyle = dataGridViewCellStyle5;
            this.gvAccounts.GridColor = System.Drawing.Color.LightGray;
            this.gvAccounts.Location = new System.Drawing.Point(6, 184);
            this.gvAccounts.MultiSelect = false;
            this.gvAccounts.Name = "gvAccounts";
            this.gvAccounts.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvAccounts.RowHeadersVisible = false;
            this.gvAccounts.RowHeadersWidth = 20;
            this.gvAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvAccounts.Size = new System.Drawing.Size(442, 135);
            this.gvAccounts.TabIndex = 13;
            this.gvAccounts.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gvAccounts_CellValidating);
            // 
            // ifSelectedDataGridViewCheckBoxColumn1
            // 
            this.ifSelectedDataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ifSelectedDataGridViewCheckBoxColumn1.DataPropertyName = "IfSelected";
            this.ifSelectedDataGridViewCheckBoxColumn1.HeaderText = "";
            this.ifSelectedDataGridViewCheckBoxColumn1.MinimumWidth = 35;
            this.ifSelectedDataGridViewCheckBoxColumn1.Name = "ifSelectedDataGridViewCheckBoxColumn1";
            this.ifSelectedDataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ifSelectedDataGridViewCheckBoxColumn1.Width = 35;
            // 
            // accountNameDataGridViewTextBoxColumn
            // 
            this.accountNameDataGridViewTextBoxColumn.DataPropertyName = "AccountName";
            this.accountNameDataGridViewTextBoxColumn.FillWeight = 38F;
            this.accountNameDataGridViewTextBoxColumn.HeaderText = "Account name";
            this.accountNameDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.accountNameDataGridViewTextBoxColumn.Name = "accountNameDataGridViewTextBoxColumn";
            this.accountNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // BuyingLimit
            // 
            this.BuyingLimit.DataPropertyName = "BuyingLimit";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.BuyingLimit.DefaultCellStyle = dataGridViewCellStyle4;
            this.BuyingLimit.FillWeight = 24F;
            this.BuyingLimit.HeaderText = "Buy limit";
            this.BuyingLimit.MinimumWidth = 50;
            this.BuyingLimit.Name = "BuyingLimit";
            this.BuyingLimit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // iTicketAccountBindingSource
            // 
            this.iTicketAccountBindingSource.DataSource = typeof(Automatick.Core.ITicketAccount);
            // 
            // rbSelectAccountList
            // 
            this.rbSelectAccountList.AutoSize = true;
            this.rbSelectAccountList.Location = new System.Drawing.Point(6, 161);
            this.rbSelectAccountList.Name = "rbSelectAccountList";
            this.rbSelectAccountList.Size = new System.Drawing.Size(158, 17);
            this.rbSelectAccountList.TabIndex = 12;
            this.rbSelectAccountList.TabStop = true;
            this.rbSelectAccountList.Text = "Select accounts from the list";
            this.rbSelectAccountList.UseVisualStyleBackColor = true;
            // 
            // rbSelectAccountAutoBuying
            // 
            this.rbSelectAccountAutoBuying.AutoSize = true;
            this.rbSelectAccountAutoBuying.Location = new System.Drawing.Point(6, 138);
            this.rbSelectAccountAutoBuying.Name = "rbSelectAccountAutoBuying";
            this.rbSelectAccountAutoBuying.Size = new System.Drawing.Size(187, 17);
            this.rbSelectAccountAutoBuying.TabIndex = 11;
            this.rbSelectAccountAutoBuying.TabStop = true;
            this.rbSelectAccountAutoBuying.Text = "Select account during auto buying";
            this.rbSelectAccountAutoBuying.UseVisualStyleBackColor = true;
            this.rbSelectAccountAutoBuying.CheckedChanged += new System.EventHandler(this.rbSelectAccountAutoBuying_CheckedChanged);
            // 
            // gvFindingCriteria
            // 
            this.gvFindingCriteria.AllowUserToResizeRows = false;
            this.gvFindingCriteria.AutoGenerateColumns = false;
            this.gvFindingCriteria.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvFindingCriteria.BackgroundColor = System.Drawing.Color.White;
            this.gvFindingCriteria.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvFindingCriteria.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.gvFindingCriteria.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvFindingCriteria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvFindingCriteria.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rowFromDataGridViewTextBoxColumn,
            this.rowToDataGridViewTextBoxColumn,
            this.sectionDataGridViewTextBoxColumn1});
            this.gvFindingCriteria.DataSource = this.iTicketFoundCriteriaBindingSource;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvFindingCriteria.DefaultCellStyle = dataGridViewCellStyle6;
            this.gvFindingCriteria.GridColor = System.Drawing.Color.LightGray;
            this.gvFindingCriteria.Location = new System.Drawing.Point(6, 26);
            this.gvFindingCriteria.MultiSelect = false;
            this.gvFindingCriteria.Name = "gvFindingCriteria";
            this.gvFindingCriteria.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvFindingCriteria.RowHeadersVisible = false;
            this.gvFindingCriteria.RowHeadersWidth = 20;
            this.gvFindingCriteria.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvFindingCriteria.Size = new System.Drawing.Size(442, 102);
            this.gvFindingCriteria.TabIndex = 10;
            // 
            // rowFromDataGridViewTextBoxColumn
            // 
            this.rowFromDataGridViewTextBoxColumn.DataPropertyName = "RowFrom";
            this.rowFromDataGridViewTextBoxColumn.FillWeight = 25F;
            this.rowFromDataGridViewTextBoxColumn.HeaderText = "Row from";
            this.rowFromDataGridViewTextBoxColumn.MinimumWidth = 85;
            this.rowFromDataGridViewTextBoxColumn.Name = "rowFromDataGridViewTextBoxColumn";
            // 
            // rowToDataGridViewTextBoxColumn
            // 
            this.rowToDataGridViewTextBoxColumn.DataPropertyName = "RowTo";
            this.rowToDataGridViewTextBoxColumn.FillWeight = 25F;
            this.rowToDataGridViewTextBoxColumn.HeaderText = "Row to";
            this.rowToDataGridViewTextBoxColumn.MinimumWidth = 85;
            this.rowToDataGridViewTextBoxColumn.Name = "rowToDataGridViewTextBoxColumn";
            // 
            // sectionDataGridViewTextBoxColumn1
            // 
            this.sectionDataGridViewTextBoxColumn1.DataPropertyName = "Section";
            this.sectionDataGridViewTextBoxColumn1.FillWeight = 30F;
            this.sectionDataGridViewTextBoxColumn1.HeaderText = "Section";
            this.sectionDataGridViewTextBoxColumn1.MinimumWidth = 105;
            this.sectionDataGridViewTextBoxColumn1.Name = "sectionDataGridViewTextBoxColumn1";
            // 
            // iTicketFoundCriteriaBindingSource
            // 
            this.iTicketFoundCriteriaBindingSource.DataSource = typeof(Automatick.Core.ITicketFoundCriteria);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.White;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(3, 5);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(108, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Ticket finding criteria:";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.lblMinutes);
            this.panel3.Controls.Add(this.nudScheduleRunningTime);
            this.panel3.Controls.Add(this.chkSchedule);
            this.panel3.Controls.Add(this.lblRunningTime);
            this.panel3.Controls.Add(this.dtpScheduleDateTime);
            this.panel3.Controls.Add(this.label18);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Location = new System.Drawing.Point(3, 333);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(374, 71);
            this.panel3.TabIndex = 265;
            // 
            // lblMinutes
            // 
            this.lblMinutes.AutoSize = true;
            this.lblMinutes.Location = new System.Drawing.Point(138, 47);
            this.lblMinutes.Name = "lblMinutes";
            this.lblMinutes.Size = new System.Drawing.Size(49, 13);
            this.lblMinutes.TabIndex = 183;
            this.lblMinutes.Text = "(minutes)";
            // 
            // nudScheduleRunningTime
            // 
            this.nudScheduleRunningTime.Location = new System.Drawing.Point(85, 45);
            this.nudScheduleRunningTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudScheduleRunningTime.Name = "nudScheduleRunningTime";
            this.nudScheduleRunningTime.Size = new System.Drawing.Size(47, 20);
            this.nudScheduleRunningTime.TabIndex = 182;
            this.nudScheduleRunningTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudScheduleRunningTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkSchedule
            // 
            this.chkSchedule.AutoSize = true;
            this.chkSchedule.Location = new System.Drawing.Point(66, 23);
            this.chkSchedule.Name = "chkSchedule";
            this.chkSchedule.Size = new System.Drawing.Size(15, 14);
            this.chkSchedule.TabIndex = 181;
            this.chkSchedule.UseVisualStyleBackColor = true;
            this.chkSchedule.CheckStateChanged += new System.EventHandler(this.chkSchedule_CheckStateChanged);
            // 
            // lblRunningTime
            // 
            this.lblRunningTime.AutoSize = true;
            this.lblRunningTime.Location = new System.Drawing.Point(6, 47);
            this.lblRunningTime.Name = "lblRunningTime";
            this.lblRunningTime.Size = new System.Drawing.Size(72, 13);
            this.lblRunningTime.TabIndex = 180;
            this.lblRunningTime.Text = "Running time:";
            // 
            // dtpScheduleDateTime
            // 
            this.dtpScheduleDateTime.CustomFormat = "dddd, dd MMMM yyyy \'at\' hh:mm:ss tt";
            this.dtpScheduleDateTime.Enabled = false;
            this.dtpScheduleDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpScheduleDateTime.Location = new System.Drawing.Point(85, 20);
            this.dtpScheduleDateTime.Name = "dtpScheduleDateTime";
            this.dtpScheduleDateTime.Size = new System.Drawing.Size(277, 20);
            this.dtpScheduleDateTime.TabIndex = 177;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 24);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(54, 13);
            this.label18.TabIndex = 179;
            this.label18.Text = "Start time:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.White;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(3, 5);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Schedule:";
            // 
            // pnlDeliveryOptions
            // 
            this.pnlDeliveryOptions.BackColor = System.Drawing.Color.White;
            this.pnlDeliveryOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDeliveryOptions.Controls.Add(this.btnManageDeliveryOptions);
            this.pnlDeliveryOptions.Controls.Add(this.gvDeliveryOptions);
            this.pnlDeliveryOptions.Controls.Add(this.txtDeliveryCountry);
            this.pnlDeliveryOptions.Controls.Add(this.txtDeliveryOption);
            this.pnlDeliveryOptions.Controls.Add(this.lblDeliveryCountry);
            this.pnlDeliveryOptions.Controls.Add(this.lblDeliveryOption);
            this.pnlDeliveryOptions.Controls.Add(this.rbSelectDeliveryOptionList);
            this.pnlDeliveryOptions.Controls.Add(this.rbSelectDeliveryOptionAutoBuying);
            this.pnlDeliveryOptions.Controls.Add(this.label11);
            this.pnlDeliveryOptions.Location = new System.Drawing.Point(3, 3);
            this.pnlDeliveryOptions.Name = "pnlDeliveryOptions";
            this.pnlDeliveryOptions.Size = new System.Drawing.Size(374, 324);
            this.pnlDeliveryOptions.TabIndex = 263;
            // 
            // btnManageDeliveryOptions
            // 
            this.btnManageDeliveryOptions.ActiveLinkColor = System.Drawing.Color.SteelBlue;
            this.btnManageDeliveryOptions.AutoSize = true;
            this.btnManageDeliveryOptions.LinkColor = System.Drawing.Color.SteelBlue;
            this.btnManageDeliveryOptions.Location = new System.Drawing.Point(238, 113);
            this.btnManageDeliveryOptions.Name = "btnManageDeliveryOptions";
            this.btnManageDeliveryOptions.Size = new System.Drawing.Size(128, 13);
            this.btnManageDeliveryOptions.TabIndex = 288;
            this.btnManageDeliveryOptions.TabStop = true;
            this.btnManageDeliveryOptions.Text = "(Manage delivery options)";
            this.btnManageDeliveryOptions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnManageDeliveryOptions_LinkClicked);
            // 
            // gvDeliveryOptions
            // 
            this.gvDeliveryOptions.AllowUserToAddRows = false;
            this.gvDeliveryOptions.AllowUserToDeleteRows = false;
            this.gvDeliveryOptions.AllowUserToResizeRows = false;
            this.gvDeliveryOptions.AutoGenerateColumns = false;
            this.gvDeliveryOptions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvDeliveryOptions.BackgroundColor = System.Drawing.Color.White;
            this.gvDeliveryOptions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvDeliveryOptions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvDeliveryOptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDeliveryOptions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ifSelectedDataGridViewCheckBoxColumn,
            this.deliveryOptionDataGridViewTextBoxColumn,
            this.deliveryCountryDataGridViewTextBoxColumn});
            this.gvDeliveryOptions.DataSource = this.iTicketDeliveryOptionBindingSource;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvDeliveryOptions.DefaultCellStyle = dataGridViewCellStyle7;
            this.gvDeliveryOptions.GridColor = System.Drawing.Color.LightGray;
            this.gvDeliveryOptions.Location = new System.Drawing.Point(6, 134);
            this.gvDeliveryOptions.MultiSelect = false;
            this.gvDeliveryOptions.Name = "gvDeliveryOptions";
            this.gvDeliveryOptions.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvDeliveryOptions.RowHeadersVisible = false;
            this.gvDeliveryOptions.RowHeadersWidth = 20;
            this.gvDeliveryOptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvDeliveryOptions.Size = new System.Drawing.Size(360, 185);
            this.gvDeliveryOptions.TabIndex = 9;
            // 
            // ifSelectedDataGridViewCheckBoxColumn
            // 
            this.ifSelectedDataGridViewCheckBoxColumn.DataPropertyName = "IfSelected";
            this.ifSelectedDataGridViewCheckBoxColumn.FillWeight = 10F;
            this.ifSelectedDataGridViewCheckBoxColumn.HeaderText = "";
            this.ifSelectedDataGridViewCheckBoxColumn.MinimumWidth = 35;
            this.ifSelectedDataGridViewCheckBoxColumn.Name = "ifSelectedDataGridViewCheckBoxColumn";
            // 
            // deliveryOptionDataGridViewTextBoxColumn
            // 
            this.deliveryOptionDataGridViewTextBoxColumn.DataPropertyName = "DeliveryOption";
            this.deliveryOptionDataGridViewTextBoxColumn.FillWeight = 45F;
            this.deliveryOptionDataGridViewTextBoxColumn.HeaderText = "Delivery option";
            this.deliveryOptionDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.deliveryOptionDataGridViewTextBoxColumn.Name = "deliveryOptionDataGridViewTextBoxColumn";
            this.deliveryOptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // deliveryCountryDataGridViewTextBoxColumn
            // 
            this.deliveryCountryDataGridViewTextBoxColumn.DataPropertyName = "DeliveryCountry";
            this.deliveryCountryDataGridViewTextBoxColumn.FillWeight = 45F;
            this.deliveryCountryDataGridViewTextBoxColumn.HeaderText = "Delivery country";
            this.deliveryCountryDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.deliveryCountryDataGridViewTextBoxColumn.Name = "deliveryCountryDataGridViewTextBoxColumn";
            this.deliveryCountryDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // iTicketDeliveryOptionBindingSource
            // 
            this.iTicketDeliveryOptionBindingSource.DataSource = typeof(Automatick.Core.ITicketDeliveryOption);
            // 
            // txtDeliveryCountry
            // 
            this.txtDeliveryCountry.BackColor = System.Drawing.Color.White;
            this.txtDeliveryCountry.Location = new System.Drawing.Point(94, 79);
            this.txtDeliveryCountry.Name = "txtDeliveryCountry";
            this.txtDeliveryCountry.Size = new System.Drawing.Size(200, 20);
            this.txtDeliveryCountry.TabIndex = 8;
            this.txtDeliveryCountry.Visible = false;
            // 
            // txtDeliveryOption
            // 
            this.txtDeliveryOption.BackColor = System.Drawing.Color.White;
            this.txtDeliveryOption.Location = new System.Drawing.Point(94, 53);
            this.txtDeliveryOption.Name = "txtDeliveryOption";
            this.txtDeliveryOption.Size = new System.Drawing.Size(200, 20);
            this.txtDeliveryOption.TabIndex = 7;
            // 
            // lblDeliveryCountry
            // 
            this.lblDeliveryCountry.AutoSize = true;
            this.lblDeliveryCountry.BackColor = System.Drawing.Color.White;
            this.lblDeliveryCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeliveryCountry.Location = new System.Drawing.Point(3, 82);
            this.lblDeliveryCountry.Name = "lblDeliveryCountry";
            this.lblDeliveryCountry.Size = new System.Drawing.Size(86, 13);
            this.lblDeliveryCountry.TabIndex = 6;
            this.lblDeliveryCountry.Text = "Delivery country:";
            this.lblDeliveryCountry.Visible = false;
            // 
            // lblDeliveryOption
            // 
            this.lblDeliveryOption.AutoSize = true;
            this.lblDeliveryOption.BackColor = System.Drawing.Color.White;
            this.lblDeliveryOption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeliveryOption.Location = new System.Drawing.Point(3, 56);
            this.lblDeliveryOption.Name = "lblDeliveryOption";
            this.lblDeliveryOption.Size = new System.Drawing.Size(80, 13);
            this.lblDeliveryOption.TabIndex = 5;
            this.lblDeliveryOption.Text = "Delivery option:";
            // 
            // rbSelectDeliveryOptionList
            // 
            this.rbSelectDeliveryOptionList.AutoSize = true;
            this.rbSelectDeliveryOptionList.Location = new System.Drawing.Point(6, 111);
            this.rbSelectDeliveryOptionList.Name = "rbSelectDeliveryOptionList";
            this.rbSelectDeliveryOptionList.Size = new System.Drawing.Size(150, 17);
            this.rbSelectDeliveryOptionList.TabIndex = 4;
            this.rbSelectDeliveryOptionList.TabStop = true;
            this.rbSelectDeliveryOptionList.Text = "Select delivery from the list";
            this.rbSelectDeliveryOptionList.UseVisualStyleBackColor = true;
            // 
            // rbSelectDeliveryOptionAutoBuying
            // 
            this.rbSelectDeliveryOptionAutoBuying.AutoSize = true;
            this.rbSelectDeliveryOptionAutoBuying.Location = new System.Drawing.Point(6, 26);
            this.rbSelectDeliveryOptionAutoBuying.Name = "rbSelectDeliveryOptionAutoBuying";
            this.rbSelectDeliveryOptionAutoBuying.Size = new System.Drawing.Size(288, 17);
            this.rbSelectDeliveryOptionAutoBuying.TabIndex = 3;
            this.rbSelectDeliveryOptionAutoBuying.TabStop = true;
            this.rbSelectDeliveryOptionAutoBuying.Text = "Select delivery option during auto buying (First time only)";
            this.rbSelectDeliveryOptionAutoBuying.UseVisualStyleBackColor = true;
            this.rbSelectDeliveryOptionAutoBuying.CheckedChanged += new System.EventHandler(this.rbSelectDeliveryOptionAutoBuying_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Delivery options:";
            // 
            // cbGroup
            // 
            this.cbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGroup.FormattingEnabled = true;
            this.cbGroup.Location = new System.Drawing.Point(460, 46);
            this.cbGroup.Name = "cbGroup";
            this.cbGroup.Size = new System.Drawing.Size(135, 21);
            this.cbGroup.TabIndex = 2;
            // 
            // btnGroup
            // 
            this.btnGroup.ActiveLinkColor = System.Drawing.Color.SteelBlue;
            this.btnGroup.AutoSize = true;
            this.btnGroup.LinkColor = System.Drawing.Color.CornflowerBlue;
            this.btnGroup.Location = new System.Drawing.Point(415, 50);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Size = new System.Drawing.Size(39, 13);
            this.btnGroup.TabIndex = 288;
            this.btnGroup.TabStop = true;
            this.btnGroup.Text = "Group:";
            this.btnGroup.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnGroup_LinkClicked);
            // 
            // chkPersistSession
            // 
            this.chkPersistSession.AutoSize = true;
            this.chkPersistSession.BackColor = System.Drawing.Color.Transparent;
            this.chkPersistSession.Location = new System.Drawing.Point(608, 72);
            this.chkPersistSession.Name = "chkPersistSession";
            this.chkPersistSession.Size = new System.Drawing.Size(168, 17);
            this.chkPersistSession.TabIndex = 289;
            this.chkPersistSession.Text = "Persist session in each search";
            this.chkPersistSession.UseVisualStyleBackColor = false;
            this.chkPersistSession.Visible = false;
            // 
            // AXSByPassWaitingRoom
            // 
            this.AXSByPassWaitingRoom.AutoSize = true;
            this.AXSByPassWaitingRoom.BackColor = System.Drawing.Color.Transparent;
            this.AXSByPassWaitingRoom.Location = new System.Drawing.Point(472, 72);
            this.AXSByPassWaitingRoom.Name = "AXSByPassWaitingRoom";
            this.AXSByPassWaitingRoom.Size = new System.Drawing.Size(130, 17);
            this.AXSByPassWaitingRoom.TabIndex = 289;
            this.AXSByPassWaitingRoom.Text = "Bypass Waiting Room";
            this.AXSByPassWaitingRoom.UseVisualStyleBackColor = false;
            this.AXSByPassWaitingRoom.Visible = false;
            // 
            // chkIsWaiting
            // 
            this.chkIsWaiting.AutoSize = true;
            this.chkIsWaiting.Location = new System.Drawing.Point(608, 50);
            this.chkIsWaiting.Name = "chkIsWaiting";
            this.chkIsWaiting.Size = new System.Drawing.Size(104, 17);
            this.chkIsWaiting.TabIndex = 290;
            this.chkIsWaiting.Text = "Is Waiting Event";
            this.chkIsWaiting.UseVisualStyleBackColor = true;
            // 
            // chkUseProxiesInCaptchaSource
            // 
            this.chkUseProxiesInCaptchaSource.AutoSize = true;
            this.chkUseProxiesInCaptchaSource.BackColor = System.Drawing.Color.Transparent;
            this.chkUseProxiesInCaptchaSource.Location = new System.Drawing.Point(217, 53);
            this.chkUseProxiesInCaptchaSource.Name = "chkUseProxiesInCaptchaSource";
            this.chkUseProxiesInCaptchaSource.Size = new System.Drawing.Size(126, 17);
            this.chkUseProxiesInCaptchaSource.TabIndex = 8;
            this.chkUseProxiesInCaptchaSource.Text = "Use proxy in captcha";
            this.chkUseProxiesInCaptchaSource.UseVisualStyleBackColor = false;
            // 
            // Bought
            // 
            this.Bought.DataPropertyName = "Bought";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.Bought.DefaultCellStyle = dataGridViewCellStyle1;
            this.Bought.HeaderText = "Bought";
            this.Bought.MinimumWidth = 50;
            this.Bought.Name = "Bought";
            this.Bought.ReadOnly = true;
            this.Bought.Width = 50;
            // 
            // MaxBought
            // 
            this.MaxBought.DataPropertyName = "MaxBought";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Format = "N0";
            this.MaxBought.DefaultCellStyle = dataGridViewCellStyle2;
            this.MaxBought.HeaderText = "Max. buy";
            this.MaxBought.MinimumWidth = 75;
            this.MaxBought.Name = "MaxBought";
            this.MaxBought.Width = 75;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.MinimumWidth = 60;
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.Width = 60;
            // 
            // dateTimeStringDataGridViewTextBoxColumn
            // 
            this.dateTimeStringDataGridViewTextBoxColumn.DataPropertyName = "DateTimeString";
            this.dateTimeStringDataGridViewTextBoxColumn.HeaderText = "Date/search string";
            this.dateTimeStringDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.dateTimeStringDataGridViewTextBoxColumn.Name = "dateTimeStringDataGridViewTextBoxColumn";
            this.dateTimeStringDataGridViewTextBoxColumn.ToolTipText = "mm/dd/yy";
            this.dateTimeStringDataGridViewTextBoxColumn.Width = 120;
            // 
            // EventTime
            // 
            this.EventTime.DataPropertyName = "EventTime";
            this.EventTime.HeaderText = "EventTime";
            this.EventTime.Name = "EventTime";
            // 
            // GetResaleTix
            // 
            this.GetResaleTix.DataPropertyName = "GetResaleTix";
            this.GetResaleTix.HeaderText = "ResaleTix";
            this.GetResaleTix.Name = "GetResaleTix";
            this.GetResaleTix.Width = 60;
            // 
            // OfferName
            // 
            this.OfferName.DataPropertyName = "OfferName";
            this.OfferName.HeaderText = "OfferName";
            this.OfferName.Name = "OfferName";
            // 
            // priceLevelStringDataGridViewTextBoxColumn
            // 
            this.priceLevelStringDataGridViewTextBoxColumn.DataPropertyName = "PriceLevelString";
            this.priceLevelStringDataGridViewTextBoxColumn.HeaderText = "Price level/search string";
            this.priceLevelStringDataGridViewTextBoxColumn.MinimumWidth = 130;
            this.priceLevelStringDataGridViewTextBoxColumn.Name = "priceLevelStringDataGridViewTextBoxColumn";
            this.priceLevelStringDataGridViewTextBoxColumn.Width = 130;
            // 
            // exactMatchDataGridViewCheckBoxColumn
            // 
            this.exactMatchDataGridViewCheckBoxColumn.DataPropertyName = "ExactMatch";
            this.exactMatchDataGridViewCheckBoxColumn.HeaderText = "Exact match";
            this.exactMatchDataGridViewCheckBoxColumn.MinimumWidth = 80;
            this.exactMatchDataGridViewCheckBoxColumn.Name = "exactMatchDataGridViewCheckBoxColumn";
            this.exactMatchDataGridViewCheckBoxColumn.Width = 80;
            // 
            // ticketTypePassswordDataGridViewTextBoxColumn
            // 
            this.ticketTypePassswordDataGridViewTextBoxColumn.DataPropertyName = "TicketTypePasssword";
            this.ticketTypePassswordDataGridViewTextBoxColumn.HeaderText = "Password/Code";
            this.ticketTypePassswordDataGridViewTextBoxColumn.MinimumWidth = 90;
            this.ticketTypePassswordDataGridViewTextBoxColumn.Name = "ticketTypePassswordDataGridViewTextBoxColumn";
            this.ticketTypePassswordDataGridViewTextBoxColumn.Width = 90;
            // 
            // LowestPrice
            // 
            this.LowestPrice.DataPropertyName = "LowestPrice";
            this.LowestPrice.HeaderText = "LowestPrice";
            this.LowestPrice.Name = "LowestPrice";
            // 
            // TopPrice
            // 
            this.TopPrice.DataPropertyName = "TopPrice";
            this.TopPrice.HeaderText = "TopPrice";
            this.TopPrice.Name = "TopPrice";
            // 
            // priceMinDataGridViewTextBoxColumn
            // 
            this.priceMinDataGridViewTextBoxColumn.DataPropertyName = "PriceMin";
            this.priceMinDataGridViewTextBoxColumn.HeaderText = "Min. price";
            this.priceMinDataGridViewTextBoxColumn.MinimumWidth = 80;
            this.priceMinDataGridViewTextBoxColumn.Name = "priceMinDataGridViewTextBoxColumn";
            this.priceMinDataGridViewTextBoxColumn.Width = 80;
            // 
            // priceMaxDataGridViewTextBoxColumn
            // 
            this.priceMaxDataGridViewTextBoxColumn.DataPropertyName = "PriceMax";
            this.priceMaxDataGridViewTextBoxColumn.HeaderText = "Max. price";
            this.priceMaxDataGridViewTextBoxColumn.MinimumWidth = 80;
            this.priceMaxDataGridViewTextBoxColumn.Name = "priceMaxDataGridViewTextBoxColumn";
            this.priceMaxDataGridViewTextBoxColumn.Width = 80;
            // 
            // MaxToMin
            // 
            this.MaxToMin.DataPropertyName = "MaxToMin";
            this.MaxToMin.HeaderText = "Max. to min. order";
            this.MaxToMin.MinimumWidth = 100;
            this.MaxToMin.Name = "MaxToMin";
            // 
            // frmTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 562);
            this.Controls.Add(this.chkIsWaiting);
            this.Controls.Add(this.AXSByPassWaitingRoom);
            this.Controls.Add(this.chkPersistSession);
            this.Controls.Add(this.btnGroup);
            this.Controls.Add(this.cbGroup);
            this.Controls.Add(this.docParameters);
            this.Controls.Add(this.btnSaveandStart);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtTicketName);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.txtTicketURL);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTicket";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add new ticket";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Silver;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTicket_FormClosed);
            this.Load += new System.EventHandler(this.frmTicket_Load);
            ((System.ComponentModel.ISupportInitialize)(this.c1CommandHolder1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.docParameters)).EndInit();
            this.docParameters.ResumeLayout(false);
            this.tabPageParameter.ResumeLayout(false);
            this.pnlOnTicketsFound.ResumeLayout(false);
            this.pnlOnTicketsFound.PerformLayout();
            this.pnlAutoCaptcha.ResumeLayout(false);
            this.pnlCaptchaService.ResumeLayout(false);
            this.pnlCaptchaService.PerformLayout();
            this.pnlCaptchaSolving.ResumeLayout(false);
            this.pnlCaptchaSolving.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartSolvingFromCaptcha)).EndInit();
            this.pnlSearchParameters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvParameters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTicketParameterBindingSource)).EndInit();
            this.pnlSearchParametersOptions.ResumeLayout(false);
            this.pnlSearchParametersOptions.PerformLayout();
            this.pnlParamters.ResumeLayout(false);
            this.pnlParamters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNoOfSearches)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudResetSearchDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Delay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStartUsingProxiesFrom)).EndInit();
            this.tabPageMoreParameters.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTicketAccountBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFindingCriteria)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTicketFoundCriteriaBindingSource)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScheduleRunningTime)).EndInit();
            this.pnlDeliveryOptions.ResumeLayout(false);
            this.pnlDeliveryOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDeliveryOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTicketDeliveryOptionBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTicketName;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtTicketURL;
        private System.Windows.Forms.Label label1;
        private C1.Win.C1Input.C1Button btnSave;
        private C1.Win.C1Input.C1Button btnSaveandStart;
        private C1.Win.C1Command.C1CommandHolder c1CommandHolder1;
        private C1.Win.C1Command.C1Command c1Command1;
        private C1.Win.C1Command.C1CommandControl c1CommandControl1;
        private C1.Win.C1Command.C1CommandControl c1CommandControl2;
        private C1.Win.C1Command.C1CommandControl c1CommandControl3;
        private C1.Win.C1Command.C1CommandControl c1CommandControl4;
        private C1.Win.C1Command.C1DockingTab docParameters;
        private C1.Win.C1Command.C1DockingTabPage tabPageParameter;
        private C1.Win.C1Command.C1DockingTabPage tabPageMoreParameters;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudNoOfSearches;
        private System.Windows.Forms.DataGridView gvParameters;
        private System.Windows.Forms.RadioButton rbUseAvailableParameters;
        private System.Windows.Forms.RadioButton rbDistributeInSearches;
        private System.Windows.Forms.RadioButton rbUseFoundOnFirstAttempt;
        private System.Windows.Forms.NumericUpDown nudResetSearchDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkRandomDelay;
        private System.Windows.Forms.CheckBox chkUseProxies;
        private System.Windows.Forms.NumericUpDown nudStartUsingProxiesFrom;
        private System.Windows.Forms.Label lblStartUsingProxiesFrom;
        private System.Windows.Forms.Panel pnlParamters;
        private System.Windows.Forms.Panel pnlSearchParametersOptions;
        private System.Windows.Forms.Panel pnlSearchParameters;
        private System.Windows.Forms.Panel pnlOnTicketsFound;
        private System.Windows.Forms.Panel pnlAutoCaptcha;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbUseRecaptcha;
        private System.Windows.Forms.RadioButton rbUseGoogle;
        private System.Windows.Forms.Panel pnlCaptchaSource;
        private System.Windows.Forms.Panel pnlCaptchaSolving;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlCaptchaService;
        private System.Windows.Forms.Label lblStartSolvingFrom;
        private System.Windows.Forms.CheckBox chkAutoCaptcha;
        private System.Windows.Forms.NumericUpDown nudStartSolvingFromCaptcha;
        private System.Windows.Forms.CheckBox chkAutoBuyWitoutProxy;
        private System.Windows.Forms.CheckBox chkAutoBuy;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkPlaySoundAlert;
        private System.Windows.Forms.CheckBox chkSendEmail;
        private System.Windows.Forms.LinkLabel btnEmailSettings;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.ComboBox cbGroup;
        private System.Windows.Forms.LinkLabel btnGroup;
        private System.Windows.Forms.LinkLabel btnAutoBuyOptions;
        private System.Windows.Forms.Panel pnlDeliveryOptions;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RadioButton rbSelectDeliveryOptionAutoBuying;
        private System.Windows.Forms.RadioButton rbSelectDeliveryOptionList;
        private System.Windows.Forms.Label lblDeliveryOption;
        private System.Windows.Forms.Label lblDeliveryCountry;
        private System.Windows.Forms.TextBox txtDeliveryCountry;
        private System.Windows.Forms.TextBox txtDeliveryOption;
        private System.Windows.Forms.DataGridView gvDeliveryOptions;
        private System.Windows.Forms.BindingSource iTicketDeliveryOptionBindingSource;
        private System.Windows.Forms.LinkLabel btnManageDeliveryOptions;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ifSelectedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deliveryOptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deliveryCountryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView gvFindingCriteria;
        private System.Windows.Forms.BindingSource iTicketFoundCriteriaBindingSource;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkSchedule;
        private System.Windows.Forms.Label lblRunningTime;
        private System.Windows.Forms.DateTimePicker dtpScheduleDateTime;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblMinutes;
        private System.Windows.Forms.NumericUpDown nudScheduleRunningTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketType;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowFromDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowToDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sectionDataGridViewTextBoxColumn1;
        private System.Windows.Forms.CheckBox chkPersistSession;
        private System.Windows.Forms.RadioButton rbSelectAccountAutoBuying;
        private System.Windows.Forms.RadioButton rbSelectAccountList;
        private System.Windows.Forms.DataGridView gvAccounts;
        private System.Windows.Forms.BindingSource iTicketAccountBindingSource;
        private System.Windows.Forms.LinkLabel btnManageAccounts;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ifSelectedDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountEmailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BuyingLimit;
        private System.Windows.Forms.BindingSource iTicketParameterBindingSource;
        private System.Windows.Forms.CheckBox chkUseProxiesInCaptchaSource;
        private System.Windows.Forms.CheckBox AXSByPassWaitingRoom;
        private System.Windows.Forms.MonthCalendar mcSelectDate;
        private System.Windows.Forms.RadioButton rbDCAutoCaptcha;
        private System.Windows.Forms.RadioButton rbDBCAutoCaptcha;
        private System.Windows.Forms.RadioButton rbCPTAutoCaptcha;
        private System.Windows.Forms.RadioButton rbRDAutoCaptcha;
        private System.Windows.Forms.RadioButton rbBPCAutoCaptcha;
        private System.Windows.Forms.RadioButton rbOCR;
        private System.Windows.Forms.RadioButton rbCAutoCaptcha;
        private System.Windows.Forms.RadioButton rbRAutoCaptcha;
        private System.Windows.Forms.RadioButton rbBoloAutoCaptcha;
        private System.Windows.Forms.RadioButton rbSOCR;
        private System.Windows.Forms.NumericUpDown Delay;
        private System.Windows.Forms.RadioButton rb2CAutoCaptcha;
        private System.Windows.Forms.DateTimePicker dtSelect;
        private System.Windows.Forms.CheckBox chkIsWaiting;
        private System.Windows.Forms.RadioButton rbAAutoCaptcha;
        private System.Windows.Forms.RadioButton rbCaptchatorAutoCaptcha;
        private System.Windows.Forms.RadioButton rbAC1AutoCaptcha;
        private System.Windows.Forms.CheckBox chkMobile;
        private System.Windows.Forms.CheckBox chkWeb;
        private System.Windows.Forms.RadioButton rbRDCAutoCaptcha;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bought;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxBought;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateTimeStringDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn GetResaleTix;
        private System.Windows.Forms.DataGridViewTextBoxColumn OfferName;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceLevelStringDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn exactMatchDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketTypePassswordDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LowestPrice;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TopPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceMinDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceMaxDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MaxToMin;
    }
}