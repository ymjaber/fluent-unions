namespace FluentUnions.Tests.Errors
{
    public class MessageOnlyConstructorTests
    {
        [Fact]
        public void Error_MessageOnlyConstructor_SetsEmptyCode()
        {
            // Arrange & Act
            var error = new Error("Something went wrong");

            // Assert
            Assert.Equal(string.Empty, error.Code);
            Assert.Equal("Something went wrong", error.Message);
        }

        [Fact]
        public void ValidationError_MessageOnlyConstructor_SetsEmptyCode()
        {
            // Arrange & Act
            var error = new ValidationError("Invalid input provided");

            // Assert
            Assert.Equal(string.Empty, error.Code);
            Assert.Equal("Invalid input provided", error.Message);
            Assert.IsType<ValidationError>(error);
        }

        [Fact]
        public void NotFoundError_MessageOnlyConstructor_SetsEmptyCode()
        {
            // Arrange & Act
            var error = new NotFoundError("Resource could not be found");

            // Assert
            Assert.Equal(string.Empty, error.Code);
            Assert.Equal("Resource could not be found", error.Message);
            Assert.IsType<NotFoundError>(error);
        }

        [Fact]
        public void ConflictError_MessageOnlyConstructor_SetsEmptyCode()
        {
            // Arrange & Act
            var error = new ConflictError("Operation conflicts with existing state");

            // Assert
            Assert.Equal(string.Empty, error.Code);
            Assert.Equal("Operation conflicts with existing state", error.Message);
            Assert.IsType<ConflictError>(error);
        }

        [Fact]
        public void AuthenticationError_MessageOnlyConstructor_SetsEmptyCode()
        {
            // Arrange & Act
            var error = new AuthenticationError("Authentication failed");

            // Assert
            Assert.Equal(string.Empty, error.Code);
            Assert.Equal("Authentication failed", error.Message);
            Assert.IsType<AuthenticationError>(error);
        }

        [Fact]
        public void AuthorizationError_MessageOnlyConstructor_SetsEmptyCode()
        {
            // Arrange & Act
            var error = new AuthorizationError("Access denied");

            // Assert
            Assert.Equal(string.Empty, error.Code);
            Assert.Equal("Access denied", error.Message);
            Assert.IsType<AuthorizationError>(error);
        }

        [Fact]
        public void Error_ImplicitStringConversion_CreatesError()
        {
            // Arrange & Act
            Error error = "This is an error message";

            // Assert
            Assert.Equal(string.Empty, error.Code);
            Assert.Equal("This is an error message", error.Message);
            Assert.IsType<Error>(error);
            Assert.IsNotType<ValidationError>(error);
        }

        [Fact]
        public void Error_ImplicitConversion_WorksWithResultFailure()
        {
            // Arrange & Act
            var result = Result.Failure<int>("Operation failed due to unexpected error");

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(string.Empty, result.Error.Code);
            Assert.Equal("Operation failed due to unexpected error", result.Error.Message);
        }

        [Fact]
        public void Error_ImplicitConversion_WorksInMethodCall()
        {
            // Arrange
            static Result<T> CreateFailure<T>(Error error) => Result.Failure<T>(error);

            // Act
            var result = CreateFailure<string>("Method failed");

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("Method failed", result.Error.Message);
        }

        [Fact]
        public void MessageOnlyErrors_ToString_FormatsCorrectly()
        {
            // Arrange
            var baseError = new Error("Base error message");
            var validationError = new ValidationError("Validation failed");
            var notFoundError = new NotFoundError("Not found");
            var conflictError = new ConflictError("Conflict occurred");
            var authError = new AuthenticationError("Auth failed");
            var authzError = new AuthorizationError("Access denied");

            // Act & Assert
            Assert.Equal("Error: Base error message", baseError.ToString());
            Assert.Equal("ValidationError: Validation failed", validationError.ToString());
            Assert.Equal("NotFoundError: Not found", notFoundError.ToString());
            Assert.Equal("ConflictError: Conflict occurred", conflictError.ToString());
            Assert.Equal("AuthenticationError: Auth failed", authError.ToString());
            Assert.Equal("AuthorizationError: Access denied", authzError.ToString());
        }

        [Fact]
        public void MessageOnlyErrors_Equality_WorksCorrectly()
        {
            // Arrange
            var error1 = new Error("Same message");
            var error2 = new Error("Same message");
            var error3 = new Error("Different message");
            var errorWithCode = new Error("", "Same message");

            // Act & Assert
            Assert.Equal(error1, error2);
            Assert.NotEqual(error1, error3);
            Assert.Equal(error1, errorWithCode); // Same code (empty) and message
        }

        [Fact]
        public void DerivedErrors_MessageOnlyEquality_ConsidersType()
        {
            // Arrange
            var validationError = new ValidationError("Same message");
            var notFoundError = new NotFoundError("Same message");

            // Act & Assert
            Assert.NotEqual<Error>(validationError, notFoundError);
            Assert.Equal(string.Empty, validationError.Code);
            Assert.Equal(string.Empty, notFoundError.Code);
            Assert.Equal("Same message", validationError.Message);
            Assert.Equal("Same message", notFoundError.Message);
        }
    }
}