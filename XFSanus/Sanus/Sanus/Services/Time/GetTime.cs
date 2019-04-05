using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Sanus.Services.Time
{
    public class GetTime : IGetTime
    {
        public DateTime CheckWeek(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public DateTime FirstDateOfWeek(int year, int weekOfYear, CultureInfo ci)
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

        public int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public DateTime GetFirstDayOfWeek(DateTime dateTime)
        {
            // so thu tu cua tuan
            int week = GetIso8601WeekOfYear(dateTime);
            // tra ve ngay dau tien cua tuan
            return FirstDateOfWeek(dateTime.Year, week, CultureInfo.CurrentCulture);
        }

        public Dictionary<string, DateTime> PosteriorWeek(DateTime dateTime)
        {
            Dictionary<string, DateTime> dictionary = new Dictionary<string, DateTime>();
            //
            if (dateTime.CompareTo(DateTime.Now) >= 0)
            {
                DateTime getweek = DateTime.Now;
                // say the week starts on a Sunday 
                while (getweek.DayOfWeek != DayOfWeek.Sunday)
                    getweek = getweek.AddDays(1);
                DateTimeFormatInfo info = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = info.Calendar;
                //Now you are on the first week add 3 more to move to the Fourth week 
                DateTime startDay = cal.AddWeeks(getweek, -1);
                DateTime endDay = startDay.AddDays(6);
                //
                dictionary.Add("startDay", startDay);
                dictionary.Add("endDay", endDay);
            }
            else if (dateTime.CompareTo(DateTime.Now) == -1)
            {

                DateTime getweek = dateTime;
                // say the week starts on a Sunday 
                while (getweek.DayOfWeek != DayOfWeek.Sunday)
                    getweek = getweek.AddDays(1);
                DateTimeFormatInfo info = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = info.Calendar;
                //Now you are on the first week add 3 more to move to the Fourth week 
                DateTime startDay = cal.AddWeeks(getweek, -1);
                DateTime endDay = startDay.AddDays(6);
                //
                dictionary.Add("startDay", startDay);
                dictionary.Add("endDay", endDay);
            }
            //
            return dictionary;
        }

        public int PreviousWeek(int week)
        {
            throw new NotImplementedException();
        }

        public DateTime PosteriorMonth(int year, int month)
        {
            DateTime dateTime = new DateTime();
            if (month < DateTime.Now.Month && year == DateTime.Now.Year)
            {
                switch (month)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        dateTime = new DateTime(year, month + 1, 1);
                        break;
                    case 12:
                        dateTime = new DateTime(year + 1, 1, 1);
                        break;
                }
            }
            else if (month >= DateTime.Now.Month && year == DateTime.Now.Year)
            {
                dateTime = DateTime.Now;
            }
            else if (year < DateTime.Now.Year)
            {
                switch (month)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        dateTime = new DateTime(year, month + 1, 1);
                        break;
                    case 12:
                        dateTime = new DateTime(year + 1, 1, 1);
                        break;
                }
            }
            return dateTime;
        }

        public DateTime PreviousMonth(int year, int month)
        {
            DateTime dateTime = new DateTime();
            if (month <= DateTime.Now.Month && year == DateTime.Now.Year)
            {
                switch (month)
                {
                    case 1:
                        dateTime = new DateTime(year - 1, 12, 1);
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                        dateTime = new DateTime(year, month - 1, 1);
                        break;
                }
            }
            else if (month > DateTime.Now.Month && year == DateTime.Now.Year)
            {
                dateTime = DateTime.Now;
            }
            else if (year != DateTime.Now.Year)
            {
                dateTime = DateTime.Now;
            }
            return dateTime;
        }

        public DateTime PreviousWeek(DateTime dateTime)
        {
            int week = GetIso8601WeekOfYear(dateTime);
            return FirstDateOfWeek(dateTime.Year, week - 1, CultureInfo.CurrentCulture);
        }

        public DateTime PreviousDay(int year, int month, int day)
        {
            DateTime tempDay = new DateTime();
            if (!DateTime.IsLeapYear(year))
            {
                if (day == 1)
                {
                    if (month == 3)
                    {
                        tempDay = new DateTime(year, month - 1, 28);
                    }
                    else if (month == 1)
                    {
                        tempDay = new DateTime(year - 1, 12, 31);
                    }
                    else if (month == 2 || month == 4 || month == 6 || month == 9 || month == 11 || month == 8)
                    {
                        tempDay = new DateTime(year, month - 1, 31);
                    }
                    else if (month == 5 || month == 7 || month == 10 || month == 12)
                    {
                        tempDay = new DateTime(year, month - 1, 30);
                    }
                }
                else
                {
                    switch (month)
                    {
                        case 1:
                        case 3:
                        case 5:
                        case 7:
                        case 8:
                        case 10:
                        case 12:
                            if (day > 0 && day <= 31)
                            {
                                tempDay = new DateTime(year, month, day - 1);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        case 4:
                        case 6:
                        case 9:
                        case 11:
                            if (day <= 30 && day > 0)
                            {
                                tempDay = new DateTime(year, month, day - 1);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        case 2:
                            if (day <= 28 && day > 0)
                            {
                                tempDay = new DateTime(year, month, day - 1);
                                break;
                            }
                            else
                            {
                                break;
                            }
                    }
                }
            }
            else
            {
                if (day == 1)
                {
                    if (month == 3)
                    {
                        tempDay = new DateTime(year, month - 1, 29);
                    }
                    else if (month == 1)
                    {
                        tempDay = new DateTime(year - 1, month - 1, 31);
                    }
                    else if (month == 2 || month == 4 || month == 6 || month == 9 || month == 11 || month == 8)
                    {
                        tempDay = new DateTime(year, month - 1, 31);
                    }
                    else if (month == 5 || month == 7 || month == 10 || month == 12)
                    {
                        tempDay = new DateTime(year, month - 1, 30);
                    }
                }
                else
                {
                    switch (month)
                    {
                        case 1:
                        case 3:
                        case 5:
                        case 7:
                        case 8:
                        case 10:
                        case 12:
                            if (day > 0 && day <= 31)
                            {
                                tempDay = new DateTime(year, month, day - 1);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        case 4:
                        case 6:
                        case 9:
                        case 11:
                            if (day <= 30 && day > 0)
                            {
                                tempDay = new DateTime(year, month, day - 1);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        case 2:
                            if (day <= 28 && day > 0)
                            {
                                tempDay = new DateTime(year, month, day - 1);
                                break;
                            }
                            else
                            {
                                break;
                            }
                    }
                }
            }
            return tempDay;
        }

        public DateTime PosteriorDay(int year, int month, int day)
        {
            DateTime tempDay = new DateTime();
            if (!DateTime.IsLeapYear(year))
            {
                switch (month)
                {
                    case 2:
                        if (day == 28)
                        {
                            tempDay = new DateTime(year, month + 1, 1);
                            break;
                        }
                        else if (day < 28 && day > 0)
                        {
                            tempDay = new DateTime(year, month, day + 1);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                        if (day == 31)
                        {
                            tempDay = new DateTime(year, month + 1, 1);
                            break;
                        }
                        else if (day < 31 && day > 0)
                        {
                            tempDay = new DateTime(year, month, day + 1);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case 12:
                        if (day == 31)
                        {
                            tempDay = new DateTime(year + 1, 1, 1);
                            break;
                        }
                        else if (day < 31 && day > 0)
                        {
                            tempDay = new DateTime(year, month, day + 1);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        if (day == 30)
                        {
                            tempDay = new DateTime(year, month + 1, 1);
                            break;
                        }
                        else if (day < 30 && day > 0)
                        {
                            tempDay = new DateTime(year, month, day + 1);
                            break;
                        }
                        else
                        {
                            break;
                        }
                }
            }
            else
            {
                switch (month)
                {
                    case 2:
                        if (day == 29)
                        {
                            tempDay = new DateTime(year, month + 1, 1);
                            break;
                        }
                        else if (day < 29 && day > 0)
                        {
                            tempDay = new DateTime(year, month, day + 1);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                        if (day == 31)
                        {
                            tempDay = new DateTime(year, month + 1, 1);
                            break;
                        }
                        else if (day < 31 && day > 0)
                        {
                            tempDay = new DateTime(year, month, day + 1);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case 12:
                        if (day == 31)
                        {
                            tempDay = new DateTime(year + 1, 1, 1);
                            break;
                        }
                        else if (day < 31 && day > 0)
                        {
                            tempDay = new DateTime(year, month, day + 1);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        if (day == 30)
                        {
                            tempDay = new DateTime(year, month + 1, 1);
                            break;
                        }
                        else if (day < 30 && day > 0)
                        {
                            tempDay = new DateTime(year, month, day + 1);
                            break;
                        }
                        else
                        {
                            break;
                        }
                }
            }
            return tempDay;
        }

        public int GetLastDayInMonth(int year, int month)
        {
            int day = 0;
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    day = 31;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    day = 30;
                    break;
                case 2:
                    if (!DateTime.IsLeapYear(year))
                    {
                        day = 28;
                        break;
                    }
                    else
                    {
                        day = 29;
                        break;
                    }
            }
            return day;
        }
    }
}
