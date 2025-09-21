using BookSwap.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookSwap.Infrastructure.Configuration
{
    public class BookConfigurations : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.Author)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.ISBN)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(13)
                   .IsRequired(false);

            builder.Property(x => x.Description)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(1000)
                   .IsRequired(false);

            builder.Property(x => x.CoverImageUrl)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(400)
                   .IsRequired();

            builder.Property(x => x.Condition)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.IsAvailable)
                   .HasColumnType("bit")
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("datetime2")
                   .IsRequired(false);

            builder.HasOne(x => x.Owner)
                   .WithMany(u => u.Books)
                   .HasForeignKey(x => x.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}


