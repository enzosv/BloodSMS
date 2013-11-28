using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    class Patient
    {
        int patient_id;
        string accession_number;
        int age;
        string last_name;
        string first_name;
        string middle_initial;
        string name;

        public string Accession_number { get { return accession_number; } }
        public int Patient_id { get {return patient_id; } set { patient_id = value; } }
        public int Age { get { return age; } set { age = value; } }
        public string Last_name { get { return last_name; } set { last_name = value; } }
        public string First_name { get { return first_name; } set { first_name = value; } }
        public string Middle_initial { get { return middle_initial; } set { middle_initial = value; } }
        public string Name { get { return name; } set { name = value; } }


        public Patient(int ACCESSION_NUMBER, string LAST_NAME, string FIRST_NAME, string MIDDLE_INITIAL, int AGE)
        {
            accession_number = ACCESSION_NUMBER;
            last_name = LAST_NAME;
            first_name = FIRST_NAME;
            middle_initial = MIDDLE_INITIAL;
            age = AGE;
            name = last_name + ", " + first_name + " " + middle_initial;
        }
    }
}
