using MD.Tools.Helpers.Core.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace MD.Tools.Helpers.Core.Properties
{
    /// <summary>
    /// 
    /// </summary>
    public class HelperSettings : IConfigParsable
    {
        private static HelperSettings defaultInstance = new HelperSettings();
        /// <summary>
        /// 
        /// </summary>
        public static HelperSettings Default
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
        private bool _refreshTypesOnAssemblyLoad;
        private bool _loggerIsFailStop;
        private string _cultureInfoLCIDMappings;
        private bool _allowMethodLogging;
        private bool _allowStopwatchTiming;
        private TimeSpan _cacheTimeout;
        private bool _checkForMisloadedPlugins;
        private string _debugLoggerTraceSwitch;
        private bool _enableCachingMessages;
        private bool _enableVariableDelayForErrors;
        private bool _eventLoggerIsEnabled;
        private string _eventLoggerLogName;
        private string _eventLoggerSourceName;
        private string _eventLoggerTraceSwitch;
        private TraceLevel _includeWebContextInfoForMessageLevel;
        private StringCollection _intranetMasks;
        private TimeSpan _maxVariableDelayForErrors;
        private string _nodesStatusMessageFormat;
        private TimeSpan _peridicTaskCheckDuration;
        private StringCollection _periodicTasks;
        private TimeSpan _periodTaskInitialWaitDuration;
        private bool _redirectAllEmail;
        private bool _redirectBrandAliasAsPermanent;
        private string _redirectDisplayNameTemplate;
        private string _redirectEmailAddress;
        private string _releaseInProgressMarkerFile;
        private StringCollection _skipDBConnectionCheck;
        private string _smtpApplication;
        private bool _smtpLoggerIsEnabled;
        private string _smtpLoggerTraceSwitch;
        private StringCollection _smtpToAddress;
        private string _testEmailRegex;
        private string _traceAssertLoggerTraceSwitch;
        private bool _traceLoggerEnabled;
        private string _traceLoggerTraceSwitch;
        private string _hardwareSearchScope;
        private int _rSACryptSize;
        private string _defaultConnectionStringFormattedString;
        private bool _netCoreLoggerIsEnabled;
        private string _netCoreLoggerTraceSwitch;
        private bool _awsCloudWatchLoggerIsEnabled;
        private string _awsCloudWatchLoggerTraceSwitch;
        private bool _pluginLoadedLoggingEnabled;
        private Dictionary<string, string> _traceSwitches;
        private int _defaultFileProvider;
        private Dictionary<string, dynamic> _providerOptions;
        private string _tempAssembliesFolder;
        private Dictionary<string, string> _loggerOptions;
        private bool _verboseLoggingReflectionEnabled;
        private StringCollection _reflectionHelperExclusions;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public bool RefreshTypesOnAssemblyLoad { get => _refreshTypesOnAssemblyLoad; set => _refreshTypesOnAssemblyLoad = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool LoggerIsFailStop { get => _loggerIsFailStop; set => _loggerIsFailStop = value; }
        /// <summary>
        /// 
        /// </summary>
        public string CultureInfoLCIDMappings { get => _cultureInfoLCIDMappings; set => _cultureInfoLCIDMappings = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool AllowMethodLogging { get => _allowMethodLogging; set => _allowMethodLogging = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool AllowStopwatchTiming { get => _allowStopwatchTiming; set => _allowStopwatchTiming = value; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan CacheTimeout { get => _cacheTimeout; set => _cacheTimeout = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool CheckForMisloadedPlugins { get => _checkForMisloadedPlugins; set => _checkForMisloadedPlugins = value; }
        /// <summary>
        /// 
        /// </summary>
        public string DebugLoggerTraceSwitch { get => _debugLoggerTraceSwitch; set => _debugLoggerTraceSwitch = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool EnableCachingMessages { get => _enableCachingMessages; set => _enableCachingMessages = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool EnableVariableDelayForErrors { get => _enableVariableDelayForErrors; set => _enableVariableDelayForErrors = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool EventLoggerIsEnabled { get => _eventLoggerIsEnabled; set => _eventLoggerIsEnabled = value; }
        /// <summary>
        /// 
        /// </summary>
        public string EventLoggerLogName { get => _eventLoggerLogName; set => _eventLoggerLogName = value; }
        /// <summary>
        /// 
        /// </summary>
        public string EventLoggerSourceName { get => _eventLoggerSourceName; set => _eventLoggerSourceName = value; }
        /// <summary>
        /// 
        /// </summary>
        public string EventLoggerTraceSwitch { get => _eventLoggerTraceSwitch; set => _eventLoggerTraceSwitch = value; }
        /// <summary>
        /// 
        /// </summary>
        public TraceLevel IncludeWebContextInfoForMessageLevel { get => _includeWebContextInfoForMessageLevel; set => _includeWebContextInfoForMessageLevel = value; }
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public StringCollection IntranetMasks { get => _intranetMasks; set => _intranetMasks = value; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan MaxVariableDelayForErrors { get => _maxVariableDelayForErrors; set => _maxVariableDelayForErrors = value; }
        /// <summary>
        /// 
        /// </summary>
        public string NodesStatusMessageFormat { get => _nodesStatusMessageFormat; set => _nodesStatusMessageFormat = value; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan PeridicTaskCheckDuration { get => _peridicTaskCheckDuration; set => _peridicTaskCheckDuration = value; }
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public StringCollection PeriodicTasks { get => _periodicTasks; set => _periodicTasks = value; }
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan PeriodTaskInitialWaitDuration { get => _periodTaskInitialWaitDuration; set => _periodTaskInitialWaitDuration = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool RedirectAllEmail { get => _redirectAllEmail; set => _redirectAllEmail = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool RedirectBrandAliasAsPermanent { get => _redirectBrandAliasAsPermanent; set => _redirectBrandAliasAsPermanent = value; }
        /// <summary>
        /// 
        /// </summary>
        public string RedirectDisplayNameTemplate { get => _redirectDisplayNameTemplate; set => _redirectDisplayNameTemplate = value; }
        /// <summary>
        /// 
        /// </summary>
        public string RedirectEmailAddress { get => _redirectEmailAddress; set => _redirectEmailAddress = value; }
        /// <summary>
        /// 
        /// </summary>
        public string ReleaseInProgressMarkerFile { get => _releaseInProgressMarkerFile; set => _releaseInProgressMarkerFile = value; }
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public StringCollection SkipDBConnectionCheck { get => _skipDBConnectionCheck; set => _skipDBConnectionCheck = value; }
        /// <summary>
        /// 
        /// </summary>
        public string SmtpApplication { get => _smtpApplication; set => _smtpApplication = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool SmtpLoggerIsEnabled { get => _smtpLoggerIsEnabled; set => _smtpLoggerIsEnabled = value; }
        /// <summary>
        /// 
        /// </summary>
        public string SmtpLoggerTraceSwitch { get => _smtpLoggerTraceSwitch; set => _smtpLoggerTraceSwitch = value; }
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public StringCollection SmtpToAddress { get => _smtpToAddress; set => _smtpToAddress = value; }
        /// <summary>
        /// 
        /// </summary>
        public string TestEmailRegex { get => _testEmailRegex; set => _testEmailRegex = value; }
        /// <summary>
        /// 
        /// </summary>
        public string TraceAssertLoggerTraceSwitch { get => _traceAssertLoggerTraceSwitch; set => _traceAssertLoggerTraceSwitch = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool TraceLoggerEnabled { get => _traceLoggerEnabled; set => _traceLoggerEnabled = value; }
        /// <summary>
        /// 
        /// </summary>
        public string TraceLoggerTraceSwitch { get => _traceLoggerTraceSwitch; set => _traceLoggerTraceSwitch = value; }
        /// <summary>
        /// 
        /// </summary>
        public string HardwareSearchScope { get => _hardwareSearchScope; set => _hardwareSearchScope = value; }
        /// <summary>
        /// 
        /// </summary>
        public int RSACryptSize { get => _rSACryptSize; set => _rSACryptSize = value; }
        /// <summary>
        /// 
        /// </summary>
        public string DefaultConnectionStringFormattedString { get => _defaultConnectionStringFormattedString; set => _defaultConnectionStringFormattedString = value; }
        /// <summary>
        /// 
        /// </summary>
        public string SectionName => "MD.Tools.Helpers.Core";
        /// <summary>
        /// 
        /// </summary>
        public bool NetCoreLoggerIsEnabled { get => _netCoreLoggerIsEnabled; set => _netCoreLoggerIsEnabled = value; }
        /// <summary>
        /// 
        /// </summary>
        public string NetCoreLoggerTraceSwitch { get => _netCoreLoggerTraceSwitch; set => _netCoreLoggerTraceSwitch = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool PluginLoadedLoggingEnabled { get => _pluginLoadedLoggingEnabled; set => _pluginLoadedLoggingEnabled = value; }
        /// <summary>
        /// 
        /// </summary>

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public Dictionary<string, string> TraceSwitches { get => _traceSwitches; set => _traceSwitches = value; }
        /// <summary>
        /// 
        /// </summary>
        public int DefaultFileProvider { get => _defaultFileProvider; set => _defaultFileProvider = value; }
        /// <summary>
        /// 
        /// </summary>

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public Dictionary<string, dynamic> ProviderOptions { get => _providerOptions; set => _providerOptions = value; }
        /// <summary>
        /// 
        /// </summary>
        public string TempAssembliesFolder { get => _tempAssembliesFolder; set => _tempAssembliesFolder = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool AwsCloudWatchLoggerIsEnabled { get => _awsCloudWatchLoggerIsEnabled; set => _awsCloudWatchLoggerIsEnabled = value; }
        /// <summary>
        /// 
        /// </summary>
        public string AwsCloudWatchLoggerTraceSwitch { get => _awsCloudWatchLoggerTraceSwitch; set => _awsCloudWatchLoggerTraceSwitch = value; }
        /// <summary>
        /// 
        /// </summary>

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public Dictionary<string, string> LoggerOptions { get => _loggerOptions; set => _loggerOptions = value; }
        /// <summary>
        /// 
        /// </summary>
        public bool VerboseLoggingReflectionEnabled { get => _verboseLoggingReflectionEnabled; set => _verboseLoggingReflectionEnabled = value; }
        /// <summary>
        /// 
        /// </summary>
        public StringCollection ReflectionHelperExclusions { get => _reflectionHelperExclusions; set => _reflectionHelperExclusions = value; }

        /// <summary>
        /// 
        /// </summary>
        #endregion

        #region Methods
        public HelperSettings()
        {
            _refreshTypesOnAssemblyLoad = true;
            _loggerIsFailStop = true;
            _cultureInfoLCIDMappings = "en-GB=1033";
            _allowMethodLogging = true;
            _allowStopwatchTiming = true;
            _cacheTimeout = TimeSpan.Parse("00:15:00", CultureInfo.InvariantCulture);
            _checkForMisloadedPlugins = false;
            _debugLoggerTraceSwitch = "DebugLogger";
            _enableCachingMessages = false;
            _enableVariableDelayForErrors = true;
            _eventLoggerIsEnabled = false;
            _eventLoggerLogName = "OmegaCMS";
            _eventLoggerSourceName = "Omega Helpers";
            _eventLoggerTraceSwitch = "EventLogger";
            _includeWebContextInfoForMessageLevel = TraceLevel.Error;
            _intranetMasks = new StringCollection() { "10.255.255.255", "192.168.255.255" };
            _maxVariableDelayForErrors = TimeSpan.Parse("00:00:00.6000000", CultureInfo.InvariantCulture);
            _nodesStatusMessageFormat = "{0}";
            _peridicTaskCheckDuration = TimeSpan.Parse("00:00:05", CultureInfo.InvariantCulture);
            _periodTaskInitialWaitDuration = TimeSpan.Parse("00:00:15", CultureInfo.InvariantCulture);
            _redirectAllEmail = false;
            _redirectBrandAliasAsPermanent = false;
            _redirectDisplayNameTemplate = "Message Originally Intended For: {0}";
            _redirectEmailAddress = string.Empty;
            _releaseInProgressMarkerFile = "~/release.config";
            _skipDBConnectionCheck = new StringCollection() { "LocalSqlServer" };
            _smtpApplication = "Omega Helpers";
            _smtpLoggerIsEnabled = false;
            _smtpLoggerTraceSwitch = "SmtpLogger";
            _smtpToAddress = new StringCollection();
            _testEmailRegex = @"^[^@]+(\.[^@\.]+@test\.).+$";
            _traceAssertLoggerTraceSwitch = "TraceAssertLogger";
            _traceLoggerEnabled = false;
            _traceLoggerTraceSwitch = "TraceLogger";
            _hardwareSearchScope = "root\\CIMV2";
            _rSACryptSize = 2048;
            _defaultConnectionStringFormattedString = "Server={0};Database={1};user id={2};password={3};";
            _netCoreLoggerIsEnabled = false;
            _netCoreLoggerTraceSwitch = "NetCoreLogger";
            _pluginLoadedLoggingEnabled = false;
            _traceSwitches = new Dictionary<string, string>();
            _providerOptions = new Dictionary<string, dynamic>();
            _defaultFileProvider = (int)Core.FileProvider.FileProviderEnum.Hosted;
            _awsCloudWatchLoggerIsEnabled = false;
            _awsCloudWatchLoggerTraceSwitch = "AwsCloudWatchLogger";
            _loggerOptions = new Dictionary<string, string>();
            _verboseLoggingReflectionEnabled = false;
            _reflectionHelperExclusions = new StringCollection { "Microsoft.AspNetCore.Razor.Language.RazorTemplateEngine", "Microsoft.CodeAnalysis.Workspaces", "Microsoft.CodeAnalysis.CodeGeneration.CodeGenerationArrayTypeSymbol" };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section"></param>
        public void Parse(IConfigurationSection section)
        {
            ConfigParser.ParseConfig(this, section);
            Logging.Logger.InitLoggers();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IConfigParsable GetStaticInstance()
        {
            return HelperSettings.Default;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="stringValue"></param>
        public void ParseComplexType(string sectionName, string stringValue)
        {
            //Do Nothing
        }
        #endregion
    }
}
