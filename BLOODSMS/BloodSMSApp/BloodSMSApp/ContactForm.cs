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
    public partial class ContactForm : Form
    {
        List<Donor> contacts;
        List<Donor> emailTos;
        public ContactForm(List<Donor> CONTACTS)
        {
            InitializeComponent();
            contacts = CONTACTS;
            dataGridView1.DataSource = contacts;
            emailTos = new List<Donor>();
            
        }

        private void ContactForm_Load(object sender, EventArgs e)
        {
            foreach (Donor d in contacts)
            {
                if (!String.IsNullOrWhiteSpace(d.Email))
                {
                    textBox1.Text += d.Email + ", ";
                    emailTos.Add(d);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length > 0 && richTextBox1.Text.Length > 0)
            {
                foreach (Donor d in emailTos)
                {
                    d.SendEmail(textBox3.Text, richTextBox1.Text);
                }
            }
        }
    }
}
