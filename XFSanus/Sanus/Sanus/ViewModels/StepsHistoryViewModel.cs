using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microcharts;
using Prism.Commands;
using Prism.Navigation;
using Sanus.Model;
using Sanus.Services;
using Sanus.Services.Charts;
using Sanus.Services.Dialog;
using Sanus.Services.Health;
using Sanus.Services.Time;

namespace Sanus.ViewModels
{
    public class StepsHistoryViewModel : ViewModelBase
    {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
#pragma warning disable IDE0044 // Add readonly modifier
        INavigationService _navigationService;
        IDialogService _dialogService;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        IChartService _chartService;
        IGetTime _getTime;
        private RestServices RestServices { get; }
        //
        private Chart _stepsInDayChart;
        private Chart _stepsInWeekChart;
        private Chart _stepsInMonthChart;
        private ObservableCollection<ValueData> _stepsInDayCollection;
        private ObservableCollection<ValueData> _stepsInWeekCollection;
        private ObservableCollection<ValueData> _stepsInMonthCollection;
        private DateTime _date;
        private DateTime _startDay;
        private DateTime _endDay;

        //
        public Chart StepsInDayChart { get => _stepsInDayChart; set => SetProperty(ref _stepsInDayChart, value); }
        public Chart StepsInWeekChart { get => _stepsInWeekChart; set => SetProperty(ref _stepsInWeekChart, value); }
        public Chart StepsInMonthChart { get => _stepsInMonthChart; set => SetProperty(ref _stepsInMonthChart, value); }
        public ObservableCollection<ValueData> StepsInDayCollection { get => _stepsInDayCollection; set => SetProperty(ref _stepsInDayCollection, value); }
        public ObservableCollection<ValueData> StepsInWeekCollection { get => _stepsInWeekCollection; set => SetProperty(ref _stepsInWeekCollection, value); }
        public ObservableCollection<ValueData> StepsInMonthCollection { get => _stepsInMonthCollection; set => SetProperty(ref _stepsInMonthCollection, value); }
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
        public StepsHistoryViewModel(INavigationService navigationService, IChartService chartService, IDialogService dialogService, IGetTime getTime, RestServices restServices) : base(navigationService)
        {
            _navigationService = navigationService;
            _chartService = chartService;
            _dialogService = dialogService;
            _getTime = getTime;
            RestServices = restServices;
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
        //
        public override void OnNavigatedTo(INavigationParameters parameters) { }
        //
        public void FetchHealthData()
        {
            GetDataInDayAsync(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Configuration.HOURS);
            GetDataInWeekAsync(_getTime.PosteriorWeek(DateTime.Now)["startDay"], _getTime.PosteriorWeek(DateTime.Now)["endDay"], Configuration.DAYS);
            GetDataInMonthAsync(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Configuration.MONTHS);
        }
        //
        public bool GetDataInDayAsync(int year, int month, int day, string timeunit)
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.STEPS, async (datas) =>
            {
                StepsInDayChart = await _chartService.GetChartAsyns(datas, timeunit, Configuration.LINECHART);
                StepsInDayCollection = GetCollection(datas);
                foreach (ValueData valueData in Gets(datas))
                {
                    //await _dialogService.ShowConfirmAsync(valueData.Value.ToString(), valueData.Value.ToString(), "OK", "Cancel");
                    var response = await RestServices.PostResponse(Configuration.APISTEPS,
                        new
                        {
                            Time = valueData.Time,
                            Values = (int)valueData.Value
                        });
                    //await _dialogService.ShowConfirmAsync(response.reponse.ToString(), response.reponse.ToString(), "OK", "Cancel");
                }
            }, new DateTime(year, month, day, 0, 0, 0), new DateTime(year, month, day, 23, 59, 59), timeunit);
            return true;
        }
        //
        public bool GetDataInWeekAsync(DateTime startDay, DateTime endDay, string timeunit)
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.STEPS, async (datas) =>
            {
                StepsInWeekChart = await _chartService.GetChartAsyns(datas, timeunit, Configuration.POINTCHART);
                StepsInWeekCollection = GetCollection(datas);
            }, new DateTime(startDay.Year, startDay.Month, startDay.Day, 0, 0, 0), new DateTime(endDay.Year, endDay.Month, endDay.Day, 23, 59, 59), timeunit);
            return true;
        }
        //
        public bool GetDataInMonthAsync(int year, int month, int day, string timeunit)
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.STEPS, async (datas) =>
            {
                StepsInMonthChart = await _chartService.GetChartAsyns(datas, timeunit, Configuration.POINTCHART);
                StepsInMonthCollection = GetCollection(datas);
            }, new DateTime(year, month, 1, 0, 0, 0), new DateTime(year, month, day, 23, 59, 59), Configuration.DAYS);
            return true;
        }
        //
        private ObservableCollection<ValueData> GetCollection(Dictionary<DateTime, double> list)
        {
            ObservableCollection<ValueData> collection = new ObservableCollection<ValueData>();
            foreach (KeyValuePair<DateTime, double> item in list)
            {
                collection.Add(new ValueData() { Time = item.Key, Value = item.Value });
            }
            //
            return collection;
        }

        private List<ValueData> Gets(Dictionary<DateTime, double> list)
        {
            List<ValueData> collection = new List<ValueData>();
            foreach (KeyValuePair<DateTime, double> item in list)
            {
                collection.Add(new ValueData() { Time = item.Key, Value = item.Value });
            }
            //
            return collection;
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
        private async void PosteriorWeekSelect()
        {
            DateTime startDay = _getTime.PosteriorWeek(EndDay.AddDays(2))["startDay"];
            DateTime endDay = _getTime.PosteriorWeek(EndDay.AddDays(2))["endDay"];
            StartDay = startDay;
            EndDay = endDay;
            GetDataInWeekAsync(_getTime.PosteriorWeek(endDay.AddDays(2))["startDay"], _getTime.PosteriorWeek(endDay.AddDays(2))["endDay"], Configuration.DAYS);
            await Task.Delay(100);
        }
        private async void PreviousWeekSelect()
        {
            DateTime startDay = _getTime.PosteriorWeek(StartDay)["startDay"];
            DateTime endDay = _getTime.PosteriorWeek(StartDay)["endDay"];
            StartDay = startDay;
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
