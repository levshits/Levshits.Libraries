using System.Collections.Generic;
using Levshits.Data.Common;

namespace Levshits.Data.Interfaces
{
    public interface ICommandHandler
    {
        int Priority { get; }
        List<string> SupportedCommands { get; }

        ExecutionResult Execute(RequestBase request, ExecutionContext context);
    }
}