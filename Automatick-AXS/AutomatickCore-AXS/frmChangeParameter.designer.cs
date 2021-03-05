namespace Automatick
{
    partial class frmChangeParameter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChangeParameter));
            this.cbTicketType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbloldParameter = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblOldPassword = new System.Windows.Forms.Label();
            this.btnSelect = new C1.Win.C1Input.C1Button();
            this.btnCancel = new C1.Win.C1Input.C1Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblTicketName = new System.Windows.Forms.Label();
            this.txtManualTicketType = new System.Windows.Forms.TextBox();
            this.tMParameterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tMParameterBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cbTicketType
            // 
            this.cbTicketType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTicketType.FormattingEnabled = true;
            this.cbTicketType.Location = new System.Drawing.Point(111, 76);
            this.cbTicketType.Name = "cbTicketType";
            this.cbTicketType.Size = new System.Drawing.Size(181, 21);
            this.cbTicketType.TabIndex = 0;
            this.cbTicketType.SelectedIndexChanged += new System.EventHandler(this.cbTicketType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ticket Type";
            // 
            // lbloldParameter
            // 
            this.lbloldParameter.AutoSize = true;
            this.lbloldParameter.Location = new System.Drawing.Point(303, 79);
            this.lbloldParameter.Name = "lbloldParameter";
            this.lbloldParameter.Size = new System.Drawing.Size(69, 13);
            this.lbloldParameter.TabIndex = 2;
            this.lbloldParameter.Text = "old password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(111, 140);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(181, 20);
            this.txtPassword.TabIndex = 4;
            // 
            // lblOldPassword
            // 
            this.lblOldPassword.AutoSize = true;
            this.lblOldPassword.Location = new System.Drawing.Point(303, 144);
            this.lblOldPassword.Name = "lblOldPassword";
            this.lblOldPassword.Size = new System.Drawing.Size(71, 13);
            this.lblOldPassword.TabIndex = 5;
            this.lblOldPassword.Text = "old parameter";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(243, 226);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "&Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(332, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(25, 48);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(45, 13);
            this.lblCount.TabIndex = 8;
            this.lblCount.Text = "lblCount";
            // 
            // lblTicketName
            // 
            this.lblTicketName.AutoSize = true;
            this.lblTicketName.Location = new System.Drawing.Point(25, 15);
            this.lblTicketName.Name = "lblTicketName";
            this.lblTicketName.Size = new System.Drawing.Size(61, 13);
            this.lblTicketName.TabIndex = 9;
            this.lblTicketName.Text = "ticketName";
            // 
            // txtManualTicketType
            // 
            this.txtManualTicketType.Enabled = false;
            this.txtManualTicketType.Location = new System.Drawing.Point(111, 111);
            this.txtManualTicketType.Name = "txtManualTicketType";
            this.txtManualTicketType.Size = new System.Drawing.Size(181, 20);
            this.txtManualTicketType.TabIndex = 10;
            // 
            // tMParameterBindingSource
            // 
            this.tMParameterBindingSource.AllowNew = true;
            this.tMParameterBindingSource.DataSource = typeof(Automatick.Core.AXSSearch);
            this.tMParameterBindingSource.CurrentChanged += new System.EventHandler(this.tMParameterBindingSource_CurrentChanged);
            this.tMParameterBindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.tMParameterBindingSource_ListChanged);
            // 
            // frmChangeParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 261);
            this.ControlBox = false;
            this.Controls.Add(this.txtManualTicketType);
            this.Controls.Add(this.lblTicketName);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lblOldPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbloldParameter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbTicketType);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(427, 215);
            this.Name = "frmChangeParameter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Parameter";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Silver;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmChangeParameter_FormClosing);
            this.Load += new System.EventHandler(this.frmChangeParameter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tMParameterBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource tMParameterBindingSource;
        private System.Windows.Forms.ComboBox cbTicketType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbloldParameter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblOldPassword;
        private C1.Win.C1Input.C1Button btnSelect;
        private C1.Win.C1Input.C1Button btnCancel;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblTicketName;
        private System.Windows.Forms.TextBox txtManualTicketType;
    }
}