using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microcharts;
using Prism.Commands;
using Prism.Navigation;
using Sanus.Model;
using Sanus.Services.Charts;
using Sanus.Services.Dialog;
using Sanus.Services.Health;
using Sanus.Services.Time;

namespace Sanus.ViewModels
{
    public class EnegyHistoryViewModel : ViewModelBase
    {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
#pragma warning disable IDE0044 // Add readonly modifier
        INavigationService _navigationService;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        IChartService _chartService;
        IDialogService _dialogService;
        IGetTime _getTime;
        //
        private Chart _enegyInDayChart;
        private Chart _enegyInWeekChart;
        private Chart _enegyInMonthChart;
        private ObservableCollection<ValueData> _enegyInDayCollection;
        private ObservableCollection<ValueData> _enegyInWeekCollection;
        private ObservableCollection<ValueData> _enegyInMonthCollection;
        private DateTime _date;
        private DateTime _startDay;
        private DateTime _endDay;
        //
        public Chart EnegyInDayChart { get => _enegyInDayChart; set => SetProperty(ref _enegyInDayChart, value); }
        public Chart EnegyInWeekChart { get => _enegyInWeekChart; set => SetProperty(ref _enegyInWeekChart, value); }
        public Chart EnegyInMonthChart { get => _enegyInMonthChart; set => SetProperty(ref _enegyInMonthChart, value); }
        public ObservableCollection<ValueData> EnegyInDayCollection { get => _enegyInDayCollection; set => SetProperty(ref _enegyInDayCollection, value); }
        public ObservableCollection<ValueData> EnegyInWeekCollection { get => _enegyInWeekCollection; set => SetProperty(ref _enegyInWeekCollection, value); }
        public ObservableCollection<ValueData> EnegyInMonthCollection { get => _enegyInMonthCollection; set => SetProperty(ref _enegyInMonthCollection, value); }
        //
        public DelegateCommand PreviousDayCommand { get; }
        public DelegateCommand PreviousWeekCommand { get; }
        public DelegateCommand PreviousMonthCommand { get; }
        public DelegateCommand PosteriorDayCommand { get; }
        public DelegateCommand PosteriorWeekCommand { get; }
        public DelegateCommand PosteriorMonthCommand { get; }
        //
        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }
        public DateTime StartDay { get => _startDay; set => SetProperty(ref _startDay, value); }
        public DateTime EndDay { get => _endDay; set => SetProperty(ref _endDay, value); }
        //
        public EnegyHistoryViewModel(INavigationService navigationService, IChartService chartService, IDialogService dialogService, IGetTime getTime) : base(navigationService)
        {
            _navigationService = navigationService;
            _chartService = chartService;
            _dialogService = dialogService;
            _getTime = getTime;
            //
            Date = DateTime.Now;
            StartDay = _getTime.PosteriorWeek(DateTime.Now)["startDay"];
            EndDay = _getTime.PosteriorWeek(DateTime.Now)["endDay"];
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
            GetDataInWeekAsync(_getTime.PosteriorWeek(DateTime.Now)["startDay"], _getTime.PosteriorWeek(DateTime.Now)["endDay"], Configuration.DAYS);
            GetDataInMonthAsync(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Configuration.MONTHS);
        }
        public bool GetDataInDayAsync(int year, int month, int day, string timeUnit)
        {
            //lay tong so buoc theo mot khoang thoi gian
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.CALORIES, async (datas) =>
            {
                // ve bieu do
                EnegyInDayChart = await _chartService.GetChartAsyns(datas, timeUnit, Configuration.LINECHART);
                // lay danh sach cac buoc theo thoi gian
                EnegyInDayCollection = GetCollection(datas);
            }, new DateTime(year, month, day, 0, 0, 0), new DateTime(year, month, day, 23, 59, 59), timeUnit);
            return true;
        }
        //
        public bool GetDataInWeekAsync(DateTime startDay, DateTime endDay, string timeunit)
        {
            //lay tong so buoc theo mot khoang thoi gian
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.CALORIES, async (datas) =>
            {
                // ve bieu do
                EnegyInWeekChart = await _chartService.GetChartAsyns(datas, timeunit, Configuration.POINTCHART);
                // lay danh sach cac buoc theo thoi gian
                EnegyInWeekCollection = GetCollection(datas);
            }, new DateTime(startDay.Year, startDay.Month, startDay.Day, 0, 0, 0), new DateTime(endDay.Year, endDay.Month, endDay.Day, 23, 59, 59), timeunit);
            return true;
        }
        //
        public bool GetDataInMonthAsync(int year, int month, int day, string timeUnit)
        {
            //lay tong so buoc theo mot khoang thoi gian
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.CALORIES, async (datas) =>
            {
                // ve bieu do
                EnegyInMonthChart = await _chartService.GetChartAsyns(datas, timeUnit, Configuration.POINTCHART);
                // lay danh sach cac buoc theo thoi gian
                EnegyInMonthCollection = GetCollection(datas);
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
        private async void PreviousDaySelect()
        {
            DateTime dateTime = _getTime.PreviousDay(Date.Year, Date.Month, Date.Day);
            Date = dateTime;
            GetDataInDayAsync(dateTime.Year, dateTime.Month, dateTime.Day, Configuration.HOURS);
            await Task.Delay(100);
        }
        private async void PosteriorDaySelect()
        {
            if (Date.CompareTo(DateTime.Today) == -1)
            {
                DateTime dateTime = _getTime.PosteriorDay(Date.Year, Date.Month, Date.Day);
                Date = dateTime;
                GetDataInDayAsync(dateTime.Year, dateTime.Month, dateTime.Day, Configuration.HOURS);
                await Task.Delay(100);
            }
            else if (Date.CompareTo(DateTime.Today) >= 0)
            {
                DateTime dateTime = DateTime.Now;
                Date = dateTime;
                GetDataInDayAsync(dateTime.Year, dateTime.Month, dateTime.Day, Configuration.HOURS);
                await Task.Delay(100);
            }
        }
        private async void PosteriorWeekSelect()
        {
            DateTime startDay = _getTime.PosteriorWeek(Date)["startDay"];
            DateTime endDay = _getTime.PosteriorWeek(Date)["endDay"];
            Date = startDay;
            EndDay = endDay;
            GetDataInWeekAsync(_getTime.PosteriorWeek(endDay.AddDays(2))["startDay"], _getTime.PosteriorWeek(endDay.AddDays(2))["endDay"], Configuration.DAYS);
            await Task.Delay(100);
        }
        private async void PreviousWeekSelect()
        {
            DateTime startDay = _getTime.PosteriorWeek(Date)["startDay"];
            DateTime endDay = _getTime.PosteriorWeek(Date)["endDay"];
            Date = startDay;
            EndDay = endDay;
            GetDataInWeekAsync(_getTime.PosteriorWeek(startDay.AddDays(-2))["startDay"], _getTime.PosteriorWeek(startDay.AddDays(-2))["endDay"], Configuration.DAYS);
            await Task.Delay(100);
        }
        private async void PreviousMonthSelect()
        {
            Date = _getTime.PreviousMonth(Date.Year, Date.Month);
            GetDataInMonthAsync(Date.Year, Date.Month, _getTime.GetLastDayInMonth(Date.Year, Date.Month), Configuration.MONTHS);
            await Task.Delay(100);
        }
        private async void PosteriorMonthSelect()
        {
            Date = _getTime.PosteriorMonth(Date.Year, Date.Month);
            GetDataInMonthAsync(Date.Year, Date.Month, _getTime.GetLastDayInMonth(Date.Year, Date.Month), Configuration.MONTHS);
            await Task.Delay(100);
        }
    }
}