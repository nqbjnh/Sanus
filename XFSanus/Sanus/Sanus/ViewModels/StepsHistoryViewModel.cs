using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using Prism.Navigation;
using Sanus.Model;
using Sanus.Services.Charts;
using Sanus.Services.Dialog;
using Sanus.Services.Health;

namespace Sanus.ViewModels
{
    public class StepsHistoryViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IChartService _chartService;
        IDialogService _dialogService;
        //
        private Chart _stepsInDayChart;
        private Chart _stepsInWeekChart;
        private Chart _stepsInMonthChart;
        private ObservableCollection<Steps> _stepsInDayCollection;
        private ObservableCollection<Steps> _stepsInWeekCollection;
        private ObservableCollection<Steps> _stepsInMonthCollection;
        //
        public Chart StepsInDayChart { get { return _stepsInDayChart; } set => SetProperty(ref _stepsInDayChart, value); }
        public Chart StepsInWeekChart { get { return _stepsInWeekChart; } set => SetProperty(ref _stepsInWeekChart, value); }
        public Chart StepsInMonthChart { get { return _stepsInMonthChart; } set => SetProperty(ref _stepsInMonthChart, value); }
        public ObservableCollection<Steps> StepsInDayCollection { get => _stepsInDayCollection; set => SetProperty(ref _stepsInDayCollection, value); }
        public ObservableCollection<Steps> StepsInWeekCollection { get => _stepsInWeekCollection; set => SetProperty(ref _stepsInWeekCollection, value); }
        public ObservableCollection<Steps> StepsInMonthCollection { get => _stepsInMonthCollection; set => SetProperty(ref _stepsInMonthCollection, value); }
        //
        public StepsHistoryViewModel(INavigationService navigationService, IChartService chartService, IDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _chartService = chartService;
            _dialogService = dialogService;
            //
            FetchHealthData();
        }
        //
        public void FetchHealthData()
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().GetHealthPermissionAsync(async (result) =>
            {
                if (result)
                {
                    GetDataAsync();
                }
                else
                {
                    await _dialogService.ShowConfirmAsync("Load data fail", "Fail", "Ok", "Cancel");
                }
            });
        }
        //
        public bool GetDataAsync()
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchListStepss(async (totalSteps) =>
            {
                //lay tong so buoc theo mot khoang thoi gian
                // ve bieu do
                StepsInWeekChart = await _chartService.GetBarChartAsyns(totalSteps);
                // lay danh sach cac buoc theo thoi gian
                StepsInWeekCollection = GetStepsCollection(totalSteps);
            }, new DateTime(2019, 3, 8, 0, 1, 1), new DateTime(2019, 3, 14, 23, 59, 59), "DAYS");

            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchListStepss(async (totalSteps) =>
            {
                //lay tong so buoc theo mot khoang thoi gian
                // ve bieu do
                StepsInDayChart = await _chartService.GetBarChartAsyns(totalSteps);
                // lay danh sach cac buoc theo thoi gian
                StepsInDayCollection = GetStepsCollection(totalSteps);
            }, new DateTime(2019, 3, 8, 0, 1, 1), new DateTime(2019, 3, 8, 23, 59, 59), "HOURS");

            return true;
        }
        //
        private ObservableCollection<Steps> GetStepsCollection(Dictionary<DateTime, double> list)
        {
            ObservableCollection<Steps> collection = new ObservableCollection<Steps>();
            //
            foreach (KeyValuePair<DateTime, double> item in list)
            {
                collection.Add(new Steps() { Day = item.Key, Step = item.Value });
            }
            //
            return collection;
        }
    }
}
