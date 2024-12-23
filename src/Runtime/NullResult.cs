using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;
using modoff.Util;

namespace modoff.Runtime {
    internal class NullResult<T> : IActionResult {
        string result;

        public NullResult() {
            result = GenerateNullXml(typeof(T));
        }

        public string GetStringData() {
            Console.WriteLine(result);
            return result;
        }

        private static string GenerateNullXml(Type type) {
            bool isArray = type.IsArray;
            string rootName;

            if (isArray) {
                Type elementType = type.GetElementType();
                rootName = $"ArrayOf{elementType.Name}";
            } else {
                var xmlRoot = type.GetCustomAttribute<XmlRootAttribute>();
                rootName = xmlRoot?.ElementName ?? type.Name;
            }

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");

            XmlWriterSettings settings = new XmlWriterSettings {
                OmitXmlDeclaration = false,
                Indent = false,
                Encoding = Encoding.UTF8
            };

            using (Utf8StringWriter stringWriter = new Utf8StringWriter()) {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings)) {
                    xmlWriter.WriteStartElement(rootName);
                    xmlWriter.WriteAttributeString("xsi", "nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
                    xmlWriter.WriteEndElement();
                }
                return stringWriter.ToString();
            }
        }
    }
}
