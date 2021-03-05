namespace Automatick
{
    partial class frmDeliveryOption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDeliveryOption));
            this.btnCancel = new C1.Win.C1Input.C1Button();
            this.btnSave = new C1.Win.C1Input.C1Button();
            this.gbGroupManagement = new System.Windows.Forms.GroupBox();
            this.gvDeliveryOptions = new System.Windows.Forms.DataGridView();
            this.ifSelectedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.deliveryOptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deliveryCountryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iTicketDeliveryOptionBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gbGroupManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDeliveryOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTicketDeliveryOptionBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(278, 384);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnCancel.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(170, 384);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.VisualStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnSave.VisualStyleBaseStyle = C1.Win.C1Input.VisualStyle.Office2007Silver;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbGroupManagement
            // 
            this.gbGroupManagement.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGroupManagement.Controls.Add(this.gvDeliveryOptions);
            this.gbGroupManagement.Location = new System.Drawing.Point(12, 12);
            this.gbGroupManagement.Name = "gbGroupManagement";
            this.gbGroupManagement.Size = new System.Drawing.Size(368, 358);
            this.gbGroupManagement.TabIndex = 0;
            this.gbGroupManagement.TabStop = false;
            this.gbGroupManagement.Text = "Delivery options management";
            // 
            // gvDeliveryOptions
            // 
            this.gvDeliveryOptions.AllowUserToResizeRows = false;
            this.gvDeliveryOptions.AutoGenerateColumns = false;
            this.gvDeliveryOptions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvDeliveryOptions.BackgroundColor = System.Drawing.Color.White;
            this.gvDeliveryOptions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvDeliveryOptions.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.gvDeliveryOptions.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvDeliveryOptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDeliveryOptions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ifSelectedDataGridViewCheckBoxColumn,
            this.deliveryOptionDataGridViewTextBoxColumn,
            this.deliveryCountryDataGridViewTextBoxColumn});
            this.gvDeliveryOptions.DataSource = this.iTicketDeliveryOptionBindingSource;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvDeliveryOptions.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvDeliveryOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvDeliveryOptions.GridColor = System.Drawing.Color.LightGray;
            this.gvDeliveryOptions.Location = new System.Drawing.Point(3, 16);
            this.gvDeliveryOptions.MultiSelect = false;
            this.gvDeliveryOptions.Name = "gvDeliveryOptions";
            this.gvDeliveryOptions.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvDeliveryOptions.RowHeadersVisible = false;
            this.gvDeliveryOptions.RowHeadersWidth = 20;
            this.gvDeliveryOptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvDeliveryOptions.Size = new System.Drawing.Size(362, 339);
            this.gvDeliveryOptions.TabIndex = 0;
            this.gvDeliveryOptions.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gvDeliveryOptions_CellBeginEdit);
            this.gvDeliveryOptions.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gvDeliveryOptions_CellValidating);
            this.gvDeliveryOptions.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gvDeliveryOptions_DataError);
            this.gvDeliveryOptions.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gvDeliveryOptions_UserDeletingRow);
            // 
            // ifSelectedDataGridViewCheckBoxColumn
            // 
            this.ifSelectedDataGridViewCheckBoxColumn.DataPropertyName = "IfSelected";
            this.ifSelectedDataGridViewCheckBoxColumn.FillWeight = 15F;
            this.ifSelectedDataGridViewCheckBoxColumn.HeaderText = "Default";
            this.ifSelectedDataGridViewCheckBoxColumn.MinimumWidth = 50;
            this.ifSelectedDataGridViewCheckBoxColumn.Name = "ifSelectedDataGridViewCheckBoxColumn";
            // 
            // deliveryOptionDataGridViewTextBoxColumn
            // 
            this.deliveryOptionDataGridViewTextBoxColumn.DataPropertyName = "DeliveryOption";
            this.deliveryOptionDataGridViewTextBoxColumn.FillWeight = 50F;
            this.deliveryOptionDataGridViewTextBoxColumn.HeaderText = "Delivery option";
            this.deliveryOptionDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.deliveryOptionDataGridViewTextBoxColumn.Name = "deliveryOptionDataGridViewTextBoxColumn";
            // 
            // deliveryCountryDataGridViewTextBoxColumn
            // 
            this.deliveryCountryDataGridViewTextBoxColumn.DataPropertyName = "DeliveryCountry";
            this.deliveryCountryDataGridViewTextBoxColumn.FillWeight = 35F;
            this.deliveryCountryDataGridViewTextBoxColumn.HeaderText = "Delivery country";
            this.deliveryCountryDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.deliveryCountryDataGridViewTextBoxColumn.Name = "deliveryCountryDataGridViewTextBoxColumn";
            // 
            // iTicketDeliveryOptionBindingSource
            // 
            this.iTicketDeliveryOptionBindingSource.DataSource = typeof(Automatick.Core.ITicketDeliveryOption);
            // 
            // frmDeliveryOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(392, 419);
            this.Controls.Add(this.gbGroupManagement);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDeliveryOption";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Delivery option management";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Silver;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDeliveryOption_FormClosed);
            this.Load += new System.EventHandler(this.frmDeliveryOption_Load);
            this.gbGroupManagement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvDeliveryOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iTicketDeliveryOptionBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1Input.C1Button btnCancel;
        private C1.Win.C1Input.C1Button btnSave;
        private System.Windows.Forms.GroupBox gbGroupManagement;
        private System.Windows.Forms.DataGridView gvDeliveryOptions;
        private System.Windows.Forms.BindingSource iTicketDeliveryOptionBindingSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ifSelectedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deliveryOptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deliveryCountryDataGridViewTextBoxColumn;
    }
}
