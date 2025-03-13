
namespace JobManagementSystem.Services;

public interface IEmployeeService
{
    Task<bool> Exists(int id);
}