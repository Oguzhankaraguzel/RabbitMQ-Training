using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

string queueName = "example-request-response-queue";

channel.QueueDeclare(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false
    );

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer
    );

consumer.Received += (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));

    byte[] responseMessage = Encoding.UTF8.GetBytes("its done");
    /*
     * Gelen mesajdaki korelasyon değerini alıp, gönderilecek mesajın property'sine işliyoruz.
     * We take the correlation value in the incoming message and process it into the property of the message to be sent.
     */
    IBasicProperties properties = e.BasicProperties;
    IBasicProperties replyProperties = channel.CreateBasicProperties();
    replyProperties.CorrelationId = properties.CorrelationId;
    channel.BasicPublish(
        exchange: string.Empty,
        //Gelen mesajdaki routing key response kuyruğu olarak kullanılır
        //It is used as the routing key response queue in the incoming message.
        routingKey: properties.ReplyTo,
        basicProperties: replyProperties,
        body: responseMessage
        );
};
Console.Read();