using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    public class Blood
    {
        int blood_type_id;
        int donor_id;
        DateTime date_donated;
        DateTime date_expire;
        string component;

        bool is_assigned;
        string patient_name;
        int patient_age;

        int age;
        bool is_quarantined;
        string reason_for_removal;
        DateTime date_removed;
        //Record Blood Ins
        //Create
        public Blood(int BLOOD_TYPE_ID, int DONOR_ID, DateTime DATE_DONATED, DateTime DATE_EXPIRE, string COMPONENT)
        {
            blood_type_id = BLOOD_TYPE_ID;
            donor_id = DONOR_ID;
            date_donated = DATE_DONATED;
            date_expire = DATE_EXPIRE;
            component = COMPONENT;

            is_assigned = false;
            patient_name = "";
            patient_age = 0;
            age = 0;
            is_quarantined = false;
            reason_for_removal = "";
        }

        //get set all the shit

    }
}
