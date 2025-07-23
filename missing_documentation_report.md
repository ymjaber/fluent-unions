# Missing XML Documentation Report

**Total files with missing documentation:** 102
**Total public members without documentation:** 458

## Summary by Member Type

- **Method:** 437
- **Struct:** 15
- **Class:** 6

## Summary by Area

### Option (162 missing)

- `Option/Extensions/AsynchronousMethods/Task/PreDefinedEvaluate/String.cs`: 20 missing
- `Option/Extensions/AsynchronousMethods/ValueTask/PreDefinedEvaluate/String.cs`: 20 missing
- `Option/Extensions/AsynchronousMethods/Task/PreDefinedEvaluate/Numeric.cs`: 11 missing
- `Option/Extensions/AsynchronousMethods/ValueTask/PreDefinedEvaluate/Numeric.cs`: 11 missing
- `Option/Extensions/AsynchronousMethods/Task/Actions.cs`: 9 missing
- `Option/Extensions/AsynchronousMethods/ValueTask/Actions.cs`: 9 missing
- `Option/Extensions/AsynchronousMethods/Task/PreDefinedEvaluate/DateTime.cs`: 8 missing
- `Option/Extensions/AsynchronousMethods/ValueTask/PreDefinedEvaluate/DateTime.cs`: 8 missing
- `Option/Extensions/AsynchronousMethods/Task/FilterBuilderExtensions.cs`: 7 missing
- `Option/Extensions/AsynchronousMethods/ValueTask/FilterBuilderExtensions.cs`: 7 missing
- *...and 20 more files*

### UnitResult (78 missing)

- `Result/UnitResult/Extensions/AsynchronousMethods/ValueTasks/Actions.cs`: 9 missing
- `Result/UnitResult/Extensions/AsynchronousMethods/Tasks/Bind.cs`: 6 missing
- `Result/UnitResult/Extensions/AsynchronousMethods/Tasks/BindAll.cs`: 6 missing
- `Result/UnitResult/Extensions/AsynchronousMethods/Tasks/WithValue.cs`: 6 missing
- `Result/UnitResult/Extensions/AsynchronousMethods/ValueTasks/Bind.cs`: 6 missing
- `Result/UnitResult/Extensions/AsynchronousMethods/ValueTasks/BindAll.cs`: 6 missing
- `Result/UnitResult/Extensions/AsynchronousMethods/ValueTasks/WithValue.cs`: 6 missing
- `Result/UnitResult/Extensions/AsynchronousMethods/Tasks/Ensure.cs`: 3 missing
- `Result/UnitResult/Extensions/AsynchronousMethods/Tasks/EnsureAll.cs`: 3 missing
- `Result/UnitResult/Extensions/AsynchronousMethods/Tasks/Match.cs`: 3 missing
- *...and 18 more files*

### ValueResult (218 missing)

- `Result/ValueResult/Extensions/AsynchronousMethods/Task/PreDefinedEnsure/String/Predicates.cs`: 20 missing
- `Result/ValueResult/Extensions/AsynchronousMethods/ValueTask/PreDefinedEnsure/String/Predicates.cs`: 20 missing
- `Result/ValueResult/Extensions/AsynchronousMethods/Task/PreDefinedEnsure/Numeric/Predicates.cs`: 11 missing
- `Result/ValueResult/Extensions/AsynchronousMethods/ValueTask/PreDefinedEnsure/Numeric/Predicates.cs`: 11 missing
- `Result/ValueResult/Extensions/PreDefinedEnsure/Numeric/Predicates.cs`: 11 missing
- `Result/ValueResult/Extensions/AsynchronousMethods/Task/EnsureBuilderExtensions.cs`: 10 missing
- `Result/ValueResult/Extensions/AsynchronousMethods/ValueTask/EnsureBuilderExtensions.cs`: 10 missing
- `Result/ValueResult/Extensions/AsynchronousMethods/Task/Actions.cs`: 9 missing
- `Result/ValueResult/Extensions/AsynchronousMethods/ValueTask/Actions.cs`: 9 missing
- `Result/ValueResult/Extensions/AsynchronousMethods/Task/PreDefinedEnsure/DateTime/Predicates.cs`: 8 missing
- *...and 34 more files*

## Detailed File List

Files sorted by number of missing documentation:

### Result/ValueResult/Extensions/AsynchronousMethods/Task/PreDefinedEnsure/String/Predicates.cs (20 missing)

