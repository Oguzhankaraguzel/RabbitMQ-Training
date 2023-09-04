using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

Console.WriteLine("write topic");
string topic = Console.ReadLine();

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
    queue: queueName,
    exchange: "topic-exchange-example",
    routingKey: topic
    );

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer
    );

consumer.Received += (sender, e) =>
{
    string messages = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(messages);
};

Console.Read();