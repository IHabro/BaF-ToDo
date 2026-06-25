using BaF_ToDo.Models;
using BaF_ToDo.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace BaF_ToDo_Unit_Testing
{
    public class TaskEntityControllerTest
    {
        [Fact]
        public async Task GetTasks_ReturnsListOfTasksAsync()
        {
            // Arrange

            var mockRepository = new Mock<ITaskEntityRepository>();
            mockRepository.Setup(repo => repo.GetAllTasksAsync()).ReturnsAsync(new List<TaskEntity> { new TaskEntity { Id = 1, Title = "First Task" }, new TaskEntity { Id = 2, Title = "Second Task" }, new TaskEntity { Id = 3, Title = "Third Task" } });

            var controller = new TaskEntityController(mockRepository.Object);

            // Act
            var result = await controller.GetTasks();

            // Assert
            var okResult = Assert.IsType<List<TaskEntityDTO>>(result.Value);
            var tasks = Assert.IsAssignableFrom<List<TaskEntityDTO>>(result.Value);

            Assert.Equal(3, tasks.Count);
        }

        [Fact]
        public async Task GetTaskById_ReturnsTask_WhenTaskExists()
        {
            // Arrange
            var taskId = 1;
            var taskEntity = new TaskEntity { Id = taskId, Title = "First Task" };

            var mockRepository = new Mock<ITaskEntityRepository>();
            mockRepository.Setup(repo => repo.GetTaskByIdAsync(taskId)).ReturnsAsync(taskEntity);

            var controller = new TaskEntityController(mockRepository.Object);

            // Act
            var result = await controller.GetTaskById(taskId);

            // Assert
            var dto = Assert.IsType<TaskEntityDTO>(result.Value);

            Assert.Equal(taskId, dto.Id);
            Assert.Equal("First Task", dto.Title);
            Assert.False(dto.IsCompleted);
        }

        [Fact]
        public async Task GetTaskById_ReturnsNotFoundResult_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = 1;

            var mockRepository = new Mock<ITaskEntityRepository>();
            mockRepository.Setup(repo => repo.GetTaskByIdAsync(taskId)).ReturnsAsync((TaskEntity?)null);

            var controller = new TaskEntityController(mockRepository.Object);

            // Act
            var result = await controller.GetTaskById(taskId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedAtActionResult_WithCreatedTask()
        {
            // Arrange
            var createDto = new CreateTaskEntityDTO { Title = "First Task" };
            var createdTask = new TaskEntity { Id = 1, Title = "First Task", IsCompleted = false };

            var mockRepository = new Mock<ITaskEntityRepository>();
            mockRepository.Setup(repo => repo.AddTaskAsync(It.IsAny<TaskEntity>())).ReturnsAsync(createdTask);

            var controller = new TaskEntityController(mockRepository.Object);

            // Act
            var result = await controller.CreateTask(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedDto = Assert.IsType<TaskEntityDTO>(createdAtActionResult.Value);

            Assert.Equal(createdTask.Id, returnedDto.Id);
            Assert.Equal(createdTask.Title, returnedDto.Title);
            Assert.Equal(createdTask.IsCompleted, returnedDto.IsCompleted);
        }

        [Fact]
        public async Task CreateTaskEntityDTO_ReturnsFalse_WhenTitleIsNull()
        {
            // Arrange
            var dto = new CreateTaskEntityDTO
            {
                Title = null
            };

            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act


            // Assert
            var isValid = Validator.TryValidateObject(
                dto,
                context,
                results,
                true);

            Assert.False(isValid);
        }

        [Fact]
        public async Task CreateTaskEntityDTO_ReturnsFalse_WhenTitleIsEmpty()
        {
            // Arrange
            var dto = new CreateTaskEntityDTO
            {
                Title = ""
            };

            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            // Act


            // Assert
            var isValid = Validator.TryValidateObject(
                dto,
                context,
                results,
                true);

            Assert.False(isValid);
        }

        [Fact]
        public async Task UpdateTask_ReturnsNoContent_WhenTaskExists()
        {
            // Arrange
            var taskToUpdate = new TaskEntity { Id = 1, Title = "First Task", IsCompleted = false };
            var updateDto = new UpdateTaskEntityDTO {  Title = "Updated Task", IsCompleted = true };

            var mockRepository = new Mock<ITaskEntityRepository>();
            mockRepository.Setup(repo => repo.UpdateTaskAsync(It.IsAny<TaskEntity>())).ReturnsAsync(true);
            mockRepository.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(taskToUpdate);

            var controller = new TaskEntityController(mockRepository.Object);

            // Act
            var result = await controller.UpdateTask(1, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var updateDto = new UpdateTaskEntityDTO { Title = "Updated Task", IsCompleted = true };

            var mockRepository = new Mock<ITaskEntityRepository>();
            mockRepository.Setup(repo => repo.UpdateTaskAsync(It.IsAny<TaskEntity>())).ReturnsAsync(true);
            mockRepository.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync((TaskEntity?)null);

            var controller = new TaskEntityController(mockRepository.Object);

            // Act
            var result = await controller.UpdateTask(1, updateDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNoContent_WhenTaskExists()
        {
            // Arrange
            var mockRepository = new Mock<ITaskEntityRepository>();
            mockRepository.Setup(repo => repo.DeleteTaskAsync(1)).ReturnsAsync(true);

            var controller = new TaskEntityController(mockRepository.Object);

            // Act
            var result = await controller.DeleteTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<ITaskEntityRepository>();
            mockRepository.Setup(repo => repo.DeleteTaskAsync(1)).ReturnsAsync(false);

            var controller = new TaskEntityController(mockRepository.Object);

            // Act
            var result = await controller.DeleteTask(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CompleteTask_ReturnsNoContent_WhenTaskExists()
        {
            // Arrange
            var taskToComplete = new TaskEntity { Id = 1, Title = "First Task", IsCompleted = false };

            var mockRepository = new Mock<ITaskEntityRepository>();
            mockRepository.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync(taskToComplete);

            var controller = new TaskEntityController(mockRepository.Object);

            // Act
            var result = await controller.CompleteTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task CompleteTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<ITaskEntityRepository>();
            mockRepository.Setup(repo => repo.GetTaskByIdAsync(1)).ReturnsAsync((TaskEntity?)null);

            var controller = new TaskEntityController(mockRepository.Object);

            // Act
            var result = await controller.CompleteTask(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
