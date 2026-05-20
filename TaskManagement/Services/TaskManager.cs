namespace TaskManagement
{
    public class TaskManager : ITaskManager
    {
        private IRepository<TaskItem> taskRepository;

        public TaskManager(IRepository<TaskItem> taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task<bool> CreateTask(TaskItem task)
        {
            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title is required.");


            if (task.Deadline.Date < DateTime.Today)
                throw new ArgumentException("The deadline cannot be in the past.");

            task.Priority = await CalculatePriority(task);

            await taskRepository.AddAsync(task);
            return true;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasks()
        {
            return await taskRepository.GetAllAsync();
        }

        public async Task UpdateTask(TaskItem task)
        {
            task.Priority = await CalculatePriority(task);
            await taskRepository.EditAsync(task);
        }

        public async Task DeleteTask(int id)
        {
            await taskRepository.RemoveAsync(id);
        }

        public Task<PriorityItem> CalculatePriority(TaskItem task)
        {
            var now = DateTime.Now;
            var daysUntilDeadline = (task.Deadline.Date - now.Date).Days;
            var tagName = task.Tag?.Name.ToLower();

            if (task.Status == StatusItem.Done)
                return Task.FromResult(PriorityItem.None);

            if (daysUntilDeadline <= 1)
                return Task.FromResult(PriorityItem.Critical);

            if (tagName == "exam" || tagName == "urgent")
                return Task.FromResult(PriorityItem.High);

            if (daysUntilDeadline <= 5)
                return Task.FromResult(PriorityItem.High);

            if (task.Tag == null)
                return Task.FromResult(PriorityItem.Low);

            return Task.FromResult(PriorityItem.Medium);
        }

        public async Task<IEnumerable<TaskItem>> GetCriticalTasks()
        {
            var tasks = await taskRepository.GetAllAsync();

            List<TaskItem> criticalTasks = new List<TaskItem>();

            foreach (var task in tasks)
            {
                var priority = await CalculatePriority(task);

                if (priority == PriorityItem.Critical)
                {
                    criticalTasks.Add(task);
                }
            }

            return criticalTasks;
        }
    }
}
