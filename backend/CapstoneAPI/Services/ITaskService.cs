using CapstoneAPI.DTOs;

namespace CapstoneAPI.Services
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetTasksByProjectIdAsync(Guid projectId);
        Task<TaskDto?> GetTaskByIdAsync(Guid id);
        Task<TaskDto> CreateTaskAsync(Guid projectId, CreateTaskDto taskDto);
        Task<TaskDto?> UpdateTaskAsync(Guid id, UpdateTaskDto taskDto);
        Task<bool> DeleteTaskAsync(Guid id);
    }
}
