namespace Application.Common.Interfaces;

public interface ICommandHandler<TCommand> where TCommand : Command
{
    ValueTask HandleAsync(TCommand command);
}
