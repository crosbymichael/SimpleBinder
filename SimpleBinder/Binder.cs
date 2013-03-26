using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBinder
{
    public static class Binder
    {
        public static T Bind<T>(
            IValueProvider valueProvider)
        {
            var bindingContext = new BindingContext
            {
                ValueProvider = valueProvider
            };

            var context = new ModelContext
            {
                ModelType = typeof(T)
            };

            var binder = ModelBinders.Binders.GetBinder(context);

            return (T)binder.BindModel(bindingContext, context);
        }
    }
}
