# Static Analyzers Reference

This guide covers the Roslyn analyzers included with FluentUnions that help catch common mistakes at compile time.

## Table of Contents
1. [Introduction](#introduction)
2. [Analyzer Rules](#analyzer-rules)
3. [Configuration](#configuration)
4. [Rule Details](#rule-details)
5. [Suppressing Warnings](#suppressing-warnings)
6. [Creating Custom Rules](#creating-custom-rules)
7. [Best Practices](#best-practices)

## Introduction

FluentUnions includes static analyzers that:
- Prevent common runtime errors
- Enforce best practices
- Guide developers to use Result and Option types correctly
- Provide helpful diagnostics with code fixes

## Analyzer Rules

### Rule Summary

| Rule ID | Category | Severity | Description |
|---------|----------|----------|-------------|
| FU001 | Usage | Warning | Accessing Value without checking IsSuccess/IsSome |
| FU002 | Usage | Warning | Match expression not exhaustive |
| FU003 | Usage | Warning | Result not used |
| FU004 | Usage | Warning | Comparing Option with null |
| FU005 | Usage | Warning | Using == or != on Result/Option types |
| FU006 | Design | Warning | Method returns Result but throws exception |
| FU007 | Design | Info | Consider using Result instead of throwing |
| FU008 | Performance | Info | Avoid creating Result in hot path |
| FU009 | Usage | Error | Cannot access Error when IsSuccess is true |
| FU010 | Usage | Warning | Async method should return Task<Result<T>> |

## Configuration

### EditorConfig

Configure analyzer severities in `.editorconfig`:

```ini
# .editorconfig
root = true

[*.cs]
# FluentUnions analyzer configuration
dotnet_diagnostic.FU001.severity = warning
dotnet_diagnostic.FU002.severity = warning
dotnet_diagnostic.FU003.severity = warning
dotnet_diagnostic.FU004.severity = warning
dotnet_diagnostic.FU005.severity = warning
dotnet_diagnostic.FU006.severity = warning
dotnet_diagnostic.FU007.severity = suggestion
dotnet_diagnostic.FU008.severity = suggestion
dotnet_diagnostic.FU009.severity = error
dotnet_diagnostic.FU010.severity = warning
```

### Global AnalyzerConfig

Create a `.globalconfig` file:

```ini
is_global = true

# Treat all FluentUnions warnings as errors in Release builds
dotnet_analyzer_diagnostic.category-FluentUnions.severity = error

# Configure specific rules
dotnet_diagnostic.FU001.severity = error
dotnet_diagnostic.FU007.severity = none  # Disable
```

### Project-Level Configuration

In your `.csproj`:

```xml
<PropertyGroup>
  <!-- Treat warnings as errors for FluentUnions analyzers -->
  <WarningsAsErrors>FU001;FU002;FU003;FU009</WarningsAsErrors>
  
  <!-- Disable specific warnings project-wide -->
  <NoWarn>FU007;FU008</NoWarn>
</PropertyGroup>

<!-- Configure analyzer properties -->
<ItemGroup>
  <CompilerVisibleProperty Include="FluentUnions_EnforceExhaustiveMatch" />
</ItemGroup>
<PropertyGroup>
  <FluentUnions_EnforceExhaustiveMatch>true</FluentUnions_EnforceExhaustiveMatch>
</PropertyGroup>
```

## Rule Details

### FU001: Accessing Value without checking

**Problem**: Accessing `Value` property without checking `IsSuccess` or `IsSome` can throw.

```csharp
// ❌ Warning: Possible InvalidOperationException
public void ProcessUser(Result<User> result)
{
    var user = result.Value; // FU001
    Console.WriteLine(user.Name);
}

// ✅ Correct: Check before accessing
public void ProcessUser(Result<User> result)
{
    if (result.IsSuccess)
    {
        var user = result.Value;
        Console.WriteLine(user.Name);
    }
}

// ✅ Also correct: Use Match
public void ProcessUser(Result<User> result)
{
    result.Match(
        onSuccess: user => Console.WriteLine(user.Name),
        onFailure: error => Console.WriteLine(error.Message)
    );
}
```

**Code Fix**: The analyzer provides automatic fixes:
1. Wrap in if statement
2. Convert to Match expression
3. Use GetValueOr

### FU002: Match expression not exhaustive

**Problem**: Match expressions should handle all cases.

```csharp
// ❌ Warning: Match not exhaustive
public string GetMessage(Result<User> result)
{
    return result.Match(
        onSuccess: user => $"Hello {user.Name}"
        // Missing onFailure!
    );
}

// ✅ Correct: Handle all cases
public string GetMessage(Result<User> result)
{
    return result.Match(
        onSuccess: user => $"Hello {user.Name}",
        onFailure: error => $"Error: {error.Message}"
    );
}
```

### FU003: Result not used

**Problem**: Result return values should not be ignored.

```csharp
// ❌ Warning: Result not used
public void ProcessData(string data)
{
    ValidateData(data); // FU003 - Result ignored
    SaveData(data);     // FU003 - Result ignored
}

// ✅ Correct: Handle the Result
public Result ProcessData(string data)
{
    var validationResult = ValidateData(data);
    if (validationResult.IsFailure)
        return validationResult;
        
    return SaveData(data);
}

// ✅ Also correct: Explicitly discard if intended
public void ProcessData(string data)
{
    _ = ValidateData(data); // Explicitly discarded
    _ = SaveData(data);
}
```

### FU004: Comparing Option with null

**Problem**: Option types should not be compared with null.

```csharp
// ❌ Warning: Use IsNone instead
public void CheckOption(Option<User> option)
{
    if (option == null) // FU004
    {
        // Handle null
    }
}

// ✅ Correct: Use IsNone
public void CheckOption(Option<User> option)
{
    if (option.IsNone)
    {
        // Handle none
    }
}
```

### FU005: Using == or != on Result/Option

**Problem**: Result and Option types should not be compared with == or !=.

```csharp
// ❌ Warning: Don't compare Results directly
public bool AreEqual(Result<int> r1, Result<int> r2)
{
    return r1 == r2; // FU005
}

// ✅ Correct: Compare values or use custom logic
public bool AreEqual(Result<int> r1, Result<int> r2)
{
    return r1.IsSuccess && r2.IsSuccess && r1.Value == r2.Value;
}
```

### FU006: Method returns Result but throws

**Problem**: Methods returning Result should not throw exceptions for expected failures.

```csharp
// ❌ Warning: Don't throw in Result-returning methods
public Result<User> GetUser(Guid id)
{
    if (id == Guid.Empty)
        throw new ArgumentException("Invalid ID"); // FU006
        
    var user = _repository.Find(id);
    if (user == null)
        throw new NotFoundException(); // FU006
        
    return Result.Success(user);
}

// ✅ Correct: Return Result for all paths
public Result<User> GetUser(Guid id)
{
    if (id == Guid.Empty)
        return new ValidationError("Invalid ID");
        
    var user = _repository.Find(id);
    if (user == null)
        return new NotFoundError($"User {id} not found");
        
    return Result.Success(user);
}
```

### FU007: Consider using Result

**Problem**: Methods that throw exceptions might benefit from Result.

```csharp
// ℹ️ Info: Consider using Result
public User GetUser(Guid id)
{
    var user = _repository.Find(id);
    if (user == null)
        throw new NotFoundException($"User {id} not found"); // FU007
        
    return user;
}

// Suggested: Use Result instead
public Result<User> GetUser(Guid id)
{
    var user = _repository.Find(id);
    return user != null
        ? Result.Success(user)
        : new NotFoundError($"User {id} not found");
}
```

### FU008: Avoid creating Result in hot path

**Problem**: Creating Results in performance-critical code may impact performance.

```csharp
// ℹ️ Info: Result creation in hot path
public Result<int> Calculate(int[] values)
{
    int sum = 0;
    foreach (var value in values)
    {
        // FU008: Creating Result in tight loop
        var validated = ValidateValue(value);
        if (validated.IsFailure)
            return validated;
        sum += validated.Value;
    }
    return Result.Success(sum);
}

// Better: Validate once, then process
public Result<int> Calculate(int[] values)
{
    // Validate all values first
    foreach (var value in values)
    {
        if (!IsValid(value))
            return new ValidationError($"Invalid value: {value}");
    }
    
    // Then process without Result overhead
    return Result.Success(values.Sum());
}
```

### FU009: Cannot access Error when IsSuccess

**Problem**: Accessing Error property when IsSuccess is true is a logic error.

```csharp
// ❌ Error: Logic error
public void ProcessResult(Result<User> result)
{
    if (result.IsSuccess)
    {
        var error = result.Error; // FU009 - Error!
        LogError(error);
    }
}

// ✅ Correct: Access Error only when IsFailure
public void ProcessResult(Result<User> result)
{
    if (result.IsFailure)
    {
        var error = result.Error;
        LogError(error);
    }
}
```

### FU010: Async method should return Task<Result<T>>

**Problem**: Async methods should return Task<Result<T>> not Result<Task<T>>.

```csharp
// ❌ Warning: Wrong async Result pattern
public Result<Task<User>> GetUserAsync(Guid id) // FU010
{
    if (id == Guid.Empty)
        return new ValidationError("Invalid ID");
        
    return Result.Success(LoadUserAsync(id));
}

// ✅ Correct: Task wraps Result
public async Task<Result<User>> GetUserAsync(Guid id)
{
    if (id == Guid.Empty)
        return new ValidationError("Invalid ID");
        
    return await LoadUserAsync(id);
}
```

## Suppressing Warnings

### Inline Suppression

```csharp
public void ProcessResult(Result<User> result)
{
#pragma warning disable FU001 // Accessing Value without checking
    var user = result.Value; // I know what I'm doing
#pragma warning restore FU001
    
    // Or use SuppressMessage attribute
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "FluentUnions", 
        "FU001:Accessing Value without checking", 
        Justification = "Value is guaranteed to exist here")]
    void InternalProcess()
    {
        var value = result.Value;
    }
}
```

### File-Level Suppression

```csharp
// Suppress for entire file
#pragma warning disable FU001, FU003

namespace MyApp
{
    // All code in file has FU001 and FU003 suppressed
}
```

### Global Suppression

In a GlobalSuppressions.cs file:

```csharp
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "FluentUnions", 
    "FU007:Consider using Result", 
    Justification = "Legacy code, will refactor later",
    Scope = "namespaceanddescendants", 
    Target = "~N:MyApp.Legacy")]
```

## Creating Custom Rules

### Custom Analyzer Example

```csharp
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ResultNamingAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "FU_CUSTOM_001";
    
    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "Result variable should have descriptive name",
        "Result variable '{0}' should have a more descriptive name",
        "Naming",
        DiagnosticSeverity.Info,
        isEnabledByDefault: true);
        
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => 
        ImmutableArray.Create(Rule);
        
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(
            GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        
        context.RegisterSyntaxNodeAction(
            AnalyzeVariable, 
            SyntaxKind.VariableDeclaration);
    }
    
    private void AnalyzeVariable(SyntaxNodeAnalysisContext context)
    {
        var declaration = (VariableDeclarationSyntax)context.Node;
        var type = context.SemanticModel.GetTypeInfo(declaration.Type);
        
        if (IsResultType(type.Type))
        {
            foreach (var variable in declaration.Variables)
            {
                var name = variable.Identifier.Text;
                if (name == "result" || name == "r")
                {
                    context.ReportDiagnostic(
                        Diagnostic.Create(Rule, variable.Identifier.GetLocation(), name));
                }
            }
        }
    }
}
```

### Code Fix Provider

```csharp
[ExportCodeFixProvider(LanguageNames.CSharp)]
public class ResultNamingCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(ResultNamingAnalyzer.DiagnosticId);
        
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document
            .GetSyntaxRootAsync(context.CancellationToken)
            .ConfigureAwait(false);
            
        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        
        var declaration = root.FindToken(diagnosticSpan.Start)
            .Parent.AncestorsAndSelf()
            .OfType<VariableDeclaratorSyntax>()
            .First();
            
        context.RegisterCodeFix(
            CodeAction.Create(
                title: "Use descriptive name",
                createChangedDocument: c => RenameVariable(
                    context.Document, declaration, c),
                equivalenceKey: "UseDescriptiveName"),
            diagnostic);
    }
}
```

## Best Practices

### 1. Configure Analyzers Early

Set up analyzer configuration at the start of your project:

```ini
# .editorconfig at solution root
[*.cs]
# Strict for new code
dotnet_diagnostic.FU001.severity = error
dotnet_diagnostic.FU002.severity = error
dotnet_diagnostic.FU003.severity = error
dotnet_diagnostic.FU009.severity = error

# Relaxed for legacy code
[**/Legacy/**.cs]
dotnet_diagnostic.FU001.severity = warning
dotnet_diagnostic.FU007.severity = none
```

### 2. Use Code Fixes

Take advantage of automatic code fixes:

```csharp
// Place cursor on warning and use:
// - Ctrl+. (Visual Studio)
// - Alt+Enter (Rider)
// - Cmd+. (VS Code)
```

### 3. Gradual Adoption

For existing codebases:

```xml
<PropertyGroup>
  <!-- Start with suggestions -->
  <FluentUnionsAnalyzerSeverity>suggestion</FluentUnionsAnalyzerSeverity>
  
  <!-- Gradually increase severity -->
  <!-- <FluentUnionsAnalyzerSeverity>warning</FluentUnionsAnalyzerSeverity> -->
  <!-- <FluentUnionsAnalyzerSeverity>error</FluentUnionsAnalyzerSeverity> -->
</PropertyGroup>
```

### 4. Team Standards

Document your team's analyzer configuration:

```markdown
## FluentUnions Analyzer Standards

### Must Fix (Errors)
- FU001: Always check IsSuccess before Value
- FU009: Logic errors must be fixed

### Should Fix (Warnings)  
- FU002: Match should be exhaustive
- FU003: Handle all Results

### Consider (Info)
- FU007: Migrate to Result pattern when refactoring
- FU008: Profile before optimizing
```

### 5. CI/CD Integration

Enforce standards in CI:

```yaml
# .github/workflows/build.yml
- name: Build with analyzers
  run: dotnet build -p:TreatWarningsAsErrors=true -p:WarningsAsErrors=FU001;FU002;FU003;FU009
```

## Summary

FluentUnions analyzers help:

1. **Prevent runtime errors** - Catch common mistakes at compile time
2. **Enforce best practices** - Guide proper Result/Option usage
3. **Improve code quality** - Consistent error handling patterns
4. **Ease adoption** - Helpful diagnostics with automatic fixes
5. **Maintain standards** - Team-wide consistency

Configure analyzers to match your team's needs and gradually increase strictness as your codebase adopts FluentUnions patterns.

Next steps:
- [API Reference](result-api.md)
- [Performance Guide](../guides/performance-best-practices.md)
- [Migration Guide](../migration/from-exceptions.md)