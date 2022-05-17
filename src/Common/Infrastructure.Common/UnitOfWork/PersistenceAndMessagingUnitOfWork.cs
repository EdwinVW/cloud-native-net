namespace Infrastructure.Common.UnitOfWork;

public class PersistenceAndMessagingUnitOfWork : IUnitOfWork
{
    private const int SQL_ERROR_KEY_CONSTRAINT_VIOLATION = 2627;

    private List<Event> _domainEvents = new List<Event>();
    private List<Event> _integrationEvents = new List<Event>();

    private readonly DbContext _dbContext;

    private readonly IEventPublisher _eventPublisher;

    public PersistenceAndMessagingUnitOfWork(DbContext dbContext, IEventPublisher eventPublisher)
    {
        this._dbContext = dbContext;
        this._eventPublisher = eventPublisher;
    }

    public void AddDomainEventToPublish(Event domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void AddIntegrationEventToPublish(Event integrationEvent)
    {
        _integrationEvents.Add(integrationEvent);
    }    

    public void AddDomainEventsToPublish(IEnumerable<Event> domainEvents)
    {
        _domainEvents.AddRange(domainEvents);
    }

    public void AddIntegrationEventsToPublish(IEnumerable<Event> integrationEvents)
    {
        _integrationEvents.AddRange(integrationEvents);
    }     

    public async Task CommitAsync()
    {
        // persist data
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException exception) when
            (exception.InnerException is SqlException { Number: SQL_ERROR_KEY_CONSTRAINT_VIOLATION })
        {
            // TODO What should we put in these error messages?
            throw new ConcurrencyException("", exception);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConcurrencyException("Version conflict.", exception);
        }

        // publish events
        _domainEvents.ForEach(async e => await _eventPublisher.PublishDomainEventAsync(e));
        _integrationEvents.ForEach(async e => await _eventPublisher.PublishIntegrationEventAsync(e));
    }
}
