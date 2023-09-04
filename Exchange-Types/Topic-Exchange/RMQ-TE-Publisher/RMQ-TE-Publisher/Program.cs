using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "topic-exchange-example",
    type: ExchangeType.Topic
    );

/*
 * try to topic like this 
 * *.hello.*
 * *.hello
 * #.world.#
 * world.#
 * *.*.world
 * #.hello
 */

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i}");
    Console.WriteLine("Write topic");
    string topic = Console.ReadLine();
    channel.BasicPublish(
        exchange: "topic-exchange-example",
        routingKey: topic,
        body: message
        );
}

Console.Read();