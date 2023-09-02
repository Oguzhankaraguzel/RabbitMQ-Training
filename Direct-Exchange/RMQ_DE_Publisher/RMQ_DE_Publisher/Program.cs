using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

/*
 * exchange:"exchange adı(names)"
 * type: "string" veya(or) sınıf sabitleri(class constant)
 * Bu exchange davranışı "direct" olduğundan rabbitmq routing key'e bakar
 * Because this exchange behavior is "direct", rabbitmq looks at the routing key
 * Routing key'e karşılık gelen kuyruk hangisi ise mesajı ona gönderir
 * It sends the message to whichever queue corresponds to the routing key
 * Aynısı consumerde olmalı
 * The same should be in the consumer
 */
channel.ExchangeDeclare(
    exchange: "direct-exchange-example", type: ExchangeType.Direct
    );

while (true)
{
    Console.Write("Mesaj : ");
    string message = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
        exchange: "direct-exchange-example",
        routingKey: "direct-queue-example",
        body: byteMessage
        );
}

Console.Read();