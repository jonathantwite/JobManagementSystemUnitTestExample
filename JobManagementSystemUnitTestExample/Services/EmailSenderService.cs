using JobManagementSystem.Models;

namespace JobManagementSystem.Services;

/// <summary>
/// This service would contain code tightly coupling the service to an email provider.
/// This service is therefore mocked in unit and integration tests to ensure that the tests can run without the provider being available.
/// </summary>
public class EmailSenderService : IEmailSenderService
{
    public void SendEmail(EmailCommunication email)
    {
        throw new NotImplementedException();
    }
}
