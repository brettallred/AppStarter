using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AppStarter.Helpers.Attributes;

namespace AppStarter.Helpers.Extensions
{
    public static class Extensions
    {
        public static List<SelectListItem> GetEnumSelectList<T>(this Enum enumeration)
        {
            var res = new List<SelectListItem>();

            foreach (var field in typeof (T).GetFields())
            {
                object value = field.GetValue(null);
                string description = value.ToString();
                var attribute =
                    Attribute.GetCustomAttribute(field, typeof (DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    description = attribute.Description;
                }

                res.Add(new SelectListItem {Text = description, Value = value.ToString()});
            }

            return res;
        }

        public static string DisplayPercentage(this decimal value)
        {
            return string.Format("{0} %", value*100);
        }

        public static DateTime ToDateTime(this long value)
        {
            return new DateTime(value);
        }


        public static long TicksFromUnixSeconds(this long value)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime msDateTime = unixEpoch.AddSeconds(value);

            return msDateTime.Ticks;
        }
    }
}