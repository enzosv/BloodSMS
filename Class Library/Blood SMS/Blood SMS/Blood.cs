using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    public class Blood
    {
        int? blood_id;
        int? taken_from;
        DateTime? date_added;
        DateTime? date_expire;
        string component;

        bool? is_assigned;
        string patient_name;
        int? patient_age;
        
        bool? is_quarantined;
        string reason_for_removal;
        DateTime? date_removed;

        int age;
        bool is_removed;

        public int Blood_id { get; set; }
        public int Taken_from { get; set; }
        public DateTime Date_added { get; set; }
        public DateTime Date_expire { get; set; }
        
        public string Patient_name { get; set; }
        public int Patient_age { get; set; }
        public bool Is_assigned { get; set; }
        public string Component { get; set; }
        public bool Is_quarantined { get; set; }
        public string Reason_for_removal { get; set; }
        public DateTime Date_removed { get; set; }

        public int Age { get; set; }
        public bool Is_removed { get; set; }

        //Record Blood Ins
        //Create

        public Blood(int BLOOD_ID, int TAKEN_FROM, DateTime DATE_ADDED, DateTime DATE_EXPIRE)
        {
            blood_id = BLOOD_ID;
            taken_from = TAKEN_FROM;
            date_added = DATE_ADDED;
            date_expire = DATE_EXPIRE;

            //set defaults
			component = "Whole";
            setDefaults();
        }

        //From SQL
        public Blood(int? BLOOD_ID, int? TAKEN_FROM, DateTime? DATE_ADDED, DateTime? DATE_EXPIRE, string COMPONENT, string PATIENT_NAME, int? PATIENT_AGE, DateTime? DATE_REMOVED, bool? IS_ASSIGNED, bool? IS_QUARANTINED, string REASON_FOR_REMOVAL )
        {
            blood_id = BLOOD_ID;
            taken_from = TAKEN_FROM;
            date_added = DATE_ADDED;
            date_expire = DATE_EXPIRE;
            component = COMPONENT;

            date_removed = DATE_REMOVED;
            is_assigned = IS_ASSIGNED;
            patient_name = PATIENT_NAME;
            patient_age = PATIENT_AGE;
            is_quarantined = IS_QUARANTINED;
            reason_for_removal = REASON_FOR_REMOVAL;

            if (date_removed != DateTime.MinValue)
            {
                is_removed = true;
            }
            else
                is_removed = false;
            Refresh();
        }
		
        //From extract
		public Blood(int? TAKEN_FROM, int? BLOOD_ID, DateTime? DATE_ADDED, DateTime? DATE_EXPIRE, string COMPONENT)
		{
			blood_id = BLOOD_ID;
			taken_from = TAKEN_FROM;
			date_added = DATE_ADDED;
			date_expire = DATE_EXPIRE;
			component = COMPONENT;

            setDefaults();
		}

        void setDefaults()
        {
            date_removed = DateTime.MinValue;
            is_assigned = false;
            patient_name = "";
            patient_age = 0;
            is_quarantined = false;
            reason_for_removal = "";
            is_removed = false;
            Refresh();
        }

        public void Refresh()
        {
            TimeSpan span = DateTime.Today - date_added.Value;
            age = span.Days;
            if (!is_quarantined.Value && date_expire.Value < DateTime.Today)
            {
                Quarantine("Expired on " + date_expire.Value.ToShortDateString(), date_expire.Value);
            }
        }

        public void Assign(string PATIENT_NAME, int PATIENT_AGE)
        {
            is_assigned = true;
            patient_name = PATIENT_NAME;
            patient_age = PATIENT_AGE;
        }

        public void Release(string PATIENT_NAME, int PATIENT_AGE, DateTime DATE_REMOVED)
        {
            Assign(PATIENT_NAME, PATIENT_AGE);
            Release(DATE_REMOVED);
            is_removed = true;
        }

        public bool Release(DateTime DATE_REMOVED)
        {
            if (is_assigned.Value)
            {
                date_removed = DATE_REMOVED;
                reason_for_removal = "Released to: " + patient_name + " on " + date_removed.Value.ToShortDateString();
                is_removed = true;
                return true;
            }
            return false;
        }

        public void Quarantine(string reason, DateTime DATE_REMOVED)
        {
            is_quarantined = true;
            date_removed = DATE_REMOVED;
            reason_for_removal = reason;
            is_removed = true;
        }
		
		public void Extract(DateTime DATE_REMOVED)
		{
			date_removed = DATE_REMOVED;
            reason_for_removal = "Extracted to multiple components on: " + date_removed.Value.ToShortDateString();
            is_removed = true;
		}

    }
}
