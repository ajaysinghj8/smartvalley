﻿using System;
using System.Threading.Tasks;
using SmartValley.Domain;
using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Votings
{
    public interface IVotingService
    {
        Task<VotingSprintDetails> GetSprintDetailsByAddressAsync(string address);

        Task<long> GetVoteAsync(string sprintAddress, string investorAddress, Guid projectId);

        Task<InvestorVotes> GetVotesAsync(string sprintAddress, string investorAddress);

        Task<VotingSprintDetails> GetLastSprintDetailsAsync();

        Task StartSprintAsync();

        Task<IReadOnlyCollection<Voting>> GetFinishedSprintsAsync();

        Task<VotingProjectDetails> GetVotingProjectDetailsAsync(long projectId);
    }
}