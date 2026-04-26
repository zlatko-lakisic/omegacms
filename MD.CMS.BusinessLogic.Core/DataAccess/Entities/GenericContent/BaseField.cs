using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Xml;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Field;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent
{
    public abstract class BaseField : BaseEntity<long>
    {
        #region Attributes
        private long _attributeTypeDefinitionId;
        private string _name;
        private string _defaultValue;
        private XmlDocument _validationXml;
        private AttributeTypeDefinition _attributeTypeDefinition;
        private string _delimiter;
        private string _listValue;
        private string _options;
        private Options _jsonField;
        private ValidationType _validation;
        private string _uniqueId;
        #endregion

        #region Properties

        public long AttributeTypeDefinitionId
        {
            get { return _attributeTypeDefinitionId; }
            set { _attributeTypeDefinitionId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string FriendlyName
        {
            get
            {
                if (_name != null)
                {
                    return _name.Replace(" ", "_")
                      .Replace("$", "")
                      .Replace(".", "")
                      .Replace("@", "")
                      .Replace("{", "")
                      .Replace("}", "")
                      .Replace("[", "")
                      .Replace("]", "")
                      .Replace("\\", "")
                      .Replace("/", "")
                      .Replace("*", "")
                      .Replace("(", "")
                      .Replace(")", "")
                      .Replace("%", "")
                      .Replace("#", "")
                      .Replace("!", "")
                      .Replace("&", "")
                      .Replace("?", "")
                      .Replace("+", "")
                      .Replace("-", "")
                      .Replace("=", "");
                }
                return _name;

            }
        }

        public bool IsNew
        {
            get
            {
                return Id <= default(long);
            }
        }

        public AttributeTypeDefinition AttributeTypeDefinition
        {
            get
            {
                return _attributeTypeDefinition;
            }
            set
            {
                _attributeTypeDefinition = value;
                if (value != null && !value.Id.Equals(default(long)))
                {
                    _attributeTypeDefinitionId = value.Id;
                }
            }
        }

        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        public string Options
        {
            get { return _options; }
            set { _options = value; }
        }

        public Options JsonField
        {
            get
            {
                if (_jsonField == null)
                {
                    Options options = new Options();
                    if (!string.IsNullOrEmpty(_options))
                    {
                        try
                        {
                            options = Newtonsoft.Json.JsonConvert.DeserializeObject<Options>(_options);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    if (options.validation == null)
                    {
                        options.validation = new Field.Validation.ValidationType();
                    }
                    _jsonField = options;
                }
                return _jsonField;
            }
            set
            {
                if (value != null)
                {
                    _options = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                }
            }
        }

        public string Delimiter
        {
            get { return _delimiter; }
            set { _delimiter = value; }
        }

        public string ListValue
        {
            get { return _listValue; }
            set { _listValue = value; }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                bool returnValue = false;

                if(JsonField != null && !JsonField.enabled)
                {
                    returnValue = true;
                }

                return returnValue;
            }
        }

        public virtual string UniqueId
        {
            get
            {
                if (string.IsNullOrEmpty(_uniqueId))
                {
                    _uniqueId = string.Format("_{0}", this.Id.ToString());
                }
                return _uniqueId;
            }
        }
        #endregion

        #region Methods
        public void Serialize()
        {
            _options = Newtonsoft.Json.JsonConvert.SerializeObject(JsonField);
        }
        #endregion

        #region Methods
        public BaseField() : base()
        {

        }

        public BaseField(BaseField obj) : base(obj)
        {
            if (obj != null)
            {
                _attributeTypeDefinitionId = obj.AttributeTypeDefinitionId;
                _name = obj.Name;
                _defaultValue = obj.DefaultValue;
                _attributeTypeDefinition = obj.AttributeTypeDefinition;
                _delimiter = obj.Delimiter;
                _listValue = obj.ListValue;
                _options = obj.Options;
                _jsonField = obj.JsonField;
                _validation = obj._validation;
                _uniqueId = obj.UniqueId;
            }
        }
        #endregion
    }
}
