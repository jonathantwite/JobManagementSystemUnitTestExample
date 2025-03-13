namespace JobManagementSystem.Entities;

public class JobCategory
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public ICollection<Job> Jobs { get; } = [];
}
