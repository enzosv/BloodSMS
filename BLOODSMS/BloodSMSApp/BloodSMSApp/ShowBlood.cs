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
        Blood x;
        public ShowBlood(Storage stor)
        {
            InitializeComponent();
            storage = stor;
            bloodList = storage.bloodList;
            foreach (Blood b in bloodList)
            {
                accessionNumbers.Items.Add(b.Accession_number);
            }
        }

        private void accessionNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            x = storage.findBlood(accessionNumbers.Text);
            bloodTypeField.SelectedIndex = (int)x.Blood_type;
            if (x.Donor_id.HasValue)
            {
                Donor d = storage.findDonor(x.Donor_id.Value);
                if(d != null)
                {
                    lName.Text = d.Last_name;
                    fName.Text = d.First_name;
                    mInitial.Text = d.Middle_initial;
                }
            }
            dateDonated.Value = x.Date_donated;
            if (x.Is_removed)
            {
                dateRemoved.Enabled = true;
                dateRemoved.Value = x.Date_removed;
            }
            foreach (Blood_SMS.Component c in x.components)
            {
                listBox1.Items.Add(MyEnums.GetDescription(c.Component_name));
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bloodComponents componentName = MyEnums.GetValueFromDescription<bloodComponents>(listBox1.Text);
            Blood_SMS.Component component = storage.findComponentWithAccessionNumberAndName(x.Accession_number, componentName);
            componentValue.SelectedIndex = (int)componentName;
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
                setRemoved("Date Quarantined", component.Date_quarantined, component.Reason_for_removal);
            }
            else if (component.Is_released)
            {
                setRemoved("Date Released", component.Date_released, component.Reason_for_removal);
            }
            else if (component.Date_reprocessed != DateTime.MinValue)
            {
                setRemoved("Date Reprocessed", component.Date_reprocessed, component.Reason_for_removal);
            }


        }

        void setRemoved(string labelText, DateTime dateRemoved, string reason)
        {
            cRemovedLabel.Enabled = true;
            cRemovedLabel.Text = labelText;
            cDateRemoved.Value = dateRemoved;
            cReason.Text = reason;
        }
    }
}
