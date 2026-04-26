using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.Helpers.Core;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MD.Tools.BaseDataAccess.PluginMethods.Core.DataAccess
{
    public partial class BaseDataAccess<T> : Singleton<T>
        where T : class, new()
    {
        #region Methods
        /// <summary>
        /// Execute method and return a DataTable
        /// </summary>
        /// <param name="method">DataBoundMethod configuration</param>
        /// <returns>DataTable</returns>
		[Obsolete]
        protected DataTable ExecuteMethodTable(DataBoundMethod method)
		{
			return ExecuteMethodTableAsync(method).Result;
		}

		/// <summary>
		/// Execute method and return a Data Set
		/// </summary>
		/// <param name="method">DataBoundMethod configuration</param>
		/// <returns>DataSet</returns>
		[Obsolete]
		protected DataSet ExecuteMethodDataSet(DataBoundMethod method)
		{
			return ExecuteMethodDataSetAsync(method).Result;
		}

		/// <summary>
		/// Execute method and return a DataRow
		/// </summary>
		/// <param name="method">DataBoundMethod configuration</param>
		/// <returns>DataRow</returns>
		[Obsolete]
		protected DataRow ExecuteMethodRow(DataBoundMethod method)
		{
			return ExecuteMethodRowAsync(method).Result;
		}
		/// <summary>
		/// Execute method and return a boolean value
		/// </summary>
		/// <param name="method">DataBoundMethod configuration</param>
		/// <returns>boolean value</returns>
		[Obsolete]
		protected bool ExecuteMethodBoolean(DataBoundMethod method)
		{
			return ExecuteMethodBooleanAsync(method).Result;
		}
		/// <summary>
		/// Execute method without returning a result
		/// </summary>
		/// <param name="method">DataBoundMethod configuration</param>
		[Obsolete]
		protected void ExecuteMethodVoid(DataBoundMethod method)
		{
			Task.Run(async () => {
				await ExecuteMethodVoidAsync(method); }).Wait();
		}
		/// <summary>
		/// Get data structure
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		[Obsolete]
		public dynamic GetDataStructure(DataBoundMethod method)
		{
			return GetDataStructureAsync(method).Result;
		}
		/// <summary>
		/// Execute method and return a DataTable
		/// </summary>
		/// <param name="method">Method configuration</param>
		/// <param name="useDefaultPlugin">Force the use of the default plugin</param>
		/// <returns>DataTable</returns>
		[Obsolete]
		protected DataTable ExecuteMethodTable(Method method, bool useDefaultPlugin = false)
		{
			return ExecuteMethodTableAsync(method, useDefaultPlugin).Result;
		}

		/// <summary>
		/// Execute method and return a Data Set
		/// </summary>
		/// <param name="method">Method configuration</param>
		/// <param name="useDefaultPlugin">Force the use of the default plugin</param>
		/// <returns>DataSet</returns>
		[Obsolete]
		protected DataSet ExecuteMethodDataSet(Method method, bool useDefaultPlugin = false)
		{
			return ExecuteMethodDataSetAsync(method, useDefaultPlugin).Result;
		}

		/// <summary>
		/// Execute method and return a DataRow
		/// </summary>
		/// <param name="method">Method configuration</param>
		/// <param name="useDefaultPlugin">Force the use of the default plugin</param>
		/// <returns>DataRow</returns>
		[Obsolete]
		protected DataRow ExecuteMethodRow(Method method, bool useDefaultPlugin = false)
		{
			return ExecuteMethodRowAsync(method, useDefaultPlugin).Result;
		}
		/// <summary>
		/// Execute method and return a boolean value
		/// </summary>
		/// <param name="method">Method configuration</param>
		/// <param name="useDefaultPlugin">Force the use of the default plugin</param>
		/// <returns>boolean value</returns>
		[Obsolete]
		protected bool ExecuteMethodBoolean(Method method, bool useDefaultPlugin = false)
		{
			return ExecuteMethodBooleanAsync(method, useDefaultPlugin).Result;
		}
		/// <summary>
		/// Execute method without returning a result
		/// </summary>
		/// <param name="method">Method configuration</param>
		/// <param name="useDefaultPlugin">Force the use of the default plugin</param>
		[Obsolete]
		protected void ExecuteMethodVoid(Method method, bool useDefaultPlugin = false)
		{
				Task.Run(async () => {
					await ExecuteMethodVoidAsync(method, useDefaultPlugin); }).Wait();
		}
		#endregion
	}
}
