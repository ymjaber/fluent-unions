namespace FluentUnions.Tests.Integration
{
    /// <summary>
    /// Examples demonstrating the new message-only constructors and implicit string conversion.
    /// </summary>
    public class ImplicitConversionExampleTests
    {
        [Fact]
        public void Example_SimpleErrorHandling_WithImplicitConversion()
        {
            // Before: Had to create Error with code or use full constructor
            // var result = Result.Failure<int>(new Error("ERR001", "Division by zero"));
            
            // Now: Can use implicit conversion from string
            var result = Result.Failure<int>("Division by zero");
            
            Assert.True(result.IsFailure);
            Assert.Equal("Division by zero", result.Error.Message);
            Assert.Equal(string.Empty, result.Error.Code);
        }

        [Fact]
        public void Example_QuickValidation_WithMessageOnlyConstructor()
        {
            // Simple validation without needing error codes
            Result<string> ValidateUsername(string username)
            {
                if (string.IsNullOrWhiteSpace(username))
                    return new ValidationError("Username cannot be empty");
                
                if (username.Length < 3)
                    return new ValidationError("Username must be at least 3 characters");
                
                if (username.Contains(" "))
                    return new ValidationError("Username cannot contain spaces");
                
                return username;
            }

            // Test the validation
            var result1 = ValidateUsername("");
            var result2 = ValidateUsername("ab");
            var result3 = ValidateUsername("user name");
            var result4 = ValidateUsername("validuser");

            Assert.True(result1.IsFailure);
            Assert.True(result2.IsFailure);
            Assert.True(result3.IsFailure);
            Assert.True(result4.IsSuccess);
        }

        [Fact]
        public void Example_MixedErrorHandling_WithAndWithoutCodes()
        {
            Result ProcessOrder(int orderId, decimal amount)
            {
                // Critical errors use codes for tracking
                if (orderId <= 0)
                    return new ValidationError("Order.InvalidId", "Order ID must be positive");
                
                // Simple validations can use message-only
                if (amount <= 0)
                    return new ValidationError("Amount must be greater than zero");
                
                // Quick checks with implicit conversion
                if (amount > 10000)
                    return Result.Failure("Amount exceeds maximum allowed value");
                
                return Result.Success();
            }

            var result1 = ProcessOrder(-1, 100);
            var result2 = ProcessOrder(1, -50);
            var result3 = ProcessOrder(1, 15000);

            Assert.Equal("Order.InvalidId", result1.Error.Code);
            Assert.Equal(string.Empty, result2.Error.Code);
            Assert.Equal(string.Empty, result3.Error.Code);
        }

        [Fact]
        public void Example_ChainedOperations_WithImplicitConversion()
        {
            Result<decimal> CalculateDiscount(decimal price, string discountCode)
            {
                if (price <= 0)
                    return Result.Failure<decimal>("Invalid price");
                
                if (string.IsNullOrEmpty(discountCode))
                    return Result.Failure<decimal>("Discount code required");
                
                // Simulate discount lookup
                return discountCode switch
                {
                    "SAVE10" => price * 0.1m,
                    "SAVE20" => price * 0.2m,
                    _ => new NotFoundError($"Unknown discount code: {discountCode}")
                };
            }

            var result1 = CalculateDiscount(-10, "SAVE10");
            var result2 = CalculateDiscount(100, "");
            var result3 = CalculateDiscount(100, "INVALID");
            var result4 = CalculateDiscount(100, "SAVE20");

            Assert.True(result1.IsFailure);
            Assert.IsType<Error>(result1.Error);
            Assert.Equal("Invalid price", result1.Error.Message);

            Assert.True(result2.IsFailure);
            Assert.Equal("Discount code required", result2.Error.Message);

            Assert.True(result3.IsFailure);
            Assert.IsType<NotFoundError>(result3.Error);

            Assert.True(result4.IsSuccess);
            Assert.Equal(20m, result4.Value);
        }

        [Fact]
        public void Example_ErrorBuilder_WithMixedErrors()
        {
            var builder = new ErrorBuilder();
            
            // Can append errors created different ways
            builder.Append(new ValidationError("Field.Required", "Name is required"));
            builder.Append(new ValidationError("Email is invalid"));
            builder.Append("General processing error");
            
            if (builder.TryBuild(out var error))
            {
                Assert.IsType<AggregateError>(error);
                var aggregate = (AggregateError)error;
                Assert.Equal(3, aggregate.Errors.Count);
            }
        }

        [Fact]
        public void Example_AuthenticationFlow_SimplifiedErrors()
        {
            Result<string> Authenticate(string username, string password)
            {
                // Quick validation with implicit conversion
                if (string.IsNullOrEmpty(username))
                    return Result.Failure<string>("Username required");
                
                if (string.IsNullOrEmpty(password))
                    return Result.Failure<string>("Password required");
                
                // More specific errors where needed
                if (username != "admin" || password != "secret")
                    return new AuthenticationError("Invalid credentials");
                
                return "auth-token-12345";
            }

            var result1 = Authenticate("", "password");
            var result2 = Authenticate("admin", "wrong");
            var result3 = Authenticate("admin", "secret");

            Assert.Equal("Username required", result1.Error.Message);
            Assert.IsType<Error>(result1.Error); // Base Error type from implicit conversion

            Assert.IsType<AuthenticationError>(result2.Error);
            Assert.Equal("Invalid credentials", result2.Error.Message);

            Assert.True(result3.IsSuccess);
            Assert.Equal("auth-token-12345", result3.Value);
        }
    }
}