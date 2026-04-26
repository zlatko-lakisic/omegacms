using System;
using System.Collections.Generic;
using System.Linq;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class ProfileTypeList : List<ProfileType>
    {

        #region Methods
        public ProfileTypeList() : base()
        {
            //Do Nothing
        }
        public ProfileTypeList(IEnumerable<ProfileType> collection) : base(collection)
        {
            //Do Nothing
        }
        public ProfileTypeList(int capacity) : base(capacity)
        {
            //Do Nothing
        }

        /// <summary>
        /// Get field value by name
        /// </summary>
        /// <typeparam name="T">Data type of the field value</typeparam>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="defaultValue">The default value of the field (optional)</param>
        /// <returns>Value of the field</returns>
        public T GetFieldValue<T>(string profileTypeName, string fieldName, T defaultValue = default(T))
        {
            T returnValue = defaultValue;
            try
            {
                ProfileType profileType = this.FirstOrDefault(p => string.Compare(p.Name, profileTypeName, true).Equals(0));
                if (profileType != null && profileType.Fields.Any(f => string.CompareOrdinal(f.Name, fieldName).Equals(0)))
                {
                    string stringValue = profileType.Fields.Single(f => string.CompareOrdinal(f.Name, fieldName).Equals(0)).Value;
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
        public void SetFieldValue<T>(string profileTypeName, string fieldName, T fieldValue)
        {
            try
            {
                ProfileType profileType = this.FirstOrDefault(p => string.Compare(p.Name, profileTypeName, true).Equals(0));
                if (profileType != null && profileType.Fields.Any(f => string.CompareOrdinal(f.Name, fieldName).Equals(0)))
                {
                    profileType.Fields.Single(f => string.CompareOrdinal(f.Name, fieldName).Equals(0)).Value = fieldValue.ToString();
                }
            }
            catch
            {
                //Silent fail
            }
        }

        public bool HasProfileType(string profileTypeName)
        {
            return this.Any(p => string.Compare(p.Name, profileTypeName, true).Equals(0));
        }
        #endregion
    }
}
