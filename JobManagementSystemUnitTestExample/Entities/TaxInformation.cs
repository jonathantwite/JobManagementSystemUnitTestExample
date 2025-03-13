namespace JobManagementSystem.Entities;

public class TaxInformation
{
    public int Id { get; set; }
    public required string Description { get; set; }

    public int JobId { get; set; }
    public Job Job { get; set; } = null!;

    public int TaxRegimeId { get; set; }
    public TaxRegime TaxRegime { get; set; } = null!;
}
