using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using FluentUnions.Analyzers.Common;

namespace FluentUnions.Analyzers.Option;

/// <summary>
/// Analyzer that detects Option values that are created but never consumed or checked.
/// </summary>
/// <remarks>
/// This analyzer helps prevent bugs where operations that return Option are called
/// but their value is never examined, potentially ignoring important data.
/// The analyzer detects when an Option is created (through method calls or constructors)
/// but is not:
/// - Assigned to a variable
/// - Returned from a method
/// - Passed as an argument
/// - Checked with IsSome/IsNone properties
/// - Handled with methods like Match, Map, Bind, or GetValueOrThrow
/// 
/// Example of code that triggers this diagnostic:
/// <code>
/// // Option is created but never handled
/// GetUserById(userId); // Warning if GetUserById returns Option&lt;User&gt;
/// 
/// // Option from TryParse is ignored
/// Option.TryParse&lt;int&gt;("123"); // Warning
/// </code>
/// 
/// Example of properly handled Option:
/// <code>
/// var userOption = GetUserById(userId);
/// if (userOption.IsSome)
/// {
///     Console.WriteLine($"Found user: {userOption.Value.Name}");
/// }
/// 
/// // Or using method chaining
/// GetUserById(userId)
///     .Match(
///         some: user => Console.WriteLine($"Found user: {user.Name}"),
///         none: () => Console.WriteLine("User not found")
///     );
/// </code>
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class UnhandledOptionAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// The diagnostic descriptor for unhandled Option values.
    /// </summary>
    /// <remarks>
    /// This rule produces warning FU0006 when an Option is created but not handled.
    /// This helps catch potential bugs where optional values are silently ignored.
    /// </remarks>
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticIds.UnhandledOption,
        title: "Option should be handled",
        messageFormat: "{0} is not being handled. {1}.",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Option values should be explicitly handled to ensure optional data is not ignored.");

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

        // Check method invocations that return Option
        context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);

        // Check object creations that return Option
        context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, SyntaxKind.ObjectCreationExpression);
    }

    /// <summary>
    /// Analyzes method invocations to detect unhandled Option return values.
    /// </summary>
    /// <param name="context">The syntax node analysis context containing the invocation to analyze.</param>
    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        var methodSymbol = context.SemanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
        if (methodSymbol == null)
            return;

        var methodName = methodSymbol.Name;

        // Check if this method returns an Option type
        var returnType = methodSymbol.ReturnType;
        if (SymbolHelpers.IsOptionType(returnType))
        {
            // Regular method returning Option
            if (IsOptionHandled(invocation, context.SemanticModel, methodName))
                return;

            var optionType = GetOptionTypeString(returnType);
            var suggestion = GetSuggestionMessage(methodName);
            var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation(), optionType, suggestion);
            context.ReportDiagnostic(diagnostic);
            return;
        }

        // Check if this is an extension method on Option type (like Match)
        if (methodSymbol.IsExtensionMethod && invocation.Expression is MemberAccessExpressionSyntax memberAccess)
        {
            var receiverType = context.SemanticModel.GetTypeInfo(memberAccess.Expression).Type;
            if (SymbolHelpers.IsOptionType(receiverType) && IsPureFunction(methodName))
            {
                // This is a pure function on Option that returns non-Option
                if (!IsOptionHandled(invocation, context.SemanticModel, methodName))
                {
                    var optionType = GetOptionTypeString(receiverType);
                    var suggestion = "This pure function returns a value that should be used. Consider assigning to a variable or returning the result";
                    var diagnostic = Diagnostic.Create(Rule, invocation.GetLocation(), optionType, suggestion);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }

    /// <summary>
    /// Analyzes object creation expressions to detect unhandled Option instances.
    /// </summary>
    /// <param name="context">The syntax node analysis context containing the object creation to analyze.</param>
    private static void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
    {
        var objectCreation = (ObjectCreationExpressionSyntax)context.Node;
        var typeInfo = context.SemanticModel.GetTypeInfo(objectCreation);

        if (!SymbolHelpers.IsOptionType(typeInfo.Type))
            return;

        if (IsOptionHandled(objectCreation, context.SemanticModel, ""))
            return;

        var optionType = GetOptionTypeString(typeInfo.Type);
        var suggestion = GetSuggestionMessage("");
        var diagnostic = Diagnostic.Create(Rule, objectCreation.GetLocation(), optionType, suggestion);
        context.ReportDiagnostic(diagnostic);
    }

    /// <summary>
    /// Determines whether an Option expression is being properly handled.
    /// </summary>
    /// <param name="optionExpression">The Option expression to check.</param>
    /// <param name="semanticModel">The semantic model for analysis.</param>
    /// <param name="methodName">The name of the method that created the Option.</param>
    /// <returns>True if the Option is being handled; otherwise, false.</returns>
    /// <remarks>
    /// An Option is considered handled if it is:
    /// - Assigned to a variable or field
    /// - Returned from a method
    /// - Passed as an argument to another method
    /// - Used with property access (IsSome, IsNone, Value)
    /// - Used with handling methods (Match, Map, Bind, GetValueOrThrow, etc.)
    /// - Used in pattern matching or conditional expressions
    /// </remarks>
    private static bool IsOptionHandled(ExpressionSyntax optionExpression, SemanticModel semanticModel, string methodName)
    {
        var parent = optionExpression.Parent;

        // Option is being assigned to a variable or returned
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

        // Option is being passed as an argument
        if (parent is ArgumentSyntax)
            return true;

        // Option is being used in a conditional
        if (parent is ConditionalExpressionSyntax or
            BinaryExpressionSyntax or
            PrefixUnaryExpressionSyntax)
            return true;

        // Option is being handled with method chaining
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
                    return parentExpr != null && IsOptionHandled(parentExpr, semanticModel, memberName);
                }
                return true;
            }

            // Check for property access that implies handling
            if (memberName == "IsSome" || memberName == "IsNone" || memberName == "Value")
                return true;
        }

        // Option is used in pattern matching
        if (parent is IsPatternExpressionSyntax or SwitchExpressionSyntax)
            return true;

        // Check if the option is part of a larger expression that's being handled
        if (parent is ExpressionSyntax parentExpression)
            return IsOptionHandled(parentExpression, semanticModel, methodName);

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
            "Filter" => true,
            "Or" => true,
            "OrElse" => true,
            "EnsureSome" => true,
            "EnsureNone" => true,
            _ => false
        };
    }

    /// <summary>
    /// Determines if a method name represents an Option handling method.
    /// </summary>
    /// <param name="methodName">The name of the method to check.</param>
    /// <returns>True if the method handles Option; otherwise, false.</returns>
    /// <remarks>
    /// Handling methods are those that consume or transform the Option in a meaningful way,
    /// ensuring that the presence/absence of values is properly addressed.
    /// </remarks>
    private static bool IsHandlingMethod(string methodName)
    {
        return methodName switch
        {
            "Match" => true,
            "Map" => true,
            "Bind" => true,
            "Filter" => true,
            "Or" => true,
            "OrElse" => true,
            "GetValueOrDefault" => true,
            "GetValueOrThrow" => true,
            "OnSome" => true,
            "OnNone" => true,
            "OnEither" => true,
            "EnsureSome" => true,
            "EnsureNone" => true,
            "TryGetValue" => true,
            _ => false
        };
    }

    /// <summary>
    /// Gets an appropriate suggestion message based on the method.
    /// </summary>
    /// <param name="methodName">The name of the method that created the Option.</param>
    /// <returns>A suggestion message for handling the Option.</returns>
    private static string GetSuggestionMessage(string methodName)
    {
        // Check if we're dealing with a pure function
        if (IsPureFunction(methodName))
        {
            return "This pure function returns a value that should be used. Consider assigning to a variable or returning the result";
        }

        return "Consider using Match, checking IsSome/IsNone, calling GetValueOrDefault/GetValueOrThrow, or using OnSome/OnNone/OnEither for side effects";
    }

    /// <summary>
    /// Gets a display string for an Option type symbol.
    /// </summary>
    /// <param name="type">The type symbol to convert to string.</param>
    /// <returns>A string representation of the Option type (e.g., "Option&lt;int&gt;").</returns>
    private static string GetOptionTypeString(ITypeSymbol? type)
    {
        if (type is INamedTypeSymbol namedType && SymbolHelpers.IsOptionType(namedType))
        {
            var typeArg = namedType.TypeArguments.FirstOrDefault();
            return $"Option<{typeArg?.ToDisplayString() ?? "T"}>";
        }
        return "Option";
    }
}
