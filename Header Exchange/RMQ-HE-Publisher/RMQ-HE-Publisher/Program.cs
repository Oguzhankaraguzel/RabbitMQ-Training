using RabbitMQ.Client;
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

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i}");
    Console.WriteLine("Please write the value");
    string value = Console.ReadLine();

    IBasicProperties basicProperties = channel.CreateBasicProperties();
    basicProperties.Headers = new Dictionary<string, object>()
    {
        ["no"] = value
    };

    channel.BasicPublish(
        exchange: "header-exchange-example",
        routingKey: string.Empty,
        body: message,
        basicProperties: basicProperties
        );
}

Console.Read();