using JobManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobController(IJobService jobService, IEmployeeService employeeService) : ControllerBase
{
    public readonly IJobService _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
    public readonly IEmployeeService _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return new JsonResult(await _jobService.GetAllJobs());
    }

    [HttpPatch]
    public async Task<IActionResult> AddEmployeeToJob(int jobId, [FromBody]int employeeId)
    {
        if (!await _jobService.Exists(jobId))
        {
            return NotFound();
        }

        if (!await _employeeService.Exists(employeeId))
        {
            return new ObjectResult("Unknown employee")
            {
                StatusCode = 422
            };
        }

        await _jobService.AddEmployeeToJob(jobId, employeeId);

        return NoContent();
    }
}
