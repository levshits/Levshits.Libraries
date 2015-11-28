using System;
using System.Collections.Generic;
using System.Linq;
using Levshits.Logic.Interfaces;
using Spring.Context;
using Spring.Context.Support;

namespace Levshits.Logic.Common
{
    public class CommandBus: ICommandBus
    {

        /// <summary>
        /// Gets or sets the application context.
        /// </summary>
        /// <value>The application context.</value>
        public IApplicationContext ApplicationContext => ContextRegistry.GetContext();

        private IList<ICommandHandler> _commandHandlers;

        /// <summary>
        ///     Gets/Sets the list of <see cref="ICommandHandler"/>.
        /// </summary>
        public IList<ICommandHandler> CommandHandlers
        {
            get
            {
                return _commandHandlers ?? (_commandHandlers = ApplicationContext
                    .GetObjects<ICommandHandler>()
                    .Values.OrderByDescending(x => x.Priority)
                    .ToList());
            }
        }
        public ExecutionResult ExecuteCommand(RequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var handler = CommandHandlers.FirstOrDefault(v => v.SupportedCommands.Contains(request.Id));

            if (handler == null)
            {
                throw new NotSupportedException(String.Format("{0} command request is not supported.", request));
            }

            return handler.Execute(request) ?? new ExecutionResult();
        }

        public ExecutionResult<T> ExecuteCommand<T>(RequestBase request)
        {
            return new ExecutionResult<T>(ExecuteCommand(request));
        }
    }
}
