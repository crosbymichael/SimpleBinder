using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBinder.ModelBinder
{
    /// <summary>
    /// Converts simple objects and value types
    /// </summary>
    class SimpleModelBinder : IModelBinder
    {
        QueryStringConverter converter;

        public SimpleModelBinder()
        {
            this.converter = new QueryStringConverter();
        }

        public object BindModel(
            BindingContext bindingContext, 
            ModelContext modelContext)
        {
            var valueProvider = bindingContext.ValueProvider;
            var rawValue = valueProvider.GetValue(bindingContext, modelContext);

            return this.converter.ConvertStringToValue(
                rawValue, 
                modelContext.ModelType);
        }

        public bool CanBind(Type type)
        {
            return this.converter.CanConvert(type);
        }
    }
}
