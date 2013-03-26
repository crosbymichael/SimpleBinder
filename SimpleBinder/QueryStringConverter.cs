using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBinder
{
    /// <summary>
    /// Adds nullable support to System.ServiceModel.Dispatcher.QueryStringConverter
    /// </summary>
    public class QueryStringConverter : System.ServiceModel.Dispatcher.QueryStringConverter
    {
        public override bool CanConvert(
            Type type)
        {
            type = GetType(type);
            return base.CanConvert(type);
        }

        public override object ConvertStringToValue(
            string parameter, 
            Type parameterType)
        {
            var type = GetType(parameterType);

            return base.ConvertStringToValue(parameter, type);
        }

        public override string ConvertValueToString(
            object parameter, 
            Type parameterType)
        {
            throw new NotImplementedException();

            return base.ConvertValueToString(parameter, parameterType);
        }

        Type GetType(Type type)
        {
            if (type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments().Single();
            }
            return type;
        }
    }
}
