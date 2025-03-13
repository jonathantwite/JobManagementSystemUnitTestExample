using JobManagementSystem.Entities;

namespace JobManagementSystem.Services;
public interface ITaxService
{
    IEnumerable<string> GetTaxLiabilities(Job job);
}