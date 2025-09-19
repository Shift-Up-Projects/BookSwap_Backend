using BookSwap.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookSwap.Infrastructure.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Address)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(200)
                   .IsRequired(false);

            builder.Property(x => x.FirstName)
                    .HasColumnType("nvarchar")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.LastName)
                    .HasColumnType("nvarchar")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Country)
                    .HasColumnType("nvarchar")
                   .HasMaxLength(200)
                   .IsRequired(false);

            builder.Property(x => x.ImageUrl)
                  .HasColumnType("nvarchar")
                  .HasMaxLength(400)
                  .IsRequired(true);
             
            builder.Property(x => x.Code)
                   .HasColumnType("nvarchar")
                  .HasMaxLength(25)
                  .IsRequired(false);

        }

    }
    
}
