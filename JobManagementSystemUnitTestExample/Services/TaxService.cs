using JobManagementSystem.Entities;

namespace JobManagementSystem.Services;

public class TaxService : ITaxService
{
    public IEnumerable<string> GetTaxLiabilities(Job job) =>
        job.TaxInformation.Select(ti => ti.TaxRegime.CountryCode);

    /// <summary>
    /// Does this need testing - I suggest yes, as it contains logic that is not trivial and could be prone to errors and/or problems with edge-cases.
    /// </summary>
    /// <param name="referenceValue"></param>
    /// <param name="regimes"></param>
    /// <returns></returns>
    public decimal GetIndicativeTaxPercentage(decimal referenceValue, IEnumerable<TaxRegime> regimes) =>
        regimes.Select(r => Math.Max(referenceValue - r.MinimumThreshold, 0) * r.TaxRate)
            .Sum() / referenceValue;
}
