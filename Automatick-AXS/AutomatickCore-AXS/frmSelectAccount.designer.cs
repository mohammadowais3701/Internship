namespace Automatick
{
    partial class frmSelectAccount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectAccount));
            this.pnlAccounts = new System.Windows.Forms.Panel();
            this.btnSelect = new C1.Win.C1Input.C1Button();
            this.btnCancel = new C1.Win.C1Input.C1Button();
            this.lblTicketName = new System.Windows.Forms.Label();
            this.lblRowSection = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnlAccounts
            // 
            this.pnlAccounts.AutoScroll = true;
            this.pnlAccounts.Location = new System.Drawing.Point(3, 53);
            this.pnlAccounts.Name = "pnlAccounts";
            this.pnlAccounts.Size = new System.Drawing.Size(469, 246);
            this.pnlAccounts.TabIndex = 0;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Location = new System.Drawing.Point(316, 307);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 20);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.VisualStyle = C1.Win.C1Input.VisualStyle.Office2010Silver;
            this.btnSelect.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2010Silver;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(397, 307);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 20);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.VisualStyle = C1.Win.C1Input.VisualStyle.Office2010Silver;
            this.btnCancel.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2010Silver;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTicketName
            // 
            this.lblTicketName.AutoSize = true;
            this.lblTicketName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketName.Location = new System.Drawing.Point(7, 6);
            this.lblTicketName.Name = "lblTicketName";
            this.lblTicketName.Size = new System.Drawing.Size(14, 17);
            this.lblTicketName.TabIndex = 6;
            this.lblTicketName.Text = "-";
            // 
            // lblRowSection
            // 
            this.lblRowSection.AutoSize = true;
            this.lblRowSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRowSection.Location = new System.Drawing.Point(7, 33);
            this.lblRowSection.Name = "lblRowSection";
            this.lblRowSection.Size = new System.Drawing.Size(14, 17);
            this.lblRowSection.TabIndex = 7;
            this.lblRowSection.Text = "-";
            // 
            // frmSelectAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 330);
            this.ControlBox = false;
            this.Controls.Add(this.lblRowSection);
            this.Controls.Add(this.lblTicketName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.pnlAccounts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectAccount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select account";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Silver;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlAccounts;
        private C1.Win.C1Input.C1Button btnSelect;
        private C1.Win.C1Input.C1Button btnCancel;
        private System.Windows.Forms.Label lblTicketName;
        private System.Windows.Forms.Label lblRowSection;
    }
}