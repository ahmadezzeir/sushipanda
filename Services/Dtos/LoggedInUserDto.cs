using System;
using System.Collections.Generic;

namespace Services.Dtos
{
    public class LoggedInUserDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}