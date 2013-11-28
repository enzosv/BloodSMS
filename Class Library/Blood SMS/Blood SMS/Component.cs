using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    public class Component
    {
        string accession_number;
        string component_name;
        string patient_last_name;
        string patient_first_name;
        string patient_middle_initial;
        int patient_age;
        DateTime date_processed;
        DateTime date_reprocessed;
        DateTime date_expired;
        DateTime date_quarantined;
        DateTime date_assigned;
        DateTime date_released;
        string reason_for_removal;

        int age;

        public string Accession_number { get { return accession_number; }}
        public string Component_name{ get { return component_name; }}
        public string Patient_last_name{ get { return patient_last_name; }}
        public string Patient_first_name{ get { return patient_first_name; }}
        public string Patient_middle_initial{ get { return patient_middle_initial; }}
        public int Patient_age { get { return patient_age; } }
        public DateTime Date_processed { get { return date_processed; }}
        public DateTime Date_reprocessed { get { return date_reprocessed; } }
        public DateTime Date_expired { get { return date_expired; } }
        public DateTime Date_quarantined { get { return date_quarantined; } }
        public DateTime Date_assigned { get { return date_assigned; } }
        public DateTime Date_released { get { return date_released; }}
        public string Reason_for_removal { get { return reason_for_removal; }}

        public int Age { get { return age; } }

        //from SQL
        public Component(string ACCESSION_NUMBER, string COMPONENT_NAME, DateTime DATE_PROCESSED, DateTime DATE_EXPIRED, string PATIENT_LAST_NAME, string PATIENT_FIRST_NAME, string PATIENT_MIDDLE_INITIAL, int PATIENT_AGE, DateTime DATE_REPROCESSED, DateTime DATE_QUARANTINED, DateTime DATE_ASSIGNED, DateTime DATE_RELEASED, string REASON_FOR_REMOVAL)
        {
            accession_number = ACCESSION_NUMBER;
            component_name = COMPONENT_NAME;
            date_processed = DATE_PROCESSED;
            date_expired = DATE_EXPIRED;

            patient_age = PATIENT_AGE;
            patient_last_name = PATIENT_LAST_NAME;
            patient_first_name = PATIENT_FIRST_NAME;
            patient_middle_initial = PATIENT_MIDDLE_INITIAL;
            date_reprocessed = DATE_REPROCESSED;
            date_quarantined = DATE_QUARANTINED;
            date_assigned = DATE_ASSIGNED;
            date_released = DATE_RELEASED;
            reason_for_removal = REASON_FOR_REMOVAL;

            Refresh();
        }

        //Create
        public Component(string ACCESSION_NUMBER, string COMPONENT_NAME, DateTime DATE_PROCESSED, DateTime DATE_EXPIRED)
        {
            accession_number = ACCESSION_NUMBER;
            component_name = COMPONENT_NAME;
            date_processed = DATE_PROCESSED;
            date_expired = DATE_EXPIRED;

            patient_age = 0;
            patient_last_name = "";
            patient_first_name = "";
            patient_middle_initial = "";
            date_reprocessed = DateTime.MinValue;
            date_quarantined = DateTime.MinValue;
            date_assigned = DateTime.MinValue;
            date_released = DateTime.MinValue;
            reason_for_removal = "";

            Refresh();
        }

        public void Refresh()
        {        
           if (date_quarantined != DateTime.MinValue && date_expired < DateTime.Today)
           {
               TimeSpan span = DateTime.Today - date_processed;
               age = span.Days;
               Quarantine(date_expired, "Expired on " + date_expired.ToShortDateString());
           }

        }

        public void Assign(string PATIENT_LAST_NAME, string PATIENT_FIRST_NAME, string PATIENT_MIDDLE_INITIAL, int PATIENT_AGE, DateTime DATE_ASSIGNED)
        {
            patient_last_name = PATIENT_LAST_NAME;
            patient_first_name = PATIENT_FIRST_NAME;
            patient_middle_initial = PATIENT_MIDDLE_INITIAL;
            patient_age = PATIENT_AGE;
            date_assigned = DATE_ASSIGNED;
        }

        public void Unassign()
        {
            patient_last_name = "";
            patient_first_name = "";
            patient_middle_initial = "";
            patient_age = 0;
            date_assigned = DateTime.MinValue;
            reason_for_removal = "";
        }

        public void Release(DateTime date, string REASON_FOR_REMOVAL)
        {
            date_released = date;
            reason_for_removal = REASON_FOR_REMOVAL;
        }

        public void Unrelease()
        {
            date_released = DateTime.MinValue;
            reason_for_removal = "";
        }

        public void Quarantine(DateTime date, string REASON_FOR_REMOVAL)
        {
            date_quarantined = date;
            reason_for_removal = REASON_FOR_REMOVAL;
        }

        public void Unquarantine()
        {
            date_quarantined = DateTime.MinValue;
            reason_for_removal = "";
        }

        public void Reprocess(DateTime date, string REASON_FOR_REMOVAL)
        {
            date_reprocessed = date;
            reason_for_removal = REASON_FOR_REMOVAL;
        }

        public void Unreprocess()
        {
            date_reprocessed = DateTime.MinValue;
            reason_for_removal = "";
        }
    }
}
