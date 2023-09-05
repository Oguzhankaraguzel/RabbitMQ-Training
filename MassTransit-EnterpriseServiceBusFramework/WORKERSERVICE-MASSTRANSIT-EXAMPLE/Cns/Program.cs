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
                 * Kuyruk ismi verilir ve bu kuyruða gelen mesajlarý hangi Consumer'ýn alacaðý belirtilir.
                 * The queue name is given and it is specified which Consumer will receive the messages sent to this queue.
                 * Mesajýn iþlenmesi ile ilgili iþlemler Consumer'da yapýlýr.
                 * The processing of the message is done at the Consumer.
                 */
                _configurator.ReceiveEndpoint("example-message-queue", e => e.ConfigureConsumer<ExampleMessageConsumer>(context));
            });
        });
    })
    .Build();

/*
 * Burada publish metodu kullanýldýðýndan gönderilen mesaj tüm kuyruklara gönderilecektir.
 * Because of the publish method is used here, the sent message will go to all queues.
 * Kuyruk ayarlamalarý consumer'da yapýlmalýdýr.
 * Queue settings must be made in the consumer.
 */

await host.RunAsync();
