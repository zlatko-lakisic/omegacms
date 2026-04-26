using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MD.Tools.Helpers.Core.Collections
{
    /// <summary>
    /// Represents a thread-safe first in-first out ordered (FIFO) collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements contained in the queue.</typeparam>
    public class MdConcurrentOrderedQueue<T> : ConcurrentQueue<KeyValuePair<int, T>>
    {
        private int counter;

        /// <summary>
        /// Initializes a new instance of the MD.Tools.Helpers.Core.Collections.MdConcurrentOrderedQueue`1 class.
        /// </summary>
        public MdConcurrentOrderedQueue() : base()
        {
            counter = 0;
        }

        /// <summary>
        /// Initializes a new instance of the MD.Tools.Helpers.Core.Collections.MdConcurrentOrderedQueue`1 class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new MD.Tools.Helpers.Core.Collections.MdConcurrentOrderedQueue`1.</param>
        public MdConcurrentOrderedQueue(IEnumerable<T> collection) : base(collection != null ? collection.Select((T obj, int i) => new KeyValuePair<int, T>(i, obj)) : null)
        {
            counter = collection != null ? collection.Count() : 0;
        }

        /// <summary>
        /// Adds an object to the end of the MD.Tools.Helpers.Core.Collections.MdConcurrentOrderedQueue`1
        /// </summary>
        /// <param name="item">The object to add to the end of the MD.Tools.Helpers.Core.Collections.MdConcurrentOrderedQueue`1. The value can be a null reference (Nothing in Visual Basic) for reference types.</param>
        public void Enqueue(T item)
        {
            Enqueue(new KeyValuePair<int, T>(counter, item));
            counter++;
        }

        /// <summary>
        /// Creates a System.Collections.Generic.List`1 from an MD.Tools.Helpers.Core.Collections.MdConcurrentOrderedQueue`1.
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            return this.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
        }
    }
}
