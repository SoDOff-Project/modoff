using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "UISM", Namespace = "")]
    [Serializable]
    public class ModoffUserItemStatsMap {
        [XmlElement(ElementName = "UISMID", IsNullable = false)]
        public int UserItemStatsMapID { get; set; }

        [XmlElement(ElementName = "UID")]
        public Guid UserID { get; set; }

        [XmlElement(ElementName = "ITM", IsNullable = false)]
        public ModoffItemData Item { get; set; }

        [XmlElement(ElementName = "ISS", IsNullable = false)]
        public ItemStat[] ItemStats { get; set; }

        [XmlElement(ElementName = "IT", IsNullable = true)]
        public ItemTier? ItemTier { get; set; }

        [XmlElement(ElementName = "CD", IsNullable = true)]
        public DateTime? CreatedDate { get; set; }
    }
}
