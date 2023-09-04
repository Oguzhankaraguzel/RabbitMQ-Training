using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

/*
 * point to point tasarımında genellikle direct exchange kullanılır.
 * In point-to-point design, direct exchange is often used.
 */

string queueName = "example-p2p-queue";

channel.QueueDeclare(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false
    );

byte[] message = Encoding.UTF8.GetBytes("Hello");

channel.BasicPublish(
    exchange: string.Empty,
    routingKey: queueName,
    body: message
    );

Console.Read();