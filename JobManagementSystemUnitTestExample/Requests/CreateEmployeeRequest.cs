namespace JobManagementSystem.Requests;

public record CreateEmployeeRequest(
    string Forenames,
    string Surname,
    DateTime DateOfBirth,
    string EmailAddress,
    int JobRoleId);
