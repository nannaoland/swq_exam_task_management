using Moq;
using Xunit;


namespace TaskManagement.Test;

 public class GetCriticalTasksUnitTests
    {
        private readonly Mock<IRepository<TaskItem>> repositoryMock;
        private readonly TaskManager manager;

        public GetCriticalTasksUnitTests()
        {
            repositoryMock = new Mock<IRepository<TaskItem>>();

            manager = new TaskManager(repositoryMock.Object);
        }

        [Fact]
        public async Task GetCriticalTasks_NoTasks_ReturnsEmptyList()
        {
            // Arrange
            repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<TaskItem>());

            // Act
            var result = await manager.GetCriticalTasks();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCriticalTasks_NoCriticalTasks_ReturnsEmptyList()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Title = "Low task",
                    Deadline = DateTime.Today.AddDays(10),
                    Status = StatusItem.Todo
                }
            };

            repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(tasks);

            // Act
            var result = await manager.GetCriticalTasks();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCriticalTasks_MixedTasks_ReturnsOnlyCriticalTasks()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Title = "Critical task",
                    Deadline = DateTime.Today,
                    Status = StatusItem.Todo
                },
                new TaskItem
                {
                    Title = "Low task",
                    Deadline = DateTime.Today.AddDays(10),
                    Status = StatusItem.Todo
                }
            };

            repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(tasks);

            // Act
            var result = await manager.GetCriticalTasks();

            // Assert
            Assert.Single(result);
            Assert.Equal("Critical task", result.First().Title);
        }
    
    }
