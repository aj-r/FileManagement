using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement
{
    /// <summary>
    /// Represents an error that occurred while reading or writing to a file.
    /// </summary>
    public class FileException : Exception
    {
        /// <summary>
        /// Gets the cause of the file exception.
        /// </summary>
        public FileExceptionType Type { get; private set; }

        public FileException(FileExceptionType type)
            : this(type, null)
        { }

        public FileException(string message, FileExceptionType type)
            : this(message, type, null)
        { }

        public FileException(FileExceptionType type, Exception innerException)
            : this("An error occurred while reading or writing to the file.", type, innerException)
        { }

        public FileException(string message, FileExceptionType type, Exception innerException)
            : base(message, innerException)
        {
            Type = type;
        }
    }

    public enum FileExceptionType
    {
        NotFound,
        InsufficientPermissions,
        SerializationError
    }
}
