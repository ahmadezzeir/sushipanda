namespace Domain.Models
{
    public class File : EntityBase
    {
        public string Caption { get; set; }

        public string Name { get; set; }

        public Dish Dish { get; set; }
    }
}