**Methods:**
- Line 8: `public static async Task<Result> Empty(`
- Line 13: `public static async Task<EnsureBuilder<string>> NotEmpty(`
- Line 18: `public static async Task<EnsureBuilder<string>> HasLength(`
- Line 24: `public static async Task<EnsureBuilder<string>> LongerThan(`
- Line 30: `public static async Task<EnsureBuilder<string>> LongerThanOrEqualTo(`
- *...and 15 more*

### Result/ValueResult/Extensions/AsynchronousMethods/ValueTask/PreDefinedEnsure/String/Predicates.cs (20 missing)

**Methods:**
- Line 8: `public static async ValueTask<Result> Empty(`
- Line 13: `public static async ValueTask<EnsureBuilder<string>> NotEmpty(`
- Line 18: `public static async ValueTask<EnsureBuilder<string>> HasLength(`
- Line 24: `public static async ValueTask<EnsureBuilder<string>> LongerThan(`
- Line 30: `public static async ValueTask<EnsureBuilder<string>> LongerThanOrEqualTo(`
- *...and 15 more*

### Option/Extensions/AsynchronousMethods/Task/PreDefinedEvaluate/String.cs (20 missing)

**Methods:**
- Line 8: `public static async Task<bool> Empty(this Task<FilterBuilder<string>> filter)`
- Line 11: `public static async Task<FilterBuilder<string>> NotEmpty(this Task<FilterBuilder...`
- Line 14: `public static async Task<FilterBuilder<string>> HasLength(this Task<FilterBuilde...`
- Line 17: `public static async Task<FilterBuilder<string>> LongerThan(this Task<FilterBuild...`
- Line 20: `public static async Task<FilterBuilder<string>> LongerThanOrEqualTo(this Task<Fi...`
- *...and 15 more*

### Option/Extensions/AsynchronousMethods/ValueTask/PreDefinedEvaluate/String.cs (20 missing)

**Methods:**
- Line 8: `public static async ValueTask<bool> Empty(this ValueTask<FilterBuilder<string>> ...`
- Line 11: `public static async ValueTask<FilterBuilder<string>> NotEmpty(this ValueTask<Fil...`
- Line 14: `public static async ValueTask<FilterBuilder<string>> HasLength(this ValueTask<Fi...`
- Line 17: `public static async ValueTask<FilterBuilder<string>> LongerThan(this ValueTask<F...`
- Line 20: `public static async ValueTask<FilterBuilder<string>> LongerThanOrEqualTo(this Va...`
- *...and 15 more*

### Result/ValueResult/Extensions/AsynchronousMethods/Task/PreDefinedEnsure/Numeric/Predicates.cs (11 missing)

**Methods:**
- Line 8: `public static async Task<EnsureBuilder<TValue>> GreaterThan<TValue>(`
- Line 14: `public static async Task<EnsureBuilder<TValue>> GreaterThanOrEqualTo<TValue>(`
- Line 20: `public static async Task<EnsureBuilder<TValue>> LessThan<TValue>(`
- Line 26: `public static async Task<EnsureBuilder<TValue>> LessThanOrEqualTo<TValue>(`
- Line 32: `public static async Task<EnsureBuilder<TValue>> InRange<TValue>(`
- *...and 6 more*

### Result/ValueResult/Extensions/AsynchronousMethods/ValueTask/PreDefinedEnsure/Numeric/Predicates.cs (11 missing)

**Methods:**
- Line 8: `public static async ValueTask<EnsureBuilder<TValue>> GreaterThan<TValue>(`
- Line 14: `public static async ValueTask<EnsureBuilder<TValue>> GreaterThanOrEqualTo<TValue...`
- Line 20: `public static async ValueTask<EnsureBuilder<TValue>> LessThan<TValue>(`
- Line 26: `public static async ValueTask<EnsureBuilder<TValue>> LessThanOrEqualTo<TValue>(`
- Line 32: `public static async ValueTask<EnsureBuilder<TValue>> InRange<TValue>(`
- *...and 6 more*

### Result/ValueResult/Extensions/PreDefinedEnsure/Numeric/Predicates.cs (11 missing)

**Methods:**
- Line 9: `public static EnsureBuilder<TValue> GreaterThan<TValue>(`
- Line 15: `public static EnsureBuilder<TValue> GreaterThanOrEqualTo<TValue>(`
- Line 21: `public static EnsureBuilder<TValue> LessThan<TValue>(`
- Line 27: `public static EnsureBuilder<TValue> LessThanOrEqualTo<TValue>(`
- Line 33: `public static EnsureBuilder<TValue> InRange<TValue>(`
- *...and 6 more*

### Option/Extensions/AsynchronousMethods/Task/PreDefinedEvaluate/Numeric.cs (11 missing)

