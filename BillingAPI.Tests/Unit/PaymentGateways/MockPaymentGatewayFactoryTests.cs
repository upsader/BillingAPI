using BillingAPI.Core.Exceptions;
using BillingAPI.Core.Interfaces;
using BillingAPI.Services.PaymentGateways;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BillingAPI.Tests.Unit.PaymentGateways
{
    public class MockPaymentGatewayFactoryTests
    {
        private readonly Mock<ILogger<MockPaymentGatewayFactory>> _mockLogger;
        private readonly MockPaymentGatewayFactory _factory;

        public MockPaymentGatewayFactoryTests()
        {
            _mockLogger = new Mock<ILogger<MockPaymentGatewayFactory>>();
            _factory = new MockPaymentGatewayFactory(_mockLogger.Object);
        }

        [Fact]
        public void InitializeDefaultGateways_Should_RegisterDefaultGateways()
        {
            // Act
            _factory.InitializeDefaultGateways();

            // Assert
            var stripeGateway = _factory.GetGateway("Stripe");
            var paypalGateway = _factory.GetGateway("PayPal");

            Assert.NotNull(stripeGateway);
            Assert.NotNull(paypalGateway);
            Assert.IsType<MockStripeGateway>(stripeGateway);
            Assert.IsType<MockPayPalGateway>(paypalGateway);

            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Default mock gateways initialized successfully.")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
                ),
                Times.Once
            );
        }

        [Fact]
        public void GetGateway_Should_ThrowNotFoundException_WhenGatewayNotRegistered()
        {
            // Act
            var exception = Assert.Throws<NotFoundException>(() => _factory.GetGateway("NonExistentGateway"));

            // Assert
            Assert.Equal("No payment gateway registered for ID: NonExistentGateway", exception.Message);

            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No payment gateway registered for ID: NonExistentGateway")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
                ),
                Times.Once
            );
        }

        [Fact]
        public void GetGateway_Should_ReturnGateway_WhenGatewayIsRegistered()
        {
            // Arrange
            var mockGateway = new Mock<IPaymentGateway>();
            _factory.RegisterGateway("CustomGateway", mockGateway.Object);

            // Act
            var result = _factory.GetGateway("CustomGateway");

            // Assert
            Assert.Equal(mockGateway.Object, result);

            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Successfully retrieved payment gateway for ID: CustomGateway")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
                ),
                Times.Once
            );
        }

        [Fact]
        public void RegisterGateway_Should_RegisterGatewaySuccessfully()
        {
            // Arrange
            var mockGateway = new Mock<IPaymentGateway>();

            // Act
            _factory.RegisterGateway("CustomGateway", mockGateway.Object);

            // Assert
            var registeredGateway = _factory.GetGateway("CustomGateway");
            Assert.Equal(mockGateway.Object, registeredGateway);

            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Successfully registered payment gateway for ID: CustomGateway")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
                ),
                Times.Once
            );
        }

        [Fact]
        public void RegisterGateway_Should_OverwriteExistingGateway()
        {
            // Arrange
            var mockGateway1 = new Mock<IPaymentGateway>();
            var mockGateway2 = new Mock<IPaymentGateway>();

            _factory.RegisterGateway("CustomGateway", mockGateway1.Object);

            // Act
            _factory.RegisterGateway("CustomGateway", mockGateway2.Object);

            // Assert
            var registeredGateway = _factory.GetGateway("CustomGateway");
            Assert.Equal(mockGateway2.Object, registeredGateway);

            _mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Successfully registered payment gateway for ID: CustomGateway")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)
                ),
                Times.Exactly(2)
            );
        }
    }
}

