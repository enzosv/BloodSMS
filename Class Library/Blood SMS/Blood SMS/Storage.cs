using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Blood_SMS
{
    enum bloodType { A, B, O };
    public class Storage
    {
        List<Blood> bloodList;
        List<Blood>[] bloodTypes;
        List<Donor> donorList;

        string connectionString;
        //const string BLOOD_FIELDS = "blood_id,donor_id,patient_name,patient_age,date_donated,date_expire,date_removed,is_assigned,is_quarantined,reason_for_removal,component";
        //const string DONOR_FIELDS = "";

        readonly string[] BLOOD_FIELDS = { "blood_id", "taken_from", "patient_name", "patient_age", "date_donated", "date_expire", "date_removed", "is_assigned", "is_quarantined", "reason_for_removal", "compoenent" };
        readonly string[] DONOR_FIELDS = {};
        Storage(string host, string db, string user, string pass)
        {
            connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3}", host, db, user, pass);
            getDonorSQL();
            getBloodSQL();
            getBloodInInventory();
            getViableDonors();
        }

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

            }
            reader.Close();
            conn.Close();
        }

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
                int blood_id = reader.GetInt32(0);
                int taken_from = reader.GetInt32(1);
                string patient_name = reader.GetString(2);
                int patient_age = reader.GetInt32(3);
                DateTime date_donated = reader.GetDateTime(4);
                DateTime date_expire = reader.GetDateTime(5);
                DateTime date_removed = DateTime.MinValue;
                if (reader.GetValue(6) != null)
                    date_removed = reader.GetDateTime(6);
                bool is_assigned = reader.GetBoolean(7);
                bool is_quarantined = reader.GetBoolean(8);
                string reason_for_removal = reader.GetString(9);
                string component = reader.GetString(10);

                Blood x = new Blood(blood_id, taken_from, date_donated, date_expire, component, patient_name, patient_age, date_removed, is_assigned, is_quarantined, reason_for_removal);

                //problem with this is that ids might change
                x.Blood_id = bloodList.Count;
                bloodList.Add(x);
                
                // or this? 
                // problem with this is that some might be null
                //bloodList[blood_id] = x;

                //but they should be the same because deletes are not allowed
            }
            reader.Close();
            conn.Close();

        }
        /*
        *<summary>
        *  Creates donor object from parameters, adds it to the donorList and creates row in SQL
        *</summary>
        */
        bool AddDonor(int donor_id, bloodType blood_type, string name, string street, string city, string province, string email, string cellphone, string reason_for_deferral, DateTime date_registered, DateTime next_available, DateTime birth_date, bool is_viable, bool is_contactable, bool is_voluntary)
        {
            Donor x = new Donor(donorList.Count, blood_type, name, street, city, province, email, cellphone, reason_for_deferral, date_registered, next_available, birth_date, is_viable, is_contactable, is_voluntary);
            donorList.Add(x);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "Insert into Donor " + AddQuery(DONOR_FIELDS);
            MySqlCommand comm = new MySqlCommand(query, conn);
            AddValue(comm, x, DONOR_FIELDS);
            return RowsAffected(comm);
        }

        /*
        *<summary>
        *  Creates blood object from parameters, adds it to the bloodList and creates row in SQL
        *</summary>
        */
        bool AddBlood(DateTime date_added, DateTime date_expire, int taken_from)
        {
            Blood x = new Blood(bloodList.Count, taken_from, date_added, date_expire);
            bloodList.Add(x);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "Insert into Blood " + AddQuery(BLOOD_FIELDS);
                //"donor_id, patient_name, patient_age, date_donated, date_expire, date_removed, is_assigned, age, is_quarantined, reason_for_removal, component) " +
                //"Values(@donor_id, ?b, ?c, ?d, ?e, ?f, ?g, ?h, ?i, ?j, ?k)";
            MySqlCommand comm = new MySqlCommand(query, conn);
            AddValue(comm, x, BLOOD_FIELDS);

            /*
             * comm.Parameters.AddWithValue("@donor_id", x.Donor_id);
            comm.Parameters.AddWithValue("?b", x.Patient_name);
            comm.Parameters.AddWithValue("?c", x.Patient_age);
            comm.Parameters.AddWithValue("?d", x.Date_donated);
            comm.Parameters.AddWithValue("?e", x.Date_expire);
            comm.Parameters.AddWithValue("?f", x.Date_removed);
            comm.Parameters.AddWithValue("?g", x.Is_assigned);
            comm.Parameters.AddWithValue("?h", x.Age);
            comm.Parameters.AddWithValue("?i", x.Is_quarantined);
            comm.Parameters.AddWithValue("?j", x.Reason_for_removal);
            comm.Parameters.AddWithValue("?k", x.Component);
             */
            return RowsAffected(comm);
        }
		
		void ExtractBlood(Blood x, DateTime date_added, DateTime date_expire, string component)
		{
			x.Extract();
		}
		
		bool AddBlood(Blood a, DateTime date_added, DateTime date_expire, string component)
		{
			Blood x = new Blood(a, bloodList.Count, date_added, date_expire, component);
            bloodList.Add(x);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "Insert into Blood " + AddQuery(BLOOD_FIELDS);
			MySqlCommand comm = new MySqlCommand(query, conn);
            AddValue(comm, x, BLOOD_FIELDS);
			return RowsAffected(comm);
		}

        /*
        *<summary>
        *  Updates the sql table with the properties found in the supplied object
        *</summary>
        *<param name="x">
        *  Blood object containing properties to be applied
        *</param>
        */
        bool UpdateBlood(Blood x)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "UPDATE Blood SET " + UpdateQuery(BLOOD_FIELDS);
            MySqlCommand comm = new MySqlCommand(query, conn);
            AddValue(comm, x, BLOOD_FIELDS);
            return RowsAffected(comm);
        }

        /*
         *<summary>
         *  Updates the sql table with the properties found in the supplied object
         *</summary>
         *<param name="x">
         *  Donor object containing properties to be applied
         *</param>
         */
        bool UpdateDonor(Donor x)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "UPDATE Donor SET " + UpdateQuery(DONOR_FIELDS);
            MySqlCommand comm = new MySqlCommand(query, conn);
            AddValue(comm, x, DONOR_FIELDS);
            return RowsAffected(comm);
        }

        #region Utility Methods

        /*
         *<summary>
         *  returns true if rows have been affected
         *</summary>
         *<param name="comm">
         *  MySQLCommand where number of affected rows will be determined from
         *</param>
         */
        bool RowsAffected (MySqlCommand comm){
            if (comm.ExecuteNonQuery() > 0)
            {
                return true;
            }
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
         */
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
         */
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
         */
        //http://stackoverflow.com/questions/4135317/make-first-letter-of-a-string-upper-case
        string Capitalize(string s)
        {
            return s.First().ToString().ToUpper() + String.Join("", s.Skip(1));
        }
        #endregion

        /*
         *<summary>
         *  creates and populates blood types by iterating through each blood object in blood list and filtering them based on their blood types
         *</summary>
         */
        void getBloodInInventory()
        {
            bloodTypes = new List<Blood>[Enum.GetNames(typeof(bloodType)).Length];
            foreach (Blood b in bloodList)
            {
                if (b.Date_removed == null)
                {
					if(b.Component != "Whole")
                    	bloodTypes[(int)findDonor(b.Taken_from).blood_type].Add(b);
					else
						bloodType[(int)findDonor(findBlood(b.Taken_from).Taken_from).blood_type].Add(b);
                }
            }
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

        /*
         *<summary>
         *  Iterates through all blood objects in bloodList
         *  and checks if any item matches id provided in parameter
         *</summary>
         *<param name="id">
         *  an integer from the objects identifier
         *</param>
         */
        bool isBloodUnique(int id)
        {
            foreach (Blood b in bloodList)
            {
                if (b.Blood_id == id)
                    return false;
            }
            return true;
        }

        /*
         *<summary>
         *  Creates a list of donors and populates it with viable donors
         *</summary>
         */
        List<Donor> getViableDonors()
        {
            List<Donor> viableDonors = new List<Donor>();
            foreach (Donor d in donorList)
            {
                if (d.Is_viable && d.Is_voluntary && d.Is_contactable)
                    viableDonors.Add(d);
            }
            return viableDonors;
        }
		
		
    }
}