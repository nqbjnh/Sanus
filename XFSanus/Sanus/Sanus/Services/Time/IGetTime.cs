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
        DateTime PosteriorDay(int year, int month, int day);
        int PreviousWeek(int week);
       
        DateTime PreviousDay(int year, int month, int day);
        int PosteriorWeek(int week);
      
        // kiểm tra ngoại lệ
        int GetLastDayInMonth(int year, int month);
        DateTime PreviousMonth(int year, int month);
        DateTime PosteriorMonth(int year, int month);
        DateTime CheckYearInputMonth0(int year, int month);
        DateTime CheckYearInputMonth1(int year, int month);
    }
}
