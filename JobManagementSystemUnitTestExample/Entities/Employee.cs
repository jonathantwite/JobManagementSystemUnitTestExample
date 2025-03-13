namespace JobManagementSystem.Entities;

public class Employee
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public int JobRoleId { get; set; }
    public JobRole JobRole { get; set; } = null!;
    public ICollection<Job> AssignedJobs { get; } = new List<Job>();
}
