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
            return this.FailedCount == 5;
        }
    }

    abstract class ListModelBinder : IModelBinder
    {
        protected virtual string GetTemplate(
            ModelContext context)
        {

            return "{propertyName}{iteration}";
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
            };

            bindingContext = new ListBindingContext(
                bindingContext, 
                GetTemplate(context), 
                name, 
                iterator);

            Action<object> add = o =>
                instance
                    .GetType()
                    .GetMethod("Add")
                    .Invoke(instance, new[] { o });

            while (iterator.Iterate())
            {
                var value = GetValue(bindingContext, context);

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
            if (type == typeof(List<>) ||
                type == typeof(IEnumerable<>) ||
                type == typeof(ICollection<>))
            {
                var genericType = type.GetGenericArguments().Single();
                return CanBindGenericType(genericType);
            }
            return false;
        }

        protected abstract bool CanBindGenericType(Type genericType);

        protected abstract object GetValue(BindingContext bindingContext, ModelContext modelContext);

        object CreateInstance(Type type)
        {
            type = typeof(List<>).MakeGenericType(type.GetGenericArguments());
            return Activator.CreateInstance(type);
        }
    }

    class SimpleListModelBinder : ListModelBinder
    {
        SimpleModelBinder simpleModelBinder;

        public SimpleListModelBinder()
        {
            this.simpleModelBinder = new SimpleModelBinder();
        }

        protected override object GetValue(
            BindingContext bindingContext, 
            ModelContext modelContext)
        {
            return this.simpleModelBinder.BindModel(
                bindingContext, 
                modelContext);
        }

        protected override bool CanBindGenericType(
            Type genericType)
        {
            return this.simpleModelBinder.CanBind(genericType);
        }
    }

    class ComplexListModelBinder : ListModelBinder
    {
        ComplexModelBinder modelBinder;

        public ComplexListModelBinder()
        {
            this.modelBinder = new ComplexModelBinder();
        }

        protected override bool CanBindGenericType(
            Type genericType)
        {
            return this.modelBinder.CanBind(genericType);
        }

        protected override object GetValue(
            BindingContext bindingContext, 
            ModelContext modelContext)
        {
            var defaultBinder = modelContext.DefaultBinder();
            if (defaultBinder == null)
            {
                defaultBinder = this.modelBinder;
            }



            return defaultBinder.BindModel(bindingContext, modelContext);
        }
    }
}
