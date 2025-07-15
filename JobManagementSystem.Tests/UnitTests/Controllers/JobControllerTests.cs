using JobManagementSystem.Controllers;
using JobManagementSystem.Responses.Job;
using JobManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobManagementSystem.Tests.UnitTests.Controllers;

/// <summary>
/// These tests test the user-interacting logic contained within the JobController.
/// They mostly test for two things:
///
/// - the correct status codes being returned
/// - the correct data in any response object
///
/// These tests completely ignore how the services work.
///
/// Note, as far as I am aware, this code does not use any JSON serialization, even when using JsonResult.
/// Therefore integration or end-2-end tests are still required to ensure that the data is being serialized correctly (e.g. empty arrays do not become nulls)
/// </summary>
public class JobControllerTests
{
    /// <summary>
    /// This test tests for the correct 200 response and that the data in the response has not been altered from the data returned by the JobService.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Get_Returns200()
    {
        //Arrange
        var data = new List<JobResponse>()
        {
            new JobResponse(1, "Job", new JobCategoryResponse(1, "JC"), ["GBR"], [new EmployeeResponse(1, "Name1")])
        }.AsEnumerable();

        var jobService = Substitute.For<IJobService>();
        var employeeService = Substitute.For<IEmployeeService>();

        jobService.GetAllJobs().Returns(Task.FromResult(data));

        var controller = new JobController(jobService, employeeService);

        //Act
        var response = await controller.Get();

        //Assert
        Assert.IsType<JsonResult>(response);
        Assert.Equal(data, (response as JsonResult)?.Value as IEnumerable<JobResponse>);
    }

    /// <summary>
    /// This test ensures that when there is no jobs to return, an empty array is returned (rather than null, or a 404).
    /// The resource "bucket" was found, but was empty, so this was a successful call to the API, hence the 200 response.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Get_Returns200AndEmptyArrayWithNoData()
    {
        //Arrange
        var data = new List<JobResponse>().AsEnumerable();

        var jobService = Substitute.For<IJobService>();
        var employeeService = Substitute.For<IEmployeeService>();

        jobService.GetAllJobs().Returns(Task.FromResult(data));

        var controller = new JobController(jobService, employeeService);

        //Act
        var response = await controller.Get();

        //Assert
        var responseData = (response as JsonResult)?.Value;
        Assert.IsType<JsonResult>(response);
        Assert.NotNull(responseData as List<JobResponse>);
        Assert.Empty(responseData as List<JobResponse>);
    }

    /// <summary>
    /// This test checks the validation failure that occurs when the supplied employee id is not found.
    /// The employee id is supplied as part of the body of the request, therefore API endpoint resource (the job) is found and so this is not a 404, but a 422 Unprocessable Entity.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddEmployeeToJob_Returns422ForUnknownEmployee()
    {
        //Arrange
        var jobService = Substitute.For<IJobService>();
        var employeeService = Substitute.For<IEmployeeService>();

        jobService.Exists(default).ReturnsForAnyArgs(Task.FromResult(true));
        employeeService.Exists(default).ReturnsForAnyArgs(Task.FromResult(false));

        var controller = new JobController(jobService, employeeService);

        //Act
        var response = await controller.AddEmployeeToJob(1, 1);

        //Assert
        Assert.Equal(422, (response as ObjectResult)?.StatusCode);
    }

    /// <summary>
    /// This test checks the validation failure that occurs when the supplied job id is not found.
    /// The job id is part of the API route (and so is the "resource" being requested) and therefore if the job is not found, the endpoint should return a 404 Not Found.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddEmployeeToJob_Returns404ForUnknownJob()
    {
        //Arrange
        var jobService = Substitute.For<IJobService>();
        var employeeService = Substitute.For<IEmployeeService>();

        jobService.Exists(default).ReturnsForAnyArgs(Task.FromResult(false));
        employeeService.Exists(default).ReturnsForAnyArgs(Task.FromResult(true));

        var controller = new JobController(jobService, employeeService);

        //Act
        var response = await controller.AddEmployeeToJob(1, 1);

        //Assert
        Assert.IsType<NotFoundResult>(response);
    }

    /// <summary>
    /// This test checks the validation failure that occurs when both the supplied job id and the supplied employee id are not found.
    /// The job id is part of the API route (and so is the "resource" being requested) and therefore if the job is not found, the endpoint should return a 404 Not Found.
    /// This will then ignore the employee id as the resource was not found to start processing.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddEmployeeToJob_Returns404ForBothUnknownJobAndEmployee()
    {
        //Arrange
        var jobService = Substitute.For<IJobService>();
        var employeeService = Substitute.For<IEmployeeService>();

        jobService.Exists(default).ReturnsForAnyArgs(Task.FromResult(false));
        employeeService.Exists(default).ReturnsForAnyArgs(Task.FromResult(false));

        var controller = new JobController(jobService, employeeService);

        //Act
        var response = await controller.AddEmployeeToJob(1, 1);

        //Assert
        Assert.IsType<NotFoundResult>(response);
    }

    /// <summary>
    /// This test checks that the correct HTTP status code of 204 (No Content) is returned when the update patch runs.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddEmployeeToJob_Returns204WhenOk()
    {
        //Arrange
        var jobService = Substitute.For<IJobService>();
        var employeeService = Substitute.For<IEmployeeService>();

        jobService.Exists(default).ReturnsForAnyArgs(Task.FromResult(true));
        employeeService.Exists(default).ReturnsForAnyArgs(Task.FromResult(true));

        var controller = new JobController(jobService, employeeService);

        //Act
        var response = await controller.AddEmployeeToJob(1, 1);

        //Assert
        Assert.IsType<NoContentResult>(response);
    }
}
