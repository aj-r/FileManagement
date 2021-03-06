﻿using System;
using System.IO;
using System.Text;
using Moq;
using NUnit.Framework;

namespace FileManagement.Test
{
    [TestFixture]
    public class FileManagerTests
    {
        [Test]
        public void SaveTest()
        {
            var gotStream = false;
            var saved = false;
            var mockSerializer = new Mock<ISerializer>();
            mockSerializer.Setup(s => s.Serialize(It.IsAny<MemoryStream>(), It.IsNotNull<Encoding>(), It.IsAny<IFile>())).Callback(() => saved = true);
            var mockStorage = new Mock<IStorage>();
            mockStorage.Setup(s => s.GetWriteStream("test.txt")).Callback(() => gotStream = true);
            var fm = new FileManager(mockSerializer.Object, mockStorage.Object) { IsHistoryEnabled = false };
            fm.Save(Mock.Of<IFile>(f => f.StorageLocation == "test.txt"));

            Assert.IsTrue(gotStream, "Failed to get stream");
            Assert.IsTrue(saved, "Failed to save");
        }

        [Test]
        public void LoadTest()
        {
            var gotStream = false;
            var loaded = false;
            var mockSerializer = new Mock<ISerializer>();
            mockSerializer.Setup(s => s.Deserialize<IFile>(It.IsAny<Stream>(), It.IsNotNull<Encoding>())).Callback(() => loaded = true);
            var mockStorage = new Mock<IStorage>();
            mockStorage.Setup(s => s.GetReadStream("test.txt")).Callback(() => gotStream = true);
            var fm = new FileManager(mockSerializer.Object, mockStorage.Object) { IsHistoryEnabled = false };
            fm.Load<IFile>("test.txt");

            Assert.IsTrue(gotStream, "Failed to get stream");
            Assert.IsTrue(loaded, "Failed to load");
        }

        [Test]
        public void SaveHistoryTest()
        {
            var recentFileStream = new MemoryStream();
            var mockStorage = new Mock<IStorage>();
            mockStorage.Setup(s => s.GetWriteStream("recent-test.txt")).Returns(recentFileStream);
            var fm = new FileManager(Mock.Of<ISerializer>(), mockStorage.Object);
            fm.RecentFilesStoragePath = "recent-test.txt";
            fm.Save(Mock.Of<IFile>(f => f.StorageLocation == "test.txt"));

            var recentFiles = Encoding.UTF8.GetString(recentFileStream.GetBuffer());
            StringAssert.Contains("test.txt", recentFiles);
        }

        [Test]
        public void LoadHistoryTest()
        {
            var recentFileStream = new MemoryStream();
            var mockStorage = new Mock<IStorage>();
            mockStorage.Setup(s => s.GetWriteStream("recent-test.txt")).Returns(recentFileStream);
            var fm = new FileManager(Mock.Of<ISerializer>(), mockStorage.Object);
            fm.RecentFilesStoragePath = "recent-test.txt";
            fm.Load<IFile>("test.txt");

            var recentFiles = Encoding.UTF8.GetString(recentFileStream.GetBuffer());
            StringAssert.Contains("test.txt", recentFiles);
        }
    }
}
