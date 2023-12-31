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
             * MassTransit sınıflarını eklemezsek hata ile karşılaşabiliriz.
             * If we do not add the MassTransit classes, we may encounter an error.
             * IPublishEndpoints hatası almak istemiyorsak burada yapılandırarak vermek zorundayız.
             * If we do not want to get an IPublishEndpoints error, we have to configure it here.
             * Kısacası MassTransit kütüphanesinden gelen ve IoC konteynerden çekip kullanacağımız her şeyi yapılandırarak IoC konteyner'e vermek zorundayız.
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
 * Burada publish metodu kullanıldığından gönderilen mesaj tüm kuyruklara gönderilecektir.
 * Because of the publish method is used here, the sent message will go to all queues.
 * Kuyruk ayarlamaları consumer'da yapılmalıdır.
 * Queue settings must be made in the consumer.
 */

await host.RunAsync();
