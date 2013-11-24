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
    public partial class MainMenu : Form
    {
        Storage storage;
        int bloodCount;
        int donorCount;
        int[] bloodTypeCount;

        List<string> notifications;
        public MainMenu()
        {
            InitializeComponent();
            storage = new Storage("localhost", "bsms", "root", "root");
            bloodCount = storage.availableBlood.Count;
            //donorCount = storage.ViableDonors.Count;
            bloodTypeCount = new int[Enum.GetNames(typeof(bloodType)).Length];
            for (int i = 0; i < bloodTypeCount.Length; i++)
            {
                bloodTypeCount[i] = storage.bloodTypes[i].Count;
            }

            #region Notifications
            notifications = new List<string>();
            //check low level
            foreach (bloodType blood_type in (bloodType[])Enum.GetValues(typeof(bloodType)))
            {
                if (storage.AlertLowLevel(blood_type))
                {
                    notifications.Add("Supply on " + blood_type.ToString() + " is critically low");
                }
            }
            //check near expirations
            foreach (Blood b in storage.availableBlood)
            {
                if (storage.AlertNearExpiration(b))
                {
                    notifications.Add("Blood with ID " + b.Blood_id + " is near expiration");
                }
            }
            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            availableBloodLabel.Text = "Available Blood: " + bloodCount;
            ABp.Text = "AB+ : " + bloodTypeCount[0];
            ABn.Text = "AB- : " + bloodTypeCount[1];
            Ap.Text = "A+ : " + bloodTypeCount[2];
            An.Text = "A- : " + bloodTypeCount[3];
            Bp.Text = "B+ : " + bloodTypeCount[4];
            Bn.Text = "B- : " + bloodTypeCount[5];
            Op.Text = "O+ : " + bloodTypeCount[6];
            On.Text = "O- : " + bloodTypeCount[6];

            //notifications
            for(int i = 0; i< notifications.Count; i++)
            {
                notificationsBox.Items.Add(notifications[i]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            AddDonor a = new AddDonor();
            a.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ViewDonor v = new ViewDonor();
            v.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            AddItem i = new AddItem();
            i.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            RemoveItem r = new RemoveItem();
            r.Show();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            DeferDonor d = new DeferDonor();
            d.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AddDonor a = new AddDonor();
            a.Show();
        }


    }
}
