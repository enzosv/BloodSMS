using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    public class Donor
    {
        int donor_id;
        int blood_type_id;
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

        //Register new donor
        public Donor(int DONOR_ID, int BLOOD_TYPE_ID, string NAME, string STREET, string CITY, string PROVINCE, string EMAIL, string CELLPHONE, DateTime DATE_REGISTERED, bool IS_VIABLE, DateTime BIRTH_DATE, string REASON_FOR_DEFERRAL, bool IS_CONTACTABLE, int AGE, DateTime NEXT_AVAILABLE, bool IS_VOLUNTARY)
        {
            donor_id = DONOR_ID;
            blood_type_id = BLOOD_TYPE_ID;
            name = NAME;
            street = STREET;
            city = CITY;
            province = PROVINCE;
            email = EMAIL;
            cellphone = CELLPHONE;
            date_registered = DATE_REGISTERED;
            is_viable = IS_VIABLE;
            birth_date = BIRTH_DATE;
            reason_for_deferral = REASON_FOR_DEFERRAL;
            is_contactable = IS_CONTACTABLE;
            age = AGE;
            next_available = NEXT_AVAILABLE;
            is_voluntary = IS_VOLUNTARY;

            times_contacted = 0;
            times_donated = 0;
            
        }

        //I think these methods should be in a donorList class
        List<Donor> donors;
        List<Donor> getViableDonors()
        {
            List<Donor> viable = new List<Donor>();
            foreach(Donor d in donors)
            {
                if (d.is_viable && d.is_voluntary && d.is_contactable)
                    viable.Add(d);
            }
            return viable;
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
