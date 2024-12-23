using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "ABIRES", Namespace = "")]
    [Serializable]
    public class ModoffAddBattleItemsResponse {
        [XmlElement(ElementName = "ST", IsNullable = false)]
        public Status Status { get; set; }

        [XmlElement(ElementName = "IISM", IsNullable = true)]
        public List<ModoffInventoryItemStatsMap> InventoryItemStatsMaps { get; set; }
    }
}
