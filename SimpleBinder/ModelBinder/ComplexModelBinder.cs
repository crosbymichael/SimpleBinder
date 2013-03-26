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

            var instance = Activator.CreateInstance(modelContext.ModelType);

            foreach (var property in properties)
            { 
                var context = new PropertyContext{
                    Property = property
                };
                var binder = ModelBinders.Binders.GetBinder(context);
                var value = binder.BindModel(bindingContext, context);

                if (value != null)
                {
                    property.SetValue(instance, value, null);
                }
            }
            return instance;
        }

        public bool CanBind(Type type)
        {
            return type.IsClass;
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
