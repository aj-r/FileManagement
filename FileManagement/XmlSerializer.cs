using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileManagement
{
    /// <summary>
    /// Serializes and deserializes objects in XML format.
    /// </summary>
    public class XmlSerializer : ISerializer
    {
        private Dictionary<Type, System.Xml.Serialization.XmlSerializer> serializerCache = new Dictionary<Type, System.Xml.Serialization.XmlSerializer>();

        /// <summary>
        /// Gets the XML serializer for the specified type.
        /// </summary>
        /// <param name="type">The type that will be serialized and deserialized.</param>
        /// <returns>an XML serializer.</returns>
        protected virtual System.Xml.Serialization.XmlSerializer GetSerializer(Type type)
        {
            System.Xml.Serialization.XmlSerializer serializer;
            if (serializerCache.TryGetValue(type, out serializer))
                return serializer;

            serializer = new System.Xml.Serialization.XmlSerializer(type);
            serializerCache.Add(type, serializer);
            return serializer;
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T">The type of object to serialize.</typeparam>
        /// <param name="stream">The stream to which the object will be serialized.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="obj">The object to serialize.</param>
        public void Serialize<T>(Stream stream, Encoding encoding, T obj)
        {
            var serializer = GetSerializer(typeof(T));
            serializer.Serialize(stream, obj);
        }

        /// <summary>
        /// Deserializes an object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="stream">The stream from which the object will be deserialized.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(Stream stream, Encoding encoding)
        {
            var serializer = GetSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }
    }
}
