using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SimpleBinder.ValueProvider
{
    public class QueryStringValueProvider : NameValueCollectionValueProvider
    {
        public QueryStringValueProvider(string values)
            : base(HttpUtility.ParseQueryString(values))
        { 
            
        }
    }
}
