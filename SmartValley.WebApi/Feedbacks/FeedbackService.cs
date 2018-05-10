﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.Feedbacks.Requests;

namespace SmartValley.WebApi.Feedbacks
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _repository;

        public FeedbackService(IFeedbackRepository repository)
        {
            _repository = repository;
        }

        public Task AddAsync(CreateFeedbackRequest request)
        {
            _repository.Add(new Feedback(request.FirstName, request.LastName, request.Email, request.Text));
            return _repository.SaveChangesAsync();
        }

        public Task<IReadOnlyCollection<Feedback>> GetAsync(int offset, int count)
            => _repository.GetAsync(offset, count);

        public Task<int> GetTotalCountAsync() => _repository.GetTotalCountAsync();
    }
}