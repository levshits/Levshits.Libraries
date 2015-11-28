using NHibernate;

namespace Levshits.Data.Common
{
    public interface ISessionStorage
    {
        /// <summary>
        /// Gets or sets the current session.
        /// </summary>
        /// <value>The current session.</value>
        ISession CurrentSession { get; set; }
    }
}