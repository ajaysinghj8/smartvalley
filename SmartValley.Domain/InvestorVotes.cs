﻿using System;
using System.Collections.Generic;

namespace SmartValley.Domain
{
    public class InvestorVotes
    {
        public long TokenAmount { get; set; }
        public List<Guid> ProjectExternalIds { get; set; }
    }
}