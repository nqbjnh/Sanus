using System;
using System.Collections.Generic;

namespace Sanus.Services.Health
{
    public interface IHealthServices
    {
        void CancelSubscription(Action<bool> completionHandler);
        void StartSubscription(Action<bool> completionHandler);
        void GetHealthPermissionAsync(Action<bool> completion);
        void FetchActiveMinutes(Action<double> completionHandler);
        // lấy danh sách giá trị nào đó (năng lương, quãng đường, bước) với thời gian của nó trong một khoảng thời gian bất kỳ
        void FetchData(string valueData, Action<Dictionary<DateTime, double>> completionHandler, DateTime startDate, DateTime endDate, string timeUnit);
    }
}
