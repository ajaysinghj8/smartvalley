﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartValley.Domain.Interfaces;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Projects
{
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectRepository projectRepository, IProjectService projectService)
        {
            _projectRepository = projectRepository;
            _projectService = projectService;
        }

        [HttpGet]
        [Route("scored")]
        public async Task<CollectionResponse<ProjectResponse>> GetAllScoredAsync()
        {
            var scoredProjects = await _projectRepository.GetAllScoredAsync();
            return new CollectionResponse<ProjectResponse>
                   {
                       Items = scoredProjects.Select(ProjectResponse.From).ToArray()
                   };
        }

        [HttpGet]
        public Task<ProjectDetailsResponse> GetByIdAsync(GetByIdRequest request)
            => _projectService.GetProjectDetailsByIdAsync(request.ProjectId);
    }
}