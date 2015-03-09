using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement
{
    public interface IFile
    {
        /// <summary>
        /// Gets or sets the file path where the object was loaded from or saved to.
        /// </summary>
        /// <remarks>
        /// A value of null indicates that the object is newly created and has not yet been saved to any file.
        /// </remarks>
        string FilePath { get; set; }
    }
}
