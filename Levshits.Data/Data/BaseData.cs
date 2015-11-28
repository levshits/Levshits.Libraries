using System;
using System.Collections;
using System.Collections.Generic;
using Levshits.Data.Common;
using Levshits.Data.Entity;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Engine;

namespace Levshits.Data.Data
{
    /// <summary>
    ///     Represents base class for all Dao objects.
    /// </summary>
    public abstract class BaseData
    {
        /// <summary>
        ///     Gets/Sets <see cref="DataProvider" />.
        /// </summary>
        protected DataProvider DataProvider { get; }

        protected BaseData(DataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }
    }

    /// <summary>
    ///     Represents base class for all Dao objects.
    /// </summary>
    /// <typeparam name="T">Type of entity for Dao object</typeparam>
    public abstract class BaseData<T> : BaseData
        where T : BaseEntity
    {
        protected BaseData(DataProvider dataProvider)
            : base(dataProvider)
        {
        }

        /// <summary>
        ///     Gets entity by identyfier property.
        /// </summary>
        public virtual T GetEntityById(object id)
        {
            if (id == null)
            {
                return null;
            }
            return DataProvider.Get<T>(id);
        }

        /// <summary>
        ///     Saves or updates entity to database.
        /// </summary>
        public virtual object Save(T entity)
        {
            return DataProvider.Save(entity);
        }

        /// <summary>
        ///     Deletes entity from database.
        /// </summary>
        /// <param name="id">Id of entity</param>
        /// <returns>True </returns>
        public virtual bool Delete(object id)
        {
            var entity = GetEntityById(id);
            if (entity == null)
            {
                return false;
            }
            Delete(entity);
            return true;
        }

        /// <summary>
        ///     Deletes entity from database.
        /// </summary>
        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            DataProvider.Delete(entity);
        }


        /// <summary>
        ///     Adds <see cref="paging" /> restrictions to <see cref="query" />.
        /// </summary>
        protected void AddPaging<TE>(IQueryOver<TE, TE> query, PagingOptions paging, bool distinct = false)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if (paging == null)
            {
                return;
            }

            query.Take(paging.ItemsPerPage);
            query.Skip(paging.ItemsPerPage*paging.Page);

            var rowQuery = distinct ? ToDistinctRowCount(query) : query.ToRowCountQuery();
            paging.ItemsCount = rowQuery.FutureValue<int>().Value;
        }

        /// <summary>
        ///     Creates row count query (with unique identifier)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private IQueryOver<TE> ToDistinctRowCount<TE>(IQueryOver<TE, TE> query)
        {
            return query.Clone()
                .Select(Projections.CountDistinct<T>(x => x.Id))
                .ClearOrders()
                .Skip(0)
                .Take(RowSelection.NoValue);
        }

        /// <summary>
        ///     Returns list of <see cref="T" /> entities.
        /// </summary>
        /// <returns></returns>
        public IList<T> List()
        {
            return DataProvider.QueryOver<T>().List<T>();
        }


        /// <summary>
        ///     Returns list of <see cref="T" /> entities with specific ids.
        /// </summary>
        public virtual IList<T> List(ICollection ids)
        {
            if (ids != null && ids.Count > 0)
            {
                return DataProvider.QueryOver<T>().WhereRestrictionOn(x => x.Id).IsIn(ids).List<T>();
            }
            return new List<T>();
        }

        /// <summary>
        ///     Checks if the field is unique in database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool IsUniqueField(int id, string fieldName, string value)
        {
            var query = DataProvider.QueryOver<T>();
            query.Where(x => x.Id != id).And(Restrictions.Eq(fieldName, value));
            return query.ToRowCountQuery().FutureValue<int>().Value == 0;
        }
    }
}
