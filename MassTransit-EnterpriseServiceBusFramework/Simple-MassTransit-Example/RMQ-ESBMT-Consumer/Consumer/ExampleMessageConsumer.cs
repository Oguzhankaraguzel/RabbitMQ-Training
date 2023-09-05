using MassTransit;
using RMQ_ESBMT_Shared.Messages;

namespace RMQ_ESBMT_Consumer.Consumer
{
    public class ExampleMessageConsumer : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine($"Recieved Message : {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
