using AutoMapper;
using CapstoneAPI.Data;
using CapstoneAPI.DTOs;
using CapstoneAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CapstoneAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly CapstoneDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(CapstoneDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TaskDto>> GetTasksByProjectIdAsync(Guid projectId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.AssignedTo)
                .ToListAsync();
            return _mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<TaskDto?> GetTaskByIdAsync(Guid id)
        {
            var task = await _context.Tasks
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id);
            return task != null ? _mapper.Map<TaskDto>(task) : null;
        }

        public async Task<TaskDto> CreateTaskAsync(Guid projectId, CreateTaskDto taskDto)
        {
            var task = _mapper.Map<Models.Task>(taskDto);
            task.Id = Guid.NewGuid();
            task.ProjectId = projectId;
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            await _context.Entry(task).Reference(t => t.AssignedTo).LoadAsync();

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<TaskDto?> UpdateTaskAsync(Guid id, UpdateTaskDto taskDto)
        {
            var task = await _context.Tasks
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) return null;

            _mapper.Map(taskDto, task);
            task.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
