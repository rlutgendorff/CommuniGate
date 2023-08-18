namespace CommuniGate.Commands;

public interface IBaseCommand : ICommunication
{
}

public interface ICommand : IBaseCommand
{
}

public interface ICommand<out TResult> : IBaseCommand
{

}