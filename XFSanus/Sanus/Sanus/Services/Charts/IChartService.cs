using Microcharts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanus.Services.Charts
{
    public interface IChartService
    {
        Task<Chart> GetRadialGaugeChartAsyns(double goal, double steps, string color);
        //Task<Chart> GetGreenChartAsync();
        //Task<PointChart> GetPointChartAsyns<T>(List<T> listData);
        //Task<BarChart> GetBarChartAsyns<T>(List<T> listData);
        //Task<LineChart> GetLineChartAsyns<T>(List<T> listData);
        //Task<DonutChart> GetDonutChartAsyns();
        //Task<RadialGaugeChart> GetRadiaChartAsyns();
        //Task<RadarChart> GetRadarChartAsyns();
        Task<Chart> GetChartAsyns<T>(Dictionary<DateTime, T> listData, string timeunit, string nameChart);
    }
}
