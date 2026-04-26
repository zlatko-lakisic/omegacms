using System;
using System.Collections.Generic;
using System.Text;

namespace MD.Tools.BaseDataAccess.Core.Properties
{
    public class Settings
    {
        private static Settings defaultInstance = new Settings();
        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
            set
            {
                defaultInstance = value;
            }
        }

        #region Attributes
        private string _defaultConnectionString;
        private TimeSpan _cacheTimeout;
        #endregion

        #region Properties
        public TimeSpan CacheTimeout { get => _cacheTimeout; set => _cacheTimeout = value; }
        public string DefaultConnectionString { get => _defaultConnectionString; set => _defaultConnectionString = value; }
        #endregion

        #region Methods
        public Settings()
        {
            _defaultConnectionString = string.Empty;
            _cacheTimeout = TimeSpan.Parse("00:00:00");
        }
        #endregion
    }
}
