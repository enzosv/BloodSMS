using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    public class Blood
    {
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

        public int Donor_id { get; set; }
        public DateTime Date_removed { get; set; }

        //Record Blood Ins
        //Create

        int Donor_id { get; set; }

        public Blood(int DONOR_ID, DateTime DATE_DONATED, DateTime DATE_EXPIRE, string COMPONENT)
        {
            donor_id = DONOR_ID;
            date_donated = DATE_DONATED;
            date_expire = DATE_EXPIRE;
            component = COMPONENT;

            is_assigned = false;
            patient_name = "";
            patient_age = 0;
            age = 0;
            is_quarantined = false;
            reason_for_removal = "";
        }

        void Assign(string PATIENT_NAME, int PATIENT_AGE, DateTime DATE_REMOVED)
        {
            is_assigned = true;
            date_removed = DATE_REMOVED;
            patient_name = PATIENT_NAME;
            patient_age = PATIENT_AGE;
            reason_for_removal = "Released to: " + patient_name + " on " + date_removed;
        }

        void Quarantine(string reason, DateTime DATE_REMOVED)
        {
            is_quarantined = true;
            date_removed = DATE_REMOVED;
            reason_for_removal = reason;
        }
        //get set all the shit

    }
}
