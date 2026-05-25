using AutoMapper;
using CapstoneAPI.Data;
using CapstoneAPI.DTOs;
using CapstoneAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly CapstoneDbContext _context;
        private readonly IMapper _mapper;

        public ProjectService(CapstoneDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects
                .Include(p => p.Owner)
                .ToListAsync();
            return _mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<ProjectDetailsDto?> GetProjectByIdAsync(Guid id)
        {
            var project = await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Tasks)
                .ThenInclude(t => t.AssignedTo)
                .FirstOrDefaultAsync(p => p.Id == id);

            return project != null ? _mapper.Map<ProjectDetailsDto>(project) : null;
        }

        public async Task<List<ProjectDto>> GetProjectsByUserIdAsync(Guid userId)
        {
            var projects = await _context.Projects
                .Where(p => p.OwnerId == userId)
                .Include(p => p.Owner)
                .ToListAsync();
            return _mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<ProjectDto> CreateProjectAsync(Guid userId, CreateProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            project.Id = Guid.NewGuid();
            project.OwnerId = userId;
            project.CreatedAt = DateTime.UtcNow;
            project.UpdatedAt = DateTime.UtcNow;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Reload with owner information
            await _context.Entry(project).Reference(p => p.Owner).LoadAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto?> UpdateProjectAsync(Guid id, UpdateProjectDto projectDto)
        {
            var project = await _context.Projects.Include(p => p.Owner).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null) return null;

            _mapper.Map(projectDto, project);
            project.UpdatedAt = DateTime.UtcNow;

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
