using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Automatick.Core;
using System.Linq;
using SortedBindingList;
namespace Automatick
{
    public partial class frmGroup : C1.Win.C1Ribbon.C1RibbonForm, IForm
    {        
        #region IAddForm Members

        public IMainForm MainForm
        {
            get;
            set;
        }

        public void load()
        {
            if (MainForm.AppStartUp.Groups != null)
            {
                this.ticketGroupBindingSource.DataSource = this.MainForm.AppStartUp.Groups;
            }
            if (MainForm.AppStartUp.Tickets != null)
            {
                this.AXSTicketBindingSource.DataSource = this.MainForm.AppStartUp.Tickets;
            }
        }

        public void save()
        {
            if (MainForm.AppStartUp.Groups != null)
            {
                this.MainForm.AppStartUp.SaveGroups();
            }
            if (MainForm.AppStartUp.Tickets != null)
            {
                this.MainForm.AppStartUp.SaveTickets();
            }
        }

        public bool validate()
        {
            return true;
        }

        public void onClosed()
        {
            this.MainForm.loadGroups();
            this.MainForm.loadTickets();
            this.MainForm.populateRecentTickets();
            GC.SuppressFinalize(this);
            //GC.Collect();
        }
        #endregion

        public frmGroup(IMainForm mainForm)
        {
            this.MainForm = mainForm;
            InitializeComponent();
        }

        private void frmGroup_Load(object sender, EventArgs e)
        {
            this.load();
        }

        private void frmGroup_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.onClosed();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                this.save();                
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvBackpageReply_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                TicketGroup group = this.MainForm.AppStartUp.Groups.FirstOrDefault(p => !String.IsNullOrEmpty(p.GroupName) ? p.GroupName == e.FormattedValue.ToString() : false);
                if (group != ticketGroupBindingSource.Current && group != null)
                {
                    MessageBox.Show("Group already exists. Please provide the unique group name.");
                    e.Cancel = true;
                }
                else if (ticketGroupBindingSource.Current != null)
                {
                    group = (TicketGroup)ticketGroupBindingSource.Current;
                    if (group.GroupId == "Default Group" && !(e.FormattedValue.ToString() == "Default Group" || e.FormattedValue.ToString() == "All Groups"))
                    {
                        e.Cancel = true;
                        MessageBox.Show("You cannot edit the predefined Group.", "Operation not allowed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gvGroups.CancelEdit();
                        gvGroups.EndEdit();
                    }
                    else if (group.GroupId != "Default Group")
                    {
                        IEnumerable<AXSTicket> filteredTickets = this.MainForm.AppStartUp.Tickets.Where(p => p.TicketGroup.GroupId == group.GroupId);
                        if (filteredTickets != null)
                        {
                            if (filteredTickets.Count() > 0)
                            {
                                foreach (AXSTicket item in filteredTickets)
                                {
                                    item.TicketGroup = group;
                                    AXSTicketBindingSource.ResetItem(AXSTicketBindingSource.IndexOf(item));
                                }
                                filterOnSelectionChanged();
                            }
                        }
                    }
                }
            }
            else if (ticketGroupBindingSource.Current != null)
            {
                TicketGroup group = (TicketGroup)ticketGroupBindingSource.Current;
                if (group.GroupId == "Default Group" && !(e.FormattedValue.ToString() == "Default Group" || e.FormattedValue.ToString() == "All Groups"))
                {
                    e.Cancel = true;
                    MessageBox.Show("You cannot edit the predefined Group.", "Operation not allowed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gvGroups.CancelEdit();
                    gvGroups.EndEdit();
                }
                else if (String.IsNullOrEmpty(group.GroupName) || String.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    ticketGroupBindingSource.Remove(group);
                    ticketGroupBindingSource.CancelEdit();
                    TicketGroup defaultGroup = this.MainForm.AppStartUp.Groups.FirstOrDefault(p => p.GroupName == "Default Group" && p.GroupId == "Default Group");
                    if (defaultGroup != null)
                    {
                        IEnumerable<AXSTicket> filteredTickets = this.MainForm.AppStartUp.Tickets.Where(p => p.TicketGroup.GroupId == group.GroupId);
                        if (filteredTickets != null)
                        {
                            if (filteredTickets.Count() > 0)
                            {
                                foreach (AXSTicket item in filteredTickets)
                                {
                                    item.TicketGroup = defaultGroup;
                                    AXSTicketBindingSource.ResetItem(AXSTicketBindingSource.IndexOf(item));
                                }
                                filterOnSelectionChanged();
                            }
                        }
                    }
                }
            }
        }

