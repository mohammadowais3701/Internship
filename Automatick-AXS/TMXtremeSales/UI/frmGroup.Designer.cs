namespace Automatick
{
    partial class frmGroup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroup));
            this.btnCancel = new C1.Win.C1Input.C1Button();
            this.btnSave = new C1.Win.C1Input.C1Button();
            this.gbGroupManagement = new System.Windows.Forms.GroupBox();
            this.gvGroups = new System.Windows.Forms.DataGridView();
            this.groupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketGroupBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gbTickets = new System.Windows.Forms.GroupBox();
            this.gvTickets = new System.Windows.Forms.DataGridView();
            this.ticketNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TicketGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AXSTicketBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbGroupManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvGroups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketGroupBindingSource)).BeginInit();
            this.gbTickets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvTickets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AXSTicketBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(578, 384);
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
            this.btnSave.Location = new System.Drawing.Point(470, 384);
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
            this.gbGroupManagement.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbGroupManagement.Controls.Add(this.gvGroups);
            this.gbGroupManagement.Location = new System.Drawing.Point(12, 12);
            this.gbGroupManagement.Name = "gbGroupManagement";
            this.gbGroupManagement.Size = new System.Drawing.Size(334, 358);
            this.gbGroupManagement.TabIndex = 0;
            this.gbGroupManagement.TabStop = false;
            this.gbGroupManagement.Text = "Events groups management";
            // 
            // gvGroups
            // 
            this.gvGroups.AllowDrop = true;
            this.gvGroups.AllowUserToResizeRows = false;
            this.gvGroups.AutoGenerateColumns = false;
            this.gvGroups.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvGroups.BackgroundColor = System.Drawing.Color.White;
            this.gvGroups.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvGroups.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.gvGroups.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvGroups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupNameDataGridViewTextBoxColumn});
            this.gvGroups.DataSource = this.ticketGroupBindingSource;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvGroups.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvGroups.GridColor = System.Drawing.Color.LightGray;
            this.gvGroups.Location = new System.Drawing.Point(3, 16);
            this.gvGroups.MultiSelect = false;
            this.gvGroups.Name = "gvGroups";
            this.gvGroups.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvGroups.RowHeadersVisible = false;
            this.gvGroups.RowHeadersWidth = 20;
            this.gvGroups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvGroups.Size = new System.Drawing.Size(328, 339);
            this.gvGroups.TabIndex = 0;
            this.gvGroups.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gvGroups_CellBeginEdit);
            this.gvGroups.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvGroups_CellMouseLeave);
            this.gvGroups.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.gvBackpageReply_CellValidating);
            this.gvGroups.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gvBackpageReply_DataError);
            this.gvGroups.SelectionChanged += new System.EventHandler(this.gvGroups_SelectionChanged);
            this.gvGroups.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.gvBackpageReply_UserDeletingRow);
            this.gvGroups.DragDrop += new System.Windows.Forms.DragEventHandler(this.gvGroups_DragDrop);
            this.gvGroups.DragEnter += new System.Windows.Forms.DragEventHandler(this.gvGroups_DragEnter);
            this.gvGroups.DragOver += new System.Windows.Forms.DragEventHandler(this.gvGroups_DragOver);
            this.gvGroups.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gvGroups_MouseDown);
            // 
            // groupNameDataGridViewTextBoxColumn
            // 
            this.groupNameDataGridViewTextBoxColumn.DataPropertyName = "GroupName";
            this.groupNameDataGridViewTextBoxColumn.HeaderText = "Group name";
            this.groupNameDataGridViewTextBoxColumn.Name = "groupNameDataGridViewTextBoxColumn";
            // 
            // ticketGroupBindingSource
            // 
            this.ticketGroupBindingSource.DataSource = typeof(Automatick.Core.TicketGroup);
            // 
            // gbTickets
            // 
            this.gbTickets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTickets.Controls.Add(this.gvTickets);
            this.gbTickets.Location = new System.Drawing.Point(352, 12);
            this.gbTickets.Name = "gbTickets";
            this.gbTickets.Size = new System.Drawing.Size(334, 358);
            this.gbTickets.TabIndex = 1;
            this.gbTickets.TabStop = false;
            this.gbTickets.Text = "Events";
            // 
            // gvTickets
            // 
            this.gvTickets.AllowDrop = true;
            this.gvTickets.AllowUserToAddRows = false;
            this.gvTickets.AllowUserToDeleteRows = false;
            this.gvTickets.AllowUserToResizeRows = false;
            this.gvTickets.AutoGenerateColumns = false;
            this.gvTickets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvTickets.BackgroundColor = System.Drawing.Color.White;
            this.gvTickets.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvTickets.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvTickets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTickets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ticketNameDataGridViewTextBoxColumn,
            this.TicketGroup});
            this.gvTickets.DataSource = this.AXSTicketBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvTickets.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvTickets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvTickets.GridColor = System.Drawing.Color.LightGray;
            this.gvTickets.Location = new System.Drawing.Point(3, 16);
            this.gvTickets.MultiSelect = false;
            this.gvTickets.Name = "gvTickets";
            this.gvTickets.ReadOnly = true;
            this.gvTickets.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gvTickets.RowHeadersVisible = false;
            this.gvTickets.RowHeadersWidth = 20;
            this.gvTickets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvTickets.Size = new System.Drawing.Size(328, 339);
            this.gvTickets.TabIndex = 0;
            this.gvTickets.DragEnter += new System.Windows.Forms.DragEventHandler(this.gvTickets_DragEnter);
            this.gvTickets.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gvTickets_MouseDown);
            // 
            // ticketNameDataGridViewTextBoxColumn
            // 
            this.ticketNameDataGridViewTextBoxColumn.DataPropertyName = "TicketName";
            this.ticketNameDataGridViewTextBoxColumn.FillWeight = 60F;
            this.ticketNameDataGridViewTextBoxColumn.HeaderText = "Event name";
            this.ticketNameDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.ticketNameDataGridViewTextBoxColumn.Name = "ticketNameDataGridViewTextBoxColumn";
            this.ticketNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // TicketGroup
            // 
            this.TicketGroup.DataPropertyName = "TicketGroup";
            this.TicketGroup.FillWeight = 40F;
            this.TicketGroup.HeaderText = "Group";
            this.TicketGroup.MinimumWidth = 100;
            this.TicketGroup.Name = "TicketGroup";
            this.TicketGroup.ReadOnly = true;
            this.TicketGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AXSTicketBindingSource
            // 
            this.AXSTicketBindingSource.DataSource = typeof(Automatick.Core.AXSTicket);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "TicketGroup";
            this.dataGridViewTextBoxColumn1.FillWeight = 40F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Group";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 325;
            // 
            // frmGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(692, 419);
            this.Controls.Add(this.gbTickets);
            this.Controls.Add(this.gbGroupManagement);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Group management";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Office2010Silver;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGroup_FormClosed);
            this.Load += new System.EventHandler(this.frmGroup_Load);
            this.gbGroupManagement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvGroups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketGroupBindingSource)).EndInit();
            this.gbTickets.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvTickets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AXSTicketBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1Input.C1Button btnCancel;
        private C1.Win.C1Input.C1Button btnSave;
        private System.Windows.Forms.GroupBox gbGroupManagement;
        private System.Windows.Forms.DataGridView gvGroups;
        private System.Windows.Forms.BindingSource ticketGroupBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox gbTickets;
        private System.Windows.Forms.DataGridView gvTickets;
        private System.Windows.Forms.BindingSource AXSTicketBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ticketNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TicketGroup;
    }
}
