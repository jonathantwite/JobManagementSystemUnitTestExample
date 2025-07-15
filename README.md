# Job Management System Unit Test Example

A demonstration of one way to go about unit testing a new API project.

This style stresses separation of concerns within the code base by defining sharp boundaries between layers, with black-box testing targeting the public API of each layer and maximising the test code's resilience to refactoring.

## Controllers

Controllers are responsible for handling HTTP requests and responses. They should not contain business logic (outside status codes) or data access code. Instead, they delegate these responsibilities to service classes (or service-like classes, e.g. validators).

When testing, the service layer calls can be mocked so that the tests focus on what the controller is responsible for in a request-in-response-out blackbox-style test.

Validation is a contentious issue.  I prefer to focus on making my tests resistant to refactoring, so that we can rely on the tests to ensure that the code continues to work while refactoring.  Therefore, we don't want to worry about whether we are using *FluentValidation*, or any other library for validation.  I prefer to test the validation as part of the controller tests.  These tests already test that the output is correct depending on the input, so I can just extend this to test different types of invalid input.  If we were to test the `IValidator` separately, we would be tying my tests to the implementation of *FluentValidation*.

## Services

Service classes should be tested against intent.  We do not care about the implementation details, just that the service layer does what it is supposed to do.  The only mocks used are for shared or external dependencies - `IEmailSenderService` which would send emails via an external process - and the database file system, which is mocked using a replacement in-memory SQLite database for each test.

The `JobService.GetAllJobs()` method originally included the tax liabilities code within the service.  By not tying our testing to the implementation, we are able to refactor this code out into a separate service, `TaxLiabilityService`, without having to change the tests, therefore being able to use the current tests to ensure our new code works as expected.

Similarly, the `EmployeeService` contains a group of methods relating to creating a new employee.  Under encapsulation best practices, several of these methods should be private.  If you wanted to test them, you would have to make them public, which would break encapsulation.  Instead, we can test the public methods that use these private methods, and ensure that they work as expected.  As this service grows, we will probably want to refactor the create employee methods into a separate service, by writing these tests as black-box tests not tied to implementation, we can do this without having to change the tests.

## Integration Tests

`EmployeeService.CreateEmployee()` is an "orchestrator" or "controller" type method - that is, a method with very little logic, but organises many other components.  By writing code in this style, there is no logic in this method and so no unit testing needs to be done.  This method is then testing as an integration test - a test that tests the interaction of multiple components together.

For integration tests, we only want to mock out external dependencies - such as the `EmailSenderService` - and shared dependencies that affect other tests (these are not full end-2-end tests).  The database file system can be mocked by using a different context - as is the case in this project - or use a blank real.  This is a cost-vs-reward decision as using a blank MS SQL database will make the tests slower, but more realistic.  The code in this project does nothing particularly complex and so an in-memory SQLite database is sufficient for the integration tests.
