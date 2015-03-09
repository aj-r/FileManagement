using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace FileManagement
{
	/// <summary>
	/// A queue that is useful for storing and maintaining a list of recent files.
	/// The queue works in the opposite order of a regular queue: items are added to the
	/// front and removed from the back.
	/// </summary>
	[Serializable]
	public class RecentFileList : IRecentFileList
	{
		private static int defaultMaxLength = int.MaxValue;

		/// <summary>
		/// Gets or sets the default maximum length that a RecentFileQueue will have
		/// when it is created.
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
		/// Creates a recent file queue.
		/// </summary>
		public RecentFileList()
			: this(DefaultMaxLength)
		{ }

		/// <summary>
		/// Creates a recent file queue.
		/// </summary>
		/// <param name="maxLength">
		/// The maximum length that the queue can have. If this length is exceeded,
		/// the queue will automatically dequeue itself until the maximum length is no longer exceeded.
		/// </param>
		public RecentFileList(int maxLength)
		{
			this.maxLength = maxLength;
			this.count = 0;
		}

		/// <summary>
		/// Creates a copy of a recent file queue.
		/// </summary>
		/// <param name="recentFileQueue">The queue to copy.</param>
		public RecentFileList(RecentFileList recentFileQueue)
			: this((IEnumerable<string>)recentFileQueue)
		{
			maxLength = recentFileQueue.maxLength;
		}

		/// <summary>
		/// Creates a recent file queue from a list of file paths.
		/// </summary>
		/// <param name="filePaths">A list of file paths.</param>
		public RecentFileList(IEnumerable<string> filePaths)
			: this()
		{
			RecentFileNode previousNode = null;
			foreach (string filePath in filePaths)
			{
				RecentFileNode newNode = new RecentFileNode(filePath);
				newNode.previousNode = previousNode;
				if (previousNode != null)
				{
					previousNode.nextNode = newNode;
				}
				else
				{
					firstNode = newNode;
				}
				previousNode = newNode;
				count++;
			}
			lastNode = previousNode;
		}

		/// <summary>
		/// Creates a copy of a recent file queue.
		/// </summary>
		public RecentFileList Clone()
		{
			return new RecentFileList(this);
		}

		private void RemoveLast()
		{
			string returnValue = lastNode.filePath;
			RecentFileNode secondLastNode = lastNode.previousNode;
			if (secondLastNode != null)
			{
				// There are still items left.
				secondLastNode.nextNode = null;
				lastNode = secondLastNode;
			}
			else
			{
				// The list is now empty
				firstNode = null;
				lastNode = null;
			}
			count--;
		}

		private void RemoveExcess()
		{
			while (Count > MaxLength)
				RemoveLast();
		}

		/// <summary>
		/// Gets or sets the maximum length of the queue.
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
        /// Adds a file path to the beginning of the queue. If the file path already exists in the queue,
        /// it is moved to the front of the list. If the maximum length is exceeded, the queue will
        /// automatically dequeue itself until the maximum length is no longer exceeded.
        /// </summary>
        /// <param name="filePath">The file path to add.</param>
        public void Add(string filePath)
        {
            // Remove the item if it already exists.
            Remove(filePath);

            // Add the file path to the list.
            RecentFileNode secondNode = firstNode;
            firstNode = new RecentFileNode(filePath);
            firstNode.nextNode = secondNode;
            if (secondNode != null)
            {
                // There is a node after this one.
                secondNode.previousNode = firstNode;
            }
            else
            {
                // There is no node after this one - it is the last node.
                lastNode = firstNode;
            }
            count++;

            // If the max size is exceeded, drop items off the end of the queue.
            RemoveExcess();
        }

        /// <summary>
        /// Removes the first occurrence of the specified file path from the queue, if it exists.
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
                    count--;
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
        /// Gets the number of items in the queue.
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
