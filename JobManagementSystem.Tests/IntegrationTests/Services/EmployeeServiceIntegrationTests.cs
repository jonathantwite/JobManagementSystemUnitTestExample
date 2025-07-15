using JobManagementSystem.DataAccess;
using JobManagementSystem.Entities;
using JobManagementSystem.Models;
using JobManagementSystem.Requests;
using JobManagementSystem.Services;

namespace JobManagementSystem.Tests.IntegrationTests.Services;

[Trait("Category", "IntegrationTests")]
public class EmployeeServiceIntegrationTests : DbMockedTestFixture<JobManagementContext>
{
    /// <summary>
    /// This test tests an "orchestrator" (or "controller") method.  The method itself does not contain much logic, but instead calls other methods and classes to do the work.
    /// Therefore this is an integration test.  It is significantly more complicated than a unit test, as it must test the outcomes of several processes and therefore is much harder to maintain.
    /// It also uses the "communication" style of testing (testing that the EmailSenderService is called with the correct parameters) which is discouraged in Unit Tests, but suitable for integration tests.
    /// Being integration tests however, they are different from unit tests and valuable in themselves for testing interactions between units.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateEmployee_CreatesEmployeeWhenInputOk()
    {
        // Arrange

        //Mock external dependencies only
        var emailSenderService = Substitute.For<IEmailSenderService>();
        var dbContext = CreateContext();
        dbContext.JobRoles.Add(new JobRole { Id = 1, Description = "Job Role 1" });
        await dbContext.SaveChangesAsync();

        // Create actual services
        var communicationService = new CommunicationService(dbContext);
        var specialUserService = new SpecialUserService();
        var employeeService = new EmployeeService(emailSenderService, communicationService, specialUserService, dbContext);

        var employeeDob = DateTime.Today.AddYears(-20);
        var newEmployee = new CreateEmployeeRequest
        (
            "John",
            "Doe",
            employeeDob,
            "test@test.com",
            1
        );

        // Act
        int newId = await employeeService.CreateEmployee(newEmployee);

        // Assert

        // Check that the employee was created in the database
        var createdEmployee = await dbContext.Employees.FindAsync(newId);
        Assert.NotNull(createdEmployee);
        Assert.Equal("John Doe", createdEmployee.Name);
        Assert.Equal(employeeDob, createdEmployee.DateOfBirth);
        Assert.Equal("test@test.com", createdEmployee.Email);

        // Check that the email was sent, including to HR
        emailSenderService.Received(1).SendEmail(Arg.Is<EmailCommunication>(email =>
            email.Subject == "New Employee Confirmation" &&
            email.Body.Contains("John Doe") &&
            email.BccAddress != null &&
            email.BccAddress.Equals("hr@company.com")));
    }
}
