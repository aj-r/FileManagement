using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileManagement
{
	public class FileManager : IFileManager
	{
        public FileManager(IStorage storage)
        {
            Storage = storage;
            IsHistoryEnabled = true;
        }

        /// <summary>
        /// Gets the storage object used to retrieve file streams.
        /// </summary>
        protected IStorage Storage { get; private set; }

        /// <summary>
        /// Gets or sets whether recent document history is enabled.
        /// </summary>
        /// <remarks>
        /// This property is true by default.
        /// </remarks>
        public bool IsHistoryEnabled { get; set; }

        /// <summary>
        /// Gets or sets the path to the application settings folder.
        /// </summary>
        public string SettingsPath { get; set; }

        /// <summary>
        /// Gets the path to the file that contains the recent file history.
        /// </summary>
        /// <returns></returns>
		protected virtual string GetRecentFilePath()
		{
            return string.IsNullOrEmpty(SettingsPath) ? "recent.txt" : Path.Combine(SettingsPath, "recent.txt");
		}

		/// <summary>
		/// Saves the list of recently opened files.
		/// </summary>
        /// <param name="recentFileList">The list to save.</param>
        /// <exception cref="FileManagement.FileException">Occurs if there was an error saving the file.</exception>
        public virtual void SaveRecentFiles(IRecentFileList recentFileList)
        {
            if (!IsHistoryEnabled)
                return;
            string recentFilePath = GetRecentFilePath();

            using (var stream = Storage.GetWriteStream(recentFilePath))
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

		/// <summary>
		/// Loads the list of recently opened files.
		/// </summary>
        /// <param name="recentFileList">The list to save.</param>
        /// <exception cref="FileManagement.FileException">Occurs if there was an error reading the file.</exception>
        public virtual IRecentFileList GetRecentFiles()
        {
            string recentFilePath = GetRecentFilePath();
			if (!Storage.Exists(recentFilePath))
                return new RecentFileList();

			// Enqueue the files in backwards order - Enqueue adds the item to the front of the queue,
			// so the last item equeued will be first, so we must add the first item last.
            var filePaths = new List<string>();
            using (var stream = Storage.GetReadStream(recentFilePath))
            {
                if (stream == null)
                    return new RecentFileList();
                using (var reader = new StreamReader(stream))
                    while (!reader.EndOfStream)
                        filePaths.Add(reader.ReadLine());
            }

			return new RecentFileList(filePaths);
        }
        
        /// <summary>
        /// Saves an object to a file.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize in the file.</typeparam>
        /// <param name="obj">The object to save.</param>
        /// <exception cref="FileManagement.FileException">Occurs if there was an error saving the file.</exception>
        public void Save<T>(T obj, ISerializer serializer) where T : IFile
        {
            Save(obj, serializer, true);
        }

        /// <summary>
        /// Saves an object to a file.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize in the file.</typeparam>
        /// <param name="obj">The object to save.</param>
        /// <param name="includeInHistory">Indicates whether the saved file should be added to the recent document history (if history is enabled).</param>
        /// <exception cref="FileManagement.FileException">Occurs if there was an error saving the file.</exception>
        public void Save<T>(T obj, ISerializer serializer, bool includeInHistory) where T : IFile
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            if (obj.FilePath == null)
                throw new ArgumentException("You must set the FilePath before saving the object.", "obj");
            
            try
            {
                using (var stream = Storage.GetWriteStream(obj.FilePath))
                {
                    try
                    {
                        serializer.Serialize(stream, obj);
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
                list.Add(obj.FilePath);
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
        public T Load<T>(string filePath, ISerializer serializer) where T : IFile
        {
            return Load<T>(filePath, serializer, true);
        }

        /// <summary>
        /// Loads an object from a file.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize from the file.</typeparam>
        /// <param name="filePath">The location of the file to load.</param>
        /// <param name="includeInHistory">Indicates whether the saved file should be added to the recent document history (if history is enabled).</param>
        /// <returns>The deserialized object.</returns>
        /// <exception cref="FileManagement.FileException">Occurs if there was an error reading the file.</exception>
        public T Load<T>(string filePath, ISerializer serializer, bool includeInHistory) where T : IFile
        {
            T obj;

            try
            {
                using (var stream = Storage.GetReadStream(filePath))
                {
                    try
                    {
                        obj = serializer.Deserialize<T>(stream);
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
                obj.FilePath = filePath;
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
