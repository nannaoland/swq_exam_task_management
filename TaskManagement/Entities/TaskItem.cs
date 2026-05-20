
namespace TaskManagement
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public Tag Tag { get; set; }
        public StatusItem Status { get; set; }
        public PriorityItem Priority { get; set; }

    }
}
