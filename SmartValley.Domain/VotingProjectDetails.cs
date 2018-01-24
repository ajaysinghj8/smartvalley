﻿using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class VotingProjectDetails
    {
        public VotingProjectDetails(long projectId, Voting voting, bool isAccepted)
        {
            ProjectId = projectId;
            Voting = voting;
            IsAccepted = isAccepted;
        }

        public long ProjectId { get; }

        public Voting Voting { get; }

        public bool IsAccepted { get; }
    }
}