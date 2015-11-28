using System;
using System.Web;
using Levshits.Data.Common;
using NHibernate;

namespace Levshits.Data.Storage
{
    /// <summary>
    ///     Represents http <see cref="ISession"/> storage.
    /// </summary
    public class HttpSessionStorage : ISessionStorage
    {
        private const string SessionKey = "9C04AB24-18EE-488B-917D-66D2CA19E36D";
        
        private HttpContext GetCurrentContext()
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("HttpContext is null.");
            }
            return HttpContext.Current;
        }

        /// <summary>
        /// 	Gets/Sets current NHibernate session.
        /// </summary>
        public ISession CurrentSession
        {
            get
            {
                return (ISession)GetCurrentContext().Items[SessionKey];
            }
            set
            {
                GetCurrentContext().Items[SessionKey] = value;
            }
        }
    }
}
