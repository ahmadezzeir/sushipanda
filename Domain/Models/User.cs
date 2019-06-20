using System.Collections.Generic;

namespace Domain.Models
{
    public class User : EntityBase
    {
        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string Email { get; set; }

        public string NormalizedEmail { get; set; }

        public string PasswordHash { get; set; }

        public bool EmailConfirmed { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}