using JobManagementSystem.Responses.Job;

namespace JobManagementSystem.Services;
public interface IJobService
{
    Task AddEmployeeToJob(int jobId, int employeeId);
    Task<bool> Exists(int id);
    Task<IEnumerable<JobResponse>> GetAllJobs();
}