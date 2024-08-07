using modoff.Util;

namespace modoff.Runtime {
    internal class XmlResult : IActionResult {
        string xmlResult = "";
        
        public XmlResult(object obj) {
            xmlResult = XmlUtil.SerializeXmlGeneric(obj);
        }

        public string GetStringData() {
            return xmlResult;
        }
    }
}
