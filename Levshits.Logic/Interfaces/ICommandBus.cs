using Levshits.Logic.Common;

namespace Levshits.Logic.Interfaces
{
    public interface ICommandBus
    {
        ExecutionResult ExecuteCommand(RequestBase request);
        ExecutionResult<T> ExecuteCommand<T>(RequestBase request);
    }
}