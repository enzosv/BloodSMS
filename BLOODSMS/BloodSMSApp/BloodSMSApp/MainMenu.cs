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
        public Storage storage;
        int bloodCount;
        List<string> notifications;
        graphCommand command;
        Dictionary<DateTime, int> days;
        Dictionary<int, DateTime> days2;
        Series seriesHit;
        Color seriesOldColor;

        public MainMenu()
        {
            InitializeComponent();
            
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 1000;
            label3.Text = DateTime.Now.ToString("MMMM d, yyyy");
            storage = new Storage("localhost", "bsms", "root", "root");
            notifications = new List<string>();
            command = graphCommand.Summary;

            days = new Dictionary<DateTime, int>();
            days2 = new Dictionary<int, DateTime>();


            //set this to date of install upon install
            dateFrom.MinDate = new DateTime(2010, 9, 1);

            dateFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
            dateTo.Value = DateTime.Today.AddMonths(1);
            dateTo.MaxDate = dateTo.Value;

            seriesHit = chart1.Series[0];

            foreach (bloodType x in (bloodType[])Enum.GetValues(typeof(bloodType)))
            {
                contactTypesBox.Items.Add(MyEnums.GetDescription(x));
            }
            contactTypesBox.SelectedIndex = 0;

            RefreshOverview();
            
        }

        private void b_refresh_Click(object sender, EventArgs e)
        {
            RefreshStorage();   
        }

        public void RefreshStorage()
        {
            storage = new Storage("localhost", "bsms", "root", "root");
            ChangeDates();
            RefreshOverview();
        }

        #region overview

        public void RefreshOverview()
        {
            oSearchField.Clear();
            RefreshNotifications();
            RefreshPieChart();
            RefreshGraph();
            RefreshBloodGrid(storage.bloodList);
            RefreshDonorGrid(storage.donorList);
        }

        void RefreshBloodGrid(List<Blood> bloods)
        {
            dataGridView2.Rows.Clear();
            for (int i = 0; i < bloods.Count; i++)
            {
                dataGridView2.Rows.Add();
                Blood b = bloods[i];
                dataGridView2.Rows[i].Cells[0].Value = b.Accession_number;
                dataGridView2.Rows[i].Cells[1].Value = MyEnums.GetDescription(b.Blood_type);
                if (b.Donor_id.HasValue)
                    dataGridView2.Rows[i].Cells[2].Value = storage.findDonor(b.Donor_id.Value).Name;
                else
                    dataGridView2.Rows[i].Cells[2].Value = "Bought from other bank";
                dataGridView2.Rows[i].Cells[3].Value = b.Date_donated.ToShortDateString();
                dataGridView2.Rows[i].Cells[4].Value = b.Is_removed;
            }
        }

        public void RefreshDonorGrid(List<Donor> donors)
        {
            dataGridView3.Rows.Clear();
            for (int i = 0; i < donors.Count; i++)
            {
                dataGridView3.Rows.Add();
                Donor d = donors[i];
                dataGridView3.Rows[i].Cells[0].Value = d.Name;
                dataGridView3.Rows[i].Cells[1].Value = MyEnums.GetDescription(d.Blood_type);
                dataGridView3.Rows[i].Cells[2].Value = MyEnums.GetDescription(d.Home_city);
                dataGridView3.Rows[i].Cells[3].Value = d.Date_registered.ToShortDateString();
                dataGridView3.Rows[i].Cells[4].Value = d.Is_viable;
                dataGridView3.Rows[i].Cells[5].Value = d.Is_contactable;
            }
        }

        void RefreshNotifications()
        {
            notifications.Clear();
            //check low level
            bloodType blood_type;
            for (int i = 0; i < storage.bloodTypes.Length; i++)
            {
                blood_type = (bloodType)i;
                if (storage.AlertLowLevel(i))
                {
                    notifications.Add("Low " + MyEnums.GetDescription(blood_type) + " Supply");
                }
            }

            //check near expirations
            foreach (string[] s in storage.AlertNearExpiration())
            {
                notifications.Add(s[1] + " - " + s[0] + " near expiration");
            }
            notificationsList.Items.Clear();
            for (int i = 0; i < notifications.Count; i++)
            {
                notificationsList.Items.Add(notifications[i]);
            }
        }

        void RefreshPieChart()
        {
            chart2.Series[0].Points.Clear();
            bloodCount = storage.availableBlood.Count;
            bloodType b;
            chart2.Series[0].Points.AddY(1200 - bloodCount);
            chart2.Series[0].Points[0].LegendText = "Free Space: " + (1200 - bloodCount);
            chart2.Series[0].Points[0].Color = Color.Transparent;
            List<Blood>[] bloodTypes = storage.bloodTypes;
            for (int i = 0; i < bloodTypes.Length; i++)
            {
                b = (bloodType)i;
                chart2.Series[0].Points.AddY(bloodTypes[i].Count);
                chart2.Series[0].Points[i+1].LegendText = MyEnums.GetDescription(b) + ": " + bloodTypes[i].Count;
            }
        }

        #endregion

        #region graph

        void RefreshGraph()
        {
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                chart1.Series[i].Points.Clear();
            }
            if (command != graphCommand.Summary)
            {
                int[] totals = storage.getBloodModifiedDuring(days, command);
                int[,] types = storage.getBloodTypeModifiedDuring(days, command);
                for (int i = 0; i < days.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(days2[i].ToString("d MMM yy"), totals[i]);
                    for (int j = 1; j <= Enum.GetNames(typeof(bloodType)).Length; j++)
                    {
                        chart1.Series[j].Points.AddXY(days2[i].ToString("d MMM yy"), types[i, j - 1]);
                    }
                }
            }
            else
            {
                int[,] wholes = storage.getSummary(days);
                for (int i = 0; i < days.Count; i++)
                {
                    chart1.Series[0].Points.AddXY(days2[i].ToString("d MMM yy"), wholes[i, 0]);
                    chart1.Series[1].Points.AddXY(days2[i].ToString("d MMM yy"), wholes[i, 1]);
                }
            }
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
            RefreshGraph();
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
            if (oSearchField.Text.Length > 0)
            {
                foreach (string s in storage.searchWithString(oSearchField.Text))
                {
                    resultsBox.Items.Add(s);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label6.Text = DateTime.Now.ToShortTimeString();
        }

        #region buttons
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
        private void b_addBlood_Click(object sender, EventArgs e)
        {
            PreAddItem a = new PreAddItem(this);
            a.ShowDialog();
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

        private void b_addDonor_Click(object sender, EventArgs e)
        {
            PreAddDonor a = new PreAddDonor(this);
            a.ShowDialog();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            PreAddItem i = new PreAddItem(this);
            i.ShowDialog();
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

        private void b_contactAB1_Click(object sender, EventArgs e)
        {
            int count;
            if (int.TryParse(t_AB1.Text, out count))
            {
                if (count > 0)
                {
                    ContactForm cf = new ContactForm(storage.getClosestByType(count, contactTypesBox.SelectedIndex));
                    cf.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Please input number of people to contact");
            }
        }

        private void t_AB1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(Char.IsNumber(e.KeyChar) || e.KeyChar == 8);
        }

        private void b_inventoryALL_Click(object sender, EventArgs e)
        {
            RefreshBloodGrid(storage.bloodList);
        }

        private void b_donorAll_Click(object sender, EventArgs e)
        {
            RefreshDonorGrid(storage.donorList);
        }

        private void resultsBox_DoubleClick(object sender, EventArgs e)
        {
            //int selection = resultsBox.SelectedIndex;
            if (resultsBox.SelectedIndex != -1)
            {
                string selection = resultsBox.Items[resultsBox.SelectedIndex].ToString();
                if (!String.IsNullOrWhiteSpace(selection))
                {
                    Donor d = storage.findDonorWithName(selection);
                    Blood b = storage.findBlood(selection);
                    if (b != null)
                    {
                        ShowBlood sb = new ShowBlood(this, b.Accession_number);
                        sb.ShowDialog();
                    }
                    else if (d != null)
                    {
                        AddDonor a = new AddDonor(this, d);
                        a.ShowDialog();
                    }

                }
            }
            
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                Blood b = storage.findBlood(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (b != null)
                {
                    ShowBlood sb = new ShowBlood(this, b.Accession_number);
                    sb.ShowDialog();
                }
            }
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                Donor d = storage.findDonorWithName(dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (d != null)
                {
                    AddDonor a = new AddDonor(this, d);
                    a.ShowDialog();
                }
            }
        }

        private void b_inventoryInventory_Click(object sender, EventArgs e)
        {
            RefreshBloodGrid(storage.availableBlood);
        }

        private void b_inventoryQuarantined_Click(object sender, EventArgs e)
        {
            RefreshBloodGrid(storage.unavailableBlood);
        }

        private void iTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshBloodGrid(storage.bloodTypes[iTypeFilter.SelectedIndex]);
        }

        private void b_donorContactable_Click(object sender, EventArgs e)
        {
            RefreshDonorGrid(storage.contactableDonors);
        }

        private void b_donorBanned_Click(object sender, EventArgs e)
        {
            RefreshDonorGrid(storage.bannedDonors);
        }

        private void b_donorViable_Click(object sender, EventArgs e)
        {
            RefreshDonorGrid(storage.viableDonors);
        }

        private void dTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDonorGrid(storage.donorTypes[dTypeFilter.SelectedIndex]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
