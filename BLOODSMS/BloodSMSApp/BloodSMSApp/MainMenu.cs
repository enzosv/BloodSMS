using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Blood_SMS;
//using System.Web.UI.DataVisualization.Charting;

namespace BloodSMSApp
{
    public partial class MainMenu : Form
    {
        Storage storage;
        int bloodCount;
        int[] bloodTypeCount;
        List<string> notifications;
        graphCommand command;
        Dictionary<DateTime, int> days;
        Dictionary<int, DateTime> days2;
        Series seriesHit;
        Color seriesOldColor;

        public MainMenu()
        {
            InitializeComponent();
            InitializeValues();
            RefreshOverview();
        }

        void InitializeValues()
        {
            storage = new Storage("localhost", "bsms", "root", "root");
            bloodTypeCount = new int[Enum.GetNames(typeof(bloodType)).Length];
            notifications = new List<string>();
            command = graphCommand.Summary;

            days = new Dictionary<DateTime, int>();
            days2 = new Dictionary<int, DateTime>();

            //set this to date of install upon install
            dateTo.MaxDate = DateTime.Now.AddYears(1);
            dateFrom.MinDate = new DateTime(2010, 9, 1);

            dateFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            dateTo.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //for (int i = 0; i < chart1.Series.Count; i++)
            //{
            //    chart1.Series[i].IsXValueIndexed = true;
            //}


            //chart1.ChartAreas[0].AxisY.Maximum = 1200;

            //for (int i = 0; i < chart1.Series.Count; i++)
            //{
            //    //chart1.Series[i].XAxisType = chart DateTime;
            //}
            seriesHit = chart1.Series[0];

            dataGridView3.DataSource = storage.donorList;
            dataGridView2.DataSource = storage.bloodList;

        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            DisplayOverview();
        }

        #region overview
        void RefreshOverview()
        {
            bloodCount = storage.availableBlood.Count;
            for (int i = 0; i < bloodTypeCount.Length; i++)
            {
                bloodTypeCount[i] = storage.bloodTypes[i].Count;
            }

            notifications.Clear();

            //check low level
            bloodType blood_type;
            for (int i = 0; i < bloodTypeCount.Length; i++)
            {
                blood_type = (bloodType)i;
                if (storage.AlertLowLevel(i))
                {
                    notifications.Add("Supply on " + MyEnums.GetDescription(blood_type) + " is critically low");
                }
            }

            //check near expirations
            foreach (string[] s in storage.AlertNearExpiration())
            {
                notifications.Add("Component " + s[0] + " with Accession Number " + s[1] + " is near expiration");
            }
        }

        void DisplayOverview()
        {
            bloodType b;
            int availableBlood = storage.availableBlood.Count;
            chart2.Series[0].Points.AddY(1200 - availableBlood);
            chart2.Series[0].Points[0].LegendText = "Available Blood: " + availableBlood;
            chart2.Series[0].Points[0].Color = Color.Transparent;

            for (int i = 1; i < bloodTypeCount.Length; i++)
            {
                b = (bloodType)i;
                chart2.Series[0].Points.AddY(bloodTypeCount[i]);
                chart2.Series[0].Points[i].LegendText = MyEnums.GetDescription(b) + ": " + bloodTypeCount[i];
            }
            //notifications
            for (int i = 0; i < notifications.Count; i++)
            {
                notificationsBox.Items.Add(notifications[i]);
            }
            RefreshGraph();
        }
        #endregion

        #region buttons

        private void b_addDonor_Click(object sender, EventArgs e)
        {
            PreAddDonor a = new PreAddDonor(storage);
            a.Show();
        }

        private void b_donorAdd_Click(object sender, EventArgs e)
        {
            PreAddDonor a = new PreAddDonor(storage);
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

        private void Summary_Click(object sender, EventArgs e)
        {
            if (command != graphCommand.Summary)
            {
                chart1.Series[0].LegendText = "Added Blood";
                chart1.Series[1].LegendText = "Removed Blood";
                chart1.Series[2].Enabled = false;
                chart1.Series[3].Enabled = false;
                chart1.Series[4].Enabled = false;
                chart1.Series[5].Enabled = false;
                chart1.Series[6].Enabled = false;
                chart1.Series[7].Enabled = false;
                chart1.Series[8].Enabled = false;
                command = graphCommand.Summary;
                RefreshGraph();
            }
        }
        #endregion

        #region graph
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
            ChangeDates();
        }

        private void dateTo_ValueChanged(object sender, EventArgs e)
        {
            dateFrom.MaxDate = dateTo.Value.AddMonths(-1);
            ChangeDates();
        }

        void RefreshLegend()
        {
            if (command == graphCommand.Summary)
            {
                chart1.Series[0].LegendText = "Total";
                chart1.Series[1].LegendText = "AB+";
                chart1.Series[2].Enabled = true;
                chart1.Series[3].Enabled = true;
                chart1.Series[4].Enabled = true;
                chart1.Series[5].Enabled = true;
                chart1.Series[6].Enabled = true;
                chart1.Series[7].Enabled = true;
                chart1.Series[8].Enabled = true;
            }
        }

