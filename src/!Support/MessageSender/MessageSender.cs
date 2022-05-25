using CustomerManagement.Application.IntegrationEvents;
using ProductManagement.Application.IntegrationEvents;
using Generic.IntegrationEvents;
using MassTransit;
using System.Globalization;
using ContractManagement.Domain.Aggregates.Contract.Commands;
using ContractManagement.Domain.Aggregates.Contract.Enums;
using System.Net.Http.Json;

public class MessageSender : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IBus _bus;
    private readonly BusObserver _busObserver;

    public MessageSender(IHostApplicationLifetime hostApplicationLifetime, IBus bus, BusObserver busObserver)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _bus = bus;
        _busObserver = busObserver;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // wait for bus to start
        while (!_busObserver.BusHasStarted)
        {
            await Task.Delay(100);
        }

        Console.WriteLine("\nBus started. Press any key to continue ...");
        Console.ReadKey(true);

        while (!stoppingToken.IsCancellationRequested)
        {
            Console.Clear();

            Console.WriteLine("======================");
            Console.WriteLine("Message Sender Utility");
            Console.WriteLine("======================");
            Console.WriteLine("Select message type:");
            Console.WriteLine("  Events:");
            Console.WriteLine("   1. CustomerRegistered");
            Console.WriteLine("   2. ProductRegistered");
            Console.WriteLine("   3. DayHasPassed");
            Console.WriteLine("  Commands:");
            Console.WriteLine("   4. RegisterContract");
            Console.WriteLine("   5. ChangeContractAmount");
            Console.WriteLine("   6. ChangeContractTerm");
            Console.WriteLine("   7. CancelContract");
            Console.WriteLine(" 0. Exit");
            var keyInfo = Console.ReadKey(true);

            switch (keyInfo.KeyChar)
            {
                case '0':
                    _hostApplicationLifetime.StopApplication();
                    return;

                case '1':
                    await PublishCustomerRegistered();
                    break;

                case '2':
                    await PublishProductRegistered();
                    break;

                case '3':
                    await PublishDayHasPassed();
                    break;

                case '4':
                    await RegisterContract();
                    break;

                case '5':
                    await ChangeContractAmount();
                    break;

                case '6':
                    await ChangeContractTerm();
                    break;

                case '7':
                    await CancelContract();
                    break;

                default:
                    Console.WriteLine("Unknown option, please try again.");
                    break;
            }

            Console.WriteLine("\nPress any key to continue ...");
            Console.ReadKey(true);
        }
    }

    private async Task PublishDayHasPassed()
    {
        Console.WriteLine("\nDayHasPassed:");
        Console.WriteLine("-------------------");
        Console.WriteLine("Publish message ...");
        var @event = new DayHasPassed(Guid.NewGuid());
        await _bus.Publish(
            message: @event, 
            messageType: @event.GetType(), 
            callback: ctx => ctx.Headers.Set("EventType", "IntegrationEvent"));  
        Console.WriteLine("Done.");
    }

    private async Task PublishCustomerRegistered()
    {
        Console.WriteLine("\nCustomerRegistered:");
        Console.WriteLine("-------------------");
        Console.Write(" CustomerNumber (C99999) : ");
        string? customerNumber = Console.ReadLine();
        Console.Write(" First name              : ");
        string? firstName = Console.ReadLine();
        Console.Write(" Last name               : ");
        string? lastName = Console.ReadLine();
        Console.Write(" Address                 : ");
        string? address = Console.ReadLine();
        Console.Write(" Email                   : ");
        string? email = Console.ReadLine();

        Console.WriteLine("\nPublish message ...");
        var @event = new CustomerRegistered(Guid.NewGuid(), customerNumber!, firstName!, lastName!, address!, email!);
        await _bus.Publish(
            message: @event, 
            messageType: @event.GetType(), 
            callback: ctx => ctx.Headers.Set("EventType", "IntegrationEvent"));  
        Console.WriteLine("Done.");
    }

    private async Task PublishProductRegistered()
    {
        Console.WriteLine("\nProductRegistered:");
        Console.WriteLine("-------------------");
        Console.Write(" ProductNumber (FAC-99999) : ");
        string? productNumber = Console.ReadLine();
        Console.Write(" Product description       : ");
        string? description = Console.ReadLine();

        Console.WriteLine("\nPublish message ...");
        var @event = new ProductRegistered(Guid.NewGuid(), productNumber!, description!);
        await _bus.Publish(
            message: @event, 
            messageType: @event.GetType(), 
            callback: ctx => ctx.Headers.Set("EventType", "IntegrationEvent"));   
        Console.WriteLine("Done.");
    }

    private async Task RegisterContract()
    {
        Console.WriteLine("\nRegisterContract:");
        Console.WriteLine("-------------------");
        Console.Write(" Contractnumber (CTR-yyyyMMdd-9999) : ");
        string? contractNumber = Console.ReadLine();
        Console.Write(" CustomerNumber (C13977)            : ");
        string? customerNumber = Console.ReadLine();
        if (string.IsNullOrEmpty(customerNumber))
        {
            customerNumber = "C13977";
        }        
        Console.Write(" ProductNumber (FAC-00011)          : ");
        string? productNumber = Console.ReadLine();
        if (string.IsNullOrEmpty(productNumber))
        {
            productNumber = "FAC-00011";
        }
        Console.Write(" Contract amount (Euros)            : ");
        decimal contractAmount = Convert.ToDecimal(Console.ReadLine()!);
        Console.Write(" Start date (yyyyMMdd)              : ");
        DateTime startDate = DateTime.ParseExact(Console.ReadLine()!, "yyyyMMdd", CultureInfo.InvariantCulture);
        Console.Write(" Contract term (years)              : ");
        int contractTerm = Convert.ToInt32(Console.ReadLine()!);
        DateTime endDate = startDate.AddYears(contractTerm);
        Console.WriteLine(" Payment period:");
        Console.WriteLine("   1. Monthly");
        Console.WriteLine("   2. Quarterly");
        Console.WriteLine("   3. Yearly");
        char paymentPeriodOption = Console.ReadKey().KeyChar;
        PaymentPeriod paymentPeriod = paymentPeriodOption switch
        {
            '1' => PaymentPeriod.Monthly,
            '2' => PaymentPeriod.Quarterly,
            '3' => PaymentPeriod.Yearly,
            _ => PaymentPeriod.Monthly
        };

        Console.WriteLine("\n\nCalling ContractManagement API ...");
        var command = new RegisterContractV2(
            Guid.NewGuid(), contractNumber!, customerNumber!, productNumber!, contractAmount, startDate, endDate, paymentPeriod);

        await CallAPI(command);
    }

    private async Task ChangeContractAmount()
    {
        Console.WriteLine("\nChangeContractAmount:");
        Console.WriteLine("-------------------");
        Console.Write(" Contractnumber (CTR-yyyyMMdd-9999) : ");
        string? contractNumber = Console.ReadLine();
        Console.Write(" New contract amount (Euros)        : ");
        decimal contractAmount = Convert.ToDecimal(Console.ReadLine()!);

        Console.WriteLine("\nCalling ContractManagement API ...");
        var command = new ChangeContractAmount(
            Guid.NewGuid(), contractNumber!, contractAmount);

        await CallAPI(command);
    }

    private async Task ChangeContractTerm()
    {
        Console.WriteLine("\nChangeContractTerm:");
        Console.WriteLine("-------------------");
        Console.Write(" Contractnumber (CTR-yyyyMMdd-9999) : ");
        string? contractNumber = Console.ReadLine();
        Console.Write(" New Start date (yyyyMMdd)          : ");
        DateTime startDate = DateTime.ParseExact(Console.ReadLine()!, "yyyyMMdd", CultureInfo.InvariantCulture);
        Console.Write(" New contract term (years)          : ");
        int contractTerm = Convert.ToInt32(Console.ReadLine()!);

        Console.WriteLine("\nCalling ContractManagement API ...");
        var command = new ChangeContractTerm(
            Guid.NewGuid(), contractNumber!, startDate, startDate.AddYears(contractTerm));

        await CallAPI(command);
    }

    private async Task CancelContract()
    {
        Console.WriteLine("\nChangeContractAmount:");
        Console.WriteLine("-------------------");
        Console.Write(" Contractnumber (CTR-yyyyMMdd-9999) : ");
        string? contractNumber = Console.ReadLine();
        Console.Write(" Reason for cancellation            : ");
        string? reason = Console.ReadLine();

        Console.WriteLine("\nCalling ContractManagement API ...");
        var command = new CancelContract(
            Guid.NewGuid(), contractNumber!, reason!);

        await CallAPI(command);
    }

    private async Task CallAPI(object command)
    {
        using var httpClient = new HttpClient();
        await httpClient.PostAsJsonAsync(
            $"https://localhost:7044/contractmanagement/command/{command.GetType().Name.ToLowerInvariant()}", command)
            .ContinueWith(r => 
                Console.WriteLine($"Done (HTTP Status Code: {(int)r.Result.StatusCode} - {r.Result.StatusCode}).\n" +
                $"{r.Result.Content.ReadAsStringAsync().Result}"));        
    }    
}
