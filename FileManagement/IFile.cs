using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement
{
    /// <summary>
    /// Represents an object that can be loaded from or saved to a form of storage.
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// Gets or sets the storage location (i.e. file path) where the object was loaded from or saved to.
        /// </summary>
        /// <remarks>
        /// A value of null indicates that the object is newly created and has not yet been saved to any file.
        /// </remarks>
        string StorageLocation { get; set; }
    }
}
