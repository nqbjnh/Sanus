using System;
using System.Collections.Generic;
using System.Text;

namespace Sanus.Services.Health
{
    public interface IHealthServices
    {
        void PrintData(Action<List<double>> completionHandler);
        void FetchSteps(Action<double> completionHandler);
        // lay so buoc theo thoi gian bat ky
        void FetchSteps(Action<double> completionHandler, DateTime startDate, DateTime endDate);
        void FetchMetersWalked(Action<double> completionHandler);
        // lay quang duong theo thoi gian bat ky
        void FetchMetersWalked(Action<double> completionHandler, DateTime startDate, DateTime endDate);
        void FetchActiveMinutes(Action<double> completionHandler);
        void GetHealthPermissionAsync(Action<bool> completion);
        void FetchActiveEnergyBurned(Action<double> completionHandler);
        // lay nang luong (cal) theo thoi gian bat ky
        void FetchActiveEnergyBurned(Action<double> completionHandler, DateTime startDate, DateTime endDate);
        void CancelSubscription(Action<bool> completionHandler);
        void StartSubscription(Action<bool> completionHandler);
    }
}
