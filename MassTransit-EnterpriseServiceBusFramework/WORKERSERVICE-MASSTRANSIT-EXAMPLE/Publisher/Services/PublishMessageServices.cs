using MassTransit;
using Shrd.Messages;

namespace Publisher.Services
{
    public class PublishMessageServices : BackgroundService
    {
        readonly IPublishEndpoint _publishEndpoint;

        public PublishMessageServices(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            while (true)
            {
                ExampleMessage message = new()
                {
                    Text = $"{++i}. message"
                };

                await _publishEndpoint.Publish(message);
            }
        }
    }
}
