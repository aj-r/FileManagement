using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileManagement
{
    /// <summary>
    /// Provides basic file management for client applications.
    /// </summary>
	public class FileManager : IFileManager
    {
        public FileManager(ISerializer serializer)
            : this(serializer, new FileStorage())
        { }

        public FileManager(ISerializer serializer, IStorage storage)
        {
            Storage = storage;
            Serializer = serializer;
            IsHistoryEnabled = true;
            RecentFilesStoragePath = "recent.txt";
        }

        /// <summary>
        /// Gets the storage object used to retrieve file streams.
        /// </summary>
        protected IStorage Storage { get; private set; }

        /// <summary>
        /// Gets the storage object used to serialize and deserialize files.
        /// </summary>
        protected ISerializer Serializer { get; private set; }

        /// <summary>
        /// Gets or sets whether recent document history is enabled.
        /// </summary>
        /// <remarks>
        /// This property is true by default.
        /// </remarks>
        public bool IsHistoryEnabled { get; set; }

        /// <summary>
        /// Gets or sets the path to the file that contains the recent file history.
        /// </summary>
        public string RecentFilesStoragePath { get; set; }

		/// <summary>
		/// Saves the list of recently opened files.
		/// </summary>
        /// <param name="recentFileList">The list to save.</param>
        /// <exception cref="FileManagement.FileException">Occurs if there was an error saving the file.</exception>
        public virtual void SaveRecentFiles(IRecentFileCollection recentFileList)
        {
            try
            {
                using (var stream = Storage.GetWriteStream(RecentFilesStoragePath))
                {
                    if (stream == null)
                        return;
                    using (var writer = new StreamWriter(stream))
                    {
                        foreach (string filePath in recentFileList)
                            writer.WriteLine(filePath);
                        writer.Close();
                    }
                }
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { }
            catch (IOException) { }
        }

		/// <summary>
		/// Loads the list of recently opened files.
		/// </summary>
        /// <param name="recentFileList">The list to save.</param>
        /// <exception cref="FileManagement.FileException">Occurs if there was an error reading the file.</exception>
        public virtual IRecentFileCollection GetRecentFiles()
        {
            var files = new RecentFileCollection();
            try
            {
                using (var stream = Storage.GetReadStream(RecentFilesStoragePath))
                {
                    if (stream == null)
                        return files;
                    using (var reader = new StreamReader(stream))
                        while (!reader.EndOfStream)
                            files.Add(reader.ReadLine());
                }
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { }
            catch (IOException) { }

            return files;
        }
        
        /// <summary>
        /// Saves an object to a file.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize in the file.</typeparam>
        /// <param name="obj">The object to save.</param>
        /// <exception cref="FileManagement.FileException">Occurs if there was an error saving the file.</exception>
        public void Save<T>(T obj) where T : IFile
        {
            Save(obj, true);
        }

        /// <summary>
        /// Saves an object to a file.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize in the file.</typeparam>
        /// <param name="obj">The object to save.</param>
        /// <param name="includeInHistory">Indicates whether the saved file should be added to the recent document history (if history is enabled).</param>
        /// <exception cref="FileManagement.FileException">Occurs if there was an error saving the file.</exception>
        public void Save<T>(T obj, bool includeInHistory) where T : IFile
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (obj.StorageLocation == null)
                throw new ArgumentException("You must set the FilePath before saving the object.", "obj");
            
            try
            {
                using (var stream = Storage.GetWriteStream(obj.StorageLocation))
                {
                    try
                    {
                        Serializer.Serialize(stream, obj);
                    }
                    catch (Exception ex)
                    {
                        throw new FileException(FileExceptionType.SerializationError, ex);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new FileException(FileExceptionType.NotFound, ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new FileException(FileExceptionType.NotFound, ex);
            }
            catch (IOException ex)
            {
                throw new FileException(FileExceptionType.InsufficientPermissions, ex);
            }

            if (IsHistoryEnabled)
            {
                var list = GetRecentFiles();
                list.Add(obj.StorageLocation);
                SaveRecentFiles(list);
            }
        }
        
        /// <summary>
        /// Loads an object from a file.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize from the file.</typeparam>
        /// <param name="filePath">The location of the file to load.</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="FileManagement.FileException">Occurs if there was an error reading the file.</exception>
        public T Load<T>(string filePath) where T : IFile
        {
            return Load<T>(filePath, true);
        }

        /// <summary>
        /// Loads an object from a file.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize from the file.</typeparam>
        /// <param name="filePath">The location of the file to load.</param>
        /// <param name="includeInHistory">Indicates whether the saved file should be added to the recent document history (if history is enabled).</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="FileManagement.FileException">Occurs if there was an error reading the file.</exception>
        public T Load<T>(string filePath, bool includeInHistory) where T : IFile
        {
            T obj;

            try
            {
                using (var stream = Storage.GetReadStream(filePath))
                {
                    try
                    {
                        obj = Serializer.Deserialize<T>(stream);
                    }
                    catch (Exception ex)
                    {
                        throw new FileException(FileExceptionType.SerializationError, ex);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new FileException(FileExceptionType.NotFound, ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new FileException(FileExceptionType.NotFound, ex);
            }
            catch (IOException ex)
            {
                throw new FileException(FileExceptionType.InsufficientPermissions, ex);
            }

            if (obj != null)
            {
                obj.StorageLocation = filePath;
            }

            if (IsHistoryEnabled)
            {
                var list = GetRecentFiles();
                list.Add(filePath);
                SaveRecentFiles(list);
            }

            return obj;
        }

        private static readonly Regex invalidCharExpr = new Regex(string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars()))));

        /// <summary>
        /// Removes all characters from the string that cannot be used in a file name.
        /// </summary>
        /// <param name="s">The string to remove invalid characters from.</param>
        /// <returns>A string with all invalid characters stripped from it.</returns>
        public static string RemoveInvalidFileNameCharacters(string s)
        {
            return s == null ? invalidCharExpr.Replace(s, string.Empty) : null;
        }
	}
}
