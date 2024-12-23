using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "pir", Namespace = "")]
    [Serializable]
    public class ModoffPrizeItemResponse {
        [XmlElement(ElementName = "i")]
        public int ItemID { get; set; }

        [XmlElement(ElementName = "pi")]
        public int PrizeItemID { get; set; }

        [XmlElement(ElementName = "pis", IsNullable = true)]
        public List<ModoffItemData> MysteryPrizeItems { get; set; }
    }
}
