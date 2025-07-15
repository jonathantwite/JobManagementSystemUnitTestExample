# Controller Tests

## Employee Controller Tests

### `CreateEmployee_Returns422ForInvalidEmail()`

This is a contentious one.  Should you mock out out the validation logic and purely test the controller logic, or should you test the validation logic here as well?  I personally prefer to include the validation logic in the controller tests, as mocking it out ties you to the implementation of the validation logic.  Currently FluentValidation is used, if we want to change the validation to use a different library, we would have to change the mock as well.  By testing only that the API returns the correct response, we can change the implementation of the validation logic without having to change the logic in tests (except the format of the text output).

### `CreateEmployee_Returns422ForTooLongForename()`

This test shows an interesting case where code coverage fails to cover everything required.  Technically, every line of the Forename validator is covered and so hits 100% coverage, however, we never actually test the text output that is returned to the user.

### `CreateEmployee_Returns201ForValidRequest()`

Question: should these tests all be split into two tests? Maybe (probably?).  The issue is that the test is actually testing two different things -  the response code and the data returned.  If split into two tests, the description of the test would be more accurate, but it would also double the number of tests.

## Job Controller Tests

These tests test the user-interacting logic contained within the JobController.  They mostly test for two things:

* the correct status codes being returned
* the correct data in any response object

These tests completely ignore how the services work. Note, as far as I am aware, this code does not use any JSON serialization, even when using JsonResult.  Therefore integration or end-2-end tests are still required to ensure that the data is being serialized correctly (e.g. empty arrays do not become nulls)

### `Get_Returns200()`

This test tests for the correct 200 response and that the data in the response has not been altered from the data returned by the JobService.

### `Get_Returns200AndEmptyArrayWithNoData()`

This test ensures that when there is no jobs to return, an empty array is returned (rather than null, or a 404).  The resource "bucket" was found, but was empty, so this was a successful call to the API, hence the 200 response.

### `AddEmployeeToJob_Returns422ForUnknownEmployee()`

This test checks the validation failure that occurs when the supplied employee id is not found.  The employee id is supplied as part of the body of the request, therefore API endpoint resource (the job) is found and so this is not a 404, but a 422 Unprocessable Entity.

### `AddEmployeeToJob_Returns404ForUnknownJob()`

This test checks the validation failure that occurs when the supplied job id is not found.  The job id is part of the API route (and so is the "resource" being requested) and therefore if the job is not found, the endpoint should return a 404 Not Found.

### `AddEmployeeToJob_Returns404ForBothUnknownJobAndEmployee()`

This test checks the validation failure that occurs when both the supplied job id and the supplied employee id are not found.  The job id is part of the API route (and so is the "resource" being requested) and therefore if the job is not found, the endpoint should return a 404 Not Found.  This will then ignore the employee id as the resource was not found to start processing.

### `AddEmployeeToJob_Returns204WhenOk()`

This test checks that the correct HTTP status code of 204 (No Content) is returned when the update patch runs.

