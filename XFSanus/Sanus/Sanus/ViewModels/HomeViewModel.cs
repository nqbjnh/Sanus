using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using Prism.Commands;
using Prism.Navigation;
using Sanus.Model;
using Sanus.Services.Charts;
using Sanus.Services.Dialog;
using Sanus.Services.Health;

namespace Sanus.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IChartService _chartService;
        private readonly IDialogService _dialogService;
        private readonly string color = "#036abb";
        private double _goal = 8000;
        //
        private string _distances;
        private string _percent;
        private string _calories;
        private string _steps;
        private string _timeActive;
        private Chart _stepsChart;
        private Chart _percentChart;
        //
        public string Distances { get => _distances; set => SetProperty(ref _distances, value); }
        public string Percent { get => _percent; set => SetProperty(ref _percent, value); }
        public string Calories { get => _calories; set => SetProperty(ref _calories, value); }
        public string Steps { get => _steps; set => SetProperty(ref _steps, value); }
        public string TimeActive { get => _timeActive; set => SetProperty(ref _timeActive, value); }
        public Chart StepsChart { get { return _stepsChart; } set => SetProperty(ref _stepsChart, value); }
        public Chart PercentChart { get { return _percentChart; } set => SetProperty(ref _percentChart, value); }
        //
        public DelegateCommand StepsCommand { get; }
        public DelegateCommand DistanceCommand { get; }
        public DelegateCommand EnegyCommand { get; }
        //
        public HomeViewModel(INavigationService navigationService, IChartService chartService, IDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _chartService = chartService;
            _dialogService = dialogService;
            //
            FetchHealthData();
            //
            StepsCommand = new DelegateCommand(StepsSelects);
            DistanceCommand = new DelegateCommand(DistanceSelects);
            EnegyCommand = new DelegateCommand(EnegySelects);
        }
        //
        private async void StepsSelects()
        {
            await _navigationService.NavigateAsync("NavigationPage/StepsHistoryPage");
        }
        private async void DistanceSelects()
        {
            await _navigationService.NavigateAsync("NavigationPage/DistanceHistoryPage");
        }
        private async void EnegySelects()
        {
            await _navigationService.NavigateAsync("NavigationPage/EnegyHistoryPage");
        }
        //
        public void FetchHealthData()
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().GetHealthPermissionAsync((result) =>
            {
                if (result)
                {
                    GetData();
                }
                else
                {
                    _dialogService.ShowConfirmAsync("Load data fail", "Fail", "Ok", "Cancel");
                }
            });
        }

        public bool GetData()
        {
            var platform = Xamarin.Forms.Device.RuntimePlatform;
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchSteps(async (totalSteps) =>
            {
                Steps = Math.Floor(totalSteps).ToString();
                // wait for them all to finish
                StepsChart = await _chartService.GetRadialGaugeChartAsyns(_goal, double.Parse(Steps), color);
                //
                PercentChart = await _chartService.GetRadialGaugeChartAsyns(_goal, double.Parse(Steps), "#23b8f9");
                Percent = Math.Round(((double.Parse(Steps) * 100) / _goal), 3).ToString();
            });

            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchMetersWalked((metersWalked) =>
            {
                Distances = String.Format("{0:0.##}", metersWalked);
            });

            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchActiveMinutes((activeMinutes) =>
            {
                TimeActive = Math.Floor(activeMinutes).ToString();
            });

            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchActiveEnergyBurned((caloriesBurned) =>
            {
                Calories = string.Format("{0:0.###}", caloriesBurned);
            });
            // lay so buoc theo mot khoang thoi gian
            //Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchSteps(async (totalSteps) =>
            //{
            //    Steps = Math.Floor(totalSteps).ToString();
            //    // wait for them all to finish
            //    StepsChart = await _chartService.GetDistancesChartAsyns(_goal, double.Parse(Steps), color);
            //    //
            //    PercentChart = await _chartService.GetDistancesChartAsyns(_goal, double.Parse(Steps), "#23b8f9");
            //    Percent = Math.Round(((double.Parse(Steps) * 100) / _goal), 3).ToString();
            //}, new DateTime(2019, 3, 4), new DateTime(2019, 3, 10, 23, 59, 59));
            //
            return true;
        }
    }
}
