using System.Xml.Serialization;

namespace modoff.Schema {

    // NOTE: This schema is NOT used by the client
    // This is a schema specific to the sodoff server
    [XmlRoot(ElementName = "Items", Namespace = "")]
    public class ServerItemArray {
        [XmlElement(ElementName = "I", IsNullable = true)]
        public ModoffItemData[] ItemDataArray;
    }
}
