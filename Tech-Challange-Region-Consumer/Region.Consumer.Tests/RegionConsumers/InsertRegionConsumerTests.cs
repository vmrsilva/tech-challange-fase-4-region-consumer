using Moq;
using TechChallange.Region.Domain.Region.Service;
using Microsoft.Extensions.Logging;
using Tech_Challange_Region_Consumer;
using TechChallange.Region.Domain.Region.Messaging;
using MassTransit;
using TechChallange.Region.Domain.Region.Entity;
using TechChallange.Region.Domain.Region.Exception;

namespace Region.Consumer.Tests.RegionConsumers
{
    public class InsertRegionConsumerTests
    {
        private readonly Mock<IRegionService> _regionServiceMock;
        private readonly Mock<ILogger<InsertRegionConsumer>> _loggerMock;
        private readonly InsertRegionConsumer _consumer;
        private readonly RegionCreateMessageDto _messageDto;

        public InsertRegionConsumerTests()
        {
            _regionServiceMock = new Mock<IRegionService>();
            _loggerMock = new Mock<ILogger<InsertRegionConsumer>>();
            _consumer = new InsertRegionConsumer(_regionServiceMock.Object, _loggerMock.Object);
            _messageDto = new RegionCreateMessageDto { Name = "Test Region", Ddd = "11" };
        }

        [Fact(DisplayName = "Should Consume Valid Message Calls CreateAsync")]
        public async Task ShouldConsumeValidMessageCallsCreateAsync()
        {
            var contextMock = new Mock<ConsumeContext<RegionCreateMessageDto>>();
            contextMock.Setup(c => c.Message).Returns(_messageDto);

            await _consumer.Consume(contextMock.Object);

            _regionServiceMock.Verify(
                s => s.CreateAsync(It.Is<RegionEntity>(r => r.Name == _messageDto.Name && r.Ddd == _messageDto.Ddd)),
                Times.Once());
        }

        [Fact(DisplayName = "Should Consume Region Return AlreadyExistsException")]
        public async Task ShouldConsumeRegionReturnAlreadyExistsException()
        {
            var contextMock = new Mock<ConsumeContext<RegionCreateMessageDto>>();
            contextMock.Setup(c => c.Message).Returns(_messageDto);
            var exception = new RegionAlreadyExistsException();
            _regionServiceMock
                .Setup(s => s.CreateAsync(It.IsAny<RegionEntity>()))
                .ThrowsAsync(exception);

            await _consumer.Consume(contextMock.Object);

            _regionServiceMock.Verify(
                s => s.CreateAsync(It.IsAny<RegionEntity>()),
                Times.Once());
        }

        [Fact(DisplayName = "Should Consume Generic Exception Throws")]
        public async Task ShouldConsumeGenericExceptionThrows()
        {
            var contextMock = new Mock<ConsumeContext<RegionCreateMessageDto>>();
            contextMock.Setup(c => c.Message).Returns(_messageDto);
            var exception = new Exception("Unexpected error");
            _regionServiceMock
                .Setup(s => s.CreateAsync(It.IsAny<RegionEntity>()))
                .ThrowsAsync(exception);

            var thrownException = await Assert.ThrowsAsync<Exception>(() => _consumer.Consume(contextMock.Object));
            Assert.Equal("Unexpected error", thrownException.Message);

            _regionServiceMock.Verify(
                s => s.CreateAsync(It.IsAny<RegionEntity>()),
                Times.Once());
        }
    }
}
