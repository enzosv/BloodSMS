using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{

    public class Donor
    {
        int? donor_id;
        string name;
        bloodType? blood_type;
        string home_province;
        string home_city;
        string home_street;
        string office_province;
        string office_city;
        string office_street;
        contactMethod? preferred_contact_method;
        string home_landline;
        string office_landline;
        string email;
        string cellphone;
        string educational_attainment;
        DateTime? birth_date;
        DateTime? date_registered;
        DateTime? last_donation;
        DateTime? next_available;
        int? times_donated;
        int? times_contacted;
        bool? is_contactable;
        bool? is_viable;
        string reason_for_deferral;

        int age;


        public int Donor_id { get; set; }
        public string Name { get; set; }
        public bloodType Blood_type { get; set; }
        public string Home_province { get; set; }
        public string Home_city { get; set; }
        public string Home_street { get; set; }
        public string Office_province { get; set; }
        public string Office_city { get; set; }
        public string Office_street { get; set; }
        public contactMethod Preferred_contact_method { get; set; }
        public string Home_landline { get; set; }
        public string Office_landline { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public string Educational_attainment { get; set; }
        public DateTime Birth_date { get; set; }
        public DateTime Date_registered { get; set; }
        public DateTime Last_donation { get; set; }
        public DateTime Next_available { get; set; }
        public int Times_donated { get; set; }
        public int Times_contacted { get; set; }
        public bool Is_contactable { get; set; }
        public bool Is_viable { get; set; }
        public string Reason_for_deferral { get; set; }

        public int Age { get; set; }
        //Register new donor
        public Donor(int? DONOR_ID,
        string NAME,
        bloodType? BLOOD_TYPE,
        string HOME_PROVINCE,
        string HOME_CITY,
        string HOME_STREET,
        string OFFICE_PROVINCE,
        string OFFICE_CITY,
        string OFFICE_STREET,
        contactMethod? PREFERRED_CONTACT_METHOD,
        string HOME_LANDLINE,
        string OFFICE_LANDLINE,
        string EMAIL,
        string CELLPHONE,
        string EDUCATIONAL_ATTAINMENT,
        DateTime? BIRTH_DATE,
        DateTime? DATE_REGISTERED,
        DateTime? LAST_DONATION,
        DateTime? NEXT_AVAILABLE,
        int? TIMES_DONATED,
        int? TIMES_CONTACTED,
        bool? IS_CONTACTABLE,
        bool? IS_VIABLE,
        string REASON_FOR_DEFERRAL
            )
        {
            donor_id = DONOR_ID;
            blood_type = BLOOD_TYPE;
            name = NAME;
            home_province = HOME_PROVINCE;
            home_city = HOME_CITY;
            home_street = HOME_STREET;
            office_province = OFFICE_PROVINCE;
            office_city = OFFICE_CITY;
            office_street = OFFICE_STREET;
            preferred_contact_method = PREFERRED_CONTACT_METHOD;
            home_landline = HOME_LANDLINE;
            office_landline = OFFICE_LANDLINE;
            email = EMAIL;
            cellphone = CELLPHONE;
            educational_attainment = EDUCATIONAL_ATTAINMENT;
            birth_date = BIRTH_DATE;
            date_registered = DATE_REGISTERED;
            last_donation = LAST_DONATION;
            next_available = NEXT_AVAILABLE;
            times_donated = TIMES_DONATED;
            times_contacted = TIMES_CONTACTED;
            is_contactable = IS_CONTACTABLE;
            is_viable = IS_VIABLE;
            reason_for_deferral = REASON_FOR_DEFERRAL;
            Refresh();
        }

        public Donor(int? DONOR_ID,
        string NAME,
        bloodType? BLOOD_TYPE,
        string HOME_PROVINCE,
        string HOME_CITY,
        string HOME_STREET,
        string OFFICE_PROVINCE,
        string OFFICE_CITY,
        string OFFICE_STREET,
        contactMethod? PREFERRED_CONTACT_METHOD,
        string HOME_LANDLINE,
        string OFFICE_LANDLINE,
        string EMAIL,
        string CELLPHONE,
        string EDUCATIONAL_ATTAINMENT,
        DateTime? BIRTH_DATE,
        DateTime? DATE_REGISTERED,
        bool? IS_CONTACTABLE,
        bool? IS_VIABLE,
        string REASON_FOR_DEFERRAL
            )
        {
            donor_id = DONOR_ID;
            blood_type = BLOOD_TYPE;
            name = NAME;
            home_province = HOME_PROVINCE;
            home_city = HOME_CITY;
            home_street = HOME_STREET;
            office_province = OFFICE_PROVINCE;
            office_city = OFFICE_CITY;
            office_street = OFFICE_STREET;
            preferred_contact_method = PREFERRED_CONTACT_METHOD;
            home_landline = HOME_LANDLINE;
            office_landline = OFFICE_LANDLINE;
            email = EMAIL;
            cellphone = CELLPHONE;
            educational_attainment = EDUCATIONAL_ATTAINMENT;
            birth_date = BIRTH_DATE;
            date_registered = DATE_REGISTERED;

            last_donation = DateTime.MinValue;
            next_available = DateTime.MinValue;
            times_donated = 0;
            times_contacted = 0;

            is_contactable = IS_CONTACTABLE;
            is_viable = IS_VIABLE;
            reason_for_deferral = REASON_FOR_DEFERRAL;
            Refresh();
        }


        public void Refresh()
        {
            //http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
            age = DateTime.Today.Year - birth_date.Value.Year;
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
