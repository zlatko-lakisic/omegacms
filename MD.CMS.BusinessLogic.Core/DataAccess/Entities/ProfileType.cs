using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;
using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ProfileType : BaseEntity<long>
    {
        #region Attributes
        private string _name;
        private XmlDocument _permissionXmlText;
        private List<ProfileTypeFieldValue> _profileTypeFieldValues;
        private Permissions.ProfileTypePermissions _permissions;
        private string _icon;
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<ProfileTypeFieldValue> Fields
        {
            get { return _profileTypeFieldValues; }
            set { _profileTypeFieldValues = value; }
        }

        public bool IsNew
        {
            get
            {
                return Id.Equals(default(long));
            }
        }

        public Permissions.ProfileTypePermissions Permissions {
            get 
            { 
                if(_permissions == null)
                {
                    _permissions = new ProfileTypePermissions();
                }
                return _permissions;
            }
            set => _permissions = value; 
        }
        public string Icon { get => _icon; set => _icon = value; }
        #endregion

        #region Methods
        /// <summary>
        /// Get field value by name
        /// </summary>
        /// <typeparam name="T">Data type of the field value</typeparam>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="defaultValue">The default value of the field (optional)</param>
        /// <returns>Value of the field</returns>
        public T GetFieldValue<T>(string fieldName, T defaultValue = default(T))
        {
            T returnValue = defaultValue;
            try
            {

                if (this.Fields.Any(f => string.CompareOrdinal(f.Name, fieldName).Equals(0)))
                {
                    string stringValue = this.Fields.Single(f => string.CompareOrdinal(f.Name, fieldName).Equals(0)).Value;
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
                    this.Fields.Single(f => string.CompareOrdinal(f.Name, fieldName).Equals(0)).Value = fieldValue.ToString();
                }
            }
            catch
            {
                //Silent fail
            }
        }
        #endregion
    }
}
