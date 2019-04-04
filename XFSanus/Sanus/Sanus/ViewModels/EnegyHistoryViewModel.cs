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
        private DateTime _month;

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
        public DateTime Month { get => _month; set => SetProperty(ref _month, value); }
        //
        public EnegyHistoryViewModel(INavigationService navigationService, IChartService chartService, IDialogService dialogService, IGetTime getTime) : base(navigationService)
        {
            _navigationService = navigationService;
            _chartService = chartService;
            _dialogService = dialogService;
            _getTime = getTime;
            //
            Date = DateTime.Now;
            Month = DateTime.Now;
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
        public bool GetDataInWeekAsync(int year, int month, int startDay, int endDay, string timeUnit)
        {
            if (endDay <= DateTime.Now.Day && startDay + 7 == endDay)
            {
                //lay tong so buoc theo mot khoang thoi gian
                Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.CALORIES, async (datas) =>
                {
                    // ve bieu do
                    EnegyInWeekChart = await _chartService.GetChartAsyns(datas, timeUnit, Configuration.POINTCHART);
                    // lay danh sach cac buoc theo thoi gian
                    EnegyInWeekCollection = GetCollection(datas);
                }, new DateTime(year, month, startDay, 0, 0, 0), new DateTime(year, month, endDay, 23, 59, 59), timeUnit);
            }
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
        private void PreviousSelects()
        {
            DateTime dateTime = _getTime.PreviousWeek(DateTime.Now);
        }
        //
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
            Month = _getTime.PreviousMonth(Month.Year, Month.Month);
            GetDataInMonthAsync(Month.Year, Month.Month, _getTime.GetLastDayInMonth(Month.Year, Month.Month), Configuration.MONTHS);
            await Task.Delay(100);
        }
        private async void PosteriorMonthSelect()
        {
            Month = _getTime.PosteriorMonth(Month.Year, Month.Month);
            GetDataInMonthAsync(Month.Year, Month.Month, _getTime.GetLastDayInMonth(Month.Year, Month.Month), Configuration.MONTHS);
            await Task.Delay(100);
        }
    }
}