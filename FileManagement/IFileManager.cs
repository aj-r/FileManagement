using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement
{
    /// <summary>
    /// Provides basic file management for client applications.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Saves an object to a file.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize in the file.</typeparam>
        /// <param name="obj">The object to save.</param>
        void Save<T>(T obj) where T : IFile;

        /// <summary>
        /// Saves an object to a file.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize in the file.</typeparam>
        /// <param name="obj">The object to save.</param>
        /// <param name="includeInHistory">Indicates whether the file should be added to the recent document history (if history is enabled).</param>
        void Save<T>(T obj, bool includeInHistory) where T : IFile;

        /// <summary>
        /// Loads an object from a file.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize from the file.</typeparam>
        /// <param name="filePath">The location of the file to load.</param>
        /// <returns>The deserialized object.</returns>
        T Load<T>(string filePath) where T : IFile;

        /// <summary>
        /// Loads an object from a file.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize from the file.</typeparam>
        /// <param name="filePath">The location of the file to load.</param>
        /// <returns>The deserialized object.</returns>
        /// <param name="includeInHistory">Indicates whether the file should be added to the recent document history (if history is enabled).</param>
        T Load<T>(string filePath, bool includeInHistory) where T : IFile;

		/// <summary>
		/// Saves the list of recently opened files.
		/// </summary>
		/// <param name="recentFileList">The list to save.</param>
		void SaveRecentFiles(IRecentFileCollection recentFileList);

		/// <summary>
		/// Loads the list of recently opened files.
		/// </summary>
        IRecentFileCollection GetRecentFiles();
    }
}
