using System.IO;

namespace FileManagement
{
    public class FileStorage : IStorage
    {
        public Stream GetReadStream(string location)
        {
            return new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public Stream GetWriteStream(string location)
        {
            return new FileStream(location, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        }

        public bool Exists(string location)
        {
            return File.Exists(location);
        }
    }
}
