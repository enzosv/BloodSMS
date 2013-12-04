using System;
using System.Windows.Forms;
using Blood_SMS;

namespace BloodSMSApp
{
    public partial class PreAddDonor : Form
    {
        Storage storage;
        MainMenu parent;
        public PreAddDonor(MainMenu mainmenu)
        {
            InitializeComponent();
            parent = mainmenu;
            storage = parent.storage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddDonor donorForm;
            if (!String.IsNullOrWhiteSpace(lName.Text) && !String.IsNullOrWhiteSpace(fName.Text) && !String.IsNullOrWhiteSpace(mInitial.Text))
            {
                donorForm = new AddDonor(storage, lName.Text, fName.Text, mInitial.Text);
                donorForm.ShowDialog();
                Close();
                parent.RefreshOverview();
            }
            else
            {
                MessageBox.Show("Invalid name");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
