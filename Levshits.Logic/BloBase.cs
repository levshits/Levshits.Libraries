using System;
using System.Collections.Generic;
using System.Linq;
using Levshits.Data.Entity;
using Levshits.Logic.Common;
using Levshits.Logic.Interfaces;

namespace Levshits.Logic
{
    public abstract class BloBase: ICommandHandler
    {
        protected BloBase(Repository repository)
        {
            Repository = repository;
            Commands = new Dictionary<string, Func<RequestBase, ExecutionResult>>();
        }

        public abstract void Init();
        public Dictionary<string, Func<RequestBase, ExecutionResult>> Commands { get; private set; }

        protected Repository Repository { get; private set; }

        protected virtual void RegisterCommand<T>(Func<T, ExecutionResult> command)where T : RequestBase
        {
            Commands.Add(typeof(T).Name, (request) => command((T)request));
        }

        public abstract int Priority { get; }
        public virtual List<string> SupportedCommands => Commands.Keys.ToList();
        /// <summary>
        ///     Executes command or query according request.
        /// </summary>
        public ExecutionResult Execute(RequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            Func<RequestBase, ExecutionResult> handler;
            if (Commands.TryGetValue(request.Id, out handler))
            {
                return handler.Invoke(request);
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