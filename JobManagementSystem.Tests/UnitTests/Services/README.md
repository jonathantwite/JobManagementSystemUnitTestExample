# Service Tests

## Communication Service Tests

## Job Service Tests

These are unit tests for the JobService.  As this is a service, we wish to test the functionality when a "unit of work" happens

### `GetAllJobs_ReturnsCorrectNumberOfRecords()`

This test that the GetAllJobs() function returns the correct number of results

### `GetAllJobs_ReturnsDescription()`

This tests that the GetAllJobs() function returns the correct values

### `GetAllJobs_ReturnsTaxLiabilities()`

This test show the difference between the Detroit (Classical) and London (Mockist/Mock-everywhere) style of testing.  A London test would mock out the TaxService and test that the value provided by the TaxService is placed in the DTO correctly.  More tests would be required to test the TaxService.  The issue with this is that the tests now become aware, and dependent on, the code architecture, and refactoring will cause the tests to fail.  This failure does not add value as it is a false negative.  

This test is written in a Classical style which defines an "Isolated Test" as an "Isolate piece of work" and only cares about one thing - is the data returned correct.  We do not care how the TaxService works (nor any other dependencies), we would only mock external or persistence dependencies.

### `Exists_ReturnsTrueIfJobFound()`

These tests test that a simple function works correctly

### `AddEmployeeToJob_AssignsEmployeeToJob()`

This is a test that a data-input method works correctly.  The DbContext used is a Mock object, just not in the same style as Moq/NSubstitute as it is a real database.  We still treat it as a mock though, as all a mock is, is a way to isolate our code from production code.

## Tax Service Tests

The `TaxService` has complicated logic in it that is not always easy to setup from other services.  Therefore the `TaxService` can be argued to warrant its own tests.
