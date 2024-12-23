using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using CI.WSANative.Web;

namespace modoff.Model {

    //[Index(nameof(Uid))]
    public class Viking {
        [Key]
        public int Id { get; set; }

        public Guid Uid { get; set; }

        [Required]
        public string Name { get; set; } = null;

        [Required]
        public Guid UserId { get; set; }

        public string AvatarSerialized { get; set; }

        public int? SelectedDragonId { get; set; }

        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual User User { get; set; }
        public virtual ICollection<Dragon> Dragons { get; set; } = new List<Dragon>();
        public virtual ICollection<Image> Images { get; set; } = new List<Image>();
        public virtual ICollection<MissionState> MissionStates { get; set; } = new List<MissionState>();
        public virtual ICollection<TaskStatus> TaskStatuses { get; set; } = new List<TaskStatus>();
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
        public virtual ICollection<SceneData> SceneData { get; set; } = new List<SceneData>();
        public virtual ICollection<AchievementPoints> AchievementPoints { get; set; } = new List<AchievementPoints>();
        public virtual ICollection<PairData> PairData { get; set; } = new List<PairData>();
        public virtual ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
        public virtual ICollection<GameData> GameData { get; set; } = new List<GameData>();
        public virtual ICollection<ProfileAnswer> ProfileAnswers { get; set; } = new List<ProfileAnswer>();
        public virtual ICollection<SavedData> SavedData { get; set; } = new List<SavedData>();
        public virtual ICollection<Party> Parties { get; set; } = new List<Party>();
        public virtual Neighborhood Neighborhood { get; set; }
        public virtual Dragon SelectedDragon { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}
