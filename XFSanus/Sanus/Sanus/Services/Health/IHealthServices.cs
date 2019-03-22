using System;
using System.Collections.Generic;
using System.Text;

namespace Sanus.Services.Health
{
    public interface IHealthServices
    {
        void CancelSubscription(Action<bool> completionHandler);
        void StartSubscription(Action<bool> completionHandler);
        void PrintData(Action<List<double>> completionHandler);
        void GetHealthPermissionAsync(Action<bool> completion);
        void FetchSteps(Action<double> completionHandler);
        void FetchMetersWalked(Action<double> completionHandler);
        void FetchActiveMinutes(Action<double> completionHandler);
        void FetchActiveEnergyBurned(Action<double> completionHandler);
        // lay tong quang duong theo thoi gian bat ky
        void FetchMetersWalked(Action<double> completionHandler, DateTime startDate, DateTime endDate);
        // lay tong so buoc theo thoi gian bat ky
        void FetchSteps(Action<double> completionHandler, DateTime startDate, DateTime endDate);
        // lay tong nang luong (cal) theo thoi gian bat ky
        void FetchActiveEnergyBurned(Action<double> completionHandler, DateTime startDate, DateTime endDate);
        // lay danh sach so buoc theo thoi gian bat ky
        void FetchListSteps(Action<List<double>> completionHandler, DateTime startDate, DateTime endDate);
        // lay danh sach buoc đi với thời gian của nó trong một khoảng thời gian bất kỳ
        void FetchListStepss(Action<Dictionary<DateTime, double>> completionHandler, DateTime startDate, DateTime endDate, string timeUnit);
    }
}
