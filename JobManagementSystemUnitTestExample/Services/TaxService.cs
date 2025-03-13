using JobManagementSystem.Entities;

namespace JobManagementSystem.Services;

public class TaxService : ITaxService
{
    public IEnumerable<string> GetTaxLiabilities(Job job) =>
        job.TaxInformation.Select(ti => ti.TaxRegime.CountryCode);

    public decimal GetIndicativeTaxPercentage(decimal referenceValue, IEnumerable<TaxRegime> regimes) =>
        regimes.Select(r => Math.Max(referenceValue - r.MinimumThreshold, 0) * r.TaxRate)
            .Sum() / referenceValue;
}
