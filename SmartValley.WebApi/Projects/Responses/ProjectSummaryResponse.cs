﻿using System;
using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.Projects.Responses
{
    public class ProjectSummaryResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string CountryCode { get; set; }

        public int StageId { get; set; }

        public DateTimeOffset? IcoDate { get; set; }

        public string Website { get; set; }

        public string WhitePaperLink { get; set; }

        public string Facebook { get; set; }

        public string Telegram { get; set; }

        public string Twitter { get; set; }

        public ScoringStatus ScoringStatus { get; set; }

        public double? Score { get; set; }

        public long? ScoringId { get; set; }

        public long AuthorId { get; set; }

        public string AuthorAddress { get; set; }

        public static ProjectSummaryResponse Create(
            Project project,
            Country country,
            Domain.Entities.Scoring scoring,
            User author)
        {
            return new ProjectSummaryResponse
                   {
                       Id = project.Id,
                       Name = project.Name,
                       ImageUrl = project.ImageUrl,
                       CountryCode = country.Code,
                       StageId = (int) project.Stage,
                       IcoDate = project.IcoDate,
                       Website = project.Website,
                       WhitePaperLink = project.WhitePaperLink,
                       Facebook = project.Facebook,
                       Telegram = project.Telegram,
                       Twitter = project.Twitter,
                       ScoringStatus = scoring == null
                                           ? ScoringStatus.Pending
                                           : (scoring.Score.HasValue ? ScoringStatus.Finished : ScoringStatus.InProgress),
                       ScoringId = scoring?.Id,
                       Score = scoring?.Score,
                       AuthorId = project.AuthorId,
                       AuthorAddress = author.Address
                   };
        }
    }
}