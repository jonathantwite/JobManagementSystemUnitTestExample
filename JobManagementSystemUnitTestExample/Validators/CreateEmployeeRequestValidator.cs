using FluentValidation;
using JobManagementSystem.Requests;

namespace JobManagementSystem.Validators;

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
    private const int maxForenamesLength = 500;
    private const int maxSurnameLength = 200;

    public CreateEmployeeRequestValidator()
    {
        RuleFor(request => request.Forenames)
            .MaximumLength(maxForenamesLength).WithMessage($"Employee forenames cannot exceed {maxForenamesLength} characters.");

        RuleFor(request => request.Surname)
            .NotEmpty().WithMessage("Employee surname cannot be empty.")
            .MaximumLength(maxSurnameLength).WithMessage($"Employee surname cannot exceed {maxSurnameLength} characters.");

        RuleFor(request => request.EmailAddress)
            .NotEmpty().WithMessage("Email address cannot be empty.")
            .EmailAddress().WithMessage("Email address must be a valid email format.");

        RuleFor(request => request.DateOfBirth)
            .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");
    }
}
