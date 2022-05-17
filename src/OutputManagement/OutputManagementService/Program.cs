using Serilog;

await Host.CreateDefaultBuilder(args)
        .UseSerilog((context, services, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration))
        .ConfigureServices((hostContext, services) =>
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumers(Assembly.GetEntryAssembly());

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(new Uri("amqp://127.0.0.1"));
                    config.ConfigureEndpoints(context);
                });
            });

            services.AddTransient<IEmailService, EmailService>();
            services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
            services.AddSingleton<IContractRepository, InMemoryContractRepository>();
        })
        .Build()
        .RunAsync();