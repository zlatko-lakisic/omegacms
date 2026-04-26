using MD.Tools.BaseDataAccess.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Core.Entities.WebRequestProperties
{
    public abstract class ChildlessProperty : IBaseWebRequestProperty
    {
        public abstract string Name { get; }

        public abstract object Value { get; set; }

        public abstract bool IsQueryStringParam { get; }

        private List<IBaseWebRequestProperty> _itemsList;

        private bool _isArrayProperty;

        /// <summary>
        /// This property does not have any child items, therefore cannot be an array
        /// </summary>
        public bool IsArray
        {
            get
            {
                return false;
            }
            set
            {
                _isArrayProperty = value;
            }
        }

        public string ToJson()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                return string.Format("{0}", Value);
            }
            else
            {
                long longValue = default(long);
                return '"' + Name + '"' + ":" + '"' + Value as string + '"';
            }
        }

        public string ToUrlEncodedValue()
        {
            return string.Format("{0}={1}", this.Name, this.Value);
        }



        public List<IBaseWebRequestProperty> Items
        {
            get
            {
                return null;
            }
            set
            {
                _itemsList = value;
            }
        }
    }
}
