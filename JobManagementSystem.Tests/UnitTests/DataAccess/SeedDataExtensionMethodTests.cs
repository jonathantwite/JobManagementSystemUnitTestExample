using JobManagementSystem.DataAccess;
using JobManagementSystem.Entities;

namespace JobManagementSystem.Tests.UnitTests.DataAccess;

/// <summary>
/// This set of tests confirms that the setup method runs and inserts data.
/// It is a FLAKY test as it depends on the data in the SeedData() method that could change at any time.
/// Therefore, we ensure to test a wide range of values are correctly there, but do not try to have 100% coverage
/// as this is unsustainable in the future.
/// One thing we definitely check is that, where multiple values are inserted, we do not just check for the first value, but look at the second value.
/// This is to check that the we haven't accidentally, for example, used Add() rather than AddRange().
/// Note, the further along an array we look, the more likely that the data will be changed in the future breaking this test.
/// Therefore, currently, we only look within the first two items and no further.
/// </summary>
public class SeedDataExtensionMethodTests : DbMockedTestFixture<JobManagementContext>
{
    [Fact]
    public void SeedData_Runs()
    {
        // Arrange
        var dbContext = CreateContext();

        // Act
        var exception = Record.Exception(dbContext.SeedData);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void SeedData_AddsData()
    {
        // Arrange
        var dbContext = CreateContext();

        // Act
        dbContext.SeedData();
        var tr2 = new TaxRegime() { Id = 2, CountryCode = "USA", Description = "Overseas - United States", MinimumThreshold = 1000M, TaxRate = 0.14M };
        var jc1 = new JobCategory() { Id = 1, Description = "Management" };
        var jr2 = new JobRole() { Id = 2, Description = "CFO" };
        var e2 = new Employee() { Id = 2, Name = "Betty Black", Email = "bb@company.com", JobRoleId = jr2.Id, JobRole = jr2 };
        var j2 = new Job() { Id = 2, Description = "Year end accounts", JobCategoryId = jc1.Id, JobCategory = jc1 };

        // Assert
        Assert.Equivalent(tr2, dbContext.TaxRegimes.SingleOrDefault(tr => tr.Id == tr2.Id));
        Assert.Equivalent(jc1, dbContext.JobCategories.SingleOrDefault(tr => tr.Id == jc1.Id));
        Assert.Equivalent(e2, dbContext.Employees.SingleOrDefault(tr => tr.Id == e2.Id));
        Assert.Equivalent(j2, dbContext.Jobs.SingleOrDefault(tr => tr.Id == j2.Id));
    }
}
