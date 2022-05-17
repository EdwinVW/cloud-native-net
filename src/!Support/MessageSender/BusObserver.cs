using MassTransit;

public class BusObserver : IBusObserver
{
    public bool BusHasStarted { get; private set; } = false;

    public void PostCreate(IBus bus)
    {
    }

    public void CreateFaulted(Exception exception)
    {
    }

    public Task PreStart(IBus bus)
    {
        return Task.CompletedTask;
    }

    public Task PostStart(IBus bus, Task<BusReady> busReady)
    {
        BusHasStarted = true;
        return Task.CompletedTask;
    }

    public Task StartFaulted(IBus bus, Exception exception)
    {
        return Task.CompletedTask;
    }

    public Task PreStop(IBus bus)
    {
        return Task.CompletedTask;
    }

    public Task PostStop(IBus bus)
    {
        return Task.CompletedTask;
    }

    public Task StopFaulted(IBus bus, Exception exception)
    {
        return Task.CompletedTask;
    }
}
