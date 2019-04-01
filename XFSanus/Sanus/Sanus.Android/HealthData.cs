using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Fitness;
using Android.Gms.Fitness.Data;
using Android.Gms.Fitness.Request;
using Android.Gms.Fitness.Result;
using Android.OS;
using Android.Util;
using Java.Util.Concurrent;
using Sanus.Droid;
using Sanus.Services.Health;
using Xamarin.Forms;
using DataType = Android.Gms.Fitness.Data.DataType;

[assembly: Xamarin.Forms.Dependency(typeof(HealthData))]
namespace Sanus.Droid
{
    //Important:
    //step1: create app from https://console.developers.google.com
    //step2: enable Google Signin from https://developers.google.com/mobile/add?platform=android
    //Create SHA1 key: https://docs.microsoft.com/en-us/xamarin/android/deploy-test/signing/keystore-signature?tabs=vswin

    public class HealthData : Java.Lang.Object, IHealthServices, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        private GoogleApiClient mGoogleApiClient;
        private bool authInProgress;
        private Action<bool> AuthorizationCallBack;

        public async void StartSubscription(Action<bool> completionHandler)
        {
            var result = Subscribe();
            completionHandler(await result);
        }

        public async void CancelSubscription(Action<bool> completionHandler)
        {
            var result = CancelSubscription();
            completionHandler(await result);
        }

        public async void FetchActiveEnergyBurned(Action<double> completionHandler)
        {
            var result = FetchGoogleFitCalories();
            completionHandler(await result);
        }

        public async void FetchActiveMinutes(Action<double> completionHandler)
        {
            var result = FetchGoogleFitActiveMinutes();
            completionHandler(await result);
        }

        public async void FetchMetersWalked(Action<double> completionHandler)
        {
            var result = FetchGoogleFitDistance();
            completionHandler(await result);
        }

        public async void FetchSteps(Action<double> completionHandler)
        {
            var result = FetchGoogleFitSteps();
            completionHandler(await result);
        }

        public async void FetchData(string valueData, Action<Dictionary<DateTime, double>> completionHandler, DateTime startDate, DateTime endDate, string timeUnit)
        {
            if (timeUnit.Equals(Configuration.DAYS))
            {
                var result = FetchGoogleFitData(valueData, startDate, endDate, TimeUnit.Days);
                completionHandler(await result);
            }
            else if (timeUnit.Equals(Configuration.HOURS))
            {
                var result = FetchGoogleFitData(valueData, startDate, endDate, TimeUnit.Hours);
                completionHandler(await result);
            }
        }

        public void GetHealthPermissionAsync(Action<bool> completion)
        {
            AuthorizationCallBack = completion;
#pragma warning disable CS0618 // Type or member is obsolete
            mGoogleApiClient = new GoogleApiClient.Builder(Forms.Context)
#pragma warning restore CS0618 // Type or member is obsolete
                .AddConnectionCallbacks(this)
                .AddApi(FitnessClass.HISTORY_API)
                .AddApi(FitnessClass.RECORDING_API)
                .AddScope(new Scope(Scopes.FitnessActivityReadWrite))
                .AddScope(new Scope(Scopes.FitnessLocationReadWrite))
                .AddOnConnectionFailedListener(result =>
                {
                    Log.Info("apiClient", "Connection failed. Cause: " + result);
                    if (!result.HasResolution)
                    {
                        // Show the localized error dialog
#pragma warning disable CS0618 // Type or member is obsolete
                        GooglePlayServicesUtil.GetErrorDialog(result.ErrorCode, (Activity)Forms.Context, 0).Show();
#pragma warning restore CS0618 // Type or member is obsolete
                        return;
                    }
                    // The failure has a resolution. Resolve it.
                    // Called typically when the app is not yet authorized, and an
                    // authorization dialog is displayed to the user.
                    if (!authInProgress)
                    {
                        try
                        {
                            Log.Info("apiClient", "Attempting to resolve failed connection");
                            authInProgress = true;
#pragma warning disable CS0618 // Type or member is obsolete
                            result.StartResolutionForResult((Activity)Forms.Context, Configuration.REQUESTOAUTH);
#pragma warning restore CS0618 // Type or member is obsolete
                        }
                        catch (Exception e)
                        {
                            Log.Error("apiClient", "Exception while starting resolution activity", e);
                        }
                    }
                }).Build();
            //mGoogleApiClient.Connect();
#pragma warning disable CS0618 // Type or member is obsolete
            ((MainActivity)Forms.Context).ActivityResult += HandleActivityResult;
#pragma warning restore CS0618 // Type or member is obsolete
            mGoogleApiClient.Connect();
        }

