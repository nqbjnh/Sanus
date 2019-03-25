using Microcharts;
using Sanus.Model;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sanus.Services.Charts
{
    public interface IChartService
    {
        Task<Chart> GetRadialGaugeChartAsyns(double goal, double steps, string color);
        Task<Chart> GetGreenChartAsync();
        Task<PointChart> GetPointChartAsyns<T>(List<T> listData);
        Task<BarChart> GetBarChartAsyns<T>(List<T> listData);
        Task<BarChart> GetBarChartAsyns<T>(Dictionary<DateTime, T> listData, string timeunit);
        Task<LineChart> GetLineChartAsyns<T>(List<T> listData);
        Task<DonutChart> GetDonutChartAsyns();
        Task<RadialGaugeChart> GetRadiaChartAsyns();
        Task<RadarChart> GetRadarChartAsyns();
    }
}
