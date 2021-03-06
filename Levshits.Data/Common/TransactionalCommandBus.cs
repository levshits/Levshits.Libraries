﻿using System;
using Common.Logging;
using Levshits.Data.Interfaces;

namespace Levshits.Data.Common
{
    public class TransactionalCommandBus: ICommandBus
    {
        private readonly ICommandBus _commandBus;
        private readonly IDataProvider _dataProvider;
        private readonly ILog _log = LogManager.GetLogger(typeof (TransactionalCommandBus));
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
                _log.Error(e);
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
                _log.Error(e);
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
