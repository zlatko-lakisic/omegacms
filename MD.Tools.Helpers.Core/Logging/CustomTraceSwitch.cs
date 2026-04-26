using MD.Tools.Helpers.Core.Properties;
using System.Diagnostics;
using MD.Tools.Helpers.Core.TypeConversion;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace MD.Tools.Helpers.Core.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomTraceSwitch
    {
        #region Attributes
        private TraceLevel _level;
        private string _description;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public TraceLevel Level { get => _level; set => _level = value; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get => _description; set => _description = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool TraceError { get => _level.Equals(TraceLevel.Error); }
        /// <summary>
        /// 
        /// </summary>
        public bool TraceInfo { get => _level.Equals(TraceLevel.Info); }
        /// <summary>
        /// 
        /// </summary>
        public bool TraceVerbose { get => _level.Equals(TraceLevel.Verbose); }
        /// <summary>
        /// 
        /// </summary>
        public bool TraceWarning { get => _level.Equals(TraceLevel.Warning); }
        /// <summary>
        /// 
        /// </summary>
        public LogLevel LogLevel
        {
            get
            {
                switch (_level)
                {
                    case TraceLevel.Error:
                        return LogLevel.Error;
                    case TraceLevel.Warning:
                        return LogLevel.Warning;
                    case TraceLevel.Info:
                        return LogLevel.Information;
                    case TraceLevel.Verbose:
                        return LogLevel.Debug;
                    default:
                        return LogLevel.None;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        public CustomTraceSwitch(string displayName, string description)
        {
            _description = description;
            _description = description;
            try
            {
                _level = (TraceLevel)HelperSettings.Default.TraceSwitches[displayName].ToInt32(default(int), CultureInfo.InvariantCulture);
            }
            catch
            {
                _level = (TraceLevel)default(int);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="defaultSwitchValue"></param>
        public CustomTraceSwitch(string displayName, string description, string defaultSwitchValue)
        {
            _description = description;
            try
            {
                _level = (TraceLevel)HelperSettings.Default.TraceSwitches[displayName].ToInt32(default(int), CultureInfo.InvariantCulture);
            }
            catch
            {
                _level = (TraceLevel)defaultSwitchValue.ToInt32(default(int), CultureInfo.InvariantCulture);
            }
        }
        #endregion
    }
}
