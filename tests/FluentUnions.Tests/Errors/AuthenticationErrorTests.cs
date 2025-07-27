namespace FluentUnions.Tests.Errors
{
    public class AuthenticationErrorTests
    {
        [Fact]
        public void Constructor_WithCodeAndMessage_CreatesErrorCorrectly()
        {
            // Arrange
            const string code = "Auth.InvalidCredentials";
            const string message = "Invalid username or password";

            // Act
            var error = new AuthenticationError(code, message);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(message, error.Message);
            Assert.NotNull(error.Metadata);
            Assert.Empty(error.Metadata);
        }

        [Fact]
        public void Constructor_WithCodeMessageAndMetadata_CreatesErrorCorrectly()
        {
            // Arrange
            const string code = "Auth.TokenExpired";
            const string message = "Authentication token has expired";
            var metadata = new Dictionary<string, object>
            {
                ["TokenType"] = "JWT",
                ["ExpiresAt"] = DateTime.UtcNow.AddHours(-1),
                ["IssuedAt"] = DateTime.UtcNow.AddDays(-1)
            };

            // Act
            var error = new AuthenticationError(code, message, metadata);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(message, error.Message);
            Assert.Equal(3, error.Metadata.Count);
            Assert.Equal("JWT", error.Metadata["TokenType"]);
        }

        [Fact]
        public void Equals_WithSameCodeAndMessage_ReturnsTrue()
        {
            // Arrange
            const string code = "Auth.InvalidCredentials";
            const string message = "Invalid credentials";
            var error1 = new AuthenticationError(code, message);
            var error2 = new AuthenticationError(code, message);

            // Act & Assert
            Assert.Equal(error1, error2);
            Assert.True(error1.Equals(error2));
            Assert.True(error1 == error2);
            Assert.False(error1 != error2);
        }

        [Fact]
        public void Equals_WithDifferentCode_ReturnsFalse()
        {
            // Arrange
            var error1 = new AuthenticationError("Auth.InvalidCredentials", "Invalid credentials");
            var error2 = new AuthenticationError("Auth.TokenExpired", "Invalid credentials");

            // Act & Assert
            Assert.NotEqual(error1, error2);
            Assert.False(error1.Equals(error2));
            Assert.False(error1 == error2);
            Assert.True(error1 != error2);
        }

        [Fact]
        public void Equals_WithDifferentErrorType_ReturnsFalse()
        {
            // Arrange
            var authError = new AuthenticationError("E001", "Error");
            var validationError = new ValidationError("E001", "Error");

            // Act & Assert
            Assert.NotEqual<Error>(authError, validationError);
            Assert.False(authError.Equals(validationError));
        }

        [Fact]
        public void ToString_WithoutMetadata_ReturnsFormattedString()
        {
            // Arrange
            var error = new AuthenticationError("Auth.InvalidCredentials", "Invalid username or password");

            // Act
            var result = error.ToString();

            // Assert
            Assert.Equal("AuthenticationError: Auth.InvalidCredentials - Invalid username or password", result);
        }

        [Fact]
        public void ToString_WithMetadata_IncludesMetadataInString()
        {
            // Arrange
            var metadata = new Dictionary<string, object>
            {
                ["Username"] = "john.doe",
                ["AuthMethod"] = "password"
            };
            var error = new AuthenticationError("Auth.InvalidCredentials", "Invalid credentials", metadata);

            // Act
            var result = error.ToString();

            // Assert
            Assert.Contains("AuthenticationError: Auth.InvalidCredentials - Invalid credentials", result);
            Assert.Contains("Metadata:", result);
            Assert.Contains("Username: john.doe", result);
            Assert.Contains("AuthMethod: password", result);
        }

        [Fact]
        public void GetHashCode_ForSameCodeAndType_ReturnsSameValue()
        {
            // Arrange
            var error1 = new AuthenticationError("Auth.InvalidToken", "Token is invalid");
            var error2 = new AuthenticationError("Auth.InvalidToken", "Different message");

            // Act & Assert
            Assert.Equal(error1.GetHashCode(), error2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ForDifferentCode_ReturnsDifferentValue()
        {
            // Arrange
            var error1 = new AuthenticationError("Auth.InvalidToken", "Token is invalid");
            var error2 = new AuthenticationError("Auth.TokenExpired", "Token is invalid");

            // Act & Assert
            Assert.NotEqual(error1.GetHashCode(), error2.GetHashCode());
        }

        [Fact]
        public void CommonScenarios_InvalidCredentials()
        {
            // Arrange & Act
            var error = new AuthenticationError("Auth.InvalidCredentials", "Invalid username or password");

            // Assert
            Assert.IsType<AuthenticationError>(error);
            Assert.IsAssignableFrom<Error>(error);
            Assert.Equal("Auth.InvalidCredentials", error.Code);
        }

        [Fact]
        public void CommonScenarios_TokenExpired()
        {
            // Arrange & Act
            var metadata = new Dictionary<string, object>
            {
                ["TokenId"] = Guid.NewGuid().ToString(),
                ["ExpiresAt"] = DateTime.UtcNow.AddHours(-1)
            };
            var error = new AuthenticationError("Auth.TokenExpired", "Authentication token has expired", metadata);

            // Assert
            Assert.Equal(2, error.Metadata.Count);
            Assert.True(error.Metadata.ContainsKey("TokenId"));
            Assert.True(error.Metadata.ContainsKey("ExpiresAt"));
        }

        [Fact]
        public void CommonScenarios_MfaRequired()
        {
            // Arrange & Act
            var metadata = new Dictionary<string, object>
            {
                ["RequiredFactors"] = new[] { "SMS", "Authenticator" },
                ["CompletedFactors"] = new[] { "Password" }
            };
            var error = new AuthenticationError("Auth.MfaRequired", "Multi-factor authentication required", metadata);

            // Assert
            Assert.Equal("Auth.MfaRequired", error.Code);
            Assert.Contains("Multi-factor", error.Message);
            Assert.Equal(2, error.Metadata.Count);
        }
    }
}