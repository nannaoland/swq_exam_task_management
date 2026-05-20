using Moq;
using Xunit;


namespace TaskManagement.Test;

 public class CalculatePriorityUnitTests
    {
        private readonly Mock<IRepository<TaskItem>> repositoryMock;
        private readonly TaskManager manager;

        public CalculatePriorityUnitTests()
        {
            repositoryMock = new Mock<IRepository<TaskItem>>();

            manager = new TaskManager(repositoryMock.Object);
        }

        [Fact]
        public async Task CalculatePriority_DoneTask_ReturnsNone()
        {
            // Arrange
            var task = new TaskItem
            {
                Title = "Test task",
                Deadline = DateTime.Today,
                Status = StatusItem.Done
            };

            // Act
            var result = await manager.CalculatePriority(task);

            // Assert
            Assert.Equal(PriorityItem.None, result);
        }

        [Fact]
        public async Task CalculatePriority_DeadlineToday_ReturnsCritical()
        {
            // Arrange
            var task = new TaskItem
            {
                Title = "Test task",
                Deadline = DateTime.Today,
                Status = StatusItem.Todo
            };

            // Act
            var result = await manager.CalculatePriority(task);

            // Assert
            Assert.Equal(PriorityItem.Critical, result);
        }

        [Fact]
        public async Task CalculatePriority_DeadlineTomorrow_ReturnsCritical()
        {
            // Arrange
            var task = new TaskItem
            {
                Title = "Test task",
                Deadline = DateTime.Today.AddDays(1),
                Status = StatusItem.Todo
            };

            // Act
            var result = await manager.CalculatePriority(task);

            // Assert
            Assert.Equal(PriorityItem.Critical, result);
        }

        [Fact]
        public async Task CalculatePriority_ExamTag_ReturnsHigh()
        {
            // Arrange
            var tag = new Tag
            {
                Name = "exam"
            };

            var task = new TaskItem
            {
                Title = "Test task",
                Deadline = DateTime.Today.AddDays(3),
                Status = StatusItem.Todo,
                Tag = tag
                
            };

            // Act
            var result = await manager.CalculatePriority(task);

            // Assert
            Assert.Equal(PriorityItem.High, result);
        }

        [Fact]
        public async Task CalculatePriority_LongDeadline_ReturnsLow()
        {
            // Arrange
            var task = new TaskItem
            {
                Title = "Test task",
                Deadline = DateTime.Today.AddDays(10),
                Status = StatusItem.Todo,
            };

            // Act
            var result = await manager.CalculatePriority(task);

            // Assert
            Assert.Equal(PriorityItem.Low, result);
        }

        [Fact]
        public async Task CalculatePriority_StatusInProgress_ReturnsMedium()
        {
            // Arrange
            var tag = new Tag
            {
                Name = "study"
            };

            var task = new TaskItem
            {
                Title = "Test task",
                Deadline = DateTime.Today.AddDays(6),
                Status = StatusItem.InProgress,
                Tag = tag
            };

            // Act
            var result = await manager.CalculatePriority(task);

            // Assert
            Assert.Equal(PriorityItem.Medium, result);
        }
    }
