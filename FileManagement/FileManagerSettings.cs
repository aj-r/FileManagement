using System;
using System.Text;

namespace FileManagement
{
    /// <summary>
    /// Contains settings for a <see cref="FileManager"/>.
    /// </summary>
    public class FileManagerSettings
    {
        private static Func<FileManagerSettings> @default = () => new FileManagerSettings();

        /// <summary>
        /// Gets or sets a function that defines the default settings.
        /// </summary>
        public static Func<FileManagerSettings> Default
        {
            get { return @default; }
            set { @default = value; }
        }

        /// <summary>
        /// Creates a new <see cref="FileManagerSettings"/> instance.
        /// </summary>
        public FileManagerSettings()
        {
            IsHistoryEnabled = true;
            RecentFilesStoragePath = "recent.txt";
            Encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Gets or sets the encoding to use for serialization. The default is UTF-8.
        /// </summary>
        public Encoding Encoding { get; set; }

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
    }
}
