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

        public Donor(string NAME,
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

        public int? Donor_id { get { return donor_id; } set { donor_id = value; } }
        public string Name { get { return name; } set { name = value; } }
        public bloodType? Blood_type { get { return blood_type; } set { blood_type = value; } }
        public string Home_province { get { return home_province; } set { home_province = value; } }
        public string Home_city { get { return home_city; } set { home_city = value; } }
        public string Home_street { get { return home_street; } set { home_street = value; } }
        public string Office_province { get { return office_province; } set { office_province = value; } }
        public string Office_city { get { return office_city; } set { office_city = value; } }
        public string Office_street { get { return office_street; } set { office_street = value; } }
        public contactMethod? Preferred_contact_method { get { return preferred_contact_method; } set { preferred_contact_method = value; } }
        public string Home_landline { get { return home_landline; } set { home_landline = value; } }
        public string Office_landline { get { return office_landline; } set { office_landline = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Cellphone { get { return cellphone; } set { cellphone = value; } }
        public string Educational_attainment { get { return educational_attainment; } set { educational_attainment = value; } }
        public DateTime? Birth_date { get { return birth_date; } set { birth_date = value; } }
        public DateTime? Date_registered { get { return date_registered; } set { date_registered = value; } }
        public DateTime? Last_donation { get { return last_donation; } set { last_donation = value; } }
        public DateTime? Next_available { get { return next_available; } set { next_available = value; } }
        public int? Times_donated { get { return times_donated; } set { times_donated = value; } }
        public int? Times_contacted { get { return times_contacted; } set { times_contacted = value; } }
        public bool? Is_contactable { get { return is_contactable; } set { is_contactable = value; } }
        public bool? Is_viable { get { return is_viable; } set { is_viable = value; } }
        public string Reason_for_deferral { get { return reason_for_deferral; } set { reason_for_deferral = value; } }

        public int Age { get { return age; } set { age = value; } }
    }
}
