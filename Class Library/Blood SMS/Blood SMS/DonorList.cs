using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    public class DonorList
    {
        string connectionString;
        List<Donor> donors;
        //Retrieve DonorList SQL
        public DonorList(string host, string db, string user, string pass)
        {
            donors = new List<Donor>();
            connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3}", host, db, user, pass);
        }
    }
}
