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

        /// <summary>
        /// Gets the path of the file that caused the exception.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Gets whether the error occurred during a write or read operation.
        /// </summary>
        public bool Writing { get; private set; }

        /// <summary>
        /// Creates a new <see cref="FileException"/> instance.
        /// </summary>
        /// <param name="type">The cause of the file exception.</param>
        /// <param name="writing">Indicates whether the error occurred during a write or read operation.</param>
        /// <param name="filePath">The path of the file that caused the exception.</param>
        public FileException(FileExceptionType type, bool writing, string filePath)
            : this(type, writing, filePath, null)
        { }

        /// <summary>
        /// Creates a new <see cref="FileException"/> instance.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="type">The cause of the file exception.</param>
        /// <param name="writing">Indicates whether the error occurred during a write or read operation.</param>
        /// <param name="filePath">The path of the file that caused the exception.</param>
        public FileException(string message, FileExceptionType type, bool writing, string filePath)
            : this(message, type, writing, filePath, null)
        { }

        /// <summary>
        /// Creates a new <see cref="FileException"/> instance.
        /// </summary>
        /// <param name="type">The cause of the file exception.</param>
        /// <param name="writing">Indicates whether the error occurred during a write or read operation.</param>
        /// <param name="filePath">The path of the file that caused the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public FileException(FileExceptionType type, bool writing, string filePath, Exception innerException)
            : this(GetErrorMessage(type, writing), type, writing, filePath, innerException)
        { }

        /// <summary>
        /// Creates a new <see cref="FileException"/> instance.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="type">The cause of the file exception.</param>
        /// <param name="writing">Indicates whether the error occurred during a write or read operation.</param>
        /// <param name="filePath">The path of the file that caused the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public FileException(string message, FileExceptionType type, bool writing, string filePath, Exception innerException)
            : base(message, innerException)
        {
            Type = type;
            FilePath = filePath;
        }

        private static string GetErrorMessage(FileExceptionType type, bool writing)
        {
            var message = "An error occurred while " + (writing ? "writing to" : "reading") + " the file: ";
            switch (type)
            {
                case FileExceptionType.NotFound:
                    message += "File or directory not found.";
                    break;
                case FileExceptionType.InsufficientPermissions:
                    message += "Insufficient permissions to access the file.";
                    break;
                case FileExceptionType.SerializationError:
                    message += "Failed to " + (writing ? "serialize" : "deserialize") + " the object.";
                    break;
            }
            return message;
        }
    }

    /// <summary>
    /// Represents the reason why a file exception happened.
    /// </summary>
    public enum FileExceptionType
    {
        /// <summary>
        /// Indicates that the file or directory was not found.
        /// </summary>
        NotFound,
        /// <summary>
        /// Indicates that the current user does not have permission to access the file.
        /// </summary>
        InsufficientPermissions,
        /// <summary>
        /// Indicates that there was an error serializing the file.
        /// </summary>
        SerializationError
    }
}
