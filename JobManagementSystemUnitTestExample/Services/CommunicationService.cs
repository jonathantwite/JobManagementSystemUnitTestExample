using JobManagementSystem.DataAccess;
using JobManagementSystem.Models;

namespace JobManagementSystem.Services;

public class CommunicationService(JobManagementContext dbContext) : ICommunicationService
{
    private readonly JobManagementContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public EmailCommunication CreateEmail(int employeeId, string message, string header, string? cc = null, string? bcc = null)
    {
        var employee = _dbContext.Employees.Find(employeeId) ?? throw new ArgumentException($"Unable to find Employee {employeeId}");

        return new EmailCommunication(employee.Email, cc, bcc, header, message);
    }
}
