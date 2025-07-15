using FluentValidation;
using JobManagementSystem.Requests;
using JobManagementSystem.Responses.Employee;
using JobManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController(IEmployeeService employeeService, IValidator<CreateEmployeeRequest> createEmployeeRequestValidator) : Controller
{
    public readonly IEmployeeService _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
    public readonly IValidator<CreateEmployeeRequest> _createEmployeeRequestValidator = createEmployeeRequestValidator ?? throw new ArgumentNullException(nameof(createEmployeeRequestValidator));

    [HttpPost]
    public async Task<IResult> CreateEmployee([FromBody] CreateEmployeeRequest employeeName)
    {
        var validationResult = await _createEmployeeRequestValidator.ValidateAsync(employeeName);
        if (!validationResult.IsValid)
        {
            return Results.UnprocessableEntity(validationResult.Errors);
        }

        var employeeId = await employeeService.CreateEmployee(employeeName);

        if (employeeId <= 0)
        {
            return Results.InternalServerError("An error occurred while creating the employee.");
        }

        return Results.Created(nameof(CreateEmployee), new NewEmployeeResponse(employeeId));
    }
}
