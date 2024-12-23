using System.Diagnostics;
using System.Linq;

namespace modoff.Runtime {
    public class Controller {
        public IActionResult Ok(object obj) {
            var callerAttributes = new StackTrace().GetFrame(1).GetMethod().GetCustomAttributes(false);

            if (callerAttributes != null && callerAttributes.Any(attr => attr.GetType().Name == "PlainText"))
                return new StringResult(obj);

            return new XmlResult(obj);
        }

        public IActionResult OkNull<T>() {
            return new NullResult<T>();
        }
    }
}
