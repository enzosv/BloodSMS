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
    public partial class AddDonor : Form
    {
        Storage storage;
        public AddDonor(Storage stor)
        {
            InitializeComponent();
            storage = stor;
            foreach (bloodType x in (bloodType[])Enum.GetValues(typeof(bloodType)))
            {
                bloodTypeField.Items.Add(MyEnums.GetDescription(x));
            }
            foreach (contactMethod x in (contactMethod[])Enum.GetValues(typeof(contactMethod)))
            {
                preferredContactField.Items.Add(MyEnums.GetDescription(x));
            }
            foreach (educationalAttainment x in (educationalAttainment[])Enum.GetValues(typeof(educationalAttainment)))
            {
                educationalAttainmentField.Items.Add(MyEnums.GetDescription(x));
            }
            bloodTypeField.SelectedIndex = 1;
            preferredContactField.SelectedIndex = 1;
            educationalAttainmentField.SelectedIndex = 1;
        }

        private void AddDonorButton_Click(object sender, EventArgs e)
        {
            int blood_type = (int)MyEnums.GetValueFromDescription<bloodType>(bloodTypeField.Text);
            int preferred_contact_method = (int)MyEnums.GetValueFromDescription<contactMethod>(preferredContactField.Text);
            int educational_attainment = (int)MyEnums.GetValueFromDescription<educationalAttainment>(educationalAttainmentField.Text);
            //defferalField.Text = storage.AddQuery(new string[]{ "name", "blood_type", "home_province", "home_city", "home_street", "office_province", "office_city", "office_street", "preferred_contact_method", "home_landline", "office_landline", "cellphone", "educational_attainment", "birth_date", "date_registered", "last_donation", "next_available", "times_donated", "is_contactable", "is_viable", "reason_for_deferral" });
            if (lastnameField.Text.Length > 1 &&
                hProvinceField.Text.Length > 1 &&
                hCityField.Text.Length > 1)
            {
                if (storage.AddDonor(lastnameField.Text, firstNameField.Text, middleInitialField.Text, blood_type, hProvinceField.Text, hCityField.Text, hStreetField.Text, hProvinceField.Text, oCityField.Text, oStreetField.Text, preferred_contact_method, hLandlineField.Text, oLandlineField.Text, emailField.Text, cellphoneField.Text, educational_attainment, birthDateField.Value, dateRegisteredField.Value, contactableYes.Checked, viableYes.Checked, defferalField.Text))
                {
                    MessageBox.Show("DONOR ADDED");
                    Hide();
                }
                else
                    MessageBox.Show("FAIL");
            }
        }

        private void oLandlineField_TextChanged(object sender, EventArgs e)
        {
            {
                bool enteredLetter = false;
                Queue<char> text = new Queue<char>();
                foreach (var ch in this.oLandlineField.Text)
                {
                    if (char.IsDigit(ch))
                    {
                        text.Enqueue(ch);
                    }
                    else
                    {
                        enteredLetter = true;
                    }
                }

                if (enteredLetter)
                {
                    StringBuilder sb = new StringBuilder();
                    while (text.Count > 0)
                    {
                        sb.Append(text.Dequeue());
                    }

                    this.oLandlineField.Text = sb.ToString();
                    this.oLandlineField.SelectionStart = this.oLandlineField.Text.Length;
                }
            }
        }

        private void cellphoneField_TextChanged(object sender, EventArgs e)
        {
            bool enteredLetter = false;
            Queue<char> text = new Queue<char>();
            foreach (var ch in this.cellphoneField.Text)
            {
                if (char.IsDigit(ch))
                {
                    text.Enqueue(ch);
                }
                else
                {
                    enteredLetter = true;
                }
            }

            if (enteredLetter)
            {
                StringBuilder sb = new StringBuilder();
                while (text.Count > 0)
                {
                    sb.Append(text.Dequeue());
                }

                this.cellphoneField.Text = sb.ToString();
                this.cellphoneField.SelectionStart = this.cellphoneField.Text.Length;
            }
        }
    }
}
