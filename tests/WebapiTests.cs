using webapi.Model;
using webapi.Services;
using webapi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace tests;

public class WebapiTests
{

    [Fact]
    public async Task UpdateStatus_WithExistingTask_ShouldUpdateStatus()
    {
        // Arrange
        var service = new TaskService();
        var controller = new TasksController(service);

        // Act
        var result = await controller.UpdateStatus(
            1,
            TaskItemStatus.Completed
        );

        // Assert
        var ok = result as OkObjectResult;
        Assert.NotNull(ok);
        var task = ok.Value as TaskItem;
        Assert.NotNull(task);
        Assert.Equal(
            TaskItemStatus.Completed,
            task.Status
        );
    }

    [Fact]
    public async Task Get_ShouldReturnAllTasks()
    {
        // Arrange
        var service = new TaskService();
        var controller = new TasksController(service);

        // Act
        var result = await controller.Get();

        // Assert
        var ok = result.Result as OkObjectResult;
        Assert.NotNull(ok);
        var tasks = ok.Value as IEnumerable<TaskItem>;
        Assert.NotNull(tasks);
        Assert.NotEmpty(tasks);
    }

    [Fact]
    public async Task Get_ShouldReturnOneTask_BasedOnIntegerID()
    {
        // Arrange
        var service = new TaskService();
        var controller = new TasksController(service);

        // Act
        var result = await controller.Get(1);

        // Assert
        var ok = result as OkObjectResult;
        Assert.NotNull(ok);
        var task = ok.Value as TaskItem;
        Assert.NotNull(task);
        Assert.Equal(1, task.Id);
    }

    [Fact]
    public async Task Post_WithValidTask_ShouldReturnCreated()
    {
        var service = new TaskService();
        var controller = new TasksController(service);
        var newTask = new TaskItem
        {
            Title = "New test task",
            Description = "Created from test"
        };
        var result = await controller.Post(newTask);
        var created = result as CreatedAtActionResult;
        Assert.NotNull(created);
        var task = created.Value as TaskItem;
        Assert.NotNull(task);
        Assert.Equal("New test task", task.Title);
    }

    [Fact]
    public async Task Post_WithoutTitle_ShouldReturnBadRequest()
    {
        var service = new TaskService();
        var controller = new TasksController(service);
        var newTask = new TaskItem
        {
            Title = ""
        };
        var result = await controller.Post(newTask);
        var badRequest = result as BadRequestObjectResult;
        Assert.NotNull(badRequest);
        Assert.Equal(400, badRequest.StatusCode);
    }
}
