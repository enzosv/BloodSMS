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
        }

        private void AddDonorButton_Click(object sender, EventArgs e)
        {
            //if(nameField.Text.Length > 1 &&
            //    hProvinceField.Text.Length > 1 &&
            //    hCityField.Text.Length >1)
            //    storage.AddDonor(nameField.Text, bloodTypeField.SelectedValue, hProvinceField.Text, hCityField.Text, hStreetField.Text, oProvinceField.Text, oCityField.Text, oStreetField.Text, preferredContactField.SelectedValue, hLandlineField.Text, oLandlineField.Text, emailField.Text, cellphoneField.Text, educationalAttainmentField.SelectedText, birthDateField.Value, dateRegisteredField.Value);

        }
    }
}
