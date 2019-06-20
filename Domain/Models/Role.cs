using System.Collections.Generic;
using Domain.Models;

namespace Domain
{
    public class Role : EntityBase
    {
        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}