using BookSwap.Core.Entities;
using BookSwap.Infrastructure.InfrastructureBases;

namespace BookSwap.Infrastructure.Abstracts
{
    public interface IExchangeOfferRepositoryAsync : IGenericRepositoryAsync<ExchangeOffer>
    {
        Task<bool> HasAcceptedExchangeAsync(int bookId);
        Task<IEnumerable<ExchangeOffer>> GetByUserAsync(int userId);
    }
}
