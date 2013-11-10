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

        public int Donor_id { get; set; }
        public bloodType Blood_type { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public DateTime Last_donation { get; set; }
        public int Times_donated { get; set; }
        public int Times_contacted { get; set; }
        public DateTime Date_registered { get; set; }
        public DateTime Birth_date { get; set; }
        public string Reason_for_deferral { get; set; }
        public int Age { get; set; }
        public DateTime Next_available { get; set; }
        public bool Is_viable { get; set; }
        public bool Is_voluntary { get; set; }
        public bool Is_contactable { get; set; }
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
            
            times_contacted = 0;
            times_donated = 0;
        }

        public void Refresh()
        {
            age = DateTime.Today.Year - birth_date.Year;
            if (birth_date > DateTime.Today.AddYears(-age)) age--;
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
