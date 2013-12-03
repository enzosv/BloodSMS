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
    public partial class PreAddItem : Form
    {
        Storage storage;
        public PreAddItem(Storage stor)
        {
            InitializeComponent();
            storage = stor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (aNumber.Text.Length > 1)
            {
                if (storage.findBlood(aNumber.Text) == null)
                {
                    if (fromDonor.Checked)
                    {
                        Donor d = storage.findDonorWithName(lName.Text, fName.Text, mInitial.Text);
                        if (d == null)
                        {
                            AddDonor ad = new AddDonor(storage, lName.Text, fName.Text, mInitial.Text);
                            ad.ShowDialog();
                            MessageBox.Show("Please Add the donor first and try again later");
                        }
                        else if (d.Is_viable)
                        {
                            AddItem a = new AddItem(storage, d, aNumber.Text);
                            a.ShowDialog();
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Donor is not viable for donations");
                        }
                    }
                    else
                    {
                        AddItem a = new AddItem(storage, aNumber.Text);
                        a.ShowDialog();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Blood with accession number already exists");
                }
            }
            else
            {
                MessageBox.Show("Invalid Accession Number");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void fromDonor_CheckedChanged(object sender, EventArgs e)
        {
            lName.Enabled = fromDonor.Checked;
            fName.Enabled = fromDonor.Checked;
            mInitial.Enabled = fromDonor.Checked;

        }
    }
}
