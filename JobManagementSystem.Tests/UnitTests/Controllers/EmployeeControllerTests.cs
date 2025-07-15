using FluentValidation.Results;
using JobManagementSystem.Controllers;
using JobManagementSystem.Requests;
using JobManagementSystem.Responses.Employee;
using JobManagementSystem.Services;
using JobManagementSystem.Validators;
using Microsoft.AspNetCore.Http.HttpResults;

namespace JobManagementSystem.Tests.UnitTests.Controllers;
public class EmployeeControllerTests
{
    /// <summary>
    /// This is a contentious one.  Should you mock out out the validation logic and purely test the controller logic, or should you test the validation logic here as well?
    /// I personally prefer to include the validation logic in the controller tests, as mocking it out ties you to the implementation of the validation logic.
    /// Currently FluentValidation is used, if we want to change the validation to use a different library, we would have to change the mock as well.
    /// By testing only that the API returns the correct response, we can change the implementation of the validation logic without having to change the logic in tests (except the format of the text output).
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateEmployee_Returns422ForInvalidEmail()
    {
        // Arrange
        var employeeService = Substitute.For<IEmployeeService>();
        var validator = new CreateEmployeeRequestValidator();

        var controller = new EmployeeController(employeeService, validator);

        // Act
        var response = await controller.CreateEmployee(new CreateEmployeeRequest
        (
            "John",
            "Doe",
            DateTime.Now.AddYears(-20), // Valid date of birth
            "invalid-email", // Invalid email format
            1
        ));

        // Assert
        Assert.IsType<UnprocessableEntity<List<ValidationFailure>>>(response);
        var unprocessableEntityResponse = response as UnprocessableEntity<List<ValidationFailure>>;
        var emailError = unprocessableEntityResponse?.Value?.FirstOrDefault(e => e.PropertyName == nameof(CreateEmployeeRequest.EmailAddress));
        Assert.NotNull(emailError);
    }

    /// <summary>
    /// This test shows an interesting case where code coverage fails to cover everything required.
    /// Technically, every line of the Forename validator is covered and so hits 100% coverage, however, we never actually test the text output that is returned to the user.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateEmployee_Returns422ForTooLongForename()
    {
        // Arrange
        var employeeService = Substitute.For<IEmployeeService>();
        var validator = new CreateEmployeeRequestValidator();

        var controller = new EmployeeController(employeeService, validator);

        // Act
        var response = await controller.CreateEmployee(new CreateEmployeeRequest
        (
            new string('A', 501), // Forename longer than 500 characters
            "Doe",
            DateTime.Now.AddYears(-20), // Valid date of birth
            "test@test.com", // Valid email format
            1
        ));

        // Assert
        Assert.IsType<UnprocessableEntity<List<ValidationFailure>>>(response);
        var unprocessableEntityResponse = response as UnprocessableEntity<List<ValidationFailure>>;
        var forenameError = unprocessableEntityResponse?.Value?.FirstOrDefault(e => e.PropertyName == nameof(CreateEmployeeRequest.Forenames));
        Assert.NotNull(forenameError);
    }

    [Fact]
    public async Task CreateEmployee_Returns422ForTooLongSurname()
    {
        // Arrange
        var employeeService = Substitute.For<IEmployeeService>();
        var validator = new CreateEmployeeRequestValidator();

        var controller = new EmployeeController(employeeService, validator);

        // Act
        var response = await controller.CreateEmployee(new CreateEmployeeRequest
        (
            "John",
            new string('A', 201), // Surname longer than 200 characters
            DateTime.Now.AddYears(-20), // Valid date of birth
            "test@test.com", // Valid email format
            1
        ));

        // Assert
        Assert.IsType<UnprocessableEntity<List<ValidationFailure>>>(response);
        var unprocessableEntityResponse = response as UnprocessableEntity<List<ValidationFailure>>;
        var surnameError = unprocessableEntityResponse?.Value?.FirstOrDefault(e => e.PropertyName == nameof(CreateEmployeeRequest.Surname));
        Assert.NotNull(surnameError);
    }

    [Fact]
    public async Task CreateEmployee_Returns422ForEmptySurname()
    {
        // Arrange
        var employeeService = Substitute.For<IEmployeeService>();
        var validator = new CreateEmployeeRequestValidator();

        var controller = new EmployeeController(employeeService, validator);

        // Act
        var response = await controller.CreateEmployee(new CreateEmployeeRequest
        (
            "John",
            "",
            DateTime.Now.AddYears(-20), // Valid date of birth
            "test@test.com", // Valid email format
            1
        ));

        // Assert
        Assert.IsType<UnprocessableEntity<List<ValidationFailure>>>(response);
        var unprocessableEntityResponse = response as UnprocessableEntity<List<ValidationFailure>>;
        var surnameError = unprocessableEntityResponse?.Value?.FirstOrDefault(e => e.PropertyName == nameof(CreateEmployeeRequest.Surname));
        Assert.NotNull(surnameError);
    }

    [Fact]
    public async Task CreateEmployee_Returns422ForFutureDoB()
    {
        // Arrange
        var employeeService = Substitute.For<IEmployeeService>();
        var validator = new CreateEmployeeRequestValidator();

        var controller = new EmployeeController(employeeService, validator);

        // Act
        var response = await controller.CreateEmployee(new CreateEmployeeRequest
        (
            "John",
            "Doe",
            DateTime.Now.AddDays(1), // Invalid date of birth
            "test@test.com", // Valid email format
            1
        ));

        // Assert
        Assert.IsType<UnprocessableEntity<List<ValidationFailure>>>(response);
        var unprocessableEntityResponse = response as UnprocessableEntity<List<ValidationFailure>>;
        var dobError = unprocessableEntityResponse?.Value?.FirstOrDefault(e => e.PropertyName == nameof(CreateEmployeeRequest.DateOfBirth));
        Assert.NotNull(dobError);
    }

    /// <summary>
    /// Question: should these tests all be split into two tests? Maybe (probably?).
    /// The issue is that the test is actually testing two different things -  the response code and the data returned.
    /// If split into two tests, the description of the test would be more accurate, but it would also double the number of tests.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateEmployee_Returns201ForValidRequest()
    {
        // Arrange
        var newEmployeeId = 1001;
        var employeeService = Substitute.For<IEmployeeService>();
        employeeService.CreateEmployee(default).ReturnsForAnyArgs(Task.FromResult(newEmployeeId));

        var validator = new CreateEmployeeRequestValidator();

        var controller = new EmployeeController(employeeService, validator);
        // Act
        var response = await controller.CreateEmployee(new CreateEmployeeRequest
        (
            "John",
            "Doe",
            DateTime.Now.AddYears(-20), // Valid date of birth
            "test@test.com", // Valid email format
            1
        ));

        // Assert
        Assert.IsType<Created<NewEmployeeResponse>>(response);
        var createdResponse = response as Created<NewEmployeeResponse>;
        Assert.Equal(newEmployeeId, createdResponse?.Value?.Id);
    }
}
