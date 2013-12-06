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
    public partial class RemoveItem : Form
    {
        Storage storage;
        Blood_SMS.Component component;
        removalType removeType;
        public RemoveItem(Storage stor, Blood_SMS.Component c, removalType rt)
        {
            InitializeComponent();
            storage = stor;
            component = c;
            removeType = rt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length > 0)
            {
                switch (removeType)
                {
                    case removalType.Quarantined:
                        component.Quarantine(dateTimePicker1.Value, richTextBox1.Text);
                        break;
                    case removalType.Released:
                        component.Release(dateTimePicker1.Value, richTextBox1.Text);
                        break;
                    case removalType.Reprocessed:
                        component.Reprocess(dateTimePicker1.Value, richTextBox1.Text);
                        break;
                }
                if (storage.UpdateComponent(component))
                {
                    MessageBox.Show("Component successfully updated");
                    Close();
                }
                else
                    MessageBox.Show("Error updating component. Please try again");
            }
            else
                MessageBox.Show("Please provide a " + reasonLabel.Text.ToLower());
        }

        private void RemoveItem_Load(object sender, EventArgs e)
        {
            switch (removeType)
            {
                case removalType.Quarantined:
                    titleLabel.Text = "QUARANTINE";
                    dateLabel.Text = "DATE QUARANTINED: ";
                    reasonLabel.Text = "REASON FOR QUARANTINE: ";
                    break;
                case removalType.Released:
                    titleLabel.Text = "RELEASE";
                    dateLabel.Text = "DATE RELEASED: ";
                    reasonLabel.Text = "NOTE: ";
                    break;
                case removalType.Reprocessed:
                    titleLabel.Text = "REPROCESS";
                    dateLabel.Text = "DATE REPROCESSED: ";
                    reasonLabel.Text = "NOTE: ";
                    break;
            }
            label3.Text = component.Accession_number;
            label4.Text = MyEnums.GetDescription(component.Component_name);
        }
    }
}
