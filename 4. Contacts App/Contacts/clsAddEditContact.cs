using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContactsBusinessLayer;

namespace Contacts
{
    public partial class clsAddEditContact : Form
    {
        enum enMode { AddNew = 0, Update = 1 }
        enMode _Mode;

        int _ContactID;
        clsContact _Contact;

        public clsAddEditContact(int ContactID)
        {
            InitializeComponent();
            
            _ContactID = ContactID;

            if (_ContactID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        private void _LoadCountries()
        {
            DataTable dt = clsCountry.GetAllCountries();

            foreach (DataRow RecordRow in dt.Rows)
            {
                cmbCountry.Items.Add(RecordRow["CountryName"].ToString());
            }
        }

        private void _LoadData()
        {
            _LoadCountries();
            cmbCountry.SelectedIndex = 0;   

            if (_Mode == enMode.AddNew)
            {
                lblContactMode.Text = "Add New Contact";
                _Contact = new clsContact();
                return;
            }

            _Contact = clsContact.Find(_ContactID);

            if (_Contact == null)
            {
                MessageBox.Show("this form will be closed because No Contact with ID = " + _ContactID);
                this.Close();   
            }
            // check this line
            lblContactMode.Text = "Edit Contact ID=" + _ContactID;
            lblContactID.Text = _ContactID.ToString();
            txtFirstName.Text = _Contact.FirstName;
            txtLastName.Text = _Contact.LastName;
            txtEmail.Text = _Contact.Email;
            txtPhone.Text  = _Contact.Phone;
            dtpDateOfBirth.Value = _Contact.DateOfBirth;
            txtAddress.Text = _Contact.Address;

            if (_Contact.ImagePath != "")
                pbImage.Load(_Contact.ImagePath);

            cmbCountry.SelectedIndex = cmbCountry.FindString(clsCountry.Find(_Contact.CountryID).CountryName);

            llRemove.Visible = (_Contact.ImagePath != "");
            
        }



        private void clsAddEditContact_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int CountryID = clsCountry.Find(cmbCountry.Text).ID;
            _Contact.FirstName = txtFirstName.Text;
            _Contact.LastName = txtLastName.Text;
            _Contact.Email = txtEmail.Text;
            _Contact.Phone = txtPhone.Text;
            _Contact.Address = txtAddress.Text;
            _Contact.DateOfBirth = dtpDateOfBirth.Value;
            _Contact.CountryID = CountryID;

            if (pbImage.ImageLocation != null)
                _Contact.ImagePath = pbImage.ImageLocation;
            else
                _Contact.ImagePath = "";

            if (_Contact.Save())
                MessageBox.Show("Data Saved Successfully.");
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.");

            _Mode = enMode.Update;
            lblContactMode.Text = "Edit Contact ID=" + _Contact.ID;
            lblContactID.Text = _Contact.ID.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.InitialDirectory = @"c:\";

            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*png;*gif;*.bmp";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbImage.Load(openFileDialog1.FileName);
            }
        }

        private void llRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbImage.ImageLocation = null;
            llRemove.Visible = false;
        }
    }
}
