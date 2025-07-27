using System.Text.RegularExpressions;

namespace FluentUnions.AwesomeAssertions.Tests;

public class IntegrationTests
{
    #region Real-World Validation Scenarios

    [Fact]
    public void UserRegistration_WithMultipleErrors_ShouldFailWithAggregateError()
    {
        // Arrange & Act
        var result = ValidateUserRegistration("", "weak", -5);

        // Assert
        result.Should()
            .FailWith<AggregateError>()
            .WithAggregateErrorCount(3)
            .WithAggregateErrorContainingMessage("String cannot be empty")  // NotEmpty error message
            .WithAggregateErrorContainingMessage("String must be at least 8 characters")  // LongerThanOrEqualTo error
            .WithAggregateErrorContainingMessage("Value must be greater than");
    }

    [Fact]
    public void UserRegistration_WithValidData_ShouldSucceed()
    {
        // Arrange & Act
        var result = ValidateUserRegistration("yousef@example.com", "StrongPass123!", 25);

        // Assert
        result.Should().Succeed()
            .Which.Should().NotBeNull();
        
        result.Should().SucceedSatisfying(user =>
        {
            user.Email.Should().Be("yousef@example.com");
            user.Password.Should().Be("StrongPass123!");
            user.Age.Should().Be(25);
        });
    }

    [Fact]
    public void OrderProcessing_WithInvalidOrder_ShouldFailWithSpecificErrors()
    {
        // Arrange
        var order = new Order 
        { 
            CustomerName = "",
            Items = new List<OrderItem>(),
            TotalAmount = -100
        };

        // Act
        var result = ProcessOrder(order);

        // Assert
        result.Should().FailWith<AggregateError>(error =>
            error.Errors.Count == 3 &&
            error.Errors.Any(e => e.Message.Contains("Customer name")) &&
            error.Errors.Any(e => e.Message.Contains("at least one item")) &&
            error.Errors.Any(e => e.Message.Contains("positive"))
        );
    }

    #endregion

    #region Option and Result Interoperability

    [Fact]
    public void FindUser_WhenUserExists_ShouldReturnSome()
    {
        // Arrange
        var repository = new UserRepository();
        repository.AddUser(new User { Email = "yousef@example.com", Password = "pass", Age = 30 });

        // Act
        var option = repository.FindByEmail("yousef@example.com");

        // Assert
        option.Should().BeSomeWithValue(new User { Email = "yousef@example.com", Password = "pass", Age = 30 });
    }

    [Fact]
    public void FindUser_WhenUserDoesNotExist_ShouldReturnNone()
    {
        // Arrange
        var repository = new UserRepository();

        // Act
        var option = repository.FindByEmail("nonexistent@example.com");

        // Assert
        option.Should().BeNone();
    }

    [Fact]
    public void ConvertOptionToResult_WithValidation_ShouldWorkCorrectly()
    {
        // Arrange
        var repository = new UserRepository();
        repository.AddUser(new User { Email = "yousef@example.com", Password = "pass", Age = 30 });

        // Act - Find user and convert to Result with validation
        var result = repository.FindByEmail("yousef@example.com")
            .EnsureSome(new NotFoundError("User not found"))
            .Bind(user => ValidateUserAge(user));

        // Assert
        result.Should().Succeed()
            .Which.Email.Should().Be("yousef@example.com");
    }

    #endregion

    #region Complex Error Scenarios

    [Fact]
    public void PaymentProcessing_WithMultipleFailurePoints_ShouldCaptureAllErrors()
    {
        // Arrange
        var payment = new PaymentRequest
        {
            Amount = -50,
            CardNumber = "1234",
            CustomerEmail = "invalid-email"
        };

        // Act
        var result = ProcessPayment(payment);

        // Assert
        result.Should().Fail()
            .WithAggregateError()
            .Which.Should().Satisfy<AggregateError>(aggregate =>
            {
                aggregate.Errors.Should().HaveCount(4);  // Includes the parent error
                aggregate.Code.Should().Be("Errors.Aggregate");  // Default AggregateError code
            });

        result.Should().Fail()
            .WithAggregateErrorMatching<ValidationError>(error => 
                error.Message.Contains("must be greater than") ||  // Amount error
                error.Message.Contains("must have exactly 16 characters") ||  // Card length error
                error.Message.Contains("must match the pattern"));
    }

