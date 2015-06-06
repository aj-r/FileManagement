using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace FileManagement.Test
{
    [Serializable]
    public class TestFile : IFile
    {
        private string testProperty;

        public string TestProperty
        {
            get { return testProperty; }
            set { testProperty = value; }
        }

        [NonSerialized]
        private string storageLocation;

        [XmlIgnore]
        [JsonIgnore]
        public string StorageLocation
        {
            get { return storageLocation; }
            set { storageLocation = value; }
        }
    }
}
