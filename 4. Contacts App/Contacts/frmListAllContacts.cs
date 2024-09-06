using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContactsBusinessLayer;

namespace Contacts
{
    public partial class frmListAllContacts : Form
    {
        public frmListAllContacts()
        {
            InitializeComponent();
        }

        private void _RefreshContactsData()
        {
            lvContactList.Items.Clear();

            DataTable dt = clsContact.GetAllContacts();

            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row["ContactID"].ToString());
                item.SubItems.Add(row["FirstName"].ToString());
                item.SubItems.Add(row["LastName"].ToString());
                item.SubItems.Add(row["Email"].ToString());
                item.SubItems.Add(row["Phone"].ToString());
                item.SubItems.Add(row["Address"].ToString());
                item.SubItems.Add(row["DateOfBirth"].ToString());
                item.SubItems.Add(row["CountryID"].ToString());
                item.SubItems.Add(row["ImagePath"].ToString());

                lvContactList.Items.Add(item);  
            }
        }

        private void frmListAllContacts_Load(object sender, EventArgs e)
        {
            _RefreshContactsData();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            clsAddEditContact form = new clsAddEditContact(-1);
            form.ShowDialog();

            _RefreshContactsData();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = lvContactList.SelectedItems[0];
            ListViewItem.ListViewSubItem firstCell = item.SubItems[0];
            string cellText = firstCell.Text;
            int ContactID = int.Parse(cellText);

            clsAddEditContact form = new clsAddEditContact(ContactID);
            form.ShowDialog();

            _RefreshContactsData();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = lvContactList.SelectedItems[0];
            ListViewItem.ListViewSubItem firstCell = item.SubItems[0];
            string cellText = firstCell.Text;
            int ContactID = int.Parse(cellText);

            if (MessageBox.Show("Are you sure you want to delete Contact [" + ContactID + "]", "Delete Contact",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsContact.DeleteContact(ContactID))
                    MessageBox.Show("Contact Deleted Successfully");
                else
                    MessageBox.Show("Contact Delete Failed");
            }

            _RefreshContactsData();
        }

        
    }
}
