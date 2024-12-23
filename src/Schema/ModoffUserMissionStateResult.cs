using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "UserMissionStateResult", Namespace = "")]
    [Serializable]
    public class ModoffUserMissionStateResult {
        [XmlElement(ElementName = "UID")]
        public Guid UserID;

        [XmlElement(ElementName = "Mission")]
        public List<ModoffMission> Missions;

        [XmlElement(ElementName = "UTA")]
        public List<UserTimedAchievement> UserTimedAchievement;

        // NOTE: It appears that the server doesn't send this
        // [XmlElement(ElementName = "D")]
        // public int Day;

        [XmlElement(ElementName = "MG")]
        public List<MissionGroup> MissionGroup;
    }
}
