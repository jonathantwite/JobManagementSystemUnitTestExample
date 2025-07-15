using JobManagementSystem.DataAccess;
using JobManagementSystem.Entities;
using JobManagementSystem.Services;

namespace JobManagementSystem.Tests.UnitTests.Services;

public class CommunicationServiceTests : DbMockedTestFixture<JobManagementContext>
{
    private async Task<Employee> AddEmployeeToDb(JobManagementContext dbContext)
    {
        var jobRole = new JobRole { Id = 1, Description = "Test Job Role" };
        var employee = new Employee
        {
            Id = 1,
            Name = "John Doe",
            Email = "jd@test.com",
            DateOfBirth = DateTime.Today.AddYears(-30),
            JobRole = jobRole
        };
        dbContext.Employees.Add(employee);
        await dbContext.SaveChangesAsync();

        return employee;
    }

    [Fact]
    public void CreateEmail_ThrowsArgumentException_WhenEmployeeNotFound()
    {
        // Arrange
        var dbContext = CreateContext();
        var service = new CommunicationService(dbContext);
        int nonExistentEmployeeId = 999; // Assuming this ID does not exist in the test database

        // Act & Assert
        Assert.Throws<ArgumentException>(() => service.CreateEmail(nonExistentEmployeeId, "Test message", "Test header"));
    }

    [Fact]
    public async Task CreateEmail_ReturnsEmailCommunication_WhenEmployeeFound()
    {
        // Arrange
        var dbContext = CreateContext();
        var employee = await AddEmployeeToDb(dbContext);

        var expectedMessage = "Welcome to the company!";
        var expectedHeader = "Welcome Email";

        var service = new CommunicationService(dbContext);

        // Act
        var email = service.CreateEmail(
            employee.Id,
            expectedMessage,
            expectedHeader);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(expectedMessage, email.Body);
        Assert.Equal(expectedHeader, email.Subject);
        Assert.Equal(employee.Email, email.ToAddress);
        Assert.Null(email.BccAddress);
        Assert.Null(email.CcAddress);
    }

    [Fact]
    public async Task CreateEmail_ReturnsEmailCommunication_WhenEmployeeFound_IncludeCc()
    {
        // Arrange
        var dbContext = CreateContext();
        var employee = await AddEmployeeToDb(dbContext);

        var expectedMessage = "Welcome to the company!";
        var expectedHeader = "Welcome Email";
        var expectedCC = "cc@test.com";

        var service = new CommunicationService(dbContext);

        // Act
        var email = service.CreateEmail(
            employee.Id,
            expectedMessage,
            expectedHeader,
            cc: expectedCC);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(expectedMessage, email.Body);
        Assert.Equal(expectedHeader, email.Subject);
        Assert.Equal(employee.Email, email.ToAddress);
        Assert.Null(email.BccAddress);
        Assert.Equal(expectedCC, email.CcAddress);
    }

    [Fact]
    public async Task CreateEmail_ReturnsEmailCommunication_WhenEmployeeFound_IncludeBcc()
    {
        // Arrange
        var dbContext = CreateContext();
        var employee = await AddEmployeeToDb(dbContext);

        var expectedMessage = "Welcome to the company!";
        var expectedHeader = "Welcome Email";
        var expectedBCC = "bcc@test.com";

        var service = new CommunicationService(dbContext);

        // Act
        var email = service.CreateEmail(
            employee.Id,
            expectedMessage,
            expectedHeader,
            bcc: expectedBCC);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(expectedMessage, email.Body);
        Assert.Equal(expectedHeader, email.Subject);
        Assert.Equal(employee.Email, email.ToAddress);
        Assert.Equal(expectedBCC, email.BccAddress);
        Assert.Null(email.CcAddress);
    }

    [Fact]
    public async Task CreateEmail_ReturnsEmailCommunication_WhenEmployeeFound_IncludeCcAndBcc()
    {
        // Arrange
        var dbContext = CreateContext();
        var employee = await AddEmployeeToDb(dbContext);

        var expectedMessage = "Welcome to the company!";
        var expectedHeader = "Welcome Email";
        var expectedCC = "cc@test.com";
        var expectedBCC = "bcc@test.com";

        var service = new CommunicationService(dbContext);

        // Act
        var email = service.CreateEmail(
            employee.Id,
            expectedMessage,
            expectedHeader,
            bcc: expectedBCC,
            cc: expectedCC);

        // Assert
        Assert.NotNull(email);
        Assert.Equal(expectedMessage, email.Body);
        Assert.Equal(expectedHeader, email.Subject);
        Assert.Equal(employee.Email, email.ToAddress);
        Assert.Equal(expectedBCC, email.BccAddress);
        Assert.Equal(expectedCC, email.CcAddress);
    }
}
