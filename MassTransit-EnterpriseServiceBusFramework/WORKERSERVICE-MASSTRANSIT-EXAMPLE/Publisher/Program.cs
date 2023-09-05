using MassTransit;
using Publisher.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, _configurator) =>
            {
                _configurator.Host("amqps://jizvoypt:baU73GaP3L0Iyi17yRGXndIhBA_1LT-F@sparrow.rmq.cloudamqp.com/jizvoypt");
            });
        });
        services.AddHostedService<PublishMessageServices>(
            /*
             * MassTransit s�n�flar�n� eklemezsek hata ile kar��la�abiliriz.
             * If we do not add the MassTransit classes, we may encounter an error.
             * IPublishEndpoints hatas� almak istemiyorsak burada yap�land�rarak vermek zorunday�z.
             * If we do not want to get an IPublishEndpoints error, we have to configure it here.
             * K�sacas� MassTransit k�t�phanesinden gelen ve IoC konteynerden �ekip kullanaca��m�z her �eyi yap�land�rarak IoC konteyner'e vermek zorunday�z.
             * In short, we have to configure everything that comes from the MassTransit library, that we will pull from the IoC container and use, and export it to the IoC container.
             */
            provider =>
            {
                using IServiceScope scope = provider.CreateScope();
                IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();
                return new(publishEndpoint);
            }
            );
    })
    .Build();

/*
 * Burada publish metodu kullan�ld���ndan g�nderilen mesaj t�m kuyruklara g�nderilecektir.
 * Because of the publish method is used here, the sent message will go to all queues.
 * Kuyruk ayarlamalar� consumer'da yap�lmal�d�r.
 * Queue settings must be made in the consumer.
 */

await host.RunAsync();
