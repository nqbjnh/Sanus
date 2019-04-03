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

        public int PosteriorMonth(int month)
        {
            throw new NotImplementedException();
        }

        public DateTime GetFirstDayOfWeek(DateTime dateTime)
        {
            // so thu tu cua tuan
            int week = GetIso8601WeekOfYear(dateTime);
            // tra ve ngay dau tien cua tuan
            return FirstDateOfWeek(dateTime.Year, week, CultureInfo.CurrentCulture);
        }

        public int PosteriorWeek(int week)
        {
            throw new NotImplementedException();
        }

        public int PreviousDay(int day)
        {
            throw new NotImplementedException();
        }

        public int PreviousMonth(int month)
        {
            throw new NotImplementedException();
        }

        public DateTime PreviousWeek(DateTime dateTime)
        {
            int week = GetIso8601WeekOfYear(dateTime);
            return FirstDateOfWeek(dateTime.Year, week - 1, CultureInfo.CurrentCulture);
        }

        public int PreviousWeek(int week)
        {
            throw new NotImplementedException();
        }

        public DateTime PosteriorDay(int year, int month, int day)
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
    }
}
