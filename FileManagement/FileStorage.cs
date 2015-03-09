using System.IO;

namespace FileManagement
{
    public class FileStorage : IStorage
    {
        public Stream GetReadStream(string location)
        {
            if (location == null || !File.Exists(location))
                return null;
            return new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public Stream GetWriteStream(string location)
        {
            if (location == null)
                return null;
            return new FileStream(location, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        }
    }
}
