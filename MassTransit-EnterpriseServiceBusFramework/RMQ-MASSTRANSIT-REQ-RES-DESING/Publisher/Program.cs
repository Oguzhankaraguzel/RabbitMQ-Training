using MassTransit;
using Shared.Messages;

string rabbitMQUri = "amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt";
string requestQueue = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});
/*
 * Publisher aynı zamanda gelen mesajları dinlediği için burada başlatılması gerek.
 * Since Publisher also listens for incoming messages, it should be started here.
 */
await bus.StartAsync();


var request = bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/{requestQueue}"));
int i = 1;
while (true)
{
    await Task.Delay(200);
    var response = await request.GetResponse<ResponseMessage>(new() { MessageNo = i, Text = $"{i++} request" });

    Console.WriteLine($"Response Received : {response.Message.Text}");
}