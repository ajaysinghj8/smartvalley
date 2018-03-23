﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartValley.Domain;
using SmartValley.Domain.Exceptions;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.ScoringApplication.Requests;
using ScoringApplicationQuestion = SmartValley.Domain.Entities.ScoringApplicationQuestion;

namespace SmartValley.WebApi.ScoringApplication
{
    public class ScoringApplicationService : IScoringApplicationService
    {
        private readonly IScoringApplicationRepository _scoringApplicationRepository;
        private readonly IScoringApplicationQuestionsRepository _scoringApplicationQuestionsRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IClock _clock;

        public ScoringApplicationService(IScoringApplicationRepository scoringApplicationRepository,
                                         IScoringApplicationQuestionsRepository scoringApplicationQuestionsRepository,
                                         ICountryRepository countryRepository,
                                         IClock clock)
        {
            _scoringApplicationRepository = scoringApplicationRepository;
            _scoringApplicationQuestionsRepository = scoringApplicationQuestionsRepository;
            _countryRepository = countryRepository;
            _clock = clock;
        }

        public Task<IReadOnlyCollection<ScoringApplicationQuestion>> GetQuestionsAsync() 
            => _scoringApplicationQuestionsRepository.GetAllAsync();

        public async Task<Domain.ScoringApplication> GetApplicationAsync(long projectId)
        {
            var scoringApplication = await _scoringApplicationRepository.GetByProjectIdAsync(projectId);
            if (scoringApplication == null)
            {
                scoringApplication = new Domain.ScoringApplication();
            }

            return scoringApplication;
        } 

        public async Task SaveApplicationAsync(long projectId, SaveScoringApplicationRequest saveScoringApplicationRequest)
        {
            var country = await _countryRepository.GetByCodeAsync(saveScoringApplicationRequest.CountryCode);
            if (country == null)
                throw new AppErrorException(ErrorCode.CountryNotFound);

            var scoringApplication = await _scoringApplicationRepository.GetByProjectIdAsync(projectId);
            if (scoringApplication == null)
                throw new AppErrorException(ErrorCode.ProjectScoringApplicationNotFound);

            scoringApplication.ProjectName = saveScoringApplicationRequest.ProjectName;
            scoringApplication.ProjectArea = saveScoringApplicationRequest.ProjectArea;
            scoringApplication.Status = saveScoringApplicationRequest.Status;
            scoringApplication.ProjectDescription = saveScoringApplicationRequest.ProjectDescription;
            scoringApplication.CountryId = country.Id;
            scoringApplication.Site = saveScoringApplicationRequest.Site;
            scoringApplication.WhitePaper = saveScoringApplicationRequest.WhitePaper;
            scoringApplication.ICODate = saveScoringApplicationRequest.ICODate;
            scoringApplication.ContactEmail = saveScoringApplicationRequest.ContactEmail;
            scoringApplication.FacebookLink = saveScoringApplicationRequest.FacebookLink;
            scoringApplication.BitcointalkLink = saveScoringApplicationRequest.BitcointalkLink;
            scoringApplication.MediumLink = saveScoringApplicationRequest.MediumLink;
            scoringApplication.RedditLink = saveScoringApplicationRequest.RedditLink;
            scoringApplication.TelegramLink = saveScoringApplicationRequest.TelegramLink;
            scoringApplication.TwitterLink = saveScoringApplicationRequest.TwitterLink;
            scoringApplication.GitHubLink = saveScoringApplicationRequest.GitHubLink;
            scoringApplication.LinkedInLink = saveScoringApplicationRequest.LinkedInLink;

            scoringApplication.Saved = _clock.UtcNow;

            var newTeamMembers = saveScoringApplicationRequest.TeamMembers.Select(x => new ScoringApplicationTeamMember
            {
                FullName = x.FullName,
                ProjectRole = x.ProjectRole,
                About = x.About,
                FacebookLink = x.FacebookLink,
                LinkedInLink = x.LinkedInLink,
                AdditionalInformation = x.AdditionalInformation
            }).ToList();

            scoringApplication.UpdateTeamMembers(newTeamMembers);

            var newAdvisers = saveScoringApplicationRequest.Advisers.Select(x => new ScoringApplicationAdviser
            {
                FullName = x.FullName,
                About = x.About,
                Reason = x.Reason,
                FacebookLink = x.FacebookLink,
                LinkedInLink = x.LinkedInLink
            }).ToList();

            scoringApplication.UpdateAdvisers(newAdvisers);

            foreach (var answer in saveScoringApplicationRequest.Answers)
            {
                scoringApplication.SetAnswer(answer.Key, answer.Value);
            }

            await _scoringApplicationRepository.SaveAsync(scoringApplication);
        }

        public async Task SubmitForScoreAsync(long projectId)
        {
            var scoringApplication = await _scoringApplicationRepository.GetByProjectIdAsync(projectId);
            if (scoringApplication == null)
                throw new AppErrorException(ErrorCode.ProjectScoringApplicationNotFound);

            if (!scoringApplication.Submitted.HasValue)
            {
                scoringApplication.Submitted = _clock.UtcNow;
                await _scoringApplicationRepository.SaveAsync(scoringApplication);
            }
        }
    }
}