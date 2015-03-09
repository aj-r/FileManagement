using System;
using System.Linq;
using NUnit.Framework;

namespace FileManagement.Test
{
    [TestFixture]
    public class RecentFileListTests
    {
        [Test]
        public void EnqueueTest()
        {
            var list = new RecentFileList();
            list.Add("test1.txt");
            list.Add("test2.txt");

            var items = list.ToList();
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(items[0], "test2.txt");
            Assert.AreEqual(items[1], "test1.txt");
        }

        [Test]
        public void EnqueueExistingFileTest()
        {
            var list = new RecentFileList();
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Add("test1.txt");

            var items = list.ToList();
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(items[0], "test1.txt");
            Assert.AreEqual(items[1], "test2.txt");
        }

        [Test]
        public void MaxLengthTest()
        {
            var list = new RecentFileList(3);
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Add("test3.txt");
            list.Add("test4.txt");

            var items = list.ToList();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(items[0], "test4.txt");
            Assert.AreEqual(items[1], "test3.txt");
            Assert.AreEqual(items[2], "test2.txt");
        }

        [Test]
        public void ContainsTest()
        {
            var list = new RecentFileList();
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Add("test3.txt");

            Assert.IsTrue(list.Contains("test2.txt"));
        }

        [Test]
        public void RemoveTest()
        {
            var list = new RecentFileList();
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Remove("test2.txt");

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(list.First(), "test1.txt");
        }

        [Test]
        public void ClearTest()
        {
            var list = new RecentFileList();
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Clear();

            Assert.AreEqual(0, list.Count);
        }
    }
}
