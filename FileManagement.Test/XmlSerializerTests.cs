using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace FileManagement.Test
{
    [TestFixture]
    public class XmlSerializerTests
    {
        [Test]
        public void CanSerialize()
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer();
                var file = new TestFile { TestProperty = "test" };

                serializer.Serialize(ms, file);

                var s = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                Assert.That(s, Is.EqualTo(
@"<?xml version=""1.0""?>
<TestFile xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <TestProperty>test</TestProperty>
</TestFile>"));

            }
        }

        [Test]
        public void CanDeserialize()
        {
            var buffer = Encoding.UTF8.GetBytes(
@"<?xml version=""1.0""?>
<TestFile xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <TestProperty>test</TestProperty>
</TestFile>");
            TestFile testFile;
            var serializer = new XmlSerializer();

            using (var ms = new MemoryStream(buffer))
                testFile = serializer.Deserialize<TestFile>(ms);

            Assert.That(testFile, Is.Not.Null);
            Assert.That(testFile.TestProperty, Is.EqualTo("test"));
        }
    }
}
