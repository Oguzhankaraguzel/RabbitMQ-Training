using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "fanout-exchange-example",
    type: ExchangeType.Fanout
    );

for (int i = 100; i <= 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i}");
    channel.BasicPublish(
        exchange: "fanout-exchange-example",
        /*
         * routing key boş bırakılarak, bu exchange'e bağlanmış her consumer'a, kuyruğa bakılmaksızın mesaj gönderilir.
         * By leaving the routing key blank, a message is sent to every consumer connected to this exchange, regardless of the queue.
         */
        routingKey: string.Empty,
        body: message
        );
}

Console.Read();