namespace JobManagementSystem.Entities;

public class Job
{
    public int Id { get; set; }
    public required string Description { get; set; }

    public int JobCategoryId { get; set; }
    public JobCategory JobCategory { get; set; } = null!;

    public ICollection<Employee> Employees { get; } = [];
    public ICollection<TaxInformation> TaxInformation { get; } = [];
}
