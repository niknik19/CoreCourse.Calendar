using System;
using System.Globalization;

namespace CoreCourse.Calendar.Service
{
    public class CalendarService
    {
        private readonly DateTimeService _dateTimeService;

        public CalendarService(DateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        public CalendarValidationResult ValidateCalendarMonth(string stringYear, string stringMonthIndex)
        {
            var validationResult = new CalendarValidationResult();
            int year = 0;
            int monthIndex = 0;
            bool isYearParsed = false;
            bool isMonthParsed = false;

            if (stringYear != null)
            {
                isYearParsed = Int32.TryParse(stringYear, out year);
                if (!isYearParsed)
                {
                    validationResult.ErrorMessages.Add("Year is presented, but is not valid number. Please provide valid year as a number");
                }
                if (isYearParsed && (year < 1900 || year > 2100))
                {
                    validationResult.ErrorMessages.Add("$Year is presented, but is not in valid range. Valid range is between 1900 and 2100. Please present valid year");
                }
            }

            if(stringMonthIndex != null)
            {
                isMonthParsed = Int32.TryParse(stringMonthIndex, out monthIndex);
                if (!isMonthParsed)
                {
                    validationResult.ErrorMessages.Add("Month is presented, but is not valid number. Please provide valid year as a number");
                }

                if (isMonthParsed && (monthIndex < 1 || monthIndex > 12))
                { 
                    validationResult.ErrorMessages.Add("Month is presented but it not in valid range. Valid range is betwee 1 and 12. Please present valid month");
                }
            }

            if (stringYear == null && stringMonthIndex == null)
            {
                var today = _dateTimeService.GetUtcNow();
                year = today.Year;
                monthIndex = today.Month;
                isYearParsed = true;
                isMonthParsed = true;
            }
           
            if (!isYearParsed && isMonthParsed)
            {
                validationResult.ErrorMessages.Add("Month is presented, but year is not presented. Please provide valid year");
            }

            if (!isMonthParsed && isYearParsed)
            {
                validationResult.ErrorMessages.Add("Year is presented, but month is not presented. Please provide valid month");
            }

            if(isMonthParsed && isYearParsed)
            {
                validationResult.Year = year;
                validationResult.MonthIndex = monthIndex;
            }

            return validationResult;
        }

        public CalendarMonth GetCalendarMonth(int year, int month)
        {
            // Get first day of month
            var firstDayOfMonthResult = GetFirstDayOfMonth(year, month);
            if (firstDayOfMonthResult == null)
            {
                return null;
            }

            // Get first day of week for current week 
            var firstDayOfMonth = firstDayOfMonthResult.Value;
            var firstDayOfWeekMonth = firstDayOfMonth;
            while (firstDayOfWeekMonth.DayOfWeek != CalendarWeek.FirstDayOfWeek)
            {
                firstDayOfWeekMonth = firstDayOfWeekMonth.AddDays(-1);
            }

            // Fill calendar
            var calendarMonth = new CalendarMonth();

            var currentCalendarDate = firstDayOfWeekMonth;
            var firstDayOfCurrentWeek = firstDayOfWeekMonth;
            var firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

            while (firstDayOfCurrentWeek < firstDayOfNextMonth)
            {
                var calendarWeek = new CalendarWeek();
                foreach (var weekDay in CalendarWeek.WeekDays)
                {
                    var calendarDay = new CalendarDay();
                    calendarDay.Type = GetCalendarDayType(currentCalendarDate, month);
                    calendarDay.Value = GetCalendarDayValue(currentCalendarDate);
                    calendarWeek[weekDay] = calendarDay;
                    currentCalendarDate = currentCalendarDate.AddDays(1);
                }

                calendarMonth.Weeks.Add(calendarWeek);
                firstDayOfCurrentWeek = currentCalendarDate;
            }

            calendarMonth.Year = year;
            calendarMonth.MonthName = GetMonthName(month);

            return calendarMonth;
        }

        private CalendarDayType GetCalendarDayType(DateTime date, int monthIndex)
        {
            var type = CalendarDayType.None;

            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
            {
                type |= CalendarDayType.Holiday;
            }

            if (date.Month == monthIndex)
            {
                type |= CalendarDayType.Selected;
            }

            return type;
        }

        private string GetCalendarDayValue(DateTime date)
        {
            return date.Day.ToString();
        }

        private DateTime? GetFirstDayOfMonth(int year, int month)
        {
            DateTime firstDayOfMonth;
            try
            {
                firstDayOfMonth = new DateTime(year, month, 1);
            }
            catch
            {
                //TODO: log item 
                return null;
            }

            return firstDayOfMonth;
        }

        private string GetMonthName(int monthIndex)
        {
            return new DateTime(2015, monthIndex, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("en-us"));
        }
    }
}
