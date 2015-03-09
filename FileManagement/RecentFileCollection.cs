using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace FileManagement
{
	/// <summary>
	/// A collection that is useful for storing and maintaining a list of recent files.
	/// The files at the start of the collection are the least recent, and the files at the end are the most recent.
	/// </summary>
	public class RecentFileCollection : IRecentFileCollection
	{
		private static int defaultMaxLength = int.MaxValue;

		/// <summary>
        /// Gets or sets the default maximum length that a RecentFileCollection will have when it is created.
		/// </summary>
		public static int DefaultMaxLength
		{
			get { return defaultMaxLength; }
			set { defaultMaxLength = value; }
		}

		#region Private Classes

		private class RecentFileNode
		{
			public string filePath;
			public RecentFileNode nextNode = null;
			public RecentFileNode previousNode = null;

			public RecentFileNode()
				: this(string.Empty)
			{ }

			public RecentFileNode(string filePath)
			{
				this.filePath = filePath;
			}
		}

		private class RecentFileEnumerator : IEnumerator<string>
		{
			RecentFileNode currentNode = null;
			RecentFileNode firstNode;

			public RecentFileEnumerator()
				: this(null)
			{ }

			public RecentFileEnumerator(RecentFileNode firstNode)
			{
				this.firstNode = firstNode;
			}

			public string Current
			{
				get
				{
					if (currentNode == null)
					{
						throw new InvalidOperationException();
					}
					return currentNode.filePath;
				}
			}

			public void Dispose()
			{
				currentNode = null;
				firstNode = null;
			}

			object IEnumerator.Current
			{
				get { return Current; }
			}

			public bool MoveNext()
			{
				if (currentNode == null)
				{
					currentNode = firstNode;
				}
				else
				{
					currentNode = currentNode.nextNode;
				}
				return currentNode != null;
			}

			public void Reset()
			{
				currentNode = null;
			}
		}

		#endregion

		private RecentFileNode firstNode;
		private RecentFileNode lastNode;

		private int maxLength;
		private int count;

		/// <summary>
        /// Creates a new RecentFileCollection instance.
		/// </summary>
		public RecentFileCollection()
			: this(DefaultMaxLength)
		{ }

        /// <summary>
        /// Creates a new RecentFileCollection instance.
		/// </summary>
		/// <param name="maxLength">
		/// The maximum length that the collection can have. If this length is exceeded,
        /// the collection will automatically dequeue itself until the maximum length is no longer exceeded.
		/// </param>
		public RecentFileCollection(int maxLength)
		{
			this.maxLength = maxLength;
			this.count = 0;
		}

        /// <summary>
        /// Creates a copy of a RecentFileCollection instance.
		/// </summary>
        /// <param name="recentFileCollection">The collection to copy.</param>
		public RecentFileCollection(RecentFileCollection recentFileCollection)
            : this((IEnumerable<string>)recentFileCollection)
		{
            maxLength = recentFileCollection.maxLength;
		}

		/// <summary>
        /// Creates a new RecentFileCollection instance from a list of file paths.
		/// </summary>
		/// <param name="filePaths">A list of file paths.</param>
		public RecentFileCollection(IEnumerable<string> filePaths)
			: this()
		{
			foreach (string filePath in filePaths)
                Add(filePath);
		}

		/// <summary>
		/// Creates a copy of a recent file queue.
		/// </summary>
		public RecentFileCollection Clone()
		{
			return new RecentFileCollection(this);
		}

		private void RemoveLeastRecent()
		{
			var secondNode = firstNode.nextNode;
			if (secondNode != null)
			{
				// There are still items left.
				secondNode.previousNode = null;
				firstNode = secondNode;
			}
			else
			{
				// The list is now empty
				firstNode = null;
				lastNode = null;
			}
			--count;
		}

		private void RemoveExcess()
		{
			while (Count > MaxLength)
                RemoveLeastRecent();
		}

		/// <summary>
		/// Gets or sets the maximum length of the collection.
		/// </summary>
		public int MaxLength
		{
			get
			{
				return maxLength;
			}
			set
			{
				maxLength = value;
				RemoveExcess();
			}
		}

        #region ICollection<string> Members

        /// <summary>
        /// Adds a file path to the end of the collection. If the file path already exists in the collection,
        /// it is moved to the front of the collection. If the maximum length is exceeded, the collection will
        /// automatically remove the least recent items until the maximum length is no longer exceeded.
        /// </summary>
        /// <param name="filePath">The file path to add.</param>
        public void Add(string filePath)
        {
            // Remove the item if it already exists.
            Remove(filePath);

            // Add the file path to the list.
            var secondLastNode = lastNode;
            lastNode = new RecentFileNode(filePath);
            lastNode.previousNode = secondLastNode;
            if (secondLastNode != null)
            {
                // There is a node before this one.
                secondLastNode.nextNode = lastNode;
            }
            else
            {
                // There is no node after this one - it is the last node.
                firstNode = lastNode;
            }
            ++count;

            // If the max size is exceeded, remove the most recent items.
            RemoveExcess();
        }

        /// <summary>
        /// Removes the first occurrence of the specified file path from the collection, if it exists.
        /// </summary>
        /// <param name="filePath">The file path to remove.</param>
        /// <returns>True if the item was removed, false if the file path was not found.</returns>
        public bool Remove(string filePath)
        {
            RecentFileNode currentNode = firstNode;
            while (currentNode != null)
            {
                if (currentNode.filePath == filePath)
                {
                    if (currentNode.previousNode != null)
                    {
                        // This was not the first node.
                        currentNode.previousNode.nextNode = currentNode.nextNode;
                    }
                    else
                    {
                        // This was the first node.
                        firstNode = currentNode.nextNode;
                    }
                    if (currentNode.nextNode != null)
                    {
                        // This was not the last node.
                        currentNode.nextNode.previousNode = currentNode.previousNode;
                    }
                    else
                    {
                        // This was the last node.
                        lastNode = currentNode.previousNode;
                    }
                    --count;
                    return true;
                }
                currentNode = currentNode.nextNode;
            }
            return false;
        }

        /// <summary>
        /// Removes all items from the list.
        /// </summary>
        public void Clear()
        {
            firstNode = null;
            lastNode = null;
            count = 0;
        }

        /// <summary>
        /// Gets whether the list contains the specified file path.
        /// </summary>
        public bool Contains(string filePath)
        {
            foreach (var s in this)
                if (s == filePath)
                    return true;
            return false;
        }

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        void ICollection<string>.CopyTo(string[] array, int arrayIndex)
        {
            foreach (var s in this)
            {
                array[arrayIndex] = s;
                ++arrayIndex;
            }
        }

        bool ICollection<string>.IsReadOnly
        {
            get { return false; }
        }

        #endregion

		#region IEnumerable<string> Members

		public IEnumerator<string> GetEnumerator()
		{
			return new RecentFileEnumerator(firstNode);
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}