        public void HandleActivityResult(object sender, ActivityResultEventArgs e)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            ((MainActivity)Forms.Context).ActivityResult -= HandleActivityResult;
#pragma warning restore CS0618 // Type or member is obsolete
            if (e.RequestCode == Configuration.REQUESTOAUTH)
            {
                authInProgress = false;
                if (e.ResultCode == Result.Ok)
                {
                    // Make sure the app is not already connected or attempting to connect
                    if (!mGoogleApiClient.IsConnecting && !mGoogleApiClient.IsConnected)
                    {
                        mGoogleApiClient.Connect();
                    }
                }
                else
                {
                    AuthorizationCallBack(false);
                }
            }
        }

        public async void OnConnected(Bundle bundle)
        {
            // This method is called when we connect to the LocationClient. We can start location updated directly form
            // here if desired, or we can do it in a lifecycle method, as shown above 
            await Task.Delay(1);
            // You must implement this to implement the IGooglePlayServicesClientConnectionCallbacks Interface
            Log.Info("LocationClient", "Now connected to client");
            AuthorizationCallBack(true);
        }

        public void OnDisconnected()
        {
            // This method is called when we disconnect from the LocationClient.

            // You must implement this to implement the IGooglePlayServicesClientConnectionCallbacks Interface
            Log.Info("LocationClient", "Now disconnected from client");
        }

        public void OnConnectionFailed(ConnectionResult bundle)
        {
            // This method is used to handle connection issues with the Google Play Services Client (LocationClient). 
            // You can check if the connection has a resolution (bundle.HasResolution) and attempt to resolve it

            // You must implement this to implement the IGooglePlayServicesClientOnConnectionFailedListener Interface
            Log.Info("LocationClient", "Connection failed, attempting to reach google play services");
        }

        public void OnConnectionSuspended(int i) { }

        public async Task<bool> Subscribe()
        {
            var status = await FitnessClass.RecordingApi.SubscribeAsync(mGoogleApiClient, DataType.TypeStepCountCumulative);
            if (status.IsSuccess)
            {
                if (status.StatusCode == FitnessStatusCodes.SuccessAlreadySubscribed)
                {
                    Log.Info("googleFitFetch", "Existing subscription for activity detected.");
                    return true;
                }
                else
                {
                    Log.Info("googleFitFetch", "Successfully subscribed!");
                    return true;
                }
            }
            else
            {
                Log.Info("googleFitFetch", "There was a problem subscribing.");
                return false;
            }
        }

        public async Task<bool> CancelSubscription()
        {
            string dataTypeStr = DataType.TypeStepCountCumulative.ToString();
            Log.Info("googleFitFetch", "Unsubscribing from data type: " + dataTypeStr);

            var status = await FitnessClass.RecordingApi.UnsubscribeAsync(mGoogleApiClient, DataType.TypeStepCountCumulative);
            if (status.IsSuccess)
            {
                Log.Info("googleFitFetch", "Successfully unsubscribed for data type: " + dataTypeStr);
                return true;
            }
            else
            {
                // Subscription not removed
                Log.Info("googleFitFetch", "Failed to unsubscribe for data type: " + dataTypeStr);
                return false;
            }
        }

        public async Task<double> FetchGoogleFitSteps()
        {
            DataReadRequest readRequest = QuerySteps();

            var dataReadResult = await FitnessClass.HistoryApi.ReadDataAsync(mGoogleApiClient, readRequest);
            var steps = 0.0;
            if (dataReadResult.Buckets.Count > 0)
            {
                foreach (Bucket bucket in dataReadResult.Buckets)
                {
                    foreach (DataSet dataSet in bucket.DataSets)
                    {
                        steps += GetDataSetValuesSum(dataSet);
                    }
                }
            }
            PrintData(dataReadResult);
            return steps;
        }

        public async Task<double> FetchGoogleFitDistance()
        {
            DataReadRequest readRequest = QueryDistance();

            var dataReadResult = await FitnessClass.HistoryApi.ReadDataAsync(mGoogleApiClient, readRequest);
            var distance = 0.0;
            if (dataReadResult.Buckets.Count > 0)
            {
                foreach (Bucket bucket in dataReadResult.Buckets)
                {
                    foreach (DataSet dataSet in bucket.DataSets)
                    {
                        distance += GetDataSetValuesSum(dataSet);
                    }
                }
            }
            PrintData(dataReadResult);
            return distance;
        }

        public async Task<double> FetchGoogleFitCalories()
        {
            DataReadRequest readRequest = QueryActiveEnergy();

            var dataReadResult = await FitnessClass.HistoryApi.ReadDataAsync(mGoogleApiClient, readRequest);
            var calories = 0.0;
            if (dataReadResult.Buckets.Count > 0)
            {
                foreach (Bucket bucket in dataReadResult.Buckets)
                {
                    foreach (DataSet dataSet in bucket.DataSets)
                    {
                        calories += (GetDataSetValuesSum(dataSet));
                    }
                }
            }
            PrintData(dataReadResult);
            return calories;
        }

        public async Task<Dictionary<DateTime, double>> FetchGoogleFitData(string value, DateTime startDate, DateTime endDate, TimeUnit timeUnit)
        {
            Dictionary<DateTime, double> listdata = new Dictionary<DateTime, double>();
            //
            DataReadRequest readRequest = QueryData(value, startDate, endDate, timeUnit);
            //
            var dataReadResult = await FitnessClass.HistoryApi.ReadDataAsync(mGoogleApiClient, readRequest);
            if (dataReadResult.Buckets.Count > 0)
            {
                foreach (Bucket bucket in dataReadResult.Buckets)
                {
                    foreach (DataSet dataSet in bucket.DataSets)
                    {
                        foreach (double item in GetDataSetValuess(dataSet))
                        {
                            listdata.Add(
                                new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                                    .AddMilliseconds(bucket.GetStartTime(TimeUnit.Milliseconds))
                                    .ToLocalTime()
                                , item);
                        }
                    }
                }
            }
            PrintData(dataReadResult);
            return listdata;
        }

        public async Task<double> FetchGoogleFitActiveMinutes()
        {
            DataReadRequest readRequest = QueryActiveEnergy();

            var dataReadResult = await FitnessClass.HistoryApi.ReadDataAsync(mGoogleApiClient, readRequest);
            var totalActiveTime = 0.0;
            if (dataReadResult.Buckets.Count > 0)
            {
                foreach (Bucket bucket in dataReadResult.Buckets)
                {
                    if (bucket.Activity.Contains(FitnessActivities.Walking) || bucket.Activity.Contains(FitnessActivities.Running))
                    {
                        long activeTime = bucket.GetEndTime(TimeUnit.Minutes) - bucket.GetStartTime(TimeUnit.Minutes);
                        totalActiveTime = totalActiveTime + activeTime;
                    }
                }
            }
            PrintData(dataReadResult);
            return totalActiveTime;
        }

        private static double GetDataSetValuesSum(DataSet dataSet)
        {
            var dataSetSum = 0.0;
            foreach (DataPoint point in dataSet.DataPoints)
            {
                foreach (Field field in point.DataType.Fields)
                {
                    try
                    {
                        string name = field.Name;
                        if (name.Equals(Configuration.STEPS))
                        {
                            dataSetSum += Convert.ToDouble(point.GetValue(field).AsInt());
                        }
                        else
                        {
                            dataSetSum += Convert.ToDouble(point.GetValue(field).AsFloat());
                        }
                    }
                    catch (Exception) { }
                }
            }
            return dataSetSum;
        }

        private static List<double> GetDataSetValuess(DataSet dataSet)
        {
            List<double> list = new List<double>();
            var dataSetSum = 0.0;
            foreach (DataPoint point in dataSet.DataPoints)
            {
                foreach (Field field in point.DataType.Fields)
                {
                    try
                    {
                        string name = field.Name;
                        if (name.Equals(Configuration.STEPS))
                        {
                            dataSetSum += Convert.ToDouble(point.GetValue(field).AsInt());
                            list.Add(dataSetSum);
                        }
                        else
                        {
                            dataSetSum += Convert.ToDouble(point.GetValue(field).AsFloat());
                            list.Add(dataSetSum);
                        }
                    }
                    catch (Exception) { }
                }
            }
            return list;
        }

        private static DataReadRequest QuerySteps()
        {
            DateTime endTime = DateTime.Now;
            DateTime startTime = DateTime.Today;
            long endTimeElapsed = GetMsSinceEpochAsLong(endTime);
            long startTimeElapsed = GetMsSinceEpochAsLong(startTime);
            //
            DataSource dataSource = new DataSource.Builder()
                .SetAppPackageName(Configuration.APPPACKAGENAME)
                .SetDataType(DataType.TypeStepCountDelta)
                .SetStreamName(Configuration.STREAMNAME)
                .SetType(DataSource.TypeDerived)
                .Build();

            var readRequest = new DataReadRequest.Builder()
                .Aggregate(dataSource, DataType.AggregateStepCountDelta)
                .SetTimeRange(startTimeElapsed, endTimeElapsed, TimeUnit.Milliseconds)
                .BucketByTime(1, TimeUnit.Days)
                .Build();

            return readRequest;
        }
        private static DataReadRequest QueryActiveEnergy()
        {
            DateTime endTime = DateTime.Now;
            DateTime startTime = DateTime.Today;
            long endTimeElapsed = GetMsSinceEpochAsLong(endTime);
            long startTimeElapsed = GetMsSinceEpochAsLong(startTime);
            //
            var readRequest = new DataReadRequest.Builder()
                .Aggregate(DataType.TypeCaloriesExpended, DataType.AggregateCaloriesExpended)
                .SetTimeRange(startTimeElapsed, endTimeElapsed, TimeUnit.Milliseconds)
                .BucketByTime(1, TimeUnit.Days)
                .Build();

            return readRequest;
        }

        private static DataReadRequest QueryDistance()
        {
            DateTime endTime = DateTime.Now;
            DateTime startTime = DateTime.Today;
            long endTimeElapsed = GetMsSinceEpochAsLong(endTime);
            long startTimeElapsed = GetMsSinceEpochAsLong(startTime);
            //
            var readRequest = new DataReadRequest.Builder()
                .Aggregate(DataType.TypeDistanceDelta, DataType.AggregateDistanceDelta)
                .SetTimeRange(startTimeElapsed, endTimeElapsed, TimeUnit.Milliseconds)
                .BucketByTime(1, TimeUnit.Days)
                .Build();

            return readRequest;
        }

        private static DataReadRequest QueryData(string value, DateTime startDate, DateTime endDate, TimeUnit timeUnit)
        {
            long endTimeElapsed = GetMsSinceEpochAsLong(endDate);
            long startTimeElapsed = GetMsSinceEpochAsLong(startDate);

            DataReadRequest readRequest = null;
            if (value.Equals(Configuration.DISTANCE))
            {
                readRequest = new DataReadRequest.Builder()
                    .Aggregate(DataType.TypeDistanceDelta, DataType.AggregateDistanceDelta)
                    .SetTimeRange(startTimeElapsed, endTimeElapsed, TimeUnit.Milliseconds)
                    .BucketByTime(1, timeUnit)
                    .Build();
            }
            else if (value.Equals(Configuration.CALORIES))
            {
                readRequest = new DataReadRequest.Builder()
                    .Aggregate(DataType.TypeCaloriesExpended, DataType.AggregateCaloriesExpended)
                    .SetTimeRange(startTimeElapsed, endTimeElapsed, TimeUnit.Milliseconds)
                    .BucketByTime(1, timeUnit)
                    .Build();
            }
            else if (value.Equals(Configuration.STEPS))
            {
                DataSource dataSource = new DataSource.Builder()
                    .SetAppPackageName(Configuration.APPPACKAGENAME)
                    .SetDataType(DataType.TypeStepCountDelta)
                    .SetStreamName(Configuration.STREAMNAME)
                    .SetType(DataSource.TypeDerived)
                    .Build();
                //
                readRequest = new DataReadRequest.Builder()
                     .Aggregate(dataSource, DataType.AggregateStepCountDelta)
                     .SetTimeRange(startTimeElapsed, endTimeElapsed, TimeUnit.Milliseconds)
                     .BucketByTime(1, timeUnit)
                     .Build();
            }
            return readRequest;
        }

        private static long GetMsSinceEpochAsLong(DateTime dateTime)
        {
            return (long)dateTime.ToUniversalTime()
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }

        private void PrintData(DataReadResult dataReadResult)
        {
            if (dataReadResult.Buckets.Count > 0)
            {
                Log.Info("TAG", "Number of returned buckets of DataSets is: " + dataReadResult.Buckets.Count);
                foreach (Bucket bucket in dataReadResult.Buckets)
                {
                    IList<DataSet> dataSets = bucket.DataSets;
                    Log.Info("TAG", "bucket is: " + bucket.Activity);
                    foreach (DataSet dataSet in dataSets)
                    {
                        DumpDataSet(dataSet);
                    }
                }
            }
            else if (dataReadResult.DataSets.Count > 0)
            {
                Log.Info("TAG", "Number of returned DataSets is: " + dataReadResult.DataSets.Count);
                foreach (DataSet dataSet in dataReadResult.DataSets)
                {
                    DumpDataSet(dataSet);
                }
            }
        }

        private void DumpDataSet(DataSet dataSet)
        {
            Log.Info("TAG", "Data returned for Data type: " + dataSet.DataType.Name);
            foreach (DataPoint dp in dataSet.DataPoints)
            {
                Log.Info("TAG", "Data point:");
                Log.Info("TAG", "\tType: " + dp.DataType.Name);
                Log.Info("TAG", "\tStart: " + new DateTime(1970, 1, 1).AddMilliseconds(
                    dp.GetStartTime(TimeUnit.Milliseconds)).ToString(Configuration.DATEFORMAT));
                Log.Info("TAG", "\tEnd: " + new DateTime(1970, 1, 1).AddMilliseconds(
                    dp.GetEndTime(TimeUnit.Milliseconds)).ToString(Configuration.DATEFORMAT));
                foreach (Field field in dp.DataType.Fields)
                {
                    Log.Info("TAG", "\tField: " + field.Name +
                    " Value: " + dp.GetValue(field));
                }
            }
        }
    }
}