**Methods:**
- Line 8: `public static async Task<FilterBuilder<TValue>> GreaterThan<TValue>(this Task<Fi...`
- Line 13: `public static async Task<FilterBuilder<TValue>> GreaterThanOrEqualTo<TValue>(thi...`
- Line 18: `public static async Task<FilterBuilder<TValue>> LessThan<TValue>(this Task<Filte...`
- Line 22: `public static async Task<FilterBuilder<TValue>> LessThanOrEqualTo<TValue>(this T...`
- Line 27: `public static async Task<FilterBuilder<TValue>> InRange<TValue>(`
- *...and 6 more*

### Option/Extensions/AsynchronousMethods/ValueTask/PreDefinedEvaluate/Numeric.cs (11 missing)

**Methods:**
- Line 8: `public static async ValueTask<FilterBuilder<TValue>> GreaterThan<TValue>(this Va...`
- Line 13: `public static async ValueTask<FilterBuilder<TValue>> GreaterThanOrEqualTo<TValue...`
- Line 18: `public static async ValueTask<FilterBuilder<TValue>> LessThan<TValue>(this Value...`
- Line 22: `public static async ValueTask<FilterBuilder<TValue>> LessThanOrEqualTo<TValue>(t...`
- Line 27: `public static async ValueTask<FilterBuilder<TValue>> InRange<TValue>(`
- *...and 6 more*

### Result/ValueResult/Extensions/AsynchronousMethods/Task/EnsureBuilderExtensions.cs (10 missing)

**Methods:**
- Line 6: `public static async Task<Result<TValue>> BuildAsync<TValue>(this Task<EnsureBuil...`
- Line 9: `public static async Task<Result<TTarget>> MapAsync<TSource, TTarget>(`
- Line 18: `public static async Task<Result<TTarget>> MapAsync<TSource, TTarget>(`
- Line 22: `public static async Task<Result<TTarget>> MapAsync<TSource, TTarget>(`
- Line 26: `public static Task<Result<TTarget>> BindAsync<TSource, TTarget>(`
- *...and 5 more*

### Result/ValueResult/Extensions/AsynchronousMethods/ValueTask/EnsureBuilderExtensions.cs (10 missing)

**Methods:**
- Line 6: `public static async ValueTask<Result<TValue>> BuildAsync<TValue>(this ValueTask<...`
- Line 9: `public static async ValueTask<Result<TTarget>> MapAsync<TSource, TTarget>(`
- Line 18: `public static async ValueTask<Result<TTarget>> MapAsync<TSource, TTarget>(`
- Line 22: `public static async ValueTask<Result<TTarget>> MapAsync<TSource, TTarget>(`
- Line 26: `public static ValueTask<Result<TTarget>> BindAsync<TSource, TTarget>(`
- *...and 5 more*

### Result/UnitResult/Extensions/AsynchronousMethods/ValueTasks/Actions.cs (9 missing)

**Methods:**
- Line 6: `public static async ValueTask<Result> OnSuccessAsync(this Result result, Func<Va...`
- Line 12: `public static async ValueTask<Result> OnSuccessAsync(this ValueTask<Result> resu...`
- Line 15: `public static async ValueTask<Result> OnSuccessAsync(this ValueTask<Result> resu...`
- Line 18: `public static async ValueTask<Result> OnFailureAsync(this Result result, Func<Er...`
- Line 24: `public static async ValueTask<Result> OnFailureAsync(this ValueTask<Result> resu...`
- *...and 4 more*

### Result/ValueResult/Extensions/AsynchronousMethods/Task/Actions.cs (9 missing)

**Methods:**
- Line 6: `public static async Task<Result<TValue>> OnSuccessAsync<TValue>(this Result<TVal...`
- Line 12: `public static async Task<Result<TValue>> OnSuccessAsync<TValue>(this Task<Result...`
- Line 15: `public static async Task<Result<TValue>> OnSuccessAsync<TValue>(`
- Line 20: `public static async Task<Result<TValue>> OnFailureAsync<TValue>(this Result<TVal...`
- Line 26: `public static async Task<Result<TValue>> OnFailureAsync<TValue>(this Task<Result...`
- *...and 4 more*

### Result/ValueResult/Extensions/AsynchronousMethods/ValueTask/Actions.cs (9 missing)

**Methods:**
- Line 6: `public static async ValueTask<Result<TValue>> OnSuccessAsync<TValue>(this Result...`
- Line 12: `public static async ValueTask<Result<TValue>> OnSuccessAsync<TValue>(this ValueT...`
- Line 15: `public static async ValueTask<Result<TValue>> OnSuccessAsync<TValue>(`
- Line 20: `public static async ValueTask<Result<TValue>> OnFailureAsync<TValue>(this Result...`
- Line 26: `public static async ValueTask<Result<TValue>> OnFailureAsync<TValue>(this ValueT...`
- *...and 4 more*

