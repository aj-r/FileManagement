using System.Collections.Generic;

namespace FileManagement
{
    public interface IRecentFileList : ICollection<string>
    {
		/// <summary>
		/// Adds a file path to the beginning of the list. If the file path already exists in the list,
		/// it is moved to the front of the list. If the maximum length is exceeded, the list will
		/// automatically remove the least recent items until the maximum length is no longer exceeded.
		/// </summary>
		/// <param name="filePath">The file path to add.</param>
		void Add(string filePath);

		/// <summary>
		/// Gets or sets the maximum length of the list.
		/// </summary>
        int MaxLength { get; set; }
    }
}
