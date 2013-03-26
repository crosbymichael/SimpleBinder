using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleBinder
{
    static class Utils
    {
        public static IEnumerable<T> GetAttributes<T>(Type type)
        {
            return type.GetCustomAttributes(true).OfType<T>();
        }

        public static IEnumerable<T> GetAttributes<T>(PropertyInfo type)
        {
            return type.GetCustomAttributes(true).OfType<T>();
        }

        public static string ReplaceWithProperties(
            string template,
            object values)
        {
            if (values == null) return null;
            string buffer = template;

            var properties = values.GetType().GetProperties();

            foreach (var property in properties)
            {
                string name = ("{" + property.Name + "}");
                if (property.PropertyType == typeof(String))
                {
                    if (buffer.Contains(name))
                    {
                        string value = property.GetValue(values, null) as string;
                        buffer = buffer.Replace(name, value);
                    }
                }
            }
            return buffer;
        }
    }
}