    [Fact]
    public void BusinessRuleValidation_WithConflictError_ShouldFailWithCorrectType()
    {
        // Arrange
        var existingUser = new User { Email = "yousef@example.com", Password = "pass", Age = 30 };
        
        // Act
        var result = RegisterNewUser(existingUser);

        // Assert
        result.Should().FailWith<ConflictError>()
            .WithErrorCode("USER_ALREADY_EXISTS")
            .WithErrorMessageContaining("yousef@example.com");
    }

    #endregion

    #region Helper Methods and Classes

    private Result<User> ValidateUserRegistration(string email, string password, int age)
    {
        var emailResult = Result.For(email).Ensure.NotEmpty();
        var passwordResult = Result.For(password).Ensure.LongerThanOrEqualTo(8);
        var ageResult = Result.For(age).Ensure.GreaterThan(0);

        var errorBuilder = new ErrorBuilder();
        errorBuilder.AppendOnFailure(emailResult);
        errorBuilder.AppendOnFailure(passwordResult);
        errorBuilder.AppendOnFailure(ageResult);

        if (errorBuilder.TryBuild(out var error))
            return Result.Failure<User>(error);

        return Result.Success(new User { Email = email, Password = password, Age = age });
    }

    private Result<Order> ProcessOrder(Order order)
    {
        var errorBuilder = new ErrorBuilder();

        if (string.IsNullOrEmpty(order.CustomerName))
            errorBuilder.Append(new ValidationError("Customer name is required"));

        if (order.Items.Count == 0)
            errorBuilder.Append(new ValidationError("Order must have at least one item"));

        if (order.TotalAmount <= 0)
            errorBuilder.Append(new ValidationError("Order amount must be positive"));

        if (errorBuilder.TryBuild(out var error))
            return Result.Failure<Order>(error);

        return Result.Success(order);
    }

    private Result<User> ValidateUserAge(User user)
    {
        if (user.Age < 18)
            return new ValidationError("User must be at least 18 years old");

        return user;
    }

    private Result<PaymentResult> ProcessPayment(PaymentRequest payment)
    {
        var amountResult = Result.For(payment.Amount).Ensure.GreaterThan(0m);
        var cardResult = Result.For(payment.CardNumber).Ensure.HasLength(16);
        var emailResult = Result.For(payment.CustomerEmail).Ensure.Matches(new Regex(@"^[^@]+@[^@]+\.[^@]+$"));

        var errorBuilder = new ErrorBuilder();
        errorBuilder.AppendOnFailure(amountResult);
        errorBuilder.AppendOnFailure(cardResult);
        errorBuilder.AppendOnFailure(emailResult);

        if (errorBuilder.TryBuild(out var error))
        {
            var builder = new ErrorBuilder();
            builder.Append(new Error("PAYMENT_VALIDATION_FAILED", "Payment validation failed"));
            if (error is AggregateError agg)
            {
                foreach (var e in agg.Errors)
                    builder.Append(e);
            }
            else
            {
                builder.Append(error);
            }
            return Result.Failure<PaymentResult>(builder.Build());
        }
        
        return Result.Success(new PaymentResult { Success = true });
    }

    private Result<User> RegisterNewUser(User user)
    {
        // Simulate checking for existing user
        if (user.Email == "yousef@example.com")
            return new ConflictError("USER_ALREADY_EXISTS", $"User with email {user.Email} already exists");

        return user;
    }

    private class User : IEquatable<User>
    {
        public string Email { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public int Age { get; init; }

        public bool Equals(User? other) => 
            other != null && 
            Email == other.Email && 
            Password == other.Password && 
            Age == other.Age;

        public override bool Equals(object? obj) => Equals(obj as User);
        public override int GetHashCode() => HashCode.Combine(Email, Password, Age);
    }

    private class Order
    {
        public string CustomerName { get; init; } = string.Empty;
        public List<OrderItem> Items { get; init; } = new();
        public decimal TotalAmount { get; init; }
    }

    private class OrderItem
    {
        public string ProductName { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public decimal Price { get; init; }
    }

    private class PaymentRequest
    {
        public decimal Amount { get; init; }
        public string CardNumber { get; init; } = string.Empty;
        public string CustomerEmail { get; init; } = string.Empty;
    }

    private class PaymentResult
    {
        public bool Success { get; init; }
    }

    private class UserRepository
    {
        private readonly List<User> _users = new();

        public void AddUser(User user) => _users.Add(user);

        public Option<User> FindByEmail(string email) =>
            _users.FirstOrDefault(u => u.Email == email).AsOption();
    }

    #endregion
}