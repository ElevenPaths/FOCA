using System;
using System.Windows.Forms;
using FOCA.Database.Entities;
using MetadataExtractCore.Diagrams;

namespace FOCA
{
    public partial class FormModifyData : Form
    {
        private readonly Applications Applications;
        private readonly Descriptions Descriptions;
        private readonly Type _type;
        private readonly Users Users;

        private enum Type
        {
            Users,
            Applications,
            Summary
        }

        public FormModifyData()
        {
            InitializeComponent();
        }

        public FormModifyData(Users u) : this()
        {
            Users = u;
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
            Applications = a;
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
            Descriptions = d;
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
            switch (_type)
            {
                case Type.Summary:
                    Descriptions.Items.Clear();
                    foreach (ListViewItem lvi in lvwValues.Items)
                    {
                        Descriptions.Items.Add(lvi.Tag as DescriptionsItem);
                    }
                    break;
                case Type.Users:
                    Users.Items.Clear();
                    foreach (ListViewItem lvi in lvwValues.Items)
                    {
                        
                        Users.Items.Add(lvi.Tag as UserItem);
                    }
                    break;
                case Type.Applications:
                    Applications.Items.Clear();
                    foreach (ListViewItem lvi in lvwValues.Items)
                    {
                        Applications.Items.Add(lvi.Tag as ApplicationsItem);
                    }
                    break;
            }
            DialogResult = DialogResult.OK;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var frm = new FormValueNotes();

            if (frm.ShowDialog() != DialogResult.OK) return;

            ListViewItem lvi;
            switch (_type)
            {
                case Type.Users:
                {
                    var user = new UserItem
                    {
                        Name = frm.Value,
                        Notes = frm.Notes,
                        IsComputerUser = true
                    };
                    lvi = lvwValues.Items.Add(user.Name);
                    lvi.SubItems.Add(user.Notes);
                    lvi.Tag = user;
                    break;
                }
                case Type.Applications:
                {
                    var application = new ApplicationsItem(frm.Value, frm.Notes);
                    lvi = lvwValues.Items.Add(application.Name);
                    lvi.SubItems.Add(application.Source);
                    lvi.Tag = application;
                    break;
                }
                case Type.Summary:
                {
                    var description = new DescriptionsItem(frm.Value, frm.Notes);
                    lvi = lvwValues.Items.Add(description.Description);
                    lvi.SubItems.Add(description.Source);
                    lvi.Tag = description;
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
    }
}