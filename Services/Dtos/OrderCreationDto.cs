using System;
using System.Collections.Generic;

namespace Services.Dtos
{
    public class OrderCreationDto
    {
        public string PhoneNumber { get; set; }

        public IEnumerable<Guid> Dishes { get; set; }
    }
}