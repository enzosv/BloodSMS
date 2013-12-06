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
    public partial class Assign_Form : Form
    {
        Storage storage;
        Blood_SMS.Component component;
        public Assign_Form(Storage stor, Blood_SMS.Component c)
        {
            InitializeComponent();
            storage = stor;
            component = c;
        }

        private void Assign_Form_Load(object sender, EventArgs e)
        {
            aNumber.Text = component.Accession_number;
            cName.Text = MyEnums.GetDescription(component.Component_name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int age;
            if (lName.Text.Length > 0 && fName.Text.Length > 0 && mInitial.Text.Length > 0)
            {
                if (int.TryParse(textBox1.Text, out age))
                {
                    component.Assign(lName.Text, fName.Text, mInitial.Text, age, dateTimePicker1.Value);
                    if (storage.UpdateComponent(component))
                    {
                        MessageBox.Show("Component successfully assigned to patient");
                        Close();
                    }
                    else
                        MessageBox.Show("Component could not be assigned. Please try again");
                }
                else
                    MessageBox.Show("Please provide patient age");
            }
            else
                MessageBox.Show("Please provide patient name");
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(Char.IsNumber(e.KeyChar) || e.KeyChar == 8);
        }
    }
}
