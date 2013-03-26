using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleBinder.ModelBinder
{
    class ComplexModelBinder : IModelBinder
    {
        public ComplexModelBinder()
        {

        }

        public object BindModel(
            BindingContext bindingContext,
            ModelContext modelContext)
        {
            var properties = GetProperties(modelContext.ModelType);
            int totalProperties = properties.Count();
            int nullProperties = 0;

            var instance = Activator.CreateInstance(modelContext.ModelType);
            object value = null;

            foreach (var property in properties)
            {
                var context = new PropertyContext
                {
                    Property = property
                };

                var binder = ModelBinders.Binders.GetBinder(context);
                if (binder != null)
                {
                    value = binder.BindModel(bindingContext, context);
                }
                else
                {
                    value = bindingContext.ValueProvider.GetValue(bindingContext, context);
                }

                if (value != null)
                {
                    property.SetValue(instance, value, null);
                }
                else
                {
                    nullProperties++;
                }
            }
            return totalProperties == nullProperties ? null : instance;
        }

        public bool CanBind(Type type)
        {
            return type.IsClass &&
                !type.IsGenericType;
        }

        IEnumerable<PropertyInfo> GetProperties(
            Type type)
        {
            return type.GetProperties(
                BindingFlags.Instance |
                BindingFlags.SetProperty |
                BindingFlags.GetProperty |
                BindingFlags.Public);
        }
    }
}
