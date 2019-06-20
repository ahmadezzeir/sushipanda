using System;

namespace Domain.Models
{
    public class RefreshToken : EntityBase
    {
        public DateTime Expiration { get; set; }

        public string Value { get; set; }
    }
}
