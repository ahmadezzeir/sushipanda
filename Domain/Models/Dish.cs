namespace Domain.Models
{
    public class Dish : EntityBase
    {
        public string Name { get; set; }

        public double Calories { get; set; }

        public double Weight { get; set; }
    }
}