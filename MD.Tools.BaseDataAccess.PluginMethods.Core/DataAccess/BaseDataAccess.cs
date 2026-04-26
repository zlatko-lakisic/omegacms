using MD.Tools.BaseDataAccess.Plugins.Core;
using MD.Tools.BaseDataAccess.Plugins.Core.Caching;
using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using MD.Tools.BaseDataAccess.Plugins.Core.Properties;
using MD.Tools.Helpers.Core;
using MD.Tools.Helpers.Core.Caching;
using MD.Tools.Helpers.Core.Caching.CacheKeys;
using MD.Tools.Helpers.Core.Caching.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        protected async Task<DataTable> ExecuteMethodTableAsync(DataBoundMethod method)
        {
            return await (await ConfigurePluginAsync(method)).ExecuteDataTableAsync(method);
		}

		/// <summary>
		/// Execute method and return a Data Set
		/// </summary>
		/// <param name="method">DataBoundMethod configuration</param>
		/// <returns>DataSet</returns>
		protected async Task<DataSet> ExecuteMethodDataSetAsync(DataBoundMethod method)
        {
            return await (await ConfigurePluginAsync(method)).ExecuteDataTableSetAsync(method);
		}

		/// <summary>
		/// Execute method and return a DataRow
		/// </summary>
		/// <param name="method">DataBoundMethod configuration</param>
		/// <returns>DataRow</returns>
		protected async Task<DataRow> ExecuteMethodRowAsync(DataBoundMethod method)
        {
            return await (await ConfigurePluginAsync(method)).ExecuteDataRowAsync(method);
		}
		/// <summary>
		/// Execute method and return a boolean value
		/// </summary>
		/// <param name="method">DataBoundMethod configuration</param>
		/// <returns>boolean value</returns>
		protected async Task<bool> ExecuteMethodBooleanAsync(DataBoundMethod method)
        {
            return await (await ConfigurePluginAsync(method)).ExecuteBoolAsync(method);
		}
		/// <summary>
		/// Execute method without returning a result
		/// </summary>
		/// <param name="method">DataBoundMethod configuration</param>
		protected async Task ExecuteMethodVoidAsync(DataBoundMethod method)
        {
			await (await ConfigurePluginAsync(method)).ExecuteAsync(method);
		}
		/// <summary>
		/// Get data structure
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public async Task<dynamic> GetDataStructureAsync(DataBoundMethod method)
        {
            return await (await ConfigurePluginAsync(method)).GetDataStructureAsync(method);
		}
		/// <summary>
		/// Execute method and return a DataTable
		/// </summary>
		/// <param name="method">Method configuration</param>
		/// <param name="useDefaultPlugin">Force the use of the default plugin</param>
		/// <returns>DataTable</returns>
		protected async Task<DataTable> ExecuteMethodTableAsync(Method method, bool useDefaultPlugin = false)
		{
			DataTable result = await GetClassCacheValueAsync<DataTable>(method);
			if(result == null || result.Rows.Count.Equals(default))
            {
				result = await (await ConfigurePluginAsync(method, useDefaultPlugin)).ExecuteDataTableAsync(method);
				await AddClassCacheValueAsync(method, result).ConfigureAwait(false);
			}
			return result;
		}

		/// <summary>
		/// Execute method and return a Data Set
		/// </summary>
		/// <param name="method">Method configuration</param>
		/// <param name="useDefaultPlugin">Force the use of the default plugin</param>
		/// <returns>DataSet</returns>
		protected async Task<DataSet> ExecuteMethodDataSetAsync(Method method, bool useDefaultPlugin = false)
		{
			DataSet result = await GetClassCacheValueAsync<DataSet>(method);
			if (result == null)
			{
				result = await (await ConfigurePluginAsync(method, useDefaultPlugin)).ExecuteDataTableSetAsync(method);
				await AddClassCacheValueAsync(method, result).ConfigureAwait(false);
			}
			return result;
		}

		/// <summary>
		/// Execute method and return a DataRow
		/// </summary>
		/// <param name="method">Method configuration</param>
		/// <param name="useDefaultPlugin">Force the use of the default plugin</param>
		/// <returns>DataRow</returns>
		/// TEMPORARY TRUE FOR DEBUG!!!!
		protected async Task<DataRow> ExecuteMethodRowAsync(Method method, bool useDefaultPlugin = false)
		{
			DataRow result = await GetDataRowCacheValueAsync<DataRow>(method);
			if (result == null)
			{
				result = await (await ConfigurePluginAsync(method, useDefaultPlugin)).ExecuteDataRowAsync(method);
				await AddClassCacheValueAsync(method, result != null ? result.Table : null).ConfigureAwait(false);
			}
			return result;
		}
		/// <summary>
		/// Execute method and return a boolean value
		/// </summary>
		/// <param name="method">Method configuration</param>
		/// <param name="useDefaultPlugin">Force the use of the default plugin</param>
		/// <returns>boolean value</returns>
		protected async Task<bool> ExecuteMethodBooleanAsync(Method method, bool useDefaultPlugin = false)
		{
			return await (await ConfigurePluginAsync(method, useDefaultPlugin)).ExecuteBoolAsync(method);
		}
		/// <summary>
		/// Execute method without returning a result
		/// </summary>
		/// <param name="method">Method configuration</param>
		/// <param name="useDefaultPlugin">Force the use of the default plugin</param>
		protected async Task ExecuteMethodVoidAsync(Method method, bool useDefaultPlugin = false)
        {
			await (await ConfigurePluginAsync(method, useDefaultPlugin)).ExecuteAsync(method);
		}

		/// <summary>
		/// Gets and configures the correct plugin
		/// </summary>
		/// <param name="method"></param>
		/// <param name="useDefaultPlugin"></param>
		/// <returns></returns>
		private async Task<IBaseDataAccessPlugin> ConfigurePluginAsync(Method method, bool useDefaultPlugin)
		{
			BaseDataAccessPlugins.Initialize();
			IBaseDataAccessPlugin plugin = BaseDataAccessPlugins.Container.DefaultPlugin.Clone();
			if (!useDefaultPlugin)
			{
				plugin = BaseDataAccessPlugins.Container.GetAppropriatePluginForMethod(method).Clone();
			}

			return plugin;
		}

		/// <summary>
		/// Gets and configures the correct plugin
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		private async Task<IBaseDataAccessPlugin> ConfigurePluginAsync(DataBoundMethod method)
		{
			BaseDataAccessPlugins.Initialize();
			return BaseDataAccessPlugins.Container.GetAppropriatePluginForMethod(method);
		}

		/// <summary>
		/// Get all data bound plugins
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<IBaseDataAccessPlugin>> GetDataBoundPluginsAsync()
		{
			BaseDataAccessPlugins.Initialize();
			return BaseDataAccessPlugins.Container.GetAllDataboundPlugins();
		}

		private string GetCacheKey(Method method, bool includeParams = true)
		{
			return GetCacheKey(method.Entity, method.Id, includeParams, method.Properties.ToArray());
		}

		private string GetCacheKey(Entities mappingEntity, int methodId = default, bool includeParams = true, params IMethodProperty[] properties)
		{
			string cacheKey = string.Empty;
			if (Settings.Default.DataCacheSettings.Entities != null &&
				Settings.Default.DataCacheSettings.Entities.Any(e => e.Enabled && e.MappedEntity.Equals(mappingEntity)))
			{
				OmegaCacheSettings.Entity entity = Settings.Default.DataCacheSettings.Entities.First(e => e.MappedEntity.Equals(mappingEntity));
				if (entity.Methods.Any(m => m.Enabled && m.MappedMethod.Equals(methodId)))
				{
					if (includeParams)
					{
						IEnumerable<string> props = new List<string>();
						if(properties != null && properties.Any())
                        {
							props = properties.Select(p => $"{p.Id}-{p.Value}");
						}
						cacheKey = new DefaultCacheKeyGenerator().MakeCacheKey($"{mappingEntity}-{methodId}", props.ToArray());
					}
					else
					{
						cacheKey = mappingEntity.ToString();
					}
				}
			}
			return cacheKey;
		}

		private IOmegaServerCachingProvider GetProvider(Method method)
		{
			return GetProvider(method.Entity, method.Id);
		}

		private IOmegaServerCachingProvider GetProvider(Entities mappingEntity, int methodId = default)
		{
			IOmegaServerCachingProvider provider = OmegaCacheController.Instance.CachingProviders[Settings.Default.DataCacheSettings.DefaultCacheProvider];
			if (Settings.Default.DataCacheSettings.Entities != null &&
				Settings.Default.DataCacheSettings.Entities.Any(e => e.Enabled && e.MappedEntity.Equals(mappingEntity)))
			{
				OmegaCacheSettings.Entity entity = Settings.Default.DataCacheSettings.Entities.First(e => e.MappedEntity.Equals(mappingEntity));

				if (entity != null && !string.IsNullOrEmpty(entity.CacheProvider) && OmegaCacheController.Instance.CachingProviders.ContainsKey(entity.CacheProvider))
				{
					provider = OmegaCacheController.Instance.CachingProviders[entity.CacheProvider];
				}

				OmegaCacheSettings.Entity.Method cacheMethod = entity.Methods.FirstOrDefault(m => m.Enabled && m.MappedMethod.Equals(methodId) && !string.IsNullOrEmpty(m.CacheProvider));
				if (cacheMethod != null && OmegaCacheController.Instance.CachingProviders.ContainsKey(cacheMethod.CacheProvider))
				{
					provider = OmegaCacheController.Instance.CachingProviders[cacheMethod.CacheProvider];
				}

			}
			return provider;
		}

		private TimeSpan GetCacheTimeout(Method method)
		{
			return GetCacheTimeout(method.Entity, method.Id);
		}

		private TimeSpan GetCacheTimeout(Entities mappingEntity, int methodId = default)
		{
			TimeSpan timespan = TimeSpan.FromSeconds(0);
			if (Settings.Default.DataCacheSettings.Entities != null &&
				Settings.Default.DataCacheSettings.Entities.Any(e => e.Enabled && e.MappedEntity.Equals(mappingEntity)))
			{
				OmegaCacheSettings.Entity entity = Settings.Default.DataCacheSettings.Entities.First(e => e.MappedEntity.Equals(mappingEntity));

				timespan = TimeSpan.FromSeconds(entity.TimeSpan);

				OmegaCacheSettings.Entity.Method cacheMethod = entity.Methods.FirstOrDefault(m => m.Enabled && m.MappedMethod.Equals(methodId));
				if (cacheMethod != null)
				{
					timespan = TimeSpan.FromSeconds(cacheMethod.TimeSpan);
				}

			}
			return timespan;
		}

		public async Task<bool> AddClassCacheValueAsync<T>(Method method, T value)
			where T : class
		{
			await ClearCacheValueAsync(method);

			if (method.MethodType == MethodTypes.ReadList || method.MethodType == MethodTypes.ReadSingle)
			{
				string cacheKey = GetCacheKey(method);
				IOmegaServerCachingProvider provider = GetProvider(method);
				TimeSpan cacheTimeout = GetCacheTimeout(method);
				if (value != null && !string.IsNullOrEmpty(cacheKey) && provider != null && cacheTimeout.TotalSeconds > 0)
				{
					if (
						value != null &&
						value.GetType() == typeof(DataTable) ||
						value.GetType() == typeof(DataSet)
						)
					{
						return await OmegaCacheController.Instance.AddToCacheAsync(provider, cacheTimeout, Settings.Default.CacheSourceName, cacheKey, JsonConvert.SerializeObject(value));
					}
				}
			}
			return false;
		}

		public async Task<bool> AddClassCacheValueAsync<T>(Entities mappingEntity, int methodId, T value)
			where T : class
		{
			await ClearCacheValueAsync(mappingEntity, methodId);

			string cacheKey = GetCacheKey(mappingEntity, methodId);
			IOmegaServerCachingProvider provider = GetProvider(mappingEntity, methodId);
			TimeSpan cacheTimeout = GetCacheTimeout(mappingEntity, methodId);
			if (value != null && !string.IsNullOrEmpty(cacheKey) && provider != null && cacheTimeout.TotalSeconds > 0)
			{
				if (
					value != null &&
					value.GetType() == typeof(DataTable) ||
					value.GetType() == typeof(DataSet)
					)
				{
					return await OmegaCacheController.Instance.AddToCacheAsync(provider, cacheTimeout, Settings.Default.CacheSourceName, cacheKey, JsonConvert.SerializeObject(value));
				}
			}
			return false;
		}

		public async Task<DataRow> GetDataRowCacheValueAsync<T>(Entities mappingEntity, int methodId = default)
		{
			DataTable resultTable = await GetClassCacheValueAsync<DataTable>(mappingEntity, methodId);

			if (resultTable != null && resultTable.Rows.Count > 0)
			{
				return resultTable.Rows[0];
			}
			return null;
		}

		public Task<DataRow> GetDataRowCacheValueAsync<T>(Method method)
		{
			return GetDataRowCacheValueAsync<T>(method.Entity, method.Id);
		}

		public Task<T> GetClassCacheValueAsync<T>(Method method)
			where T : class
		{
			return GetClassCacheValueAsync<T>(method.Entity, method.Id);
		}

		public async Task<T> GetClassCacheValueAsync<T>(Entities mappingEntity, int methodId = default)
			where T : class
		{
			string cacheKey = GetCacheKey(mappingEntity, methodId);
			IOmegaServerCachingProvider provider = GetProvider(mappingEntity, methodId);
			if (!string.IsNullOrEmpty(cacheKey) && provider != null)
			{
				string result = await OmegaCacheController.Instance.GetFromCacheAsync(provider, cacheKey);
				if (!string.IsNullOrEmpty(result))
				{
					return JsonConvert.DeserializeObject<T>(result);
				}
			}
			return null;
		}

		public Task<T> GetStructCacheValueAsync<T>(Method method)
			where T : struct
		{
			return GetStructCacheValueAsync<T>(method.Entity, method.Id);
		}

		public async Task<T> GetStructCacheValueAsync<T>(Entities mappingEntity, int methodId = default)
			where T : struct
		{
			string cacheKey = GetCacheKey(mappingEntity, methodId);
			string result = await OmegaCacheController.Instance.GetFromCacheAsync(GetProvider(mappingEntity, methodId), cacheKey);
			if (!string.IsNullOrEmpty(result))
			{
				return JsonConvert.DeserializeObject<T>(result);
			}
			return default;
		}

		public async Task<bool> ClearCacheValueAsync(Method method)
		{
			if (method.ClearCache || method.MethodType == MethodTypes.Create || method.MethodType == MethodTypes.Delete || method.MethodType == MethodTypes.Update)
			{
				return await ClearCacheValueAsync(method.Entity, method.Id);
			}
			return false;
		}

		public async Task<bool> ClearCacheValueAsync(Entities mappingEntity, int methodId = default)
		{
			string cacheKey = GetCacheKey(mappingEntity, methodId, false);
			if (!string.IsNullOrEmpty(cacheKey))
			{
				return await OmegaCacheController.Instance.InvalidateCacheAsync(GetProvider(mappingEntity, methodId), cacheKey, true);
			}
			return false;
		}
		#endregion
	}
}
