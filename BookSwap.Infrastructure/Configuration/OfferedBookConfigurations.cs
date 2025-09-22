using BookSwap.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookSwap.Infrastructure.Configuration
{
    public class OfferedBookConfigurations : IEntityTypeConfiguration<OfferedBook>
    {
        public void Configure(EntityTypeBuilder<OfferedBook> builder)
        {
            builder.HasKey(ob => ob.Id);

            builder.Property(ob => ob.IsSelected)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.HasOne(ob => ob.ExchangeOffer)
                   .WithMany(eo => eo.OfferedBooks)
                   .HasForeignKey(ob => ob.ExchangeOfferId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ob => ob.Book)
                   .WithMany(x=>x.OfferedBooks)
                   .HasForeignKey(ob => ob.BookId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}