        private void gvBackpageReply_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row.DataBoundItem.GetType() == typeof(TicketGroup))
                {
                    TicketGroup group = (TicketGroup)e.Row.DataBoundItem;
                    if (group.GroupId == "Default Group")
                    {
                        e.Cancel = true;
                        MessageBox.Show("You cannot delete the predefined Group.", "Operation not allowed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        TicketGroup defaultGroup = this.MainForm.AppStartUp.Groups.FirstOrDefault(p => p.GroupName == "Default Group" && p.GroupId == "Default Group");
                        if (defaultGroup != null)
                        {
                            IEnumerable<AXSTicket> filteredTickets = this.MainForm.AppStartUp.Tickets.Where(p => p.TicketGroup.GroupId == group.GroupId);
                            if (filteredTickets != null)
                            {
                                if (filteredTickets.Count() > 0)
                                {
                                    if (MessageBox.Show("Do you really want to delete this group?" + Environment.NewLine + "If yes then assigned tickets would be assigned to Default Group.", "Group is allocated to one or more tickets!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        foreach (AXSTicket item in filteredTickets)
                                        {
                                            item.TicketGroup = defaultGroup;
                                        }
                                    }
                                    else
                                    {
                                        e.Cancel = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void gvBackpageReply_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }
        }

        private void gvGroups_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (ticketGroupBindingSource.Current != null)
                {
                    TicketGroup group = (TicketGroup)ticketGroupBindingSource.Current;
                    if (group.GroupId == "Default Group")
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception)
            {

            }
        }


        private void gvGroups_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DataGridView.HitTestInfo info = gvGroups.HitTest(e.X, e.Y);
                if (info.RowIndex >= 0)
                {
                    TicketGroup view = (TicketGroup)
                    gvGroups.Rows[info.RowIndex].DataBoundItem;
                    if (view != null)
                        gvGroups.DoDragDrop(view, DragDropEffects.Move);
                }
            }
        }

        private void gvGroups_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gvGroups_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(TicketGroup)))
                {
                    TicketGroup oldRowItem = (TicketGroup)e.Data.GetData(typeof(TicketGroup));

                    // Determine which category the item was draged to
                    Point p = gvGroups.PointToClient(new Point(e.X, e.Y));
                    DataGridView.HitTestInfo info = gvGroups.HitTest(p.X, p.Y);

                    if (info.RowIndex > -1)
                    {
                        int oldIndex = info.RowIndex;
                        DataGridViewRow newRow = (DataGridViewRow)gvGroups.Rows[oldIndex];
                        if (newRow.DataBoundItem != null)
                        {
                            TicketGroup newRowItem = (TicketGroup)newRow.DataBoundItem;

                            if (newRowItem != oldRowItem)
                            {
                                ticketGroupBindingSource.Remove(oldRowItem);
                                int newIndex = ticketGroupBindingSource.IndexOf(newRowItem);
                                if (oldIndex > newIndex && oldIndex - newIndex == 1)
                                {
                                    newIndex = oldIndex;
                                }
                                if (newIndex == (ticketGroupBindingSource.Count - 1) && oldIndex != (ticketGroupBindingSource.Count - 1))
                                {
                                    ticketGroupBindingSource.Add(oldRowItem);
                                }
                                else
                                {
                                    ticketGroupBindingSource.Insert(newIndex, oldRowItem);
                                }
                                newIndex = ticketGroupBindingSource.IndexOf(oldRowItem);
                            }
                        }
                    }
                }
                else if (e.Data.GetDataPresent(typeof(AXSTicket)))
                {
                    AXSTicket ticketItem = (AXSTicket)e.Data.GetData(typeof(AXSTicket));

                    // Determine which category the item was draged to
                    Point p = gvGroups.PointToClient(new Point(e.X, e.Y));
                    DataGridView.HitTestInfo info = gvGroups.HitTest(p.X, p.Y);
                    if (info.RowIndex > -1)
                    {
                        int index = info.RowIndex;
                        DataGridViewRow groupRow = (DataGridViewRow)gvGroups.Rows[index];
                        if (groupRow.DataBoundItem != null)
                        {
                            TicketGroup group = (TicketGroup)groupRow.DataBoundItem;
                            if (group.GroupName != "All Groups")
                            {
                                ticketItem.TicketGroup = group;
                                AXSTicketBindingSource.ResetItem(AXSTicketBindingSource.IndexOf(ticketItem));
                                filterOnSelectionChanged();
                            }
                            else if (group.GroupName == "All Groups")
                            {
                                MessageBox.Show("You cannot assign a ticket into All Groups.");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void gvTickets_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DataGridView.HitTestInfo info = gvTickets.HitTest(e.X, e.Y);
                if (info.RowIndex >= 0)
                {
                    AXSTicket view = (AXSTicket)gvTickets.Rows[info.RowIndex].DataBoundItem;
                    if (view != null)
                        gvTickets.DoDragDrop(view, DragDropEffects.Move);
                }
            }
        }

        private void gvTickets_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void gvGroups_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                try
                {
                    foreach (DataGridViewRow Row in gvGroups.Rows)
                    {
                        if (Row != null)
                        {
                            Row.DefaultCellStyle.BackColor = Color.White;
                            Row.DefaultCellStyle.ForeColor = Color.Black;
                            Row.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.SteelBlue;
                            Row.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            catch (Exception)
            {

            }
        }

        private void gvGroups_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow Row in gvGroups.Rows)
                {
                    if (Row != null)
                    {
                        Row.DefaultCellStyle.BackColor = Color.White;
                        Row.DefaultCellStyle.ForeColor = Color.Black;
                        Row.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.SteelBlue;
                        Row.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                    }
                }

                Point p = gvGroups.PointToClient(new Point(e.X, e.Y));
                DataGridView.HitTestInfo info = gvGroups.HitTest(p.X, p.Y);
                if (info.RowIndex > -1)
                {
                    DataGridViewRow Row = (DataGridViewRow)gvGroups.Rows[info.RowIndex];
                    if (Row != null)
                    {
                        Row.DefaultCellStyle.BackColor = Color.Orange;
                        Row.DefaultCellStyle.ForeColor = Color.White;
                        if (Row.Selected)
                        {
                            Row.DefaultCellStyle.SelectionBackColor = Color.Orange;
                            Row.DefaultCellStyle.SelectionForeColor = Color.White;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void gvGroups_SelectionChanged(object sender, EventArgs e)
        {
            filterOnSelectionChanged();
        }

        private void filterOnSelectionChanged()
        {
            if (gvGroups.SelectedRows != null)
            {
                if (gvGroups.SelectedRows.Count > 0)
                {
                    DataGridViewRow Row = gvGroups.SelectedRows[0];
                    if (Row.DataBoundItem != null)
                    {
                        TicketGroup group = (TicketGroup)Row.DataBoundItem;
                        this.AXSTicketBindingSource.DataSource = this.MainForm.AppStartUp.filterTickets(group);
                    }
                }
            }
        }
    }
}
