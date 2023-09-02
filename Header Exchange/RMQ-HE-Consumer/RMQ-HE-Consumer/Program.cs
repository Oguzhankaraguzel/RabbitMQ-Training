using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

/*
 * xmatch : ilgili queue'nun mesajı hangi davranışla alacağının kararını veren bir parametredir
 * 
 * xmatch : it is a parameter that decides the behavior of the corresponding queue to receive the message.
 * 
 * xmatch : any
 * 
 * ilgili queue'nun sadece tek bir key-value eşleşmesi durumunda mesajı alır.
 *
 * receives the message if only one key-value match of the corresponding queue.
 * 
 * xmatch : all
 * 
 * mesajı almak için bütün key-value değerleri eşleşmek zorunda
 * 
 * All key-values have to match to get the message
 */

channel.ExchangeDeclare(
    exchange: "header-exchange-example",
    type: ExchangeType.Headers
    );

Console.WriteLine("Please write the value");
string value = Console.ReadLine();

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(
    queue: queueName,
    exchange: "header-exchange-example",
    routingKey: string.Empty,
    arguments: new Dictionary<string, object>
    {
        //varsayılan - default
        //["x-match"] = "any",
        ["no"] = value
    }
    );

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer
    );

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();