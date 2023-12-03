using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        const byte H = 0;
        const byte L = 1;
        const byte O = 2;
        const byte C = 3;
        public bool preferflag = true;


        public Form1()
        {
            InitializeComponent();
        }

        readonly List<string> dates = new List<string>();

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
            EndAddingPoints("스킨앤스킨", "005931");
        }

        public void Clear()
        {
            foreach(var a in Series)
                a.Points.Clear();
            dates.Clear();
        }

        static int Parse(string s)
        {
            return int.Parse(s, NumberStyles.AllowThousands);
        }


        static void Assert(bool x, string msg)
        {
            if (!x) throw new Exception("Assertion failed: " + msg);
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
        public AnnotationCollection Annotations => chart1.Annotations;

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
            Assert(xviewpos >= AxisXMin, $"xviewpos {xviewpos} >= AxisXMin {AxisXMin}");
            Assert(xviewpos + xviewsize <= AxisXMax, $"xviewpos {xviewpos} + xviewsize {xviewsize} <= AxisXMax {AxisXMax}");
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
            Assert(h >= l, $"h {h} >= l {l}");
            Assert(h >= o, $"h {h} >= o {o}");
            Assert(h >= c, $"h {h} >= c {c}");
            Assert(l <= o, $"l {l} <= o {o}");
            Assert(l <= c, $"l {l} <= c {c}");
            Assert(l > 0, $"l {l} > 0");
            Assert(v >= 0, $"{v} >= 0");
            int idx = PPoints.AddXY(date, h, l, o, c);
            DataPoint pPoint = PPoints[idx];
            idx = VPoints.AddXY(date, v);
            DataPoint vPoint = VPoints[idx];
            if (c > o) pPoint.Color = Color.Red;
            dates.Add(date);
            vPoint.ToolTip = pPoint.ToolTip = $"일자: {date}\n시가: {o:N0}\n고가: {h:N0}\n저가: {l:N0}\n종가: {c:N0}\n거래량: {v:N0}";
        }


        /// <summary>
        /// Call me when all points were added.
        /// </summary>
        public void EndAddingPoints(string jmname, string jmcode)
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

            for(int i = 0; i < PPointsCount; i++)
            {
                double[] yvalues = PPoints[i].YValues;
                double h = yvalues[H];
                double l = yvalues[L];
                double o = yvalues[O];
                double c = yvalues[C];
                double v = VPoints[i].YValues[0];
                DataPoint vpoint = VPoints[i];
                double pc = c;
                double pv = v;
                string date = dates[i];

                if (i+1 < PPointsCount)
                {
                    pv = VPoints[i + 1].YValues[0];
                    pc = PPoints[i + 1].YValues[C];
                }

                double hc = h / pc - 1;
                double lc = l / pc - 1;
                double oc = o / pc - 1;
                double cc = c / pc - 1;
                double vc;
                if(pv == 0)
                    vc = 0;
                else
                    vc = v / pv;

                if(i == PPointsCount - 1)
                    hc = lc = oc = cc = vc = 0;

                string toolTip = $"일자: {date}\n{jmname}({jmcode})\n시가: {o:N0} ({oc:P2})\n고가: {h:N0} ({hc:P2})\n" +
                    $"저가: {l:N0} ({lc:P2})\n종가: {c:N0} ({cc:P2})\n거래량: {v:N0} ({vc:P2})";
                if (pv < v) vpoint.Color = Color.Red;
                foreach (var s in Series)
                {
                    if(i < s.Points.Count)
                        s.Points[i].ToolTip = toolTip;
                }
            }

            if (preferflag)
                SetXViewSizeSafely(preferredXViewSize);
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
            if (PPointsCount == 0) return;
            SetXViewSizeSafely(trackBar1.Value);
            preferredXViewSize = trackBar1.Value;
        }

        int preferredXViewSize = -1;

        void UpdateTrackBar()
        {
            trackBar1.Value = XViewSize;
            __trackBarToolTip.SetToolTip(trackBar1, XViewSize.ToString());
        }

        readonly ToolTip __trackBarToolTip = new ToolTip();

        bool leftflag;
        private void chart1_SelectionRangeChanging(object sender, CursorEventArgs e)
        {
            double start = e.NewSelectionStart;
            double end = e.NewSelectionEnd;
            leftflag = end > start;
        }
        private void chart1_SelectionRangeChanged(object sender, CursorEventArgs e)
        {
            if (PPointsCount == 0) return;

            if (leftflag)
            {
                SetXViewPosSize(AxisXMin, PPointsCount);
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

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            Point location = e.Location;
            ChartArea chartArea = FindChartArea(location);
            if(chartArea != null)
            {
                chartArea.CursorX.SetCursorPixelPosition(location, true);
                chartArea.CursorY.SetCursorPixelPosition(location, true);
                double ypos = chartArea.CursorY.Position;
                if (double.IsNaN(ypos))
                {
                    yLabel.Visible = false;
                }
                else
                {
                    yLabel.Text = ypos.ToString("N0");
                    yLabel.Top = chart1.Top + e.Y - yLabel.Height / 2;
                    yLabel.Visible = true;
                    
                    ElementPosition pos = chartArea.Position;
                    ElementPosition inpos = chartArea.InnerPlotPosition;
                    float areaX = pos.X * chart1.Width / 100;
                    float areaWidth = pos.Width * chart1.Width / 100;
                    float inX = inpos.X * areaWidth / 100;
                    float inWidth = inpos.Width * areaWidth / 100;
                    float plotX = areaX + inX;
                    yLabel.Left = chart1.Left + (int)(plotX + inWidth) + 9;
                }

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
            yLabel.Visible = false;
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
