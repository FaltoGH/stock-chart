namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.yLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea3.AxisX.IntervalOffset = 0.5D;
            chartArea3.AxisX.IsMarginVisible = false;
            chartArea3.AxisX.IsReversed = true;
            chartArea3.AxisX.LabelStyle.Enabled = false;
            chartArea3.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea3.AxisX.ScrollBar.Enabled = false;
            chartArea3.AxisY.IsStartedFromZero = false;
            chartArea3.AxisY.LabelStyle.Format = "N0";
            chartArea3.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea3.AxisY.ScrollBar.Enabled = false;
            chartArea3.AxisY2.LabelStyle.Format = "N0";
            chartArea3.AxisY2.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea3.AxisY2.ScrollBar.Enabled = false;
            chartArea3.CursorX.AutoScroll = false;
            chartArea3.CursorX.IntervalOffset = 0.5D;
            chartArea3.CursorX.IsUserSelectionEnabled = true;
            chartArea3.Name = "p";
            chartArea4.AlignWithChartArea = "p";
            chartArea4.AxisX.IntervalOffset = 0.5D;
            chartArea4.AxisX.IsMarginVisible = false;
            chartArea4.AxisX.IsReversed = true;
            chartArea4.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea4.AxisX.ScrollBar.Enabled = false;
            chartArea4.AxisY.LabelStyle.Format = "#,##0,K";
            chartArea4.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea4.AxisY.ScrollBar.Enabled = false;
            chartArea4.CursorX.AutoScroll = false;
            chartArea4.CursorX.IntervalOffset = 0.5D;
            chartArea4.CursorX.IsUserSelectionEnabled = true;
            chartArea4.Name = "v";
            this.chart1.ChartAreas.Add(chartArea3);
            this.chart1.ChartAreas.Add(chartArea4);
            this.chart1.Location = new System.Drawing.Point(23, 39);
            this.chart1.Name = "chart1";
            series7.ChartArea = "p";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Color = System.Drawing.Color.Red;
            series7.Name = "5";
            series7.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            series8.BorderWidth = 2;
            series8.ChartArea = "p";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Color = System.Drawing.Color.Gold;
            series8.Name = "20";
            series9.BorderWidth = 2;
            series9.ChartArea = "p";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series9.Color = System.Drawing.Color.Green;
            series9.Name = "60";
            series10.ChartArea = "p";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series10.Color = System.Drawing.Color.Gray;
            series10.Name = "120";
            series11.ChartArea = "p";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series11.CustomProperties = "PriceDownColor=Blue, PriceUpColor=Red";
            series11.Name = "p";
            series11.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            series11.YValuesPerPoint = 4;
            series11.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series12.ChartArea = "v";
            series12.Name = "v";
            this.chart1.Series.Add(series7);
            this.chart1.Series.Add(series8);
            this.chart1.Series.Add(series9);
            this.chart1.Series.Add(series10);
            this.chart1.Series.Add(series11);
            this.chart1.Series.Add(series12);
            this.chart1.Size = new System.Drawing.Size(776, 417);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.SelectionRangeChanging += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CursorEventArgs>(this.chart1_SelectionRangeChanging);
            this.chart1.SelectionRangeChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CursorEventArgs>(this.chart1_SelectionRangeChanged);
            this.chart1.MouseLeave += new System.EventHandler(this.chart1_MouseLeave);
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.LargeChange = 1;
            this.hScrollBar1.Location = new System.Drawing.Point(23, 603);
            this.hScrollBar1.Maximum = 0;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(574, 22);
            this.hScrollBar1.TabIndex = 1;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(600, 603);
            this.trackBar1.Maximum = 1;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(95, 45);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Value = 1;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(727, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(52, 21);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "0";
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(782, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "/0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(694, 600);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 25);
            this.button1.TabIndex = 5;
            this.button1.Text = "-";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(727, 600);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(27, 25);
            this.button2.TabIndex = 5;
            this.button2.Text = "+";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // yLabel
            // 
            this.yLabel.AutoSize = true;
            this.yLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(223)))), ((int)(((byte)(114)))));
            this.yLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.yLabel.Location = new System.Drawing.Point(767, 432);
            this.yLabel.Name = "yLabel";
            this.yLabel.Size = new System.Drawing.Size(45, 14);
            this.yLabel.TabIndex = 6;
            this.yLabel.Text = "yLabel";
            this.yLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 654);
            this.Controls.Add(this.yLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.chart1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label yLabel;
    }
}