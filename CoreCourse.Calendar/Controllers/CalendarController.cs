using CoreCourse.Calendar.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCourse.Calendar.Controllers
{
    
    public class CalendarController : Controller
    {
        private readonly CalendarService _calendarService; 

        public CalendarController(CalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [Route("[controller]/{year?}/{month?}")]
        public ViewResult Index(string year = null, string month = null)
        {
            var validationResult = _calendarService.ValidateCalendarMonth(year, month);
            if (!validationResult.IsValid)
            {
                return View("Error", validationResult);
            }

            var calendarMonth = _calendarService.GetCalendarMonth(validationResult.Year, validationResult.MonthIndex);
            return View(calendarMonth);
        }
    }
}
