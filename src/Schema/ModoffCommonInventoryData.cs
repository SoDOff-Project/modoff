using System;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "CI", Namespace = "")]
    [Serializable]
    public class ModoffCommonInventoryData {
        [XmlElement(ElementName = "uid")]
        public Guid UserID;

        [XmlElement(ElementName = "i")]
        public ModoffUserItemData[] Item;
    }
}
