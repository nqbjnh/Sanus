using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Sanus
{
    public class Configuration
    {
        public const string MONTHS = "MONTHS"; // thêm vào 
        public const string DAYS = "DAYS";
        public const string HOURS = "HOURS";
        public const string MICROSECONDS = "MICROSECONDS";
        public const string MILLISECONDS = "MILLISECONDS";
        public const string MINUTES = "MINUTES";
        public const string NANOSECONDS = "NANOSECONDS";
        public const string SECONDS = "SECONDS";
        //
        public const string CALORIES = "Calories";
        public const string DISTANCE = "Distance";
        public const string STEPS = "Steps";
        public const string APPPACKAGENAME = "com.google.android.gms";
        public const string STREAMNAME = "estimated_steps";
        public const int REQUESTOAUTH = 1;
        public const string DATEFORMAT = "yyyy.MM.dd HH:mm:ss";
        //
        public const string LINECHART = "LineChart";
        public const string BARCHART = "BarChart";
        public const string POINTCHART = "PointChart";
        //
        public const string HOST = "https://sanusapi.azurewebsites.net";
        public const string CONTENTYPE = "application/json";
        #region API
        public const string APISTEPS = "api/StepsDemo";
        public const string APIDISTANCES = "api/Distances";
        public const string APIENERGYS = "api/Energys";
        #endregion
        public const int PAGE_SIZE = 10;
        public const string PERMISSION = "You do not have permission to view this directory or page.";
        public const int MAXRESPONSECONTENTBUFFERSIZE = 256000;
        public const string TIME_FORMAT = "HH:mm";
        public const string DATE_FORMAT = "MMM dd, yyyy";
        public static JsonSerializerSettings JsonSettings { get; } = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        public static string CoreApi(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException();
            var p = path.Trim().Trim('/');
            return HOST + "/" + p;
        }
    }
}
