namespace JobManagementSystem.Models;

public record EmailCommunication (
    string ToAddress,
    string? CcAddress,
    string? BccAddress,
    string Subject,
    string Body
);
