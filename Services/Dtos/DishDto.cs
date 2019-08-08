using System;

namespace Services.Dtos
{
    public class DishDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Calories { get; set; }

        public int Weight { get; set; }

        public decimal Price { get; set; }

        public string ImgPath { get; set; }

        public Guid ImgId { get; set; }
    }
}