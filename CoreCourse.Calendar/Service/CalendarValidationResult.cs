using System.Collections.Generic;

namespace CoreCourse.Calendar.Service
{
    public class CalendarValidationResult
    {
        public CalendarValidationResult()
        {
            ErrorMessages = new List<string>();
        }

        public int Year { get; set; }

        public int MonthIndex { get; set; }

        public List<string> ErrorMessages { get; }

        public bool IsValid => ErrorMessages.Count == 0;
    }

}
