using System;
using System.Linq;
using NUnit.Framework;

namespace FileManagement.Test
{
    [TestFixture]
    public class RecentFileListTests
    {
        [Test]
        public void AddTest()
        {
            var list = new RecentFileCollection();
            list.Add("test1.txt");
            list.Add("test2.txt");

            var items = list.ToList();
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("test1.txt", items[0]);
            Assert.AreEqual("test2.txt", items[1]);
        }

        [Test]
        public void AddExistingFileTest()
        {
            var list = new RecentFileCollection();
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Add("test1.txt");

            var items = list.ToList();
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("test2.txt", items[0]);
            Assert.AreEqual("test1.txt", items[1]);
        }

        [Test]
        public void MaxLengthTest()
        {
            var list = new RecentFileCollection(3);
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Add("test3.txt");
            list.Add("test4.txt");

            var items = list.ToList();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("test2.txt", items[0]);
            Assert.AreEqual("test3.txt", items[1]);
            Assert.AreEqual("test4.txt", items[2]);
        }

        [Test]
        public void ContainsTest()
        {
            var list = new RecentFileCollection();
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Add("test3.txt");

            Assert.IsTrue(list.Contains("test2.txt"));
        }

        [Test]
        public void RemoveFromEndTest()
        {
            var list = new RecentFileCollection();
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Remove("test2.txt");

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("test1.txt", list.First());
        }

        [Test]
        public void RemoveFromMiddleTest()
        {
            var list = new RecentFileCollection();
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Add("test3.txt");
            list.Remove("test2.txt");

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("test1.txt", list.First());
            Assert.AreEqual("test3.txt", list.Last());
        }

        [Test]
        public void RemoveFromStartTest()
        {
            var list = new RecentFileCollection();
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Remove("test1.txt");

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("test2.txt", list.First());
        }

        [Test]
        public void ClearTest()
        {
            var list = new RecentFileCollection();
            list.Add("test1.txt");
            list.Add("test2.txt");
            list.Clear();

            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void EnumerableConstructorTest()
        {
            var list = new RecentFileCollection(new[] { "test1.txt", "test2.txt" });
            var items = list.ToList();

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("test1.txt", items[0]);
            Assert.AreEqual("test2.txt", items[1]);
        }
    }
}
