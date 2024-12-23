using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "FIRES", Namespace = "")]
    [Serializable]
    public class ModoffFuseItemsResponse {
        [XmlElement(ElementName = "ST", IsNullable = false)]
        public Status Status { get; set; }

        [XmlElement(ElementName = "UID", IsNullable = false)]
        public Guid UserID { get; set; }

        [XmlElement(ElementName = "IISM", IsNullable = true)]
        public List<ModoffInventoryItemStatsMap> InventoryItemStatsMaps { get; set; }

        [XmlElement(ElementName = "VMSG", IsNullable = true)]
        public ValidationMessage VMsg { get; set; }
    }
}
