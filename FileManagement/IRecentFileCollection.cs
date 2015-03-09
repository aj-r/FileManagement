using System.Collections.Generic;

namespace FileManagement
{
    /// <summary>
    /// Represents a collection of recently accessed files.
    /// </summary>
    public interface IRecentFileCollection : ICollection<string>
    {
		/// <summary>
        /// Gets or sets the maximum length of the collection.
		/// </summary>
        int MaxLength { get; set; }
    }
}
