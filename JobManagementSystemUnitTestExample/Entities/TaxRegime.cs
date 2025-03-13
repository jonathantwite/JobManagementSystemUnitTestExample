namespace JobManagementSystem.Entities;

public class TaxRegime
{
    public int Id { get; set; }
    public required string CountryCode { get; set; }
    public required string Description { get; set; }
    public required decimal MinimumThreshold { get; set; }
    public required decimal TaxRate { get; set; }

    public ICollection<TaxInformation> TaxInformations { get; } = [];
}
