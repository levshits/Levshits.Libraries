﻿using System;
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
            var handlers = CommandHandlers
                .Where(v => v.SupportedCommands
                .Contains(request.Id)).OrderBy(x=>x.Priority);

            if (!handlers.Any())
            {
                throw new NotSupportedException(String.Format("{0} command request is not supported.", request));
            }

            ExecutionContext context = new ExecutionContext();
            foreach (var handler in handlers)
            {
                context.PreviousResult = handler.Execute(request, context);
                if (context.PreviousResult == null)
                {
                    throw new InvalidOperationException(string.Format("{0} return null result for {1}", handler.GetType().Name, request.Id));
                }
                if (!context.PreviousResult.Success)
                {
                    return context.PreviousResult;
                }
            }
            return context.PreviousResult ?? new ExecutionResult();
        }

        public ExecutionResult<T> ExecuteCommand<T>(RequestBase request)
        {
            return new ExecutionResult<T>(ExecuteCommand(request));
        }
    }
}
