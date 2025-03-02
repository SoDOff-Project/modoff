﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace modoff.Schema {
    [XmlRoot(ElementName = "Mission", Namespace = "")]
    [Serializable] // FIXME: Remove serializable once we have a different way of deep copying than BinaryFormatter
    public class ModoffMission {
        [XmlElement(ElementName = "I")]
        public int MissionID;

        [XmlElement(ElementName = "N")]
        public string Name;

        [XmlElement(ElementName = "G")]
        public int GroupID;

        [XmlElement(ElementName = "P", IsNullable = true)]
        public int? ParentID;

        [XmlElement(ElementName = "S")]
        public string Static;

        [XmlElement(ElementName = "A")]
        public bool Accepted;

        [XmlElement(ElementName = "C")]
        public int Completed;

        [XmlElement(ElementName = "R")]
        public string Rule;

        [XmlElement(ElementName = "MR")]
        public MissionRule MissionRule;

        [XmlElement(ElementName = "V")]
        public int VersionID;

        [XmlElement(ElementName = "AID")]
        public int AchievementID;

        [XmlElement(ElementName = "AAID")]
        public int AcceptanceAchievementID;

        [XmlElement(ElementName = "M")]
        public List<ModoffMission> Missions;

        [XmlElement(ElementName = "Task")]
        public List<ModoffTask> Tasks;

        [XmlElement(ElementName = "AR")]
        public List<AchievementReward> Rewards;

        [XmlElement(ElementName = "AAR")]
        public List<AchievementReward> AcceptanceRewards;

        [XmlElement(ElementName = "RPT")]
        public bool Repeatable;
    }
}
