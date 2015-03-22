using System.IO;

namespace FileManagement
{
    /// <summary>
    /// Handles reading and writing to a form of persistent storage.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Gets a stream with read-only access to the file at the specified location.
        /// </summary>
        /// <param name="location">A storage location (e.g. file path).</param>
        /// <returns>A stream.</returns>
        Stream GetReadStream(string location);

        /// <summary>
        /// Gets a stream with write-only access to the file at the specified location.
        /// </summary>
        /// <param name="location">A storage location (e.g. file path).</param>
        /// <returns>A stream.</returns>
        Stream GetWriteStream(string location);
    }
}
