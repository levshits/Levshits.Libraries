using Levshits.Data.Common;

namespace Levshits.Data.Interfaces
{
    public interface ICommandBus
    {
        ExecutionResult ExecuteCommand(RequestBase request);
        ExecutionResult<T> ExecuteCommand<T>(RequestBase request);
    }
}