using MassTransit;
using TechChallange.Region.Domain.Region.Entity;
using TechChallange.Region.Domain.Region.Exception;
using TechChallange.Region.Domain.Region.Messaging;
using TechChallange.Region.Domain.Region.Service;

namespace Tech_Challange_Region_Consumer
{
    public class InsertRegionConsumer : IConsumer<RegionCreateMessageDto>
    {
        private readonly IRegionService _regionService;
        private readonly ILogger<InsertRegionConsumer> _logger;
        public InsertRegionConsumer(IRegionService regionService, ILogger<InsertRegionConsumer> logger)
        {
            _regionService = regionService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<RegionCreateMessageDto> context)
        {
            var regionMessage = context.Message;

            _logger.LogInformation($"Received region message: {regionMessage.ToString()}");
            try
            {

                await _regionService.CreateAsync(new RegionEntity(regionMessage.Name, regionMessage.Ddd)).ConfigureAwait(false);
            }
            catch (RegionAlreadyExistsException ex)
            {

                _logger.LogError($"Error processing message {regionMessage.ToString()} - error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message {regionMessage.ToString()} - error: {ex.Message} - Message sent to DLQ");
                throw;
            }
        }
    }
}
