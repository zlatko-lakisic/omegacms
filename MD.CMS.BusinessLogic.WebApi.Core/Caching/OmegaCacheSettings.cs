using MD.Tools.Helpers.Core.Caching.Providers;
using System.Collections.Generic;
using System.Linq;

namespace MD.CMS.BusinessLogic.WebApi.Core.Caching
{
    /// <summary>
    /// Output Cache Settings
    /// </summary>
    public class OmegaCacheSettings
    {
        #region Attributes
        private string _defaultCacheProvider;
        #endregion

        #region Properties
        /// <summary>
        /// Cache Settings Controller Class
        /// </summary>
        public class Controller
        {
            /// <summary>
            /// Cache Settings Method Class
            /// </summary>
            public class Method
            {
                /// <summary>
                /// Method Name
                /// </summary>
                public string Name { get; set; }
                /// <summary>
                /// Cache client time span in seconds
                /// </summary>
                public int ClientTimeSpan { get; set; }
                /// <summary>
                /// Cache server time span in seconds
                /// </summary>
                public int ServerTimeSpan { get; set; }
                /// <summary>
                /// Is the cache enabled?
                /// </summary>
                public bool Enabled { get; set; }
                /// <summary>
                /// Cache provider
                /// </summary>
                public string CacheProvider { get; set; }
                /// <summary>
                /// Invalidate cache by
                /// </summary>
                public string[] InvalidateBy { get; set; }
            }

            /// <summary>
            /// Controller Name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Cache client time span in seconds
            /// </summary>
            public int ClientTimeSpan { get; set; }
            /// <summary>
            /// Cache server time span in seconds
            /// </summary>
            public int ServerTimeSpan { get; set; }
            /// <summary>
            /// Is the cache enabled?
            /// </summary>
            public bool Enabled { get; set; }
            /// <summary>
            /// Controller method settings
            /// </summary>
            public IEnumerable<Method> Methods { get; set; }
            /// <summary>
            /// Cache provider
            /// </summary>
            public string CacheProvider { get; set; }
            /// <summary>
            /// Invalidate cache by
            /// </summary>
            public string[] InvalidateBy { get; set; }

            /// <summary>
            /// Get method by name
            /// </summary>
            /// <param name="name">Method name to search for</param>
            /// <returns></returns>
            public Method GetMethod(string name)
            {
                if (Methods != null)
                {
                    return Methods.FirstOrDefault(m => string.Compare(m.Name, name, true).Equals(0));
                }
                return null;
            }
        }

        /// <summary>
        /// Cache controller settings
        /// </summary>
        public IEnumerable<Controller> Controllers { get; set; }

        /// <summary>
        /// Default cache provider name
        /// </summary>
        public string DefaultCacheProvider 
        {
            get
            {
                if (string.IsNullOrEmpty(_defaultCacheProvider))
                {
                    _defaultCacheProvider = new MemoryCacheProvider().ProviderName;
                }
                return _defaultCacheProvider;
            }
            set => _defaultCacheProvider = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get controller by name
        /// </summary>
        /// <param name="name">Controller name to search for</param>
        /// <returns></returns>
        public Controller GetController(string name)
        {
            if (Controllers != null)
            {
                return Controllers.FirstOrDefault(c => string.Compare(c.Name, name, true).Equals(0));
            }
            return null;
        }
        #endregion
    }
}