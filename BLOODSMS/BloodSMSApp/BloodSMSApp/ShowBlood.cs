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
    public partial class ShowBlood : Form
    {
        Storage storage;
        List<Blood> bloodList;
        Blood b;
        MainMenu parent;

        public ShowBlood(MainMenu mainmenu, Blood blood)
        {
            InitializeComponent();
            parent = mainmenu;
            b = blood;
            accessionNumbers.Text = b.Accession_number;
            foreach (bloodType x in (bloodType[])Enum.GetValues(typeof(bloodType)))
            {
                bloodTypeField.Items.Add(MyEnums.GetDescription(x));
            }

        }
        #region BLOOD
        private void ShowBlood_Load(object sender, EventArgs e)
        {
            Reload();
        }

        void Reload()
        {
            storage = parent.storage;
            bloodList = storage.bloodList;
            accessionNumbers.Items.Clear();
            foreach (Blood b in bloodList)
            {
                accessionNumbers.Items.Add(b.Accession_number);
            }
            listBox1.Items.Clear();
            lName.Clear();
            fName.Clear();
            mInitial.Clear();
            pLast.Clear();
            pMid.Clear();
            pFirst.Clear();
            pAge.Clear();
            label5.Text = "Not Removed";
            cRemovedPanel.Visible = false;
            DisplayBlood();
        }

        void DisplayBlood()
        {
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
                    label5.Text = b.Date_removed.ToLongDateString();
                }
                foreach (Blood_SMS.Component c in b.components)
                {
                    listBox1.Items.Add(MyEnums.GetDescription(c.Component_name));
                }
                listBox1.SelectedIndex = 0;
            }

        }
        private void accessionNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            b = storage.findBlood(accessionNumbers.Text);
            if (b != null)
                DisplayBlood();
            else
                MessageBox.Show("Error displaying blood. Please try again");
        }

        private void b_back_Click(object sender, EventArgs e)
        {
            if (b_back.Text == "BACK")
                Close();
            else
            {
                bDisableEdit();
            }
        }
