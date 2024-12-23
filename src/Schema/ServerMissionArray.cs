using System.Xml.Serialization;

namespace modoff.Schema;

// NOTE: This schema is NOT used by the client
// This is a schema specific to the sodoff server
[XmlRoot(ElementName = "Missions", Namespace = "")]
public class ServerMissionArray {

    [XmlElement(ElementName = "Mission", IsNullable = true)]
    public ModoffMission[] MissionDataArray;
}
