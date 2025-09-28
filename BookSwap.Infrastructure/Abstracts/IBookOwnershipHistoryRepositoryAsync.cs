using BookSwap.Core.Entities;
using BookSwap.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap.Infrastructure.Abstracts
{
    public interface IBookOwnershipHistoryRepositoryAsync:IGenericRepositoryAsync<BookOwnershipHistory>
    {
    }
}
