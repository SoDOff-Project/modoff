using System;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "S", Namespace = "", IsNullable = true)]
    [Serializable]
    public class ModoffItemsInStoreData {
        [XmlElement(ElementName = "i", IsNullable = true)]
        public int? ID;

        [XmlElement(ElementName = "s")]
        public string StoreName;

        [XmlElement(ElementName = "d")]
        public string Description;

        [XmlElement(ElementName = "is")]
        public ModoffItemData[] Items;

        [XmlElement(ElementName = "ss")]
        public ItemsInStoreDataSale[] SalesAtStore;

        [XmlElement(ElementName = "pitem")]
        public PopularStoreItem[] PopularItems;
    }
}
