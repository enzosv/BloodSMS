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
        Donor donor;
        bool hasDonor;
        public AddItem(Storage stor, string accNumber)
        {
            InitializeComponent();
            storage = stor;
            t_accessionNumber.Text = accNumber;

            foreach (bloodType x in (bloodType[])Enum.GetValues(typeof(bloodType)))
            {
                bTypeField.Items.Add(MyEnums.GetDescription(x));
            }
            bTypeField.SelectedIndex = 0;
            hasDonor = false;
            d_last.Enabled = false;
            dFirst.Enabled = false;
            dMid.Enabled = false;
        }

        public AddItem(Storage stor, Donor d, string accNumber)
        {
            InitializeComponent();   
            storage = stor;
            donor = d;
            t_accessionNumber.Text = accNumber;

            foreach (bloodType x in (bloodType[])Enum.GetValues(typeof(bloodType)))
            {
                bTypeField.Items.Add(MyEnums.GetDescription(x));
            }
            bTypeField.SelectedIndex = (int)d.Blood_type;
            d_last.Text = d.Last_name;
            dFirst.Text = d.First_name;
            dMid.Text = d.Middle_initial;
            hasDonor = true;
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
            //if patient
            if (isStringValid(pLast.Text, 1) && isStringValid(pFirst.Text, 1) && isStringValid(t_patientAge.Text, 1))
            {
                int age;
                if (int.TryParse(t_patientAge.Text, out age))
                {
                    Blood b;
                    if (hasDonor)
                        b = new Blood(t_accessionNumber.Text, blood_type, donor.Donor_id, dateAddedField.Value);
                    else
                        b = new Blood(t_accessionNumber.Text, blood_type, dateAddedField.Value);
                    
                    if (storage.AddBlood(b, dateExpireField.Value, pLast.Text, pFirst.Text, pMid.Text, age))
                    {
                        MessageBox.Show("Blood added");
                        Close();
                    }
                }
            }
            //if no patient
            else if (String.IsNullOrEmpty(pLast.Text) && String.IsNullOrEmpty(pFirst.Text) && String.IsNullOrEmpty(t_patientAge.Text))
            {
                Blood b;
                if (hasDonor)
                    b = new Blood(t_accessionNumber.Text, blood_type, donor.Donor_id, dateAddedField.Value);
                else
                    b = new Blood(t_accessionNumber.Text, blood_type, dateAddedField.Value);
                if (storage.AddBlood(b, dateExpireField.Value))
                {
                    MessageBox.Show("Blood added");
                    Close();
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
