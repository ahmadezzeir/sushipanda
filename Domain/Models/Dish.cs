using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class Dish : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Calories { get; set; }

        public int Weight { get; set; }

        public decimal Price { get; set; }

        public virtual File File { get; set; }

        public Guid FileId { get; set; }

        public virtual ICollection<OrderDish> Orders { get; set; } = new List<OrderDish>();
    }
}