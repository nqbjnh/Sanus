using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microcharts;
using Prism.Commands;
using Prism.Navigation;
using Sanus.Model;
using Sanus.Services.Charts;
using Sanus.Services.Dialog;
using Sanus.Services.Health;

namespace Sanus.ViewModels
{
    public class DistanceHistoryViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IChartService _chartService;
        IDialogService _dialogService;
        //
        private Chart _distanceInDayChart;
        private Chart _distanceInWeekChart;
        private Chart _distanceInMonthChart;
        private ObservableCollection<ValueData> _distanceInDayCollection;
        private ObservableCollection<ValueData> _distanceInWeekCollection;
        private ObservableCollection<ValueData> _distanceInMonthCollection;
        //
        public Chart DistanceInDayChart { get { return _distanceInDayChart; } set => SetProperty(ref _distanceInDayChart, value); }
        public Chart DistanceInWeekChart { get { return _distanceInWeekChart; } set => SetProperty(ref _distanceInWeekChart, value); }
        public Chart DistanceInMonthChart { get { return _distanceInMonthChart; } set => SetProperty(ref _distanceInMonthChart, value); }
        public ObservableCollection<ValueData> DistanceInDayCollection { get { return _distanceInDayCollection; } set => SetProperty(ref _distanceInDayCollection, value); }
        public ObservableCollection<ValueData> DistanceInWeekCollection { get { return _distanceInWeekCollection; } set => SetProperty(ref _distanceInWeekCollection, value); }
        public ObservableCollection<ValueData> DistanceInMonthCollection { get { return _distanceInMonthCollection; } set => SetProperty(ref _distanceInMonthCollection, value); }
        //
        public DelegateCommand PreviousDayCommand { get; }
        public DelegateCommand PreviousWeekCommand { get; }
        public DelegateCommand PreviousMonthCommand { get; }
        public DelegateCommand PosteriorDayCommand { get; }
        public DelegateCommand PosteriorWeekCommand { get; }
        public DelegateCommand PosteriorMonthCommand { get; }
        //
        public DistanceHistoryViewModel(INavigationService navigationService, IChartService chartService, IDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _chartService = chartService;
            _dialogService = dialogService;
            //
            FetchHealthData();
            //
            PreviousDayCommand = new DelegateCommand(PreviousDaySelect);
            PreviousWeekCommand = new DelegateCommand(PreviousWeekSelect);
            PreviousMonthCommand = new DelegateCommand(PreviousMonthSelect);
            PosteriorDayCommand = new DelegateCommand(PosteriorDaySelect);
            PosteriorWeekCommand = new DelegateCommand(PosteriorWeekSelect);
            PosteriorMonthCommand = new DelegateCommand(PosteriorMonthSelect);
        }
        public void FetchHealthData()
        {
            GetDataInDayAsync(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Configuration.HOURS);
            GetDataInWeekAsync(DateTime.Now.Year, 3, 20, 27, Configuration.DAYS);
            GetDataInMonthAsync(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Configuration.MONTHS);
        }
        public bool GetDataInDayAsync(int year, int month, int day, string timeunit)
        {
            //lay tong so buoc theo mot khoang thoi gian
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.DISTANCE, async (datas) =>
            {
                // ve bieu do
                DistanceInDayChart = await _chartService.GetChartAsyns(datas, timeunit, Configuration.LINECHART);
                // lay danh sach cac buoc theo thoi gian
                DistanceInDayCollection = GetCollection(datas);
            }, new DateTime(year, month, day, 0, 0, 0), new DateTime(year, month, day, 23, 59, 59), timeunit);
            return true;
        }
        //
        public bool GetDataInWeekAsync(int year, int month, int startDay, int endDay, string timeunit)
        {
            //lay tong so buoc theo mot khoang thoi gian
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.DISTANCE, async (datas) =>
            {
                // ve bieu do
                DistanceInWeekChart = await _chartService.GetChartAsyns(datas, timeunit, Configuration.POINTCHART);
                // lay danh sach cac buoc theo thoi gian
                DistanceInWeekCollection = GetCollection(datas);
            }, new DateTime(year, month, startDay, 0, 0, 0), new DateTime(year, month, endDay, 23, 59, 59), timeunit);
            return true;
        }
        //
        public bool GetDataInMonthAsync(int year, int month, int day, string timeunit)
        {
            //lay tong so buoc theo mot khoang thoi gian
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.DISTANCE, async (datas) =>
             {
                 // ve bieu do
                 DistanceInMonthChart = await _chartService.GetChartAsyns(datas, timeunit, Configuration.POINTCHART);
                 // lay danh sach cac buoc theo thoi gian
                 DistanceInMonthCollection = GetCollection(datas);
             }, new DateTime(year, month, 1, 0, 0, 0), new DateTime(year, month, day, 23, 59, 59), Configuration.DAYS);
            return true;
        }
        //
        private ObservableCollection<ValueData> GetCollection(Dictionary<DateTime, double> list)
        {
            ObservableCollection<ValueData> collection = new ObservableCollection<ValueData>();
            //
            foreach (KeyValuePair<DateTime, double> item in list)
            {
                collection.Add(new ValueData() { Day = item.Key, Value = item.Value });
            }
            //
            return collection;
        }
        //
        private void PreviousSelects()
        {
            DateTime dateTime = Configuration.PreviousWeek(DateTime.Now);
        }
        //
        private async void PreviousDaySelect()
        {
            await _dialogService.ShowAlertAsync("lùi một ngày", "lùi ngày", "Ok");
        }
        private async void PosteriorDaySelect()
        {
            await _dialogService.ShowAlertAsync("tiến một ngày", "tiến ngày", "Ok");
        }
        private async void PreviousWeekSelect()
        {
            await _dialogService.ShowAlertAsync("tiến một tuần", "tiến tuần", "Ok");
        }
        private async void PosteriorWeekSelect()
        {
            await _dialogService.ShowAlertAsync("lùi một tuần", "lùi tuần", "Ok");
        }
        private async void PreviousMonthSelect()
        {
            await _dialogService.ShowAlertAsync("lùi một tháng", "lùi tháng", "Ok");
        }
        private async void PosteriorMonthSelect()
        {
            await _dialogService.ShowAlertAsync("tiến một tháng", "tiến tháng", "Ok");
        }
    }
}
