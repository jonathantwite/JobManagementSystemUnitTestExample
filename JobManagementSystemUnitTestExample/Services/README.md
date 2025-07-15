# Services

## Communication Service

## Email Sender Service

This service would contain code tightly coupling the service to an email provider.  This service is therefore mocked in unit and integration tests to ensure that the tests can run without the provider being available.

## Employee Service

As this class gets bigger an more complicated, you will want to break out the CreateEmployee section of methods into a separate employee-creation service class.  You want to be able to do this without breaking your tests, otherwise your tests will be of no help while implementing this refactoring.  By testing against the public interface of this class and not the internal implementation, you can refactor the implementation without breaking the tests, giving you more assurity for the new code you have written.

### `CreateEmployee()`

This is a "orchestrator" or "controller" method that contains very little logic, but instead calls other methods to do the work.  A test against this method is technically an integration test, as it tests the interaction between multiple methods and services.  Do we care? probably not, but the issue does arise that it cannot be tested in a pure-functional way, and it may change a lot of state.  Personally, I would flag this as an integration test, write it in the same style as a unit test, but understand that it will be slower and more complicated to maintain.

### `CreateEmployeeRecord()`

This is a private method and so should stay private.  We do not want to make it public just to test it.

## Job Service

### `GetAllJobs()`

This method uses the TaxService, however, when testing, we do not care about this internal implementation detail.

## Special User Service

### `GetSpecialUserEmailAddress()`

Does this even need testing?  I say no - there is very little logic here - just test that the code where this is called from gives the correct result.

## Tax Service

### `GetIndicativeTaxPercentage()`

Does this need testing - I suggest yes, as it contains logic that is not trivial and could be prone to errors and/or problems with edge-cases.