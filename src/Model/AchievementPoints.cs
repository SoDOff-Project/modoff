﻿using System.ComponentModel.DataAnnotations;

namespace modoff.Model {
    public class AchievementPoints {
        public int VikingId { get; set; }

        public int Type { get; set; }

        public int Value { get; set; }

        public virtual Viking Viking { get; set; }
    }
}
