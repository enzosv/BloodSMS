using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using MySql.Data.MySqlClient;

namespace Blood_SMS
{

    public class Storage
    {
        public List<Blood> bloodList;
        public List<Blood>[] bloodTypes;
        public List<Blood> availableBlood;
        public List<Blood> unavailableBlood;
        public List<Donor> donorList;
        public List<Donor>[] donorTypes;
        public List<Donor> contactableDonors;
        public List<Donor> viableDonors;
        public List<Donor> bannedDonors;

        //public List<Blood> BloodList { get; set; }
        //public List<Blood>[] BloodTypes { get; set; }
        //public List<Blood> AvailableBlood { get { return availableBlood; } set; }
        //public List<Blood> QuarantinedBlood { get; set; }
        //public List<Blood> UsedBlood { get; set; }
        //public List<Donor> DonorList { get; set; }
        //public List<Donor>[] DonorTypes { get; set; }
        //public List<Donor> ViableDonors { get; set; }
        //public List<Donor> BannedDonors { get; set; }

        string connectionString;

        const int MINIMUMBLOODVALUE = 5;
        const int MINIMUMEXPIRYALERTVALUE = 3;
        readonly string[] BLOOD_FIELDS = { "accession_number", "blood_type", "donor_id", "date_donated", "date_removed" };
        readonly string[] DONOR_FIELDS = { "last_name", "first_name", "middle_initial", "blood_type", "home_province", "home_city", "home_street", "office_province", "office_city", "office_street", "home_landline", "office_landline", "email", "cellphone", "educational_attainment", "birth_date", "date_registered", "next_available", "is_contactable", "is_viable", "reason_for_deferral" };
        readonly string[] COMPONENT_FIELDS = { "accession_number", "component_name", "removal_type", "date_processed", "date_expired", "date_assigned", "date_removed", "patient_last_name", "patient_first_name", "patient_middle_initial", "patient_age", "reason_for_removal" };
        int BlOODTYPECOUNT = Enum.GetNames(typeof(bloodType)).Length;

        public Storage(string host, string db, string user, string pass)
        {
            connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3}", host, db, user, pass);

            bloodList = new List<Blood>();
            bloodTypes = new List<Blood>[BlOODTYPECOUNT];
            availableBlood = new List<Blood>();
            unavailableBlood = new List<Blood>();
            donorList = new List<Donor>();
            donorTypes = new List<Donor>[BlOODTYPECOUNT];
            for (int i = 0; i < BlOODTYPECOUNT; i++)
            {
                bloodTypes[i] = new List<Blood>();
                donorTypes[i] = new List<Donor>();
            }
            contactableDonors = new List<Donor>();
            viableDonors = new List<Donor>();
            bannedDonors = new List<Donor>();

            getDonorSQL();
            getBloodSQL();
            getComponentSQL();
        }

        #region Donor methods

        /*
        *<summary>
        *  Gets all rows from the SQL Donor table and adds them to the donorList
        *</summary>
        */
        void getDonorSQL()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "Select * from donor";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int? DONOR_ID = reader.GetValue(0) as int?;
                string LAST_NAME = reader.GetValue(1) as string;
                string FIRST_NAME = reader.GetValue(2) as string;
                string MIDDLE_INITIAL = reader.GetValue(3) as string;
                int? BLOOD_TYPE = reader.GetValue(4) as int?;
                int? HOME_PROVINCE = reader.GetValue(5) as int?;
                int? HOME_CITY = reader.GetValue(6) as int?;
                string HOME_STREET = reader.GetValue(7) as string;
                int? OFFICE_PROVINCE = reader.GetValue(8) as int?;
                int? OFFICE_CITY = reader.GetValue(9) as int?;
                string OFFICE_STREET = reader.GetValue(10) as string;
                string HOME_LANDLINE = reader.GetValue(11) as string;
                string OFFICE_LANDLINE = reader.GetValue(12) as string;
                string EMAIL = reader.GetValue(13) as string;
                string CELLPHONE = reader.GetValue(14) as string;
                int? EDUCATIONAL_ATTAINMENT = reader.GetValue(15) as int?;
                DateTime? BIRTH_DATE = reader.GetValue(16) as DateTime?;
                DateTime? DATE_REGISTERED = reader.GetValue(17) as DateTime?;
                DateTime? NEXT_AVAILABLE = reader.GetValue(18) as DateTime?;
                bool? IS_CONTACTABLE = reader.GetValue(19) as bool?;
                bool? IS_VIABLE = reader.GetValue(20) as bool?;
                string REASON_FOR_DEFERRAL = reader.GetValue(21) as string;

