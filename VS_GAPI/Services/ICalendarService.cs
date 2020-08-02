using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Text;
using VS_Models;

namespace VS_GAPI.Services
{
    public interface ICalendarService
    {
        CalendarService Initialize(string user);
        Event CreateEvent(CalendarService calendarService,CalendarEvent cEvent);

        Events ListEvents(CalendarService calendarService);
    }
}
