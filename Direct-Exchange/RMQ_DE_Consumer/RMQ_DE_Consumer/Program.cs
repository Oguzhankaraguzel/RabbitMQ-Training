using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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
 * The same should be in the publisher
 */
channel.ExchangeDeclare(
    exchange: "direct-exchange-example",
    type: ExchangeType.Direct
    );

/*
 * Publisher tarafında bulunan routing key değerine sahip kuyruğa gönderilen mesajları, oluşturduğumuz kuyruğa yönlendirmek gerekir. Bunun için öncelikli olarak bir kuyruk oluşturulur.
 * 
 * It is necessary to forward the messages sent to the queue with the routing key value on the Publisher side to the queue we created. For this, a queue is first created.
 * 
 * Eğer kuyruğa bir isim vermezsek rabbitmq rastgele bir isim atar. Bu ismi aşağıdaki şekilde elde ederiz.
 * 
 * If we don't give the queue a name, rabbitmq assigns a random name. We obtain this name as follows.
 */
string queueName = channel.QueueDeclare().QueueName;

/**/
channel.QueueBind(
    queue: queueName,
    exchange: "direct-exchange-example",
    //Publisher'daki routing key - routing key in Publisher
    routingKey: "direct-queue-example"
    );

//mesajları alma işlemi -- recive operation

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