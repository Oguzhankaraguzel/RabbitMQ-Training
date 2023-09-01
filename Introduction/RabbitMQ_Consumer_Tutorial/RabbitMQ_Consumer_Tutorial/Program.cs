using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

/*
 * Bağlantı oluşturma
 * Create Connection
 * 
 * Bu bağlantı url ile sağlandı.
 * This Connection was provided with the url.
 * 
 * Diğer senaryo için kullanıcı adı,şifre ve port bilgileri gerekli
 * Username, password and port information is required for the other scenario.
 */

ConnectionFactory factory = new();
factory.Uri = new("amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt");

/*
 * Bağlantıyı aktifleştirme ve kanal açma
 * Activate connection and open a channel
 * 
 * Using anahtar kelimesi kullanılmasının sebebi; IConnection arayüzünün IDisposable arayüzünden miras almasıdır.
 * The reason for using the "using" keyword; The IConnection interface inherits from the IDisposable interface.
 */

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

// kuyruk oluşturma - Create queue

channel.QueueDeclare(queue: "example-queue", exclusive: false); // Publisher ile birebir aynı yapıda tanımlanmalıdır. It should be defined in the same structure as the Publisher.

/*
 * Mesaj okuma
 * read message
 * 
 * Kanaldan mesaj okumak için bir event operasyonu yapılması gerekli
 * An event operation is required to read a message from the channel.
 * 
 * EventingBasicConsumer sınıfı kullanılır.
 * The EventingBasicConsumer class is used.
 */

EventingBasicConsumer consumer = new(channel);

// Eğer kuyrukta bir mesaj varsa "recive" edilir.

channel.BasicConsume(queue: "example-queue", autoAck: false, consumer);
consumer.Received += (sender, e) =>
{
    // Kuyruğa gelen mesajın işlendiği yer.  Where the queued message is processed
    // e.Body : Kuyruktaki mesajın verisini getirir. Returns the data of the message in the queue.
    // ↓↓↓↓↓↓↓↓↓↓↓↓ byte[] türüne çevirmek gerekir. it should be convert to byte[]; ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
    // e.Body.Span veya (or) e.Body.ToArray() : Kuyruktaki mesajın byte verisini getirir. Retrieves the byte data of the message in the queue.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};


Console.Read();