using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
        public string StorageLocation
        {
            get { return storageLocation; }
            set { storageLocation = value; }
        }
    }
}
