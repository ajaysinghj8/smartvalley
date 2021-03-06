﻿using System;
using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.Domain.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EthereumTransactionService : IEthereumTransactionService
    {
        private readonly IEthereumTransactionRepository _repository;
        private readonly IClock _clock;

        public EthereumTransactionService(IEthereumTransactionRepository repository, IClock clock)
        {
            _repository = repository;
            _clock = clock;
        }

        public async Task<long> StartAsync(string hash, long userId, EthereumTransactionEntityType entityType, long entityId, EthereumTransactionType transactionType)
        {
            var transaction = new EthereumTransaction(userId, hash, _clock.UtcNow, entityType, entityId, transactionType);

            _repository.Add(transaction);

            await _repository.SaveChangesAsync();

            return transaction.Id;
        }

        public async Task CompleteAsync(string hash)
        {
            var transaction = await _repository.GetByHashAsync(hash) ?? throw new InvalidOperationException($"Transaction '{hash}' not found");

            transaction.Complete();

            await _repository.SaveChangesAsync();
        }

        public async Task FailAsync(string hash)
        {
            var transaction = await _repository.GetByHashAsync(hash) ?? throw new InvalidOperationException($"Transaction '{hash}' not found");

            transaction.Fail();

            await _repository.SaveChangesAsync();
        }

        public Task<PagingCollection<EthereumTransaction>> GetAsync(EtheriumTransactionsQuery query) 
            => _repository.GetAsync(query);
    }
}