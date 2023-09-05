using MassTransit;
using Shrd.Messages;

namespace Cns.Consumer
{
    public class ExampleMessageConsumer : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine($"Recived Message : {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
