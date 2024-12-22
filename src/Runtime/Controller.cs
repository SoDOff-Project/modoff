using System.Linq;

namespace modoff.Runtime {
    public class Controller {
        public IActionResult Ok(object obj) {
            var callerAttributes = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.GetCustomAttributes(false);

            if (callerAttributes != null && callerAttributes.Any(attr => attr.GetType().Name == "PlainText"))
                return new StringResult(obj);

            return new XmlResult(obj);
        }
    }
}
