using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using FluentUnions.Analyzers.Common;

namespace FluentUnions.Analyzers.Result;

/// <summary>
/// Analyzer that detects Result values that are created but never consumed or checked.
/// </summary>
/// <remarks>
/// This analyzer helps prevent bugs where operations that return Result are called
/// but their success/failure status is never examined, potentially ignoring errors.
/// The analyzer detects when a Result is created (through method calls or constructors)
/// but is not:
/// - Assigned to a variable
/// - Returned from a method
/// - Passed as an argument
/// - Checked with IsSuccess/IsFailure properties
/// - Handled with methods like Match, Map, Bind, or ThrowIfFailure
/// 
/// Example of code that triggers this diagnostic:
/// <code>
/// // Result is created but never handled
/// repository.SaveData(data); // Warning if SaveData returns Result
/// 
/// // Result from TryParse is ignored
/// Result.TryParse&lt;int&gt;("123"); // Warning
/// </code>
/// 
/// Example of properly handled Result:
/// <code>
/// var result = repository.SaveData(data);
/// if (result.IsFailure)
/// {
///     Console.WriteLine($"Save failed: {result.Error}");
/// }
/// 
/// // Or using method chaining
/// repository.SaveData(data)
///     .Match(
///         success: () => Console.WriteLine("Saved successfully"),
///         failure: error => Console.WriteLine($"Save failed: {error}")
///     );
/// </code>
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class UnhandledResultAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic descriptor for unhandled Result values.
    /// </summary>
    /// <remarks>
    /// This rule produces warning FU0104 when a Result is created but not handled.
    /// This helps catch potential bugs where errors are silently ignored.
    /// </remarks>
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticIds.UnhandledResult,
        title: "Result should be handled",
        messageFormat: "{0} is not being handled. {1}.",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Result values should be explicitly handled to ensure errors are not ignored.");

    /// <summary>
    /// Gets the collection of diagnostics supported by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    /// <summary>
    /// Initializes the analyzer by registering actions to be performed during compilation.
    /// </summary>
    /// <param name="context">The analysis context used to register analyzer actions.</param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // Check method invocations that return Result
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);

        // Check object creations that return Result
        context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
    }

    /// <summary>
    /// Analyzes method invocations to detect unhandled Result return values.
    /// </summary>
    /// <param name="context">The syntax node analysis context containing the invocation to analyze.</param>
    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        var methodSymbol = context.SemanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
        if (methodSymbol == null)
            return;

        var methodName = methodSymbol.Name;

        // Check if this method returns a Result type
        var returnType = methodSymbol.ReturnType;
        if (SymbolHelpers.IsResultType(returnType))
        {
            // Regular method returning Result
            if (IsResultHandled(invocation, context.SemanticModel, methodName))
                return;

            var resultType = GetResultTypeString(returnType);
            var suggestion = GetSuggestionMessage(returnType, methodName);
            var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation(), resultType, suggestion);
            context.ReportDiagnostic(diagnostic);
            return;
        }

        // Check if this is an extension method on Result type (like Match)
        if (methodSymbol.IsExtensionMethod && invocation.Expression is MemberAccessExpressionSyntax memberAccess)
        {
            var receiverType = context.SemanticModel.GetTypeInfo(memberAccess.Expression).Type;
            if (SymbolHelpers.IsResultType(receiverType) && IsPureFunction(methodName))
            {
                // This is a pure function on Result that returns non-Result
                if (!IsResultHandled(invocation, context.SemanticModel, methodName))
                {
                    var resultType = GetResultTypeString(receiverType);
                    var suggestion = "This pure function returns a value that should be used. Consider assigning to a variable or returning the result";
                    var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation(), resultType, suggestion);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }

    /// <summary>
    /// Analyzes object creation expressions to detect unhandled Result instances.
    /// </summary>
    /// <param name="context">The syntax node analysis context containing the object creation to analyze.</param>
    private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
    {
        var objectCreation = (ObjectCreationExpressionSyntax)context.Node;
        var typeInfo = context.SemanticModel.GetTypeInfo(objectCreation);

        if (!SymbolHelpers.IsResultType(typeInfo.Type))
            return;

        if (IsResultHandled(objectCreation, context.SemanticModel, ""))
            return;

        var resultType = GetResultTypeString(typeInfo.Type);
        var suggestion = GetSuggestionMessage(typeInfo.Type, "");
        var diagnostic = Diagnostic.Create(Rule, objectCreation.GetLocation(), resultType, suggestion);
        context.ReportDiagnostic(diagnostic);
    }

    /// <summary>
    /// Determines whether a Result expression is being properly handled.
    /// </summary>
    /// <param name="resultExpression">The Result expression to check.</param>
    /// <param name="semanticModel">The semantic model for analysis.</param>
    /// <param name="methodName">The name of the method that created the Result.</param>
    /// <returns>True if the Result is being handled; otherwise, false.</returns>
    /// <remarks>
    /// A Result is considered handled if it is:
    /// - Assigned to a variable or field
    /// - Returned from a method
    /// - Passed as an argument to another method
    /// - Used with property access (IsSuccess, IsFailure, Value, Error)
    /// - Used with handling methods (Match, Map, Bind, ThrowIfFailure, etc.)
    /// - Used in pattern matching or conditional expressions
    /// </remarks>
    private static bool IsResultHandled(ExpressionSyntax resultExpression, SemanticModel semanticModel, string methodName)
    {
        var parent = resultExpression.Parent;

        // Result is being assigned to a variable or returned
        if (parent is AssignmentExpressionSyntax or
            EqualsValueClauseSyntax or
            ReturnStatementSyntax)
            return true;

        // For expression-bodied members, check if the containing method returns void
        if (parent is ArrowExpressionClauseSyntax arrowClause)
        {
            // Find the containing member declaration
            var memberDeclaration = arrowClause.Parent;

            // Check if it's a method that returns void
            if (memberDeclaration is MethodDeclarationSyntax methodDecl)
            {
                var methodSymbol = semanticModel.GetDeclaredSymbol(methodDecl);
                if (methodSymbol?.ReturnsVoid == true)
                {
                    // This is a void method with expression body - the result is not handled
                    return false;
                }
            }
            else if (memberDeclaration is LocalFunctionStatementSyntax localFunc)
            {
                var localFuncSymbol = semanticModel.GetDeclaredSymbol(localFunc);
                if (localFuncSymbol?.ReturnsVoid == true)
                {
                    // This is a void local function with expression body - the result is not handled
                    return false;
                }
            }
            else if (memberDeclaration is AccessorDeclarationSyntax accessor)
            {
                // Property setters and event accessors are void
                if (accessor.Kind() == SyntaxKind.SetAccessorDeclaration ||
                    accessor.Kind() == SyntaxKind.AddAccessorDeclaration ||
                    accessor.Kind() == SyntaxKind.RemoveAccessorDeclaration)
                {
                    return false;
                }
            }

            // For non-void methods or other members, the result is being returned
            return true;
        }

        // Result is being passed as an argument
        if (parent is ArgumentSyntax)
            return true;

        // Result is being used in a conditional
        if (parent is ConditionalExpressionSyntax or
            BinaryExpressionSyntax or
            PrefixUnaryExpressionSyntax)
            return true;

        // Result is being handled with method chaining
        if (parent is MemberAccessExpressionSyntax memberAccess)
        {
            var memberName = memberAccess.Name.Identifier.Text;

            // Check for handling methods - but don't consider pure functions as "handled"
            // unless their result is also handled
            if (IsHandlingMethod(memberName))
            {
                // For pure functions, we need to check if their result is handled
                if (IsPureFunction(memberName))
                {
                    var parentExpr = memberAccess.Parent as ExpressionSyntax;
                    return parentExpr != null && IsResultHandled(parentExpr, semanticModel, memberName);
                }
                return true;
            }

            // Check for property access that implies handling
            if (memberName == "IsSuccess" || memberName == "IsFailure" ||
                memberName == "Value" || memberName == "Error")
                return true;
        }

        // Result is used in pattern matching
        if (parent is IsPatternExpressionSyntax or SwitchExpressionSyntax)
            return true;

        // Check if the result is part of a larger expression that's being handled
        if (parent is ExpressionSyntax parentExpression)
            return IsResultHandled(parentExpression, semanticModel, methodName);

        return false;
    }

    /// <summary>
    /// Determines if a method is a pure function (returns a value without side effects).
    /// </summary>
    /// <param name="methodName">The name of the method to check.</param>
    /// <returns>True if the method is a pure function; otherwise, false.</returns>
    private static bool IsPureFunction(string methodName)
    {
        return methodName switch
        {
            "Match" => true,
            "Map" => true,
            "Bind" => true,
            "BindAll" => true,
            "BindAppend" => true,
            "BindAllAppend" => true,
            "DiscardValue" => true,
            "WithValue" => true,
            "Ensure" => true,
            "EnsureAll" => true,
            _ => false
        };
    }

    /// <summary>
    /// Determines if a method name represents a Result handling method.
    /// </summary>
    /// <param name="methodName">The name of the method to check.</param>
    /// <returns>True if the method handles Result; otherwise, false.</returns>
    /// <remarks>
    /// Handling methods are those that consume or transform the Result in a meaningful way,
    /// ensuring that success/failure states are properly addressed.
    /// </remarks>
    private static bool IsHandlingMethod(string methodName)
    {
        return methodName switch
        {
            "Match" => true,
            "Map" => true,
            "Bind" => true,
            "BindAll" => true,
            "BindAppend" => true,
            "BindAllAppend" => true,
            "DiscardValue" => true,
            "WithValue" => true,
            "ThrowIfFailure" => true,
            "GetValueOrThrow" => true,
            "Ensure" => true,
            "EnsureAll" => true,
            "OnSuccess" => true,
            "OnFailure" => true,
            "OnEither" => true,
            "TryGetValue" => true,
            "TryGetError" => true,
            _ => false
        };
    }

    /// <summary>
    /// Gets an appropriate suggestion message based on the Result type and method.
    /// </summary>
    /// <param name="type">The Result type.</param>
    /// <param name="methodName">The name of the method that created the Result.</param>
    /// <returns>A suggestion message for handling the Result.</returns>
    private static string GetSuggestionMessage(ITypeSymbol? type, string methodName)
    {
        // Check if we're dealing with a pure function
        if (IsPureFunction(methodName))
        {
            return "This pure function returns a value that should be used. Consider assigning to a variable or returning the result";
        }

        if (type is INamedTypeSymbol namedType)
        {
            if (SymbolHelpers.IsValueResultType(namedType))
            {
                // Result<T> - has GetValueOrThrow
                return "Consider using Match, checking IsSuccess/IsFailure, calling GetValueOrThrow, or using OnSuccess/OnFailure/OnEither for side effects";
            }
            else if (SymbolHelpers.IsUnitResultType(namedType))
            {
                // Unit Result - has ThrowIfFailure
                return "Consider using Match, checking IsSuccess/IsFailure, calling ThrowIfFailure, or using OnSuccess/OnFailure/OnEither for side effects";
            }
        }

        return "Consider using Match, checking IsSuccess/IsFailure, or handling the result appropriately";
    }

    /// <summary>
    /// Gets a display string for a Result type symbol.
    /// </summary>
    /// <param name="type">The type symbol to convert to string.</param>
    /// <returns>A string representation of the Result type (e.g., "Result&lt;int&gt;" or "Result").</returns>
    private static string GetResultTypeString(ITypeSymbol? type)
    {
        if (type is INamedTypeSymbol namedType)
        {
            if (SymbolHelpers.IsValueResultType(namedType))
            {
                var typeArg = namedType.TypeArguments.FirstOrDefault();
                return $"Result<{typeArg?.ToDisplayString() ?? "T"}>";
            }
            else if (SymbolHelpers.IsUnitResultType(namedType))
            {
                return "Result";
            }
        }
        return "Result";
    }
}
