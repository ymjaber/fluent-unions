# Testing Guide

This guide covers testing strategies and patterns for applications using FluentUnions, including the FluentUnions.AwesomeAssertions library.

## Table of Contents
1. [Introduction](#introduction)
2. [FluentUnions.AwesomeAssertions](#fluentunionsawesomeassertions)
3. [Testing Result Types](#testing-result-types)
4. [Testing Option Types](#testing-option-types)
5. [Testing Error Handling](#testing-error-handling)
6. [Testing Async Operations](#testing-async-operations)
7. [Mocking and Test Doubles](#mocking-and-test-doubles)
8. [Testing Patterns](#testing-patterns)
9. [Integration Testing](#integration-testing)
10. [Best Practices](#best-practices)

## Introduction

Testing code that uses FluentUnions requires verifying both success and failure paths. The FluentUnions.AwesomeAssertions package provides specialized assertions to make this easier.

### Installation

```bash
dotnet add package FluentUnions.AwesomeAssertions
```

### Basic Setup

```csharp
using FluentUnions;
using FluentUnions.AwesomeAssertions;
using Xunit; // or NUnit, MSTest, etc.

public class UserServiceTests
{
    private readonly UserService _sut; // System Under Test
    
    public UserServiceTests()
    {
        _sut = new UserService();
    }
}
```

## FluentUnions.AwesomeAssertions

### Result Assertions

```csharp
// Success assertions
result.Should().BeSuccess();
result.Should().BeSuccessWithValue(expectedValue);
result.Should().BeSuccessWithValueMatching(v => v.Id == expectedId);

// Failure assertions
result.Should().BeFailure();
result.Should().BeFailureWithError<ValidationError>();
result.Should().BeFailureWithMessage("Expected error message");
result.Should().BeFailureWithCode("ERROR_CODE");

// Error type assertions
result.Should().HaveError<NotFoundError>();
result.Should().HaveErrorMatching(e => e.Code == "USER_NOT_FOUND");
```

### Option Assertions

```csharp
// Some assertions
option.Should().BeSome();
option.Should().BeSomeWithValue(42);
option.Should().BeSomeWithValueMatching(v => v.Name == "John");

// None assertions
option.Should().BeNone();

// Value assertions
option.Should().HaveValue(expectedValue);
option.Should().HaveValueMatching(v => v.IsActive);
```

### Custom Assertions

```csharp
// Complex assertions
result.Should().Satisfy(r => 
    r.IsSuccess && 
    r.Value.Items.Count == 3 &&
    r.Value.Total > 100);

// Chained assertions
result.Should()
    .BeSuccess()
    .And.HaveValue()
    .Which.Should().BeOfType<User>()
    .And.Match(u => u.Email == "test@example.com");
```

## Testing Result Types

### Testing Success Cases

```csharp
[Fact]
public void GetUser_WithValidId_ReturnsSuccess()
{
    // Arrange
    var userId = Guid.NewGuid();
    var expectedUser = new User { Id = userId, Name = "John Doe" };
    _repository.Setup(r => r.FindById(userId)).Returns(expectedUser);
    
    // Act
    var result = _sut.GetUser(userId);
    
    // Assert
    result.Should().BeSuccessWithValue(expectedUser);
}

[Fact]
public void CreateOrder_WithValidRequest_ReturnsSuccessfulOrder()
{
    // Arrange
    var request = new OrderRequest
    {
        CustomerId = Guid.NewGuid(),
        Items = new[] { new OrderItem("SKU123", 2) }
    };
    
    // Act
    var result = _sut.CreateOrder(request);
    
    // Assert
    result.Should().BeSuccess();
    result.Value.Should().NotBeNull();
    result.Value.Id.Should().NotBeEmpty();
    result.Value.Items.Should().HaveCount(1);
    result.Value.Status.Should().Be(OrderStatus.Pending);
}
```

### Testing Failure Cases

```csharp
[Fact]
public void GetUser_WithInvalidId_ReturnsNotFoundError()
{
    // Arrange
    var userId = Guid.NewGuid();
    _repository.Setup(r => r.FindById(userId)).Returns((User)null);
    
    // Act
    var result = _sut.GetUser(userId);
    
    // Assert
    result.Should()
        .BeFailure()
        .WithError<NotFoundError>()
        .WithMessage($"User {userId} not found");
}

[Theory]
[InlineData("", "Email is required")]
[InlineData("invalid", "Invalid email format")]
[InlineData("test@", "Invalid email format")]
public void ValidateEmail_WithInvalidInput_ReturnsValidationError(
    string email, string expectedMessage)
{
    // Act
    var result = _sut.ValidateEmail(email);
    
    // Assert
    result.Should()
        .BeFailure()
        .WithError<ValidationError>()
        .WithMessage(expectedMessage);
}
```

### Testing Multiple Outcomes

```csharp
[Fact]
public void ProcessPayment_TestAllOutcomes()
{
    var testCases = new[]
    {
        new { Amount = 100m, CardNumber = "4111111111111111", 
              Expected = PaymentStatus.Approved },
        new { Amount = 0m, CardNumber = "4111111111111111", 
              Expected = PaymentStatus.Rejected },
        new { Amount = 100m, CardNumber = "4111111111111112", 
              Expected = PaymentStatus.Declined }
    };
    
    foreach (var testCase in testCases)
    {
        // Arrange
        var request = new PaymentRequest
        {
            Amount = testCase.Amount,
            CardNumber = testCase.CardNumber
        };
        
        // Act
        var result = _sut.ProcessPayment(request);
        
        // Assert
        if (testCase.Expected == PaymentStatus.Approved)
        {
            result.Should().BeSuccessWithValueMatching(
                p => p.Status == PaymentStatus.Approved);
        }
        else
        {
            result.Should().BeFailure();
        }
    }
}
```

## Testing Option Types

### Testing Some Cases

```csharp
[Fact]
public void FindUser_WhenUserExists_ReturnsSome()
{
    // Arrange
    var email = "test@example.com";
    var expectedUser = new User { Email = email };
    _repository.Setup(r => r.FindByEmail(email)).Returns(expectedUser);
    
    // Act
    var result = _sut.FindUser(email);
    
    // Assert
    result.Should().BeSomeWithValue(expectedUser);
}

[Fact]
public void GetConfiguration_WithExistingKey_ReturnsSomeValue()
{
    // Arrange
    _configStore.Add("api.key", "secret123");
    
    // Act
    var result = _sut.GetConfiguration("api.key");
    
    // Assert
    result.Should()
        .BeSome()
        .And.HaveValue("secret123");
}
```

### Testing None Cases

```csharp
[Fact]
public void FindUser_WhenUserDoesNotExist_ReturnsNone()
{
    // Arrange
    var email = "nonexistent@example.com";
    _repository.Setup(r => r.FindByEmail(email)).Returns((User)null);
    
    // Act
    var result = _sut.FindUser(email);
    
    // Assert
    result.Should().BeNone();
}

[Fact]
public void GetOptionalFeature_WhenDisabled_ReturnsNone()
{
    // Arrange
    _featureFlags.Setup(f => f.IsEnabled("NewFeature")).Returns(false);
    
    // Act
    var result = _sut.GetOptionalFeature("NewFeature");
    
    // Assert
    result.Should().BeNone();
}
```

## Testing Error Handling

### Testing Specific Error Types

```csharp
[Fact]
public void TransferMoney_WithInsufficientFunds_ReturnsSpecificError()
{
    // Arrange
    var fromAccount = new Account { Balance = 50 };
    var request = new TransferRequest { Amount = 100 };
    
    // Act
    var result = _sut.TransferMoney(fromAccount, request);
    
    // Assert
    result.Should()
        .BeFailure()
        .WithError<InsufficientFundsError>()
        .Which.Should().Match<InsufficientFundsError>(e => 
            e.Available == 50 && 
            e.Requested == 100);
}

[Fact]
public void AuthenticateUser_WithInvalidCredentials_ReturnsAuthError()
{
    // Arrange
    var credentials = new LoginRequest
    {
        Username = "user",
        Password = "wrong"
    };
    
    // Act
    var result = _sut.Authenticate(credentials);
    
    // Assert
    result.Should()
        .BeFailure()
        .WithError<AuthenticationError>()
        .WithMetadata("attemptedUsername", "user");
}
```

### Testing Error Metadata

```csharp
[Fact]
public void ValidateOrder_WithMultipleErrors_ReturnsAggregateError()
{
    // Arrange
    var order = new Order
    {
        Items = new List<OrderItem>(),
        CustomerId = Guid.Empty
    };
    
    // Act
    var result = _sut.ValidateOrder(order);
    
    // Assert
    result.Should().BeFailure();
    
    var error = result.Error as AggregateError;
    error.Should().NotBeNull();
    error.Errors.Should().HaveCount(2);
    error.Errors.Should().Contain(e => e.Message.Contains("items"));
    error.Errors.Should().Contain(e => e.Message.Contains("customer"));
}

[Fact]
public void ProcessRequest_OnFailure_IncludesContextInError()
{
    // Arrange
    var request = new ProcessRequest { Id = "REQ123" };
    
    // Act
    var result = _sut.ProcessRequest(request);
    
    // Assert
    result.Should()
        .BeFailure()
        .WithErrorMatching(e => 
            e.Metadata.ContainsKey("requestId") &&
            e.Metadata["requestId"].ToString() == "REQ123");
}
```

## Testing Async Operations

### Async Result Tests

```csharp
[Fact]
public async Task GetUserAsync_WithValidId_ReturnsSuccess()
{
    // Arrange
    var userId = Guid.NewGuid();
    var expectedUser = new User { Id = userId };
    _repository.Setup(r => r.FindByIdAsync(userId))
              .ReturnsAsync(expectedUser);
    
    // Act
    var result = await _sut.GetUserAsync(userId);
    
    // Assert
    result.Should().BeSuccessWithValue(expectedUser);
}

[Fact]
public async Task ProcessOrderAsync_WithValidRequest_CompletesSuccessfully()
{
    // Arrange
    var request = CreateValidOrderRequest();
    
    // Act
    var result = await _sut.ProcessOrderAsync(request);
    
    // Assert
    result.Should().BeSuccess();
    
    // Verify async operations were called
    _paymentGateway.Verify(
        p => p.ProcessPaymentAsync(It.IsAny<PaymentRequest>()), 
        Times.Once);
    _inventoryService.Verify(
        i => i.ReserveItemsAsync(It.IsAny<List<OrderItem>>()), 
        Times.Once);
}
```

### Testing Async Chains

```csharp
[Fact]
public async Task ComplexWorkflow_ExecutesAllStepsInOrder()
{
    // Arrange
    var callOrder = new List<string>();
    
    _service1.Setup(s => s.Step1Async())
        .ReturnsAsync(Result.Success("step1"))
        .Callback(() => callOrder.Add("Step1"));
        
    _service2.Setup(s => s.Step2Async(It.IsAny<string>()))
        .ReturnsAsync(Result.Success("step2"))
        .Callback(() => callOrder.Add("Step2"));
        
    _service3.Setup(s => s.Step3Async(It.IsAny<string>()))
        .ReturnsAsync(Result.Success("step3"))
        .Callback(() => callOrder.Add("Step3"));
    
    // Act
    var result = await _sut.ExecuteWorkflowAsync();
    
    // Assert
    result.Should().BeSuccess();
    callOrder.Should().Equal("Step1", "Step2", "Step3");
}
```

## Mocking and Test Doubles

### Mocking Result-Returning Methods

```csharp
public interface IUserRepository
{
    Result<User> GetById(Guid id);
    Result<User> Create(User user);
    Result Delete(Guid id);
}

[Fact]
public void CreateUser_CallsRepositoryCorrectly()
{
    // Arrange
    var mockRepo = new Mock<IUserRepository>();
    var newUser = new User { Email = "test@example.com" };
    
    mockRepo.Setup(r => r.Create(It.IsAny<User>()))
           .Returns<User>(u => Result.Success(u));
    
    var service = new UserService(mockRepo.Object);
    
    // Act
    var result = service.CreateUser("test@example.com", "password");
    
    // Assert
    result.Should().BeSuccess();
    mockRepo.Verify(r => r.Create(It.Is<User>(u => 
        u.Email == "test@example.com")), Times.Once);
}
```

### Creating Test Doubles

```csharp
public class TestUserRepository : IUserRepository
{
    private readonly Dictionary<Guid, User> _users = new();
    
    public Result<User> GetById(Guid id)
    {
        return _users.TryGetValue(id, out var user)
            ? Result.Success(user)
            : new NotFoundError($"User {id} not found");
    }
    
    public Result<User> Create(User user)
    {
        if (_users.Any(u => u.Value.Email == user.Email))
            return new ConflictError("Email already exists");
            
        _users[user.Id] = user;
        return Result.Success(user);
    }
    
    public Result Delete(Guid id)
    {
        return _users.Remove(id)
            ? Result.Success()
            : new NotFoundError($"User {id} not found");
    }
}
```

## Testing Patterns

### Arrange-Act-Assert with Results

```csharp
[Fact]
public void StandardTestPattern()
{
    // Arrange
    var input = new TestInput { Value = 42 };
    var expected = new TestOutput { Result = 84 };
    
    // Act
    var result = _sut.Process(input);
    
    // Assert
    result.Should().BeSuccessWithValue(expected);
}
```

### Table-Driven Tests

```csharp
[Theory]
[MemberData(nameof(ValidationTestCases))]
public void ValidateInput_TestVariousCases(
    ValidationTestCase testCase)
{
    // Act
    var result = _sut.Validate(testCase.Input);
    
    // Assert
    if (testCase.ShouldSucceed)
    {
        result.Should().BeSuccess();
    }
    else
    {
        result.Should()
            .BeFailure()
            .WithError(testCase.ExpectedErrorType)
            .WithMessage(testCase.ExpectedMessage);
    }
}

public static IEnumerable<object[]> ValidationTestCases =>
    new List<object[]>
    {
        new object[] { new ValidationTestCase 
        { 
            Input = "valid@email.com", 
            ShouldSucceed = true 
        }},
        new object[] { new ValidationTestCase 
        { 
            Input = "invalid", 
            ShouldSucceed = false,
            ExpectedErrorType = typeof(ValidationError),
            ExpectedMessage = "Invalid email format"
        }},
    };
```

### Testing Railway Flows

```csharp
[Fact]
public void OrderProcessing_FailsAtInventoryCheck_StopsProcessing()
{
    // Arrange
    var request = CreateValidOrderRequest();
    
    _validator.Setup(v => v.Validate(request))
              .Returns(Result.Success(request));
              
    _inventory.Setup(i => i.CheckAvailability(It.IsAny<List<Item>>()))
              .Returns(Result.Failure("Out of stock"));
    
    // Act
    var result = _sut.ProcessOrder(request);
    
    // Assert
    result.Should()
        .BeFailure()
        .WithMessage("Out of stock");
        
    // Verify payment was never attempted
    _paymentService.Verify(
        p => p.ProcessPayment(It.IsAny<PaymentRequest>()), 
        Times.Never);
}

[Fact]
public void OrderProcessing_SuccessfulFlow_ExecutesAllSteps()
{
    // Arrange
    var request = CreateValidOrderRequest();
    SetupSuccessfulMocks();
    
    // Act
    var result = _sut.ProcessOrder(request);
    
    // Assert
    result.Should().BeSuccess();
    
    // Verify all steps were executed
    VerifyAllStepsExecuted();
}
```

## Integration Testing

### Testing with Real Dependencies

```csharp
public class UserServiceIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly UserService _sut;
    
    public UserServiceIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _sut = new UserService(_fixture.DbContext);
    }
    
    [Fact]
    public async Task CreateUser_PersistsToDatabase()
    {
        // Arrange
        var email = $"test_{Guid.NewGuid()}@example.com";
        
        // Act
        var result = await _sut.CreateUserAsync(email, "password");
        
        // Assert
        result.Should().BeSuccess();
        
        // Verify in database
        var userInDb = await _fixture.DbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
            
        userInDb.Should().NotBeNull();
        userInDb.Id.Should().Be(result.Value.Id);
    }
}
```

### Testing Error Scenarios

```csharp
[Fact]
public async Task TransferMoney_WithConcurrentTransfers_HandlesCorrectly()
{
    // Arrange
    var account = await CreateAccountWithBalance(1000);
    var transferTasks = new List<Task<Result<Transfer>>>();
    
    // Act - Attempt 10 concurrent transfers of 200 each
    for (int i = 0; i < 10; i++)
    {
        transferTasks.Add(_sut.TransferMoneyAsync(
            account.Id, 
            "recipient", 
            200));
    }
    
    var results = await Task.WhenAll(transferTasks);
    
    // Assert
    var successCount = results.Count(r => r.IsSuccess);
    var failureCount = results.Count(r => r.IsFailure);
    
    successCount.Should().Be(5); // Only 5 should succeed
    failureCount.Should().Be(5); // 5 should fail with insufficient funds
    
    // Verify final balance
    var finalAccount = await _sut.GetAccountAsync(account.Id);
    finalAccount.Value.Balance.Should().Be(0);
}
```

## Best Practices

### 1. Test Both Success and Failure Paths

```csharp
public class PaymentServiceTests
{
    [Fact]
    public void ProcessPayment_WithValidCard_Succeeds() { }
    
    [Fact]
    public void ProcessPayment_WithInvalidCard_FailsWithDecline() { }
    
    [Fact]
    public void ProcessPayment_WithExpiredCard_FailsWithExpiredError() { }
    
    [Fact]
    public void ProcessPayment_WithNetworkError_FailsWithRetryableError() { }
}
```

### 2. Use Descriptive Test Names

```csharp
// Good test names
[Fact]
public void GetUser_WhenUserExists_ReturnsSuccessWithUser() { }

[Fact]
public void GetUser_WhenUserDoesNotExist_ReturnsNotFoundError() { }

[Fact]
public void TransferMoney_WhenInsufficientFunds_ReturnsInsufficientFundsError() { }
```

### 3. Test Error Metadata

```csharp
[Fact]
public void FailedOperation_IncludesRelevantContext()
{
    // Act
    var result = _sut.PerformOperation(request);
    
    // Assert
    result.Should().BeFailure();
    result.Error.Metadata.Should().ContainKey("requestId");
    result.Error.Metadata.Should().ContainKey("timestamp");
    result.Error.Metadata.Should().ContainKey("userId");
}
```

### 4. Use Builder Pattern for Complex Test Data

```csharp
public class OrderBuilder
{
    private Guid _customerId = Guid.NewGuid();
    private List<OrderItem> _items = new() { new OrderItem("DEFAULT", 1) };
    
    public OrderBuilder WithCustomerId(Guid id)
    {
        _customerId = id;
        return this;
    }
    
    public OrderBuilder WithItems(params OrderItem[] items)
    {
        _items = items.ToList();
        return this;
    }
    
    public Order Build() => new Order
    {
        CustomerId = _customerId,
        Items = _items
    };
}

// Usage in tests
var order = new OrderBuilder()
    .WithCustomerId(testCustomerId)
    .WithItems(
        new OrderItem("SKU1", 2),
        new OrderItem("SKU2", 1))
    .Build();
```

### 5. Test Async Properly

```csharp
// Don't do this
[Fact]
public void BadAsyncTest()
{
    var result = _sut.GetAsync().Result; // Can deadlock
}

// Do this
[Fact]
public async Task GoodAsyncTest()
{
    var result = await _sut.GetAsync();
    result.Should().BeSuccess();
}
```

### 6. Verify Side Effects

```csharp
[Fact]
public void SuccessfulOperation_LogsAndSendsMetrics()
{
    // Arrange
    var mockLogger = new Mock<ILogger>();
    var mockMetrics = new Mock<IMetrics>();
    var sut = new Service(mockLogger.Object, mockMetrics.Object);
    
    // Act
    var result = sut.PerformOperation();
    
    // Assert
    result.Should().BeSuccess();
    
    mockLogger.Verify(
        l => l.LogInfo(It.Is<string>(s => s.Contains("success"))),
        Times.Once);
        
    mockMetrics.Verify(
        m => m.Increment("operation.success"),
        Times.Once);
}
```

## Summary

Testing with FluentUnions requires:

1. **Comprehensive assertions** using FluentUnions.AwesomeAssertions
2. **Testing both paths** - success and failure scenarios
3. **Verifying error details** including types and metadata
4. **Proper async testing** with async/await
5. **Integration tests** for end-to-end scenarios
6. **Clear test organization** with descriptive names

The key is to embrace the explicit nature of Result and Option types in your tests, ensuring all possible outcomes are verified.

Next steps:
- [Source Generators Documentation](../reference/source-generators.md)
- [Analyzer Rules Reference](../reference/analyzers.md)
- [Performance Guide](performance-best-practices.md)