using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleBinder.ValueProvider
{
    public class StreamValueProvider : IValueProvider
    {
        QueryStringValueProvider queryStringProvider;

        public StreamValueProvider(
            Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            using (var reader = new StreamReader(stream))
            {
                string values = reader.ReadToEnd();
                this.queryStringProvider = new QueryStringValueProvider(values);
            }
        }

        public object GetValue(
            BindingContext bindingContext, 
            ModelContext modelContext)
        {
            return this.queryStringProvider.GetValue(
                bindingContext, 
                modelContext);
        }
    }
}
