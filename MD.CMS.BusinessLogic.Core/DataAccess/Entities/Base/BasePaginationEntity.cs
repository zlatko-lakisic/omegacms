using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.Base
{
    public class BasePaginationEntity<T>
    {
        #region Attributes
        private int _totalCount;
        private List<T> _items;
        #endregion

        #region Properties
        public int TotalCount
        {
            get{
                return _totalCount;
            }
            set
            {
                _totalCount = value;
            }
        }

        public List<T> Items
        {
            get
            {
                return _items; 
            }
            set
            {
                _items = value;
            }
        }
        #endregion

        #region Methods
        public BasePaginationEntity()
        {
            TotalCount = 0;
            Items = new List<T>();
        }
        #endregion
    }
}
