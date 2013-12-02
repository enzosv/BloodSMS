using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    public class Component
    {
        string accession_number;
        bloodComponents component_name;
        removalType removal_type;
        string patient_last_name;
        string patient_first_name;
        string patient_middle_initial;
        int patient_age;
        DateTime date_processed;
        DateTime date_expired;
        DateTime date_assigned;
        DateTime date_removed;
        string reason_for_removal;

        bool is_removed;
        bool is_quarantined;
        bool is_released;
        bool is_reprocessed;

        public string Accession_number { get { return accession_number; } set { accession_number = value; } }
        public bloodComponents Component_name { get { return component_name; } }
        public removalType Removal_Type { get { return removal_type; } }
        public string Patient_last_name { get { return patient_last_name; } }
        public string Patient_first_name { get { return patient_first_name; } }
        public string Patient_middle_initial { get { return patient_middle_initial; } }
        public int Patient_age { get { return patient_age; } }
        public DateTime Date_processed { get { return date_processed; } }
        public DateTime Date_expired { get { return date_expired; } }
        public DateTime Date_assigned { get { return date_assigned; } }
        public DateTime Date_removed { get { return date_removed; } }
        public string Reason_for_removal { get { return reason_for_removal; } }

        public bool Is_removed { get { return is_removed; } }
        public bool Is_quarantined { get { return is_quarantined; } }
        public bool Is_released { get { return is_released; } }
        public bool Is_reprocessed { get { return is_reprocessed; } }
        public string Patient_name { get { return patient_last_name + ", " + patient_first_name + " " + patient_middle_initial; } }

        //from SQL
        public Component(string ACCESSION_NUMBER, int COMPONENT_NAME, int REMOVAL_TYPE, DateTime DATE_PROCESSED, DateTime DATE_EXPIRED, DateTime DATE_ASSIGNED, DateTime DATE_REMOVED, string PATIENT_LAST_NAME, string PATIENT_FIRST_NAME, string PATIENT_MIDDLE_INITIAL, int PATIENT_AGE, string REASON_FOR_REMOVAL)
        {
            accession_number = ACCESSION_NUMBER;
            component_name = (bloodComponents)COMPONENT_NAME;
            removal_type = (removalType)REMOVAL_TYPE;
            date_processed = DATE_PROCESSED;
            date_expired = DATE_EXPIRED;
            date_assigned = DATE_ASSIGNED;
            date_removed = DATE_REMOVED;

            patient_age = PATIENT_AGE;
            patient_last_name = PATIENT_LAST_NAME;
            patient_first_name = PATIENT_FIRST_NAME;
            patient_middle_initial = PATIENT_MIDDLE_INITIAL;
            reason_for_removal = REASON_FOR_REMOVAL;

            if (removal_type != removalType.NotRemoved)
            {
                is_removed = true;
                switch (removal_type)
                {
                    case removalType.Released:
                        is_released = true;
                        break;
                    case removalType.Reprocessed:
                        is_reprocessed = true;
                        break;
                    case removalType.Quarantined:
                        is_quarantined = true;
                        break;
                }
            }
            else
            {
                is_removed = false;
                is_released = false;
                is_quarantined = false;
                is_reprocessed = false;
            }
            Refresh();
        }

        //Create
        public Component(string ACCESSION_NUMBER, int COMPONENT_NAME, DateTime DATE_PROCESSED, DateTime DATE_EXPIRED)
        {
            accession_number = ACCESSION_NUMBER;
            component_name = (bloodComponents)COMPONENT_NAME;
            date_processed = DATE_PROCESSED;
            date_expired = DATE_EXPIRED;

            removal_type = removalType.NotRemoved;
            patient_age = 0;
            patient_last_name = "";
            patient_first_name = "";
            patient_middle_initial = "";
            date_assigned = DateTime.MinValue;
            date_removed = DateTime.MinValue;
            reason_for_removal = "";

            Refresh();
        }

        public void Refresh()
        {
            if (!is_removed && date_expired <= DateTime.Today)
                Quarantine(date_expired, "Expired on " + date_expired.ToShortDateString());
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

        public void Unremove()
        {
            date_removed = DateTime.MinValue;
            reason_for_removal = "";
            is_released = false;
            is_quarantined = false;
            is_reprocessed = false;
            is_removed = false;
        }

        public void Release(DateTime date, string REASON_FOR_REMOVAL)
        {
            date_removed = date;
            reason_for_removal = REASON_FOR_REMOVAL;
            removal_type = removalType.Released;
            is_released = true;
            is_removed = true;
        }

        public void Quarantine(DateTime date, string REASON_FOR_REMOVAL)
        {
            date_removed = date;
            reason_for_removal = REASON_FOR_REMOVAL;
            removal_type = removalType.Released;
            is_removed = true;
            is_quarantined = true;
        }

        public void Reprocess(DateTime date, string REASON_FOR_REMOVAL)
        {
            date_removed = date;
            reason_for_removal = REASON_FOR_REMOVAL;
            removal_type = removalType.Released;
            is_reprocessed = true;
            is_removed = true;
        }

    }
}
