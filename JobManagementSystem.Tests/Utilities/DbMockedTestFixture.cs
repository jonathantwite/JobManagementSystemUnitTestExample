using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace JobManagementSystem.Tests.Utilities;

/// <summary>
/// This is the current MS recommended way of "mocking" the database.
/// MS recommend using your live database, but if not wanted, then use an in-memory SQLite database.
/// </summary>
public abstract class DbMockedTestFixture<T> : IDisposable where T : DbContext
{
    private protected SqliteConnection _connection;
    private protected DbContextOptions<T> _contextOptions;

    public DbMockedTestFixture()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<T>()
            .UseSqlite(_connection)
            .Options;

        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    private protected T CreateContext() => (T)Activator.CreateInstance(typeof(T), _contextOptions);

    public void Dispose()
    {
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
