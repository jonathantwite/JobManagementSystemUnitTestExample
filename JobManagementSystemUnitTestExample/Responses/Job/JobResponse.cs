namespace JobManagementSystem.Responses.Job;

public record JobResponse (
    int Id,
    string Description,
    JobCategoryResponse Category,
    IEnumerable<string> TaxLiabilities,
    IEnumerable<EmployeeResponse> Employees);
