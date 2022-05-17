var builder = Host.CreateDefaultBuilder(args);

builder
    .UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration))

    .ConfigureHostOptions(options =>
        options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore)

    .ConfigureServices((hostContext, services) =>
    {
        var connectionString = hostContext.Configuration.GetConnectionString("Database");
        var maxRetryCount = hostContext.Configuration
            .GetSection("Db")
            .GetValue<int>("MaxRetryCount");

        var massTransitOptions = new MassTransitOptions();
        massTransitOptions.IsConsumer = true;
        massTransitOptions.RabbitMqOptions = new RabbitMQOptions
        {
            Host = "127.0.0.1"
        };

        services.AddInfrastructure(connectionString, maxRetryCount, massTransitOptions);
        services.AddApplication();
    });

var app = builder.Build();
await app.RunAsync();