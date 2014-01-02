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
        Donor x;
        MainMenu parent;
        public AddDonor(Storage stor, string lName, string fName, string mInitial)
        {
            InitializeComponent();
            storage = stor;
            lastName = lName;
            firstName = fName;
            middleInitial = mInitial;
        }

        public AddDonor(MainMenu mm, Donor d)
        {
            InitializeComponent();
            parent = mm;
            storage = parent.storage;
            x = d;
            lastName = d.Last_name;
            firstName = d.First_name;
            middleInitial = d.Middle_initial;
        }

        void populateData()
        {
            nameLabel.Text = lastName + ", " + firstName + " " + middleInitial;
            if (x == null)
            {
                x = storage.findDonorWithName(lastName, firstName, middleInitial);
            }
            if (x == null)
            {
                birthDateField.Value = DateTime.Today.AddYears(-16);
                bloodTypeField.SelectedIndex = 0;
                educationalAttainmentField.SelectedIndex = 0;
                hProvince.SelectedIndex = Enum.GetNames(typeof(province)).Length - 1;
                hCity.SelectedIndex = Enum.GetNames(typeof(city)).Length - 1;
                oProvince.SelectedIndex = Enum.GetNames(typeof(province)).Length - 1;
                oCity.SelectedIndex = Enum.GetNames(typeof(city)).Length - 1;
                editButton.Visible = false;
                deleteButton.Visible = false;
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
                viableNo.Checked = !x.Is_viable;
                contactableYes.Checked = x.Is_contactable;
                contactableNo.Checked = !x.Is_contactable;
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
                AddDonorButton.Visible = false;
                editButton.Visible = true;
                DisableEdit();
            }
        }

        void DisableEdit()
        {
            editButton.Text = "EDIT";
            deleteButton.Text = "DEFER";
            if (viableYes.Checked)
                deleteButton.Visible = true;
            else
                deleteButton.Visible = false;
            bloodTypeField.Enabled = false;
            educationalAttainmentField.Enabled = false;
            hProvince.Enabled = false;
            hCity.Enabled = false;
            oProvince.Enabled = false;
            oCity.Enabled = false;
            dateRegisteredField.Enabled = false;
            birthDateField.Enabled = false;
            nextAvailableField.Enabled = false;
            hStreetField.Enabled = false;
            oStreetField.Enabled = false;
            hLandlineField.Enabled = false;
            oLandlineField.Enabled = false;
            emailField.Enabled = false;
            cellphoneField.Enabled = false;
            viableYes.Enabled = false;
            viableNo.Enabled = false;
            contactableYes.Enabled = false;
            contactableNo.Enabled = false;
            defferalField.Enabled = false;
            
        }

        void EnableEdit()
        {
            editButton.Text = "SAVE";
            deleteButton.Visible = true;
            deleteButton.Text = "DELETE";
            
            bloodTypeField.Enabled = true;
            educationalAttainmentField.Enabled = true;
            hProvince.Enabled = true;
            hCity.Enabled = true;
            oProvince.Enabled = true;
            oCity.Enabled = true;
            dateRegisteredField.Enabled = true;
            birthDateField.Enabled = true;
            nextAvailableField.Enabled = true;
            hStreetField.Enabled = true;
            oStreetField.Enabled = true;
            hLandlineField.Enabled = true;
            oLandlineField.Enabled = true;
            emailField.Enabled = true;
            cellphoneField.Enabled = true;
            viableYes.Enabled = true;
            viableNo.Enabled = true;
            contactableYes.Enabled = true;
            contactableNo.Enabled = true;
            defferalField.Enabled = true;
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


        Donor GenerateDonorFromFields()
        {
            int blood_type = (int)MyEnums.GetValueFromDescription<bloodType>(bloodTypeField.Text);
            int educational_attainment = (int)MyEnums.GetValueFromDescription<educationalAttainment>(educationalAttainmentField.Text);
            int h_province = (int)MyEnums.GetValueFromDescription<province>(hProvince.Text);
            int h_city = (int)MyEnums.GetValueFromDescription<city>(hCity.Text);
            int o_province = (int)MyEnums.GetValueFromDescription<province>(oProvince.Text);
            int o_city = (int)MyEnums.GetValueFromDescription<city>(oCity.Text);

            if ((!viableYes.Checked && isStringValid(defferalField.Text, 2)) || (viableYes.Checked))
            {
                if ((isStringValid(emailField.Text, 6) && isEmailValid(emailField.Text)) || String.IsNullOrEmpty(emailField.Text))
                {
                    if (isStringValid(cellphoneField.Text, 10) || String.IsNullOrEmpty(cellphoneField.Text))
                    {
                        if (isStringValid(hLandlineField.Text, 7) || String.IsNullOrEmpty(hLandlineField.Text))
                        {
                            if (isStringValid(oLandlineField.Text, 7) || String.IsNullOrEmpty(oLandlineField.Text))
                            {
                                Donor d = new Donor(id, lastName, firstName, middleInitial, blood_type, h_province, h_city, hStreetField.Text, o_province, o_city, oStreetField.Text, hLandlineField.Text, oLandlineField.Text, emailField.Text, cellphoneField.Text, educational_attainment, birthDateField.Value, dateRegisteredField.Value, nextAvailableField.Value, contactableYes.Checked, viableYes.Checked, defferalField.Text);
                                return d;
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
            return null;

        }
        private void AddDonorButton_Click(object sender, EventArgs e)
        {
            Donor d = GenerateDonorFromFields();
            if (d != null)
            {
                if (!duplicate)
                {
                    if (storage.AddDonor(d))
                    {
                        MessageBox.Show("Donor added");
                        Close();
                    }
                    else
                        MessageBox.Show("Failed to add new donor");
                }
                else
                {
                    if (storage.UpdateDonor(d))
                    {
                        MessageBox.Show("Updated Donor");
                        Close();
                    }
                    else
                        MessageBox.Show("Failed to update donor");
                }
            }
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
            if (editButton.Text == "EDIT")
                Close();
            else
                DisableEdit();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (editButton.Text == "EDIT")
                EnableEdit();
            else
            {
                Donor d = GenerateDonorFromFields();
                if(d!=null)
                {
                    if (storage.UpdateDonor(d))
                    {
                        x = d;
                        parent.RefreshStorage();
                        populateData();
                        DisableEdit();
                        MessageBox.Show("Donor Updated");
                    }
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (deleteButton.Text == "DELETE")
            {
                if (storage.DeleteDonorWithId(id))
                {
                    MessageBox.Show("Donor removed from database");
                    Close();
                }
            }
            else
            {
                DeferDonor dd = new DeferDonor(storage, x);
                dd.ShowDialog();
                parent.RefreshStorage();
                populateData();
            }
        }

    }
}
