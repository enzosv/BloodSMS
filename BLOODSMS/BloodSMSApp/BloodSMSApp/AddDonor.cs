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
            bloodTypeField.DataSource = Enum.GetValues(typeof(bloodType));
            preferredContactField.DataSource = Enum.GetValues(typeof(contactMethod));
        }

        private void AddDonorButton_Click(object sender, EventArgs e)
        {
            if (nameField.Text.Length > 1 &&
                hProvinceField.Text.Length > 1 &&
                hCityField.Text.Length > 1)
                storage.AddDonor(nameField.Text, (bloodType)bloodTypeField.SelectedValue, hProvinceField.Text, hCityField.Text, hStreetField.Text, oProvinceField.Text, oCityField.Text, oStreetField.Text, (contactMethod)preferredContactField.SelectedValue, hLandlineField.Text, oLandlineField.Text, emailField.Text, cellphoneField.Text, educationalAttainmentField.SelectedText, birthDateField.Value, dateRegisteredField.Value, contactableYes.Checked, viableYes.Checked, defferalField.Text);
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
