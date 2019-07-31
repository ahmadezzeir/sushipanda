using System.Collections.Generic;

namespace Domain.Models
{
    public class Dish : EntityBase
    {
        public string Name { get; set; }

        public double Calories { get; set; }

        public double Weight { get; set; }

        public ICollection<OrderDish> Orders { get; set; } = new List<OrderDish>();
    }
}