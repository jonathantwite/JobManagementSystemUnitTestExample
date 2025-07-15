
using JobManagementSystem.Requests;

namespace JobManagementSystem.Services;

public interface IEmployeeService
{
    Task<int> CreateEmployee(CreateEmployeeRequest employeeName);
    Task<bool> Exists(int id);
}