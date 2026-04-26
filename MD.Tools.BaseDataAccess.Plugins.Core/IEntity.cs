using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public interface IEntity<M>
    {
        #region Properties
        IEnumerable<M> Methods { get; }
        #endregion
    }
}
