using JobManagementSystem.DataAccess;
using JobManagementSystem.Entities;
using JobManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace JobManagementSystem.Tests.Services;

/// <summary>
/// These are unit tests for the JobService.  As this is a service, we wish to test the functionality when a "unit of work" happens
/// </summary>
public class JobServiceTests : DbMockedTestFixture<JobManagementContext>
{
    public readonly IEnumerable<Job> _jobs;
    public readonly IEnumerable<Employee> _employees;
    public const string job1Description = "Job 1 Description";
    public const string job2Description = "Job 2 Description";
    public const string jobCategory1Description = "Job Category 1 Description";
    public const string jobCategory2Description = "Job Category 2 Description";
    public const string employee1Name = "Employee 1 Name";
    public const string employee2Name = "Employee 2 Name";
    public const string jobRole1Description = "Job Role 1 Description";
    public const string jobRole2Description = "Job Role 2 Description";
    public const string taxInformation1Description = "Tax Info 1";
    public const string taxInformation2Description = "Tax Info 2";
    public const string taxInformation3Description = "Tax Info 3";
    public const string taxRegime1CountryCode = "ABC";
    public const string taxRegime2CountryCode = "ZYX";
    public const string taxRegime1Description = "Tax Regime 1 Description";
    public const string taxRegime2Description = "Tax Regime 2 Description";
    public const decimal taxRegime1MinimumThreshold = 10000M;
    public const decimal taxRegime2MinimumThreshold = 20000M;
    public const decimal taxRegime1TaxRate = 0.1M;
    public const decimal taxRegime2TaxRate = 0.2M;

    public JobServiceTests()
    {
        var tr1 = new TaxRegime
        {
            CountryCode = taxRegime1CountryCode,
            Description = taxInformation1Description,
            MinimumThreshold = taxRegime1MinimumThreshold,
            TaxRate = taxRegime1TaxRate
        };
        var tr2 = new TaxRegime
        {
            CountryCode = taxRegime2CountryCode,
            Description = taxInformation2Description,
            MinimumThreshold = taxRegime2MinimumThreshold,
            TaxRate = taxRegime2TaxRate
        };

        var ti1 = new TaxInformation { Description = taxInformation1Description, TaxRegime = tr1 };
        var ti2 = new TaxInformation { Description = taxInformation2Description, TaxRegime = tr2 };
        var ti3 = new TaxInformation { Description = taxInformation3Description, TaxRegime = tr1 };

        var j1 = new Job
        {
            Id = 1,
            Description = job1Description,
            JobCategory = new JobCategory
            {
                Id = 1,
                Description = jobCategory1Description
            }
        };

        var j2 = new Job
        {
            Id = 2,
            Description = job2Description,
            JobCategory = new JobCategory
            {
                Id = 2,
                Description = jobCategory2Description
            }
        };

        j1.TaxInformation.Add(ti1);
        j1.TaxInformation.Add(ti2);
        j2.TaxInformation.Add(ti3);

        var e1 = new Employee { Id = 1, Name = employee1Name, JobRole = new JobRole { Id = 1, Description = jobRole1Description } };
        var e2 = new Employee { Id = 2, Name = employee2Name, JobRole = new JobRole { Id = 2, Description = jobRole2Description } };
        j1.Employees.Add(e1);
        j2.Employees.Add(e1);
        j2.Employees.Add(e2);

        _jobs = [j1, j2];
        _employees = [e1, e2];

        using var context = new JobManagementContext(_contextOptions);
        context.Jobs.AddRange(_jobs);
        context.SaveChanges();
    }

