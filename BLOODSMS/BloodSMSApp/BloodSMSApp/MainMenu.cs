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
    public partial class MainMenu : Form
    {
        Storage storage;
        int bloodCount;
        int[] bloodTypeCount;
        List<string> notifications;
        graphCommand command;

        public MainMenu()
        {
            InitializeComponent();
            InitializeValues();
            RefreshCount();
            RefreshNotifications();
        }

        void InitializeValues()
        {
            storage = new Storage("localhost", "bsms", "root", "root");
            bloodTypeCount = new int[Enum.GetNames(typeof(bloodType)).Length];
            notifications = new List<string>();
            command = graphCommand.Add;
            dateTo.MaxDate = DateTime.Now;

            dateFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            dateTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //set this to date of install upon install
            dateFrom.MinDate = new DateTime(2013, 9, 1);
            //chart1.ChartAreas[0].AxisX.Maximum = 366;
            chart1.ChartAreas[0].AxisY.Maximum = 1200;
            
        }

        void RefreshCount()
        {
            bloodCount = storage.availableBlood.Count;
            for (int i = 0; i < bloodTypeCount.Length; i++)
            {
                bloodTypeCount[i] = storage.bloodTypes[i].Count;
            }
        }

        void RefreshNotifications()
        {
            notifications.Clear();
            //check low level
            foreach (bloodType blood_type in (bloodType[])Enum.GetValues(typeof(bloodType)))
            {
                if (storage.AlertLowLevel(blood_type))
                {
                    notifications.Add("Supply on " + blood_type.ToString().Replace('p', '+').Replace('n', '-') + " is critically low");
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
        }

        void DisplayOverview()
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
            for (int i = 0; i < notifications.Count; i++)
            {
                notificationsBox.Items.Add(notifications[i]);
            }
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            DisplayOverview();
        }

        void tabPage2_Click(object sender, EventArgs e)
        {
            RefreshGraph();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
                RefreshGraph();
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            AddDonor a = new AddDonor(storage);
            a.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ViewDonor v = new ViewDonor();
            v.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            AddItem i = new AddItem(storage);
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
            AddDonor a = new AddDonor(storage);
            a.Show();
        }

        private void addedButton_Click(object sender, EventArgs e)
        {
            RefreshLegend(); 
            command = graphCommand.Add;
            RefreshGraph();
        }

        private void dateFrom_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                dateTo.MinDate = dateFrom.Value.AddMonths(1);
            }
            catch
            {
                dateFrom.Value = dateTo.Value.AddMonths(-1);
            }
            RefreshGraph();
        }

        private void dateTo_ValueChanged(object sender, EventArgs e)
        {
            dateFrom.MaxDate = dateTo.Value.AddMonths(-1);
            RefreshGraph();
        }

        void GetNumbers(int[] ints)
        {
            chart1.Series["AB+"].Points.AddY(ints[0]);
            chart1.Series["AB-"].Points.AddY(ints[1]);
            chart1.Series["A+"].Points.AddY(ints[2]);
            chart1.Series["A-"].Points.AddY(ints[3]);
            chart1.Series["B+"].Points.AddY(ints[4]);
            chart1.Series["B-"].Points.AddY(ints[5]);
            chart1.Series["O+"].Points.AddY(ints[6]);
            chart1.Series["O-"].Points.AddY(ints[7]);
        }

        void RefreshLegend()
        {
            if (command == graphCommand.Summary)
            {
                chart1.Series[0].LegendText = "Total";
                chart1.Series[1].LegendText = "AB+";
                chart1.Series[2].LegendText = "AB-";
                chart1.Series[3].LegendText = "A+";
                chart1.Series[4].LegendText = "A-";
                chart1.Series[5].Enabled = true;
                chart1.Series[6].Enabled = true;
                chart1.Series[7].Enabled = true;
                chart1.Series[8].Enabled = true;
            }
        }

        void RefreshGraph()
        {
            TimeSpan span = dateTo.Value - dateFrom.Value;
            chart1.ChartAreas[0].AxisX.Maximum = span.TotalDays;
            switch (command)
            {
                case graphCommand.Add:
                    for (DateTime day = dateFrom.Value; day <= dateTo.Value; day = day.AddDays(1))
                    {
                        string xValue = day.ToString("MMM d");
                        chart1.Series["Total"].Points.AddXY(xValue, storage.getWholeBloodAddedOn(day));
                        GetNumbers(storage.getWholeBloodTypeAddedOn(day));
                    }
                    break;
                case graphCommand.Remove:
                    for (DateTime day = dateFrom.Value; day <= dateTo.Value; day = day.AddDays(1))
                    {
                        string xValue = day.ToString("MMM d");
                        chart1.Series["Total"].Points.AddXY(xValue, storage.getBloodRemovedOn(day));
                        GetNumbers(storage.getBloodTypeRemovedOn(day));
                    }
                    break;
                case graphCommand.Use:
                    for (DateTime day = dateFrom.Value; day <= dateTo.Value; day = day.AddDays(1))
                    {
                        string xValue = day.ToString("MMM d");
                        chart1.Series["Total"].Points.AddXY(xValue, storage.getBloodUsedOn(day));
                        GetNumbers(storage.getBloodTypeUsedOn(day));
                    }
                    break;
                case graphCommand.Quarantine:
                    for (DateTime day = dateFrom.Value; day <= dateTo.Value; day = day.AddDays(1))
                    {
                        string xValue = day.ToString("MMM d");
                        chart1.Series["Total"].Points.AddXY(xValue, storage.getBloodQuarantinedOn(day));
                        GetNumbers(storage.getBloodTypeQuarantinedOn(day));
                    }
                    break;
                case graphCommand.Release:
                    for(DateTime day = dateFrom.Value; day <= dateTo.Value; day = day.AddDays(1))
                    {
                        string xValue = day.ToString("MMM d");
                        chart1.Series["Total"].Points.AddXY(xValue, storage.getBloodReleasedOn(day));
                        GetNumbers(storage.getBloodTypeReleasedOn(day));
                    }
                    break;
                case graphCommand.Summary:
                    chart1.Series[0].LegendText = "Added Blood";
                    chart1.Series[1].LegendText = "Removed Blood";
                    chart1.Series[2].LegendText = "Released Blood";
                    chart1.Series[3].LegendText = "Used Blood";
                    chart1.Series[4].LegendText = "Quarantined Blood";
                    chart1.Series[5].Enabled = false;
                    chart1.Series[6].Enabled = false;
                    chart1.Series[7].Enabled = false;
                    chart1.Series[8].Enabled = false;
                    for (DateTime day = dateFrom.Value; day <= dateTo.Value; day = day.AddDays(1))
                    {
                        string xValue = day.ToString("MMM d");
                        chart1.Series[0].Points.AddXY(xValue, storage.getWholeBloodAddedOn(day));
                        chart1.Series[1].Points.AddXY(xValue, storage.getBloodRemovedOn(day));
                        chart1.Series[2].Points.AddXY(xValue, storage.getBloodReleasedOn(day));
                        chart1.Series[3].Points.AddXY(xValue, storage.getBloodUsedOn(day));
                        chart1.Series[4].Points.AddXY(xValue, storage.getBloodQuarantinedOn(day));
                    }
                    break;
            }
        }

        private void removedButton_Click(object sender, EventArgs e)
        {
            RefreshLegend(); 
            command = graphCommand.Remove;
            RefreshGraph();
        }

        private void releasedButton_Click(object sender, EventArgs e)
        {
            RefreshLegend(); 
            command = graphCommand.Release;
            RefreshGraph();
        }

        private void quarantinedButton_Click(object sender, EventArgs e)
        {
            RefreshLegend(); 
            command = graphCommand.Quarantine;
            RefreshGraph();
        }

        private void usedButton_Click(object sender, EventArgs e)
        {
            RefreshLegend(); 
            command = graphCommand.Use;
            RefreshGraph();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            command = graphCommand.Summary;
            RefreshGraph();
        }

    }
}
