using BookSwap.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookSwap.Infrastructure.Configuration
{
    public class BookOwnershipHistoryConfigurations : IEntityTypeConfiguration<BookOwnershipHistory>
    {
        public void Configure(EntityTypeBuilder<BookOwnershipHistory> builder)
        {
            builder.HasKey(boh => boh.Id);

            builder.HasOne(boh => boh.Book)
                   .WithMany()
                   .HasForeignKey(boh => boh.BookId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(boh => boh.PreviousOwner)
                   .WithMany()
                   .HasForeignKey(boh => boh.PreviousOwnerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(boh => boh.NewOwner)
                   .WithMany()
                   .HasForeignKey(boh => boh.NewOwnerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(boh => boh.ExchangeOffer)
                   .WithMany()
                   .HasForeignKey(boh => boh.ExchangeOfferId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}


