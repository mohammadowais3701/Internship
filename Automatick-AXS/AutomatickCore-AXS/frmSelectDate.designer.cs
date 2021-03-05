namespace Automatick
{
    partial class frmSelectDate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectDate));
            this.cmbEventDates = new System.Windows.Forms.ComboBox();
            this.btnCancel = new C1.Win.C1Input.C1Button();
            this.btnSelect = new C1.Win.C1Input.C1Button();
            this.lblTicketName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.axsEventDatesBindingSource = new System.Windows.Forms.BindingSource();
            ((System.ComponentModel.ISupportInitialize)(this.axsEventDatesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbEventDates
            // 
            this.cmbEventDates.FormattingEnabled = true;
            this.cmbEventDates.Location = new System.Drawing.Point(121, 38);
            this.cmbEventDates.Name = "cmbEventDates";
            this.cmbEventDates.Size = new System.Drawing.Size(201, 21);
            this.cmbEventDates.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(262, 84);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(178, 84);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 8;
            this.btnSelect.Text = "&Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblTicketName
            // 
            this.lblTicketName.AutoSize = true;
            this.lblTicketName.Location = new System.Drawing.Point(24, 9);
            this.lblTicketName.Name = "lblTicketName";
            this.lblTicketName.Size = new System.Drawing.Size(35, 13);
            this.lblTicketName.TabIndex = 10;
            this.lblTicketName.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Event dates";
            // 
            // axsEventDatesBindingSource
            // 
            this.axsEventDatesBindingSource.DataSource = typeof(Automatick.Core.AXSSearch);
            this.axsEventDatesBindingSource.CurrentChanged += new System.EventHandler(this.axsEventDatesBindingSource_CurrentChanged);
            this.axsEventDatesBindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.axsEventDatesBindingSource_ListChanged);
            // 
            // frmSelectDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 119);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTicketName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.cmbEventDates);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(427, 215);
            this.MinimumSize = new System.Drawing.Size(300, 150);
            this.Name = "frmSelectDate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Select Event Date";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Silver;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSelectDate_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.axsEventDatesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource axsEventDatesBindingSource;
        private System.Windows.Forms.ComboBox cmbEventDates;
        private C1.Win.C1Input.C1Button btnCancel;
        private C1.Win.C1Input.C1Button btnSelect;
        private System.Windows.Forms.Label lblTicketName;
        private System.Windows.Forms.Label label1;

    }
}