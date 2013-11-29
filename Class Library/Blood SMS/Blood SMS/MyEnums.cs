using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Blood_SMS
{
    public enum bloodComponents
    {
        [Description("Whole")]
        Whole,
        [Description("Red Blood Cells")]
        RedBloodCells,
        [Description("Platelets")]
        LiquidPlasma,
        [Description("Fresh Frozen Plasma")]
        FreshFrozenPlasma,
        [Description("White Blood Cells")]
        WhiteBloodCells,
        [Description("Platelets")]
        Platelets,
        [Description("Cryoprecipitate")]
        Cryoprecipitate,
        [Description("Cryosupernate")]
        Cryosupernate,
    };
    public enum graphCommand { Add, Release, Quarantine, Remove, Summary };
    public enum bloodType { [Description("AB+")]ABp, [Description("AB-")] ABn, [Description("A+")] Ap, [Description("A-")] An, [Description("B+")] Bp, [Description("B-")] Bn, [Description("O+")] Op, [Description("O-")] On };
    public enum contactMethod { [Description("None")]none,[Description("Email")] email, [Description("Cellphone")] cellphone };
    public enum educationalAttainment { [Description("Other")]other, [Description("None")] none, [Description("Grade School")] gradeschool, [Description("Highschool")] highschool, [Description("College")] college };
    public enum city { QuezonCity, SanJuan, Manila, Caloocan, Mandaluyong, Malabon, Pateros, Makati, Valenzuela, Navotas, Pasay, Taguig, Paranaque, Muntinlupa, LasPinas, Other };

    public static class MyEnums
    {
                /*
         * Quezon City 4.5km
    Marikina 9.6
    San Juan 10.1
    Manila 10.1
    Caloocan 11.3km
    Mandaluyong 11.6
    Malabon 12.3
    Pateros 12.9
    Makati 13.7km
    Valenzuela 14.5
    Navotas 15.4
    Pasay 16.4
    Taguig 21.3
    Paranaque 25.2
    Muntinlupa 30.4km
    las pinas 34.9km
    */
        

        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        //http://stackoverflow.com/questions/4367723/get-enum-from-description-attribute
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
            // or return default(T);
        }
    }
}
