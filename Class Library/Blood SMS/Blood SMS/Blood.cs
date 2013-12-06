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
        DateTime date_donated;
        DateTime date_removed;
        bool is_removed;

        public List<Component> components;

        public string Accession_number { get { return accession_number; } }
        public bloodType Blood_type { get { return blood_type; } }
        public int? Donor_id { get { return donor_id; } }
        public DateTime Date_donated { get { return date_donated; } }
        public DateTime Date_removed { get { return date_removed; } set { date_removed = value; } }
        public bool Is_removed { get { return is_removed; } set { is_removed = value; } }

        //new donation
        public Blood(string ACCESSION_NUMBER, int BLOOD_TYPE, int? DONOR_ID, DateTime DATE_DONATED)
        {
            accession_number = ACCESSION_NUMBER;
            blood_type = (bloodType)BLOOD_TYPE;
            donor_id = DONOR_ID;
            date_donated = DATE_DONATED;

            date_removed = DateTime.MinValue;
            is_removed = false;
            components = new List<Component>();

        }

        //from SQL
        public Blood(string ACCESSION_NUMBER, int BLOOD_TYPE, int? DONOR_ID, DateTime DATE_DONATED, DateTime DATE_REMOVED)
        {
            accession_number = ACCESSION_NUMBER;
            blood_type = (bloodType)BLOOD_TYPE;
            donor_id = DONOR_ID;
            date_donated = DATE_DONATED;
            date_removed = DATE_REMOVED;
            components = new List<Component>();
            if (date_removed == DateTime.MinValue)
                is_removed = false;
            else
                is_removed = true;
        }

        //NO DONOR
        public Blood(string ACCESSION_NUMBER, int BLOOD_TYPE, DateTime DATE_DONATED)
        {
            accession_number = ACCESSION_NUMBER;
            blood_type = (bloodType)BLOOD_TYPE;
            date_donated = DATE_DONATED;

            date_removed = DateTime.MinValue;
            is_removed = false;
            components = new List<Component>();
        }

        public bool checkRemoved()
        {
            bool old_is_removed = is_removed;
            DateTime latestDate = DateTime.MinValue;
            foreach (Component c in components)
            {
                if (!c.Is_removed)
                {
                    date_removed = DateTime.MinValue;
                    is_removed = false;
                    break;
                }
                else
                {
                    if (c.Date_removed > latestDate)
                        latestDate = c.Date_removed;
                    is_removed = true;
                    date_removed = latestDate;
                }
            }
            return (old_is_removed != is_removed);
        }

        public void AddComponent(Component c)
        {
            components.Add(c);
        }

        public void RemoveComponent(Component c)
        {
            if (components.Contains(c))
                components.Remove(c);
        }

    }
}
