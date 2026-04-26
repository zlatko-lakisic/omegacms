using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent;
using MD.CMS.BusinessLogic.Core.Helpers.Calculations;
using MD.Tools.BaseDataAccess.Core.Entities;
namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ContentTypeDefinition<FT> : BaseEntity<long>
        where FT : GenericContent.GenericContentField
    {
        #region Attributes
        private string _name;
        private string _description;
        private List<FT> _fields;
        private string _options;
        private bool _isEditable;
        private string _icon;
		private List<ContentTypeDataSource> _dataSources;
		private List<ContentTypeDataSourceJoin> _joins;
		#endregion

		#region Properties
		public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public bool IsNew
        {
            get
            {
                return Id.Equals(default(long));
            }
        }

        public List<FT> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        public string Options
        {
            get { return _options; }
            set { _options = value; }
        }

        public dynamic JsonOptions
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(_options))
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject(_options);
                    }
                }
                catch (Exception e)
                {
                    MD.Tools.Helpers.Core.Logging.Logger.Log(e);
                }
                return null;
            }
            
            set
            {
                try
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        _options = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                    }
                }
                catch (Exception e)
                {
                    MD.Tools.Helpers.Core.Logging.Logger.Log(e);
                }
            }
        }

        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public bool IsEditable
        {
            get { return _isEditable; }
            set { _isEditable = value; }
        }

		public List<ContentTypeDataSource> DataSources 
		{ 
			get
			{
				if(_dataSources == null){
					_dataSources = new List<ContentTypeDataSource>();
				}
				return _dataSources;
			}
			set{
				_dataSources = value;
			}
		}
		public List<ContentTypeDataSourceJoin> Joins
		{
			get
			{
				if (_joins == null)
				{
					_joins = new List<ContentTypeDataSourceJoin>();
				}
				return _joins;
			}
			set
			{
				_joins = value;
			}
		}
        #endregion

        #region Methods
        public FT GetField(string fieldName)
        {
            return this.Fields.FirstOrDefault(f => string.CompareOrdinal(f.Name, fieldName).Equals(0));
        }
        /// <summary>
        /// Get field value by name
        /// </summary>
        /// <typeparam name="T">Data type of the field value</typeparam>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="defaultValue">The default value of the field (optional)</param>
        /// <returns>Value of the field</returns>
        [Obsolete]
        public T GetFieldValue<T>(string fieldName, T defaultValue = default(T))
        {
            return GetFieldValueAsync<T>(fieldName, default).Result;
        }
        /// <summary>
        /// Get field value by name
        /// </summary>
        /// <typeparam name="T">Data type of the field value</typeparam>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="defaultValue">The default value of the field (optional)</param>
        /// <param name="userMakingTheCall">User making the call</param>
        /// <returns>Value of the field</returns>
        public async Task<T> GetFieldValueAsync<T>(string fieldName, T defaultValue = default(T), User userMakingTheCall = null)
        {
            T returnValue = defaultValue;
            try
            {
                FT _field = GetField(fieldName);
                if (_field != null)
                {
                    ContentTypeDefinitionFieldValue field = _field as ContentTypeDefinitionFieldValue;
                    if (field.AttributeTypeDefinitionId == 17)
                    {
                        if (userMakingTheCall is null)
                        {
                            throw new ArgumentNullException(nameof(userMakingTheCall));
                        }
                        if (field.ContentTypeDefinitionId.Equals(default))
                        {
                            throw new ArgumentOutOfRangeException(nameof(field.ContentTypeDefinitionId));
                        }
                        await PostfixEvaluator.EvaluateAsync(userMakingTheCall, this.Fields.Select(f => f as ContentTypeDefinitionFieldValue), field, field.DefaultValue);
                    }

                    string stringValue = field.Value;
                    switch (typeof(T).ToString())
                    {
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Boolean":
                        case "System.Decimal":
                            returnValue = MD.Tools.Helpers.Core.Helpers.Parser<T>.Parse(stringValue);
                            break;
                        default:
                            returnValue = (T)Convert.ChangeType(stringValue, typeof(T));
                            break;
                    }
                }
            }
            catch
            {
                //Silent fail
            }
            return returnValue;
        }
        /// <summary>
        /// Set the field value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        public void SetFieldValue<T>(string fieldName, T fieldValue)
        {
            try
            {
                if (this.Fields.Any(f => string.CompareOrdinal(f.Name, fieldName).Equals(0)))
                {
                    ContentTypeDefinitionFieldValue field = this.Fields.First(f => string.CompareOrdinal(f.Name, fieldName).Equals(0)) as ContentTypeDefinitionFieldValue;
                    field.Value = fieldValue.ToString();
                }
            }
            catch
            {
                //Silent fail
            }
        }
        /// <summary>
		/// Get a datasource based on a field
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public ContentTypeDataSource GetDataSourceForField(ContentTypeDefinitionField field)
		{
			GenericContentField foundField = _fields.FirstOrDefault(f => f.Id == field.Id || f.Name == field.Name);
			if(foundField != null)
			{
				return _dataSources.FirstOrDefault(ds => ds.Id == foundField.DataSourceId);
			}
			return null;
		}
		#endregion
    }
}
