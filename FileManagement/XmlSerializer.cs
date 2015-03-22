using System;
using System.Collections.Generic;
using System.IO;

namespace FileManagement
{
    public class XmlSerializer : ISerializer
    {
        private Dictionary<Type, System.Xml.Serialization.XmlSerializer> serializerCache = new Dictionary<Type, System.Xml.Serialization.XmlSerializer>();

        protected virtual System.Xml.Serialization.XmlSerializer GetSerializer(Type type)
        {
            System.Xml.Serialization.XmlSerializer serializer;
            if (serializerCache.TryGetValue(type, out serializer))
                return serializer;

            serializer = new System.Xml.Serialization.XmlSerializer(type);
            serializerCache.Add(type, serializer);
            return serializer;
        }

        public void Serialize<T>(Stream stream, T obj)
        {
            var serializer = GetSerializer(typeof(T));
            serializer.Serialize(stream, obj);
        }

        public T Deserialize<T>(Stream stream)
        {
            var serializer = GetSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }
    }
}
