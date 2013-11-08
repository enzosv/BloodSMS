using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    public static enum bloodType { A, B, O };
    public class Storage
    {
        List<Blood> bloodList;
        List<Blood>[] bloodTypes;
        List<Donor> donorList;
        Storage()
        {
            bloodList = new List<Blood>();
            bloodTypes = new List<Blood>[Enum.GetNames(typeof(bloodType)).Length];
            donorList = new List<Donor>();
        }

        void AddDonor(int donor_id, bloodType blood_type, string name, string street, string city, string province, string email, string cellphone, string reason_for_deferral, DateTime date_registered, DateTime next_available, DateTime birth_date, bool is_viable, bool is_contactable, bool is_voluntary)
        {
            Donor newDonor = new Donor(donor_id, blood_type, name, street, city, province, email, cellphone, reason_for_deferral, date_registered, next_available, birth_date, is_viable, is_contactable, is_voluntary);
            donorList.Add(newDonor);
        }

        void AddBlood(DateTime date_donated, DateTime date_expire, int donor_id, string component)
        {
            Blood newBlood = new Blood(donor_id, date_donated, date_expire, component);
            bloodList.Add(newBlood);
        }

        void getBloodInInventory()
        {
            foreach (Blood b in bloodList)
            {
                if (b.date_removed == null)
                {
                    bloodTypes[(int)findDonor(b.donor_id).blood_type].Add(b);
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