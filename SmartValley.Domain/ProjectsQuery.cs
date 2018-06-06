﻿using System.Collections.Generic;
using SmartValley.Domain.Entities;

namespace SmartValley.Domain
{
    public class ProjectsQuery
    {
        public ProjectsQuery(int offset,
                             int count,
                             bool? isPrivate,
                             bool onlyScored,
                             string searchString,
                             Stage? stage,
                             string countryCode,
                             Category? category,
                             int? minimumScore,
                             int? maximumScore,
                             ProjectsOrderBy? orderBy,
                             SortDirection? direction,
                             IReadOnlyCollection<ScoringStatus> scoringStatuses)
        {
            Offset = offset;
            Count = count;
            SearchString = searchString;
            Stage = stage;
            CountryCode = countryCode;
            Category = category;
            MinimumScore = minimumScore;
            MaximumScore = maximumScore;
            OrderBy = orderBy;
            Direction = direction;
            OnlyScored = onlyScored;
            IsPrivate = isPrivate;
            ScoringStatuses = scoringStatuses ?? new List<ScoringStatus>();
        }

        public int Offset { get; }

        public int Count { get; }

        public bool OnlyScored { get; }

        public bool? IsPrivate { get; }

        public string SearchString { get; }

        public Stage? Stage { get; }

        public IReadOnlyCollection<ScoringStatus> ScoringStatuses { get; }

        public string CountryCode { get; }

        public Category? Category { get; }

        public int? MinimumScore { get; }

        public int? MaximumScore { get; }

        public ProjectsOrderBy? OrderBy { get; }

        public SortDirection? Direction { get; }
    }
}