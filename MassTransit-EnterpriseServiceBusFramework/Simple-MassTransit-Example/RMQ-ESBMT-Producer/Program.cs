using MassTransit;
using RMQ_ESBMT_Shared.Messages;

string rabbitMQUri = "amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt";

string queueName = "example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});

/*
 * Send operasyonu tek bir kuyruğa mesaj göndermemizi sağlayan metoddur.
 * The Send operation is a method that allows us to send messages to a specific queue.
 * 
 * Tüm kuyruklara mesaj yollamak için başka metodlar kullanılır.
 * Other methods are used to send messages to all queues.
 */

ISendEndpoint senEndpoint = await bus.GetSendEndpoint(new($"{rabbitMQUri}/{queueName}"));

Console.Write("Message : ");
string message = Console.ReadLine();
senEndpoint.Send<IMessage>(new ExampleMessage()
{
    Text = message
});

Console.Read();