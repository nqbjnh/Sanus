﻿using System;
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
    public class StepsHistoryViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        IChartService _chartService;
        IDialogService _dialogService;
        //
        private Chart _stepsInDayChart;
        private Chart _stepsInWeekChart;
        private Chart _stepsInMonthChart;
        private ObservableCollection<ValueData> _stepsInDayCollection;
        private ObservableCollection<ValueData> _stepsInWeekCollection;
        private ObservableCollection<ValueData> _stepsInMonthCollection;
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
        public StepsHistoryViewModel(INavigationService navigationService, IChartService chartService, IDialogService dialogService) : base(navigationService)
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
        //
        public override void OnNavigatedTo(INavigationParameters parameters) { }
        //
        public void FetchHealthData()
        {
            GetDataInDayAsync(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Configuration.HOURS);
            GetDataInWeekAsync(DateTime.Now.Year, 3, 20, 27, Configuration.DAYS);
            GetDataInMonthAsync(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Configuration.MONTHS);
        }
        //
        public bool GetDataInDayAsync(int year, int month, int day, string timeunit)
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.STEPS, async (datas) =>
            {
                StepsInDayChart = await _chartService.GetChartAsyns(datas, timeunit, Configuration.LINECHART);
                StepsInDayCollection = GetCollection(datas);
            }, new DateTime(year, month, day, 0, 0, 0), new DateTime(year, month, day, 23, 59, 59), timeunit);
            return true;
        }
        //
        public bool GetDataInWeekAsync(int year, int month, int startDay, int endDay, string timeunit)
        {
            Xamarin.Forms.DependencyService.Get<IHealthServices>().FetchData(Configuration.STEPS, async (datas) =>
            {
                StepsInWeekChart = await _chartService.GetChartAsyns(datas, timeunit, Configuration.POINTCHART);
                StepsInWeekCollection = GetCollection(datas);
            }, new DateTime(year, month, startDay, 0, 0, 0), new DateTime(year, month, endDay, 23, 59, 59), timeunit);
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
