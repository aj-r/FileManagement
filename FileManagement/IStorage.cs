using System.IO;

namespace FileManagement
{
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

        /// <summary>
        /// Gets whether an object exists in the specified location.
        /// </summary>
        /// <param name="location">A storage location (e.g. file path).</param>
        /// <returns>A boolean value indicating whether the location exists.</returns>
        bool Exists(string location);
    }
}
