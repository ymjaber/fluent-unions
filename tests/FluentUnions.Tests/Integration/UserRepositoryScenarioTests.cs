namespace FluentUnions.Tests.Integration
{
    /// <summary>
    /// Integration tests demonstrating real-world usage of FluentUnions in a repository pattern scenario.
    /// </summary>
    public class UserRepositoryScenarioTests
    {
        [Fact]
        public async Task CompleteUserRegistrationFlow_Success()
        {
            // Arrange
            var repository = new UserRepository();
            var service = new UserService(repository);

            // Act
            var result = await service.RegisterUserAsync(
                email: "john.doe@example.com",
                password: "SecureP@ssw0rd",
                name: "John Doe"
            );

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("john.doe@example.com", result.Value.Email);
            Assert.Equal("John Doe", result.Value.Name);
        }

        [Fact]
        public async Task CompleteUserRegistrationFlow_ValidationFailures()
        {
            // Arrange
            var repository = new UserRepository();
            var service = new UserService(repository);

            // Act
            var result = await service.RegisterUserAsync(
                email: "invalid-email",
                password: "weak",
                name: ""
            );

            // Assert
            Assert.True(result.IsFailure);
            Assert.IsType<AggregateError>(result.Error);
            
            var aggregateError = (AggregateError)result.Error;
            Assert.Equal(3, aggregateError.Errors.Count);
            Assert.All(aggregateError.Errors, e => Assert.IsType<ValidationError>(e));
        }

        [Fact]
        public async Task CompleteUserRegistrationFlow_DuplicateEmail()
        {
            // Arrange
            var repository = new UserRepository();
            var service = new UserService(repository);
            
            // Register first user
            await service.RegisterUserAsync("existing@example.com", "Password123!", "First User");

            // Act - Try to register with same email
            var result = await service.RegisterUserAsync(
                email: "existing@example.com",
                password: "Password456!",
                name: "Second User"
            );

            // Assert
            Assert.True(result.IsFailure);
            Assert.IsType<ConflictError>(result.Error);
            Assert.Contains("already exists", result.Error.Message);
        }

        [Fact]
        public async Task FindUserAndUpdateProfile_Success()
        {
            // Arrange
            var repository = new UserRepository();
            var service = new UserService(repository);
            
            // Register user
            var registerResult = await service.RegisterUserAsync("user@example.com", "Password123!", "Original Name");
            var userId = registerResult.Value.Id;

            // Act - Find and update
            var updateResult = await service.UpdateUserProfileAsync(userId, "Updated Name", "New bio");

            // Assert
            Assert.True(updateResult.IsSuccess);
            Assert.Equal("Updated Name", updateResult.Value.Name);
            Assert.Equal("New bio", updateResult.Value.Bio);
        }

        [Fact]
        public async Task FindUserAndUpdateProfile_UserNotFound()
        {
            // Arrange
            var repository = new UserRepository();
            var service = new UserService(repository);

            // Act - Try to update non-existent user
            var result = await service.UpdateUserProfileAsync(Guid.NewGuid(), "Name", "Bio");

            // Assert
            Assert.True(result.IsFailure);
            Assert.IsType<NotFoundError>(result.Error);
        }

        [Fact]
        public async Task AuthenticateUser_ChainedOperations()
        {
            // Arrange
            var repository = new UserRepository();
            var service = new UserService(repository);
            
            // Register user
            await service.RegisterUserAsync("auth@example.com", "Password123!", "Auth User");

            // Act - Authenticate and get profile
            var result = await service.AuthenticateAndGetProfileAsync("auth@example.com", "Password123!");

            // Assert
            result.OnEither(
                success: profile =>
                {
                    Assert.Equal("auth@example.com", profile.Email);
                    Assert.Equal("Auth User", profile.Name);
                    Assert.True(profile.LastLoginTime > DateTime.UtcNow.AddMinutes(-1));
                },
                failure: error => throw new Exception($"Authentication should have succeeded: {error.Message}")
            );
        }

        [Fact]
        public async Task SearchUsers_WithOptionPattern()
        {
            // Arrange
            var repository = new UserRepository();
            var service = new UserService(repository);
            
            // Add some users
            await service.RegisterUserAsync("alice@example.com", "Password123!", "Alice Smith");
            await service.RegisterUserAsync("bob@example.com", "Password123!", "Bob Smith");
            await service.RegisterUserAsync("charlie@example.com", "Password123!", "Charlie Brown");

            // Act - Search with various criteria
            var searchByEmail = await service.SearchUsersAsync(email: Option.Some("alice@example.com"));
            var searchByName = await service.SearchUsersAsync(name: Option.Some("Smith"));
            var searchAll = await service.SearchUsersAsync();

            // Assert
            Assert.Single(searchByEmail);
            Assert.Equal("alice@example.com", searchByEmail.First().Email);

            Assert.Equal(2, searchByName.Count());
            Assert.All(searchByName, u => Assert.Contains("Smith", u.Name));

            Assert.Equal(3, searchAll.Count());
        }

        // Domain Models
        private class User
        {
            public Guid Id { get; set; }
            public string Email { get; set; } = "";
            public string PasswordHash { get; set; } = "";
            public string Name { get; set; } = "";
            public string Bio { get; set; } = "";
            public DateTime CreatedAt { get; set; }
            public DateTime? LastLoginTime { get; set; }
        }

        private class UserProfile
        {
            public string Email { get; set; } = "";
            public string Name { get; set; } = "";
            public string Bio { get; set; } = "";
            public DateTime? LastLoginTime { get; set; }
        }

        // Repository Implementation
        private class UserRepository
        {
            private readonly Dictionary<Guid, User> _users = new();
            private readonly Dictionary<string, Guid> _emailIndex = new();

            public Task<Option<User>> FindByIdAsync(Guid id)
            {
                return Task.FromResult(
                    _users.TryGetValue(id, out var user) 
                        ? Option.Some(user) 
                        : Option<User>.None
                );
            }

            public Task<Option<User>> FindByEmailAsync(string email)
            {
                return Task.FromResult(
                    _emailIndex.TryGetValue(email.ToLowerInvariant(), out var id) && _users.TryGetValue(id, out var user)
                        ? Option.Some(user)
                        : Option<User>.None
                );
            }

            public Task<Result> SaveAsync(User user)
            {
                try
                {
                    _users[user.Id] = user;
                    _emailIndex[user.Email.ToLowerInvariant()] = user.Id;
                    return Task.FromResult(Result.Success());
                }
                catch (Exception ex)
                {
                    return Task.FromResult(Result.Failure(new Error("SAVE_ERROR", ex.Message)));
                }
            }

            public Task<IEnumerable<User>> SearchAsync(Option<string> email, Option<string> name)
            {
                var query = _users.Values.AsEnumerable();

                query = email.Match(
                    some: e => query.Where(u => u.Email.Contains(e, StringComparison.OrdinalIgnoreCase)),
                    none: () => query
                );

                query = name.Match(
                    some: n => query.Where(u => u.Name.Contains(n, StringComparison.OrdinalIgnoreCase)),
                    none: () => query
                );

                return Task.FromResult(query);
            }
        }

        // Service Layer Implementation
        private class UserService
        {
            private readonly UserRepository _repository;

            public UserService(UserRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result<User>> RegisterUserAsync(string email, string password, string name)
            {
                // Validate inputs
                var validationResult = ValidateRegistrationData(email, password, name);
                if (validationResult.IsFailure)
                    return validationResult.Error;

                // Check if email already exists
                var existingUser = await _repository.FindByEmailAsync(email);
                if (existingUser.IsSome)
                    return new ConflictError("USER_EXISTS", $"User with email '{email}' already exists");

                // Create new user
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    PasswordHash = HashPassword(password),
                    Name = name,
                    CreatedAt = DateTime.UtcNow
                };

                // Save to repository
                var saveResult = await _repository.SaveAsync(user);
                return saveResult.IsSuccess 
                    ? Result.Success(user) 
                    : Result.Failure<User>(saveResult.Error);
            }

            public async Task<Result<User>> UpdateUserProfileAsync(Guid userId, string name, string bio)
            {
                var userOption = await _repository.FindByIdAsync(userId);
                
                return await userOption.Match(
                    some: async user =>
                    {
                        user.Name = name;
                        user.Bio = bio;
                        
                        var saveResult = await _repository.SaveAsync(user);
                        return saveResult.IsSuccess 
                            ? Result.Success(user) 
                            : Result.Failure<User>(saveResult.Error);
                    },
                    none: () => Task.FromResult(Result.Failure<User>(
                        new NotFoundError("USER_NOT_FOUND", $"User with ID '{userId}' was not found")
                    ))
                );
            }

            public async Task<Result<UserProfile>> AuthenticateAndGetProfileAsync(string email, string password)
            {
                var userOption = await _repository.FindByEmailAsync(email);
                var result = await userOption
                    .Match(
                        some: async user =>
                    {
                        // Verify password
                        if (!VerifyPassword(password, user.PasswordHash))
                            return Result.Failure<User>(new ValidationError("AUTH_FAILED", "Invalid email or password"));

                        // Update last login time
                        user.LastLoginTime = DateTime.UtcNow;
                        await _repository.SaveAsync(user);

                        return Result.Success(user);
                    },
                        none: () => Task.FromResult(Result.Failure<User>(
                            new ValidationError("AUTH_FAILED", "Invalid email or password")
                        ))
                    );

                return result.Map(user => new UserProfile
                {
                    Email = user.Email,
                    Name = user.Name,
                    Bio = user.Bio,
                    LastLoginTime = user.LastLoginTime
                });
            }

            public async Task<IEnumerable<User>> SearchUsersAsync(
                Option<string> email = default, 
                Option<string> name = default)
            {
                return await _repository.SearchAsync(email, name);
            }

            private Result ValidateRegistrationData(string email, string password, string name)
            {
                var errorBuilder = new ErrorBuilder();

                if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                    errorBuilder.Append(new ValidationError("INVALID_EMAIL", "Email must be a valid email address"));

                if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                    errorBuilder.Append(new ValidationError("WEAK_PASSWORD", "Password must be at least 8 characters long"));

                if (string.IsNullOrWhiteSpace(name))
                    errorBuilder.Append(new ValidationError("INVALID_NAME", "Name is required"));

                return errorBuilder.HasErrors 
                    ? Result.Failure(errorBuilder.Build()) 
                    : Result.Success();
            }

            private string HashPassword(string password) => 
                Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));

            private bool VerifyPassword(string password, string hash) => 
                HashPassword(password) == hash;
        }
    }
}