#endregion

        #region COMPONENT
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bloodComponents componentName = MyEnums.GetValueFromDescription<bloodComponents>(listBox1.Text);
            Blood_SMS.Component component = storage.findComponentWithAccessionNumberAndName(b.Accession_number, componentName);

            dateProcessed.Value = component.Date_processed;
            expiryDate.Value = component.Date_expired;
            if (!String.IsNullOrWhiteSpace(component.Patient_first_name))
            {
                pLast.Text = component.Patient_last_name;
                pFirst.Text = component.Patient_first_name;
                pMid.Text = component.Patient_middle_initial;
                pAge.Text = component.Patient_age.ToString();

            }
            
            if (component.Date_assigned != DateTime.MinValue)
                assignButton.Text = "RELEASE";
            if (component.Is_removed)
            {
                if (component.Is_quarantined)
                    setRemoved("Date Quarantined", component.Date_removed, component.Reason_for_removal);
                else if (component.Is_released)
                    setRemoved("Date Released", component.Date_removed, component.Reason_for_removal);
                else
                    setRemoved("Date Reprocessed", component.Date_removed, component.Reason_for_removal);
            }
            else
            {
                quarantineButton.Visible = true;
                assignButton.Visible = true;
                reprocessButton.Visible = true;
            }


        }

        void setRemoved(string labelText, DateTime dateRemoved, string reason)
        {
            cRemovedLabel.Enabled = true;
            cRemovedLabel.Text = labelText;
            cDateRemoved.Value = dateRemoved;
            cReason.Text = reason;

            quarantineButton.Visible = false;
            assignButton.Visible = false;
            reprocessButton.Visible = false;
        }
        #endregion

        #region BLOOD EDITS

        void bEnableEdit()
        {
            accessionNumbers.Enabled = false;
            bloodTypeField.Enabled = true;
            lName.Enabled = true;
            fName.Enabled = true;
            mInitial.Enabled = true;
            dateDonated.Enabled = true;
            textBox1.Visible = true;
            textBox1.Text = accessionNumbers.Text;

            b_edit.Text = "SAVE";
            b_back.Text = "CANCEL";
            bDeleteButton.Visible = true;
        }

        void bDisableEdit()
        {
            accessionNumbers.Enabled = true;
            bloodTypeField.Enabled = false;
            lName.Enabled = false;
            fName.Enabled = false;
            mInitial.Enabled = false;
            dateDonated.Enabled = false;
            textBox1.Visible = false;
            textBox1.Clear();

            b_edit.Text = "EDIT";
            b_back.Text = "BACK";
            bDeleteButton.Visible = false;
        }

        private void b_edit_Click(object sender, EventArgs e)
        {
            if (b_edit.Text == "EDIT")
            {
                if (storage.findBlood(accessionNumbers.Text) != null)
                    bEnableEdit();
                else
                    MessageBox.Show("Please select an accession number from the list");
            }
            else
            {
                Blood b;
                Donor d = storage.findDonorWithName(lName.Text, fName.Text, mInitial.Text);
                if (d != null)
                    b = new Blood(textBox1.Text, bloodTypeField.SelectedIndex, d.Donor_id, dateDonated.Value);
                else
                    b = new Blood(textBox1.Text, bloodTypeField.SelectedIndex, dateDonated.Value);
                if (storage.UpdateBlood(b, accessionNumbers.Text))
                {
                    bDisableEdit();
                    parent.RefreshStorage();
                    Reload();
                    accessionNumbers.Text = b.Accession_number;
                    MessageBox.Show("Blood Updated");
                }
                else
                {
                    textBox1.Text = accessionNumbers.Text;
                    MessageBox.Show("Error updating blood values");
                }
            }
        }

        private void accessionNumbers_TextUpdate(object sender, EventArgs e)
        {
            b = storage.findBlood(accessionNumbers.Text);
            DisplayBlood();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text != accessionNumbers.Text && storage.findBlood(textBox1.Text) != null)
            {
                MessageBox.Show("Blood with accession number already exists");
                textBox1.Focus();
            }

        }

        #endregion

        #region COMPONENT EDITS
        void cEnableEdit(Blood_SMS.Component c)
        {
            assignButton.Visible = false;
            quarantineButton.Visible = false;
            reprocessButton.Visible = false;
            cancelButton.Visible = true;
            if (c.Is_removed)
            {
                cReturn.Visible = true;
            }

            deleteButton.Text = "CANCEL";
            editComponent.Text = "SAVE";
            listBox1.Enabled = false;
            dateProcessed.Enabled = true;
            expiryDate.Enabled = true;
            if (c.Date_assigned != DateTime.MinValue)
            {
                pLast.Enabled = true;
                pFirst.Enabled = true;
                pMid.Enabled = true;
                pAge.Enabled = true;
            }
            if (c.Is_removed)
            {
                cRemovedPanel.Visible = true;
            }
            cNameBox.Visible = true;
            cNameBox.Items.Clear();
            cNameBox.Items.Add(listBox1.SelectedItem.ToString());
            foreach (bloodComponents x in (bloodComponents[])Enum.GetValues(typeof(bloodComponents)))
            {
                if (!listBox1.Items.Contains(MyEnums.GetDescription(x)))
                {
                    cNameBox.Items.Add(MyEnums.GetDescription(x));
                }
            }
            cNameBox.SelectedIndex = 0;
        }

        void cDisableEdit()
        {
            assignButton.Visible = true;
            quarantineButton.Visible = true;
            reprocessButton.Visible = true;
            cancelButton.Visible = false;
            cReturn.Visible = false;
            editComponent.Text = "EDIT";
            cancelButton.Visible = true;
            deleteButton.Visible = true;
            listBox1.Enabled = true;
            dateProcessed.Enabled = false;
            expiryDate.Enabled = false;
            pLast.Enabled = false;
            pFirst.Enabled = false;
            pMid.Enabled = false;
            pAge.Enabled = false;

            cRemovedPanel.Visible = false;
            cNameBox.Visible = false;
            parent.RefreshStorage();
            Reload();
        }
        private void editComponent_Click(object sender, EventArgs e)
        {
            if (editComponent.Text == "EDIT")
            {
                if (listBox1.SelectedIndex > 0)
                {
                    Blood_SMS.Component c = storage.findComponentWithAccessionNumberAndName(accessionNumbers.Text, MyEnums.GetValueFromDescription<bloodComponents>(listBox1.SelectedItem.ToString()));
                    if (c != null)
                        cEnableEdit(c);
                    else
                       MessageBox.Show("Component not found. Please refresh and try again");
                    
                }
                else
                   MessageBox.Show("Component may not be edited");
            }
            else
            {
                Blood_SMS.Component c;
                int removal_type = 0;
                DateTime date_removed = DateTime.MinValue;
                if (cDateRemoved.Enabled)
                {
                    switch (cRemovedLabel.Text)
                    {
                        case "DATE QUARANTINED":
                            removal_type = 2;
                            date_removed = cDateRemoved.Value;
                            break;
                        case "DATE RELEASED":
                            removal_type = 3;
                            date_removed = cDateRemoved.Value;
                            break;
                        case "DATE_REPROCESSED":
                            removal_type = 1;
                            date_removed = cDateRemoved.Value;
                            break;
                    }
                }
                int age;
                if (pLast.Enabled && !String.IsNullOrWhiteSpace(pLast.Text) && !String.IsNullOrWhiteSpace(pFirst.Text) && !String.IsNullOrWhiteSpace(pMid.Text) && !String.IsNullOrWhiteSpace(pAge.Text))
                {
                    if (int.TryParse(pAge.Text, out age))
                    {
                        c = new Blood_SMS.Component(accessionNumbers.Text, (int)MyEnums.GetValueFromDescription<bloodComponents>(cNameBox.SelectedItem.ToString()), removal_type, dateProcessed.Value, expiryDate.Value, dateAssigned.Value, date_removed, pLast.Text, pFirst.Text, pMid.Text, age, cReason.Text);
                        if (storage.UpdateComponent(c, (int)MyEnums.GetValueFromDescription<bloodComponents>(listBox1.SelectedItem.ToString())))
                        {
                            cDisableEdit();
                            MessageBox.Show("Component was successfully updated");
                        }
                        else
                            MessageBox.Show("Error updating component. Please try again later");
                    }
                }
                else
                {
                    c = new Blood_SMS.Component(accessionNumbers.Text, (int)MyEnums.GetValueFromDescription<bloodComponents>(cNameBox.SelectedItem.ToString()), removal_type, dateProcessed.Value, expiryDate.Value, DateTime.MinValue, date_removed, "", "", "", 0, cReason.Text);
                    if (storage.UpdateComponent(c, (int)MyEnums.GetValueFromDescription<bloodComponents>(listBox1.SelectedItem.ToString())))
                    {
                        cDisableEdit();
                        MessageBox.Show("Component was successfully updated");
                    }
                    else
                        MessageBox.Show("Error updating component. Please try again later");
                }

            }
        }

        private void pAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(Char.IsNumber(e.KeyChar) || e.KeyChar == 8);
        }
#endregion

        #region COMPONENT BUTTONS
        private void addComponent_Click(object sender, EventArgs e)
        {

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (storage.DeleteComponentWithAccessionNumberAndName(accessionNumbers.Text, MyEnums.GetValueFromDescription<bloodComponents>(textBox1.Text)))
            {
                MessageBox.Show("Component was successfully deleted");
                cDisableEdit();
            }
        }

        private void assignButton_Click(object sender, EventArgs e)
        {
            if (assignButton.Text == "ASSIGN")
            {

            }
            else
            {

            }
        }
        #endregion
    }
}
