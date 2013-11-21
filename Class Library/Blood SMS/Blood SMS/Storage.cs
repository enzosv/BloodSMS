using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Blood_SMS
{
    public enum bloodType { A, B, O };
    public enum contactMethod { home_landline, office_landline, email, cellphone };
	public enum city { QuezonCity, SanJuan, Manila, Caloocan, Mandaluyong, Malabon, Pateros, Makati, Valenzuela, Navotas, Pasay, Taguig, Paranaque, Muntinlupa, LasPinas, Other};
	/*
	 * Quezon City 4.5km
Marikina 9.6
San Juan 10.1
Manila 10.1
Caloocan 11.3km
Mandaluyong 11.6
Malabon 12.3
Pateros 12.9
Makati 13.7km
Valenzuela 14.5
Navotas 15.4
Pasay 16.4
Taguig 21.3
Paranaque 25.2
Muntinlupa 30.4km
las pinas 34.9km
*/
    public class Storage
    {
        List<Blood> bloodList;
        List<Blood>[] bloodTypes;
		List<Blood> availableBlood;
		List<Blood> quarantinedBlood;
        List<Donor> donorList;
        List<Donor> viableDonors;
		List<Donor> bannedDonors;

        string connectionString;
	
		const int MINIMUMBLOODVALUE = 20;
		const int MINIMUMEXPIRYALERTVALUE = 3;
        readonly string[] BLOOD_FIELDS = {"taken_from", "patient_name", "patient_age", "date_donated", "date_expire", "date_removed", "is_assigned", "is_quarantined", "reason_for_removal", "compoenent" };
        readonly string[] DONOR_FIELDS = {"name", "blood_type", "home_province", "home_city", "home_street", "office_province", "office_city", "office_street", "preferred_contact_method", "home_landline", "office_landline", "cellphone", "educational_attainment", "birth_date", "date_registered", "last_donation", "next_available", "times_donated", "is_contactable", "is_viable", "reason_for_deferral" };
        Storage(string host, string db, string user, string pass)
        {
			cities = new List<City>();
			GenerateCities();
            connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3}", host, db, user, pass);
            viableDonors = new List<Donor>();
            getDonorSQL();
            getBloodSQL();
            getBloodInInventory();
        }
		
        #region Donor methods

        /*
        *<summary>
        *  Gets all rows from the SQL Donor table and adds them to the donorList
        *</summary>
        */
        void getDonorSQL()
        {
            donorList = new List<Donor>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "Select * from Donor";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int? DONOR_ID = reader.GetValue(0) as int?;
                string NAME = reader.GetValue(1) as string;
                bloodType? BLOOD_TYPE = reader.GetValue(2) as bloodType?;
                string HOME_PROVINCE = reader.GetValue(3) as string;
                string HOME_CITY = reader.GetValue(4) as string;
                string HOME_STREET = reader.GetValue(5) as string;
                string OFFICE_PROVINCE = reader.GetValue(6) as string;
                string OFFICE_CITY = reader.GetValue(7) as string;
                string OFFICE_STREET = reader.GetValue(8) as string;
                contactMethod? PREFERRED_CONTACT_METHOD = reader.GetValue(9) as contactMethod?;
                string HOME_LANDLINE = reader.GetValue(10) as string;
                string OFFICE_LANDLINE = reader.GetValue(11) as string;
                string EMAIL = reader.GetValue(12) as string;
                string CELLPHONE = reader.GetValue(13) as string;
                string EDUCATIONAL_ATTAINMENT = reader.GetValue(14) as string;
                DateTime? BIRTH_DATE = reader.GetValue(15) as DateTime?;
                DateTime? DATE_REGISTERED = reader.GetValue(16) as DateTime?;
                DateTime? LAST_DONATION = reader.GetValue(17) as DateTime?;
                DateTime? NEXT_AVAILABLE = reader.GetValue(18) as DateTime?;
                int? TIMES_DONATED = reader.GetValue(19) as int?;
                int? TIMES_CONTACTED = reader.GetValue(20) as int?;
                bool? IS_CONTACTABLE = reader.GetValue(21) as bool?;
                bool? IS_VIABLE = reader.GetValue(22) as bool?;
                string REASON_FOR_DEFERRAL = reader.GetValue(23) as string;

                Donor x = new Donor(DONOR_ID,
                    NAME,
                    BLOOD_TYPE,
                    HOME_PROVINCE,
                    HOME_CITY,
                    HOME_STREET,
                    OFFICE_PROVINCE,
                    OFFICE_CITY,
                    OFFICE_STREET,
                    PREFERRED_CONTACT_METHOD,
                    HOME_LANDLINE,
                    OFFICE_LANDLINE,
                    EMAIL,
                    CELLPHONE,
                    EDUCATIONAL_ATTAINMENT,
                    BIRTH_DATE,
                    DATE_REGISTERED,
                    LAST_DONATION,
                    NEXT_AVAILABLE,
                    TIMES_DONATED,
                    TIMES_CONTACTED,
                    IS_CONTACTABLE,
                    IS_VIABLE,
                    REASON_FOR_DEFERRAL);
                donorList.Add(x);
            }
            reader.Close();
            conn.Close();
        }

        /*
        *<summary>
        *  Creates donor object from parameters, adds it to the donorList and creates row in SQL
        *</summary>
        */
        bool AddDonor(
            string NAME,
            bloodType BLOOD_TYPE,
            string HOME_PROVINCE,
            string HOME_CITY,
            string HOME_STREET,
            string OFFICE_PROVINCE,
            string OFFICE_CITY,
            string OFFICE_STREET,
            contactMethod PREFERRED_CONTACT_METHOD,
            string HOME_LANDLINE,
            string OFFICE_LANDLINE,
            string EMAIL,
            string CELLPHONE,
            string EDUCATIONAL_ATTAINMENT,
            DateTime BIRTH_DATE,
            DateTime DATE_REGISTERED,
            bool IS_CONTACTABLE,
            bool IS_VIABLE,
            string REASON_FOR_DEFERRAL
            )
        {
            Donor x = new Donor(NAME,
                BLOOD_TYPE,
                HOME_PROVINCE,
                HOME_CITY,
                HOME_STREET,
                OFFICE_PROVINCE,
                OFFICE_CITY,
                OFFICE_STREET,
                PREFERRED_CONTACT_METHOD,
                HOME_LANDLINE,
                OFFICE_LANDLINE,
                EMAIL,
                CELLPHONE,
                EDUCATIONAL_ATTAINMENT,
                BIRTH_DATE,
                DATE_REGISTERED,
                IS_CONTACTABLE,
                IS_VIABLE,
                REASON_FOR_DEFERRAL
            );
            donorList.Add(x);
            //if (x.Is_viable && x.Is_contactable)
            //    viableDonors.Add(x);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "Insert into Donor " + AddQuery(DONOR_FIELDS);
            MySqlCommand comm = new MySqlCommand(query, conn);
            donorCommands(comm, x);
            return RowsAffected(comm, conn);

        }

        /*
         *<summary>
         *  Updates the sql table with the properties found in the supplied object
         *</summary>
         *<param name="x">
         *  Donor object containing properties to be applied
         *</param>
         */
        bool UpdateDonor(int DONOR_ID, string NAME,
            bloodType BLOOD_TYPE,
            string HOME_PROVINCE,
            string HOME_CITY,
            string HOME_STREET,
            string OFFICE_PROVINCE,
            string OFFICE_CITY,
            string OFFICE_STREET,
            contactMethod PREFERRED_CONTACT_METHOD,
            string HOME_LANDLINE,
            string OFFICE_LANDLINE,
            string EMAIL,
            string CELLPHONE,
            string EDUCATIONAL_ATTAINMENT,
            DateTime BIRTH_DATE,
            DateTime DATE_REGISTERED,
            DateTime LAST_DONATION,
            DateTime NEXT_AVAILABLE,
            int TIMES_DONATED,
            int TIMES_CONTACTED,
            bool IS_CONTACTABLE,
            bool IS_VIABLE,
            string REASON_FOR_DEFERRAL)
        {
            donorList.Remove(findDonor(DONOR_ID));
            Donor x = new Donor(DONOR_ID,
                    NAME,
                    BLOOD_TYPE,
                    HOME_PROVINCE,
                    HOME_CITY,
                    HOME_STREET,
                    OFFICE_PROVINCE,
                    OFFICE_CITY,
                    OFFICE_STREET,
                    PREFERRED_CONTACT_METHOD,
                    HOME_LANDLINE,
                    OFFICE_LANDLINE,
                    EMAIL,
                    CELLPHONE,
                    EDUCATIONAL_ATTAINMENT,
                    BIRTH_DATE,
                    DATE_REGISTERED,
                    LAST_DONATION,
                    NEXT_AVAILABLE,
                    TIMES_DONATED,
                    TIMES_CONTACTED,
                    IS_CONTACTABLE,
                    IS_VIABLE,
                    REASON_FOR_DEFERRAL);
            donorList.Add(x);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "UPDATE Donor SET " + UpdateQuery(DONOR_FIELDS);
            MySqlCommand comm = new MySqlCommand(query, conn);
            donorCommands(comm, x);
            return RowsAffected(comm, conn);
        }

        /*
         *<summary>
         *  Iterates through donorList and returns the donor object with the same id as provided in the parameter
         *</summary>
         *<param name="">
         * 
         *</param>
         */
        Donor findDonor(int id)
        {
            foreach (Donor d in donorList)
            {
                if (d.Donor_id == id)
                {
                    return d;
                }
            }
            return null;
        }

        List<Donor> findDonorsWithBloodType(bloodType blood_type)
        {
            List<Donor> typeDonors = new List<Donor>();
            foreach (Donor d in viableDonors)
            {
                if (d.Blood_type == blood_type)
                {
                    typeDonors.Add(d);
                }
            }
            return typeDonors;
        }

        /*
         *<summary>
         *  Iterates through all donor objects in bloodList
         *  and checks if any item matches id provided in parameter
         *</summary>
         *<param name="id">
         *  an integer from the objects identifier
         *</param>
         */
        bool isDonorUnique(int id)
        {
            foreach (Donor d in donorList)
            {
                if (d.Donor_id == id)
                    return false;
            }
            return true;
        }

        bool DeleteDonorWithId(int id)
        {
            donorList.Remove(findDonor(id));
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "DELETE FROM Donor WHERE donor_id =@donor_id";
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.Parameters.AddWithValue("@donor_id", id);
            return RowsAffected(comm, conn);
        }

        void donorCommands(MySqlCommand comm, Donor x)
        {
            long id = comm.LastInsertedId;
            x.Donor_id = (int)id;
            comm.Parameters.AddWithValue("@donor_id", x.Donor_id);

            comm.Parameters.AddWithValue("@name", x.Name);
            comm.Parameters.AddWithValue("@blood_type", x.Blood_type);
            comm.Parameters.AddWithValue("@home_province", x.Home_province);
            comm.Parameters.AddWithValue("@home_city", x.Home_city);
            comm.Parameters.AddWithValue("@home_street", x.Home_street);
            comm.Parameters.AddWithValue("@office_province", x.Office_province);
            comm.Parameters.AddWithValue("@office_city", x.Office_city);
            comm.Parameters.AddWithValue("@office_street", x.Office_street);
            comm.Parameters.AddWithValue("@preferred_contact_method", x.Preferred_contact_method);
            comm.Parameters.AddWithValue("@home_landline", x.Home_landline);
            comm.Parameters.AddWithValue("@office_landline", x.Office_landline);
            comm.Parameters.AddWithValue("@email", x.Email);
            comm.Parameters.AddWithValue("@cellphone", x.Cellphone);
            comm.Parameters.AddWithValue("@educational_attainment", x.Educational_attainment);
            comm.Parameters.AddWithValue("@birth_date", x.Birth_date);
            comm.Parameters.AddWithValue("@date_registered", x.Date_registered);
            comm.Parameters.AddWithValue("@last_donation", x.Last_donation);
            comm.Parameters.AddWithValue("@next_available", x.Next_available);
            comm.Parameters.AddWithValue("@times_donated", x.Times_donated);
            comm.Parameters.AddWithValue("@times_contacted", x.Times_contacted);
            comm.Parameters.AddWithValue("@is_contactable", x.Is_contactable);
            comm.Parameters.AddWithValue("@is_viable", x.Is_viable);
            comm.Parameters.AddWithValue("@reason_for_deferral", x.Reason_for_deferral);
        }

        /*
         *<summary>
         *  Creates a list of donors and populates it with viable donors
         *</summary>
         */
        void getViableDonors()
        {
            foreach (Donor d in donorList)
            {
                if (d.Is_viable && d.Is_contactable)
                    viableDonors.Add(d);
            }
        }
		
		void sortByDistance()
		{
			int previousDistance = 0;
			foreach(Donor d in viableDonors)
			{
				
			}
		}
        #endregion

        #region Blood methods

        /*
        *<summary>
        *  Gets all rows from the SQL blood table and adds them to the bloodList
        *</summary>
        */
        void getBloodSQL()
        {
            bloodList = new List<Blood>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "Select * from Blood";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int? blood_id = reader.GetValue(0) as int?;
                int? taken_from = reader.GetValue(1) as int?;
                string patient_name = reader.GetValue(2) as string;
                int? patient_age = reader.GetValue(3) as int?;
                DateTime? date_donated = reader.GetValue(4) as DateTime?;
                DateTime? date_expire = reader.GetValue(5) as DateTime?;
                DateTime? date_removed = reader.GetValue(6) as DateTime?;
                bool? is_assigned = reader.GetValue(7) as bool?;
                bool? is_quarantined = reader.GetValue(8) as bool?;
                string reason_for_removal = reader.GetValue(9) as string;
                string component = reader.GetValue(10) as string;

                Blood x = new Blood(blood_id, taken_from, date_donated, date_expire, component, patient_name, patient_age, date_removed, is_assigned, is_quarantined, reason_for_removal);
                
                SortBlood(x);
            }
            reader.Close();
            conn.Close();

        }

        /*
        *<summary>
        *  Creates blood object from parameters, adds it to the bloodList and creates row in SQL
        *</summary>
        */
        bool AddBlood(DateTime date_added, DateTime date_expire, int taken_from)
        {
            Blood x = new Blood(bloodList.Count, taken_from, date_added, date_expire);
            SortBlood(x);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "Insert into Blood " + AddQuery(BLOOD_FIELDS);
            MySqlCommand comm = new MySqlCommand(query, conn);
            bloodCommands(comm, x);
            return RowsAffected(comm, conn);
        }

        /*summary
         * for extracted blood
         */
        bool AddBlood(Blood a, DateTime date_added, DateTime date_expire, string component)
        {
            Blood x = new Blood(a.Blood_id, bloodList.Count, date_added, date_expire, component);
            SortBlood(x);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "Insert into Blood " + AddQuery(BLOOD_FIELDS);
            MySqlCommand comm = new MySqlCommand(query, conn);
            bloodCommands(comm, x);
            return RowsAffected(comm, conn);
        }

        /*
       *<summary>
       *  Updates the sql table with the properties found in the supplied object
       *</summary>
       *<param name="x">
       *  Blood object containing properties to be applied
       *</param>
       */
        bool UpdateBlood(int blood_id, int taken_from, DateTime date_donated, DateTime date_expire, string component, string patient_name, int patient_age, DateTime date_removed, bool is_assigned, bool is_quarantined, string reason_for_removal)
        {
            removeBloodFromSort(findBlood(blood_id));
            Blood x = new Blood(blood_id, taken_from, date_donated, date_expire, component, patient_name, patient_age, date_removed, is_assigned, is_quarantined, reason_for_removal);
            SortBlood(x);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "UPDATE Blood SET " + UpdateQuery(BLOOD_FIELDS);
            MySqlCommand comm = new MySqlCommand(query, conn);
            bloodCommands(comm, x);
            return RowsAffected(comm, conn);
        }

        int getBloodRemovedOn(DateTime date)
        {
            int count = 0;
            foreach (Blood b in bloodList)
            {
                if (b.Is_removed)
                {
                    if (b.Date_removed == date)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        int getBloodReleasedOn(DateTime date)
        {
            int count = 0;
            foreach (Blood b in bloodList)
            {
                if (b.Is_assigned && b.Is_removed)
                {
                    if (b.Date_removed == date)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        int getBloodQuarantinedOn(DateTime date)
        {
            int count = 0;
            foreach (Blood b in bloodList)
            {
                if (b.Is_quarantined && b.Is_removed)
                {
                    if (b.Date_removed == date)
                        count++;
                }
            }
            return count;
        }

        int getWholeBloodAddedOn(DateTime date)
        {
            int count = 0;
            foreach (Blood b in bloodList)
            {
                if (b.Component == "Whole" && b.Date_added == date)
                {
                    count++;
                }
            }
            return count;
        }

        void ExtractBlood(Blood x, DateTime date_added, DateTime date_expire)
        {
            x.Extract(date_added);
            AddBlood(x, date_added, date_expire, "Fresh Frozen Plasma");
            AddBlood(x, date_added, date_expire, "Packed Red Cells");
        }

        void SortBlood(Blood b)
        {
			if (!b.Is_removed)
            {
	            availableBlood.Add(b);
	            if (b.Component == "Whole")
	                bloodTypes[(int)findDonor(b.Taken_from).Blood_type].Add(b);
	            else
	                bloodTypes[(int)findDonor(findBlood(b.Taken_from).Taken_from).Blood_type].Add(b);
			}
			else
				quarantinedBlood.Add(b);
        }

        void bloodCommands(MySqlCommand comm, Blood x)
        {
            long id = comm.LastInsertedId;
            x.Blood_id = (int)id;
            comm.Parameters.AddWithValue("@blood_id", x.Blood_id);

            comm.Parameters.AddWithValue("@taken_from", x.Taken_from);
            comm.Parameters.AddWithValue("@patient_name", x.Patient_name);
            comm.Parameters.AddWithValue("@patient_age", x.Patient_age);
            comm.Parameters.AddWithValue("@date_added", x.Date_added);
            comm.Parameters.AddWithValue("@date_expire", x.Date_expire);
            comm.Parameters.AddWithValue("@date_removed", x.Date_removed);
            comm.Parameters.AddWithValue("@is_assigned", x.Is_assigned);
            comm.Parameters.AddWithValue("@is_quarantined", x.Is_quarantined);
            comm.Parameters.AddWithValue("@reason_for_removal", x.Reason_for_removal);

        }

        void getBloodInInventory()
        {
            bloodTypes = new List<Blood>[Enum.GetNames(typeof(bloodType)).Length];
            foreach (Blood b in bloodList)
            {
            	SortBlood(b);
            }
        }

        bool isBloodUnique(int id)
        {
            foreach (Blood b in bloodList)
            {
                if (b.Blood_id == id)
                    return false;
            }
            return true;
        }

        bool DeleteBloodWithId(int id)
        {
            removeBloodFromSort(findBlood(id));
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "DELETE FROM Blood WHERE blood_id =@blood_id";
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.Parameters.AddWithValue("@blood_id", id);
            return RowsAffected(comm, conn);
        }

        void removeBloodFromSort(Blood b)
        {
            foreach (List<Blood> bType in bloodTypes)
            {
                if (bType.Contains(b))
                {
                    bType.Remove(b);
                    break;
                }
            }
            bloodList.Remove(b);
        }

        /*
         *<summary>
         *  Iterates through bloodList and returns the blood object with the same id as provided in the parameter
         *</summary>
         *<param name="">
         * 
         *</param>
         */
        Blood findBlood(int id)
        {
            foreach (Blood b in bloodList)
            {
                if (b.Blood_id == id)
                {
                    return b;
                }
            }
            return null;
        }
        #endregion

        #region Utility Methods
        /*
         *<summary>
         *  returns true if rows have been affected
         *</summary>
         *<param name="comm">
         *  MySQLCommand where number of affected rows will be determined from
         *</param>
         */
        bool RowsAffected(MySqlCommand comm, MySqlConnection conn)
        {
            if (comm.ExecuteNonQuery() > 0)
            {
                conn.Close();
                return true;
            }
            conn.Close();
            return false;
        }

        /*
         *<summary>
         *  returns a string which assigns the values to be updated
         *</summary>
         *<param name="fields">
         *  string array containing all fields for an SQL table
         *</param>
         */
        string UpdateQuery(string[] fields)
        {
            string valueParameters = "";
            for (int i = 1; i < fields.Length; i++)
            {
                valueParameters += fields[i] + "=@" + fields[i] + ", ";
            }
            valueParameters += "Where " + fields[0] + "=@" + fields[0];
            return valueParameters;
        }

        /*
         *<summary>
         *  returns a string which assigns the values to be populated
         *</summary>
         *<param name="fields">
         *  string array containing all fields for an SQL table
         *</param>
         */
        string AddQuery(string[] fields)
        {
            string fieldNames = "(";
            string valueParameters = "";
            for (int i = 0; i < fields.Length; i++)
            {
                fieldNames += fields[i];
                valueParameters += "@" + fields[i];
                if (i < fields.Length - 1)
                {
                    fieldNames += ", ";
                    valueParameters += ", ";
                }
            }
            return fieldNames + ") Values(" + valueParameters + ")";
        }
        /*
         *<summary>
         *  iterates through all fields provided in the parameter and adds the value to the sql from the object's properties
         *</summary>
         *<param name="comm">
         *  MySqlCommand object where parameters and values will be passed
         *</param>
         *<param name="x">
         *  Object where properties will be taken from
         *</param>
         *<param name="fields">
         *  string array containing all fields for an SQL table
         *</param>
         
        void AddValue(MySqlCommand comm, Object x, string[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                comm.Parameters.AddWithValue("@" + fields[i], GetPropValue(x, Capitalize(fields[i])));
            }
        }

        /*
         *<summary>
         *  returns a property of an object depending on the provided string
         *</summary>
         *<param name="src">
         *  the object where the property will come from
         *</param>
         *<param name="propName">
         *  the string which contains the name of the property to be used
         *</param>

        //http://stackoverflow.com/questions/1196991/get-property-value-from-string-using-reflection-in-c-sharp
        object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        /*
         *<summary>
         *  Capitalizes the firs letter of the word provided in the parameter
         *</summary>
         *<param name="s">
         *  A one word string in lowercase
         *</param>
         
        //http://stackoverflow.com/questions/4135317/make-first-letter-of-a-string-upper-case
        string Capitalize(string s)
        {
            return s.First().ToString().ToUpper() + String.Join("", s.Skip(1));
        }
        */
		
		/*void AlertLowLevel(bloodType blood_type)
		{
			foreach(Blood b in bloodTypes[(int)blood_type])
			{
				
			}
		}*/
		
		bool AlertLowLevel(bloodType blood_type)
		{
			if(bloodTypes[(int)blood_type] < MINIMUMBLOODVALUE)
				return true;
			return false;
		}
		
		bool AlertNearExpiration(Blood b)
		{
			if(DateTime.Now - b.Date_expire < MINIMUMEXPIRYALERTVALUE)
				return true;
			return false;
		}
		
		void CheckExpirations()
		{
			foreach(Blood b in availableBlood)
			{
				AlertNearExpiration(b);
			}
		}
		
		void CheckLowLevel()
		{
			foreach (bloodType blood_type in (bloodType[]) Enum.GetValues(typeof(bloodType)))
			{
				AlertLowLevel(blood_type);
			}
		}
        #endregion
 
    }
}

