using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Cursor = System.Windows.Forms.DataVisualization.Charting.Cursor;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            AddPoint("20230001", 4500, 500, 4300, 4400);
            for (int i = 15000; i < 15300; i++)
            {
                AddPoint("324" + i, 9+i, 4+i, 5+i, 6+i);
                AddPoint("999" + i, 9+i*2, 4+i*2, 6+i*2, 5+i*2);
            }
            AddPoint("20271231", 3200, 400, 1500, 400);
            EndAddingPoints();
        }
        void Assert(bool x, string msg=null)
        {
            if (!x)
                throw new Exception("Assertion failed: " + msg);
        }
        Series Series => chart1.Series[0];
        DataPointCollection Points => Series.Points;
        ChartArea ChartArea => chart1.ChartAreas[0];
        Axis AxisX => ChartArea.AxisX;
        AxisScaleView XView => AxisX.ScaleView;
        double AxisXMin => AxisX.Minimum;
        double AxisXMax => AxisX.Maximum;

        double XViewPos
        {
            get
            {
                if (double.IsNaN(XView.Position))
                    return 0.5;
                return XView.Position;
            }
            set
            {
                Assert(value >= AxisXMin, $"XViewPos: value({value})>=AxisXMin({AxisXMin})");
                Assert(value + XViewSize <= AxisXMax, $"XViewPos: value({value})+XViewSize({XViewSize})<=AxisXMax({AxisXMax})");
                hScrollBar1.Value = (int)value;
                XView.Position = value;
                UpdateY2View();
            }
        }

        int PointsCount => Points.Count;

        int XViewSize
        {
            get
            {
                if (double.IsNaN(XView.Size))
                    return Points.Count;
                return (int)XView.Size;
            }
            set
            {
                Assert(XViewPos + value <= AxisXMax, $"XViewSize: XViewPos({XViewPos})+value({value})<=AxisMax({AxisXMax})");
                trackBar1.Value = value;
                toolTip.SetToolTip(trackBar1, value.ToString());
                hScrollBar1.LargeChange = value;
                textBox1.Text = value.ToString();
                XView.Size = value;
                UpdateTextBox();
                UpdateY2View();
            }
        }

        /// <summary>
        /// Add point. First date first in.
        /// </summary>
        public void AddPoint(string date, int h, int l, int o, int c)
        {
            Assert(h >= l);
            Assert(h >= o);
            Assert(h >= c);
            Assert(l <= o);
            Assert(l <= c);
            Assert(l > 0);

            int idx = Points.AddXY(date, h, l, o, c);
            DataPoint point = Points[idx];
            point.Color = c > o ? Color.Red : Color.Blue;
        }

        public void EndAddingPoints()
        {
            AxisX.Minimum = 0.5;
            AxisX.Maximum = PointsCount + 0.5;
            hScrollBar1.Maximum = PointsCount - 1;
            hScrollBar1.LargeChange = trackBar1.Value = trackBar1.Maximum = PointsCount;
            label1.Text = $"/{PointsCount}";
            UpdateTextBox();
            chart1.Series[1].Points.MovingAverage(Points, 10);
        }

        void UpdateTextBox()
        {
            textBox1.Text = XViewSize.ToString();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            XViewPos = e.NewValue + 0.5;
        }

        public void SetXViewSizeBackward(int newSize)
        {
            if(newSize > PointsCount)
                newSize = PointsCount;
            if (newSize <= 0)
                newSize = 1;

            int oldSize = XViewSize;
            int delta = newSize - oldSize;
            double newPos = XViewPos - delta;
            if (delta < 0)
            {
                XViewSize = newSize;
                if (newPos < AxisXMin)
                    XViewPos = AxisXMin;
                else
                    XViewPos = newPos;
            }
            else
            {
                if (newPos < AxisXMin)
                    XViewPos = AxisXMin;
                else
                    XViewPos = newPos;
                XViewSize = newSize;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SetXViewSizeBackward(trackBar1.Value);
        }
        readonly ToolTip toolTip = new ToolTip();

        bool leftflag;
        private void chart1_SelectionRangeChanging(object sender, CursorEventArgs e)
        {
            double start = e.NewSelectionStart;
            double end = e.NewSelectionEnd;
            leftflag = end < start;
        }
        private void chart1_SelectionRangeChanged(object sender, CursorEventArgs e)
        {
            if (leftflag)
            {
                XViewPos = AxisXMin;
                XViewSize = PointsCount;
                leftflag = false;
            }
            hScrollBar1.LargeChange = trackBar1.Value = XViewSize;
            hScrollBar1.Value = (int)XViewPos;
            UpdateTextBox();
            UpdateY2View();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if('0' <= e.KeyChar && e.KeyChar <= '9')
            {

            }
            else if(e.KeyChar == (char)Keys.Back)
            {

            }
            else if(e.KeyChar == (char)Keys.Enter)
            {
                if(int.TryParse(textBox1.Text, out int newSize))
                {
                    SetXViewSizeBackward(newSize);
                    textBox1.SelectAll();
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        Cursor CursorX => ChartArea.CursorX;
        Cursor CursorY => ChartArea.CursorY;

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            Point location = e.Location;
            CursorX.SetCursorPixelPosition(location, true);
            CursorY.SetCursorPixelPosition(location, true);
        }

        private void chart1_MouseLeave(object sender, EventArgs e)
        {
            CursorX.SetCursorPosition(double.NegativeInfinity);
            CursorY.SetCursorPosition(double.NegativeInfinity);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetXViewSizeBackward((int)(XViewSize*1.1) + 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetXViewSizeBackward((int)(XViewSize * 0.9));
            UpdateY2View();
        }

        const byte L = 1;
        const byte H = 0;
        Axis AxisY2 => ChartArea.AxisY2;
        AxisScaleView Y2View => AxisY2.ScaleView;
        IEnumerable<DataPoint> ViewPoints => Points.Skip((int)XViewPos).Take(XViewSize);
        double ViewPointsMin => ViewPoints.Min(x => x.YValues[L]);
        double ViewPointsMax => ViewPoints.Max(x => x.YValues[H]);

        void UpdateY2View()
        {
            double y2viewmin = ViewPointsMin * 0.9;
            double y2viewmax = ViewPointsMax * 1.1;
            Y2View.Position = y2viewmin;
            Y2View.Size = y2viewmax - y2viewmin;
        }

    }
}
