using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBinder
{
    /// <summary>
    /// Adds nullable support to System.ServiceModel.Dispatcher.QueryStringConverter
    /// </summary>
    class QueryStringConverter : System.ServiceModel.Dispatcher.QueryStringConverter
    {
        public override bool CanConvert(Type type)
        {
            return base.CanConvert(type);
        }

        public override object ConvertStringToValue(string parameter, Type parameterType)
        {
            return base.ConvertStringToValue(parameter, parameterType);
        }

        public override string ConvertValueToString(object parameter, Type parameterType)
        {
            return base.ConvertValueToString(parameter, parameterType);
        }
    }
}
