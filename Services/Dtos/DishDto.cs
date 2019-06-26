using System;

namespace Services.Dtos
{
    public class DishDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Calories { get; set; }

        public double Weight { get; set; }
    }
}