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
        Task<Microcharts.Chart> GetDistancesChartAsyns(double goal, double steps, string color);
        Task<Microcharts.Chart> GetGreenChartAsync();
        Task<Microcharts.PointChart> GetPointChartAsyns();
        Task<Microcharts.BarChart> GetBarChartAsyns();
        Task<Microcharts.LineChart> GetLineChartAsyns();
        Task<Microcharts.DonutChart> GetDonutChartAsyns();
        Task<Microcharts.RadialGaugeChart> GetRadiaChartAsyns();
        Task<Microcharts.RadarChart> GetRadarChartAsyns();
    }
}
