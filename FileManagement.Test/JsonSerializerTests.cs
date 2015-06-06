using System.IO;
using System.Text;
using FileManagement.Json;
using NUnit.Framework;

namespace FileManagement.Test
{
    [TestFixture]
    public class JsonSerializerTests
    {
        [Test]
        public void CanSerialize()
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new JsonSerializer();
                var file = new TestFile { TestProperty = "test" };

                serializer.Serialize(ms, Encoding.UTF8, file);

                var s = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                // The first character seems to be something that indicates the endocidng. Ignore it.
                if (s.Length > 0 && s[0] > 255)
                    s = s.Substring(1);
                Assert.That(s, Is.EqualTo(@"{""TestProperty"":""test""}"));
            }
        }

        [Test]
        public void CanDeserialize()
        {
            var buffer = Encoding.UTF8.GetBytes(@"{""TestProperty"":""test""}");
            TestFile testFile;
            var serializer = new JsonSerializer();

            using (var ms = new MemoryStream(buffer))
            {
                testFile = serializer.Deserialize<TestFile>(ms, Encoding.UTF8);
                // Make sure the stream is still open.
                // The caller probably is not expecting us to close the stream, especially if this is a network stream or something.
                // It is the caller's responsibility to close the stream.
                Assert.That(ms.CanRead);
            }

            Assert.That(testFile, Is.Not.Null);
            Assert.That(testFile.TestProperty, Is.EqualTo("test"));
        }
    }
}
