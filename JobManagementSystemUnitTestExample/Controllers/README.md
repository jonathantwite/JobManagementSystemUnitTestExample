# Controllers

## Job Controller

### `AddEmployeeToJob()`

This is an example of a controller method that only considers the response required given the request.  When testing, the service layer calls can be mocked so that the tests focus on what the controller is responsible for.

## Employee Controller

### `CreateEmployee()`

This is an example of a controller method with validation using FluentValidation.  When testing, if possible, we do not want to be tied into the FluentValidation library so that we can swap this library out.  It would be better if Microsoft supplied a validation abstraction (like they do with logging) so that we could inject any validation library we wanted to.  They do not currently do this, so we must use the FluentValidation interfaces.
