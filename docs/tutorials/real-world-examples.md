# Real-World Examples

This tutorial demonstrates how to use FluentUnions in real-world applications through complete, practical examples.

## Table of Contents
1. [E-Commerce Platform](#e-commerce-platform)
2. [Banking System](#banking-system)
3. [User Authentication Service](#user-authentication-service)
4. [File Processing System](#file-processing-system)
5. [API Integration Layer](#api-integration-layer)
6. [Notification System](#notification-system)
7. [Data Validation Pipeline](#data-validation-pipeline)
8. [Microservices Communication](#microservices-communication)

## E-Commerce Platform

### Shopping Cart Service

```csharp
public class ShoppingCartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductService _productService;
    private readonly IPricingService _pricingService;
    private readonly IInventoryService _inventoryService;
    
    public Result<Cart> AddToCart(Guid userId, Guid productId, int quantity)
    {
        return GetOrCreateCart(userId)
            .Bind(cart => ValidateProduct(productId))
            .Bind(product => CheckInventory(product, quantity))
            .Bind(product => CalculatePrice(product, quantity))
            .Bind(price => AddItemToCart(cart, productId, quantity, price))
            .Bind(SaveCart)
            .Tap(cart => PublishCartUpdatedEvent(cart));
    }
    
    private Result<Cart> GetOrCreateCart(Guid userId)
    {
        return _cartRepository.FindByUserId(userId)
            .OrElse(() => CreateNewCart(userId));
    }
    
    private Result<Product> ValidateProduct(Guid productId)
    {
        return _productService.GetProduct(productId)
            .Bind(product => product.IsActive 
                ? Result.Success(product)
                : new ValidationError("Product is not available"));
    }
    
    private Result<Product> CheckInventory(Product product, int quantity)
    {
        return _inventoryService.GetAvailableQuantity(product.Id)
            .Bind(available => available >= quantity
                ? Result.Success(product)
                : new InsufficientInventoryError(product.Id, quantity, available));
    }
    
    public Result<Order> Checkout(Guid userId, CheckoutRequest request)
    {
        return GetCart(userId)
            .Ensure(cart => cart.Items.Any(), "Cart is empty")
            .Bind(cart => ValidateShippingAddress(request.ShippingAddress))
            .Bind(address => ValidatePaymentMethod(request.PaymentMethod))
            .Bind(payment => GetCart(userId)) // Re-fetch for latest state
            .Bind(cart => CalculateTotals(cart, request))
            .Bind(totals => ReserveInventory(cart.Items))
            .Bind(reservations => ProcessPayment(totals, request.PaymentMethod)
                .MapError(error =>
                {
                    ReleaseReservations(reservations);
                    return error;
                }))
            .Bind(payment => CreateOrder(userId, cart, payment, request))
            .Bind(order => ClearCart(userId).Map(_ => order))
            .Tap(order => SendOrderConfirmation(order));
    }
}

// Domain-specific errors
public class InsufficientInventoryError : Error
{
    public Guid ProductId { get; }
    public int Requested { get; }
    public int Available { get; }
    
    public InsufficientInventoryError(Guid productId, int requested, int available)
        : base("INSUFFICIENT_INVENTORY", 
               $"Requested {requested} items but only {available} available")
    {
        ProductId = productId;
        Requested = requested;
        Available = available;
    }
}
```

### Order Processing Workflow

```csharp
public class OrderProcessingWorkflow
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentGateway _paymentGateway;
    private readonly IShippingService _shippingService;
    private readonly INotificationService _notificationService;
    
    public Result<ProcessedOrder> ProcessOrder(Guid orderId)
    {
        return GetOrder(orderId)
            .Bind(ValidateOrderStatus)
            .Bind(ProcessPayment)
            .Bind(CreateShipment)
            .Bind(UpdateOrderStatus)
            .Bind(NotifyCustomer)
            .Map(ToProcessedOrder);
    }
    
    private Result<Order> ValidateOrderStatus(Order order)
    {
        return order.Status switch
        {
            OrderStatus.Pending => Result.Success(order),
            OrderStatus.Cancelled => new InvalidOrderStateError(
                "Cannot process cancelled order"),
            OrderStatus.Completed => new InvalidOrderStateError(
                "Order already processed"),
            _ => new Error("INVALID_STATUS", $"Unknown status: {order.Status}")
        };
    }
    
    private Result<(Order order, PaymentResult payment)> ProcessPayment(Order order)
    {
        return order.PaymentStatus switch
        {
            PaymentStatus.Paid => Result.Success((order, new PaymentResult())),
            PaymentStatus.Pending => ChargePayment(order),
            PaymentStatus.Failed => RetryPayment(order),
            _ => Result.Failure<(Order, PaymentResult)>("Invalid payment status")
        };
    }
    
    private Result<(Order order, PaymentResult payment)> ChargePayment(Order order)
    {
        return _paymentGateway.Charge(new ChargeRequest
        {
            Amount = order.Total,
            Currency = order.Currency,
            CustomerId = order.CustomerId,
            OrderId = order.Id
        })
        .Map(payment => (order, payment))
        .TapError(error => LogPaymentFailure(order, error));
    }
    
    private Result<(Order order, ShipmentDetails shipment)> CreateShipment(
        (Order order, PaymentResult payment) data)
    {
        var shippingRequest = new ShippingRequest
        {
            OrderId = data.order.Id,
            Items = data.order.Items.Select(MapToShippingItem).ToList(),
            Address = data.order.ShippingAddress,
            ShippingMethod = data.order.ShippingMethod
        };
        
        return _shippingService.CreateShipment(shippingRequest)
            .Map(shipment => (data.order, shipment));
    }
}
```

## Banking System

### Money Transfer Service

```csharp
public class MoneyTransferService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IFraudDetectionService _fraudDetection;
    private readonly INotificationService _notifications;
    
    public Result<TransferResult> Transfer(TransferRequest request)
    {
        return ValidateTransferRequest(request)
            .Bind(ValidateTransferLimits)
            .Bind(LoadAccounts)
            .Bind(ValidateAccounts)
            .Bind(CheckFraud)
            .Bind(ExecuteTransfer)
            .Bind(RecordTransaction)
            .Tap(SendNotifications);
    }
    
    private Result<TransferRequest> ValidateTransferRequest(TransferRequest request)
    {
        var errors = new List<Error>();
        
        if (request.Amount <= 0)
            errors.Add(new ValidationError("Amount must be positive"));
            
        if (request.FromAccountId == request.ToAccountId)
            errors.Add(new ValidationError("Cannot transfer to same account"));
            
        if (string.IsNullOrWhiteSpace(request.Reference))
            errors.Add(new ValidationError("Reference is required"));
            
        return errors.Any()
            ? Result.Failure<TransferRequest>(new AggregateError(errors))
            : Result.Success(request);
    }
    
    private Result<TransferContext> LoadAccounts(TransferRequest request)
    {
        return Result.BindAll(
            _accountRepository.GetAccount(request.FromAccountId),
            _accountRepository.GetAccount(request.ToAccountId)
        )
        .Map((from, to) => new TransferContext
        {
            Request = request,
            FromAccount = from,
            ToAccount = to
        });
    }
    
    private Result<TransferContext> ValidateAccounts(TransferContext context)
    {
        return ValidateSourceAccount(context.FromAccount, context.Request.Amount)
            .Bind(() => ValidateDestinationAccount(context.ToAccount))
            .Map(() => context);
    }
    
    private Result<Account> ValidateSourceAccount(Account account, decimal amount)
    {
        if (!account.IsActive)
            return new AccountInactiveError(account.Number);
            
        if (account.IsFrozen)
            return new AccountFrozenError(account.Number, account.FreezeReason);
            
        if (account.Balance < amount)
            return new InsufficientFundsError(account.Number, account.Balance, amount);
            
        return Result.Success(account);
    }
    
    private Result<TransferContext> ExecuteTransfer(TransferContext context)
    {
        using var transaction = _accountRepository.BeginTransaction();
        
        return DebitAccount(context.FromAccount, context.Request.Amount)
            .Bind(fromAccount => CreditAccount(context.ToAccount, context.Request.Amount)
                .Map(toAccount => context with 
                { 
                    FromAccount = fromAccount, 
                    ToAccount = toAccount 
                }))
            .Tap(_ => transaction.Commit())
            .TapError(_ => transaction.Rollback());
    }
    
    private Result<TransferResult> RecordTransaction(TransferContext context)
    {
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Type = TransactionType.Transfer,
            FromAccountId = context.FromAccount.Id,
            ToAccountId = context.ToAccount.Id,
            Amount = context.Request.Amount,
            Reference = context.Request.Reference,
            Status = TransactionStatus.Completed,
            CreatedAt = DateTime.UtcNow
        };
        
        return _transactionRepository.Save(transaction)
            .Map(_ => new TransferResult
            {
                TransactionId = transaction.Id,
                FromBalance = context.FromAccount.Balance,
                ToBalance = context.ToAccount.Balance,
                CompletedAt = transaction.CreatedAt
            });
    }
}

// Domain models
public record TransferContext
{
    public TransferRequest Request { get; init; }
    public Account FromAccount { get; init; }
    public Account ToAccount { get; init; }
}

public class AccountFrozenError : Error
{
    public string AccountNumber { get; }
    public string Reason { get; }
    
    public AccountFrozenError(string accountNumber, string reason)
        : base("ACCOUNT_FROZEN", $"Account {accountNumber} is frozen: {reason}")
    {
        AccountNumber = accountNumber;
        Reason = reason;
    }
}
```

## User Authentication Service

### Complete Authentication Flow

```csharp
public class AuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITwoFactorService _twoFactorService;
    private readonly ITokenService _tokenService;
    private readonly ILoginHistoryService _loginHistory;
    private readonly IRateLimiter _rateLimiter;
    
    public Result<AuthenticationResult> Authenticate(LoginRequest request)
    {
        return CheckRateLimit(request.IpAddress)
            .Bind(() => ValidateLoginRequest(request))
            .Bind(FindUserByIdentifier)
            .Bind(user => VerifyPassword(user, request.Password))
            .Bind(CheckAccountStatus)
            .Bind(user => VerifyTwoFactorIfRequired(user, request))
            .Bind(GenerateTokens)
            .Bind(RecordSuccessfulLogin)
            .TapError(error => RecordFailedLogin(request, error));
    }
    
    private Result<LoginRequest> CheckRateLimit(string ipAddress)
    {
        return _rateLimiter.CheckLimit(ipAddress, "login", maxAttempts: 5, window: TimeSpan.FromMinutes(15))
            ? Result.Success()
            : new TooManyAttemptsError("Too many login attempts. Please try again later.");
    }
    
    private Result<User> FindUserByIdentifier(LoginRequest request)
    {
        Option<User> user = request.Identifier.Contains('@')
            ? _userRepository.FindByEmail(request.Identifier)
            : _userRepository.FindByUsername(request.Identifier);
            
        return user.ToResult(() => new AuthenticationError("Invalid credentials"));
    }
    
    private Result<User> VerifyPassword(User user, string password)
    {
        if (!_passwordHasher.Verify(password, user.PasswordHash))
        {
            RecordFailedPasswordAttempt(user);
            return new AuthenticationError("Invalid credentials");
        }
        
        return ShouldUpdatePasswordHash(user.PasswordHash)
            ? UpdatePasswordHash(user, password)
            : Result.Success(user);
    }
    
    private Result<User> CheckAccountStatus(User user)
    {
        if (user.IsLocked)
        {
            return new AccountLockedError(
                $"Account locked due to: {user.LockReason}",
                user.LockedUntil);
        }
        
        if (!user.IsActive)
            return new AccountInactiveError("Account is deactivated");
            
        if (!user.EmailVerified)
            return new EmailNotVerifiedError(user.Email);
            
        return Result.Success(user);
    }
    
    private Result<User> VerifyTwoFactorIfRequired(User user, LoginRequest request)
    {
        if (!user.TwoFactorEnabled)
            return Result.Success(user);
            
        if (string.IsNullOrEmpty(request.TwoFactorCode))
            return new TwoFactorRequiredError();
            
        return _twoFactorService.VerifyCode(user.Id, request.TwoFactorCode)
            .Map(_ => user);
    }
    
    private Result<AuthenticationTokens> GenerateTokens(User user)
    {
        return Result.BindAll(
            _tokenService.GenerateAccessToken(user),
            _tokenService.GenerateRefreshToken(user)
        )
        .Map((access, refresh) => new AuthenticationTokens
        {
            AccessToken = access,
            RefreshToken = refresh,
            ExpiresIn = 3600,
            TokenType = "Bearer"
        });
    }
    
    private Result<AuthenticationResult> RecordSuccessfulLogin(AuthenticationTokens tokens)
    {
        var loginRecord = new LoginHistory
        {
            UserId = tokens.UserId,
            IpAddress = request.IpAddress,
            UserAgent = request.UserAgent,
            LoginTime = DateTime.UtcNow,
            Success = true
        };
        
        return _loginHistory.Record(loginRecord)
            .Map(_ => new AuthenticationResult
            {
                Tokens = tokens,
                User = new UserInfo(user),
                RequiresPasswordChange = user.RequiresPasswordChange,
                RequiresTwoFactorSetup = !user.TwoFactorEnabled && user.ShouldEnforceTwoFactor
            });
    }
    
    public Result<Unit> Logout(LogoutRequest request)
    {
        return _tokenService.ValidateToken(request.AccessToken)
            .Bind(claims => _tokenService.RevokeToken(request.AccessToken))
            .Bind(() => _tokenService.RevokeRefreshToken(request.RefreshToken))
            .Tap(() => PublishUserLoggedOutEvent(claims.UserId));
    }
}

// Custom authentication errors
public class TwoFactorRequiredError : Error
{
    public TwoFactorRequiredError() 
        : base("2FA_REQUIRED", "Two-factor authentication code required") { }
}

public class AccountLockedError : Error
{
    public DateTime? LockedUntil { get; }
    
    public AccountLockedError(string message, DateTime? lockedUntil)
        : base("ACCOUNT_LOCKED", message)
    {
        LockedUntil = lockedUntil;
    }
}
```

## File Processing System

### CSV Import Service

```csharp
public class CsvImportService<T> where T : class, new()
{
    private readonly ICsvParser _parser;
    private readonly IValidator<T> _validator;
    private readonly IRepository<T> _repository;
    private readonly IImportHistoryService _historyService;
    
    public Result<ImportResult> ImportFile(string filePath, ImportOptions options)
    {
        var importId = Guid.NewGuid();
        
        return ValidateFile(filePath)
            .Bind(file => ReadFileContent(file))
            .Bind(content => ParseCsv(content, options))
            .Bind(records => ValidateRecords(records, options))
            .Bind(validRecords => ProcessRecords(validRecords, options))
            .Bind(processed => SaveImportHistory(importId, filePath, processed))
            .TapError(error => LogImportFailure(importId, filePath, error));
    }
    
    private Result<FileInfo> ValidateFile(string filePath)
    {
        if (!File.Exists(filePath))
            return new NotFoundError($"File not found: {filePath}");
            
        var fileInfo = new FileInfo(filePath);
        
        if (fileInfo.Extension.ToLower() != ".csv")
            return new ValidationError("File must be a CSV file");
            
        if (fileInfo.Length == 0)
            return new ValidationError("File is empty");
            
        if (fileInfo.Length > 100 * 1024 * 1024) // 100MB limit
            return new ValidationError("File size exceeds 100MB limit");
            
        return Result.Success(fileInfo);
    }
    
    private Result<List<T>> ParseCsv(string content, ImportOptions options)
    {
        return Result.Try(() => _parser.Parse<T>(content, new CsvConfiguration
        {
            HasHeader = options.HasHeader,
            Delimiter = options.Delimiter,
            DateFormat = options.DateFormat,
            IgnoreEmptyLines = true
        }))
        .MapError(ex => new Error("PARSE_ERROR", $"Failed to parse CSV: {ex.Message}"));
    }
    
    private Result<List<ValidatedRecord<T>>> ValidateRecords(
        List<T> records, ImportOptions options)
    {
        var validatedRecords = new List<ValidatedRecord<T>>();
        var errors = new List<Error>();
        
        for (int i = 0; i < records.Count; i++)
        {
            var record = records[i];
            var rowNumber = options.HasHeader ? i + 2 : i + 1;
            
            _validator.Validate(record)
                .Match(
                    onSuccess: () => validatedRecords.Add(new ValidatedRecord<T>
                    {
                        Record = record,
                        RowNumber = rowNumber
                    }),
                    onFailure: error => errors.Add(error
                        .WithMetadata("row", rowNumber)
                        .WithMetadata("record", JsonSerializer.Serialize(record)))
                );
                
            if (errors.Count >= options.MaxErrors)
            {
                errors.Add(new Error("TOO_MANY_ERRORS", 
                    $"Import aborted: more than {options.MaxErrors} errors found"));
                break;
            }
        }
        
        if (errors.Any() && !options.ContinueOnError)
            return Result.Failure<List<ValidatedRecord<T>>>(new AggregateError(errors));
            
        return Result.Success(validatedRecords);
    }
    
    private Result<ProcessedRecords<T>> ProcessRecords(
        List<ValidatedRecord<T>> records, ImportOptions options)
    {
        var processed = new ProcessedRecords<T>();
        
        using var transaction = options.UseTransaction 
            ? _repository.BeginTransaction() 
            : null;
            
        try
        {
            foreach (var batch in records.Chunk(options.BatchSize))
            {
                ProcessBatch(batch, options, processed);
                
                if (processed.Failed.Count > options.MaxErrors)
                {
                    transaction?.Rollback();
                    return new Error("PROCESSING_FAILED", 
                        "Too many failures during processing");
                }
            }
            
            transaction?.Commit();
            return Result.Success(processed);
        }
        catch (Exception ex)
        {
            transaction?.Rollback();
            return Result.Failure<ProcessedRecords<T>>(
                new Error("PROCESSING_ERROR", ex.Message));
        }
    }
    
    private void ProcessBatch(
        ValidatedRecord<T>[] batch, 
        ImportOptions options, 
        ProcessedRecords<T> processed)
    {
        foreach (var record in batch)
        {
            var result = options.UpdateExisting
                ? UpsertRecord(record.Record)
                : InsertRecord(record.Record);
                
            result.Match(
                onSuccess: () => processed.Successful.Add(record),
                onFailure: error => processed.Failed.Add((record, error))
            );
        }
    }
}

// Supporting types
public class ImportOptions
{
    public bool HasHeader { get; set; } = true;
    public string Delimiter { get; set; } = ",";
    public string DateFormat { get; set; } = "yyyy-MM-dd";
    public bool ContinueOnError { get; set; } = false;
    public int MaxErrors { get; set; } = 100;
    public int BatchSize { get; set; } = 1000;
    public bool UseTransaction { get; set; } = true;
    public bool UpdateExisting { get; set; } = false;
}

public class ProcessedRecords<T>
{
    public List<ValidatedRecord<T>> Successful { get; } = new();
    public List<(ValidatedRecord<T> Record, Error Error)> Failed { get; } = new();
    
    public ImportResult ToImportResult()
    {
        return new ImportResult
        {
            TotalRecords = Successful.Count + Failed.Count,
            SuccessfulRecords = Successful.Count,
            FailedRecords = Failed.Count,
            Errors = Failed.Select(f => f.Error).ToList()
        };
    }
}
```

## API Integration Layer

### Third-Party API Client

```csharp
public class PaymentGatewayClient
{
    private readonly HttpClient _httpClient;
    private readonly IApiAuthenticator _authenticator;
    private readonly ICircuitBreaker _circuitBreaker;
    private readonly IRetryPolicy _retryPolicy;
    
    public Result<PaymentResponse> ProcessPayment(PaymentRequest request)
    {
        return _circuitBreaker.Execute(() =>
            AuthenticateRequest()
                .Bind(token => BuildHttpRequest(request, token))
                .Bind(SendRequest)
                .Bind(ValidateResponse)
                .Bind(DeserializeResponse)
                .Bind(MapToPaymentResponse)
        );
    }
    
    private Result<string> AuthenticateRequest()
    {
        return _authenticator.GetToken()
            .OrElse(() => _authenticator.RefreshToken())
            .MapError(error => new AuthenticationError(
                "Failed to authenticate with payment gateway"));
    }
    
    private Result<HttpRequestMessage> BuildHttpRequest(
        PaymentRequest request, string token)
    {
        return Result.Try(() =>
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/payments");
            httpRequest.Headers.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
            httpRequest.Content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");
            return httpRequest;
        });
    }
    
    private Result<HttpResponseMessage> SendRequest(HttpRequestMessage request)
    {
        return _retryPolicy.ExecuteAsync(async () =>
        {
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode
                ? Result.Success(response)
                : HandleErrorResponse(response);
        });
    }
    
    private Result<HttpResponseMessage> HandleErrorResponse(HttpResponseMessage response)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => ParseValidationError(response),
            HttpStatusCode.Unauthorized => new AuthenticationError("Invalid API credentials"),
            HttpStatusCode.PaymentRequired => ParsePaymentError(response),
            HttpStatusCode.TooManyRequests => new RateLimitError(GetRetryAfter(response)),
            HttpStatusCode.ServiceUnavailable => new ServiceUnavailableError(),
            _ => new Error($"API_ERROR_{(int)response.StatusCode}", 
                           $"Unexpected response: {response.ReasonPhrase}")
        };
    }
    
    private Result<ApiPaymentResponse> DeserializeResponse(HttpResponseMessage response)
    {
        return Result.TryAsync(async () =>
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiPaymentResponse>(content)
                ?? throw new InvalidOperationException("Empty response body");
        })
        .MapError(ex => new Error("DESERIALIZATION_ERROR", 
                                  $"Failed to parse response: {ex.Message}"));
    }
    
    private Result<PaymentResponse> MapToPaymentResponse(ApiPaymentResponse apiResponse)
    {
        return apiResponse.Status switch
        {
            "approved" => Result.Success(new PaymentResponse
            {
                TransactionId = apiResponse.Id,
                Status = PaymentStatus.Approved,
                Amount = apiResponse.Amount,
                ProcessedAt = apiResponse.Timestamp
            }),
            "declined" => new PaymentDeclinedError(apiResponse.DeclineReason),
            "pending" => Result.Success(new PaymentResponse
            {
                TransactionId = apiResponse.Id,
                Status = PaymentStatus.Pending,
                Amount = apiResponse.Amount
            }),
            _ => new Error("UNKNOWN_STATUS", $"Unknown payment status: {apiResponse.Status}")
        };
    }
}

// Rate limiting with circuit breaker
public class CircuitBreaker : ICircuitBreaker
{
    private readonly object _lock = new();
    private CircuitState _state = CircuitState.Closed;
    private int _failureCount;
    private DateTime _lastFailureTime;
    
    private readonly int _failureThreshold;
    private readonly TimeSpan _openDuration;
    
    public Result<T> Execute<T>(Func<Result<T>> operation)
    {
        lock (_lock)
        {
            if (_state == CircuitState.Open && !ShouldAttemptReset())
                return new ServiceUnavailableError("Service temporarily unavailable");
                
            if (_state == CircuitState.Open)
                _state = CircuitState.HalfOpen;
        }
        
        var result = operation();
        
        lock (_lock)
        {
            if (result.IsSuccess)
            {
                OnSuccess();
            }
            else
            {
                OnFailure();
            }
        }
        
        return result;
    }
    
    private void OnSuccess()
    {
        _failureCount = 0;
        _state = CircuitState.Closed;
    }
    
    private void OnFailure()
    {
        _failureCount++;
        _lastFailureTime = DateTime.UtcNow;
        
        if (_failureCount >= _failureThreshold)
            _state = CircuitState.Open;
    }
    
    private bool ShouldAttemptReset()
    {
        return DateTime.UtcNow - _lastFailureTime >= _openDuration;
    }
}
```

## Notification System

### Multi-Channel Notification Service

```csharp
public class NotificationService
{
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;
    private readonly IPushNotificationService _pushService;
    private readonly INotificationRepository _repository;
    private readonly IUserPreferencesService _preferencesService;
    
    public Result<NotificationResult> SendNotification(NotificationRequest request)
    {
        return ValidateRequest(request)
            .Bind(GetUserPreferences)
            .Bind(DetermineChannels)
            .Bind(SendViaChannels)
            .Bind(RecordNotification);
    }
    
    private Result<NotificationContext> GetUserPreferences(NotificationRequest request)
    {
        return _preferencesService.GetPreferences(request.UserId)
            .Map(prefs => new NotificationContext
            {
                Request = request,
                Preferences = prefs
            });
    }
    
    private Result<NotificationContext> DetermineChannels(NotificationContext context)
    {
        var channels = new List<NotificationChannel>();
        
        if (context.Request.ForceChannels?.Any() == true)
        {
            channels.AddRange(context.Request.ForceChannels);
        }
        else
        {
            if (context.Preferences.EmailEnabled && 
                context.Request.Type.SupportsEmail())
                channels.Add(NotificationChannel.Email);
                
            if (context.Preferences.SmsEnabled && 
                context.Request.Type.SupportsSms() &&
                context.Request.Priority >= Priority.High)
                channels.Add(NotificationChannel.Sms);
                
            if (context.Preferences.PushEnabled && 
                context.Request.Type.SupportsPush())
                channels.Add(NotificationChannel.Push);
        }
        
        if (!channels.Any())
            return new Error("NO_CHANNELS", "No notification channels available");
            
        context.Channels = channels;
        return Result.Success(context);
    }
    
    private Result<NotificationContext> SendViaChannels(NotificationContext context)
    {
        var results = new List<ChannelResult>();
        
        foreach (var channel in context.Channels)
        {
            var result = channel switch
            {
                NotificationChannel.Email => SendEmail(context),
                NotificationChannel.Sms => SendSms(context),
                NotificationChannel.Push => SendPush(context),
                _ => Result.Failure<string>("Unknown channel")
            };
            
            results.Add(new ChannelResult
            {
                Channel = channel,
                Success = result.IsSuccess,
                Error = result.IsFailure ? result.Error : null,
                MessageId = result.IsSuccess ? result.Value : null
            });
        }
        
        context.Results = results;
        
        // Fail if all channels failed
        if (results.All(r => !r.Success))
            return new AggregateError(results
                .Where(r => r.Error != null)
                .Select(r => r.Error!));
                
        return Result.Success(context);
    }
    
    private Result<string> SendEmail(NotificationContext context)
    {
        var template = GetEmailTemplate(context.Request.Type);
        
        return template
            .Bind(tmpl => RenderTemplate(tmpl, context.Request.Data))
            .Bind(content => _emailService.Send(new EmailMessage
            {
                To = context.Preferences.Email,
                Subject = context.Request.Subject ?? content.Subject,
                Body = content.Body,
                IsHtml = true,
                Priority = MapPriority(context.Request.Priority)
            }));
    }
    
    private Result<NotificationResult> RecordNotification(NotificationContext context)
    {
        var notification = new NotificationRecord
        {
            Id = Guid.NewGuid(),
            UserId = context.Request.UserId,
            Type = context.Request.Type,
            Subject = context.Request.Subject,
            Channels = context.Results
                .Where(r => r.Success)
                .Select(r => r.Channel)
                .ToList(),
            Status = context.Results.Any(r => r.Success) 
                ? NotificationStatus.Sent 
                : NotificationStatus.Failed,
            SentAt = DateTime.UtcNow,
            ChannelResults = context.Results
        };
        
        return _repository.Save(notification)
            .Map(_ => new NotificationResult
            {
                NotificationId = notification.Id,
                SuccessfulChannels = notification.Channels,
                FailedChannels = context.Results
                    .Where(r => !r.Success)
                    .Select(r => r.Channel)
                    .ToList()
            });
    }
}

// Retry with exponential backoff
public class RetryPolicy : IRetryPolicy
{
    public async Task<Result<T>> ExecuteAsync<T>(
        Func<Task<Result<T>>> operation,
        int maxAttempts = 3,
        int baseDelayMs = 1000)
    {
        var attempts = 0;
        var errors = new List<Error>();
        
        while (attempts < maxAttempts)
        {
            attempts++;
            var result = await operation();
            
            if (result.IsSuccess)
                return result;
                
            errors.Add(result.Error
                .WithMetadata("attempt", attempts)
                .WithMetadata("timestamp", DateTime.UtcNow));
            
            if (attempts < maxAttempts && IsRetryable(result.Error))
            {
                var delay = baseDelayMs * Math.Pow(2, attempts - 1);
                await Task.Delay(TimeSpan.FromMilliseconds(delay));
            }
            else
            {
                break;
            }
        }
        
        return Result.Failure<T>(new AggregateError(errors)
            .WithMetadata("totalAttempts", attempts)
            .WithMetadata("retryPolicy", "exponential"));
    }
    
    private bool IsRetryable(Error error)
    {
        return error is not (
            ValidationError or 
            AuthenticationError or 
            AuthorizationError or
            NotFoundError);
    }
}
```

## Data Validation Pipeline

### Complex Validation Rules

```csharp
public class CustomerValidationService
{
    private readonly IAddressValidator _addressValidator;
    private readonly ICreditCheckService _creditCheck;
    private readonly IBlacklistService _blacklist;
    
    public Result<ValidatedCustomer> ValidateNewCustomer(CustomerRegistration registration)
    {
        return RunBasicValidation(registration)
            .Bind(RunBusinessRules)
            .Bind(RunExternalValidations)
            .Bind(RunRiskAssessment)
            .Map(ToValidatedCustomer);
    }
    
    private Result<CustomerData> RunBasicValidation(CustomerRegistration registration)
    {
        var validators = new List<IValidator<CustomerRegistration>>
        {
            new RequiredFieldsValidator(),
            new EmailValidator(),
            new PhoneNumberValidator(),
            new DateOfBirthValidator(),
            new TaxIdValidator()
        };
        
        var errors = validators
            .SelectMany(v => v.Validate(registration))
            .ToList();
            
        return errors.Any()
            ? Result.Failure<CustomerData>(new AggregateError(errors))
            : Result.Success(new CustomerData(registration));
    }
    
    private Result<CustomerData> RunBusinessRules(CustomerData data)
    {
        var rules = new List<Func<CustomerData, Result>>
        {
            d => d.Age >= 18 
                ? Result.Success() 
                : new ValidationError("Must be 18 or older"),
                
            d => d.Country.IsServicedCountry() 
                ? Result.Success() 
                : new ValidationError($"Service not available in {d.Country}"),
                
            d => !d.Email.IsDisposableEmail() 
                ? Result.Success() 
                : new ValidationError("Disposable email addresses not allowed"),
                
            d => d.Income >= GetMinimumIncomeForCountry(d.Country)
                ? Result.Success()
                : new ValidationError("Income below minimum requirement")
        };
        
        var errors = rules
            .Select(rule => rule(data))
            .Where(result => result.IsFailure)
            .Select(result => result.Error)
            .ToList();
            
        return errors.Any()
            ? Result.Failure<CustomerData>(new AggregateError(errors))
            : Result.Success(data);
    }
    
    private Result<CustomerData> RunExternalValidations(CustomerData data)
    {
        return ValidateAddress(data.Address)
            .Bind(() => CheckBlacklist(data))
            .Bind(() => VerifyIdentity(data))
            .Map(() => data);
    }
    
    private Result<RiskAssessment> RunRiskAssessment(CustomerData data)
    {
        return _creditCheck.GetCreditScore(data.TaxId)
            .Bind(score => CalculateRiskLevel(data, score))
            .Bind(risk => ApplyRiskBasedRules(data, risk));
    }
}

// Fluent validation builder
public class ValidationBuilder<T>
{
    private readonly List<Func<T, Result>> _rules = new();
    
    public ValidationBuilder<T> Must(
        Func<T, bool> predicate, 
        string errorMessage)
    {
        _rules.Add(item => predicate(item) 
            ? Result.Success() 
            : new ValidationError(errorMessage));
        return this;
    }
    
    public ValidationBuilder<T> MustAsync(
        Func<T, Task<bool>> predicate, 
        string errorMessage)
    {
        _rules.Add(item => Result.TryAsync(async () =>
        {
            var isValid = await predicate(item);
            return isValid 
                ? Result.Success() 
                : new ValidationError(errorMessage);
        }).Flatten());
        return this;
    }
    
    public Result Validate(T item)
    {
        var errors = _rules
            .Select(rule => rule(item))
            .Where(result => result.IsFailure)
            .Select(result => result.Error)
            .ToList();
            
        return errors.Any() 
            ? Result.Failure(new AggregateError(errors))
            : Result.Success();
    }
}
```

## Microservices Communication

### Saga Pattern Implementation

```csharp
public class OrderSaga
{
    private readonly IOrderService _orderService;
    private readonly IInventoryService _inventoryService;
    private readonly IPaymentService _paymentService;
    private readonly IShippingService _shippingService;
    private readonly ISagaCoordinator _coordinator;
    
    public async Task<Result<OrderResult>> ExecuteOrderSaga(OrderRequest request)
    {
        var sagaId = Guid.NewGuid();
        var context = new SagaContext<OrderState>
        {
            SagaId = sagaId,
            State = new OrderState { Request = request }
        };
        
        return await _coordinator.Execute(context, saga => saga
            .Step("CreateOrder", CreateOrder, CompensateOrder)
            .Step("ReserveInventory", ReserveInventory, ReleaseInventory)
            .Step("ProcessPayment", ProcessPayment, RefundPayment)
            .Step("CreateShipment", CreateShipment, CancelShipment)
            .Step("SendNotification", SendNotification)
        );
    }
    
    private async Task<Result<OrderState>> CreateOrder(OrderState state)
    {
        var result = await _orderService.CreateOrder(new CreateOrderCommand
        {
            CustomerId = state.Request.CustomerId,
            Items = state.Request.Items,
            ShippingAddress = state.Request.ShippingAddress
        });
        
        return result.Map(order =>
        {
            state.OrderId = order.Id;
            state.OrderTotal = order.Total;
            return state;
        });
    }
    
    private async Task<Result> CompensateOrder(OrderState state)
    {
        if (state.OrderId.HasValue)
        {
            return await _orderService.CancelOrder(state.OrderId.Value);
        }
        return Result.Success();
    }
    
    private async Task<Result<OrderState>> ReserveInventory(OrderState state)
    {
        var reservations = new List<InventoryReservation>();
        
        foreach (var item in state.Request.Items)
        {
            var result = await _inventoryService.Reserve(new ReservationRequest
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                OrderId = state.OrderId!.Value
            });
            
            if (result.IsFailure)
                return Result.Failure<OrderState>(result.Error);
                
            reservations.Add(result.Value);
        }
        
        state.Reservations = reservations;
        return Result.Success(state);
    }
    
    private async Task<Result> ReleaseInventory(OrderState state)
    {
        if (state.Reservations?.Any() == true)
        {
            var tasks = state.Reservations
                .Select(r => _inventoryService.Release(r.ReservationId))
                .ToList();
                
            var results = await Task.WhenAll(tasks);
            var failures = results.Where(r => r.IsFailure).ToList();
            
            return failures.Any()
                ? Result.Failure(new AggregateError(failures.Select(f => f.Error)))
                : Result.Success();
        }
        return Result.Success();
    }
}

// Saga coordinator
public class SagaCoordinator : ISagaCoordinator
{
    private readonly ILogger _logger;
    private readonly ISagaRepository _repository;
    
    public async Task<Result<TState>> Execute<TState>(
        SagaContext<TState> context,
        Func<ISagaBuilder<TState>, ISagaBuilder<TState>> configure)
    {
        var builder = new SagaBuilder<TState>();
        var saga = configure(builder).Build();
        
        await _repository.SaveContext(context);
        
        foreach (var step in saga.Steps)
        {
            _logger.LogInfo($"Executing saga step: {step.Name}");
            
            var result = await ExecuteStep(step, context.State);
            
            if (result.IsSuccess)
            {
                context.CompletedSteps.Add(step.Name);
                context.State = result.Value;
                await _repository.SaveContext(context);
            }
            else
            {
                _logger.LogError($"Saga step {step.Name} failed: {result.Error}");
                await Compensate(context, saga.Steps);
                return Result.Failure<TState>(result.Error);
            }
        }
        
        return Result.Success(context.State);
    }
    
    private async Task Compensate<TState>(
        SagaContext<TState> context,
        List<SagaStep<TState>> steps)
    {
        var stepsToCompensate = steps
            .Where(s => context.CompletedSteps.Contains(s.Name) && s.Compensate != null)
            .Reverse();
            
        foreach (var step in stepsToCompensate)
        {
            _logger.LogInfo($"Compensating saga step: {step.Name}");
            
            try
            {
                await step.Compensate!(context.State);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Compensation failed for {step.Name}: {ex.Message}");
            }
        }
    }
}
```

## Summary

These real-world examples demonstrate:

1. **Complex Business Logic** - E-commerce, banking, and authentication flows
2. **Error Handling** - Domain-specific errors and recovery strategies
3. **Integration Patterns** - API clients, file processing, and data validation
4. **Distributed Systems** - Microservices communication and saga patterns
5. **Performance Considerations** - Batching, retries, and circuit breakers

Key takeaways:
- Use Result pattern for all operations that can fail
- Create rich domain errors with context
- Compose small functions into complex workflows
- Handle both synchronous and asynchronous operations
- Implement proper compensation and rollback strategies

Next steps:
- [Functional Operations Reference](../guides/functional-operations.md)
- [Testing Guide](../guides/testing-guide.md)
- [Advanced Patterns](../patterns/service-patterns.md)