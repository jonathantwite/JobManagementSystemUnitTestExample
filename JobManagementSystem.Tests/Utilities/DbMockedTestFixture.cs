using JobManagementSystem.DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace JobManagementSystem.Tests.Utilities;

/// <summary>
/// This is the current MS recommended way of "mocking" the database.
/// MS recommend using your live database, but if not wanted, then use an in-memory SQLite database.
/// </summary>
public abstract class DbMockedTestFixture: IDisposable
{
    private protected SqliteConnection _connection;
    private protected DbContextOptions<JobManagementContext> _contextOptions;

    public DbMockedTestFixture()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<JobManagementContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new JobManagementContext(_contextOptions);
        context.Database.EnsureCreated();
    }

    private protected JobManagementContext CreateContext() => new JobManagementContext(_contextOptions);

    public void Dispose() => _connection.Dispose();
}
