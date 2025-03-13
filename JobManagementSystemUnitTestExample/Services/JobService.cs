using JobManagementSystem.DataAccess;
using JobManagementSystem.Responses.Job;
using Microsoft.EntityFrameworkCore;

namespace JobManagementSystem.Services;

public class JobService(JobManagementContext dbContext, ITaxService taxService) : IJobService
{
    private readonly JobManagementContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly ITaxService _taxService = taxService ?? throw new ArgumentNullException(nameof(taxService));

    public async Task<IEnumerable<JobResponse>> GetAllJobs()
    {
        var jobs = await _dbContext.Jobs
            .Include(j => j.TaxInformation)
            .ThenInclude(ti => ti.TaxRegime)
            .Include(j => j.JobCategory)
            .Include(j => j.Employees)
            .ToListAsync();

        return jobs.Select(j => new JobResponse(
            j.Id,
            j.Description,
            new JobCategoryResponse(j.JobCategory.Id, j.JobCategory.Description),
            _taxService.GetTaxLiabilities(j),
            j.Employees.Select(e => new EmployeeResponse(e.Id, e.Name))));
    }

    public async Task<bool> Exists(int id) => (await _dbContext.Jobs.SingleOrDefaultAsync(j => j.Id == id)) != null;

    public async Task AddEmployeeToJob(int jobId, int employeeId)
    {
        var job = await _dbContext.Jobs.FirstOrDefaultAsync(j => j.Id == jobId) ?? throw new ArgumentException($"Unable to find Job {jobId}");
        var employee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.Id == employeeId) ?? throw new ArgumentException($"Unable to fine Employee {employeeId}");


        job.Employees.Add(employee);
        await _dbContext.SaveChangesAsync();
    }
}
