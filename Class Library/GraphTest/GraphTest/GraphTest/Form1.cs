using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadChart();
        }

        void LoadChart()
        {
            chart1.ChartAreas.Add("area");
            chart1.ChartAreas["area"].AxisX.Minimum = 0;
            chart1.ChartAreas["area"].AxisX.Maximum = 20;
            chart1.ChartAreas["area"].AxisX.Interval = 1;
            chart1.ChartAreas["area"].AxisY.Minimum = 0;
            //chart1.ChartAreas["area"].AxisY.Maximum = 20;
            chart1.ChartAreas["area"].AxisY.Interval = 10;

            chart1.Series.Add("A");
            chart1.Series.Add("B");

            chart1.Series["A"].Color = Color.Red;
            chart1.Series["B"].Color = Color.Green;

            chart1.Series["A"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["B"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series["A"].Points.AddXY(0, 0);
            chart1.Series["B"].Points.AddXY(0, 0);

            chart1.Series["A"].Points.AddXY(1, 5);
            chart1.Series["B"].Points.AddXY(1, 7);
        }

    }
}
