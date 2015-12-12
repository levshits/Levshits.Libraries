using System;

namespace Levshits.Data.Dto
{
    [Serializable]
    public class DtoBase
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public int Version { get; set; }
         
    }
}