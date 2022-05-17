using Infrastructure.Common.Messaging.MassTransit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Setup logging

builder.Host
    .UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add<ConcurrencyExceptionFilter>();
        options.Filters.Add<ConsistencyExceptionFilter>();
        options.Filters.Add<InvalidValueObjectExceptionFilter>();
    })
    .AddFluentValidation(configuration => configuration.DisableDataAnnotationsValidation = true)
    .AddJsonOptions(j => j.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

// Add Application services
builder.Services.AddApplication();

// Add Infrastructure services
var connectionString = builder.Configuration.GetConnectionString("Database");
var maxRetryCount = builder.Configuration
    .GetSection("Db")
    .GetValue<int>("MaxRetryCount");

var massTransitOptions = new MassTransitOptions();
massTransitOptions.IsConsumer = false;
massTransitOptions.RabbitMqOptions = new RabbitMQOptions
{
    Host = "amqp://127.0.0.1"
};

builder.Services.AddInfrastructure(connectionString, maxRetryCount, massTransitOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    using var dbContext = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