                Donor x = new Donor(DONOR_ID.Value, LAST_NAME, FIRST_NAME, MIDDLE_INITIAL,
                    BLOOD_TYPE.Value,
                    HOME_PROVINCE.Value,
                    HOME_CITY.Value,
                    HOME_STREET,
                    OFFICE_PROVINCE.Value,
                    OFFICE_CITY.Value,
                    OFFICE_STREET,
                    HOME_LANDLINE,
                    OFFICE_LANDLINE,
                    EMAIL,
                    CELLPHONE,
                    EDUCATIONAL_ATTAINMENT.Value,
                    BIRTH_DATE.Value,
                    DATE_REGISTERED.Value,
                    NEXT_AVAILABLE.Value,
                    IS_CONTACTABLE.Value,
                    IS_VIABLE.Value,
                    REASON_FOR_DEFERRAL);
                sortDonor(x);
            }
            reader.Close();
            conn.Close();
        }

        /*
        *<summary>
        *  Creates donor object from parameters, adds it to the donorList and creates row in SQL
        *</summary>
        */
        //NEW REGISTRANT
        public bool AddDonor(Donor x)
        {
            if (donorCommands("Insert into donor " + AddQuery(DONOR_FIELDS), x))
            {
                sortDonor(x);
                return true;
            }
            return false;
        }

        public bool UpdateDonor(Donor d)
        {
            if (donorCommands("UPDATE donor SET " + UpdateQuery(DONOR_FIELDS, new string[] { "donor_id" }), d))
            {
                unsortDonor(findDonor(d.Donor_id));
                sortDonor(d);
                return true;
            }
            return false;
        }

        bool donorCommands(string query, Donor x)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.CommandType = CommandType.Text;

            comm.Parameters.AddWithValue("@last_name", x.Last_name);
            comm.Parameters.AddWithValue("@first_name", x.First_name);
            comm.Parameters.AddWithValue("@middle_initial", x.Middle_initial);
            comm.Parameters.AddWithValue("@blood_type", x.Blood_type);
            comm.Parameters.AddWithValue("@home_province", x.Home_province);
            comm.Parameters.AddWithValue("@home_city", x.Home_city);
            comm.Parameters.AddWithValue("@home_street", x.Home_street);
            comm.Parameters.AddWithValue("@office_province", x.Office_province);
            comm.Parameters.AddWithValue("@office_city", x.Office_city);
            comm.Parameters.AddWithValue("@office_street", x.Office_street);
            comm.Parameters.AddWithValue("@home_landline", x.Home_landline);
            comm.Parameters.AddWithValue("@office_landline", x.Office_landline);
            comm.Parameters.AddWithValue("@email", x.Email);
            comm.Parameters.AddWithValue("@cellphone", x.Cellphone);
            comm.Parameters.AddWithValue("@educational_attainment", x.Educational_attainment);
            comm.Parameters.AddWithValue("@birth_date", x.Birth_date);
            comm.Parameters.AddWithValue("@date_registered", x.Date_registered);
            comm.Parameters.AddWithValue("@next_available", x.Next_available);
            comm.Parameters.AddWithValue("@is_contactable", x.Is_contactable);
            comm.Parameters.AddWithValue("@is_viable", x.Is_viable);
            comm.Parameters.AddWithValue("@reason_for_deferral", x.Reason_for_deferral);

            if (query.Contains("UPDATE") || query.Contains("DELETE"))
            {
                comm.Parameters.AddWithValue("@donor_id", x.Donor_id);
            }
            conn.Open();
            int rowsAffected = comm.ExecuteNonQuery();
            long id;

            conn.Close();
            if (query.Contains("Insert"))
            {
                id = comm.LastInsertedId;
                x.Donor_id = (int)id;
            }
            return (rowsAffected > 0);
        }

        public bool DeleteDonorWithId(int id)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            string query = "DELETE FROM Donor WHERE donor_id =@donor_id";
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.Parameters.AddWithValue("@donor_id", id);
            conn.Open();
            int rowsAffected = comm.ExecuteNonQuery();
            conn.Close();
            if (rowsAffected > 0)
            {
                unsortDonor(findDonor(id));
                return true;
            }
            return false;
        }

        /*
         *<summary>
         *  Iterates through donorList and returns the donor object with the same id as provided in the parameter
         *</summary>
         *<param name="">
         * 
         *</param>
         */
        public Donor findDonor(int id)
        {
            foreach (Donor d in donorList)
            {
                if (d.Donor_id == id)
                    return d;
            }
            return null;
        }

        public Donor findDonorWithName(string lName, string fName, string mInitial)
        {
            foreach (Donor d in donorList)
            {
                if (d.Last_name == lName && d.First_name == fName && d.Middle_initial == mInitial)
                    return d;
            }
            return null;
        }

        public Donor findDonorWithName(string name)
        {
            foreach (Donor d in donorList)
            {
                if (d.Name == name)
                    return d;
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

        void sortDonor(Donor d)
        {
            donorList.Add(d);
            if (d.Is_viable)
            {
                viableDonors.Add(d);
                if (d.Is_contactable)
                {
                    contactableDonors.Add(d);
                    donorTypes[(int)d.Blood_type].Add(d);
                }
            }
            else
                bannedDonors.Add(d);
        }

        public int getNumDonations(Donor d)
        {
            int count = 0;
            foreach (Blood b in bloodList)
            {
                if (b.Donor_id.HasValue && b.Donor_id.Value == d.Donor_id)
                    count++;
            }
            return count;
        }

        void unsortDonor(Donor d)
        {
            donorList.Remove(d);
            if (viableDonors.Contains(d))
            {
                viableDonors.Remove(d);
                if (contactableDonors.Contains(d))
                    contactableDonors.Remove(d);
                foreach (List<Donor> dType in donorTypes)
                {
                    if (dType.Contains(d))
                    {
                        dType.Remove(d);
                        break;
                    }
                }
            }
            else if (bannedDonors.Contains(d))
                bannedDonors.Remove(d);
        }

        public List<Donor> getClosestByType(int count, int blood_type)
        {
            int cityCount = Enum.GetNames(typeof(city)).Length;
            List<Donor> closestByType = new List<Donor>();
            for (int i = 0; i < cityCount; i++)
            {
                foreach (Donor d in donorTypes[blood_type])
                {
                    if (d.Home_city == ((city)i))
                    {
                        closestByType.Add(d);
                        if (closestByType.Count >= count)
                            return closestByType;
                    }
                }
            }
            return closestByType;
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
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "Select * from blood";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string accession_number = reader.GetValue(0) as string;
                int? blood_type = reader.GetValue(1) as int?;
                int? donor_id = reader.GetValue(2) as int?;
                DateTime? date_donated = reader.GetValue(3) as DateTime?;
                DateTime? date_removed = reader.GetValue(4) as DateTime?;

                Blood x = new Blood(accession_number, blood_type.Value, donor_id, date_donated.Value, date_removed.Value);
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
        //new donation

        public bool AddBlood(Blood b, DateTime date_expire, string p_last, string p_first, string p_mid, int p_age)
        {
            if (bloodCommands("Insert into Blood " + AddQuery(BLOOD_FIELDS), b))
            {
                if (AddWholeComponent(b, date_expire, p_last, p_first, p_mid, p_age))
                    return true;
            }
            return false;
        }

        public bool AddBlood(Blood b, DateTime date_expire)
        {
            if (bloodCommands("Insert into Blood " + AddQuery(BLOOD_FIELDS), b))
            {
                if(AddWholeComponent(b, date_expire))
                    return true;
            }
            return false;
        }
        public bool UpdateBlood(Blood b, string oldAccessionNumber)
        {

            if (bloodCommands("UPDATE Blood SET " + UpdateQueryChangePrimary(BLOOD_FIELDS), b, oldAccessionNumber))
            {
                UnsortBlood(findBlood(oldAccessionNumber));
                if (oldAccessionNumber != b.Accession_number)
                {
                    foreach (Component c in b.components)
                    {
                        c.Accession_number = b.Accession_number;
                        UpdateComponent(c, b);
                    }
                }
                b.checkRemoved();
                SortBlood(b);
                return true;
            }
            return false;
        }

        bool UpdateBlood(Blood b)
        {
            if (bloodCommands("UPDATE Blood SET " + UpdateQuery(BLOOD_FIELDS, new string[] { "accession_number" }), b))
            {
                UnsortBlood(findBlood(b.Accession_number));
                b.checkRemoved();
                SortBlood(b);
                return true;
            }
            return false;
        }

        bool bloodCommands(string query, Blood x)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.CommandType = CommandType.Text;

            comm.Parameters.AddWithValue("@accession_number", x.Accession_number);
            comm.Parameters.AddWithValue("@blood_type", x.Blood_type);
            if (x.Donor_id.HasValue)
                comm.Parameters.AddWithValue("@donor_id", x.Donor_id.Value);
            else
                comm.Parameters.AddWithValue("@donor_id", null);
            comm.Parameters.AddWithValue("@date_donated", x.Date_donated);
            comm.Parameters.AddWithValue("@date_removed", x.Date_removed);

            conn.Open();
            int rowsAffected = comm.ExecuteNonQuery();
            conn.Close();
            return (rowsAffected > 0);
        }

        bool bloodCommands(string query, Blood x, string oldKey)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.CommandType = CommandType.Text;

            comm.Parameters.AddWithValue("@accession_number", x.Accession_number);
            comm.Parameters.AddWithValue("@blood_type", x.Blood_type);
            if (x.Donor_id.HasValue)
                comm.Parameters.AddWithValue("@donor_id", x.Donor_id.Value);
            else
                comm.Parameters.AddWithValue("@donor_id", null);
            comm.Parameters.AddWithValue("@date_donated", x.Date_donated);
            comm.Parameters.AddWithValue("@date_removed", x.Date_removed);
            comm.Parameters.AddWithValue("@oldPrimaryKey", oldKey);

            conn.Open();
            int rowsAffected = comm.ExecuteNonQuery();
            conn.Close();
            return (rowsAffected > 0);
        }

        void UnsortBlood(Blood b)
        {
            bloodList.Remove(b);
            if (availableBlood.Contains(b))
            {
                availableBlood.Remove(b);
                foreach (List<Blood> bType in bloodTypes)
                {
                    if (bType.Contains(b))
                    {
                        bType.Remove(b);
                        break;
                    }
                }
            }
            else if (unavailableBlood.Contains(b))
                unavailableBlood.Remove(b);

        }

        void SortBlood(Blood b)
        {
            bloodList.Add(b);
            
            if (!b.Is_removed)
            {
                availableBlood.Add(b);
                bloodTypes[(int)b.Blood_type].Add(b);
            }
            else
                unavailableBlood.Add(b);
        }

        bool isBloodUnique(string id)
        {
            foreach (Blood b in bloodList)
            {
                if (b.Accession_number == id)
                    return false;
            }
            return true;
        }

        bool DeleteBloodWithAccessionNumber(string accession_number)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            string query = "DELETE FROM Blood WHERE accession_number =@accession_number";
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.Parameters.AddWithValue("@accession_number", accession_number);
            conn.Open();
            int rowsAffected = comm.ExecuteNonQuery();
            conn.Close();
            if (rowsAffected > 0)
            {
                foreach (Component c in findBlood(accession_number).components)
                {
                    DeleteAllComponentsWithAccessionNumber(accession_number);
                }
                UnsortBlood(findBlood(accession_number));
                return true;
            }
            return false;
        }

        /*
         *<summary>
         *  Iterates through bloodList and returns the blood object with the same id as provided in the parameter
         *</summary>
         *<param name="">
         * 
         *</param>
         */
        public Blood findBlood(string accession_number)
        {
            foreach (Blood b in bloodList)
            {
                if (b.Accession_number == accession_number)
                    return b;
            }
            return null;
        }
        #endregion

        #region Component methods
        void getComponentSQL()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "Select * from component";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string accession_number = reader.GetValue(0) as string;
                int? component_name = reader.GetValue(1) as int?;
                int? removal_type = reader.GetValue(2) as int?;
                DateTime? date_processed = reader.GetValue(3) as DateTime?;
                DateTime? date_expired = reader.GetValue(4) as DateTime?;
                DateTime? date_assigned = reader.GetValue(5) as DateTime?;
                DateTime? date_removed = reader.GetValue(6) as DateTime?;
                string patient_last_name = reader.GetValue(7) as string;
                string patient_first_name = reader.GetValue(8) as string;
                string patient_middle_initial = reader.GetValue(9) as string;
                int? patient_age = reader.GetValue(10) as int?;
                string reason_for_removal = reader.GetValue(11) as string;

                findBlood(accession_number).AddComponent(new Component(accession_number, component_name.Value, removal_type.Value, date_processed.Value, date_expired.Value, date_assigned.Value, date_removed.Value, patient_last_name, patient_first_name, patient_middle_initial, patient_age.Value, reason_for_removal));
            }
            reader.Close();
            conn.Close();
        }

        bool AddComponent(string ACCESSION_NUMBER, int COMPONENT_NAME, DateTime DATE_PROCESSED, DateTime DATE_EXPIRED)
        {
            Component c = new Component(ACCESSION_NUMBER, COMPONENT_NAME, DATE_PROCESSED, DATE_EXPIRED);

            if (ComponentCommands("Insert into component " + AddQuery(COMPONENT_FIELDS), c))
            {
                Blood b = findBlood(ACCESSION_NUMBER);
                UnsortBlood(b);
                b.AddComponent(c);
                SortBlood(b);
                return true;
            }
            return false;
        }

        bool AddWholeComponent(Blood b, DateTime DATE_EXPIRED)
        {
            Component c = new Component(b.Accession_number, 0, b.Date_donated, DATE_EXPIRED);
            if (ComponentCommands("Insert into component " + AddQuery(COMPONENT_FIELDS), c))
            {
                b.AddComponent(c);
                SortBlood(b);
                return true;
            }
            return false;
        }

        bool AddWholeComponent(Blood b, DateTime DATE_EXPIRED, string pLast, string pFirst, string pMid, int pAge)
        {
            Component c = new Component(b.Accession_number, 0, b.Date_donated, DATE_EXPIRED);
            c.Assign(pLast, pFirst, pMid, pAge, b.Date_donated);
            if (ComponentCommands("Insert into component " + AddQuery(COMPONENT_FIELDS), c))
            {
                b.AddComponent(c);
                SortBlood(b);
                return true;
            }
            return false;
        }

        public bool UpdateComponent(Component c)
        {
            if (ComponentCommands("Update component set " + UpdateQuery(COMPONENT_FIELDS, new string[] { "accession_number", "component_name" }), c))
            {
                Blood b = findBlood(c.Accession_number);
                b.RemoveComponent(findComponentWithAccessionNumberAndName(c.Accession_number, c.Component_name));
                b.AddComponent(c);
                if (b.checkRemoved())
                    UpdateBlood(b);
                return true;
            }
            return false;
        }

        bool UpdateComponent(Component c, Blood b)
        {
            if (ComponentCommands("Update component set " + UpdateQuery(COMPONENT_FIELDS, new string[] { "accession_number", "component_name" }), c))
            {
                b.RemoveComponent(findComponentWithAccessionNumberAndName(c.Accession_number, c.Component_name));
                b.AddComponent(c);
                return true;
            }
            return false;
        }

        bool ComponentCommands(string query, Component x)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.CommandType = CommandType.Text;

            comm.Parameters.AddWithValue("@accession_number", x.Accession_number);
            comm.Parameters.AddWithValue("@component_name", (int)x.Component_name);
            comm.Parameters.AddWithValue("@removal_type", (int)x.Removal_Type);
            comm.Parameters.AddWithValue("@date_processed", x.Date_processed);
            comm.Parameters.AddWithValue("@date_expired", x.Date_expired);
            comm.Parameters.AddWithValue("@date_assigned", x.Date_assigned);
            comm.Parameters.AddWithValue("@date_removed", x.Date_removed);
            comm.Parameters.AddWithValue("@patient_last_name", x.Patient_last_name);
            comm.Parameters.AddWithValue("@patient_first_name", x.Patient_first_name);
            comm.Parameters.AddWithValue("@patient_middle_initial", x.Patient_middle_initial);
            comm.Parameters.AddWithValue("@patient_age", x.Patient_age);
            comm.Parameters.AddWithValue("@reason_for_removal", x.Reason_for_removal);
            conn.Open();
            int rowsAffected = comm.ExecuteNonQuery();
            conn.Close();
            return (rowsAffected > 0);
        }

        public Component findComponentWithAccessionNumberAndName(string accession_number, bloodComponents name)
        {
            foreach (Component c in findBlood(accession_number).components)
            {
                if (c.Component_name == name)
                    return c;
            }
            return null;
        }

        public bool DeleteComponentWithAccessionNumberAndName(string accession_number, bloodComponents name)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            string query = "DELETE FROM component WHERE accession_number = @accession_number AND component_name = @component_name";
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.Parameters.AddWithValue("@accession_number", accession_number);
            comm.Parameters.AddWithValue("@component_name", (int)name);
            conn.Open();
            int rowsAffected = comm.ExecuteNonQuery();
            conn.Close();
            if (rowsAffected > 0)
            {
                findBlood(accession_number).RemoveComponent(findComponentWithAccessionNumberAndName(accession_number, name));
                return true;
            }
            return false;
        }

        bool DeleteAllComponentsWithAccessionNumber(string accession_number)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            string query = "DELETE FROM component WHERE accession_number = @accession_number";
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.Parameters.AddWithValue("@accession_number", accession_number);
            conn.Open();
            int rowsAffected = comm.ExecuteNonQuery();
            conn.Close();
            if (rowsAffected > 0)
                return true;
            return false;
        }


        #endregion

        #region Utility Methods
        /*
         *<summary>
         *  returns a string which assigns the values to be updated
         *</summary>
         *<param name="fields">
         *  string array containing all fields for an SQL table
         *</param>
         */
        string UpdateQuery(string[] fields, string[] comparator)
        {
            string valueParameters = fields[1] + "=@" + fields[1];
            for (int i = 2; i < fields.Length; i++)
            {
                valueParameters += ", " + fields[i] + "=@" + fields[i];
            }
            valueParameters += " Where " + comparator[0] + "=@" + comparator[0];
            for (int i = 1; i < comparator.Length; i++)
            {
                valueParameters += " AND Where " + comparator[i] + "=@" + comparator[i];
            }

            return valueParameters;
        }

        string UpdateQueryChangePrimary(string[] fields)
        {
            string valueParameters = fields[1] + "=@" + fields[1];
            for (int i = 2; i < fields.Length; i++)
            {
                valueParameters += ", " + fields[i] + "=@" + fields[i];
            }
            valueParameters += " Where " + fields[0] + "=@oldPrimaryKey";

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
        public string AddQuery(string[] fields)
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

        public bool AlertLowLevel(int blood_type)
        {
            if (bloodTypes[blood_type].Count < MINIMUMBLOODVALUE)
                return true;
            return false;
        }

        public List<string[]> AlertNearExpiration()
        {
            List<string[]> expiringComponents = new List<string[]>();
            foreach (Blood b in bloodList)
            {
                foreach (Component c in b.components)
                {
                    TimeSpan span = c.Date_expired.Date - DateTime.Today;
                    if (span.TotalDays < MINIMUMEXPIRYALERTVALUE)
                        expiringComponents.Add(new string[2] { c.Component_name.ToString(), c.Accession_number });
                }
            }

            return expiringComponents;
        }

        #endregion

        #region Graph Methods
        public int[] getBloodModifiedDuring(Dictionary<DateTime, int> days, graphCommand command)
        {

            int[] ints = new int[days.Count];
            switch (command)
            {
                case graphCommand.Release:
                    foreach (Blood b in bloodList)
                    {
                        foreach (Component c in b.components)
                        {
                            if (c.Is_released)
                            {
                                if (days.ContainsKey(b.Date_removed.Date))
                                    ints[(int)days[c.Date_removed.Date]]++;
                            }
                        }

                    }
                    break;

                case graphCommand.Quarantine:
                    foreach (Blood b in bloodList)
                    {
                        foreach (Component c in b.components)
                        {
                            if (c.Is_quarantined)
                            {
                                if (days.ContainsKey(b.Date_removed.Date))
                                    ints[(int)days[c.Date_removed.Date]]++;
                            }
                        }
                    }
                    break;

                case graphCommand.Reprocess:
                    foreach (Blood b in bloodList)
                    {
                        foreach (Component c in b.components)
                        {
                            if (c.Is_reprocessed)
                            {
                                if (days.ContainsKey(b.Date_removed.Date))
                                    ints[(int)days[c.Date_removed.Date]]++;
                            }
                        }
                    }
                    break;

                case graphCommand.Add:
                    foreach (Blood b in bloodList)
                    {
                        if (days.ContainsKey(b.Date_donated.Date))
                            ints[(int)days[b.Date_donated.Date]]++;
                    }
                    break;
                case graphCommand.Remove:
                    foreach (Blood b in bloodList)
                    {
                        if (days.ContainsKey(b.Date_removed.Date))
                            ints[(int)days[b.Date_removed.Date]]++;
                    }
                    break;
            }

            return ints;
        }

        public int[,] getSummary(Dictionary<DateTime, int> days)
        {
            int[,] ints = new int[days.Count, 2];
            foreach (Blood b in bloodList)
            {
                if (days.ContainsKey(b.Date_donated.Date))
                    ints[(int)days[b.Date_donated.Date], 0]++;
                if (days.ContainsKey(b.Date_removed.Date))
                    ints[(int)days[b.Date_removed.Date], 1]++;
            }
            return ints;
        }

        public int[,] getBloodTypeModifiedDuring(Dictionary<DateTime, int> days, graphCommand command)
        {

            int[,] ints = new int[days.Count, BlOODTYPECOUNT];
            switch (command)
            {
                case graphCommand.Release:
                    foreach (Blood b in bloodList)
                    {
                        foreach (Component c in b.components)
                        {
                            if (c.Is_released)
                            {
                                if (days.ContainsKey(c.Date_removed.Date))
                                    ints[(int)days[c.Date_removed.Date], (int)b.Blood_type]++;
                            }
                        }
                    }
                    break;
                case graphCommand.Quarantine:
                    foreach (Blood b in bloodList)
                    {
                        foreach (Component c in b.components)
                        {
                            if (c.Is_quarantined)
                            {
                                if (days.ContainsKey(c.Date_removed.Date))
                                    ints[(int)days[c.Date_removed.Date], (int)b.Blood_type]++;
                            }
                        }
                    }
                    break;
                case graphCommand.Reprocess:
                    foreach (Blood b in bloodList)
                    {
                        foreach (Component c in b.components)
                        {
                            if (c.Is_reprocessed)
                            {
                                if (days.ContainsKey(c.Date_removed.Date))
                                    ints[(int)days[c.Date_removed.Date], (int)b.Blood_type]++;
                            }
                        }
                    }
                    break;
                case graphCommand.Add:
                    foreach (Blood b in bloodList)
                    {
                        if (days.ContainsKey(b.Date_donated.Date))
                            ints[(int)days[b.Date_donated.Date], (int)b.Blood_type]++;
                    }
                    break;
                case graphCommand.Remove:
                    foreach (Blood b in bloodList)
                    {
                        if (days.ContainsKey(b.Date_removed.Date))
                            ints[(int)days[b.Date_removed.Date], (int)b.Blood_type]++;
                    }
                    break;
            }

            return ints;
        }
        #endregion

        #region search

        public List<Blood> searchBloodWithString(string s)
        {
            List<Blood> bloods = new List<Blood>();
            foreach (Blood b in bloodList)
            {
                if (b.Accession_number.Contains(s))
                {
                    bloods.Add(b);
                }
                else
                {
                    foreach (Component c in b.components)
                    {
                        if (c.Patient_name.Contains(s))
                        {
                            bloods.Add(b);
                        }
                    }
                }
            }

            return bloods;
        }

        public List<string> searchWithString(string s)
        {
            List<string> objects = new List<string>();
            foreach (Donor d in donorList)
            {
                if (d.Name.Contains(s))
                {
                    objects.Add(d.Name);
                }
            }
            foreach (Blood b in bloodList)
            {
                if (b.Accession_number.Contains(s))
                {
                    objects.Add(b.Accession_number);
                }
                else
                {
                    foreach (Component c in b.components)
                    {
                        if (c.Patient_name.Contains(s))
                        {
                            objects.Add(b.Accession_number);
                        }
                    }
                }
            }
            return objects;
        }

        public List<Donor> searchDonorsWithString(string s)
        {
            List<Donor> donors = new List<Donor>();
            foreach (Donor d in donorList)
            {
                if (d.Name.Contains(s))
                {
                    donors.Add(d);
                }
                if (!donors.Contains(d))
                {
                    foreach (Blood b in bloodList)
                    {
                        if (b.Donor_id.HasValue && b.Donor_id == d.Donor_id)
                        {
                            if (b.Accession_number.Contains(s))
                                donors.Add(d);
                        }
                    }
                }
            }
            return donors;
        }
        #endregion
    }
}

