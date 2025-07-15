# Data Access Tests

## Seed Data Extension Method Tests

This set of tests confirms that the setup method runs and inserts data.  It is a FLAKY test as it depends on the data in the `SeedData()` method that could change at any time.  Therefore, we ensure to test a wide range of values are correctly there, but do not try to have 100% coverage as this is unsustainable in the future.  

One thing we definitely check is that, where multiple values are inserted, we do not just check for the first value, but look at the second value.  This is to check that the we haven't accidentally, for example, used `Add()` rather than `AddRange()`.  Note, the further along an array we look, the more likely that the data will be changed in the future breaking this test.  Therefore, currently, we only look within the first two items and no further.
