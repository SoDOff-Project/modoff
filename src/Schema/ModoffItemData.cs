using System;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "I", Namespace = "", IsNullable = true)]
    public class ModoffItemData : ItemData {
        [XmlElement(ElementName = "r")]
        public new ModoffItemDataRelationship[] Relationship;

        [XmlIgnore]
        public float NormalDiscoutModifier;

        [XmlIgnore]
        public float MemberDiscountModifier;

        [XmlIgnore]
        public float FinalDiscoutModifier {
            get {
                return Math.Min(1f, (1f - NormalDiscoutModifier) * (1f - MemberDiscountModifier));
            }
        }
    }
}
