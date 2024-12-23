using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "ABIR", Namespace = "")]
    [Serializable]
    public class ModoffAddBattleItemsRequest {
        [XmlElement(ElementName = "BITM", IsNullable = false)]
        public List<ModoffBattleItemTierMap> BattleItemTierMaps { get; set; }
    }
}
