﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{

    public class Donor
    {
        int donor_id;
        string last_name;
        string first_name;
        string middle_initial;
        bloodType blood_type;
        string home_province;
        string home_city;
        string home_street;
        string office_province;
        string office_city;
        string office_street;
        contactMethod preferred_contact_method;
        string home_landline;
        string office_landline;
        string email;
        string cellphone;
        educationalAttainment educational_attainment;
        DateTime birth_date;
        DateTime date_registered;
        DateTime next_available;
        int times_donated;
        int times_contacted;
        bool is_contactable;
        bool is_viable;
        string reason_for_deferral;

        int age;
        string name;

        //FROM SQL
        public Donor(int? DONOR_ID, string LAST_NAME, string FIRST_NAME, string MIDDLE_INITIAL,
        int? BLOOD_TYPE,
        string HOME_PROVINCE,
        string HOME_CITY,
        string HOME_STREET,
        string OFFICE_PROVINCE,
        string OFFICE_CITY,
        string OFFICE_STREET,
        int? PREFERRED_CONTACT_METHOD,
        string HOME_LANDLINE,
        string OFFICE_LANDLINE,
        string EMAIL,
        string CELLPHONE,
        int? EDUCATIONAL_ATTAINMENT,
        DateTime? BIRTH_DATE,
        DateTime? DATE_REGISTERED,
        DateTime? NEXT_AVAILABLE,
        int? TIMES_DONATED,
        int? TIMES_CONTACTED,
        bool? IS_CONTACTABLE,
        bool? IS_VIABLE,
        string REASON_FOR_DEFERRAL
            )
        {
            donor_id = DONOR_ID.Value;
            blood_type = (bloodType)BLOOD_TYPE.Value;
            last_name = LAST_NAME;
            first_name = FIRST_NAME;
            middle_initial = MIDDLE_INITIAL;
            home_province = HOME_PROVINCE;
            home_city = HOME_CITY;
            home_street = HOME_STREET;
            office_province = OFFICE_PROVINCE;
            office_city = OFFICE_CITY;
            office_street = OFFICE_STREET;
            preferred_contact_method = (contactMethod)PREFERRED_CONTACT_METHOD.Value;
            home_landline = HOME_LANDLINE;
            office_landline = OFFICE_LANDLINE;
            email = EMAIL;
            cellphone = CELLPHONE;
            educational_attainment = (educationalAttainment)EDUCATIONAL_ATTAINMENT.Value;
            birth_date = BIRTH_DATE.Value;
            date_registered = DATE_REGISTERED.Value;
            next_available = NEXT_AVAILABLE.Value;
            times_donated = TIMES_DONATED.Value;
            times_contacted = TIMES_CONTACTED.Value;
            is_contactable = IS_CONTACTABLE.Value;
            is_viable = IS_VIABLE.Value;
            reason_for_deferral = REASON_FOR_DEFERRAL;
            Refresh();

            name = LAST_NAME +", " + FIRST_NAME +" " + MIDDLE_INITIAL;
        }

        //NEW REGISTER
        public Donor(string LAST_NAME, string FIRST_NAME, string MIDDLE_INITIAL,
        int BLOOD_TYPE,
        string HOME_PROVINCE,
        string HOME_CITY,
        string HOME_STREET,
        string OFFICE_PROVINCE,
        string OFFICE_CITY,
        string OFFICE_STREET,
        int PREFERRED_CONTACT_METHOD,
        string HOME_LANDLINE,
        string OFFICE_LANDLINE,
        string EMAIL,
        string CELLPHONE,
        int EDUCATIONAL_ATTAINMENT,
        DateTime BIRTH_DATE,
        DateTime DATE_REGISTERED,
        bool IS_CONTACTABLE,
        bool IS_VIABLE,
        string REASON_FOR_DEFERRAL
            )
        {
            last_name = LAST_NAME;
            first_name = FIRST_NAME;
            middle_initial = MIDDLE_INITIAL;
            blood_type = (bloodType) BLOOD_TYPE;
            home_province = HOME_PROVINCE;
            home_city = HOME_CITY;
            home_street = HOME_STREET;
            office_province = OFFICE_PROVINCE;
            office_city = OFFICE_CITY;
            office_street = OFFICE_STREET;
            preferred_contact_method = (contactMethod) PREFERRED_CONTACT_METHOD;
            home_landline = HOME_LANDLINE;
            office_landline = OFFICE_LANDLINE;
            email = EMAIL;
            cellphone = CELLPHONE;
            educational_attainment = (educationalAttainment) EDUCATIONAL_ATTAINMENT;
            birth_date = BIRTH_DATE;
            date_registered = DATE_REGISTERED;

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
            age = DateTime.Today.Year - birth_date.Year;
            if (birth_date > DateTime.Today.AddYears(-age)) age--;
        }

        public int Donor_id { get { return donor_id; } set { donor_id = value; } }
        public string Last_name { get { return last_name; } set { last_name = value; } }
        public string First_name { get { return first_name; } set { first_name = value; } }
        public string Middle_initial { get { return middle_initial; } set { middle_initial = value; } }
        public bloodType Blood_type { get { return blood_type; } set { blood_type = value; } }
        public string Home_province { get { return home_province; } set { home_province = value; } }
        public string Home_city { get { return home_city; } set { home_city = value; } }
        public string Home_street { get { return home_street; } set { home_street = value; } }
        public string Office_province { get { return office_province; } set { office_province = value; } }
        public string Office_city { get { return office_city; } set { office_city = value; } }
        public string Office_street { get { return office_street; } set { office_street = value; } }
        public contactMethod Preferred_contact_method { get { return preferred_contact_method; } set { preferred_contact_method = value; } }
        public string Home_landline { get { return home_landline; } set { home_landline = value; } }
        public string Office_landline { get { return office_landline; } set { office_landline = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Cellphone { get { return cellphone; } set { cellphone = value; } }
        public educationalAttainment Educational_attainment { get { return educational_attainment; } set { educational_attainment = value; } }
        public DateTime Birth_date { get { return birth_date; } set { birth_date = value; } }
        public DateTime Date_registered { get { return date_registered; } set { date_registered = value; } }
        public DateTime Next_available { get { return next_available; } set { next_available = value; } }
        public int Times_donated { get { return times_donated; } set { times_donated = value; } }
        public int Times_contacted { get { return times_contacted; } set { times_contacted = value; } }
        public bool Is_contactable { get { return is_contactable; } set { is_contactable = value; } }
        public bool Is_viable { get { return is_viable; } set { is_viable = value; } }
        public string Reason_for_deferral { get { return reason_for_deferral; } set { reason_for_deferral = value; } }

        public int Age { get { return age; } set { age = value; } }
        public string Name {get {return name;} set {name = value;}}
    }
}
