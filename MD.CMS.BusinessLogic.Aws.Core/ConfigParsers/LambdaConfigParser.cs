using MD.Tools.Helpers.Core.Config;
using MD.Tools.Helpers.Core.Logging;
using MD.Tools.Helpers.Core.TypeConversion;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MD.CMS.BusinessLogic.Aws.Core.ConfigParsers
{
    /// <summary>
    /// Lambda config parser
    /// </summary>
    public class LambdaConfigParser : IConfigParserProvier
    {
        /// <summary>
        /// Config parser order
        /// </summary>
        public int Order => 1;

        /// <summary>
        /// Parse config section from lambda settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingsObject"></param>
        /// <param name="section"></param>
        public void ParseConfig<T>(T settingsObject, IConfigurationSection section) where T : IConfigParsable
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            IConfigurationSection settingsSection = section.GetSection(settingsObject.SectionName);
            if (section != null && settingsSection != null)
            {
                Type settingsFileType = typeof(T);
                IEnumerable<PropertyInfo> settingsFileTypeFields = settingsFileType.GetProperties().Where(p => p.CanWrite);
                foreach (PropertyInfo settingsFileTypeField in settingsFileTypeFields)
                {
                    try
                    {
                        if (string.CompareOrdinal(settingsFileTypeField.Name, "Default").Equals(0))
                        {
                            continue;
                        }

                        string fullPropertyPath = $"{settingsObject.SectionName}.{settingsFileTypeField.Name}";
                        string lambdaPropertyName = fullPropertyPath.Replace(".", "_");
                        string value = Environment.GetEnvironmentVariable(lambdaPropertyName);

                        if (string.IsNullOrEmpty(value))
                        {
                            continue;
                        }

                        switch (settingsFileTypeField.PropertyType.ToString())
                        {
                            case "System.Int16":
#pragma warning disable CA1305 // Specify IFormatProvider
                                settingsFileTypeField.SetValue(settingsObject, value.ToInt16(default(int)));
#pragma warning restore CA1305 // Specify IFormatProvider
                                break;
                            case "System.Int32":
                                settingsFileTypeField.SetValue(settingsObject, value.ToInt32(default(int), CultureInfo.InvariantCulture));
                                break;
                            case "System.Int64":
                                settingsFileTypeField.SetValue(settingsObject, value.ToInt64(default(long), CultureInfo.InvariantCulture));
                                break;
                            case "System.Boolean":
                                settingsFileTypeField.SetValue(settingsObject, value.ToBoolean(default(bool)));
                                break;
                            case "System.Decimal":
                                settingsFileTypeField.SetValue(settingsObject, value.ToDecimal(default(decimal), CultureInfo.InvariantCulture));
                                break;
                            case "System.TimeSpan":
                                settingsFileTypeField.SetValue(settingsObject, value.ToTimeSpan(default(TimeSpan), CultureInfo.InvariantCulture));
                                break;
                            case "System.String":
                                settingsFileTypeField.SetValue(settingsObject, value);
                                break;
                            case "System.Collections.Specialized.StringCollection":
                                try
                                {
                                    StringCollection stringCollection = JsonConvert.DeserializeObject<StringCollection>(value);
                                    settingsFileTypeField.SetValue(settingsObject, stringCollection);
                                }
                                catch(JsonSerializationException e)
                                {
                                    typeof(LambdaConfigParser).LogInformation($"Error occured while parsing property: ({settingsFileTypeField.Name}, {settingsObject.SectionName.Replace(".", "_")}_{settingsFileTypeField.Name})");
                                    typeof(LambdaConfigParser).LogInformation(value);
                                    typeof(LambdaConfigParser).Log(e);
                                }
                                break;
                            case "System.Collections.Generic.Dictionary`2[System.String,System.Object]":
                                try
                                {
                                    Dictionary<string, dynamic> dynamicDictionary = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(value);
                                    settingsFileTypeField.SetValue(settingsObject, dynamicDictionary);
                                }
                                catch (JsonSerializationException e)
                                {
                                    typeof(LambdaConfigParser).LogInformation($"Error occured while parsing property: ({settingsFileTypeField.Name}, {settingsObject.SectionName.Replace(".", "_")}_{settingsFileTypeField.Name})");
                                    typeof(LambdaConfigParser).LogInformation(value);
                                    typeof(LambdaConfigParser).Log(e);
                                }
                                break;
                            case "System.Collections.Generic.Dictionary`2[System.String,System.String]":
                                try
                                {
                                    Dictionary<string, string> stringDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
                                    settingsFileTypeField.SetValue(settingsObject, stringDictionary);
                                }
                                catch (JsonSerializationException e)
                                {
                                    typeof(LambdaConfigParser).LogInformation($"Error occured while parsing property: ({settingsFileTypeField.Name}, {settingsObject.SectionName.Replace(".", "_")}_{settingsFileTypeField.Name})");
                                    typeof(LambdaConfigParser).LogInformation(value);
                                    typeof(LambdaConfigParser).Log(e);
                                }
                                break;
                            default:
                                if (value.StartsWith('"') && value.EndsWith('"'))
                                {
                                    value = value.Substring(1, value.Length - 2);
                                }
                                settingsObject.ParseComplexType(settingsFileTypeField.Name, value);
                                break;
                        }
                    } 
                    catch (Exception e)
                    {
                        typeof(LambdaConfigParser).LogInformation($"Error occured while parsing property: ({settingsFileTypeField.Name}, {settingsObject.SectionName.Replace(".", "_")}_{settingsFileTypeField.Name})");
                        typeof(LambdaConfigParser).Log(e);
                    }
                }
            }
        }
    }
}
