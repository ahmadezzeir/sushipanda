using System;

namespace Services.Dtos
{
    public class RefreshTokenDto
    {
        public Guid Id { get; set; }

        public DateTime Expiration { get; set; }

        public string Value { get; set; }
    }
}