### Option/Extensions/AsynchronousMethods/Task/Actions.cs (9 missing)

**Methods:**
- Line 6: `public static async Task<Option<TValue>> OnSomeAsync<TValue>(`
- Line 15: `public static async Task<Option<TValue>> OnSomeAsync<TValue>(`
- Line 21: `public static async Task<Option<TValue>> OnSomeAsync<TValue>(`
- Line 27: `public static async Task<Option<TValue>> OnNoneAsync<TValue>(`
- Line 36: `public static async Task<Option<TValue>> OnNoneAsync<TValue>(`
- *...and 4 more*

### Option/Extensions/AsynchronousMethods/ValueTask/Actions.cs (9 missing)

**Methods:**
- Line 6: `public static async ValueTask<Option<TValue>> OnSomeAsync<TValue>(`
- Line 15: `public static async ValueTask<Option<TValue>> OnSomeAsync<TValue>(`
- Line 21: `public static async ValueTask<Option<TValue>> OnSomeAsync<TValue>(`
- Line 27: `public static async ValueTask<Option<TValue>> OnNoneAsync<TValue>(`
- Line 36: `public static async ValueTask<Option<TValue>> OnNoneAsync<TValue>(`
- *...and 4 more*

### Result/ValueResult/Extensions/AsynchronousMethods/Task/PreDefinedEnsure/DateTime/Predicates.cs (8 missing)

**Methods:**
- Line 6: `public static async Task<EnsureBuilder<DateTime>> InFuture(`
- Line 11: `public static async Task<EnsureBuilder<DateOnly>> InFuture(`
- Line 16: `public static async Task<EnsureBuilder<TimeOnly>> InFuture(`
- Line 21: `public static async Task<EnsureBuilder<DateTime>> InPast(`
- Line 26: `public static async Task<EnsureBuilder<DateOnly>> InPast(`
- *...and 3 more*

### Result/ValueResult/Extensions/AsynchronousMethods/ValueTask/PreDefinedEnsure/DateTime/Predicates.cs (8 missing)

**Methods:**
- Line 6: `public static async ValueTask<EnsureBuilder<DateTime>> InFuture(`
- Line 11: `public static async ValueTask<EnsureBuilder<DateOnly>> InFuture(`
- Line 16: `public static async ValueTask<EnsureBuilder<TimeOnly>> InFuture(`
- Line 21: `public static async ValueTask<EnsureBuilder<DateTime>> InPast(`
- Line 26: `public static async ValueTask<EnsureBuilder<DateOnly>> InPast(`
- *...and 3 more*

### Result/ValueResult/Extensions/PreDefinedEnsure/DateTime/Predicates.cs (8 missing)

**Methods:**
- Line 8: `public static EnsureBuilder<DateTime> InFuture(`
- Line 13: `public static EnsureBuilder<DateOnly> InFuture(`
- Line 19: `public static EnsureBuilder<TimeOnly> InFuture(`
- Line 25: `public static EnsureBuilder<DateTime> InPast(`
- Line 30: `public static EnsureBuilder<DateOnly> InPast(`
- *...and 3 more*

### Option/Extensions/AsynchronousMethods/Task/PreDefinedEvaluate/DateTime.cs (8 missing)

**Methods:**
- Line 6: `public static async Task<FilterBuilder<DateTime>> InFuture(this Task<FilterBuild...`
- Line 9: `public static async Task<FilterBuilder<DateOnly>> InFuture(this Task<FilterBuild...`
- Line 12: `public static async Task<FilterBuilder<TimeOnly>> InFuture(this Task<FilterBuild...`
- Line 15: `public static async Task<FilterBuilder<DateTime>> InPast(this Task<FilterBuilder...`
- Line 18: `public static async Task<FilterBuilder<DateOnly>> InPast(this Task<FilterBuilder...`
- *...and 3 more*


*...and 82 more files with missing documentation*

## Recommendations

1. **Priority Areas:**
   - Focus on public APIs first (methods, properties, classes)
   - Extension methods should have clear documentation as they are user-facing
   - Factory methods and builders need documentation for proper usage

2. **Documentation Guidelines:**
   - Use `<summary>` tags for brief descriptions
   - Add `<param>` tags for all parameters
   - Include `<returns>` tags for methods with return values
   - Add `<example>` tags for complex usage scenarios
   - Use `<remarks>` for additional important information

3. **Consider using:**
   - Documentation generation tools
   - IDE features for generating XML documentation templates
   - Code analysis rules to enforce documentation requirements
