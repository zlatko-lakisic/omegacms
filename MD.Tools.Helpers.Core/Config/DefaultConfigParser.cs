using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using MD.Tools.Helpers.Core.TypeConversion;
using System.Collections.Specialized;
using System.Linq;
using System.Globalization;
using Newtonsoft.Json.Linq;
using MD.Tools.Helpers.Core.Serializer;

namespace MD.Tools.Helpers.Core.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultConfigParser : IConfigParserProvier
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order => 0;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingsObject"></param>
        /// <param name="section"></param>
        public void ParseConfig<T>(T settingsObject, IConfigurationSection section)
            where T: IConfigParsable
        {
            if(section == null)
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
                    if(string.CompareOrdinal(settingsFileTypeField.Name, "Default").Equals(0))
                    {
                        continue;
                    }

                    try
                    {
                        switch (settingsFileTypeField.PropertyType.ToString())
                        {
                            case "System.Int16":
#pragma warning disable CA1305 // Specify IFormatProvider
                                settingsFileTypeField.SetValue(settingsObject, settingsSection[settingsFileTypeField.Name].ToInt16(default(int)));
#pragma warning restore CA1305 // Specify IFormatProvider
                                break;
                            case "System.Int32":
                                settingsFileTypeField.SetValue(settingsObject, settingsSection[settingsFileTypeField.Name].ToInt32(default(int), CultureInfo.InvariantCulture));
                                break;
                            case "System.Int64":
                                settingsFileTypeField.SetValue(settingsObject, settingsSection[settingsFileTypeField.Name].ToInt64(default(long), CultureInfo.InvariantCulture));
                                break;
                            case "System.Boolean":
                                settingsFileTypeField.SetValue(settingsObject, settingsSection[settingsFileTypeField.Name].ToBoolean(default(bool)));
                                break;
                            case "System.Decimal":
                                settingsFileTypeField.SetValue(settingsObject, settingsSection[settingsFileTypeField.Name].ToDecimal(default(decimal), CultureInfo.InvariantCulture));
                                break;
                            case "System.TimeSpan":
                                settingsFileTypeField.SetValue(settingsObject, settingsSection[settingsFileTypeField.Name].ToTimeSpan(default(TimeSpan), CultureInfo.InvariantCulture));
                                break;
                            case "System.DateTime":
                                settingsFileTypeField.SetValue(settingsObject, settingsSection[settingsFileTypeField.Name].ToDateTime(DateTime.MinValue, DateTimeStyles.None, CultureInfo.InvariantCulture));
                                break;
                            case "System.String":
                                if (settingsSection[settingsFileTypeField.Name] != null)
                                {
                                    settingsFileTypeField.SetValue(settingsObject, settingsSection[settingsFileTypeField.Name]);
                                }
                                break;
                            case "System.Collections.Specialized.StringCollection":
                                StringCollection stringCollection = new StringCollection();
                                if (settingsSection.GetSection(settingsFileTypeField.Name).GetChildren() != null && settingsSection.GetSection(settingsFileTypeField.Name).GetChildren().Any())
                                {
                                    IEnumerable<string> stringCollectionList = settingsSection.GetSection(settingsFileTypeField.Name).GetChildren().Select(item => item.Value);
                                    stringCollection.AddRange(stringCollectionList.ToArray());
                                    settingsFileTypeField.SetValue(settingsObject, stringCollection);
                                }
                                break;
                            case "System.Collections.Generic.Dictionary`2[System.String,System.Object]":
                                Dictionary<string, dynamic> dynamicDictionary = new Dictionary<string, dynamic>();
                                if (settingsSection.GetSection(settingsFileTypeField.Name).GetChildren() != null && settingsSection.GetSection(settingsFileTypeField.Name).GetChildren().Any())
                                {
                                    string dynamicDictionaryString = Serialize(settingsSection.GetSection(settingsFileTypeField.Name)).ToString();
                                    dynamicDictionary = OmegaJsonSerializer.DeserializeObject(dynamicDictionaryString, new Dictionary<string, dynamic>());
                                    settingsFileTypeField.SetValue(settingsObject, dynamicDictionary);
                                }
                                break;
                            case "System.Collections.Generic.Dictionary`2[System.String,System.String]":
                                Dictionary<string, string> stringDictionary = new Dictionary<string, string>();
                                if (settingsSection.GetSection(settingsFileTypeField.Name).GetChildren() != null && settingsSection.GetSection(settingsFileTypeField.Name).GetChildren().Any())
                                {
                                    stringDictionary = settingsSection.GetSection(settingsFileTypeField.Name).GetChildren().ToDictionary(item => item.Key, item => item.Value);
                                    settingsFileTypeField.SetValue(settingsObject, stringDictionary);
                                }
                                break;
                            default:
                                IConfigurationSection complexObjectSection = settingsSection.GetSection(settingsFileTypeField.Name);
                                JToken complexObject = Serialize(complexObjectSection);
                                settingsObject.ParseComplexType(settingsFileTypeField.Name, complexObject.ToString());
                                break;
                        }
                    }
                    catch (NotImplementedException e)
                    {
                        Logging.Logger.LogWarning($"Error occured while parsing property: ({settingsFileTypeField.Name}). The error is: {0}", e.Message);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        Logging.Logger.LogWarning($"Error occured while parsing property: ({settingsFileTypeField.Name}). The error is: {0}", e.Message);
                    }
                }
            }
        }



        private JToken Serialize(IConfiguration config)
        {
            JObject obj = new JObject();

            foreach (var child in config.GetChildren())
            {
                if (child.Path.EndsWith(":0", true, CultureInfo.InvariantCulture))
                {
                    var arr = new JArray();

                    foreach (var arrayChild in config.GetChildren())
                    {
                        arr.Add(Serialize(arrayChild));
                    }

                    return arr;
                }
                else
                {
                    obj.Add(child.Key, Serialize(child));
                }
            }

            if (!obj.HasValues && config is IConfigurationSection section && section != null && !string.IsNullOrEmpty(section.Value))
            {
                if (bool.TryParse(section.Value, out bool boolean))
                {
                    return new JValue(boolean);
                }
#pragma warning disable CA1307 // Specify StringComparison
                else if (section.Value.Contains(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator) && decimal.TryParse(section.Value, out decimal real))
#pragma warning restore CA1307 // Specify StringComparison
                {
                    return new JValue(real);
                }
                else if (long.TryParse(section.Value, out long integer))
                {
                    return new JValue(integer);
                }

                return new JValue(section.Value);
            }

            return obj;
        }
    }
}
