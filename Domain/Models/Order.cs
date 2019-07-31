using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Order : EntityBase
    {
        public string PhoneNumber { get; set; }

        public Guid UserId { get; set; }

        public ICollection<OrderDish> Dishes { get; set; } = new List<OrderDish>();
    }
}