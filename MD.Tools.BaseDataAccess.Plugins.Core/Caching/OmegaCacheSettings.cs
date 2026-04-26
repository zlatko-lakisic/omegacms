using MD.Tools.BaseDataAccess.Plugins.Core.Mapping;
using MD.Tools.Helpers.Core.Caching.Providers;
using System.Collections.Generic;
using System.Linq;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Caching
{
    /// <summary>
    /// Data Cache Settings
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
        public class Entity
        {
            /// <summary>
            /// Cache Settings Method Class
            /// </summary>
            public class Method
            {
                /// <summary>
                /// Mapped Method
                /// </summary>
                public int MappedMethod { get; set; }
                /// <summary>
                /// Cache time span in seconds
                /// </summary>
                public int TimeSpan { get; set; }
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
                public Method()
                {
                    Enabled = true;
                }
            }

            /// <summary>
            /// Controller Name
            /// </summary>
            public Entities MappedEntity { get; set; }
            /// <summary>
            /// Cache time span in seconds
            /// </summary>
            public int TimeSpan { get; set; }
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

            public Entity()
            {
                Enabled = true;
            }
            /// <summary>
            /// Get method by name
            /// </summary>
            /// <param name="mappedMethod">Method int to search for</param>
            /// <returns></returns>
            public Method GetMethod(int mappedMethod)
            {
                if (Methods != null)
                {
                    return Methods.FirstOrDefault(m => m.MappedMethod.Equals(mappedMethod));
                }
                return null;
            }
        }

        /// <summary>
        /// Cache entity settings
        /// </summary>
        public IEnumerable<Entity> Entities { get; set; }

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
        /// Get entity by name
        /// </summary>
        /// <param name="mappedEntity">Entity to search for</param>
        /// <returns></returns>
        public Entity GetEntity(Entities mappedEntity)
        {
            if (Entities != null)
            {
                return Entities.FirstOrDefault(e => mappedEntity.Equals(e.MappedEntity));
            }
            return null;
        }
        #endregion
    }
}