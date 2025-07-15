using JobManagementSystem.Models;

namespace JobManagementSystem.Services;
public interface ISpecialUserService
{
    string GetSpecialUserEmailAddress(SpecialUser userType);
}