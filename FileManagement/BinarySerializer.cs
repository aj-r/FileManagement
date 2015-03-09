using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FileManagement
{
    public class BinarySerializer : ISerializer
    {
        private IFormatter serializer;

        public BinarySerializer()
            : this(new BinaryFormatter())
        { }

        public BinarySerializer(IFormatter formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException("formatter");
            serializer = formatter;
        }

        public T Deserialize<T>(Stream stream)
        {
            // For now just deserialize the file.
            // In the future we may need to check the version first to prevent serialization errors.
            return (T)serializer.Deserialize(stream);
        }

        public void Serialize<T>(Stream stream, T obj)
        {
            serializer.Serialize(stream, obj);
        }
    }
}
