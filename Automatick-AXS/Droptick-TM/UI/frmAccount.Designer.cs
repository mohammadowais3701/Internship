namespace Automatick
{
    partial class frmAccount
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccount));
            this.btnCancel = new C1.Win.C1Input.C1Button();
            this.btnSave = new C1.Win.C1Input.C1Button();
            this.gbGroupManagement = new System.Windows.Forms.GroupBox();
            this.txtAccName = new System.Windows.Forms.TextBox();
            this.iAccountBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.States = new System.Windows.Forms.ComboBox();
            this.Country = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.CCVNum = new System.Windows.Forms.TextBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.cardNum = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.address1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Mobile = new System.Windows.Forms.TextBox();
            this.Telephone = new System.Windows.Forms.TextBox();
            this.ConfirmEmail = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Email = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.postCode = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.town = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.address2 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lastName = new System.Windows.Forms.TextBox();
            this.FirstName = new System.Windows.Forms.TextBox();
            this.gvAccounts = new System.Windows.Forms.DataGridView();
            this.accountNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmailAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExport = new C1.Win.C1Input.C1Button();
            this.btnImport = new C1.Win.C1Input.C1Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gbGroupManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iAccountBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAccounts)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(1063, 573);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnCancel.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(955, 573);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnSave.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbGroupManagement
            // 
            this.gbGroupManagement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbGroupManagement.Controls.Add(this.txtAccName);
            this.gbGroupManagement.Controls.Add(this.label9);
            this.gbGroupManagement.Controls.Add(this.groupBox1);
            this.gbGroupManagement.Controls.Add(this.groupBox3);
            this.gbGroupManagement.Controls.Add(this.groupBox2);
            this.gbGroupManagement.Controls.Add(this.gvAccounts);
            this.gbGroupManagement.Location = new System.Drawing.Point(12, 4);
            this.gbGroupManagement.Name = "gbGroupManagement";
            this.gbGroupManagement.Size = new System.Drawing.Size(1161, 563);
            this.gbGroupManagement.TabIndex = 1;
            this.gbGroupManagement.TabStop = false;
            this.gbGroupManagement.Text = "Buying accounts";
            // 
            // txtAccName
            // 
            this.txtAccName.BackColor = System.Drawing.Color.GhostWhite;
            this.txtAccName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAccName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "AccountName", true));
            this.txtAccName.Enabled = false;
            this.txtAccName.Location = new System.Drawing.Point(809, 20);
            this.txtAccName.Name = "txtAccName";
            this.txtAccName.Size = new System.Drawing.Size(317, 20);
            this.txtAccName.TabIndex = 1;
            // 
            // iAccountBindingSource
            // 
            this.iAccountBindingSource.DataSource = typeof(Automatick.Core.ITicketAccount);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(674, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Account Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.States);
            this.groupBox1.Controls.Add(this.Country);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(672, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(481, 42);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Location";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(296, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "States";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Country";
            // 
            // States
            // 
            this.States.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "State", true));
            this.States.Enabled = false;
            this.States.FormattingEnabled = true;
            this.States.Location = new System.Drawing.Point(339, 17);
            this.States.Name = "States";
            this.States.Size = new System.Drawing.Size(111, 21);
            this.States.TabIndex = 1;
            this.States.DropDownClosed += new System.EventHandler(this.States_DropDownClosed);
            // 
            // Country
            // 
            this.Country.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "Country", true));
            this.Country.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Country.FormattingEnabled = true;
            this.Country.Items.AddRange(new object[] {
            "Select Country",
            "Canada",
            "United States",
            "Argentina",
            "Australia",
            "Austria",
            "Belgium",
            "Brazil",
            "Botswana",
            "Chile",
            "China",
            "Costa Rica",
            "Croatia",
            "Denmark",
            "Estonia",
            "Finland",
            "France",
            "Georgia",
            "Germany",
            "Greece",
            "Hong Kong",
            "Hungary",
            "Iceland",
            "Indonesia",
            "Ireland",
            "Israel",
            "Italy",
            "Jamaica",
            "Japan",
            "Korea, Republic Of",
            "Lithuania",
            "Mexico",
            "Monaco",
            "Netherlands",
            "New Zealand",
            "Norway",
            "Panama",
            "Philippines",
            "Poland",
            "Portugal",
            "Russian Federation",
            "Serbia",
            "South Africa",
            "Slovenia",
            "Singapore",
            "Sweden",
            "Spain",
            "Switzerland",
            "Turkey",
            "United Kingdom",
            "Zimbabwe"});
            this.Country.Location = new System.Drawing.Point(137, 17);
            this.Country.Name = "Country";
            this.Country.Size = new System.Drawing.Size(134, 21);
            this.Country.TabIndex = 0;
            this.Country.DropDownClosed += new System.EventHandler(this.Country_DropDownClosed);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox1);
            this.groupBox3.Controls.Add(this.comboBox2);
            this.groupBox3.Controls.Add(this.CCVNum);
            this.groupBox3.Controls.Add(this.comboBox3);
            this.groupBox3.Controls.Add(this.cardNum);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(672, 427);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(481, 126);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Payment Details";
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.GhostWhite;
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "CardType", true));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Select Card Type",
            "AMEX",
            "Master Card",
            "Visa",
            "DISCOVER"});
            this.comboBox1.Location = new System.Drawing.Point(137, 22);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(151, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // comboBox2
            // 
            this.comboBox2.BackColor = System.Drawing.Color.GhostWhite;
            this.comboBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "ExpiryYear", true));
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Year",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26"});
            this.comboBox2.Location = new System.Drawing.Point(204, 99);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(84, 21);
            this.comboBox2.TabIndex = 4;
            // 
            // CCVNum
            // 
            this.CCVNum.BackColor = System.Drawing.Color.GhostWhite;
            this.CCVNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CCVNum.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "CCVNum", true));
            this.CCVNum.Location = new System.Drawing.Point(137, 72);
            this.CCVNum.MaxLength = 5;
            this.CCVNum.Name = "CCVNum";
            this.CCVNum.Size = new System.Drawing.Size(106, 20);
            this.CCVNum.TabIndex = 2;
            // 
            // comboBox3
            // 
            this.comboBox3.BackColor = System.Drawing.Color.GhostWhite;
            this.comboBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "ExpiryMonth", true));
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "Month",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.comboBox3.Location = new System.Drawing.Point(137, 99);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(61, 21);
            this.comboBox3.TabIndex = 3;
            // 
            // cardNum
            // 
            this.cardNum.BackColor = System.Drawing.Color.GhostWhite;
            this.cardNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardNum.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "CardNumber", true));
            this.cardNum.Location = new System.Drawing.Point(137, 47);
            this.cardNum.MaxLength = 25;
            this.cardNum.Name = "cardNum";
            this.cardNum.Size = new System.Drawing.Size(317, 20);
            this.cardNum.TabIndex = 1;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(22, 103);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(79, 13);
            this.label19.TabIndex = 0;
            this.label19.Text = "Expiration Date";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(22, 76);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(112, 13);
            this.label21.TabIndex = 0;
            this.label21.Text = "Card Code Verification";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(22, 26);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(86, 13);
            this.label17.TabIndex = 0;
            this.label17.Text = "Credit Card Type";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(22, 51);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(99, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Credit Card Number";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPassword);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.address1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.Mobile);
            this.groupBox2.Controls.Add(this.Telephone);
            this.groupBox2.Controls.Add(this.ConfirmEmail);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.Email);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.postCode);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.town);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.address2);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.lastName);
            this.groupBox2.Controls.Add(this.FirstName);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(672, 105);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(481, 316);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Your Details";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Address 1";
            // 
            // address1
            // 
            this.address1.BackColor = System.Drawing.Color.GhostWhite;
            this.address1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.address1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "Address1", true));
            this.address1.Location = new System.Drawing.Point(137, 153);
            this.address1.MaxLength = 25;
            this.address1.Name = "address1";
            this.address1.Size = new System.Drawing.Size(317, 20);
            this.address1.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "First Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Last Name";
            // 
            // Mobile
            // 
            this.Mobile.BackColor = System.Drawing.Color.GhostWhite;
            this.Mobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Mobile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "Mobile", true));
            this.Mobile.Location = new System.Drawing.Point(137, 282);
            this.Mobile.MaxLength = 20;
            this.Mobile.Name = "Mobile";
            this.Mobile.Size = new System.Drawing.Size(317, 20);
            this.Mobile.TabIndex = 9;
            // 
            // Telephone
            // 
            this.Telephone.BackColor = System.Drawing.Color.GhostWhite;
            this.Telephone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Telephone.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "Telephone", true));
            this.Telephone.Location = new System.Drawing.Point(137, 256);
            this.Telephone.MaxLength = 15;
            this.Telephone.Name = "Telephone";
            this.Telephone.Size = new System.Drawing.Size(317, 20);
            this.Telephone.TabIndex = 8;
            // 
            // ConfirmEmail
            // 
            this.ConfirmEmail.BackColor = System.Drawing.Color.GhostWhite;
            this.ConfirmEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ConfirmEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "ConfirmEmail", true));
            this.ConfirmEmail.Location = new System.Drawing.Point(137, 100);
            this.ConfirmEmail.MaxLength = 50;
            this.ConfirmEmail.Name = "ConfirmEmail";
            this.ConfirmEmail.Size = new System.Drawing.Size(317, 20);
            this.ConfirmEmail.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 184);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Address 2";
            // 
            // Email
            // 
            this.Email.BackColor = System.Drawing.Color.GhostWhite;
            this.Email.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Email.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "EmailAddress", true));
            this.Email.Location = new System.Drawing.Point(137, 74);
            this.Email.MaxLength = 50;
            this.Email.Name = "Email";
            this.Email.Size = new System.Drawing.Size(317, 20);
            this.Email.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(22, 209);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "City";
            // 
            // postCode
            // 
            this.postCode.BackColor = System.Drawing.Color.GhostWhite;
            this.postCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.postCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "PostCode", true));
            this.postCode.Location = new System.Drawing.Point(137, 231);
            this.postCode.MaxLength = 15;
            this.postCode.Name = "postCode";
            this.postCode.Size = new System.Drawing.Size(317, 20);
            this.postCode.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(22, 235);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Postal Code";
            // 
            // town
            // 
            this.town.BackColor = System.Drawing.Color.GhostWhite;
            this.town.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.town.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "Town", true));
            this.town.Location = new System.Drawing.Point(137, 205);
            this.town.MaxLength = 25;
            this.town.Name = "town";
            this.town.Size = new System.Drawing.Size(317, 20);
            this.town.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 78);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Email address";
            // 
            // address2
            // 
            this.address2.BackColor = System.Drawing.Color.GhostWhite;
            this.address2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.address2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "Address2", true));
            this.address2.Location = new System.Drawing.Point(137, 180);
            this.address2.MaxLength = 25;
            this.address2.Name = "address2";
            this.address2.Size = new System.Drawing.Size(317, 20);
            this.address2.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(22, 104);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(93, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Confirm your Email";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 286);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mobile Phone";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(22, 260);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(66, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Phone (Day)";
            // 
            // lastName
            // 
            this.lastName.BackColor = System.Drawing.Color.GhostWhite;
            this.lastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lastName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "LastName", true));
            this.lastName.Location = new System.Drawing.Point(137, 47);
            this.lastName.MaxLength = 20;
            this.lastName.Name = "lastName";
            this.lastName.Size = new System.Drawing.Size(317, 20);
            this.lastName.TabIndex = 1;
            // 
            // FirstName
            // 
            this.FirstName.BackColor = System.Drawing.Color.GhostWhite;
            this.FirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FirstName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "FirstName", true));
            this.FirstName.Location = new System.Drawing.Point(137, 21);
            this.FirstName.MaxLength = 20;
            this.FirstName.Name = "FirstName";
            this.FirstName.Size = new System.Drawing.Size(317, 20);
            this.FirstName.TabIndex = 0;
            // 
            // gvAccounts
            // 
            this.gvAccounts.AllowUserToResizeRows = false;
            this.gvAccounts.AutoGenerateColumns = false;
            this.gvAccounts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvAccounts.BackgroundColor = System.Drawing.Color.White;
            this.gvAccounts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvAccounts.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.gvAccounts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAccounts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.accountNameDataGridViewTextBoxColumn,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.EmailAddress,
            this.CardNumber,
            this.dataGridViewTextBoxColumn3});
            this.gvAccounts.DataSource = this.iAccountBindingSource;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvAccounts.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvAccounts.Dock = System.Windows.Forms.DockStyle.Left;
            this.gvAccounts.GridColor = System.Drawing.Color.LightGray;
            this.gvAccounts.Location = new System.Drawing.Point(3, 16);
            this.gvAccounts.MultiSelect = false;
            this.gvAccounts.Name = "gvAccounts";
            this.gvAccounts.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvAccounts.RowHeadersVisible = false;
            this.gvAccounts.RowHeadersWidth = 20;
            this.gvAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvAccounts.Size = new System.Drawing.Size(655, 544);
            this.gvAccounts.TabIndex = 0;
            this.gvAccounts.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gvAccounts_CellBeginEdit);
            this.gvAccounts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvAccounts_CellClick);
            this.gvAccounts.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gvAccounts_CellValidating);
            this.gvAccounts.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gvAccounts_DataError);
            this.gvAccounts.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvAccounts_RowLeave);
            // 
            // accountNameDataGridViewTextBoxColumn
            // 
            this.accountNameDataGridViewTextBoxColumn.DataPropertyName = "AccountName";
            this.accountNameDataGridViewTextBoxColumn.FillWeight = 20F;
            this.accountNameDataGridViewTextBoxColumn.HeaderText = "Account name";
            this.accountNameDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.accountNameDataGridViewTextBoxColumn.Name = "accountNameDataGridViewTextBoxColumn";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "FirstName";
            this.dataGridViewTextBoxColumn1.HeaderText = "FirstName";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "LastName";
            this.dataGridViewTextBoxColumn2.HeaderText = "LastName";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // EmailAddress
            // 
            this.EmailAddress.DataPropertyName = "EmailAddress";
            this.EmailAddress.HeaderText = "EmailAddress";
            this.EmailAddress.Name = "EmailAddress";
            // 
            // CardNumber
            // 
            this.CardNumber.DataPropertyName = "CardNumber";
            this.CardNumber.HeaderText = "CardNumber";
            this.CardNumber.Name = "CardNumber";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "CCVNum";
            this.dataGridViewTextBoxColumn3.HeaderText = "CCVNum";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExport.Location = new System.Drawing.Point(123, 570);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(102, 23);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export to file";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnExport.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.Location = new System.Drawing.Point(15, 570);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(102, 23);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import from file";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnImport.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.GhostWhite;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.iAccountBindingSource, "Password", true));
            this.txtPassword.Location = new System.Drawing.Point(137, 127);
            this.txtPassword.MaxLength = 50;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(317, 20);
            this.txtPassword.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Password";
            // 
            // frmAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1177, 651);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.gbGroupManagement);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Buying accounts";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Silver;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAccount_FormClosed);
            this.Load += new System.EventHandler(this.frmAccount_Load);
            this.gbGroupManagement.ResumeLayout(false);
            this.gbGroupManagement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iAccountBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAccounts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1Input.C1Button btnCancel;
        private C1.Win.C1Input.C1Button btnSave;
        private System.Windows.Forms.GroupBox gbGroupManagement;
        private System.Windows.Forms.BindingSource iAccountBindingSource;
        private System.Windows.Forms.DataGridView gvAccounts;
        private C1.Win.C1Input.C1Button btnExport;
        private C1.Win.C1Input.C1Button btnImport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox States;
        private System.Windows.Forms.ComboBox Country;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox CCVNum;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.TextBox cardNum;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox address1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Mobile;
        private System.Windows.Forms.TextBox Telephone;
        private System.Windows.Forms.TextBox ConfirmEmail;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox Email;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox postCode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox town;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox address2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox lastName;
        private System.Windows.Forms.TextBox FirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn accountNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmailAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.TextBox txtAccName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
    }
}
