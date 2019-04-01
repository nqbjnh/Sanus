using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microcharts;
using SkiaSharp;

namespace Sanus.Services.Charts
{
    public class ShowChartService : IChartService
    {
        public async Task<Chart> GetRadialGaugeChartAsyns(double goal, double steps, string color)
        {
            await Task.Delay(500);
            // truyen vao du lieu
            List<Entry> entri = new List<Entry>();
            //
            Entry e = new Entry((int)goal)
            {
                Color = SKColor.Parse(color)
            };
            //
            Entry en = new Entry((int)steps)
            {
                Color = SKColors.White
            };
            //
            entri.Add(e);
            entri.Add(en);
            return new RadialGaugeChart() { Entries = entri, BackgroundColor = SKColors.Transparent, Margin = 0 };
        }
        
        public async Task<Chart> GetChartAsyns<T>(Dictionary<DateTime, T> listData, string timeunit, string nameChart)
        {
            await Task.Delay(1);
            // truyen vao du lieu
            List<Entry> entri = new List<Entry>();
            //
            if (timeunit.Equals(Configuration.DAYS))
            {
                foreach (KeyValuePair<DateTime, T> item in listData)
                {
                    float a = float.Parse(item.Value.ToString());
                    Entry en = new Entry(a)
                    {
                        Color = SKColors.White,
                        Label = string.Format("{0:M/d}", item.Key),
                        TextColor = SKColors.White
                    };
                    entri.Add(en);
                }
            }
            else if (timeunit.Equals(Configuration.HOURS))
            {
                foreach (KeyValuePair<DateTime, T> item in listData)
                {
                    float a = float.Parse(item.Value.ToString());
                    Entry en = new Entry(a)
                    {
                        Color = SKColors.White,
                        Label = string.Format("{0:t}", item.Key),
                        TextColor = SKColors.White
                    };
                    entri.Add(en);
                }
            }
            else if (timeunit.Equals(Configuration.MONTHS))
            {
                foreach (KeyValuePair<DateTime, T> item in listData)
                {
                    float a = float.Parse(item.Value.ToString());
                    Entry en = new Entry(a)
                    {
                        Color = SKColors.White,
                        Label = item.Key.Day.ToString(),
                        TextColor = SKColors.White
                    };
                    entri.Add(en);
                }
            }
            // vẽ hình
            Chart chart = null;
            //
            switch (nameChart)
            {
                case Configuration.LINECHART:
                    chart = new LineChart() { Entries = entri, BackgroundColor = SKColors.Transparent, Margin = 20 };
                    break;
                case Configuration.BARCHART:
                    chart = new BarChart() { Entries = entri, BackgroundColor = SKColors.Transparent, Margin = 20 };
                    break;
                case Configuration.POINTCHART:
                    chart = new PointChart() { Entries = entri, BackgroundColor = SKColors.Transparent };
                    break;
            }
            return chart;
        }
    }
}
