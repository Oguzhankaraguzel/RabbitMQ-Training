using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

/*
 * Genellikle fanout Exchange tipi kullanılır.
 * Usually fanout Exchange type is used.
 * 
 * Mesaj, bu Exchange bağlı tüm kuyruklara gönderieceği için routing key’e bakılmaz ve boş bırakılır
 * 
 * Since the message will be sent to all queues connected to this Exchange, the routing key is not checked and is left blank.
 */

channel.ExchangeDeclare(
    exchange: "pubsub-example",
    type: ExchangeType.Fanout
    );


byte[] message = Encoding.UTF8.GetBytes("Hello");
channel.BasicPublish(
    exchange: "pubsub-example",
    routingKey: string.Empty,
    body: message
    );


Console.Read();