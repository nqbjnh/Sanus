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
        // lay tong so buoc theo thoi gian bat ky
        //void FetchSteps(Action<double> completionHandler, DateTime startDate, DateTime endDate);
        //// lay tong quang duong theo thoi gian bat ky
        //void FetchMetersWalked(Action<double> completionHandler, DateTime startDate, DateTime endDate);
        //// lay tong nang luong (cal) theo thoi gian bat ky
        //void FetchActiveEnergyBurned(Action<double> completionHandler, DateTime startDate, DateTime endDate);
        // lay danh sach buoc đi với thời gian của nó trong một khoảng thời gian bất kỳ
        //void FetchListSteps(Action<Dictionary<DateTime, double>> completionHandler, DateTime startDate, DateTime endDate, string timeUnit);
        //// lay danh sach quang duong với thời gian của nó trong một khoảng thời gian bất kỳ
        //void FetchMetersWalked(Action<Dictionary<DateTime, double>> completionHandler, DateTime startDate, DateTime endDate, string timeUnit);
        //// lấy danh sách năng lượng với thời gian của nó trong một khoảng thời gian bất kỳ
        //void FetchActiveEnergyBurned(Action<Dictionary<DateTime, double>> completionHandler, DateTime startDate, DateTime endDate, string timeUnit);
        // lấy danh sách giá trị nào đó (năng lương, quãng đường, bước) với thời gian của nó trong một khoảng thời gian bất kỳ
        void FetchData(string valueData, Action<Dictionary<DateTime, double>> completionHandler, DateTime startDate, DateTime endDate, string timeUnit);
    }
}
