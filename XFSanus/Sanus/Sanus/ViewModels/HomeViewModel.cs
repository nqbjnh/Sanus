using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using Prism.Navigation;
using Sanus.Model;
using Sanus.Services.Charts;
using Sanus.Services.Health;

namespace Sanus.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IChartService _chartService;
        private readonly string color = "#036abb";
        private double _goal = 8000;
        //
        private string _distances;
        public string Distances { get => _distances; set => SetProperty(ref _distances, value); }

        private string _percent;
        public string Percent { get => _percent; set => SetProperty(ref _percent, value); }

        private string _calories;
        public string Calories { get => _calories; set => SetProperty(ref _calories, value); }

        private string _steps;
        public string Steps { get => _steps; set => SetProperty(ref _steps, value); }

        private string _timeActive;
        public string TimeActive { get => _timeActive; set => SetProperty(ref _timeActive, value); }

        private Chart _stepsChart;
        public Chart StepsChart { get { return _stepsChart; } set => SetProperty(ref _stepsChart, value); }

        private Chart _percentChart;
        public Chart PercentChart { get { return _percentChart; } set => SetProperty(ref _percentChart, value); }
        //
        public HomeViewModel(INavigationService navigationService, IChartService chartService) : base(navigationService)
        {
            _navigationService = navigationService;
            _chartService = chartService;
            //
            FetchHealthData();
        }

        public void FetchHealthData()
        {
            //List<Task> tasks = new List<Task>();
            Xamarin.Forms.DependencyService.Get<IHealthServices>().GetHealthPermissionAsync((result) =>
            {
                //var a = result;
                if (result)
                {
                    GetData();
                }
                else
                {

                }
            });
        }

        public bool GetData()
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchSteps(async (totalSteps) =>
            {
                Steps = Math.Floor(totalSteps).ToString();
                // wait for them all to finish
                StepsChart = await _chartService.GetDistancesChartAsyns(_goal, double.Parse(Steps), color);
                //
                PercentChart = await _chartService.GetDistancesChartAsyns(_goal, double.Parse(Steps), "#23b8f9");
                Percent = Math.Round(((double.Parse(Steps) * 100) / _goal), 3).ToString();
            });

            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchMetersWalked((metersWalked) =>
            {
                double tempD = metersWalked / 10000000;
                Distances = String.Format("{0:0.##}", tempD);
            });

            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchActiveMinutes((activeMinutes) =>
            {
                TimeActive = Math.Floor(activeMinutes).ToString();
            });

            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchActiveEnergyBurned((caloriesBurned) =>
            {
                double tempC = caloriesBurned / 100000;
                Calories = String.Format("{0:0.##}", tempC);
            });
            //
            return true;
        }
    }
}
