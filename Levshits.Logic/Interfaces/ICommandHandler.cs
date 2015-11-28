using System.Collections.Generic;
using Levshits.Logic.Common;

namespace Levshits.Logic.Interfaces
{
    public interface ICommandHandler
    {
        int Priority { get; }
        List<string> SupportedCommands { get; }

        ExecutionResult Execute(RequestBase request);
    }
}