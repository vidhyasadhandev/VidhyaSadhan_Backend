using System;
using System.Collections.Generic;
using System.Text;

namespace VS_Models
{
    public class CalendarEvent
    {
        public string Summary { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string TimeZone { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public IEnumerable<Recurrence> Recurrence { get; set; }
        public IEnumerable<string> Attendees { get; set; }
        public string Organizer { get; set; }
        public string UserEmail { get; set; }
    }

    public class Recurrence
    {
        public string Frequency { get; set; }
        public string Count { get; set; }
    }
}
