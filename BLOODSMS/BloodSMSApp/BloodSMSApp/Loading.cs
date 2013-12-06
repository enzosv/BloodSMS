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
    public partial class Loading : Form
    {
        List<Donor> emailTos;
        string subject, body;
        public Loading(List<Donor> mailTos, string subj, string bod)
        {
            InitializeComponent();
            emailTos = mailTos;
            subject = subj;
            body = bod;
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "OK")
                Close();
            else
            {
                SendMessages();
            }
        }

        void SendMessages()
        {
            progressBar1.Maximum = emailTos.Count;
            progressBar1.Visible = true;
            foreach (Donor d in emailTos)
            {
                if (d.SendEmail(subject, body))
                {
                    progressBar1.Value++;
                    if (progressBar1.Value >= progressBar1.Maximum)
                    {
                        progressBar1.Visible = false;
                        button1.Text = "OK";
                        label1.Text = "Messages Sent";
                    }
                }
            }
        }
    }
}
