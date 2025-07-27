namespace FluentUnions.Tests.Errors
{
    public class AuthorizationErrorTests
    {
        [Fact]
        public void Constructor_WithCodeAndMessage_CreatesErrorCorrectly()
        {
            // Arrange
            const string code = "Auth.InsufficientPermissions";
            const string message = "User lacks required permissions";

            // Act
            var error = new AuthorizationError(code, message);

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
            const string code = "Auth.RoleRequired";
            const string message = "Admin role required";
            var metadata = new Dictionary<string, object>
            {
                ["RequiredRole"] = "Admin",
                ["UserRole"] = "Member",
                ["Resource"] = "UserManagement"
            };

            // Act
            var error = new AuthorizationError(code, message, metadata);

            // Assert
            Assert.Equal(code, error.Code);
            Assert.Equal(message, error.Message);
            Assert.Equal(3, error.Metadata.Count);
            Assert.Equal("Admin", error.Metadata["RequiredRole"]);
            Assert.Equal("Member", error.Metadata["UserRole"]);
        }

        [Fact]
        public void Equals_WithSameCodeAndMessage_ReturnsTrue()
        {
            // Arrange
            const string code = "Auth.AccessDenied";
            const string message = "Access denied";
            var error1 = new AuthorizationError(code, message);
            var error2 = new AuthorizationError(code, message);

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
            var error1 = new AuthorizationError("Auth.AccessDenied", "Access denied");
            var error2 = new AuthorizationError("Auth.RoleRequired", "Access denied");

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
            var authzError = new AuthorizationError("E001", "Error");
            var authError = new AuthenticationError("E001", "Error");

            // Act & Assert
            Assert.NotEqual<Error>(authzError, authError);
            Assert.False(authzError.Equals(authError));
        }

        [Fact]
        public void ToString_WithoutMetadata_ReturnsFormattedString()
        {
            // Arrange
            var error = new AuthorizationError("Auth.AccessDenied", "Access denied to resource");

            // Act
            var result = error.ToString();

            // Assert
            Assert.Equal("AuthorizationError: Auth.AccessDenied - Access denied to resource", result);
        }

        [Fact]
        public void ToString_WithMetadata_IncludesMetadataInString()
        {
            // Arrange
            var metadata = new Dictionary<string, object>
            {
                ["Resource"] = "Document",
                ["Action"] = "Delete",
                ["UserId"] = 12345
            };
            var error = new AuthorizationError("Auth.ResourceAccessDenied", "Cannot delete document", metadata);

            // Act
            var result = error.ToString();

            // Assert
            Assert.Contains("AuthorizationError: Auth.ResourceAccessDenied - Cannot delete document", result);
            Assert.Contains("Metadata:", result);
            Assert.Contains("Resource: Document", result);
            Assert.Contains("Action: Delete", result);
            Assert.Contains("UserId: 12345", result);
        }

        [Fact]
        public void GetHashCode_ForSameCodeAndType_ReturnsSameValue()
        {
            // Arrange
            var error1 = new AuthorizationError("Auth.RoleRequired", "Admin role required");
            var error2 = new AuthorizationError("Auth.RoleRequired", "Different message");

            // Act & Assert
            Assert.Equal(error1.GetHashCode(), error2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ForDifferentCode_ReturnsDifferentValue()
        {
            // Arrange
            var error1 = new AuthorizationError("Auth.RoleRequired", "Admin role required");
            var error2 = new AuthorizationError("Auth.AccessDenied", "Admin role required");

            // Act & Assert
            Assert.NotEqual(error1.GetHashCode(), error2.GetHashCode());
        }

        [Fact]
        public void CommonScenarios_InsufficientPermissions()
        {
            // Arrange & Act
            var error = new AuthorizationError("Auth.InsufficientPermissions", "User lacks required permissions");

            // Assert
            Assert.IsType<AuthorizationError>(error);
            Assert.IsAssignableFrom<Error>(error);
            Assert.Equal("Auth.InsufficientPermissions", error.Code);
        }

        [Fact]
        public void CommonScenarios_RoleBasedAccess()
        {
            // Arrange & Act
            var metadata = new Dictionary<string, object>
            {
                ["RequiredRoles"] = new[] { "Admin", "SuperUser" },
                ["UserRoles"] = new[] { "Member" },
                ["Feature"] = "UserManagement"
            };
            var error = new AuthorizationError("Auth.RoleRequired", "Admin or SuperUser role required", metadata);

            // Assert
            Assert.Equal(3, error.Metadata.Count);
            Assert.True(error.Metadata.ContainsKey("RequiredRoles"));
            Assert.True(error.Metadata.ContainsKey("UserRoles"));
        }

        [Fact]
        public void CommonScenarios_ResourceBasedAccess()
        {
            // Arrange & Act
            var metadata = new Dictionary<string, object>
            {
                ["ResourceType"] = "Document",
                ["ResourceId"] = "doc-123",
                ["OwnerId"] = "user-456",
                ["RequestingUserId"] = "user-789",
                ["Action"] = "Edit"
            };
            var error = new AuthorizationError("Auth.ResourceAccessDenied", 
                "You don't have permission to edit this document", metadata);

            // Assert
            Assert.Equal("Auth.ResourceAccessDenied", error.Code);
            Assert.Equal(5, error.Metadata.Count);
            Assert.Equal("Document", error.Metadata["ResourceType"]);
        }

        [Fact]
        public void CommonScenarios_FeatureRestriction()
        {
            // Arrange & Act
            var metadata = new Dictionary<string, object>
            {
                ["Feature"] = "AdvancedReporting",
                ["UserPlan"] = "Basic",
                ["RequiredPlan"] = "Premium"
            };
            var error = new AuthorizationError("Auth.FeatureRestricted", 
                "This feature requires a Premium subscription", metadata);

            // Assert
            Assert.Contains("Premium", error.Message);
            Assert.Equal("Basic", error.Metadata["UserPlan"]);
            Assert.Equal("Premium", error.Metadata["RequiredPlan"]);
        }
    }
}