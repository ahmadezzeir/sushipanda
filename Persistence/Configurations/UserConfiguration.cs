using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.NormalizedName).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.NormalizedEmail).IsRequired();
            builder.Property(x => x.PasswordHash).IsRequired();
        }
    }
}