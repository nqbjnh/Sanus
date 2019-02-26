using System;

namespace StepCounter
{
	public interface IHealthData
	{		
        void FetchSteps(Action<double> completionHandler);
        void FetchMetersWalked(Action<double> completionHandler);
		void FetchActiveMinutes(Action<double> completionHandler);
		void GetHealthPermissionAsync(Action<bool> completion);
        void FetchActiveEnergyBurned(Action<double> completionHandler);
        void CancelSubscription(Action<bool> completionHandler);
        void StartSubscription(Action<bool> completionHandler);
	}
}
