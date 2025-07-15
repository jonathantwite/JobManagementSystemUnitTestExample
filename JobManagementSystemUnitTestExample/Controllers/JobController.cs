using JobManagementSystem.Services;
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

    /// <summary>
    /// This is an example of a controller method that only considers the response required given the request.
    /// When testing, the service layer calls can be mocked so that the tests focus on what the controller is responsible for.
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="employeeId"></param>
    /// <returns></returns>
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
