using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "CIRS", Namespace = "")]
    [Serializable]
    public class ModoffCommonInventoryResponse {
        [XmlElement(ElementName = "pir", IsNullable = true)]
        public List<ModoffPrizeItemResponse> PrizeItems { get; set; }

        [XmlElement(ElementName = "s")]
        public bool Success;

        [XmlElement(ElementName = "cids")]
        public CommonInventoryResponseItem[] CommonInventoryIDs;

        [XmlElement(ElementName = "ugc", IsNullable = true)]
        public UserGameCurrency UserGameCurrency;
    }
}
