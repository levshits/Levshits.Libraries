using System.Collections.Generic;
using Levshits.Data.Common.Errors;

namespace Levshits.Data.Common
{
    public class ExecutionResult
    {
        public bool Success { get; set; }
        public object Result { get; set; }
        public List<ErrorBase> Errors { get; set; }

        public ExecutionResult()
        {
            Success = true;
            Errors = new List<ErrorBase>();
        }
    }

    public class ExecutionResult<T> : ExecutionResult
    {
        public ExecutionResult()
        {
        }
        public ExecutionResult(ExecutionResult executionResult)
        {
            Success = executionResult.Success;
            Result = (T)executionResult.Result;
            Errors = executionResult.Errors;
        }

        public T TypedResult { get { return (T) Result; } set { Result = value; } }   
    }
}