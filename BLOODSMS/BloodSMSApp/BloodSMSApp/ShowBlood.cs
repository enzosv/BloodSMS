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
    public partial class ShowBlood : Form
    {
        Storage storage;
        List<Blood> bloodList;
        Blood b;
        public ShowBlood(Storage stor)
        {
            InitializeComponent();
            storage = stor;
            foreach (bloodType x in (bloodType[])Enum.GetValues(typeof(bloodType)))
            {
                bloodTypeField.Items.Add(MyEnums.GetDescription(x));
            }
        }

        public ShowBlood(Storage stor, Blood blood)
        {
            InitializeComponent();
            storage = stor;
            b = blood;
            accessionNumbers.Text = b.Accession_number;
            foreach (bloodType x in (bloodType[])Enum.GetValues(typeof(bloodType)))
            {
                bloodTypeField.Items.Add(MyEnums.GetDescription(x));
            }
            DisplayBlood();
        }

        void DisplayBlood()
        {
            listBox1.Items.Clear();
            lName.Clear();
            fName.Clear();
            mInitial.Clear();
            pLast.Clear();
            pMid.Clear();
            pFirst.Clear();
            pAge.Clear();
            dateRemoved.Visible = false;
            label11.Visible = false;
            cRemovedPanel.Visible = false;
            if (b != null)
            {
                bloodTypeField.SelectedIndex = (int)b.Blood_type;
                if (b.Donor_id.HasValue)
                {
                    Donor d = storage.findDonor(b.Donor_id.Value);
                    if (d != null)
                    {
                        lName.Text = d.Last_name;
                        fName.Text = d.First_name;
                        mInitial.Text = d.Middle_initial;
                    }
                }
                dateDonated.Value = b.Date_donated;
                if (b.Is_removed)
                {
                    label11.Visible = true;
                    dateRemoved.Visible = true;
                    dateRemoved.Value = b.Date_removed;
                }
                foreach (Blood_SMS.Component c in b.components)
                {
                    listBox1.Items.Add(MyEnums.GetDescription(c.Component_name));
                }
            }
        }
        private void accessionNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            b = storage.findBlood(accessionNumbers.Text);
            DisplayBlood();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bloodComponents componentName = MyEnums.GetValueFromDescription<bloodComponents>(listBox1.Text);
            Blood_SMS.Component component = storage.findComponentWithAccessionNumberAndName(b.Accession_number, componentName);
            //componentValue.SelectedIndex = (int)componentName;
            dateProcessed.Value = component.Date_processed;
            if (!String.IsNullOrWhiteSpace(component.Patient_first_name))
            {
                pLast.Text = component.Patient_first_name;
                pFirst.Text = component.Patient_first_name;
                pMid.Text = component.Patient_middle_initial;
                pAge.Text = component.Patient_age.ToString();

            }
            if (component.Is_quarantined)
            {
                setRemoved("Date Quarantined", component.Date_removed, component.Reason_for_removal);
            }
            else if (component.Is_released)
            {
                setRemoved("Date Released", component.Date_removed, component.Reason_for_removal);
            }
            else if (component.Is_removed)
            {
                setRemoved("Date Reprocessed", component.Date_removed, component.Reason_for_removal);
            }


        }

        void setRemoved(string labelText, DateTime dateRemoved, string reason)
        {
            cRemovedLabel.Enabled = true;
            cRemovedLabel.Text = labelText;
            cDateRemoved.Value = dateRemoved;
            cReason.Text = reason;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ShowBlood_Load(object sender, EventArgs e)
        {
            bloodList = storage.bloodList;
            foreach (Blood b in bloodList)
            {
                accessionNumbers.Items.Add(b.Accession_number);
            }

        }

        private void b_edit_Click(object sender, EventArgs e)
        {
            b_cancel.Show();
            b_save.Show();
        }

        private void accessionNumbers_TextUpdate(object sender, EventArgs e)
        {
            b = storage.findBlood(accessionNumbers.Text);
            DisplayBlood();
        }
    }
}
