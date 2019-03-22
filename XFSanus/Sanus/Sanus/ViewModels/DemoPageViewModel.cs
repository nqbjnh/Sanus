using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Sanus.Services.Charts;
using Sanus.Services.Health;
using Xamarin.Forms;

namespace Sanus.ViewModels
{
    public class DemoPageViewModel : ViewModelBase
    {
        private string _clickButton;
        public string ClickButton { get => _clickButton; set => SetProperty(ref _clickButton, value); }

        private string _result;
        public string Result { get => _result; set => SetProperty(ref _result, value); }
        private string _result1;
        public string Result1 { get => _result1; set => SetProperty(ref _result1, value); }
        private string _result2;
        public string Result2 { get => _result2; set => SetProperty(ref _result2, value); }
        private string _result3;
        public string Result3 { get => _result3; set => SetProperty(ref _result3, value); }
        private string _result4;
        public string Result4 { get => _result4; set => SetProperty(ref _result4, value); }
        //
        private ObservableCollection<double> gardenDataCollection;
        public ObservableCollection<double> GardenDataCollection { get => gardenDataCollection; set => SetProperty(ref gardenDataCollection, value); }
        //
        public DelegateCommand<object> RefreshCommand { get; }
        public DelegateCommand<object> StartCommand { get; }
        public DelegateCommand<object> StopCommand { get; }
        //
        private Chart _stepsChart;
        public Chart StepsChart { get { return _stepsChart; } set => SetProperty(ref _stepsChart, value); }
        //
        INavigationService _navigationService;
        IChartService _chartService;
        //IHealthServices _healthService;
        public DemoPageViewModel(INavigationService navigationService, IChartService chartService) : base(navigationService)
        {
            _navigationService = navigationService;
            _chartService = chartService;
            //
            FetchHealthData();
            //
            RefreshCommand = new DelegateCommand<object>(RefreshSelects);
            StartCommand = new DelegateCommand<object>(StartSelects);
            StopCommand = new DelegateCommand<object>(StopSelects);
        }

        private void RefreshSelects(object click)
        {
            ClickButton = "Refresh";
            FetchHealthData();
        }

        private void StartSelects(object click)
        {
            ClickButton = "Start";
            Xamarin.Forms.DependencyService.Get<IHealthServices>().StartSubscription((isStart) =>
            {
                ClickButton += isStart + "";
                Result = "Subscription: " + isStart;
            });
        }

        private void StopSelects(object click)
        {
            ClickButton = "Cancel";
            Xamarin.Forms.DependencyService.Get<IHealthServices>().CancelSubscription((isStop) =>
            {
                Result = "Cancel Subscription: " + isStop;
            });
        }

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
                    Result = result + "";
                }
            });
        }

        public bool GetData()
        {
            //lay so buoc theo mot khoang thoi gian
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchListSteps(async (totalSteps) =>
            {
                StepsChart = await _chartService.GetLineChartAsyns<double>(totalSteps);
            }, new DateTime(2019, 3, 4), new DateTime(2019, 3, 10, 23, 59, 59));


            // wait for them all to finish
            return true;
        }
    }
}
