using System;
using Levshits.Data.Common;
using Levshits.Logic.Interfaces;

namespace Levshits.Logic.Common
{
    public class TransactionalCommandBus: ICommandBus
    {
        private readonly ICommandBus _commandBus;
        private readonly IDataProvider _dataProvider;
        public TransactionalCommandBus(ICommandBus commandBus, IDataProvider dataProvider)
        {
            _commandBus = commandBus;
            _dataProvider = dataProvider;
        }

        public ExecutionResult ExecuteCommand(RequestBase request)
        {
            try
            {
                var result = _commandBus.ExecuteCommand(request);
                _dataProvider.CommitTransaction();
                return result;
            }
            catch(Exception e)
            {
                _dataProvider.RollbackTransaction();
                return new ExecutionResult() { Success = false };
            }
            finally
            {
                _dataProvider.CloseSession();
            }
        }

        public ExecutionResult<T> ExecuteCommand<T>(RequestBase request)
        {
            try
            {
                var result = _commandBus.ExecuteCommand<T>(request);
                _dataProvider.CommitTransaction();
                return result;
            }
            catch (Exception e)
            {
                _dataProvider.RollbackTransaction();
               return new ExecutionResult<T>() {Success = false};
            }
            finally
            {
                _dataProvider.CloseSession();
            }
        }
    }
}
