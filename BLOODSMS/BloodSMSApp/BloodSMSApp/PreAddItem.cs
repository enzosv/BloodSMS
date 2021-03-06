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
    public partial class PreAddItem : Form
    {
        Storage storage;
        MainMenu parent;
        public PreAddItem(MainMenu mainmenu)
        {
            InitializeComponent();
            parent = mainmenu;
            storage = parent.storage;
            dateTimePicker1.MaxDate = DateTime.Today;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (aNumber.Text.Length > 0)
            {
                if (storage.findBlood(aNumber.Text) == null)
                {
                    if (fromDonor.Checked)
                    {
                        if (lName.Text.Length > 0 && fName.Text.Length > 0 && mInitial.Text.Length > 0)
                        {
                            Donor d = storage.findDonorWithName(lName.Text, fName.Text, mInitial.Text);
                            if (d == null)
                            {
                                MessageBox.Show("Please Add the donor first and try again later");
                                AddDonor ad = new AddDonor(storage, lName.Text, fName.Text, mInitial.Text);
                                ad.ShowDialog();
                            }
                            else if (d.Is_viable)
                            {
                                //MessageBox.Show("Next Available: " + d.Next_available.ToString() + ". " + dateTimePicker1.Value.ToString());
                                if (d.Next_available <= dateTimePicker1.Value)
                                {
                                    AddItem a = new AddItem(storage, d, aNumber.Text, dateTimePicker1.Value);
                                    parent.RefreshDonorGrid(storage.donorList);
                                    a.ShowDialog();
                                    parent.RefreshOverview();
                                    Close();
                                }
                                else
                                    MessageBox.Show("Donor is not yet valid for donations\nNext available donation for donor is " + d.Next_available.ToString("MMMM d, yyyy"));
                            }
                            else
                            {
                                MessageBox.Show("Donor is not viable for donations");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please input name of donor");
                        }
                    }
                    else
                    {
                        AddItem a = new AddItem(storage, aNumber.Text, dateTimePicker1.Value);
                        a.ShowDialog();
                        parent.RefreshOverview();
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
