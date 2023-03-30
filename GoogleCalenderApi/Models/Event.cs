using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace GoogleCalenderApi.Models
{
    public class Event
    {
        public Event()
        {
            this.Start = new EventDateTime()
            {
                TimeZone = "Africa/Cairo"
            };
            this.End = new EventDateTime()
            {
                TimeZone = "Africa/Cairo"
            };
        }
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public EventDateTime Start { get; set; }
        public EventDateTime End { get; set; }
    }
    public class EventDateTime
    {
        public string DateTime { get; set; }
        public string TimeZone { get; set; }

    }
}
