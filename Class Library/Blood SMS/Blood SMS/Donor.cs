using System;
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
        province home_province;
        city home_city;
        string home_street;
        province office_province;
        city office_city;
        string office_street;
        string home_landline;
        string office_landline;
        string email;
        string cellphone;
        educationalAttainment educational_attainment;
        DateTime birth_date;
        DateTime date_registered;
        DateTime next_available;
        int times_contacted;
        bool is_contactable;
        bool is_viable;
        string reason_for_deferral;
        List<Blood> bloods;

        int age;
        int times_donated;

        //FROM SQL
        public Donor(int DONOR_ID, string LAST_NAME, string FIRST_NAME, string MIDDLE_INITIAL,
        int BLOOD_TYPE,
        int HOME_PROVINCE,
        int HOME_CITY,
        string HOME_STREET,
        int OFFICE_PROVINCE,
        int OFFICE_CITY,
        string OFFICE_STREET,
        string HOME_LANDLINE,
        string OFFICE_LANDLINE,
        string EMAIL,
        string CELLPHONE,
        int EDUCATIONAL_ATTAINMENT,
        DateTime BIRTH_DATE,
        DateTime DATE_REGISTERED,
        DateTime NEXT_AVAILABLE,
        int TIMES_CONTACTED,
        bool IS_CONTACTABLE,
        bool IS_VIABLE,
        string REASON_FOR_DEFERRAL
            )
        {
            donor_id = DONOR_ID;
            blood_type = (bloodType)BLOOD_TYPE;
            last_name = LAST_NAME;
            first_name = FIRST_NAME;
            middle_initial = MIDDLE_INITIAL;
            home_province = (province)HOME_PROVINCE;
            home_city = (city)HOME_CITY;
            home_street = HOME_STREET;
            office_province = (province)OFFICE_PROVINCE;
            office_city = (city)OFFICE_CITY;
            office_street = OFFICE_STREET;
            home_landline = HOME_LANDLINE;
            office_landline = OFFICE_LANDLINE;
            email = EMAIL;
            cellphone = CELLPHONE;
            educational_attainment = (educationalAttainment)EDUCATIONAL_ATTAINMENT;
            birth_date = BIRTH_DATE;
            date_registered = DATE_REGISTERED;
            next_available = NEXT_AVAILABLE;
            times_contacted = TIMES_CONTACTED;
            is_contactable = IS_CONTACTABLE;
            is_viable = IS_VIABLE;
            reason_for_deferral = REASON_FOR_DEFERRAL;
            Refresh();

            bloods = new List<Blood>();
        }

        public void Refresh()
        {
            //http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
            age = DateTime.Today.Year - birth_date.Year;
            if (birth_date > DateTime.Today.AddYears(-age)) age--;
        }

        public void AddBlood(Blood b)
        {
            bloods.Add(b);
        }

        public void RemoveBlood(Blood b)
        {
            bloods.Remove(b);
        }

        public void SendEmail(string subject, string body)
        {
            Email mail = new Email(email, subject, body);
        }

        public int Donor_id { get { return donor_id; } set { donor_id = value; } }
        public string Last_name { get { return last_name; } set { last_name = value; } }
        public string First_name { get { return first_name; } set { first_name = value; } }
        public string Middle_initial { get { return middle_initial; } set { middle_initial = value; } }
        public bloodType Blood_type { get { return blood_type; } set { blood_type = value; } }
        public province Home_province { get { return home_province; } set { home_province = value; } }
        public city Home_city { get { return home_city; } set { home_city = value; } }
        public string Home_street { get { return home_street; } set { home_street = value; } }
        public province Office_province { get { return office_province; } set { office_province = value; } }
        public city Office_city { get { return office_city; } set { office_city = value; } }
        public string Office_street { get { return office_street; } set { office_street = value; } }
        public string Home_landline { get { return home_landline; } set { home_landline = value; } }
        public string Office_landline { get { return office_landline; } set { office_landline = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Cellphone { get { return cellphone; } set { cellphone = value; } }
        public educationalAttainment Educational_attainment { get { return educational_attainment; } set { educational_attainment = value; } }
        public DateTime Birth_date { get { return birth_date; } set { birth_date = value; } }
        public DateTime Date_registered { get { return date_registered; } set { date_registered = value; } }
        public DateTime Next_available { get { return next_available; } set { next_available = value; } }
        public int Times_contacted { get { return times_contacted; } set { times_contacted = value; } }
        public bool Is_contactable { get { return is_contactable; } set { is_contactable = value; } }
        public bool Is_viable { get { return is_viable; } set { is_viable = value; } }
        public string Reason_for_deferral { get { return reason_for_deferral; } set { reason_for_deferral = value; } }

        public int Age { get { return age; } set { age = value; } }
        public string Name { get { return last_name + ", " + first_name + " " + middle_initial; } }
        public List<Blood> Bloods { get { return bloods; } }
        public int Times_donated { get { return times_donated; } set { times_donated = value; } }

    }
}
