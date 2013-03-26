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

        public NameValueCollectionValueProvider(NameValueCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            this.collection = collection;
        }

        public string GetValue(
            BindingContext bindingContext, 
            ModelContext modelContext)
        {
            var key = bindingContext.GetKey(modelContext);
            return this.collection.Get(key);
        }
    }
}
