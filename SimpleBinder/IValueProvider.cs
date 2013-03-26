using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBinder
{
    public interface IValueProvider
    {
        string GetValue(BindingContext bindingContext, ModelContext modelContext);
    }
}
