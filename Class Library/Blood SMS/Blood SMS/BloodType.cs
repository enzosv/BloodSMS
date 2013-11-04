using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blood_SMS
{
    public class BloodType
    {
        //Create blood type
        string name;
        int id;
     
        public BloodType(int ID, string NAME)
        {
            id = ID;
            name = NAME;
        }

        void AddBlood(DateTime date_donated, DateTime date_expire, int donor_id, string component )
        {
            Blood newBlood = new Blood(id, donor_id, date_donated, date_expire, component);
        }

        //I think this should be in a storage class with a foreach bloodtype
        List<Blood> bloods = new List<Blood>();
        List<Blood> getBloodInInventory()
        {
            List<Blood> stock = new List<Blood>();
            foreach (Blood b in bloods)
            {
                if (b.blood_type_id == id && b.date_removed == null)
                {
                    stock.Add(b);
                }
            }
            return stock;
        }
    }
}
