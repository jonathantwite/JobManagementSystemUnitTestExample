using JobManagementSystem.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace JobManagementSystem.Services;

public class EmployeeService(JobManagementContext dbContext) : IEmployeeService
{
    private readonly JobManagementContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<bool> Exists(int id) => (await _dbContext.Employees.SingleOrDefaultAsync(j => j.Id == id)) != null;
}
