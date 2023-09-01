using RabbitMQ.Client;
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

channel.QueueDeclare(queue: "example-queue", exclusive: false);

/*
 * Mesaj gönderme
 * send message
 * 
 * RabbitMQ kuyruğa atacağı mesajı byte türünden kabul etmektedir. Bundan dolayı mesajları byte'a dönüştürmemiz gerekmektedir.
 * RabbitMQ accepts the message to be queued in bytes. Therefore, we need to convert messages to bytes.
 * 
 * !!! Encoding.UTF8.GetBytes() !!!
 * 
 * 
 */

//byte[] message = Encoding.UTF8.GetBytes("Hello");
//channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Hello " + i);
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
}

// !!! exchange:"" => Varsayılan (Default) Exchange => Direct Exchange => routingKey = "Kuyruk ismi (Queue name)"

Console.Read();