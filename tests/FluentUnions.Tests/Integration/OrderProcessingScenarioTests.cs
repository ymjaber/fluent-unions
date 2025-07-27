namespace FluentUnions.Tests.Integration
{
    /// <summary>
    /// Integration tests demonstrating real-world usage of FluentUnions in an e-commerce order processing scenario.
    /// </summary>
    public class OrderProcessingScenarioTests
    {
        [Fact]
        public async Task ProcessOrder_CompleteFlow_Success()
        {
            // Arrange
            var inventoryService = new InventoryService();
            var paymentService = new PaymentService();
            var shippingService = new ShippingService();
            var orderService = new OrderService(inventoryService, paymentService, shippingService);

            var orderRequest = new OrderRequest
            {
                CustomerId = Guid.NewGuid(),
                Items = new[]
                {
                    new OrderItem { ProductId = "LAPTOP-001", Quantity = 1, Price = 999.99m },
                    new OrderItem { ProductId = "MOUSE-001", Quantity = 2, Price = 29.99m }
                },
                PaymentMethod = new CreditCardPayment { CardNumber = "4111111111111111", CVV = "123" },
                ShippingAddress = new Address { Street = "123 Main St", City = "Anytown", ZipCode = "12345" }
            };

            // Act
            var result = await orderService.ProcessOrderAsync(orderRequest);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value.OrderId);
            Assert.Equal(OrderStatus.Confirmed, result.Value.Status);
            Assert.NotNull(result.Value.TrackingNumber);
        }

        [Fact]
        public async Task ProcessOrder_InsufficientInventory_Failure()
        {
            // Arrange
            var inventoryService = new InventoryService();
            var paymentService = new PaymentService();
            var shippingService = new ShippingService();
            var orderService = new OrderService(inventoryService, paymentService, shippingService);

            var orderRequest = new OrderRequest
            {
                CustomerId = Guid.NewGuid(),
                Items = new[]
                {
                    new OrderItem { ProductId = "OUT-OF-STOCK", Quantity = 1, Price = 100m }
                },
                PaymentMethod = new CreditCardPayment { CardNumber = "4111111111111111", CVV = "123" },
                ShippingAddress = new Address { Street = "123 Main St", City = "Anytown", ZipCode = "12345" }
            };

            // Act
            var result = await orderService.ProcessOrderAsync(orderRequest);

            // Assert
            Assert.True(result.IsFailure);
            Assert.IsType<ValidationError>(result.Error);
            Assert.Contains("OUT-OF-STOCK", result.Error.Message);
        }

        [Fact]
        public async Task ProcessOrder_PaymentDeclined_Failure()
        {
            // Arrange
            var inventoryService = new InventoryService();
            var paymentService = new PaymentService();
            var shippingService = new ShippingService();
            var orderService = new OrderService(inventoryService, paymentService, shippingService);

            var orderRequest = new OrderRequest
            {
                CustomerId = Guid.NewGuid(),
                Items = new[]
                {
                    new OrderItem { ProductId = "LAPTOP-001", Quantity = 1, Price = 999.99m }
                },
                PaymentMethod = new CreditCardPayment { CardNumber = "4000000000000002", CVV = "123" }, // Decline card
                ShippingAddress = new Address { Street = "123 Main St", City = "Anytown", ZipCode = "12345" }
            };

            // Act
            var result = await orderService.ProcessOrderAsync(orderRequest);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("PAYMENT_DECLINED", result.Error.Code);
        }

        [Fact]
        public async Task ProcessOrder_ValidationErrors_AggregatesAllErrors()
        {
            // Arrange
            var inventoryService = new InventoryService();
            var paymentService = new PaymentService();
            var shippingService = new ShippingService();
            var orderService = new OrderService(inventoryService, paymentService, shippingService);

            var orderRequest = new OrderRequest
            {
                CustomerId = Guid.Empty, // Invalid
                Items = Array.Empty<OrderItem>(), // Invalid
                PaymentMethod = new CreditCardPayment { CardNumber = "invalid", CVV = "" }, // Invalid
                ShippingAddress = new Address { Street = "", City = "", ZipCode = "" } // Invalid
            };

            // Act
            var result = await orderService.ProcessOrderAsync(orderRequest);

            // Assert
            Assert.True(result.IsFailure);
            Assert.IsType<AggregateError>(result.Error);
            
            var aggregateError = (AggregateError)result.Error;
            Assert.Equal(2, aggregateError.Errors.Count);
            Assert.All(aggregateError.Errors, error => Assert.Equal("INVALID_ORDER", error.Code));
        }

        [Fact]
        public async Task GetOrderStatus_ExistingOrder_ReturnsStatus()
        {
            // Arrange
            var inventoryService = new InventoryService();
            var paymentService = new PaymentService();
            var shippingService = new ShippingService();
            var orderService = new OrderService(inventoryService, paymentService, shippingService);

            // Create an order first
            var orderRequest = new OrderRequest
            {
                CustomerId = Guid.NewGuid(),
                Items = new[] { new OrderItem { ProductId = "LAPTOP-001", Quantity = 1, Price = 999.99m } },
                PaymentMethod = new CreditCardPayment { CardNumber = "4111111111111111", CVV = "123" },
                ShippingAddress = new Address { Street = "123 Main St", City = "Anytown", ZipCode = "12345" }
            };

            var orderResult = await orderService.ProcessOrderAsync(orderRequest);
            var orderId = orderResult.Value.OrderId;

            // Act
            var statusOption = await orderService.GetOrderStatusAsync(orderId);

            // Assert
            statusOption.OnEither(
                some: status => Assert.Equal(OrderStatus.Confirmed, status),
                none: () => throw new Exception("Order should exist")
            );
        }

        [Fact]
        public async Task ApplyDiscountCode_ValidCode_Success()
        {
            // Arrange
            var inventoryService = new InventoryService();
            var paymentService = new PaymentService();
            var shippingService = new ShippingService();
            var orderService = new OrderService(inventoryService, paymentService, shippingService);

            var items = new[]
            {
                new OrderItem { ProductId = "LAPTOP-001", Quantity = 1, Price = 1000m }
            };

            // Act
            var result = await orderService.ApplyDiscountCodeAsync(items, "SAVE20");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(800m, result.Value.Sum(i => i.Price * i.Quantity));
        }

        // Domain Models
        private class OrderRequest
        {
            public Guid CustomerId { get; set; }
            public OrderItem[] Items { get; set; } = Array.Empty<OrderItem>();
            public IPaymentMethod PaymentMethod { get; set; } = null!;
            public Address ShippingAddress { get; set; } = null!;
        }

        private class OrderItem
        {
            public string ProductId { get; set; } = "";
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        private class OrderConfirmation
        {
            public Guid OrderId { get; set; }
            public OrderStatus Status { get; set; }
            public string? TrackingNumber { get; set; }
            public decimal TotalAmount { get; set; }
        }

        private enum OrderStatus
        {
            Pending,
            Confirmed,
            Shipped,
            Delivered,
            Cancelled
        }

        private interface IPaymentMethod { }

        private class CreditCardPayment : IPaymentMethod
        {
            public string CardNumber { get; set; } = "";
            public string CVV { get; set; } = "";
        }

        private class Address
        {
            public string Street { get; set; } = "";
            public string City { get; set; } = "";
            public string ZipCode { get; set; } = "";
        }

        private class PaymentResult
        {
            public bool Success { get; set; }
            public string? TransactionId { get; set; }
            public string? DeclineReason { get; set; }
        }

        // Service Implementations
        private class InventoryService
        {
            private readonly Dictionary<string, int> _inventory = new()
            {
                ["LAPTOP-001"] = 10,
                ["MOUSE-001"] = 50,
                ["KEYBOARD-001"] = 30
            };

            public Task<Result> CheckAvailabilityAsync(OrderItem[] items)
            {
                var errorBuilder = new ErrorBuilder();

                foreach (var item in items)
                {
                    if (item.ProductId == "OUT-OF-STOCK" || 
                        !_inventory.TryGetValue(item.ProductId, out var stock) || 
                        stock < item.Quantity)
                    {
                        errorBuilder.Append(new ValidationError(
                            "INSUFFICIENT_INVENTORY", 
                            $"Product {item.ProductId} has insufficient inventory"
                        ));
                    }
                }

                return Task.FromResult(
                    errorBuilder.HasErrors 
                        ? Result.Failure(errorBuilder.Build()) 
                        : Result.Success()
                );
            }

            public Task<Result> ReserveItemsAsync(OrderItem[] items)
            {
                // Simulate reserving items
                foreach (var item in items)
                {
                    if (_inventory.ContainsKey(item.ProductId))
                    {
                        _inventory[item.ProductId] -= item.Quantity;
                    }
                }
                return Task.FromResult(Result.Success());
            }
        }

        private class PaymentService
        {
            public Task<Result<PaymentResult>> ProcessPaymentAsync(decimal amount, IPaymentMethod paymentMethod)
            {
                if (paymentMethod is CreditCardPayment creditCard)
                {
                    // Simulate payment processing
                    if (creditCard.CardNumber == "4000000000000002")
                    {
                        return Task.FromResult(Result.Success(new PaymentResult
                        {
                            Success = false,
                            DeclineReason = "Insufficient funds"
                        }));
                    }

                    if (!IsValidCreditCard(creditCard))
                    {
                        return Task.FromResult(Result.Failure<PaymentResult>(
                            new ValidationError("INVALID_PAYMENT", "Invalid credit card information")
                        ));
                    }

                    return Task.FromResult(Result.Success(new PaymentResult
                    {
                        Success = true,
                        TransactionId = Guid.NewGuid().ToString()
                    }));
                }

                return Task.FromResult(Result.Failure<PaymentResult>(
                    new ValidationError("UNSUPPORTED_PAYMENT", "Payment method not supported")
                ));
            }

            private bool IsValidCreditCard(CreditCardPayment card)
            {
                return card.CardNumber.Length >= 13 && 
                       card.CardNumber.All(char.IsDigit) && 
                       !string.IsNullOrEmpty(card.CVV);
            }
        }

        private class ShippingService
        {
            public Task<Result<string>> CreateShipmentAsync(Address address, OrderItem[] items)
            {
                var validationResult = ValidateAddress(address);
                if (validationResult.IsFailure)
                    return Task.FromResult(Result.Failure<string>(validationResult.Error));

                // Generate tracking number
                var trackingNumber = $"TRACK-{Guid.NewGuid().ToString()[..8].ToUpper()}";
                return Task.FromResult(Result.Success(trackingNumber));
            }

            private Result ValidateAddress(Address address)
            {
                var errorBuilder = new ErrorBuilder();

                if (string.IsNullOrWhiteSpace(address.Street))
                    errorBuilder.Append(new ValidationError("INVALID_ADDRESS", "Street is required"));

                if (string.IsNullOrWhiteSpace(address.City))
                    errorBuilder.Append(new ValidationError("INVALID_ADDRESS", "City is required"));

                if (string.IsNullOrWhiteSpace(address.ZipCode))
                    errorBuilder.Append(new ValidationError("INVALID_ADDRESS", "Zip code is required"));

                return errorBuilder.HasErrors 
                    ? Result.Failure(errorBuilder.Build()) 
                    : Result.Success();
            }
        }

        private class OrderService
        {
            private readonly InventoryService _inventoryService;
            private readonly PaymentService _paymentService;
            private readonly ShippingService _shippingService;
            private readonly Dictionary<Guid, OrderStatus> _orders = new();

            public OrderService(
                InventoryService inventoryService, 
                PaymentService paymentService, 
                ShippingService shippingService)
            {
                _inventoryService = inventoryService;
                _paymentService = paymentService;
                _shippingService = shippingService;
            }

            public async Task<Result<OrderConfirmation>> ProcessOrderAsync(OrderRequest request)
            {
                // Validate order
                var validationResult = ValidateOrder(request);
                if (validationResult.IsFailure)
                    return validationResult.Error;

                // Check inventory
                var inventoryResult = await _inventoryService.CheckAvailabilityAsync(request.Items);
                if (inventoryResult.IsFailure)
                    return inventoryResult.Error;

                // Calculate total
                var totalAmount = request.Items.Sum(i => i.Price * i.Quantity);

                // Process payment
                var paymentResult = await _paymentService.ProcessPaymentAsync(totalAmount, request.PaymentMethod);
                if (paymentResult.IsFailure)
                    return paymentResult.Error;

                if (!paymentResult.Value.Success)
                    return new Error("PAYMENT_DECLINED", paymentResult.Value.DeclineReason ?? "Payment was declined");

                // Reserve inventory
                await _inventoryService.ReserveItemsAsync(request.Items);

                // Create shipment
                var shippingResult = await _shippingService.CreateShipmentAsync(request.ShippingAddress, request.Items);
                if (shippingResult.IsFailure)
                    return shippingResult.Error;

                // Create order confirmation
                var orderId = Guid.NewGuid();
                _orders[orderId] = OrderStatus.Confirmed;

                return new OrderConfirmation
                {
                    OrderId = orderId,
                    Status = OrderStatus.Confirmed,
                    TrackingNumber = shippingResult.Value,
                    TotalAmount = totalAmount
                };
            }

            public Task<Option<OrderStatus>> GetOrderStatusAsync(Guid orderId)
            {
                return Task.FromResult(
                    _orders.TryGetValue(orderId, out var status) 
                        ? Option.Some(status) 
                        : Option<OrderStatus>.None
                );
            }

            public async Task<Result<OrderItem[]>> ApplyDiscountCodeAsync(OrderItem[] items, string discountCode)
            {
                var discount = await GetDiscountAsync(discountCode);
                
                return discount.Match(
                    some: d =>
                    {
                        var discountedItems = items.Select(item => new OrderItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Price = item.Price * (1 - d.Percentage / 100)
                        }).ToArray();

                        return Result.Success(discountedItems);
                    },
                    none: () => Result.Failure<OrderItem[]>(
                        new ValidationError("INVALID_DISCOUNT", "Discount code is invalid or expired")
                    )
                );
            }

            private Task<Option<Discount>> GetDiscountAsync(string code)
            {
                var discounts = new Dictionary<string, Discount>
                {
                    ["SAVE10"] = new Discount { Code = "SAVE10", Percentage = 10 },
                    ["SAVE20"] = new Discount { Code = "SAVE20", Percentage = 20 }
                };

                return Task.FromResult(
                    discounts.TryGetValue(code, out var discount) 
                        ? Option.Some(discount) 
                        : Option<Discount>.None
                );
            }

            private Result ValidateOrder(OrderRequest request)
            {
                var errorBuilder = new ErrorBuilder();

                if (request.CustomerId == Guid.Empty)
                    errorBuilder.Append(new ValidationError("INVALID_ORDER", "Customer ID is required"));

                if (request.Items == null || request.Items.Length == 0)
                    errorBuilder.Append(new ValidationError("INVALID_ORDER", "Order must contain at least one item"));

                if (request.PaymentMethod == null)
                    errorBuilder.Append(new ValidationError("INVALID_ORDER", "Payment method is required"));

                if (request.ShippingAddress == null)
                    errorBuilder.Append(new ValidationError("INVALID_ORDER", "Shipping address is required"));

                return errorBuilder.HasErrors 
                    ? Result.Failure(errorBuilder.Build()) 
                    : Result.Success();
            }

            private class Discount
            {
                public string Code { get; set; } = "";
                public decimal Percentage { get; set; }
            }
        }
    }
}