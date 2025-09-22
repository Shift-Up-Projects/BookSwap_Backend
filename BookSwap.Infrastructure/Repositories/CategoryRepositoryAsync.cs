using BookSwap.Core.Entities;
using BookSwap.Infrastructure.Abstracts;
using BookSwap.Infrastructure.Context;
using BookSwap.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

public class CategoryRepositoryAsync : GenericRepositoryAsync<Category>, ICategoryRepositoryAsync
{
    public CategoryRepositoryAsync(BookSwapDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Category?> GetCategoryByIdAsync(int Id)
    {
        return await GetTableNoTracking().FirstOrDefaultAsync(c => c.Id == Id);
    }
}
