using System;
using System.Data;
using System.Linq.Expressions;
using Common.Logging;
using Levshits.Data.Common;
using Levshits.Data.Entity;
using NHibernate;
using NHibernate.Linq;

namespace Levshits.Data
{
    /// <summary>
    /// 	Represents provider class for working with database.
    /// </summary>
    public abstract class DataProvider : IDataProvider
    {
        private readonly ISessionFactory _factory;
        private readonly ISessionStorage _storage;
        private readonly ILog _log = LogManager.GetLogger(typeof (DataProvider));

        /// <summary>
        /// 	Creates new instance of <see cref = "DataProvider" />.
        /// </summary>
        public DataProvider(ISessionStorage storage)
        {
            _storage = storage;
            _factory = InitFactory();
            
        }

        public abstract ISessionFactory InitFactory();

        /// <summary>
        /// 	Returns current NHibernate session or opens new one.
        /// </summary>
        private ISession OpenSession()
        {
            var session = _storage.CurrentSession;
            if (session == null || !session.IsOpen)
            {
                session = _factory.OpenSession();
                session.FlushMode = FlushMode.Never;
                session.BeginTransaction(IsolationLevel.ReadCommitted);
                _storage.CurrentSession = session;
            }
            return session;
        }

        /// <summary>
        /// 	Close current NHibernate session.
        /// </summary>
        public void CloseSession()
        {
            var session = _storage.CurrentSession;
            if (session != null && session.IsOpen)
            {
                session.Close();
                _storage.CurrentSession = null;
            }
        }

        /// <summary>
        /// 	Commits NHibernate transaction.
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                var session = _storage.CurrentSession;
                if (session != null && session.Transaction.IsActive)
                {
                    session.Transaction.Commit();
                }
            }
            catch(Exception e)
            {
                _log.Error(e);
                RollbackTransaction();
                throw;
            }
        }

        /// <summary>
        /// 	Rollbacks NHibernate transaction and close session.
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                var session = _storage.CurrentSession;
                if (session != null && session.Transaction.IsActive)
                {
                    session.Transaction.Rollback();
                }
            }
            finally
            {
                CloseSession();
            }
        }

        /// <summary>
        ///     Completely clears the session. Evict all loaded instances and cancel pending saves, updates and deletes.
        /// </summary>
        public void Clear()
        {
            var session = _storage.CurrentSession;
            if (session != null && session.IsOpen)
            {
                session.Clear();
            }
        }

        /// <summary>
        /// 	Creates root criteria for current session.
        /// </summary>
        public ICriteria CreateCriteria<T>()
            where T : BaseEntity
        {
            var session = OpenSession();
            return session.CreateCriteria(typeof(T));
        }

        /// <summary>
        ///     Forces current session to flush.
        /// </summary>
        public void Flush()
        {
            var session = _storage.CurrentSession;
            if (session != null && session.IsOpen)
            {
                session.Flush();
            }
        }

        /// <summary>
        /// 	Saves entity to database.
        /// </summary>
        public object Save<T>(T entity)
            where T : BaseEntity
        {
            var session = OpenSession();
            var id = session.Save(entity);
            session.Flush();
            return id;
        }

        /// <summary>
        /// 	Saves entity to database.
        /// </summary>
        public void Delete<T>(T entity)
            where T : BaseEntity
        {
            var session = OpenSession();
            session.Delete(entity);
            session.Flush();
        }

        public IQueryOver<T, T> QueryOver<T>() where T : BaseEntity
        {
            return OpenSession().QueryOver<T>();
        }

        public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : BaseEntity
        {
            return OpenSession().QueryOver(alias);
        }

        public IQueryOver<T, T> QueryOver<T>(global::NHibernate.Criterion.QueryOver<T> detachedQuery) where T : BaseEntity
        {
            return detachedQuery.GetExecutableQueryOver(OpenSession());
        }

        public System.Linq.IQueryable<T> Query<T>()
            where T : BaseEntity
        {
            return OpenSession().Query<T>();
        }


        /// <summary>
        ///     Return the persistent instance of the <see cref="T"/> entity with the given identifier, or null if there is no such persistent instance. (If the instance, or a proxy for the instance, is already associated with the session, return that instance or proxy.)
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="id">an identifier</param>
        /// <returns>a persistent instance or null</returns>
        public T Get<T>(object id)
            where T : BaseEntity
        {
            return OpenSession().Get<T>(id);
        }

        /// <summary>
        ///     Gets new stateless session. Can be used only in specific cases.
        /// </summary>
        /// <returns></returns>
        public IStatelessSession OpenStatelessSession()
        {
            return _factory.OpenStatelessSession();
        }

        /// <summary>
        /// Execute raw sql query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ISQLQuery CreateSqlQuery(string query)
        {
            return OpenSession().CreateSQLQuery(query);
        }
    }
}
