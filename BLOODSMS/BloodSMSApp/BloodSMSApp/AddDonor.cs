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
        bool duplicate;
        int id;
        public AddDonor(Storage stor, string lName, string fName, string mInitial)
        {
            InitializeComponent();
            storage = stor;
            lastName = lName;
            firstName = fName;
            middleInitial = mInitial;
            nameLabel.Text = lastName + ", " + firstName + " " + middleInitial;
        }

        public AddDonor(Storage stor, Donor d)
        {
            InitializeComponent();
            storage = stor;
            nameLabel.Text = d.Last_name + ", " + d.First_name + " " + d.Middle_initial;
        }

        void populateData()
        {
            Donor x = storage.findDonorWithName(lastName, firstName, middleInitial);
            if (x == null)
            {
                birthDateField.Value = DateTime.Today.AddYears(-16);
                bloodTypeField.SelectedIndex = 0;
                educationalAttainmentField.SelectedIndex = 0;
                hProvince.SelectedIndex = Enum.GetNames(typeof(province)).Length - 1;
                hCity.SelectedIndex = Enum.GetNames(typeof(city)).Length - 1;
                oProvince.SelectedIndex = Enum.GetNames(typeof(province)).Length - 1;
                oCity.SelectedIndex = Enum.GetNames(typeof(city)).Length - 1;
                duplicate = false;
            }
            else
            {
                //this code should be the same as show donor
                id = x.Donor_id;
                bloodTypeField.SelectedIndex = (int)x.Blood_type;
                educationalAttainmentField.SelectedIndex = (int)x.Educational_attainment;
                hProvince.SelectedIndex = (int)x.Home_province;
                hCity.SelectedIndex = (int)x.Home_city;
                oProvince.SelectedIndex = (int)x.Office_province;
                oCity.SelectedIndex = (int)x.Office_city;
                dateRegisteredField.Value = x.Date_registered;
                birthDateField.Value = x.Birth_date;
                nextAvailableField.Value = x.Next_available;
                hStreetField.Text = x.Home_street;
                oStreetField.Text = x.Office_street;
                hLandlineField.Text = x.Home_landline;
                oLandlineField.Text = x.Office_landline;
                emailField.Text = x.Email;
                cellphoneField.Text = x.Cellphone;
                viableYes.Checked = x.Is_viable;
                contactableYes.Checked = x.Is_contactable;
                defferalField.Text = x.Reason_for_deferral;

                ageText.Visible = true;
                ageText.Text = "Age: " + x.Age;
                int numDonations = storage.getNumDonations(x);
                if (numDonations > 0)
                {
                    timesDonated.Visible = true;
                    timesDonated.Text = "Times Donated: " + numDonations.ToString();
                }
                duplicate = true;
            }
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

        bool isStringValid(string s, int minLength)
        {
            return (!String.IsNullOrWhiteSpace(s) && s.Length>=minLength);
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

            if ((!viableYes.Checked && isStringValid(defferalField.Text,2)) || (viableYes.Checked))
            {
                if ((isStringValid(emailField.Text, 6) && isEmailValid(emailField.Text)) || String.IsNullOrEmpty(emailField.Text))
                {
                    if (isStringValid(cellphoneField.Text, 10) || String.IsNullOrEmpty(cellphoneField.Text))
                    {
                        if (isStringValid(hLandlineField.Text, 7) || String.IsNullOrEmpty(hLandlineField.Text))
                        {
                            if (isStringValid(oLandlineField.Text, 7) || String.IsNullOrEmpty(oLandlineField.Text))
                            {
                                Donor x = new Donor(id, lastName, firstName, middleInitial, blood_type, h_province, h_city, hStreetField.Text, o_province, o_city, oStreetField.Text, hLandlineField.Text, oLandlineField.Text, emailField.Text, cellphoneField.Text, educational_attainment, birthDateField.Value, dateRegisteredField.Value, nextAvailableField.Value, contactableYes.Checked, viableYes.Checked, defferalField.Text);
                                if (!duplicate)
                                {
                                    if (storage.AddDonor(x))
                                    {
                                        MessageBox.Show("Donor added");
                                        Close();
                                    }
                                    else
                                        MessageBox.Show("Failed to add new donor");
                                }
                                else
                                {
                                    if (storage.UpdateDonor(x))
                                    {
                                        MessageBox.Show("Updated Donor");
                                        Close();
                                    }
                                    else
                                        MessageBox.Show("Failed to update donor");
                                }
                            }
                            else
                                MessageBox.Show("Invalid office landline number");
                        }
                        else
                            MessageBox.Show("Invalid home landline number");
                    }
                    else
                        MessageBox.Show("Invalid cellphone number");
                    
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

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddDonor_Load(object sender, EventArgs e)
        {
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
            populateData();
        }

    }
}
