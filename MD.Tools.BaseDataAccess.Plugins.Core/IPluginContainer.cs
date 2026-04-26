using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public interface IPluginContainer
    {
        #region Properties
        IBaseDataAccessPlugin DefaultPlugin { get; }
        IBaseDataAccessPlugin SearchPlugin { get; }
		#endregion

		#region Methods
		IBaseDataAccessPlugin GetAppropriatePluginForMethod(Method method);
		IBaseDataAccessPlugin GetAppropriatePluginForMethod(DataBoundMethod method);
		#endregion
	}
}
