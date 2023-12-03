using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
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
        const byte H = 0;
        const byte L = 1;
        const byte O = 2;
        const byte C = 3;
        public Form1()
        {
            InitializeComponent();
        }

        public void AddAllPointsByFile(string csvfilename)
        {
            Clear();
            string[] lines = File.ReadAllLines(csvfilename);
            foreach(string line in lines)
            {
                string[] tokens = line.Split('\t');
                if(tokens.Length >= 11)
                {
                    if (tokens[0].Length > 0)
                        AddPoint(tokens[0], Parse(tokens[2]), Parse(tokens[3]), Parse(tokens[1]), Parse(tokens[4]), Parse(tokens[10]));
                }
            }
            EndAddingPoints();
        }

        public void Clear()
        {
            foreach(var a in Series)
                a.Points.Clear();
        }

        static int Parse(string s)
        {
            return int.Parse(s, NumberStyles.AllowThousands);
        }


        static void Assert(bool x, string msg=null)
        {
            if (!x)
                throw new Exception("Assertion failed: " + msg);
        }

        SeriesCollection Series => chart1.Series;
        Series PSeries => Series["p"];
        DataPointCollection PPoints => PSeries.Points;
        ChartAreaCollection ChartAreas => chart1.ChartAreas;
        ChartArea PChartArea => ChartAreas["p"];
        ChartArea VChartArea => ChartAreas["v"];
        Axis PAxisX => PChartArea.AxisX;
        AxisScaleView XView => PAxisX.ScaleView;

        Axis VAxisX => VChartArea.AxisX;

        double AxisXMin
        {
            get
            {
                if (double.IsNaN(PAxisX.Minimum))
                    return 0;
                return PAxisX.Minimum;
            }
            set
            {
                VAxisX.Minimum = PAxisX.Minimum = value;
            }
        }

        double AxisXMax
        {
            get
            {
                if (double.IsNaN(PAxisX.Maximum))
                    return 0;
                return PAxisX.Maximum;
            }
            set
            {
                VAxisX.Maximum = PAxisX.Maximum = value;
            }
        }

        double AxisXSize => AxisXMax - AxisXMin;
        double XViewPos
        {
            get
            {
                if (double.IsNaN(XView.Position))
                    return AxisXMin;
                return XView.Position;
            }
            set
            {
                SetXViewPosSize(value, XViewSize);
            }
        }

        void SetXViewPosSize(double xviewpos, int xviewsize)
        {
            Assert(xviewpos >= AxisXMin);
            Assert(xviewpos + xviewsize <= AxisXMax);
            XView.Position = xviewpos;
            XView.Size = xviewsize;

            UpdateYViews();
            UpdateScrollBar();
            UpdateTrackBar();
            UpdateTextBox();
        }
        

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.F12)
            {
                AddAllPointsByFile("LG생활건강.txt");
            }
            else if (e.KeyCode == Keys.F11)
            {
                AddAllPointsByFile("엔에이치스팩30.txt");
            }
        }



        int PPointsCount => PPoints.Count;

        int XViewSize
        {
            get
            {
                if (double.IsNaN(XView.Size))
                    return PPoints.Count;
                return (int)XView.Size;
            }
            set
            {
                SetXViewPosSize(XViewPos, value);
            }
        }

        DataPointCollection VPoints => Series["v"].Points;

        /// <summary>
        /// Add point. Last date first in.
        /// </summary>
        public void AddPoint(string date, int h, int l, int o, int c, int v)
        {
            Assert(h >= l);
            Assert(h >= o);
            Assert(h >= c);
            Assert(l <= o);
            Assert(l <= c);
            Assert(l > 0);
            Assert(v >= 0);
            int idx = PPoints.AddXY(date, h, l, o, c);
            VPoints.AddXY(date, v);
            DataPoint point = PPoints[idx];
            point.Color = c > o ? Color.Red : Color.Blue;
        }

        /// <summary>
        /// Call me when all points were added.
        /// </summary>
        public void EndAddingPoints()
        {
            hScrollBar1.Maximum = PPointsCount - 1;
            hScrollBar1.LargeChange = trackBar1.Value = trackBar1.Maximum = PPointsCount;
            AxisXMin = 0.5;
            AxisXMax = PPointsCount + 0.5;
            SetXViewPosSize(0.5, PPoints.Count);
            label1.Text = $"/{PPointsCount}";
            UpdateTextBox();
            MovingAverage(Series["5"].Points, PPoints, 5);
            MovingAverage(Series["20"].Points, PPoints, 20);
            MovingAverage(Series["60"].Points, PPoints, 60);
            MovingAverage(Series["120"].Points, PPoints, 120);
            UpdateYViews();
            SetXViewSizeSafely(preferredXViewSize);
        }

        ChartArea FindChartArea(Point point)
        {
            for (int i = 0; i < ChartAreas.Count; i++)
            {
                ChartArea chartArea = ChartAreas[i];
                ElementPosition pos = chartArea.Position;
                ElementPosition inpos = chartArea.InnerPlotPosition;
                float areaX = pos.X * chart1.Width / 100;
                float areaWidth = pos.Width * chart1.Width / 100;
                float areaHeight = pos.Height * chart1.Height / 100;
                float areaY = pos.Y * chart1.Height / 100;
                float inX = inpos.X * areaWidth / 100;
                float inY = inpos.Y * areaHeight / 100;
                float inWidth = inpos.Width * areaWidth / 100;
                float inHeight = inpos.Height * areaHeight / 100;
                float plotX = areaX + inX;
                float plotY = areaY + inY;
                if (point.X >= plotX && point.Y >= plotY && point.X - plotX <= inWidth && point.Y - plotY <= inHeight)
                {
                    return chartArea;
                }
            }
            return null;
        }

        static void MovingAverage(DataPointCollection points, DataPointCollection target, int n)
        {
            for (int i = n - 1; i < target.Count; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += target[i - j].YValues[C];
                }
                points.AddXY(target[i].XValue, sum / n);
            }
        }


        void UpdateTextBox()
        {
            textBox1.Text = XViewSize.ToString();
        }



        //---
        // Value = AxisXSize - XViewPos - XViewSize + 0.5
        // LargeChange = XViewSize;
        void UpdateScrollBar()
        {
            hScrollBar1.Value = (int)(AxisXSize - XViewPos - XViewSize + 0.5);
            hScrollBar1.LargeChange = XViewSize;
        }
        // XViewPos = AxisXSize - Value - XViewSize + 0.5
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if(PPointsCount > 0)
                XViewPos = AxisXSize - e.NewValue - XViewSize + 0.5;
        }
        //---

        public void SetXViewSizeSafely(int newSize)
        {
            if (PPointsCount > 0)
            {
                if (newSize == -1)
                    newSize = PPointsCount;
                else if (newSize == 0)
                    newSize = 1;
                else if (newSize > PPointsCount)
                    newSize = PPointsCount;

                if (XViewPos + newSize > AxisXMax)
                    XViewPos -= XViewPos + newSize - AxisXMax;
                XViewSize = newSize;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (PPointsCount > 0)
            {
                SetXViewSizeSafely(trackBar1.Value);
                preferredXViewSize = trackBar1.Value;
            }
        }

        int preferredXViewSize = -1;

        void UpdateTrackBar()
        {
            trackBar1.Value = XViewSize;
            toolTip.SetToolTip(trackBar1, XViewSize.ToString());
        }

        readonly ToolTip toolTip = new ToolTip();

        bool leftflag;
        private void chart1_SelectionRangeChanging(object sender, CursorEventArgs e)
        {
            double start = e.NewSelectionStart;
            double end = e.NewSelectionEnd;
            leftflag = end > start;
        }
        private void chart1_SelectionRangeChanged(object sender, CursorEventArgs e)
        {
            if (PPointsCount > 0)
            {
                if (leftflag)
                {
                    XViewPos = AxisXMin;
                    XViewSize = PPointsCount;
                    preferredXViewSize = -1;
                    leftflag = false;
                }
                else
                    preferredXViewSize = XViewSize;
                UpdateTrackBar();
                UpdateScrollBar();
                UpdateTextBox();
                UpdateYViews();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ('0' <= e.KeyChar && e.KeyChar <= '9')
            {

            }
            else if (e.KeyChar == (char)Keys.Back)
            {

            }
            else if (e.KeyChar == (char)Keys.Enter)
            {

                if (int.TryParse(textBox1.Text, out int newSize))
                {
                    preferredXViewSize = newSize;
                    SetXViewSizeSafely(newSize);
                }
                else
                    SetXViewSizeSafely(int.MaxValue);
                UpdateTextBox();
                textBox1.SelectAll();
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            Point location = e.Location;
            ChartArea chartArea = FindChartArea(location);
            if(chartArea != null)
            {
                chartArea.CursorX.SetCursorPixelPosition(location, true);
                chartArea.CursorY.SetCursorPixelPosition(location, true);
            }
            else
                HideCursors();
        }

        void HideCursors()
        {
            foreach(var chartArea in ChartAreas)
            {
                chartArea.CursorX.SetCursorPosition(double.NegativeInfinity);
                chartArea.CursorY.SetCursorPosition(double.NegativeInfinity);
            }
        }

        private void chart1_MouseLeave(object sender, EventArgs e)
        {
            HideCursors();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(PPointsCount > 0)
            {
                int newSize = (int)Math.Ceiling(XViewSize * 1.1);
                if (newSize > PPointsCount)
                    newSize = PPointsCount;
                SetXViewSizeSafely(newSize);
                preferredXViewSize = newSize;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (PPointsCount > 0)
            {
                int newSize = (int)(XViewSize * 0.9);
                if (newSize == 0)
                    newSize = 1;
                SetXViewSizeSafely(newSize);
                preferredXViewSize = newSize;
            }
        }


        Axis PAxisY => PChartArea.AxisY;
        AxisScaleView PYView => PAxisY.ScaleView;
        IEnumerable<DataPoint> PViewPoints => PPoints.Skip((int)XViewPos).Take(XViewSize);
        double PViewPointsMin => PViewPoints.Min(x => x.YValues[L]);
        double PViewPointsMax => PViewPoints.Max(x => x.YValues[H]);

        void UpdatePYView()
        {
            double min = PViewPointsMin * 0.9;
            double max = PViewPointsMax * 1.1;
            PYView.Position = min;
            PYView.Size = max - min;
        }
        Axis VAxisY => VChartArea.AxisY;
        AxisScaleView VYView => VAxisY.ScaleView;

        IEnumerable<DataPoint> VViewPoints => VPoints.Skip((int)XViewPos).Take(XViewSize);
        double VViewPointsMax => VViewPoints.Max(x => x.YValues[0]);
        void UpdateVYView()
        {
            VYView.Size = VViewPointsMax * 1.1;
        }
        void UpdateYViews()
        {
            UpdatePYView();
            UpdateVYView();
        }
       

    }
}
