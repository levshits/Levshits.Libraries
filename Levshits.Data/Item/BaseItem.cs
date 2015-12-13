using System;

namespace Levshits.Data.Item
{
    /// <summary>
    /// Class BaseItem.
    /// </summary>
    public class BaseItem
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public virtual int Version { get; set; } 
    }
}