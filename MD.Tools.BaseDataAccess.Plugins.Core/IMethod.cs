using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public interface IMethod<M, P>
        where P : IMethodProperty
    {
        #region Properties
        M Method { get; }
        int MethodInt { get; }
        List<P> Properties { get; set; }
        string PluginSettings { get; set; }
        MethodTypes MethodType { get; set; }
        #endregion

        #region Methods
        Task<DataSet> ExecuteDataSetAsync();
        Task<DataTable> ExecuteDataTableAsync();
        Task<DataRow> ExecuteDataRowAsync();
        Task<bool> ExecuteBooleanAsync();
        Task ExecuteVoidAsync();
        #endregion
    }
}
