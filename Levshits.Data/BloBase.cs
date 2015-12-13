using System;
using System.Collections.Generic;
using System.Linq;
using Levshits.Data.Common;
using Levshits.Data.Entity;
using Levshits.Data.Interfaces;

namespace Levshits.Data
{
    public abstract class BloBase: ICommandHandler
    {
        protected BloBase(Repository repository)
        {
            Repository = repository;
            Commands = new Dictionary<string, Func<RequestBase, ExecutionContext, ExecutionResult>>();
        }

        public abstract void Init();
        public Dictionary<string, Func<RequestBase, ExecutionContext, ExecutionResult>> Commands { get; private set; }

        protected Repository Repository { get; private set; }

        protected virtual void RegisterCommand<T>(Func<T, ExecutionContext, ExecutionResult> command)where T : RequestBase
        {
            Commands.Add(typeof(T).Name, (request, context) => command((T)request, context));
        }

        public abstract int Priority { get; }
        public virtual List<string> SupportedCommands => Commands.Keys.ToList();

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="context">The context.</param>
        /// <returns>Levshits.Logic.Common.ExecutionResult.</returns>
        public ExecutionResult Execute(RequestBase request, ExecutionContext context)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            Func<RequestBase, ExecutionContext, ExecutionResult> handler;
            if (Commands.TryGetValue(request.Id, out handler))
            {
                return handler.Invoke(request, context);
            }

            throw new NotSupportedException($"Request {request.Id} is not supported");
        }
    }
    public abstract class BloBase<T> : BloBase
        where T : BaseEntity, new()
    {
        protected BloBase(Repository repository)
            : base(repository)
        {
        }

        public virtual T CreateEntity()
        {
            var entity = new T();
            return entity;
        }
    }
}