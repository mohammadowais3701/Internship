namespace Automatick
{
    partial class frmEmail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmail));
            this.gbFromEmailSettings = new System.Windows.Forms.GroupBox();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.cbIsSSL = new System.Windows.Forms.CheckBox();
            this.txtSMTP = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbEmailAddresses = new System.Windows.Forms.GroupBox();
            this.gvBackpageReply = new System.Windows.Forms.DataGridView();
            this.emailAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSave = new C1.Win.C1Input.C1Button();
            this.btnCancel = new C1.Win.C1Input.C1Button();
            this.gbFromEmailSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.gbEmailAddresses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvBackpageReply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emailBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gbFromEmailSettings
            // 
            this.gbFromEmailSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbFromEmailSettings.Controls.Add(this.nudPort);
            this.gbFromEmailSettings.Controls.Add(this.cbIsSSL);
            this.gbFromEmailSettings.Controls.Add(this.txtSMTP);
            this.gbFromEmailSettings.Controls.Add(this.txtPassword);
            this.gbFromEmailSettings.Controls.Add(this.txtEmail);
            this.gbFromEmailSettings.Controls.Add(this.label5);
            this.gbFromEmailSettings.Controls.Add(this.label4);
            this.gbFromEmailSettings.Controls.Add(this.label3);
            this.gbFromEmailSettings.Controls.Add(this.label2);
            this.gbFromEmailSettings.Controls.Add(this.label1);
            this.gbFromEmailSettings.Location = new System.Drawing.Point(12, 12);
            this.gbFromEmailSettings.Name = "gbFromEmailSettings";
            this.gbFromEmailSettings.Size = new System.Drawing.Size(368, 144);
            this.gbFromEmailSettings.TabIndex = 0;
            this.gbFromEmailSettings.TabStop = false;
            this.gbFromEmailSettings.Text = "Email address to be used for sending emails";
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(108, 91);
            this.nudPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(50, 20);
            this.nudPort.TabIndex = 3;
            this.nudPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudPort.Value = new decimal(new int[] {
            587,
            0,
            0,
            0});
            // 
            // cbIsSSL
            // 
            this.cbIsSSL.AutoSize = true;
            this.cbIsSSL.Location = new System.Drawing.Point(108, 116);
            this.cbIsSSL.Name = "cbIsSSL";
            this.cbIsSSL.Size = new System.Drawing.Size(15, 14);
            this.cbIsSSL.TabIndex = 4;
            this.cbIsSSL.UseVisualStyleBackColor = true;
            // 
            // txtSMTP
            // 
            this.txtSMTP.BackColor = System.Drawing.Color.GhostWhite;
            this.txtSMTP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMTP.Location = new System.Drawing.Point(108, 67);
            this.txtSMTP.Name = "txtSMTP";
            this.txtSMTP.Size = new System.Drawing.Size(246, 20);
            this.txtSMTP.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.GhostWhite;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Location = new System.Drawing.Point(108, 43);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(246, 20);
            this.txtPassword.TabIndex = 1;
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.GhostWhite;
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Location = new System.Drawing.Point(108, 19);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(246, 20);
            this.txtEmail.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Is Required SSL :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Port :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "SMTP Server :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Password :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Email Address :";
            // 
            // gbEmailAddresses
            // 
            this.gbEmailAddresses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEmailAddresses.Controls.Add(this.gvBackpageReply);
            this.gbEmailAddresses.Location = new System.Drawing.Point(12, 162);
            this.gbEmailAddresses.Name = "gbEmailAddresses";
            this.gbEmailAddresses.Size = new System.Drawing.Size(368, 216);
            this.gbEmailAddresses.TabIndex = 1;
            this.gbEmailAddresses.TabStop = false;
            this.gbEmailAddresses.Text = "Email addresses to be used for receiving emails";
            // 
            // gvBackpageReply
            // 
            this.gvBackpageReply.AllowUserToResizeRows = false;
            this.gvBackpageReply.AutoGenerateColumns = false;
            this.gvBackpageReply.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvBackpageReply.BackgroundColor = System.Drawing.Color.White;
            this.gvBackpageReply.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvBackpageReply.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.gvBackpageReply.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvBackpageReply.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvBackpageReply.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.emailAddressDataGridViewTextBoxColumn});
            this.gvBackpageReply.DataSource = this.emailBindingSource;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvBackpageReply.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvBackpageReply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvBackpageReply.GridColor = System.Drawing.Color.LightGray;
            this.gvBackpageReply.Location = new System.Drawing.Point(3, 16);
            this.gvBackpageReply.MultiSelect = false;
            this.gvBackpageReply.Name = "gvBackpageReply";
            this.gvBackpageReply.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvBackpageReply.RowHeadersVisible = false;
            this.gvBackpageReply.RowHeadersWidth = 20;
            this.gvBackpageReply.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvBackpageReply.Size = new System.Drawing.Size(362, 197);
            this.gvBackpageReply.TabIndex = 2;
            this.gvBackpageReply.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gvBackpageReply_CellValidating);
            // 
            // emailAddressDataGridViewTextBoxColumn
            // 
            this.emailAddressDataGridViewTextBoxColumn.DataPropertyName = "EmailAddress";
            this.emailAddressDataGridViewTextBoxColumn.HeaderText = "Email address";
            this.emailAddressDataGridViewTextBoxColumn.Name = "emailAddressDataGridViewTextBoxColumn";
            // 
            // emailBindingSource
            // 
            this.emailBindingSource.DataSource = typeof(Automatick.Core.Email);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(170, 389);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnSave.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(278, 389);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnCancel.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmEmail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(392, 419);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbEmailAddresses);
            this.Controls.Add(this.gbFromEmailSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 450);
            this.Name = "frmEmail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Email setting and email address management";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Silver;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEmail_FormClosed);
            this.Load += new System.EventHandler(this.frmEmail_Load);
            this.gbFromEmailSettings.ResumeLayout(false);
            this.gbFromEmailSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.gbEmailAddresses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvBackpageReply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emailBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFromEmailSettings;
        private System.Windows.Forms.CheckBox cbIsSSL;
        private System.Windows.Forms.TextBox txtSMTP;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.GroupBox gbEmailAddresses;
        private C1.Win.C1Input.C1Button btnSave;
        private C1.Win.C1Input.C1Button btnCancel;
        private System.Windows.Forms.DataGridView gvBackpageReply;
        private System.Windows.Forms.BindingSource emailBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailAddressDataGridViewTextBoxColumn;
    }
}
