using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBinder
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BinderForAttribute : Attribute
    {
        public Type BinderType { get; set; }

        public BinderForAttribute(Type binderType)
        {
            if (binderType == null)
            {
                throw new ArgumentNullException("binderType");
            }

            this.BinderType = binderType;    
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TemplateAttribute : Attribute
    {
        public string Template { get; set; }

        public TemplateAttribute(string template)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            this.Template = template;
        }
    }
}
