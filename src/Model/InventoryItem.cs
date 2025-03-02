﻿using System.ComponentModel.DataAnnotations;

namespace modoff.Model {
    public class InventoryItem {
        [Key]
        public int Id {  get; set; }

        public int ItemId { get; set; }

        public int VikingId { get; set; }

        public string StatsSerialized { get; set; }

        public string AttributesSerialized { get; set; }

        public virtual Viking Viking { get; set; } = null;

        public int Quantity { get; set; }
    }
}
