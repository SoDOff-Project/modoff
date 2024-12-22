using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace modoff.Model {
    public class User {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; } = null;

        [Required]
        public string Username { get; set; } = null;

        [Required]
        public string Password { get; set; } = null;

        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual ICollection<Viking> Vikings { get; set; } = new List<Viking>();
        public virtual ICollection<PairData> PairData { get; set; } = new List<PairData>();
    }
}
