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
        public Task<BarChart> GetBarChartAsyns()
        {
            throw new NotImplementedException();
        }

        public async Task<Chart> GetDistancesChartAsyns(double goal, double steps, string color)
        {
            await Task.Delay(500);
            // truyen vao du lieu
            List<Entry> entri = new List<Entry>();
            //
            Entry a = new Entry((int)goal)
            {
                Color = SKColor.Parse(color)
            };
            //
            Entry en = new Entry((int)steps)
            {
                Color = SKColors.White
            };
            //
            entri.Add(a);
            entri.Add(en);
            return new RadialGaugeChart() { Entries = entri, BackgroundColor = SKColors.Transparent, Margin = 0 };
        }

        public Task<DonutChart> GetDonutChartAsyns()
        {
            throw new NotImplementedException();
        }

        public Task<Chart> GetGreenChartAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LineChart> GetLineChartAsyns()
        {
            throw new NotImplementedException();
        }

        public Task<PointChart> GetPointChartAsyns()
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
