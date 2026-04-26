using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
  public  class ReportData:BaseEntity<long>
    {
        #region Attributes
        private DateTime _dateCreated;
        private DataSet _data;
        private long _reportId;
        #endregion

        #region Properties     
      
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }

        public DataSet Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public long ReportId
        {
            get { return _reportId; }
            set { _reportId = value; }
        }
        #endregion
    }
}
