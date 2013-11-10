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

        Storage(string host, string db, string user, string pass)
        {
            connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3}", host, db, user, pass);
        }

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
                int donor_id = reader.GetInt32(1);
                string patient_name = reader.GetString(2);
                int patient_age = reader.GetInt32(3);
                DateTime date_donated = reader.GetDateTime(4);
                DateTime date_expire = reader.GetDateTime(5);
                DateTime date_removed = DateTime.MinValue;
                if (reader.GetValue(6) != null)
                    date_removed = reader.GetDateTime(6);
                bool is_assigned = reader.GetBoolean(7);
                int age = reader.GetInt32(8);
                bool is_quarantined = reader.GetBoolean(9);
                string reason_for_removal = reader.GetString(10);
                string component = reader.GetString(11);

                Blood x = new Blood(bloodList.Count, donor_id, date_donated, date_expire, component);
                if (is_assigned)
                {
                    x.Assign(patient_name, patient_age);
                }
                if (date_removed != DateTime.MinValue)
                {
                    if (is_assigned)
                    {
                        x.Release(date_removed);
                    }
                    else if (is_quarantined)
                    {
                        x.Quarantine(reason_for_removal, date_removed);
                    }
                }
                x.Component = component;
                bloodList.Add(x);
            }
            reader.Close();
            conn.Close();

        }
        bool AddDonor(int donor_id, bloodType blood_type, string name, string street, string city, string province, string email, string cellphone, string reason_for_deferral, DateTime date_registered, DateTime next_available, DateTime birth_date, bool is_viable, bool is_contactable, bool is_voluntary)
        {
            Donor x = new Donor(donorList.Count, blood_type, name, street, city, province, email, cellphone, reason_for_deferral, date_registered, next_available, birth_date, is_viable, is_contactable, is_voluntary);
            donorList.Add(x);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string fields = "donor_id,patient_name,patient_age,date_donated,date_expire,date_removed,is_assigned,age,is_quarantined,reason_for_removal,component";
            string query = "Insert into blood (" + AddQuery(fields) + ")";
            MySqlCommand comm = new MySqlCommand(query, conn);
            AddValue(comm, x, fields);
            int affectedRows = comm.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                return true;
            }
            return false;
        }

        bool AddBlood(DateTime date_donated, DateTime date_expire, int donor_id, string component)
        {
            Blood x = new Blood(bloodList.Count, donor_id, date_donated, date_expire, component);
            bloodList.Add(x);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            string fields = "blood_id, donor_id,patient_name,patient_age,date_donated,date_expire,date_removed,is_assigned,age,is_quarantined,reason_for_removal,component";
            string query = "Insert into blood (" + AddQuery(fields) + ")";
                
                //"donor_id, patient_name, patient_age, date_donated, date_expire, date_removed, is_assigned, age, is_quarantined, reason_for_removal, component) " +
                //"Values(@donor_id, ?b, ?c, ?d, ?e, ?f, ?g, ?h, ?i, ?j, ?k)";
            MySqlCommand comm = new MySqlCommand(query, conn);
            AddValue(comm, x, fields);
            /*
            comm.Parameters.AddWithValue("@donor_id", x.Donor_id);
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
             * */


            int affectedRows = comm.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                return true;
            }
            return false;
        }

        string AddQuery(string fields)
        {
            string[] values = fields.Split(',');
            string valueParameters = "";
            for (int i = 0; i < values.Length; i++)
            {
                valueParameters += "@" + values[i];
                if (i < values.Length - 1)
                {
                    valueParameters += ", ";
                }
            }
            return fields + ") Values(" + valueParameters;
        }

        void AddValue(MySqlCommand comm, Object x, string fields)
        {
            string[] values = fields.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                comm.Parameters.AddWithValue("@" + values[i], GetPropValue(x, Capitalize(values[i])));
            }
        }

        //http://stackoverflow.com/questions/1196991/get-property-value-from-string-using-reflection-in-c-sharp
        object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        //http://stackoverflow.com/questions/4135317/make-first-letter-of-a-string-upper-case
        string Capitalize(string s)
        {
            return s.First().ToString().ToUpper() + String.Join("", s.Skip(1));
        }

        void getBloodInInventory()
        {
            bloodTypes = new List<Blood>[Enum.GetNames(typeof(bloodType)).Length];
            foreach (Blood b in bloodList)
            {
                if (b.Date_removed == null)
                {
                    bloodTypes[(int)findDonor(b.Donor_id).blood_type].Add(b);
                }
            }
        }

        Donor findDonor(int id)
        {
            foreach (Donor d in donorList)
            {
                if (d.donor_id == id)
                {
                    return d;
                }
            }
            return null;
        }

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