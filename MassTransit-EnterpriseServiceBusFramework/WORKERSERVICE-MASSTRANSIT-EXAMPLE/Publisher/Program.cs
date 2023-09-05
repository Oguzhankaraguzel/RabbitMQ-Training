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
             * MassTransit sýnýflarýný eklemezsek hata ile karþýlaþabiliriz.
             * If we do not add the MassTransit classes, we may encounter an error.
             * IPublishEndpoints hatasý almak istemiyorsak burada yapýlandýrarak vermek zorundayýz.
             * If we do not want to get an IPublishEndpoints error, we have to configure it here.
             * Kýsacasý MassTransit kütüphanesinden gelen ve IoC konteynerden çekip kullanacaðýmýz her þeyi yapýlandýrarak IoC konteyner'e vermek zorundayýz.
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
 * Burada publish metodu kullanýldýðýndan gönderilen mesaj tüm kuyruklara gönderilecektir.
 * Because of the publish method is used here, the sent message will go to all queues.
 * Kuyruk ayarlamalarý consumer'da yapýlmalýdýr.
 * Queue settings must be made in the consumer.
 */

await host.RunAsync();
