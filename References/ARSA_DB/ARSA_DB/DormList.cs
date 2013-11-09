using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ARSA_DB
{
    class DormList
    {
        string conn_string;
        List<Dormer> dormers = new List<Dormer>();
        List<Event> events = new List<Event>();

        public DormList(string host, string db, string user, string pass)
        {
            conn_string = string.Format("Server={0};Database={1};Uid={2};Pwd={3}", host, db, user, pass);
            dormers = getAllDormers();
        }

        public List<Dormer> getAllDormers()
        {
            List<Dormer> output = new List<Dormer>();
            MySqlConnection conn = new MySqlConnection(conn_string);
            conn.Open();

            string query = "Select * from dormerlist";
            MySqlCommand command = new MySqlCommand(query, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int idNumber = (int)reader.GetValue(0);
                string firstName = reader.GetValue(1).ToString();
                string mI = reader.GetValue(2).ToString();
                string lastName = reader.GetValue(3).ToString();
                int year = (int)reader.GetValue(4);
                string course = reader.GetValue(5).ToString();
                string roomNumber = reader.GetValue(6).ToString();
                int points = (int)reader.GetValue(7);
                int attendance = (int)reader.GetValue(8);
                string position = reader.GetValue(9).ToString();

                Dormer x = new Dormer(idNumber, firstName, mI, lastName, year, course, roomNumber, points, attendance, position);
                output.Add(x);
            }
            reader.Close();
            conn.Close();
            return output;
        }

        public List<Event> getAllEvents()
        {
            List<Event> output = new List<Event>();
            MySqlConnection conn = new MySqlConnection(conn_string);
            conn.Open();

            string query = "Select * from eventlist";
            MySqlCommand command = new MySqlCommand(query, conn);

            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int eventId = (int)reader.GetValue(0);
                string eventName = reader.GetValue(1).ToString();
                string eventDate = reader.GetValue(2).ToString();
                string eventTime = reader.GetValue(3).ToString();
                string eventVenue = reader.GetValue(4).ToString();

                Event x = new Event(eventId, eventName, eventDate, eventTime, eventVenue);
                output.Add(x);
            }
            reader.Close();
            conn.Close();
            return output;
        }

        public Boolean isUnique(Dormer x)
        {
            foreach (Dormer d in dormers)
            {
                int idNumber = d.IdNumber;
                if (idNumber.Equals(x.IdNumber))
                {
                    return false;
                }
            }
            return true;
        }

        public bool addNewDormer(Dormer x)
        {
            dormers.Add(x);
            MySqlConnection conn = new MySqlConnection(conn_string);
            conn.Open();

            string query = "Insert into dormerlist " +
                "(idNumber, firstName, mI, lastName, year, course, roomNumber, points, attendance, position) " +
                "Values(?a, ?b, ?c, ?d, ?e, ?f, ?g, ?h, ?i, ?j)";
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.Parameters.AddWithValue("?a", x.IdNumber);
            comm.Parameters.AddWithValue("?b", x.FirstName);
            comm.Parameters.AddWithValue("?c", x.MI);
            comm.Parameters.AddWithValue("?d", x.LastName);
            comm.Parameters.AddWithValue("?e", x.Year);
            comm.Parameters.AddWithValue("?f", x.Course);
            comm.Parameters.AddWithValue("?g", x.RoomNumber);
            comm.Parameters.AddWithValue("?h", x.Points);
            comm.Parameters.AddWithValue("?i", x.Attendance);
            comm.Parameters.AddWithValue("?j", x.Position);


            int affectedRows = comm.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                return true;
            }
            return false;
        }

        public bool updateDormerInfo(Dormer x, string nFirst, string nMi, string nLast, int nYear, string nCourse, string nRoom, int nPts, int nAtt, string nPos)
        {
            MySqlConnection conn = new MySqlConnection(conn_string);
            conn.Open();

            string query = "Update dormerlist Set " +
                "firstName = ?b, " +
                "mI = ?c, " +
                "lastName = ?d, " +
                "year = ?e, " +
                "course = ?f, " +
                "roomNumber = ?g, " +
                "points = ?h, " +
                "attendance = ?i, " +
                "position = ?j, " +
                "Where idNumber = ?a";
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.Parameters.AddWithValue("?a", x.IdNumber);
            comm.Parameters.AddWithValue("?b", nFirst);
            comm.Parameters.AddWithValue("?c", nMi);
            comm.Parameters.AddWithValue("?d", nLast);
            comm.Parameters.AddWithValue("?e", nYear);
            comm.Parameters.AddWithValue("?f", nCourse);
            comm.Parameters.AddWithValue("?g", nRoom);
            comm.Parameters.AddWithValue("?h", nPts);
            comm.Parameters.AddWithValue("?i", nAtt);
            comm.Parameters.AddWithValue("?j", nPos);
            int affectedRows = comm.ExecuteNonQuery();

            Dormer temp = new Dormer(x.IdNumber, x.FirstName, x.MI, x.LastName, x.Year, x.Course, x.RoomNumber, x.Points, x.Attendance, x.Position);
            temp.FirstName = nFirst;
            temp.MI = nMi;
            temp.LastName = nLast;
            temp.Year = nYear;
            temp.Course = nCourse;
            temp.RoomNumber = nRoom;
            temp.Points = nPts;
            temp.Attendance = nAtt;
            temp.Position = nPos;

            dormers.Remove(getDormer(x.IdNumber));
            dormers.Add(temp);

            if (affectedRows > 0)
            {
                return true;
            }
            return false;
        }

        public bool updateDormerPoints(Dormer x, int nPts, int nAtt, string nPos)
        {
            MySqlConnection conn = new MySqlConnection(conn_string);
            conn.Open();

            string query = "Update dormerlist Set " +
                "points = ?h, " +
                "attendance = ?i, " +
                "position = ?j, " +
                "Where idNumber = ?a";
            MySqlCommand comm = new MySqlCommand(query, conn);
            comm.Parameters.AddWithValue("?a", x.IdNumber);
            comm.Parameters.AddWithValue("?h", nPts);
            comm.Parameters.AddWithValue("?i", nAtt);
            comm.Parameters.AddWithValue("?j", nPos);
            int affectedRows = comm.ExecuteNonQuery();

            Dormer temp = new Dormer(x.IdNumber, x.FirstName, x.MI, x.LastName, x.Year, x.Course, x.RoomNumber, x.Points, x.Attendance, x.Position);
            temp.Points = nPts;
            temp.Attendance = nAtt;
            temp.Position = nPos;

            dormers.Remove(getDormer(x.IdNumber));
            dormers.Add(temp);

            if (affectedRows > 0)
            {
                return true;
            }
            return false;
        }

        public Dormer getDormer(int x)
        {
            int localCounter = 0;
            foreach (Dormer d in dormers)
            {
                if (x.Equals(d.IdNumber))
                {
                    return dormers[localCounter];
                }
                localCounter++;
            }
            return null;
        }

        public Event getEvent(int i)
        {
            int localCounter = 0;
            foreach (Event e in events)
            {
                if (i.Equals(e.EventId))
                {
                    return events[localCounter];
                }
                localCounter++;
            }
            return null;
        }
    }
}
