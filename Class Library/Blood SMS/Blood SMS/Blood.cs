using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    public class Blood
    {
        int blood_id;
        int donor_id;
        DateTime date_donated;
        DateTime date_expire;
        string component;

        bool is_assigned;
        string patient_name;
        int patient_age;

        int age;
        bool is_quarantined;
        string reason_for_removal;
        DateTime date_removed;

        public int Blood_id { get; set; }
        public int Donor_id { get; set; }
        public DateTime Date_donated { get; set; }
        public DateTime Date_expire { get; set; }
        public int Age { get; set; }
        public string Patient_name { get; set; }
        public int Patient_age { get; set; }
        public bool Is_assigned { get; set; }
        public string Component { get; set; }
        public bool Is_quarantined { get; set; }
        public string Reason_for_removal { get; set; }
        public DateTime Date_removed { get; set; }

        

        //Record Blood Ins
        //Create

        public Blood(int DONOR_ID, DateTime DATE_DONATED, DateTime DATE_EXPIRE, string COMPONENT)
        {
            donor_id = DONOR_ID;
            date_donated = DATE_DONATED;
            date_expire = DATE_EXPIRE;
            component = COMPONENT;

            date_removed = DateTime.MinValue;
            is_assigned = false;
            patient_name = "";
            patient_age = 0;
            age = 0;
            is_quarantined = false;
            reason_for_removal = "";
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
        }

        public void Release(DateTime DATE_REMOVED)
        {
            if (is_assigned)
            {
                date_removed = DATE_REMOVED;
                reason_for_removal = "Released to: " + patient_name + " on " + date_removed;
            }
        }

        public void Quarantine(string reason, DateTime DATE_REMOVED)
        {
            is_quarantined = true;
            date_removed = DATE_REMOVED;
            reason_for_removal = reason;
        }
        //get set all the shit

    }
}
