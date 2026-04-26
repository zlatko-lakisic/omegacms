using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options.Interfaces;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers.V2.Options
{
    public class DataBoundContentRequestOptions : ContentRequestOptions, IDataBoundContentRequestOptions
    {
        #region Attributes
        private IEnumerable<ContentTypeDefinitionFolderDataBoundCondition> _dataBoundConditions;
        private int _lcid;
        #endregion

        #region Properties
        public IEnumerable<ContentTypeDefinitionFolderDataBoundCondition> DataBoundConditions
        {
            get
            {
                if (_dataBoundConditions == null)
                {
                    _dataBoundConditions = new List<ContentTypeDefinitionFolderDataBoundCondition>();
                }
                return _dataBoundConditions;
            }
            set => _dataBoundConditions = value;
        }
        #endregion

        #region Methods
        public DataBoundContentRequestOptions() : base()
        {

        }

        public DataBoundContentRequestOptions(IDataBoundContentRequestOptions obj) : base(obj)
        {
            DataBoundConditions = obj.DataBoundConditions;
        }

        public DataBoundContentRequestOptions(IContentRequestOptions obj) : base(obj)
        {
        }
        #endregion
    }
}
