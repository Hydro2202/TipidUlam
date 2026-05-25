using CapstoneAPI.DTOs;
using CapstoneAPI.Models;

namespace CapstoneAPI.Services
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDetailsDto?> GetProjectByIdAsync(Guid id);
        Task<List<ProjectDto>> GetProjectsByUserIdAsync(Guid userId);
        Task<ProjectDto> CreateProjectAsync(Guid userId, CreateProjectDto projectDto);
        Task<ProjectDto?> UpdateProjectAsync(Guid id, UpdateProjectDto projectDto);
        Task<bool> DeleteProjectAsync(Guid id);
    }
}
