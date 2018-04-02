using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCourse.Calendar.Service
{
    public class CalendarWeek
    {
        public CalendarWeek()
        {
            values = new CalendarDay[7];
        }

        private CalendarDay[] values { get; set; }

        public CalendarDay this[DayOfWeek dayOfWeek]
        {
            get { return values[(int)dayOfWeek]; }
            set { values[(int)dayOfWeek] = value; }
        }

        public static DayOfWeek FirstDayOfWeek => DayOfWeek.Sunday;

        public static IEnumerable<DayOfWeek> WeekDays
        {
            get
            {
                var currentDayOfWeek = FirstDayOfWeek;
                do
                {
                    yield return currentDayOfWeek;

                    currentDayOfWeek = currentDayOfWeek == DayOfWeek.Saturday
                        ? 0
                        : currentDayOfWeek + 1;
                }
                while (currentDayOfWeek != FirstDayOfWeek);
            }
        }
    }

}
