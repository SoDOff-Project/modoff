using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace modoff.Schema {
    public class ModoffApplyRewardsResponse {
        [XmlElement(ElementName = "ST", IsNullable = false)]
        public Status Status { get; set; }

        [XmlElement(ElementName = "UID", IsNullable = false)]
        public Guid UserID { get; set; }

        [XmlElement(ElementName = "ARS", IsNullable = false)]
        public AchievementReward[] AchievementRewards { get; set; }

        [XmlElement(ElementName = "RISM", IsNullable = true)]
        public ModoffUserItemStatsMap RewardedItemStatsMap { get; set; }

        [XmlElement(ElementName = "CIR", IsNullable = true)]
        public CommonInventoryResponse CommonInventoryResponse { get; set; }
    }
}
