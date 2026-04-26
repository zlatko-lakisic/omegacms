using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MD.Tools.Helpers.Core.Collections
{
    /// <summary>
    /// Same as Queue except Dequeue function blocks until there is an object to return.
    /// Note: This class does not need to be synchronized
    /// </summary>
    /// <remarks>Based on implementation at: http://www.eggheadcafe.com/articles/20060414.asp</remarks>
    public class BlockingQueue<T> : Queue<T>, ICollection<T>
    {

        private bool _open;
        private object _syncRoot = new object();



        /// <summary>
        /// Initializes a new instance of the <see cref="BlockingQueue&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new <see cref="T:System.Collections.Generic.Queue`1"/>.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="collection"/> is null.
        /// </exception>
        public BlockingQueue(IEnumerable<T> collection)
            : base(collection)
        {
            _open = true;
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="BlockingQueue&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the queue can contain</param>
        public BlockingQueue(int capacity)
            : base(capacity)
        {
            _open = true;
        }



        /// <summary>
        /// Create new BlockingQueue.
        /// </summary>
        public BlockingQueue()
            : base()
        {
            _open = true;
        }

        /// <summary>
        /// BlockingQueue Destructor (Close queue, resume any waiting thread).
        /// </summary>
        ~BlockingQueue()
        {
            Close();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is open.
        /// </summary>
        /// <value><c>true</c> if this instance is open; otherwise, <c>false</c>.</value>
        public bool IsOpen { get { return _open; } }

        /// <summary>
        /// Remove all objects from the Queue.
        /// </summary>
        public new void Clear()
        {
            lock (_syncRoot)
            {
                base.Clear();
            }

        }

        /// <summary>
        /// Remove all objects from the Queue, resume all dequeue threads.
        /// </summary>
        public void Close()
        {
            lock (_syncRoot)
            {
                _open = false;
                base.Clear();
                Monitor.PulseAll(_syncRoot);    // resume any waiting threads
            }
        }

        /// <summary>
        /// Gets the contents.
        /// </summary>
        /// <value>The contents.</value>
        public IEnumerable<T> Contents
        {
            get
            {
                lock (_syncRoot)
                {
                    return base.ToArray().OfType<T>();
                }
            }
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the Queue.
        /// </summary>
        /// <returns>Object in queue.</returns>
        [System.Diagnostics.DebuggerStepThrough()]
        public new T Dequeue()
        {
            return Dequeue(Timeout.Infinite);
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the Queue.
        /// </summary>
        /// <param name="timeout">time to wait before returning</param>
        /// <returns>Object in queue.</returns>
        [System.Diagnostics.DebuggerStepThrough()]
        public T Dequeue(TimeSpan timeout)
        {

            return Dequeue(timeout.Milliseconds);

        }

        /// <summary>
        /// Removes and returns the object at the beginning of the Queue.
        /// </summary>
        /// <param name="timeout">time to wait before returning (in milliseconds)</param>
        /// <returns>Object in queue.</returns>
        [System.Diagnostics.DebuggerStepThrough()]
        public T Dequeue(int timeout)
        {

            lock (_syncRoot)
            {


                while (_open && (base.Count == 0))
                {
                    if (!Monitor.Wait(_syncRoot, timeout))
                        throw new InvalidOperationException("Timeout");
                }

                if (_open)

                    return base.Dequeue();

                else

                    throw new InvalidOperationException("Queue Closed");

            }

        }

        /// <summary>
        /// Adds an object to the end of the Queue.
        /// </summary>
        /// <param name="obj">Object to put in queue</param>
        public new void Enqueue(T obj)
        {

            lock (_syncRoot)
            {

                base.Enqueue(obj);
                Queue<T> que = this;
                Monitor.Pulse(_syncRoot);

            }

        }

        /// <summary>
        /// Open Queue.
        /// </summary>
        public void Open()
        {

            lock (_syncRoot)
            {

                _open = true;

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets flag indicating if queue has been closed.
        /// </summary>
        public bool Closed
        {

            get
            {

                return !_open;

            }

        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly => false;
    }
}
