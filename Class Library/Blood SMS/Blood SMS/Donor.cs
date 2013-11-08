using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    
    public class Donor
    {
        public int donor_id;
        public bloodType blood_type;
        string name;
        string street;
        string city;
        string province;
        string email;
        string cellphone;
        DateTime last_donation;
        int times_donated;
        int times_contacted;
        DateTime date_registered;
        bool is_viable;
        DateTime birth_date;
        string reason_for_deferral;
        bool is_contactable;
        int age;
        DateTime next_available;
        bool is_voluntary;

        int Donor_id { get; set; }
        bloodType Blood_type { get; set; }
        //Register new donor
        public Donor(int DONOR_ID, 
            bloodType BLOOD_TYPE, 
            string NAME, 
            string STREET, 
            string CITY, 
            string PROVINCE, 
            string EMAIL, 
            string CELLPHONE,
            string REASON_FOR_DEFERRAL,
            DateTime DATE_REGISTERED,
            DateTime NEXT_AVAILABLE,
            DateTime BIRTH_DATE,
            bool IS_VIABLE, 
            bool IS_CONTACTABLE,
            bool IS_VOLUNTARY
            )
        {
            donor_id = DONOR_ID;
            blood_type = BLOOD_TYPE;
            name = NAME;
            street = STREET;
            city = CITY;
            province = PROVINCE;
            email = EMAIL;
            cellphone = CELLPHONE;
            reason_for_deferral = REASON_FOR_DEFERRAL;
            date_registered = DATE_REGISTERED;
            next_available = NEXT_AVAILABLE;
            birth_date = BIRTH_DATE;
            is_viable = IS_VIABLE;
            is_contactable = IS_CONTACTABLE;
            is_voluntary = IS_VOLUNTARY;

            //http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
            age = DateTime.Today.Year - BIRTH_DATE.Year;
            if (BIRTH_DATE > DateTime.Today.AddYears(-age)) age--;
            times_contacted = 0;
            times_donated = 0;
        }

        //I think these methods should be in a donorList class
        List<Donor> donors;
        List<Donor> getViableDonors()
        {
            List<Donor> viableDonors = new List<Donor>();
            foreach(Donor d in donors)
            {
                if (d.is_viable && d.is_voluntary && d.is_contactable)
                    viableDonors.Add(d);
            }
            return viableDonors;
        }

        void Contact()
        {
            times_contacted++;
        }

        void Donate()
        {
            times_donated++;
        }
    }
}
