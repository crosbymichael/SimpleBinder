using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace SimpleBinder.ValueProvider
{
    public class NameValueCollectionValueProvider : IValueProvider
    {
        NameValueCollection collection;
        QueryStringConverter converter;

        public NameValueCollectionValueProvider(
            NameValueCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            this.collection = collection;
            this.converter = new QueryStringConverter();
        }

        public object GetValue(
            BindingContext bindingContext, 
            ModelContext modelContext)
        {
            var key = bindingContext.GetKey(modelContext);
            var rawValue = this.collection.Get(key);

            if (string.IsNullOrEmpty(rawValue))
            {
                return modelContext.ModelType.IsValueType ?
                    Activator.CreateInstance(modelContext.ModelType) :
                    null;
            }
            return this.converter.ConvertStringToValue(
                rawValue, 
                modelContext.ModelType);
        }
    }
}
