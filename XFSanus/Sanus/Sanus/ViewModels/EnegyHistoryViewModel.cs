using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microcharts;
using Prism.Navigation;
using Sanus.Model;
using Sanus.Services.Charts;
using Sanus.Services.Dialog;
using Sanus.Services.Health;

namespace Sanus.ViewModels
{
    public class EnegyHistoryViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IChartService _chartService;
        IDialogService _dialogService;
        //
        private Chart _distanceInDayChart;
        private Chart _distanceInWeekChart;
        private Chart _distanceInMonthChart;
        private ObservableCollection<Enegy> _distanceInDayCollection;
        private ObservableCollection<Enegy> _distanceInWeekCollection;
        private ObservableCollection<Enegy> _distanceInMonthCollection;
        //
        public Chart EnegyInDayChart { get { return _distanceInDayChart; } set => SetProperty(ref _distanceInDayChart, value); }
        public Chart EnegyInWeekChart { get { return _distanceInWeekChart; } set => SetProperty(ref _distanceInWeekChart, value); }
        public Chart EnegyInMonthChart { get { return _distanceInMonthChart; } set => SetProperty(ref _distanceInMonthChart, value); }
        public ObservableCollection<Enegy> EnegyInDayCollection { get { return _distanceInDayCollection; } set => SetProperty(ref _distanceInDayCollection, value); }
        public ObservableCollection<Enegy> EnegyInWeekCollection { get { return _distanceInWeekCollection; } set => SetProperty(ref _distanceInWeekCollection, value); }
        public ObservableCollection<Enegy> EnegyInMonthCollection { get { return _distanceInMonthCollection; } set => SetProperty(ref _distanceInMonthCollection, value); }
        //
        public EnegyHistoryViewModel(INavigationService navigationService, IChartService chartService, IDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _chartService = chartService;
            _dialogService = dialogService;
            //
            FetchHealthData();
        }
        public void FetchHealthData()
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().GetHealthPermissionAsync(async (result) =>
            {
                if (result)
                {
                    GetDataInDayAsync(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Configuration.HOURS);
                    GetDataInWeekAsync(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 7, DateTime.Now.Day, Configuration.DAYS);
                    GetDataInMonthAsync(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Configuration.MONTHS);
                }
                else
                {
                    await _dialogService.ShowConfirmAsync("Load data fail", "Fail", "Ok", "Cancel");
                }
            });
        }
        public bool GetDataInDayAsync(int year, int month, int day, string timeunit)
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.CALORIES, async (totalDatas) =>
            {
                //lay tong so buoc theo mot khoang thoi gian
                // ve bieu do
                EnegyInDayChart = await _chartService.GetBarChartAsyns(totalDatas, timeunit);
                // lay danh sach cac buoc theo thoi gian
                EnegyInDayCollection = GetDataCollection(totalDatas);
            }, new DateTime(year, month, day, 0, 0, 0), new DateTime(year, month, day, 23, 59, 59), timeunit);
            return true;
        }
        //
        public bool GetDataInWeekAsync(int year, int month, int startDay, int endDay, string timeunit)
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.CALORIES, async (totalDatas) =>
            {
                //lay tong so buoc theo mot khoang thoi gian
                // ve bieu do
                EnegyInWeekChart = await _chartService.GetBarChartAsyns(totalDatas, timeunit);
                // lay danh sach cac buoc theo thoi gian
                EnegyInWeekCollection = GetDataCollection(totalDatas);
            }, new DateTime(year, month, startDay, 0, 0, 0), new DateTime(year, month, endDay, 23, 59, 59), timeunit);
            return true;
        }
        //
        public bool GetDataInMonthAsync(int year, int month, int day, string timeunit)
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.CALORIES, async (totalDatas) =>
            {
                //lay tong so buoc theo mot khoang thoi gian
                // ve bieu do
                EnegyInMonthChart = await _chartService.GetBarChartAsyns(totalDatas, timeunit);
                // lay danh sach cac buoc theo thoi gian
                EnegyInMonthCollection = GetDataCollection(totalDatas);
            }, new DateTime(year, month, 1, 0, 0, 0), new DateTime(year, month, day, 23, 59, 59), Configuration.DAYS);
            return true;
        }
        //
        private ObservableCollection<Enegy> GetDataCollection(Dictionary<DateTime, double> list)
        {
            ObservableCollection<Enegy> collection = new ObservableCollection<Enegy>();
            //
            foreach (KeyValuePair<DateTime, double> item in list)
            {
                collection.Add(new Enegy() { Day = item.Key, Step = item.Value });
            }
            //
            return collection;
        }
    }
}