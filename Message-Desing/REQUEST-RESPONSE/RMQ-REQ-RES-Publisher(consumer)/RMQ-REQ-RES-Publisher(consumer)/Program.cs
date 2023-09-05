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

//Response'un dinleneceği queue tanımlanır.
//The queue where the response will be listened to is defined.

string replyQueueName = channel.QueueDeclare().QueueName;
//Response sürecinde hangi request'e karşılık, response'un yapılacağını ifade edecek olan korelasyon değeri oluşturulur
//In the response process, a correlation value is created to express which request the response will be made against.
string correlationId = Guid.NewGuid().ToString();

#region Create Request message - Request mesajını oluşturma ve yollama

IBasicProperties properties = channel.CreateBasicProperties();
//Request korelasyon değeri ile eşleştirilir.
//It is matched with the Request correlation value.
properties.CorrelationId = correlationId;
//Response Yapılacak queue'nun "ReplyTo" özelliğine atanır.
//It is assigned to the "ReplyTo" property of the queue to which the response will be made.
properties.ReplyTo = replyQueueName;

byte[] message = Encoding.UTF8.GetBytes("Request Message");
channel.BasicPublish(
    exchange: string.Empty,
    routingKey: queueName,
    body: message,
    basicProperties: properties
    );
#endregion

#region Response dinleme - Listen to response

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(
    queue: replyQueueName,
    autoAck: true,
    consumer: consumer
    );

consumer.Received += (sender, e) =>
{
    //Gelen mesajın gönderilmiş mesajla aynı ıd değerine sahip olup olmadığını bakılır. aynı ise işleme tabi tutulur
    //It is checked whether the incoming message has the same ID value as the sent message. If it is the same, it is processed
    if (e.BasicProperties.CorrelationId == correlationId)
    {
        Console.WriteLine($"Response = {Encoding.UTF8.GetString(e.Body.Span)}");
    }
};

#endregion

Console.Read();