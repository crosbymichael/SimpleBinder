using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBinder.ModelBinder
{
    class Iterator
    {
        public int CurrentIteration { get; private set; }
        public int FailedCount { get; private set; }

        public void Next()
        {
            this.CurrentIteration++;
        }

        public void Failure()
        {
            Next();
            this.FailedCount++;
        }

        public bool Iterate()
        {
            return this.FailedCount < 5;
        }
    }

    class ListModelBinder : IModelBinder
    {
        protected virtual string GetTemplate(
            ModelContext context)
        {
            var template = context.GetTemplate();
            if (string.IsNullOrEmpty(template))
            {
                template = "{contextName}{iteration}";
            }
            return template;
        }

        public object BindModel(
            BindingContext bindingContext,
            ModelContext modelContext)
        {
            var instance = CreateInstance(modelContext.ModelType);
            var name = bindingContext.GetKey(modelContext);
            var iterator = new Iterator();

            var context = new ModelContext
            {
                ModelType = instance.GetType().GetGenericArguments().Single(),
                Name = name
            };

            bindingContext = new ListBindingContext(
                bindingContext, 
                GetTemplate(context), 
                iterator);

            Action<object> add = o =>
                instance
                    .GetType()
                    .GetMethod("Add")
                    .Invoke(instance, new[] { o });

            while (iterator.Iterate())
            {
                var value = GetValue(bindingContext as ListBindingContext, context);

                if (value != null)
                {
                    add(value);
                    iterator.Next();
                }
                else
                {
                    iterator.Failure();
                }
            }
            return instance;
        }

        public bool CanBind(
            Type type)
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();

                if (type == typeof(List<>) ||
                    type == typeof(IEnumerable<>) ||
                    type == typeof(ICollection<>))
                {
                    return true;
                }
            }
            return false;
        }

        object CreateInstance(Type type)
        {
            type = typeof(List<>).MakeGenericType(type.GetGenericArguments());
            return Activator.CreateInstance(type);
        }

        object GetValue(
            ListBindingContext bindingContext,
            ModelContext modelContext)
        {
            var valueProvider = bindingContext.ValueProvider;
            var binder = ModelBinders.Binders.GetBinder(modelContext);
            if (binder == null)
            {
                return valueProvider.GetValue(bindingContext, modelContext); 
            }
            return binder.BindModel(bindingContext, modelContext);
        }
    }
}
