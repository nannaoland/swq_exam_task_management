using Moq;
using Xunit;

namespace TaskManagement.Test
{
    public class CreateTaskUnitTests
    {
        private readonly Mock<IRepository<TaskItem>> repositoryMock;
        private readonly TaskManager manager;

        public CreateTaskUnitTests()
        {
            repositoryMock = new Mock<IRepository<TaskItem>>();

            manager = new TaskManager(repositoryMock.Object);
        }

        [Fact]
        public async Task CreateTask_TitleAndDeadlineValid_ReturnTrue()
        {

            // Arrange
            var task = new TaskItem
            {
                Title = "Test title",
                Deadline = DateTime.Today.AddDays(2),
            };

            // Act
            var result = await manager.CreateTask(task);

            //Assert
            Assert.True(result);

            repositoryMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Once);
        }


        [Fact]
        public async Task CreateTask_NoTitle_ReturnException()
        {
            // Arrange
            var task = new TaskItem
            {
                Deadline = DateTime.Today.AddDays(6),
                Status = StatusItem.Todo,

            };

            // Act
            Task CreateTaskTask() =>
                manager.CreateTask(task);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(CreateTaskTask);
        }

        [Fact]

           public async Task CreateTask_DeadlineYesterday_ReturnException()
        {

            // Arrange        
            var task = new TaskItem
            {
                Title = "Test title",
                Deadline = DateTime.Today.AddDays(-1),
            };

            // Act
            Task CreateTaskTask() =>
                manager.CreateTask(task);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(CreateTaskTask);
        }

    }
}