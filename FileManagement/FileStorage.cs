using System.IO;

namespace FileManagement
{
    /// <summary>
    /// Handles reading and writing data to a file.
    /// </summary>
    public class FileStorage : IStorage
    {
        /// <summary>
        /// Gets a read-only stream for the specified file.
        /// </summary>
        /// <param name="location">The path to a file.</param>
        /// <returns>A file stream.</returns>
        public Stream GetReadStream(string location)
        {
            if (location == null)
                return null;
            return new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Gets a write stream to the specified file. If the file or directory do not exist, they will be created.
        /// </summary>
        /// <param name="location">The path to a file.</param>
        /// <returns>A file stream.</returns>
        public Stream GetWriteStream(string location)
        {
            if (location == null)
                return null;
            try
            {
                return new FileStream(location, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(location));
                return new FileStream(location, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            }
        }
    }
}
