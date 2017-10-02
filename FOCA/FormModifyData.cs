using System;
using System.Windows.Forms;
using MetadataExtractCore.Diagrams;

namespace FOCA
{
    public partial class FormModifyData : Form
    {
        private readonly Applications aLst;
        private readonly Descriptions dLst;
        private readonly Type _type;
        private readonly Users uLst;
        public object Result;

        public FormModifyData()
        {
            InitializeComponent();
        }

        public FormModifyData(Users u) : this()
        {
            uLst = u;
            _type = Type.Users;
            foreach (var ui in u.Items)
            {
                var lvi = lvwValues.Items.Add(ui.Name);
                lvi.SubItems.Add(ui.Notes);
                lvi.Tag = ui;
            }
        }

        public FormModifyData(Applications a) : this()
        {
            aLst = a;
            _type = Type.Applications;
            foreach (var ai in a.Items)
            {
                var lvi = lvwValues.Items.Add(ai.Name);
                lvi.SubItems.Add(ai.Source);
                lvi.Tag = ai;
            }
        }

        public FormModifyData(Descriptions d) : this()
        {
            dLst = d;
            _type = Type.Summary;
            foreach (var di in d.Items)
            {
                var lvi = lvwValues.Items.Add(di.Description);
                lvi.SubItems.Add(di.Source);
                lvi.Tag = di;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            uLst.Items.Clear();
            switch (_type)
            {
                case Type.Summary:
                    foreach (ListViewItem lvi in lvwValues.Items)
                    {
                        dLst.Items.Add(lvi.Tag as DescriptionsItem);
                    }
                    break;
                case Type.Users:

                    foreach (ListViewItem lvi in lvwValues.Items)
                    {
                        uLst.Items.Add(lvi.Tag as UserItem);
                    }
                    break;
                case Type.Applications:
                    foreach (ListViewItem lvi in lvwValues.Items)
                    {
                        aLst.Items.Add(lvi.Tag as ApplicationsItem);
                    }
                    break;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var frm = new FormValueNotes();
            ListViewItem lvi;
            switch (_type)
            {
                case Type.Users:
                {
                    if (frm.ShowDialog() != DialogResult.OK) return;
                    var ui = new UserItem
                    {
                        Name = frm.Value,
                        Notes = frm.Notes,
                        IsComputerUser = true
                    };
                    lvi = lvwValues.Items.Add(ui.Name);
                    lvi.SubItems.Add(ui.Notes);
                    lvi.Tag = ui;
                    break;
                }
                case Type.Applications:
                {
                    if (frm.ShowDialog() != DialogResult.OK) return;
                    var ai = new ApplicationsItem(frm.Value, frm.Notes);
                    lvi = lvwValues.Items.Add(ai.Name);
                    lvi.SubItems.Add(ai.Source);
                    lvi.Tag = ai;
                    break;
                }
                case Type.Summary:
                {
                    if (frm.ShowDialog() != DialogResult.OK) return;
                    var di = new DescriptionsItem(frm.Value, frm.Notes);
                    lvi = lvwValues.Items.Add(di.Description);
                    lvi.SubItems.Add(di.Source);
                    lvi.Tag = di;
                    break;
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lvwValues.SelectedItems.Count <= 0) return;
            foreach (ListViewItem lvi in lvwValues.SelectedItems)
                lvi.Remove();
        }

        private enum Type
        {
            Users,
            Applications,
            Summary
        }
    }
}