using System;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "IISM", Namespace = "")]
    [Serializable]
    public class ModoffInventoryItemStatsMap {
        [XmlElement(ElementName = "CID", IsNullable = false)]
        public int CommonInventoryID { get; set; }

        [XmlElement(ElementName = "ITM", IsNullable = false)]
        public ModoffItemData Item { get; set; }

        [XmlElement(ElementName = "ISM", IsNullable = false)]
        public ItemStatsMap ItemStatsMap { get; set; }
    }
}
