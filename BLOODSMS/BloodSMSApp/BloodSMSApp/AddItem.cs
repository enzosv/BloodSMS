using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Blood_SMS;

namespace BloodSMSApp
{
    public partial class AddItem : Form
    {
        Storage storage;
        public AddItem(Storage stor)
        {
            InitializeComponent();
            storage = stor;
        }

        void isNumber(KeyPressEventArgs e)
        {
            e.Handled = !(Char.IsNumber(e.KeyChar) || e.KeyChar == 8);
        }

        bool isStringValid(string s, int minLength)
        {
            return (!String.IsNullOrWhiteSpace(s) && s.Length >= minLength);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int blood_type = (int)MyEnums.GetValueFromDescription<bloodType>(bTypeField.Text);
            if (isStringValid(t_accessionNumber.Text, 1))
            {
                if (String.IsNullOrEmpty(takenFromField.Text) || isStringValid(takenFromField.Text, 1))
                {
                    if (isStringValid(pLast.Text, 1) && isStringValid(pFirst.Text, 1) && isStringValid(t_patientAge.Text, 1))
                    {
                        int age;
                        if (int.TryParse(t_patientAge.Text, out age))
                        {
                            Blood b = new Blood(t_accessionNumber.Text, blood_type, storage.findDonorWithName(, dateAddedField.Value);
                            if (storage.AddBlood(b, dateExpireField.Value, pLast.Text, pFirst.Text, pMid.Text, age))
                            {
                                MessageBox.Show("Blood added");
                                Close();
                            }
                        }
                    }
                    else if (String.IsNullOrEmpty(pLast.Text))
                    {
                        Blood b = new Blood(t_accessionNumber.Text, blood_type, dateAddedField.Value);
                    }
                }
            }
        }

        private void dateAddedField_ValueChanged(object sender, EventArgs e)
        {
            dateExpireField.MinDate = dateAddedField.Value;
        }

        private void dateExpireField_ValueChanged(object sender, EventArgs e)
        {
            dateAddedField.MaxDate = dateExpireField.Value;
        }

        private void t_accessionNumber_TextChanged(object sender, EventArgs e)
        {
            Blood b = storage.findBlood(t_accessionNumber.Text);
            if (b != null)
            {
                if (b.Donor_id.HasValue && storage.findDonor(b.Donor_id.Value) != null)
                {
                    Donor d = storage.findDonor(b.Donor_id.Value);
                    takenFromField.Text = d.Name;
                    listBox1.Items.Add("Date Registered: " + d.Date_registered);
                    //listBox1.Items.Add("Date Registered: " + d.Date_registered);
                }
                if (b.components[0].Date_assigned != DateTime.MinValue)
                {
                    pLast.Text = b.components[0].Patient_name;
                    t_patientAge.Text = b.components[0].Patient_age.ToString();
                }
                dateAddedField.Value = b.Date_donated;
                dateExpireField.Value = b.components[0].Date_expired;
            }
        }

        private void b_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void t_patientAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            isNumber(e);
        }



    }
}
