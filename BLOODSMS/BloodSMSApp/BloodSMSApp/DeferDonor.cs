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
    public partial class DeferDonor : Form
    {
        Donor donor;
        Storage storage;
        public DeferDonor(Storage stor, Donor d)
        {
            InitializeComponent();
            storage = stor;
            donor = d;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length > 1)
            {
                donor.Is_viable = false;
                donor.Reason_for_deferral = richTextBox1.Text;
                if (storage.UpdateDonor(donor))
                {
                    MessageBox.Show("Donor successfully deferred");
                    Close();
                }
                else
                {
                    MessageBox.Show("Error defferring donor. Please try again");
                }
            }
            else
            {
                MessageBox.Show("Please provide a reason for defferal");
            }
            
        }

        private void DeferDonor_Load(object sender, EventArgs e)
        {
            label3.Text = donor.Name;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
