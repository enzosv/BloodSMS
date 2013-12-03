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
        Storage storage;
        public ContactForm(Storage stor)
        {
            InitializeComponent();
            storage = stor;
        }

        private void ContactForm_Load(object sender, EventArgs e)
        {

        }
    }
}
