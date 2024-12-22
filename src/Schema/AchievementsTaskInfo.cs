using System;
using System.Xml.Serialization;

namespace modoff.Schema;

[XmlRoot(ElementName = "AchievementsTaskInfo", Namespace = "")]
[Serializable]
public class AchievementsTaskInfo
{
	[XmlElement(ElementName = "TID")]
	public int TaskID;
	
	[XmlElement(ElementName = "AR")]
	public AchievementReward[] AchievementReward;
}
