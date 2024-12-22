using modoff.Util;

namespace modoff.Runtime {
    internal class StringResult : IActionResult {
        string stringResult = "";

        public StringResult(object obj) {
            stringResult = obj.ToString();
        }

        public string GetStringData() {
            return stringResult;
        }
    }
}
