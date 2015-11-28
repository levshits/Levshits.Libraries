namespace Levshits.Data.Common
{
    public interface IDataProvider
    {
        /// <summary>
        /// Closes the session.
        /// </summary>
        void CloseSession();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        void RollbackTransaction();
    }
}