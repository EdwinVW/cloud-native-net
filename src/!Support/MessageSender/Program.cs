using System.Reflection;

using MassTransit;
using Serilog;

var cancellationTokenSource = new CancellationTokenSource();

await Host.CreateDefaultBuilder(args)
        .UseSerilog((context, services, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration))
            
        .ConfigureServices((hostContext, services) =>
        {
            var busObserver = new BusObserver();
            services.AddSingleton<BusObserver>(busObserver);

            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(new Uri("amqp://127.0.0.1"));
                    config.ConfigureEndpoints(context);
                    config.ConnectBusObserver(busObserver);
                });

                x.AddConsumers(Assembly.GetEntryAssembly());                
            });

            services.AddHostedService<MessageSender>();
        })
        .Build()
        .RunAsync(cancellationTokenSource.Token);
