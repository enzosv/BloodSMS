using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSA_DB
{
    class Dormer
    {
        private int idNumber;
        private string firstName;
        private string mI;
        private string lastName;
        private int year;
        private string course;
        private string roomNumber;
        private int points;
        private int attendance;
        private string position;

        public Dormer(int id, string fn, string m, string ln, int y, string c, string rm, int pts, int a, string pos)
        {
            idNumber = id;
            firstName = fn;
            mI = m;
            lastName = ln;
            year = y;
            course = c;
            roomNumber = rm;
            points = pts;
            position = pos;
        }

        public int IdNumber
        {
            get { return idNumber; }
            set { idNumber = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string MI
        {
            get { return mI; }
            set { mI = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public string Course
        {
            get { return course; }
            set { course = value; }
        }

        public string RoomNumber
        {
            get { return roomNumber; }
            set { roomNumber = value; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public int Attendance
        {
            get { return attendance; }
            set { attendance = value; }
        }

        public string Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
