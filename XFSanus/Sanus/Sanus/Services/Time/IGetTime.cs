using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Sanus.Services.Time
{
    public interface IGetTime
    {
        // lay ngay dau tien cua tuan phia truoc tuan hien tai
        DateTime CheckWeek(DateTime dateTime);
        DateTime PreviousWeek(DateTime dateTime);
        // lay ngay dau tien cua tuan phia sau tuan hien tai
        DateTime GetFirstDayOfWeek(DateTime dateTime);
        int GetIso8601WeekOfYear(DateTime time);
        DateTime FirstDateOfWeek(int year, int weekOfYear, CultureInfo ci);
        int PreviousDay(int day);
        int PreviousWeek(int week);
        int PreviousMonth(int month);
        DateTime PosteriorDay(int year, int month, int day);
        int PosteriorWeek(int week);
        int PosteriorMonth(int month);
    }
}
