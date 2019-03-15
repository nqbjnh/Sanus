using System;
using System.Collections.Generic;
using System.Text;

namespace Sanus.Services.Health
{
    public interface IHealthServices
    {
        void PrintData(Action<List<double>> completionHandler);
        void FetchSteps(Action<double> completionHandler);
        void FetchMetersWalked(Action<double> completionHandler);
        void FetchActiveMinutes(Action<double> completionHandler);
        void GetHealthPermissionAsync(Action<bool> completion);
        void FetchActiveEnergyBurned(Action<double> completionHandler);
        void CancelSubscription(Action<bool> completionHandler);
        void StartSubscription(Action<bool> completionHandler);
    }
}
