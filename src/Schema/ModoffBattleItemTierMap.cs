﻿using System;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "BITM", Namespace = "")]
    [Serializable]
    public class ModoffBattleItemTierMap {
        [XmlElement(ElementName = "IID", IsNullable = false)]
        public int ItemID { get; set; }

        [XmlElement(ElementName = "T", IsNullable = true)]
        public ItemTier? Tier { get; set; }

        [XmlElement(ElementName = "QTY", IsNullable = true)]
        public int? Quantity { get; set; }

        [XmlElement(ElementName = "iss", IsNullable = true)]
        public ItemStat[] ItemStats { get; set; }
    }
}
