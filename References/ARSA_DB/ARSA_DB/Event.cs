using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSA_DB
{
    class Event
    {
        private int eventId;
        private string eventName;
        private string eventDate;
        private string eventTime;
        private string eventVenue;

        public Event(int eid, string en, string ed, string et, string ev)
        {
            eventId = eid;
            eventName = en;
            eventDate = ed;
            eventTime = et;
            eventVenue = ev;
        }

        public int EventId
        {
            get { return eventId; }
            set { eventId = value; }
        }

        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

        public string EventDate
        {
            get { return eventDate; }
            set { eventDate = value; }
        }

        public string EventTime
        {
            get { return eventTime; }
            set { eventTime = value; }
        }

        public string EventVenue
        {
            get { return eventVenue; }
            set { eventVenue = value; }
        }
    }
}
