using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    public class Blood
    {
        string accession_number;
        bloodType blood_type;
        int? donor_id;
        DateTime date_added;
        DateTime date_expire;
        DateTime date_removed;
        bool is_assigned;
        bool is_processed;
        bool is_quarantined;
        string reason_for_removal;

        int age;
        bool is_removed;

        public List<Patient> patients;

        public string Accession_number { get { return accession_number; } set { accession_number = value; } }
        public bloodType Blood_type { get { return blood_type; } set { blood_type = value; } }
        public int? Donor_id { get { return donor_id; } set { donor_id = value; } }
        public DateTime Date_added { get { return date_added; } set { date_added = value; } }
        public DateTime Date_expire { get { return date_expire; } set { date_expire = value; } }
        public DateTime Date_removed { get { return date_removed; } set { date_removed = value; } }
        public bool Is_assigned { get { return is_assigned; } set { is_assigned = value; } }
        public bool Is_processed { get { return is_processed; } set { is_processed = value; } }
        public bool Is_quarantined { get { return is_quarantined; } set { is_quarantined = value; } }
        public string Reason_for_removal { get{return reason_for_removal;} set{ reason_for_removal = value;} }

        public int Age { get{return age;}}
        public bool Is_removed { get{return is_removed;} set{ is_removed = value;} }

        //Record Blood Ins
        //Create

        public Blood(string ACCESSION_NUMBER, int BLOOD_TYPE, int? DONOR_ID, DateTime DATE_ADDED, DateTime DATE_EXPIRE)
        {
            accession_number = ACCESSION_NUMBER;
            blood_type = (bloodType)BLOOD_TYPE;
            donor_id = DONOR_ID;
            date_added = DATE_ADDED;
            date_expire = DATE_EXPIRE;

            date_removed = DateTime.MinValue;
            is_assigned = false;
            is_processed = false;
            is_quarantined = false;
            reason_for_removal = "";

            is_removed = false;
            Refresh();
        }

        //From SQL
        public Blood(string ACCESSION_NUMBER, int? BLOOD_TYPE, int? DONOR_ID, DateTime? DATE_ADDED, DateTime? DATE_EXPIRE, DateTime? DATE_REMOVED, bool? IS_ASSIGNED, bool? IS_PROCESSED, bool? IS_QUARANTINED, string REASON_FOR_REMOVAL)
        {
            accession_number = ACCESSION_NUMBER;
            blood_type = (bloodType)BLOOD_TYPE.Value;
            donor_id = DONOR_ID;
            date_added = DATE_ADDED.Value;
            date_expire = DATE_EXPIRE.Value;

            date_removed = DATE_REMOVED.Value;
            is_assigned = IS_ASSIGNED.Value;
            is_processed = IS_PROCESSED.Value;
            is_quarantined = IS_QUARANTINED.Value;
            reason_for_removal = REASON_FOR_REMOVAL;

            if (date_removed != DateTime.MinValue)
            {
                is_removed = true;
            }
            else
                is_removed = false;
            patients = new List<Patient>();
            Refresh();
        }

        //foreach blood in available blood b.refresh; updateBlood(b);
        //
        public void Refresh()
        {
            TimeSpan span = DateTime.Today - date_added;
            age = span.Days;
            if (!is_quarantined && date_expire < DateTime.Today)
            {
                Quarantine("Expired on " + date_expire.ToShortDateString(), date_expire);
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

        /*public bool Release(DateTime DATE_REMOVED)
        {
            if (is_assigned)
            {
                date_removed = DATE_REMOVED;
                reason_for_removal = "Released to: " + patient_name + " on " + date_removed.ToShortDateString();
                is_removed = true;
                return true;
            }
            return false;
        }*/

        public void Quarantine(string reason, DateTime DATE_REMOVED)
        {
            is_quarantined = true;
            date_removed = DATE_REMOVED;
            reason_for_removal = reason;
            is_removed = true;
        }

        public void AddPatient(Patient patient)
        {
            patients.Add(patient);
        }

        public void RemovePatient(Patient patient)
        {
            if(patients.Contains(patient))
                patients.Remove(patient);
        }

    }
}
