namespace Levshits.Data.Common
{
    public class PagingOptions
    {
        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>The page.</value>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the items per page.
        /// </summary>
        /// <value>The items per page.</value>
        public int ItemsPerPage { get; set; } = 10;

        /// <summary>
        /// Gets or sets the items count.
        /// </summary>
        /// <value>The items count.</value>
        public int ItemsCount { get; set; }
    }
}