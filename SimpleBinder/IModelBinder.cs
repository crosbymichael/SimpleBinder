using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleBinder
{
    public interface IModelBinder
    {
        object BindModel(BindingContext bindingContext, ModelContext modelContext);

        bool CanBind(Type type);
    }
}
