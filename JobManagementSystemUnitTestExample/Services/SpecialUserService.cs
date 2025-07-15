using JobManagementSystem.Models;

namespace JobManagementSystem.Services;

public class SpecialUserService : ISpecialUserService
{
    /// <summary>
    /// Does this even need testing?  I say no - there is very little logic here - just test that the code where this is called from gives the correct result.
    /// </summary>
    /// <param name="userType"></param>
    /// <returns></returns>
    public string GetSpecialUserEmailAddress(SpecialUser userType) =>
        userType switch
        {
            SpecialUser.CEO => "ceo@company.com",
            SpecialUser.CFO => "cfo@company.com",
            SpecialUser.HrManager => "hr@company.com",
            _ => throw new ArgumentOutOfRangeException(nameof(userType), userType, "Invalid special user type")
        };
}
