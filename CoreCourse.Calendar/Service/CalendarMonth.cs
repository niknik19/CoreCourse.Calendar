
using System.Collections.Generic;

namespace CoreCourse.Calendar.Service
{ 
    public class CalendarMonth
    {
        public CalendarMonth()
        {
            Weeks = new List<CalendarWeek>();
        }

        public int Year { get; set; }

        public string MonthName { get; set; }

        public List<CalendarWeek> Weeks { get; set; }
    }
}
