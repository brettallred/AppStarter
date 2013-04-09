using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace AppStarter.Helpers.Attributes
{
    public static class AttributeExtensions
    {
        public static T GetAttribute<T>(this object value) where T : Attribute
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null) return null;
            var attributes = (T[]) fi.GetCustomAttributes(typeof (T), false);

            return attributes.FirstOrDefault(x => x.GetType() == typeof (T));
        }

        public static string GetDisplayName(this object value)
        {
            var attribute = value.GetAttribute<DisplayAttribute>();
            return attribute == null ? value.ToString() : attribute.Name;
        }
    }
}