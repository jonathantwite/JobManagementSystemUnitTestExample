using JobManagementSystem.Models;

namespace JobManagementSystem.Services;
public interface ICommunicationService
{
    EmailCommunication CreateEmail(int employeeId, string message, string header, string? cc = null, string? bcc = null);
}