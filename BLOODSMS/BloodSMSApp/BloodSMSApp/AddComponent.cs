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
    public partial class AddComponent : Form
    {
        Storage storage;
        Blood blood;
        public AddComponent(Storage stor, Blood b, List<string> bc)
        {
            InitializeComponent();
            storage = stor;
            blood = b;
            foreach (string cName in bc)
            {
                componentName.Items.Add(cName);
            }
            componentName.SelectedIndex = 0;
        }

        private void b_add_Click(object sender, EventArgs e)
        {
            if (storage.AddComponent(blood.Accession_number, (int)MyEnums.GetValueFromDescription<bloodComponents>(componentName.Text), dateTimePicker1.Value, dateTimePicker2.Value))
            {
                MessageBox.Show("Component successfully added");
                Close();
            }
            else
                MessageBox.Show("Component could not be added. Please try again");
            
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
