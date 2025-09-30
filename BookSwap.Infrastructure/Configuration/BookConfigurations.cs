using BookSwap.Core.Entities;
using BookSwap.Core.Enums;
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
                   .HasMaxLength(100)
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
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(x => x.CoverImageUrl)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(400)
                   .IsRequired();

            builder.Property(b => b.Condition)
                   .HasColumnType("int")
                   .IsRequired()
                   .HasDefaultValue(BookCondition.Good);

            builder.Property(b => b.Status)
                   .HasColumnType("int")
                   .IsRequired()
                   .HasDefaultValue(BookStatus.Available);
            
            builder.Property(x => x.IsAvailable)
                   .HasColumnType("bit")
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(b => b.IsApproved)
                   .HasColumnType("bit")
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                   .HasColumnType("datetime2")
                   .IsRequired();

            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("datetime2")
                   .IsRequired(false);

            builder.Property(b => b.RejectionReason)
                 .HasColumnType("nvarchar")
                 .HasMaxLength(500)
                 .HasDefaultValue(string.Empty);

            builder.HasOne(x => x.Owner)
                   .WithMany(u => u.OwnedBooks)
                   .HasForeignKey(x => x.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                       .WithMany(u => u.Books)
                       .HasForeignKey(x => x.CategoryId)
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }

}


