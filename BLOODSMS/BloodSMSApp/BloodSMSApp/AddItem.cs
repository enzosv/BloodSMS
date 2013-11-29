﻿using System;
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
    public partial class AddItem : Form
    {
        Storage storage;
        int taken_from;
        Donor d;
        public AddItem(Storage stor)
        {
            InitializeComponent();
            storage = stor;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (d != null)
            {
                //storage.AddBlood(dateAddedField.Value, dateExpireField.Value, taken_from);
                this.Hide();
            }
        }

        private void dateAddedField_ValueChanged(object sender, EventArgs e)
        {
            dateExpireField.MinDate = dateAddedField.Value;
        }

        private void dateExpireField_ValueChanged(object sender, EventArgs e)
        {
            dateAddedField.MaxDate = dateExpireField.Value;
        }

        private void takenFromField_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(takenFromField.Text, out taken_from))
            {
               
                if (storage.findDonor(taken_from) != null)
                {
                    d = storage.findDonor(taken_from);
                    listBox1.Items.Add("Name: " + d.Name);
                    listBox1.Enabled = true;
                }
                
            }
        }

    }
}
