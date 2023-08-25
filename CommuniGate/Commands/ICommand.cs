using CommuniGate.Bases;

namespace CommuniGate.Commands;

public interface ICommand 
{
}

public interface ICommand<out TResult> : ICommunication
{

}