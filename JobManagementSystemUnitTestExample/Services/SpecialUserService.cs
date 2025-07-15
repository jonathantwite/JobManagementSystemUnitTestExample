using JobManagementSystem.Models;

namespace JobManagementSystem.Services;

public class SpecialUserService : ISpecialUserService
{
    public string GetSpecialUserEmailAddress(SpecialUser userType) =>
        userType switch
        {
            SpecialUser.CEO => "ceo@company.com",
            SpecialUser.CFO => "cfo@company.com",
            SpecialUser.HrManager => "hr@company.com",
        };
}
