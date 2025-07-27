# Installation Guide

This guide will help you install and configure FluentUnions in your .NET project.

## Prerequisites

- .NET 6.0 or higher
- A C# project (Console, Web API, Class Library, etc.)
- NuGet package manager (built into most .NET IDEs)

## Installation Methods

### 1. Using .NET CLI (Recommended)

Open a terminal in your project directory and run:

```bash
# Install the core library
dotnet add package FluentUnions

# Optional: Install testing extensions in testing projects
dotnet add package FluentUnions.AwesomeAssertions
```

### 2. Using Package Manager Console

In Visual Studio, open the Package Manager Console (`Tools > NuGet Package Manager > Package Manager Console`) and run:

```powershell
# Install the core library
Install-Package FluentUnions

# Optional: Install testing extensions in testing projects
Install-Package FluentUnions.AwesomeAssertions
```

### 3. Using PackageReference in .csproj

Add the following to your `.csproj` file:

```xml
<ItemGroup>
  <PackageReference Include="FluentUnions" Version="*" />
  
  <!-- Optional: For testing -->
  <PackageReference Include="FluentUnions.AwesomeAssertions" Version="*" />
</ItemGroup>
```

Then restore packages:
```bash
dotnet restore
```

### 4. Using Visual Studio Package Manager UI

1. Right-click on your project in Solution Explorer
2. Select "Manage NuGet Packages"
3. Click "Browse" tab
4. Search for "FluentUnions"
5. Click "Install"

## Verify Installation

Create a simple test file to verify the installation:

```csharp
using FluentUnions;

// Test Result type
var successResult = Result.Success();
var failureResult = Result.Failure("Something went wrong");

// Test Option type
var someOption = Option.Some(42);
var noneOption = Option<int>.None;

Console.WriteLine($"Success: {successResult.IsSuccess}");
Console.WriteLine($"Some: {someOption.IsSome}");
```

## Configuration

### Enable Analyzers

FluentUnions includes static analyzers to help catch common mistakes. They're enabled by default, but you can configure them in your `.editorconfig`:

```ini
# .editorconfig
root = true

[*.cs]
# FluentUnions analyzer severities
dotnet_diagnostic.FU001.severity = warning  # Accessing Value without checking
dotnet_diagnostic.FU002.severity = warning  # Incomplete Match expressions
dotnet_diagnostic.FU003.severity = warning  # Ignoring Result return values
dotnet_diagnostic.FU004.severity = warning  # Comparing Option with null
```

### Global Usings (Optional)

For convenience, you can add global usings in your project:

```csharp
// GlobalUsings.cs or in .csproj
global using FluentUnions;
global using FluentUnions.Extensions;
```

Or in your `.csproj`:
```xml
<ItemGroup>
  <Using Include="FluentUnions" />
  <Using Include="FluentUnions.Extensions" />
</ItemGroup>
```

## Package Contents

### FluentUnions (Core Package)

- **Result Types**: `Result`, `Result<T>`
- **Option Types**: `Option<T>`
- **Error Types**: `Error`, `ValidationError`, `NotFoundError`, etc.
- **Extension Methods**: Functional operations (Map, Bind, Match, etc.)
- **Source Generators**: Performance optimizations and additional methods
- **Static Analyzers**: Compile-time safety checks

### FluentUnions.AwesomeAssertions

- **Result Assertions**: `BeSuccess()`, `BeFailure()`, etc.
- **Option Assertions**: `BeSome()`, `BeNone()`, etc.
- **Custom Matchers**: For complex assertion scenarios

## Troubleshooting

### Common Issues

1. **"Type or namespace 'FluentUnions' not found"**
   - Ensure you've restored NuGet packages
   - Check that the package is listed in your .csproj
   - Try cleaning and rebuilding the solution

2. **Analyzer warnings not showing**
   - Ensure analyzers are enabled in your project
   - Check your .editorconfig settings
   - Restart your IDE

3. **IntelliSense not working**
   - Rebuild the project
   - Restart your IDE
   - Clear NuGet cache: `dotnet nuget locals all --clear`

### Version Compatibility

| FluentUnions Version | .NET Version | C# Version |
|---------------------|--------------|------------|
| 1.0+               | .NET 6.0+    | C# 10+     |
| 0.5                | .NET 5.0+    | C# 9+      |

## Next Steps

Now that you have FluentUnions installed, check out:
- [Quick Start Guide](quick-start.md) for your first examples
- [Core Concepts](core-concepts.md) to understand the fundamentals
- [Result Pattern Tutorial](../tutorials/result-pattern-basics.md) for in-depth learning

## Additional Resources

- [NuGet Package Page](https://www.nuget.org/packages/FluentUnions/)
- [GitHub Repository](https://github.com/ymjaber/fluent-unions)
- [Release Notes](https://github.com/ymjaber/fluent-unions/releases)