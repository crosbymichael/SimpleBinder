using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBinder
{
    public interface IValueProvider
    {
        object GetValue(BindingContext bindingContext, ModelContext modelContext);
    }
}
