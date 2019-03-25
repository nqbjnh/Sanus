using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using Sanus.Model;
using SkiaSharp;

namespace Sanus.Services.Charts
{
    public class ShowChartService : IChartService
    {
        public async Task<BarChart> GetBarChartAsyns<T>(List<T> listData)
        {
            await Task.Delay(1);
            // truyen vao du lieu
            List<Entry> entri = new List<Entry>();
            //
            for (int i = 0; i < listData.Count(); i++)
            {
                float a = float.Parse(listData.ElementAt(i).ToString());
                Entry en = new Entry(a)
                {
                    Color = SKColors.White
                };
                //
                entri.Add(en);
            }
            //
            return new BarChart() { Entries = entri, BackgroundColor = SKColors.Transparent, Margin = 20 };
        }

        public async Task<BarChart> GetBarChartAsyns<T>(Dictionary<DateTime, T> listData, string timeunit)
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

            //
            return new BarChart() { Entries = entri, BackgroundColor = SKColors.Transparent, Margin = 20 };
        }

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

        public async Task<PointChart> GetPointChartAsyns<T>(List<T> listData)
        {
            await Task.Delay(1);
            // truyen vao du lieu
            List<Entry> entri = new List<Entry>();
            //
            for (int i = 0; i < listData.Count(); i++)
            {
                float a = float.Parse(listData.ElementAt(i).ToString());
                Entry en = new Entry(a)
                {
                    Color = SKColors.White
                };
                //
                entri.Add(en);
            }
            //
            return new PointChart() { Entries = entri, BackgroundColor = SKColors.Transparent, Margin = 0 };
        }

        public async Task<LineChart> GetLineChartAsyns<T>(List<T> listData)
        {
            await Task.Delay(1);
            // truyen vao du lieu
            List<Entry> entri = new List<Entry>();
            //
            for (int i = 0; i < listData.Count(); i++)
            {
                float a = float.Parse(listData.ElementAt(i).ToString());
                Entry en = new Entry(a)
                {
                    Color = SKColors.White
                };
                //
                entri.Add(en);
            }
            //
            return new LineChart() { Entries = entri, BackgroundColor = SKColors.Transparent, Margin = 0 };
        }

        public Task<DonutChart> GetDonutChartAsyns()
        {
            throw new NotImplementedException();
        }

        public Task<Chart> GetGreenChartAsync()
        {
            throw new NotImplementedException();
        }



        public Task<RadarChart> GetRadarChartAsyns()
        {
            throw new NotImplementedException();
        }

        public Task<RadialGaugeChart> GetRadiaChartAsyns()
        {
            throw new NotImplementedException();
        }
    }
}
