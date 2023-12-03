using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public static class Extensions
    {
        public static void MovingAverage(this DataPointCollection points, DataPointCollection target, int n)
        {
            const byte C = 3;
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
    }
}
