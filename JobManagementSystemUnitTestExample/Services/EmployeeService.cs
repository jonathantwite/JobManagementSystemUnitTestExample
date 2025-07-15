using JobManagementSystem.DataAccess;
using JobManagementSystem.Entities;
using JobManagementSystem.Requests;
using Microsoft.EntityFrameworkCore;

namespace JobManagementSystem.Services;

/// <summary>
/// As this class gets bigger an more complicated, you will want to break out the CreateEmployee section of methods into a separate employee-creation service class.
/// You want to be able to do this without breaking your tests, otherwise your tests will be of no help while implementing this refactoring.
/// By testing against the public interface of this class and not the internal implementation, you can refactor the implementation without breaking the tests, giving you more assurity for the new code you have written.
/// </summary>
/// <param name="emailSenderService"></param>
/// <param name="communicationService"></param>
/// <param name="specialUserService"></param>
/// <param name="dbContext"></param>
public class EmployeeService(
    IEmailSenderService emailSenderService,
    ICommunicationService communicationService,
    ISpecialUserService specialUserService,
    JobManagementContext dbContext) : IEmployeeService
{
    private readonly IEmailSenderService _emailSenderService = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
    private readonly ICommunicationService _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
    private readonly ISpecialUserService _specialUserService = specialUserService ?? throw new ArgumentNullException(nameof(specialUserService));
    private readonly JobManagementContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<bool> Exists(int id) => (await _dbContext.Employees.SingleOrDefaultAsync(j => j.Id == id)) != null;

    /// <summary>
    /// This is a "orchestrator" or "controller" method that contains very little logic, but instead calls other methods to do the work.
    /// A test against this method is technically an integration test, as it tests the interaction between multiple methods and services.
    /// Do we care? probably not, but the issue does arise that it cannot be tested in a pure-functional way, and it may change a lot of state.
    /// Personally, I would flag this as an integration test, write it in the same style as a unit test, but understand that it will be slower and more complicated to maintain.
    /// </summary>
    /// <param name="newEmployee"></param>
    /// <returns></returns>
    public async Task<int> CreateEmployee(CreateEmployeeRequest newEmployee)
    {
        var id = await CreateEmployeeRecord(newEmployee);
        var hrEmail = _specialUserService.GetSpecialUserEmailAddress(Models.SpecialUser.HrManager);
        var email = _communicationService.CreateEmail(id, NewElectorMessage(newEmployee.Forenames, newEmployee.Surname), NewElectorEmailSubjectText, bcc: hrEmail);
        _emailSenderService.SendEmail(email);

        return id;
    }

    /// <summary>
    /// This is a private method and so should stay private.  We do not want to make it public just to test it.
    /// </summary>
    /// <param name="newEmployee"></param>
    /// <returns></returns>
    private async Task<int> CreateEmployeeRecord(CreateEmployeeRequest newEmployee)
    {
        var employee = new Employee
        {
            Name = newEmployee.Forenames + " " + newEmployee.Surname,
            Email = newEmployee.EmailAddress,
            DateOfBirth = newEmployee.DateOfBirth,
            JobRoleId = newEmployee.JobRoleId
        };
        _dbContext.Employees.Add(employee);
        await _dbContext.SaveChangesAsync();
        return employee.Id;
    }

    private const string NewElectorEmailSubjectText = "New Employee Confirmation";

    private static string NewElectorMessage(string forenames, string surname)
    {
        return $"Dear {forenames} {surname},\n\n" +
            "Welcome to the company!\n"
            + "Yours Sincerely\nThe CEO";
    }

}
