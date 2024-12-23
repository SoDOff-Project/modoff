using System;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "GetStoreResponse", Namespace = "", IsNullable = true)]
    [Serializable]
    public class ModoffGetStoreResponse {
        [XmlElement(ElementName = "Stores")]
        public ModoffItemsInStoreData[] Stores;
    }
}
