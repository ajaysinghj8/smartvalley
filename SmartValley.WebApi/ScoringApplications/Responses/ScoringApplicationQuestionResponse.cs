﻿using SmartValley.Domain.Entities;

namespace SmartValley.WebApi.ScoringApplications.Responses
{
    public class ScoringApplicationQuestionResponse
    {
        public long Id { get; set; }

        public string Key { get; set; }

        public QuestionControlType Type { get; set; }

        public string ExtendedInfo { get; set; }

        public long? ParentId { get; set; }

        public string ParentTriggerValue { get; set; }

        public string Answer { get; set; }

        public int Order { get; set; }
    }
}