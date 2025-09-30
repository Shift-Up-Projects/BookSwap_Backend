﻿using BookSwap.Core.Entities;
using BookSwap.Core.Enums;
using BookSwap.Infrastructure.Abstracts;
using BookSwap.Infrastructure.Context;
using BookSwap.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

public class ExchangeOfferRepositoryAsync : GenericRepositoryAsync<ExchangeOffer>, IExchangeOfferRepositoryAsync
{
    public ExchangeOfferRepositoryAsync(BookSwapDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> HasAcceptedExchangeAsync(int bookId)
    {
        return await GetTableNoTracking()
            .Include(x => x.OfferedBooks)
            .AnyAsync(eo => (eo.RequestedBookId == bookId || eo.OfferedBooks.Any(ob => ob.BookId == bookId)) && eo.Status == ExchangeOfferStatus.Accepted);
    }

    public async Task<IEnumerable<ExchangeOffer>> GetByUserAsync(int userId)
    {
        return await GetTableNoTracking()
            .Include(eo => eo.Sender)
            .Include(eo => eo.Receiver)
            .Include(eo => eo.RequestedBook)
            .Include(eo => eo.OfferedBooks)
            .ThenInclude(ob => ob.Book)
            .Where(eo => eo.SenderId == userId || eo.ReceiverId == userId)
            .ToListAsync();
    }
    public async Task<IEnumerable<ExchangeOffer>> GetOffersBySenderAsync(int senderId)
    {
        return await GetTableNoTracking()
            .Where(e => e.SenderId == senderId)
            .Include(e => e.OfferedBooks)
            .Include(e => e.RequestedBook)
            .Include(e => e.Sender)
            .Include(e => e.Receiver)
            .ToListAsync();
    }

    public async Task<IEnumerable<ExchangeOffer>> GetOffersByReceiverAsync(int receiverId)
    {
        return await GetTableNoTracking()
            .Where(e => e.ReceiverId == receiverId)
            .Include(e => e.OfferedBooks)
            .Include(e => e.RequestedBook)
            .Include(e => e.Sender)
            .Include(e => e.Receiver)
            .ToListAsync();
    }

    public async Task<IEnumerable<ExchangeOffer>> GetOffersByStatusAsync(ExchangeOfferStatus status)
    {
        return await GetTableNoTracking()
            .Where(e => e.Status == status)
            .Include(e => e.OfferedBooks)
            .Include(e => e.RequestedBook)
            .Include(e => e.Sender)
            .Include(e => e.Receiver)
            .ToListAsync();
    }
}