        void ChangeDates()
        {
            TimeSpan span = dateTo.Value - dateFrom.Value;
            //

            days.Clear();
            days2.Clear();
            int index = 0;
            for (DateTime day = dateFrom.Value; day <= dateTo.Value; day = day.AddDays(1))
            {
                //key, value
                days.Add(day, index);
                days2.Add(index, day);
                index++;
            }
            chart1.ChartAreas[0].AxisX.Maximum = span.TotalDays;
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                chart1.Series[i].Points.Clear();
            }
            RefreshGraph();
            

        }

        void RefreshGraph()
        {
            Random random = new Random();
            if (command != graphCommand.Summary)
            {
                int[] totals = storage.getBloodModifiedDuring(days, command);
                int[,] types = storage.getBloodTypeModifiedDuring(days, command);
                for (int i = 0; i < days.Count; i++)
                {
                    chart1.Series[0].Points.Clear();
                    chart1.Series[0].Points.AddXY(days2[i].ToString("d MMM yy"), totals[i] + random.Next(0, i * 3));
                    
                    for (int j = 1; j < Enum.GetNames(typeof(bloodType)).Length; j++)
                    {
                        chart1.Series[j].Points.Clear();
                        chart1.Series[j].Points.AddXY(days2[i].ToString("d MMM yy"), types[i, j - 1] + random.Next(0, i * 3));
                    }
                }
            }
            else
            {
                int[,] wholes = storage.getSummary(days);
                for (int i = 0; i < days.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(days2[i].ToString("d MMM yy"), wholes[i, 0] + random.Next(0, i * 3));
                    chart1.Series[1].Points.AddXY(days2[i].ToString("d MMM yy"), wholes[i, 1] + random.Next(0, i * 3));
                }
            }
        }

        private void addedButton_Click(object sender, EventArgs e)
        {
            if (command != graphCommand.Add)
            {
                RefreshLegend();
                command = graphCommand.Add;
                RefreshGraph();
            }
        }

        private void removedButton_Click(object sender, EventArgs e)
        {
            if (command != graphCommand.Remove)
            {
                RefreshLegend();
                command = graphCommand.Remove;
                RefreshGraph();
            }
        }

        private void releasedButton_Click(object sender, EventArgs e)
        {
            if (command != graphCommand.Release)
            {
                RefreshLegend();
                command = graphCommand.Release;
                RefreshGraph();
            }
        }

        void clearLine()
        {
            seriesHit.MarkerStyle = MarkerStyle.None;
            seriesHit.Color = seriesOldColor;
            seriesHit.BorderWidth = 1;
            seriesHit.BorderDashStyle = ChartDashStyle.Dot;
        }
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            clearLine();
            if (chart1.HitTest(e.X, e.Y).ChartElementType == ChartElementType.LegendItem)
            {
                seriesHit = chart1.HitTest(e.X, e.Y).Series;
                seriesOldColor = seriesHit.Color;

                seriesHit.MarkerStyle = MarkerStyle.Circle;
                seriesHit.Color = Color.DarkRed;
                seriesHit.BorderWidth = 3;
                seriesHit.BorderDashStyle = ChartDashStyle.Solid;
            }
        }

        #endregion

        private void oSearchField_TextChanged(object sender, EventArgs e)
        {
            resultsBox.Items.Clear();
            //resultsBox.Items.AddRange(storage.searchWithString(oSearchField.Text));
            if (oSearchField.Text.Length > 0)
            {
                foreach (string s in storage.searchWithString(oSearchField.Text))
                {
                    resultsBox.Items.Add(s);
                }
            }
        }

        private void quarantinedButton_Click(object sender, EventArgs e)
        {
            if (command != graphCommand.Quarantine)
            {
                RefreshLegend();
                command = graphCommand.Quarantine;
                RefreshGraph();
            }
        }
        private void reprocessedButton_Click(object sender, EventArgs e)
        {
            if (command != graphCommand.Reprocess)
            {
                RefreshLegend();
                command = graphCommand.Reprocess;
                RefreshGraph();
            }
        }

        private void b_refresh_Click(object sender, EventArgs e)
        {
            InitializeValues();
            RefreshOverview();
        }

        //private void t_inventorySearch_TextChanged(object sender, EventArgs e)
        //{
        //    //resultsBox.Items.Clear();
        //    //resultsBox.Items.AddRange(storage.searchWithString(oSearchField.Text));
        //    if (oSearchField.Text.Length > 0)
        //    {
        //        foreach (Blood b in storage.searchBloodWithString(t_inventorySearch.Text))
        //        {
        //            //resultsBox.Items.Add(s);
        //        }
        //    }
        //}

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(tabControl1.SelectedIndex)
            {
                case 0:

                    break;
                case 1:
                    
                    break;
                case 2:
                    
                    break;
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           MessageBox.Show(dataGridView3.SelectedRows.ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label6.Text = DateTime.Now.ToShortTimeString();
        }

        private void b_addBlood_Click(object sender, EventArgs e)
        {
            AddItem a = new AddItem(storage);
            a.Show();
        }


        #region PieChart

        

        #endregion
    }
}
