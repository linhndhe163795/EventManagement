using System;
using System.Collections.Generic;

namespace MyAPI.Models
{
    public partial class User
    {
        public User()
        {
            UserEvents = new HashSet<UserEvent>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
        public string? Avatar { get; set; }

        public virtual ICollection<UserEvent> UserEvents { get; set; }
    }
}
