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

        public List<Component> components;

        public string Accession_number { get { return accession_number; } }
        public bloodType Blood_type { get { return blood_type; }  }
        public int? Donor_id { get { return donor_id; } }
        public DateTime Date_donated { get { return date_donated; } }

        public Blood(string ACCESSION_NUMBER, int BLOOD_TYPE, int? DONOR_ID, DateTime DATE_ADDED)
        {
            accession_number = ACCESSION_NUMBER;
            blood_type = (bloodType)BLOOD_TYPE;
            donor_id = DONOR_ID;
            components = new List<Component>();
        }

        public void AddComponent(Component c)
        {
            components.Add(c);
        }

        public void RemoveComponent(Component c)
        {
            if(components.Contains(c))
                components.Remove(c);
        }
    }
}
