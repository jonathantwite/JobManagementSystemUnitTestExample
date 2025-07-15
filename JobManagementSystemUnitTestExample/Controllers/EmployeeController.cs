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

    /// <summary>
    /// This is an example of a controller method with validation using FluentValidation.  When testing, if possible, we do not want to be tied into the FluentValidation library so that we can swap this library out.
    /// It would be better if Microsoft supplied a validation abstraction (like they do with logging) so that we could inject any validation library we wanted to.  They do not currently do this, so we must use the FluentValidation interfaces.
    /// </summary>
    /// <param name="employeeName"></param>
    /// <returns></returns>
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
