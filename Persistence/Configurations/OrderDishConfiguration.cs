using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class OrderDishConfiguration : IEntityTypeConfiguration<OrderDish>
    {
        public void Configure(EntityTypeBuilder<OrderDish> builder)
        {
            builder.HasKey("OrderId", "DishId");

            builder.HasOne(x => x.Order)
                .WithMany(x => x.Dishes)
                .HasForeignKey("OrderId");

            builder.HasOne(x => x.Dish)
                .WithMany(x => x.Orders)
                .HasForeignKey("DishId");
        }
    }
}