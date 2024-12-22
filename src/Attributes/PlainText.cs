using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modoff.Attributes {
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PlainText : Attribute {
    }
}
