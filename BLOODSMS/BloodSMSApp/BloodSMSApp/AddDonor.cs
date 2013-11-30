using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Blood_SMS;
namespace BloodSMSApp
{
    public partial class AddDonor : Form
    {
        Storage storage;
        string oldDefferalText;
        string lastName, firstName, middleInitial;
        public AddDonor(Storage stor, string lName, string fName, string mInitial)
        {
            InitializeComponent();
            storage = stor;
            lastName = lName;
            firstName = fName;
            middleInitial = mInitial;
            nameLabel.Text = lastName + ", " + firstName + " " + middleInitial; 
            foreach (bloodType x in (bloodType[])Enum.GetValues(typeof(bloodType)))
            {
                bloodTypeField.Items.Add(MyEnums.GetDescription(x));
            }
            foreach (educationalAttainment x in (educationalAttainment[])Enum.GetValues(typeof(educationalAttainment)))
            {
                educationalAttainmentField.Items.Add(MyEnums.GetDescription(x));
            }
            foreach (province x in (province[])Enum.GetValues(typeof(province)))
            {
                hProvince.Items.Add(MyEnums.GetDescription(x));
                oProvince.Items.Add(MyEnums.GetDescription(x));
            }
            foreach (city x in (city[])Enum.GetValues(typeof(city)))
            {
                hCity.Items.Add(MyEnums.GetDescription(x));
                oCity.Items.Add(MyEnums.GetDescription(x));
            }
            bloodTypeField.SelectedIndex = 0;
            educationalAttainmentField.SelectedIndex = 0;
            hProvince.SelectedIndex = Enum.GetNames(typeof(province)).Length-1;
            hCity.SelectedIndex = Enum.GetNames(typeof(city)).Length - 1;
            oProvince.SelectedIndex = Enum.GetNames(typeof(province)).Length - 1;
            oCity.SelectedIndex = Enum.GetNames(typeof(city)).Length - 1;
        }

        bool isEmailValid(string emailaddress)
        {
            try
            {
                System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        bool isStringValid(string s)
        {
            return (!String.IsNullOrWhiteSpace(s));
        }

        void isNumber(KeyPressEventArgs e)
        {
            e.Handled = !(Char.IsNumber(e.KeyChar) || e.KeyChar == 8);
        }

        private void AddDonorButton_Click(object sender, EventArgs e)
        {
            int blood_type = (int)MyEnums.GetValueFromDescription<bloodType>(bloodTypeField.Text);
            int educational_attainment = (int)MyEnums.GetValueFromDescription<educationalAttainment>(educationalAttainmentField.Text);
            int h_province = (int)MyEnums.GetValueFromDescription<province>(hProvince.Text);
            int h_city = (int)MyEnums.GetValueFromDescription<city>(hCity.Text);
            int o_province = (int)MyEnums.GetValueFromDescription<province>(oProvince.Text);
            int o_city = (int)MyEnums.GetValueFromDescription<city>(oCity.Text);
            //defferalField.Text = storage.AddQuery(new string[]{ "name", "blood_type", "home_province", "home_city", "home_street", "office_province", "office_city", "office_street", "preferred_contact_method", "home_landline", "office_landline", "cellphone", "educational_attainment", "birth_date", "date_registered", "last_donation", "next_available", "times_donated", "is_contactable", "is_viable", "reason_for_deferral" });

            if ((!viableYes.Checked && isStringValid(defferalField.Text) && isStringValid(defferalField.Text)) || (viableYes.Checked))
            {
                if ((isStringValid(emailField.Text) && isEmailValid(emailField.Text)) || String.IsNullOrEmpty(emailField.Text))
                {
                    if (storage.AddDonor(lastName, firstName, middleInitial, blood_type, h_province, h_city, hStreetField.Text, o_province, o_city, oStreetField.Text, hLandlineField.Text, oLandlineField.Text, emailField.Text, cellphoneField.Text, educational_attainment, birthDateField.Value, dateRegisteredField.Value, contactableYes.Checked, viableYes.Checked, defferalField.Text))
                    {
                        MessageBox.Show("DONOR ADDED");
                        Close();
                    }
                    else
                        MessageBox.Show("FAIL");
                }
                else
                    MessageBox.Show("Invalid Email");
            }
            else
                MessageBox.Show("Please provide a reason for defferal");

        }

        private void oLandlineField_KeyPress(object sender, KeyPressEventArgs e)
        {
            isNumber(e);
        }

        private void hLandlineField_KeyPress(object sender, KeyPressEventArgs e)
        {
            isNumber(e);
        }

        private void cellphoneField_KeyPress(object sender, KeyPressEventArgs e)
        {
            isNumber(e);
        }

        private void viableYes_CheckedChanged(object sender, EventArgs e)
        {
            if (viableYes.Checked)
            {
                contactPanel.Enabled = true;
                oldDefferalText = defferalField.Text;
                defferalField.Text = "";
                defferalPanel.Enabled = false;
            }
            else
            {
                contactPanel.Enabled = false;
                defferalPanel.Enabled = true;
                defferalField.Text = oldDefferalText;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
