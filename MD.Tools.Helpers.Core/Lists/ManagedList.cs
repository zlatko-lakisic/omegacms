using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MD.Tools.Helpers.Core.Lists
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ManagedListCollection<T>: IEnumerable<T>
    {
        #region Attributes
        private List<T> _innerList;
        private List<T> _originalInnerList;
        private bool _listModified;
        #endregion

        #region Properties
        /// <summary>
        /// Inner list, cannot be directly modified
        /// </summary>
        public ReadOnlyCollection<T> InnerList
        {
            get { return _innerList.AsReadOnly(); }
        }
        /// <summary>
        /// Original inner list, cannot be directly modified
        /// </summary>
        public ReadOnlyCollection<T> OriginalInnerList
        {
            get { return _originalInnerList.AsReadOnly(); }
        }
        /// <summary>
        /// Indicator weather the list is modified or not
        /// </summary>
        public bool ListModified
        {
            get { return _listModified; }
        }
        /// <summary>
        /// Get array of inner list
        /// </summary>
        public T[] InnerListArray
        {
            get
            {
                List<T> tList = new List<T>();

                if (InnerList != null)
                {
                    tList = InnerList.ToList();
                }

                return tList.ToArray();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Default constructor sets the inner list to empty list
        /// </summary>
        public ManagedListCollection()
        {
            _innerList = new List<T>();
            _originalInnerList = new List<T>();
        }
        /// <summary>
        /// Overloaded constructor sets the inner list 
        /// </summary>
        /// <param name="list">List to manage</param>
        public ManagedListCollection(IEnumerable<T> list)
        {
            _innerList = list.ToList();
            _originalInnerList = list.ToList();
        }
        /// <summary>
        /// Adds an item to the inner list
        /// </summary>
        /// <param name="item">Item to add to the inner list</param>
        public void AddItem(T item)
        {
            _innerList.Add(item);
            _listModified = true;
        }
        /// <summary>
        /// Removes an item from the inner list
        /// </summary>
        /// <param name="item">Item to remove from the inner list</param>
        public void RemoveItem(T item)
        {
            _innerList.Remove(item);
            _listModified = true;
        }
        /// <summary>
        /// Sets the inner list
        /// </summary>
        /// <param name="list">list to override with</param>
        public void SetList(IEnumerable<T> list)
        {
            _innerList = list.ToList();
            _listModified = true;
        }
        /// <summary>
        /// Empties the inner list
        /// </summary>
        public void Empty()
        {
            _innerList = new List<T>();
            _listModified = true;
        }
        /// <summary>
        /// Gets a list of removed items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetRemovedItems()
        {
            return _originalInnerList.Except(_innerList);
        }
        /// <summary>
        /// Gets a list of new items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetNewItems()
        {
            return _innerList.Except(_originalInnerList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var a in InnerList)
            {
                yield return a;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