    /// <summary>
    /// This test that the GetAllJobs() function returns the correct number of results
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAllJobs_ReturnsCorrectNumberOfRecords()
    {
        // Arrange
        var dbContext = CreateContext();

        var service = new JobService(dbContext, new TaxService());
        var expected = _jobs.Count();

        // Act
        var allJobs = await service.GetAllJobs();
        var actual = allJobs.Count();

        // Assert
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// This tests that the GetAllJobs() function returns the correct values
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAllJobs_ReturnsDescription()
    {
        // Arrange
        var dbContext = CreateContext();

        var service = new JobService(dbContext, new TaxService());
        var expected = new List<string>() { job1Description, job2Description };

        // Act
        var allJobs = await service.GetAllJobs();
        var actual = allJobs.Select(j => j.Description);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetAllJobs_ReturnsEmployeesNames()
    {
        // Arrange
        var dbContext = CreateContext();

        var service = new JobService(dbContext, new TaxService());
        var expectedJ1 = new List<string>() { employee1Name };
        var expectedJ2 = new List<string>() { employee1Name, employee2Name };

        // Act
        var allJobs = await service.GetAllJobs();
        var actualJ1 = allJobs.SingleOrDefault(j => j.Id == 1)?.Employees?.Select(e => e.Name);
        var actualJ2 = allJobs.SingleOrDefault(j => j.Id == 2)?.Employees?.Select(e => e.Name);

        // Assert
        Assert.Equivalent(expectedJ1, actualJ1);
        Assert.Equivalent(expectedJ2, actualJ2);
    }

    /// <summary>
    /// This test show the difference between the Detroit (Classical) and London (Mockist) style of testing.
    /// A London test would mock out the TaxService and test that the value provided by the TaxService is placed in the DTO correctly.  More tests would be required to test the TaxService.
    /// The issue with this is that the tests now become aware, and dependent on, the code architecture, and refactoring will cause the tests to fail.
    /// This failure does not add value as it is a false negative.
    /// This test is written in a Classical style which defines an "Isolated Test" as an "Isolate piece of work" and only cares about one thing - is the data returned correct.
    /// We do not care how the TaxService works (nor any other dependencies), we would only mock external or persistence dependencies.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAllJobs_ReturnsTaxLiabilities()
    {
        // Arrange
        var dbContext = CreateContext();

        var service = new JobService(dbContext, new TaxService());
        var expectedJ1 = new List<string>() { taxRegime1CountryCode, taxRegime2CountryCode };
        var expectedJ2 = new List<string>() { taxRegime1CountryCode };

        // Act
        var allJobs = await service.GetAllJobs();
        var actualJ1 = allJobs.SingleOrDefault(j => j.Id == 1)?.TaxLiabilities;
        var actualJ2 = allJobs.SingleOrDefault(j => j.Id == 2)?.TaxLiabilities;

        // Assert
        Assert.Equivalent(expectedJ1, actualJ1);
        Assert.Equivalent(expectedJ2, actualJ2);
    }

    /// <summary>
    /// These tests test that a simple function works correctly
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Exists_ReturnsTrueIfJobFound()
    {
        // Arrange
        var dbContext = CreateContext();
        var service = new JobService(dbContext, new TaxService());

        // Act
        var actual = await service.Exists(2);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task Exists_ReturnsFalseIfJobFound()
    {
        // Arrange
        var dbContext = CreateContext();
        var service = new JobService(dbContext, new TaxService());

        // Act
        var actual = await service.Exists(99);

        // Assert
        Assert.False(actual);
    }

    /// <summary>
    /// This is a test that a data-input method works correctly.
    /// The DbContext used is a Mock object, just not in the same style as Moq/NSubstitute as it is a real database.
    /// We still treat it as a mock though, as all a mock is, is a way to isolate our code from production code.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddEmployeeToJob_AssignsEmployeeToJob()
    {
        // Arrange
        var dbContext = CreateContext();
        var service = new JobService(dbContext, new TaxService());

        // Act
        await service.AddEmployeeToJob(1, 2);
        var employeesForJob1 = await dbContext.Jobs
            .Include(j => j.Employees)
            .Where(j => j.Id == 1)
            .SelectMany(e => e.Employees)
            .Select(e => e.Id)
            .ToListAsync() ?? [];

        // Assert
        Assert.Contains(1, employeesForJob1);
        Assert.Contains(2, employeesForJob1);
    }
}
