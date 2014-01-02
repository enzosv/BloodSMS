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
        Blood_SMS.Component component;
        MainMenu parent;

        public ShowBlood(MainMenu mainmenu, string accessionNumber)
        {
            InitializeComponent();
            parent = mainmenu;
            accessionNumbers.Text = accessionNumber;
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
            //storage.getBloodSQL();
            //storage.getComponentSQL();
            bloodList = storage.bloodList;
            accessionNumbers.Items.Clear();
            foreach (Blood bl in bloodList)
            {
                accessionNumbers.Items.Add(bl.Accession_number);
            }

            foreach (Donor d in storage.donorList)
            {
                dNameBox.Items.Add(d.Name);
            }
            pLast.Clear();
            pMid.Clear();
            pFirst.Clear();
            pAge.Clear();
            label5.Text = "Not Removed";
            cRemovedPanel.Visible = false;

            quarantineButton.Visible = false;
            assignButton.Visible = false;
            reprocessButton.Visible = false;
            cRemovedPanel.Visible = false;
            editComponent.Visible = false;

            DisplayBlood();
        }

        void DisplayBlood()
        {
            b = storage.findBlood(accessionNumbers.Text);
            if (b != null)
            {
                bloodTypeField.SelectedIndex = (int)b.Blood_type;
                dateDonated.Value = b.Date_donated;
                if (b.Donor_id.HasValue)
                {
                    Donor d = storage.findDonor(b.Donor_id.Value);
                    if (d != null)
                    {
                        dNameBox.Text = d.Name;
                    }
                }
                if (b.Is_removed)
                {
                    label5.Text = b.Date_removed.ToLongDateString();
                }

                listBox1.Items.Clear();
                foreach (Blood_SMS.Component c in b.components)
                {
                    listBox1.Items.Add(MyEnums.GetDescription(c.Component_name));
                }
                //listBox1.SelectedIndex = 0;
            }
            else
                MessageBox.Show("Error displaying blood. Please try again");

        }
        private void accessionNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reload();
        }

        private void b_back_Click(object sender, EventArgs e)
        {
            if (b_back.Text == "BACK")
                Close();
            else
                bDisableEdit();
        }
        #endregion

        #region BLOOD EDITS

        void bDisableEdit()
        {
            accessionNumbers.Enabled = true;
            bloodTypeField.Enabled = false;
            dNameBox.Enabled = false;
            dateDonated.Enabled = false;
            textBox1.Visible = false;
            textBox1.Clear();

            b_edit.Text = "EDIT";
            b_back.Text = "BACK";
            bDeleteButton.Visible = false;
        }

        void bEnableEdit()
        {
            accessionNumbers.Enabled = false;
            bloodTypeField.Enabled = true;
            dNameBox.Enabled = true;
            dateDonated.Enabled = true;
            textBox1.Visible = true;
            textBox1.Text = accessionNumbers.Text;

            b_edit.Text = "SAVE";
            b_back.Text = "CANCEL";
            bDeleteButton.Visible = true;
        }

        private void b_edit_Click(object sender, EventArgs e)
        {
            if (b_edit.Text == "EDIT")
            {
                if (b != null)
                    bEnableEdit();
                else
                    MessageBox.Show("Please select an accession number from the list");
            }
            else //SAVE
            {
                Blood b;
                Donor d = storage.findDonorWithName(dNameBox.Text);

                if (d != null)
                    b = new Blood(textBox1.Text, bloodTypeField.SelectedIndex, d.Donor_id, dateDonated.Value);
                else
                {
                    b = new Blood(textBox1.Text, bloodTypeField.SelectedIndex, dateDonated.Value);
                }
                if (storage.UpdateBlood(b, accessionNumbers.Text))
                {
                    bDisableEdit();
                    storage.getBloodSQL();
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
            Reload();
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

        #region COMPONENT

        void DisplayComponent()
        {
            if (listBox1.SelectedIndex > -1)
            {
                bloodComponents componentName = MyEnums.GetValueFromDescription<bloodComponents>(listBox1.SelectedItem.ToString());
                component = storage.findComponentWithAccessionNumberAndName(b.Accession_number, componentName);

                dateProcessed.Value = component.Date_processed;
                expiryDate.Value = component.Date_expired;
                if (!String.IsNullOrWhiteSpace(component.Patient_first_name))
                {
                    pLast.Text = component.Patient_last_name;
                    pFirst.Text = component.Patient_first_name;
                    pMid.Text = component.Patient_middle_initial;
                    pAge.Text = component.Patient_age.ToString();
                }
                else
                {
                    pLast.Text = "";
                    pFirst.Text = "";
                    pMid.Text = "";
                    pAge.Text = "";
                }

                if (component.Date_assigned != DateTime.MinValue)
                    assignButton.Text = "RELEASE";
                else
                    assignButton.Text = "ASSIGN";

                if (component.Is_removed)
                {
                    if (component.Is_quarantined)
                        setRemoved("Date Quarantined", component.Date_removed, component.Reason_for_removal);
                    else if (component.Is_released)
                        setRemoved("Date Released", component.Date_removed, component.Reason_for_removal);
                    else
                        setRemoved("Date Reprocessed", component.Date_removed, component.Reason_for_removal);

                    cRemovedPanel.Visible = true;
                }
                else
                {
                    cRemovedLabel.Enabled = false;
                    cRemovedLabel.Text = "Not Removed";
                    cDateRemoved.Value = component.Date_processed;
                    cReason.Text = "Not Removed";
                    quarantineButton.Visible = true;
                    assignButton.Visible = true;
                    reprocessButton.Visible = true;
                    cRemovedPanel.Visible = false;
                }
                editComponent.Visible = true;
            }
            else
            {
                listBox1.SelectedIndex = 0;
                DisplayComponent();
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayComponent();
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

        #region COMPONENT EDITS
        void cEnableEdit()
        {
            if (listBox1.SelectedItem.ToString() == "Whole")
            {
                deleteButton.Visible = false;
                cNameBox.Text = "Whole";
            }
            else
            {
                deleteButton.Visible = true;
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
            assignButton.Visible = false;
            quarantineButton.Visible = false;
            reprocessButton.Visible = false;
            cancelButton.Visible = true;

            editComponent.Text = "SAVE";
            listBox1.Enabled = false;
            dateProcessed.Enabled = true;
            expiryDate.Enabled = true;
            if (component.Is_removed)
            {
                cRemovedPanel.Visible = true;
                cReturn.Visible = true;
                cReturn.Text = "RETURN TO INVENTORY";
            }
            else if (component.Date_assigned != DateTime.MinValue)
            {
                //pLast.Enabled = true;
                //pFirst.Enabled = true;
                //pMid.Enabled = true;
                //pAge.Enabled = true;
                cReturn.Visible = true;
                cReturn.Text = "UNASSIGN COMPONENT";
            }


        }

        void cDisableEdit()
        {
            assignButton.Visible = true;
            quarantineButton.Visible = true;
            reprocessButton.Visible = true;
            cancelButton.Visible = false;
            deleteButton.Visible = false;
            cReturn.Visible = false;
            editComponent.Text = "EDIT";
            listBox1.Enabled = true;
            dateProcessed.Enabled = false;
            expiryDate.Enabled = false;
            pLast.Enabled = false;
            pFirst.Enabled = false;
            pMid.Enabled = false;
            pAge.Enabled = false;

            cRemovedPanel.Visible = false;
            cNameBox.Visible = false;
            //parent.RefreshStorage();
            Reload();
        }
        private void editComponent_Click(object sender, EventArgs e)
        {
            if (editComponent.Text == "EDIT")
            {
                if (listBox1.SelectedIndex > -1)
                {
                    if (component != null)
                        cEnableEdit();
                    else
                        MessageBox.Show("Component not found. Please refresh and try again");
                }
                else
                    MessageBox.Show("Please select a component to edit");
            }
            //SAVE
            else
            {
                Blood_SMS.Component c;
                int removal_type;
                DateTime date_removed;
                switch (cRemovedLabel.Text)
                {
                    case "Date Quarantined":
                        removal_type = 2;
                        date_removed = cDateRemoved.Value;
                        break;
                    case "Date Released":
                        removal_type = 3;
                        date_removed = cDateRemoved.Value;
                        break;
                    case "Date Reprocessed":
                        removal_type = 1;
                        date_removed = cDateRemoved.Value;
                        break;
                    default:
                        removal_type = 0;
                        date_removed = DateTime.MinValue;
                        break;
                }
                int age;
                //parent.RefreshStorage();
                if (int.TryParse(pAge.Text, out age))
                {
                    c = new Blood_SMS.Component(accessionNumbers.Text, (int)MyEnums.GetValueFromDescription<bloodComponents>(cNameBox.Text.ToString()), removal_type, dateProcessed.Value, expiryDate.Value, dateAssigned.Value, date_removed, pLast.Text, pFirst.Text, pMid.Text, age, cReason.Text);
                    UpdateComponent(c);
                }
                else
                {
                    c = new Blood_SMS.Component(accessionNumbers.Text, (int)MyEnums.GetValueFromDescription<bloodComponents>(cNameBox.Text.ToString()), removal_type, dateProcessed.Value, expiryDate.Value, DateTime.MinValue, date_removed, "", "", "", 0, cReason.Text);
                    UpdateComponent(c);
                }

            }
        }

        void UpdateComponent(Blood_SMS.Component c)
        {
            if (storage.UpdateComponent(c, (int)MyEnums.GetValueFromDescription<bloodComponents>(listBox1.SelectedItem.ToString())))
            {
                cDisableEdit();
                MessageBox.Show("Component was successfully updated");
            }
            else
                MessageBox.Show("Error updating component. Please try again later");
            DisplayBlood();
            DisplayComponent();
        }
        private void pAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(Char.IsNumber(e.KeyChar) || e.KeyChar == 8);
        }
        #endregion

        #region COMPONENT BUTTONS
        private void addComponent_Click(object sender, EventArgs e)
        {
            List<string> componentNames = new List<string>();
            foreach (bloodComponents x in (bloodComponents[])Enum.GetValues(typeof(bloodComponents)))
            {
                if (!listBox1.Items.Contains(MyEnums.GetDescription(x)))
                {
                    componentNames.Add(MyEnums.GetDescription(x));
                }
            }
            if (componentNames.Count > 0)
            {
                AddComponent ac = new AddComponent(storage, b, componentNames);
                ac.ShowDialog();
                DisplayBlood();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

            if (storage.DeleteComponentWithAccessionNumberAndName(accessionNumbers.Text, (int)MyEnums.GetValueFromDescription<bloodComponents>(listBox1.SelectedItem.ToString())))
            {
                MessageBox.Show("Component was successfully deleted");
                cDisableEdit();
            }
            else
                MessageBox.Show("Component could not be deleted. Please try again");

        }

        private void assignButton_Click(object sender, EventArgs e)
        {
            if (assignButton.Text == "ASSIGN")
            {
                Assign_Form af = new Assign_Form(storage, component);
                af.Show();
                Reload();
                DisplayComponent();
                assignButton.Text = "RELEASE";
            }
            else
            {
                RemoveItem(removalType.Released);
            }
        }

        private void reprocessButton_Click(object sender, EventArgs e)
        {
            RemoveItem(removalType.Reprocessed);
        }

        private void quarantineButton_Click(object sender, EventArgs e)
        {
            RemoveItem(removalType.Quarantined);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            cDisableEdit();
            DisplayBlood();
            DisplayComponent();
        }

        void RemoveItem(removalType rt)
        {
            RemoveItem ri = new RemoveItem(storage, component, rt);
            ri.ShowDialog();
            DisplayBlood();
            DisplayComponent();
        }

        private void cReturn_Click(object sender, EventArgs e)
        {
            if (cReturn.Text == "RETURN TO INVENTORY")
            {
                component.Unremove();
                
                DisplayBlood();
                DisplayComponent();
                cEnableEdit();
                cRemovedPanel.Visible = false;
            }
            //UNASSIGN
            else
            {
                component.Unassign();
                DisplayBlood();
                DisplayComponent();
                cEnableEdit();
                assignButton.Text = "ASSIGN";
                //if (storage.UpdateComponent(component))
                //{
                //    MessageBox.Show("Unassigned component");

                //}
            }
            cReturn.Visible = false;
        }
        #endregion

        private void dNameBox_Leave(object sender, EventArgs e)
        {
            if (!dNameBox.Items.Contains(dNameBox.Text))
            {
                MessageBox.Show("Donor could not be found");
                dNameBox.Text = "";
            }
        }

        private void bDeleteButton_Click(object sender, EventArgs e)
        {
            if (storage.DeleteBloodWithAccessionNumber(accessionNumbers.Text))
            {
                parent.RefreshOverview();
                Close();
            }
            else
            {
                MessageBox.Show("Blood could not be deleted. Please refresh and try again");
            }
        }

    }
}
