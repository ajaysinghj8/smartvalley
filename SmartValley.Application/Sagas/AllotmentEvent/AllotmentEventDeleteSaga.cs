﻿using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Services;
using SmartValley.Messages.Commands;
using SmartValley.Messages.Events;

namespace SmartValley.Application.Sagas.AllotmentEvent
{
    public class AllotmentEventDeleteSaga : SqlSaga<AllotmentEventDeleteSagaData>,
        IAmStartedByMessages<DeleteAllotmentEvent>,
        IHandleMessages<TransactionCompleted>,
        IHandleMessages<TransactionFailed>
    {
        private readonly IAllotmentEventService _allotmentEventService;
        private readonly IEthereumTransactionService _ethereumTransactionService;

        public AllotmentEventDeleteSaga(IAllotmentEventService allotmentEventService, IEthereumTransactionService ethereumTransactionService)
        {
            _allotmentEventService = allotmentEventService;
            _ethereumTransactionService = ethereumTransactionService;
        }

        protected override string CorrelationPropertyName => nameof(AllotmentEventDeleteSagaData.TransactionHash);

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
            mapper.ConfigureMapping<DeleteAllotmentEvent>(x => x.TransactionHash);

            mapper.ConfigureMapping<TransactionCompleted>(m => m.TransactionHash);
            mapper.ConfigureMapping<TransactionFailed>(m => m.TransactionHash);
        }

        public async Task Handle(DeleteAllotmentEvent command, IMessageHandlerContext context)
        {
            Data.AllotmentEventId = command.AllotmentEventId;
            Data.UserId = command.UserId;
            Data.TransactionHash = command.TransactionHash;

            await _ethereumTransactionService.StartAsync(command.TransactionHash, command.UserId, EthereumTransactionType.DeleteAllotmentEvent);
            await context.SendLocal(new WaitForTransaction { TransactionHash = command.TransactionHash });
        }

        public async Task Handle(TransactionCompleted message, IMessageHandlerContext context)
        {
            await _allotmentEventService.UpdateAsync(Data.AllotmentEventId);
            MarkAsComplete();
        }

        public Task Handle(TransactionFailed message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            return Task.CompletedTask;
        }
    }
}