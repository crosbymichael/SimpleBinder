using SimpleBinder.ModelBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleBinder
{
    /// <summary>
    /// Model Context to for the current type being bound
    /// </summary>
    public class ModelContext
    {
        public virtual Type ModelType { get; set; }
        public virtual string Name { get; set; }

        internal IModelBinder DefaultBinder()
        {
            var attribute = Utils.GetAttributes<BinderForAttribute>(this.ModelType).SingleOrDefault();
            if (attribute != null)
            {
                var binderType = attribute.BinderType;
                return Activator.CreateInstance(binderType) as IModelBinder;
            }
            return null;
        }

        public override string ToString()
        {
            return this.Name;
        }

        internal virtual string GetTemplate()
        {
            var attribute = Utils.GetAttributes<TemplateAttribute>(this.ModelType).SingleOrDefault();
            if (attribute != null)
            {
                return attribute.Template;
            }
            return null;
        }
    }

    public class PropertyContext : ModelContext
    {
        public PropertyInfo Property { get; set; }
        
        public override string Name
        {
            get
            {
                return this.Property.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        public override Type ModelType
        {
            get
            {
                return this.Property.PropertyType;
            }
            set
            {
                base.ModelType = value;
            }
        }

        internal override string GetTemplate()
        {
            var attribute = Utils.GetAttributes<TemplateAttribute>(this.Property).SingleOrDefault();
            if (attribute != null)
            {
                return attribute.Template;
            }
            return null;
        }
    }

    class ListBindingContext : BindingContext
    {
        string template;
        Iterator iterator;

        public ListBindingContext(
            BindingContext context,
            string template,
            string propertyName,
            Iterator iterator)
        {
            if (string.IsNullOrEmpty(template))
            {
                throw new ArgumentNullException("template");
            }
            this.ValueProvider = context.ValueProvider;

            this.template = template;
            this.PropertyName = propertyName;
            this.iterator = iterator;
        }

        public int Iteration { get { return this.iterator.CurrentIteration; } }
        public string PropertyName { get; private set; }

        public override string GetKey(
            ModelContext context)
        {
            return Utils.ReplaceWithProperties(
                this.template,
                new 
                { 
                    propertyName = this.PropertyName,
                    iteration = this.Iteration,
                    contextName = context.Name
                });
        }
    }

    public class BindingContext
    {
        public IValueProvider ValueProvider { get; set; }

        public virtual string GetKey(ModelContext context)
        {
            return context.Name;
        }
    }
}
