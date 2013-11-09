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
            bloodList = new List<Blood>();
            bloodTypes = new List<Blood>[Enum.GetNames(typeof(bloodType)).Length];
            donorList = new List<Donor>();

            connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3}", host, db, user, pass);
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

                Blood x = new Blood(donor_id, date_donated, date_expire, component);
                x.Blood_id = blood_id;
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
        void AddDonor(int donor_id, bloodType blood_type, string name, string street, string city, string province, string email, string cellphone, string reason_for_deferral, DateTime date_registered, DateTime next_available, DateTime birth_date, bool is_viable, bool is_contactable, bool is_voluntary)
        {
            Donor newDonor = new Donor(donor_id, blood_type, name, street, city, province, email, cellphone, reason_for_deferral, date_registered, next_available, birth_date, is_viable, is_contactable, is_voluntary);
            donorList.Add(newDonor);
        }

        bool AddBlood(DateTime date_donated, DateTime date_expire, int donor_id, string component)
        {
            Blood newBlood = new Blood(donor_id, date_donated, date_expire, component);
            bloodList.Add(newBlood);

            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            //string query = "Insert into dormerlist " +
            //    "(idNumber, firstName, mI, lastName, year, course, roomNumber, points, attendance, position) " +
            //    "Values(?a, ?b, ?c, ?d, ?e, ?f, ?g, ?h, ?i, ?j)";
            //MySqlCommand comm = new MySqlCommand(query, conn);
            //comm.Parameters.AddWithValue("?a", x.IdNumber);
            //comm.Parameters.AddWithValue("?b", x.FirstName);
            //comm.Parameters.AddWithValue("?c", x.MI);
            //comm.Parameters.AddWithValue("?d", x.LastName);
            //comm.Parameters.AddWithValue("?e", x.Year);
            //comm.Parameters.AddWithValue("?f", x.Course);
            //comm.Parameters.AddWithValue("?g", x.RoomNumber);
            //comm.Parameters.AddWithValue("?h", x.Points);
            //comm.Parameters.AddWithValue("?i", x.Attendance);
            //comm.Parameters.AddWithValue("?j", x.Position);


            //int affectedRows = comm.ExecuteNonQuery();

            //if (affectedRows > 0)
            //{
            //    return true;
            //}
            return false;
        }

        void getBloodInInventory()
        {
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
    }
}