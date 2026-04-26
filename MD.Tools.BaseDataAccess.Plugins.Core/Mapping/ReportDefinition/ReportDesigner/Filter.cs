using MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner.Enumerations;
using MD.Tools.Helpers.Core.Data;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportDefinition.ReportDesigner
{
    public class Filter : BaseUniqueIdPropertyClass
    {
        #region Attributes
        private ComparerTypeEnum _type;
        private string _value;
        private Entity _entity;
        private Property _property;
        private bool _isDynamic;
        #endregion

        #region Properties
        /// <summary>
        /// Comparer Type
        /// </summary>
        public ComparerTypeEnum Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// Filter Value
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        /// <summary>
        /// Entity
        /// </summary>
        public Entity Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }
        /// <summary>
        /// Property
        /// </summary>
        public Property Property
        {
            get { return _property; }
            set { _property = value; }
        }
        /// <summary>
        /// This property is dynamic (ie. from QueryString)
        /// </summary>
        public bool IsDynamic
        {
            get { return _isDynamic; }
            set { _isDynamic = value; }
        }
        #endregion
    }
}
