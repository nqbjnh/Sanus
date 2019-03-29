using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

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
        // 
        // lay ngay dau tien cua tuan phia truoc tuan hien tai
        public static DateTime PreviousWeek(DateTime dateTime)
        {
            int week = GetIso8601WeekOfYear(dateTime);
            return FirstDateOfWeek(dateTime.Year, week - 1, CultureInfo.CurrentCulture);
        }
        // lay ngay dau tien cua tuan phia sau tuan hien tai
        public static DateTime PosteriorWeek(DateTime dateTime)
        {
            int week = GetIso8601WeekOfYear(dateTime);
            return FirstDateOfWeek(dateTime.Year, week + 1, CultureInfo.CurrentCulture);
        }
        private static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        private static DateTime FirstDateOfWeek(int year, int weekOfYear, CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }
    }
}
