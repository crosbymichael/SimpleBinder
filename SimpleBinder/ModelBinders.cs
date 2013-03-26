using SimpleBinder.ModelBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBinder
{
    public class ModelBinders
    {
        public static ModelBinders Binders
        {
            get;
            private set;
        }

        static ModelBinders()
        {
            if (Binders == null)
            {
                Binders = new ModelBinders();
            }
        }

        List<IModelBinder> binders;

        public IEnumerable<IModelBinder> RegisteredBinders
        {
            get { return this.binders.AsReadOnly(); }
        }

        ModelBinders()
        {
            this.binders = new List<IModelBinder>();
            RegisterDefaultBinders();
        }

        void RegisterDefaultBinders()
        {
            Register<ComplexModelBinder>();
            Register<ListModelBinder>();
        }

        public void Register<T>()
            where T : IModelBinder
        {
            var instance = Activator.CreateInstance<T>();
            this.binders.Add(instance);
        }

        public IModelBinder GetBinder(
            ModelContext context)
        {
            var binder = context.DefaultBinder();
            var converter = new QueryStringConverter();
            if (binder == null && !converter.CanConvert(context.ModelType))
            {
                binder = this.binders.SingleOrDefault(b => b.CanBind(context.ModelType));
            }
            return binder;
        }
    }
}
