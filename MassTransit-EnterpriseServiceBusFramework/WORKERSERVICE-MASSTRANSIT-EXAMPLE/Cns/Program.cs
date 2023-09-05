using Cns.Consumer;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<ExampleMessageConsumer>();

            configurator.UsingRabbitMq((context, _configurator) =>
            {
                _configurator.Host("amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt");
                /*
                 * Kuyruk ismi verilir ve bu kuyru�a gelen mesajlar� hangi Consumer'�n alaca�� belirtilir.
                 * The queue name is given and it is specified which Consumer will receive the messages sent to this queue.
                 * Mesaj�n i�lenmesi ile ilgili i�lemler Consumer'da yap�l�r.
                 * The processing of the message is done at the Consumer.
                 */
                _configurator.ReceiveEndpoint("example-message-queue", e => e.ConfigureConsumer<ExampleMessageConsumer>(context));
            });
        });
    })
    .Build();

/*
 * Burada publish metodu kullan�ld���ndan g�nderilen mesaj t�m kuyruklara g�nderilecektir.
 * Because of the publish method is used here, the sent message will go to all queues.
 * Kuyruk ayarlamalar� consumer'da yap�lmal�d�r.
 * Queue settings must be made in the consumer.
 */

await host.RunAsync();
