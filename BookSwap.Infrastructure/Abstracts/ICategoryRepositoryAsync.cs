using BookSwap.Core.Entities;
using BookSwap.Infrastructure.InfrastructureBases;

namespace BookSwap.Infrastructure.Abstracts
{
    public interface ICategoryRepositoryAsync : IGenericRepositoryAsync<Category>
    {
       public Task<Category?> GetCategoryByIdAsync(int Id);
    }
}
