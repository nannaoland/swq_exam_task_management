
namespace TaskManagement
{
       public interface ITaskManager
    {
        Task<bool> CreateTask(TaskItem task);

        Task<IEnumerable<TaskItem>> GetAllTasks();

        Task UpdateTask(TaskItem task);

        Task DeleteTask(int id);

        Task<PriorityItem> CalculatePriority(TaskItem task);

        Task<IEnumerable<TaskItem>> GetCriticalTasks();
    }

}
