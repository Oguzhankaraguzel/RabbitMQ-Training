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

channel.QueueDeclare(
    queue: "example-queue",
    exclusive: false,
    //♣herhangi bir problemde mesajların kalıcılığını sağlamak için // for the message durability
    durable: true); // Publisher ile birebir aynı yapıda tanımlanmalıdır. It should be defined in the same structure as the Publisher.


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

channel.BasicConsume(queue: "example-queue", autoAck: /*false*//*For message acknowledgement*/ true, consumer);

/*
 * adil dağılım - fair dispatch
 * 
 * prefetchSize; Consumer tarafından alınabilecek en büyük mesaj boyutunu byte cinsinden ifade eder. 0 sonsuz demektir
 * prefetctCount: Bir consumer tarafından aynı anda işleme alınabilecek mesaj sayısını belirler.
 * global: bu ayar tüm tüketiciler için mi yoksa çağrı yapılan tüketici için mi olduğunu belirler. 
 * 
 * prefetchSize; It expresses the maximum message size in bytes that can be received by the consumer. 0 means infinity
 * prefetch Count: It determines the number of messages that can be processed simultaneously by a consumer.
 * global: this setting determines whether it is for all consumers or the called consumer.
 */
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);


consumer.Received += async (sender, e) =>
{
    // Kuyruğa gelen mesajın işlendiği yer.  Where the queued message is processed
    // e.Body : Kuyruktaki mesajın verisini getirir. Returns the data of the message in the queue.
    // ↓↓↓↓↓↓↓↓↓↓↓↓ byte[] türüne çevirmek gerekir. it should be convert to byte[]; ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
    // e.Body.Span veya (or) e.Body.ToArray() : Kuyruktaki mesajın byte verisini getirir. Retrieves the byte data of the message in the queue.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    await Task.Delay(3000);
    /*For message acknowledgement
     * e.DeliveryTag sahip mesaj silinir. The message is delete which is have e.DeliveryTag
     * multiple true olursa işlenmemiş önceki mesajlarda silinir.
     * if the multiple:true, other messages which are not processed yet also delete
     * mesaj kuyruğa 30 dakika sonra eklenir.
     * messege added to the queue after 30 minutes
     */
    channel.BasicAck(e.DeliveryTag, multiple: false);

    //Bazen istemli olarak mesajı işlemeyebilir ya da işleyemeyeceğimizi anladığımı durumlarda buna göre cevap verebiliriz. 
    //Sometimes we may not voluntarily process the message or respond accordingly in cases where we understand that we cannot.
    //requeue parametresi mesajın tekrardan kuyruğa eklenip eklenmeyeceğini belirtir.
    //The requeue parameter specifies whether the message will be added to the queue again.
    channel.BasicNack(e.DeliveryTag, multiple: false, requeue: true);

    //Belirli bir mesajın işlenmesini reddetmek için
    //To reject processing of a specific message
    channel.BasicReject(deliveryTag: 3, requeue: true);
};


Console.Read();