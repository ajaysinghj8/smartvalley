﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SmartValley.Domain.Core;

namespace SmartValley.Domain.Entities
{
    public class Scoring : IEntityWithId
    {
        public long Id { get; set; }

        public long ProjectId { get; set; }

        [Required]
        [MaxLength(42)]
        public Address ContractAddress { get; set; }

        public double? Score { get; set; }

        public ScoringStatus Status { get; set; }

        public DateTimeOffset? ScoringStartDate { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset OffersDueDate { get; set; }

        public DateTimeOffset? EstimatesDueDate { get; set; }

        public DateTimeOffset? ScoringEndDate { get; set; }

        public Project Project { get; set; }

        public ICollection<AreaScoring> AreaScorings { get; set; }

        public ICollection<ScoringOffer> ScoringOffers { get; set; }

        public ICollection<ExpertScoringConclusion> ExpertScoringConclusions { get; set; }

        public IReadOnlyCollection<ExpertScoringConclusion> GetConclusionsForArea(AreaType area)
        {
            return ExpertScoringConclusions.Where(x => x.Area == area).ToArray();
        }

        public void AddConclusionForArea(ExpertScoringConclusion conclusion)
        {
            ExpertScoringConclusions.Add(conclusion);
        }

        public void SetScoreForArea(AreaType areaType, double score)
        {
            var areaScoring = AreaScorings.FirstOrDefault(x => x.AreaId == areaType);
            if (areaScoring == null)
            {
                throw new InvalidOperationException($"Can't find score for area: '{areaType}'");
            }

            areaScoring.Score = score;
        }

        public double? GetScoreForArea(AreaType areaType)
        {
            var areaScoring = AreaScorings.FirstOrDefault(x => x.AreaId == areaType);
            if (areaScoring == null)
            {
                throw new InvalidOperationException($"Can't find score for area: '{areaType}'");
            }

            return areaScoring.Score;
        }
    }
}