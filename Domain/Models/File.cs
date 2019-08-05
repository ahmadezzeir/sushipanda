namespace Domain.Models
{
    public class File : EntityBase
    {
        public string Caption { get; set; }

        public string Name { get; set; }

        public virtual Dish Dish { get; set; }
    }
}