using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Blood_SMS
{
    public enum graphCommand { Add, Remove, Release, Quarantine, Use, Summary };
    public enum bloodType { ABp, ABn, Ap, An, Bp, Bn, Op, On };
    public enum contactMethod { none, email, cellphone };
    public enum educationalAttainment { other, gradeschool, highschool, college };
    public enum city { QuezonCity, SanJuan, Manila, Caloocan, Mandaluyong, Malabon, Pateros, Makati, Valenzuela, Navotas, Pasay, Taguig, Paranaque, Muntinlupa, LasPinas, Other };
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
        public List<Blood> bloodList;
        public List<Blood>[] bloodTypes;
        public List<Blood> availableBlood;
        public List<Blood> quarantinedBlood;
        public List<Blood> usedBlood;
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

        int BlOODTYPECOUNT = Enum.GetNames(typeof(bloodType)).Length;
        const int MINIMUMBLOODVALUE = 20;
        const int MINIMUMEXPIRYALERTVALUE = 3;
        readonly string[] BLOOD_FIELDS = { "accession_number", "blood_type", "patient_name", "patient_age", "date_added", "date_expire", "date_removed", "is_assigned", "is_processed", "is_quarantined", "reason_for_removal" };
        readonly string[] DONOR_FIELDS = { "donor_id", "name", "blood_type", "home_province", "home_city", "home_street", "office_province", "office_city", "office_street", "preferred_contact_method", "home_landline", "office_landline", "email", "cellphone", "educational_attainment", "birth_date", "date_registered", "next_available", "times_donated", "times_contacted", "is_contactable", "is_viable", "reason_for_deferral" };
        public Storage(string host, string db, string user, string pass)
        {
            connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3}", host, db, user, pass);

            bloodList = new List<Blood>();
            bloodTypes = new List<Blood>[BlOODTYPECOUNT];
            availableBlood = new List<Blood>();
            quarantinedBlood = new List<Blood>();
            usedBlood = new List<Blood>();
            
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
                string NAME = reader.GetValue(1) as string;
                int? BLOOD_TYPE = reader.GetValue(2) as int?;
                string HOME_PROVINCE = reader.GetValue(3) as string;
                string HOME_CITY = reader.GetValue(4) as string;
                string HOME_STREET = reader.GetValue(5) as string;
                string OFFICE_PROVINCE = reader.GetValue(6) as string;
                string OFFICE_CITY = reader.GetValue(7) as string;
                string OFFICE_STREET = reader.GetValue(8) as string;
                int? PREFERRED_CONTACT_METHOD = reader.GetValue(9) as int?;
                string HOME_LANDLINE = reader.GetValue(10) as string;
                string OFFICE_LANDLINE = reader.GetValue(11) as string;
                string EMAIL = reader.GetValue(12) as string;
                string CELLPHONE = reader.GetValue(13) as string;
                int? EDUCATIONAL_ATTAINMENT = reader.GetValue(14) as int?;
                DateTime? BIRTH_DATE = reader.GetValue(15) as DateTime?;
                DateTime? DATE_REGISTERED = reader.GetValue(16) as DateTime?;
                DateTime? NEXT_AVAILABLE = reader.GetValue(17) as DateTime?;
                int? TIMES_DONATED = reader.GetValue(18) as int?;
                int? TIMES_CONTACTED = reader.GetValue(19) as int?;
                bool? IS_CONTACTABLE = reader.GetValue(20) as bool?;
                bool? IS_VIABLE = reader.GetValue(21) as bool?;
                string REASON_FOR_DEFERRAL = reader.GetValue(22) as string;

                Donor x = new Donor(DONOR_ID, NAME,
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
                    NEXT_AVAILABLE,
                    TIMES_DONATED,
                    TIMES_CONTACTED,
                    IS_CONTACTABLE,
                    IS_VIABLE,
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
        public bool AddDonor(
            string NAME,
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

            //http://stackoverflow.com/questions/5228780/how-to-get-last-inserted-id
            if (donorCommands("Insert into donor output donor_id" + AddQuery(DONOR_FIELDS), x) > 0)
            {
                sortDonor(x);
                return true;
            }
            return false;
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
            DateTime NEXT_AVAILABLE,
            int TIMES_DONATED,
            int TIMES_CONTACTED,
            bool IS_CONTACTABLE,
            bool IS_VIABLE,
            string REASON_FOR_DEFERRAL)
        {
            
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
                    NEXT_AVAILABLE,
                    TIMES_DONATED,
                    TIMES_CONTACTED,
                    IS_CONTACTABLE,
                    IS_VIABLE,
                    REASON_FOR_DEFERRAL);
            if (donorCommands("UPDATE donor SET " + UpdateQuery(DONOR_FIELDS), x) > 0)
            {
                unsortDonor(findDonor(DONOR_ID));
                x.Donor_id = DONOR_ID;
                sortDonor(x);
                return true;
            }
            return false;
        }

        int donorCommands(string query, Donor x)
        {
            //FIND A WAY TO SET THE ID
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.CommandType = CommandType.Text;
            //long id = comm.LastInsertedId;
            //x.Donor_id = (int)id;
            //comm.Parameters.AddWithValue("@donor_id", x.Donor_id);
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
            comm.Parameters.AddWithValue("@next_available", x.Next_available);
            comm.Parameters.AddWithValue("@times_donated", x.Times_donated);
            comm.Parameters.AddWithValue("@times_contacted", x.Times_contacted);
            comm.Parameters.AddWithValue("@is_contactable", x.Is_contactable);
            comm.Parameters.AddWithValue("@is_viable", x.Is_viable);
            comm.Parameters.AddWithValue("@reason_for_deferral", x.Reason_for_deferral);

            
            conn.Open();
            x.Donor_id = (int)comm.ExecuteScalar();
            int rowsAffected = comm.ExecuteNonQuery();
            conn.Close();
            return rowsAffected;
        }

        bool DeleteDonorWithId(int id)
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
                {
                    return d;
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
            {
                bannedDonors.Add(d);
            }
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

        

        List<Donor> getClosestByType(int count, bloodType blood_type)
        {
            int cityCount = Enum.GetNames(typeof(city)).Length;
            int bType = (int)blood_type;
            List<Donor> closestByType = new List<Donor>();
            for (int i = 0; i < cityCount; i++)
            {
                foreach (Donor d in donorTypes[bType])
                {
                    if (d.Home_city == ((city)i).ToString())
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
                string patient_name = reader.GetValue(3) as string;
                int? patient_age = reader.GetValue(4) as int?;
                DateTime? date_added = reader.GetValue(5) as DateTime?;
                DateTime? date_expire = reader.GetValue(6) as DateTime?;
                DateTime? date_removed = reader.GetValue(7) as DateTime?;
                bool? is_assigned = reader.GetValue(8) as bool?;
                bool? is_processed = reader.GetValue(9) as bool?;
                bool? is_quarantined = reader.GetValue(10) as bool?;
                string reason_for_removal = reader.GetValue(11) as string;

                Blood x = new Blood(accession_number, blood_type, donor_id, patient_name, patient_age, date_added, date_expire, date_removed, is_assigned, is_processed, is_quarantined, reason_for_removal);
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
        public bool AddBlood(string accession_number, int blood_type, int? donor_id, DateTime date_added, DateTime date_expire)
        {
            Blood x = new Blood(accession_number, blood_type, donor_id, date_added, date_expire);

            if (bloodCommands("Insert into Blood " + AddQuery(BLOOD_FIELDS), x) > 0)
            {
                SortBlood(x);
                return true;
            }
            return false;
        }

        /*
       *<summary>
       *  Updates the sql table with the properties found in the supplied object
       *</summary>
       *<param name="x">
       *  Blood object containing properties to be applied
       *</param>
       */
        bool UpdateBlood(string accession_number, int blood_type, int? donor_id, string patient_name, int patient_age, DateTime date_added, DateTime date_expire, DateTime date_removed, bool is_assigned, bool is_processed, bool is_quarantined, string reason_for_removal)
        {
            Blood x = new Blood(accession_number, blood_type, donor_id, patient_name, patient_age, date_added, date_expire, date_removed, is_assigned, is_processed, is_quarantined, reason_for_removal);
            if (bloodCommands("UPDATE Blood SET " + UpdateQuery(BLOOD_FIELDS), x) > 0)
            {
                UnsortBlood(findBlood(accession_number));
                SortBlood(x);
                return true;
            }
            return false;
        }

        int bloodCommands(string query, Blood x)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.CommandType = CommandType.Text;

            comm.Parameters.AddWithValue("@accession_number", x.Accession_number);
            comm.Parameters.AddWithValue("@blood_type", x.Blood_type);
            if (x.Donor_id.HasValue)
                comm.Parameters.AddWithValue("@donor_id", x.Donor_id);
            comm.Parameters.AddWithValue("@patient_name", x.Patient_name);
            comm.Parameters.AddWithValue("@patient_age", x.Patient_age);
            comm.Parameters.AddWithValue("@date_added", x.Date_added);
            comm.Parameters.AddWithValue("@date_expire", x.Date_expire);
            comm.Parameters.AddWithValue("@date_removed", x.Date_removed);
            comm.Parameters.AddWithValue("@is_assigned", x.Is_assigned);
            comm.Parameters.AddWithValue("@is_processed", x.Is_processed);
            comm.Parameters.AddWithValue("@is_quarantined", x.Is_quarantined);
            comm.Parameters.AddWithValue("@reason_for_removal", x.Reason_for_removal);

            conn.Open();
            int rowsAffected = comm.ExecuteNonQuery();
            conn.Close();
            return rowsAffected;
        }

        #region bloodGraphMethods
        public int getBloodRemovedOn(DateTime date)
        {
            int count = 0;
            foreach (Blood b in bloodList)
            {
                if (b.Is_removed && b.Date_removed == date)
                {
                    count++;
                }
            }
            return count;
        }

        /*
        public int[] getAllBloodChangedOn(DateTime date)
        {
            int[] ints = new int[Enum.GetNames(typeof(graphCommand)).Length];
            for (int i = 0; i < ints.Length; i++)
            {
                ints[i] = 0;
            }
            foreach (Blood b in bloodList)
            {
                if (b.Is_removed && b.Date_removed == date)
                {
                    ints[(int)graphCommand.Remove]++;
                    if (b.Is_quarantined)
                        ints[(int)graphCommand.Quarantine]++;
                    else if (b.Reason_for_removal.StartsWith("Extracted"))
                        ints[(int)graphCommand.Use]++;
                    else if (b.Is_assigned)
                        ints[(int)graphCommand.Release]++;
                }
                if (b.Component == "Whole" && b.Date_added == date)
                {
                    ints[(int)graphCommand.Add]++;
                }
            }
            return ints;
        }

        public int[,] getAllBloodTypeChangedOn(DateTime date)
        {
            int[,] ints = new int[Enum.GetNames(typeof(graphCommand)).Length,BlOODTYPECOUNT];
            for (int i = 0; i < Enum.GetNames(typeof(graphCommand)).Length; i++)
            {
                for(int j = 0; j<BlOODTYPECOUNT; j++)
                    ints[i, j] = 0;
            }
            foreach (Blood b in bloodList)
            {
                if (b.Is_removed && b.Date_removed == date)
                {
                    ints[(int)graphCommand.Remove, (int)findDonor(b.Taken_from).Blood_type]++;
                    if(b.Is_quarantined)
                        ints[(int)graphCommand.Quarantine, (int)findDonor(b.Taken_from).Blood_type]++;
                    else if (b.Reason_for_removal.StartsWith("Extracted"))
                        ints[(int)graphCommand.Use, (int)findDonor(b.Taken_from).Blood_type]++;
                    else if (b.Is_assigned)
                        ints[(int)graphCommand.Release, (int)findDonor(b.Taken_from).Blood_type]++;
                }
                if (b.Component == "Whole" && b.Date_added == date)
                {
                    ints[(int)graphCommand.Add, (int)findDonor(b.Taken_from).Blood_type]++;
                } 
            }
            return ints;
        }
        */
        public int[] getBloodTypeRemovedOn(DateTime date)
        {
            int[] ints = new int[BlOODTYPECOUNT];
            foreach (Blood b in bloodList)
            {
                if (b.Is_removed && b.Date_removed == date)
                {
                    ints[(int)b.Blood_type]++;
                }
            }
            return ints;
        }

        public int getBloodUsedOn(DateTime date)
        {
            int count = 0;
            foreach (Blood b in bloodList)
            {
                if (b.Is_removed && b.Reason_for_removal.StartsWith("Extracted") && b.Date_removed == date)
                {
                    count++;
                }
            }
            return count;
        }

        public int[] getBloodTypeUsedOn(DateTime date)
        {
            int[] ints = new int[BlOODTYPECOUNT];
            foreach (Blood b in bloodList)
            {
                if (b.Is_removed && b.Reason_for_removal.StartsWith("Extracted") && b.Date_removed == date)
                {
                    ints[(int)b.Blood_type]++;
                }
            }
            return ints;
        }

        public int getBloodReleasedOn(DateTime date)
        {
            int count = 0;
            foreach (Blood b in bloodList)
            {
                if (b.Is_assigned && b.Is_removed && b.Date_removed == date)
                {
                    count++;
                }
            }
            return count;
        }

        public int[] getBloodTypeReleasedOn(DateTime date)
        {
            int[] ints = new int[BlOODTYPECOUNT];
            foreach (Blood b in bloodList)
            {
                if (b.Is_assigned && b.Is_removed && b.Date_removed == date)
                {
                    ints[(int)b.Blood_type]++;
                }
            }
            return ints;
        }

        public int getBloodQuarantinedOn(DateTime date)
        {
            int count = 0;
            foreach (Blood b in bloodList)
            {
                if (b.Is_quarantined && b.Date_removed == date)
                {
                    count++;
                }
            }
            return count;
        }

        public int[] getBloodTypeQuarantinedOn(DateTime date)
        {
            int[] ints = new int[BlOODTYPECOUNT];
            foreach (Blood b in bloodList)
            {
                if (b.Is_quarantined && b.Date_removed == date)
                {
                    ints[(int)b.Blood_type]++;
                }
            }
            return ints;
        }

        public int getBloodAddedOn(DateTime date)
        {
            int count = 0;
            foreach (Blood b in bloodList)
            {
                if (b.Date_added == date)
                {
                    count++;
                }
            }
            return count;
        }

        public int[] getBloodTypeAddedOn(DateTime date)
        {
            int[] ints = new int[BlOODTYPECOUNT];
            foreach (Blood b in bloodList)
            {
                if (b.Date_added == date)
                {
                    ints[(int)b.Blood_type]++;
                }
            }
            return ints;
        }

        //List<int[]> getWholeBloodTypeAddedOn(DateTime dateFrom, DateTime dateTo)
        //{
        //    TimeSpan span = dateTo - dateFrom;
        //    List<int[]> wholeBloodTypeAdded = new List<int[]>();
        //    for (int i = 0; i < span.TotalDays; i++)
        //    {
        //        wholeBloodTypeAdded.Add(new int[BlOODTYPECOUNT]);
        //    }

                
        //    foreach (Blood b in bloodList)
        //    {
        //        for (int i = 0; i < span.TotalDays; i++)
        //        {
        //            if (b.Component == "Whole")
        //            {
        //                wholeBloodTypeAdded[i][(int)findDonor(b.Taken_from).Blood_type]++;
        //            }
        //        }
        //    }
        //    return wholeBloodTypeAdded;
        //}
        #endregion

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
            else if (quarantinedBlood.Contains(b))
                quarantinedBlood.Remove(b);
            else if (usedBlood.Contains(b))
                usedBlood.Remove(b);

        }

        void SortBlood(Blood b)
        {
            if (!b.Is_removed)
            {
                availableBlood.Add(b);
                bloodTypes[(int)b.Blood_type].Add(b);
            }
            else if (b.Is_quarantined)
            {
                quarantinedBlood.Add(b);
            }
            else
            {
                usedBlood.Add(b);
            }
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
        Blood findBlood(string accession_number)
        {
            foreach (Blood b in bloodList)
            {
                if (b.Accession_number == accession_number)
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

        /*void AlertLowLevel(bloodType blood_type)
        {
            foreach(Blood b in bloodTypes[(int)blood_type])
            {
				
            }
        }*/

        public bool AlertLowLevel(bloodType blood_type)
        {
            if (bloodTypes[(int)blood_type].Count < MINIMUMBLOODVALUE)
                return true;
            return false;
        }

        public bool AlertNearExpiration(Blood b)
        {
            TimeSpan span = DateTime.Now - b.Date_expire;
            if (span.TotalDays < MINIMUMEXPIRYALERTVALUE)
                return true;
            return false;
        }

        #endregion

    }
}

