using JobManagementSystem.Models;

namespace JobManagementSystem.Services;
public interface IEmailSenderService
{
    void SendEmail(EmailCommunication email);
}