using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
namespace Blood_SMS
{
    class OMC
    {
        string connectionString;
        List<BloodType> bloodTypes;

        public OMC(string host, string db, string user, string pass)
        {
            bloodTypes = new List<BloodType>();
            connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3}", host, db, user, pass);
        }

        List<BloodType> GetAllBloodTypes()
        {
            List<BloodType> output = new List<BloodType>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "Select * from Blood_Type";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {

            }
            reader.Close();
            conn.Close();
            return output;
        }
    }
}
