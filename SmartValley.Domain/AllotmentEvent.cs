﻿using System;
using SmartValley.Domain.Core;

namespace SmartValley.Domain
{
    public class AllotmentEvent : Entity
    {
        public string Name { get; set; }

        public AllotmentEventStatus Status { get; private set; }

        public string TokenContractAddress { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset FinishDate { get; set; }

        public long TotalTokens { get; set; }

        public string TokenTicker { get; set; }

        public long ProjectId { get; set; }

        public void SetStatus(AllotmentEventStatus newStatus)
        {

        }
    }
}