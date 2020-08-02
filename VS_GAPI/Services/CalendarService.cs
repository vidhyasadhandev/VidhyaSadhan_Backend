using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using VS_Models;

namespace VS_GAPI.Services
{
    public class CalenderService : ICalendarService
    {
        static string[] Scopes = { CalendarService.Scope.Calendar, CalendarService.Scope.CalendarEvents };
        static string ApplicationName = "vidhyasadhan-v1";

        public CalenderService()
        {

        }

        public CalendarService Initialize(string user)
        {
            try
            {
                GoogleCredential credential;
                using (var stream = new FileStream("vsadhan-svc.json", FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                         .CreateScoped(Scopes).CreateWithUser(user);
                }

                return new CalendarService(new BaseClientService.Initializer()
                {
                    ApplicationName = ApplicationName,
                    HttpClientInitializer = credential,
                });
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        public Event CreateEvent(CalendarService calendarService, CalendarEvent cEvent)
        {
            try
            {
                var eventBody = new Event
                {
                    Summary = cEvent.Summary,
                    Location = cEvent.Location,
                    Description = cEvent.Description,
                    Start = new EventDateTime() { DateTime = cEvent.Start, TimeZone = "Asia/Calcutta" },
                    End = new EventDateTime() { DateTime = cEvent.End, TimeZone = "Asia/Calcutta" },
                    Recurrence = cEvent.Recurrence.Select(item => "RRULE:FREQ=" + item.Frequency + ";COUNT=" + item.Count + "").ToList(),
                    Attendees = cEvent.Attendees.Select(x => new EventAttendee { Email = x, }).ToList(),
                    Reminders = new Event.RemindersData() { UseDefault = false, Overrides = new List<EventReminder> { new EventReminder { Method = "email", Minutes = 24 * 60 } } },
                    Organizer = new Event.OrganizerData() { Email = cEvent.Organizer, DisplayName = "Admin" },
                };
                var request  = calendarService.Events.Insert(eventBody, "primary");
                request.SendUpdates = EventsResource.InsertRequest.SendUpdatesEnum.All;
                return request.Execute();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private IEnumerable<string> GetRecurrence(IEnumerable<Recurrence> recurrence)
        {
            foreach (var item in recurrence)
            {
                string rule = "RRULE:FREQ=" + item.Frequency + ";COUNT=" + item.Count + "";
                yield return rule;
            }
            /*new String[] { "RRULE:FREQ=DAILY;COUNT=2" },*/
        }

        public Events ListEvents(CalendarService calendarService)
        {
            EventsResource.ListRequest request = calendarService.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 100;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();
            return events;
        }
    }
}
